using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public abstract class Produit : IPrix
    {
        protected int quantite;
        protected string nomItem;
        protected double prixBase;
        protected string taille, type;
        public Produit(string type, string taille, int quantite, double prix)
        {
            this.taille = taille;
            this.type = type;
            this.quantite = quantite;
            this.prixBase = prix;
        }

        public Produit(double prix)
        {
            prixBase = prix;
        }

        #region
        public int Quantite
        {
            get { return quantite; }
            set { quantite = value; }
        }

        public string Label
        {
            get { return nomItem; }
        }

        public virtual string Taille
        {
            get { return taille; }
            set { taille = value; }
        }
        public string Type
        {
            get { return type; }
        }

        // 3 accesseur de prix pour faciliter l'affichage WPF 
        public double PrixBase
        {
            get { return prixBase; }
        }
        public double PrixMoyen
        {
            get { return prixBase*1.5; }
        }
        public double PrixGrand
        {
            get { return PrixBase*2; }
        }

        public double GetPrix
        {
            get { return Prix(); }
        }
        #endregion

        public abstract double Prix();
        public virtual string AfficheFichier()
        {
            return type + ";" + prixBase;
        }

        public abstract string AfficheDetail();
        public override string ToString()
        {
            return  type + ";" + taille + ";" + quantite + ";" + Prix();
        }

    }
}
