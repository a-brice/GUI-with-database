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
    /// Logique d'interaction pour PageModifCommande.xaml
    /// </summary>
    public partial class PageModifCommande : Page
    {
        Pizzeria pizzeria;
        Commande com;
        public PageModifCommande(Pizzeria piz, Commande c)
        {
            InitializeComponent();
            pizzeria = piz;
            com = c;

            boxlivreur.ItemsSource = pizzeria.LstPersonne<Livreur>();
            boxboisson.ItemsSource = pizzeria.LstProduit.FindAll(x => x is Boisson);
            boxpizza.ItemsSource = pizzeria.LstProduit.FindAll(x => x is Pizza);

            lstviewprod.ItemsSource = c.LstProduit;
            lbltitre.Content += com.Id + "";
        }

        private void Btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageGestionCommande(pizzeria));
        }


        /// <summary>
        /// Ajoute une pizza à la commande et modifie le fichiers DetailCommande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Addpizza_Click(object sender, RoutedEventArgs e)
        {
            if (boxpizza.SelectedValue != null && int.TryParse(boxquantite.Text, out int quantite) && boxtaille.SelectedValue != null)
            {
                Pizza p = (Pizza)boxpizza.SelectedValue;        // Pizza existante 
                string taille = ((ListBoxItem)boxtaille.SelectedValue).Content.ToString();
                Pizza piz = new Pizza(p.Type, taille, quantite, p.PrixBase);
                Commande.Modification(com.Ajout, piz);
                pizzeria.MajFichierCommande("DetailsCommandes.csv");
                lstviewprod.ItemsSource = "";
                lstviewprod.ItemsSource = com.LstProduit;       // Update listview

            }
        }


        /// <summary>
        /// Ajoute une boisson à la commande et modifie le fichiers DetailCommande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Addboisson_Click(object sender, RoutedEventArgs e)
        {

            if (boxboisson.SelectedValue != null && int.TryParse(boxqb.Text, out int quantite) && boxformat.SelectedValue != null)
            {
                Boisson b = (Boisson)boxboisson.SelectedValue;        // Boisson existante 
                string vol = ((ListBoxItem)boxformat.SelectedValue).Content.ToString().Substring(0, ((ListBoxItem)boxformat.SelectedValue).Content.ToString().Length - 2);
                Boisson bs = new Boisson(b.Type, vol, quantite, b.PrixBase);
                Commande.Modification(com.Ajout, bs);
                pizzeria.MajFichierCommande("DetailsCommandes.csv");
                lstviewprod.ItemsSource = "";
                lstviewprod.ItemsSource = com.LstProduit;       // Update

            }
        }


        /// <summary>
        /// Mettre a jour la commande en fonction du nom du livreur, de son etat et du solde si l'une des valeur entrée a changé 
        /// Si la commande est valide, alors le cumul des point du client augmente 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btnmaj_Click(object sender, RoutedEventArgs e)
        {
            if (boxlivreur.SelectedIndex >= 0 && boxlivreur.SelectedItem != null)   // listbox livreur
                com.NomLivreur = ((Livreur) boxlivreur.SelectedItem).Nom;     // Recupere la valeur des listbox pour mettre a jour la commande

            if (boxetat.SelectedIndex >= 0 && ((ListBoxItem)boxetat.SelectedValue).Content != null )    // Listboxitem etat
            {
                com.Etat = ((ListBoxItem)boxetat.SelectedValue).Content.ToString();
                if (((ListBoxItem)boxetat.SelectedValue).Content.ToString() == "en livraison" || ((ListBoxItem)boxetat.SelectedValue).Content.ToString()=="fermée")
                {
                    com.ImpressionFacture();
                }
            }


            if (boxsolde.SelectedIndex >= 0 && ((ListBoxItem)boxsolde.SelectedValue).Content != null)   // listboxitem solde
                com.Encaisser = ((ListBoxItem)boxsolde.SelectedValue).Content.ToString();
            MessageBox.Show("Mise à jour effectué !");

            if (boxetat.SelectedValue != null && boxsolde.SelectedValue != null &&
                ((ListBoxItem)boxetat.SelectedValue).Content.ToString() == "fermée" && ((ListBoxItem)boxsolde.SelectedValue).Content.ToString() == "ok")
            {
                ((Client)pizzeria.LstPersonne<Client>().Find(x => "0" + x.Num.ToString() == com.NumClient)).CumulCalcul(com.Prix());
                MessageBox.Show("Le cumul des point du client a augmenté !");
                pizzeria.MajFichierPersonne<Client>("Clients.csv");
            }
            else if (boxetat.SelectedValue != null && boxsolde.SelectedValue != null && 
                ((ListBoxItem)boxetat.SelectedValue).Content.ToString()  == "fermée" && 
                ((ListBoxItem)boxsolde.SelectedValue).Content.ToString() == "perdue")
                MessageBox.Show("Le cumul des point du client est resté constant !");

            pizzeria.MajFichierCommande("Commandes.csv");
        }


        /// <summary>
        /// Esthétique de la page 
        /// A chaque que l'utilisateur choisi une valeur d'une textbox, elle se ferme immédiatement après 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fermer_expander(object sender, SelectionChangedEventArgs e)
        {
            exp1.IsExpanded = false;
            exp2.IsExpanded = false;
            exp3.IsExpanded = false;
            exp4.IsExpanded = false;
            exp5.IsExpanded = false;
            exp6.IsExpanded = false;
            exp7.IsExpanded = false;
            exp8.IsExpanded = false;
            exp9.IsExpanded = false;
        }
    }
}
