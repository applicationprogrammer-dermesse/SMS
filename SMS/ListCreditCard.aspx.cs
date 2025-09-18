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
    public partial class ListCreditCard : System.Web.UI.Page
    {
        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

        public string startDate;
        public string EndDate;

        public int TheType;
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

                        var yesterday = DateTime.Now.AddDays(-1);
                        txtDateFrom.Text = yesterday.ToShortDateString();
                        txtDateTo.Text = yesterday.ToShortDateString(); //SysClass.getLday(EndDate);
                        loadBranch();
                    }
                    else
                    {
                        //var yesterday = DateTime.Now.AddDays(-1);
                        //txtDateFrom.Text = SysClass.getSday(startDate);
                        //txtDateFrom.Text = yesterday.ToShortDateString();
                        //txtDateTo.Text = yesterday.ToShortDateString(); //SysClass.getLday(EndDate);
                        LoadSystemDate();
                        loadPerBranch();

                    }


                }


            }
        }

        private void LoadSystemDate()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CurrentDate FROM SystemMaster where BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        // DateTime theSAlesDate   DateTime.Now.AddDays(-1)
                        //var Prevday =Convert.ToDateTime(dR[0].ToString());
                        txtDateFrom.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                        txtDateTo.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
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
                    ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }


        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (rbAll.Checked == true & ddOption.SelectedValue == "10")
            {
                TheType = 1;
            }
            else if (rbAll.Checked == true & ddOption.SelectedValue == "11")
            {
                TheType = 11;
            }
            else if (rbAll.Checked == true & ddOption.SelectedValue == "12")
            {
                TheType = 12;
            }

            else if (rbCC.Checked == true & ddOption.SelectedValue == "10")
            {
                TheType = 2;
            }
            else if (rbCC.Checked == true & ddOption.SelectedValue == "11")
            {
                TheType = 21;
            }
            else if (rbCC.Checked == true & ddOption.SelectedValue == "12")
            {
                TheType = 22;
            }


            else if (rbPaymaya.Checked == true & ddOption.SelectedValue == "10")
            {
                TheType = 3;
            }
            else if (rbPaymaya.Checked == true & ddOption.SelectedValue == "11")
            {
                TheType = 31;
            }
            else if (rbPaymaya.Checked == true & ddOption.SelectedValue == "12")
            {
                TheType = 32;
            }
            loadCCardAll();
        }

        public string stRCC;
        public void loadCCardAll()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                if (ddBranch.SelectedValue == "0")
                {
                    stRCC = @"dbo.CreditCardReportAllBranches";
                }
                else
                {
                    stRCC = @"dbo.CreditCardReport";
                }
                
                using (SqlCommand cmD = new SqlCommand(stRCC, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@Type", TheType);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCC.DataSource = dT;
                    gvCC.DataBind();

                    if (gvCC.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }
                }
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvCC.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                genCC();
            }
        }

        private void genCC()
        {

            string localPath = Server.MapPath("~/exlTMP/rptCreditCard.xlsx");
            string newPath = Server.MapPath("~/exlDUMP/rptCreditCard.xlsx");
            newFileName = Server.MapPath("~/exlDUMP/CreditCard.xlsx");

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

                for (int i = 0; i < gvCC.Rows.Count; i++)
                {
                    worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvCC.Rows[i].Cells[0].Text);
                    //worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvCC.Rows[i].Cells[1].Text);
                    worksheet.Cell(i + 5, 2).Value = Server.HtmlDecode(gvCC.Rows[i].Cells[2].Text);
                    worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvCC.Rows[i].Cells[3].Text);
                    worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvCC.Rows[i].Cells[4].Text);
                    worksheet.Cell(i + 5, 5).Value = Server.HtmlDecode(gvCC.Rows[i].Cells[5].Text);
                    worksheet.Cell(i + 5, 6).Value = "'" + Server.HtmlDecode(gvCC.Rows[i].Cells[6].Text);
                    worksheet.Cell(i + 5, 7).Value = "'" + Server.HtmlDecode(gvCC.Rows[i].Cells[7].Text);
                    worksheet.Cell(i + 5, 8).Value = "'" + Server.HtmlDecode(gvCC.Rows[i].Cells[8].Text);
                    worksheet.Cell(i + 5, 9).Value = "'" + Server.HtmlDecode(gvCC.Rows[i].Cells[9].Text);
                    worksheet.Cell(i + 5, 10).Value = "'" + Server.HtmlDecode(gvCC.Rows[i].Cells[10].Text);
                    worksheet.Cell(i + 5, 11).Value = ((Label)gvCC.Rows[i].FindControl("lblTotalAmount")).Text;

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

        protected void ddOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCC.DataSource = null;
            gvCC.DataBind();
        }

        protected void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            gvCC.DataSource = null;
            gvCC.DataBind();
        }

        protected void rbCC_CheckedChanged(object sender, EventArgs e)
        {
            gvCC.DataSource = null;
            gvCC.DataBind();
        }

        protected void rbPaymaya_CheckedChanged(object sender, EventArgs e)
        {
            gvCC.DataSource = null;
            gvCC.DataBind();
        }

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCC.DataSource = null;
            gvCC.DataBind();
        }

    }
}