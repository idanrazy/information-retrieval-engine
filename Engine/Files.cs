using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Net;
using System.Net.Http;


namespace Engine
{
    class Files
    {
        private string path;
        public string filename;
        private int num_of_files;
        private List<int> doc_start;
        private string[] data;


        public Files(string path)
        {
            filename = "";
            this.path = path+@"\corpus";
            data = null;
            num_of_files = -1;
            doc_start = new List<int>();
        }
        public bool checkPath()
        {
            try
            {
                if (Directory.Exists(path) == true)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool load()
        {
            if (!checkPath())
                return false;
            num_of_files = Directory.GetDirectories(path).Length;
            return true;
        }
        public string[] getFile(int index)
        {
            if (index < 0 || index >= num_of_files)
                return new string[0];
            try
            {
                doc_start.Clear();

                string f = Directory.GetDirectories(path)[index];
                f = f.Split('\\')[f.Split('\\').Length - 1];
                filename = f;

                //File.Copy(path + @"\" + f + @"\" + f, Directory.GetCurrentDirectory() + @"\" + f + ".txt");
                data = File.ReadAllLines(Directory.GetDirectories(path)[index] + @"\" + f);
                //Console.WriteLine(Directory.GetDirectories(path)[index]+@"\"+f);
                //File.Delete(Directory.GetCurrentDirectory() + @"\" + f + ".txt");

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] == "<DOC>")
                        doc_start.Add(i);
                }

                return data;
            }
            catch (Exception)
            {
                return new string[0];
            }

        }
        public string[] getDocument(int index)
        {
            if (index < 0 || index >= doc_start.Count||data==null)
                 return new string[0];

            List<string> doc = new List<string>();
            int i = doc_start[index];
            while (data[i] != @"</DOC>")
            {
                doc.Add(data[i]);
                i++;
            }
            doc.Add(@"</DOC>");

            return doc.ToArray();
        }

        public int getFilesCount()
        {
            return num_of_files;
        }
        public int getDocumentsCount()
        {
            if (data == null)
                return -1;
            return doc_start.Count;
        }

        public static string[] getStopWords(string s)
        {
            if (File.Exists(s + @"\stop_words.txt"))
                return File.ReadAllLines(s + @"\stop_words.txt");
            else
                return new string[0];
        }







        public static List<string> viki(string term)
        {
            string source = sourceCode(" https://en.wikipedia.org/wiki/" + term);
            if (source == "") 
                return new List<string>();
            string x = "div-col columns column-width";

            int start = source.IndexOf(x);
            if (start==-1)
                return new List<string>();
            source= source.Substring(start);

            x = "</ul>";
            int end = source.IndexOf(x);
            source = source.Substring(0, end);

            string ans = "";
            bool b1 = false, b2 = false;
            List<string> l = new List<string>();
            for (int i = 0; i < source.Length; i++)
            {         
                if (source[i] == '<')
                {
                    b1 = false;
                    b2 = false;
                    if (ans!="")
                    {
                        l.Add(ans);
                        ans = "";
                    }                  
                }
                if (b1&&b2)
                    ans += source[i];
                if (source[i] == 'h' && source[i+1] == 'r' && source[i+2] == 'e' && source[i+3] == 'f')
                    b1 = true;
                if ( source[i] == '>')
                    b2 = true;
            }

            return l;
        }
        private static string sourceCode(string Url)
        {
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(Url);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                sr.Close();
                myResponse.Close();

                return result;
            }
            catch (Exception)
            {
                return "";
            }
            
        }

        public static List<string> qureys(string url)
        {
            string[] qry = null;
            List<string> ans = new List<string>();
            try
            {
                qry = File.ReadAllLines(url);
            }
            catch (Exception) { }

            for (int i = 0; i < qry.Length; i++)
            {
                if (qry[i].Length < 5)
                    continue;
                string s = "";
                s= qry[i].Substring(0, 5);
                if (s == "<num>") 
                   ans.Add(qry[i].Split(':')[1].Replace(" ", ""));

                if (qry[i].Length < 7)
                    continue;
                s = qry[i].Substring(0, 7);
                if (s == "<title>")
                    ans.Add(qry[i].Substring(8, qry[i].Length - 9));
            }
            return ans;
        }


    }
}
