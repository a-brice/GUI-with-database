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
    /// Logique d'interaction pour PagePizzaProduit.xaml
    /// </summary>
    public partial class PagePizzaProduit : Page
    {
        public Pizzeria pizzeria;
        public PagePizzaProduit(Pizzeria piz)
        {
            InitializeComponent();
            pizzeria = piz;
            chargeliste();
        }


        public void chargeliste()
        {
            if(pizzeria != null)
            {
                lstboisson.ItemsSource = pizzeria.LstProduit.FindAll(x => x is Boisson);
                lstpizza.ItemsSource = pizzeria.LstProduit.FindAll(x => x is Pizza);
                
            }
        }

        /// <summary>
        /// Ajouter une pizza a la liste de produit proposer par la pizzeria et maj des fichier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(pizzeria != null && txtboxpizza.Text.Length > 2 && double.TryParse(txtbox2pizza.Text, out double prix))
            {
                pizzeria.LstProduit.Add(new Pizza(prix, txtboxpizza.Text));
                chargeliste();
                pizzeria.MajFichierProduit();
            }
        }

        /// <summary>
        /// Ajouter une boisson a la liste de produit proposer par la pizzeria et maj des fichier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (pizzeria != null && txtboxboisson.Text.Length > 2 && double.TryParse(txtbox2boisson.Text, out double prix))
            {
                pizzeria.LstProduit.Add(new Boisson(txtboxboisson.Text,prix));
                chargeliste();
                pizzeria.MajFichierProduit();
            }
        }

        /// <summary>
        /// Supprimer une pizza de la liste des produit et mettre a jour 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if(lstpizza.SelectedValue != null)
            {
                pizzeria.LstProduit.Remove((Produit)lstpizza.SelectedValue);
                pizzeria.MajFichierProduit();
                chargeliste();
            }
        }


        /// <summary>
        /// Supprimer une boisson de la liste des produit et mettre a jour 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (lstboisson.SelectedValue != null)
            {
                pizzeria.LstProduit.Remove((Produit)lstboisson.SelectedValue);
                pizzeria.MajFichierProduit();
                chargeliste();
            }
        }

        private void Btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage(pizzeria));
        }
    }
}
