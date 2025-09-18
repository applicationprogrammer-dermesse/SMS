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
    public partial class NewCustomersCount : System.Web.UI.Page
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

            lblNote.Text = string.Empty;
            btnExcelList.Visible = false;

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                if (Session["vUser_Branch"].ToString() == "1")
                {
                    stRtoGen = @"dbo.CountNewCustomersPerBranch";
                }
                else
                {
                    stRtoGen = @"dbo.CountNewCustomersByBranch";
                }

                using (SqlCommand cmD = new SqlCommand(stRtoGen, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@SDate", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@EDate", txtDate.Text);
                    if (Session["vUser_Branch"].ToString() != "1")
                    {
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    }

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
              


                string localPath = Server.MapPath("~/exlTMP/rptNewCustomer.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptNewCustomer.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/NewCustomer.xlsx");
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
                        worksheet.Cell(r + 5, 2).Value = ((Label)gvCustomerCount.Rows[r].FindControl("lnkPandS")).Text; //Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[2].Text.TrimEnd());
                        worksheet.Cell(r + 5, 3).Value = Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[3].Text.TrimEnd());
                        worksheet.Cell(r + 5, 4).Value = Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[4].Text.TrimEnd());
                        worksheet.Cell(r + 5, 5).Value = Server.HtmlDecode(gvCustomerCount.Rows[r].Cells[5].Text.TrimEnd());
                       

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

        protected void gvCustomerCount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName=="ViewList")
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
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListBothCustomersPerBranchService";


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
                        lblNote.Text = "List of new customers availed service only.";
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
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string stRBoth = @"dbo.ListBothCustomersPerBranchProduct";


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

                        worksheet.Cell(r + 5, 1).Value = Server.HtmlDecode(gvCustList.Rows[r].Cells[1].Text);
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

    }
}