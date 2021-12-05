using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Velomax_GUI
{
    public class Commande : IElement
    {
        string nocommande;
        string noclient;
        DateTime dateC;
        DateTime dateL;

        public Commande(string nocommande, string noclient, DateTime dateC, DateTime dateL)
        {
            this.nocommande = nocommande;
            this.noclient = noclient;
            this.dateC = dateC;
            this.dateL = dateL;
        }

        public Commande() { }

        #region Accesseur
        public string NoC
        {
            get { return nocommande; }
            set { nocommande = value; }
        }
        public string Noclient
        {
            get { return noclient; }
            set { noclient = value; }
        }
        public string DateC
        {
            get { return dateC.ToShortDateString(); }
        }
        public string DateL
        {
            get { return dateL.ToShortDateString(); }
        }

        #endregion

        public List<Equipement> Equipements_Commande { get; set; }

        public static string NextID()
        {
            string req = "select max(convert(SUBSTRING_INDEX(no_c,'COM',-1), signed integer)) as id from commande;";
            MySqlDataReader reader = Controle.Requete(req, true);
            reader.Read();
            string id = "COM" + (Convert.ToInt32(reader["id"]) + 1);
            return id;
        }

        public void Ajout()
        {
            string req = $"insert into commande values ('{nocommande}','{noclient}','{dateC.ToString("yyyy-MM-dd")}','{dateL.ToString("yyyy-MM-dd")}'); ";
            Controle.Requete(req, false);
            
        }

        public void Suppression()
        {
            string req = $"delete from commande where no_c = '{nocommande}' ; ";
            Controle.Requete(req, false);
        }


        public void Modif(string attribut, string val)
        {
            string req = $"update commande set {attribut} = '{val}' where no_c = '{nocommande}'; ";
            Controle.Requete(req, false);

        }

        public void AjoutElement(string num_e, int quantite , string taille, bool piece)
        {
            string req = "";
            if (piece)
                req = $"select sum(stock) stock from delivrer where no_p = '{num_e}';";
            else
                req = $"select stock from grandeur where label = '{taille}' and no_m = '{num_e}'; ";
            MySqlDataReader reader = Controle.Requete(req, true);
            int stock = 0;
            if (reader.Read())
            {
                stock = reader.GetInt32("stock");
            } else
            {
                MessageBox.Show("Produit inexistant !");
            }
            if (quantite > stock) MessageBox.Show("Il n'y a pas assez de stock !");
            else
            {
                if (stock - quantite < 3) MessageBox.Show("Attention ! Stock bientot épuisé !");

                req = $"select count(quantite) from compose where no_c = '{this.nocommande}' and no_equipement = '{num_e}'; ";
                reader = Controle.Requete(req, true);

                if (reader.Read())
                    if (reader.GetInt64(0) == 0)
                    {
                        req = $"insert into compose values ('{this.nocommande}','{num_e}',{quantite});";
                        Controle.Requete(req, false);
                        if (!piece)
                        {
                            req = $"update grandeur set stock = stock - {quantite} where no_m = '{num_e}' and label = '{taille}';";
                            Controle.Requete(req, false);
                        }
                    }
                    else
                    {

                        req = $"update compose set quantite = {quantite} where no_equipement = '{num_e}' and no_c = '{this.nocommande}';";
                        Controle.Requete(req, false);
                        if (!piece)
                        {
                            req = $"update grandeur set stock = stock - {quantite} where no_m = '{num_e}' and label = '{taille}';";
                            Controle.Requete(req, false);
                        }
                    }

            }
        }

        public void SupElement(Equipement e)
        {
            string req = $"delete from compose where no_c = '{this.nocommande}' and no_equipement = '{e.Noequipement}'; ";
            Controle.Requete(req, false);
        }
    }
}
