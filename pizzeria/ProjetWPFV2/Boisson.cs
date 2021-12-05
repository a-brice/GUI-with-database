using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public class Boisson : Produit
    {

        public Boisson(string type, string taille, int quantite, double prix) : base(type, taille, quantite, prix)
        {
            this.nomItem = "Produit";
        }

        public Boisson(string type, double prix) : base(prix)
        {
            this.type = type;
            quantite = 1;
            taille = "";
            nomItem = "Produit";
        }

        public override string Taille
        {
            get { return taille + "cl"; }
        }

        /// <summary>
        /// Prix final d'un produit
        /// </summary>
        /// <returns>retourne son prix en fct de son type</returns>
        public override double Prix()
        {
            double lgth = taille == "33" ? prixBase : taille == "50" ? 1.5 * prixBase : taille == "100" ? 2 * prixBase : 0;
            return lgth *quantite;
        }

        /// <summary>
        /// Utiliser pour afficher les attribut de chaque produit dans un fichier 
        /// Les produit seront écrit dans Produit.csv
        /// </summary>
        /// <returns>chaine de caractère permettant de construire une ligne du fichier</returns>
        public override string AfficheFichier()
        {
            return "Produit;" + base.AfficheFichier();
        }

        /// <summary>
        /// Utiliser pour afficher les attribut de chaque produit dans un fichier 
        /// Les produit seront écrit dans DetailsCommandes.csv
        /// </summary>
        /// <returns>chaine de caractère permettant de construire une ligne du fichier</returns>
        public override string AfficheDetail()
        {
            return $"{type};{prixBase};;;{taille};{quantite}";
        }
    }
}
