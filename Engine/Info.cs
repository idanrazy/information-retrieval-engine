using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Engine
{
    class Info
    {
        
        public static long postsize;
        public static long d_size;
        public static int terms;
        public static int docs;
        public static long cache;
        public static string p;
        public static string time = "";
        public static void sizes(string path)
        {
            docs = Parse.DocDic.Count();
            p = path;
            FileStream fs;
            if (Parse.stemB)
                fs = new FileStream(path + @"\stempostlist.dat", FileMode.Open, FileAccess.Read, FileShare.None);
            else
                fs = new FileStream(path + @"\postlist.dat", FileMode.Open, FileAccess.Read, FileShare.None);

            postsize =fs.Length / 1024; //size of the postlist
            fs.Close();
            d_size = 0;
            try { 
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, Indexer.idf);
                d_size = s.Length/1024;
            }
            cache = 0;
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, Indexer.cache);
                cache = s.Length/1024;
            }


            terms = Indexer.idf.Count;
            write_info();
            }
            catch(Exception)
            { }
        }

        public static void write_info()
        {
            StreamWriter w = new StreamWriter(p+@"\info.txt");
            w.WriteLine("postlist file size :"+postsize+"[kb]");
            w.WriteLine("dictionary file size :" + d_size + "[kb]");
            w.WriteLine("cache file size :" + cache + "[kb]");
            w.WriteLine("number of terms in the dictionary :" + terms);
            w.WriteLine("number of docs in the corpus:" + docs);
            w.WriteLine(time);
            w.Close();
        }

        public static void setTime(string t)
        {
            time = t;
        }
        public static void csv_write()
        {

            using (var w = new StreamWriter(p+@"\terms-tf.csv"))
            {
                foreach (KeyValuePair<string,Term> kvp in Indexer.idf)
                {
                    var first = kvp.Key;
                    var second = kvp.Value.totaltf;
                    var line = string.Format("{0},{1}", first, second);
                    w.WriteLine(line);
                    w.Flush();
                }
            }


        }

    }
}
