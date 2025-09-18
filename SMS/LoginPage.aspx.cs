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
    public partial class LoginPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

            }



        }



        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (login_username.Value == string.Empty | login_password.Value==string.Empty)
            {
                 lblMsgWarning.Text = "Please supply value!";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                try
                {
                    int num = Convert.ToInt32(login_username.Value);
                    string sNum = num.ToString("00000");
                    login_username.Value = sNum.ToString();

                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
//                        string stR1 = @"SELECT vUser_EmpNo
//                                      ,vUser_Level
//                                      ,vUser_FullName
//                                      ,vUser_Stat
//                                      ,vUser_Type
//                                      ,vUser_EmailAdd
//                                        ,vUser_Branch
//                                      ,vUser_Dept
//                                      ,vUser_Comp
//                                       ,vIsAllowedToVoid
//                                    FROM MyUserLogin WHERE vUser_EmpNo=@vUser_EmpNo AND vUser_Pass=@vUser_Pass";
                        string stR1 = @"SELECT vUser_EmpNo
                                      ,vUser_Level
                                      ,vUser_FullName
                                      ,vUser_Type
                                      ,vUser_EmailAdd
                                        ,vUser_Branch
                                      ,vUser_Dept
                                      ,vUser_Comp
                                       ,vIsAllowedToVoid
                                    FROM MyUserLogin WHERE vUser_EmpNo=@vUser_EmpNo AND vUser_Pass=@vUser_Pass";
                        using (SqlCommand cmD = new SqlCommand(stR1, conN))
                        {
                            conN.Open();
                            cmD.Parameters.AddWithValue("@vUser_EmpNo", login_username.Value);
                            cmD.Parameters.AddWithValue("@vUser_Pass", MyClass.Encrypt(login_password.Value, true));

                            SqlDataAdapter loginDA = new SqlDataAdapter(cmD);
                            DataTable loginDT = new DataTable();
                            loginDA.Fill(loginDT);
                            if (loginDT.Rows.Count > 0)
                            {
                                // FOR FUTURE USE!!!!! (RESIGNED EMPLOYEE)
                                // Check if user status is 0 (inactive/disabled)
                                //int userStat = Convert.ToInt32(loginDT.Rows[0]["vUser_Stat"]);
                                //if (userStat == 0)
                                //{
                                //    lblMsgWarning.Text = "User account is inactive or disabled. Please contact administrator.";
                                //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                //    return;
                                //}

                                Session["EmpNo"] = loginDT.Rows[0]["vUser_EmpNo"].ToString();
                                Session["Level"] = loginDT.Rows[0]["vUser_Level"].ToString();
                                Session["FullName"] = loginDT.Rows[0]["vUser_FullName"].ToString();
                                Session["EmailAdd"] = loginDT.Rows[0]["vUser_EmailAdd"].ToString();
                                Session["Type"] = loginDT.Rows[0]["vUser_Type"].ToString();
                                Session["Comp"] = loginDT.Rows[0]["vUser_Comp"].ToString();
                                Session["vUser_Branch"] = loginDT.Rows[0]["vUser_Branch"].ToString();
                                Session["Dept"] = loginDT.Rows[0]["vUser_Dept"].ToString();
                                Session["IsAllowed"] = loginDT.Rows[0]["vIsAllowedToVoid"].ToString();
                                Session["theBRnm"] = "Select Branch";
                                Session["theBRcd"] = "0";
                                Session["sDate"] = "01/01/1900";
                                Response.Redirect("~/HomePage.aspx");
                            }

                            else
                            {
                                //login_username.Focus();
                                lblMsgWarning.Text = "User info does not exists!,  Please re encode employee number and password.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            }

                        }
                    }
                }
                catch (FormatException x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                }
                catch (SqlException s)
                {
                    lblMsgWarning.Text = s.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                }
            }
        }






    }
}