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

        string oneWordQuery(string word)
        {
            return "Intersect select DISTINCT kwp_p.URL_ID,url.URLName from KeyWords w,KeyWordsPosition_Paragraphs kwp_p, Url_Container url where w.KeyWords='" + word + "' and w.KeyWord_ID = kwp_p.KeyWord_ID and kwp_p.URL_ID = url.URL_ID ";

        }

        public DataTable querySearch(string[] words)
        {
            string tempquery = "";
            for (int i = 0; i < words.Length; i++)
            {
                tempquery += oneWordQuery(words[i]);
            }
            string query = tempquery.Substring(10);

            return dbMan.ExecuteReader(query);
        }

    }
}