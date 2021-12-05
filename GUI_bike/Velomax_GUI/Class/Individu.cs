using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    public class Individu : Client
    {
        string prenom;

        public Individu(string noclient, string nom, string adresse, string couriel, int num, string prenom) : base(noclient,nom,adresse,couriel,num)
        {
            this.prenom = prenom;
        }

        public Individu() { }

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }

        public List<Fidelio> Adhesions { get; set; }

        public override string ToString()
        {
            return "Prénom : " + prenom + "\n" + base.ToString();
        }

        public static string NextID()
        {
            string req = "select max(convert(SUBSTRING_INDEX(no_i,'I',-1), signed integer)) as id from individu;";
            MySqlDataReader reader = Controle.Requete(req, true);
            reader.Read();
            string id = "I" + (Convert.ToInt32(reader["id"]) + 1);
            return id;
        } 

        public override void Ajout()
        {
            string req = $"insert into individu values ('{noclient}','{nom}','{prenom}','{adresse}',{num},'{couriel}'); ";
            Controle.Requete(req, false);
        }

        public override void Suppression()
        {
            string req = $"delete from individu where no_i =  '{noclient}' ; ";
            Controle.Requete(req, false);
            req = $"delete from commande where no_client =  '{noclient}' ; ";
            Controle.Requete(req, false);
        }

        
        public override void Modif(string attribut, string val)
        {
            string req = $"update individu set {attribut} = '{val}' where no_i = '{noclient}'; ";
            Controle.Requete(req, false);
        }


        public void Adherance(int programme, int duree)
        {
            string req = $"select no_i from adhere where no_i = '{noclient}' ;";
            MySqlDataReader reader = Controle.Requete(req, true);
            
            if (!reader.Read())
               req = $"insert into adhere values ('{noclient}',{programme},{duree});";

            else
               req = $"update adhere set no_programme = '{programme}', duree = '{duree}' where no_i = '{noclient}';";

            Controle.Requete(req, false);
        }
        
    }
}
