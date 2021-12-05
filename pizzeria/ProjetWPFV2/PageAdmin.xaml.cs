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
    /// Logique d'interaction pour PageAdmin.xaml
    /// </summary>
    public partial class PageAdmin : Page
    {
        Pizzeria pizzeria;
        List<Personne> lstAffichable;
        public PageAdmin(Pizzeria piz)
        {
            InitializeComponent();
            pizzeria = piz;
            if(pizzeria != null)
            {
                lstAffichable = pizzeria.LstPersonne<Personne>();
                lstviewPersonne.ItemsSource = lstAffichable;
            }
        }

        /// <summary>
        /// methode affichant la liste trier en fonction des choix des utilisateur 
        /// Peut afficher en fonction du type (Client, commis et livreur ) 
        /// Peut afficher la meme liste trier et l'actualise 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkbox_changement(object sender, RoutedEventArgs e)
        {
                lstAffichable = pizzeria.LstPersonne<Personne>();
            if (chkboxclient.IsChecked == false) lstAffichable = lstAffichable.FindAll(x => !(x is Client));
            if (chkboxcommis.IsChecked == false) lstAffichable = lstAffichable.FindAll(x => !(x is Commis));
            if (chkboxlivreur.IsChecked == false) lstAffichable = lstAffichable.FindAll(x => !(x is Livreur));

            // l'Essentiel de la fonction TriPersonnePar() pour ne pas l'appeler directement et former une boucle infini
            if (radionom.IsChecked == true) lstAffichable = Personne.Tri(Personne.triNom, lstAffichable);
            if (radioville.IsChecked == true) lstAffichable = Personne.Tri(Personne.triVille, lstAffichable);
            if (radiocumul.IsChecked == true) lstAffichable = Personne.Tri(Personne.triCumul, lstAffichable.FindAll(x => x is Client));

            lstviewPersonne.ItemsSource = lstAffichable;
        }


        /// <summary>
        /// Methode qui supprime une personne de la liste et des fichier et actualise la liste 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lstviewPersonne.SelectedValue != null)
            {
                Personne pers = (Personne)lstviewPersonne.SelectedValue;
                pizzeria.Modification(pizzeria.Suppression,pers.Num,pers);
                MessageBox.Show($"{pers.Nom} {pers.Prenom} a été supprimé des fichiers");

                lstAffichable = pizzeria.LstPersonne<Personne>();
                lstviewPersonne.ItemsSource = lstAffichable;

            }
        }

        // Bouton de retour 
        private void Btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage(pizzeria));
        }
    }
}
