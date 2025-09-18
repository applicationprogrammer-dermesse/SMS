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
    public partial class AddNewItem : System.Web.UI.Page
    {
        public bool IsPageRefresh = false;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));

                if (Session["vUser_Branch"].ToString() == "1" & Session["Dept"].ToString() == "CIT")
                {
                    
                    ViewState["ViewStateId"] = System.Guid.NewGuid().ToString();
                    Session["SessionId"] = ViewState["ViewStateId"].ToString();

                    getLastPlucode();
                }
                else
                {
                    Response.Redirect("~/UnauthorizedPAge.aspx");

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


        private void getLastPlucode()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT MAX(vPluCode) FROM [vItemMaster] where vPluCode like '010%'";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    SqlDataReader dR = cmD.ExecuteReader();
                    while (dR.Read())
                    {
                        
                        int theNext = Convert.ToInt32(dR[0].ToString()) + 1;

                        lblLastPlucode.Text = dR[0].ToString();
                        txtPluCode.Text = "0" + theNext.ToString();
                    }
                }
            }
        }


        private void loadSessionGroupName()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT DISTINCT [GroupName]
                                  FROM [tblGroupName]
                                  WHERE [GroupType]=@Type
                                  ORDER BY [GroupName]";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@Type", ddType.SelectedItem.Text);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddGroupName.Items.Clear();
                    ddGroupName.DataSource = dR;
                    ddGroupName.DataValueField = "GroupName";
                    ddGroupName.DataTextField = "GroupName";
                    ddGroupName.DataBind();
                    ddGroupName.Items.Insert(0, new ListItem("Select Group", "0"));
                    ddGroupName.SelectedIndex = 0;
                }
            }
        }

        private void loadSessionGroup()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT DISTINCT [SessionGroup] FROM [tblSubCategory] where sType=@Type ORDER BY [SessionGroup]";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@Type", ddType.SelectedItem.Text);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddSessionGroup.Items.Clear();
                    ddSessionGroup.DataSource = dR;
                    ddSessionGroup.DataValueField = "SessionGroup";
                    ddSessionGroup.DataTextField = "SessionGroup";
                    ddSessionGroup.DataBind();
                    ddSessionGroup.Items.Insert(0, new ListItem("Select Session Group", "0"));
                    ddSessionGroup.SelectedIndex = 0;
                }
            }
        }
        private void loadSessionType()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT DISTINCT [SessionType] FROM [tblSubCategory] WHERE [SessionGroup]=@SessionGroup ORDER BY SessionType";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@SessionGroup", ddSessionGroup.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddSessionType.Items.Clear();
                    ddSessionType.DataSource = dR;
                    ddSessionType.DataValueField = "SessionType";
                    ddSessionType.DataTextField = "SessionType";
                    ddSessionType.DataBind();
                    ddSessionType.Items.Insert(0, new ListItem("Select Session Type", "0"));
                    ddSessionType.SelectedIndex = 0;
                }
            }
        }
        private void loadCategory()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"Select sDescription FROM tblTypeItemCategory WHERE iItemClass_typ=@iItemClass_typ ORDER BY sDescription";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    cmD.Parameters.AddWithValue("@iItemClass_typ", ddType.SelectedValue);
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCategory.Items.Clear();
                    ddCategory.DataSource = dR;
                    ddCategory.DataValueField = "sDescription";
                    ddCategory.DataTextField = "sDescription";
                    ddCategory.DataBind();
                    ddCategory.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategory.SelectedIndex = 0;
                }
            }
        }

        private void loadCategoryAll()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"Select sDescription FROM tblTypeItemCategory ORDER BY iItemClass_typ,sDescription";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    cmD.CommandTimeout = 0;
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddCategory.Items.Clear();
                    ddCategory.DataSource = dR;
                    ddCategory.DataValueField = "sDescription";
                    ddCategory.DataTextField = "sDescription";
                    ddCategory.DataBind();
                    ddCategory.Items.Insert(0, new ListItem("Select Category", "0"));
                    ddCategory.SelectedIndex = 0;
                }
            }
        }

        protected void ddType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddType.SelectedValue == "0")
            {
                loadCategoryAll();
            }
            else
            {
                if (ddType.SelectedItem.Text == "Service")
                {
                    txtNoSession.ReadOnly = false;
                    RVSession.Enabled = true;
                    ckWithInv.Checked = false;
                    ckWithInv.Enabled = false;
                    loadSessionGroup();
                    
                    RequiredddSessionGroup.Enabled = true;
                    RequiredSessionType.Enabled = true;

                }
                else
                {
                    txtNoSession.ReadOnly = true;
                    RVSession.Enabled = false;
                    ckWithInv.Checked = true;
                    ckWithInv.Enabled = true;
                    loadSessionGroup();
                    RequiredddSessionGroup.Enabled = true;
                    RequiredSessionType.Enabled = false;

                    //ddSessionGroup.DataSource = null;
                    //ddSessionGroup.Items.Clear();

                    ddSessionType.DataSource = null;
                    ddSessionType.Items.Clear();
                }
                loadSessionGroupName();
                loadCategory();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveNewItem();
        }

        private void SaveNewItem()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.AddNewItemRegular";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@vPluCode", txtPluCode.Text.Trim());
                        cmD.Parameters.AddWithValue("@vFGCode", txtItemcode.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@vDESCRIPTION", txtDescription.Text.Trim());
                        cmD.Parameters.AddWithValue("@ShortName", txtShortName.Text.Trim());
                        cmD.Parameters.AddWithValue("@GroupName", ddGroupName.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@vCATEGORY", ddCategory.SelectedItem.Text);
                        
                        if (ddType.SelectedItem.Text == "Product")
                        {
                            cmD.Parameters.AddWithValue("@vUOM", "PC");
                            cmD.Parameters.AddWithValue("@SessionGroup", ddSessionGroup.SelectedItem.Text);
                            cmD.Parameters.AddWithValue("@SessionType", "NULL");
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@vUOM", "SVC");
                            cmD.Parameters.AddWithValue("@SessionGroup", ddSessionGroup.SelectedItem.Text);
                            cmD.Parameters.AddWithValue("@SessionType", ddSessionType.SelectedItem.Text);
                        }

                        cmD.Parameters.AddWithValue("@ItemType", ddType.SelectedItem.Text);

                        if (ddType.SelectedItem.Text == "Product")
                        {
                            cmD.Parameters.AddWithValue("@WithInventory", 1);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@WithInventory", 0);
                        }

                        if (ddType.SelectedItem.Text == "Product")
                        {
                            cmD.Parameters.AddWithValue("@NoSession", 0);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@NoSession", txtNoSession.Text);
                        }

                        cmD.Parameters.AddWithValue("@vUnitCost", txtPrice.Text);
                        cmD.Parameters.AddWithValue("@ValidFrom", DateTime.Now.ToShortDateString());
                        cmD.Parameters.AddWithValue("@ValidThru", DateTime.Now.AddYears(5).ToShortDateString());
                        cmD.Parameters.AddWithValue("@vCreatedBy", Session["EmpNo"].ToString());

                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();

                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            lblMsgWarning.Text = "Itemcode <b>" + txtItemcode.Text + "</b>\n already exists!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }
                        else
                        {
                            getLastPlucode();
                            loadCategory();
                            ddType.SelectedValue = "0";
                            ckWithInv.Checked = false;
                            ckWithInv.Enabled = false;


                            txtDescription.Text = string.Empty;
                            txtItemcode.Text = string.Empty;
                            txtNoSession.Text = string.Empty;
                            txtPrice.Text = string.Empty;
                            txtShortName.Text = string.Empty;

                            ddSessionGroup.DataSource = null;
                            ddSessionGroup.Items.Clear();

                            ddSessionType.DataSource = null;
                            ddSessionType.Items.Clear();

                            lblNote.Text = "Newly added item";
                            loadNewItem();
                            lblMsgWarning.Text = "New item successfully added!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }



                    }
                }
            }
        }


        private void loadNewItem()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT TOP 1 [vFGCode]
                                  ,[vPluCode]
                                  ,[vDESCRIPTION]
                                  ,[vCATEGORY]
                                  ,[ItemType]
                                  ,[NoSession]
                                  ,[vUnitCost]
                              FROM [vItemMaster]  ORDER BY vCreatedDate DESC";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        //cmD.CommandType = CommandType.StoredProcedure;
                        //cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        DataTable dT = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(cmD);
                        dA.Fill(dT);

                        gvNewItem.DataSource = dT;
                        gvNewItem.DataBind();
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

        protected void gvNewItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvNewItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    string dKey = gvNewItem.DataKeys[e.RowIndex].Value.ToString();
                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"dbo.DeleteNewItem";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@vFGCode", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    getLastPlucode();
                    lblNote.Text = string.Empty;
                    gvNewItem.DataSource = null;
                    gvNewItem.DataBind();
             

                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }

        protected void ddSessionGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSessionGroup.SelectedValue == "0")
            {
                ddSessionType.DataSource = null;
                ddSessionType.Items.Clear();
            }
            else
            {
                loadSessionType();
            }
        }


    }
}