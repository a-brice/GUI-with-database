using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velomax_GUI
{
    public abstract class Client : IElement
    {
        protected string noclient,
                         nom,
                         adresse,
                         couriel;
        protected int num;

        protected Client(string noclient, string nom, string adresse, string couriel, int num)
        {
            this.noclient = noclient;
            this.nom = nom;
            this.adresse = adresse;
            this.couriel = couriel;
            this.num = num;
        }

        protected Client() { }

        #region Accesseur
        public string Noclient
        {
            get { return noclient; }
            set { noclient = value;  }
        }
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public string Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }
        public string Couriel
        {
            get { return couriel; }
            set { couriel = value; }
        }
        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        public string Tel
        {
            get { return "0" + num; }
        }
        #endregion

        public override string ToString()
        {
            string phrase = $"Nom : {nom}";
            phrase += $"\nAdresse : {adresse}";
            phrase += $"\nCouriel : {couriel}";
            phrase += $"\nNum : {"0" + num.ToString()}";
            return phrase;
        }

        public abstract void Ajout();
        public abstract void Suppression();
        public abstract void Modif(string attribut, string valeur);
    }
}
