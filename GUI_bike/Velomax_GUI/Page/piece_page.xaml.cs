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

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour piece_page.xaml
    /// </summary>
    public partial class piece_page : Page
    {
        bool clic = false;
        public piece_page()
        {
            InitializeComponent();
            fill_list_piece();
            
        }

        public void fill_list_piece()
        {
            string req = "select * from piece;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Piece> lstp = new List<Piece>();

            while (reader.Read())
            {
                lstp.Add(new Piece((string)reader["no_p"], (string)reader["nom_p"], (double)reader["prix"], (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["description"]));
            }
            listview_piece.ItemsSource = lstp;
        }


        private void Listview_piece_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            button_validation.Content = "Modifier la pièce";
            clic = true;
            Piece current = (Piece)listview_piece.SelectedItem;
            if (current != null)
            {
                box_no.Text = current.Noequipement;
                box_nom.Text = current.Nom;
                box_prix.Text = current.Prix.ToString();
                box_desc.Text = current.Description;
                box_dated.Text = current.Datedebut.ToString();
                box_datef.Text = current.Datefin.ToString();
            }
        }

        private void validate_click(object sender, RoutedEventArgs e)
        {

            string no = box_no.Text;
            string nom = box_nom.Text;
            double.TryParse(box_prix.Text.Replace('.', ','), out double prix);
            string desc = box_desc.Text;
            DateTime.TryParse(box_dated.Text, out DateTime dated);
            DateTime.TryParse(box_datef.Text, out DateTime datef);
            if (no != "" && nom != "" && prix > 0 && desc != "" && dated != new DateTime() && datef != new DateTime())
            {
                Piece current = new Piece(no, nom, prix, dated, datef, desc);
                if (!clic)
                {
                    current.Ajout();
                    MessageBox.Show("Bien Ajouté");
                }
                else
                {
                    current.Modif("no_p", no);
                    current.Modif("nom_p", nom);
                    current.Modif("prix", prix.ToString().Replace(',', '.'));
                    current.Modif("description", desc);
                    current.Modif("date_debut", dated.ToString("yyyy-MM-dd"));
                    current.Modif("date_fin", datef.ToString("yyyy-MM-dd"));
                    MessageBox.Show("Bien Modifié");

                }
                fill_list_piece();
            }
              
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Piece current = (Piece)((Image)sender).DataContext;
            current.Suppression();
            fill_list_piece();

        }
    }
}
