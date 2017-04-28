using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace APT
{
    public class Controller
    {
        DBManager dbMan;
        public Controller()
        {
            dbMan = new DBManager();
        }


        public void TerminateConnection()
        {
            dbMan.CloseConnection();
        }

        string oneWordQueryParagraph(string word)
        {
            return "Intersect select DISTINCT url.URL_ID,url.URLName,url.URL_Title from KeyWords w,KeyWordsPosition_Paragraphs kwp_p, Url_Container url where w.KeyWords='" + word + "' and w.KeyWord_ID = kwp_p.KeyWord_ID and kwp_p.URL_ID = url.URL_ID ";
        }
        string oneWordQueryHeader(string word)
        {
            return "Intersect select DISTINCT url.URL_ID,url.URLName,url.URL_Title from KeyWords w, KeyWordsPosition_Headers kwp_h, Url_Container url where w.KeyWords='" + word + "' and w.KeyWord_ID = kwp_h.KeyWord_ID and kwp_h.URL_ID = url.URL_ID ";
        }
        string oneWordQueryTitle(string word)
        {
            return "Intersect select DISTINCT url.URL_ID,url.URLName,url.URL_Title from KeyWords w,KeyWordsPosition_Titles kwp_t, Url_Container url where w.KeyWords='" + word + "' and w.KeyWord_ID = kwp_t.KeyWord_ID and kwp_t.URL_ID = url.URL_ID ";
        }
        public DataTable querySearch(string[] words)
        {
            string tempQueryParagraph = "", tempQueryHeader = "", tempQueryTitle = "";
            for (int i = 0; i < words.Length; i++)
            {
                tempQueryParagraph += oneWordQueryParagraph(words[i]);
                tempQueryHeader += oneWordQueryHeader(words[i]);
                tempQueryTitle += oneWordQueryTitle(words[i]);
            }
            tempQueryParagraph = tempQueryParagraph.Substring(10);
            tempQueryHeader = tempQueryHeader.Substring(10);
            tempQueryTitle = tempQueryTitle.Substring(10);

            string query = "select distinct URL_ID,URLName, URL_Title from( " + tempQueryTitle + " union all " + tempQueryHeader + " union all " + tempQueryParagraph + " )t";
            return dbMan.ExecuteReader(query);
        }



    }
}