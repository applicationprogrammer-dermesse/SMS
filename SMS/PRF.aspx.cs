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
    public partial class PRF : System.Web.UI.Page
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

                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        Response.Redirect("~/UnauthorizedPage.aspx");
                    }
                    else
                    {
                        loadPRFNo();
                        LoadFGItem();
                        LoadBrName();
                        loadUnposted();
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


        private void loadPRFNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CONCAT('PRF-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('00000'+CAST(PRFno AS VARCHAR(8)),8)) FROM SystemMaster where [BrCode]='" + Session["vUser_Branch"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtPRFNo.Text = dR[0].ToString();
                    }

                }
            }
        }


        private void LoadFGItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT vFGCode,LTRIM(RTRIM(vPluCode)) + '-' + LTRIM(RTRIM(vDESCRIPTION)) AS [vDESCRIPTION] FROM vItemMaster where vStat=1 and ItemType='Product'";
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


        private void LoadBrName()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [BrCode]
                                  ,[BrName]
                              FROM [MyBranchList] where BrCode <=29 and BrCode not in (10) order by BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    ddBranch.Items.Insert(0, new ListItem("Please select branch", "0"));
                    ddBranch.SelectedIndex = 0;
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


                    else
                    {

                        CheckIfExistsPRFno();
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

        private void CheckIfExistsPRFno()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT PRFno FROM PostedPRF WHERE PRFno=@PRFno";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@PRFno", txtPRFNo.Text);

                        SqlDataReader dR = cmD.ExecuteReader();

                        if (dR.HasRows)
                        {
                            while (dR.Read())
                            {
                                txtPRFNo.Focus();
                                lblMsgWarning.Text = "PRF number " + txtPRFNo.Text + " already exists";
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
                    string stR = @"INSERT INTO UnpostedPRF
                        ([PRFno]
                       ,BrCode
                      ,[PRFdate]
                       ,vItemID
                      ,[vFGCode]
                      ,[vQty]
                        ,vBatchNo
                        ,vDateExpiry
                       ,vStat
                       ,ReasonOfReturn
                      ,[vUser_ID]
                      ,[TransactionDate]
                        ,TargetBR)
                    VALUES 
                     (@PRFno
                      ,@BrCode
                      ,@PRFdate
                        ,@vItemID
                      ,@vFGCode
                      ,@vQty
                        ,@vBatchNo
                        ,@vDateExpiry
                      ,@vStat
                       ,@ReasonOfReturn
                      ,@vUser_ID
                      ,@TransactionDate
                       ,@TargetBR)";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@PRFdate", txtDateReceived.Text);
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@vItemID", txtItemID.Text);
                        cmD.Parameters.AddWithValue("@vStat", 0);
                        cmD.Parameters.AddWithValue("@ReasonOfReturn", txtReason.Text);
                        cmD.Parameters.AddWithValue("@PRFno", txtPRFNo.Text);
                        cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                        cmD.Parameters.AddWithValue("@vQty", txtQty.Text);
                        cmD.Parameters.AddWithValue("@vBatchNo",txtBatchNo.Text);
                        cmD.Parameters.AddWithValue("@vDateExpiry", txtExpirationDate.Text);
                        cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                        cmD.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                        cmD.Parameters.AddWithValue("@TargetBR", ddBranch.SelectedValue);
                        cmD.ExecuteNonQuery();
                    }


                    //txtBatchNo.Text = "";
                    //txtDateExpiry.Text = "";



                }
                LoadFGItem();
                txtItemID.Text = "";
                txtAvailable.Text = "";
                txtQty.Text = "";


                loadUnposted();

            }
        }

        private void loadUnposted()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT a.ID
                              ,a.PRFno
                              ,a.PRFdate
                              ,a.vFGCode
	                          ,b.vDESCRIPTION
                              ,a.vBatchNo
                               ,a.vDateExpiry
                              ,a.vQty
                              ,a.ReasonOfReturn
                          ,c.BrName
                          FROM UnpostedPRF a
                           LEFT JOIN vItemMaster b
	                        ON a.[vFGCode]=b.[vFGCode]
						 LEFT JOIN MyBranchList c
						   ON a.TargetBr = c.BrCode
                            WHERE a.BrCode=@BrCode AND a.vStat=0
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
                        string stR = @"DELETE FROM UnpostedPRF where ID=@ID";
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
            TextBox xtReasonOfReturn = gvUnposted.Rows[e.RowIndex].FindControl("txtReasonOfReturn") as TextBox;
            Label bllblQtyOrig = gvUnposted.Rows[e.RowIndex].FindControl("lblQtyOrig") as Label;

            TextBox xtDateExpiryDR = gvUnposted.Rows[e.RowIndex].FindControl("txtDateExpiryDR") as TextBox;
            TextBox xtBatchNoDR = gvUnposted.Rows[e.RowIndex].FindControl("txtBatchNoDR") as TextBox;

            UpdateGrid(RecNo, xtQtyDR, xtReasonOfReturn, xtDateExpiryDR,xtBatchNoDR);

            //if (Convert.ToInt32(xtQtyDR.Text) <= Convert.ToInt32(bllblQtyOrig.Text))
            //{
            //    UpdateGrid(RecNo, xtQtyDR, xtReasonOfReturn);
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
            //                    UpdateGrid(RecNo, xtQtyDR, xtReasonOfReturn);
            //                }
            //            }
            //        }
            //    }


            //}


        }

        private void UpdateGrid(string RecNo, TextBox xtQtyDR, TextBox xtReasonOfReturn, TextBox xtDateExpiryDR, TextBox xtBatchNoDR)
        {
            using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"Update UnpostedPRF set vQty=@vQty,ReasonOfReturn=@ReasonOfReturn,vBatchNo=@vBatchNo,vDateExpiry=@vDateExpiry where ID=@vRecNum";

                using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                {
                    sqlConn.Open();
                    cmD.Parameters.AddWithValue("@vQty", xtQtyDR.Text);
                    cmD.Parameters.AddWithValue("@ReasonOfReturn", xtReasonOfReturn.Text);
                    cmD.Parameters.AddWithValue("@vBatchNo",xtBatchNoDR.Text);
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
                                string stR = @"dbo.SubmitUnpostedPRF";
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
                    txtPRFNo.Text = "";
                    UpdatePRFNo();
                    loadPRFNo();
                    loadUnposted();
                    btnSubmit.Disabled = true;
                    lblMsgWarning.Text = "PRF succesfully send to HO for posting!";
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


        private void UpdatePRFNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET PRFno=PRFno + 1 WHERE BrCode=@BrCode";
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
            getBalance();
        }


    }
}