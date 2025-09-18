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
    public partial class PostedReceipt : System.Web.UI.Page
    {
        public string TheReceiptNo;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


               // TheReceiptNo = "25-2021-2-00000005";
               TheReceiptNo = Request.QueryString["SeriesNo"];
                loadOR();
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
                                    ,[vUser_ID]
                            FROM PostedSalesDetailed
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




                    crp.Load(Server.MapPath("~/Reports/PostedReceipt.rpt"));
                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                    //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
                    crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                    crp.SetDataSource(dS.Tables["table"]);

                    
                    //crp.SetParameterValue("ParamReceiptNo", TheReceiptNo.ToString().TrimEnd());
                    




                    crPostedReceipt.ReportSource = crp;





                    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PostedReceipt");


                }
            }
        }


    }
}