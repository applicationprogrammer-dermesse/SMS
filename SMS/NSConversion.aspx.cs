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
    public partial class NSConversion : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
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
                    loadAdjustmentNo();
                    LoadNSItem();
                    loadUnpostedAdj();

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

        private void loadAdjustmentNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CONCAT('ADJ-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('00000'+CAST(AdjustmentNo AS VARCHAR(8)),8)) FROM SystemMaster where [BrCode]='" + Session["vUser_Branch"].ToString() + "'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        txtAdjustmentNo.Text = dR[0].ToString();
                    }

                }
            }
        }

        private void LoadNSItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ListNSItems";
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

        protected void ddITemFG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddITemFG.SelectedIndex == 0)
            {
                txtItemID.Text = "";
                txtAvailable.Text = "";
            }
            else
            {
                getBalance();
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
        protected void lnkSelect_Click(object sender, EventArgs e)
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ShowItemBatchBalance";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@vFGcode", ddITemFG.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvItemBatch.DataSource = dT;
                    gvItemBatch.DataBind();

                    if (gvItemBatch.Rows.Count == 0)
                    {
                        lblShowItemBatchesHeader.Text = "No available balance!";
                        txtAvailable.Text = "0";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatches();", true);
                        return;
                    }
                    else
                    {
                        lblShowItemBatchesHeader.Text = "List of item batches";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridItemBatches();", true);
                        return;
                    }
                }
            }

        }

        protected void gvItemBatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {
                    //string dKey = gvItemBatch.DataKeys[e.RowIndex].Value.ToString();
                    GridViewRow row = gvItemBatch.Rows[e.RowIndex];
                    Label lblExp = gvItemBatch.Rows[e.RowIndex].FindControl("lbDateExpiryIssue") as Label;

                    string theBalance = gvItemBatch.Rows[e.RowIndex].Cells[3].Text;
                    string theID = gvItemBatch.Rows[e.RowIndex].Cells[5].Text;

                    passString(theBalance, theID);

                    //txtAvailable.Text = theBalance.ToString(); //lblExp.Text;// row.Cells[3].Text;
                    //txtItemID.Text = row.Cells[5].Text;
                    //lblID.Text = row.Cells[5].Text;        


                }
                catch (Exception X)
                {
                    lblMsgWarning.Text = X.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }

            }
        }

        public void passString(string bal, string id)
        {
            if (bal == "0")
            {
                txtAvailable.Text = "0";

            }
            else
            {
                txtAvailable.Text = bal.ToString(); //lblExp.Text;// row.Cells[3].Text;
                txtItemID.Text = id.ToString();

            }
        }
        protected void gvItemBatch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void gvItemBatch_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

                if (Convert.ToDecimal(e.Row.Cells[3].Text) <= 0)
                {
                    ((LinkButton)e.Row.FindControl("lnkSelectItemBatch")).ForeColor = System.Drawing.Color.Gray;
                    ((LinkButton)e.Row.FindControl("lnkSelectItemBatch")).Enabled = false;
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
                if (Convert.ToInt32(txtAvailable.Text) == 0 & Convert.ToInt32(txtQty.Text) <= 0)
                {
                    lblMsgWarning.Text = "Minus adjustment is not allowed for item with 0 available balance!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {


                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"INSERT INTO UnpostedAdjustment
                        ([vStat],BrCode,AdjustmentNo
                          ,[vItemID]
                          ,[vFGCode]
                          ,[vQtyBalance]
                          ,[vQty]
                          ,Remarks
                          ,[vUser_ID]
                          ,[TransactionDate])
                    VALUES 
                     (@vStat,@BrCode,@AdjustmentNo
                          ,@vItemID
                          ,@vFGCode
                          ,@vQtyBalance
                          ,@vQty
                           ,@Remarks
                      ,@vUser_ID
                      ,@TransactionDate)";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@vStat", 0);
                            cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                            cmD.Parameters.AddWithValue("@AdjustmentNo", txtAdjustmentNo.Text);
                            cmD.Parameters.AddWithValue("@vItemID", txtItemID.Text);
                            cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                            cmD.Parameters.AddWithValue("@vQtyBalance", txtAvailable.Text);
                            cmD.Parameters.AddWithValue("@vQty", txtQty.Text);
                            cmD.Parameters.AddWithValue("@Remarks", txtReason.Text);
                            cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                            cmD.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
                            cmD.ExecuteNonQuery();
                        }


                    }

                    LoadNSItem();
                    txtQty.Text = "";
                    txtAvailable.Text = "";
                    txtItemID.Text = "";
                    loadUnpostedAdj();
                }
            }
        }



        private void loadUnpostedAdj()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQtyBalance]
                                      ,A.[vQty]
                                      ,A.Remarks
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  FROM [UnpostedAdjustment] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  WHERE A.[vStat]=0 and a.BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvAdjustment.DataSource = dT;
                    gvAdjustment.DataBind();

                    if (gvAdjustment.Rows.Count == 0)
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
                if (gvAdjustment.Rows.Count == 0)
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

                    foreach (GridViewRow gvR in gvAdjustment.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                           // Label lbQty = gvR.Cells[6].FindControl("lblQty") as Label;
                            string Rec = gvAdjustment.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "DBO.PostConversion";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[0].Text);
                                        cmD.Parameters.AddWithValue("@vItemID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@vQtyAdjustment", ((Label)gvR.FindControl("lblQty")).Text);
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["FullName"].ToString());

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
 
                    }


                    UpdateAdjustmentNo();
                    loadAdjustmentNo();
                    LoadNSItem();
                    txtAvailable.Text = "";
                    txtItemID.Text = "";
                    txtQty.Text = "";
                    txtReason.Text = "";

                    gvAdjustment.DataSource = null;
                    gvAdjustment.DataBind();

                    btnSubmit.Disabled = true;
                    lblMsgWarning.Text = "Adjustment succesfully posted";
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

        private void UpdateAdjustmentNo()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {


                string stR = @"UPDATE SystemMaster SET AdjustmentNo=AdjustmentNo + 1 WHERE BrCode=@BrCode";
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

        protected void gvAdjustment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    string dKey = gvAdjustment.DataKeys[e.RowIndex].Value.ToString();
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"DELETE FROM UnpostedAdjustment where ID=@ID";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@ID", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    gvAdjustment.EditIndex = -1;
                    loadUnpostedAdj();
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }



    }
}