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
using Newtonsoft.Json;
using MySql.Data.MySqlClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour page_export.xaml
    /// </summary>
    public partial class page_export : Page
    {
        Brush global;
        public page_export()
        {
            InitializeComponent();
             global = btn1.Background;
        }

        private void change_color(object sender, MouseEventArgs e)
        {
            Border button = (Border)sender;
            if (button.IsMouseOver)
                button.Background = Brushes.Azure;
            else button.Background = global;
        }

        
        public void serializetoJSON<T>(string file, List<T> lst)
        {
            StreamWriter sr = new StreamWriter("JSON_Export/" + file);            JsonTextWriter jsonWriter = new JsonTextWriter(sr);

            JsonSerializer serializer = new JsonSerializer();            serializer.Serialize(jsonWriter, lst);

            jsonWriter.Close();            sr.Close();
            MessageBox.Show("Table exportée avec succès ! A trouver dans le bin/debug");

        }

        public void serializetoXML<T>(string file, List<T> lst)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<T>)); 
            StreamWriter sr = new StreamWriter("XML_Export/" + file); 
            xs.Serialize(sr, lst);
            sr.Close();
        }


        private void export_indiv(object sender, MouseButtonEventArgs e)
        {
            string req = "select * from individu;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Individu> lsti = new List<Individu>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Individu i = new Individu((string)reader["no_i"], reader["nom_i"].ToString(), reader["adresse_i"].ToString(),
                        reader["couriel_i"].ToString(), (int)reader["tel_i"], reader["prenom_i"].ToString());
                    lsti.Add(i);
                }

            }

            serializetoJSON<Individu>("individu.json", lsti);
            serializetoXML<Individu>("individu.xml", lsti);
        }

        private void export_fournisseur(object sender, MouseButtonEventArgs e)
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
            serializetoJSON<Fournisseur>("fournisseur.json", lstf);
            serializetoXML<Fournisseur>("fournisseur.xml", lstf);
        }

        private void export_modele(object sender, MouseButtonEventArgs e)
        {
            string req = "select * from modele;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Modele> lstm = new List<Modele>();
            while (reader.Read())
            {
                Modele m = new Modele((string)reader["no_m"], (string)reader["nom_m"], (double)reader["prix_m"],
                    (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["categorie"] );
                lstm.Add(m);
            }
            serializetoJSON<Modele>("modele.json", lstm);
            serializetoXML<Modele>("modele.xml", lstm);
        }

        private void export_boutique(object sender, MouseButtonEventArgs e)
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
            serializetoJSON<Boutique>("boutique.json", lstb);
            serializetoXML<Boutique>("boutique.xml", lstb);
        }

        private List<Equipement> get_composition_commande(Commande c)
        {
            List<Equipement> lste = new List<Equipement>();
            if (c != null)
            {
                string req = $"select m.* from compose join modele m on m.no_m = no_equipement where no_c ='{c.NoC}';";

                MySqlDataReader reader = Controle.Requete(req, true);
                if(reader != null)
                {
                    while (reader.Read())
                    {
                        lste.Add(new Modele((string)reader["no_m"], (string)reader["nom_m"], (double)reader["prix_m"],
                        (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["categorie"]));
                    }
                    reader.Close();

                }

                req = $"select p.* from compose join piece p on p.no_p = no_equipement where no_c = '{c.NoC}';";
                reader = Controle.Requete(req, true);
                if(reader != null)
                {
                    while (reader.Read())
                    {
                        lste.Add(new Piece((string)reader["no_p"], (string)reader["nom_p"], (double)reader["prix"],
                        (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["description"]));
                    }
                }
                
            }

            return lste;
        }

        private void export_commande(object sender, MouseButtonEventArgs e)
        {
            string req = "select * from commande limit 30;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Commande> lstc = new List<Commande>();

            while (reader.Read())
            {
                lstc.Add(
                new Commande((string)reader["no_c"], (string)reader["no_client"], (DateTime)reader["date_c"], (DateTime)reader["date_l"]));
            }
            lstc.ForEach(x => x.Equipements_Commande = get_composition_commande(x));
            serializetoJSON<Commande>("commande.json", lstc);
            //serializetoXML<Commande>("commande.xml", lstc);
        }

        private void export_piece(object sender, MouseButtonEventArgs e)
        {
            string req = $"select * from piece;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Piece> lstp = new List<Piece>();
            if (reader != null)
            {
                while (reader.Read())
                {
                    lstp.Add(new Piece((string)reader["no_p"], (string)reader["nom_p"], (double)reader["prix"],
                    (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["description"]));
                }
            }
            serializetoJSON<Piece>("piece.json", lstp);
            serializetoXML<Piece>("piece.xml", lstp);
        }

        private void export_indiv_expiration(object sender, MouseButtonEventArgs e)
        {
            string req = "select i.* from individu i natural join adhere natural join programme " +
                "where DATEDIFF(DATE_ADD(date_debut, interval duree_p year), now()) < 4*30; "; // 4 mois
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Individu> lsti = new List<Individu>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Individu i = new Individu((string)reader["no_i"], reader["nom_i"].ToString(), reader["adresse_i"].ToString(),
                        reader["couriel_i"].ToString(), (int)reader["tel_i"], reader["prenom_i"].ToString());
                    lsti.Add(i);
                }
                lsti.ForEach(x => x.Adhesions = get_all_historic(x));

            }

            serializetoXML<Individu>("particulier_adhesion_expiration.xml", lsti);
            MessageBox.Show("Table exportée avec succès ! A trouver dans le bin/debug");

        }

        public List<Fidelio> get_all_historic(Individu i)
        {
            string req = $"select * from adhere natural join programme where no_i = '{i.Noclient}';";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Fidelio> lstf = new List<Fidelio>();

            if (reader != null)
            {
                while (reader.Read())
                {
                    Fidelio f = new Fidelio((int)reader["no_programme"], (double)reader["cout"], reader["description"].ToString(),
                        (double)reader["rabais"], (DateTime)reader["date_debut"], (int)reader["duree_p"]);
                    lstf.Add(f);
                }

            }
            return lstf;
        }


        private List<Fournisseur> get_all_fournisseur(string no_p)
        {
            string req = $"select * from delivrer natural join fournisseur where no_p = '{no_p}';";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Fournisseur> lstf = new List<Fournisseur>();

            while (reader.Read())
            {
                lstf.Add(
                    new Fournisseur((double)reader["siret"], (string)reader["nom_f"], (string)reader["contact"], (string)reader["adresse_f"], reader.GetInt32("libelle"))
                    { Delai = (int)reader["delai"], Cout = (double)reader["cout"], Stock = (int)reader["stock"] });
            }
            return lstf;
        }

        private void export_stock_faible(object sender, MouseButtonEventArgs e)
        {
            string req = "select p.* from piece p natural join delivrer group by no_p having sum(stock) < 40;";
            MySqlDataReader reader = Controle.Requete(req, true);
            List<Piece> lstc = new List<Piece>();

            while (reader.Read())
            {
                lstc.Add(
                new Piece((string)reader["no_p"], (string)reader["nom_p"], reader.GetDouble("prix"),
                (DateTime)reader["date_debut"], (DateTime)reader["date_fin"], (string)reader["description"]));
            }
            lstc.ForEach(x => x.Fournisseurs = get_all_fournisseur(x.Noequipement));

            serializetoJSON<Piece>("stock_faible.json", lstc);
        }

    }

    
    
}
