using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
//using System.Reflection;
//using System.Threading;
using System.Globalization;

namespace SMS
{
    public partial class EncodeSales : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;

        //DataTable dt = new DataTable();
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

                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        Response.Redirect("~/UnauthorizedPage.aspx");
                    }
                    else
                    {

                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();

                        //Session["vUser_Branch"] = "5";

                        LoadSystemDate();

                        checkIfMonthlyPostingISDone();

                        loadReceiptNo();
                        LoadtemMasterList();
                        loadSource();
                        loadPaymentType();
                        //loadBanks();
                        LoadDoctors();
                        txtManualOR.Attributes.Add("onfocus", "this.select()");
                        txtManualOR.Focus();

                        txtSRP.Attributes.Add("onfocus", "this.select()");
                        txtAmtToPaid.Attributes.Add("onfocus", "this.select()");
                        txtCustomer.Attributes.Add("onfocus", "this.select()");
                        txtAmtDeposit.Attributes.Add("onfocus", "this.select()");

                        txtxSession.Attributes.Add("onfocus", "this.select()");
                        //start
                        DataTable dtDetail = new DataTable();
                        dtDetail.Columns.AddRange(new DataColumn[19] 
                            {  new DataColumn("vItemID"), 
                                new DataColumn("vFGCode"), 
                                new DataColumn("ItemDescription"), 
                                new DataColumn("vUnitCost"),
                                new DataColumn("vQty"),
                                new DataColumn("TotalSession"),
                                new DataColumn("NoSession"),
                                new DataColumn("sConstant"),
                                new DataColumn("DiscDescription"),
                                new DataColumn("DiscountsAmt"),
                                new DataColumn("VatExemption"),
                                new DataColumn("vDiscPerc"),
                                new DataColumn("NetAmount"),
                                new DataColumn("ItemType"),
                                new DataColumn("IsKit"),
                                new DataColumn("IsDeposit"),
                                new DataColumn("PerformedBy"),
                                new DataColumn("TransactionID"),
                                new DataColumn("IsDepositPaid")
                                
                            });
                        ViewState["UnpostedDetail"] = dtDetail;
                        this.BindItemGrid();


                        //end


                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[7] 
                            {  new DataColumn("PaymentMode"), 
                                new DataColumn("TotalAmount"), 
                                new DataColumn("BankName"), 
                                new DataColumn("BatchNumber"), 
                                new DataColumn("ReferenceNumber"),
                            new DataColumn("CCNumber"),
                            new DataColumn("CCname")});

                        ViewState["Payment"] = dt;
                        this.BindPaymentGrid();
                    }
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

        int TheYear;
        int TheMonth;
        private void checkIfMonthlyPostingISDone()
        {

            TheYear = Convert.ToDateTime(txtDate.Text).Year;
            TheMonth = Convert.ToDateTime(txtDate.Text).Month;

            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {

                sqlConn.Open();
                string stRinvCurr = @"SELECT MonthlyPostingYear,MonthlyPostingMonth FROM SystemMaster 
                        WHERE MonthlyPostingYear=@MonthlyPostingYear
                        AND MonthlyPostingMonth=@MonthlyPostingMonth
                        AND  BrCode=@BrCode";
                using (SqlCommand cmDCheck = new SqlCommand(stRinvCurr, sqlConn))
                {

                    cmDCheck.CommandTimeout = 0;
                    cmDCheck.Parameters.AddWithValue("@MonthlyPostingYear", TheYear);
                    cmDCheck.Parameters.AddWithValue("@MonthlyPostingMonth", TheMonth);
                    cmDCheck.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dRCheck = cmDCheck.ExecuteReader();
                    if (dRCheck.HasRows)
                    {
                    }
                    else
                    {

                        lblMsgWithPending.Text = "Monthly Posting of the previous month not yet done!\n\nPlease process monthly posting first.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsgWithPending();", true);
                        return;
                    }
                }
            }
        }




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
                        // DateTime theSAlesDate
                        txtDate.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                    }

                }
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                UpdateReceiptNo();
                loadReceiptNo();
            }
        }
        private void loadReceiptNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CAST([BrCode] AS VARCHAR(4)) + '-' + CAST(YEAR([CurrentDate]) AS VARCHAR(4)) + '-'+ CAST(MONTH([CurrentDate]) AS VARCHAR(2)) + '-' + RIGHT('000000000'+CAST(Series AS VARCHAR(8)),8)  FROM SystemMaster where [BrCode]='" + Session["vUser_Branch"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtOrderNo.Text = dR[0].ToString();
                    }

                }
            }
        }
        private void loadPaymentType()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT PaymentMode FROM ModeOfPayment Where Status=1 ORDER BY PaymentID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddPayment.Items.Clear();
                    ddPayment.DataSource = dR;
                    ddPayment.DataValueField = "PaymentMode";
                    ddPayment.DataTextField = "PaymentMode";
                    ddPayment.DataBind();
                    //ddPayment.Items.Insert(0, new ListItem("Please select mode of payment", "0"));
                    //ddPayment.SelectedIndex = 1;
                }
            }
        }

        protected void ddPayment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddPayment.SelectedItem.Text == "Cash")
            {
                txtBatch.Text = string.Empty;
                txtReferenceNumber.Text = string.Empty;
                ddBanks.Items.Clear();
                ddBanks.Enabled = false;
                txtBatch.Enabled = false;
                txtReferenceNumber.Enabled = false;

                RFBanks.Enabled = false;
                rbBatch.Enabled = false;
                rfReferenceNumber.Enabled = false;



                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;

                //cvPayment.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "Credit Card")
            {
                ddBanks.Enabled = true;
                loadBanks(1);
                lblBatchVoucher.Text = "Batch #";
                lblRefNo.Text = "Aprroval No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;

                RFBanks.Enabled = true;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Enabled = true;
                txtCCName.Enabled = true;
                rftxt4Digit.Enabled = true;
                rftxtCCName.Enabled = true;

                //cvPayment.Enabled = true;
            }

            else if (ddPayment.SelectedItem.Text == "Debit Card")
            {
                ddBanks.Enabled = true;
                loadBanks(1);
                lblBatchVoucher.Text = "Slip #";
                lblRefNo.Text = "Ref. No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;

                RFBanks.Enabled = true;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;


                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }

            else if (ddPayment.SelectedItem.Text == "Paymaya")
            {

                ddBanks.Items.Clear();
                ddBanks.Enabled = false;
                lblBatchVoucher.Text = "Batch #";
                lblRefNo.Text = "Aprroval No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;

                RFBanks.Enabled = false;

                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;


                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "G Cash")
            {
                lblBatchVoucher.Text = "Batch #";
                txtBatch.Enabled = false;
                txtReferenceNumber.Enabled = true;
                RFBanks.Enabled = false;
                rbBatch.Enabled = false;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "Gift Cheque")
            {
                ddBanks.Items.Clear();
                lblBatchVoucher.Text = "Company";
                lblRefNo.Text = "GC No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;
                RFBanks.Enabled = false;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "Voucher")
            {
                ddBanks.Items.Clear();
                lblBatchVoucher.Text = "Voucher";
                lblRefNo.Text = "Ref. No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;
                RFBanks.Enabled = false;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;


                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "Bank/Fund Transfer")
            {
                ddBanks.Enabled = false;
                ddBanks.Items.Clear();
                lblBatchVoucher.Text = "Client Bank Name";
                lblRefNo.Text = "Ref. No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;
                RFBanks.Enabled = false;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;

                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;
            }
            else if (ddPayment.SelectedItem.Text == "Bank Cheque")
            {
                ddBanks.Items.Clear();
                lblBatchVoucher.Text = "Bank";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;
                RFBanks.Enabled = false;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "Others")
            {
                ddBanks.Items.Clear();
                ddBanks.Enabled = false;
                lblBatchVoucher.Text = "Others Info";
                txtReferenceNumber.Enabled = true;
                txtBatch.Enabled = true;
                //txtBatch.ReadOnly = false;

                RFBanks.Enabled = false;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }
            else if (ddPayment.SelectedItem.Text == "Digital Payment")
            {
                ddBanks.Enabled = true;
                loadBanks(2);
                lblBatchVoucher.Text = "Batch #";
                lblRefNo.Text = "Ref. No.";
                txtBatch.Enabled = false;
                txtReferenceNumber.Enabled = true;

                RFBanks.Enabled = true;
                rbBatch.Enabled = false;
                rfReferenceNumber.Enabled = true;

                txt4Digit.Text = string.Empty;
                txtCCName.Text = string.Empty;

                txt4Digit.Enabled = false;
                txtCCName.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;
            }

        }

        private void loadBanks(int tYpe)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT BankCode FROM Banks where Type=@Type and vStatus=1 ORDER BY BankID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@Type", tYpe);
                    SqlDataReader dR = cmD.ExecuteReader();
                    ddBanks.Items.Clear();
                    ddBanks.DataSource = dR;
                    ddBanks.DataValueField = "BankCode";
                    ddBanks.DataTextField = "BankCode";
                    ddBanks.DataBind();
                    ddBanks.Items.Insert(0, new ListItem("Please select", "0"));
                    ddBanks.SelectedIndex = 0;
                }
            }
        }

        private void loadDiscounts()
        {
            //DateTime dtSales = Convert.ToDateTime(txtDate.Text).ToShortDateString();
            //DateTime dtSales = DateTime.ParseExact(txtDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            //dtSales.ToString("yyyyMMdd");

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                //                string stR = @"SELECT sConstant,sDescription FROM tblTypeDiscount WHERE iValidUntil_dt >= CONVERT(CHAR(10),GETDATE(),112)
                //                    AND iValidFrom_dt <= CONVERT(CHAR(10),GETDATE(),112)
                //                     AND BrCode='" + Session["vUser_Branch"].ToString() + "' ORDER BY sDescription";

                string stR = @"dbo.LoadValidDiscounts";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    ddDiscounts.Items.Clear();
                    ddDiscounts.DataSource = dR;
                    ddDiscounts.DataValueField = "sConstant";
                    ddDiscounts.DataTextField = "sDescription";
                    ddDiscounts.DataBind();
                    ddDiscounts.Items.Insert(0, new ListItem("select discount", "0"));
                    ddDiscounts.SelectedIndex = 0;
                }
            }
        }
        private void loadSource()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT RecID,OrderSource FROM Source ORDER BY RecID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddSouce.Items.Clear();
                    ddSouce.DataSource = dR;
                    ddSouce.DataValueField = "RecID";
                    ddSouce.DataTextField = "OrderSource";
                    ddSouce.DataBind();
                    ddSouce.Items.Insert(0, new ListItem("select Source", "0"));
                    ddSouce.SelectedIndex = 0;
                }
            }
        }



        //private void loadCustomers()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"SELECT CustID,CustomerName FROM CustomerTable where BrCode=@BrCode ORDER BY CustomerName";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            cmD.CommandTimeout = 0;
        //            cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
        //            SqlDataReader dR = cmD.ExecuteReader();

        //            ddCustomerName.Items.Clear();
        //            ddCustomerName.DataSource = dR;
        //            ddCustomerName.DataValueField = "CustID";
        //            ddCustomerName.DataTextField = "CustomerName";
        //            ddCustomerName.DataBind();
        //            ddCustomerName.Items.Insert(0, new ListItem("Please select customer", "0"));
        //            ddCustomerName.SelectedIndex = 0;
        //        }
        //    }
        //}

        private void LoadtemMasterList()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadItemMasterListForSelling";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddITemFG.Items.Clear();
                    ddITemFG.DataSource = dR;
                    ddITemFG.DataValueField = "vFGCode";
                    ddITemFG.DataTextField = "vDESCRIPTION";
                    ddITemFG.DataBind();
                    ddITemFG.Items.Insert(0, new ListItem("Please select item", "0"));
                    ddITemFG.SelectedIndex = 0;
                }
            }
        }

        protected void ddITemFG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddITemFG.SelectedIndex == 0)
            {

                txtSRP.Text = "";
                txtItemID.Text = "";
                txtAvailable.Text = "";

                lblType.Text = "";
                lblNoSession.Text = "";
                lblIsKit.Text = "";
                lblWithInv.Text = "";

                ddConsultationType.DataSource = null;
                ddConsultationType.Items.Clear();
            }
            else
            {
                getFGinfo();
                //getBalance();
                if (ddITemFG.SelectedValue == "CON00112" | ddITemFG.SelectedValue == "CON00707" | ddITemFG.SelectedValue == "CON00842")
                {
                    loadConsultationType();
                    rvConsultation.Enabled = true;
                }
                else
                {
                    rvConsultation.Enabled = false;
                    ddConsultationType.DataSource = null;
                    ddConsultationType.Items.Clear();
                }
            }
            txtGCcode.Text = "";
            txtGCcode.ReadOnly = true;
            loadDiscounts();
            lblDiscPercentage.Text = string.Empty;
            lblDiscAmt.Text = string.Empty;
            lblVat.Text = string.Empty;
            lblFlag.Text = string.Empty;

            txtCardNo.Text = string.Empty;
        }

        private void loadConsultationType()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [ConsultationID]
                                  ,[ConsultaionType]
                              FROM [ConsultationType] ORDER BY ConsultaionType";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddConsultationType.Items.Clear();
                    ddConsultationType.DataSource = dR;
                    ddConsultationType.DataValueField = "ConsultationID";
                    ddConsultationType.DataTextField = "ConsultaionType";
                    ddConsultationType.DataBind();
                    ddConsultationType.Items.Insert(0, new ListItem("Select Type", "0"));
                    ddConsultationType.SelectedIndex = 0;
                }
            }
        }

        //private void getBalance()
        //{
        //    if (Session["vUser_Branch"] == null)
        //    {
        //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
        //        "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='LoginPage.aspx';", true);
        //    }
        //    else
        //    {
        //        using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //        {
        //            string stR = @"dbo.ShowItemBatchBalance";
        //            using (SqlCommand cmD = new SqlCommand(stR, Conn))
        //            {
        //                Conn.Open();
        //                cmD.CommandType = CommandType.StoredProcedure;
        //                cmD.CommandTimeout = 0;
        //                cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
        //                cmD.Parameters.AddWithValue("@vFGcode", ddITemFG.SelectedValue);
        //                DataTable dT = new DataTable();
        //                SqlDataAdapter dA = new SqlDataAdapter(cmD);
        //                dA.Fill(dT);

        //                if (dT.Rows.Count > 0)
        //                {
        //                    if (lblType.Text == "Service")
        //                    {
        //                        txtAvailable.Text = "";
        //                        txtSRP.ReadOnly = false;
        //                    }
        //                    else
        //                    {
        //                        txtAvailable.Text = dT.Rows[0]["Available Balance"].ToString();
        //                        txtItemID.Text = dT.Rows[0]["vItemID"].ToString();
        //                        txtSRP.ReadOnly = true;
        //                    }
        //                }
        //                else
        //                {
        //                    txtAvailable.Text = "0";
        //                    txtItemID.Text = "";
        //                }

        //            }
        //        }
        //    }
        //}

        private void getFGinfo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.GetItemInfo";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        Double theSRP = Convert.ToDouble(dR[0].ToString());
                        //txtSRP.Text =dR[0].ToString();
                        txtSRP.Text = theSRP.ToString();
                        lblType.Text = dR[1].ToString();
                        lblIsKit.Text = dR[3].ToString();
                        if (dR[1].ToString() == "Service")
                        {
                            txtSRP.ReadOnly = false;
                            lblNoSession.Visible = true;
                            lblNoSession.Text = dR[2].ToString();
                            txtNoSesson.ReadOnly = false;
                            txtNoSesson.Text = "1";
                            //LoadDoctors();
                            ReqAvail.Enabled = false;
                            cvQty.Enabled = false;
                            lblWithInv.Text = dR[4].ToString();
                            RequiredDoctors.Enabled = true;
                            txtAvailable.Text = "0";
                            txtItemID.Text = "0";
                            txtAvailable.ForeColor = System.Drawing.Color.Transparent;
                            //dposit
                            ckDeposit.Enabled = true;



                        }

                        else if (dR[1].ToString() == "Product" & dR[4].ToString() == "False")
                        {
                            //ddDrs.Items.Clear();
                            lblNoSession.Visible = false;
                            txtNoSesson.ReadOnly = true;
                            txtNoSesson.Text = "";
                            ReqAvail.Enabled = true;
                            cvQty.Enabled = false;
                            RequiredDoctors.Enabled = true;
                            lblWithInv.Text = dR[4].ToString();
                            txtAvailable.Text = dR[5].ToString();
                            txtItemID.Text = dR[6].ToString();
                            txtSRP.ReadOnly = true;
                            //dposit
                            ckDeposit.Enabled = false;
                            txtAmtDeposit.ReadOnly = true;
                            RequiredDeposit.Enabled = false;
                            txtAvailable.ForeColor = System.Drawing.Color.Transparent;
                        }
                        else
                        {
                            //ddDrs.Items.Clear();
                            RequiredDoctors.Enabled = true;
                            lblNoSession.Visible = false;
                            txtNoSesson.ReadOnly = true;
                            txtNoSesson.Text = "";
                            ReqAvail.Enabled = true;
                            cvQty.Enabled = true;
                            lblWithInv.Text = dR[4].ToString();
                            txtAvailable.Text = dR[5].ToString();
                            txtItemID.Text = dR[6].ToString();
                            txtSRP.ReadOnly = true;
                            txtAvailable.ForeColor = System.Drawing.Color.Black;

                        }

                    }

                }
            }
        }


        private void LoadDoctors()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT ID,EmployeeName FROM DoctosList ORDER BY EmployeeName";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddDrs.Items.Clear();
                    ddDrs.DataSource = dR;
                    ddDrs.DataValueField = "ID";
                    ddDrs.DataTextField = "EmployeeName";
                    ddDrs.DataBind();
                    ddDrs.Items.Insert(0, new ListItem("Please select staff", "0"));
                    ddDrs.SelectedIndex = 0;
                }
            }

        }




        private void CheckIfKitHAsBalance()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.CheckIfKitItemHasBalance";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@PromoCode", ddITemFG.SelectedValue);
                    cmD.Parameters.AddWithValue("@vQty", txtQty.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();

                    if (dR.HasRows)
                    {
                        while (dR.Read())
                        {
                            if (dR[5].ToString() != "0")
                            {
                                lblMsgWarning.Text = "Insufficient balance for Plu Code <b> " + dR[1].ToString() + "</b> in the inventory. \n Available balance = <b>" + dR[3].ToString() + "</b>";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            }
                            else
                            {
                                if (ddDiscounts.SelectedValue == "GCODEP20")
                                {
                                    CheckGCCodeIFValid();
                                }
                                else
                                {
                                    CheckIfReceiptNoExistsInUnpostedBeforeSaving();
                                    //
                                }

                            }
                        }
                    }
                    else
                    {
                        if (ddDiscounts.SelectedValue == "GCODEP20")
                        {
                            CheckGCCodeIFValid();
                        }
                        else
                        {
                            CheckIfReceiptNoExistsInUnpostedBeforeSaving();
                            //InserDetailRow();
                        }
                    }


                }
            }

        }



        private void CheckIfReceiptNoExistsInUnpostedBeforeSaving()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (Session["EmpNo"] == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                    "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='LoginPage.aspx';", true);
                }
                else
                {
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"SELECT ReceiptNo FROM UnpostedSalesDetailed WHERE ReceiptNo=@ReceiptNo";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@ReceiptNo", txtOrderNo.Text);
                            SqlDataReader dR = cmD.ExecuteReader();

                            if (dR.HasRows)
                            {
                                btnRefresh.Focus();
                                lblMsgWarning.Text = "Series number " + txtOrderNo.Text + " already used. Please click refresh button.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;


                            }
                            else
                            {
                                //20220504
                                CheckDate();
                                //InserDetailRow();

                            }


                        }


                    }
                }
            }

        }


        private void CheckDate()
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
                        // DateTime theSAlesDate
                        if (Convert.ToDateTime(dR[0].ToString()).ToShortDateString() == txtDate.Text)
                        {
                            InserDetailRow();
                        }
                        else
                        {
                            lblMsgWarning3.Text = "System Date not equal to SMS date. Need to sign in again.";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg3();", true);
                            return;
                        }
                    }

                }
            }
        }


        public decimal theTotalDiscAmount;

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (grdItemGrid.Rows.Count == 0)
                {

                    lblMsgWarning.Text = "Please generate data to post";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                if (Convert.ToDecimal(txtTotalAmount.Text) > 0 & grdPayment.Rows.Count == 0)
                {
                    lblMsgWarning.Text = "Please generate payment.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }



                else if (txtCustID.Text == string.Empty)
                {
                    txtCustomer.Focus();
                    lblMsgWarning.Text = "Please supply customer name";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                else if (ddStatus.SelectedValue == "0")
                {
                    ddStatus.Focus();
                    lblMsgWarning.Text = "Please supply customer type";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                else if (txtAROR.Text == string.Empty)
                {
                    txtAROR.Focus();
                    lblMsgWarning.Text = "Please supply AR#/OR#";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                else if (Convert.ToDecimal(xtlblAmounttobepaid.Text) > 0 & ckDeposit.Checked == false)
                {
                    txtAmtToPaid.Focus();
                    lblMsgWarning.Text = "Total amount render is greater than the total amount to be paid.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                else
                {
                    CheckIfReceiptNoExistsInUnposted();

                    //PostInsertUnpostedDetailedEntryNew();


                }
            }
        }

        private void CheckIfReceiptNoExistsInUnposted()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (Session["EmpNo"] == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "redirect script",
                    "alert('You been idle for a long period of time, Need to Sign in again!'); location.href='LoginPage.aspx';", true);
                }
                else
                {
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"SELECT ReceiptNo FROM UnpostedSalesDetailed WHERE ReceiptNo=@ReceiptNo and vStat=1";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@ReceiptNo", txtOrderNo.Text);
                            SqlDataReader dR = cmD.ExecuteReader();

                            if (dR.HasRows)
                            {
                                btnRefresh.Focus();
                                lblMsgWarning.Text = "Series number " + txtOrderNo.Text + " already used. Please click refresh button.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;


                            }
                            else
                            {
                                PostInsertUnpostedDetailedEntryNew();

                            }


                        }


                    }
                }
            }

        }

        private void UpdateReceiptNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET Series=Series + 1 WHERE BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlTransaction myTrans = conN.BeginTransaction();
                    cmD.Transaction = myTrans;
                    try
                    {
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.ExecuteNonQuery();
                        myTrans.Commit();
                    }
                    catch (Exception x)
                    {
                        myTrans.Rollback();
                        lblMsgWarning.Text = x.Message;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                }
            }

        }
        protected void btnConfirmSubmit_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    //PostUnpostedDetailedEntryNew();
                    //oRIG        PostUnpostedDetailedEntry();
                    PostInsertUnpostedDetailedEntryNew();
                    //lblMsgSuccess.Text = "Sales succesfully posted!";
                    //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                    //return;
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }

        }






        protected void ddDiscounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddDiscounts.SelectedValue == "0")
            {
                lblDiscPercentage.Text = string.Empty;
                lblVat.Text = string.Empty;
                lblFlag.Text = string.Empty;
                lblDiscAmt.Text = string.Empty;
                RequiredGCCode.Enabled = false;
                RequiredCardNo.Enabled = false;
            }
            else
            {
                txtGCcode.Text = "";
                txtGCcode.ReadOnly = true;
                RequiredGCCode.Enabled = false;
                RequiredCardNo.Enabled = false;

                getDiscountDetail();
            }


        }

        private void getDiscountDetail()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT iPercent,iSpecialPercent,iAllItems_fl,mDiscount_amt FROM tblTypeDiscount where BrCode=@BrCode AND sConstant=@sConstant";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@sConstant", ddDiscounts.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    while (dR.Read())
                    {
                        if (dR[2].ToString() == "1")
                        {
                            lblDiscPercentage.Text = dR[0].ToString();
                            lblVat.Text = dR[1].ToString();
                            lblFlag.Text = dR[2].ToString();
                            lblDiscAmt.Text = dR[3].ToString();
                        }
                        else
                        {
                            checkIfDiscountIsValid();
                        }
                    }


                }
            }


        }

        private void checkIfDiscountIsValid()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT Distinct [vFGCode]
                              FROM [tblTypeDiscountDetail] WHERE 
                              sConstant=@sConstant AND vFGCode=@vFGCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@sConstant", ddDiscounts.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {
                        getDiscountDetailPerConstant();
                    }
                    else
                    {
                        lblMsgWarning.Text = ddDiscounts.SelectedItem.Text + " discount is not allowed for this item";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                }
            }

        }

        private void getDiscountDetailPerConstant()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT iPercent,iSpecialPercent,iAllItems_fl,mDiscount_amt FROM tblTypeDiscount where BrCode=@BrCode AND sConstant=@sConstant";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@sConstant", ddDiscounts.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    while (dR.Read())
                    {
                        lblDiscPercentage.Text = dR[0].ToString();
                        lblVat.Text = dR[1].ToString();
                        lblFlag.Text = dR[2].ToString();
                        lblDiscAmt.Text = dR[3].ToString();
                    }

                }
            }

            //20240702
            if (ddDiscounts.SelectedValue == "GCODEP20")
            {
                txtGCcode.ReadOnly = false;
                RequiredGCCode.Enabled = true;
                RequiredCardNo.Enabled = false;
                txtCardNo.ReadOnly = true;
            }
            else if (ddDiscounts.SelectedValue == "SMACPDIS" | ddDiscounts.SelectedValue == "SMACSDIS" | ddDiscounts.SelectedValue == "SCD30031" | ddDiscounts.SelectedValue == "PWD30032")
            {
                txtGCcode.ReadOnly = true;
                txtCardNo.ReadOnly = false;
                RequiredGCCode.Enabled = false;
                RequiredCardNo.Enabled = true;
            }
            else
            {
                txtGCcode.ReadOnly = true;
                RequiredGCCode.Enabled = false;
                RequiredCardNo.Enabled = false;
                txtCardNo.ReadOnly = true;
            }

        }








        private void GetSessionBalance()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowCustomerSessionBalance";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@CustID", txtCustID.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        string thePerformedBy = dT.Rows[0]["PerformedBy"].ToString();

                        LoadPerformedBy(thePerformedBy);

                        gvSessionBalance.DataSource = dT;
                        gvSessionBalance.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridSessionBalance();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "No session balance Found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }

        }



        private void LoadPerformedBy(string thePerformedByDr)
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT ID,EmployeeName FROM DoctosList ORDER BY EmployeeName";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddPerformedBy.Items.Clear();
                    ddPerformedBy.DataSource = dR;
                    ddPerformedBy.DataValueField = "ID";
                    ddPerformedBy.DataTextField = "EmployeeName";
                    ddPerformedBy.DataBind();
                    ddPerformedBy.Items.Insert(0, new ListItem(thePerformedByDr.ToString(), "0"));
                    ddDrs.SelectedIndex = 0;
                }
            }

        }


        protected void btnShowSessionBalance_Click(object sender, EventArgs e)
        {
            ckNewDR.Checked = false;
            GetSessionBalance();
        }


        //protected void gvSessionBalance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{
        //    GridViewRow gvR = gvSessionBalance.Rows[e.RowIndex];
        //    TextBox txtSessionToAvailed = gvSessionBalance.Rows[e.RowIndex].FindControl("xtSessionToAvailed") as TextBox;
        //    //string cellvalue3 = ((Label)gvR.Cells[0].FindControl("lblTransactionID")).Text;
        //    string cellvalue4 = gvR.Cells[4].Text;
        //    string cellvalue6 = gvR.Cells[6].Text;
        //    string cellvalue5 = gvR.Cells[5].Text;
        //    string cellvalue0 = gvR.Cells[0].Text;
        //    int number = Convert.ToInt32(((TextBox)gvR.Cells[9].FindControl("xtSessionToAvailed")).Text);

        //    DataTable dt = (DataTable)ViewState["UnpostedDetail"];
        //    dt.Rows.Add(0,
        //   "1",
        //    "1",
        //    0,
        //    0,
        //    "1",
        //    number,
        //    0,
        //    "",
        //    0,
        //    0,
        //    0,
        //    0,
        //    "Service",
        //    0,
        //    0,
        //    "1",
        //    cellvalue0);
        //    ViewState["UnpostedDetail"] = dt;
        //    this.BindItemGrid();

        //    //DataTable dt = (DataTable)ViewState["UnpostedDetail"];
        //    //dt.Rows.Add(0,
        //    //gvR.Cells[3].Text,
        //    //gvR.Cells[4].Text,
        //    //0,
        //    //0,
        //    //gvR.Cells[6].Text,
        //    //txtSessionToAvailed.Text,
        //    //0,
        //    //"",
        //    //0,
        //    //0,
        //    //0,
        //    //0,
        //    //"Service",
        //    //0,
        //    //0,
        //    //gvR.Cells[5].Text,
        //    //gvR.Cells[0].Text);
        //    //ViewState["UnpostedDetail"] = dt;
        //    //this.BindItemGrid();


        //    //using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    //{
        //    //    string stR = @"DBO.AddNewSessionEntry";

        //    //    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
        //    //    {
        //    //        sqlConn.Open();
        //    //        cmD.CommandType = CommandType.StoredProcedure;
        //    //        cmD.Parameters.AddWithValue("@SalesDate", txtDate.Text);
        //    //        cmD.Parameters.AddWithValue("@ReceiptNo", "");
        //    //        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
        //    //        cmD.Parameters.AddWithValue("@TransactionID", gvR.Cells[0].Text);
        //    //        cmD.Parameters.AddWithValue("@vFGCode", gvR.Cells[3].Text);
        //    //        cmD.Parameters.AddWithValue("@ItemDescription", );
        //    //        cmD.Parameters.AddWithValue("@PerformedBy", gvR.Cells[5].Text);
        //    //        cmD.Parameters.AddWithValue("@TotalSession", );
        //    //        cmD.Parameters.AddWithValue("@NoSession", txtSessionToAvailed.Text); // );
        //    //        //cmD.Parameters.AddWithValue("@NoSession", gvR.Cells[9].Text);


        //    //        cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
        //    //        cmD.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
        //    //        cmD.ExecuteNonQuery();

        //    //    }
        //    //}



        //}

        protected void grdPayment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            GridViewRow row = grdPayment.Rows[e.RowIndex];
            Label lblTotalAmount = (Label)row.FindControl("lblTotalAmount");

            decimal theBalancePayment;
            decimal theTotalRenderPayment;
            if (txtTotalAmount.Text != string.Empty)
            {
                theBalancePayment = Convert.ToDecimal(lblTotalAmount.Text) + Convert.ToDecimal(txtAmtToPaid.Text);
                txtAmtToPaid.Text = theBalancePayment.ToString();
                xtlblAmounttobepaid.Text = theBalancePayment.ToString();

                theTotalRenderPayment = Convert.ToDecimal(txtTotalAmtRender.Text) - Convert.ToDecimal(lblTotalAmount.Text);
                txtTotalAmtRender.Text = theTotalRenderPayment.ToString();

                txtChange.Text = "0";
            }

            DataTable dt = ViewState["Payment"] as DataTable;
            dt.Rows[index].Delete();
            ViewState["Payment"] = dt;
            BindPaymentGrid();
        }

        decimal RunningTotal = 0;
        protected void grdPayment_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RunningTotal += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "TotalAmount"));
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                ((Label)e.Row.FindControl("lblTotalAmt")).Text = RunningTotal.ToString("N");
                txtTotalAmtRender.Text = RunningTotal.ToString("N");
                decimal theBalancePayment;
                if (txtTotalAmount.Text != string.Empty)
                {
                    theBalancePayment = Convert.ToDecimal(txtTotalAmount.Text) - Convert.ToDecimal(txtTotalAmtRender.Text);
                    txtAmtToPaid.Text = theBalancePayment.ToString();
                    xtlblAmounttobepaid.Text = theBalancePayment.ToString();
                }

            }

        }



        protected void BindPaymentGrid()
        {
            //try
            //{
            //DataTable dt = (DataTable)ViewState["Payment"];  
            grdPayment.DataSource = (DataTable)ViewState["Payment"];
            grdPayment.DataBind();

            //if (grdPayment.Rows.Count > 0)
            //{
            //    grdPayment.FooterRow.Cells[0].Text = "Total -> ";
            //    grdPayment.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;

            //    // double total =  dt.AsEnumerable().Sum(row => row.Field<double>("TotalAmount"));
            //    Double total = dt.AsEnumerable().Sum(row => row.Field<Double?>("TotalAmount") ?? 0);

            //   // decimal? total = dt.AsEnumerable().Sum(r => r.Field<decimal?>("TotalAmount"));
            //    grdPayment.FooterRow.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            //    grdPayment.FooterRow.Cells[1].Text = total.ToString();

            //}
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }


        string thePaymentType;
        string theBatch;
        string TheReference;
        protected void InsertPaymentRow(decimal theAmtChange)
        {
            try
            {
                if (ddPayment.SelectedItem.Text == "Cash")
                {
                    thePaymentType = string.Empty;
                    theBatch = string.Empty;
                    TheReference = string.Empty;
                }
                else if (ddPayment.SelectedItem.Text == "Gift Cheque" | (ddPayment.SelectedItem.Text == "Voucher") | ddPayment.SelectedItem.Text == "Bank/Fund Transfer")
                {
                    thePaymentType = txtBatch.Text;
                    theBatch = string.Empty;
                    TheReference = txtReferenceNumber.Text;
                }
                else if (ddPayment.SelectedItem.Text == "Paymaya")
                {
                    thePaymentType = string.Empty;
                    theBatch = txtBatch.Text;
                    TheReference = txtReferenceNumber.Text;
                }
                else
                {
                    thePaymentType = ddBanks.SelectedItem.Text;
                    theBatch = txtBatch.Text;
                    TheReference = txtReferenceNumber.Text;

                }

                decimal theAmountPaidLessChange = Convert.ToDecimal(txtAmtToPaid.Text) - theAmtChange;
                DataTable dt = (DataTable)ViewState["Payment"];
                dt.Rows.Add(ddPayment.SelectedItem.Text.Trim(),
                    theAmountPaidLessChange, //txtAmtToPaid.Text.Trim(),
                    thePaymentType.ToString(),
                    theBatch.ToString(),
                    TheReference.ToString(),
                    txt4Digit.Text,
                    txtCCName.Text);
                ViewState["Payment"] = dt;
                this.BindPaymentGrid();
                ClearFields();
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;

            }


            //if (ddBanks.Enabled == false)
            //{
            //    thePaymentType = string.Empty;
            //}
            //else
            //{
            //    if (ddPayment.SelectedItem.Text == "Gift Cheque" | (ddPayment.SelectedItem.Text == "Voucher") | ddPayment.SelectedItem.Text == "Others")
            //        {
            //            thePaymentType = txtBatch.Text;
            //        }
            //    else
            //    {
            //            if (string.IsNullOrEmpty(ddBanks.SelectedItem.Text))
            //            {
            //                thePaymentType = string.Empty;
            //            }
            //            else
            //            {


            //                    thePaymentType = ddBanks.SelectedItem.Text.Trim();

            //            }
            //    }
            //}


            //if (ddPayment.SelectedItem.Text == "Gift Cheque" | (ddPayment.SelectedItem.Text == "Voucher") | ddPayment.SelectedItem.Text == "Others")
            //{
            //    theBatch = string.Empty;
            //}
            //else
            //{
            //    if (txtBatch.Text == string.Empty)
            //    {
            //        theBatch = string.Empty;
            //    }
            //    else
            //    {
            //        theBatch = txtBatch.Text;
            //    }
            //}


            //if (txtReferenceNumber.Text == string.Empty)
            //{
            //    TheReference = string.Empty;
            //}
            //else
            //{
            //    TheReference = txtReferenceNumber.Text;
            //}




            //try
            //{

            //Double AmtRender;
            //Double TotalAmtRender;
            //AmtRender = Convert.ToDouble(txtAmtToPaid.Text);


            //TotalAmtRender =+ AmtRender;
            //txtTotalAmtRender.Text =Convert.ToString(TotalAmtRender);

            // txtTotalAmtRender.Text =Convert.ToString(TotalAmtRender + TotalAmtRender); 

            //decimal theBalancePayment;
            //if (txtTotalAmount.Text != string.Empty)
            //{
            //    theBalancePayment = Convert.ToDecimal(txtTotalAmount.Text) - Convert.ToDecimal(txtTotalAmtRender.Text);
            //    txtAmtToPaid.Text = theBalancePayment.ToString();
            //    xtlblAmounttobepaid.Text = theBalancePayment.ToString();
            //}


            //}
            //catch (Exception ex)
            //{

            //    throw;
            //}
        }

        protected void ClearFields()
        {
            try
            {
                ddPayment.Items.Clear();
                loadPaymentType();

                //ddBanks.Items.Clear();
                //loadBanks();

                txtBatch.Text = string.Empty;
                txtReferenceNumber.Text = string.Empty;
                ddBanks.Items.Clear();
                ddBanks.Enabled = false;
                txtBatch.Enabled = false;
                txtReferenceNumber.Enabled = false;


                txtCCName.Text = string.Empty;
                txt4Digit.Text = string.Empty;


                txtCCName.Enabled = false;
                txt4Digit.Enabled = false;
                rftxt4Digit.Enabled = false;
                rftxtCCName.Enabled = false;

                RFBanks.Enabled = false;
                rbBatch.Enabled = false;
                rfReferenceNumber.Enabled = false;



            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        protected void btnSavePayment_Click(object sender, EventArgs e)
        {

            if (xtlblAmounttobepaid.Text == "0")
            {
                lblMsgWarning.Text = "Total payment already exceed.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

            else
            {
                if (ddPayment.SelectedItem.Text == "Cash" & Convert.ToDecimal(txtAmtToPaid.Text) > Convert.ToDecimal(xtlblAmounttobepaid.Text))
                {
                    decimal theChange = Convert.ToDecimal(txtAmtToPaid.Text) - Convert.ToDecimal(xtlblAmounttobepaid.Text);
                    txtChange.Text = theChange.ToString();
                    InsertPaymentRow(theChange);
                }
                else if (ddPayment.SelectedItem.Text != "Cash" & Convert.ToDecimal(txtAmtToPaid.Text) > Convert.ToDecimal(xtlblAmounttobepaid.Text))
                {
                    lblMsgWarning.Text = "Total payment exceed.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    decimal theChange = 0;
                    InsertPaymentRow(theChange);
                }
            }

        }



        protected void ckDeposit_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDeposit.Checked == true)
            {
                //ddDrs.DataSource = null;
                //ddDrs.Enabled = false;
                //RequiredDoctors.Enabled = false;

                ddDiscounts.Enabled = false;

                txtNoSesson.Text = "0";
                txtAmtDeposit.ReadOnly = false;
                RequiredDeposit.Enabled = true;

            }
            else
            {
                RequiredDeposit.Enabled = false;
                txtAmtDeposit.Text = string.Empty;
                txtAmtDeposit.ReadOnly = true;
                ddDiscounts.Enabled = true;
                loadDiscounts();
                //ddDrs.Enabled = true;
                RequiredDoctors.Enabled = true;
                LoadDoctors();
            }
        }








        //################################START########################################################



        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //CheckGCCodeIFValid();
                //start
                try
                {

                    if (ddITemFG.SelectedValue == "0")
                    {
                        ddITemFG.Focus();
                        lblMsgWarning.Text = "Please select item!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    else if (lblType.Text == "Service" & txtNoSesson.Text == string.Empty)
                    {

                        txtNoSesson.Focus();
                        lblMsgWarning.Text = "Please supply no of session.  Put 0 if will not avail session";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                    else if (lblType.Text == "Service" & ddDrs.SelectedValue == "0" & ckDeposit.Checked == false)
                    {

                        ddDrs.Focus();
                        lblMsgWarning.Text = "Please select staff!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    else if (lblIsKit.Text == "True")
                    {

                        CheckIfKitHAsBalance();
                    }

                    else if (ddDiscounts.SelectedValue == "SCD30031" & Convert.ToInt32(txtAge.Text) < 60)
                    {

                        lblMsgWarning.Text = txtAge.Text +  " years old is not valid to avail Señor Citizen Discount, Please update Customer Info.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                    else if (grdPayment.Rows.Count > 0)
                    {
                        lblMsgWarning.Text = "Please removed payment breakdown first before adding another item.";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }




                    else if (ckDeposit.Checked == true)
                    {
                        if (grdItemGrid.Rows.Count > 0)
                        {
                            lblMsgWarning.Text = "Only one item is allowed for deposit transaction";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else if ((Convert.ToDecimal(txtSRP.Text) * Convert.ToDecimal(txtQty.Text)) < Convert.ToDecimal(txtAmtDeposit.Text))
                        {
                            txtAmtDeposit.Focus();
                            lblMsgWarning.Text = "Amount deposit greater than total amount availed is invaild!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            if (ddDiscounts.SelectedValue == "GCODEP20")
                            {
                                CheckGCCodeIFValid();
                            }
                            else
                            {
                                CheckIfReceiptNoExistsInUnpostedBeforeSaving();

                            }
                        }
                    }

                    else
                    {
                        if (ddDiscounts.SelectedValue == "GCODEP20")
                        {
                            CheckGCCodeIFValid();
                        }
                        else
                        {
                            CheckIfReceiptNoExistsInUnpostedBeforeSaving();
                            //InserDetailRow();
                        }

                    }
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                //end

            }
        }


        protected void BindItemGrid()
        {
            try
            {
                //DataTable dt = (DataTable)ViewState["Payment"];  
                grdItemGrid.DataSource = (DataTable)ViewState["UnpostedDetail"];
                grdItemGrid.DataBind();

            }
            catch (Exception ex)
            {
                lblMsgWarning.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        public int TheTotalSession;
        public string NoSession;
        public string PerformedBy;


        public string sConstant;
        public string DiscDescription;
        public decimal vDiscPerc;
        public decimal DiscountsAmt;

        public decimal theNet;
        public decimal NetAmount;
        public decimal VatExemption;

        public int IsDeposit;

        public string theDoctor;

        private void InserDetailRow()
        {
            if (lblType.Text == "Product")
            {

                TheTotalSession = 0;
                NoSession = "0";
                PerformedBy = string.Empty;
            }
            else
            {

                int TotalSession = Convert.ToInt32(lblNoSession.Text) * Convert.ToInt32(txtQty.Text);

                TheTotalSession = TotalSession;
                NoSession = txtNoSesson.Text;
                //if (NoSession != "0")
                //{
                PerformedBy = ddDrs.SelectedItem.Text;
                //}
                //else
                //{
                //    PerformedBy = ddDrs.SelectedItem.Text;
                //}

            }



            if (ckDeposit.Checked == true)
            {
                IsDeposit = 1;

                sConstant = string.Empty;
                DiscDescription = string.Empty;
                vDiscPerc = 0;
                DiscountsAmt = 0;

                NetAmount = Convert.ToDecimal(txtAmtDeposit.Text);
                VatExemption = 0;
            }
            else
            {
                IsDeposit = 0;

                if (ddDiscounts.SelectedValue == "0")
                {
                    sConstant = string.Empty;
                    DiscDescription = string.Empty;
                    vDiscPerc = 0;
                    DiscountsAmt = 0;

                    theNet = Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text);
                    NetAmount = theNet;
                    VatExemption = 0;

                }
                else
                {
                    sConstant = ddDiscounts.SelectedValue;
                    if (ddDiscounts.SelectedValue == "GCODEP20")
                    {
                        DiscDescription = ddDiscounts.SelectedItem.Text + '-' + txtGCcode.Text.ToUpper();
                    }
                    else if (ddDiscounts.SelectedValue == "SMACPDIS" | ddDiscounts.SelectedValue == "SMACSDIS" | ddDiscounts.SelectedValue == "SCD30031" | ddDiscounts.SelectedValue == "PWD30032")
                    {
                        DiscDescription = ddDiscounts.SelectedItem.Text + '-' + txtCardNo.Text.ToUpper();
                    }
                    else
                    {
                        DiscDescription = ddDiscounts.SelectedItem.Text;
                    }

                    if (ddDiscounts.SelectedValue != "0")
                    {
                        //start 1128
                        if ((Convert.ToInt32(lblDiscPercentage.Text) != 0))
                        {
                            if (Convert.ToInt32(lblVat.Text) == 0)
                            {

                                vDiscPerc = Convert.ToDecimal(lblDiscPercentage.Text);
                                decimal theDiscAmount = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) * (Convert.ToDecimal(lblDiscPercentage.Text) / 100);
                                theTotalDiscAmount = theDiscAmount;
                                DiscountsAmt = theTotalDiscAmount;

                                decimal theNet = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) - theTotalDiscAmount;
                                NetAmount = theNet;
                                VatExemption = 0;
                            }

                            else if (Convert.ToInt32(lblVat.Text) > 0)
                            {
                                vDiscPerc = Convert.ToDecimal(lblDiscPercentage.Text);

                                decimal theTotalPrice = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text));
                                decimal theDiscAmount = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) * (Convert.ToDecimal(lblDiscPercentage.Text) / 100);
                                decimal theVat = 100 / Convert.ToDecimal(lblVat.Text);

                                theTotalDiscAmount = ((Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) - theDiscAmount) * theVat;
                                decimal theNet = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) - theTotalDiscAmount;

                                DiscountsAmt = theNet;


                                NetAmount = theTotalDiscAmount;
                                VatExemption = (theTotalPrice - theDiscAmount) - theTotalDiscAmount;

                            }
                        }
                        else if ((Convert.ToInt32(lblDiscPercentage.Text) == 0))
                        {

                            // 20230514 start
                            if (Convert.ToInt32(lblVat.Text) > 0)
                            {

                                vDiscPerc = Convert.ToDecimal(lblDiscPercentage.Text);

                                decimal theTotalPrice = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text));
                                decimal theDiscAmount = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) * (Convert.ToDecimal(lblDiscPercentage.Text) / 100);
                                decimal theVat = 100 / Convert.ToDecimal(lblVat.Text);

                                theTotalDiscAmount = ((Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) - theDiscAmount) * theVat;
                                decimal theNet = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) - theTotalDiscAmount;

                                DiscountsAmt = theNet;


                                NetAmount = theTotalDiscAmount;
                                VatExemption = (theTotalPrice - theDiscAmount) - theTotalDiscAmount;
                            }
                            // 20230514 end
                            else
                            {
                                decimal theDiscAmount = Convert.ToDecimal(lblDiscAmt.Text);
                                theTotalDiscAmount = theDiscAmount;
                                DiscountsAmt = theTotalDiscAmount;

                                decimal theNet = (Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text)) - theTotalDiscAmount;
                                NetAmount = theNet;
                                VatExemption = 0;
                            }
                        }
                        //end 1128
                    }
                }
            }




            if (lblType.Text == "Product" & ddDrs.SelectedItem.Text=="Please select staff")
            {
                theDoctor = string.Empty;
            }
            else
            {
                if (ckDeposit.Checked == true)
                {
                    if (ddDrs.SelectedValue == "0")
                    {
                        theDoctor = string.Empty;
                    }
                    else
                    {
                        theDoctor = ddDrs.SelectedItem.Text;
                    }
                }
                else
                {
                    //if (NoSession != "0")
                    //{
                    theDoctor = ddDrs.SelectedItem.Text;
                    //}
                    //else
                    //{
                    //    theDoctor = string.Empty;
                    //}


                }
            }

            //if (ddConsultationType.Enabled == true)
            //{
            if (ddITemFG.SelectedValue == "CON00112" | ddITemFG.SelectedValue == "CON00707" | ddITemFG.SelectedValue == "CON00842")
            {
                saveConsultationType();
            }
            //}

            DataTable dt = (DataTable)ViewState["UnpostedDetail"];
            dt.Rows.Add(txtItemID.Text,
            ddITemFG.SelectedValue,
            ddITemFG.SelectedItem.Text,
            txtSRP.Text,
            txtQty.Text,
            TheTotalSession,
            NoSession,
            ddDiscounts.SelectedValue,
            DiscDescription,
            DiscountsAmt.ToString("0.##"),
            VatExemption.ToString("0.##"),
            vDiscPerc,
            NetAmount.ToString("0.##"),
            lblType.Text,
            lblIsKit.Text,
            IsDeposit,
            theDoctor.ToString(),
            txtOrderNo.Text,
            0);
            ViewState["UnpostedDetail"] = dt;
            this.BindItemGrid();


            txtQty.Text = string.Empty;
            txtAmtDeposit.ReadOnly = true;
            txtGCcode.Text = string.Empty;
            txtCardNo.Text = string.Empty;
            //txtAmtDeposit.Text = string.Empty;
            //ckDeposit.Checked = false;

        }


        //################################END########################################################

        private void saveConsultationType()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {


                    string stR = @"dbo.AddConsultationRecord";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@ReceiptNo", txtOrderNo.Text);
                        cmD.Parameters.AddWithValue("@ConsultaionType", ddConsultationType.SelectedItem.Text);
                        cmD.ExecuteNonQuery();

                    }
                }
            }
        }
        private void PostInsertUnpostedDetailedEntryNew()
        {
            foreach (GridViewRow row in grdItemGrid.Rows)
            {
                Label ThelblNetAmount = (Label)row.FindControl("lblNetAmount");
                Label ThelblvUnitCost = (Label)row.FindControl("lblvUnitCost");
                Label ThelblDiscountsAmt = (Label)row.FindControl("lblDiscountsAmt");
                Label ThelblVatExemption = (Label)row.FindControl("lblVatExemption");


                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {


                    string stR = @"dbo.SaveUnpostedDT";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        //try
                        //{
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;

                        cmD.Parameters.AddWithValue("@SalesDate", txtDate.Text);
                        cmD.Parameters.AddWithValue("@CustID", txtCustID.Text);
                        cmD.Parameters.AddWithValue("@CustomerName", txtCustomer.Text.ToUpper());
                        if (ddSouce.SelectedValue == "0")
                        {
                            cmD.Parameters.AddWithValue("@OrderSource", string.Empty);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@OrderSource", ddSouce.SelectedItem.Text);
                        }
                        cmD.Parameters.AddWithValue("@PatientType", ddStatus.SelectedItem.Text);

                        cmD.Parameters.AddWithValue("@ReceiptNo", txtOrderNo.Text);
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@vItemID", row.Cells[0].Text);
                        cmD.Parameters.AddWithValue("@vFGCode", row.Cells[1].Text);
                        cmD.Parameters.AddWithValue("@ItemDescription", Server.HtmlDecode(row.Cells[2].Text));
                        cmD.Parameters.AddWithValue("@vUnitCost", ThelblvUnitCost.Text);
                        cmD.Parameters.AddWithValue("@vQty", row.Cells[4].Text);
                        cmD.Parameters.AddWithValue("@TotalSession", row.Cells[5].Text);
                        cmD.Parameters.AddWithValue("@NoSession", row.Cells[6].Text);
                        cmD.Parameters.AddWithValue("@sConstant", row.Cells[7].Text);
                        cmD.Parameters.AddWithValue("@DiscDescription", Server.HtmlDecode(row.Cells[8].Text));
                        cmD.Parameters.AddWithValue("@DiscountsAmt", ThelblDiscountsAmt.Text);
                        cmD.Parameters.AddWithValue("@VatExemption", ThelblVatExemption.Text);
                        cmD.Parameters.AddWithValue("@vDiscPerc", row.Cells[11].Text);
                        cmD.Parameters.AddWithValue("@NetAmount", ThelblNetAmount.Text);
                        cmD.Parameters.AddWithValue("@ItemType", row.Cells[13].Text);
                        cmD.Parameters.AddWithValue("@IsKit", row.Cells[14].Text);
                        cmD.Parameters.AddWithValue("@IsDeposit", row.Cells[15].Text);
                        cmD.Parameters.AddWithValue("@PerformedBy", Server.HtmlDecode(row.Cells[16].Text));
                        cmD.Parameters.AddWithValue("@vStat", 1);
                        cmD.Parameters.AddWithValue("@TransactionID", row.Cells[17].Text);
                        cmD.Parameters.AddWithValue("@IsDepositPaid", row.Cells[18].Text);
                        cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                        cmD.Parameters.AddWithValue("@ORNo", txtManualOR.Text);
                        cmD.ExecuteNonQuery();



                    }
                }
            }


            if (grdPayment.Rows.Count > 0)
            {
                foreach (GridViewRow rowPay in grdPayment.Rows)
                {
                    Label ThelblTotalAmount = (Label)rowPay.FindControl("lblTotalAmount");

                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {


                        string stR = @"dbo.PostPaymentBreakDown";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@ReceiptNo", txtOrderNo.Text);
                            cmD.Parameters.AddWithValue("@ORNo", txtManualOR.Text);
                            cmD.Parameters.AddWithValue("@SalesDate", txtDate.Text);
                            cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                            cmD.Parameters.AddWithValue("@PaymentMode", rowPay.Cells[0].Text);
                            cmD.Parameters.AddWithValue("@TotalAmount", ThelblTotalAmount.Text);
                            cmD.Parameters.AddWithValue("@BankName", Server.HtmlDecode(rowPay.Cells[2].Text));
                            cmD.Parameters.AddWithValue("@BatchNumber", Server.HtmlDecode(rowPay.Cells[3].Text));
                            cmD.Parameters.AddWithValue("@ReferenceNumber", Server.HtmlDecode(rowPay.Cells[4].Text));

                            cmD.Parameters.AddWithValue("@CC4Digit", Server.HtmlDecode(rowPay.Cells[5].Text));
                            cmD.Parameters.AddWithValue("@CCName", Server.HtmlDecode(rowPay.Cells[6].Text));



                            cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                            cmD.Parameters.AddWithValue("@ARORNo", txtAROR.Text);
                            cmD.ExecuteNonQuery();

                        }
                    }
                }

                saveAmtChange();
            }

            lblSeriesNo.Text = txtOrderNo.Text;
            UpdateReceiptNo();

            loadReceiptNo();
            LoadtemMasterList();
            loadDiscounts();
            loadSource();
            ddStatus.SelectedValue = "0";
            loadPaymentType();
            LoadDoctors();

            lblType.Text = string.Empty;
            lblNoSession.Text = string.Empty;
            lblIsKit.Text = string.Empty;
            lblWithInv.Text = string.Empty;

            txtAvailable.Text = string.Empty;
            txtItemID.Text = string.Empty;
            txtSRP.Text = string.Empty;
            txtNoSesson.Text = string.Empty;
            txtQty.Text = string.Empty;

            lblVat.Text = string.Empty;
            lblFlag.Text = string.Empty;
            lblDiscPercentage.Text = string.Empty;
            lblDiscAmt.Text = string.Empty;

            txtTotalAmount.Text = string.Empty;
            txtAmtToPaid.Text = string.Empty;
            xtlblAmounttobepaid.Text = string.Empty;
            txtTotalAmtRender.Text = string.Empty;
            txtManualOR.Text = string.Empty;


            txt4Digit.Text = string.Empty;
            txtCCName.Text = string.Empty;

            txtChange.Text = string.Empty;

            txtCustID.Text = string.Empty;
            txtCustomer.Text = string.Empty;

            txtAROR.Text = string.Empty;

            if (ckDeposit.Checked == true)
            {
                ckDeposit.Checked = false;
                RequiredDeposit.Enabled = false;
                txtAmtDeposit.Text = string.Empty;
                txtAmtDeposit.ReadOnly = true;
                ddDrs.Enabled = true;
            }
            //You have to bind the grid with new data table.

            DataTable dtDetail = new DataTable();
            dtDetail.Columns.AddRange(new DataColumn[19] 
                            {  new DataColumn("vItemID"), 
                                new DataColumn("vFGCode"), 
                                new DataColumn("ItemDescription"), 
                                new DataColumn("vUnitCost"),
                                new DataColumn("vQty"),
                                new DataColumn("TotalSession"),
                                new DataColumn("NoSession"),
                                new DataColumn("sConstant"),
                                new DataColumn("DiscDescription"),
                                new DataColumn("DiscountsAmt"),
                                new DataColumn("VatExemption"),
                                new DataColumn("vDiscPerc"),
                                new DataColumn("NetAmount"),
                                new DataColumn("ItemType"),
                                new DataColumn("IsKit"),
                                new DataColumn("IsDeposit"),
                                new DataColumn("PerformedBy"),
                                new DataColumn("TransactionID"),
                                new DataColumn("IsDepositPaid")
                                
                            });
            ViewState["UnpostedDetail"] = dtDetail;
            this.BindItemGrid();


            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[7] 
                            {  new DataColumn("PaymentMode"), 
                                new DataColumn("TotalAmount"), 
                                new DataColumn("BankName"), 
                                new DataColumn("BatchNumber"), 
                                new DataColumn("ReferenceNumber"),
                                new DataColumn("CCNumber"),
                                new DataColumn("CCname")});
            ViewState["Payment"] = dt;
            this.BindPaymentGrid();

            //grdItemGrid.DataSource = null;
            //grdItemGrid.DataBind();

            //grdPayment.DataSource = null;
            //grdPayment.DataBind();



            //20220321
            //lblMsgSuccessPosting.Text = "Sales succesfully posted!";
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsgPosting();", true);
            //return;

            lblMsgSuccess.Text = "Sales succesfully posted!";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
            return;


        }


        private void saveAmtChange()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {


                string stR = @"INSERT INTO [dbo].[TransactionChange]
				                   ([BrCode]
				                   ,[ReceiptNo]
				                   ,[ChangeAmt])
			                 VALUES
				                   (@BrCode
				                   ,@ReceiptNo
				                   ,@ChangeAmt)";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@ReceiptNo", txtOrderNo.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@ChangeAmt", txtChange.Text);
                    cmD.ExecuteNonQuery();

                }
            }


        }

        decimal RunningTotalItem = 0;
        protected void grdItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RunningTotalItem += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "NetAmount"));
                txtTotalAmount.Text = RunningTotalItem.ToString("0.##");
                xtlblAmounttobepaid.Text = RunningTotalItem.ToString("0.##");
                txtAmtToPaid.Text = RunningTotalItem.ToString("0.##");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                ((Label)e.Row.FindControl("lblNetAmountDetail")).Text = RunningTotalItem.ToString("N");

                // xtlblAmounttobepaid.Text = RunningTotalItem.ToString("N");

                //decimal theBalancePayment;
                //if (txtTotalAmount.Text != string.Empty)
                //{
                //    theBalancePayment = Convert.ToDecimal(txtTotalAmount.Text) - Convert.ToDecimal(txtTotalAmtRender.Text);
                //    txtAmtToPaid.Text = theBalancePayment.ToString();
                //    xtlblAmounttobepaid.Text = theBalancePayment.ToString();
                //}

            }
        }

        //decimal TheNewTotal;
        protected void grdItemGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int index = Convert.ToInt32(e.RowIndex);

            GridViewRow row = grdItemGrid.Rows[e.RowIndex];
            Label lblNetAmount = (Label)row.FindControl("lblNetAmount");


            decimal theBalancePayment2;
            if (txtTotalAmount.Text != string.Empty | Convert.ToDecimal(txtTotalAmount.Text) > 0)
            {
                theBalancePayment2 = Convert.ToDecimal(lblNetAmount.Text) - Convert.ToDecimal(txtAmtToPaid.Text);
                txtAmtToPaid.Text = theBalancePayment2.ToString();
                txtTotalAmount.Text = theBalancePayment2.ToString();
                xtlblAmounttobepaid.Text = theBalancePayment2.ToString();



            }



            DataTable dtDetail = ViewState["UnpostedDetail"] as DataTable;
            dtDetail.Rows[index].Delete();
            ViewState["UnpostedDetail"] = dtDetail;
            this.BindItemGrid();

        }


        protected void btnSearchBranch_Click(object sender, EventArgs e)
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [BrCode]
	                          ,[CustID]
                               ,[CustomerName]
                               ,CASE WHEN DateOfBirth = '01/01/1900' THEN 0
							  ELSE
							  ISNULL((CONVERT(int,CONVERT(char(10),GETDATE(),112))-CONVERT(char(10),DateOfBirth,112))/10000,0) 
							  END AS AgeIntYears
                          FROM [CustomerTable]
                          where CustomerName like '" + Server.HtmlDecode(txtCustomer.Text) + "%' ORDER BY [CustomerName]";

                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    //cmD.Parameters.AddWithValue("@CustID", ddCustomerName.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {

                        gvCustomerList.DataSource = dT;
                        gvCustomerList.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridCustomer();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "Customer name does not exists!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }

            //protected void ddITemFG_SelectedIndexChanged(object sender, EventArgs e)
            //{
            //    gvCustomer.DataSource = null;
            //    gvCustomer.DataBind();
            //}

        }

        protected void gvCustomerList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvR = gvCustomerList.Rows[e.RowIndex];

            txtCustID.Text = gvR.Cells[1].Text;
            txtCustomer.Text = Server.HtmlDecode(gvR.Cells[2].Text);
            txtAge.Text = gvR.Cells[3].Text;



            GetDepositBalance();

            //0216
            //ckNewDR.Checked = false;
            //GetSessionBalanceAuto();


        }

        public string SessionToAvailed;
        public string theFollowUpPerformedBy;
        protected void gvSessionBalance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectSession")
            {

                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                TextBox txtSessionToAvailed = (TextBox)gvr.FindControl("xtSessionToAvailed");

                if (gvr.Cells[5].Text == "&#160;" & ddPerformedBy.Text == "0" & ckNewDR.Checked == false)
                {

                    lblMsgWarning2.Text = "Please check the checkbox and choose on the dropdown list the Doctor/Staff's name";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg2();", true);
                    return;


                }
                else if (gvr.Cells[5].Text == "&#160;" & ddPerformedBy.Text == "0" & ckNewDR.Checked == true)
                {

                    lblMsgWarning2.Text = "Please check the checkbox and choose on the dropdown list the Doctor/Staff's name";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg2();", true);
                    return;


                }
                else
                {

                    if (!string.IsNullOrWhiteSpace(txtSessionToAvailed.Text))
                    {

                        SessionToAvailed = txtSessionToAvailed.Text;
                    }
                    else
                    {
                        SessionToAvailed = "1";
                    }
                    //cell5
                    int RowIndex = gvr.RowIndex;
                    string cell0 = gvr.Cells[0].Text;
                    string cell3 = gvr.Cells[3].Text;
                    string cell4 = gvr.Cells[4].Text;
                    string cell6 = gvr.Cells[6].Text;
                    string cell5 = gvr.Cells[5].Text;

                    if (ckNewDR.Checked == true)
                    {
                        theFollowUpPerformedBy = ddPerformedBy.SelectedItem.Text;

                    }
                    else
                    {
                        theFollowUpPerformedBy = cell5.ToString();
                    }

                    DataTable dt = (DataTable)ViewState["UnpostedDetail"];
                    dt.Rows.Add(0,
                    cell3,
                    cell4,
                    0,
                    0,
                    cell6,
                    SessionToAvailed.ToString(),
                    0,
                    "",
                    0,
                    0,
                    0,
                    0,
                    "Service",
                    0,
                    0,
                    theFollowUpPerformedBy.ToString(),
                    cell0,
                    0);
                    ViewState["UnpostedDetail"] = dt;
                    this.BindItemGrid();
                }

            }
        }


        private void GetSessionBalanceAuto()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowCustomerSessionBalance";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@CustID", txtCustID.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        string thePerformedBy = dT.Rows[0]["PerformedBy"].ToString();

                        LoadPerformedBy(thePerformedBy);

                        gvSessionBalance.DataSource = dT;
                        gvSessionBalance.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridSessionBalance();", true);
                        return;
                    }

                }
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            loadDiscounts();
            lblVat.Text = string.Empty;
            lblDiscPercentage.Text = string.Empty;
            lblFlag.Text = string.Empty;
            lblDiscAmt.Text = string.Empty;
        }


        private void GetDepositBalance()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT TOP (1) A.[SalesID]
                                      ,A.[BrCode]
	                                  ,B.BrName
                                      ,A.[ReceiptNo]
                                      ,A.[SalesDate]
                                      ,A.[CustID]
	                                  ,D.CustomerName
                                      ,A.[vFGCode]
	                                  ,RTRIM(C.vPluCode) + ' - ' +	 C.vDESCRIPTION as [ItemDescription]
	                                  ,C.vDESCRIPTION
                                      ,A.[TotalSession]
                                      ,A.NoSession
                                      ,A.[ItemAmount]
                                      ,A.[DepAmount]
                                      ,A.[DueAmount]
                                  FROM [DepositTransaction] A
                                  LEFT JOIN MyBranchList B
                                  ON A.BrCode=B.BrCode
                                  LEFT JOIN vItemMaster C
                                  ON A.[vFGCode]=C.[vFGCode]
                                  LEFT JOIN CustomerTable D
                                  ON A.CustID=D.CustID
                                  where A.[CustID]=@CustID
                                  AND A.PaidReceiptNo IS NULL";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    //cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@CustID", txtCustID.Text);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {

                        lblxCustID.Text = dT.Rows[0]["CustID"].ToString();
                        lblxBrCode.Text = dT.Rows[0]["BrCode"].ToString();
                        lblxBrName.Text = dT.Rows[0]["BrName"].ToString();
                        lblxCustomerName.Text = dT.Rows[0]["CustomerName"].ToString();
                        lblxDepAmount.Text = Convert.ToDecimal(dT.Rows[0]["DepAmount"]).ToString("N");
                        lblxDueAmount.Text = Convert.ToDecimal(dT.Rows[0]["DueAmount"]).ToString("N");
                        lblxFGCode.Text = dT.Rows[0]["vFGCode"].ToString();
                        lblxItemAmount.Text = Convert.ToDecimal(dT.Rows[0]["ItemAmount"]).ToString("N");
                        lblxItemDescription.Text = dT.Rows[0]["ItemDescription"].ToString();
                        lblxReceiptNo.Text = dT.Rows[0]["ReceiptNo"].ToString();
                        lblxSalesDate.Text = Convert.ToDateTime(dT.Rows[0]["SalesDate"]).ToShortDateString();
                        lblxTotalSession.Text = dT.Rows[0]["TotalSession"].ToString();
                        lblxSessionAvailed.Text = dT.Rows[0]["NoSession"].ToString();
                        LoadPerformedX();
                        loadDiscountsX();

                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridDepositBalance();", true);
                        return;
                    }
                    else
                    {
                        ckNewDR.Checked = false;
                        GetSessionBalanceAuto();
                    }

                }
            }

        }


        private void LoadPerformedX()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT ID,EmployeeName FROM DoctosList ORDER BY EmployeeName";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddxDoctor.Items.Clear();
                    ddxDoctor.DataSource = dR;
                    ddxDoctor.DataValueField = "ID";
                    ddxDoctor.DataTextField = "EmployeeName";
                    ddxDoctor.DataBind();
                    ddxDoctor.Items.Insert(0, new ListItem("Please Select", "0"));
                    ddxDoctor.SelectedIndex = 0;
                }
            }

        }


        private void loadDiscountsX()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                //                string stR = @"SELECT sConstant,sDescription FROM tblTypeDiscount WHERE iValidUntil_dt >= CONVERT(CHAR(10),GETDATE(),112)
                //                     AND BrCode='" + Session["vUser_Branch"].ToString() + "' ORDER BY sDescription";

                string stR = @"dbo.LoadDepositsDiscounts";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@FGcode", lblxFGCode.Text);
                    SqlDataReader dR = cmD.ExecuteReader();
                    ddxDiscount.Items.Clear();
                    ddxDiscount.DataSource = dR;
                    ddxDiscount.DataValueField = "sConstant";
                    ddxDiscount.DataTextField = "sDescription";
                    ddxDiscount.DataBind();
                    ddxDiscount.Items.Insert(0, new ListItem("select discount", "0"));
                    ddxDiscount.SelectedIndex = 0;
                }
            }
        }

        public string xConstant;
        public string xDiscDescription;
        public decimal xDiscPerc;
        public decimal xDiscountsAmt;
        public decimal xVatExemption;
        public decimal xNetAmount;

        public string xDiscPercentage;
        public string xVat;
        public string xFlag;

        public decimal xtheDiscAmount;

        protected void btnXpaybalance_Click(object sender, EventArgs e)
        {
            if (ddxDiscount.SelectedValue == "0")
            {
                xConstant = "0";
                xDiscDescription = string.Empty;
                xDiscPercentage = "0";
                xDiscountsAmt = 0;
                xNetAmount = Convert.ToDecimal(lblxDueAmount.Text);
                xVatExemption = 0;
            }
            else
            {
                xgetDiscountDetail();

                if (Convert.ToInt32(xVat) == 0)
                {
                    xtheDiscAmount = (Convert.ToDecimal(lblxDueAmount.Text)) * (Convert.ToDecimal(xDiscPercentage) / 100);
                    decimal xtheNet = (Convert.ToDecimal(lblxDueAmount.Text)) - xtheDiscAmount;
                    xNetAmount = xtheNet;
                    xVatExemption = 0;
                }
                else
                {
                    decimal xtheVat = 100 / Convert.ToDecimal(xVat);
                    decimal theDiscOnly = (Convert.ToDecimal(lblxDueAmount.Text)) * (Convert.ToDecimal(xDiscPercentage) / 100);
                    decimal TheNetLessDiscountx = Convert.ToDecimal(lblxDueAmount.Text) - theDiscOnly;


                    xNetAmount = TheNetLessDiscountx * xtheVat;
                    xVatExemption = TheNetLessDiscountx - xNetAmount;
                    xtheDiscAmount = theDiscOnly + xVatExemption;



                }
                xConstant = ddxDiscount.SelectedValue;
                xDiscDescription = ddxDiscount.SelectedItem.Text;
            }
            DataTable dt = (DataTable)ViewState["UnpostedDetail"];
            dt.Rows.Add(0,
            lblxFGCode.Text,
            lblxItemDescription.Text,
            lblxDueAmount.Text,
            1, //qty
            lblxTotalSession.Text,
            Convert.ToInt32(lblxSessionAvailed.Text) + Convert.ToInt32(txtxSession.Text),
            xConstant.ToString(),
            xDiscDescription.ToString(),
            xtheDiscAmount.ToString("0.##"), //DISC AMT
            xVatExemption.ToString("0.##"), //VAT EXMTION
            xDiscPercentage, //Perc(%)
            xNetAmount.ToString("0.##"),
            "Service",
            0,
            1,
            ddxDoctor.SelectedItem.Text,
            lblxReceiptNo.Text,
            1);
            ViewState["UnpostedDetail"] = dt;
            this.BindItemGrid();
        }


        private void xgetDiscountDetail()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT iPercent,iSpecialPercent,iAllItems_fl FROM tblTypeDiscount where BrCode=@BrCode AND sConstant=@sConstant";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@sConstant", ddxDiscount.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    while (dR.Read())
                    {
                        //if (dR[2].ToString() == "1")
                        //{
                        xDiscPercentage = dR[0].ToString();
                        xVat = dR[1].ToString();
                        xFlag = dR[2].ToString();
                        //}
                        //else
                        //{
                        //    checkIfDiscountIsValidX();
                        //}
                    }


                }
            }
        }


        //        private void checkIfDiscountIsValidX()
        //        {
        //            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //            {
        //                string stR = @"SELECT Distinct [vFGCode]
        //                              FROM [tblTypeDiscountDetail] WHERE 
        //                              sConstant=@sConstant AND vFGCode=@vFGCode";
        //                using (SqlCommand cmD = new SqlCommand(stR, conN))
        //                {
        //                    conN.Open();
        //                    cmD.Parameters.AddWithValue("@vFGCode", lblxFGCode.Text);
        //                    cmD.Parameters.AddWithValue("@sConstant", ddxDiscount.SelectedValue);
        //                    SqlDataReader dR = cmD.ExecuteReader();
        //                    if (dR.HasRows)
        //                    {
        //                        getDiscountDetailPerConstantX();
        //                    }
        //                    else
        //                    {

        //                        grdItemGrid.DataSource = null;
        //                        grdItemGrid.DataBind();
        //                        lblMsgWarningX.Text  = ddxDiscount.SelectedItem.Text + " discount is not allowed for this item";
        //                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsgX();", true);
        //                        return;
        //                    }

        //                }
        //            }

        //        }


        //        private void getDiscountDetailPerConstantX()
        //        {
        //            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //            {
        //                string stR = @"SELECT iPercent,iSpecialPercent,iAllItems_fl FROM tblTypeDiscount where BrCode=@BrCode AND sConstant=@sConstant";
        //                using (SqlCommand cmD = new SqlCommand(stR, conN))
        //                {
        //                    conN.Open();
        //                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
        //                    cmD.Parameters.AddWithValue("@sConstant",ddxDiscount.SelectedValue);
        //                    SqlDataReader dR = cmD.ExecuteReader();

        //                    while (dR.Read())
        //                    {
        //                        xDiscPercentage = dR[0].ToString();
        //                        xVat = dR[1].ToString();
        //                        xFlag = dR[2].ToString();
        //                    }

        //                }
        //            }
        //        }


        protected void btnEaddEmployee_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.AddNewEmployee";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@EmpNo", txtEEmployeeNo.Text.Trim());
                        cmD.Parameters.AddWithValue("@EmployeeName", txtEEmployeeSurname.Text.Trim().ToUpper() + ", " + txtEEmployeeFirstNaame.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@Position", ddEPosition.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsgWarning.Text = "Employee Number =  <b>" + txtEEmployeeNo.Text + "</b>\n already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            LoadDoctors();
                            lblMsgSuccess.Text = "New employee successfully added!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                    }
                }
            }
        }

        protected void btnAddEmploee_Click(object sender, EventArgs e)
        {

            LoadPositionE();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridAddEmployee();", true);
            return;

        }

        private void LoadPositionE()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT ID,Position FROM Position ORDER BY Position";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddEPosition.Items.Clear();
                    ddEPosition.DataSource = dR;
                    ddEPosition.DataValueField = "ID";
                    ddEPosition.DataTextField = "Position";
                    ddEPosition.DataBind();
                    ddEPosition.Items.Insert(0, new ListItem("Please Select", "0"));
                    ddEPosition.SelectedIndex = 0;
                }
            }

        }

        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {

            //Session["PrintReceiptOption"] = "Print";

            string url = "OfficialReceipt.aspx?SeriesNo=" + lblSeriesNo.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

        }


        protected void btnClosePrint_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "HidePopup", "$('#messageSuccessPosting').modal('hide')", true);

        }


        private void CheckGCCodeIFValid()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.CheckGCSeries";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@GCSeries", txtGCcode.Text);
                    cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmD.ExecuteNonQuery();
                    int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                    if (Result == 99)
                    {
                        lblMsgWarning.Text = "Invalid GC Promo Code = " + txtGCcode.Text;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    else if (Result == 10)
                    {
                        lblMsgWarning.Text = "GC Promo Code = " + txtGCcode.Text + " already used!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    else
                    {
                        InserDetailRow();
                    }
                }
            }

        }




        private void LoadtemMasterListSMOnline()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadItemMasterListForOnlineSelling";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddITemFG.Items.Clear();
                    ddITemFG.DataSource = dR;
                    ddITemFG.DataValueField = "vFGCode";
                    ddITemFG.DataTextField = "vDESCRIPTION";
                    ddITemFG.DataBind();
                    ddITemFG.Items.Insert(0, new ListItem("Please select item", "0"));
                    ddITemFG.SelectedIndex = 0;
                }
            }
        }



    }
}

