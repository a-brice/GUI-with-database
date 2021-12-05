using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.IO;

namespace ProjetWPFV2
{
    /// <summary>
    /// Logique d'interaction pour PageGestionCommande.xaml
    /// </summary>
    public partial class PageGestionCommande : Page
    {
        Pizzeria pizzeria;
        public PageGestionCommande(Pizzeria piz)
        {
            InitializeComponent();
            pizzeria = piz;
            fillListe(new DateTime(1970, 01, 01), new DateTime(2170, 01, 01));
        }

        /// <summary>
        /// actualise la liste en fonction de 2 date 
        /// </summary>
        /// <param name="dtdebut">date de début</param>
        /// <param name="dtfin">date de fin</param>
        public void fillListe(DateTime dtdebut, DateTime dtfin)
        {
            lstviewcommande.ItemsSource = pizzeria.LstCommande.FindAll(x => x.DateHeure > dtdebut && x.DateHeure < dtfin);
        }


        /// <summary>
        /// Actualise la listview en fonction des date entrées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void actualise(object sender, SelectionChangedEventArgs e)
        {
            DateTime dt1 = new DateTime(1970, 01, 01);
            DateTime dt2 = new DateTime(2170, 01, 01);

            if (datedebut.Text != null)
            {
                DateTime.TryParse(datedebut.Text, out dt1);
            }
            if (datefin.Text != null)
            {
                DateTime.TryParse(datefin.Text, out dt2);

            }

            dt2 = dt2 > dt1 ? dt2 : new DateTime(2170, 01, 01);
            fillListe(dt1, dt2);
        }

        /// <summary>
        /// Permet de rechercher un id en fonction de l'id entré par l'utilisateur 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recherche(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(boxsearch.Text, out int id))
            {
                lstviewcommande.ItemsSource = pizzeria.LstCommande.FindAll(x => x.Id == id || x.Id.ToString().Contains(id.ToString()));
            }

        }

        private void Btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage(pizzeria));
        }

        /// <summary>
        /// Prend la commande sélectionnée et l'envoie sur la nouvelle page de modification 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lstviewcommande.SelectedValue != null)
            {
                Commande c = (Commande)lstviewcommande.SelectedValue;
                this.NavigationService.Navigate(new PageModifCommande(pizzeria, c));
            }
        }



        private void Button_Facture(object sender, RoutedEventArgs e)
        {
            if (lstviewcommande.SelectedValue != null)
            {
                Commande c = (Commande)lstviewcommande.SelectedValue;
                string id = c.Id.ToString();
                string path = $@"factures\Facture_n°{c.Id}.csv";


                if  (c.Etat=="fermée" || c.Etat=="en livraison")
                {
                    if (File.Exists(path))
                    {
                        Process.Start(path);
                    } else  MessageBox.Show("Erreur dans les paramètres "); 
                }
                else
                {
                    MessageBox.Show("Cette commande n'est pas encore prête ");
                }
                
            }
        }
    }
}
