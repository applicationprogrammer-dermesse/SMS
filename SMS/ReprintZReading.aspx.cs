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
    public partial class ReprintZReading : System.Web.UI.Page
    {
        public string ThezDate;
        public string theBranchCode;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                //TheReceiptNo = "25-2021-3-00000186";
                ThezDate =   Request.QueryString["Date"];
                theBranchCode = Session["cellBranch"].ToString();
                loadZreading();
            }
        }


        private void loadZreading()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                string stR = @"dbo.ReprintZReading";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["cellBranch"].ToString());
                    cmD.Parameters.AddWithValue("@Date", ThezDate);
                    cmD.Parameters.AddWithValue("@BrCode", theBranchCode.ToString());
                    //cmD.Parameters.AddWithValue("@Date", "08/03/2021");
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    DataSet dS = new DataSet();
                    dA.Fill(dS);

                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);


                    string ServerName = builder["Data Source"].ToString();
                    string DatabaseName = builder["Initial Catalog"].ToString();
                    string UserID = builder["User ID"].ToString();
                    string Password = builder["Password"].ToString();

                    ReportDocument crp = new ReportDocument();




                    crp.Load(Server.MapPath("~/Reports/zReadPosted.rpt"));
                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                    //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
                    crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                    crp.SetDataSource(dS.Tables["table"]);

                    crp.SetParameterValue("@BrCode", theBranchCode.ToString());
                    crp.SetParameterValue("@Date", ThezDate);
                    

                    //crp.SetParameterValue("@BrCode", "14");
                    //crp.SetParameterValue("@Date", "08/03/2021");
                    //crp.SetParameterValue("ParamUserName", Session["FullName"].ToString());


                    crReprintZ.ReportSource = crp;


                    if (Session["TheZoption"].ToString() == "2")
                    {
                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        //settings.PrinterName = fun.GetSchoolSettings("x reading");
                        System.Drawing.Printing.PageSettings pagesettings = new System.Drawing.Printing.PageSettings();
                        //pagesettings.PaperSize = new System.Drawing.Printing.PaperSize(“Custom”, 275, 3000);
                        crp.PrintToPrinter(settings, pagesettings, false);

                        //print directly end

                        ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
                    }
                    else
                    {


                        crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "zReadingPosted");
                    }

                }
            }
        }


    }
}