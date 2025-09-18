using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.HtmlControls;

namespace SMS
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["vUser_Branch"] == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='LoginPage.aspx';", true);
            }
            else
            {
                if (!IsPostBack)
                {

                    ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));


                    Session["theBRnm"] = "Select Branch";
                    Session["theBRcd"] = "0";
                    Session["sDate"] = "01/01/1900";

                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        CountUnpostedPRF();
                        CountUnpostedComplimentary();
                        CountUnpostedAdjustment();
                        CountUnpostedIssuance();
                    }
                    else
                    {
                        CountUnpostedPRFPerBranch();
                        CountUnpostedComplimentaryPerBranch();
                        CountUnpostedAdjustmentPerBranch();
                        CountUnpostedIssuancePerBranch();
                        CountUnpostedTransaction();

                        LoadSystemDate();
                    }


                }
            }
        }

        //private void HideSomeInfo()
        //{

        //    //if (Session["vUser_Branch"].ToString() != "1")
        //    //{

        //    //    ((HtmlGenericControl)Master.FindControl("HOIssuance")).Visible = false;
        //    //    ((HtmlGenericControl)Master.FindControl("HOCreditCardTagging")).Visible = false;
        //    //    ((HtmlGenericControl)Master.FindControl("BRSalesReturn")).Visible = false;

        //    //((HtmlGenericControl)Master.FindControl("HOAddNewItem")).Visible = false;
        //    //((HtmlGenericControl)Master.FindControl("HOSetupDiscount")).Visible = false;
        //    //((HtmlGenericControl)Master.FindControl("HOAddPromoItem")).Visible = false;
        //    //((HtmlGenericControl)Master.FindControl("HOSpecialDiscount")).Visible = false;
        //    //((HtmlGenericControl)Master.FindControl("HOSetupSalesTarget")).Visible = false;
        //    //((HtmlGenericControl)Master.FindControl("BRSalesReturn")).Visible = false;

             

        //    //}

        //    //else if (Convert.ToInt32(Session["UserType"]) == 3)
        //    //{
        //    //    ((HtmlGenericControl)Master.FindControl("trHO")).Visible = false;
        //    //    ((HtmlGenericControl)Master.FindControl("rptHO")).Visible = false;
        //    //    ((HtmlGenericControl)Master.FindControl("setHO")).Visible = false;

        //    //}
        //}
        
        private void LoadSystemDate()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CurrentDate FROM SystemMaster where BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        lblTransactionDate.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                    }

                }
            }
        }

        private void CountUnpostedTransaction()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(DISTINCT ReceiptNo) as [CountUnpostedSales]  FROM [UnpostedSalesDetailed] WHERE vStat=1  and BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblSalesfortheday.Text = dT.Rows[0]["CountUnpostedSales"].ToString();

                }
            }
        }

        private void CountUnpostedIssuancePerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [IssuanceNo]) as [CountIssuanceNo]  FROM [UnpostedIssuance] WHERE vStat=1  and BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblIssuance.Text = dT.Rows[0]["CountIssuanceNo"].ToString();

                }
            }
        }

        private void CountUnpostedIssuance()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [IssuanceNo]) as [CountIssuanceNo]  FROM [UnpostedIssuance] WHERE vStat=1";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblIssuance.Text = dT.Rows[0]["CountIssuanceNo"].ToString();

                }
            }
        }

        private void CountUnpostedAdjustment()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [AdjustmentNo]) as [CountAdjustmentNo]  FROM [UnpostedAdjustment] WHERE vStat=1";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblAdjustment.Text = dT.Rows[0]["CountAdjustmentNo"].ToString();

                }
            }
        }


        private void CountUnpostedPRF()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [PRFno]) as [CountPRFno]  FROM [UnpostedPRF] WHERE vStat=1";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblPRFno.Text = dT.Rows[0]["CountPRFno"].ToString();

                }
            }
        }

        private void CountUnpostedComplimentary()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [ComplimentaryNo]) as [CountComplimentaryNo]  FROM [UnpostedComplimentary] WHERE vStat=1";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblComplimentaryno.Text = dT.Rows[0]["CountComplimentaryno"].ToString();

                }
            }
        }


        private void CountUnpostedAdjustmentPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [AdjustmentNo]) as [CountAdjustmentNo]  FROM [UnpostedAdjustment] WHERE vStat=1 and BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblAdjustment.Text = dT.Rows[0]["CountAdjustmentNo"].ToString();

                }
            }
        }


        private void CountUnpostedPRFPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
               // string stR = @"SELECT COUNT(distinct [PRFno]) as [CountPRFno]  FROM [UnpostedPRF] WHERE vStat=1 and BrCode=@BrCode";
                string stR = @"SELECT COUNT(distinct [PRFno]) as [CountPRFno]  FROM [UnpostedPRF] WHERE vStat=1 and TargetBr=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblPRFno.Text = dT.Rows[0]["CountPRFno"].ToString();

                }
            }
        }

        private void CountUnpostedComplimentaryPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT COUNT(distinct [Complimentaryno]) as [CountComplimentaryno]  FROM [UnpostedComplimentary] WHERE vStat=1 and BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    lblComplimentaryno.Text = dT.Rows[0]["CountComplimentaryno"].ToString();

                }
            }
        }
        


 

    }
}