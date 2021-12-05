using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public class Commis : Effectif
    {
        DateTime dateE;

        public Commis(string nom, string prenom, string addresse, int num, string etat, DateTime dateE) : base(nom, prenom, addresse, num, etat)
        {
            this.dateE = dateE;
        }
        public Commis(string nom, string prenom, string addresse, int num, string etat) : base(nom, prenom, addresse, num, etat)
        {
            this.dateE = DateTime.Now;
        }

        public DateTime DateE
        {
            get { return dateE; }
        }

        
        public override string ToString()
        {
            return base.ToString() + ";" + dateE.ToShortDateString();
        }
    }
}
