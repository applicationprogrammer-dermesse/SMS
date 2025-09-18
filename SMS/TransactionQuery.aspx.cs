using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using ClosedXML.Excel;
using System.Reflection;

namespace SMS
{
    public partial class TransactionQuery : System.Web.UI.Page
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

                    txtDateFrom.Text = DateClass.getSday(startDate);
                    txtDateTo.Text = DateClass.getLday(EndDate);
                    txtCustomer.Attributes.Add("onfocus", "this.select()");
                //    loadCustomers();
                    LoadtemMasterList();

                }
            }
        }





        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvCustomer.Rows.Count == 0)
            {
                xlsItem();
            }
            else
            {
                xlsCustomer();
            }
        }


        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

        private void xlsCustomer()
        {
            if (gvCustomer.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {
                string localPath = Server.MapPath("~/exlTMP/rptPRF.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptPRF.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/PRF.xlsx");

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



                    //worksheet.Cell("A2").Value = ddBranch.SelectedItem.Text;
                    worksheet.Cell("A3").Value = "Covered Date = " + txtDateFrom.Text + " to " + txtDateTo.Text;

                    for (int i = 0; i < gvCustomer.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvCustomer.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 5, 5).Value = ((Label)gvCustomer.Rows[i].FindControl("lblvQty")).Text;
                        worksheet.Cell(i + 5, 6).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 5, 7).Value = "'" + Server.HtmlDecode(gvCustomer.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 5, 8).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[7].Text);
                        worksheet.Cell(i + 5, 9).Value = "'" + Server.HtmlDecode(gvCustomer.Rows[i].Cells[8].Text.Trim());
                        worksheet.Cell(i + 5, 10).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[9].Text);

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




        //private void loadCustomers()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"SELECT CustID,CustomerName FROM CustomerTable where BrCode=@BrCode ORDER BY CustomerName";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            cmD.CommandTimeout = 0;
        //            cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
        //            SqlDataReader dR = cmD.ExecuteReader();

        //            ddCustomerName.Items.Clear();
        //            ddCustomerName.DataSource = dR;
        //            ddCustomerName.DataValueField = "CustID";
        //            ddCustomerName.DataTextField = "CustomerName";
        //            ddCustomerName.DataBind();
        //            ddCustomerName.Items.Insert(0, new ListItem("Please select customer", "0"));
        //            ddCustomerName.SelectedIndex = 0;
        //        }
        //    }
        //}

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (gvItem.Rows.Count > 0)
            {
                gvItem.DataSource = null;
                gvItem.DataBind();
                LoadtemMasterList(); 
            }
            loadCustomerTransaction();
        }



        private void loadCustomerTransaction()
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.CustomerTransaction";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@dFrom", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@dTo", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@CustCode", txtCustID.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvCustomer.DataSource = dT;
                    gvCustomer.DataBind();

                    if (gvCustomer.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }
            }
        }

        protected void btnSearchBranch_Click(object sender, EventArgs e)
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [BrCode]
	                          ,[CustID]
                               ,[CustomerName]
                          FROM [CustomerTable]
                          where CustomerName like '" + txtCustomer.Text + "%' ORDER BY [CustomerName]";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@CustID", ddCustomerName.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {

                        gvCustomerList.DataSource = dT;
                        gvCustomerList.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridCustomer();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }

            //protected void ddITemFG_SelectedIndexChanged(object sender, EventArgs e)
            //{
            //    gvCustomer.DataSource = null;
            //    gvCustomer.DataBind();
            //}

        }

        protected void gvCustomerList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvR = gvCustomerList.Rows[e.RowIndex];
            txtCustomer.Text = gvR.Cells[2].Text;
            txtCustID.Text = gvR.Cells[1].Text;
        }


        protected void btnItem_Click(object sender, EventArgs e)
        {
            if (gvCustomer.Rows.Count > 0)
            {
                gvCustomer.DataSource = null;
                gvCustomer.DataBind();
                txtCustomer.Text = string.Empty;
                txtCustID.Text = string.Empty;
            }
            loadItemTransaction();
        }

        private void loadItemTransaction()
        {


            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ItemTransaction";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@dFrom", txtDateFrom.Text);
                    cmD.Parameters.AddWithValue("@dTo", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@ItemCode", ddITemFG.SelectedValue);
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvItem.DataSource = dT;
                    gvItem.DataBind();

                    if (gvItem.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }
            }
        }


        private void LoadtemMasterList()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadItemMasterListForSelling";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.CommandType = CommandType.StoredProcedure;
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


        private void xlsItem()
        {
            if (gvItem.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Generate data to export.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }
            else
            {
                string localPath = Server.MapPath("~/exlTMP/rptPRF.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptPRF.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/PRF.xlsx");

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



                    //worksheet.Cell("A2").Value = ddBranch.SelectedItem.Text;
                    worksheet.Cell("A3").Value = "Covered Date = " + txtDateFrom.Text + " to " + txtDateTo.Text;

                    for (int i = 0; i < gvCustomer.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvCustomer.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 5, 5).Value = ((Label)gvCustomer.Rows[i].FindControl("lblvQty")).Text;
                        worksheet.Cell(i + 5, 6).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 5, 7).Value = "'" + Server.HtmlDecode(gvCustomer.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 5, 8).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[7].Text);
                        worksheet.Cell(i + 5, 9).Value = "'" + Server.HtmlDecode(gvCustomer.Rows[i].Cells[8].Text.Trim());
                        worksheet.Cell(i + 5, 10).Value = Server.HtmlDecode(gvCustomer.Rows[i].Cells[9].Text);

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

    }
}