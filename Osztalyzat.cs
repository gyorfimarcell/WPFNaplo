using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp8
{
    internal class Osztalyzat
    {
        string nev;
        string targy;
        string datum;
        int jegy;

        public Osztalyzat(string nev, string targy, string datum, int jegy)
        {
            this.nev = nev;
            this.targy = targy;
            this.datum = datum;
            this.jegy = jegy;
        }

        public string Nev { get => nev; }
        public string Targy { get => targy; }
        public string Datum { get => datum; }
        public int Jegy { get => jegy; }
    }
}
