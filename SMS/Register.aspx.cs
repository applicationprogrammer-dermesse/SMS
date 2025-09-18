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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //LoadDepartment();
                loadBranch();
            }

        }

        //private void LoadDepartment()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"SELECT DEP_Department FROM VBP_Department ORDER BY DEP_Department";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            SqlDataReader dR = cmD.ExecuteReader();

        //            selDepartment.Items.Clear();
        //            selDepartment.DataSource = dR;
        //            selDepartment.DataValueField = "DEP_Department";
        //            selDepartment.DataTextField = "DEP_Department";
        //            selDepartment.DataBind();
        //            selDepartment.Items.Insert(0, new ListItem("Please select department", String.Empty));
        //            selDepartment.SelectedIndex = 0;
        //        }
        //    }
        //}

        private void loadBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT BrCode,BrName FROM MyBranchList where BrCode <= 30 ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    selBranch.Items.Clear();
                    selBranch.DataSource = dR;
                    selBranch.DataValueField = "BrCode";
                    selBranch.DataTextField = "BrName";
                    selBranch.DataBind();
                    selBranch.Items.Insert(0, new ListItem("Select Branch", "0"));
                    selBranch.SelectedIndex = 0;
                }
            }
        }

        protected void btnReg_Click(object sender, EventArgs e)
        {
            if (selBranch.SelectedIndex == 0)
            {
                lblMsg.Text = "Please select branch.";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }
            else
            {
                ifInfoIsExists_EmpNo();
            }
        }

        private void ifInfoIsExists_EmpNo()
        {
            int num = Convert.ToInt32(inpEmpNo.Value);
            string sNum = num.ToString("00000");
            inpEmpNo.Value = sNum.ToString();

            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"SELECT vUser_EmpNo FROM MyUserLogin WHERE vUser_EmpNo=@vUser_EmpNo";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@vUser_EmpNo", inpEmpNo.Value);
                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {
                        inpEmpNo.Focus();
                        lblMsg.Text = "Employee number already exists!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        ifInfoIsExists_EmailAdd();
                    }
                }
            }
        }

        private void ifInfoIsExists_EmailAdd()
        {
            string theEmailAddress = inpEmail.Value + selemail.Value;
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"SELECT vUser_EmailAdd FROM MyUserLogin WHERE vUser_EmailAdd=@vUser_EmailAdd";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@vUser_EmailAdd", theEmailAddress);
                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {
                        inpEmail.Focus();
                        lblMsg.Text = "Email address already exists!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        //ifInfoIsExists_EmpID();
                        RegisterUser();
                    }
                }
            }
        }

        private void ifInfoIsExists_EmpID()
        {
            string theEmailAddress = inpEmail.Value + selemail.Value;
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR1 = @"SELECT vUser_EmpNo FROM MyUserLogin WHERE vUser_ID=@inpEmpNo";
                using (SqlCommand cmD = new SqlCommand(stR1, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@inpEmpNo", inpEmpNo.Value);
                    SqlDataReader dR = cmD.ExecuteReader();
                    if (dR.HasRows)
                    {
                        inpEmpNo.Focus();
                        lblMsg.Text = "Employee number already exists!";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                        return;
                    }
                    else
                    {
                        RegisterUser();
                    }
                }
            }
        }

        private void RegisterUser()
        {
            try
            {

                int num = Convert.ToInt32(inpEmpNo.Value);
                string sNum = num.ToString("00000");
                inpEmpNo.Value = sNum.ToString();

                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"INSERT INTO MyUserLogin
                                        (vUser_Pass,vUser_EmpNo,vUser_Level,vUser_FullName,vUser_Stat,vUser_Type,vUser_DateCreated,vUser_EmailAdd,vUser_Comp,vUser_Branch,vUser_Dept)
                                        VALUES
                                        (@User_Pass,@User_EmpNo,@User_Level,@User_FullName,@User_Stat,@User_Type,@User_DateCreated,@User_EmailAdd,@User_Comp,@vUser_Branch,@vUser_Dept)";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.Parameters.AddWithValue("@User_Pass",MyClass.Encrypt(inpRegPass.Value, true));
                        cmD.Parameters.AddWithValue("@User_EmpNo", inpEmpNo.Value);
                        cmD.Parameters.AddWithValue("@User_Level", selLevel.Value);
                        cmD.Parameters.AddWithValue("@User_FullName", inpfullname.Value);
                        cmD.Parameters.AddWithValue("@User_Stat", 1);
                        cmD.Parameters.AddWithValue("@User_Type", 2);
                        cmD.Parameters.AddWithValue("@User_DateCreated", DateTime.Now);
                        cmD.Parameters.AddWithValue("@User_EmailAdd", inpEmail.Value + selemail.Value);
                        cmD.Parameters.AddWithValue("@User_Comp", selCompany.Value);
                        cmD.Parameters.AddWithValue("@vUser_Branch", selBranch.Value);
                        cmD.Parameters.AddWithValue("@vUser_Dept", selBranch.Items[selBranch.SelectedIndex].Text);
                        // cmD.Parameters.AddWithValue("@User_Dept", selDepartment.Value);

                        cmD.ExecuteNonQuery();
                    }
                }


                //selDepartment.SelectedIndex = 0;
                selCompany.SelectedIndex = 0;
                selLevel.SelectedIndex = 0;
                inpEmail.Value = "";
                inpEmpNo.Value = "";
                inpfullname.Value = "";
                inpRegPass.Value = "";
                selBranch.SelectedIndex = 0;





            }
            catch (SqlException SQLx)
            {
                lblMsg.Text = SQLx.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;

            }
            catch (Exception X)
            {
                lblMsg.Text = X.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                return;
            }


            lblMsg.Text = "New User Succesfully Created!";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
            return;
        }
    }
}