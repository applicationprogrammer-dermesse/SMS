using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace SMS
{
    public partial class ViewTransactionReturn : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmpNo"] == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='LoginPage.aspx';", true);
            }
            else
            {
                if (!IsPostBack)
                {
                    ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                    lblSeriesNo.Text = Session["ViewTransactionDetailReturn"].ToString();
                    lblTransactionStatus.Text = Session["cellSatusPosted"].ToString();
                    LoadTransactionDetail();
                    LoadPaymentDetail();
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
        }

        private void LoadPaymentDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT  [PaymentMode]
		                    ,[TotalAmount]
                           ,[BankName]
                          ,[BatchNumber]
                          ,[ReferenceNumber]
                          FROM [PostedSalesPayment]
                          WHERE ReceiptNo='" + lblSeriesNo.Text + "'";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvPaymentDetail.DataSource = dT;
                    gvPaymentDetail.DataBind();
                }
            }
        }
        private void LoadTransactionDetail()
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [RecID]
                              ,[SalesDate]
                              ,[CustomerName]
                              ,[PatientType]
                               ,OrderSource
                              ,[ReceiptNo]
                              ,[vFGCode]
                              ,[ItemDescription]
                              ,[vUnitCost]
                              ,[vQty]
                              ,[NoSession]
                              ,[DiscDescription]
                              ,[DiscountsAmt]
                              ,[vDiscPerc]
                               ,PerformedBy
                              ,[NetAmount]
                              ,[ItemType]
                              ,CASE WHEN IsDeposit=1 AND IsDepositPaid=0 THEN 'Deposit Transaction'
                                    WHEN IsDeposit=1 AND IsDepositPaid=1 THEN 'Full payment of Recipt No.' + ' ' + [TransactionID]
							  ELSE '' END AS [IsDeposit]
                          FROM [PostedSalesDetailed]
                          WHERE ReceiptNo='" + lblSeriesNo.Text + "'";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvViewTransaction.DataSource = dT;
                    gvViewTransaction.DataBind();

                    lblDate.Text = Convert.ToDateTime(dT.Rows[0]["SalesDate"]).ToShortDateString();
                    lblCustomerName.Text = dT.Rows[0]["CustomerName"].ToString();
                    lblPatientStatus.Text = dT.Rows[0]["PatientType"].ToString();

                    string isReturn = dT.Rows[0]["OrderSource"].ToString();

                    if (isReturn.ToString() == "Sales Return")
                    {
                        lblDeposit.Text = "Sales Return";
                    }
                    else
                    {
                        lblDeposit.Text = dT.Rows[0]["IsDeposit"].ToString();
                    }

                }
            }
        }


    }
}