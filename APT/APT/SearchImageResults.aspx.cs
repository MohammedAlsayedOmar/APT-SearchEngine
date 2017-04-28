using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace APT
{
    public partial class SearchImageResults : System.Web.UI.Page
    {
        const string imagesPath = "~/Images/MyImages/";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Basic Search
                string searchResult = Request.QueryString["Textbox_Input"];
                Label_Data.Text = searchResult;
                Textbox_Input.Text = searchResult;

                //Control
                for (int i = 0; i < 5; i++)
                {
                    ImageUserControl images = Page.LoadControl("~/UserControls/ImageUserControl.ascx") as ImageUserControl;
                    if (images != null)
                    {
                        images.ImageSRC1 = imagesPath + "8_" + (i + 5) + ".jpg";
                        images.ImageLink1 = "https://www.google.com/";

                        images.ImageSRC2 = imagesPath + "8_" + (i + 6) + ".jpg";
                        images.ImageLink2 = "https://www.google.com/";

                        images.ImageSRC3 = imagesPath + "8_" + (i + 7) + ".jpg";
                        images.ImageLink3 = "";
                    }
                    ImagesPlaceHolder.Controls.Add(images);
                }
            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (Label_Data.Text == "")
            {

            }
            else
            {
                Response.Redirect("SearchResults.aspx?Textbox_Input=" + Label_Data.Text);
            }
        }

        protected void BtnSearchAgain_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchImageResults.aspx?Textbox_Input=" + Textbox_Input.Text);
        }
    }
}