using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public abstract class Effectif: Personne
    {
        protected string etat;

        public Effectif(string nom, string prenom, string addresse, int num, string etat) : base( nom,  prenom,  addresse, num)
        {
            this.etat = etat;
        }

        public string Etat
        {
            get { return etat; }
            set { etat = value; }
        }

        public override string ToString()
        {
            return base.ToString() + ";" + etat;
        }
    }
}
