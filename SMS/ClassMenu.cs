using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMS
{
    public class ClassMenu
    {
        public static void disablecontrol(int UserBranch)
        {

            var pageHandler = HttpContext.Current.CurrentHandler;
            if (pageHandler is System.Web.UI.Page)
            {
                if (Convert.ToInt32(UserBranch) == 1)
                {
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("BRDelivery").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("BRMonthlyPosting").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("BRSalesReturn").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("EncodeSales").Visible = false;
                    
                }
                else 
                {
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOIssuance").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOCreditCardTagging").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("BRSalesReturn").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOAddNewItem").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOSetupDiscount").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOAddPromoItem").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOSpecialDiscount").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOSetupSalesTarget").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("BRrptSalesReturn").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("BranchTarget").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("HOVATExempted").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("AdminSetupItem").Visible = false;
                    ((System.Web.UI.Page)pageHandler).Master.FindControl("AddItemToDiscount").Visible = false;

                    
                    
                    //
                }
            }

        }
    }
}