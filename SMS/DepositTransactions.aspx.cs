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
    public partial class DepositTransactions : System.Web.UI.Page
    {

        public string newFileName;
        public string filenameOfFile;

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
                        loadDepositsAllBranch();
                    }
                    else
                    {

                        loadPerBranch();
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
                    ddBranch.Items.Insert(0, new ListItem("ALL BRANCHES", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }

        public string stRtoGen;
        public int theType;
        private void loadDepositsAllBranch()
        {
            if (ddBranch.SelectedValue == "0" & rbAll.Checked==true)
            {
                theType=1;
            }
            else if (ddBranch.SelectedValue != "0" & rbAll.Checked == true)
            {
                theType=2;
            }

            else if (ddBranch.SelectedValue == "0" & rbPaid.Checked == true)
            {
                theType = 3;
            }
            else if (ddBranch.SelectedValue != "0" & rbPaid.Checked == true)
            {
                theType = 4;
            }

            else if (ddBranch.SelectedValue == "0" & rbUnpaid.Checked == true)
            {
                theType = 5;
            }
            else if (ddBranch.SelectedValue != "0" & rbUnpaid.Checked == true)
            {
                theType = 6;
            }


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                stRtoGen = @"dbo.DepositsReport";

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@theType", theType);
                    
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvDeposits.DataSource = dT;
                    gvDeposits.DataBind();


                    if (gvDeposits.Rows.Count  == 0)
                    {
                        lblMsgWarning.Text = "No record found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    

                }
            }
        }


        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

            gvDeposits.DataSource = null;
            gvDeposits.DataBind();
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            loadDepositsAllBranch();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (gvDeposits.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {
                int colCnt = gvDeposits.Rows[0].Cells.Count;


                string localPath = Server.MapPath("~/exlTMP/rptDepositTransaction.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptDepositTransaction.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/DepositTransaction.xlsx");
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
                    if (rbAll.Checked==true)
                    {
                        worksheet.Cell("A2").Value = "Status : " + "All";
                    }
                    else if (rbPaid.Checked == true)
                    {
                        worksheet.Cell("A2").Value = "Status : " + "Paid";
                    }
                    else
                    {
                        worksheet.Cell("A2").Value = "Status : " + "Unpaid";
                    }
                    


                    for (int r = 0; r < gvDeposits.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 4, 1).Value = Server.HtmlDecode(gvDeposits.Rows[r].Cells[0].Text);
                        worksheet.Cell(r + 4, 2).Value = Server.HtmlDecode(gvDeposits.Rows[r].Cells[1].Text.TrimEnd());
                        worksheet.Cell(r + 4, 3).Value = "'" + Server.HtmlDecode(gvDeposits.Rows[r].Cells[2].Text);
                        worksheet.Cell(r + 4, 4).Value = Server.HtmlDecode(gvDeposits.Rows[r].Cells[3].Text);
                        worksheet.Cell(r + 4, 5).Value = Server.HtmlDecode(gvDeposits.Rows[r].Cells[4].Text);
                        worksheet.Cell(r + 4, 6).Value = "'" + Server.HtmlDecode(gvDeposits.Rows[r].Cells[5].Text);
                        worksheet.Cell(r + 4, 7).Value = Server.HtmlDecode(gvDeposits.Rows[r].Cells[6].Text);
                        worksheet.Cell(r + 4, 8).Value = ((Label)gvDeposits.Rows[r].FindControl("lblTotalSession")).Text;
                        worksheet.Cell(r + 4, 9).Value = ((Label)gvDeposits.Rows[r].FindControl("lblNoSession")).Text;
                        worksheet.Cell(r + 4, 10).Value = ((Label)gvDeposits.Rows[r].FindControl("lblItemAmount")).Text;
                        worksheet.Cell(r + 4, 11).Value = ((Label)gvDeposits.Rows[r].FindControl("lblDepAmountt")).Text;
                        worksheet.Cell(r + 4, 12).Value = ((Label)gvDeposits.Rows[r].FindControl("lblDueAmount")).Text;
                        worksheet.Cell(r + 4, 13).Value = ((Label)gvDeposits.Rows[r].FindControl("lblDatePaid")).Text;
                        worksheet.Cell(r + 4, 14).Value = ((Label)gvDeposits.Rows[r].FindControl("lblPaidAmount")).Text;
                        worksheet.Cell(r + 4, 15).Value = Server.HtmlDecode(gvDeposits.Rows[r].Cells[14].Text);

                        worksheet.Cell(r + 4, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 4, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


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

        protected void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAll.Checked == true)
            {
                gvDeposits.DataSource = null;
                gvDeposits.DataBind();
            }
        }

        protected void rbPaid_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPaid.Checked == true)
            {
                gvDeposits.DataSource = null;
                gvDeposits.DataBind();
            }
        }

        protected void rbUnpaid_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUnpaid.Checked == true)
            {
                gvDeposits.DataSource = null;
                gvDeposits.DataBind();
            }
        }





    }
}