using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Reflection;


namespace SMS
{
    public partial class AddNewCustomer : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["vUser_Branch"] == null)
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
                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
                        btnUpdate.Enabled = false;
                        btnSave.Enabled = false;
                        btnBuyungOnly.Enabled = false;
                        loadProvince();
                        ddCity.Items.Insert(0, new ListItem(string.Empty, "0"));

                    }
                    else
                    {
                        //loadCustomerNo();
                        btnUpdate.Enabled = true;
                        btnSave.Enabled = true;
                        btnBuyungOnly.Enabled = true;
                        loadProvince();
                        ddCity.Items.Insert(0, new ListItem(string.Empty, "0"));

                        ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                        Session["SessionId"] = ViewState["ViewStateId"].ToString();
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

        private void loadProvince()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [RegionID]
                                      ,[RegionName]
                                  FROM [tblRegion]
                                  ORDER BY
                                  (CASE WHEN [RegionName] = 'METRO MANILA' THEN  1 ELSE 2 END), [RegionName]";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddRegion.Items.Clear();
                    ddRegion.DataSource = dR;
                    ddRegion.DataValueField = "RegionID";
                    ddRegion.DataTextField = "RegionName";
                    ddRegion.DataBind();
                    ddRegion.Items.Insert(0, new ListItem("Please select region/province", "0"));
                    ddRegion.SelectedIndex = 0;
                }
            }
        }


        //private void loadCustomerNo()
        //{
        //    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
        //    {
        //        string stR = @"SELECT CONCAT('CUST-',[BrCode],'-',YEAR([CurrentDate]),'-',RIGHT('00000'+CAST(CustomerNo AS VARCHAR(8)),8)) FROM SystemMaster where [BrCode]='" + Session["vUser_Branch"].ToString() + "'";
        //        using (SqlCommand cmD = new SqlCommand(stR, conN))
        //        {
        //            conN.Open();
        //            SqlDataReader dR = cmD.ExecuteReader();
        //            while (dR.Read())
        //            {
        //                txtCustomerNo.Text = dR[0].ToString();
        //            }

        //        }
        //    }

        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.AddNewCustomer";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                        cmD.Parameters.AddWithValue("@CustomerName", txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@FirstName ", txtFirstName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@MI", txtMiddleName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@HouseAddress", txtHouseAddress.Text.Trim().ToUpper());
                        if (ddRegion.SelectedValue != "0")
                        {
                            cmD.Parameters.AddWithValue("@RegionName", ddRegion.SelectedItem.Text);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@RegionName", string.Empty);
                        }

                        if (ddCity.SelectedValue != "0")
                        {
                            cmD.Parameters.AddWithValue("@CityName", ddCity.SelectedItem.Text);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@CityName", string.Empty);
                        }

                        cmD.Parameters.AddWithValue("@Gender", ddGender.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@CivilSatus", ddCivilSatus.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                        cmD.Parameters.AddWithValue("@Contact1", txtContatNo1.Text);
                        cmD.Parameters.AddWithValue("@Contact2", txtContatNo2.Text);
                        cmD.Parameters.AddWithValue("@EmailAddress", txtEmailAddress.Text);
                        cmD.Parameters.AddWithValue("@RegDate", DateTime.Now);
                        cmD.Parameters.AddWithValue("@CreatedBy", Session["EmpNo"].ToString());
                        //cmD.Parameters["@ResultValue"].Direction = ParameterDirection.Output;
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsgWarning.Text = "Customer =  <b>" + txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper() + "</b>\n already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            lblMsgSuccess.Text = "Customer =  <b>" + txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper() + "</b>\n Successfully added!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                            return;
                        }
                    }





                }




            }

        }



        protected void ddRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddRegion.SelectedValue != "0")
            {
                loadCity();
                btnAddCity.Enabled = true;
            }
            else
            {
                btnAddCity.Enabled = false;
            }
        }

        private void loadCity()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT CityName FROM tblCity where RegionID=@RegionID order by CityName";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.Parameters.AddWithValue("@RegionID", ddRegion.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCity.Items.Clear();
                    ddCity.DataSource = dR;
                    ddCity.DataValueField = "CityName";
                    ddCity.DataTextField = "CityName";
                    ddCity.DataBind();
                    ddCity.Items.Insert(0, new ListItem("Please select city", "0"));
                    ddCity.SelectedIndex = 0;
                }
            }
        }

        protected void btnBuyungOnly_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                if (txtLastName.Text == string.Empty)
                {
                    txtLastName.Focus();
                    lblMsgWarning.Text = "Please supply customer last name.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;

                }
                else if (txtFirstName.Text == string.Empty)
                {
                    txtFirstName.Focus();
                    lblMsgWarning.Text = "Please supply customer first name.";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
                else
                {
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"dbo.AddNewCustomerBuyingOnly";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.CommandTimeout = 0;
                            cmD.Parameters.AddWithValue("@BrCode", Session["vUser_Branch"].ToString());
                            cmD.Parameters.AddWithValue("@CustomerName", txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper());
                            cmD.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim().ToUpper());
                            cmD.Parameters.AddWithValue("@FirstName ", txtFirstName.Text.Trim().ToUpper());
                            cmD.Parameters.AddWithValue("@MI", txtMiddleName.Text.Trim().ToUpper());
                            cmD.Parameters.AddWithValue("@Gender", ddGender.SelectedItem.Text);
                            cmD.Parameters.AddWithValue("@RegDate", DateTime.Now);
                            cmD.Parameters.AddWithValue("@CreatedBy", Session["EmpNo"].ToString());
                            cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                            cmD.ExecuteNonQuery();
                            int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                            if (Result == 99)
                            {
                                lblMsgWarning.Text = "Customer =  <b>" + txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper() + "</b>\n already exists!";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            }
                            else
                            {
                                lblMsgSuccess.Text = "Customer =  <b>" + txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper() + "</b>\n Successfully added!";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowSuccessMsg();", true);
                                return;
                            }
                        }
                    }
                }


            }
        }







        protected void btnSearchBranch_Click(object sender, EventArgs e)
        {
            using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT [BrCode]
	                          ,[CustID]
                               ,[CustomerName]
                          FROM [CustomerTable]
                          where CustomerName like '" + txtCustomer.Text + "%' ORDER BY [CustomerName]";
                using (SqlCommand cmD = new SqlCommand(stR, Conn))
                {
                    Conn.Open();
                    cmD.CommandTimeout = 0;
                    DataTable dT = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(cmD);
                    dA.Fill(dT);

                    if (dT.Rows.Count > 0)
                    {

                        gvCustomerList.DataSource = dT;
                        gvCustomerList.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridCustomer();", true);
                        return;
                    }
                    else
                    {
                        lblMsgWarning.Text = "No Record Found";
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                        return;
                    }
                }
            }

        }


        protected void gvCustomerList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gvR = gvCustomerList.Rows[e.RowIndex];
            txtCustomer.Text = gvR.Cells[2].Text;
            txtCustID.Text = gvR.Cells[1].Text;

            getCustomerInfo(gvR.Cells[1].Text);

            btnBuyungOnly.Enabled = false;
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;

            lblNote.Visible=true;
        }

        private void getCustomerInfo(string theID)
        {
            //try
            //{
                using (SqlConnection Conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT [LastName]
                                      ,[FirstName]
                                      ,[MI]
                                      ,[HouseAddress]
                                      ,[CityName]
                                      ,[RegionName]
                                      ,[Gender]
                                      ,[CivilSatus]
                                      ,ISNULL([DateOfBirth],'01/01/1900') AS [DateOfBirth]
                                      ,[Contact1]
                                      ,[Contact2]
                                      ,[EmailAddress]
                                  FROM [CustomerTable]
                             where CustID=@CustID";
                    using (SqlCommand cmD = new SqlCommand(stR, Conn))
                    {
                        Conn.Open();
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@CustID", theID);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);


                        txtLastName.Text = dT.Rows[0]["LastName"].ToString();
                        txtFirstName.Text = dT.Rows[0]["FirstName"].ToString();
                        txtMiddleName.Text = dT.Rows[0]["MI"].ToString();
                        txtHouseAddress.Text = dT.Rows[0]["HouseAddress"].ToString();
                        ddCity.SelectedItem.Text = dT.Rows[0]["CityName"].ToString();
                        ddRegion.SelectedItem.Text = dT.Rows[0]["RegionName"].ToString();

                        if (dT.Rows[0]["Gender"].ToString() == "Male")
                        {
                            ddGender.SelectedValue = "1";
                        }
                        else if (dT.Rows[0]["Gender"].ToString() == "Female")
                        {
                            ddGender.SelectedValue = "2";
                        }
                        else if (dT.Rows[0]["Gender"].ToString() == "Gay")
                        {
                            ddGender.SelectedValue = "3";
                        }
                        else if (dT.Rows[0]["Gender"].ToString() == "Lesbian")
                        {
                            ddGender.SelectedValue = "4";
                        }
                        else if (dT.Rows[0]["Gender"].ToString() == "Not Sure")
                        {
                            ddGender.SelectedValue = "5";
                        }

                        ddGender.SelectedItem.Text = dT.Rows[0]["Gender"].ToString();

                        if (dT.Rows[0]["CivilSatus"].ToString() == "Single")
                        {
                            ddCivilSatus.SelectedValue = "1";
                        }
                        else if (dT.Rows[0]["CivilSatus"].ToString() == "Married")
                        {
                            ddCivilSatus.SelectedValue = "2";
                        }
                        else if (dT.Rows[0]["CivilSatus"].ToString() == "Separated")
                        {
                            ddCivilSatus.SelectedValue = "3";
                        }

                        else if (dT.Rows[0]["CivilSatus"].ToString() == "Widow")
                        {
                            ddCivilSatus.SelectedValue = "4";
                        }

                        else if (dT.Rows[0]["CivilSatus"].ToString() == "Other")
                        {
                            ddCivilSatus.SelectedValue = "5";
                        }


                        ddCivilSatus.SelectedItem.Text = dT.Rows[0]["CivilSatus"].ToString();


                        txtDateOfBirth.Text = Convert.ToDateTime(dT.Rows[0]["DateOfBirth"]).ToShortDateString();
                        txtContatNo1.Text = dT.Rows[0]["Contact1"].ToString();
                        txtContatNo2.Text = dT.Rows[0]["Contact2"].ToString();
                        txtEmailAddress.Text = dT.Rows[0]["EmailAddress"].ToString();

                    }
                }
            //}
            //catch(Exception x)
            //{
            //                lblMsgWarning.Text = x.Message;
            //                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
            //                return;
            //}
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ddRegion.SelectedItem.Text == string.Empty)
            {

                lblMsgWarning.Text = "Please select region/province";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else if (ddCity.SelectedItem.Text == "Please select city")
            {

                lblMsgWarning.Text = "Please select city";
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                return;
            }
            else
            {
                upDateCustomerRecord();
            }
        }



        private void upDateCustomerRecord()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.UpdateCustomer";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;
                        cmD.Parameters.AddWithValue("@CustomerName", txtLastName.Text.Trim().ToUpper() + ", " + txtFirstName.Text.Trim().ToUpper() + " " + txtMiddleName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@FirstName ", txtFirstName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@MI", txtMiddleName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@HouseAddress", txtHouseAddress.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@RegionName", ddRegion.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@CityName", ddCity.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@Gender", ddGender.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@CivilSatus", ddCivilSatus.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@DateOfBirth", txtDateOfBirth.Text);
                        cmD.Parameters.AddWithValue("@Contact1", txtContatNo1.Text);
                        cmD.Parameters.AddWithValue("@Contact2", txtContatNo2.Text);
                        cmD.Parameters.AddWithValue("@EmailAddress", txtEmailAddress.Text);
                        cmD.Parameters.AddWithValue("@CustID", txtCustID.Text);
                        cmD.ExecuteNonQuery();

                        
                        }
                    }

                    btnBuyungOnly.Enabled = true;
                    btnSave.Enabled = true;
                    btnUpdate.Enabled = false;
                    lblNote.Visible=false;
                    txtContatNo1.Text=string.Empty;
                    txtContatNo2.Text = string.Empty;
                    txtCustID.Text = string.Empty;
                    txtCustomer.Text = string.Empty;
                    txtDateOfBirth.Text = string.Empty;
                    txtEmailAddress.Text = string.Empty;
                    txtFirstName.Text = string.Empty;
                    txtHouseAddress.Text = string.Empty;
                    txtLastName.Text = string.Empty;
                    txtMiddleName.Text = string.Empty;
                    loadCity();
                    loadProvince();
                    


                            lblMsgWarning.Text = "Customer information succesfully updated!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;

                    

                }


        }

        protected void btnAddRegion_Click(object sender, EventArgs e)
        {
            txtERegionName.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridAddRegion();", true);
            return;
        }


        protected void btnEaddRegion_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.AddNewRegion";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;

                        cmD.Parameters.AddWithValue("@RegionName", txtERegionName.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsgWarning.Text = "REGION =  <b>" + txtERegionName.Text + "</b>\n already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            loadProvince();
                            lblMsgWarning.Text = "Successfully added";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                    }
                }
            }
        }




        //start city
        protected void btnAddCity_Click(object sender, EventArgs e)
        {
            txtECityName.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowGridAddCity();", true);
            return;
        }


        protected void btnEaddCity_Click(object sender, EventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.AddNewCity";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.CommandTimeout = 0;

                        cmD.Parameters.AddWithValue("@CityName", txtECityName.Text.Trim().ToUpper());

                        cmD.Parameters.AddWithValue("@RegionID", ddRegion.SelectedValue);
                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();
                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsgWarning.Text = "City =  <b>" + txtECityName.Text + "</b>\n already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            loadCity();
                            lblMsgWarning.Text = "Successfully added";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                    }
                }
            }
        }

        //end city







    }
}