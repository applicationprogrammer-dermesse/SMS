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
    public partial class SetupDiscount : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                if (Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "CIT")
                {
                    LoadFGItem();
                    loadBranches();

                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[2] 
                            {  new DataColumn("vFGCode"), 
                                new DataColumn("vDESCRIPTION")});
                    ViewState["ItemDetail"] = dt;
                    this.BindItemDetailGrid();

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

        protected void BindItemDetailGrid()
        {

            grdItem.DataSource = (DataTable)ViewState["ItemDetail"];
            grdItem.DataBind();

        }

        private void LoadFGItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT vFGCode,LTRIM(RTRIM(vDESCRIPTION)) AS [vDESCRIPTION] FROM vItemMaster where vStat=1 and vPluCode like '0%' and IsPromo=0";
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


        private void loadBranches()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [BrCode]
                                      ,[BrName]
                                  FROM [MyBranchList] Where BrCode between 4 and 29 and BrCode <> 10 ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                   //cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvBranches.DataSource = dT;
                    gvBranches.DataBind();

        
                }
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            if (ddITemFG.SelectedValue == "0")
            {
                lblMsgWarning.Text = "Please select item";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                if (rbItemsYes.Checked == true)
                {
                    PostAddedDiscount();
                }

                else
                {
                    DataTable dt = (DataTable)ViewState["ItemDetail"];
                    dt.Rows.Add(ddITemFG.SelectedValue,
                         ddITemFG.SelectedItem.Text.Trim());
                    ViewState["ItemDetail"] = dt;
                    this.BindItemDetailGrid();
                }
            }
            
        }

        protected void grdItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            GridViewRow row = grdItem.Rows[e.RowIndex];
            DataTable dt = ViewState["ItemDetail"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["ItemDetail"] = dt;
            BindItemDetailGrid();
        }

        public string strSRS;
        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = "SELECT sConstant FROM [dbo].[tblTypeDiscount] where sConstant=@sConstant";

                    sqlConn.Open();

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@sConstant", txtDiscountCode.Text);
                        SqlDataReader dR = cmD.ExecuteReader();
                        if (dR.HasRows)
                        {
                            txtDiscountCode.Focus();
                            lblMsgWarning.Text = txtDiscountCode.Text + " already exists";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            PostAddedDiscount();
                        }

                    }
                }

            }

            
        }

        private void PostAddedDiscount()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (rbItemsYes.Checked == true & grdItem.Rows.Count > 0)
                {
                    lblMsgWarning.Text = "Please delete items if All Items option is <b>YES</b>";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    foreach (GridViewRow gvrow in gvBranches.Rows)
                    {
                        CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                        if (chk != null & chk.Checked)
                        {
                            strSRS += "'" + gvBranches.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                        }

                    }

                    if (strSRS != null)
                    {

                        foreach (GridViewRow gvR in gvBranches.Rows)
                        {
                            if (gvR.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                                if (chk != null & chk.Checked)
                                {

                                    string Rec = gvBranches.DataKeys[gvR.RowIndex].Value.ToString();
                                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                    {
                                        string stR = "dbo.CreateDiscount";

                                        sqlConn.Open();

                                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                        {
                                            cmD.CommandTimeout = 0;
                                            cmD.CommandType = CommandType.StoredProcedure;

                                            cmD.Parameters.AddWithValue("@BrCode", gvR.Cells[1].Text);
                                            cmD.Parameters.AddWithValue("@sDescription", txtDescription.Text);
                                            cmD.Parameters.AddWithValue("@sConstant", txtDiscountCode.Text.ToUpper());
                                            cmD.Parameters.AddWithValue("@iPercent", txtPercentage.Text);

                                            cmD.Parameters.AddWithValue("@mDiscount_amt", txtDiscountAmount.Text);

                                            if (rbVatYes.Checked == true)
                                            {
                                                cmD.Parameters.AddWithValue("@iSpecialPercent", 112);
                                            }
                                            else
                                            {
                                                cmD.Parameters.AddWithValue("@iSpecialPercent", 0);
                                            }

                                            cmD.Parameters.AddWithValue("@iValidFrom_dt", Convert.ToDateTime(txtDateFrom.Text).ToString("yyyyMMdd"));
                                            cmD.Parameters.AddWithValue("@iValidUntil_dt", Convert.ToDateTime(txtDate.Text).ToString("yyyyMMdd"));

                                            if (rbItemsYes.Checked == true)
                                            {
                                                cmD.Parameters.AddWithValue("@iAllItems_fl", 1);
                                            }
                                            else
                                            {
                                                cmD.Parameters.AddWithValue("@iAllItems_fl", 0);
                                            }

                                            if (ckIsPromo.Checked == true)
                                            {
                                                cmD.Parameters.AddWithValue("@IsPromo", 1);
                                            }
                                            else
                                            {
                                                cmD.Parameters.AddWithValue("@IsPromo", 0);
                                            }

                                            cmD.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }


                        if (rbItemsNo.Checked == true)
                        {
                            saveDiscountItem();
                        }
                        else
                        {
                            clearDetail();
                            lblMsgWarning.Text = "Discount succesfully added";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                    }
                    else
                    {
                        lblMsgWarning.Text = "Please select at least one branch";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }
        }

        private void saveDiscountItem()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvRItems in grdItem.Rows)
                {
                    if (gvRItems.RowType == DataControlRowType.DataRow)
                    {
                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                        {
                            string stR = "dbo.insertIntotblTypeDiscountDetail";

                            sqlConn.Open();

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;

                                //cmD.Parameters.AddWithValue("@BrCode", gvRItems.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@sDescription", gvRItems.Cells[1].Text);
                                cmD.Parameters.AddWithValue("@sConstant", txtDiscountCode.Text.ToUpper());
                                cmD.Parameters.AddWithValue("@vFGCode", gvRItems.Cells[0].Text);
                                cmD.ExecuteNonQuery();
                            }
                        }
                    }
                }

                clearDetail();
                lblMsgWarning.Text = "Discount succesfully added";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }

        private void clearDetail()
        {
           // txtDate.Text = string.Empty;
            //txtDateFrom.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtDiscountAmount.Text = string.Empty;
            txtDiscountCode.Text = string.Empty;
            txtPercentage.Text = string.Empty;
            LoadFGItem();


            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[2] 
                            {  new DataColumn("vFGCode"), 
                                new DataColumn("vDESCRIPTION")});
            ViewState["ItemDetail"] = dt;
            this.BindItemDetailGrid();

        }
        protected void rbItemsNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbItemsNo.Checked == true)
            {
                ddITemFG.Enabled = true;
                LoadFGItem();
            }
        }

        protected void rbItemsYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbItemsYes.Checked == true)
            {
                ddITemFG.DataSource = null;
                ddITemFG.Items.Clear();
                ddITemFG.Enabled = false;
            }
        }

        
    }
}