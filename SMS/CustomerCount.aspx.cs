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
    public partial class CustomerCount : System.Web.UI.Page
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


            gvCustList.DataSource = null;
            gvCustList.DataBind();

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            

            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();


            lblNote.Text = string.Empty;
            btnExcelList.Visible = false;
            btnExcelListInactive.Visible = false;
            btnExcelListTRansactionRecord.Visible = false;


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
                CountNewCustomer();
                CountInactiveCustomer();
                CountActiveCustomer();
                CountTransactionRecordPerBranch();
            }



        }

        private void CountNewCustomer()
        {
            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();
            
            btnExcelListInactive.Visible = false;

            gvCustList.DataSource = null;
            gvCustList.DataBind();
            btnExcelList.Visible = false;

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                
                string stRtoGen = @"dbo.CountNewCustomersAllBranches";
                

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    //if (Session["vUser_Branch"].ToString() != "1")
                    //{
                    //    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    //}

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustomerCount.DataSource = dT;
                    gvCustomerCount.DataBind();




                }
            }
        }

        private void CountInactiveCustomer()
        {
            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();
            btnExcelListInactive.Visible = false;

            gvCustList.DataSource = null;
            gvCustList.DataBind();
            btnExcelList.Visible = false;

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRInactive = @"dbo.CountInactiveCustomers";


                using (SqlCommand cmD = new SqlCommand(stRInactive, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    //if (Session["vUser_Branch"].ToString() != "1")
                    //{
                    //    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    //}

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                   gvInactive.DataSource = dT;
                   gvInactive.DataBind();




                }
            }
        }


        private void CountTransactionRecordPerBranch()
        {
     
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRtoGen = @"dbo.TransactionRecordPerBranch";


                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
          
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvTransactionRecord.DataSource = dT;
                    gvTransactionRecord.DataBind();
                    


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



                string localPath = Server.MapPath("~/exlTMP/rptCustomerCount.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptCustomerCount.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/CustomerCount.xlsx");
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


                    worksheet.Cell("A1").Value = "New Customers Count";
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;



                    for (int r = 0; r < gvCustomerCount.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 2).Value = ((LinkButton)gvCustomerCount.Rows[r].FindControl("lnkServiceOnly")).Text;  //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = ((LinkButton)gvCustomerCount.Rows[r].FindControl("lnkProduct")).Text;
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[4].Text.TrimEnd());

                      
                    }

                    for (int r = 0; r < gvActive.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 6).Value = Server.HtmlDecode(gvActive.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 7).Value = ((LinkButton)gvActive.Rows[r].FindControl("lnkActiveS")).Text; //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 8).Value = ((LinkButton)gvActive.Rows[r].FindControl("lnkActiveP")).Text;


                    }

                    for (int r = 0; r < gvTransactionRecord.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 10).Value = Server.HtmlDecode(gvTransactionRecord.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 11).Value = ((LinkButton)gvTransactionRecord.Rows[r].FindControl("lnkTransactionRecords")).Text; //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        


                    }

                    for (int r = 0; r < gvInactive.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 13).Value = Server.HtmlDecode(gvInactive.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 14).Value = ((LinkButton)gvInactive.Rows[r].FindControl("lnkInactiveS")).Text; //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 15).Value = ((LinkButton)gvInactive.Rows[r].FindControl("lnkInactiveP")).Text;


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
            if (e.CommandName == "ViewList")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustonersList(gvr.Cells[0].Text);


                //Session["cellBranch"] = gvr.Cells[0].Text;
                //Session["cellType"] = "Availed Product and Service";

                //Session["startDate"] = txtDateFrom.Text;
                //Session["endDate"] = txtDate.Text;

                //Response.Redirect("~/ListNewCustomerPerBranch.aspx?val=" + Session["cellBranch"].ToString() + "(" + Session["cellType"].ToString() + ")");


            }

            else if (e.CommandName == "ViewListProduct")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustomersListProduct(gvr.Cells[0].Text);

            }

            else if (e.CommandName == "ViewListService")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadNewCustomersListService(gvr.Cells[0].Text);

            }


        }

        private void loadNewCustomersListService(string BrCode)
        {
            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();
            btnExcelListInactive.Visible = false;
            
            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListNewCustomersAvailedServicePerBranch";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers availed service.";
                        btnExcelList.Visible = true;
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelList.Visible = false;
                    }


                }
            }
        }

        private void loadNewCustomersListProduct(string BrCode)
        {
            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();

            btnExcelListInactive.Visible = false;
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListNewCustomersAvailedProductPerBranch";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers availed product only";
                        btnExcelList.Visible = true;
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelList.Visible = false;
                    }


                }
            }
        }

        private void loadNewCustonersList(string BrCode)
        {

            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();

            btnExcelListInactive.Visible = false;
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListBothCustomersPerBranch";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustList.DataSource = dT;
                    gvCustList.DataBind();

                    if (gvCustList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of new customers availed both product and service.";
                        btnExcelList.Visible = true;
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelList.Visible = false;
                    }


                }
            }
        }


        protected void btnExcelList_Click(object sender, EventArgs e)
        {

            if (gvCustList.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {



                string localPath = Server.MapPath("~/exlTMP/rptNewCustomerList.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptNewCustomerList.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/NewCustomerList.xlsx");
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


                    worksheet.Cell("A1").Value = "New Customers List";
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;
                    worksheet.Cell("A3").Value = lblNote.Text;



                    for (int r = 0; r < gvCustList.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value =  Server.HtmlDecode(gvCustList.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 2).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[2].Text.TrimEnd()); //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[3].Text.TrimEnd());
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[4].Text.TrimEnd());
                        worksheet.Cell(r + 5, 5).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[5].Text.TrimEnd());
                        worksheet.Cell(r + 5, 6).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[6].Text.TrimEnd());
                        worksheet.Cell(r + 5, 7).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[7].Text.TrimEnd());


                        worksheet.Cell(r + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;



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

        protected void gvInactive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewListIS")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadInactiveS(gvr.Cells[0].Text);

            }

            else if (e.CommandName == "ViewListIP")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadInactiveP(gvr.Cells[0].Text);

            }
        }


        private void loadInactiveS(String BrCode)
        {

            gvCustList.DataSource = null;
            gvCustList.DataBind();

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();

            btnExcelList.Visible = false;
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListInactiveCustomersS";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInactiveList.DataSource = dT;
                    gvInactiveList.DataBind();

                    if (gvInactiveList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of inactive customers availed service.";
                        btnExcelListInactive.Visible = true;
                        
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelListInactive.Visible = false;
                    }


                }
            }
        }


        private void loadInactiveP(String BrCode)
        {
            gvCustList.DataSource = null;
            gvCustList.DataBind();

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();

            btnExcelList.Visible = false;
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListInactiveCustomersP";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInactiveList.DataSource = dT;
                    gvInactiveList.DataBind();

                    if (gvInactiveList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of inactive customers(buying only) .";
                        btnExcelListInactive.Visible = true;
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelListInactive.Visible = false;
                    }


                }
            }
        }


        protected void btnExcelListInactive_Click(object sender, EventArgs e)
        {

            if (gvInactiveList.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {



                string localPath = Server.MapPath("~/exlTMP/rptInactiveCustomerList.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptInactiveCustomerList.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/Customers.xlsx");
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


                    worksheet.Cell("A1").Value = lblNote.Text;
                    worksheet.Cell("A2").Value = "";
                    //worksheet.Cell("A3").Value = lblNote.Text;



                    for (int r = 0; r < gvInactiveList.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvInactiveList.Rows[r].Cells[2].Text);
                        worksheet.Cell(r + 5, 2).Value = ((LinkButton)gvInactiveList.Rows[r].FindControl("lnkInactiveCN")).Text;// Server.HtmlDecode(gvInactiveList.Rows[r].Cells[3].Text.TrimEnd()); //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvInactiveList.Rows[r].Cells[4].Text.TrimEnd());
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvInactiveList.Rows[r].Cells[5].Text.TrimEnd());
                        worksheet.Cell(r + 5, 5).Value = "'" + Server.HtmlDecode(gvInactiveList.Rows[r].Cells[6].Text.TrimEnd());


                        worksheet.Cell(r + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

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

        protected void gvInactiveList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewListICN")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                ViewCustomerHistory(gvr.Cells[0].Text, gvr.Cells[1].Text);

            }
        }



        private void ViewCustomerHistory(string theBranchCode, string theCustomerCode)
        {
            ShowHistory(theBranchCode, theCustomerCode);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridCustomerTransaction();", true);
            return;
        }

        private void ShowHistory(string theBranchCode, string theCustomerCode)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"SELECT [SalesDate]
                                      ,[ReceiptNo]
                                      ,[ItemDescription]
                                      ,[NetAmount]
                                      ,CustomerName
                                  FROM [PostedSalesDetailed]
                                  where [vStat]=2
                                  AND [NetAmount] > 0
                                  AND [CustID]=@CustID
                                  AND BrCode=@BrCode
                                  ORDER BY [SalesDate] DESC";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(theBranchCode));
                    cmD.Parameters.AddWithValue("@CustID", Convert.ToInt32(theCustomerCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvHistory.DataSource = dT;
                    gvHistory.DataBind();

                    lblCustName.Text = dT.Rows[0][4].ToString();
                }
            }
        }


        private void CountActiveCustomer()
        {
            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();
            btnExcelListInactive.Visible = false;

            gvCustList.DataSource = null;
            gvCustList.DataBind();
            btnExcelList.Visible = false;


            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRInactive = @"dbo.CountActiveCustomersAllBranches";


                using (SqlCommand cmD = new SqlCommand(stRInactive, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
              

                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvActive.DataSource = dT;
                    gvActive.DataBind();




                }
            }
        }

        protected void gvActive_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewListAS")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadActiveS(gvr.Cells[0].Text);

            }

            else if (e.CommandName == "ViewListAP")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadActiveP(gvr.Cells[0].Text);

            }
        }


        private void loadActiveS(String BrCode)
        {

            gvCustList.DataSource = null;
            gvCustList.DataBind();
            btnExcelList.Visible = false;


            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListActiveCustomersS";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInactiveList.DataSource = dT;
                    gvInactiveList.DataBind();

                    if (gvInactiveList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of Active customers availed service.";
                        btnExcelListInactive.Visible = true;

                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelListInactive.Visible = false;
                    }


                }
            }
        }


        private void loadActiveP(String BrCode)
        {
            gvCustList.DataSource = null;
            gvCustList.DataBind();
            btnExcelList.Visible = false;

            gvTransactionRecordDetails.DataSource = null;
            gvTransactionRecordDetails.DataBind();
            btnExcelListTRansactionRecord.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListActiveCustomersP";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvInactiveList.DataSource = dT;
                    gvInactiveList.DataBind();

                    if (gvInactiveList.Rows.Count > 0)
                    {
                        lblNote.Text = "List of Active customers(buying only) .";
                        btnExcelListInactive.Visible = true;
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelListInactive.Visible = false;
                    }


                }
            }
        }

        protected void gvTransactionRecord_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewTransactionRecords")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;
                loadTransactionRecords(gvr.Cells[0].Text);

            }
        }


        private void loadTransactionRecords(String BrCode)
        {
            gvCustList.DataSource = null;
            gvCustList.DataBind();


            gvInactiveList.DataSource = null;
            gvInactiveList.DataBind();

            //gvTransactionRecordDetails.DataSource = null;
            //gvTransactionRecordDetails.DataBind();
            
            btnExcelList.Visible = false;
            btnExcelListInactive.Visible = false;
            btnExcelListTRansactionRecord.Visible = true;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListTransactionRecord";


                using (SqlCommand cmD = new SqlCommand(stRBoth, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Convert.ToInt32(BrCode));


                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvTransactionRecordDetails.DataSource = dT;
                    gvTransactionRecordDetails.DataBind();

                    if (gvTransactionRecordDetails.Rows.Count > 0)
                    {
                        lblNote.Text = "Transaction Record Customer Type";
                        btnExcelListTRansactionRecord.Visible = true;
                    }
                    else
                    {
                        lblNote.Text = "No Record Found.";
                        btnExcelListTRansactionRecord.Visible = false;
                         
                    }


                }
            }
        }

        protected void btnExcelListTRansactionRecord_Click(object sender, EventArgs e)
        {
            if (gvTransactionRecordDetails.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {



                string localPath = Server.MapPath("~/exlTMP/rptCustomerType.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptCustomerType.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/CustomerTypeList.xlsx");
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


                    worksheet.Cell("A1").Value = "Transaction Record Customer Type";
                    worksheet.Cell("A2").Value = "Covered Date : " + txtDateFrom.Text + " - " + txtDate.Text;
                    worksheet.Cell("A3").Value = lblNote.Text;



                    for (int r = 0; r < gvTransactionRecordDetails.Rows.Count; r++)
                    {

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvTransactionRecordDetails.Rows[r].Cells[1].Text);
                        worksheet.Cell(r + 5, 2).Value = Server.HtmlDecode(gvTransactionRecordDetails.Rows[r].Cells[2].Text.TrimEnd()); //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvTransactionRecordDetails.Rows[r].Cells[3].Text.TrimEnd());
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvTransactionRecordDetails.Rows[r].Cells[4].Text.TrimEnd());
                        

                        worksheet.Cell(r + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(r + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        


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