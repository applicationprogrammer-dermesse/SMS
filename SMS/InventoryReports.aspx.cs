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
    public partial class InventoryReports : System.Web.UI.Page
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
                        LoadFGItem();
                    }
                    else
                    {
                        lblRemarks.Text = string.Empty;
                        loadPerBranch();
                        loadInvSummary();
                    }




                }
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
                    ddBranch.Items.Insert(1, new ListItem("Consolidated", "1"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }

        private void loadInvSummary()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.rptInventorySummary";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSummary.DataSource = dT;
                        gvSummary.DataBind();

                        //if (gvSummary.Rows.Count > 0)
                        //{

                        //    gvSummary.FooterRow.Cells[4].Text = "Total";
                        //    gvSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;


                        //    float total5 = dT.AsEnumerable().Sum(row => row.Field<float>("QtyReceived"));
                        //    gvSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        //    gvSummary.FooterRow.Cells[5].Text = total5.ToString();
                        //}
                    }
                }

                if (ddBranch.SelectedValue == "0")
                {
                    lblNote.Text = string.Empty;
                }
                else
                {
                    lblNote.Text = ddBranch.SelectedItem.Text;
                }
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }



        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (rbSummary.Checked == true)
            {
                ExportSummary();
            }
            else
            {
                ExportDetailed();
            }


        }




        private void ExportSummary()
        {
            if (gvSummary.Rows.Count == 0 & gvSummAllBranchPerItem.Rows.Count == 0)
            {
                lblMsgWarning.Text = "Please generate data to export";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                if (gvSummary.Rows.Count > 0)
                {
                    genSummary();
                }
                else
                {
                    genPerItemAllBranch();
                }
            }
        }



        private void genSummary()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptInvSummary.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptInvSummary.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/InventorySummary.xlsx");

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
                    worksheet.Cell("A2").Value = "AS OF  " + DateTime.Now.ToLongDateString();

                    for (int i = 0; i < gvSummary.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvSummary.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = "'" + Server.HtmlDecode(gvSummary.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvSummary.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvSummary.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyBegBalS")).Text;
                        worksheet.Cell(i + 4, 6).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyReceivedS")).Text;
                        worksheet.Cell(i + 4, 7).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyPRF")).Text;
                        worksheet.Cell(i + 4, 8).Value = ((Label)gvSummary.Rows[i].FindControl("lblUnpostedPRF")).Text;
                        worksheet.Cell(i + 4, 9).Value = ((Label)gvSummary.Rows[i].FindControl("lblUnpostedQtyS")).Text;
                        worksheet.Cell(i + 4, 10).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtySales")).Text;
                        worksheet.Cell(i + 4, 11).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyFree")).Text;
                        worksheet.Cell(i + 4, 12).Value = ((Label)gvSummary.Rows[i].FindControl("lblUnpostedAdjustment")).Text;
                        worksheet.Cell(i + 4, 13).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyAdjustmentS")).Text;
                        worksheet.Cell(i + 4, 14).Value = ((Label)gvSummary.Rows[i].FindControl("lblUnpostedComplimentary")).Text;
                        worksheet.Cell(i + 4, 15).Value = ((Label)gvSummary.Rows[i].FindControl("lblQtyComplimentaryS")).Text;
                        worksheet.Cell(i + 4, 16).Value = ((Label)gvSummary.Rows[i].FindControl("lblAvailableBalanceS")).Text;

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

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

            gvDetailed.DataSource = null;
            gvDetailed.DataBind();

            gvSummAllBranchPerItem.DataSource = null;
            gvSummAllBranchPerItem.DataBind();

            //LoadFGItem();
            gvSummary.DataSource = null;
            gvSummary.DataBind();

            gvSummAllBranchPerItem.DataSource = null;
            gvSummAllBranchPerItem.DataBind();

            lblNote.Text = string.Empty;


        }


        private void loadDetailed()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.InventoryDetailed";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);


                        gvDetailed.DataSource = dT;
                        gvDetailed.DataBind();

                    }
                }

                lblNote.Text = "Item expiration is subject for branch verification(Expiration Date is based on the last delivery of item)";
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }
        protected void gvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //GridViewRow row = gvSummary.Rows[e.RowIndex];
                //Label lblNetAmount = (Label)row.FindControl("lblNetAmount");

                //Label lblSales = ((Label)e.Row.FindControl("lblUnpostedQtyS"));
                //if (Convert.ToDecimal(lblSales.Text) > 0)
                //{
                //    lblSales.ForeColor = System.Drawing.Color.Maroon;
                //    lblSales.Font.Bold = true;
                //}

                if (((Label)e.Row.FindControl("lblUnpostedQtyS")).Text != "0" | ((Label)e.Row.FindControl("lblUnpostedPRF")).Text != "0" | ((Label)e.Row.FindControl("lblUnpostedAdjustment")).Text != "0" | ((Label)e.Row.FindControl("lblQtyReceivedS")).Text != "0" | ((Label)e.Row.FindControl("lblUnpostedComplimentary")).Text != "0" | ((Label)e.Row.FindControl("lblQtySales")).Text != "0")
                {
                    e.Row.BackColor = System.Drawing.Color.PaleGoldenrod;
                    //((Label)e.Row.FindControl("lblUnpostedQtyS")).ForeColor = System.Drawing.Color.Blue;
                    //((Label)e.Row.FindControl("lblUnpostedQtyS")).Font.Bold = true;
                    //((Label)e.Row.FindControl("lblUnpostedQtyS")).BackColor = System.Drawing.Color.PaleGoldenrod;

                }




            }
        }



        protected void ddITemFG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbSummary.Checked == true)
            {
                if (Session["vUser_Branch"].ToString() == "1")
                {
                    loadBranch();

                }

                gvDetailed.DataSource = null;
                gvDetailed.DataBind();

                gvSummary.DataSource = null;
                gvSummary.DataBind();

                loadInvSummaryPerItemAllBranch();
            }
            else
            {
                if (Session["vUser_Branch"].ToString() == "1")
                {
                    loadBranch();

                }
                gvDetailed.DataSource = null;
                gvDetailed.DataBind();

                gvSummary.DataSource = null;
                gvSummary.DataBind();
            }
        }




        private void loadInvSummaryPerItemAllBranch()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.InventorySummaryPerItemAllBranch";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@FGCode", ddITemFG.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSummAllBranchPerItem.DataSource = dT;
                        gvSummAllBranchPerItem.DataBind();

                    }
                }

                lblNote.Text = string.Empty;
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }



        private void genPerItemAllBranch()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptAllBranchPerItem.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptAllBranchPerItem.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/InventoryPerItem.xlsx");

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


                    worksheet.Cell("A1").Value = ddITemFG.SelectedItem.Text;
                    worksheet.Cell("A2").Value = "AS OF  " + DateTime.Now.ToLongDateString();


                    for (int i = 0; i < gvSummAllBranchPerItem.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvSummAllBranchPerItem.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = "'" + Server.HtmlDecode(gvSummAllBranchPerItem.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = "'" + Server.HtmlDecode(gvSummAllBranchPerItem.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvSummAllBranchPerItem.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = Server.HtmlDecode(gvSummAllBranchPerItem.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 4, 6).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtyBegBalS")).Text;
                        worksheet.Cell(i + 4, 7).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtyReceivedS")).Text;
                        worksheet.Cell(i + 4, 8).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtyPRF")).Text;
                        worksheet.Cell(i + 4, 9).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblUnpostedPRF")).Text;
                        worksheet.Cell(i + 4, 10).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblUnpostedQtyS")).Text;
                        worksheet.Cell(i + 4, 11).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtySales")).Text;
                        worksheet.Cell(i + 4, 12).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtyFree")).Text;
                        worksheet.Cell(i + 4, 13).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblUnpostedAdjustment")).Text;
                        worksheet.Cell(i + 4, 14).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtyAdjustmentS")).Text;
                        worksheet.Cell(i + 4, 15).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblUnpostedComplimentary")).Text;
                        worksheet.Cell(i + 4, 16).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblQtyComplimentaryS")).Text;
                        worksheet.Cell(i + 4, 17).Value = ((Label)gvSummAllBranchPerItem.Rows[i].FindControl("lblAvailableBalanceS")).Text;

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

        protected void rbDetailed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDetailed.Checked == true)
            {
                gvSummAllBranchPerItem.DataSource = null;
                gvSummAllBranchPerItem.DataBind();

                gvSummary.DataSource = null;
                gvSummary.DataBind();


            }
        }

        protected void rbSummary_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSummary.Checked == true)
            {
                gvDetailed.DataSource = null;
                gvDetailed.DataBind();

                //loadInvSummary();
            }

        }



        private void ExportDetailed()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptInvDetailed.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptInvDetailed.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/InventoryDetailed.xlsx");

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
                    worksheet.Cell("A2").Value = "AS OF  " + DateTime.Now.ToLongDateString();
                    worksheet.Cell("A3").Value = lblNote.Text;

                    for (int i = 0; i < gvDetailed.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 5, 1).Value = Server.HtmlDecode(gvDetailed.Rows[i].Cells[0].Text.Trim());
                        worksheet.Cell(i + 5, 2).Value = "'" + Server.HtmlDecode(gvDetailed.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 5, 3).Value = Server.HtmlDecode(gvDetailed.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 5, 4).Value = Server.HtmlDecode(gvDetailed.Rows[i].Cells[3].Text.Trim());
                        worksheet.Cell(i + 5, 5).Value = Server.HtmlDecode(((Label)gvDetailed.Rows[i].FindControl("lblBatchNo")).Text.Trim());
                        worksheet.Cell(i + 5, 6).Value = ((Label)gvDetailed.Rows[i].FindControl("lblvDateExpiry")).Text;
                        worksheet.Cell(i + 5, 7).Value = ((Label)gvDetailed.Rows[i].FindControl("lblBalance")).Text;
                        worksheet.Cell(i + 5, 8).Value = ((Label)gvDetailed.Rows[i].FindControl("lblRemarks")).Text;

                        worksheet.Cell(i + 5, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 5, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


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



        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (rbSummary.Checked == true)
            {
                if (ddBranch.SelectedItem.Text == "All Branches")
                {
                    lblMsgWarning.Text = "Please select branch.  If option is Summary";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    loadInvSummary();
                }
            }
            else if (rbDetailed.Checked == true)
            {

                if (ddBranch.SelectedItem.Text == "Consolidated")
                {
                    lblMsgWarning.Text = "Please select branch";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    if (ddBranch.SelectedValue == "1")
                    {
                        lblMsgWarning.Text = "Please select branch";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    else
                    {
                        loadDetailed();
                    }
                }

                //loadDetailed();
            }
            else if (rbMoreThanOne.Checked == true)
            {

                if (ddBranch.SelectedValue == "1")
                {
                    lblMsgWarning.Text = "Please select branch";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    loadDetailedMoreThanOne();
                }

            }
            else if (rbLessThanOne.Checked == true)
            {
                if (ddBranch.SelectedValue == "1")
                {
                    lblMsgWarning.Text = "Please select branch";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {


                    loadDetailedLessThanOne();
                }
            }
            else if (rbLessThanSix.Checked == true)
            {

                if (ddBranch.SelectedValue == "1")
                {
                    lblMsgWarning.Text = "Please select branch";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    loadDetailedLessThanSixMonths();
                }

            }
            else if (rbExpired.Checked == true)
            {
                if (ddBranch.SelectedValue == "1")
                {
                    lblMsgWarning.Text = "Please select branch";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    loadExpired();
                }
            }



        }


        private void loadDetailedMoreThanOne()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.InventoryDetailedMoreThanOne";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);


                        gvDetailed.DataSource = dT;
                        gvDetailed.DataBind();

                    }
                }

                lblNote.Text = "Item expiration is subject for branch verification(Expiration Date is based on the last delivery of item)";
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }

        //
        private void loadDetailedLessThanOne()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.InventoryDetailedLessThanOne";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);


                        gvDetailed.DataSource = dT;
                        gvDetailed.DataBind();

                    }
                }

                lblNote.Text = "Item expiration is subject for branch verification(Expiration Date is based on the last delivery of item)";
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }


        private void loadDetailedLessThanSixMonths()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.InventoryDetailedLessThanSixMonths";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);


                        gvDetailed.DataSource = dT;
                        gvDetailed.DataBind();

                    }
                }

                lblNote.Text = "Item expiration is subject for branch verification(Expiration Date is based on the last delivery of item)";
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        private void loadExpired()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.InventoryDetailedExpired";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);


                        gvDetailed.DataSource = dT;
                        gvDetailed.DataBind();

                    }
                }

                lblNote.Text = "Item expiration is subject for branch verification(Expiration Date is based on the last delivery of item)";
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