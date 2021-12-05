using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velomax_GUI
{
    public class Fidelio
    {
        int noprogramme;
        double cout;
        string description;
        double rabais;
        DateTime date_debut;
        int duree;

        public Fidelio(int noprogramme, double cout, string description, double rabais, DateTime date_debut, int duree)
        {
            this.noprogramme = noprogramme;
            this.cout = cout;
            this.description = description;
            this.rabais = rabais;
            this.date_debut = date_debut;
            this.duree = duree;
        }


        public Fidelio() { }

        public int Noprogramme
        {
            get { return noprogramme; }
            set { noprogramme = value; }
        }
        public double Cout
        {
            get { return cout; }
            set { cout = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public double Rabais
        {
            get { return rabais; }
            set { rabais = value; }
        }

        public DateTime DateDebut
        {
            get { return date_debut; }
            set { date_debut = value; }
        }

        public int Duree
        {
            get { return duree; }
            set { duree = value; }
        }
    }
}
