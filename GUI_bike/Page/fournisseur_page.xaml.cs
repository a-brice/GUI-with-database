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

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour fournisseur_page.xaml
    /// </summary>
    public partial class fournisseur_page : Page
    {
        public fournisseur_page()
        {
            InitializeComponent();
            update_list_fournisseur();
            string req = "select no_p from piece;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> nopieces = new List<string>();

            while (reader.Read())
            {
                nopieces.Add((string)reader["no_p"]);
            }
            
            box_no.ItemsSource = nopieces;
        }

        public void update_list_fournisseur()
        {
            string req = "select * from fournisseur;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Fournisseur> lstf = new List<Fournisseur>();
            while (reader.Read())
            {
                Fournisseur f = new Fournisseur((double)reader["siret"], reader["nom_f"].ToString(), reader["contact"].ToString(),
                    reader["adresse_f"].ToString(), (int)reader["libelle"]);
                lstf.Add(f);
            }
            listview_fournisseur.ItemsSource = lstf;
        }

        private void suprimer_fournisseur(object sender, RoutedEventArgs e)
        {
            Fournisseur current_fournisseur = (Fournisseur)((Image)sender).DataContext;
            current_fournisseur.Suppression();
            MessageBox.Show("Bien supprimé");
            update_list_fournisseur();
        }

        private void supprimer_piece_fornisseur(object sender, RoutedEventArgs e)
        {
            try
            {
                Delivrer current_d = (Delivrer)((Image)sender).DataContext;
                Fournisseur current_f = (Fournisseur)listview_fournisseur.SelectedItem;
                if (current_d != null && current_f != null)
                {
                    string req = $"delete from delivrer where siret = {current_f.Siret} and no_p = '{current_d.No}';";
                    Controle.Requete(req, false);
                    update_list_piece(current_f);
                }
            }
            catch (Exception){  }
            
        }
        

        private void get_fournisseur_in_boxes(object sender, SelectionChangedEventArgs e)
        {
            Fournisseur current_f = (Fournisseur)listview_fournisseur.SelectedItem;
            if(current_f != null)
            {
                box_nom.Text = current_f.Nom;
                box_adresse.Text = current_f.Adresse;
                box_siret.Text = current_f.Siret.ToString();
                box_contact.Text = current_f.Contact;
                box_libelle.Text = current_f.Libelle.ToString();
                update_list_piece(current_f);
            }
        }

        private void update_list_piece(Fournisseur f)
        {
            string req = $"select no_p, nom_p, num_piece, delai, cout, stock from delivrer natural join fournisseur natural join piece where siret = {f.Siret} ;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Delivrer> lstd = new List<Delivrer>();
            while (reader.Read())
            {
                Delivrer d = new Delivrer {No = (string)reader["no_p"], Nom = reader["nom_p"].ToString(), Delai = (int)reader["delai"],
                    NoF = (string)reader["num_piece"], Cout = (double)reader["cout"], Stock = (int)reader["stock"] };
                lstd.Add(d);
            }
            list_piece.ItemsSource = lstd;

        }

        private void get_piece_in_boxes(object sender, SelectionChangedEventArgs e)
        {
            Delivrer current_d = (Delivrer)list_piece.SelectedItem;
            if(current_d != null)
            {
                box_stock.Text = current_d.Stock.ToString();
                box_cout.Text = current_d.Cout.ToString();
                box_delai.Text = current_d.Delai.ToString();
                box_no.Text = current_d.No;
                box_numf.Text = current_d.NoF;
            }
            else
            {
                box_no.Text = "";
                box_stock.Text = "";
                box_cout.Text = "";
                box_numf.Text = "";
                box_delai.Text = "";
            }
        }

        private void Add_Checked(object sender, RoutedEventArgs e)
        {
            box_adresse.Text = "";
            box_contact.Text = "";
            box_cout.Text = "";
            box_delai.Text = "";
            box_libelle.Text = "";
            box_no.Text = "";
            box_numf.Text = "";
            box_nom.Text = "";
            box_siret.Text = "";
            box_stock.Text = "";
        }

        private void validation_fournisseur(object sender, RoutedEventArgs e)
        {
            string choix = mod.IsChecked == true ? "modifier" : add.IsChecked == true ? "ajout" : "";
            double.TryParse(box_siret.Text, out double siret);
            int.TryParse(box_libelle.Text, out int libelle);
            string nom = box_nom.Text;
            string adresse = box_adresse.Text;
            string contact = box_contact.Text;

            if(choix != "" && nom != "" && adresse != "" && contact != "")
            {
                switch (choix)
                {
                    case "ajout":
                        {
                            Fournisseur new_f = new Fournisseur(siret, nom, contact, adresse, libelle);
                            new_f.Ajout();
                            break;
                        }
                    case "modifier":
                        {
                            Fournisseur current = (Fournisseur)listview_fournisseur.SelectedItem;
                            if(current != null)
                            {
                                current.Modif("nom_f", nom);
                                current.Modif("contact", contact);
                                current.Modif("adresse_f", adresse);
                                current.Modif("libelle", libelle.ToString());
                            }
                            break;
                        }
                }
                update_list_fournisseur();
            }

        }

        private void validation_piece(object sender, RoutedEventArgs e)
        {
            string choix = mod.IsChecked == true ? "modifier" : add.IsChecked == true ? "ajout" : "";

            Fournisseur current = ((Fournisseur)listview_fournisseur.SelectedItem);
            int.TryParse(box_delai.Text, out int delai);
            double.TryParse(box_cout.Text, out double cout);
            int.TryParse(box_stock.Text, out int stock);
            string no = box_no.Text;
            string numf = box_numf.Text;
            MessageBox.Show((choix != "" && delai > 0 && cout > 0 && no != "" && numf != "" && current != null).ToString());

            if (choix != "" && delai > 0 && cout > 0 && no != "" && numf != "" && current != null)
            {
                current.LivrePiece(no, numf, delai, cout, stock);
                update_list_piece(current);
            }

        }

       
    }

    class Delivrer
    {
        public string No { get; set; }
        public string NoF { get; set; }
        public string Nom { get; set; }
        public int Delai { get; set; }
        public double Cout { get; set; }
        public int Stock { get; set; }

    }
}
