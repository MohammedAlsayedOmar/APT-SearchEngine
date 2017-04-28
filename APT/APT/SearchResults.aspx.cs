using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace APT
{
    public partial class SearchResults : System.Web.UI.Page
    {
        Controller controller;
        int pageNumber;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //Basic Search
                string searchResult = Request.QueryString["Textbox_Input"];
                Label_Data.Text = searchResult;
                Textbox_Input.Text = searchResult;

                //Actual Data
                if (searchResult == null)
                    return;
                string[] words = searchResult.Split(null);
                //STEM WORDS BEFORE SENDING NOT IMPLEMENTED!
                controller = new Controller();
                DataTable dt = controller.querySearch(words);

                //Controls
                for (int i = 0; i < words.Length; i++)
                {
                    LinkUserControl link = Page.LoadControl("~/UserControls/LinkUserControl.ascx") as LinkUserControl;
                    if (link != null && dt != null)
                    {
                        link.LinkHead = (dt.Rows[i][2]).ToString();
                        link.LinkHyperLink = (dt.Rows[i][1]).ToString();
                    }
                    LinksPlaceHolder.Controls.Add(link);
                    if (dt == null)
                        break;
                }
               
            }
        }

        protected void Btn_ImageSearch_Click(object sender, EventArgs e)
        {
            if (Label_Data.Text == "")
            {

            }
            else
            {
                Response.Redirect("SearchImageResults.aspx?Textbox_Input=" + Label_Data.Text);
            }
        }

        protected void BtnSearchAgain_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchResults.aspx?Textbox_Input=" + Textbox_Input.Text);
        }
    }
}