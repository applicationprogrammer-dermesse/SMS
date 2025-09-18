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
    public partial class RecordComplimentary : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
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

                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        LoadPrimaryCustomer();
                        btnSave.Disabled = true;
                        btnSubmit.Disabled = true;
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
                    }
                    else
                    {
                        loadComplimentaryNo();
                        LoadFGItem();
                        LoadPrimaryCustomer();
                        loadUnposted();
                        btnSave.Disabled = false;
                        btnSubmit.Disabled = false;
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
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


        private void loadComplimentaryNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CONCAT('Compli-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('00000'+CAST(ComplimentaryNo AS VARCHAR(8)),8)) FROM SystemMaster where [BrCode]='" + Session["vUser_Branch"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtComplimentaryNo.Text = dR[0].ToString();
                    }

                }
            }
        }

        private void LoadPrimaryCustomer()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CompliID,CompliName FROM CompliList where vStat=1 order by CompliName";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddPrimaryCustomer.Items.Clear();
                    ddPrimaryCustomer.DataSource = dR;
                    ddPrimaryCustomer.DataValueField = "CompliID";
                    ddPrimaryCustomer.DataTextField = "CompliName";
                    ddPrimaryCustomer.DataBind();
                    ddPrimaryCustomer.Items.Insert(0, new ListItem("Please select customer", "0"));
                    ddPrimaryCustomer.SelectedIndex = 0;
                }
            }
        }
        private void LoadFGItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT vFGCode,LTRIM(RTRIM(vPluCode)) + '-' + LTRIM(RTRIM(vDESCRIPTION)) AS [vDESCRIPTION] FROM vItemMaster where vStat=1 and vPluCode like '0%'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    if (ddITemFG.SelectedValue == "0")
                    {
                        ddITemFG.Focus();
                        lblMsgWarning.Text = "Please select finish good item!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                    else if ((Convert.ToDouble(txtAmountLimit.Text) - Convert.ToDouble(txtTotalAmount.Text)) < (Convert.ToDouble(txtQty.Text) * Convert.ToDouble(txtSRP.Text)))
                    {
                        lblMsgWarning.Text = "Allowable amount already exceeds";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                    else
                    {

                        CheckIfExistsComplimentaryNo();
                        //SaveUnposted();

                    }
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }

        private void CheckIfExistsComplimentaryNo()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT ComplimentaryNo FROM PostedComplimentary WHERE ComplimentaryNo=@ComplimentaryNo";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@ComplimentaryNo", txtComplimentaryNo.Text);

                        SqlDataReader dR = cmD.ExecuteReader();

                        if (dR.HasRows)
                        {
                            while (dR.Read())
                            {
                                txtComplimentaryNo.Focus();
                                lblMsgWarning.Text = "Complimentary number " + txtComplimentaryNo.Text + " already exists";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            }
                        }
                        else
                        {
                            SaveUnposted();
                        }


                    }


                }
            }
        }

        private void SaveUnposted()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"INSERT INTO UnpostedComplimentary
                        ([ComplimentaryNo]
                      ,CompliType
                       ,BrCode
                      ,[Complimentarydate]
                       ,vItemID
                      ,[vFGCode]
                      ,[vQty]
                       ,vStat
                       ,CompliCustomerName
                      ,[vUser_ID]
                      ,[TransactionDate]
                       ,CompliID
                       ,SRP
                      ,[CompliAmount])
                    VALUES 
                     (@ComplimentaryNo
                      ,@CompliType
                      ,@BrCode
                      ,@Complimentarydate
                        ,@vItemID
                      ,@vFGCode
                      ,@vQty
                      ,@vStat
                       ,@CompliCustomerName
                      ,@vUser_ID
                      ,@TransactionDate
                       ,@CompliID
                       ,@SRP
                      ,@CompliAmount)";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@Complimentarydate", txtDateReceived.Text);
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@vItemID", txtItemID.Text);
                        cmD.Parameters.AddWithValue("@vStat", 0);
                        cmD.Parameters.AddWithValue("@CompliCustomerName", txtCustomerName.Text);
                        cmD.Parameters.AddWithValue("@ComplimentaryNo", txtComplimentaryNo.Text);
                        cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                        cmD.Parameters.AddWithValue("@vQty", txtQty.Text);
                        cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                        cmD.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        cmD.Parameters.AddWithValue("@CompliID" ,ddPrimaryCustomer.SelectedValue);
                        cmD.Parameters.AddWithValue("@SRP", txtSRP.Text);
                        cmD.Parameters.AddWithValue("@CompliAmount", Convert.ToDecimal(txtSRP.Text) * Convert.ToDecimal(txtQty.Text));
                        cmD.Parameters.AddWithValue("@CompliType", 1);
                        cmD.ExecuteNonQuery();
                    }


                    //txtBatchNo.Text = "";
                    //txtDateExpiry.Text = "";



                }
                LoadFGItem();
                txtItemID.Text = "";
                txtAvailable.Text = "";
                txtQty.Text = "";
                txtSRP.Text = "";
                
                loadUnposted();
                getPrimaryLimit();

            }
        }

        private void loadUnposted()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT a.ID
                                ,a.CompliID
                              ,a.ComplimentaryNo
                              ,a.Complimentarydate
                              ,a.vFGCode
	                          ,b.vDESCRIPTION
                              ,a.vQty
                              ,a.SRP
                              ,a.CompliAmount
                          FROM UnpostedComplimentary a
                           LEFT JOIN vItemMaster b
	                        ON a.[vFGCode]=b.[vFGCode]
                            WHERE a.BrCode=@BrCode AND a.vStat=0 and a.CompliType = 1
                          ORDER BY a.ID";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    //cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvUnposted.DataSource = dT;
                    gvUnposted.DataBind();

                    if (gvUnposted.Rows.Count == 0)
                    {
                        btnSubmit.Disabled = true;
                    }
                    else
                    {
                        btnSubmit.Disabled = false;
                    }

                }
            }
        }

        protected void gvUnposted_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUnposted.EditIndex = e.NewEditIndex;
            loadUnposted();
        }

        protected void gvUnposted_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUnposted.EditIndex = -1;
            loadUnposted();
        }

        protected void gvUnposted_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    string dKey = gvUnposted.DataKeys[e.RowIndex].Value.ToString();
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"DELETE FROM UnpostedComplimentary where ID=@ID";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@ID", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    gvUnposted.EditIndex = -1;
                    getPrimaryLimit();
                    loadUnposted();
                    
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }

        protected void gvUnposted_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string RecNo = gvUnposted.DataKeys[e.RowIndex].Value.ToString();
            TextBox xtQtyDR = gvUnposted.Rows[e.RowIndex].FindControl("txtQtyDR") as TextBox;
            TextBox xtRemarks = gvUnposted.Rows[e.RowIndex].FindControl("txtCustomerName") as TextBox;
            Label bllblQtyOrig = gvUnposted.Rows[e.RowIndex].FindControl("lblQtyOrig") as Label;

            UpdateGrid(RecNo, xtQtyDR, xtRemarks);

            //if (Convert.ToInt32(xtQtyDR.Text) <= Convert.ToInt32(bllblQtyOrig.Text))
            //{
            //    UpdateGrid(RecNo, xtQtyDR, xtRemarks);
            //}
            //else
            //{

            //    int theDiff;
            //    theDiff = Convert.ToInt32(xtQtyDR.Text) - Convert.ToInt32(bllblQtyOrig.Text);

            //    using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            //    {
            //        string stR = @"dbo.ShowItemBatchBalance";
            //        using (SqlCommand cmD = new SqlCommand(stR, Conn))
            //        {
            //            Conn.Open();
            //            cmD.CommandType = CommandType.StoredProcedure;
            //            cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
            //            cmD.Parameters.AddWithValue("@vFGcode", ddITemFG.SelectedValue);
            //            DataTable dT = new DataTable();
            //            SqlDataAdapter dA = new SqlDataAdapter(cmD);
            //            dA.Fill(dT);

            //            if (dT.Rows.Count > 0)
            //            {
            //                int TheBalance;
            //                TheBalance = Convert.ToInt32(dT.Rows[0]["Available Balance"].ToString());
            //                if (theDiff > TheBalance)
            //                {
            //                    lblMsgWarning.Text = "Insufficent balance";
            //                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //                    return;
            //                }
            //                else
            //                {
            //                    UpdateGrid(RecNo, xtQtyDR, xtRemarks);
            //                }
            //            }
            //        }
            //    }


            //}


        }

        private void UpdateGrid(string RecNo, TextBox xtQtyDR, TextBox xtRemarks)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"Update UnpostedComplimentary set vQty=@vQty,Remarks=@Remarks where ID=@vRecNum";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.Parameters.AddWithValue("@vQty", xtQtyDR.Text);
                    cmD.Parameters.AddWithValue("@Remarks", xtRemarks.Text);
                    cmD.Parameters.AddWithValue("@vRecNum", RecNo);
                    cmD.ExecuteNonQuery();
                }

                gvUnposted.EditIndex = -1;
                loadUnposted();
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                //ReqAvail.Enabled = false;
                //cvQty.Enabled = false;
                //
                if (gvUnposted.Rows.Count == 0)
                {

                    lblMsgWarning.Text = "Please generate data to post";
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
                    foreach (GridViewRow gvR in gvUnposted.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                            {
                                string stR = @"dbo.SubmitUnpostedComplimentary";
                                using (SqlCommand cmD = new SqlCommand(stR, conN))
                                {
                                    conN.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.Parameters.AddWithValue("@ID", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.ExecuteNonQuery();
                                }
                            }
                        }
                    }


                    txtDateReceived.Text = "";
                    txtComplimentaryNo.Text = "";
                    txtCustomerName.Text = "";
                    UpdateComplimentaryNo();
                    loadComplimentaryNo();
                    loadUnposted();
                    LoadPrimaryCustomer();
                    txtTotalAmount.Text = "";
                    txtAmountLimit.Text = "";
                    btnSubmit.Disabled = true;
                    lblMsgWarning.Text = "Complimentary succesfully send to HO for posting!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }

        }


        private void UpdateComplimentaryNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET ComplimentaryNo=ComplimentaryNo + 1 WHERE BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    //SqlTransaction myTrans = conN.BeginTransaction();    
                    //cmD.Transaction = myTrans;
                    //try
                    //{
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.ExecuteNonQuery();
                    //    myTrans.Commit();
                    //}
                    //catch (Exception x)
                    //{
                    //    myTrans.Rollback();
                    //    lblMsgWarning.Text = x.Message;
                    //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    //    return;
                    //}

                }
            }

        }

        private void getBalance()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowItemBatchBalance";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@vFGcode", ddITemFG.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {

                        txtAvailable.Text = dT.Rows[0]["Available Balance"].ToString();
                        txtItemID.Text = dT.Rows[0]["vItemID"].ToString();
                    }
                    else
                    {
                        txtAvailable.Text = "0";
                        txtItemID.Text = "";
                    }

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
                ReqAvail.Enabled = false;
             
            }
            else
            {
                //getBalance();
                getFGinfo();
            }
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
                        txtSRP.Text = theSRP.ToString();
                        if (dR[1].ToString() == "Service")
                        {
                            txtSRP.ReadOnly = false;
                            ReqAvail.Enabled = false;
                            cvQty.Enabled = false;
                        }
                        else
                        {
                            txtAvailable.Text = dR[5].ToString();
                            txtItemID.Text = dR[6].ToString();
                            txtSRP.ReadOnly = true;
                            ReqAvail.Enabled = true;
                            cvQty.Enabled = true;
                        }
                    }
                }
            }
        }

        protected void ddPrimaryCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddPrimaryCustomer.SelectedValue == "0")
            {
                txtAmountLimit.Text = string.Empty;
                txtTotalAmount.Text = string.Empty;
            }
            else
            {
                getPrimaryLimit();
            }
        }


        private void getPrimaryLimit()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.GetCreditBalance";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@CompliID", ddPrimaryCustomer.SelectedValue);

                        SqlDataReader dR = cmD.ExecuteReader();
                        while (dR.Read())
                        {


                            txtAmountLimit.Text = dR[0].ToString();
                            txtTotalAmount.Text = dR[1].ToString();


                        }
                    }
                }
            }

            catch (Exception x)
            {
             
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }

        

        protected void btnView_Click(object sender, EventArgs e)
        {

            Session["iNo"] = ddPrimaryCustomer.SelectedValue;

            //Response.Redirect("~/ViewCompliTransaction.aspx?iD=" + ddPrimaryCustomer.SelectedValue);
            Response.Write("<script>window.open ('ViewCompliTransaction.aspx?iNo=" + ddPrimaryCustomer.SelectedValue + " ','_blank');</script>");

        }
        
    }
}