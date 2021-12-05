using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Velomax_GUI
{
    public abstract class Equipement
    {
        protected string noequipement;
        protected string nom;
        protected double prix;
        protected DateTime datedebut;
        protected DateTime datefin;

        public Equipement(string noequipement, string nom, double prix, DateTime datedebut, DateTime datefin)
        {
            this.nom = nom;
            this.noequipement = noequipement;
            this.prix = prix;
            this.datedebut = datedebut;
            this.datefin = datefin;
        }

        public Equipement() { }
        #region Acceusseur
        public string Noequipement
        {
            get { return noequipement; }
            set { noequipement = value; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public double Prix
        {
            get { return prix; }
            set { prix = value; }
        }
        public DateTime Datedebut
        {
            get { return datedebut; }
            set { datedebut = value; }
        }
        public DateTime Datefin
        {
            get { return datefin; }
            set { datefin = value; }
        }
        

        #endregion



        public abstract void Ajout();
        public abstract void Suppression();
        public abstract void Modif(string attribut, string valeur);
    }
}
