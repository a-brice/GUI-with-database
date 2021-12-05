using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    public class Piece : Equipement
    {
        string description;

        public Piece(string noequipement, string nom, double prix, DateTime datedebut, DateTime datefin, string description) : base(noequipement, nom, prix, datedebut, datefin)
        {
            this.description = description;
        }

        public Piece() { }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public List<Fournisseur> Fournisseurs { get; set; }


        public override void Ajout()
        {
            string req = $"insert into piece values ('{noequipement}','{nom}',{prix.ToString().Replace(',', '.')},'{description}','{datedebut.ToString("yyyy-MM-dd")}','{datefin.ToString("yyyy-MM-dd")}'); ";
            Controle.Requete(req, false);
            
        }

        public override void Suppression()
        {
            string req = $"delete from piece where no_p =  '{noequipement}' ; ";
            Controle.Requete(req, false);
        }


        public override void Modif(string attribut, string val)
        {
            string req = $"update piece set {attribut} = '{val}' where no_p = '{noequipement}'; ";
            Controle.Requete(req, false);
        }

    }
}
