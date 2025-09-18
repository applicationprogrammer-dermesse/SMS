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
    public partial class SpecialDiscount : System.Web.UI.Page
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
                    if (Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "CIT")
                    {
                        loadBranch();
                        loadSpecialDiscount();
                        loadActiveDiscount();
                    }
                    else
                    {
                        Response.Redirect("~/UnauthorizedPAge.aspx");
                    }


                }
            }
        }

        private void loadBranch()
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
                    ddBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
                    ddBranch.SelectedIndex = 0;
                }
            }
        }

        private void loadSpecialDiscount()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.LoadSpecialDiscounts";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();

                    
                    ddSpecialDiscount.Items.Clear();
                    ddSpecialDiscount.DataSource = dR;
                    ddSpecialDiscount.DataValueField = "sConstant";
                    ddSpecialDiscount.DataTextField = "sDescription";
                    ddSpecialDiscount.DataBind();
                    ddSpecialDiscount.Items.Insert(0, new ListItem("Select Special Discount", "0"));
                    ddSpecialDiscount.SelectedIndex = 0;
                }
            }
        }


        private void loadActiveDiscount()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"loadActiveDiscountList";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dR = cmD.ExecuteReader();
                    gvDiscount.DataSource = dR;
                    gvDiscount.DataBind();

                }
            }
        }

        protected void gvDiscount_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string RecNo = gvDiscount.DataKeys[e.RowIndex].Value.ToString();
                string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                string sql = @"UPDATE tblTypeDiscount SET iValidFrom_dt=convert(char(10),getdate() - 1,112), iValidUntil_dt =convert(char(10),getdate() - 1,112), dtLastUpdate_dt=GETDATE(),updateBy='" + Session["FullName"].ToString() + "'  where DiscID='" + RecNo + "'";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.ExecuteNonQuery();
                    }
                }

                loadActiveDiscount();
            }
            catch (Exception y)
            {
                 lblMsgWarning.Text = y.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

        protected void btnActibate_Click(object sender, EventArgs e)
        {
            try
            {
              string connString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                string sql = @"UPDATE tblTypeDiscount SET iValidFrom_dt=@iValidFrom_dt, iValidUntil_dt = @iValidUntil_dt, dtLastUpdate_dt= @dtLastUpdate_dt,updateBy=@updateBy
                                where BrCode=@BrCode and sConstant=@sConstant";
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@iValidFrom_dt",Convert.ToDateTime(txtDateFrom.Text).ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@iValidUntil_dt", Convert.ToDateTime(txtDateTo.Text).ToString("yyyyMMdd"));
                        cmd.Parameters.AddWithValue("@dtLastUpdate_dt",DateTime.Now);
                        cmd.Parameters.AddWithValue("@updateBy",Session["FullName"].ToString());
                        cmd.Parameters.AddWithValue("@sConstant",ddSpecialDiscount.SelectedValue);
                        cmd.Parameters.AddWithValue("@BrCode",ddBranch.SelectedValue);
                        cmd.ExecuteNonQuery();
                    }
                }

                txtDateFrom.Text = string.Empty;
                txtDateTo.Text = string.Empty;
                loadBranch();
                loadSpecialDiscount();
                loadActiveDiscount();
            }
            catch (Exception y)
            {
                lblMsgWarning.Text = y.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
        }

    }
}