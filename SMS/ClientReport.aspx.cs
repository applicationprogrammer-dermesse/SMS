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
    public partial class ClientReport : System.Web.UI.Page
    {
        public string newFileName;
        public string filenameOfFile;


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

        private void LoadFGItem()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT vFGCode,vPluCode + ' - ' + vDESCRIPTION AS [vDESCRIPTION] FROM vItemMaster where vStat=1 ORDER BY vDESCRIPTION";
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
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
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
            if (ckPerItem.Checked == true)
            {
                if (ddITemFG.SelectedValue == "0")
                {
                    lblMsgWarning.Text = "Please select item";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    gvPerSku.DataSource = null;
                    gvPerSku.DataBind();

                    genSalesPerSKU();
                }
            }
            else
            {

                gvSalesPerItem.DataSource = null;
                gvSalesPerItem.DataBind();

                SalesAllSKU();
            }




        }

        private void genSalesPerSKU()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                stRtoGen = @"dbo.SalesPerItemAllBranchpErClienT";

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvSalesPerItem.DataSource = dT;
                    gvSalesPerItem.DataBind();


                    if (gvSalesPerItem.Rows.Count > 0)
                    {
                        gvSalesPerItem.FooterRow.Cells[3].Text = "Total";
                        gvSalesPerItem.FooterRow.Cells[3].HorizontalAlign = HorizontalAlign.Right;


                        Double total4 = dT.AsEnumerable().Sum(row => row.Field<Double>("Qty"));
                        gvSalesPerItem.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                        gvSalesPerItem.FooterRow.Cells[4].Text = total4.ToString("N0");

                        decimal total5 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossAmt"));
                        gvSalesPerItem.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesPerItem.FooterRow.Cells[5].Text = total5.ToString("N2");

                        decimal total6 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscAmt"));
                        gvSalesPerItem.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesPerItem.FooterRow.Cells[6].Text = total6.ToString("N2");

                        decimal total7 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetAmt"));
                        gvSalesPerItem.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                        gvSalesPerItem.FooterRow.Cells[7].Text = total7.ToString("N2");
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

        private void SalesAllSKU()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                stRtoGen = @"dbo.SalesPerSKUpErClienT";

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@ItemType", ddType.SelectedValue);
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvPerSku.DataSource = dT;
                    gvPerSku.DataBind();


                    if (gvPerSku.Rows.Count > 0)
                    {
                        gvPerSku.FooterRow.Cells[6].Text = "Total";
                        gvPerSku.FooterRow.Cells[6].HorizontalAlign = HorizontalAlign.Right;


                        decimal total6 = dT.AsEnumerable().Sum(row => row.Field<decimal>("GrossAmt"));
                        gvPerSku.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                        gvPerSku.FooterRow.Cells[7].Text = total6.ToString("N2");

                        decimal total7 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscAmt"));
                        gvPerSku.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                        gvPerSku.FooterRow.Cells[8].Text = total7.ToString("N2");

                        decimal total8 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetAmt"));
                        gvPerSku.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                        gvPerSku.FooterRow.Cells[9].Text = total8.ToString("N2");
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




                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Category")) == "0  - Unknown Category")
                {
                    e.Row.Cells[0].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[5].BackColor = System.Drawing.Color.Red;
                    e.Row.Cells[6].BackColor = System.Drawing.Color.Red;
                }
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
            if (ckPerItem.Checked == true)
            {
                EXportPerItems();
            }
            else
            {
                EXportAllItems();
            }
        }

        private void EXportAllItems()
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


                string localPath = Server.MapPath("~/exlTMP/rptSalesPerSKUperClient.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesPerSKUperClient.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesPerSKUperClient.xlsx");
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
                    worksheet.Cell("A3").Value = "Sales - Per SKU";


                    for (int r = 0; r < gvPerSku.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[0].Text);
                        worksheet.Cell(r + 5, 2).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[1].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 4).Value = "'" + Server.HtmlDecode(gvPerSku.Rows[r].Cells[3].Text);
                        worksheet.Cell(r + 5, 5).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[4].Text);
                        worksheet.Cell(r + 5, 6).Value = Server.HtmlDecode(gvPerSku.Rows[r].Cells[5].Text);
                        worksheet.Cell(r + 5, 7).Value = ((Label)gvPerSku.Rows[r].FindControl("lblQty")).Text;
                        worksheet.Cell(r + 5, 8).Value = ((Label)gvPerSku.Rows[r].FindControl("lblGrossAmt")).Text;
                        worksheet.Cell(r + 5, 9).Value = ((Label)gvPerSku.Rows[r].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(r + 5, 10).Value = ((Label)gvPerSku.Rows[r].FindControl("lblNetAmt")).Text;
                        worksheet.Cell(r + 5, 11).Value = "'" + Server.HtmlDecode(gvPerSku.Rows[r].Cells[10].Text);
                        worksheet.Cell(r + 5, 12).Value = "'" + Server.HtmlDecode(gvPerSku.Rows[r].Cells[11].Text);
                        worksheet.Cell(r + 5, 13).Value = ((Label)gvPerSku.Rows[r].FindControl("lbDateOfBirth")).Text;
                        worksheet.Cell(r + 5, 14).Value = "'" + Server.HtmlDecode(gvPerSku.Rows[r].Cells[13].Text);
                        worksheet.Cell(r + 5, 15).Value = "'" + Server.HtmlDecode(gvPerSku.Rows[r].Cells[14].Text);

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
                        worksheet.Cell(r + 5, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


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

        protected void ckPerItem_CheckedChanged(object sender, EventArgs e)
        {
            if (ckPerItem.Checked == true)
            {
                gvPerSku.DataSource = null;
                gvPerSku.DataBind();
                ReqV2.Enabled = false;
                LoadFGItem();
                ddITemFG.Enabled = true;
            }
            else
            {
                gvSalesPerItem.DataSource = null;
                gvSalesPerItem.DataBind();

                ReqV2.Enabled = true;
                ddITemFG.DataSource = null;
                ddITemFG.Items.Clear();
                ddITemFG.Enabled = false;
            }
        }


        private void EXportPerItems()
        {
            if (gvSalesPerItem.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {
                int colCnt = gvSalesPerItem.Rows[0].Cells.Count;


                string localPath = Server.MapPath("~/exlTMP/rptSalesPerITEMperClient.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesPerITEMperClient.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesPerITEMperClient.xlsx");
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


                    worksheet.Cell("A1").Value = "All Branches";
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;
                    worksheet.Cell("A3").Value = "Sales - Per Item";


                    for (int r = 0; r < gvSalesPerItem.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[0].Text);
                        worksheet.Cell(r + 5, 2).Value = "'" + Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[2].Text);
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[3].Text);
                        worksheet.Cell(r + 5, 5).Value = ((Label)gvSalesPerItem.Rows[r].FindControl("lblQty")).Text;
                        worksheet.Cell(r + 5, 6).Value = ((Label)gvSalesPerItem.Rows[r].FindControl("lblGrossAmt")).Text;
                        worksheet.Cell(r + 5, 7).Value = ((Label)gvSalesPerItem.Rows[r].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(r + 5, 8).Value = ((Label)gvSalesPerItem.Rows[r].FindControl("lblNetAmt")).Text;
                        worksheet.Cell(r + 5, 9).Value = "'" + Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[8].Text);
                        worksheet.Cell(r + 5, 10).Value = "'" + Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[9].Text);
                        worksheet.Cell(r + 5, 11).Value = ((Label)gvSalesPerItem.Rows[r].FindControl("lbDateOfBirth")).Text;
                        worksheet.Cell(r + 5, 12).Value = "'" + Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[11].Text);
                        worksheet.Cell(r + 5, 13).Value = "'" + Server.HtmlDecode(gvSalesPerItem.Rows[r].Cells[12].Text);

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