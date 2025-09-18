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
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtEmployeeNo.Attributes.Add("onfocus", "this.select()");

                txtEmployeeNo.Text = string.Empty;
                txtNewPassword.Text = string.Empty;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtEmployeeNo.Text == "Emp. No.")
            {
                txtEmployeeNo.Focus();
                lblMsg.Text = "Please supply employee number!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {

                int num = Convert.ToInt32(txtEmployeeNo.Text);
                string sNum = num.ToString("00000");
                txtEmployeeNo.Text = sNum.ToString();


                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.ForgotPassword";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@vUser_EmpNo", txtEmployeeNo.Text);
                        cmD.Parameters.AddWithValue("@vUser_Pass", MyClass.Encrypt(txtNewPassword.Text, true));
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsg.Text = "Employee number does not exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                        else
                        {
                            lblMsgWarning.Text = "Password succesfully updated! Close this dialog to redirect you yo Login Page.";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                    }
                }
            }
        }



    }
}