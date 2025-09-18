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
    public partial class setupEmployee : System.Web.UI.Page
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

                    loadEmployee();


                }
            }
        }


        private void loadEmployee()
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [ID]
                              ,[EmpNo]
                              ,[EmployeeName]
                              ,[Position]
                          FROM [DoctosList] order by EmployeeName";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);
                    gvEmployee.DataSource = dT;
                    gvEmployee.DataBind();

                    

                }
            }
        }

        protected void gvEmployee_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvEmployee.EditIndex = e.NewEditIndex;
            loadEmployee();
        }

        protected void gvEmployee_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvEmployee.EditIndex = -1;
            loadEmployee();
        }

        protected void gvEmployee_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string dKey = gvEmployee.DataKeys[e.RowIndex].Value.ToString();

            
            Label lblPerformedBy = gvEmployee.Rows[e.RowIndex].FindControl("lblPerformedBy") as Label;
            TextBox txtEmpNo = gvEmployee.Rows[e.RowIndex].FindControl("txtEmpNo") as TextBox;
            TextBox txtEmployeeName = gvEmployee.Rows[e.RowIndex].FindControl("txtEmployeeName") as TextBox;
            DropDownList ddPosition = gvEmployee.Rows[e.RowIndex].FindControl("ddPosition") as DropDownList;
           // loadPosition(ddPosition);

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"dbo.UpdateEmployeeInfo";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.CommandType = CommandType.StoredProcedure;
                    cmD.Parameters.AddWithValue("@ID", dKey);
                    if (lblPerformedBy.Text == txtEmployeeName.Text)
                    {
                        cmD.Parameters.AddWithValue("@Opt", 0);
                    }
                    else
                    {
                        cmD.Parameters.AddWithValue("@Opt", 1);
                    }
                    
                    cmD.Parameters.AddWithValue("@EmpNo",txtEmpNo.Text.Trim());
                    cmD.Parameters.AddWithValue("@EmployeeName", txtEmployeeName.Text.ToUpper());
                    cmD.Parameters.AddWithValue("@PerformedBy", lblPerformedBy.Text);
                    cmD.Parameters.AddWithValue("@Position", ddPosition.SelectedItem.Text.ToUpper());
                    cmD.Parameters.AddWithValue("@UpdateBy", Session["FullName"].ToString());
                    cmD.ExecuteNonQuery();
                }
            }

            gvEmployee.EditIndex = -1;
            loadEmployee();
         
        }

        //private void loadPosition(DropDownList ddPosition)
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"SELECT distinct Position FROM DoctosList ORDER BY Position";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            SqlDataReader dR = cmD.ExecuteReader();

        //            ddPosition.Items.Clear();
        //            ddPosition.DataSource = dR;
        //            ddPosition.DataValueField = "Position";
        //            ddPosition.DataTextField = "Position";
        //            ddPosition.DataBind();
        //            ddPosition.Items.Insert(0, new ListItem("Please select", "0"));
        //            //ddPayment.SelectedIndex = 1;
        //        }
        //    }

        //}

        protected void gvEmployee_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvEmployee.EditIndex == e.Row.RowIndex)
            {
                DropDownList ddPosition = (DropDownList)e.Row.FindControl("ddPosition");
                string sql = "SELECT distinct Position FROM DoctosList ORDER BY Position";
                string conString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString;
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(sql, con))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ddPosition.DataSource = dt;
                            ddPosition.DataTextField = "Position";
                            ddPosition.DataValueField = "Position";
                            ddPosition.DataBind();
                            string selectedPosition = DataBinder.Eval(e.Row.DataItem, "Position").ToString();
                            ddPosition.Items.FindByValue(selectedPosition).Selected = true;
                        }
                    }
                }
            }

        }

    }
}