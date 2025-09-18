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
    public partial class rptConsultation : System.Web.UI.Page
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

                }
            }
        }


        public string stRtoGen;
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                stRtoGen = @"dbo.rptConsulationRecord";
            

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    if (Session["vUser_Branch"].ToString() != "1")
                    {
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    }
                    else 
                    {
                        cmD.Parameters.AddWithValue("@BrCode", 0);
                    }

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCunsultation.DataSource = dT;
                    gvCunsultation.DataBind();




                }
            }



        }


        protected void btnExcel_Click(object sender, EventArgs e)
        {

            if (gvCunsultation.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {



                string localPath = Server.MapPath("~/exlTMP/rptConsultation.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptConsultation.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/Consultation.xlsx");
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


                    worksheet.Cell("A1").Value = "Consultation";
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;



                    for (int r = 0; r < gvCunsultation.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[0].Text);
                        worksheet.Cell(r + 5, 2).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[3].Text.TrimEnd());
                        worksheet.Cell(r + 5, 5).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[4].Text.TrimEnd());
                        worksheet.Cell(r + 5, 6).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[5].Text.TrimEnd());
                        worksheet.Cell(r + 5, 7).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[6].Text.TrimEnd());
                        worksheet.Cell(r + 5, 8).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[7].Text.TrimEnd());
                        worksheet.Cell(r + 5, 9).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[8].Text.TrimEnd());
                        worksheet.Cell(r + 5, 10).Value = Server.HtmlDecode(gvCunsultation.Rows[r].Cells[9].Text.TrimEnd());


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