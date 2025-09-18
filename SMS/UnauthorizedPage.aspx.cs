using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMS
{
    public partial class UnauthorizedPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ClassMenu.disablecontrol(Convert.ToInt32(Session["vUser_Branch"]));
            }
        }
    }
}