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

namespace SMS
{
    public partial class MonthlyPosting : System.Web.UI.Page
    {
        public string filenameOfFile;
        public string newFileName;
        public string xlsHeader;

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

                        loadPerBranch();
                        LoadSystemYear();
                        loadInvSummary();
                    }




                }
            }
        }
        public int theMonth;

        private void LoadSystemYear()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT MonthlyPostingYear,MonthlyPostingMonth FROM SystemMaster where BrCode=@BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        lblYear.Text = dR[0].ToString();
                        

                        if (dR[1].ToString() == "1")
                        {
                            lblMonth.Text = "January";
                        }
                        else if (dR[1].ToString() == "2")
                        {
                            lblMonth.Text = "February";
                        }
                        else if (dR[1].ToString() == "3")
                        {
                            lblMonth.Text = "March";
                        }
                        else if (dR[1].ToString() == "4")
                        {
                            lblMonth.Text = "April";
                        }
                        else if (dR[1].ToString() == "5")
                        {
                            lblMonth.Text = "May";
                        }
                        else if (dR[1].ToString() == "6")
                        {
                            lblMonth.Text = "June";
                        }
                        else if (dR[1].ToString() == "7")
                        {
                            lblMonth.Text = "July";
                        }
                        else if (dR[1].ToString() == "8")
                        {
                            lblMonth.Text = "August";
                        }
                        else if (dR[1].ToString() == "9")
                        {
                            lblMonth.Text = "September";
                        }
                        else if (dR[1].ToString() == "10")
                        {
                            lblMonth.Text = "October";
                        }
                        else if (dR[1].ToString() == "11")
                        {
                            lblMonth.Text = "November";
                        }
                        else if (dR[1].ToString() == "12")
                        {
                            lblMonth.Text = "December";
                        }

                       // theMonth=Convert.ToInt32(dR[1].ToString());
                    }

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
                    //ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    //ddBranch.SelectedIndex = 0;
                }
            }
        }

  

        private void loadInvSummary()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.rptInventorySummary";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvSummary.DataSource = dT;
                        gvSummary.DataBind();

                        //if (gvSummary.Rows.Count > 0)
                        //{

                        //    gvSummary.FooterRow.Cells[4].Text = "Total";
                        //    gvSummary.FooterRow.Cells[4].HorizontalAlign = HorizontalAlign.Right;


                        //    float total5 = dT.AsEnumerable().Sum(row => row.Field<float>("QtyReceived"));
                        //    gvSummary.FooterRow.Cells[5].HorizontalAlign = HorizontalAlign.Right;
                        //    gvSummary.FooterRow.Cells[5].Text = total5.ToString();
                        //}
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

        

        

        private void ProceeedWithMonthlyPosting()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (lblMonth.Text == "January")
                {
                    theMonth = 1;
                }
                else if (lblMonth.Text == "February")
                {
                    theMonth = 2;
                }
                else if (lblMonth.Text == "March")
                {
                    theMonth = 3;

                }
                else if (lblMonth.Text == "April")
                {
                    theMonth = 4;

                }
                else if (lblMonth.Text == "May")
                {
                    theMonth = 5;

                }
                else if (lblMonth.Text == "June")
                {
                    theMonth = 6;
                }
                else if (lblMonth.Text == "July")
                {
                    theMonth = 7;

                }
                else if (lblMonth.Text == "August")
                {
                    theMonth = 8;

                }
                else if (lblMonth.Text == "September")
                {
                    theMonth = 9;

                }
                else if (lblMonth.Text == "October")
                {
                    theMonth = 10;

                }
                else if (lblMonth.Text == "November")
                {
                    theMonth = 11;

                }
                else if (lblMonth.Text == "December")
                {
                    theMonth = 12;

                }



                using (SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.ProcessMonthlyPosting";

                    sqlConn.Open();

                    using (SqlCommand cmD = new SqlCommand(stR, sqlConn))
                    {
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@MonthlyPostingYear", lblYear.Text);
                        cmD.Parameters.AddWithValue("@MonthlyPostingMonth", theMonth);
                        cmD.Parameters.AddWithValue("@vUserID", Session["EmpNo"].ToString());
                        cmD.ExecuteNonQuery();

                    }
                }

                lblMsgSuccess.Text = "Monthly posting for the month of " + lblMonth.Text + " Year " + lblYear.Text + "\nSuccesfullly done!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
        }



        
        

        protected void gvSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //GridViewRow row = gvSummary.Rows[e.RowIndex];
                //Label lblNetAmount = (Label)row.FindControl("lblNetAmount");

                //Label lblSales = ((Label)e.Row.FindControl("lblUnpostedQtyS"));
                //if (Convert.ToDecimal(lblSales.Text) > 0)
                //{
                //    lblSales.ForeColor = System.Drawing.Color.Maroon;
                //    lblSales.Font.Bold = true;
                //}

                if (((Label)e.Row.FindControl("lblUnpostedQtyS")).Text != "0" | ((Label)e.Row.FindControl("lblUnpostedPRF")).Text != "0" | ((Label)e.Row.FindControl("lblUnpostedAdjustment")).Text != "0")
                {
                    e.Row.BackColor = System.Drawing.Color.PaleGoldenrod;
                    ((Label)e.Row.FindControl("lblUnpostedQtyS")).ForeColor = System.Drawing.Color.Blue;
                    ((Label)e.Row.FindControl("lblUnpostedQtyS")).Font.Bold = true;
                    ((Label)e.Row.FindControl("lblUnpostedQtyS")).BackColor = System.Drawing.Color.PaleGoldenrod;

                }




            }
        

        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                btnProcess.Enabled = false;

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR1 = @"SELECT BrCode FROM UnpostedSalesDetailed WHERE BrCode=@BrCode";
                    using (SqlCommand cmD = new SqlCommand(stR1, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        SqlDataReader dR = cmD.ExecuteReader();
                        if (dR.HasRows)
                        {

                            lblMsgWithPending.Text = "There are transaction on the system for processing end of day!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsgWithPending();", true);
                            return;
                        }
                        else
                        {
                            ProceeedWithMonthlyPosting();
                        }
                    }
                }
            }
        }

    }
}