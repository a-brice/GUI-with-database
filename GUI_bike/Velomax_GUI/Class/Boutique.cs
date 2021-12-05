using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    public class Boutique : Client
    {
        string contact;
        double remise;

        public Boutique(string noclient, string nom, string adresse, string couriel, int num, string contact, double remise) : base(noclient, nom, adresse, couriel, num)
        {
            this.contact = contact;
            this.remise = remise;
        }

        public Boutique() { }

        public string Contact
        {
            get { return contact; }
            set { contact = value; }
        }

        public double Remise
        {
            get { return remise; }
            set { remise = value; }
        }
        public static string NextID()
        {
            string req = "select max(convert(SUBSTRING_INDEX(no_b,'B',-1), signed integer)) as id from boutique;";
            MySqlDataReader reader = Controle.Requete(req, true);
            reader.Read();
            string id = "B" + (Convert.ToInt32(reader["id"]) + 1);
            return id;
        }

        public override void Ajout()
        {
            string req = $"insert into boutique values ('{noclient}','{nom}','{adresse}',{num},'{couriel}','{contact}','{remise.ToString().Replace(",", ".")}'); ";
            Controle.Requete(req, false);
        }

        public override void Suppression()
        {
            string req = $"delete from boutique where no_b =  '{noclient}' ; ";
            Controle.Requete(req, false);
        }


        public override void Modif(string attribut, string val)
        {
            string req = $"update boutique set {attribut} = '{val}' where no_b = '{noclient}'; ";
            Controle.Requete(req, false);
        }

        public override string ToString()
        {
            return base.ToString() + "\nContact : " + contact;
        }
    }
}
