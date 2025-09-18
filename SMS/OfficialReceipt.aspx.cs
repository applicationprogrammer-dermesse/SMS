using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMS
{
    public partial class OfficialReceipt : System.Web.UI.Page
    {

        public string TheReceiptNo;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                

                //TheReceiptNo = "25-2021-3-00000186";
                //TheReceiptNo = "25-2021-3-00000221";
                TheReceiptNo = Request.QueryString["SeriesNo"];
                loadOR();
            }
        }


        private void loadOR()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {

                    string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                    string stR = @"SELECT DISTINCT [SalesDate]
                                  ,[CustID]
                                  ,[CustomerName]
                                  ,[ReceiptNo]
                                  ,[vFGCode]
                                  ,[vUnitCost]
                                  ,[vQty]
                                  ,[DiscountsAmt]
                                  ,[VatExemption]
                                  ,[NetAmount]
                                    ,[BrCode]
                                    ,vUser_ID
                            FROM UnpostedSalesDetailed
                            where ReceiptNo='" + TheReceiptNo + "'";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        DataSet dS = new DataSet();
                        dA.Fill(dS);

                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);


                        string ServerName = builder["Data Source"].ToString();
                        string DatabaseName = builder["Initial Catalog"].ToString();
                        string UserID = builder["User ID"].ToString();
                        string Password = builder["Password"].ToString();

                        ReportDocument crp = new ReportDocument();




                        crp.Load(Server.MapPath("~/Reports/OfficialReceipt.rpt"));
                        crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                        //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
                        crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                        crp.SetDataSource(dS.Tables["table"]);

                        //crp.SetParameterValue("BrCode", 25);
                        //crp.SetParameterValue("ParamBrCode", 25);
                        crp.SetParameterValue("ParamReceiptNo", TheReceiptNo.ToString().TrimEnd());
                        //crp.SetParameterValue("BrCode", "25");




                        crReceipt.ReportSource = crp;

                        //if (Session["PrintReceiptOption"].ToString() == "Print")
                        //{
                        //    System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        //    //settings.PrinterName = fun.GetSchoolSettings("x reading");
                        //    System.Drawing.Printing.PageSettings pagesettings = new System.Drawing.Printing.PageSettings();
                        //    //pagesettings.PaperSize = new System.Drawing.Printing.PaperSize(“Custom”, 275, 3000);
                        //    crp.PrintToPrinter(settings, pagesettings, false);

                        //    //print directly end

                        //    ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                        //}
                        //else
                        //{



                            crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "OfficialReceipt");
                        //}


                    }
                }
            }
            catch(Exception x)
            {
                Response.Write("<script>alert('" + Server.HtmlEncode(x.Message) + "')</script>");

            }

        }


    }
}