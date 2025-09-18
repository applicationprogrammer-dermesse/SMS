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
using System.Threading;
using System.Reflection;
using System.Globalization;

namespace SMS
{
    public partial class SalesBook : System.Web.UI.Page
    {
        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

        public string startDate;
        public string EndDate;

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
                        txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = DateClass.getLday(EndDate);
                        loadBranch();
                        btnProcess.Enabled = false;
                    }
                    else
                    {

                        btnProcess.Enabled = true;
                        //txtDateFrom.Text = SysClass.getSday(startDate);
                        //txtDateTo.Text = SysClass.getLday(EndDate);
                       // var yesterday = DateTime.Now.AddDays(-1);

                       
                        //txtDateTo.Text = yesterday.ToShortDateString();
                        loadPerBranch();
                        

                        LoadSystemDate();
                        LoadLastProcessDate();

                        //txtDateFrom.Text = DateClass.getSday(startDate);
                        DateTime dtLast = Convert.ToDateTime(txtDateTo.Text);

                        txtDateFrom.Text = txtDateTo.Text.Substring(0, 2) + "/01/" + txtDateTo.Text.Substring(6, 4); 

                        loadSalesBook();
                    }


                }
            }
        }

        private void loadBranch()
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
                    ddBranch.Items.Insert(0, new ListItem("Please select branch", "0"));
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
                    //ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    //ddBranch.SelectedIndex = 0;
                }
            }
        }

        public int theOption;
        private void loadSalesBook()
        {
            if (ddBranch.SelectedValue == "0")
            {
                lblMsgWarning.Text = "Please select branch to generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                if (rbDESC.Checked==true)
                {
                    theOption = 1;
                }
                else
                {
                    theOption = 2;
                }

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"rptSalesBook";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@Opt", theOption);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSalesBook.DataSource = dT;
                        gvSalesBook.DataBind();

                       
                       
                            //lblMsgWarning.Text = "No record found";
                            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            //return;
                       
                    }
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            loadSalesBook();
        }



        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvSalesBook.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                printSalesSummary();
            }
        }




        private void printSalesSummary()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptSalesBook.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesBook.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesBook.xlsx");

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


                    worksheet.Cell("A1").Value = ddBranch.SelectedItem.Text;
                    worksheet.Cell("A2").Value = "Transaction Date :  " + txtDateFrom.Text + " - " + txtDateTo.Text;


                    for (int i = 0; i < gvSalesBook.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvSalesBook.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 2).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblPrevGT_amt")).Text;
                        worksheet.Cell(i + 4, 3).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblProduct_amt")).Text;
                        worksheet.Cell(i + 4, 4).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblService_amt")).Text;
                        worksheet.Cell(i + 4, 5).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblProdDisc_amt")).Text;
                        worksheet.Cell(i + 4, 6).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblServDisc_amt")).Text;
                        worksheet.Cell(i + 4, 7).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblNetSales_amt")).Text;
                        worksheet.Cell(i + 4, 8).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblCash_amt")).Text;
                        worksheet.Cell(i + 4, 9).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblCCard_amt")).Text;
                        worksheet.Cell(i + 4, 10).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblDebit_amt")).Text;
                        worksheet.Cell(i + 4, 11).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblBankTransfer_amt")).Text;
                        worksheet.Cell(i + 4, 12).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblVoucher_amt")).Text;
                        worksheet.Cell(i + 4, 13).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblPaymaya_amt")).Text;
                        worksheet.Cell(i + 4, 14).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblDigital_amt")).Text;
                        worksheet.Cell(i + 4, 15).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblGiftCheque_amt")).Text;
                        worksheet.Cell(i + 4, 16).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblCurrGT_amt")).Text;
                        worksheet.Cell(i + 4, 17).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblVat_amt")).Text;
                        worksheet.Cell(i + 4, 18).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblVATExempt_amt")).Text;
                        worksheet.Cell(i + 4, 19).Value = ((Label)gvSalesBook.Rows[i].FindControl("lblVatableAmt")).Text;
                        worksheet.Cell(i + 4, 20).Value = Server.HtmlDecode(gvSalesBook.Rows[i].Cells[21].Text);

                        worksheet.Cell(i + 4, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 18).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 19).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 20).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

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

            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }


        private void LoadSystemDate()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT DATEADD(d,-1, CurrentDate) FROM SystemMaster where BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
        
                        lblSMSTransactionDate.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                    }

                }
            }
        }

        private void LoadLastProcessDate()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT  CONVERT(CHAR(10),MAX(SalesDate),101) FROM [SalesBook] where BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {

                        lblLastProcessDate.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                        txtDateTo.Text = dR[0].ToString();
                        
                    }
                    if (Convert.ToDateTime(lblSMSTransactionDate.Text) == Convert.ToDateTime(lblLastProcessDate.Text))
                    {
                        btnProcess.Enabled = false;
                    }
                    else
                    {
                        btnProcess.Enabled = true;
                    }

                }
            }
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (Convert.ToDateTime(lblSMSTransactionDate.Text) == Convert.ToDateTime(lblLastProcessDate.Text))
                {
                    lblMsgWarning.Text = "End of Day processing not yet done.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"dbo.GenSalesBook";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                            cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                            cmD.ExecuteNonQuery();
                        }

                        
                    }

                    LoadLastProcessDate();
                    loadSalesBook();
                    
                }
            }
        }

        //public DateTime theDateToDelete;
        protected void gvSalesBook_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //if (Session["vUser_Branch"].ToString() == "1")
            //{
            //    lblMsgWarning.Text = "You are not authorize to delete transaction!";
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //    return;
            //}
            //else
            //{
                lblDate.Text = gvSalesBook.Rows[e.RowIndex].Cells[2].Text;

                lblMsgConfirm.Text = "Are you sure want to delete this record?";
                lblConfirmNote.Text = "NOTE: This will also delete all records from the selected sales date up to the last record(if any).";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowConfirmMsg();", true);
                return;
            //}

        }


        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

                //DateTime theDate = theDateToDelete;

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"Delete from SalesBook where SalesDate >= @SalesDate and BrCode=@BrCode";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@SalesDate", lblDate.Text);
                        cmD.ExecuteNonQuery();
                    }


                }

                LoadLastProcessDate();
                loadSalesBook();
                lblDate.Text = string.Empty;
            }
        }

        protected void gvSalesBook_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName=="RePrint")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                string cellBrName = gvr.Cells[1].Text;
                string cellDate = gvr.Cells[2].Text;
                Session["cellDate"] = cellDate;
                Session["cellBranch"] = ddBranch.SelectedValue;
                Session["cellBranchName"] = cellBrName;

                //Response.Redirect("~/RePrintXandZ.aspx");
                //Response.Redirect("~/RePrintXandZ.aspx");
                Response.Redirect("~/RePrintXandZ.aspx?val=" + Session["cellDate"].ToString() + " - " + Session["cellBranchName"].ToString());
               
            }
        }

        protected void gvSalesBook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "CIT")
            {
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[22].Visible = true;
            }
            else
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[22].Visible = false;
            }
        }


    }
}