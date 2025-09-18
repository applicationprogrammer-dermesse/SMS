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
    public partial class setupItem : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));


                if (Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "CIT" | Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "DCI-Marketing" | Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "DCI-Logistics")
                {

                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();

                }
                else
                {
                    Response.Redirect("~/UnauthorizedPAge.aspx");

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

        protected void btnGen_Click(object sender, EventArgs e)
        {
            loadItemMasterList();
        }

        private void loadItemMasterList()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.loadItemMasterList";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@Type", ddType.Text);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvItem.DataSource = dT;
                        gvItem.DataBind();
                    }
                }
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvItem.DataSource = null;
            gvItem.DataBind();
        }


        

        protected void gvItem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Label lblStat = (Label)gvItem.Rows[e.NewEditIndex].FindControl("lblvStatus");
            
            gvItem.EditIndex = e.NewEditIndex;
            theStat = lblStat.Text;
            loadItemMasterList();
        }

        protected void gvItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvItem.EditIndex = -1;
            loadItemMasterList();
        }

        protected void gvItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string dKey = gvItem.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtvDESCRIPTION = gvItem.Rows[e.RowIndex].FindControl("txtvDESCRIPTION") as TextBox;
            TextBox txtiNoSession = gvItem.Rows[e.RowIndex].FindControl("txtiNoSession") as TextBox;
            TextBox txtivUnitCost = gvItem.Rows[e.RowIndex].FindControl("txtivUnitCost") as TextBox;
            DropDownList ddCategory = gvItem.Rows[e.RowIndex].FindControl("ddCategory") as DropDownList;
            DropDownList ddItemType = gvItem.Rows[e.RowIndex].FindControl("ddItemType") as DropDownList;
            DropDownList ddStatus = gvItem.Rows[e.RowIndex].FindControl("ddStatus") as DropDownList;
            TextBox txtGroupName = gvItem.Rows[e.RowIndex].FindControl("txtGroupName") as TextBox;
            

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"UPDATE [dbo].[vItemMaster]
                               SET [NoSession] = @NoSession
                                  ,[vUnitCost] = @vUnitCost
                                  ,[ItemType] = @ItemType
                                   ,[vCATEGORY] = @vCATEGORY
                                   ,[vDESCRIPTION] = @vDESCRIPTION
                                   ,vStat= @Status
                                   ,GroupName=@GroupName
                                 ,[vCreatedBy]=@vCreatedBy
                                      ,[vCreatedDate]=@vCreatedDate
                             WHERE vFGCode=@vFGCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@vFGCode", dKey);
                    cmD.Parameters.AddWithValue("@vDESCRIPTION", txtvDESCRIPTION.Text.Trim());
                    cmD.Parameters.AddWithValue("@NoSession", txtiNoSession.Text);
                    cmD.Parameters.AddWithValue("@vUnitCost", txtivUnitCost.Text);
                    cmD.Parameters.AddWithValue("@ItemType", ddItemType.SelectedItem.Text);
                    cmD.Parameters.AddWithValue("@vCATEGORY", ddCategory.SelectedItem.Text.ToUpper());
                    cmD.Parameters.AddWithValue("@GroupName", txtGroupName.Text);
                    cmD.Parameters.AddWithValue("@vCreatedBy",Session["FullName"].ToString());
                    cmD.Parameters.AddWithValue("@vCreatedDate", DateTime.Now);
                    if (ddStatus.SelectedValue == "Active")
                    {
                        cmD.Parameters.AddWithValue("@Status", "1");
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Status", "0");
                    }
                   
                    cmD.ExecuteNonQuery();
                }
            }

            gvItem.EditIndex = -1;
            loadItemMasterList();

        }

        public string theStat;
        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvItem.EditIndex == e.Row.RowIndex)
            {
                loadItemType(e);
                loadCategory(e);

                DropDownList ddStat = (DropDownList)e.Row.FindControl("ddStatus");

                if (theStat.ToString() == "Active")
                {

                    ddStat.Items.Insert(0, "Active");
                    ddStat.Items.Insert(1, "Inactive");
                }
                else
                {
                    ddStat.Items.Insert(0, "Inactive");
                    ddStat.Items.Insert(1, "Active");
                }
            }
        }

        private static void loadItemType(GridViewRowEventArgs e)
        {
            DropDownList ddItemType = (DropDownList)e.Row.FindControl("ddItemType");
            string sql = "SELECT distinct ItemType FROM vItemMaster";
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(sql, con))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        ddItemType.DataSource = dt;
                        ddItemType.DataTextField = "ItemType";
                        ddItemType.DataValueField = "ItemType";
                        ddItemType.DataBind();
                        string selectedddItemType = DataBinder.Eval(e.Row.DataItem, "ItemType").ToString();
                        ddItemType.Items.FindByValue(selectedddItemType).Selected = true;
                    }
                }
            }
        }

        private static void loadCategory(GridViewRowEventArgs e)
        {
            DropDownList ddItemType = (DropDownList)e.Row.FindControl("ddItemType");
            DropDownList ddCategory = (DropDownList)e.Row.FindControl("ddCategory");
            string sql = "SELECT distinct vCATEGORY FROM vItemMaster where ItemType='" + ddItemType.SelectedItem.Text + "' ORDER BY vCATEGORY";
            string conString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter(sql, con))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        ddCategory.DataSource = dt;
                        ddCategory.DataTextField = "vCATEGORY";
                        ddCategory.DataValueField = "vCATEGORY";
                        ddCategory.DataBind();
                        string selectedvCATEGORY = DataBinder.Eval(e.Row.DataItem, "vCATEGORY").ToString();
                        ddCategory.Items.FindByValue(selectedvCATEGORY).Selected = true;
                    }
                }
            }
        }


        

    }
}