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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour statistique_page.xaml
    /// </summary>
    public partial class statistique_page : Page
    {
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection3 { get; set; }
        public SeriesCollection SeriesCollection4 { get; set; }
        public SeriesCollection SeriesCollection5 { get; set; }
        public string[] LabelClients { get; set; }
        public string[] LabelClients2 { get; set; }
        public string[] LabelsParticulier { get; set; }
        public statistique_page()
        {
            InitializeComponent();
            SeriesCollection = new SeriesCollection();
            SeriesCollection2 = new SeriesCollection();
            SeriesCollection3 = new SeriesCollection();
            SeriesCollection4 = new SeriesCollection();
            SeriesCollection5 = new SeriesCollection();
            pie_modele_draw();
            draw_chart_bestclient();
            draw_subscription();
            update_list_adhesion(0);
        }

      

        public void pie_modele_draw()
        {
            string req = "select m.no_m, sum(c.quantite) as quantite from compose c join modele m on c.no_equipement = m.no_m group by m.no_m order by sum(c.quantite) desc limit 5;";
            MySqlDataReader reader = Controle.Requete(req, true);

            while (reader.Read())
            {
                SeriesCollection.Add(
                    new PieSeries
                    {
                        Title = (string)reader["no_m"],
                        Values = new ChartValues<ObservableValue> { new ObservableValue(reader.GetInt32("quantite")) },
                        DataLabels = true
                    }
                    );

            }
            reader.Close();

            req = "select p.no_p, sum(c.quantite) as quantite from compose c join piece p on c.no_equipement = p.no_p group by p.no_p order by sum(c.quantite) desc limit 5;";
            reader = Controle.Requete(req, true);

            while (reader.Read())
            {
                SeriesCollection2.Add(
                    new PieSeries
                    {
                        Title = (string)reader["no_p"],
                        Values = new ChartValues<ObservableValue> { new ObservableValue(reader.GetInt32("quantite")) },
                        DataLabels = true
                    }
                    );

            }
            DataContext = this;


        }

        public void draw_chart_bestclient()
        {
            // meilleur individu et boutique au cumul
            string req = "select no_i,concat(nom_i,' ' , prenom_i) nom, sum(prix*quantite) prix_tot " + 
                "from individu join commande on no_i = no_client natural join compose c, piece p " + 
                "where c.no_equipement = p.no_p " + 
                " group by no_i order by prix_tot desc limit 1;";

            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> lstnom_cumul = new List<string>();
            List<string> lstnom_nbvendu = new List<string>();

            if (reader.Read())
            {
                SeriesCollection3.Add(new StackedColumnSeries
                {
                    Title = "Particulier",
                    Values = new ChartValues<double> { (double)reader["prix_tot"],0 },
                    StackMode = StackMode.Values
                });
                lstnom_cumul.Add((string)reader["nom"]);
            }
            reader.Close();

            req = "select no_b,nom_b, sum(prix*quantite) prix_tot " + 
                "from boutique join commande on no_b = no_client natural join compose c, piece p " + 
                "where c.no_equipement = p.no_p " + 
                "group by no_b order by prix_tot desc limit 1; ";
            
            reader = Controle.Requete(req, true);

            if (reader.Read())
            {
                SeriesCollection3.Add(new StackedColumnSeries
                {
                    Title = "Boutique",
                    Values = new ChartValues<double> { 0,(double)reader["prix_tot"] },
                    StackMode = StackMode.Values
                });
                lstnom_cumul.Add((string)reader["nom_b"]);
            }
            reader.Close();



            // meilleur individu et boutique au nb d'article vendu

            req = "select no_b, nom_b,sum(quantite) nbvendu from compose natural join commande join boutique on no_b = no_client group by no_b order by nbvendu desc limit 1; ";


            reader = Controle.Requete(req, true);

            if (reader.Read())
            {
                SeriesCollection4.Add(new StackedColumnSeries
                {
                    Title = "Boutique",
                    Values = new ChartValues<double> { reader.GetInt32("nbvendu"),0 },
                    StackMode = StackMode.Values
                });
                lstnom_nbvendu.Add((string)reader["nom_b"]);
            }
            reader.Close();



            req = "select no_i, concat(nom_i,' ', prenom_i) nom, sum(quantite) nbvendu from compose natural join commande " +
                "join individu on no_i = no_client group by no_i order by nbvendu desc limit 1;";


            reader = Controle.Requete(req, true);

            if (reader.Read())
            {
                SeriesCollection4.Add(new StackedColumnSeries
                {
                    Title = "Particulier",
                    Values = new ChartValues<double> { 0,reader.GetInt32("nbvendu") },
                    StackMode = StackMode.Values
                });
                lstnom_nbvendu.Add((string)reader["nom"]);
            }

            reader.Close();

            LabelClients = lstnom_cumul.ToArray<string>();
            LabelClients2 = lstnom_nbvendu.ToArray<string>();
        }

        public void draw_subscription()
        {
            LabelsParticulier = new[] { "Fidélio", "Fidélio Or", "Fidélio Platine", "Fidélio Max" };

            string req = "select no_programme, count(no_i) nb from programme natural join adhere group by no_programme;";

            MySqlDataReader reader = Controle.Requete(req, true);
            ChartValues<double> lstnb = new ChartValues<double>();

            while (reader.Read())
            {
                lstnb.Add(reader.GetInt32("nb"));
            }
            SeriesCollection5.Add(new LineSeries
            {
                Title = "Souscriptions",
                Values = lstnb
            });
        }

        public void update_list_adhesion(int no_programme)
        {
            string req = "select tel_i,nom_i,date_debut, duree_p from individu natural join adhere natural join programme ";
            req = no_programme == 0 ? req : req + $" where no_programme = {no_programme}";
            req += " order by date_debut;";

            MySqlDataReader reader = Controle.Requete(req, true);
            List<Adhesion> lsta = new List<Adhesion>();

            while (reader.Read())
            {
                lsta.Add(new Adhesion { Num=(int)reader["tel_i"], Nom=(string)reader["nom_i"], DateA = (DateTime)reader["date_debut"], duree = (int)reader["duree_p"] });
            }
            listview_adhesion.ItemsSource = lsta;
        }

        private void change_background_button(object sender, MouseEventArgs e)
        {
            if(IsMouseOver)
                button_stat.Background = Brushes.AliceBlue;
            else
                button_stat.Background = Brushes.Transparent;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            int num_p = rd1.IsChecked == true ? 1 : rd2.IsChecked == true ? 2 : rd3.IsChecked == true ? 3 : rd4.IsChecked == true ? 4 : 0;
            update_list_adhesion(num_p);
        }

        private void show_pop_up(object sender, MouseButtonEventArgs e)
        {
            new popup_stat().Show();
        }
    }

    class Adhesion
    {
        public int Num { get; set; }
        public string Nom { get; set; }
        public DateTime DateA { get; set; }
        public int duree { get; set; }

        public string Tel { get { return "0" + Num; } }
        public string Date_A { get { return DateA.AddYears(duree).ToString("yyyy-MM-dd"); } }
    }
}
