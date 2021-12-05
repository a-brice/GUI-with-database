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
using System.Windows.Shapes;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour Window_Accueil.xaml
    /// </summary>
    public partial class Window_Accueil : Window
    {
        public Window_Accueil()
        {
            InitializeComponent();
            frame.Content = new page_accueil();
        }

        private void open_commande(object sender, RoutedEventArgs e)
        {
            frame.Content = new commande_page();
        }

        private void open_client(object sender, RoutedEventArgs e)
        {
            frame.Content = new client_page();
        }

        private void open_fournisseur(object sender, RoutedEventArgs e)
        {
            frame.Content = new fournisseur_page();

        }

        private void open_stock(object sender, RoutedEventArgs e)
        {
            frame.Content = new Stock_page();
        }

        private void open_produit(object sender, RoutedEventArgs e)
        {
            frame.Content = new modele_page();
        }

        private void open_stat(object sender, RoutedEventArgs e)
        {
            frame.Content = new statistique_page();
        }

        private void open_piece(object sender, RoutedEventArgs e)
        {
            frame.Content = new piece_page();

        }

        private void open_export(object sender, RoutedEventArgs e)
        {
            frame.Content = new page_export();
        }

        private void open_demo(object sender, RoutedEventArgs e)
        {
            frame.Content = new demo_page();
        }
    }
}
