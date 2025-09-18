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
    public partial class ItemMasterList : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                

                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();
                
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

        protected void btnGen_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.loadItemMasterList";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@Type", ddType.Text);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvItem.DataSource = dT;
                        gvItem.DataBind();
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

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvItem.DataSource = null;
            gvItem.DataBind();
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvItem.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                print();
            }

        }


        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;
        private void print()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptItemMasterList.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptItemMasterList.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/ItemMaster.xlsx");

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



                    worksheet.Cell("A2").Value = "AS of " + DateTime.Now;

                    
                    for (int i = 0; i < gvItem.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[0].Text);
                        worksheet.Cell(i + 4, 2).Value = "'" + Server.HtmlDecode(gvItem.Rows[i].Cells[1].Text);
                        worksheet.Cell(i + 4, 3).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[2].Text);
                        worksheet.Cell(i + 4, 4).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[3].Text);
                        worksheet.Cell(i + 4, 5).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[4].Text);
                        worksheet.Cell(i + 4, 6).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[5].Text);
                        worksheet.Cell(i + 4, 7).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[6].Text);
                        worksheet.Cell(i + 4, 8).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[7].Text);
                        worksheet.Cell(i + 4, 9).Value = ((Label)gvItem.Rows[i].FindControl("lblNoSession")).Text;
                        worksheet.Cell(i + 4, 10).Value = ((Label)gvItem.Rows[i].FindControl("lblvUnitCostS")).Text;
                        worksheet.Cell(i + 4, 11).Value = Server.HtmlDecode(gvItem.Rows[i].Cells[10].Text); 
                        
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

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")) == "Inactive")
            {
                e.Row.Cells[0].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[1].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[2].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[3].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[4].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[5].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[6].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[7].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[8].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[9].BackColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[10].ForeColor = System.Drawing.Color.PaleGoldenrod;
                e.Row.Cells[10].ForeColor = System.Drawing.Color.Red;
            }

        }

    }
}