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
using MySql.Data.MySqlClient;
using System.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour Stock_page.xaml
    /// </summary>
    public partial class Stock_page : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public string[] LabelMarque { get; set; }

        public string[] reqs = {
                                "select * from delivrer natural join piece natural join fournisseur order by no_p;",
                                "select no_m, nom_m, categorie,label,stock from modele natural join grandeur;",
                                "select siret, nom_f, libelle, sum(stock) as stock from fournisseur natural join delivrer group by siret;",
                                "select categorie, sum(stock) as stock from modele natural join grandeur group by categorie;",
                                "select nom_m as marque, prix_m, categorie, sum(stock) as stock from modele natural join grandeur group by nom_m;"
            };
        public Stock_page()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            SeriesCollection2 = new SeriesCollection();
            fill_modele();
            fill_piece();
            fill_cate();
            fill_marque();
        }

        public void fill_piece()
        {
            MySqlDataReader reader = Controle.Requete(reqs[0], true);
            List<Stock_Piece> lstp = new List<Stock_Piece>();

            while (reader.Read())
            {
                Stock_Piece sp = new Stock_Piece
                {
                    No = (string)reader["no_p"],
                    Nom = (string)reader["nom_p"],
                    Cout = (double)reader["prix"],
                    Stock = reader.GetInt32("stock"),
                    Siret = (double)reader["siret"],
                    Nomf = (string)reader["nom_f"],
                    NoPF = (string)reader["num_piece"],
                    Delai = (int)reader["delai"]
                };
                lstp.Add(sp);

            }
            listview_piece.ItemsSource = lstp;
        }


        public void fill_modele()
        {
            MySqlDataReader reader = Controle.Requete(reqs[1], true);
            List<Stock_Modele> lstm = new List<Stock_Modele>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    Stock_Modele sm = new Stock_Modele
                    {
                        No = (string)reader["no_m"],
                        Nom = (string)reader["nom_m"],
                        Categorie = (string)reader["categorie"],
                        Grandeur = (string)reader["label"],
                        Stock = reader.GetInt32("stock")
                    };
                    lstm.Add(sm);

                }
                listview_modele.ItemsSource = lstm;
            }
        }

        public void fill_cate()
        {
            MySqlDataReader reader = Controle.Requete(reqs[3], true);

            while (reader.Read())
            {
                SeriesCollection.Add(
                    new PieSeries
                    {
                        Title = (string)reader["categorie"],
                        Values = new ChartValues<ObservableValue> { new ObservableValue(reader.GetInt32("stock")) },
                        DataLabels = true
                    }
                    );

            }
            DataContext = this;
        }

        public void fill_marque()
        {
            MySqlDataReader reader = Controle.Requete(reqs[4], true);
            ChartValues<double> lststock = new ChartValues<double>();
            List<string> lstlabel = new List<string>();
            while (reader.Read())
            {
                lstlabel.Add((string)reader["marque"]);
                lststock.Add(reader.GetInt32("stock"));
            }

            SeriesCollection2.Add(new StackedColumnSeries
            {
                Title = "Quantité restantes",
                Values = lststock,
                StackMode = StackMode.Values
            });

            LabelMarque = lstlabel.ToArray<string>();
        }

        class Stock_Modele
        {
            public string No { get; set; }
            public string Nom { get; set; }
            public string Categorie { get; set; }
            public string Grandeur { get; set; }
            public int Stock { get; set; }
        }

        class Stock_Piece
        {
            public string No { get; set; }
            public double Siret { get; set; }
            public string Nomf { get; set; }
            public string NoPF { get; set; }
            public string Nom { get; set; }
            public int Delai { get; set; }
            public double Cout { get; set; }
            public int Stock { get; set; }

        }
    }
}