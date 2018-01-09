using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using System.Threading;
using System.Collections;


namespace Engine
{
    public partial class Control : Form
    {
        static Semaphore sema;
        static int n = 0;
        static int m = 0;
        static Files f;
        static Indexer index = new Indexer();
        static string wave = "";
        static string cache_dic_path = "";
        public Control()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        
        private void compute(object sender, EventArgs e)
        {
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            long d1 = 0, d2 = 0, d3 = 0;

            clear_Click(sender, e);
            Parse.setMonth();
            Indexer.path=pathPost.Text; // enter the post lists path

            for (int k = 0; k <10; k++)
            {
                f.getFile(k);
                sema = new Semaphore(10, 1000);

                for (int j = 0; j < f.getDocumentsCount(); j++)
                {
                    d2 = watch.ElapsedMilliseconds;
                    sema.WaitOne();
                    ThreadPool.QueueUserWorkItem(thread1, j);
                    d3 += watch.ElapsedMilliseconds - d2;
                }
                sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne();
                sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne();

                if (k % 4 == 0) // size of files in postlist
                {
                    index.write_to_post(pathPost.Text + @"\" + Indexer.fileid + ".dat");
                }

                //Console.WriteLine(k);
            }
            if (f.getFilesCount() % 4 != 0)
                index.write_to_post(pathPost.Text + @"\" + Indexer.fileid + ".dat");

            d1 = watch.ElapsedMilliseconds;


            int count = Indexer.fileid;
            int wantedThread = 10,filesPerThread=10;
            while (count!=0)
            {
                for (int j = 0; j < count; j=j+ wantedThread* filesPerThread)
                {
                    sema = new Semaphore(wantedThread, 1000);
                    for (int i = j; i < j+ wantedThread * filesPerThread; i=i+ wantedThread)
                    {
                        sema.WaitOne();
                        thread2(i);
                    }
                    sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne();
                    sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne(); sema.WaitOne();
                }

                wave += "w";
                if (count % filesPerThread != 0)
                    count = count / filesPerThread + 1;
                if (count % filesPerThread != 0)
                    count = count / filesPerThread;
                Indexer.fileid= count;
            }
            Indexer.build_maxidf_tf("idf");
            Indexer.merge(0, wave, true);

            
            string time = "total time: " + (((d1 - d3)) / (1000) + d3/ (1000) + (watch.ElapsedMilliseconds - d1) / (1000)) + " sec\n" +
                        "raedfiles time:" + ((d1 - d3) / (1000)) + " sec\n" +
                        "parse time:" + (((d3 * 0.407)) / (1000)) +" sec\n"+
                        "index time:" + (((d3 * 0.592)) / (1000) + (watch.ElapsedMilliseconds - d1) / (1000)) + "sec";
            watch.Stop();
            Info.setTime(time);
            Info.sizes(pathPost.Text);
        }

        public static void thread1(object j)
        {
            Parse p = new Parse();
            KeyValuePair<List<string>, List<bool>> pair = p.parseDoc(f.getDocument((int)j));

            try
            {
                Dictionary<string, int[]> dic = Parse.remove(pair, p.docName, f.filename);
                index.posting(p.docName, dic);
            }
            catch (Exception){}

            sema.Release();
        }

        public static void thread2(object j)
        {
            try
            {
                Indexer.merge((int)j, wave,false);
            }
            catch (Exception)
            {
                m++;
            }

            sema.Release();
        }




        ///////////////////////////////////////////buttons part 1

        private void run_Click(object sender, EventArgs e)  
        {
            if (pathCurpus.Text == "" || pathPost.Text == "")
            {
                MessageBox.Show("fill the required fields");
                return;
            }
            compute(sender, e);

            autoinfo();
            
        }
        private void browseCurpus_Click(object sender, EventArgs e)
        {
            browser.ShowDialog();
            pathCurpus.Text = browser.SelectedPath;
            f = new Files(browser.SelectedPath);
            f.load();
            Parse.setStopWords(browser.SelectedPath);
            browser.SelectedPath = "";
        }
        private void browsePost_Click(object sender, EventArgs e)
        {
            browser.ShowDialog();
            pathPost.Text = browser.SelectedPath;
            browser.SelectedPath = "";
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                Parse.stemB = true;
            else
                Parse.stemB = false;
        }
 
       
        private void saveCache_Click(object sender, EventArgs e)
        {
            browser.SelectedPath = "";
            browser.ShowDialog();

            if (browser.SelectedPath != "")
            {
                cache_dic_path = browser.SelectedPath;
                string s = "";
                if (Parse.stemB)
                    s = "stem";

                Indexer.saveCache(browser.SelectedPath + @"\" + s);
                Indexer.saveDictionary(browser.SelectedPath + @"\" + s);
                Indexer.saveDocDictionary(browser.SelectedPath + @"\" + s);
            }
        }
        private void loadCache_Click(object sender, EventArgs e)
        {
            browser.SelectedPath = "";
            browser.ShowDialog();
            
            
            if (browser.SelectedPath != "")
            {
                cache_dic_path = browser.SelectedPath;
                string s="";
                if (Parse.stemB)
                    s = "stem";

                Indexer.loadCache(browser.SelectedPath+@"\"+s);
                Indexer.loadDictionary(browser.SelectedPath + @"\" + s);
                Indexer.loadDocDictionary(browser.SelectedPath + @"\" + s);
            }
        }    
        private void diplayDictionary_Click(object sender, EventArgs e)
        {
            Form f3 = new Form();
            f3.Text = "Dictionary";
            f3.Size = new Size(300, 720);
            f3.Show();

            DataGrid g = new DataGrid();
            DataTable t = new DataTable();
            t.Columns.Add("Term", typeof(string));
            t.Columns.Add("idf", typeof(int));
            t.Columns.Add("max-tf", typeof(int));

            try
            {
                int i = 0;
                foreach (KeyValuePair<string, Term> item in Indexer.idf)
                { t.Rows.Add(item.Value.term, item.Value.idf, item.Value.totaltf); i++; if (i == 10000) break; }

            }
            catch (Exception) { }
            t.DefaultView.Sort = "max-tf DESC";

            g.DataSource = t;
            g.Size = new Size(300, 700);
            g.ReadOnly = true;

            f3.Controls.Add(g);

        }
        private void diplayCache_Click(object sender, EventArgs e)
        {
            Form f2 = new Form();
            f2.Text = "Cache";
            f2.Size = new Size(1220, 720);
            f2.Show();

            ListBox l = new ListBox();
            f2.Controls.Add(l);
            l.Size = new Size(1200, 700);

            try
            {
                foreach (KeyValuePair<string, string> item in Indexer.cache)
                {
                    if (item.Value.Length > 5000)
                        l.Items.Add(item.Value.Substring(0, 5000));
                    else
                        l.Items.Add(item.Value);
                }
            }
            catch (Exception){}
            
        }

        private void clear_Click(object sender, EventArgs e)
        {
            if (Parse.stemB)
            {
                try { File.Delete(pathPost.Text + @"\" + "stempostlist.dat"); } catch (Exception) { }
                try { File.Delete(cache_dic_path + @"\" + "stemdictionary.dat"); } catch (Exception) { }
                try { File.Delete(cache_dic_path + @"\" + "stemcache.dat"); } catch (Exception) { }
                try { File.Delete(cache_dic_path + @"\" + "stemDocDictionary.dat"); } catch (Exception) { }
            }
            else
            {
                try { File.Delete(pathPost.Text + @"\" + "postlist.dat"); } catch (Exception) { }
                try { File.Delete(cache_dic_path + @"\" + "dictionary.dat"); } catch (Exception) { }
                try { File.Delete(cache_dic_path + @"\" + "cache.dat"); } catch (Exception) { }
                try { File.Delete(cache_dic_path + @"\" + "DocDictionary.dat"); } catch (Exception) { }
            }
            
            try { File.Delete(pathPost.Text + @"\" + "10idf.txt"); } catch (Exception) { }
            try { File.Delete(pathPost.Text + @"\" + "info.txt"); } catch (Exception) { }

            Parse.DocDic.Clear();
            index.postlist.Clear();
            Indexer.cache.Clear();
            Indexer.idf.Clear();

        }

        private void newshow(object sender, EventArgs e)
        {
            Form f2 = new Form();
            f2.Text = "Info";
            f2.Size = new Size(300, 350);
            f2.Show();

            Label l = new Label();
            l.Size = new Size(300, 300);
            f2.Controls.Add(l);

            try
            {
                string[] s = File.ReadAllLines(pathPost.Text + @"\info.txt");
                for (int i = 0; i < s.Length; i++)
                    l.Text += s[i] + "\n";
            }
            catch (Exception){}

            

        }
        private void autoinfo()
        {
            Form f2 = new Form();
            f2.Text = "Info";
            f2.Size = new Size(300, 350);
            f2.Show();

            Label l = new Label();
            l.Size = new Size(300, 300);
            f2.Controls.Add(l);

            try
            {
                string[] s = File.ReadAllLines(pathPost.Text + @"\info.txt");
                for (int i = 0; i < s.Length; i++)
                    l.Text += s[i] + "\n";
                f2.ShowDialog();
            }
            catch (Exception) { }



        }





        ///////////////////////////////////////////buttons part 2
        private void choose_btn_Click(object sender, EventArgs e)
        {
            Ranker.cleanList();

            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            List<string> qry=new List<string>();
            if (openFileDialog1.FileName != "")
            {
                try
                {
                    qry = Files.qureys(openFileDialog1.FileName);
                }
                catch (Exception)
                {
                    MessageBox.Show("wrong file");
                }               
            }
            List<string> qid = new List<string>();
            List<string> q = new List<string>();
            for (int i = 0; i < qry.Count; i=i+2)
            {
                qid.Add(qry[i]);
                q.Add(qry[i+1]);
            }
            Searcher.parseQuerys(qid, q, 50);
        }

        private void reset_btn_Click(object sender, EventArgs e)
        {
            //if index doesnt exixt -> alert
                
            Ranker.cleanList();
            qry_txt.Text = "";
            openFileDialog1.FileName = "";
            Doc_Search_chk.Checked = false;
            expantion_chk.Checked = false;

        }

        private void run_query_btn_Click(object sender, EventArgs e)
        {
            Ranker.cleanList();
            //if index doesnt exixt -> alert

            if (qry_txt.Text=="")
                return;

            if (Doc_Search_chk.Checked)
            {
                if(Parse.DocDic.ContainsKey(qry_txt.Text))
                {
                    string s=Parse.DocDic[qry_txt.Text].Split(',')[0];//name of file
                    string s2 = browseCurpus.Text;//corpus location
                    string docpath = s2 + "/corpus/" + s + "/" + s;
                    //read the file , find the doc, parse 5 sentences from the doc;

                }
                return;
            }


            Parse p = new Parse();
            List<string> ans = new List<string>();
            int back = 50;

            if (expantion_chk.Checked && !qry_txt.Text.Contains(" "))
                ans = Files.viki(qry_txt.Text);//need 50 docs instead of 70
            if (ans.Count != 0)
                back = 70;

            Indexer.path = pathPost.Text;
            string final = qry_txt.Text;
            for (int i = 0; i < ans.Count; i++)
                final += " "+ ans[i] ;
            ans.Clear();

            Random rnd = new Random();
            Searcher.parseQuery("" + rnd.Next(100000, 999999), final,back,true);
           
        }

        private void save_qry_Click(object sender, EventArgs e)
        {
            browser.ShowDialog();
            try
            {
                Ranker.writeList(@"C:\Users\aviv9_000\Desktop\all");
            }
            catch (Exception){}
            Ranker.cleanList();          
        }
    }
}
