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
    public partial class SalesForTheDay : System.Web.UI.Page
    {
        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

        public bool IsPageRefresh = false;
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
                        Response.Redirect("~/UnauthorizedPage.aspx");
                    }
                    else
                    {
                        LoadSystemDate();
                        lblBranch.Text = Session["Dept"].ToString();

                        loadSalesForTheDay();

                        loadSummary();
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
                    }
                }

                else
                {
                    if (ViewState["ViewStateId"].ToString() != Session["SessionId"].ToString())
                    {
                        IsPageRefresh = true;
                    }
                    Session["SessionId"] = System.Guid.NewGuid().ToString();
                    ViewState["ViewStateId"] = Session["SessionId"].ToString();
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

                        lblDate.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                    }

                }
            }
        }

        private void loadSummary()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.SalesSummaryForTheDayPerBranch";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@Date", lblDate.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvSummary.DataSource = dT;
                    gvSummary.DataBind();

                    if (gvSummary.Rows.Count == 0)
                    {
                        lblSummary.Visible = false;
                        btnPrintSumm.Visible = false;
                    }
                }
            }
        }

        private void loadSalesForTheDay()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.SalesDetailedForTheDayPerBranch";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@Date", lblDate.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvSalesForTheDay.DataSource = dT;
                    gvSalesForTheDay.DataBind();

                    if (gvSalesForTheDay.Rows.Count == 0)
                    {
                        lblDet.Text = "No Transaction found";
                        btnPrintDet.Visible = false;
                    }


                }
            }
        }

        protected void gvSalesForTheDay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                string cellTranNoToView = gvr.Cells[0].Text;
                string cellSatus = gvr.Cells[15].Text;
                Session["ViewTransactionDetail"] = cellTranNoToView;
                Session["cellSatus"] = cellSatus;

                Response.Redirect("~/ViewTransaction.aspx?val=" + Session["ViewTransactionDetail"].ToString() + "(" + Session["cellSatus"] + ")");
                //Response.Redirect("~/ViewTransaction.aspx");
            }
            else
            {
                if (Session["IsAllowed"].ToString() == "False")
                {
                    lblMsgWarning.Text = "You are not allowed to void transaction!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;

                }
                else
                {
                    GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;

                    string cellTranNoToView = gvr.Cells[0].Text;
                    //string cellSatus = gvr.Cells[11].Text;
                    Session["ViewTransactionDetail"] = cellTranNoToView;
                    // Session["cellSatus"] = cellSatus;

                    Response.Redirect("~/VoidTransaction.aspx?val=" + Session["ViewTransactionDetail"].ToString());
                }

            }
        }

        protected void gvSalesForTheDay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")) == "Void")
                {

                    e.Row.ForeColor = System.Drawing.Color.Red;
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                    ((LinkButton)e.Row.FindControl("btnVoidUnposted")).Enabled = false;
                    ((LinkButton)e.Row.FindControl("btnVoidUnposted")).Visible = false;

                }
            }
        }




        //Summary start
        protected void btnPrintSumm_Click(object sender, EventArgs e)
        {
            PrintSummary();


        }

        private void PrintSummary()
        {
            //try
            //{
            string localPath = Server.MapPath("~/exlTMP/rptSalesSummary.xlsx");
            string newPath = Server.MapPath("~/exlDUMP/rptSalesSummary.xlsx");
            newFileName = Server.MapPath("~/exlDUMP/SalesSummary.xlsx");

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


                worksheet.Cell("A1").Value = lblBranch.Text;
                worksheet.Cell("A2").Value = "Transaction Date :  " + lblDate.Text;

                for (int i = 0; i < gvSummary.Rows.Count; i++)
                {
                    worksheet.Cell(i + 5, 1).Value = ((Label)gvSummary.Rows[i].FindControl("lblProductGrossAmtsumm")).Text;
                    worksheet.Cell(i + 5, 2).Value = ((Label)gvSummary.Rows[i].FindControl("lblProductDiscountAmtsumm")).Text;
                    worksheet.Cell(i + 5, 3).Value = ((Label)gvSummary.Rows[i].FindControl("lblServiceGrossAmtsumm")).Text;
                    worksheet.Cell(i + 5, 4).Value = ((Label)gvSummary.Rows[i].FindControl("lblServiceDiscountAmtsumm")).Text;
                    worksheet.Cell(i + 5, 5).Value = ((Label)gvSummary.Rows[i].FindControl("lblNetSalessumm")).Text;
                    worksheet.Cell(i + 5, 6).Value = ((Label)gvSummary.Rows[i].FindControl("lblCASHAMTsumm")).Text;
                    worksheet.Cell(i + 5, 7).Value = ((Label)gvSummary.Rows[i].FindControl("lblCreditCardAMTsumm")).Text;
                    worksheet.Cell(i + 5, 8).Value = ((Label)gvSummary.Rows[i].FindControl("lblDebitCardAMTsumm")).Text;
                    worksheet.Cell(i + 5, 9).Value = ((Label)gvSummary.Rows[i].FindControl("lblPaymayaAMTsumm")).Text;
                    worksheet.Cell(i + 5, 10).Value = ((Label)gvSummary.Rows[i].FindControl("lblVoucherAMTsumm")).Text;
                    worksheet.Cell(i + 5, 11).Value = ((Label)gvSummary.Rows[i].FindControl("lblGiftChequeAMTsumm")).Text;
                    worksheet.Cell(i + 5, 12).Value = ((Label)gvSummary.Rows[i].FindControl("lblBankFundTransferAMTsumm")).Text;
                    worksheet.Cell(i + 5, 13).Value = ((Label)gvSummary.Rows[i].FindControl("lblDigitalPaymentAMTsumm")).Text;

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
                    worksheet.Cell(i + 5, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(i + 5, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

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
            //}

            //catch (Exception x)
            //{
            //    lblMsgWarning.Text = x.Message;
            //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //    return;
            //}
        }

        //Summarry End

        //Detail start
        protected void btnPrintDet_Click(object sender, EventArgs e)
        {
            PrintDetail();


        }

        private void PrintDetail()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptSalesDetailed.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesDetailed.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SalesDetailed.xlsx");

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


                    worksheet.Cell("A1").Value = lblBranch.Text;
                    worksheet.Cell("A2").Value = "Transaction Date :  " + lblDate.Text;

                    for (int i = 0; i < gvSalesForTheDay.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvSalesForTheDay.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvSalesForTheDay.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvSalesForTheDay.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvSalesForTheDay.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 5, 5).Value = Server.HtmlDecode(gvSalesForTheDay.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 5, 6).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblDiscountsAmt")).Text;
                        worksheet.Cell(i + 5, 7).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblNetAmt")).Text;
                        worksheet.Cell(i + 5, 8).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblCASHAMT")).Text;
                        worksheet.Cell(i + 5, 9).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblCreditCardAMT")).Text;
                        worksheet.Cell(i + 5, 10).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblDebitCardAMT")).Text;
                        worksheet.Cell(i + 5, 11).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblPaymayaAMT")).Text;
                        worksheet.Cell(i + 5, 12).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblVoucherAMT")).Text;
                        worksheet.Cell(i + 5, 13).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblGiftChequeAMT")).Text;
                        worksheet.Cell(i + 5, 14).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblBankFundTransferAMT")).Text;
                        worksheet.Cell(i + 5, 15).Value = ((Label)gvSalesForTheDay.Rows[i].FindControl("lblDigitalPaymentAMT")).Text;
                        worksheet.Cell(i + 5, 16).Value = Server.HtmlDecode(gvSalesForTheDay.Rows[i].Cells[15].Text);

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
                        worksheet.Cell(i + 5, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

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

        //Detail End







    }
}