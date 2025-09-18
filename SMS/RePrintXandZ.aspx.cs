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
    public partial class RePrintXandZ : System.Web.UI.Page
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

                    //if (Session["vUser_Branch"].ToString() == "1")
                    //{
                    //    Response.Redirect("~/UnauthorizedPage.aspx");
                    //}
                    //else
                    //{
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
                        lblBranch.Text = Session["cellBranchName"].ToString();
                        lblDate.Text = Session["cellDate"].ToString();
                        loadSalesForTheDay();
                    //}

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



        


        private void loadSalesForTheDay()
        {

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.ViewSalesDetailedForRePrint";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@Date", lblDate.Text);
                    //cmD.Parameters.AddWithValue("@EDate", txtDateTo.Text);
                    cmD.Parameters.AddWithValue("@BrCode", Session["cellBranch"].ToString());
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    gvSalesDetailed.DataSource = dT;
                    gvSalesDetailed.DataBind();

                    if (gvSalesDetailed.Rows.Count == 0)
                    {
                        lblMsgWarning.Text = "No record found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;

                    }

                }
            }
        }











        protected void btnPreviewX_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>window.open ('XReading.aspx?Date=" + lblDate.Text + "','_blank');</script>");
            //Session["TheXoption"] = "1";
            string url = "ReprintXReading.aspx?Date=" + lblDate.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }

        protected void btnPrintX_Click(object sender, EventArgs e)
        {
            //Response.Write("<script>window.open ('XReading.aspx?Date=" + lblDate.Text + "','_blank');</script>");
            //Session["TheXoption"] = "2";
            string url = "ReprintXReading.aspx?Date=" + lblDate.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
        }


        protected void btnPreviewZ_Click(object sender, EventArgs e)
        {

            // updatePrevGT();
            //  ProceedWithEndOfDay();
            //Session["TheZoption"] = "1";
            //Response.Write("<script>window.open ('ZReading.aspx?Date=" + lblDate.Text + "','_blank');</script>");
            string url = "ReprintZReading.aspx?Date=" + lblDate.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);



        }


        protected void btnPrintZ_Click(object sender, EventArgs e)
        {

            // updatePrevGT();
            //  ProceedWithEndOfDay();
            //Session["TheZoption"] = "2";
            //Response.Write("<script>window.open ('ZReading.aspx?Date=" + lblDate.Text + "','_blank');</script>");
            string url = "ReprintZReading.aspx?Date=" + lblDate.Text;
            string s = "window.open('" + url + "', 'popup_window', 'width=700,height=520,left=300,top=100,resizable=no,copyhistory=no');";
            ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);



        }

    }
}