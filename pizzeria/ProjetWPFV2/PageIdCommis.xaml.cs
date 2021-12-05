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
    /// Logique d'interaction pour PageIdCommis.xaml
    /// </summary>
    public partial class PageIdCommis : Page
    {
        Pizzeria pizzeria;
        
        public PageIdCommis(Pizzeria piz)
        {
            InitializeComponent();
            if(piz != null)
            pizzeria = piz;
            lstboxhide.ItemsSource = pizzeria.LstPersonne<Commis>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            stkpconnexion.Visibility = Visibility.Visible;
            stkpcreation.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            stkpconnexion.Visibility = Visibility.Collapsed;
            stkpcreation.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Fonction qui créer et ajoute un nouveau commis ou un nouveau livreur en recherchant si la personne n'existe paas deja dans les fichiers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btncreation_Click(object sender, RoutedEventArgs e)
        {
            string nom = txtboxNom.Text, prenom = txtboxPrenom.Text , add = txtboxAdd.Text, transport = txtboxTransport.Text;
            int eff = txtboxEff.SelectedIndex;

            if ( nom != "" && prenom != "" && int.TryParse(txtboxNum.Text, out int num) && num  > 10000 && add != "")
            {
                Personne p = pizzeria.RecherchePersonne(num);
                if (p != null)
                {
                    MessageBox.Show($"Le numéro saisit est déja attribué à Mr/Mme {p.Nom} {p.Prenom}");
                }
                else if (eff == 1 && transport != "")
                {

                    Livreur l = new Livreur(nom, prenom, add, num, "sur place", transport);
                    pizzeria.Modification(pizzeria.Ajout, num, l);
                    MessageBox.Show($"{nom} {prenom} à bien été ajouté !");
                    txtboxNom.Text = ""; txtboxPrenom.Text = ""; txtboxAdd.Text = ""; txtboxTransport.Text = ""; txtboxNum.Text = ""; txtboxEff.Text = "";
                    Button_Click(sender, e);
                    lstboxhide.ItemsSource = pizzeria.LstPersonne<Commis>();        // Update

                }
                else if (eff == 0)
                {
                    Commis c = new Commis(nom, prenom, add, num, "sur place");
                    pizzeria.Modification(pizzeria.Ajout, num, c);
                    MessageBox.Show($"{nom} {prenom} à bien été ajouté !");
                    txtboxNom.Text = ""; txtboxPrenom.Text = ""; txtboxAdd.Text = ""; txtboxTransport.Text = ""; txtboxNum.Text = ""; txtboxEff.Text = "";
                    Button_Click(sender, e);
                    lstboxhide.ItemsSource = pizzeria.LstPersonne<Commis>();    // Update

                }
                else MessageBox.Show("Veuillez remplir correctement tout les champs !");
            }
            else MessageBox.Show("Veuillez remplir correctement tout les champs !");
        }

        /// <summary>
        /// Afficher/Masquer la textbox concernant le moyen de transport du livreur 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtboxEff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (txtboxEff.SelectedIndex == 1)
            {
                lblTransport.Visibility = Visibility.Visible;
                txtboxTransport.Visibility = Visibility.Visible;
            }
            else
            {

                lblTransport.Visibility = Visibility.Collapsed;
                txtboxTransport.Visibility = Visibility.Collapsed;
            }
        }
        


        /// <summary>
        /// Redirige l'utilisateur vers la page de gestion du commis avec le commis associé et la pizzeria en parametre 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtboxid.Text, out int num);
            Commis cms = null;

            if (pizzeria.RecherchePersonne(num) is Commis)
                cms = (Commis)pizzeria.RecherchePersonne(num);


            if (cms != null && txtboxpwsd.Password != null && txtboxpwsd.Password == "admin")
            {
                this.NavigationService.Navigate(new PageGestionCommis(cms,pizzeria));
            } else
            {
                MessageBox.Show("Invalide");
            }
        }

        /// <summary>
        /// Ecrire dans la textbox de l'identifiant du commis le numéro choisi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lstboxhide_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtboxid.Text = ((Commis)lstboxhide.SelectedValue).Num.ToString();
        }

        private void Btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage(pizzeria));
        }
    }
}
