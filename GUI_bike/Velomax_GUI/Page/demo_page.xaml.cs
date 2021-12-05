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
using MySql.Data.MySqlClient;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour demo_page.xaml
    /// </summary>
    public partial class demo_page : Page
    {
        Grid[] grids;
        public string[] LabelProduit { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        public demo_page()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            grids = new Grid[4]{ grid1, grid2, grid3, grid4 };
            fill_box_noclient();
            fill_box_nofournisseur();
            fill_grid1();
            fill_grid3();
        }

        private void change_color(object sender, MouseEventArgs e)
        {
            Border button = (Border)sender;
            if (button.IsMouseOver)
                button.Background = Brushes.Azure;
            else button.Background = Brushes.Transparent;
        }

        private void next_grid(object sender, MouseButtonEventArgs e)
        {
            int current = 0;
            for (int i = 0; i < grids.Length; i++)
            {
                if (grids[i].Visibility == Visibility.Visible) current = i;
            }
            grids[current].Visibility = Visibility.Collapsed;
            if(current >= 3)
                grids[0].Visibility = Visibility.Visible;
            else 
                grids[current + 1].Visibility = Visibility.Visible;
        }

        public void fill_grid1()
        {
            string req = "select count(*) nbclient from (select no_i from individu union select no_b from boutique) as t;";
            MySqlDataReader reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                lbl_nbclient.Content = reader.GetInt32("nbclient").ToString();
            }
        }

        private void fill_grid2(object sender, SelectionChangedEventArgs e)
        {
            string noc = (string)box_client.SelectedItem;
            string req = $"select count(no_c) nbcom from commande where no_client = '{noc}';";
            MySqlDataReader reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                lblnb_commande.Content = reader.GetInt32("nbcom").ToString();
            }
            req = $"select sum(quantite * prix) cumul from commande natural join compose c join piece p on c.no_equipement = p.no_p where no_client = '{noc}'; ";
            reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                lbl_cumul.Text = reader.GetDouble("cumul").ToString() + " euros";
            }
        }

        public void fill_grid3()
        {
            string req = "select nom_p nom, stock from piece natural join delivrer where stock <= 2 union " +
                "select concat(nom_m,' taille ',label) nom, stock from modele natural join grandeur where stock <= 2;";
            MySqlDataReader reader = Controle.Requete(req, true);
            ChartValues<double> lststock = new ChartValues<double>();
            List<string> lstlabel = new List<string>();
            while (reader.Read())
            {
                lstlabel.Add((string)reader["nom"]);
                lststock.Add(reader.GetInt32("stock"));
            }

            SeriesCollection.Add(new StackedColumnSeries
            {
                Title = "Quantité restante",
                Values = lststock,
                StackMode = StackMode.Values
            });

            LabelProduit = lstlabel.ToArray<string>();
            DataContext = this;
        }

        public void fill_box_noclient()
        {
            string req = "select no_i as no from individu union select no_b as no from boutique;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> lstn = new List<string>();
            while (reader.Read())
            {
                lstn.Add((string)reader["no"]);
            }
            box_client.ItemsSource = lstn;
        }

        public void fill_box_nofournisseur()
        {
            string req = "select siret from fournisseur;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<double> lstn = new List<double>();
            while (reader.Read())
            {
                lstn.Add((double)reader["siret"]);
            }
            box_fournisseur.ItemsSource = lstn;
        }

        private void fill_grid4(object sender, SelectionChangedEventArgs e)
        {
            double siret = (double)box_fournisseur.SelectedItem;
            string req = $"select nom_f from fournisseur where siret = '{siret}';";
            MySqlDataReader reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                lbl_nomf.Content = reader.GetString("nom_f");
            }
            req = $"select count(no_p) nbpiece from delivrer where siret = '{siret}'; ";
            reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                lblnb_piece.Text = reader.GetDouble("nbpiece").ToString();
            }
        }
    }
}
