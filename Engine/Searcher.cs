using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace Engine
{
    class Searcher
    {
        public static void parseQuery(string qryid,string qry,int num,bool show)
        {
            Parse p = new Parse();
            List<string> words = new List<string>();
            p.parse(words, new List<bool>(), qry);
            //groupby
            HashSet<string> hash = new HashSet<string>();

            for (int i = 0; i < words.Count; i++)//optional groupby
                hash.Add(words[i]);


            List<string> docs= Ranker.getdocs(num, hash.ToArray());
            for (int i = 0; i < docs.Count; i++)
                Console.WriteLine(docs[i]);

            
            Ranker.addTolist(qryid, docs);
            if (show)
                Searcher.show(docs);         
        }
        public static void parseQuerys(List<string> qryID,List<string> qry,int num)
        {
            if (qryID.Count != qry.Count)
                return;
            for (int i = 0; i < qryID.Count; i++)           
                parseQuery(qryID[i], qry[i], num, false);
        }

        private static void show(List<string> docs)
        {
            Form f1 = new Form();
            f1.Show();
            f1.Size = new System.Drawing.Size(600, 500);
            f1.Name = "result";

            ListBox l = new ListBox();
            f1.Controls.Add(l);
            l.Location = new System.Drawing.Point(20, 20);
            l.Size = new System.Drawing.Size(500, 300);

            for (int i = 0; i < docs.Count; i++)
            {
                l.Items.Add( docs[i] );
            }
        }

    }
}
