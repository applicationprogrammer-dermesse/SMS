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
    public partial class SalesReturnReceipt : System.Web.UI.Page
    {
        public string TheReceiptNo;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {



                //TheReceiptNo = "6-2021-3-00000327";
                //TheReceiptNo = Session["TheRecieptNo"].ToString();
                if (Request.QueryString["SeriesNo"] != string.Empty)
                {
                    TheReceiptNo = Request.QueryString["SeriesNo"];
                    loadOR();
                }
            }
        }


        private void loadOR()
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




                    crSalesReturn.ReportSource = crp;





                    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "SalesReturnReceipt");


                }
            }
        }


    }
}