using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetWPFV2
{
    /// <summary>
    /// Logique d'interaction pour PageGestionCommis.xaml
    /// </summary>
    public partial class PageGestionCommis : Page
    {
        Pizzeria pizzeria;
        Commis commis;
        public PageGestionCommis(Commis cms, Pizzeria pz)
        {
            InitializeComponent();
            lblnom.Content += " " + cms.Nom + " " + cms.Prenom;
            pizzeria = pz;
            commis = cms;
            fillList();
        }
        /// <summary>
        /// Rempli la liste en fonction du nom du commis identifier 
        /// rempli la liste de tout les client dans listeClient
        /// </summary>
        public void fillList()
        {
            if(commis != null && pizzeria != null)
            {
                // Liste de commande 
                List<Commande> lstcom = pizzeria.LstCommande.FindAll(x => x.NomCommis == commis.Nom && x.Etat != "fermee" && x.Etat != "fermée");     // COmmande gérée par le commis, non fermé 
                listeCommande.ItemsSource = lstcom;


                // Liste de client
                listeClient.ItemsSource = pizzeria.LstPersonne<Client>();
            }

            
        }

        /// <summary>
        /// Permet d'envoyer a la page PageModifCommande une commande a modifier en fonction de la selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (listeCommande.SelectedValue != null)
            {
                Commande comm = (Commande)listeCommande.SelectedValue;
                this.NavigationService.Navigate(new PageModifCommande(pizzeria, comm));
            }
            else MessageBox.Show("Veuillez d'abord séléctionner une commande ");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageIdCommis(pizzeria));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            grid1.Visibility = Visibility.Collapsed;
            grid2.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Enregistre les informartion du client pour sa création et creer une nouvelle commande avec le numéro du client creer 
        /// et redirige l'utilisateur vers la page de modification de commande pour entrer les produit etc 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (boxadresse.Text != null && boxnom.Text != null && int.TryParse(boxnum.Text, out int num) && boxprenom.Text != null)
            {
                pizzeria.Modification(pizzeria.Ajout, num, new Client(boxnom.Text, boxprenom.Text, boxadresse.Text, num));
                Commande com = new Commande(num, commis.Nom, null);
                pizzeria.LstCommande.Add(com);
                pizzeria.MajFichierCommande("Commandes.csv");
                MessageBox.Show("Le Client a bien été ajouté aux fichiers !");
                this.NavigationService.Navigate(new PageModifCommande(pizzeria,com));
            }
            else MessageBox.Show("Entrée incorrect !");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            gridclient1.Visibility = Visibility.Collapsed;
            gridclient2.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// Créer une commande et redirige l'utilisateur vers la page de gestion de commande 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListeClient_Selected(object sender, RoutedEventArgs e)
        {
            Client c = (Client)listeClient.SelectedValue;
            Commande com = new Commande(c.Num, commis.Nom, null);
            pizzeria.LstCommande.Add(com);
            pizzeria.MajFichierCommande("Commandes.csv");
            this.NavigationService.Navigate(new PageModifCommande(pizzeria,com));
        }
        /// <summary>
        /// Recherrcher un client a chaque saisi utilisateur 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rechercheClient(object sender, TextChangedEventArgs e)
        {
            listeClient.ItemsSource = pizzeria.LstPersonne<Client>().FindAll(x => x.Num.ToString() == boxsearchclient.Text || x.Num.ToString().Contains(boxsearchclient.Text));
        }
    }
}
