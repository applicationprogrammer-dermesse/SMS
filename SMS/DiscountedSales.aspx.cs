using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using ClosedXML.Excel;

namespace SMS
{
    public partial class DiscountedSales : System.Web.UI.Page
    {
        public string newFileName;
        public string filenameOfFile;


        // public string theMonthNum;


        //  public int CurrMonth;
        // public string CurrmonthName;

        public string startDate;

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
                        //loadYear();
                        //loadMonth();
                        loadBranch();
                    }
                    else
                    {

                        //loadYear();
                        //loadMonth();
                        loadPerBranch();
                        //loadSalesDetailed();

                    }

                    var today = DateTime.Now;
                    txtDateFrom.Text = DateClass.getSday(startDate);
                    txtDate.Text = today.ToShortDateString();

                }
            }
        }

        //private void loadYear()
        //{
        //    int CurrYear = System.DateTime.Now.Year;

        //    int lessYear = 3;
        //    ddYear.Items.Insert(0, "" + CurrYear + "");

        //    for (int y = 1; y <= lessYear; y++)
        //    {
        //        ddYear.Items.Add((CurrYear - y).ToString());
        //    }

        //    ddYear.Items.Add(CurrYear.ToString());

        //}

        //public void loadMonth()
        //{

        //    CurrMonth = DateTime.Now.Month;
        //    CultureInfo usEnglish = new CultureInfo("en-US");
        //    DateTimeFormatInfo englishInfo = usEnglish.DateTimeFormat;
        //    CurrmonthName = englishInfo.MonthNames[CurrMonth - 1];

        //    ddMonth.Items.Insert(0, "" + CurrmonthName + "");
        //    ddMonth.Items.Add("January");
        //    ddMonth.Items.Add("February");
        //    ddMonth.Items.Add("March");
        //    ddMonth.Items.Add("April");
        //    ddMonth.Items.Add("May");
        //    ddMonth.Items.Add("June");
        //    ddMonth.Items.Add("July");
        //    ddMonth.Items.Add("August");
        //    ddMonth.Items.Add("September");
        //    ddMonth.Items.Add("October");
        //    ddMonth.Items.Add("November");
        //    ddMonth.Items.Add("December");



        //}



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
                    ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }




        public string stRtoGen;
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                stRtoGen = @"dbo.DiscountedSales";

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvPerSku.DataSource = dT;
                    gvPerSku.DataBind();


                    if (gvPerSku.Rows.Count > 0)
                    {
                        //decimal total2 = dT.AsEnumerable().Sum(row => row.Field<decimal>("Product - Gross Amt"));
                        gvPerSku.FooterRow.Cells[9].Text = "Total";
                        gvPerSku.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;

                        //int total5 = dT.AsEnumerable().Sum(row => row.Field<int>("Qty"));
                        //string total5 = dT.AsEnumerable().Sum(x => x.Field<decimal>("Qty")).ToString();
                        //string total5 = dT.AsEnumerable().Sum(x => x.Field<decimal>("Qty")).ToString();

                        //gvPerSku.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                        //gvPerSku.FooterRow.Cells[5].Text = total5.ToString();

                        decimal total6 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossAmt"));
                        gvPerSku.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                        gvPerSku.FooterRow.Cells[10].Text = total6.ToString("N2");

                        decimal total8 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscAmt"));
                        gvPerSku.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                        gvPerSku.FooterRow.Cells[12].Text = total8.ToString("N2");

                        decimal total9 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetAmt"));
                        gvPerSku.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                        gvPerSku.FooterRow.Cells[13].Text = total9.ToString("N2");
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

        protected void gvPerSku_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string forcedCss = "alignCenter";


                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;


                e.Row.Cells[0].CssClass = forcedCss;
                e.Row.Cells[1].CssClass = forcedCss;
                e.Row.Cells[2].CssClass = forcedCss;
                e.Row.Cells[3].CssClass = forcedCss;
                e.Row.Cells[4].CssClass = forcedCss;
                e.Row.Cells[5].CssClass = forcedCss;
                e.Row.Cells[6].CssClass = forcedCss;

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[4].Text = Convert.ToDecimal(e.Row.Cells[4].Text).ToString("#,##0.00");
                e.Row.Cells[5].Text = Convert.ToDecimal(e.Row.Cells[5].Text).ToString("#,##0.00");
                e.Row.Cells[6].Text = Convert.ToDecimal(e.Row.Cells[6].Text).ToString("#,##0.00");




               
            }

        }

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPerSku.DataSource = null;
            gvPerSku.DataBind();
        }

        protected void ddMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPerSku.DataSource = null;
            gvPerSku.DataBind();
        }

        protected void ddYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPerSku.DataSource = null;
            gvPerSku.DataBind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvPerSku.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {
                int colCnt = gvPerSku.Rows[0].Cells.Count;


                string localPath = Server.MapPath("~/exlTMP/rptDiscountedSales.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptDiscountedSales.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/DiscountedSales.xlsx");
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
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;
                    worksheet.Cell("A3").Value = "Discounted Sales";


                    for (int r = 0; r < gvPerSku.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[0].Text);
                        worksheet.Cell(r + 5, 2).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[1].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = "'" + Server.HtmlDecode(gvPerSku.Rows[r].Cells[2].Text);
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[3].Text);
                        worksheet.Cell(r + 5, 5).Value = ((Label)gvPerSku.Rows[r].FindControl("lbDateOfBirth")).Text;
                        worksheet.Cell(r + 5, 6).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[5].Text);
                        worksheet.Cell(r + 5, 7).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[6].Text);
                        worksheet.Cell(r + 5, 8).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[7].Text);
                        worksheet.Cell(r + 5, 9).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[8].Text);
                        worksheet.Cell(r + 5, 10).Value = ((Label)gvPerSku.Rows[r].FindControl("lblQty")).Text;
                        worksheet.Cell(r + 5, 11).Value = ((Label)gvPerSku.Rows[r].FindControl("lblGrossAmt")).Text;
                        worksheet.Cell(r + 5, 12).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[11].Text);
                        worksheet.Cell(r + 5, 13).Value = ((Label)gvPerSku.Rows[r].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(r + 5, 14).Value = ((Label)gvPerSku.Rows[r].FindControl("lblNetAmt")).Text;

                        worksheet.Cell(r + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



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
}