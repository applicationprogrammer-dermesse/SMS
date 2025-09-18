using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using ClosedXML.Excel;
//using System.IO;
//using System.Threading;
//using System.Reflection; 


namespace SMS
{
    public partial class VoidTransaction : System.Web.UI.Page
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

                    lblSeriesNo.Text = Session["ViewTransactionDetail"].ToString();
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
                          FROM [UnpostedSalesPayment]
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
                              ,[ReceiptNo]
                              ,[vFGCode]
                              ,[ItemDescription]
                              ,[vUnitCost]
                              ,[vQty]
                              ,[NoSession]
                              ,[DiscDescription]
                              ,[DiscountsAmt]
                              ,[vDiscPerc]
                              ,[NetAmount]
                              ,[ItemType]
                          FROM [UnpostedSalesDetailed]
                          WHERE ReceiptNo='" + lblSeriesNo.Text + "'";
                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvViewTransaction.DataSource = dT;
                    gvViewTransaction.DataBind();

                    lblCustomerName.Text = dT.Rows[0]["CustomerName"].ToString();
                    lblPatientStatus.Text = dT.Rows[0]["PatientType"].ToString();
                }
            }
        }

        protected void btnVoid_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.VoidTransaction";
                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@ReceiptNo", lblSeriesNo.Text);
                        cmD.ExecuteNonQuery();
                    }
                }


                lblMsgSuccess.Text = lblSeriesNo.Text + " succesfully voided!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }

        }


    }
}