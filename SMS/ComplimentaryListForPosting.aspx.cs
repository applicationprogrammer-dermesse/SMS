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
    public partial class ComplimentaryListForPosting : System.Web.UI.Page
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
                        loadAllBranch();
                        loadUnpostedComplimentaryAllBranch();
                        btnSubmit.Disabled = false;
                    }
                    else
                    {
                        loadPerBranch();
                        loadControlNoPerBranch();
                        loadUnpostedComplimentaryPerBranch();
                        btnSubmit.Disabled = true;
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
                string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode >=4  ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
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


        private void loadUnpostedComplimentaryAllBranch()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.Complimentaryno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                                      ,case when a.CompliType <> 1 then A.Remarks
									  ELSE D.CompliName + '-' + a.CompliCustomerName END AS [Remarks]
									   ,f.CompliType
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  FROM [UnpostedComplimentary] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								  LEFT JOIN CompliList D
								  ON A.CompliID=D.CompliID
								  LEFT JOIN CompliType f
							      ON a.CompliType= f.id
                                  WHERE A.[vStat]=1 ORDER BY A.BrCode,A.Complimentaryno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvComplimentary.DataSource = dT;
                    gvComplimentary.DataBind();


                }
            }
        }

        private void loadUnpostedComplimentaryPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.Complimentaryno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                                      ,case when a.CompliType <> 1 then A.Remarks
									  ELSE D.CompliName + '-' + a.CompliCustomerName END AS [Remarks]
									   ,f.CompliType
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  FROM [UnpostedComplimentary] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								  LEFT JOIN CompliList D
								  ON A.CompliID=D.CompliID
								  LEFT JOIN CompliType f
							      ON a.CompliType= f.id
                                  WHERE A.[vStat]=1 and a.BrCode=@BrCode ORDER BY A.BrCode,A.Complimentaryno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvComplimentary.DataSource = dT;
                    gvComplimentary.DataBind();


                }
            }
        }

        private void loadUnpostedComplimentaryPerComplimentaryNo()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.Complimentaryno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                                      ,case when a.CompliType <> 1 then A.Remarks
									  ELSE D.CompliName + '-' + a.CompliCustomerName END AS [Remarks]
									   ,f.CompliType
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  FROM [UnpostedComplimentary] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								  LEFT JOIN CompliList D
								  ON A.CompliID=D.CompliID
								  LEFT JOIN CompliType f
							      ON a.CompliType= f.id
                                  WHERE A.[vStat]=1 and Complimentaryno=@Complimentaryno ORDER BY A.BrCode,A.Complimentaryno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@Complimentaryno", ddComplimentaryno.SelectedItem.Text);
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvComplimentary.DataSource = dT;
                    gvComplimentary.DataBind();


                }
            }
        }


        protected void gvComplimentary_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    string dKey = gvComplimentary.DataKeys[e.RowIndex].Value.ToString();
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

                    gvComplimentary.EditIndex = -1;
                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        loadUnpostedComplimentaryAllBranch();
                    }
                    else
                    {
                        loadUnpostedComplimentaryPerBranch();
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
                ddComplimentaryno.Items.Clear();
                loadUnpostedComplimentaryAllBranch();
            }
            else
            {
                loadControlNoPerBranch();
                loadUnpostedComplimentaryPerBranch();
            }
        }

        private void loadControlNoPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT distinct Complimentaryno
                                  FROM [UnpostedComplimentary]
                                  WHERE BrCode=@BrCode and vStat=1 ORDER BY Complimentaryno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddComplimentaryno.Items.Clear();
                    ddComplimentaryno.DataSource = dR;
                    ddComplimentaryno.DataValueField = "Complimentaryno";
                    ddComplimentaryno.DataTextField = "Complimentaryno";
                    ddComplimentaryno.DataBind();
                    ddComplimentaryno.Items.Insert(0, new ListItem("Select Control No.", "0"));
                    ddComplimentaryno.SelectedIndex = 0;



                }
            }

        }

        public string strSRS;
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                foreach (GridViewRow gvrow in gvComplimentary.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRS += "'" + gvComplimentary.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRS != null)
                {

                    foreach (GridViewRow gvR in gvComplimentary.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            Label lbQty = gvR.Cells[6].FindControl("lblQty") as Label;
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            if (chk != null & chk.Checked)
                            {
                                string Rec = gvComplimentary.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "dbo.PostUnpostedComplimentary";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@vItemID", gvR.Cells[2].Text);
                                        cmD.Parameters.AddWithValue("@vQtyComplimentary", ((Label)gvR.FindControl("lblQty")).Text);
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["FullName"].ToString());

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }


                    //loadControlNoPerBranch();
                    //loadUnpostedComplimentaryPerBranchToPost();
                    loadAllBranch();
                    ddComplimentaryno.Items.Clear();
                    loadUnpostedComplimentaryAllBranch();

                    lblMsgWarning.Text = "Complimentary Successfully Posted";
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

        protected void ddComplimentaryno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddComplimentaryno.SelectedValue == "0")
            {
                loadUnpostedComplimentaryPerBranchToPost();
            }
            else
            {
                loadUnpostedComplimentaryPerComplimentaryNo();
            }
        }



        private void loadUnpostedComplimentaryPerBranchToPost()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.Complimentaryno
                                      ,A.[vFGCode]
	                                  ,b.vDESCRIPTION
                                      ,A.[vQty]
                                      ,case when CompliType = 0 then A.Remarks
									  ELSE D.CompliName + '-' + a.CompliCustomerName END AS [Remarks]
                                      ,A.[vUser_ID]
                                      ,A.[TransactionDate]
                                  FROM [UnpostedComplimentary] A
                                  LEFT JOIN vItemMaster B
                                  ON A.vFGCode=b.vFGCode
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
								  LEFT JOIN CompliList D
								  ON A.CompliID=D.CompliID
                                  WHERE A.[vStat]=1 and A.BrCode=@BrCode ORDER BY A.BrCode,A.Complimentaryno";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvComplimentary.DataSource = dT;
                    gvComplimentary.DataBind();


                }
            }
        }

        protected void gvComplimentary_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (ddComplimentaryno.SelectedValue == "0")
                {
                    lblMsgWarning.Text = "You must select Complimentary No. on the dropdown list above before editing.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    gvComplimentary.EditIndex = e.NewEditIndex;
                    loadUnpostedComplimentaryPerComplimentaryNo();
                }
            }

            catch (Exception x)
            {
                lblMsgWarning.Text = "You must select Complimentary No. on the dropdown list above before editing.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }


            //}
            //else
            //{
            //    loadUnpostedComplimentaryPerBranch();
            //}
        }

        protected void gvComplimentary_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvComplimentary.EditIndex = -1;
            loadUnpostedComplimentaryPerComplimentaryNo();
            //if (Session["vUser_Branch"].ToString() == "1")
            //{
            //    loadUnpostedComplimentaryAllBranch();
            //}
            //else
            //{
            //    loadUnpostedComplimentaryPerBranch();
            //}
        }

        protected void gvComplimentary_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string RecNo = gvComplimentary.DataKeys[e.RowIndex].Value.ToString();
            TextBox xtQtyDR = gvComplimentary.Rows[e.RowIndex].FindControl("txtQtyDR") as TextBox;
            //TextBox xtRemarks = gvComplimentary.Rows[e.RowIndex].FindControl("txtRemarks") as TextBox;
            Label bllblQtyOrig = gvComplimentary.Rows[e.RowIndex].FindControl("lblQtyOrig") as Label;

            UpdateGrid(RecNo, xtQtyDR);

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

        private void UpdateGrid(string RecNo, TextBox xtQtyDR)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"Update UnpostedComplimentary set vQty=@vQty,Remarks=@Remarks where ID=@vRecNum";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.Parameters.AddWithValue("@vQty", xtQtyDR.Text);
                        //cmD.Parameters.AddWithValue("@Remarks", xtRemarks.Text);
                        cmD.Parameters.AddWithValue("@vRecNum", RecNo);
                        cmD.ExecuteNonQuery();
                    }

                    gvComplimentary.EditIndex = -1;
                    loadUnpostedComplimentaryPerComplimentaryNo();
                    //if (Session["vUser_Branch"].ToString() == "1")
                    //{
                    //    loadUnpostedComplimentaryAllBranch();
                    //}
                    //else
                    //{
                    //    loadUnpostedComplimentaryPerBranch();
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

        protected void gvComplimentary_RowDataBound(object sender, GridViewRowEventArgs e)
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