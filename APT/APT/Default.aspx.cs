using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace APT
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            if (Textbox_Input.Text == "")
            {

            }
            else
            {
                Response.Redirect("SearchResults.aspx?Textbox_Input=" + Textbox_Input.Text + "&Page_Number=1");
            }
            
        }

        protected void Btn_ImageSearch_Click(object sender, EventArgs e)
        {
            if(Textbox_Input.Text!= "")
                Response.Redirect("SearchImageResults.aspx?Textbox_Input=" + Textbox_Input.Text + "&Page_Number=1");
        }
    }
}