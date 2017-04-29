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


        string FromClasuePhraseSearch(string KWP_TableName, int index)
        {
            return "KeyWords w" + index.ToString() + "," + KWP_TableName + " kwp_t" + index.ToString() + ", Url_Container url" + index.ToString() + " ";
        }
        string WhereClasuePhraseSearch(string word, int index)
        {
            return "w" + index.ToString() + ".KeyWords='" + word + "' and w" + index.ToString() + ".KeyWord_ID = kwp_t" + index.ToString() + ".KeyWord_ID and kwp_t" + index.ToString() + ".URL_ID = url" + index.ToString() + ".URL_ID ";
        }
        string WhereSpecialPhraseSearchClause(int index)
        {
            return "kwp_t" + index.ToString() + ".Position = kwp_t" + (index - 1).ToString() + ".Position + 1 ";
        }
        public DataTable phraseSearch(string[] words)
        {
            string innerQueryTitle = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title from ";
            string innerQueryHeader = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title from ";
            string innerQueryParagraph = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title from ";

            //1
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitle += FromClasuePhraseSearch("KeyWordsPosition_Titles", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitle += ", ";
                }
            }
            innerQueryTitle += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitle += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitle += "and ";                    
                }
            }
            innerQueryTitle += "and ";
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryTitle += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitle += "and ";
                }
            }
           
            //2
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeader += FromClasuePhraseSearch("KeyWordsPosition_Headers", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeader += ", ";
                }
            }
            innerQueryHeader += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeader += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeader += "and ";
                    if (i > 0)
                    {
                        innerQueryHeader += WhereSpecialPhraseSearchClause(i);
                        innerQueryHeader += "and ";
                    }
                }
            }

            //3
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraph += FromClasuePhraseSearch("KeyWordsPosition_Paragraphs", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraph += ", ";
                }
            }
            innerQueryParagraph += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraph += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraph += "and ";
                    if (i > 0)
                    {
                        innerQueryParagraph += WhereSpecialPhraseSearchClause(i);
                        innerQueryParagraph += "and ";
                    }
                }
            }
            string query = "select distinct URL_ID,URLName, URL_Title from( " + innerQueryTitle + " union all " + innerQueryHeader + " union all " + innerQueryParagraph + " )t";
            return dbMan.ExecuteReader(query);
        }


    }
}