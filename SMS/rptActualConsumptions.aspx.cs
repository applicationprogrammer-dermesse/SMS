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
    public partial class rptActualConsumptions : System.Web.UI.Page
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
                        //loadBranch();
                    }
                    else
                    {

                        //txtDateFrom.Text = SysClass.getSday(startDate);
                        //txtDateTo.Text = SysClass.getLday(EndDate);
                        var yesterday = DateTime.Now.AddDays(-1);

                        txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = yesterday.ToShortDateString();
                        //loadPerBranch();
                       
                    }


                }
            }
        }

        //private void loadBranch()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"dbo.LoadBranches";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            cmD.CommandTimeout = 0;
        //            cmD.CommandType = CommandType.StoredProcedure;
        //            SqlDataReader dR = cmD.ExecuteReader();

        //            ddBranch.Items.Clear();
        //            ddBranch.DataSource = dR;
        //            ddBranch.DataValueField = "BrCode";
        //            ddBranch.DataTextField = "BrName";
        //            ddBranch.DataBind();
        //            ddBranch.Items.Insert(0, new ListItem("All branches", "0"));
        //            ddBranch.SelectedIndex = 0;
        //        }
        //    }
        //}

        //private void loadPerBranch()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode='" + Session["vUser_Branch"].ToString() + "'  ORDER BY BrCode";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            SqlDataReader dR = cmD.ExecuteReader();

        //            ddBranch.Items.Clear();
        //            ddBranch.DataSource = dR;
        //            ddBranch.DataValueField = "BrCode";
        //            ddBranch.DataTextField = "BrName";
        //            ddBranch.DataBind();
        //            //ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
        //            //ddBranch.SelectedIndex = 0;
        //        }
        //    }
        //}

        public int theOption;
        public string stRCon;
        private void loadAllBranches()
        {

            //if (ddBranch.SelectedValue == "0")
            //{
            //    stRCon = @"dbo.rtpActualConsumptions";
            //}
            //else
            //{
            //    stRCon = @"dbo.rtpActualConsumptions";
            //}

            stRCon = @"dbo.rtpActualConsumptions";
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                using (SqlCommand cmD = new SqlCommand(stRCon, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);
                    //cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvAllBranches.DataSource = dT;
                    gvAllBranches.DataBind();

                    if (gvAllBranches.Rows.Count > 0)
                    {
                        lblNote.Text = "Transaction Date :  " + txtDateFrom.Text + " - " + txtDateTo.Text;
                    }
                    else
                    {
                        lblNote.Text = string.Empty;
                    }

                }
            }
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            loadAllBranches();
        }



        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvAllBranches.Rows.Count == 0)
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
                string localPath = Server.MapPath("~/exlTMP/rptConsumption.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptConsumption.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/Consumption.xlsx");

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


                    worksheet.Cell("A1").Value = "Consumption";
                    worksheet.Cell("A2").Value = lblNote.Text;


                    for (int i = 0; i < gvAllBranches.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 4, 6).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 4, 7).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 4, 8).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[7].Text);
                        worksheet.Cell(i + 4, 9).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[8].Text);
                        worksheet.Cell(i + 4, 10).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[9].Text);
                        worksheet.Cell(i + 4, 11).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[10].Text);
                        worksheet.Cell(i + 4, 12).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[11].Text);
                        worksheet.Cell(i + 4, 13).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[12].Text);
                        worksheet.Cell(i + 4, 14).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[13].Text);
                        worksheet.Cell(i + 4, 15).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[14].Text);
                        worksheet.Cell(i + 4, 16).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[15].Text);
                        worksheet.Cell(i + 4, 17).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[16].Text);
                        worksheet.Cell(i + 4, 18).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[17].Text);
                        worksheet.Cell(i + 4, 19).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[18].Text);
                        worksheet.Cell(i + 4, 20).Value = Server.HtmlDecode(gvAllBranches.Rows[i].Cells[19].Text);

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

        protected void txtDateFrom_TextChanged(object sender, EventArgs e)
        {
            gvAllBranches.DataSource = null;
            gvAllBranches.DataBind();
        }

        protected void txtDateTo_TextChanged(object sender, EventArgs e)
        {
               gvAllBranches.DataSource = null;
               gvAllBranches.DataBind();
        
        }

    }
}