using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

using System.Threading;

namespace Engine
{
    class Indexer
    {
        public static Dictionary<string, Term> idf = new Dictionary<string, Term>();// count idf fot each terms
        Mutex m1, m2;

        public static string path;

        public static Dictionary<string, string> cache = new Dictionary<string, string>();

        Dictionary<string, int> map;// maping terms in doc
        public Dictionary<string, string> postlist { get; set; }

        public string docname { get; set; }
        public static int fileid = 0;
        static int z = 0;



        public Indexer()
        {
            m1 = new Mutex();
            m2 = new Mutex();

            Indexer.path = "";

            postlist = new Dictionary<string, string>();
            map = new Dictionary<string, int>();

        }
        // map the tf off terms in a file , and update the idf terms in the DB
        //make the postlists
        public void posting(string doc_name, Dictionary<string, int[]> docs)
        {
            foreach (KeyValuePair<string, int[]> kvp in docs)
            {
                m1.WaitOne();
                if (!postlist.ContainsKey(kvp.Key))
                    postlist[kvp.Key] = kvp.Key + "=>";
                m1.ReleaseMutex();
                postlist[kvp.Key] += doc_name + ":" + kvp.Value[0] + "," + kvp.Value[1] + "#";

                m2.WaitOne();
                if (!idf.ContainsKey(kvp.Key))
                    idf.Add(kvp.Key, new Term(kvp.Key, 0, 0));
                m2.ReleaseMutex();
                idf[kvp.Key].idf++;
                idf[kvp.Key].totaltf = idf[kvp.Key].totaltf + kvp.Value[0] + kvp.Value[1];

            }
        }

        // appand write to the file 
        public void write_to_post(string path)
        {
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>();
            myList = sortpostlist(myList);
            using (var fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None))
            using (var bw = new BinaryWriter(fileStream))
            {
                foreach (KeyValuePair<string, string> kvp in myList)
                {
                    if (kvp.Value != null)
                        bw.Write(kvp.Value);
                }
            }
            fileid++;

            // re-set the postlist dic for the next file
            postlist.Clear();

        }
        //sortpostlist
        public List<KeyValuePair<string, string>> sortpostlist(List<KeyValuePair<string, string>> myList)
        {
            myList = postlist.ToList();

            myList.Sort(
                delegate (KeyValuePair<string, string> pair1,
                KeyValuePair<string, string> pair2)
                {
                    try
                    {
                        return pair1.Key.CompareTo(pair2.Key);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }

                }
            );
            return myList;
        }
        //sort dicttionary by idf value
        public static List<KeyValuePair<string, Term>> sortidf(List<KeyValuePair<string, Term>> myList)
        {
            myList = idf.ToList();

            myList.Sort(
                delegate (KeyValuePair<string, Term> pair1,
                KeyValuePair<string, Term> pair2)
                {
                    try
                    {
                        return pair1.Value.idf.CompareTo(pair2.Value.idf);
                    }
                    catch (Exception)
                    {
                        return 1;
                    }

                }
            );
            return myList;
        }
        //sort dicttionary by tf value
        public static List<KeyValuePair<string, Term>> sorttf(List<KeyValuePair<string, Term>> myList)
        {
            myList = idf.ToList();

            myList.Sort(
                delegate (KeyValuePair<string, Term> pair1,
                KeyValuePair<string, Term> pair2)
                {
                    try
                    {

                        return pair1.Value.totaltf.CompareTo(pair2.Value.totaltf);

                    }
                    catch (Exception)
                    {
                        return 1;
                    }

                }
            );
            return myList;
        }


        //save the dictionary to the disk
        public static bool saveDictionary(string newpath)
        {

            using (FileStream fs = File.OpenWrite(newpath + "dictionary.dat"))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Put count.
                writer.Write(idf.Count);
                // Write pairs.
                foreach (var pair in idf)
                {
                    writer.Write(pair.Value.ToString());
                }
                return true;
            }

        }

        //save the doc dictionary
        public static bool saveDocDictionary(string newpath)
        {
            try
            {
                using (FileStream fs = File.OpenWrite(newpath + "DocDictionary.dat"))
                using (BinaryWriter writer = new BinaryWriter(fs))
                {
                    // Put count.
                    writer.Write(Parse.DocDic.Count);
                    // Write pairs.
                    foreach (var pair in Parse.DocDic)
                    {
                        if (pair.Value == null)
                        {
                            continue;
                        }
                        writer.Write(pair.Key);
                        writer.Write(pair.Value);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        //save the cache to the disk
        public static bool saveCache(string newpath)
        {

            using (FileStream fs = File.OpenWrite(newpath + "cache.dat"))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                // Put count.
                writer.Write(cache.Count);
                // Write pairs.
                foreach (var pair in cache)
                {
                    writer.Write(pair.Key);
                    writer.Write(pair.Value);
                }
                return true;
            }
        }
        public static bool loadDictionary(string newpath)
        {

            idf = new Dictionary<string, Term>();
            using (FileStream fs = File.OpenRead(newpath + "dictionary.dat"))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                int count = reader.ReadInt32();
                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    string value = reader.ReadString();
                    Term t = new Term(value);
                    idf[t.term] = t;
                }
                return true;
            }
        }
        public static bool loadDocDictionary(string newpath)
        {
            try
            {
                Parse.DocDic = new Dictionary<string, string>();
                using (FileStream fs = File.OpenRead(newpath + "DocDictionary.dat"))
                using (BinaryReader reader = new BinaryReader(fs))
                {

                    // Get count.
                    int count = reader.ReadInt32();

                    // Read in all pairs.
                    for (int i = 0; i < count && reader.BaseStream.Position < reader.BaseStream.Length; i++)
                    {
                        if (i == reader.BaseStream.Length)
                            Console.WriteLine();
                        string key = reader.ReadString();
                        string value = reader.ReadString();
                        Parse.DocDic[key] = value;

                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static bool loadCache(string newpath)
        {

            cache = new Dictionary<string, string>();
            using (FileStream fs = File.OpenRead(newpath + "cache.dat"))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                // Get count.
                int count = reader.ReadInt32();
                // Read in all pairs.
                for (int i = 0; i < count; i++)
                {
                    string key = reader.ReadString();
                    string value = reader.ReadString();
                    cache[key] = value;
                }
                return true;
            }

        }



        public static void calculate_w(string postlist)
        {

            //split term and post doc
            string[] post = postlist.Split('=');

            if (!Indexer.idf.ContainsKey(post[0]))
                return;
            //split by docs
            string[] docs = post[1].Split('#');
            docs[0] = docs[0].Substring(docs[0].IndexOf(">") + 1);
            //split by args
            char[] del = { ':', ',' };
            double weight = 0;
            string term = post[0];
            double idf = Ranker.calculate_idf(term);
            if (idf == -1)
                return;
            foreach (string i in docs)
            {
                if (i.Equals(""))
                { //end of postlist
                    return;
                }
                string[] doci = i.Split(del);
                if (!Parse.DocDic.ContainsKey(doci[0]))
                    continue;
                double tf = (int.Parse(doci[1]) + int.Parse(doci[2]));
                tf = Ranker.calculate_itf(post[0], doci[0], tf);
                if (tf == -1)
                    return;
                weight = Math.Pow(idf * tf, 2);

                //add the Wi to the relevant doc
                string[] doctemp = Parse.DocDic[doci[0]].Split(',');
                //if already exist add the weight 
                if (doctemp.Length == 4)
                {
                    weight = weight + double.Parse(doctemp[3]);
                    string ans = doctemp[0] + "," + doctemp[1] + "," + doctemp[2] + "," + weight;
                    Parse.DocDic[doci[0]] = ans;
                }
                //first W-term of the doc 
                else
                {
                    string ans = Parse.DocDic[doci[0]] + "," + weight;
                    Parse.DocDic[doci[0]] = ans;
                }

            }


        }

        // sort post list by tf
        public static List<KeyValuePair<string, double>> post_list(string term)
        {

            List<KeyValuePair<string, double>> sortlist = new List<KeyValuePair<string, double>>();
            string[] post = term.Split('=');
            //split by docs
            string[] docs = post[1].Split('#');
            //clean > char
            docs[0] = docs[0].Substring(docs[0].IndexOf(">") + 1);
            char[] del = { ':', ',' };
            double tf;
            for (int i = 0; i < docs.Length; i++)
            {
                if (docs[i].Equals(""))
                    break;
                string[] doci = docs[i].Split(del);
                tf = (double.Parse(doci[1]) + double.Parse(doci[2]));
                //use our formula of weight
                if (Ranker.mweight == true)
                {
                    double x = double.Parse(doci[1]);
                    double y = double.Parse(doci[2]);
                    if (x == 0)
                        x = 0.5;
                    if (y == 0)
                        y = 1;
                    tf = tf * (x / y);
                }

                sortlist.Add(new KeyValuePair<string, double>(doci[0], tf));
            }
            sortlist.Sort(delegate (KeyValuePair<string, double> pair1,
                KeyValuePair<string, double> pair2)
            {
                try
                {

                    return pair1.Value.CompareTo(pair2.Value);

                }
                catch (Exception)
                {
                    return 1;
                }

            });
            return sortlist;
        }

        //calculte 10000 most df value for the cache and 10 to df and 10 top tf for the report  
        public static void build_maxidf_tf(string name)
        {

            List<KeyValuePair<string, Term>> myList = new List<KeyValuePair<string, Term>>();
            if (name.Equals("idf"))
                myList = sortidf(myList);
            if (name.Equals("tf"))
                myList = sorttf(myList);
            StreamWriter w = new StreamWriter(path + @"\10" + name + ".txt");
            if (name.Equals("idf"))
            {
                for (int i = 0; i < 10000; i++)
                {

                    cache.Add(myList.ElementAt(myList.Count - i - 1).Key, "");
                }
            }
            for (int i = 0; i < 20; i++)
            {
                if (i < 10)
                {
                    if (name.Equals("idf"))
                        w.WriteLine(myList.ElementAt(myList.Count - i - 1).Key + "," + myList.ElementAt(myList.Count - i - 1).Value.idf);
                    else
                        w.WriteLine(myList.ElementAt(myList.Count - i - 1).Key + "," + myList.ElementAt(myList.Count - i - 1).Value.totaltf);
                }
                else
                {
                    if (name.Equals("idf"))
                        w.WriteLine(myList.ElementAt(20 - i + 1).Key + "," + myList.ElementAt(20 - i + 1).Value.idf);
                    else
                        w.WriteLine(myList.ElementAt(20 - i + 1).Key + "," + myList.ElementAt(20 - i + 1).Value.totaltf);
                }
            }



            w.Close();
        }

        //help func to read files

        public static FileStream fileopen(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
            return fs;
        }
        //help func to read files
        public static string read_form_post(FileStream fs)
        {
            string s = "";

            if (fs.Position != fs.Length)
            {
                BinaryReader br = new BinaryReader(fs);
                s = br.ReadString();
            }
            return s;

        }
        //help func to read files
        public static void fileclose(FileStream fs)
        {
            fs.Close();
        }
        //help func to read files
        public static string get_term_frompost(string term, string path)
        {
            path = path + "\\";
            if (Parse.stemB == true)
                path = path + "stem";
            FileStream fs = fileopen(path + "postlist.dat");
            BinaryReader br = new BinaryReader(fs);
            if (Indexer.idf.ContainsKey(term))
            {
                if (idf[term].cache == false)
                {
                    //Console.WriteLine(idf[term].Positin);
                    fs.Seek(idf[term].Positin, SeekOrigin.Begin);
                    string ans = br.ReadString();
                    fs.Close();
                    return ans;
                }
                else if (cache.ContainsKey(term))
                {
                    fs.Close();
                    return cache[term];
                }
            }
            fs.Close();
            return null;
        }
        //merge all the postlist
        public static void merge(int start, string pre, bool lastround)
        {
            int size = 10;

            FileStream[] fpointer = new FileStream[size];

            PriorityQueue<KeyValuePair<string, int>> q = new PriorityQueue<KeyValuePair<string, int>>(size * 4, Comparer<KeyValuePair<string, int>>.Create((x, y) => x.Key.CompareTo(y.Key)));
            string s;

            //set filestreams to all posts
            for (int i = start; i < start + size; i++)
            {
                try
                {
                    fpointer[i % size] = fileopen(path + @"\" + pre + i + ".dat");
                    for (int j = 0; j < 3; j++)//read first 3 lines from each postlist file
                    {
                        s = read_form_post(fpointer[i % size]);
                        if (!s.Equals(""))
                            q.Enqueue(new KeyValuePair<string, int>(s, i % size));
                    }
                }
                catch (Exception)
                {
                }
            }

            FileStream mp;
            if (lastround)
            {
                if (Parse.stemB)
                    mp = new FileStream(path + @"\stempostlist.dat", FileMode.Append, FileAccess.Write, FileShare.None);
                else
                    mp = new FileStream(path + @"\postlist.dat", FileMode.Append, FileAccess.Write, FileShare.None);
            }
            else
                mp = new FileStream(path + @"\" + pre + "w" + (start / 10) + ".dat", FileMode.Append, FileAccess.Write, FileShare.None);
            BinaryWriter bw = new BinaryWriter(mp);

            //help vars
            try
            {
                string g;
                string z;


                KeyValuePair<string, int> t1 = q.Dequeue();
                s = t1.Key.Substring(0, t1.Key.IndexOf('=')); // first term
                z = read_form_post(fpointer[t1.Value]);
                if (!z.Equals(""))
                    q.Enqueue(new KeyValuePair<string, int>(z, t1.Value));
                string build = t1.Key;
                while (q.Count > 0)
                {
                    KeyValuePair<string, int> t2 = q.Dequeue();
                    z = read_form_post(fpointer[t2.Value]);
                    if (!z.Equals("")) { q.Enqueue(new KeyValuePair<string, int>(z, t2.Value)); }

                    g = t2.Key.Substring(0, t2.Key.IndexOf('=')); //term
                    if (s.Equals(g))
                    {
                        build = build + t2.Key.Substring(t2.Key.IndexOf('=') + 2);
                    }
                    else
                    {

                        //next line in t1 until we finish to read all term line
                        while (true)
                        {
                            z = read_form_post(fpointer[t1.Value]);
                            if (!z.Equals("")) { if (z.Substring(0, z.IndexOf('=')).Equals(s)) { build = build + z.Substring(z.IndexOf('=') + 2); } else { q.Enqueue(new KeyValuePair<string, int>(z, t1.Value)); break; } }
                            else { break; }
                        }

                        //here we generate cache and count tf 
                        if (lastround)
                        {

                            //add the binary position to the Dictionary
                            string temp = t1.Key.Substring(0, t1.Key.IndexOf('='));
                            idf[temp].Positin = mp.Position;

                            //calculte weight of the docs
                            calculate_w(build);
                            //cache
                            if (cache.ContainsKey(temp))
                            {
                                cache[temp] = t1.Key;
                                idf[temp].cache = true;
                            }

                        }
                        //flash the term post list to the merge file 
                        bw.Write(build);

                        t1 = t2;
                        s = g;
                        build = t1.Key;

                    }
                }
            }
            catch (Exception e)
            {

            }

            try
            {
                mp.Close();
                foreach (FileStream i in fpointer)
                {
                    i.Close();
                }

                for (int i = start; i < start + size; i++)
                    File.Delete(path + @"\" + pre + i + ".dat");
                try
                {
                    //for (int i = start; i < start + size; i++)
                    if (lastround)
                        for (int i = 0; i < 500; i++)
                            File.Delete(path + @"\" + i + ".dat");
                }
                catch (Exception) { }

            }
            catch (Exception)
            {
            }


        }




    }
}
