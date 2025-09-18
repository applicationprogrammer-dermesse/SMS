using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using ClosedXML.Excel;
using System.IO;
using System.Threading;
using System.Reflection;

namespace SMS
{
    public partial class SalesPerCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                loadYear();

                

               // this.BindGrid();
                //AddHeaders();
             
            }
        }

        //protected void AddHeaders()
        //{
            
        //    if (gvSalesPerSubCategory.Rows.Count > 0)
        //    {
        //        //This replaces <td> with <th>    
        //        gvSalesPerSubCategory.UseAccessibleHeader = true;
        //        //This will add the <thead> and <tbody> elements    
        //        gvSalesPerSubCategory.HeaderRow.TableSection = TableRowSection.TableHeader;
        //        //This adds the <tfoot> element. Remove if you don't have a footer row    
        //        gvSalesPerSubCategory.FooterRow.TableSection = TableRowSection.TableFooter;
        //    }
        //}  

        private void loadYear()
        {
            int CurrYear = System.DateTime.Now.Year;
            int lessYear = 5;
            ddYear.Items.Insert(0, "" + CurrYear + "");

            for (int y = 1; y <= lessYear; y++)
            {
                ddYear.Items.Add((CurrYear - y).ToString());
            }

            ddYear.Items.Add(CurrYear.ToString());

        }

        public string query;
        private void BindGrid()
        {
            if (ddType.SelectedValue =="Service")
            {
                query = "dbo.SalesPErformancePerCategory";
            }
            else if (ddType.SelectedValue == "Product")
            {
                query = "dbo.SalesPErformancePerCategoryProduct";
            }
             
            //string conString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Option", DDoPTION.SelectedItem.Text);
                        cmd.Parameters.AddWithValue("@Month", ddMonth.SelectedValue);
                        cmd.Parameters.AddWithValue("@CYear", ddYear.SelectedValue);
                        cmd.Parameters.AddWithValue("@ItemType", ddType.SelectedValue);

                        sda.SelectCommand = cmd;

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            

                            gvSalesPerSubCategory.DataSource = dt;
                            gvSalesPerSubCategory.DataBind();
                        }
                    }
                }
            }
        }

        decimal subTotalPQ = 0;
        decimal subTotalCQ = 0;
        decimal subTotal = 0;
        decimal subTotalC = 0;


        protected void gvSalesPerSubCategory_DataBound(object sender, EventArgs e)
        {

            

            for (int i = subTotalRowIndex; i < gvSalesPerSubCategory.Rows.Count; i++)
            {
                subTotalPQ += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[2].Text);
                subTotal += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[3].Text);
                subTotalCQ += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[5].Text);
                subTotalC += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[6].Text);
            }
            this.AddTotalRow("<b>" + currentId.ToString() + "&nbsp;&nbsp;Sub Total &nbsp;&nbsp;:&nbsp;&nbsp;</b> ", subTotalPQ.ToString("N0"), subTotal.ToString("N2"), subTotalCQ.ToString("N0"), subTotalC.ToString("N2"));
            this.AddTotalRow("<b>Total &nbsp;&nbsp;: &nbsp;&nbsp;</b>", totalPQ.ToString("N0"), total.ToString("N2"), totalCQ.ToString("N0"), totalC.ToString("N2"));
        }


        string currentId = "0";
        decimal totalCQ = 0;
        decimal totalPQ = 0;
        decimal total = 0;
        decimal totalC = 0;
        int subTotalRowIndex = 0;
        protected void gvSalesPerSubCategory_RowCreated(object sender, GridViewRowEventArgs e)
        {

            try
            {
                
            



                    subTotalCQ = 0;
                    subTotalPQ = 0;
                    subTotal = 0;
                    subTotalC = 0;
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        DataTable dt = (e.Row.DataItem as DataRowView).DataView.Table;

                        string orderId = dt.Rows[e.Row.RowIndex]["SessionGroup"].ToString();
                        totalPQ += Convert.ToDecimal(dt.Rows[e.Row.RowIndex]["QtyPM"]);
                        total += Convert.ToDecimal(dt.Rows[e.Row.RowIndex]["NetAmtPM"]);
                        totalCQ += Convert.ToDecimal(dt.Rows[e.Row.RowIndex]["QtyCM"]);
                        totalC += Convert.ToDecimal(dt.Rows[e.Row.RowIndex]["NetAmtCM"]);

                        if (orderId != currentId)
                        {
                            if (e.Row.RowIndex > 0)
                            {
                                for (int i = subTotalRowIndex; i < e.Row.RowIndex; i++)
                                {
                                    subTotalPQ += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[2].Text);
                                    subTotal += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[3].Text);
                                    subTotalCQ += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[5].Text);
                                    subTotalC += Convert.ToDecimal(gvSalesPerSubCategory.Rows[i].Cells[6].Text);
                                }
                                this.AddTotalRow("<b>" + currentId.ToString() + "&nbsp;&nbsp; Sub Total : &nbsp;&nbsp;</b>", subTotalPQ.ToString("N0"), subTotal.ToString("N2"), subTotalCQ.ToString("N0"), subTotalC.ToString("N2"));
                                subTotalRowIndex = e.Row.RowIndex;
                            }
                            currentId = orderId;
                        }

                
                }
            }
            catch (Exception x)
            {
                gvSalesPerSubCategory.DataSource = null;
                gvSalesPerSubCategory.DataBind();
            }
        }

        
        private void AddTotalRow(string labelText, string valuePQ, string value, string valueCQ, string valueC)
        {
            
            
            if (gvSalesPerSubCategory.Rows.Count > 0)
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
                row.BackColor = Color.PaleGoldenrod;
                row.Cells.AddRange(new TableCell[8] { new TableCell (), 
                                        new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right},
                                        new TableCell { Text = valuePQ, HorizontalAlign = HorizontalAlign.Center},
                                        new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Right },
                                        new TableCell { Text = string.Empty, HorizontalAlign = HorizontalAlign.Center},
                                        new TableCell { Text = valueCQ, HorizontalAlign = HorizontalAlign.Center},
                                        new TableCell { Text = valueC, HorizontalAlign = HorizontalAlign.Right },
                                        new TableCell { Text = string.Empty, HorizontalAlign = HorizontalAlign.Center},});


                gvSalesPerSubCategory.Controls[0].Controls.Add(row);
            }
        }

        protected void btnGen_Click(object sender, EventArgs e)
        {
            this.BindGrid();
            
            
        }

        protected void ddMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSalesPerSubCategory.DataSource = null;
            gvSalesPerSubCategory.DataBind();
        }

        protected void ddYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSalesPerSubCategory.DataSource = null;
            gvSalesPerSubCategory.DataBind();
        }

        protected void DDoPTION_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvSalesPerSubCategory.DataSource = null;
            gvSalesPerSubCategory.DataBind();
        }


        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (gvSalesPerSubCategory.Rows.Count == 0)
            {
                lblMsgWarning.Text = "No data to export,  please generate";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                Print2();
            }

        }


        private void Print2()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=SubCategory.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                
                gvSalesPerSubCategory.AllowPaging = false;
                this.BindGrid();


                gvSalesPerSubCategory.Caption =  DDoPTION.SelectedItem.Text +  " SERVICES SALES -" + ddMonth.SelectedItem.Text + "  " + ddYear.SelectedItem.Text;
                gvSalesPerSubCategory.CaptionAlign = TableCaptionAlign.Top;


                //gvSalesPerSubCategory.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvSalesPerSubCategory.HeaderRow.Cells)
                {
                    cell.BackColor = gvSalesPerSubCategory.HeaderStyle.BackColor;
                }
                //foreach (GridViewRow row in gvSalesPerSubCategory.Rows)
                //{
                //    row.BackColor = Color.White;
                //    foreach (TableCell cell in row.Cells)
                //    {
                //        if (row.RowIndex % 2 == 0)
                //        {
                //            cell.BackColor = gvSalesPerSubCategory.AlternatingRowStyle.BackColor;
                //        }
                //        else
                //        {
                //            cell.BackColor = gvSalesPerSubCategory.RowStyle.BackColor;
                //        }
                //        cell.CssClass = "textmode";
                //    }
                //}

                gvSalesPerSubCategory.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;
        private void print()
        {
            try
            {
                string localPath = Server.MapPath("~/exlTMP/rptSalesPerSubCat.xlsx");
                string newPath = Server.MapPath("~/exlDUMP/rptSalesPerSubCat.xlsx");
                newFileName = Server.MapPath("~/exlDUMP/SubCategory.xlsx");

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


                    worksheet.Cell("A1").Value = "MONTHLY SERVICES SALES -" + ddMonth.SelectedItem.Text + "  " + ddYear.SelectedItem.Text;
                    //worksheet.Cell("A2").Value = "AS of " + DateTime.Now;

                    
                    for (int i = 0; i < gvSalesPerSubCategory.Rows.Count; i++)
                    {
                        worksheet.Cell(i + 4, 1).Value = ((Label)gvSalesPerSubCategory.Rows[i].FindControl("lblSessionGroup")).Text;// gvSalesPerSubCategory.Rows[i].Cells[0].Text;
                        worksheet.Cell(i + 4, 2).Value = gvSalesPerSubCategory.Rows[i].Cells[1].Text;
                        worksheet.Cell(i + 4, 3).Value = gvSalesPerSubCategory.Rows[i].Cells[2].Text;
                        worksheet.Cell(i + 4, 4).Value = gvSalesPerSubCategory.Rows[i].Cells[3].Text;
                        worksheet.Cell(i + 4, 5).Value = gvSalesPerSubCategory.Rows[i].Cells[4].Text;
                        worksheet.Cell(i + 4, 6).Value = gvSalesPerSubCategory.Rows[i].Cells[5].Text;
                        worksheet.Cell(i + 4, 7).Value = gvSalesPerSubCategory.Rows[i].Cells[6].Text;
                        worksheet.Cell(i + 4, 8).Value = gvSalesPerSubCategory.Rows[i].Cells[7].Text;
                    
                        worksheet.Cell(i + 4, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(i + 4, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        

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

        //protected void gvSalesPerSubCategory_PreRender(object sender, EventArgs e)
        //{
        //    GridViewRow row = gvSalesPerSubCategory.FooterRow;

        //    if (gvSalesPerSubCategory.Rows.Count > 0)
        //    {
        //        gvSalesPerSubCategory.UseAccessibleHeader = true;
        //        gvSalesPerSubCategory.HeaderRow.TableSection = TableRowSection.TableHeader;
        //        gvSalesPerSubCategory.FooterRow.TableSection = TableRowSection.TableFooter;
        //    }
        //}

        //protected void gvSalesPerSubCategory_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
           
        //}

        public override void VerifyRenderingInServerForm(Control control)
        {


        }

    }
}