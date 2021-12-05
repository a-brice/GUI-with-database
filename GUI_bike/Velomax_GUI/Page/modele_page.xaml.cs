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
    /// Logique d'interaction pour produit_page.xaml
    /// </summary>
    public partial class modele_page : Page
    {
        bool clic = false;
        public modele_page()
        {
            InitializeComponent();
            fill_box_piece();
            box_no.Text = Modele.NextID().ToString();
            fill_listview_modele();
            
        }

        public void fill_listview_modele()
        {
            string req = "select * from modele;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Modele> lstm = new List<Modele>();
            while (reader.Read())
            {
                Modele m = new Modele((string)reader["no_m"], (string)reader["nom_m"], (double)reader["prix_m"],
                    (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["categorie"]);
                lstm.Add(m);
            }
            listview_modele.ItemsSource = lstm;
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

        public void update_listview_piece(string no_m, string grandeur)
        {
            string req = $"select no_p from associe where no_m = '{no_m}' and grandeur = '{grandeur}';";

            MySqlDataReader reader = Controle.Requete(req, true);
            List<string> nopiece = new List<string>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    nopiece.Add((string)reader["no_p"]);
                }
                listview_piece.ItemsSource = nopiece;
            }
        }

        private void add_piece(object sender, RoutedEventArgs e)
        {
            string no = box_no.Text;
            string nopiece = box_piece.SelectedItem != null ? box_piece.SelectedItem.ToString() : "";
            string grandeur = box_grandeur.Text;

            if (no != "" && nopiece != "" && grandeur != "" )
            {
                string req = $"insert into associe values ('{no}','{nopiece}','{grandeur}');";
                Controle.Requete(req, false);
                update_listview_piece(no, grandeur);
            }
        }


        private void validate_click(object sender, RoutedEventArgs e)
        {
            string no = box_no.Text;
            string nom = box_nom.Text;
            double.TryParse(box_prix.Text.Replace('.', ','), out double prix);
            string categorie = box_categorie.Text;
            DateTime.TryParse(box_dated.Text, out DateTime dated);
            DateTime.TryParse(box_datef.Text, out DateTime datef);

            if (no != "" && nom != "" && prix > 0 && categorie != "" && dated != new DateTime() && datef != new DateTime())
            {
                Modele current = new Modele(no, nom, prix, dated, datef, categorie);

                if (!clic) // Ajout d'un modele
                {
                    //string req = $"insert into modele values ('{no}','{nom}','{prix.ToString().Replace(',', '.')}','{categorie}','{dated.ToString("yyyy-MM-dd")}','{datef.ToString("yyyy-MM-dd")}');";
                    //Controle.Requete(req, false);
                    current.Ajout();
                    MessageBox.Show("Bien ajoutée ! Vous pouvez commencer à y associer les pièces, le stock et les grandeurs.");
                    box_grandeur.IsEnabled = true;
                    box_stock.IsEnabled = true;
                    box_piece.IsEnabled = true;
                    clic = true;
                    button_validation.Content = "Modifier le modèle";
                    fill_listview_modele();
                }
                else  // Modif d'un modèle
                {
                    current.Modif("no_m", no);
                    current.Modif("nom_m", nom);
                    current.Modif("prix_m", prix.ToString().Replace(',', '.'));
                    current.Modif("categorie", categorie);
                    current.Modif("date_debut", dated.ToString("yyyy-MM-dd"));
                    current.Modif("date_fin", datef.ToString("yyyy-MM-dd"));
                    MessageBox.Show("Modele up to date !");
                    
                    if (int.TryParse(box_stock.Text, out int stock) && box_grandeur.Text != "")
                    {
                        string req = $"select stock from grandeur where label = '{box_grandeur.Text}' and no_m = '{no}';";
                        MySqlDataReader reader = Controle.Requete(req, true);
                        if (!reader.Read())
                            req = $"insert into grandeur values ('{box_grandeur.Text}','{no}',{stock});";
                        else
                            req = $"update grandeur set stock = {stock} where no_m = '{no}' and label = '{box_grandeur.Text}';";
                        Controle.Requete(req, false);
                        MessageBox.Show("Stock bien update !");
                    }
                    update_listview_grandeur();
                }
               
            }
           
        }

        public void sup_list_modele(object sender, MouseButtonEventArgs e)
        {
            Modele current = (Modele)((Image)sender).DataContext;
            current.Suppression();
            fill_listview_modele();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string no_p = (string)((Image)sender).DataContext;

            Modele current = (Modele)listview_modele.SelectedItem;

            string req = $"delete from associe where no_m = '{current.Noequipement}' and no_p = '{no_p}' and grandeur = '{box_grandeur.Text}';";
            Controle.Requete(req, false);
            update_listview_piece(current.Noequipement, box_grandeur.Text);
        }

        private void Listview_modele_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Modele current = (Modele) listview_modele.SelectedItem;
            if(current != null)
            {
                box_no.Text = current.Noequipement;
                box_nom.Text = current.Nom;
                box_prix.Text = current.Prix.ToString();
                box_categorie.Text = current.Categorie;
                box_dated.Text = current.Datedebut.ToString("dd/MM/yyyy");
                box_datef.Text = current.Datefin.ToString("dd/MM/yyyy");
                box_grandeur.Text = "";
                box_stock.Text = "";
                update_listview_grandeur();
                clic = true;
                box_grandeur.IsEnabled = true;
                box_stock.IsEnabled = true;
                box_piece.IsEnabled = true;
                button_validation.Content = "Modifier le modèle";
            }
        }

        private void update_listview_grandeur()
        {
            Modele current = (Modele) listview_modele.SelectedItem;
            if (current != null)
            {
                string req = $"select label, stock from grandeur where no_m = '{current.Noequipement}';";
                MySqlDataReader reader = Controle.Requete(req, true);
                List<Association> lstass = new List<Association>();
                while (reader.Read())
                {
                    lstass.Add(new Association { Grandeur = (string)reader["label"], Stock = reader.GetInt32("stock") });
                }
                listview_grandeur.ItemsSource = lstass;
            }
        }

        private void Listview_grandeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Association assoc = (Association)listview_grandeur.SelectedItem;
            Modele current = (Modele)listview_modele.SelectedItem;
            if (assoc != null)
            {
                box_grandeur.Text = assoc.Grandeur;
                box_stock.Text = assoc.Stock.ToString();

                update_listview_piece(current.Noequipement,assoc.Grandeur);
            }
        }

        private void sup_list_grandeur(object sender, MouseButtonEventArgs e)
        {
            Association current = (Association)((Image)sender).DataContext;
            Modele current_m = (Modele)listview_modele.SelectedItem;

            string req = $"delete from grandeur where no_m = '{current_m.Noequipement}' and label = '{current.Grandeur}'; ";
            Controle.Requete(req, false);
            update_listview_grandeur();
        }
    }

    class Association
    {
        public int Stock { get; set; }
        public string Grandeur { get; set; }
    }
}
