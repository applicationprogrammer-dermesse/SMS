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
    public partial class PrintIssueSlip : System.Web.UI.Page
    {
        public string theprint;
        public string theBranch;
        public string theDate;
        public string theDelBy;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                theBranch = Session["Branch"].ToString();  
                theDate = Session["Date"].ToString();  
                theprint = Session["IssueSlipNo"].ToString();
                theDelBy = Session["DeliveredBy"].ToString();
                loadIssueSlip();
            }
        }


        private void loadIssueSlip()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                string stR = @"SELECT A.[vFGCode]
                                  ,B.vDESCRIPTION
	                              ,B.vUOM
                                  ,A.[vBatchNo]
                                  ,A.[vDateExpiry]
                                  ,A.[vQty]
                            FROM UnpostedIssuance A 
                            LEFT JOIN vItemMaster B
                            ON A.vFGCode=B.vFGCode
                            where A.IssuanceNo='" + theprint + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataSet dS = new DataSet();
                    dA.Fill(dS);

                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);

                    //ConnectionInfo crConnectionInfo = new ConnectionInfo();
                    //crConnectionInfo.ServerName = builder["Data Source"].ToString();
                    //crConnectionInfo.DatabaseName = builder["Initial Catalog"].ToString();
                    //crConnectionInfo.UserID = builder["User Id"].ToString();
                    //crConnectionInfo.Password = builder["Password"].ToString();



                    string ServerName = builder["Data Source"].ToString();
                    string DatabaseName = builder["Initial Catalog"].ToString();
                    string UserID = builder["User ID"].ToString();
                    string Password = builder["Password"].ToString();

                    ReportDocument crp = new ReportDocument();
                    

                    

                    crp.Load(Server.MapPath("~/Reports/IssueSlip.rpt"));
                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                    //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
                    crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                    crp.SetDataSource(dS.Tables["table"]);



                    crp.SetParameterValue("theBranch", theBranch.ToString().TrimEnd());
                    crp.SetParameterValue("theDate", theDate.ToString());
                    crp.SetParameterValue("theDelivered", theDelBy.ToString());
                    crp.SetParameterValue("theIssueNo", theprint.ToString());
                    //crp.SetParameterValue("theIssuanceNo", theprint.ToString());

                    crViewer.ReportSource = crp;
                    
                    //TableLogOnInfo crTableLogoninfo = new TableLogOnInfo();

                    //foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in crp.Database.Tables)
                    //{
                    //    crTableLogoninfo = CrTable.LogOnInfo;
                    //    crTableLogoninfo.ConnectionInfo = crConnectionInfo;
                    //    CrTable.ApplyLogOnInfo(crTableLogoninfo);
                    //}

                    

                    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "IssueSlip");


                }
            }
        }


    }
}