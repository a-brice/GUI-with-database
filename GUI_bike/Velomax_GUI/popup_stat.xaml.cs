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
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour popup_stat.xaml
    /// </summary>
    public partial class popup_stat : Window
    {
        public popup_stat()
        {
            InitializeComponent();

            string req = "select avg(somme_piece_modele) as montant from ( " + 
                            "select no_c, sum(sp) somme_piece_modele from( " +
                            "select no_c, sum(prix) sp from compose join piece on no_equipement = no_p group by no_c UNION " +
                            "select no_c, SUM(prix_m) sp from compose join modele on no_equipement = no_m  group by no_c) as t " + 
                            " group by no_c) as t2; ";
            MySqlDataReader reader = Controle.Requete(req, true);

            if (reader.Read())
            {
                pie_montant.Value = Math.Round( reader.GetDouble("montant"),3);
            }
            reader.Close();
            req = "select avg(sp) moy from (select no_c, count(no_m) sp from compose join modele on no_equipement = no_m  group by no_c) as t; ";
            reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                pie_velo.Value = Math.Round(reader.GetDouble("moy"),2);
            }
            reader.Close();
            req = "select avg(sp) as moy from (select no_c, count(no_p) sp from compose join piece on no_equipement = no_p group by no_c) as t; ";
            reader = Controle.Requete(req, true);
            if (reader.Read())
            {
                pie_piece.Value = Math.Round(reader.GetDouble("moy"),2);
            }

        }

    }
}
