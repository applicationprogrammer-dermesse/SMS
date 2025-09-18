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
    public partial class PRFListForPosting : System.Web.UI.Page
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

                    DayOfWeek today = DateTime.Now.DayOfWeek;
                    bool isWeekend = (today == DayOfWeek.Saturday || today == DayOfWeek.Sunday);

                    if (Session["vUser_Branch"].ToString() == "1"
                        || (Session["vUser_Branch"].ToString() == "11" && Session["EmpNo"].ToString() == "05433")
                        || (Session["vUser_Branch"].ToString() == "4" && Session["EmpNo"].ToString() == "05439")
                        || (Session["vUser_Branch"].ToString() == "17" && Session["EmpNo"].ToString() == "05430"))
                    {
                        loadAllBranch();
                        loadUnpostedPRFAllBranch();
                        //btnSubmit.Disabled = false;
                    }
                    else
                    {
                        loadPerBranch();
                        loadControlNoPerBranch();
                        loadUnpostedPRFPerBranch();
                        //btnSubmit.Disabled = true;
                    }



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

        private void loadPerBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode='" + Session["vUser_Branch"].ToString() + "'  ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    //ddBranch.Items.Insert(0, new ListItem("Select branch", "0"));
                    //ddBranch.SelectedIndex = 0;
                }
            }
        }


        private void loadUnpostedPRFAllBranch()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.PRFno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                                  ,a.vBatchNo
                                   ,a.vDateExpiry

                                      ,A.ReasonOfReturn
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  ,D.BrName AS [TargetBR]
                                  FROM [UnpostedPRF] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								    LEFT JOIN MyBranchList D
                                  ON A.TargetBr=D.BrCode
                                  WHERE A.[vStat]=1  and A.TargetBr=1 ORDER BY A.BrCode,A.PRFno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvPRF.DataSource = dT;
                    gvPRF.DataBind();


                }
            }
        }

        private void loadUnpostedPRFPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.PRFno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                              ,a.vBatchNo
                               ,a.vDateExpiry
                                      ,A.ReasonOfReturn
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                   ,D.BrName AS [TargetBR]
                                  FROM [UnpostedPRF] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								    LEFT JOIN MyBranchList D
                                  ON A.TargetBr=D.BrCode
                                  WHERE A.[vStat]=1 and a.TargetBr=@BrCode ORDER BY A.BrCode,A.PRFno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvPRF.DataSource = dT;
                    gvPRF.DataBind();


                }
            }
        }

        private void loadUnpostedPRFPerPRFNo()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.PRFno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                              ,a.vBatchNo
                               ,a.vDateExpiry

                                      ,A.ReasonOfReturn
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  ,D.BrName AS [TargetBR]
                                  FROM [UnpostedPRF] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								    LEFT JOIN MyBranchList D
                                  ON A.TargetBr=D.BrCode
                                  WHERE A.[vStat]=1 and PRFno=@PRFno ORDER BY A.BrCode,A.PRFno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@PRFno", ddPRFno.SelectedItem.Text);
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvPRF.DataSource = dT;
                    gvPRF.DataBind();


                }
            }
        }


        protected void gvPRF_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    string dKey = gvPRF.DataKeys[e.RowIndex].Value.ToString();
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

                    gvPRF.EditIndex = -1;
                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        loadUnpostedPRFAllBranch();
                    }
                    else
                    {
                        loadUnpostedPRFPerBranch();
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

        protected void ddBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddBranch.SelectedValue == "0")
            {
                ddPRFno.Items.Clear();
                loadUnpostedPRFAllBranch();
            }
            else
            {
                loadControlNoPerBranch();
                loadUnpostedPRFPerBranch();
            }
        }

        private void loadControlNoPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT distinct PRFno
                                  FROM [UnpostedPRF]
                                  WHERE TargetBr=@BrCode and vStat=1 ORDER BY PRFno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddPRFno.Items.Clear();
                    ddPRFno.DataSource = dR;
                    ddPRFno.DataValueField = "PRFno";
                    ddPRFno.DataTextField = "PRFno";
                    ddPRFno.DataBind();
                    ddPRFno.Items.Insert(0, new ListItem("Select Control No.", "0"));
                    ddPRFno.SelectedIndex = 0;



                }
            }

        }

        //

        public string strSRS;
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvrow in gvPRF.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRS += "'" + gvPRF.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRS != null)
                {

                    foreach (GridViewRow gvR in gvPRF.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            //CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            //Label lbQty = gvR.Cells[6].FindControl("lblQty") as Label;
                            //Label lblBatchNoDR = gvR.Cells[6].FindControl("lblBatchNoDR") as Label;
                            //Label lblvDateExpiry = gvR.Cells[6].FindControl("lblvDateExpiry") as Label;
                            Label lbQty = (Label)gvR.FindControl("lblQty") as Label;
                            Label lblBatchNoDR = (Label)gvR.FindControl("lblBatchNoDR") as Label;
                            Label lblvDateExpiry = (Label)gvR.FindControl("lblvDateExpiry") as Label;
                            
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            if (chk != null & chk.Checked)
                            {
                                string Rec = gvPRF.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "dbo.PostUnpostedPRF";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@vItemID", gvR.Cells[2].Text);
                                        cmD.Parameters.AddWithValue("@vQtyPRF", lbQty.Text);
                                        cmD.Parameters.AddWithValue("@vBatchNo", lblBatchNoDR.Text);
                                        cmD.Parameters.AddWithValue("@vDateExpiry", lblvDateExpiry.Text);
                                        cmD.Parameters.AddWithValue("@vFGCode", gvR.Cells[5].Text);
                                        
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["FullName"].ToString());
                                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }


                    //loadControlNoPerBranch();
                    //loadUnpostedPRFPerBranchToPost();
                    //loadAllBranch();
                    //ddPRFno.Items.Clear();
                    //loadUnpostedPRFAllBranch();


                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        loadAllBranch();
                        loadUnpostedPRFAllBranch();
                        //btnSubmit.Disabled = false;
                    }
                    else
                    {
                        loadPerBranch();
                        loadControlNoPerBranch();
                        loadUnpostedPRFPerBranch();
                        //btnSubmit.Disabled = true;
                    }


                    lblMsgWarning.Text = "PRF Successfully Posted";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;

                }
                else
                {

                    lblMsgWarning.Text = "Please check at least one item to post.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }


        }

        protected void ddPRFno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddPRFno.SelectedValue == "0")
            {
                loadUnpostedPRFPerBranchToPost();
            }
            else
            {
                loadUnpostedPRFPerPRFNo();
            }
        }



        private void loadUnpostedPRFPerBranchToPost()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.PRFno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                              ,a.vBatchNo
                               ,a.vDateExpiry

                                      ,A.ReasonOfReturn
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  FROM [UnpostedPRF] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and A.BrCode=@BrCode ORDER BY A.BrCode,A.PRFno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvPRF.DataSource = dT;
                    gvPRF.DataBind();


                }
            }
        }

        protected void gvPRF_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (ddPRFno.SelectedValue == "0")
                {
                    lblMsgWarning.Text = "You must select PRF No. on the dropdown list above before editing.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    gvPRF.EditIndex = e.NewEditIndex;
                    loadUnpostedPRFPerPRFNo();
                }
            }

            catch (Exception x)
            {
                lblMsgWarning.Text = "You must select PRF No. on the dropdown list above before editing.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }


            //}
            //else
            //{
            //    loadUnpostedPRFPerBranch();
            //}
        }

        protected void gvPRF_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvPRF.EditIndex = -1;
            loadUnpostedPRFPerPRFNo();
            //if (Session["vUser_Branch"].ToString() == "1")
            //{
            //    loadUnpostedPRFAllBranch();
            //}
            //else
            //{
            //    loadUnpostedPRFPerBranch();
            //}
        }

        protected void gvPRF_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string RecNo = gvPRF.DataKeys[e.RowIndex].Value.ToString();
            TextBox xtQtyDR = gvPRF.Rows[e.RowIndex].FindControl("txtQtyDR") as TextBox;
            TextBox xtReasonOfReturn = gvPRF.Rows[e.RowIndex].FindControl("txtReasonOfReturn") as TextBox;
            Label bllblQtyOrig = gvPRF.Rows[e.RowIndex].FindControl("lblQtyOrig") as Label;
            TextBox xtDateExpiryDR = gvPRF.Rows[e.RowIndex].FindControl("txtDateExpiryDR") as TextBox;
            TextBox xtBatchNoDR = gvPRF.Rows[e.RowIndex].FindControl("txtBatchNoDR") as TextBox;

            UpdateGrid(RecNo, xtQtyDR, xtReasonOfReturn, xtDateExpiryDR, xtBatchNoDR);

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
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"Update UnpostedPRF set vQty=@vQty,ReasonOfReturn=@ReasonOfReturn,vBatchNo=@vBatchNo,vDateExpiry=@vDateExpiry where ID=@vRecNum";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.Parameters.AddWithValue("@vQty", xtQtyDR.Text);
                        cmD.Parameters.AddWithValue("@ReasonOfReturn", xtReasonOfReturn.Text);
                        cmD.Parameters.AddWithValue("@vBatchNo", xtBatchNoDR.Text);
                        cmD.Parameters.AddWithValue("@vDateExpiry", xtDateExpiryDR.Text);
                        cmD.Parameters.AddWithValue("@vRecNum", RecNo);
                        cmD.ExecuteNonQuery();
                    }

                    gvPRF.EditIndex = -1;
                    loadUnpostedPRFPerPRFNo();
                    //if (Session["vUser_Branch"].ToString() == "1")
                    //{
                    //    loadUnpostedPRFAllBranch();
                    //}
                    //else
                    //{
                    //    loadUnpostedPRFPerBranch();
                    //}

                }
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message + "\nPlease check character encoded.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        protected void gvPRF_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");

                if (Session["vUser_Branch"].ToString() == "1")
                {
                    ((LinkButton)e.Row.FindControl("lnkEditGrid")).ForeColor = System.Drawing.Color.Gray;
                    ((LinkButton)e.Row.FindControl("lnkEditGrid")).Enabled = false;
                }
            }
        }


    }
}