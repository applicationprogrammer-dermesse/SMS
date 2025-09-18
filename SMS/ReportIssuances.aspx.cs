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
    public partial class ReportIssuances : System.Web.UI.Page
    {
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
                        txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = DateClass.getLday(EndDate);
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
                    ddBranch.Items.Insert(0, new ListItem("Consolidated", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvIssuances.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                gentemIssuances();
            }
        }


        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

        private void gentemIssuances()
        {
            if (gvIssuances.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {
                string localPath = Server.MapPath("~/exlTMP/rptItemIssuances.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptItemIssuances.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/ItemIssuances.xlsx");

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
                    worksheet.Cell("A3").Value = "Covered Date = " + txtDateFrom.Text + " to " + txtDateTo.Text;

                    for (int i = 0; i < gvIssuances.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvIssuances.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 5, 5).Value = "'" + Server.HtmlDecode(gvIssuances.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 5, 6).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 5, 7).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 5, 8).Value = ((Label)gvIssuances.Rows[i].FindControl("lblvQty")).Text;
                        worksheet.Cell(i + 5, 9).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[8].Text.Trim());
                        worksheet.Cell(i + 5, 10).Value = Server.HtmlDecode(gvIssuances.Rows[i].Cells[9].Text);

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
                gvIssuances.DataBind();
                ddITemFG.Items.Clear();
                ddITemFG.Enabled = false;
            }
        }

        protected void rbPerItem_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPerItem.Checked == true)
            {
                gvIssuances.DataBind();
                ddITemFG.Enabled = true;
                LoadFGItem();
            }
        }


        private void LoadFGItem()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT vFGCode,vPluCode + ' - ' + vDESCRIPTION AS [vDESCRIPTION] FROM vItemMaster where vStat=1 and ItemType='Product' and WithInventory=1 ORDER BY vDESCRIPTION";
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

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (rbAll.Checked == true)
            {

                loadItemReceivedAll();
            }
            else
            {
                loadItemReceivedPerItem();
            }
        }

        public string stRAllItem;
        public void loadItemReceivedAll()
        {
            if (ddBranch.SelectedValue=="0")
            {
                stRAllItem = @"SELECT b.BrName
                              ,a.[IssuanceNo]
                              ,a.[IssuanceDate]
                              ,a.[Remarks]
                              ,c.[vPluCode]
                              ,a.[vFGCode]
                              ,c.[vDESCRIPTION]
                              ,a.[vQty]
                              ,a.[PostedBy]
                              ,a.[PostedDate]
                          FROM [PostedIssuance] a
						  LEFT JOIN MyBranchList b
						  ON a.BrCode=b.BrCode
						  LEFT JOIN vItemMaster c
						  ON a.vFGCode=c.vFGCode
                            WHERE a.[IssuanceDate] between @dFrom and @dTo
                          ORDER BY a.ID";
            }
            else
            {
                stRAllItem = @"SELECT b.BrName
                              ,a.[IssuanceNo]
                              ,a.[IssuanceDate]
                              ,a.[Remarks]
                              ,c.[vPluCode]
                              ,a.[vFGCode]
                              ,c.[vDESCRIPTION]
                              ,a.[vQty]
                              ,a.[PostedBy]
                              ,a.[PostedDate]
                          FROM [PostedIssuance] a
						  LEFT JOIN MyBranchList b
						  ON a.BrCode=b.BrCode
						  LEFT JOIN vItemMaster c
						  ON a.vFGCode=c.vFGCode
                            WHERE a.BrCode=@BrCode and a.[IssuanceDate] between @dFrom and @dTo
                          ORDER BY a.ID";
            }
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                
                using (SqlCommand cmD = new SqlCommand(stRAllItem, conN))
                {
                    conN.Open();

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@dFrom", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@dTo", txtDateTo.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvIssuances.DataSource = dT;
                    gvIssuances.DataBind();

                    if (gvIssuances.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }
                }
            }
        }


        public string stRPerItem; 
        private void loadItemReceivedPerItem()
        {
            if (ddBranch.SelectedValue=="0")
            {
                stRPerItem = @"SELECT b.BrName
                              ,a.[IssuanceNo]
                              ,a.[IssuanceDate]
                              ,a.[Remarks]
                              ,c.[vPluCode]
                              ,a.[vFGCode]
                              ,c.[vDESCRIPTION]
                              ,a.[vQty]
                              ,a.[PostedBy]
                              ,a.[PostedDate]
                          FROM [PostedIssuance] a
						  LEFT JOIN MyBranchList b
						  ON a.BrCode=b.BrCode
						  LEFT JOIN vItemMaster c
						  ON a.vFGCode=c.vFGCode
                            WHERE a.[IssuanceDate] between @dFrom and @dTo and a.vFGCode=@vFGCode
                          ORDER BY a.ID";
            }
            else
            {
                stRPerItem = @"SELECT b.BrName
                              ,a.[IssuanceNo]
                              ,a.[IssuanceDate]
                              ,a.[Remarks]
                              ,c.[vPluCode]
                              ,a.[vFGCode]
                              ,c.[vDESCRIPTION]
                              ,a.[vQty]
                              ,a.[PostedBy]
                              ,a.[PostedDate]
                          FROM [PostedIssuance] a
						  LEFT JOIN MyBranchList b
						  ON a.BrCode=b.BrCode
						  LEFT JOIN vItemMaster c
						  ON a.vFGCode=c.vFGCode
                            WHERE a.BrCode=@BrCode and a.[IssuanceDate] between @dFrom and @dTo and a.vFGCode=@vFGCode
                          ORDER BY a.ID";
            }
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                
                using (SqlCommand cmD = new SqlCommand(stRPerItem, conN))
                {
                    conN.Open();

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@dFrom", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@dTo", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    
                    gvIssuances.DataSource = dT;
                    gvIssuances.DataBind();

                    if (gvIssuances.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }
            }
        }

    }
}