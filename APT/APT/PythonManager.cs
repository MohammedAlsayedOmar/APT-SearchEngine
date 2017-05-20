using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using LemmaSharp;

namespace APT
{
    public class PythonManager
    {
        static PythonManager instance;
        public static PythonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PythonManager();
                }
                return instance;
            }
        }

        ILemmatizer lmtz;

        private PythonManager()
        {
            lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);
        }


        //const string pythonLocation = @"C:\Python34\python.exe";
        //private ProcessStartInfo _startInfo = new ProcessStartInfo();
        //Process p;

        //private PythonManager()
        //{
        //    _startInfo.FileName = pythonLocation;
        //    _startInfo.UseShellExecute = false;
        //    _startInfo.RedirectStandardInput = true;
        //    _startInfo.RedirectStandardOutput = true;
        //    p = new Process();
        //    p.StartInfo = _startInfo;
        //    p.Start();
        //    p.StandardInput.WriteLine("python");
        //    p.StandardInput.WriteLine("from nltk.stem.wordnet import WordNetLemmatizer");
        //    p.StandardInput.WriteLine("lmtzr = WordNetLemmatizer()");
        //    //using (StreamWriter sw = p.StandardInput)
        //    //{
        //    //    if (sw.BaseStream.CanWrite)
        //    //    {
        //    //        sw.WriteLine("from nltk.stem.wordnet import WordNetLemmatizer");
        //    //        sw.WriteLine("lmtzr = WordNetLemmatizer()");
        //    //    }
        //    //}
        //}

        public string StemWord(string word)
        {
            string wordLower = word.ToLower();
            string lemma = lmtz.Lemmatize(wordLower);
            return lemma;
        }
      
    }
}