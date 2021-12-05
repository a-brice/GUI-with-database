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
using System.Globalization;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour Commande.xaml
    /// </summary>
    public partial class commande_page : Page
    {
        public commande_page()
        {
            InitializeComponent();
            update_list_commande();
            fill_box_client();
            fill_box_piece();
            fill_box_velo();
            
        }

        public void fill_box_client()
        {
            string req = "select no_i from individu;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> noclients = new List<string>();

            while (reader.Read())
            {
                noclients.Add((string)reader["no_i"]);
            }
            reader.Close();
            req = "select no_b from boutique;";
            reader = Controle.Requete(req, true);
            while (reader.Read())
            {
                noclients.Add((string)reader["no_b"]);
            }
            box_client.ItemsSource = noclients;
        }

        public void fill_box_velo()
        {
            string req = "select no_m from modele;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> nomodele = new List<string>();

            while (reader.Read())
            {
                nomodele.Add((string)reader["no_m"]);
            }
            box_velo.ItemsSource = nomodele;
        }
        public void fill_box_piece()
        {
            string req = "select no_p from piece;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> nopiece = new List<string>();

            while (reader.Read())
            {
                nopiece.Add((string)reader["no_p"]);
            }
            box_piece.ItemsSource = nopiece;
        }
        public void update_list_commande()
        {
            string req = "select * from commande";
            req += " order by convert(SUBSTRING_INDEX(no_c,'COM',-1), signed integer) desc;";

            MySqlDataReader reader = Controle.Requete(req, true);
            List<Commande> lstc = new List<Commande>();

            while (reader.Read())
            {
                lstc.Add(
                new Commande((string)reader["no_c"],(string)reader["no_client"], (DateTime)reader["date_c"],(DateTime)reader["date_l"] ));
            }
            listview_com.ItemsSource = lstc;
        }
        private void fermer_expander(object sender, SelectionChangedEventArgs e)
        {
            exp4.IsExpanded = false;
            exp5.IsExpanded = false;
            exp6.IsExpanded = false;
            exp7.IsExpanded = false;
            exp9.IsExpanded = false;
        }

        private List<Composer> get_composition_commande(Commande c)
        {
            List<Composer> lstc = new List<Composer>();
            if (c != null)
            {
                string req = $"select no_p, nom_p, prix, quantite from piece join compose on no_equipement = no_p where no_c = '{c.NoC}';";

                MySqlDataReader reader = Controle.Requete(req, true);

                while (reader.Read())
                {
                    lstc.Add(
                    new Composer { NoP = (string)reader["no_p"], NomP = (string)reader["nom_p"], Prix = (double)reader["prix"], Quantite = (int)reader["quantite"] }
                    );
                }

                req = $"select no_m, nom_m, prix_m, quantite from modele join compose on no_equipement = no_m where no_c = '{c.NoC}';";
                reader.Close();
                reader = Controle.Requete(req, true);

                while (reader.Read())
                {
                    lstc.Add(
                    new Composer { NoP = (string)reader["no_m"], NomP = (string)reader["nom_m"], Prix = (double)reader["prix_m"], Quantite = (int)reader["quantite"] }
                    );
                }
            }
            
            return lstc;
        }

        private void Listview_com_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Commande current_com = (Commande)listview_com.SelectedItem;
            if(current_com != null)
            {
                box_no.Text = current_com.NoC;
                box_client.Text = current_com.Noclient;
                box_datec.Text = current_com.DateC;
                box_datel.Text = current_com.DateL;
                lbltitre.Content = "Modification sur la commande n°" + current_com.NoC;

                lstview_comp.ItemsSource = get_composition_commande(current_com);
                lbl_nom_client.Content = "N° du client :\n" + current_com.Noclient;
                lbl_datec.Content = "Date de commande :\n" + current_com.DateC;
                lbl_datel.Content = "Date de livraison :\n" + current_com.DateL;
            }

            
        }

        private void info_com(object sender, MouseButtonEventArgs e)
        {
            Commande current = (Commande)((Image)sender).DataContext;
            List<Composer> lstc = get_composition_commande(current);
            string phrase = "";
            lstc.ForEach(x => phrase += x.ToString() + "\n");
            MessageBox.Show(phrase);
        }

        private void Add_com_Checked(object sender, RoutedEventArgs e)
        {
            box_no.Text = Commande.NextID();
            box_datel.Text = "";
            box_datec.Text = "";
            box_client.Text = "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string choix = del_com.IsChecked == true ? "supprimer" : mod_com.IsChecked == true ? "modifier" : add_com.IsChecked == true ? "ajout" : "";
            string noc = box_no.Text;
            string noclient = box_client.Text;
            DateTime.TryParse(box_datel.Text, out DateTime datel);
            DateTime.TryParse(box_datel.Text, out DateTime datec);
            if (choix != "" && noclient != "" && datec != new DateTime() && datel != new DateTime() )
            {
                switch (choix)
                {
                    case "ajout":
                        {
                            Commande c = new Commande(Commande.NextID(), noclient, datec, datel);
                            c.Ajout();
                            break;
                        }
                    case "supprimer":
                        {
                            Commande current = (Commande) listview_com.SelectedItem;
                            current.Suppression();
                            break;
                        }
                    case "modifier":
                        {
                            Commande current = (Commande)listview_com.SelectedItem;
                            if (current == null) break;
                            current.Modif("no_client", noclient);
                            current.Modif("date_l", datel.ToString("yyyy-MM-dd"));
                            current.Modif("date_c", datec.ToString("yyyy-MM-dd"));
                            
                            break;
                        }
                }
                update_list_commande();
            }
        }

        private void add_velo(object sender, RoutedEventArgs e)
        {
            Commande current = (Commande)listview_com.SelectedItem;
            if (box_velo.SelectedItem != null && box_taille.SelectedItem != null && box_quantite_velo.Text != "" && current != null)
            {
                string numvelo = box_velo.SelectedItem.ToString();
                string taille = ((ListBoxItem)box_taille.SelectedItem).Content.ToString();
                int.TryParse(box_quantite_velo.Text, out int quantite);
                current.AjoutElement(numvelo, quantite, taille, false);
                lstview_comp.ItemsSource = get_composition_commande(current);

            }
        }

        private void sup_element(object sender, MouseButtonEventArgs e)
        {
            Composer current_compo = (Composer)((Image)sender).DataContext;
            Commande current_com = (Commande)listview_com.SelectedItem;
            if (current_com != null && current_compo != null)
            {
                string req = $"delete from compose where no_c = '{current_com.NoC}' and no_equipement = '{current_compo.NoP}';";
                Controle.Requete(req, false);
                lstview_comp.ItemsSource = get_composition_commande(current_com);
            }

        }

        private void add_piece(object sender, RoutedEventArgs e)
        {
            Commande current = (Commande)listview_com.SelectedItem;
            string no_p = box_piece.SelectedItem.ToString();
            int.TryParse(box_quantite_piece.Text, out int quantite);
            if(no_p != "" && quantite > 0)
            {
                current.AjoutElement(no_p, quantite,"",true);
                lstview_comp.ItemsSource = get_composition_commande(current);
            }
        }
    }
    class Composer
    {
        public string NoP { get; set; }
        public string NomP { get; set; }
        public double Prix { get; set; }
        public int Quantite { get; set; }

        public override string ToString()
        {
            return NomP + " N°" + NoP + " au prix de " + Prix + "$ => " + Quantite + " commandés";
        }
    }
}
