using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    class Controle
    {
        static string UID = "root";
        static string PSWD = "root";
        public static string uid
        {
            get { return UID; }
            set { UID = value; }
        }
        public static string password
        {
            get { return PSWD; }
            set { PSWD = value; }
        }
        public static MySqlConnection Connect()
        {
            try
            {
                string bdd = "SERVER=localhost;PORT=3306;DATABASE=Velomax;";
                bdd += $"UID={UID};PASSWORD={PSWD}";
                MySqlConnection connexion = new MySqlConnection(bdd);
                connexion.Open();
                return connexion;
            }
            catch (MySqlException) {  }

            return null;
        }

        public static MySqlDataReader Requete(string req,bool query)
        {

            MySqlCommand comm = new MySqlCommand(req, Connect());
            MySqlDataReader reader = null;
            try
            {
                if (query) reader = comm.ExecuteReader();
                else comm.ExecuteNonQuery();
            }
            catch (Exception e) { MessageBox.Show("Impossible d'instancier la connexion à la base de données !\n\nMessage d'erreur : " + e.Message); }
            
            return reader;
        }
        

        
        
    }
}
