using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public class Livreur : Effectif
    {
        string transport;

        public Livreur(string nom, string prenom, string addresse, int num, string etat,string t) : base(nom,  prenom,  addresse, num,  etat)
        {
            this.transport = t;
        }

        public string Transport
        {
            get { return transport; }
        }

        public override string ToString()
        {
            return base.ToString() + ";" + transport;
        }
    }
}
