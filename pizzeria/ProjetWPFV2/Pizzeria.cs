using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace ProjetWPFV2
{
    public class Pizzeria 
    {
        string nom = "Del Arte";
        Dictionary<int, Personne> repertoire = new Dictionary<int, Personne>();
        List<Commande> lstCommande = new List<Commande>();
        List<Produit> lstproduit;
        public Pizzeria(string nom, Dictionary<int, Personne> rpr, List<Commande> lstCommande)
        {
            this.nom = nom;
            if (rpr != null) repertoire = rpr;
            else repertoire = new Dictionary<int, Personne>();
            if (lstCommande != null) this.lstCommande = lstCommande;
            else this.lstCommande = new List<Commande>();
        }

        public Pizzeria()
        {
            repertoire = new Dictionary<int, Personne>();
            lstCommande = new List<Commande>();
            lstproduit = new List<Produit>();
        }

        /// <summary>
        /// avoir la list des personne presente dans les fihiers
        /// </summary>
        /// <typeparam name="T">Type de personne (Personne,Commis,Livreur,Client) </typeparam>
        /// <returns></returns>
        public List<Personne> LstPersonne<T>()
        {
            return repertoire.Values.ToList().FindAll(x => x is T); 
        }

        public List<Commande> LstCommande
        {
            get { return lstCommande; }
        }

        public List<Produit> LstProduit
        {
            get { return lstproduit; }
        }
        /// <summary>
        /// cette fonction enregistre chaque ligne des fichiers de personne dans le repertoire sous forme de commis, livreur et client
        /// les defaut dans les informations seront enregisterr et afficher si elle existe
        /// </summary>
        public void chargeFichierPersonne()
        {
            try
            {
                StreamReader sr1 = new StreamReader("Livreur.csv");
                StreamReader sr2 = new StreamReader("Commis.csv");
                StreamReader sr3 = new StreamReader("Clients.csv");
                int erreur = 0;
                while (!sr1.EndOfStream)
                {
                    string[] ligne = sr1.ReadLine().Split(';');
                    int numero = 0;
                    try
                    {
                        int.TryParse(ligne[3], out numero);
                        Livreur l = new Livreur(ligne[0], ligne[1], ligne[2], numero, ligne[4], ligne[5]);
                        repertoire.Add(numero, l);
                    }
                    catch (Exception e) { erreur++; }
                }
                sr1.Close();
                while (!sr2.EndOfStream)
                {
                    string[] ligne = sr2.ReadLine().Split(';');
                    int numero = 0; DateTime date = new DateTime();
                    try
                    {
                        int.TryParse(ligne[3], out numero);
                        DateTime.TryParse(ligne[5], out date);
                        Commis c = new Commis(ligne[0], ligne[1], ligne[2], numero, ligne[4], date);
                        repertoire.Add(numero, c);
                    }
                    catch (Exception) { erreur++;}
                }
                sr2.Close();
                while (!sr3.EndOfStream)
                {
                    string[] ligne = sr3.ReadLine().Split(';');
                    int numero = 0; DateTime date = new DateTime(); double cumul = 0;
                    try
                    {
                        int.TryParse(ligne[3], out numero);
                        DateTime.TryParse(ligne[4], out date);
                        double.TryParse(ligne[5], out cumul);
                        Client c = new Client(ligne[0], ligne[1], ligne[2], numero, date, cumul);
                        repertoire.Add(numero, c);
                    }
                    catch (Exception) { erreur++; }
                    
                }
                sr3.Close();
                if(erreur > 0)
                    MessageBox.Show(erreur + " contact n'ont pas pu etre importer due aux défauts present dans leurs donnée ");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\nIl est nécessaire de relancer l'application");
            }
        }


        /// <summary>
        /// Enregistre les informations des commandes et des produits dans les list lstCommande et lstProduit
        /// </summary>
        public void chargeFichierCommande()
        {
            try
            {
                StreamReader sr1 = new StreamReader("Commandes.csv");
                StreamReader sr2 = new StreamReader("DetailsCommandes.csv");
                StreamReader sr3 = new StreamReader("Produit.csv");
                int erreur = 0;
                sr1.ReadLine();
                sr2.ReadLine();
                while (!sr1.EndOfStream)
                {
                    string[] ligne = sr1.ReadLine().Split(';');
                    int numClient = 0, numCommande = 0;
                    try
                    {
                        int.TryParse(ligne[2], out numClient);
                        int.TryParse(ligne[0], out numCommande);
                        DateTime.TryParse(ligne[1], out DateTime date);
                        Commande c = new Commande(numCommande, date, numClient, ligne[3], ligne[4], ligne[5], null, ligne[6]);
                        lstCommande.Add(c);
                    }
                    catch (Exception) { erreur++; }
                }
                sr1.Close();
                while (!sr2.EndOfStream)
                {
                    string[] ligne = sr2.ReadLine().Split(';');
                    int prix = 0, numCommande = 0, quantite = 0;
                    try
                    {

                        int.TryParse(ligne[6], out quantite);
                        int.TryParse(ligne[2], out prix);
                        int.TryParse(ligne[0], out numCommande);

                        if (ligne[1] == "Pizza")
                        {
                            Pizza pz = new Pizza( ligne[3],ligne[4],quantite,  prix);
                            Commande.Modification(lstCommande.Find(x => x.Id == numCommande).Ajout, pz);    // Delegate 
                        } else
                        {
                            Boisson bs = new Boisson( ligne[1], ligne[5], quantite, prix);
                            Commande.Modification(lstCommande.Find(x => x.Id == numCommande).Ajout, bs);    // Delegate 
                        }
                    }
                    catch (Exception) { erreur++; }
                }
                sr2.Close();

                while (!sr3.EndOfStream)
                {
                    string[] ligne = sr3.ReadLine().Split(';');
                    try
                    {
                        double.TryParse(ligne[2], out double prix);

                        if (ligne[0] == "Pizza")
                        {
                            Pizza pz = new Pizza(prix, ligne[1]);
                            lstproduit.Add(pz);
                        }
                        else
                        {
                            Boisson bs = new Boisson(ligne[1], prix);
                            lstproduit.Add(bs);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                sr3.Close();
                if(erreur > 0)
                    MessageBox.Show(erreur + " erreurs dans les commandes !");
            } catch (Exception e) { MessageBox.Show(e.Message); }
       }

        
        // Methode delegate pour les modification (Ajout,Suppression)
        public delegate void Modif(int numero, Personne p);


        /// <summary>
        /// Ajouter ou supprimer une personne 
        /// </summary>
        /// <param name="modif">delegate servant aaporter une modification sur une personne</param>
        /// <param name="num">numéro de la personne</param>
        /// <param name="p">Nouvelle personne ou personne existante </param>
        public void Modification(Modif modif, int num, Personne p)
        {
            if(p != null && num > 100000)
            modif(num, p);
            if (p is Commis) MajFichierPersonne<Commis>("Commis.csv");
            if (p is Client) MajFichierPersonne<Client>("Clients.csv");
            if (p is Livreur) MajFichierPersonne<Livreur>("Livreur.csv");
        }

        public void Ajout(int num, Personne p) // Ajout Personne
        {
            if (p != null && num > 100000 && !repertoire.ContainsKey(num))
            {
                if (p is Client) repertoire.Add(num, (Client)p);
                else if (p is Commis) repertoire.Add(num, (Commis)p);
                else if (p is Livreur) repertoire.Add(num, (Livreur)p);
                else { }
            }
        }
        public void Suppression(int num, Personne p) // Supprimer Personne
        {
            if (repertoire.ContainsKey(num) && repertoire[num].Nom == p.Nom)
            {
                repertoire.Remove(num);
            }
        }

        /// <summary>
        /// Cette fonction cherche si une personne et presente dans les fichiers
        /// </summary>
        /// <param name="num">numéro de la personne</param>
        /// <returns>personne ayant le numéro entrée en parametre ou null si non trouvé </returns>
        public Personne RecherchePersonne(int num)
        {
            Personne pers = null;
            if (repertoire.ContainsKey(num))
                pers = repertoire[num] is Client ? (Client)repertoire[num] : repertoire[num] is Livreur ? (Livreur)repertoire[num] : repertoire[num] is Commis ? (Commis)repertoire[num] : pers;
            return pers;
        }


        /// <summary>
        /// Recréer un fichier avec les information présentes dans l'instance pizzeria
        /// </summary>
        /// <typeparam name="T">Type de fichier dans lequel nous voulons faire une maj</typeparam>
        /// <param name="fich"></param>
        public void MajFichierPersonne<T>(string fich)
        {
            try
            {
                StreamWriter sw = new StreamWriter(fich, false);
                List<Personne> lst = repertoire.Values.ToList();
                lst = lst.FindAll(x => x is T);
                lst.ForEach(x => sw.WriteLine(x));
                sw.Close();
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Recréer le fichier produit avec les la liste lstProduit de l'instance pizzeria
        /// </summary>
        public void MajFichierProduit()
        {
            try
            {
                StreamWriter sw = new StreamWriter("Produit.csv", false);
                LstProduit.ForEach(x => sw.WriteLine(x.AfficheFichier()));
                sw.Close();
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Recréer les fichier pour effectuer la mise a jour 
        /// </summary>
        /// <param name="fichier">nom du fichier à recréer</param>
        public void MajFichierCommande(string fichier)
        {
            try
            {
                if(fichier == "Commandes.csv")
                {
                    StreamWriter sw = new StreamWriter(fichier, false);

                    sw.WriteLine("N° de commande;jour heure;client;commis;livreur;etat;solde");
                    lstCommande.ForEach(x => sw.WriteLine(x));
                    sw.Close();
                }
                else if(fichier == "DetailsCommandes.csv")
                {
                    StreamWriter sw = new StreamWriter(fichier, false);

                    sw.WriteLine("N°Commande;Nom Item;Prix (à l'unité);Type;Taille;Volume (cl);Quantite;solde");
                    lstCommande.ForEach(delegate(Commande com) {
                        com.LstProduit.ForEach(x => sw.WriteLine(com.Id + ";" + x.AfficheDetail()));
                    });
                    sw.Close();
                }
                
            }
            catch (Exception e)
            {

                System.Windows.MessageBox.Show(e.Message);
            }
        }

    }
}
