using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public class Client : Personne
    {
        DateTime dateP;
        double cumul;

        public Client(string nom, string prenom, string adresse, int num, DateTime dateP, double cumul) : base(nom, prenom, adresse, num)
        {
            this.dateP = dateP;
            this.cumul = cumul;
        }

        public Client(string nom, string prenom, string adresse, int num) : base(nom, prenom, adresse, num)
        {
            dateP = DateTime.Now;
            cumul = 0;
        }


        public DateTime DateP
        {
            get { return dateP; }
        }
        
        public double Cumul
        {
            get { return cumul; }
        }

        /// <summary>
        /// Permet d'ajouter au cumul le montant d'une commande 
        /// </summary>
        /// <param name="montant">Montant à ajouter au cumul d'un client</param>
        public void CumulCalcul(double montant)
        {
            cumul += montant;
        }

        public override string ToString()
        {
            return base.ToString() + ";" + dateP.ToShortDateString() + ";" + cumul;
        }

    }
}
