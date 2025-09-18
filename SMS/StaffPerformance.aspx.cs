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
    public partial class StaffPerformance : System.Web.UI.Page
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
                        //var yesterday = DateTime.Now.AddDays(-1);
                        //txtDateFrom.Text = yesterday.ToShortDateString();
                        txtDateTo.Text = DateClass.getLday(EndDate);
                        loadBranch();
                        LoadDoctors();
                    }
                    else
                    {
                        txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = DateClass.getLday(EndDate);
                        loadPerBranch();
                        LoadDoctors();
                       // loadStaffPerformance();

                    }


                }


            }
        }


        private void LoadDoctors()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT ID,EmployeeName FROM DoctosList ORDER BY EmployeeName";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddDrs.Items.Clear();
                    ddDrs.DataSource = dR;
                    ddDrs.DataValueField = "ID";
                    ddDrs.DataTextField = "EmployeeName";
                    ddDrs.DataBind();
                    ddDrs.Items.Insert(0, new ListItem("All staff", "0"));
                    ddDrs.Items.Insert(1, new ListItem("All Doctors", "1"));
                    ddDrs.Items.Insert(2, new ListItem("All Derma Nurse", "2"));
                    ddDrs.SelectedIndex = 0;
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
                    if (ckAll.Checked == false)
                    {
                        ddBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
                    }
                    else
                    {
                        ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    }
                    ddBranch.SelectedIndex = 0;
                }
            }
        }


        public int theOption;
        public int theOptionStaff;
        private void loadStaffPerformance()
        {
            
                if (ddDrs.SelectedValue == "0")
                {
                    //RequiredBranch.Enabled = true;
                    theOptionStaff = 1;
                }
                else if (ddDrs.SelectedValue != "0" & ckAll.Checked==true)
                {
                    //RequiredBranch.Enabled = false;
                    theOptionStaff = 2;
                }
                else if (ddDrs.SelectedValue != "0" & ckAll.Checked == false)
                {
                    //RequiredBranch.Enabled = true;
                    theOptionStaff = 3;
                }

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.StaffPerformance";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@theOptionStaff", theOptionStaff);
                        if (ddDrs.SelectedValue == "0")
                        {
                            cmD.Parameters.AddWithValue("@PerformedBy", string.Empty);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@PerformedBy", ddDrs.SelectedItem.Text);
                        }
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

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ddBranch.SelectedValue == "0" & ckAll.Checked==false)
            {
                lblMsgWarning.Text = "Please select branch";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else if (ckAll.Checked == true & ddDrs.SelectedValue == "0")
            {
                loadStaffPerformanceAll();

            }
            else if (ckAll.Checked == true & ddDrs.SelectedValue == "1")
            {
                if (ckConsolidated.Checked == true)
                {
                    loadStaffPerformanceDoctorsOnlySummary();
                }
                else
                {
                    loadStaffPerformanceDoctorsOnly();
                }

            }

            else if (ckAll.Checked == true & ddDrs.SelectedValue == "2")
            {
                if (ckConsolidated.Checked == true)
                {
                    loadStaffPerformanceNursesOnlySummary();
                }
                else
                {
                    loadStaffPerformanceNursesOnly();
                }

            }

            else if (ckAll.Checked == false & ddDrs.SelectedValue == "1")
            {
                if (ckConsolidated.Checked == true)
                {
                    loadStaffPerformanceDoctorsOnlySummary();
                }
                else
                {
                    loadStaffPerformanceDoctorsOnly();
                }

            }

            else if (ckAll.Checked == false & ddDrs.SelectedValue == "2")
            {
                if (ckConsolidated.Checked == true)
                {
                    loadStaffPerformanceNursesOnlySummary();
                }
                else
                {
                    loadStaffPerformanceNursesOnly();
                }

            }

            else
            {
                loadStaffPerformance();
            }
        }



        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (ckConsolidated.Checked == true)
            {
                if (gvConso.Rows.Count == 0)
                {
                    lblMsgWarning.Text = "No data to export,  please generate";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    printStaffPerformanceSummary();
                }
            }
            else
            {
                if (gvStaff.Rows.Count == 0)
                {
                    lblMsgWarning.Text = "No data to export,  please generate";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    printStaffPerformance();
                }
            }
        }


        private void printStaffPerformanceSummary()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptStaffPerformanceSumm.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptStaffPerformanceSumm.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/StaffPerformanceSummary.xlsx");

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


                    worksheet.Cell("A1").Value = "Consolidated " + ddBranch.SelectedItem.Text;
                    worksheet.Cell("A2").Value = "Transaction Date :  " + txtDateFrom.Text + " - " + txtDateTo.Text;


                    for (int i = 0; i < gvConso.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 4, 6).Value = "'" + Server.HtmlDecode(gvConso.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 4, 7).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 4, 8).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[7].Text);
                        worksheet.Cell(i + 4, 9).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[8].Text);
                        worksheet.Cell(i + 4, 10).Value = Server.HtmlDecode(gvConso.Rows[i].Cells[9].Text);
                        worksheet.Cell(i + 4, 11).Value = ((Label)gvConso.Rows[i].FindControl("lblQty")).Text;
                        worksheet.Cell(i + 4, 12).Value = ((Label)gvConso.Rows[i].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(i + 4, 13).Value = ((Label)gvConso.Rows[i].FindControl("lblNetAmt")).Text;
                     

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

        private void printStaffPerformance()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptStaffPerformance.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptStaffPerformance.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/StaffPerformance.xlsx");

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
                        worksheet.Cell(i + 4, 10).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[9].Text);
                        worksheet.Cell(i + 4, 11).Value = "'" + Server.HtmlDecode(gvStaff.Rows[i].Cells[10].Text);
                        worksheet.Cell(i + 4, 12).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[11].Text);
                        worksheet.Cell(i + 4, 13).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[12].Text);
                        worksheet.Cell(i + 4, 14).Value = ((Label)gvStaff.Rows[i].FindControl("lblPrice")).Text;
                        worksheet.Cell(i + 4, 15).Value = ((Label)gvStaff.Rows[i].FindControl("lblQty")).Text;
                        worksheet.Cell(i + 4, 16).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[15].Text);
                        worksheet.Cell(i + 4, 17).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[16].Text);
                        worksheet.Cell(i + 4, 18).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[17].Text);
                        worksheet.Cell(i + 4, 19).Value = ((Label)gvStaff.Rows[i].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(i + 4, 20).Value = Server.HtmlDecode(gvStaff.Rows[i].Cells[19].Text);
                        worksheet.Cell(i + 4, 21).Value = ((Label)gvStaff.Rows[i].FindControl("lblNetAmt")).Text;

                        worksheet.Cell(i + 4, 22).Value = ((Label)gvStaff.Rows[i].FindControl("lblGender")).Text;
                        worksheet.Cell(i + 4, 23).Value = ((Label)gvStaff.Rows[i].FindControl("lblCivilSatus")).Text;
                        worksheet.Cell(i + 4, 24).Value = ((Label)gvStaff.Rows[i].FindControl("lblDateOfBirth")).Text;
                        worksheet.Cell(i + 4, 25).Value = ((Label)gvStaff.Rows[i].FindControl("lblAge")).Text;

                

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
                        worksheet.Cell(i + 4, 21).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 22).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 23).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 24).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 25).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



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



        protected void gvStaff_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");


            }
        }

        protected void ckAll_CheckedChanged(object sender, EventArgs e)
        {
            gvStaff.DataSource = null;
            gvStaff.DataBind();
            gvConso.DataSource = null;
            gvConso.DataBind();

            loadBranch();
            //if (ckAll.Checked == true)
            //{
            //    //RequiredBranch.Enabled = false;
            //    if (ddBranch.SelectedValue != "0")
            //    {
            //        loadBranch();
             
            //    }
            //}
            //else
            //{
            //    loadBranch();
             
            //}
        }

        private void loadStaffPerformanceDoctorsOnly()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.StaffPerformanceAllDoctors";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);

                    if (ddBranch.SelectedItem.Text == "All Branches")
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 2);
                    }

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

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

        private void loadStaffPerformanceDoctorsOnlySummary()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.StaffPerformanceAllDoctorsSummary";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);

                    if (ddBranch.SelectedItem.Text == "All Branches")
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 2);
                    }

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvConso.DataSource = dT;
                    gvConso.DataBind();

                    if (gvConso.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }

            }
        }
        private void loadStaffPerformanceAll()
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.StaffPerformanceAll";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);
                    
                    if (ddBranch.SelectedItem.Text=="All Branches")
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 2);
                    }

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    
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

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStaff.DataSource = null;
            gvStaff.DataBind();
            gvConso.DataSource = null;
            gvConso.DataBind();


            if (ddBranch.SelectedItem.Text=="All Branches")
            {
                ckAll.Checked = true;
            }
            else
            {
                ckAll.Checked = false;
            }

        }

        protected void ddDrs_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStaff.DataSource = null;
            gvStaff.DataBind();
            gvConso.DataSource = null;
            gvConso.DataBind();

            

            if (ddDrs.SelectedItem.Text=="All Doctors")
            {
                ckConsolidated.Enabled = true;
            }
            else
            {
                ckConsolidated.Checked = false;
                ckConsolidated.Enabled = false;
                
            }
        }

        protected void ckConsolidated_CheckedChanged(object sender, EventArgs e)
        {
            gvStaff.DataSource = null;
            gvStaff.DataBind();
            gvConso.DataSource = null;
            gvConso.DataBind();
        }




        private void loadStaffPerformanceNursesOnlySummary()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.StaffPerformanceAllNursesSummary";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);

                    if (ddBranch.SelectedItem.Text == "All Branches")
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 2);
                    }

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvConso.DataSource = dT;
                    gvConso.DataBind();

                    if (gvConso.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }

            }
        }

        private void loadStaffPerformanceNursesOnly()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.StaffPerformanceAllNurses";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);

                    if (ddBranch.SelectedItem.Text == "All Branches")
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@OptionBr", 2);
                    }

                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

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


    }
}
