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
    public partial class AdjustmentListForPosting : System.Web.UI.Page
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
                        loadUnpostedAdjAllBranch();
                        btnSubmit.Disabled = false;
                    }
                    else
                    {
                        loadPerBranch();
                        loadUnpostedAdjPerBranch();
                        loadControlNoPerBranch();
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


        private void loadUnpostedAdjAllBranch()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.AdjustmentNo
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
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 ORDER BY A.BrCode,A.AdjustmentNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvAdjustment.DataSource = dT;
                    gvAdjustment.DataBind();


                }
            }
        }

        private void loadUnpostedAdjPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.AdjustmentNo
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
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and a.BrCode=@BrCode ORDER BY A.BrCode,A.AdjustmentNo";
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


                }
            }
        }

        private void loadUnpostedAdjPerAdjustmentNo()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.AdjustmentNo
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
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and AdjustmentNo=@AdjustmentNo ORDER BY A.BrCode,A.AdjustmentNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@AdjustmentNo", ddControlNo.SelectedItem.Text);
                    //cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvAdjustment.DataSource = dT;
                    gvAdjustment.DataBind();


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
                    if (Session["vUser_Branch"].ToString() == "1")
                    {
                        loadUnpostedAdjAllBranch();
                    }
                    else
                    {
                        loadUnpostedAdjPerBranch();
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
                ddControlNo.Items.Clear();
            }
            else
            {
                loadControlNoPerBranch();
            }
        }

        private void loadControlNoPerBranch()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT distinct AdjustmentNo
                                  FROM [UnpostedAdjustment]
                                  WHERE BrCode=@BrCode and vStat=1 ORDER BY AdjustmentNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddControlNo.Items.Clear();
                    ddControlNo.DataSource = dR;
                    ddControlNo.DataValueField = "AdjustmentNo";
                    ddControlNo.DataTextField = "AdjustmentNo";
                    ddControlNo.DataBind();
                    ddControlNo.Items.Insert(0, new ListItem("Select Control No.", "0"));
                    ddControlNo.SelectedIndex = 0;



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
                foreach (GridViewRow gvrow in gvAdjustment.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("ckStat");
                    if (chk != null & chk.Checked)
                    {
                        strSRS += "'" + gvAdjustment.DataKeys[gvrow.RowIndex].Value.ToString() + "',";
                    }

                }

                if (strSRS != null)
                {

                    foreach (GridViewRow gvR in gvAdjustment.Rows)
                    {
                        if (gvR.RowType == DataControlRowType.DataRow)
                        {
                            Label lbQty = gvR.Cells[6].FindControl("lblQty") as Label;
                            CheckBox chk = (CheckBox)gvR.FindControl("ckStat");
                            if (chk != null & chk.Checked)
                            {
                                string Rec = gvAdjustment.DataKeys[gvR.RowIndex].Value.ToString();
                                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                                {
                                    string stR = "DBO.PostAdjustment";

                                    sqlConn.Open();

                                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                                    {
                                        cmD.CommandTimeout = 0;
                                        cmD.CommandType = CommandType.StoredProcedure;

                                        cmD.Parameters.AddWithValue("@ID", gvR.Cells[1].Text);
                                        cmD.Parameters.AddWithValue("@vItemID", gvR.Cells[2].Text);
                                        cmD.Parameters.AddWithValue("@vQtyAdjustment", ((Label)gvR.FindControl("lblQty")).Text);
                                        cmD.Parameters.AddWithValue("@PostedBy", Session["FullName"].ToString());

                                        cmD.ExecuteNonQuery();

                                    }
                                }
                            }
                        }
                    }

                    loadControlNoPerBranch();
                    loadUnpostedAdjPerBranchToPost();
                }
                else
                {

                    lblMsgWarning.Text = "Please check at least one item to post.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }


        }

        protected void ddControlNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddControlNo.SelectedValue == "0")
            {
                loadUnpostedAdjPerBranchToPost();
            }
            else
            {
                loadUnpostedAdjPerAdjustmentNo();
            }
        }



        private void loadUnpostedAdjPerBranchToPost()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT A.[ID]
                                      ,A.[vItemID]
                                      ,C.BrName
                                      ,A.AdjustmentNo
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
                                  LEFT JOIN MyBranchList C
                                  ON A.BrCode=C.BrCode
                                  WHERE A.[vStat]=1 and A.BrCode=@BrCode ORDER BY A.BrCode,A.AdjustmentNo";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    // cmD.Parameters.AddWithValue("@vUser_ID", Session["EmpNo"].ToString());
                    cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvAdjustment.DataSource = dT;
                    gvAdjustment.DataBind();


                }
            }
        }

        protected void gvAdjustment_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                if (ddControlNo.SelectedValue == "0")
                {
                    lblMsgWarning.Text = "You must select Adjustment No. on the dropdown list above before editing.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    gvAdjustment.EditIndex = e.NewEditIndex;
                    loadUnpostedAdjPerAdjustmentNo();
                }
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = "You must select Adjustment No. on the dropdown list above before editing.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }

        }

        protected void gvAdjustment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAdjustment.EditIndex = -1;
            loadUnpostedAdjPerAdjustmentNo();
        }

        protected void gvAdjustment_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            string RecNo = gvAdjustment.DataKeys[e.RowIndex].Value.ToString();
            TextBox xtQtyAdjustment = gvAdjustment.Rows[e.RowIndex].FindControl("txtQtyAdjustment") as TextBox;
            TextBox xtRemarks = gvAdjustment.Rows[e.RowIndex].FindControl("txtRemarks") as TextBox;

            UpdateGrid(RecNo, xtQtyAdjustment, xtRemarks);




        }

        private void UpdateGrid(string RecNo, TextBox xtQtyAdjustment, TextBox xtRemarks)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"Update UnpostedAdjustment set vQty=@vQty,Remarks=@Remarks where ID=@vRecNum";

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        sqlConn.Open();
                        cmD.Parameters.AddWithValue("@vQty", xtQtyAdjustment.Text);
                        cmD.Parameters.AddWithValue("@Remarks", xtRemarks.Text);
                        cmD.Parameters.AddWithValue("@vRecNum", RecNo);
                        cmD.ExecuteNonQuery();
                    }

                    gvAdjustment.EditIndex = -1;
                    loadUnpostedAdjPerAdjustmentNo();
                }
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message + "\nPlease check character encoded.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        protected void gvAdjustment_RowDataBound(object sender, GridViewRowEventArgs e)
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