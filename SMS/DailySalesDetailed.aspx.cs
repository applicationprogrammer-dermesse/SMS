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

namespace SMS
{
    public partial class DailySalesDetailed : System.Web.UI.Page
    {
        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

        public string startDate;
        public string EndDate;
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
                            if (Session["sDate"].ToString() == "01/01/1900")
                            {
                                var yesterday = DateTime.Now.AddDays(-1);
                                txtDateFrom.Text = yesterday.ToShortDateString();
                                txtDateTo.Text = yesterday.ToShortDateString(); //SysClass.getLday(EndDate);

                            }
                            else
                            {
                                txtDateFrom.Text = Session["sDate"].ToString();
                                txtDateTo.Text = Session["eDate"].ToString();
                            }

                            if (Session["theBRnm"].ToString() == "Select Branch")
                            {
                                loadBranch();
                            }
                            else
                            {
                                loadBranch();
                                ddBranch.SelectedItem.Text = Session["theBRnm"].ToString();
                                ddBranch.SelectedValue = Session["theBRcd"].ToString();
                                loadSalesDetailed();
                            }
                            
                            
                        }
                        else
                        {
                            if (Session["sDate"].ToString() == "01/01/1900")
                            {
                                var yesterday = DateTime.Now.AddDays(-1);
                                txtDateFrom.Text = yesterday.ToShortDateString();
                                txtDateTo.Text = yesterday.ToShortDateString(); //SysClass.getLday(EndDate);

                            }
                            else
                            {
                                txtDateFrom.Text = Session["sDate"].ToString();
                                txtDateTo.Text = Session["eDate"].ToString();
                            }

                            
                            loadPerBranch();
                            loadSalesDetailed();


                        }
                

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
                    ddBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }


        public int theOption;
        private void loadSalesDetailed()
        {
            if (ddBranch.SelectedValue == "0")
            {
                lblMsgWarning.Text = "Please select branch";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {


                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SalesDetailedForTheMonthPerBranch";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSalesDetailed.DataSource = dT;
                        gvSalesDetailed.DataBind();

                        if (gvSalesDetailed.Rows.Count == 0)
                        {
                            lblMsgWarning.Text = "No record found";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;

                        }

                    }
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            loadSalesDetailed();
        }



        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvSalesDetailed.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                printSalesDetailed();
            }
        }




        private void printSalesDetailed()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptSalesDetailed.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesDetailed.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesDetailed.xlsx");

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


                    for (int i = 0; i < gvSalesDetailed.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 5, 5).Value = Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 5, 6).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(i + 5, 7).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblNetAmt")).Text;
                        worksheet.Cell(i + 5, 8).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblCASHAMT")).Text;
                        worksheet.Cell(i + 5, 9).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblCreditCardAMT")).Text;
                        worksheet.Cell(i + 5, 10).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblDebitCardAMT")).Text;
                        worksheet.Cell(i + 5, 11).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblPaymayaAMT")).Text;
                        worksheet.Cell(i + 5, 12).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblVoucherAMT")).Text;
                        worksheet.Cell(i + 5, 13).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblGiftChequeAMT")).Text;
                        worksheet.Cell(i + 5, 14).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblBankFundTransferAMT")).Text;
                        worksheet.Cell(i + 5, 15).Value = ((Label)gvSalesDetailed.Rows[i].FindControl("lblDigitalPaymentAMT")).Text;
                        worksheet.Cell(i + 5, 16).Value = Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[15].Text);
                        worksheet.Cell(i + 5, 17).Value = Server.HtmlDecode(gvSalesDetailed.Rows[i].Cells[16].Text);

                        worksheet.Cell(i + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

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

        protected void gvSalesDetailed_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                string cellTranNoToView = gvr.Cells[0].Text;
                string cellSatus = gvr.Cells[14].Text;
                Session["ViewTransactionDetailPosted"] = cellTranNoToView;
                Session["cellSatusPosted"] = cellSatus;

                Session["sDate"] = txtDateFrom.Text;
                Session["eDate"] = txtDateTo.Text;
                Session["theBRnm"] = ddBranch.SelectedItem.Text;
                Session["theBRcd"] = ddBranch.SelectedValue;
                //Response.Redirect("~/ViewTransactionPosted.aspx?val=" + Session["ViewTransactionDetailPosted"].ToString());
                Response.Redirect("~/ViewTransactionPosted.aspx?val=" + Session["ViewTransactionDetailPosted"].ToString() + "(" + Session["cellSatusPosted"] + ")");
                //Response.Redirect("~/ViewTransaction.aspx");
            }
        }

        protected void gvSalesDetailed_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")) == "Void")
                {

                    e.Row.ForeColor = System.Drawing.Color.Red;
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                }

            }
        }

    }
}
