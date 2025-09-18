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
    public partial class CountNewCustomerByGender : System.Web.UI.Page
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


        


            DateTime startDate = Convert.ToDateTime(txtDateFrom.Text);
            DateTime endDate = Convert.ToDateTime(txtDate.Text);

            if (startDate.ToString("MM") != endDate.ToString("MM"))
            {
                lblMsgWarning.Text = "Different month is not allowed!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else if (startDate.Year != endDate.Year)
            {
                lblMsgWarning.Text = "Different year is not allowed!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                if (ddCustomer.SelectedValue == "1")
                {
                    CountNewCustByGender();
                }
                else
                {
                    CountAllCustByGender();
                }
              
            }



        }

        private void CountAllCustByGender()
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRtoGen = @"dbo.CountGenderAllCustomer";


                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDate.Text);

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustomerCount.DataSource = dT;
                    gvCustomerCount.DataBind();




                }
            }
        }


        private void CountNewCustByGender()
        {
          

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRtoGen = @"dbo.CountGenderNewCustomer";


                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDate.Text);
        
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustomerCount.DataSource = dT;
                    gvCustomerCount.DataBind();




                }
            }
        }

        
       


       

        protected void btnExcel_Click(object sender, EventArgs e)
        {

            if (gvCustomerCount.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {



                string localPath = Server.MapPath("~/exlTMP/rptCustomerCountByGender.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptCustomerCountByGender.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/CustomerCountByGender.xlsx");
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


                    worksheet.Cell("A1").Value = ddCustomer.SelectedItem.Text;
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;
                    



                    for (int r = 0; r < gvCustomerCount.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 2).Value = ((LinkButton)gvCustomerCount.Rows[r].FindControl("lnkMaleOnly")).Text; //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = ((LinkButton)gvCustomerCount.Rows[r].FindControl("lnkFemaleOnly")).Text;
                        worksheet.Cell(r + 5, 4).Value = ((LinkButton)gvCustomerCount.Rows[r].FindControl("lnkNoGender")).Text;
                        worksheet.Cell(r + 5, 5).Value = ((LinkButton)gvCustomerCount.Rows[r].FindControl("lnkAllGender")).Text;


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

        protected void gvCustomerCount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           


        }

        


        protected void btnExcelList_Click(object sender, EventArgs e)
        {

            
        }

        protected void gvCustomerCount_RowCommand1(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewMaleOnly")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustomersViewMaleOnly(gvr.Cells[0].Text);

            }
            else  if (e.CommandName == "ViewFemaleOnly")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustomersViewFemaleOnly(gvr.Cells[0].Text);
            }
            else if (e.CommandName == "ViewNoGender")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustomersViewNoGender(gvr.Cells[0].Text);

            }
            else if (e.CommandName == "ViewAllGender")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustomersViewAllGender(gvr.Cells[0].Text);

            }
        }

        private void loadNewCustomersViewAllGender(string BrCode)
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListNewCustomerAllGender";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers(AllGender) .";

                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";

                    }


                }
            }
        }

        private void loadNewCustomersViewNoGender(string BrCode)
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListNewCustomerNoGender";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers(No Gender) .";

                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";

                    }


                }
            }
        }


        private void loadNewCustomersViewFemaleOnly(string BrCode)
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListNewCustomerFemale";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers(Female) .";

                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";

                    }


                }
            }
        }


        private void loadNewCustomersViewMaleOnly(string BrCode)
        {
      

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListNewCustomerMale";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@sDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@eDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers(Male) .";
                     
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                     
                    }


                }
            }
        }

        protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCustomerCount.DataSource = null;
            gvCustomerCount.DataBind();
        }

    }
}