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
    public partial class XReading : System.Web.UI.Page
    {
        public string ThexDate;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                //TheReceiptNo = "25-2021-3-00000186";
                ThexDate = Request.QueryString["Date"];
                loadXreading();
            }
        }


        private void loadXreading()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                string stR = @"dbo.xReadingUnposted";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandType=CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@Date", ThexDate);
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataSet dS = new DataSet();
                    dA.Fill(dS);

                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);


                    string ServerName = builder["Data Source"].ToString();
                    string DatabaseName = builder["Initial Catalog"].ToString();
                    string UserID = builder["User ID"].ToString();
                    string Password = builder["Password"].ToString();

                    ReportDocument crp = new ReportDocument();




                    crp.Load(Server.MapPath("~/Reports/xReadUnposted.rpt"));
                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                    //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
                    crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                    crp.SetDataSource(dS.Tables["table"]);

                    crp.SetParameterValue("@BrCode", Session["vUser_Branch"].ToString());
                    crp.SetParameterValue("@Date", ThexDate);
                    crp.SetParameterValue("ParamUserName", Session["FullName"].ToString());



                    crvXreading.ReportSource = crp;





                    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "xReadingUnposted");


                }
            }
        }


    }
}