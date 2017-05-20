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
    public partial class SearchResults : System.Web.UI.Page
    {
        Controller controller;
        string pageNumber;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                controller = new Controller();

                //Basic Search
                string searchResult = Request.QueryString["Textbox_Input"];
                Label_Data.Text = searchResult;
                Textbox_Input.Text = searchResult;
                pageNumber = Request.QueryString["Page_Number"];
                pageNum.InnerText = pageNumber;
                if (searchResult != null)
                {
                    controller.SearchHistory(searchResult);
                }

                //Actual Data
                if (searchResult == null)
                    return;
                string phraseSearchWords = null;
                string[] words = null;
                string[] phraseWords = null;
                //string[] words = searchResult.Split(null);
                var reg = new Regex("\".*?\"");
                var matches = reg.Matches(searchResult);
                foreach (var item in matches)
                {
                    phraseSearchWords = item.ToString(); //take only first
                    break;
                }
                if (phraseSearchWords != null)
                {
                    phraseSearchWords = phraseSearchWords.Replace('"', ' ').Trim();
                    phraseWords = phraseSearchWords.Split(null);

                }

                string text = Regex.Replace(searchResult, "\".*?\"", string.Empty);
                string []text2 = text.Split(null);
                text2 = text2.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                try
                {
                    if (searchResult != "" && text2[0] != "")
                    {
                        words = text2;
                    }
                }
                catch
                {
                    words = null;
                }

                //STEM WORDS BEFORE SENDING NOT IMPLEMENTED!             
                if(words != null)
                {
                    for (int i = 0; i < words.Length; i++)
                    {
                        words[i] = PythonManager.Instance.StemWord(words[i]);
                    }
                }
                if (phraseWords != null)
                {
                    for (int i = 0; i < phraseWords.Length; i++)
                    {
                        phraseWords[i] = PythonManager.Instance.StemWord(phraseWords[i]);
                    }
                }

                DataTable dt = null;
                int total = 0;
                int currentStart = (Int32.Parse(pageNumber) - 1) * 10;


                if (words != null && phraseWords == null)
                {
                    dt = controller.RankedQuerySearch(words);
                }
                else if (phraseWords != null && words == null)
                {
                    dt = controller.RankedPhraseSearch(phraseWords);
                }
                else if (words != null && phraseWords != null)
                {
                    dt = controller.CombinedRankedSearch(words, phraseWords);
                }
                
                if(dt==null)
                {
                    LinkUserControl link = Page.LoadControl("~/UserControls/LinkUserControl.ascx") as LinkUserControl;
                    LinksPlaceHolder.Controls.Add(link);
                    Btn_Left.Visible = false;
                    Btn_Right.Visible = false;
                    return;
                }

                total = dt.Rows.Count;

                for (int i = currentStart; i < currentStart + 10; i++)
                {
                    if (i > total - 1)
                        break;
                    LinkUserControl link = Page.LoadControl("~/UserControls/LinkUserControl.ascx") as LinkUserControl;
                    if (link != null && dt != null)
                    {
                        link.LinkHead = (dt.Rows[i][2]).ToString();
                        link.LinkHyperLink = (dt.Rows[i][1]).ToString();
                    }
                    LinksPlaceHolder.Controls.Add(link);
                }



                //Handle arrow keys ASUUME ONE DATABALE dt
                if (currentStart + 10 < total)
                {
                    Btn_Right.Visible = true;
                }
                else
                {
                    Btn_Right.Visible = false;
                }
                if (currentStart == 0)
                {
                    Btn_Left.Visible = false;
                }
                else
                {
                    Btn_Left.Visible = true;
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
                Response.Redirect("SearchImageResults.aspx?Textbox_Input=" + Label_Data.Text + "&Page_Number=1");
            }
        }

        protected void BtnSearchAgain_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchResults.aspx?Textbox_Input=" + Textbox_Input.Text + "&Page_Number=1");
        }

        protected void Btn_Right_Click(object sender, EventArgs e)
        {
            pageNumber = Request.QueryString["Page_Number"];
            string RightLink = "SearchResults.aspx?Textbox_Input=" + Label_Data.Text + "&Page_Number=" + (Int32.Parse(pageNumber) + 1);
            Response.Redirect(RightLink);
        }

        protected void Btn_Left_Click(object sender, EventArgs e)
        {
            pageNumber = Request.QueryString["Page_Number"];
            string LeftLink = "SearchResults.aspx?Textbox_Input=" + Label_Data.Text + "&Page_Number=" + (Int32.Parse(pageNumber) - 1);
            Response.Redirect(LeftLink);
        }
    }
}