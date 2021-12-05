using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetWPFV2
{
    public abstract class Personne
    {
        protected string nom, prenom, addresse;
        protected int num;

        public Personne(string nom, string prenom, string addresse,int num)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.addresse = addresse;
            this.num = num;
        }

        public Personne() { }

        public override string ToString()
        {
            return nom + ";" + prenom + ";" + addresse + ";" + num;
        }


        public string Nom
        {
            get { return nom; }
        }
        public string Prenom
        {
            get { return prenom; }
        }
        public string Adresse
        {
            get { return addresse; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        // Methode delegate pour le tri
        public delegate int triPersonne(Personne p1, Personne p2);

        public static triPersonne triNom = delegate (Personne x, Personne y) { return x.Nom.CompareTo(y.Nom); };
        public static triPersonne triVille = delegate (Personne x, Personne y) { return x.Adresse.CompareTo(y.Adresse); };
        public static triPersonne triCumul = delegate (Personne x, Personne y) { if (x is Client && y is Client) return ((Client)x).Cumul.CompareTo(((Client)y).Cumul); else return 0; };

        /// <summary>
        /// Methode triant une liste de personne en fontion d'une methode de tri delegate choisi
        /// </summary>
        /// <param name="tri">methode souhaiter (triNom,triVille,triCumul)</param>
        /// <param name="lst">Liste a trier</param>
        /// <returns>liste trier</returns>
        public static List<Personne> Tri(triPersonne tri, List<Personne> lst)
        {

            lst.Sort((x, y) => tri(x, y));      // On trie cette liste grace au delegate attribuer en parametre 

            return lst;
        }


    }
}
