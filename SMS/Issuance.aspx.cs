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
    public partial class Issuance : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                if (Session["vUser_Branch"].ToString() == "1")
                {
                    loadIssuanceNo();
                    loadAllBranch();
                    LoadFGItem();
                    loadUnposted();
                    loadDeliveredBy();
                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();
                    //requiredBalance.Enabled = true;

                }
                else
                {
                    Response.Redirect("~/UnauthorizedPAge.aspx");

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

        private void loadDeliveredBy()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [ID]
                                      ,[DeliveredBy]
                                  FROM [tblDeliveredBy] WHERE [stat]=1";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    //cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddDeliveredBy.Items.Clear();
                    ddDeliveredBy.DataSource = dR;
                    ddDeliveredBy.DataValueField = "ID";
                    ddDeliveredBy.DataTextField = "DeliveredBy";
                    ddDeliveredBy.DataBind();
                    ddDeliveredBy.Items.Insert(0, new ListItem("Please Select", "0"));
                    ddDeliveredBy.SelectedIndex = 0;
                }
            }
        }

        private void loadAllBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadBranches";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    ddBranch.Items.Insert(0, new ListItem("Select branch", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }

        private void loadIssuanceNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CONCAT('IssNo-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('00000'+CAST(IssuanceNo AS VARCHAR(8)),8)) FROM SystemMaster where [BrCode]='" + Session["vUser_Branch"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtIssuanceNo.Text = dR[0].ToString();
                    }

                }
            }
        }


        private void LoadFGItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT vFGCode,LTRIM(RTRIM(vFGCode)) + '-' + LTRIM(RTRIM(vDESCRIPTION)) AS [ItemDescription] 
                    FROM vItemMaster where vStat=1 and ItemType='Product' and vFGCode like 'CLN%' ORDER BY vDESCRIPTION";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddITemFG.Items.Clear();
                    ddITemFG.DataSource = dR;
                    ddITemFG.DataValueField = "vFGCode";
                    ddITemFG.DataTextField = "ItemDescription";
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

                    else if (ckOption.Checked == true & txtBalance.Text == string.Empty & ckOption2.Checked==false)
                    {
                        requiredBalance.Enabled = true;
                    }

                    //else if (lblSource.Text == "PIS" & Convert.ToInt32(txtBalance.Text) < Convert.ToInt32(txtQty.Text))
                    //{
                    //    txtQty.Focus();
                    //    lblMsgWarning.Text = "Insuficient balance";
                    //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    //    return;
                    //}


                    else
                    {

                        CheckIfExistsIssuanceno();
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

        private void CheckIfExistsIssuanceno()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT IssuanceNo FROM PostedIssuance WHERE IssuanceNo=@Issuanceno
                                UNION ALL
                                SELECT IssuanceNo FROM UnpostedIssuance WHERE IssuanceNo=@Issuanceno and vStat=1";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@Issuanceno", txtIssuanceNo.Text);

                        SqlDataReader dR = cmD.ExecuteReader();

                        if (dR.HasRows)
                        {
                            while (dR.Read())
                            {
                                btnRefreshIssNo.Focus();
                                lblMsgWarning.Text = "Issue Slip <b> " + txtIssuanceNo.Text + "</b> already exists.  \nPlease click refresh button to re generate issue slip no.";
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
                    string stR = @"INSERT INTO UnpostedIssuance
                        ([IssuanceNo]
                      ,[IssuanceDate]
                      ,vItemID
                      ,[vFGCode]
                      ,[vBatchNo]
                      ,[vDateExpiry]
                      ,[vQty]
                       ,vStat
                       ,Remarks
                      ,[vUser_ID]
                      ,[TransactionDate]
                      ,DeliveredBy
                      ,IsPIS)
                    VALUES 
                     (@IssuanceNo
                      ,@IssuanceDate
                       ,@vItemID
                      ,@vFGCode
                      ,@vBatchNo
                      ,@vDateExpiry
                      ,@vQty
                      ,@vStat
                       ,@Remarks
                      ,@vUser_ID
                      ,@TransactionDate
                       ,@DeliveredBy
                       ,@IsPIS)";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@IssuanceDate", txtDateReceived.Text);
                        //cmD.Parameters.AddWithValue("@BrCode",ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@vStat", 0);
                        cmD.Parameters.AddWithValue("@Remarks", txtReason.Text);
                        cmD.Parameters.AddWithValue("@IssuanceNo", txtIssuanceNo.Text);
                        cmD.Parameters.AddWithValue("@vItemID", txtID.Text);
                        cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                        cmD.Parameters.AddWithValue("@vBatchNo",txtBatchNo.Text);
                        cmD.Parameters.AddWithValue("@vDateExpiry", txtExpirationDate.Text);
                        cmD.Parameters.AddWithValue("@vQty", txtQty.Text);
                        cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                        cmD.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        cmD.Parameters.AddWithValue("@DeliveredBy", ddDeliveredBy.SelectedItem.Text);
                        if (lblSource.Text=="PIS")
                        {
                            cmD.Parameters.AddWithValue("@IsPIS", 1);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@IsPIS", 0);
                        }
                        cmD.ExecuteNonQuery();
                    }


                    //txtBatchNo.Text = "";
                    //txtDateExpiry.Text = "";



                }

                if (ckOption.Checked == true & ckOption2.Checked==false)
                {
                    LoadFGItem();
                    lnkSelectBatch.Visible = true;
                    //lnkBtachPRF.Visible = false;
                    cvQty.Enabled = true;
                    ckOption.Text = " Uncheck to show not clinic supplies item";
                    requiredBalance.Enabled = true;
                }
                else
                {
                    LoadFGItemALL();
                    lnkSelectBatch.Visible = false;
                    //lnkBtachPRF.Visible = true; 
                    cvQty.Enabled = false;
                    ckOption.Text = " Check to show clinic supplies item only";
                    requiredBalance.Enabled = false;
                }

                txtID.Text = "";
                txtBalance.Text = "";
                txtQty.Text = "";
                txtExpirationDate.Text = "";
                txtBatchNo.Text = "";

                loadUnposted();

            }
        }

        private void loadUnposted()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT a.ID
                              ,a.IssuanceNo
                              ,a.IssuanceDate
                              ,a.vFGCode
                              ,a.vBatchNo
                              ,a.vDateExpiry
	                          ,b.vDESCRIPTION
                              ,a.vQty
                          FROM UnpostedIssuance a
                           LEFT JOIN vItemMaster b
	                        ON a.[vFGCode]=b.[vFGCode]
                            WHERE  a.vStat=0
                            AND vUser_ID=@vUser_ID
                          ORDER BY a.ID";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
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
                        string stR = @"DELETE FROM UnpostedIssuance where ID=@ID";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@ID", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    gvUnposted.EditIndex = -1;
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
            TextBox xtBatchNoDR = gvUnposted.Rows[e.RowIndex].FindControl("txtBatchNoDR") as TextBox;
            TextBox xtDateExpiryDR = gvUnposted.Rows[e.RowIndex].FindControl("txtDateExpiryDR") as TextBox;

            UpdateGrid(RecNo, xtQtyDR, xtBatchNoDR, xtDateExpiryDR);
            //UpdateGrid(RecNo, xtQtyDR);




        }

        private void UpdateGrid(string RecNo, TextBox xtQtyDR, TextBox xtBatchNoDR, TextBox xtDateExpiryDR)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"Update UnpostedIssuance set vQty=@vQty,vDateExpiry=@vDateExpiry,vBatchNo=@vBatchNo  where ID=@vRecNum";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.Parameters.AddWithValue("@vQty", xtQtyDR.Text);
                    cmD.Parameters.AddWithValue("@vBatchNo", xtBatchNoDR.Text);
                    cmD.Parameters.AddWithValue("@vDateExpiry", xtDateExpiryDR.Text);
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

                else if (ddBranch.SelectedValue == "0")
                {
                    ddBranch.Focus();
                    lblMsgWarning.Text = "Please select branch";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

                else
                {
                    CheckIfExistsIssuancenoBeforeSubmit();
                    

                }
            }
        }



        private void CheckIfExistsIssuancenoBeforeSubmit()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT IssuanceNo FROM PostedIssuance WHERE IssuanceNo=@Issuanceno
                                UNION ALL
                                SELECT IssuanceNo FROM UnpostedIssuance WHERE IssuanceNo=@Issuanceno and vStat=1";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@Issuanceno", txtIssuanceNo.Text);

                        SqlDataReader dR = cmD.ExecuteReader();

                        if (dR.HasRows)
                        {
                            while (dR.Read())
                            {
                                btnRefreshIssNo.Focus();
                                lblMsgWarning.Text = "Issue Slip <b> " + txtIssuanceNo.Text + "</b> already exists.  \nPlease click refresh button to re generate issue slip no. and delete encoded item to assign new issue slip no.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            }
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
                                string stR = @"dbo.SubmitUnpostedIssuance";
                                using (SqlCommand cmD = new SqlCommand(stR, conN))
                                {
                                    conN.Open();
                                    cmD.CommandTimeout = 0;
                                    cmD.Parameters.AddWithValue("@ID", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                                    cmD.CommandType = CommandType.StoredProcedure;
                                    cmD.ExecuteNonQuery();
                                }
                            }
                        }
                    }


                    txtDateReceived.Text = "";
                    txtIssuanceNo.Text = "";
                    txtReason.Text = "";
                    UpdateIssuanceNo();
                    loadIssuanceNo();
                    loadUnposted();
                    //loadAllBranch();
                    btnSubmit.Disabled = true;
                    lblMsgWarning.Text = "Issuance succesfully send to " + ddBranch.SelectedItem.Text + " for posting!";
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


        private void UpdateIssuanceNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET IssuanceNo=IssuanceNo + 1 WHERE BrCode=@BrCode";
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

        protected void lnkSelectBatch_Click(object sender, EventArgs e)
        {
            if (ddITemFG.SelectedValue == "0")
            {
                lblMsgWarning.Text = "Please select item!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                GetItemBatch();
            }
        }


        private void GetItemBatch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowClinicBalance";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddITemFG.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        gvItemBalance.DataSource = dT;
                        gvItemBalance.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }
        }



        protected void gvItemBalance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            string theID = gvItemBalance.Rows[e.RowIndex].Cells[0].Text;
            string theBalance = gvItemBalance.Rows[e.RowIndex].Cells[5].Text;
            txtID.Text = theID.ToString();
            txtBalance.Text = theBalance.ToString();

            lblSource.Text = "PIS";

            txtBatchNo.Text = gvItemBalance.Rows[e.RowIndex].Cells[3].Text;
            txtExpirationDate.Text = gvItemBalance.Rows[e.RowIndex].Cells[4].Text;

        }

        protected void ckOption_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOption.Checked == true)
            {
                LoadFGItem();
                lnkSelectBatch.Visible = true;
                cvQty.Enabled = true;
                ckOption.Text = " Uncheck to show not clinic supplies item";
                //requiredBalance.Enabled = true;
                //txtExpirationDate.ReadOnly = true;
                //txtBatchNo.ReadOnly = true;
                lblSource.Text = string.Empty;

            }
            else
            {
                LoadFGItemALL();
                lnkSelectBatch.Visible = false;
                cvQty.Enabled = false;
                ckOption.Text = " Check to show clinic supplies item only";
                //requiredBalance.Enabled = false;
                //txtExpirationDate.ReadOnly = false;
                //txtBatchNo.ReadOnly = false;

                lblSource.Text = string.Empty;
            }
        }

        private void LoadFGItemALL()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadItemForIssuance";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
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


        protected void btnRefreshIssNo_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                UpdateIssuanceNo();
                loadIssuanceNo();
            }
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            txtEEmployeeName.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridAddEmployee();", true);
            return;
        }



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
                    string stR = @"dbo.AddNewDeliveredBy";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;

                        cmD.Parameters.AddWithValue("@EmployeeName", txtEEmployeeName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsgWarning.Text = "Employee Name =  <b>" + txtEEmployeeName.Text + "</b>\n already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            loadDeliveredBy();
                            lblMsgWarning.Text = "Successfully added";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                    }
                }
            }
        }



        protected void btnPrintPreview_Click(object sender, EventArgs e)
        {
            if (gvUnposted.Rows.Count == 0)
            {

                lblMsgWarning.Text = "Please generate data to print";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

            else if (ddBranch.SelectedValue == "0")
            {
                ddBranch.Focus();
                lblMsgWarning.Text = "Please select branch";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else if (ddDeliveredBy.SelectedValue == "0")
            {
                ddDeliveredBy.Focus();
                lblMsgWarning.Text = "Please select delivered by";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                Session["Branch"] = ddBranch.SelectedItem.Text;
                Session["DeliveredBy"] = ddDeliveredBy.SelectedItem.Text;
                Session["IssueSlipNo"] = txtIssuanceNo.Text;
                Session["Date"] = txtDateReceived.Text;
                Response.Write("<script>window.open ('PrintIssueSlip.aspx?val=" + txtIssuanceNo.Text + "','_blank');</script>");
            }
        }

        protected void lnkBtachPRF_Click(object sender, EventArgs e)
        {
            if (ddITemFG.SelectedValue == "0")
            {
                lblMsgWarning.Text = "Please select item!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                GetPRFItemBatch();
            }
        }


        private void GetPRFItemBatch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowPRFbatch";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddITemFG.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        gvPRFBatch.DataSource = dT;
                        gvPRFBatch.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridPRFBatch();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }
        }

        //public string theBatchPRF;
        //public DateTime theBatchDateExpiry;
        protected void gvPRFBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //string theID = gvItemBalance.Rows[e.RowIndex].Cells[0].Text;
            string theBalance = gvPRFBatch.Rows[e.RowIndex].Cells[6].Text;
            
            txtID.Text = "0";
            
            txtBalance.Text = theBalance.ToString();

            lblSource.Text = "PRF";

            txtBatchNo.Text = gvPRFBatch.Rows[e.RowIndex].Cells[3].Text;
            txtExpirationDate.Text = gvPRFBatch.Rows[e.RowIndex].Cells[4].Text;
        }

        protected void lnkBatchPIS_Click(object sender, EventArgs e)
        {
            if (ddITemFG.SelectedValue == "0")
            {
                lblMsgWarning.Text = "Please select item!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                GetItemBatchPIS();
            }
        }


        private void GetItemBatchPIS()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowPISBalance";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Sup_ItemCode", ddITemFG.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {
                        gvItemBalance.DataSource = dT;
                        gvItemBalance.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatch();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }
        }

        protected void ckOption2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckOption2.Checked == true)
            {
                requiredBalance.Enabled = false;
                txtExpirationDate.ReadOnly = false;
                txtBatchNo.ReadOnly = false;
                lnkBatchPIS.Visible = false;
                lnkBtachPRF.Visible = false;
                lnkSelectBatch.Visible = false;
                lblSource.Text = "NS";
                cvQty.Enabled = false;
                

            }
            else
            {
                requiredBalance.Enabled = true;
                txtExpirationDate.ReadOnly = true;
                txtBatchNo.ReadOnly = true;
                lnkBatchPIS.Visible = true;
                lnkBtachPRF.Visible = true;
                lnkSelectBatch.Visible = true;
            }

        }

    }
}