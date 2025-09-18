using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.IO;
//using System.Threading;
//using System.Reflection;


namespace SMS
{
    public partial class IssuanceForPosting : System.Web.UI.Page
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
                        loadAllBranch();
                        loadUnpostedIssuanceAllBranch();
                        btnSubmit.Disabled = true;
                        btnSubmit.Visible = false;
                    }
                    else
                    {
                        loadPerBranch();
                        loadUnpostedIssuancePerBranch();
                        loadIssueSlipNoPerBranch();
                        btnSubmit.Disabled = false;
                        btnSubmit.Visible = true;
                    }



                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();
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


        private void loadAllBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadBranches";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    ddBranch.Items.Insert(0, new ListItem("Select branch", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }

        private void loadPerBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode='" + Session["vUser_Branch"].ToString() + "'  ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    //ddBranch.Items.Insert(0, new ListItem("Select branch", "0"));
                    //ddBranch.SelectedIndex = 0;
                }
            }
        }


        private void loadUnpostedIssuanceAllBranch()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.IssuanceNo
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vBatchNo]
                                      ,A.[vDateExpiry]
                                      ,A.[vQty]
                                      ,A.Remarks
                                      ,A.IssuanceDate
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                      ,A.[IsPIS]
                                      ,A.SourceRecord
                                      ,A.vItemID

                                  FROM [UnpostedIssuance] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 ORDER BY A.BrCode,A.IssuanceNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvIssuance.DataSource = dT;
                    gvIssuance.DataBind();


                }
            }
        }

        private void loadUnpostedIssuancePerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.IssuanceNo
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vBatchNo]
                                      ,A.[vDateExpiry]
                                      ,A.[vQty]
                                      ,A.Remarks
                                       ,A.IssuanceDate
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                      ,A.[IsPIS]
                                      ,A.SourceRecord
                                      ,A.vItemID

                                  FROM [UnpostedIssuance] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and a.BrCode=@BrCode ORDER BY A.BrCode,A.IssuanceNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvIssuance.DataSource = dT;
                    gvIssuance.DataBind();


                }
            }
        }

        private void loadUnpostedIssuancePerIssuanceNo()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.IssuanceNo
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                       ,A.[vBatchNo]
                                      ,A.[vDateExpiry]
                                      ,A.[vQty]
                                      ,A.Remarks
                                      ,A.IssuanceDate
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                      ,A.[IsPIS]
                                      ,A.SourceRecord
                                      ,A.vItemID

                                  FROM [UnpostedIssuance] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and IssuanceNo=@IssuanceNo ORDER BY A.BrCode,A.IssuanceNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@IssuanceNo", ddIssueNo.SelectedItem.Text);
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvIssuance.DataSource = dT;
                    gvIssuance.DataBind();


                }
            }
        }


        protected void gvIssuance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    string dKey = gvIssuance.DataKeys[e.RowIndex].Value.ToString();
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"dbo.DeleteUnpostedIssuance";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ID", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    gvIssuance.EditIndex = -1;
                    if (ddBranch.SelectedValue == "0")
                    {
                        loadUnpostedIssuanceAllBranch();
                    }
                    else
                    {
                        if (ddIssueNo.SelectedItem.Text == string.Empty | ddIssueNo.SelectedValue == "0")
                        {
                            loadUnpostedIssuancePerBranch();
                        }
                        else
                        {
                            loadUnpostedIssuancePerIssuanceNo();

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
        }

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddBranch.SelectedValue == "0")
            {
                ddIssueNo.Items.Clear();
            }
            else
            {
                loadIssueSlipNoPerBranch();
                loadUnpostedIssuancePerBranchToPost();
            }
        }

        private void loadIssueSlipNoPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT distinct IssuanceNo
                                  FROM [UnpostedIssuance]
                                  WHERE BrCode=@BrCode and vStat=1 ORDER BY IssuanceNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddIssueNo.Items.Clear();
                    ddIssueNo.DataSource = dR;
                    ddIssueNo.DataValueField = "IssuanceNo";
                    ddIssueNo.DataTextField = "IssuanceNo";
                    ddIssueNo.DataBind();
                    ddIssueNo.Items.Insert(0, new ListItem("Select Control No.", "0"));
                    ddIssueNo.SelectedIndex = 0;



                }
            }

        }

        public string strSRS;
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvrow in gvIssuance.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRS += "'" + gvIssuance.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRS != null)
                {

                    foreach (GridViewRow gvR in gvIssuance.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            Label lbQty = gvR.Cells[6].FindControl("lblQty") as Label;
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            if (chk != null & chk.Checked)
                            {
                                string Rec = gvIssuance.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "dbo.PostUnpostedIssuance";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@vItemID", gvR.Cells[2].Text);
                                        cmD.Parameters.AddWithValue("@vFGCode", gvR.Cells[5].Text);
                                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                                        cmD.Parameters.AddWithValue("@vQtyIssuance", ((Label)gvR.FindControl("lblQty")).Text);
                                        cmD.Parameters.AddWithValue("@vBatchNo", ((Label)gvR.FindControl("lblBatchNoDR")).Text);
                                        cmD.Parameters.AddWithValue("@vDateExpiry", ((Label)gvR.FindControl("lblvDateExpiry")).Text);
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["FullName"].ToString());
                                        cmD.Parameters.AddWithValue("@IsPIS", gvR.Cells[12].Text);

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }

                    loadIssueSlipNoPerBranch();
                    loadUnpostedIssuancePerBranchToPost();

                    lblMsgWarning.Text = "Successfully posted";
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

        protected void ddIssueNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddIssueNo.SelectedValue == "0")
            {
                loadUnpostedIssuancePerBranchToPost();
            }
            else
            {
                loadUnpostedIssuancePerIssuanceNo();
            }
        }



        private void loadUnpostedIssuancePerBranchToPost()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.IssuanceNo
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                        ,A.[vBatchNo]
                                      ,A.[vDateExpiry]
                                      ,A.[vQty]
                                      ,A.Remarks
                                      ,A.IssuanceDate
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                      ,A.[IsPIS]
                                      ,A.SourceRecord
                                      ,A.vItemID
                                  FROM [UnpostedIssuance] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and A.BrCode=@BrCode ORDER BY A.BrCode,A.IssuanceNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvIssuance.DataSource = dT;
                    gvIssuance.DataBind();


                }
            }
        }

        protected void gvIssuance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvIssuance.EditIndex = e.NewEditIndex;

            if (ddBranch.SelectedValue == "0")
            {
                loadUnpostedIssuanceAllBranch();
            }
            else
            {
                if (ddIssueNo.SelectedItem.Text == "Select Control No." | ddIssueNo.SelectedValue=="0")
                {
                    loadUnpostedIssuancePerBranch();
                }
                else
                {
                    loadUnpostedIssuancePerIssuanceNo();
                    
                }
            }

            
            //try
            //{
            //    if (ddIssueNo.SelectedValue == "0")
            //    {
            //        gvIssuance.EditIndex = -1;
            //        lblMsgWarning.Text = "You must select Issue Slip No. on the dropdown list above before editing.";
            //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //        return;
            //    }
            //    else
            //    {
            //        gvIssuance.EditIndex = e.NewEditIndex;
            //        loadUnpostedIssuancePerIssuanceNo();
            //    }
            //}
            //catch (Exception x)
            //{
            //    gvIssuance.EditIndex = -1;
            //    lblMsgWarning.Text = "You must select Issue Slip No. on the dropdown list above before editing.";
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //    return;
            //}
        }

        protected void gvIssuance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvIssuance.EditIndex = -1;
            //loadUnpostedIssuancePerIssuanceNo();
            if (ddBranch.SelectedValue == "0")
            {
                loadUnpostedIssuanceAllBranch();
            }
            else
            {
                if (ddIssueNo.SelectedItem.Text == string.Empty | ddIssueNo.SelectedValue == "0")
                {
                    loadUnpostedIssuancePerBranch();
                }
                else
                {
                    loadUnpostedIssuancePerIssuanceNo();

                }
            }
        }

        protected void gvIssuance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvR = gvIssuance.Rows[e.RowIndex];

            string vItemID = gvR.Cells[2].Text;
            string SourceRecord = gvR.Cells[13].Text;

            string RecNo = gvIssuance.DataKeys[e.RowIndex].Value.ToString();
            TextBox xtQtyIssuance = gvIssuance.Rows[e.RowIndex].FindControl("txtQtyIssuance") as TextBox;
            TextBox xtRemarks = gvIssuance.Rows[e.RowIndex].FindControl("txtRemarks") as TextBox;
            TextBox xtBatchNoDR = gvIssuance.Rows[e.RowIndex].FindControl("txtBatchNoDR") as TextBox;
            TextBox xtDateExpiryDR = gvIssuance.Rows[e.RowIndex].FindControl("txtDateExpiryDR") as TextBox;

            Label xlblQtyOrig = gvIssuance.Rows[e.RowIndex].FindControl("lblQtyOrig") as Label;
            
            TextBox xtIssuanceDateDR = gvIssuance.Rows[e.RowIndex].FindControl("txtIssuanceDateDR") as TextBox;

            UpdateGrid(RecNo, xtQtyIssuance, xtRemarks, xtBatchNoDR, xtDateExpiryDR, xtIssuanceDateDR, xlblQtyOrig,vItemID,SourceRecord);




        }

        private void UpdateGrid(string RecNo, TextBox xtQtyIssuance, TextBox xtRemarks, TextBox xtBatchNoDR, TextBox xtDateExpiryDR, TextBox xtIssuanceDateDR, Label xlblQtyOrig, string vItemID, string SourceRecord)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.UpdateUnpostedIssuance";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@vQty", xtQtyIssuance.Text);
                        cmD.Parameters.AddWithValue("@vQtyOrig", xlblQtyOrig.Text);
                        cmD.Parameters.AddWithValue("@Remarks", xtRemarks.Text);
                        cmD.Parameters.AddWithValue("@vBatchNo", xtBatchNoDR.Text);
                        cmD.Parameters.AddWithValue("@vDateExpiry", xtDateExpiryDR.Text);

                        cmD.Parameters.AddWithValue("@IssuanceDate", xtIssuanceDateDR.Text);
                        cmD.Parameters.AddWithValue("@vRecNum", RecNo);

                        cmD.Parameters.AddWithValue("@vItemID", vItemID.ToString());
                        cmD.Parameters.AddWithValue("@SourceRecord", SourceRecord.ToString());

                        cmD.ExecuteNonQuery();
                    }

                    gvIssuance.EditIndex = -1;
                    if (ddBranch.SelectedValue == "0")
                    {
                        loadUnpostedIssuanceAllBranch();
                    }
                    else
                    {
                        if (ddIssueNo.SelectedItem.Text == string.Empty | ddIssueNo.SelectedValue == "0")
                        {
                            loadUnpostedIssuancePerBranch();
                        }
                        else
                        {
                            loadUnpostedIssuancePerIssuanceNo();

                        }
                    }
                    //loadUnpostedIssuancePerIssuanceNo();
                }
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message + "\nPlease check character encoded.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        protected void gvIssuance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

                if (Session["vUser_Branch"].ToString() != "1")
                {
                    ((LinkButton)e.Row.FindControl("lnkEditGrid")).ForeColor = System.Drawing.Color.Gray;
                    ((LinkButton)e.Row.FindControl("lnkEditGrid")).Enabled = false;
                }
            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvIssuance.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Please generate data to export!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                Export();
            }
        }

        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;
        private void Export()
        {

            string localPath = Server.MapPath("~/exlTMP/rptUnpostedIssuances.xlsx");
            string newPath = Server.MapPath("~/exlDUMP/rptUnpostedIssuances.xlsx");
            newFileName = Server.MapPath("~/exlDUMP/UnpostedIssuances.xlsx");

            File.Copy(localPath, newPath, overwrite: true);

            FileInfo fi = new FileInfo(newPath);
            if (fi.Exists)
            {
                if (File.Exists(newFileName))
                {
                    File.Delete(newFileName);
                }

                fi.MoveTo(newFileName);
                var workbook = new XLWorkbook(newFileName);
                var worksheet = workbook.Worksheet(1);

                

                worksheet.Cell("A1").Value = "UNPOSTED ISSUANCE";
                //worksheet.Cell("A3").Value = "Covered Date = " + txtDateFrom.Text + " to " + txtDateTo.Text;

                for (int i = 0; i < gvIssuance.Rows.Count; i++)
                {
                    worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvIssuance.Rows[i].Cells[3].Text);
                    worksheet.Cell(i + 4, 2).Value = "'" + Server.HtmlDecode(gvIssuance.Rows[i].Cells[4].Text);
                    worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvIssuance.Rows[i].Cells[5].Text);
                    worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvIssuance.Rows[i].Cells[6].Text);
                    worksheet.Cell(i + 4, 5).Value = ((Label)gvIssuance.Rows[i].FindControl("lblBatchNoDR")).Text;
                    worksheet.Cell(i + 4, 6).Value = ((Label)gvIssuance.Rows[i].FindControl("lblvDateExpiry")).Text;
                    worksheet.Cell(i + 4, 7).Value = ((Label)gvIssuance.Rows[i].FindControl("lblQty")).Text;
                    worksheet.Cell(i + 4, 8).Value = ((Label)gvIssuance.Rows[i].FindControl("lblRemarks")).Text;
                    worksheet.Cell(i + 4, 9).Value = ((Label)gvIssuance.Rows[i].FindControl("lblIssuanceDate")).Text;
                    

                    worksheet.Cell(i + 4, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 4, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    



                }



                var fileName = Path.GetFileName(newFileName);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "inline; filename=" + fileName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    workbook.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }

            }

        }


    }
}