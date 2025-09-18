using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class SalesReturn : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        public string TheReceiptNo;

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
                        
                        //Session["TheRecieptNo"] = "25-2021-3-00000221";                        
                        
                        //Session["vUser_Branch"] = "5";

                        LoadSystemDate();

                        //checkIfMonthlyPostingISDone();

                        loadReceiptNo();
                        LoadtemMasterList();
                        //loadSource();
                        loadPaymentType();
                        loadBanks();

                        txtManualOR.Attributes.Add("onfocus", "this.select()");
                        txtManualOR.Focus();

                        txtSRP.Attributes.Add("onfocus", "this.select()");
                        txtAmtToPaid.Attributes.Add("onfocus", "this.select()");
                        txtCustomer.Attributes.Add("onfocus", "this.select()");

                        //txtxSession.Attributes.Add("onfocus", "this.select()");
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
                        dt.Columns.AddRange(new DataColumn[5] 
                            {  new DataColumn("PaymentMode"), 
                                new DataColumn("TotalAmount"), 
                                new DataColumn("BankName"), 
                                new DataColumn("BatchNumber"), 
                                new DataColumn("ReferenceNumber") });
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
            }
            else if (ddPayment.SelectedItem.Text == "Credit Card")
            {
                ddBanks.Enabled = true;
                loadBanks();
                lblBatchVoucher.Text = "Batch #";
                lblRefNo.Text = "Aprroval No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;

                RFBanks.Enabled = true;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;
            }

            else if (ddPayment.SelectedItem.Text == "Debit Card")
            {
                ddBanks.Enabled = true;
                loadBanks();
                lblBatchVoucher.Text = "Slip #";
                lblRefNo.Text = "Ref. No.";
                txtBatch.Enabled = true;
                txtReferenceNumber.Enabled = true;

                RFBanks.Enabled = true;
                rbBatch.Enabled = true;
                rfReferenceNumber.Enabled = true;
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
            }
            else if (ddPayment.SelectedItem.Text == "G Cash")
            {
                lblBatchVoucher.Text = "Batch #";
                txtBatch.Enabled = false;
                txtReferenceNumber.Enabled = true;
                RFBanks.Enabled = false;
                rbBatch.Enabled = false;
                rfReferenceNumber.Enabled = true;
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
            }
        }

        private void loadBanks()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT BankCode FROM Banks ORDER BY BankID";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBanks.Items.Clear();
                    ddBanks.DataSource = dR;
                    ddBanks.DataValueField = "BankCode";
                    ddBanks.DataTextField = "BankCode";
                    ddBanks.DataBind();
                    ddBanks.Items.Insert(0, new ListItem("Please select Bank", "0"));
                    ddBanks.SelectedIndex = 0;
                }
            }
        }


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
            }
            else
            {
                getFGinfo();
                //getBalance();

                //loadValidDiscounts();
            }
            //loadDiscounts();


        }


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
                            //txtSRP.ReadOnly = false;
                            lblNoSession.Visible = true;
                            lblNoSession.Text = dR[2].ToString();
                            txtNoSesson.ReadOnly = false;
                            txtNoSesson.Text = "1";
                            LoadDoctors();
                            ReqAvail.Enabled = false;
                            cvQty.Enabled = false;
                            lblWithInv.Text = dR[4].ToString();
                            RequiredDoctors.Enabled = true;
                            txtAvailable.Text = "0";
                            txtItemID.Text = "0";
                            //dposit
                        }

                        else if (dR[1].ToString() == "Product" & dR[4].ToString() == "False")
                        {
                            lblNoSession.Visible = false;
                            txtNoSesson.ReadOnly = true;
                            txtNoSesson.Text = "";
                            ReqAvail.Enabled = true;
                            cvQty.Enabled = false;
                            lblWithInv.Text = dR[4].ToString();
                            txtAvailable.Text = dR[5].ToString();
                            txtItemID.Text = dR[6].ToString();
                            RequiredDoctors.Enabled = true;
                            //txtSRP.ReadOnly = true;

                        }
                        else
                        {
                            RequiredDoctors.Enabled = true;
                            lblNoSession.Visible = false;
                            txtNoSesson.ReadOnly = true;
                            txtNoSesson.Text = "";
                            ReqAvail.Enabled = true;
                            cvQty.Enabled = true;
                            lblWithInv.Text = dR[4].ToString();
                            txtAvailable.Text = dR[5].ToString();
                            txtItemID.Text = dR[6].ToString();
                            //txtSRP.ReadOnly = true;


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
                                InserDetailRow();
                            }
                        }
                    }
                    else
                    {
                        InserDetailRow();
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

                else if (grdPayment.Rows.Count > 0 & txtManualOR.Text == string.Empty)
                {
                    txtManualOR.Focus();
                    lblMsgWarning.Text = "Please supply manual OR.";
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


                else
                {
                    PostInsertUnpostedDetailedEntryNew();

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
                                //btnRefresh.Focus();
                                lblMsgWarning.Text = "Series number " + txtOrderNo.Text + " already used. Please refresh page";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;


                            }
                            else
                            {
                                lblConfirmSubmit.Text = "Are all entries correct?";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowConfirmSubmit();", true);
                                return;
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

                    PostInsertUnpostedDetailedEntryNew();
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }

        }



        



        //protected void btnShowSessionBalance_Click(object sender, EventArgs e)
        //{
        //    ckNewDR.Checked = false;
        //    GetSessionBalance();
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

            grdPayment.DataSource = (DataTable)ViewState["Payment"];
            grdPayment.DataBind();

        }


        string thePaymentType;
        string theBatch;
        string TheReference;
        protected void InsertPaymentRow()
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


                DataTable dt = (DataTable)ViewState["Payment"];
                dt.Rows.Add(ddPayment.SelectedItem.Text.Trim(),
                    txtAmtToPaid.Text.Trim(),
                    thePaymentType.ToString(),
                    theBatch.ToString(),
                    TheReference.ToString());
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


        }

        protected void ClearFields()
        {
            try
            {
                ddPayment.Items.Clear();
                loadPaymentType();

                txtBatch.Text = string.Empty;
                txtReferenceNumber.Text = string.Empty;
                ddBanks.Items.Clear();
                ddBanks.Enabled = false;
                txtBatch.Enabled = false;
                txtReferenceNumber.Enabled = false;

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
                InsertPaymentRow();
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
                //try
                //{

                if (ddITemFG.SelectedValue == "0")
                {
                    ddITemFG.Focus();
                    lblMsgWarning.Text = "Please select item!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                //else if (lblType.Text == "Service" & ddDrs.SelectedValue == "0" & ckDeposit.Checked == false)
                //{

                //    ddDrs.Focus();
                //    lblMsgWarning.Text = "Please select staff!";
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //    return;
                //}
                else if (lblIsKit.Text == "True")
                {

                    CheckIfKitHAsBalance();
                }


                //else if (ckDeposit.Checked == true)
                //{
                //    if (grdItemGrid.Rows.Count > 0)
                //    {
                //        lblMsgWarning.Text = "Only one item is allowed for deposit transaction";
                //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //        return;
                //    }
                //    else if ((Convert.ToDecimal(txtSRP.Text) * Convert.ToDecimal(txtQty.Text)) < Convert.ToDecimal(txtAmtDeposit.Text))
                //    {
                //        txtAmtDeposit.Focus();
                //        lblMsgWarning.Text = "Amount deposit greater than total amount availed is invaild!";
                //        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //        return;
                //    }
                //    else
                //    {
                //        InserDetailRow();
                //    }
                //}

                else
                {
                    InserDetailRow();

                }
                //}
                //catch (Exception x)
                //{
                //    lblMsgWarning.Text = x.Message;
                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                //    return;
                //}
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

        //public decimal theNet;
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
                //PerformedBy = string.Empty;
                theDoctor = string.Empty;
            }
            else
            {

                int TotalSession = Convert.ToInt32(lblNoSession.Text) * Convert.ToInt32(txtQty.Text);

                TheTotalSession = TotalSession;
                NoSession = txtNoSesson.Text;
                theDoctor = ddDrs.SelectedItem.Text;
            }



            
                IsDeposit = 0;

                sConstant = string.Empty;
                DiscDescription = string.Empty;
                vDiscPerc = 0;
                DiscountsAmt = 0;
                NetAmount = Convert.ToDecimal(txtSRP.Text) * Convert.ToInt32(txtQty.Text);
                VatExemption = 0;
                


            DataTable dt = (DataTable)ViewState["UnpostedDetail"];
            dt.Rows.Add(txtItemID.Text,
            ddITemFG.SelectedValue,
            ddITemFG.SelectedItem.Text,
            txtSRP.Text,
            txtQty.Text,
            TheTotalSession,
            NoSession,
            0,
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
            

        }


        //################################END########################################################


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
                        cmD.Parameters.AddWithValue("@OrderSource", "Sales Return");
                        cmD.Parameters.AddWithValue("@PatientType", string.Empty);
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
                        cmD.ExecuteNonQuery();


                        //}
                        //catch (Exception x)
                        //{
                        //    lblMsgWarning.Text = x.Message;
                        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        //    return;
                        //}

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
                            cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                            cmD.ExecuteNonQuery();

                        }
                    }
                }
            }

            UpdateReceiptNo();
            loadReceiptNo();

            LoadtemMasterList();
            loadPaymentType();

            lblType.Text = string.Empty;
            lblNoSession.Text = string.Empty;
            lblIsKit.Text = string.Empty;
            lblWithInv.Text = string.Empty;

            txtAvailable.Text = string.Empty;
            txtItemID.Text = string.Empty;
            txtSRP.Text = string.Empty;
            txtNoSesson.Text = string.Empty;
            txtQty.Text = string.Empty;

            


            txtTotalAmount.Text = string.Empty;
            txtAmtToPaid.Text = string.Empty;
            xtlblAmounttobepaid.Text = string.Empty;
            txtTotalAmtRender.Text = string.Empty;
            txtManualOR.Text = string.Empty;

            txtCustID.Text = string.Empty;
            txtCustomer.Text = string.Empty;

         
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
            dt.Columns.AddRange(new DataColumn[5] 
                            {  new DataColumn("PaymentMode"), 
                                new DataColumn("TotalAmount"), 
                                new DataColumn("BankName"), 
                                new DataColumn("BatchNumber"), 
                                new DataColumn("ReferenceNumber") });
            ViewState["Payment"] = dt;
            this.BindPaymentGrid();

            


            //03/22/2021

            //lblMsgPrintPreview.Text = "Printing of receipt is not allowed(Not yet register).) ";
            //I1.Attributes.Add("src", "SalesReturnReceipt.aspx?SeriesNo=" + txtOrderNo.Text);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowPrintPreview();", true);

            //UpdateReceiptNo();
            //loadReceiptNo();
            //return;

            lblMsgSuccess.Text = "Sales succesfully posted!";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
            return;


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


        }

        protected void gvCustomerList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvR = gvCustomerList.Rows[e.RowIndex];

            txtCustID.Text = gvR.Cells[1].Text;
            txtCustomer.Text = Server.HtmlDecode(gvR.Cells[2].Text);
            //GetDepositBalance();



        }


        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            //lblMsgPrintPreview.Text = "PRINT RECEIPT";
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowPrintPreview();", true);
            //return;

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //lblMsgPrintPreview.Text = "Printing of receipt is not allowed(Not yet register).) ";
            //I1.Attributes.Add("src", "SalesReturnReceipt.aspx?SeriesNo=" + txtOrderNo.Text);
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowPrintPreview();", true);
            //return;
        }

//        private void loadOR()
//        {
//            TheReceiptNo = "25-2021-3-00000221";
//            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
//            {

//                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
//                string stR = @"SELECT DISTINCT [SalesDate]
//                                  ,[CustID]
//                                  ,[CustomerName]
//                                  ,[ReceiptNo]
//                                  ,[vFGCode]
//                                  ,[vUnitCost]
//                                  ,[vQty]
//                                  ,[DiscountsAmt]
//                                  ,[VatExemption]
//                                  ,[NetAmount]
//                                    ,[BrCode]
//                                    ,vUser_ID
//                            FROM UnpostedSalesDetailed
//                            where ReceiptNo='" + TheReceiptNo + "'";
//                using (SqlCommand cmD = new SqlCommand(stR, conN))
//                {
//                    conN.Open();
//                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
//                    DataSet dS = new DataSet();
//                    dA.Fill(dS);

//                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(conStr);


//                    string ServerName = builder["Data Source"].ToString();
//                    string DatabaseName = builder["Initial Catalog"].ToString();
//                    string UserID = builder["User ID"].ToString();
//                    string Password = builder["Password"].ToString();

//                    ReportDocument crp = new ReportDocument();




//                    crp.Load(Server.MapPath("~/Reports/OfficialReceipt.rpt"));
//                    crp.SetDatabaseLogon(UserID, Password, ServerName, DatabaseName);
//                    //crp.SetDatabaseLogon("sa", "citadmin", "192.168.5.85", "SMSTEST1");
//                    crp.DataSourceConnections[0].SetConnection(ServerName, DatabaseName, UserID, Password);

//                    crp.SetDataSource(dS.Tables["table"]);

//                    //crp.SetParameterValue("BrCode", 25);
//                    //crp.SetParameterValue("ParamBrCode", 25);
//                    crp.SetParameterValue("ParamReceiptNo", TheReceiptNo.ToString().TrimEnd());
//                    //crp.SetParameterValue("BrCode", "25");




//                    crvReceipt.ReportSource = crp;
                    




//                    //crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "OfficialReceipt");


//                }
//            }
//        }

    }
}

