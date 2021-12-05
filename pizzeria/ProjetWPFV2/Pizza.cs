using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public class Pizza : Produit
    {
        

        public Pizza(string type, string taille, int quantite, double prix) : base( type,taille, quantite, prix)
        {
            this.nomItem = "Pizza";
        }

        public Pizza(double prix,string type) : base(prix)
        {
            this.nomItem = "Pizza";
            this.type = type;
            quantite = 1;
            taille = "";
        }
        

        /// <summary>
        /// Cout total du produit 
        /// </summary>
        /// <returns></returns>
        public override double Prix()
        {
            double lgth = taille == "Petite" ? prixBase : taille == "Moyenne" ? 1.5 * prixBase : taille == "Grande" ? 2 * prixBase : 0;
            return lgth * quantite;
        }

        

        /// <summary>
        /// Afficher dans Produit.csv le produit 
        /// </summary>
        /// <returns></returns>
        public override string AfficheFichier()
        {
            return "Pizza;" + base.AfficheFichier();
        }

        /// <summary>
        /// Affiche le produit dans le fichier DetailCommandes.csv avec tout les attribut caratérisant le produit
        /// </summary>
        /// <returns></returns>
        public override string AfficheDetail()
        {
            return $"{nomItem};{prixBase};{type};{taille};;{quantite}";
        }
    }
}
