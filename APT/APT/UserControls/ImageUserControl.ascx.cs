using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APT
{
    public partial class ImageUserControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public string ImageLink1
        {
            get { return imageLink1.HRef; }
            set { imageLink1.HRef = value; }
        }
        public string ImageSRC1
        {
            get { return imageSRC1.Src; }
            set { imageSRC1.Src = value; }
        }

        public string ImageLink2
        {
            get { return imageLink2.HRef; }
            set { imageLink2.HRef = value; }
        }
        public string ImageSRC2
        {
            get { return imageSRC2.Src; }
            set { imageSRC2.Src = value; }
        }

        public string ImageLink3
        {
            get { return imageLink3.HRef; }
            set { imageLink3.HRef = value; }
        }
        public string ImageSRC3
        {
            get { return imageSRC3.Src; }
            set { imageSRC3.Src = value; }
        }
    }   
}