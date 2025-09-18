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
    public partial class AddPromoItem : System.Web.UI.Page
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
                    loadPerBranch();
                    loadNewItem();
                    
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
        private void loadPerBranch()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT BrCode,BrName FROM MyBranchList Where BrCode between 4 and 29  ORDER BY BrCode";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddBranch.Items.Clear();
                    ddBranch.DataSource = dR;
                    ddBranch.DataValueField = "BrCode";
                    ddBranch.DataTextField = "BrName";
                    ddBranch.DataBind();
                    ddBranch.Items.Insert(0, new ListItem("All Branches", "0"));
                    ddBranch.SelectedIndex = 0;
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
                    rbWithInventory.Checked = false;
                    rbWithInventory.Enabled = false;
                    loadSessionGroup();

                    RequiredddSessionGroup.Enabled = true;
                    RequiredSessionType.Enabled = true;
                    rbRegPromo.Checked = true;
                    rbRegPromo.Enabled = true;
                }
                else
                {
                    txtNoSession.ReadOnly = true;
                    RVSession.Enabled = false;
                    rbWithInventory.Enabled = true;

                    rbRegPromo.Checked = false;
                    rbRegPromo.Enabled = false;

                    loadSessionGroup();
                    RequiredddSessionGroup.Enabled = true;
                    RequiredSessionType.Enabled = false;

                    //ddSessionGroup.DataSource = null;
                    //ddSessionGroup.Items.Clear();

                    ddSessionType.DataSource = null;
                    ddSessionType.Items.Clear();
                }
                loadCategory();
                loadSessionGroupName();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveNewItem();
            
            //if (rbSales.Checked == true)
            //{
            //    int KitOption = 3;
            //    SaveKitItem(KitOption);
            //}
            //else if (rbFree.Checked == true)
            //{
            //    int KitOption = 4;
            //    SaveKitItem(KitOption);
            //}

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
                    string stR = @"dbo.AddNewItemRegularPromo";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@vPluCode", txtPluCode.Text.Trim());
                        cmD.Parameters.AddWithValue("@PromoCode", txtItemcode.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@vDESCRIPTION", txtDescription.Text.Trim());
                        cmD.Parameters.AddWithValue("@ShortName", txtShortName.Text.Trim());
                        cmD.Parameters.AddWithValue("@GroupName", ddGroupName.SelectedItem.Text);
                        cmD.Parameters.AddWithValue("@vCATEGORY", ddCategory.SelectedItem.Text);

                        if (ddType.SelectedItem.Text == "Product")
                        {
                            cmD.Parameters.AddWithValue("@vUOM", "PC");
                            cmD.Parameters.AddWithValue("@SessionGroup", "NULL");
                            cmD.Parameters.AddWithValue("@SessionType", "NULL");
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@vUOM", "SVC");
                            cmD.Parameters.AddWithValue("@SessionGroup", ddSessionGroup.SelectedItem.Text);
                            cmD.Parameters.AddWithValue("@SessionType", ddSessionType.SelectedItem.Text);
                        }

                        cmD.Parameters.AddWithValue("@ItemType", ddType.SelectedItem.Text);

                        if (rbWithInventory.Checked == true)
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
                        cmD.Parameters.AddWithValue("@ValidFrom", txtDateFrom.Text);
                        cmD.Parameters.AddWithValue("@ValidThru", txtDate.Text);
                        cmD.Parameters.AddWithValue("@vCreatedBy", Session["EmpNo"].ToString());
                        if (ddBranch.SelectedValue == "0")
                        {
                            cmD.Parameters.AddWithValue("@ToAllBranches", 1);
                            cmD.Parameters.AddWithValue("@BrCode", 0);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@ToAllBranches", 0);
                            cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        }


                        if (rbFree.Checked == true | rbSales.Checked==true)
                        {
                            cmD.Parameters.AddWithValue("@Iskit", 1);
                        }
                        else
                        {
                            cmD.Parameters.AddWithValue("@Iskit", 0);
                        }

                        cmD.Parameters.AddWithValue("@ResultValue", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();




                        int Result = Convert.ToInt32(cmD.Parameters["@ResultValue"].Value);

                        if (Result == 99)
                        {
                            //20240902
                            if (ddBranch.SelectedValue == "0")
                            {
                                lblMsgWarning.Text = "Itemcode <b>" + txtItemcode.Text + "</b>\n already exists!";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            }
                            else
                            {
                                AddPromoItemToOtherBranch();
                            }
                        }
                        else
                        {

                            if (rbSales.Checked == true)
                            {
                                if (ddBranch.SelectedValue == "0")
                                {
                                    int KitOption = 3;
                                    SaveKitItem(KitOption);
                                }
                                else
                                {
                                    int KitOption = 5;
                                    SaveKitItem(KitOption);
                                }
                            }
                            else if (rbFree.Checked == true)
                            {
                                if (ddBranch.SelectedValue == "0")
                                {
                                    int KitOption = 4;
                                    SaveKitItem(KitOption);
                                }
                                else
                                {
                                    int KitOption = 6;
                                    SaveKitItem(KitOption);
                                }
                            }


                            if (ddBranch.SelectedValue == "0")
                            {
                                loadCategory();
                                loadSessionGroup();
                                loadSessionGroupName();
                                loadSessionType();
                                ddType.SelectedValue = "0";
                                txtPluCode.Text = string.Empty;
                                txtDescription.Text = string.Empty;
                                txtItemcode.Text = string.Empty;
                                txtNoSession.Text = string.Empty;
                                txtPrice.Text = string.Empty;
                                txtShortName.Text = string.Empty;
                            }
                           



                            

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



        private void AddPromoItemToOtherBranch()
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"dbo.AddNewItemRegularPromoOtherBranch";
                    using (SqlCommand cmD = new SqlCommand(stR, conN))
                    {
                        conN.Open();
                        cmD.CommandTimeout = 0;
                        cmD.CommandType = CommandType.StoredProcedure;
                        cmD.Parameters.AddWithValue("@PromoCode", txtItemcode.Text.Trim().ToUpper());
                        cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                        cmD.Parameters.AddWithValue("@ResultValue2", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmD.ExecuteNonQuery();




                        int Result2 = Convert.ToInt32(cmD.Parameters["@ResultValue2"].Value);

                        if (Result2 == 98)
                        {
                            //20240902
                            
                                lblMsgWarning.Text = "Itemcode promo <b>" + txtItemcode.Text + "</b>\n already exist on the selected branch.";
                                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                                return;
                            
                        }
                        else
                        {

                            lblNote.Text = "Newly added item";
                            loadNewItem();
                            lblMsgWarning.Text = "New promo item successfully added!";
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                            return;
                        }



                    }
                }
            }
        }


        private void SaveKitItem(int theOption)
        {
            if (IsPageRefresh == true)
            {
                Response.Redirect(Request.Url.AbsoluteUri);
            }
            else
            {
                try
                {

                    using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                    {
                        string stR = @"dbo.InsertIntoKitItems";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@PromoCode", txtItemcode.Text);
                            cmD.Parameters.AddWithValue("@vFGCode", ddITemFG.SelectedValue);
                            cmD.Parameters.AddWithValue("@KitOption", theOption);
                            cmD.Parameters.AddWithValue("@qty", txtQty.Text);
                            cmD.Parameters.AddWithValue("@BrCode", ddBranch.SelectedValue);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    LoadFGItem();
                    txtQty.Text = string.Empty;

                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }

        }

        private void loadNewItem()
        {
            try
            {
                using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
                {
                    string stR = @"SELECT [vFGCode]
                                  ,[vPluCode]
                                  ,[vDESCRIPTION]
                                  ,[vCATEGORY]
                                  ,[ItemType]
                                  ,[NoSession]
                                  ,[vUnitCost]
                              FROM [vItemMaster] where CONVERT(CHAR(10),vCreatedDate,121)=CONVERT(CHAR(10),GETDATE(),121) and IsPromo=1 ORDER BY vCreatedDate DESC";
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
                        string stR = @"dbo.DeleteNewItemPromo";
                        using (SqlCommand cmD = new SqlCommand(stR, conN))
                        {
                            conN.Open();
                            cmD.CommandTimeout = 0;
                            cmD.CommandType = CommandType.StoredProcedure;
                            cmD.Parameters.AddWithValue("@vFGCode", dKey);
                            cmD.ExecuteNonQuery();
                        }
                    }

                    //getLastPlucode();
                    lblNote.Text = string.Empty;

                    loadNewItem();
                    //gvNewItem.DataSource = null;
                    //gvNewItem.DataBind();
                    lblMsgWarning.Text = "Item successfully deleted!";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;

                    
                }
                catch (Exception x)
                {
                    lblMsgWarning.Text = x.Message;
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Popup", "ShowWarningMsg();", true);
                    return;
                }
            }
        }

        //public int KitOption;
        protected void rbRegPromo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRegPromo.Checked==true)
            {
                RequiredItem.Enabled = false;
                ddITemFG.DataSource = null;
                ddITemFG.Enabled = false;
                txtQty.Text = string.Empty;
                RequiredQty.Enabled = false;
                txtQty.Enabled = false;
                //KitOption = 1;

            }
        }

        protected void rbWithInventory_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWithInventory.Checked == true)
            {
                RequiredItem.Enabled = false;
                ddITemFG.DataSource = null;
                ddITemFG.Enabled = false;
                //KitOption = 2;
                RequiredQty.Enabled = false;
                txtQty.Text = string.Empty;
                txtQty.Enabled = false;

            }
        }

        protected void rbSales_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSales.Checked==true)
            {
                RequiredQty.Enabled = true;
                RequiredItem.Enabled = true;
                ddITemFG.Enabled = true;
                LoadFGItem();
               // KitOption = 3;

                RequiredQty.Enabled = true;
                txtQty.Enabled = true;
            }
        }

        protected void rbFree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFree.Checked == true)
            {
                RequiredQty.Enabled = true;
                RequiredItem.Enabled = true;
                ddITemFG.Enabled = true;
                LoadFGItem();
               // KitOption = 4;

                RequiredQty.Enabled = true;
                txtQty.Enabled = true;
            }
        }


        private void LoadFGItem()
        {
            using (SqlConnection conN = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString))
            {
                string stR = @"SELECT vFGCode,vPluCode + ' - ' + LTRIM(RTRIM(vDESCRIPTION)) AS [vDESCRIPTION] FROM vItemMaster where vStat=1 and vPluCode like '0%' and IsPromo=0 and ItemType='Product' and WithInventory=1";
                using (SqlCommand cmD = new SqlCommand(stR, conN))
                {
                    conN.Open();
                    SqlDataReader dR = cmD.ExecuteReader();

                    ddITemFG.Items.Clear();
                    ddITemFG.DataSource = dR;
                    ddITemFG.DataValueField = "vFGCode";
                    ddITemFG.DataTextField = "vDESCRIPTION";
                    ddITemFG.DataBind();
                    ddITemFG.Items.Insert(0, new ListItem("Please select item", "0"));
                    ddITemFG.SelectedIndex = 0;
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

    }
}