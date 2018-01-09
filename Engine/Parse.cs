using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Collections;

namespace Engine
{
    class Parse
    {
        private static HashSet<string> stop = new HashSet<string>();       
        private static bool isSet = true;
        private static Hashtable month = new Hashtable();
        public static void setMonth()
        {
            if (isSet)
            {
                isSet = false;

                month.Add("january", "01"); month.Add("jan", "01");
                month.Add("february", "02"); month.Add("feb", "02");
                month.Add("march", "03"); month.Add("mar", "03");
                month.Add("april", "04"); month.Add("apr", "04");
                month.Add("may", "05");
                month.Add("june", "06"); month.Add("jun", "06");
                month.Add("july", "07"); month.Add("jul", "07");
                month.Add("august", "08"); month.Add("aug", "08");
                month.Add("september", "09"); month.Add("sep", "09");
                month.Add("october", "10"); month.Add("oct", "10");
                month.Add("november", "11"); month.Add("nov", "11");
                month.Add("december", "12"); month.Add("dec", "12");
          
            }
        }
        public static void setStopWords(string path)
        {
            string[] sw = Files.getStopWords(path);
            stop.Clear();
            for (int i = 0; i < sw.Length; i++)
                stop.Add(sw[i]);
        }


        private string w;
        private  string lw ;
        private bool[] b;
        private bool[] lb;
        private  bool relevant;
        public string docName;
        public Parse()
        {
            w = "";
            lw = "";
            b = new bool[7];
            lb = new bool[7];
            rest(b);//str num month cap dot div perc
            rest(lb);
            relevant = true;
            docName = "";
        }
        public KeyValuePair<List<string>, List<bool>> parseDoc(string[] doc)
        {
            List<string> Lterms = new List<string>();
            List<bool> Lrelevant = new List<bool>();

            relevant = true;
            double relevant_border = doc.Length * 0.3;
            bool on = false;
            setMonth();

            for (int i = 0; i < doc.Length; i++)
            {
                if (relevant && i > relevant_border)
                    relevant = false;

                if (doc[i] == @"</TEXT>")
                    on = false;

                if (on)
                {
                    parse(Lterms, Lrelevant, doc[i]);
                }
                else if (doc[i].Length>0&&doc[i][0] == '<' && doc[i][1] == 'D' && doc[i][2] == 'O' && doc[i][3] == 'C' && doc[i][4] == 'N')
                    docName= doc[i].Substring(7, doc[i].Length - 7 - 8).Replace(" ", "");                 

                if (doc[i] == "<TEXT>")
                    on = true;
            }

            KeyValuePair<List<string>, List<bool>> pair = new KeyValuePair<List<string>, List<bool>>(Lterms,Lrelevant);

            return pair;
        }
        private void rest(bool[] b)
        {
            b[0] = false; b[1] = false; b[2] = false; b[3] = false; b[4] = false; b[5] = false; b[6] = false;
        }
        private void cutWord(List<string> Lterms, List<bool> Lrelevant)
        {
            if (month.Contains(w))
            {
                b[2] = true;
                w = month[w] + "";
            }


            if (lw.Length != 0)
            {
                if (lb[2] && b[1] && !b[4] && !b[5] && !b[6])
                {
                    if (w.Length == 2 && (w[0] == '0' || w[0] == '1' || w[0] == '2' || w[0] == '3'))
                        lw=w + "/" + lw;
                    else
                        lw=lw + "/" + w;
                    w = "";
                }
                else if (b[2] && lb[1] && !lb[4])
                {
                    if (lw.Length == 2 && (lw[0] == '0' || lw[0] == '1' || lw[0] == '2' || lw[0] == '3'))
                        lw = lw + "/" + w;
                    else
                        lw = w + "/" + lw;
                    lb[0] = b[0]; lb[1] = b[1]; lb[2] = b[2]; lb[3] = b[3]; lb[4] = b[4]; 
                    w = "";
                }
                else if (lb[1] && w.Length > 6 && w[0] == 'p' && w[1] == 'e' && w[2] == 'r' && w[3] == 'c' && w[4] == 'e' && w[5] == 'n' && w[6] == 't')
                {
                    add(Lterms,Lrelevant, lw+ " percent");
                    lw = "";
                    lb[1] = false; lb[4] = false;//rest(lb);
                    w = "";
                }
                else if (lb[3] && b[3])
                {
                    add(Lterms, Lrelevant, lw);
                    add(Lterms, Lrelevant, w);
                    add(Lterms, Lrelevant, lw +" "+w);
                    lw = "";
                    lb[0] = false; lb[2] = false; lb[3] = false;//rest(lb);
                    w = "";
                }
                else
                {
                    add(Lterms, Lrelevant, lw);
                    lw = "";
                    lb[0] = false; lb[1] = false; lb[2] = false; lb[3] = false; lb[4] = false;
                }

            }







            if (w.Length == 0)
                ;
            else if (b[0])
            {
                if (b[2] || b[3])
                {
                    lw = w;
                    lb[0] = b[0]; lb[1] = b[1]; lb[2] = b[2]; lb[3] = b[3]; lb[4] = b[4];
                }
                else
                    add(Lterms, Lrelevant, w);

            }
            else if (b[1])
            {
                if (b[4])
                {
                    try
                    {
                        double d = Convert.ToDouble(w);
                        if ((int)(d * 1000) % 10 == 0)
                            lw = "" + ((double)((int)(d * 100))) / 100;
                        else
                            lw = "" + ((double)((int)(d * 100 + 1))) / 100;
                        lb[0] = b[0]; lb[1] = b[1]; lb[2] = b[2]; lb[3] = b[3]; lb[4] = b[4]; 
                    }
                    catch (Exception) { add(Lterms, Lrelevant, w); }
                }
                else if (b[5])
                {
                    try
                    {
                        double d = Convert.ToDouble(w.Split('/')[0]) / Convert.ToDouble(w.Split('/')[1]);
                        if ((int)(d * 1000) % 10 == 0)
                            add(Lterms, Lrelevant, "" + ((double)((int)(d * 100))) / 100);
                        else
                            add(Lterms, Lrelevant, "" + ((double)((int)(d * 100 + 1))) / 100);
                    }
                    catch (Exception) { add(Lterms, Lrelevant, w); }
                }
                else if (b[6])
                {
                    add(Lterms, Lrelevant, w + " percent");
                }
                else
                {
                    lw =w;
                    lb[0] = b[0]; lb[1] = b[1]; lb[2] = b[2]; lb[3] = b[3]; lb[4] = b[4];
                }
            }

            rest(b);
            w = "";
        }
        public  void parse(List<string> Lterms, List<bool> Lrelevant, String s)
        {
       

            for (int i = 0; i < s.Length; i++)
            {
                
                if (s[i] == ' ')
                {
                    cutWord(Lterms,Lrelevant);
                }
                else if (char.IsLetter(s[i]))
                {
                    if (b[1] && (s[i] == 't' || s[i] == 'h'))
                        ;
                    else
                    {
                        b[0] = true;
                        if (char.IsLower(s[i]))
                            w += s[i];
                        else
                        {
                            b[3] = true;
                            w += char.ToLower(s[i]);////////////////
                        }
                    }

                }
                else if (char.IsDigit(s[i]))
                {
                    if (b[0])
                        w += s[i];
                    else
                    {
                        if (b[6])
                        {
                            b[0] = true;
                            b[1] = false;
                            b[6] = false;
                        }
                        else
                        {
                            b[1] = true;
                            w += s[i];
                        }
                    }
                }
                else if(b[1]&&!b[0])
                {
                    if (s[i] == '.' && !b[4])
                    {
                        b[4] = true;
                        w += s[i];
                    }
                    else if (s[i] == '/' && !b[5])
                    {
                        b[5] = true;
                        w += s[i];
                    }
                    else if (s[i] == '%' && !b[6])
                        b[6] = true;
                   
                }
                else if(s[i] == '/'|| s[i] == '-')
                {
                        cutWord(Lterms, Lrelevant);
                }

            }//end while

            cutWord(Lterms,Lrelevant);
            if (lw.Length!=0)
            {
                add(Lterms,Lrelevant,lw);
                lw = "";
                lb[0] = false; lb[1] = false; lb[2] = false; lb[3] = false; lb[4] = false;
            }

            
        }
        private void add(List<string> Lterms, List<bool> Lrelevant, string word)
        {
            if (stop.Contains(word))
                return;
            Lterms.Add(word);
            Lrelevant.Add(relevant);
        }





        public static Dictionary<string, string> DocDic = new Dictionary<string, string>();
        public static bool stemB = false;
  
        public static Dictionary<string, int[]> remove(KeyValuePair<List<string>, List<bool>> pair,string docname,string filename)
        {
            int maxTf = 0;

            int[] temp;
            Dictionary<string, int[]> dic = new Dictionary<string, int[]>();
            if (stemB)
            {
                Stemmer stem = new Stemmer();
                for (int i = pair.Key.Count-1; i >= 0; i--)
                {                  
                    pair.Key[i] = stem.stemTerm(pair.Key[i]);

                    if (dic.ContainsKey(pair.Key[i]))
                    {
                        if (pair.Value[i])
                            dic[pair.Key[i]][0]++;
                        else
                            dic[pair.Key[i]][1]++;

                        //docfile
                        temp = dic[pair.Key[i]];
                        if (temp[0]+temp[1]> maxTf)
                            maxTf = temp[0] + temp[1];

                        
                    }
                    else
                    {
                        dic[pair.Key[i]] = new int[2];
                        if (pair.Value[i])
                            dic[pair.Key[i]][0]++;
                        else
                            dic[pair.Key[i]][1]++;

                        //docfile
                        if (1 > maxTf)
                            maxTf = 1;
                    }
                }
            }
            else
            {
                for (int i = pair.Key.Count - 1; i >= 0; i--)
                {
                    if (dic.ContainsKey(pair.Key[i]))
                    {
                        if (pair.Value[i])
                            dic[pair.Key[i]][0]++;
                        else
                            dic[pair.Key[i]][1]++;
                        
                        //docfile
                        temp = dic[pair.Key[i]];
                        if (temp[0] + temp[1] > maxTf)
                            maxTf = temp[0] + temp[1];

                    }
                    else
                    {
                        dic[pair.Key[i]] = new int[2];
                        if (pair.Value[i])
                            dic[pair.Key[i]][0]++;
                        else
                            dic[pair.Key[i]][1]++;

                        //docfile
                        if (1 > maxTf)
                            maxTf = 1;
                    }
                }
            }


            
            DocDic.Add(docname, filename + "," + maxTf + "," + pair.Key.Count);

            return dic;

        }





    }
}

