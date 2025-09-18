using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
//using System.IO;
//using System.Linq;
//using System.Threading;


namespace SMS
{
    public partial class ProcessEndOfDay : System.Web.UI.Page
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
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
                        lblBranch.Text = Session["Dept"].ToString();

                        CheckIfThereArePendingTrnsaction();
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

        private void CheckIfThereArePendingTrnsaction()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"SELECT BrCode FROM UnpostedSalesDetailed WHERE BrCode=@BrCode and vStat=0";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {

                        lblMsgWithPending.Text = "There are pending transaction on the system, You will be redirected to finish the transaction!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsgWithPending();", true);
                        return;
                    }
                    else
                    {
                        LoadSystemDate();
                        loadSalesForTheDay();
                        loadSalesKitForTheDay();
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
                        lblDate.Text = Convert.ToDateTime(dR[0].ToString()).ToShortDateString();
                    }

                }
            }
        }

        private void loadSalesKitForTheDay()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT a.[ID]
                              ,a.[SalesDetailedID]
                              ,a.[SalesDate]
                              ,a.[ReceiptNo]
                              ,a.[PromoCode]
                              ,a.[vFGCode]
	                          ,b.vDESCRIPTION
                              ,ISNULL(a.[vQty],0) * ISNULL(a.[Sales_qty],0) AS [Sales_qty]
                              ,ISNULL(a.[vQty],0) * ISNULL(a.[Free_qty],0) AS [Free_qty]
                          FROM [SalesKitItemDetailed] a
                          LEFT JOIN vItemMaster B
                          ON A.vFGCode=B.vFGCode
                          WHERE a.[BrCode]=@BrCode AND a.vStat=1
                          ORDER BY a.[ID]";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvKit.DataSource = dT;
                    gvKit.DataBind();


                }
            }
        }

        private void loadSalesForTheDay()
        {

            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [RecID]
                                  ,[vItemID]
                                  ,[SalesDate]
                                  ,[CustomerName]
                                  ,[ReceiptNo]
                                  ,[vFGCode]
                                  ,[ItemDescription]
	                              ,CASE WHEN vStat=3 THEN 0
		                            ELSE [vQty] END AS [Qty]
      
                                    ,CASE WHEN vStat=3 THEN 0
		                            ELSE [NetAmount] END AS [Net Amount]
	                                ,CASE WHEN vStat=3 THEN 'Void'
	                                ELSE 'Good' END AS [Status]
                              FROM  [UnpostedSalesDetailed]
                              WHERE [BrCode]=@BrCode and vStat <> 0
                              ORDER BY [RecID]";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvSalesForTheDay.DataSource = dT;
                    gvSalesForTheDay.DataBind();


                }
            }
        }

        protected void gvSalesForTheDay_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                // when mouse is over the row, save original color to new attribute, and change it to highlight color
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#EEFFAA'");

                // when mouse leaves the row, change the bg color to its original value  
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;");
                if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Status")) == "Void")
                {

                    e.Row.ForeColor = System.Drawing.Color.Red;
                    e.Row.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                }
            }
        }


        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (gvSalesForTheDay.Rows.Count == 0)
                {
                    //updateSalesDate();
                    lblMsgNoSales.Text = "No Transaction record for the day.\n Proceeds to change transaction date?";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowNoSalesMsg();", true);
                    return;
                }
                else
                {
                    updatePrevGT();
                    ProceedWithEndOfDay();
                }
            }
        }


        private void updatePrevGT()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.UpdatePrevGT";

                    sqlConn.Open();

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@SalesDate", lblDate.Text);
                        cmD.ExecuteNonQuery();

                    }
                }
            }
        }

        private void ProceedWithEndOfDay()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {

                btnPost.Disabled = true;

                foreach (GridViewRow gvR in gvSalesForTheDay.Rows)
                {
                    if (gvR.RowType == DataControlRowType.DataRow)
                    {
                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                        {
                            string stR = "dbo.ProcessEndOfDay";
                            sqlConn.Open();
                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {

                                try
                                {

                                    cmD.CommandTimeout = 0;
                                    cmD.CommandType = CommandType.StoredProcedure;

                                    cmD.Parameters.AddWithValue("@RecID", gvR.Cells[0].Text);
                                    cmD.Parameters.AddWithValue("@ItemID", gvR.Cells[1].Text);
                                    cmD.Parameters.AddWithValue("@Qty", ((Label)gvR.FindControl("lblQty")).Text);
                                    cmD.Parameters.AddWithValue("@ReceiptNo", gvR.Cells[4].Text);
                                    cmD.Parameters.AddWithValue("@PostedBy", Session["EmpNo"].ToString());
                                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                                    cmD.ExecuteNonQuery();

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

                if (gvKit.Rows.Count > 0)
                {
                    ProcessKitItemsForTheDay();

                }
                else
                {
                    updateSalesDate();
                    gvSalesForTheDay.DataSource = null;
                    gvSalesForTheDay.DataBind();

                }

            }
        }

        private void updateSalesDate()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.SaveDailyInventory";

                    sqlConn.Open();

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@SalesDate", lblDate.Text);
                        cmD.Parameters.AddWithValue("@vUserID", Session["EmpNo"].ToString());
                        cmD.ExecuteNonQuery();

                    }
                }

                lblMsgWarning.Text = "End of day succesfully process";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }
        private void ProcessKitItemsForTheDay()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {


                foreach (GridViewRow gvRkit in gvKit.Rows)
                {
                    if (gvRkit.RowType == DataControlRowType.DataRow)
                    {
                        using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                        {
                            string stR = "dbo.PostUnpostedSalesKit";

                            sqlConn.Open();

                            using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                            {
                                cmD.CommandTimeout = 0;
                                cmD.CommandType = CommandType.StoredProcedure;

                                cmD.Parameters.AddWithValue("@RecID", gvRkit.Cells[0].Text);
                                cmD.Parameters.AddWithValue("@FGCode", gvRkit.Cells[5].Text);
                                cmD.Parameters.AddWithValue("@Qty", ((Label)gvRkit.FindControl("lblSales_qty")).Text);
                                cmD.Parameters.AddWithValue("@FreeQty", ((Label)gvRkit.FindControl("lblFree_qty")).Text);
                                cmD.Parameters.AddWithValue("@PostedBy", Session["EmpNo"].ToString());
                                cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                                cmD.ExecuteNonQuery();

                            }
                        }

                    }
                }

                updateSalesDate();


            }
        }

        protected void btnProceedsWithProcess_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                updateSalesDate();
            }
        }



        protected void btnPrintX_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>window.open ('XReading.aspx?Date=" + lblDate.Text + "','_blank');</script>");

            string url = "XReading.aspx?Date=" + lblDate.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void btnPrintZ_Click(object sender, EventArgs e)
        {

           // updatePrevGT();
          //  ProceedWithEndOfDay();

            //Response.Write("<script>window.open ('ZReading.aspx?Date=" + lblDate.Text + "','_blank');</script>");
            string url = "ZReading.aspx?Date=" + lblDate.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);

            

        }

    }
}