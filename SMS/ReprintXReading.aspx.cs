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
    public partial class ReprintXReading : System.Web.UI.Page
    {
        public string ThexDate;
        public string theBranchCode;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {


                //TheReceiptNo = "25-2021-3-00000186";
                ThexDate = Request.QueryString["Date"];
                theBranchCode = Session["cellBranch"].ToString();
                loadXreading();
            }
        }


        private void loadXreading()
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
                    cmD.Parameters.AddWithValue("@Date", ThexDate);
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




                    crp.Load(Server.MapPath("~/Reports/xReadPosted.rpt"));
                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
                   crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

                    crp.SetDataSource(dS.Tables["table"]);

                    crp.SetParameterValue("@BrCode", theBranchCode.ToString());
                    crp.SetParameterValue("@Date", ThexDate);

           
                    crp.SetParameterValue("@BrCode", theBranchCode, crp.Subreports[0].Name.ToString());
                    crp.SetParameterValue("@Date", ThexDate, crp.Subreports[0].Name.ToString());
           

                    crReprintX.ReportSource = crp;
                    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "xReadingPosted");

                            
                    //if (Session["TheXoption"].ToString() == "2")
                    //{
                    //    //print directly start

                    //    System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                    //                                         //settings.PrinterName = fun.GetSchoolSettings("x reading");
                    //    System.Drawing.Printing.PageSettings pagesettings = new System.Drawing.Printing.PageSettings();
                    //                                            //pagesettings.PaperSize = new System.Drawing.Printing.PaperSize(“Custom”, 275, 3000);

                                
                                
                        
                    //    crp.PrintToPrinter(1, true, 0, 0);
             
                        
                    //    //print directly end
                    //}
                    //else
                    //{

                    //    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "xReadingPosted");
                    //}

                }
            }


           // ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);

        }


    }
}