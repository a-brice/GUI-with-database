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
    /// Logique d'interaction pour client_page.xaml
    /// </summary>
    public partial class client_page : Page
    {
        public client_page()
        {
            InitializeComponent();
            update_listclient();
            update_listboutique();
        }

        public void update_listclient()
        {
            string req = "select * from individu";
            req += " order by convert(SUBSTRING_INDEX(no_i,'I',-1), signed integer) desc;";
            MySqlDataReader reader = Controle.Requete(req, true);
            if(reader != null)
            {
                List<Individu> lsti = new List<Individu>();
                while (reader.Read())
                {
                    Individu i = new Individu((string)reader["no_i"], reader["nom_i"].ToString(), reader["adresse_i"].ToString(),
                        reader["couriel_i"].ToString(), (int)reader["tel_i"], reader["prenom_i"].ToString());
                    lsti.Add(i);
                }
                listview_indiv.ItemsSource = lsti;
            }
            
        }
        public void update_listboutique()
        {
            string req = "select * from boutique";
            req += " order by convert(SUBSTRING_INDEX(no_b,'B',-1), signed integer) desc;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Boutique> lstb = new List<Boutique>();
            while (reader.Read())
            {
                Boutique b = new Boutique((string)reader["no_b"], reader["nom_b"].ToString(), reader["adresse_b"].ToString(),
                    reader["couriel_b"].ToString(), (int)reader["tel_b"], reader["contact_b"].ToString(), (double)reader["remise"]);
                lstb.Add(b);
            }
            listview_boutique.ItemsSource = lstb;
        }

        private void info_client(object sender, MouseButtonEventArgs e)
        {
            Client current_client = (Client)((Image)sender).DataContext;
            MessageBox.Show(current_client.ToString());
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            box_particulier_adresse.Text = "";
            box_particulier_couriel.Text = "";
            box_particulier_nom.Text = "";
            box_particulier_prenom.Text = "";
            box_particulier_tel.Text = "";
            box_programme.SelectedIndex = -1;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            box_boutique_adresse.Text = "";
            box_boutique_couriel.Text = "";
            box_boutique_nom.Text = "";
            box_boutique_tel.Text = "";
            box_boutique_contact.Text = "";
            box_boutique_remise.Text = "";

        }

        private void Listview_indiv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Individu current_indiv = (Individu)listview_indiv.SelectedItem;
            if(current_indiv != null)
            {
                box_particulier_adresse.Text = current_indiv.Adresse;
                box_particulier_couriel.Text = current_indiv.Couriel;
                box_particulier_nom.Text = current_indiv.Nom;
                box_particulier_prenom.Text = current_indiv.Prenom;
                box_particulier_tel.Text = current_indiv.Num.ToString();
                string req = $"select no_programme from adhere where no_i = '{current_indiv.Noclient}';";
                MySqlDataReader reader = Controle.Requete(req, true);
                if (reader.Read())
                    box_programme.SelectedIndex =  (int)reader["no_programme"] - 1 ;
                else
                    box_programme.SelectedIndex =  - 1;

            }
        }

        private void Listview_boutique_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Boutique current_bout = (Boutique)listview_boutique.SelectedItem;
            if(current_bout != null)
            {
                box_boutique_adresse.Text = current_bout.Adresse;
                box_boutique_couriel.Text = current_bout.Couriel;
                box_boutique_nom.Text = current_bout.Nom;
                box_boutique_remise.Text = current_bout.Remise.ToString();
                box_boutique_contact.Text = current_bout.Contact;
                box_boutique_tel.Text = current_bout.Num.ToString();
            }
        }

        private void individuValidation_Click(object sender, RoutedEventArgs e)
        {
            string choix = del_indiv.IsChecked == true ? "supprimer" : mod_indiv.IsChecked==true ? "modifier" : add_indiv.IsChecked==true ? "ajout" : "";
            string adresse = box_particulier_adresse.Text;
            string couriel = box_particulier_couriel.Text;
            string nom = box_particulier_nom.Text;
            string prenom = box_particulier_prenom.Text;
            int.TryParse(box_particulier_tel.Text, out int tel);

            if (choix != "")
            {
                switch (choix)
                {
                    case "ajout":
                        {
                            if (adresse != "" && couriel != "" && nom != "" && prenom != "" && tel > 500000000)
                            {
                                string no = Individu.NextID();
                                Individu new_ind = new Individu(no, nom, adresse, couriel, tel, prenom);
                                new_ind.Ajout();
                                if(box_programme.SelectedIndex > -1)
                                {
                                    string req = $"insert into adhere values ('{no}',{box_programme.SelectedIndex+1},'{DateTime.Now.ToString("yyyy-MM-dd")}');";
                                    Controle.Requete(req, false);
                                }
                            }
                            break;
                        }
                    case "supprimer":
                        {
                            Individu current = (Individu)listview_indiv.SelectedItem;
                            if (current != null)
                                current.Suppression();
                            break;
                        }
                    case "modifier":
                        {
                            Individu current = (Individu)listview_indiv.SelectedItem;
                            if (prenom != "" && adresse != "" && couriel != "" && nom != "" && tel > 500000000 && current != null)
                            {
                                current.Modif("nom_i", nom);
                                current.Modif("prenom_i", prenom);
                                current.Modif("tel_i", tel.ToString());
                                current.Modif("couriel_i", couriel);
                                current.Modif("adresse_i", adresse);
                                if (box_programme.SelectedIndex > -1)
                                {
                                    string req = $"select no_programme from adhere where no_i = '{current.Noclient}';";
                                    MySqlDataReader reader = Controle.Requete(req, true);
                                    if (!reader.Read())
                                        req = $"insert into adhere values ('{current.Noclient}',{box_programme.SelectedIndex + 1},'{DateTime.Now.ToString("yyyy-MM-dd")}');";
                                    else
                                        req = $"update adhere set no_programme = {box_programme.SelectedIndex + 1} where no_i = '{current.Noclient}';";

                                    Controle.Requete(req, false);
                                }
                                MessageBox.Show("Mis à jour !");
                            }
                                
                            break;
                        }
                }
                update_listclient();

            }
        }

        private void BoutiqueValidation_Click(object sender, RoutedEventArgs e)
        {
            string choix = del_bout.IsChecked == true ? "supprimer" : mod_bout.IsChecked == true ? "modifier" : add_bout.IsChecked == true ? "ajout" : "";
            string adresse = box_boutique_adresse.Text;
            string couriel = box_boutique_couriel.Text;
            string nom = box_boutique_nom.Text;
            string contact = box_boutique_contact.Text;
            int.TryParse(box_boutique_tel.Text, out int tel);
            double.TryParse(box_boutique_remise.Text, out double remise);

            if (choix != "")
            {
                switch (choix)
                {
                    case "ajout":
                        {
                            if (adresse != "" && couriel != "" && nom != "" && contact != "" && remise > 0.0 && tel > 500000000)
                            {
                                Boutique new_bout = new Boutique(Boutique.NextID(), nom, adresse, couriel, tel, contact, remise);
                                new_bout.Ajout();
                            }
                            break;
                        }
                    case "supprimer":
                        {
                            Boutique current = (Boutique)listview_boutique.SelectedItem;
                            if (current != null)
                                current.Suppression();
                            break;
                        }
                    case "modifier":
                        {
                            Boutique current = (Boutique)listview_boutique.SelectedItem;
                            if (adresse != "" && couriel != "" && nom != "" && contact != "" && remise > 0.0 && tel > 500000000 && current != null)
                            {
                                current.Modif("nom_b", nom);
                                current.Modif("contact_b", contact);
                                current.Modif("tel_b", tel.ToString());
                                current.Modif("couriel_b", couriel);
                                current.Modif("adresse_b", adresse);
                                current.Modif("remise", remise.ToString().Replace(',','.'));
                            }

                            break;
                        }
                }
                update_listboutique();

            }
        }
    }
}
