using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    public class Modele : Equipement
    {
        string categorie;
        public Modele(string noequipement, string nom, double prix, DateTime datedebut, DateTime datefin, string cate): base(noequipement, nom, prix, datedebut, datefin)
        {
            categorie = cate;
        }

        public Modele()
        {

        }

        public string Categorie
        {
            get { return categorie; }
            set { categorie = value; }
        }
        public string Grandeur { get; set; }
        public int Stock { get; set; }

        public static int NextID()
        {
            string req = "select max(no_m) as id from modele;";
            MySqlDataReader reader = Controle.Requete(req, true);
            reader.Read();
            int id = Convert.ToInt32(reader["id"]) + 1;
            return id;
        }


        public override void Ajout()
        {
            string req = $"insert into modele values ('{noequipement}','{nom}',{prix},'{categorie}','{datedebut.ToString("yyyy-MM-dd")}','{datefin.ToString("yyyy-MM-dd")}'); ";
            Controle.Requete(req, false);
        }

        public override void Suppression()
        {
            string req = $"delete from modele where no_m = '{noequipement}'; ";
            Controle.Requete(req, false);
        }


        public override void Modif(string attribut, string val)
        {
            string req = $"update modele set {attribut} = '{val}' where no_m = '{noequipement}'; ";
            Controle.Requete(req, false);
        }

        public void AjoutPiece(Piece p, string grandeur)
        {
            string req = $"select grandeur from associe where no_m = '{this.noequipement}' and no_p = '{p.Noequipement}'; ";
            MySqlDataReader reader = Controle.Requete(req, true);
            
            if (!reader.Read())
            {
                req = $"insert into associe values ('{this.noequipement}','{p.Noequipement}','{grandeur}');";
                Controle.Requete(req, false);
            }
                
        }

        public void SupPiece(Piece p, string grandeur)
        {
            string req = $"select grandeur from associe where no_m = {this.noequipement} and no_p = {p.Noequipement}; ";
            MySqlDataReader reader = Controle.Requete(req, false);
            reader.Read();
            if (reader["grandeur"] != null)
            {
                reader.Close();
                req = $"delete from associe where no_m =  '{this.noequipement}' and no_p = '{p.Noequipement}' and grandeur = '{grandeur}' ;";
                Controle.Requete(req, false);
            }

        }
    }
}
