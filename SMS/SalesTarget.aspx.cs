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
    public partial class SalesTarget : System.Web.UI.Page
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

                       var yesterday = DateTime.Now.AddDays(-1);
                        //txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = yesterday.ToString("MM/dd/yyyy"); //DateClass.getLday(EndDate);

                        DateTime date = Convert.ToDateTime(txtDateTo.Text);
                        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                        //txtDateFrom.Text = firstDayOfMonth.ToShortDateString();
                        txtDateFrom.Text = firstDayOfMonth.ToString("MM/dd/yyyy");
                        
                       if (Session["vUser_Branch"].ToString() == "1")
                        {
                            loadSalesTarget();
                            loadGrossSales();
                            loadGrossSalesLessVAT();
                            loadGrossSalesDiscount();
                            loadNetOfVAT();
                            loadVAT();

                        }
                        else
                        {
                            loadSalesTargetPerBranch();
                        }

                     
                }
            }
        }

        

        public int theOption;
        private void loadSalesTargetPerBranch()
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.SalesTargetPerBranch";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvSalesSummary.DataSource = dT;
                    gvSalesSummary.DataBind();

                    if (gvSalesSummary.Rows.Count > 0)
                    {

                        gvSalesSummary.FooterRow.Cells[1].Text = "Total";
                        gvSalesSummary.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                        decimal total2 = dT.AsEnumerable().Sum(row => row.Field<decimal>("iTarget"));
                        gvSalesSummary.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[2].Text = total2.ToString("N2");

                        decimal total3 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Daily Target"));
                        gvSalesSummary.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[3].Text = total3.ToString("N2");

                        decimal total4 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossSales4TheDay"));
                        gvSalesSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[4].Text = total4.ToString("N2");

                        decimal total5 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscSales4TheDay"));
                        gvSalesSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[5].Text = total5.ToString("N2");

                        decimal total6 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossSalesProduct"));
                        gvSalesSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[6].Text = total6.ToString("N2");

                        decimal total7 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscSalesProduct"));
                        gvSalesSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[7].Text = total7.ToString("N2");

                        decimal total8 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetSalesProduct"));
                        gvSalesSummary.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[8].Text = total8.ToString("N2");


                        decimal total9 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossSalesService"));
                        gvSalesSummary.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[9].Text = total9.ToString("N2");

                        decimal total10 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscSalesService"));
                        gvSalesSummary.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[10].Text = total10.ToString("N2");

                        decimal total11 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetSalesService"));
                        gvSalesSummary.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[11].Text = total11.ToString("N2");


                        decimal total12 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GiftCheck"));
                        gvSalesSummary.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[12].Text = total12.ToString("N2");

                        decimal total13 = dT.AsEnumerable().Sum(row => row.Field<decimal>("TOTALNETSALES"));
                        gvSalesSummary.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[13].Text = total13.ToString("N2");

                        decimal total14 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Vat"));
                        gvSalesSummary.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[14].Text = total14.ToString("N2");

                        decimal total15 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetOfVat"));
                        gvSalesSummary.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[15].Text = total15.ToString("N2");


                        decimal total16 = dT.AsEnumerable().Sum(row => row.Field<decimal>("TOTALGROSSSALES"));
                        gvSalesSummary.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[16].Text = total16.ToString("N2");

                        decimal total18 = dT.AsEnumerable().Sum(row => row.Field<decimal>("TargetToDateSalesAsOf"));
                        gvSalesSummary.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesSummary.FooterRow.Cells[18].Text = total18.ToString("N2");
                    }

                }
            }

        }


        //private void loadSummaryGrossSales()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"dbo.SalesTargetSummary";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            cmD.CommandTimeout = 0;
        //            cmD.CommandType = CommandType.StoredProcedure;
        //            cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
        //            cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

        //            DataTable dT = new DataTable();
        //            SqlDataAdapter dA = new SqlDataAdapter(cmD);
        //            dA.Fill(dT);

        //            gvSummaryTotal.DataSource = dT;
        //            gvSummaryTotal.DataBind();

                    
                    
             
        //        }
        //    }
        //}
        private void loadSalesTarget()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SalesTarget";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSalesSummary.DataSource = dT;
                        gvSalesSummary.DataBind();

                        if (gvSalesSummary.Rows.Count > 0)
                        {

                            gvSalesSummary.FooterRow.Cells[1].Text = "Total";
                            gvSalesSummary.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;

                            decimal total2 = dT.AsEnumerable().Sum(row => row.Field<decimal>("iTarget"));
                            gvSalesSummary.FooterRow.Cells[2].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[2].Text = total2.ToString("N2");

                            decimal total3 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Daily Target"));
                            gvSalesSummary.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[3].Text = total3.ToString("N2");

                            decimal total4 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossSales4TheDay"));
                            gvSalesSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[4].Text = total4.ToString("N2");

                            decimal total5 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscSales4TheDay"));
                            gvSalesSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[5].Text = total5.ToString("N2");

                            decimal total6 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossSalesProduct"));
                            gvSalesSummary.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[6].Text = total6.ToString("N2");

                            decimal total7 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscSalesProduct"));
                            gvSalesSummary.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[7].Text = total7.ToString("N2");

                            decimal total8 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetSalesProduct"));
                            gvSalesSummary.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[8].Text = total8.ToString("N2");


                            decimal total9 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossSalesService"));
                            gvSalesSummary.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[9].Text = total9.ToString("N2");

                            decimal total10 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscSalesService"));
                            gvSalesSummary.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[10].Text = total10.ToString("N2");

                            decimal total11 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetSalesService"));
                            gvSalesSummary.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[11].Text = total11.ToString("N2");


                            decimal total12 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GiftCheck"));
                            gvSalesSummary.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[12].Text = total12.ToString("N2");

                            decimal total13 = dT.AsEnumerable().Sum(row => row.Field<decimal>("TOTALNETSALES"));
                            gvSalesSummary.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[13].Text = total13.ToString("N2");

                            decimal total14 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Vat"));
                            gvSalesSummary.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[14].Text = total14.ToString("N2");

                            decimal total15 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetOfVat"));
                            gvSalesSummary.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[15].Text = total15.ToString("N2");


                            decimal total16 = dT.AsEnumerable().Sum(row => row.Field<decimal>("TOTALGROSSSALES"));
                            gvSalesSummary.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[16].Text = total16.ToString("N2");

                            decimal total18 = dT.AsEnumerable().Sum(row => row.Field<decimal>("TargetToDateSalesAsOf"));
                            gvSalesSummary.FooterRow.Cells[18].HorizontalAlign = HorizontalAlign.Right;
                            gvSalesSummary.FooterRow.Cells[18].Text = total18.ToString("N2");
                        }
                        //else
                        //{
                        //    lblMsgWarning.Text = "No record found";
                        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        //    return;
                        //}
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

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            string ValidDate = "01/31/2021";
            DateTime startDate = Convert.ToDateTime(txtDateFrom.Text);
            DateTime endDate = Convert.ToDateTime(txtDateTo.Text);

            if (Convert.ToDateTime(txtDateFrom.Text) > DateTime.Now)
            {
                lblMsgWarning.Text = "Date range should less than or equal to " + DateTime.Now.ToShortDateString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else if (startDate.ToString("MMM") != endDate.ToString("MMM"))
            {
                lblMsgWarning.Text = "Different month is not allowed!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else if (startDate.Year != endDate.Year)
            {
                lblMsgWarning.Text = "Different year is not allowed!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else if (startDate <= DateTime.Parse(ValidDate))
            {
                lblMsgWarning.Text = "Transaction date is invalid!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                if (Session["vUser_Branch"].ToString() == "1")
                {
                    loadSalesTarget();

                    loadGrossSales();
                    loadGrossSalesLessVAT();
                    loadGrossSalesDiscount();
                    loadNetOfVAT();
                    loadVAT();
                }
                else
                {
                    loadSalesTargetPerBranch();
                }
                //loadSalesTarget();
            }
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
                ExportSalesTarget();
            }
        }

        decimal RunningTargetSalestoDate = 0;
        //decimal RunningTotalTotalSales = 0;
        decimal RunningTotalTotalTarget = 0;
        decimal RunningTotalTotalNetSales = 0;
        protected void gvSalesSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RunningTargetSalestoDate += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TargetToDateSalesAsOf"));
                //RunningTotalTotalSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TOTALGROSSSALES"));
                RunningTotalTotalNetSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TOTALNETSALES"));
                
                RunningTotalTotalTarget += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "iTarget"));
                if (Session["vUser_Branch"].ToString() == "1")
                {
                    if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "AREA")) == "CENTRAL AREA")
                    {

                        e.Row.BackColor = System.Drawing.Color.PaleGoldenrod;
                    }
                    else if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "AREA")) == "NORTH AREA")
                    {

                        e.Row.BackColor = System.Drawing.Color.LightGray;
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    }
                }
                string theDT = ((Label)e.Row.FindControl("lblSalesAsOf")).Text;
                if (theDT != txtDateTo.Text)
                {
                    e.Row.Cells[17].BackColor = System.Drawing.Color.Red;

                }

             
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (RunningTargetSalestoDate > 0)
                {
                    decimal PctToDate = ((RunningTotalTotalNetSales / RunningTargetSalestoDate) * 100);
                    ((Label)e.Row.FindControl("lblPercToDate")).Text = PctToDate.ToString("N") + " %";
                }

                if (RunningTotalTotalTarget > 0)
                {
                    decimal PctToMonth = ((RunningTotalTotalNetSales / RunningTotalTotalTarget) * 100);
                    ((Label)e.Row.FindControl("lblPercMonthF")).Text = PctToMonth.ToString("N") + " %";
                }
            }
        }




        private void ExportSalesTarget()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptSalesTarget.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesTarget.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesTarget.xlsx");

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


                    worksheet.Cell("A1").Value = "Sales Summary with Target";
                    worksheet.Cell("A2").Value = "Transaction Date :  " + txtDateFrom.Text + " - " + txtDateTo.Text;


                    for (int i = 0; i < gvSalesSummary.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvSalesSummary.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = Server.HtmlDecode(gvSalesSummary.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lbliTarget")).Text;
                        worksheet.Cell(i + 4, 4).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblDailyTarget")).Text;
                        worksheet.Cell(i + 4, 5).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblGrossSales4TheDay")).Text;
                        worksheet.Cell(i + 4, 6).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblDiscSales4TheDay")).Text;
                        worksheet.Cell(i + 4, 7).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblGrossSalesProduct")).Text;
                        worksheet.Cell(i + 4, 8).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblDiscSalesProduct")).Text;
                        worksheet.Cell(i + 4, 9).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblNetSalesProduct")).Text;

                        worksheet.Cell(i + 4, 10).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblGrossSalesService")).Text;
                        worksheet.Cell(i + 4, 11).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblDiscSalesService")).Text;
                        worksheet.Cell(i + 4, 12).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblNetSalesService")).Text;

                        
                        worksheet.Cell(i + 4, 13).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblGiftCheck")).Text;
                        worksheet.Cell(i + 4, 14).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblTOTALNETSALES")).Text;
                        worksheet.Cell(i + 4, 15).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblVat")).Text;
                        worksheet.Cell(i + 4, 16).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblNetOfVat")).Text;

                        worksheet.Cell(i + 4, 17).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblTOTALGROSSSALES")).Text;
                        worksheet.Cell(i + 4, 18).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblSalesAsOf")).Text;
                        worksheet.Cell(i + 4, 19).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblTargetToDateSalesAsOf")).Text;
                        worksheet.Cell(i + 4, 20).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblPerc")).Text;
                        worksheet.Cell(i + 4, 21).Value = ((Label)gvSalesSummary.Rows[i].FindControl("lblPercMonth")).Text;
                        
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
                        worksheet.Cell(i + 4, 21).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    }

                    
                    int maxRow = gvSalesSummary.Rows.Count;
                    worksheet.Cell(maxRow + 4, 2).Value = "TOTAL";
                    worksheet.Cell(maxRow + 4, 2).Style.Font.Bold = true;

                    int sRow = 4;
                    int lRow = (maxRow + sRow) - 1;

                    string FormulaC = "=sum(C" + sRow + ":" + "C" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 3).FormulaA1 = FormulaC;
                    worksheet.Cell(maxRow + 4, 3).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 3).Style.Font.Bold = true;

                    string FormulaD = "=sum(D" + sRow + ":" + "D" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 4).FormulaA1 = FormulaD;
                    worksheet.Cell(maxRow + 4, 4).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 4).Style.Font.Bold = true;

                    string FormulaE = "=sum(E" + sRow + ":" + "E" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 5).FormulaA1 = FormulaE;
                    worksheet.Cell(maxRow + 4, 5).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 5).Style.Font.Bold = true;

                    string FormulaF = "=sum(F" + sRow + ":" + "F" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 6).FormulaA1 = FormulaF;
                    worksheet.Cell(maxRow + 4, 6).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 6).Style.Font.Bold = true;

                    string FormulaG = "=sum(G" + sRow + ":" + "G" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 7).FormulaA1 = FormulaG;
                    worksheet.Cell(maxRow + 4, 7).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 7).Style.Font.Bold = true;

                    string FormulaH = "=sum(H" + sRow + ":" + "H" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 8).FormulaA1 = FormulaH;
                    worksheet.Cell(maxRow + 4, 8).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 8).Style.Font.Bold = true;

                    string FormulaI = "=sum(I" + sRow + ":" + "I" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 9).FormulaA1 = FormulaI;
                    worksheet.Cell(maxRow + 4, 9).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 9).Style.Font.Bold = true;

                    string FormulaJ = "=sum(J" + sRow + ":" + "J" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 10).FormulaA1 = FormulaJ;
                    worksheet.Cell(maxRow + 4, 10).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 10).Style.Font.Bold = true;

                    string FormulaK = "=sum(K" + sRow + ":" + "K" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 11).FormulaA1 = FormulaK;
                    worksheet.Cell(maxRow + 4, 11).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 11).Style.Font.Bold = true;

                    string FormulaL = "=sum(L" + sRow + ":" + "L" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 12).FormulaA1 = FormulaL;
                    worksheet.Cell(maxRow + 4, 12).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 12).Style.Font.Bold = true;

                    string FormulaM = "=sum(M" + sRow + ":" + "M" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 13).FormulaA1 = FormulaM;
                    worksheet.Cell(maxRow + 4, 13).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 13).Style.Font.Bold = true;

                    string FormulaN = "=sum(N" + sRow + ":" + "N" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 14).FormulaA1 = FormulaN;
                    worksheet.Cell(maxRow + 4, 14).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 14).Style.Font.Bold = true;

                    string FormulaO = "=sum(O" + sRow + ":" + "O" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 15).FormulaA1 = FormulaO;
                    worksheet.Cell(maxRow + 4, 15).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 15).Style.Font.Bold = true;

                    string FormulaP = "=sum(P" + sRow + ":" + "P" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 16).FormulaA1 = FormulaP;
                    worksheet.Cell(maxRow + 4, 16).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 16).Style.Font.Bold = true;

                    string FormulaQ = "=sum(Q" + sRow + ":" + "Q" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 17).FormulaA1 = FormulaQ;
                    worksheet.Cell(maxRow + 4, 17).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 17).Style.Font.Bold = true;

                    string FormulaS = "=sum(S" + sRow + ":" + "S" + lRow + ")";
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 19).FormulaA1 = FormulaS;
                    worksheet.Cell(maxRow + 4, 19).Style.Font.Bold = true;
                    worksheet.Cell(gvSalesSummary.Rows.Count + 4, 19).Style.Font.Bold = true;
                    
                    //decimal theTotal = Convert.ToDecimal(FormulaM) * Convert.ToDecimal(FormulaO);

                    //string FormulaP = "=sum(" + FormulaM + "*" + FormulaO + ")"; 
                    //worksheet.Cell(gvSalesSummary.Rows.Count + 4, 16).FormulaA1 = FormulaP;
                    //worksheet.Cell(maxRow + 4, 16).Style.Font.Bold = true;
                    //worksheet.Cell(gvSalesSummary.Rows.Count + 4, 16).Style.Font.Bold = true;

                    for (int i = 0; i < gvGrossSales.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 26, 3).Value = ((Label)gvGrossSales.Rows[i].FindControl("lblsAmt")).Text;
                    }

                    for (int i = 0; i < gvGrossSalesLessVAT.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 31, 3).Value = ((Label)gvGrossSalesLessVAT.Rows[i].FindControl("lbls2Amt")).Text;
                    }

                    for (int i = 0; i < gvDiscounts.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 36, 3).Value = ((Label)gvDiscounts.Rows[i].FindControl("lbls3Amt")).Text;
                    }

                    for (int i = 0; i < gvNetOfVAT.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 48, 3).Value = ((Label)gvNetOfVAT.Rows[i].FindControl("lbls4Amt")).Text;
                    }

                    for (int i = 0; i < gvVAT.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 54, 3).Value = ((Label)gvVAT.Rows[i].FindControl("lbls5Amt")).Text;
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





        private void loadGrossSales()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SummaryGrossSales";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvGrossSales.DataSource = dT;
                        gvGrossSales.DataBind();

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



        private void loadGrossSalesLessVAT()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SummaryGrossSalesLessVAT";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvGrossSalesLessVAT.DataSource = dT;
                        gvGrossSalesLessVAT.DataBind();

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


        private void loadGrossSalesDiscount()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SummaryGrossSalesDISCOUNT";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvDiscounts.DataSource = dT;
                        gvDiscounts.DataBind();

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



        private void loadNetOfVAT()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SummaryNetOfVAT";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvNetOfVAT.DataSource = dT;
                        gvNetOfVAT.DataBind();

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


        private void loadVAT()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SummaryVAT";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvVAT.DataSource = dT;
                        gvVAT.DataBind();

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