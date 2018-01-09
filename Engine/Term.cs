using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    [Serializable]
    class Term { 
    public string term { get; set; }
    public long Positin { get; set; }
    public int idf { get; set; }
    public int totaltf { get; set; }
    public bool cache { get; set; }

        public Term(string t, int f, int total, long p = -1)
        {
            term = t;
            Positin = p;
            idf = f;
            totaltf = total;
            cache = false;
        }
        public Term(string t)
        {
            string[] values = t.Split('#');
            term = values[0];
            int s;
            int.TryParse(values[1],out s);
            idf = s;
            int.TryParse(values[2], out s);
            totaltf = s;
            long x;
            long.TryParse(values[3], out x);
            Positin = x;
            bool y;
            bool.TryParse(values[4], out y);
            cache = y;

        }
        public override string ToString()
        {
            return term + '#' + idf + '#' + totaltf + '#' + Positin+'#'+cache;
        }


    }
}
