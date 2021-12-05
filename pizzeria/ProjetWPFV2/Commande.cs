using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjetWPFV2
{
    public class Commande : IPrix
    {
        int numClient,id;
        DateTime date;
        string  nomCommis, 
            nomLivreur,
            etat;
        List<Produit> lstprod;
        string encaisse;

        static int cpt = 0;

        public Commande(int numClient, string nomCommis,  List<Produit> lstprod)
        {
            cpt += 10;
            this.id = cpt;
            this.date = DateTime.Now;
            this.numClient = numClient;
            this.nomCommis = nomCommis;
            this.etat = "en préparation";
            if (lstprod != null)
                this.lstprod = lstprod;
            else this.lstprod = new List<Produit>();
            nomLivreur = "";
            encaisse = "";
        }

        public Commande(int id, DateTime date, int numClient, string nomCommis, string nomlivreur, string etat, List<Produit> lstprod, string encaisse)
        {
            this.id = id;
            cpt = id;
            this.date = date;
            this.numClient = numClient;
            this.nomCommis = nomCommis;
            this.nomLivreur = nomlivreur;
            this.etat = etat;
            if (lstprod != null) this.lstprod = lstprod;
            else this.lstprod = new List<Produit>();
            this.encaisse = encaisse;

            
        }

        #region Accesseur
        public int Id
        {
            get { return id; }
        }

        public string Etat
        {
            get { return etat; }
            set { etat = value; }
        }

        public List<Produit> LstProduit
        {
            get { return lstprod; }
            set { lstprod = value; }
        }

        public string NumClient
        {
            get { return "0" + numClient; }
        }
        public string NomCommis
        {
            get { return nomCommis; }
        }

        public string NomLivreur
        {
            get { return nomLivreur; }
            set { nomLivreur = value; }
        }

        public DateTime DateHeure
        {
            get { return date; }
        }
        public string Date
        {
            get { return date.ToShortDateString(); }
        }

        public string Heure
        {
            get { return date.ToShortTimeString(); }
        }
       
        public string Encaisser
        {
            get { return encaisse; }
            set { encaisse = value; }
        }

        public string PrixActuel
        {
            get { if (Prix() == 0) return "Aucun produit"; else return Prix() + "€"; }
        }

        #endregion

        /// <summary>
        /// Additionne le prix final de tout les produit et le stocke pour ensuite le retourner
        /// </summary>
        /// <returns></returns>
        public double Prix()
        {
            double total = 0;
            if(lstprod != null && lstprod.Count > 0)
            {
                lstprod.ForEach(x => total += x.Prix());
            }
            return total;
        }

        public delegate void modif(Produit p);

        /// <summary>
        /// Ajoute ou Supprime un produit
        /// </summary>
        /// <param name="modif">Fonction delegate permettant une modification sur le fichier</param>
        /// <param name="p">Produit a modifier</param>
        public static void Modification(modif modif, Produit p)
        {
            modif(p);
        }

        public void Ajout(Produit p)
        {
            if (p != null)
            {
                lstprod.Add(p);
            }
        }

        public void Suppression(Produit p)
        {
            if(p!= null)
            {
                Produit prod = lstprod.Find(x => x.Label == p.Label && x.Quantite == p.Quantite);
                if (prod != null) lstprod.Remove(prod);
            }
        }

        
        /// <summary>
        /// Imprime la facture de la commande actuel pour que le livreur pprenne et effectue sa commande
        /// </summary>
        public void ImpressionFacture()
        {
            StreamWriter sw = new StreamWriter($"factures/Facture_n°{id}.csv",false);
            #region titre
            sw.WriteLine(new string('-', 100));
            sw.WriteLine($";;Facture_n°{id}");
            sw.WriteLine(new string('-', 100));
            sw.WriteLine("\n\n\n");
            sw.WriteLine($"Numero du client : ;; {NumClient}");
            sw.WriteLine("\n\n");
            #endregion
            sw.WriteLine(";Pizza Commandees \n");
            sw.WriteLine("Type de Pizza ;Taille;Quantite;Total\n");

            lstprod.ForEach(delegate (Produit prod) { if (prod is Pizza) sw.WriteLine($"{prod}"); });
            sw.WriteLine("\n\n\n");

            sw.WriteLine(";Produits Annexes \n");
            sw.WriteLine("Label;Volume;Quantite;Total\n");

            lstprod.ForEach(delegate (Produit prod) { if (prod is Boisson) sw.WriteLine($"{prod}"); });
            sw.Close();
        }

        public override string ToString()
        {
            return id + ";" + date + ";" + NumClient + ";" + nomCommis + ";" + NomLivreur + ";" + etat + ";" +  encaisse;

        }
    }
}
