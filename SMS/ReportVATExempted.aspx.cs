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
    public partial class ReportVATExempted : System.Web.UI.Page
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
                        txtDateFrom.Text = DateClass.getSday(startDate);
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
                    ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
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
        private void loadSalesSummary()
        {

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.VATExemptSales";
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

                        
                        gvVATExemptSales.DataSource = dT;
                        gvVATExemptSales.DataBind();

                        if (gvVATExemptSales.Rows.Count > 0)
                        {
                            
                            gvVATExemptSales.FooterRow.Cells[8].Text = "Total";
                            gvVATExemptSales.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;

                            decimal total9 = dT.AsEnumerable().Sum(row => row.Field<decimal>("DiscountsAmt"));
                            gvVATExemptSales.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                            gvVATExemptSales.FooterRow.Cells[9].Text = total9.ToString("N2");

                            decimal total10 = dT.AsEnumerable().Sum(row => row.Field<decimal>("NetAmount"));
                            gvVATExemptSales.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                            gvVATExemptSales.FooterRow.Cells[10].Text = total10.ToString("N2");

                            
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
            if (gvVATExemptSales.Rows.Count == 0)
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
                string localPath = Server.MapPath("~/exlTMP/rptVatExemptSales.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptVatExemptSales.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/VatExemptSales.xlsx");

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


                    worksheet.Cell("A2").Value = ddBranch.SelectedItem.Text;
                    worksheet.Cell("A3").Value = "Transaction Date :  " + txtDateFrom.Text + " - " + txtDateTo.Text;


                    for (int i = 0; i < gvVATExemptSales.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 5, 5).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 5, 6).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 5, 7).Value = ((Label)gvVATExemptSales.Rows[i].FindControl("lblvUnitCost")).Text;
                        worksheet.Cell(i + 5, 8).Value = ((Label)gvVATExemptSales.Rows[i].FindControl("lblvQty")).Text;
                        worksheet.Cell(i + 5, 9).Value = Server.HtmlDecode(gvVATExemptSales.Rows[i].Cells[8].Text);
                        worksheet.Cell(i + 5, 10).Value = ((Label)gvVATExemptSales.Rows[i].FindControl("lblvDiscPerc")).Text;
                        worksheet.Cell(i + 5, 11).Value = ((Label)gvVATExemptSales.Rows[i].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(i + 5, 12).Value = ((Label)gvVATExemptSales.Rows[i].FindControl("lblNetAmount")).Text;
 
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

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvVATExemptSales.DataSource = null;
            gvVATExemptSales.DataBind();
        }

    }
}