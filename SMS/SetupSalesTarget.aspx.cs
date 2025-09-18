using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace SMS
{
    public partial class SetupSalesTarget : System.Web.UI.Page
    {
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
                        loadYear();
                        loadMonth();
                        LoadTargetPerMonthList();
                    }
                    else
                    {
                        Response.Redirect("~/UnauthorizedPAge.aspx");
                    }


                }
            }
        }


        private void loadYear()
        {
            int CurrYear = System.DateTime.Now.Year;
            string theYear = Convert.ToString(CurrYear);
            ddYear.Items.Insert(0, theYear);
            string thePYear = Convert.ToString(CurrYear - 1);

            ddYear.Items.Insert(1, thePYear);
            for (int x = CurrYear + 1; x <= CurrYear + 1; x++)
            {
                string theRangeYear = Convert.ToString(x);

                ddYear.Items.Add(theRangeYear);

            }
        }
        
        private void loadMonth()
        {
            int month = DateTime.Now.Month;


            CultureInfo usEnglish = new CultureInfo("en-US");
            DateTimeFormatInfo englishInfo = usEnglish.DateTimeFormat;
            string monthName = englishInfo.MonthNames[month - 1];

            ddMonth.Items.Insert(0, monthName);
            ddMonth.Items.Add("January");
            ddMonth.Items.Add("February");
            ddMonth.Items.Add("March");
            ddMonth.Items.Add("April");
            ddMonth.Items.Add("May");
            ddMonth.Items.Add("June");
            ddMonth.Items.Add("July");
            ddMonth.Items.Add("August");
            ddMonth.Items.Add("September");
            ddMonth.Items.Add("October");
            ddMonth.Items.Add("November");
            ddMonth.Items.Add("December");

            
        }

        public int theMonth;
        private void LoadTargetPerMonthList()
        {
            if (ddMonth.SelectedItem.Text == "January") {theMonth = 1;}
            else if (ddMonth.SelectedItem.Text == "February") {theMonth = 2;}
            else if (ddMonth.SelectedItem.Text == "March") {theMonth = 3;}
            else if (ddMonth.SelectedItem.Text == "April") {theMonth = 4;}
            else if (ddMonth.SelectedItem.Text == "May") {theMonth = 5;}
            else if (ddMonth.SelectedItem.Text == "June") {theMonth = 6;}
            else if (ddMonth.SelectedItem.Text == "July") {theMonth = 7;}
            else if (ddMonth.SelectedItem.Text == "August") {theMonth = 8;}
            else if (ddMonth.SelectedItem.Text == "September") {theMonth = 9;}
            else if (ddMonth.SelectedItem.Text == "October") {theMonth = 10;}
            else if (ddMonth.SelectedItem.Text == "November") {theMonth = 11;}
            else if (ddMonth.SelectedItem.Text == "December") { theMonth = 12; }


            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadTargetPerMonth";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Year", ddYear.SelectedItem.Text);
                    cmD.Parameters.AddWithValue("@Month", theMonth);
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvTarget.DataSource = dT;
                    gvTarget.DataBind();

                    if (gvTarget.Rows.Count==0)
                    {
                        lblMsgWarning.Text = "No Record Found. Please Contact system Administrator";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }

                }
            }
        }

        protected void gvTarget_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTarget.EditIndex = e.NewEditIndex;
            LoadTargetPerMonthList();
        }

        protected void gvTarget_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTarget.EditIndex = -1;
            LoadTargetPerMonthList();

            
        }

        protected void gvTarget_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {

                string dKey = gvTarget.DataKeys[e.RowIndex].Value.ToString();
                TextBox txtTarget = gvTarget.Rows[e.RowIndex].FindControl("txtiTarget") as TextBox;
                TextBox txtDay = gvTarget.Rows[e.RowIndex].FindControl("txtiDay") as TextBox;


                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"UPDATE MonthlyTarget SET iTarget=@iTarget,iDay=@iDay,EditedBy=@EditedBy,EditedDate=@EditedDate where ID=@ID";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@ID", dKey);
                        cmD.Parameters.AddWithValue("@iTarget", txtTarget.Text);
                        cmD.Parameters.AddWithValue("@iDay", txtDay.Text);
                        cmD.Parameters.AddWithValue("@EditedBy", Session["FullName"].ToString());
                        cmD.Parameters.AddWithValue("@EditedDate", DateTime.Now);
                        cmD.ExecuteNonQuery();
                    }
                }

                gvTarget.EditIndex = -1;
                LoadTargetPerMonthList();
            }
            catch (Exception x)
            {
                lblMsgWarning.Text = x.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        protected void ddYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTargetPerMonthList();
        }

        protected void ddMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTargetPerMonthList();
        }


        decimal RunningTargetSales = 0;
        protected void gvTarget_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RunningTargetSales += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "iTarget"));
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                    //decimal PctToDate = ((RunningTotalTotalNetSales / RunningTargetSalestoDate) * 100);
                ((Label)e.Row.FindControl("lbltotalTarget")).Text = RunningTargetSales.ToString("N");
             

             
            }
        }

    }
}