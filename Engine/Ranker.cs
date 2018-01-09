using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Engine
{
    class Ranker
    {

        static public bool mweight = false;

        //calculate idf 
        public static double calculate_idf(string s)
        {
            if (Indexer.idf.ContainsKey(s))
                return Math.Log(Parse.DocDic.Count / Indexer.idf[s].idf, 2);
            return -1;
        }
        //calculate itf 
        public static double calculate_itf(string term, string docname, double tf)
        {
            if (Parse.DocDic.ContainsKey(docname))
            {
                string[] temp = Parse.DocDic[docname].Split(',');
                int docsize = -1;
                int.TryParse(temp[2], out docsize);
                if (docsize != -1)
                    return tf / docsize;
            }
            return -1;
        }
        //cosim formula
        public static Dictionary<string, double> cosim(string[] q)
        {
            double[] idf_q = new double[q.Length];
            Dictionary<string, double> grade = new Dictionary<string, double>();

            for (int i = 0; i < q.Length; i++)
            {

                idf_q[i] = calculate_idf(q[i]);
                if (idf_q[i] == -1)
                    continue;
                //read the post list of term-i 
                string postlist = Indexer.get_term_frompost(q[i], Indexer.path);
                if (postlist == null)
                    continue;
                List<KeyValuePair<string, double>> sortpost = Indexer.post_list(postlist);
                foreach (KeyValuePair<string, double> kvp in sortpost)
                {
                    double itf = calculate_itf(q[i], kvp.Key, kvp.Value);
                    if (itf == -1) // not found
                        continue;
                    //cosim formula
                    double g = itf * idf_q[i] / Math.Pow(getdocweight(kvp.Key) * Math.Pow(q.Length, 2), 0.5);
                    if (!grade.ContainsKey(kvp.Key))
                    {
                        grade.Add(kvp.Key, g);
                    }
                    else
                    {
                        grade[kvp.Key] = grade[kvp.Key] + g;

                    }

                }
            }
            return grade;
        }
        /// <summary>
        /// retrun order list by the docs that relvant to the query
        /// </summary>
        /// <param name="n"> numbers of docs to return</param>
        /// <returns></returns>
        public static List<string> getdocs(int n, string[] q)
        {
            Dictionary<string, double> grade = cosim(q);
            if (grade == null)
                return null;
            List<string> doclist = new List<string>();
            List<KeyValuePair<string, double>> doclistgrade = grade.ToList();
            doclistgrade.Sort((KeyValuePair<string, double> p1, KeyValuePair<string, double> p2) => { return p1.Value.CompareTo(p2.Value); });
            for (int i = 0; i < n && i < doclistgrade.Count; i++)
            {
                doclist.Add(doclistgrade.ElementAt(doclistgrade.Count - i - 1).Key);
            }
            return doclist;
        }

        //get the weight of doc(all words)
        public static double getdocweight(string docname)
        {
            if (Parse.DocDic.ContainsKey(docname))
            {
                string[] args = Parse.DocDic[docname].Split(',');
                if (args.Length == 4)
                    return double.Parse(args[3]);
            }
            return -1;
        }


        private static List<string> rows =new  List<string>();
        public static void addTolist(string qid, List<string> docs)
        {
            for (int i = 0; i < docs.Count; i++)
                rows.Add(qid + " 0 " + docs[i] + " 1 1.1 mt");
        }
        public static void writeList(string path)
        {
            try
            {
                StreamWriter w = new StreamWriter(path + @"\results.txt");
                foreach (string s in rows)
                    w.WriteLine(s);
                w.Close();
            }
            catch (Exception){}
            rows = new List<string>();
        }
        public static void cleanList()
        {
            rows.Clear();
        }






        //---------------


    }
}
