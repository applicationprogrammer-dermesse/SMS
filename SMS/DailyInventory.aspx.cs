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
    public partial class DailyInventory : System.Web.UI.Page
    {
        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

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
                        loadBranch();
                        var yesterday = DateTime.Now.AddDays(-1);
                        txtDate.Text = yesterday.ToShortDateString();

                    }
                    else
                    {

                        loadPerBranch();
                        var yesterday = DateTime.Now.AddDays(-1);
                        txtDate.Text = yesterday.ToShortDateString();
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
                    //ddbranch.items.insert(0, new listitem("all branches", "0"));
                    //ddbranch.selectedindex = 0;
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





        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvSummary.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                genSummary();
            }


        }



        private void genSummary()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptDailyInventory.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptDailyInventory.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/DailyInventory.xlsx");

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



                    worksheet.Cell("A2").Value = ddBranch.SelectedItem.Text + " - " + txtDate.Text;

                    for (int i = 0; i < gvSummary.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = "'" + Server.HtmlDecode(gvSummary.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = "'" + Server.HtmlDecode(gvSummary.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvSummary.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvSummary.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyBegBalS")).Text;
                        worksheet.Cell(i + 4, 6).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyReceivedS")).Text;
                        worksheet.Cell(i + 4, 7).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyPRF")).Text;
                        worksheet.Cell(i + 4, 8).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyFree")).Text;
                        worksheet.Cell(i + 4, 9).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtySales")).Text;
                        worksheet.Cell(i + 4, 10).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyAdjustmentS")).Text;
                        worksheet.Cell(i + 4, 11).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyComplimentaryS")).Text;
                        worksheet.Cell(i + 4, 12).Value = ((Label)gvSummary.Rows[i].FindControl("lblAvailableBalanceS")).Text;

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
            gvSummary.DataSource = null;
            gvSummary.DataBind();
        }


        protected void btnGen_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.rtpDailyInventory";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;

                        if (ddBranch.SelectedValue == "0")
                        {
                            cmD.Parameters.AddWithValue("@Option", 2);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@Option", 1);
                        }

                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@SalesDate", txtDate.Text);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSummary.DataSource = dT;
                        gvSummary.DataBind();

                        if (gvSummary.Rows.Count == 0)
                        {
                            lblMsgWarning.Text = "No record found!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;

                        }
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