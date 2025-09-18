using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace SMS
{
    public partial class AddItemToDiscount : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                if (Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "CIT")
                {
                    loadDiscounts();
                    LoadFGItem();
                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();
                }
                else
                {
                    Response.Redirect("~/UnauthorizedPage.aspx");
                }

            }

            else
            {
                if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
                {
                    IsPageRefresh = true;
                }
                Session["SessionId"] = System.Guid.NewGuid().ToString();
                ViewState["ViewStateId"] = Session["SessionId"].ToString();
            }
        }


        private void loadDiscounts()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT DISTINCT
                               [sConstant]
	                          ,[sConstant] + ' - ' + [sDescription] as [sDescription]
                          FROM [tblTypeDiscount]
                          WHERE [iValidUntil_dt] >= convert(char(10),getdate(),112)
                          ORDER BY [sDescription]";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddDiscount.Items.Clear();
                    ddDiscount.DataSource = dR;
                    ddDiscount.DataValueField = "sConstant";
                    ddDiscount.DataTextField = "sDescription";
                    ddDiscount.DataBind();
                    ddDiscount.Items.Insert(0, new ListItem("Please select discount", "0"));
                    ddDiscount.SelectedIndex = 0;
                }
            }
        }
        private void LoadFGItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT vFGCode, LTRIM(RTRIM(vDESCRIPTION)) AS [vDESCRIPTION] FROM vItemMaster where vStat=1 and vPluCode like '0%' and IsPromo=0";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddITemFG.Items.Clear();
                    ddITemFG.DataSource = dR;
                    ddITemFG.DataValueField = "vFGCode";
                    ddITemFG.DataTextField = "vDESCRIPTION";
                    ddITemFG.DataBind();
                    ddITemFG.Items.Insert(0, new ListItem("Please select item", "0"));
                    ddITemFG.SelectedIndex = 0;
                }
            }
        }

        protected void ddDiscount_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadDiscountDetail();
        }

        private void loadDiscountDetail()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [DiscountDetailID]
                                  ,[BrCode]
                                  ,[sDescription]
                                  ,[sConstant]
                                  ,[vFGCode]
                              FROM [tblTypeDiscountDetail] where sConstant=@sConstant
                              order by [sDescription]";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@sConstant", ddDiscount.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvDisc.DataSource = dT;
                    gvDisc.DataBind();
                }
            }
        }

        protected void gvDisc_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                
                    string dKey = gvDisc.DataKeys[e.RowIndex].Value.ToString();
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"DELETE FROM [tblTypeDiscountDetail] WHERE DiscountDetailID=@DiscountDetailID";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            
                            cmD.Parameters.AddWithValue("@DiscountDetailID", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }


                    loadDiscountDetail();

            }   
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

                
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"INSERT INTO [dbo].[tblTypeDiscountDetail]
                                               ([BrCode]
                                               ,[sDescription]
                                               ,[sConstant]
                                               ,[vFGCode])
                                         VALUES
                                               (@BrCode
                                               ,@sDescription
                                               ,@sConstant
                                               ,@vFGCode)";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;

                        cmD.Parameters.AddWithValue("@BrCode", 0);
                        cmD.Parameters.AddWithValue("@sDescription", ddITemFG.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@sConstant", ddDiscount.SelectedValue);
                        cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                        cmD.ExecuteNonQuery();
                    }
                }


                loadDiscountDetail();

            }   
        }

    }
}