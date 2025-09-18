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
    public partial class ReportCash : System.Web.UI.Page
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
                        //txtDateFrom.Text = yesterday.ToShortDateString();
                        // txtDateFrom.Text = DateClass.getSday(startDate);
                        txtDateTo.Text = yesterday.ToShortDateString(); //SysClass.getLday(EndDate);

                        DateTime date = Convert.ToDateTime(txtDateTo.Text);
                        var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                        txtDateFrom.Text = firstDayOfMonth.ToString("MM/dd/yyyy");

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

            loadCashPaymentAll();
        }

        public int opt;

        public void loadCashPaymentAll()
        {
            
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.CashPaymentReport";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    if (ddBranch.SelectedValue == "0")
                    {
                        cmD.Parameters.AddWithValue("@All", 1);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@All", 2);
                    }
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDateTo.Text);
                    
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCashPayment.DataSource = dT;
                    gvCashPayment.DataBind();

                    if (gvCashPayment.Rows.Count == 0)
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
            if (gvCashPayment.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                genDP();
            }
        }

        private void genDP()
        {

            string localPath = Server.MapPath("~/exlTMP/rptCashPayment.xlsx");
            string newPath = Server.MapPath("~/exlDUMP/rptCashPayment.xlsx");
            newFileName = Server.MapPath("~/exlDUMP/CashPayment.xlsx");

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

                for (int i = 0; i < gvCashPayment.Rows.Count; i++)
                {
                    worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvCashPayment.Rows[i].Cells[0].Text);
                    worksheet.Cell(i + 5, 2).Value = Server.HtmlDecode(gvCashPayment.Rows[i].Cells[2].Text);
                    worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvCashPayment.Rows[i].Cells[3].Text);
                    worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvCashPayment.Rows[i].Cells[4].Text);
                    worksheet.Cell(i + 5, 5).Value = Server.HtmlDecode(gvCashPayment.Rows[i].Cells[5].Text);
                    worksheet.Cell(i + 5, 6).Value = ((Label)gvCashPayment.Rows[i].FindControl("lblTotalAmount")).Text;
                    
                    worksheet.Cell(i + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 5, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 5, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    



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

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCashPayment.DataSource = null;
            gvCashPayment.DataBind();
        }

       


    }
}