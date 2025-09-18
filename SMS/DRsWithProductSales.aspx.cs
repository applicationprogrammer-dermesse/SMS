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


namespace SMS
{
    public partial class DRsWithProductSales : System.Web.UI.Page
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
                        LoadDoctors();
                    }
                    else
                    {
                        txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = DateClass.getLday(EndDate);

                        LoadDoctors();
                        

                    }


                }


            }
        }


        private void LoadDoctors()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.loadDoctorsOnly";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddDrs.Items.Clear();
                    ddDrs.DataSource = dR;
                    ddDrs.DataValueField = "ID";
                    ddDrs.DataTextField = "EmployeeName";
                    ddDrs.DataBind();
                    ddDrs.Items.Insert(0, new ListItem("Please select", "0"));
                    ddDrs.SelectedIndex = 0;
                }
            }

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.SalesDRsWithProducts";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@PerformedBy", ddDrs.SelectedItem.Text);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvStaff.DataSource = dT;
                    gvStaff.DataBind();

                    if (gvStaff.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }

            }
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            //try
            //{
                string localPath = Server.MapPath("~/exlTMP/rptDrsWithProduct.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptDrsWithProduct.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/DrsWithProduct.xlsx");

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


                    worksheet.Cell("A1").Value = ddDrs.SelectedItem.Text;
                    worksheet.Cell("A2").Value = "Transaction Date :  " + txtDateFrom.Text + " - " + txtDateTo.Text;


                    for (int i = 0; i < gvStaff.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 4, 6).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 4, 7).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 4, 8).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[7].Text);
                        worksheet.Cell(i + 4, 9).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[8].Text);
                        worksheet.Cell(i + 4, 10).Value = "'" + Server.HtmlDecode(gvStaff.Rows[i].Cells[9].Text);
                        worksheet.Cell(i + 4, 11).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[10].Text);
                        worksheet.Cell(i + 4, 12).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[11].Text);
                        worksheet.Cell(i + 4, 13).Value = ((Label)gvStaff.Rows[i].FindControl("lblPrice")).Text;
                        worksheet.Cell(i + 4, 14).Value = ((Label)gvStaff.Rows[i].FindControl("lblQty")).Text;
                        worksheet.Cell(i + 4, 15).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[14].Text);
                        worksheet.Cell(i + 4, 16).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[15].Text);
                        worksheet.Cell(i + 4, 17).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[16].Text);
                        worksheet.Cell(i + 4, 18).Value = ((Label)gvStaff.Rows[i].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(i + 4, 19).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[18].Text);
                        worksheet.Cell(i + 4, 20).Value = ((Label)gvStaff.Rows[i].FindControl("lblNetAmt")).Text;





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
            //}

            //catch (Exception x)
            //{
            //    lblMsgWarning.Text = x.Message;
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //    return;
            //}
        }

        protected void ddDrs_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStaff.DataSource = null;
            gvStaff.DataBind();
        }
    }
}