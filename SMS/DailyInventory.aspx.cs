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
                    ddBranch.Items.Insert(0, new ListItem("ALL BRANCHES", "0"));
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

                    int startRow = 4;
                    int branchColumnIndex = 0;

                    // Check if we have branch column (when All Branches is selected)
                    bool hasBranchColumn = gvSummary.Columns.Cast<DataControlField>()
                        .Any(c => c.HeaderText == "Branch");

                    if (hasBranchColumn)
                    {
                        // Adjust column positions for branch data
                        branchColumnIndex = 1;
                        startRow = 4;
                    }

                    for (int i = 0; i < gvSummary.Rows.Count; i++)
                    {
                        int colOffset = hasBranchColumn ? 1 : 0;

                        if (hasBranchColumn)
                        {
                            // Add branch name
                            worksheet.Cell(i + startRow, 1).Value = Server.HtmlDecode(gvSummary.Rows[i].Cells[0].Text);
                            worksheet.Cell(i + startRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }

                        worksheet.Cell(i + startRow, 1 + colOffset).Value = "'" + Server.HtmlDecode(
                            hasBranchColumn ? gvSummary.Rows[i].Cells[1].Text : gvSummary.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + startRow, 2 + colOffset).Value = "'" + Server.HtmlDecode(
                            hasBranchColumn ? gvSummary.Rows[i].Cells[2].Text : gvSummary.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + startRow, 3 + colOffset).Value = Server.HtmlDecode(
                            hasBranchColumn ? gvSummary.Rows[i].Cells[3].Text : gvSummary.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + startRow, 4 + colOffset).Value = Server.HtmlDecode(
                            hasBranchColumn ? gvSummary.Rows[i].Cells[4].Text : gvSummary.Rows[i].Cells[3].Text);

                        // Continue with other columns...
                        worksheet.Cell(i + startRow, 5 + colOffset).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyBegBalS")).Text;
                        worksheet.Cell(i + startRow, 6 + colOffset).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyReceivedS")).Text;
                        // ... add other columns

                        // Apply borders to all cells
                        for (int col = 1; col <= 12 + colOffset; col++)
                        {
                            worksheet.Cell(i + startRow, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                    }

                    // Rest of the export code remains the same...
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
                            cmD.Parameters.AddWithValue("@Option", 2); // All branches
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@Option", 1); // Single branch
                        }

                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@SalesDate", txtDate.Text);

                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        // Add branch name column to gridview if it doesn't exist
                        if (ddBranch.SelectedValue == "0" && !gvSummary.Columns.Cast<DataControlField>().Any(c => c.HeaderText == "Branch"))
                        {
                            BoundField branchField = new BoundField();
                            branchField.HeaderText = "Branch";
                            branchField.DataField = "BrName";
                            branchField.ItemStyle.Width = Unit.Pixel(150);
                            branchField.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            branchField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                            gvSummary.Columns.Add(branchField);
                        }
                        else if (ddBranch.SelectedValue != "0")
                        {
                            // Remove branch column if it exists and single branch is selected
                            var branchColumn = gvSummary.Columns.Cast<DataControlField>()
                                .FirstOrDefault(c => c.HeaderText == "Branch");
                            if (branchColumn != null)
                            {
                                gvSummary.Columns.Remove(branchColumn);
                            }
                        }

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