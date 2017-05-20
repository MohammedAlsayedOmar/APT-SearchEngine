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

        public int SearchHistory(string search)
        {
            search = search.Replace("'", "''");
            string query = "insert into SearchHistory (SearchHistory) values ('" + search + "')";
            return dbMan.ExecuteReaderString(query);
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
        string QueryNormalGenerator (string[] words)
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

            return "select distinct URL_ID,URLName, URL_Title from( " + tempQueryTitle + " union all " + tempQueryHeader + " union all " + tempQueryParagraph + " )t";
        }
        public DataTable QuerySearch(string[] words)
        {
            string query = QueryNormalGenerator(words);
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
            return "url"+ index.ToString() + ".URL_ID = url"+ (index - 1).ToString() + ".URL_ID and kwp_t" + index.ToString() + ".Position = kwp_t" + (index - 1).ToString() + ".Position + 1 ";
        }
        string QueryPhraseGenerator(string[] words)
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
            if(words.Length>1)
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
                }
            }
            if (words.Length > 1)
                innerQueryHeader += "and ";
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryHeader += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeader += "and ";
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
                }
            }
            if (words.Length > 1)
                innerQueryParagraph += "and ";
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryParagraph += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraph += "and ";
                }
            }

            //Final
            return  "select distinct URL_ID,URLName, URL_Title from( " + innerQueryTitle + " union all " + innerQueryHeader + " union all " + innerQueryParagraph + " )t";
        }
        public DataTable PhraseSearch(string[] words)
        {
            string query = QueryPhraseGenerator(words);
            return dbMan.ExecuteReader(query);
        }

        public DataTable CombinedSearch(string[] words, string[] phrases)
        {
            string normalSearch = QueryNormalGenerator(words);
            string phraseSearch = QueryPhraseGenerator(phrases);
            string query = "select distinct URL_ID, URLName, URL_Title from( " + normalSearch + " union all " + phraseSearch + " )t";
            return dbMan.ExecuteReader(query);
        }

        string oneWordImageSearch (string word)
        {
            return "intersect select DISTINCT url.URL_ID,url.URLName,name.ImageFileName from ImageNames name, KeyWordLocationURL kwL,ImageKeyWords w, Url_Container url where w.ImageKeyWords='" + word + "' and w.ImageKeyWord_ID = kwL.ImageKeyWords_ID and kwL.URL_ID = url.URL_ID and name.ImageFile_ID = kwL.ImageFile_ID ";
        }
        string ImageQueryGenerator(string[]words)
        {
            string tempQuery = "";
            for (int i = 0; i < words.Length; i++)
            {
                tempQuery += oneWordImageSearch(words[i]);
            }
            return tempQuery.Substring(10);
        }
        public DataTable ImageQuerySearch(string[] words)
        {
            string query = ImageQueryGenerator(words);
            return dbMan.ExecuteReader(query);
        }

        //http://stackoverflow.com/questions/10373561/convert-a-number-to-a-letter-in-c-sharp-for-use-in-microsoft-excel
        static string GetColumnName(int index)
        {
            const string letters = "abcdefghijklmnopqrstuvwxyz";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        string Joins(string KWP_TableName, string word, int letterIndex)
        {
            string letter = GetColumnName(letterIndex);
            string returned = "left join (select url0.URL_ID ,count(kwp_t0.ID) as word from KeyWords w0,"+ KWP_TableName + " kwp_t0, Url_Container url0 where w0.KeyWords = '" + word + "' and w0.KeyWord_ID = kwp_t0.KeyWord_ID and kwp_t0.URL_ID = url0.URL_ID group by url0.URL_ID) " + letter + " on a.URL_ID = " + letter + ".URL_ID ";
            return returned;
        }

        string JoinsWhereClause(string word, int letterIndex)
        {
            string letter = GetColumnName(letterIndex);
            string returned = "cast(" + letter + ".word as float) / cast(a.TotalNumberOfWords as float) < 0.5 ";
            return returned;
        }

        string RankedQuerySearchQuery(string[] words)
        {
            const int TitleFactor = 10, HeaderFactor = 5, ParaFactor = 1;


            //TITLE
            string innerQueryTitleBeforeJoin = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title, url0.TotalNumberOfWords, url0.Frequency from ";
            //1
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitleBeforeJoin += FromClasuePhraseSearch("KeyWordsPosition_Titles", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitleBeforeJoin += ", ";
                }
            }
            innerQueryTitleBeforeJoin += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitleBeforeJoin += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitleBeforeJoin += "and ";
                }
            }
            //if (words.Length > 1)
            //    innerQueryTitleBeforeJoin += "and ";
            //for (int i = 1; i < words.Length; i++)
            //{
            //    innerQueryTitleBeforeJoin += WhereSpecialPhraseSearchClause(i);
            //    if (words.Length > 1 && i != words.Length - 1)
            //    {
            //        innerQueryTitleBeforeJoin += "and ";
            //    }
            //}

            string innerQueryTitle = "select a.URL_ID,a.URLName,a.URL_Title,a.TotalNumberOfWords, total = ( ";
            for (int i = 0; i < words.Length; i++)
            {
                string toAdd = GetColumnName(i + 1) + ".word ";
                innerQueryTitle += toAdd;
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitle += "+ ";
                }
            }

            innerQueryTitle += ") * " + TitleFactor.ToString();
            innerQueryTitle += "+ a.Frequency ";
            innerQueryTitle += " from (( ";
            innerQueryTitle += innerQueryTitleBeforeJoin;
            innerQueryTitle += " ) a ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitle += Joins("KeyWordsPosition_Titles", words[i], i + 1);
            }
            innerQueryTitle += ") where a.TotalNumberOfWords > 1 and ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitle += JoinsWhereClause(words[i], i + 1);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitle += "and ";
                }
            }
            //innerQueryTitle += "order by total desc ";


            //HEADERS
            string innerQueryHeaderBeforeJoin = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title, url0.TotalNumberOfWords, url0.Frequency from "; 
            //2
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeaderBeforeJoin += FromClasuePhraseSearch("KeyWordsPosition_Headers", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaderBeforeJoin += ", ";
                }
            }
            innerQueryHeaderBeforeJoin += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeaderBeforeJoin += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaderBeforeJoin += "and ";
                }
            }
            //if (words.Length > 1)
            //    innerQueryHeaderBeforeJoin += "and ";
            //for (int i = 1; i < words.Length; i++)
            //{
            //    innerQueryHeaderBeforeJoin += WhereSpecialPhraseSearchClause(i);
            //    if (words.Length > 1 && i != words.Length - 1)
            //    {
            //        innerQueryHeaderBeforeJoin += "and ";
            //    }
            //}

            string innerQueryHeaders = "select a.URL_ID,a.URLName,a.URL_Title,a.TotalNumberOfWords, total = ( ";
            for (int i = 0; i < words.Length; i++)
            {
                string toAdd = GetColumnName(i + 1) + ".word ";
                innerQueryHeaders += toAdd;
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaders += "+ ";
                }
            }

            innerQueryHeaders += ") * " + HeaderFactor.ToString();
            innerQueryHeaders += "+ a.Frequency ";
            innerQueryHeaders += " from (( ";
            innerQueryHeaders += innerQueryHeaderBeforeJoin;
            innerQueryHeaders += " ) a ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeaders += Joins("KeyWordsPosition_Headers", words[i], i + 1);
            }
            innerQueryHeaders += ") where a.TotalNumberOfWords > 1 and ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeaders += JoinsWhereClause(words[i], i + 1);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaders += "and ";
                }
            }
            //innerQueryHeaders += "order by total desc ";


            //PARAGRAPHS
            string innerQueryParagraphsBeforeJoin = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title, url0.TotalNumberOfWords, url0.Frequency from "; 
            //3
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraphsBeforeJoin += FromClasuePhraseSearch("KeyWordsPosition_Paragraphs", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphsBeforeJoin += ", ";
                }
            }
            innerQueryParagraphsBeforeJoin += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraphsBeforeJoin += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphsBeforeJoin += "and ";
                }
            }
            if (words.Length > 1)
                innerQueryParagraphsBeforeJoin += "and ";
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryParagraphsBeforeJoin += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphsBeforeJoin += "and ";
                }
            }

            string innerQueryParagraphs = "select a.URL_ID,a.URLName,a.URL_Title,a.TotalNumberOfWords, total = ( ";
            for (int i = 0; i < words.Length; i++)
            {
                string toAdd = GetColumnName(i + 1) + ".word ";
                innerQueryParagraphs += toAdd;
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphs += "+ ";
                }
            }

            innerQueryParagraphs += ") * " + ParaFactor.ToString();
            innerQueryParagraphs += "+ a.Frequency ";
            innerQueryParagraphs += " from (( ";
            innerQueryParagraphs += innerQueryParagraphsBeforeJoin;
            innerQueryParagraphs += " ) a ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraphs += Joins("KeyWordsPosition_Paragraphs", words[i], i + 1);
            }
            innerQueryParagraphs += ") where a.TotalNumberOfWords > 1 and ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraphs += JoinsWhereClause(words[i], i + 1);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphs += "and ";
                }
            }
            //innerQueryParagraphs += "order by total desc ";



           return "select distinct URL_ID,URLName, URL_Title, total = sum(total) from( " + innerQueryTitle + " union all " + innerQueryHeaders + " union all " + innerQueryParagraphs + " )t group by URL_ID,URLName, URL_Title order by total desc";        
        }


        string JoinsPhrase(string KWP_TableName, string[] words, int letterIndex)
        {
            //genereta 7aga zai el inner da bs replace kam 7aga kda
            return "";
        }
        string RankedPhraseSearchQuery(string[] words)
        {
            const int TitleFactor = 10, HeaderFactor = 5, ParaFactor = 1;

            //TITLE
            string innerQueryTitleBeforeJoin = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title, url0.TotalNumberOfWords, url0.Frequency from ";
            string innerQueryTitleJoin = "left join (select url0.URL_ID ,count(kwp_t0.ID) as word from ";
            //1
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitleBeforeJoin += FromClasuePhraseSearch("KeyWordsPosition_Titles", i);
                innerQueryTitleJoin += FromClasuePhraseSearch("KeyWordsPosition_Titles", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitleBeforeJoin += ", ";
                    innerQueryTitleJoin += ", ";
                }
            }
            innerQueryTitleBeforeJoin += "where ";
            innerQueryTitleJoin += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryTitleBeforeJoin += WhereClasuePhraseSearch(words[i], i);
                innerQueryTitleJoin += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitleBeforeJoin += "and ";
                    innerQueryTitleJoin += "and ";
                }
            }
            if (words.Length > 1)
            {
                innerQueryTitleBeforeJoin += "and ";
                innerQueryTitleJoin += "and ";
            }
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryTitleBeforeJoin += WhereSpecialPhraseSearchClause(i);
                innerQueryTitleJoin += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryTitleBeforeJoin += "and ";
                    innerQueryTitleJoin += "and ";
                }
            }
            innerQueryTitleJoin += "group by url0.URL_ID) b on a.URL_ID = b.URL_ID ";
            string innerQueryTitle = "select a.URL_ID,a.URLName,a.URL_Title,a.TotalNumberOfWords, total = ( b.word ";

            innerQueryTitle += ") * " + TitleFactor.ToString();
            innerQueryTitle += "+ a.Frequency ";
            innerQueryTitle += " from (( ";
            innerQueryTitle += innerQueryTitleBeforeJoin;
            innerQueryTitle += " ) a ";
            innerQueryTitle += innerQueryTitleJoin;

            innerQueryTitle += ") where a.TotalNumberOfWords > 1 and cast(b.word as float) / cast(a.TotalNumberOfWords as float) < 0.5 ";

            //Header
            string innerQueryHeaderBeforeJoin = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title, url0.TotalNumberOfWords, url0.Frequency from "; 
            string innerQueryHeaderJoin = "left join (select url0.URL_ID ,count(kwp_t0.ID) as word from ";
            //2
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeaderBeforeJoin += FromClasuePhraseSearch("KeyWordsPosition_Headers", i);
                innerQueryHeaderJoin += FromClasuePhraseSearch("KeyWordsPosition_Headers", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaderBeforeJoin += ", ";
                    innerQueryHeaderJoin += ", ";
                }
            }
            innerQueryHeaderBeforeJoin += "where ";
            innerQueryHeaderJoin += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryHeaderBeforeJoin += WhereClasuePhraseSearch(words[i], i);
                innerQueryHeaderJoin += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaderBeforeJoin += "and ";
                    innerQueryHeaderJoin += "and ";
                }
            }
            if (words.Length > 1)
            {
                innerQueryHeaderBeforeJoin += "and ";
                innerQueryHeaderJoin += "and ";
            }
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryHeaderBeforeJoin += WhereSpecialPhraseSearchClause(i);
                innerQueryHeaderJoin += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryHeaderBeforeJoin += "and ";
                    innerQueryHeaderJoin += "and ";
                }
            }
            innerQueryHeaderJoin += "group by url0.URL_ID) b on a.URL_ID = b.URL_ID ";
            string innerQueryHeader = "select a.URL_ID,a.URLName,a.URL_Title,a.TotalNumberOfWords, total = ( b.word ";

            innerQueryHeader += ") * " + HeaderFactor.ToString();
            innerQueryHeader += "+ a.Frequency ";
            innerQueryHeader += " from (( ";
            innerQueryHeader += innerQueryHeaderBeforeJoin;
            innerQueryHeader += " ) a ";
            innerQueryHeader += innerQueryHeaderJoin;

            innerQueryHeader += ") where a.TotalNumberOfWords > 1 and cast(b.word as float) / cast(a.TotalNumberOfWords as float) < 0.5 ";


            //Paragraph
            string innerQueryParagraphBeforeJoin = "select DISTINCT url0.URL_ID,url0.URLName ,url0.URL_Title, url0.TotalNumberOfWords, url0.Frequency from ";
            string innerQueryParagraphJoin = "left join (select url0.URL_ID ,count(kwp_t0.ID) as word from ";
            //3
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraphBeforeJoin += FromClasuePhraseSearch("KeyWordsPosition_Paragraphs", i);
                innerQueryParagraphJoin += FromClasuePhraseSearch("KeyWordsPosition_Paragraphs", i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphBeforeJoin += ", ";
                    innerQueryParagraphJoin += ", ";
                }
            }
            innerQueryParagraphBeforeJoin += "where ";
            innerQueryParagraphJoin += "where ";
            for (int i = 0; i < words.Length; i++)
            {
                innerQueryParagraphBeforeJoin += WhereClasuePhraseSearch(words[i], i);
                innerQueryParagraphJoin += WhereClasuePhraseSearch(words[i], i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphBeforeJoin += "and ";
                    innerQueryParagraphJoin += "and ";
                }
            }
            if (words.Length > 1)
            {
                innerQueryParagraphBeforeJoin += "and ";
                innerQueryParagraphJoin += "and ";
            }
            for (int i = 1; i < words.Length; i++)
            {
                innerQueryParagraphBeforeJoin += WhereSpecialPhraseSearchClause(i);
                innerQueryParagraphJoin += WhereSpecialPhraseSearchClause(i);
                if (words.Length > 1 && i != words.Length - 1)
                {
                    innerQueryParagraphBeforeJoin += "and ";
                    innerQueryParagraphJoin += "and ";
                }
            }
            innerQueryParagraphJoin += "group by url0.URL_ID) b on a.URL_ID = b.URL_ID ";
            string innerQueryParagraph = "select a.URL_ID,a.URLName,a.URL_Title,a.TotalNumberOfWords, total = ( b.word ";

            innerQueryParagraph += ") * " + ParaFactor.ToString();
            innerQueryParagraph += "+ a.Frequency ";

            innerQueryParagraph += " from (( ";
            innerQueryParagraph += innerQueryParagraphBeforeJoin;
            innerQueryParagraph += " ) a ";
            innerQueryParagraph += innerQueryParagraphJoin;

            innerQueryParagraph += ") where a.TotalNumberOfWords > 1 and cast(b.word as float) / cast(a.TotalNumberOfWords as float) < 0.5 ";

            return "select distinct URL_ID,URLName, URL_Title, total = sum(total) from( " + innerQueryTitle + " union all " + innerQueryHeader + " union all " + innerQueryParagraph + " )t group by URL_ID,URLName, URL_Title order by total desc";         
        }

        public DataTable RankedQuerySearch(string[] words)
        {
            string query = RankedQuerySearchQuery(words);
            return dbMan.ExecuteReader(query);
        }


        public DataTable RankedPhraseSearch(string[] words)
        {
            string query = RankedPhraseSearchQuery(words);
            return dbMan.ExecuteReader(query);
        }


        public DataTable CombinedRankedSearch(string[] words, string[] phrases)
        {
            string normalSearch = RankedQuerySearchQuery(words);
            string phraseSearch = RankedPhraseSearchQuery(phrases);
            string query = "select URL_ID, URLName, URL_Title, total = sum(total) from( " + normalSearch + " union all " + phraseSearch + " )t";
            query = query.Replace("order by total desc", "");
            query += " group by URL_ID, URLName, URL_Title order by total desc";
            return dbMan.ExecuteReader(query);
        }

    }
}