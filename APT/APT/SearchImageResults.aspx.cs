using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;


namespace APT
{
    public partial class SearchImageResults : System.Web.UI.Page
    {
        Controller controller;
        const string imagesPath = "~/Images/MyImages/";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Basic Search
                string searchResult = Request.QueryString["Textbox_Input"];
                Label_Data.Text = searchResult;
                Textbox_Input.Text = searchResult;
                string pageNumber = Request.QueryString["Page_Number"];
                pageNum.InnerText = pageNumber;

                //Actual Data
                if (searchResult == null)
                    return;
                string[] words = null;              

                string text = Regex.Replace(searchResult, "\".*?\"", string.Empty);
                string[] text2 = text.Split(null);
                text2 = text2.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                if (searchResult != "" && text2 != null)
                {
                    words = text2;
                }

                //STEM WORDS BEFORE SENDING NOT IMPLEMENTED!


                controller = new Controller();
                DataTable dt = null;
                int total = 0;
                int currentStart = (Int32.Parse(pageNumber) - 1) * 15;


                if (words != null)
                {
                    dt = controller.ImageQuerySearch(words);
                }

                if (dt == null)
                {
                    LinkUserControl link = Page.LoadControl("~/UserControls/LinkUserControl.ascx") as LinkUserControl;
                    ImagesPlaceHolder.Controls.Add(link);
                    arrowLeft.Visible = false;
                    arrowRight.Visible = false;
                    return;
                }

                total = dt.Rows.Count;

                for (int i = currentStart; i < currentStart + 15; i+=3)
                {
                    if (i > total - 1)
                        break;
                    ImageUserControl images = Page.LoadControl("~/UserControls/ImageUserControl.ascx") as ImageUserControl;
                    if (images != null)
                    {
                        if (!(i > total - 1))
                        {
                            images.ImageSRC1 = imagesPath + (dt.Rows[i][2]).ToString();
                            images.ImageLink1 = (dt.Rows[i][1]).ToString();
                        }
                        if (!(i + 1 > total - 1))
                        {
                            images.ImageSRC2 = imagesPath + (dt.Rows[i + 1][2]).ToString();
                            images.ImageLink2 = (dt.Rows[i + 1][1]).ToString();
                        }
                        if (!(i + 2 > total - 1))
                        {
                            images.ImageSRC3 = imagesPath + (dt.Rows[i + 2][2]).ToString();
                            images.ImageLink3 = (dt.Rows[i + 2][1]).ToString();
                        }
                    }
                    ImagesPlaceHolder.Controls.Add(images);
                }


                //Handle arrow keys ASUUME ONE DATABALE dt
                if (currentStart + 15 < total)
                {
                    arrowRight.Visible = true;
                }
                else
                {
                    arrowRight.Visible = false;
                }
                if (currentStart == 0)
                {
                    arrowLeft.Visible = false;
                }
                else
                {
                    arrowLeft.Visible = true;
                }
                arrowRight.HRef = "SearchImageResults.aspx?Textbox_Input=" + Label_Data.Text + "&Page_Number=" + (Int32.Parse(pageNumber) + 1);
                arrowLeft.HRef = "SearchImageResults.aspx?Textbox_Input=" + Label_Data.Text + "&Page_Number=" + (Int32.Parse(pageNumber) - 1);




            }
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (Label_Data.Text == "")
            {

            }
            else
            {
                Response.Redirect("SearchResults.aspx?Textbox_Input=" + Label_Data.Text + "&Page_Number=1");
            }
        }

        protected void BtnSearchAgain_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchImageResults.aspx?Textbox_Input=" + Textbox_Input.Text + "&Page_Number=1");
        }
    }
}