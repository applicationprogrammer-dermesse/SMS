using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace SMS
{
    public partial class Deliveries : System.Web.UI.Page
    {

        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmpNo"] == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='LoginPage.aspx';", true);
            }
            else
            {
                if (!IsPostBack)
                {
                    ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        Response.Redirect("~/UnauthorizedPage.aspx");
                    }
                    else
                    {
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
                        lblBranch.Text = Session["Dept"].ToString();
                        var today = DateTime.Now;
                        txtDate.Text = today.ToShortDateString();
                        LoadDrDetail();
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

        }


        protected void btnGet_Click(object sender, EventArgs e)
        {
            lblNote.Text = string.Empty;
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    CheckIfDRExists();

                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }


        private void CheckIfDRExists()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"dbo.CheckIFDRAlreadyPosted";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrName", lblBranch.Text);
                    cmD.Parameters.AddWithValue("@DR_Number", txtDelReceiptNo.Text.Trim());
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);
                    if (dT.Rows.Count > 0)
                    {
                        GetDrDetail();
                    }
                    else
                    {
                        DoubleCheckDR();
                        
                    }

                }
            }
            //GetDrDetail();
        }


        private void DoubleCheckDR()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"SELECT DISTINCT DR_Number FROM UnpostedDelivery WHERE DR_Number=@DR_Number
	                            UNION ALL
	                            SELECT DISTINCT DR_Number FROM PostedDelivery WHERE DR_Number=@DR_Number";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@DR_Number", txtDelReceiptNo.Text.Trim());
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataTable dT = new DataTable();
                    dA.Fill(dT);
                    if (dT.Rows.Count > 0)
                    {
                        lblMsgWarning.Text = "DR Number already exists in download record!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                        
                    }
                    else
                    {
                        lblMsgWarning.Text = "Invalid DR Number.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                }
            }
            //GetDrDetail();
        }

        private void LoadDrDetail()
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT [ID]
                          ,[BrCode]
                          ,[DR_Number]
                          ,[MRFNo]
                          ,[vPluCode]
                          ,[vFGCode]
                          ,[vDESCRIPTION]
                          ,[vQty]
                          ,[vBatchNo]
                          ,[vDateExpiry]
                      FROM [UnpostedDelivery] WHERE [BrCode]=@BrCode";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvDeliveries.DataSource = dT;
                        gvDeliveries.DataBind();

                        if (dT.Rows.Count > 0)
                        {
                            lblNote.Text = string.Empty;
                        }
                        else
                        {
                            lblNote.Text = "No record found";
                        }

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


        private void GetDrDetail()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"dbo.Get_DRdetail";
                        using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                        {
                            sqlConn.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@BrName", Session["Dept"].ToString());
                            cmD.Parameters.AddWithValue("@DR_Number", txtDelReceiptNo.Text);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    LoadDrDetail();
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }


        protected void btnPost_Click(object sender, EventArgs e)
        {
            PostDelivery();
        }

        public string strSRS;
        private void PostDelivery()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvrow in gvDeliveries.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRS += "'" + gvDeliveries.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRS != null)
                {

                    foreach (GridViewRow gvR in gvDeliveries.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            Label lbQty = gvR.Cells[8].FindControl("lblQty") as Label;
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            if (chk != null & chk.Checked & gvR.Cells[7].Text != "No matching itemcode on item master list")
                            {
                                string Rec = gvDeliveries.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "dbo.PostUnpostedDelivery";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@vFGCode", gvR.Cells[6].Text);
                                        cmD.Parameters.AddWithValue("@Qty", ((Label)gvR.FindControl("lblQty")).Text);
                                        cmD.Parameters.AddWithValue("@vBatchNo", gvR.Cells[9].Text);
                                        cmD.Parameters.AddWithValue("@vDateExpiry", gvR.Cells[10].Text);

                                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["EmpNo"].ToString());
                                        cmD.Parameters.AddWithValue("@DateReceived", txtDate.Text);

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }

                    txtDelReceiptNo.Text = string.Empty;
                    LoadDrDetail();
                    lblMsgWarning.Text = "Delivery Succesfully posted.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {

                    lblMsgWarning.Text = "Please check at least one item to post.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }


        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearDelivery();
        }

        public string strSRSClear;
        private void ClearDelivery()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvrow in gvDeliveries.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRSClear += "'" + gvDeliveries.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRSClear != null)
                {

                    foreach (GridViewRow gvR in gvDeliveries.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            Label lbQty = gvR.Cells[8].FindControl("lblQty") as Label;
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            if (chk != null & chk.Checked)
                            {
                                string Rec = gvDeliveries.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "Delete from [UnpostedDelivery] where ID=@ID";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        //cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }

                    //txtDelReceiptNo.Text = string.Empty;
                    LoadDrDetail();
                    //lblMsgWarning.Text = "Delivery Succesfully posted.";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    //return;
                }
                else
                {

                    lblMsgWarning.Text = "Please check at least one item to delete";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }

        protected void gvDeliveries_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "vDESCRIPTION")) == "No matching itemcode on item master list")
            {
                e.Row.BackColor = System.Drawing.Color.PaleGoldenrod;
            }

        }


    }
}