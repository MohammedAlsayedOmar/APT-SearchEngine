using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APT
{
    public partial class LinkUserControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string LinkHead
        {
            get {
                return linkHead.InnerHtml;
            }
            set {
                linkHead.InnerHtml = value;
            }
        }

        public string LinkHyperLink
        {
            get
            {
                return linkHyperlink.InnerHtml;
            }
            set
            {
                linkHyperlink.InnerHtml = value;
                linkHyperlink.HRef = value;
            }
        }
    }
}