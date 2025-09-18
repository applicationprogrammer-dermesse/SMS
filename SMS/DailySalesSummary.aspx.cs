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
    public partial class DailySalesSummary : System.Web.UI.Page
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
                        txtDateFrom.Text =DateClass.getSday(startDate);
                        txtDateTo.Text = DateClass.getLday(EndDate);
                        loadBranch();
                    }
                    else
                    {

                        //txtDateFrom.Text = SysClass.getSday(startDate);
                        //txtDateTo.Text = SysClass.getLday(EndDate);
                        var yesterday = DateTime.Now.AddDays(-1);

                        txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = yesterday.ToShortDateString();
                        loadPerBranch();
                        loadSalesSummary();
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
                    ddBranch.Items.Insert(0, new ListItem("All branches", "0"));
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
        public string stRSum;
        private void loadSalesSummary()
        {
            if (ddBranch.SelectedValue == "0")
            {
                stRSum = @"dbo.SalesSummaryForTheMonthAllBranch";
            }
            else
            {
                stRSum = @"dbo.SalesSummaryForTheMonthPerBranch";
            }

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    
                    using (SqlCommand cmD = new SqlCommand(stRSum, conN))
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

                        gvSalesSummary.DataSource = dT;
                        gvSalesSummary.DataBind();

                        if (gvSalesSummary.Rows.Count > 0)
                        {
                            //decimal total2 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Product - Gross Amt"));
                            gvSalesSummary.FooterRow.Cells[1].Text = "Total";
                            gvSalesSummary.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            decimal total2 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Product - Gross Amt"));
                            gvSalesSummary.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[2].Text = total2.ToString("N2");

                            decimal total3 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Product - DiscountAmt"));
                            gvSalesSummary.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[3].Text = total3.ToString("N2");

                            //decimal total4 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Product - NetAmt"));
                            //gvSalesSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            //gvSalesSummary.FooterRow.Cells[4].Text = total4.ToString("N2");

                            decimal total4 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Service - Gross Amt"));
                            gvSalesSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[4].Text = total4.ToString("N2");

                            decimal total5 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Service - DiscountAmt"));
                            gvSalesSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[5].Text = total5.ToString("N2");

                            //decimal total7 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Service - NetAmt"));
                            //gvSalesSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            //gvSalesSummary.FooterRow.Cells[7].Text = total7.ToString("N2");

                            decimal total6 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Net Sales"));
                            gvSalesSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[6].Text = total6.ToString("N2");

                            decimal total7 = dT.AsEnumerable().Sum(row => row.Field<decimal>("CASH AMT"));
                            gvSalesSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[7].Text = total7.ToString("N2");

                            decimal total8 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Credit Card AMT"));
                            gvSalesSummary.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[8].Text = total8.ToString("N2");

                            decimal total9 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Debit Card AMT"));
                            gvSalesSummary.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[9].Text = total9.ToString("N2");

                            decimal total10 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Paymaya AMT"));
                            gvSalesSummary.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[10].Text = total10.ToString("N2");

                            decimal total11 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Voucher AMT"));
                            gvSalesSummary.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[11].Text = total11.ToString("N2");

                            decimal total12 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Gift Cheque AMT"));
                            gvSalesSummary.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[12].Text = total12.ToString("N2");

                            decimal total13 = dT.AsEnumerable().Sum(row => row.Field<decimal>("BankFundTransfer AMT"));
                            gvSalesSummary.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[13].Text = total13.ToString("N2");

                            decimal total14 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DigitalPayment AMT"));
                            gvSalesSummary.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[14].Text = total14.ToString("N2");
                        }
                        else
                        {
                            lblMsgWarning.Text = "No record found";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                    
                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            loadSalesSummary();
        }



        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvSalesSummary.Rows.Count == 0)
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
                string localPath = Server.MapPath("~/exlTMP/rptSalesSummaryDateRange.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesSummaryDateRange.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesSummary.xlsx");

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


                    for (int i = 0; i < gvSalesSummary.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvSalesSummary.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = Server.HtmlDecode(gvSalesSummary.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblProductGrossAmt")).Text;
                        worksheet.Cell(i + 5, 4).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblProductDiscountAmt")).Text;
                        worksheet.Cell(i + 5, 5).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblServiceGrossAmt")).Text;
                        worksheet.Cell(i + 5, 6).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblServiceDiscountAmt")).Text;
                        worksheet.Cell(i + 5, 7).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblNetSales")).Text;
                        worksheet.Cell(i + 5, 8).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblCASHAMT")).Text;
                        worksheet.Cell(i + 5, 9).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblCreditCardAMT")).Text;
                        worksheet.Cell(i + 5, 10).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblDebitCardAMT")).Text;
                        worksheet.Cell(i + 5, 11).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblPaymayaAMT")).Text;
                        worksheet.Cell(i + 5, 12).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblVoucherAMT")).Text;
                        worksheet.Cell(i + 5, 13).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblGiftChequeAMT")).Text;
                        worksheet.Cell(i + 5, 14).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblBankFundTransferAMT")).Text;
                        worksheet.Cell(i + 5, 15).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblDigitalPaymentAMT")).Text;

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

    }
}