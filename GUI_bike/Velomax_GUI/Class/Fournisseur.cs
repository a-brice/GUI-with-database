using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Velomax_GUI
{
    public class Fournisseur : IElement
    {
        double siret;
        string nom;
        string contact;
        string adresse;
        int libelle;

        public Fournisseur(double siret, string nom, string contact, string adresse, int libelle)
        {
            this.siret = siret;
            this.nom = nom;
            this.contact = contact;
            this.adresse = adresse;
            this.libelle = libelle;
        }

        public Fournisseur() { }

        #region Accesseur

        public double Siret
        {
            get { return siret; }
            set { siret = value; }
        }
        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }
        public string Contact
        {
            get { return contact; }
            set { contact = value; }
        }
        public string Adresse
        {
            get { return adresse; }
            set { adresse = value; }
        }
        public int Libelle
        {
            get { return libelle; }
            set { libelle = value; }
        }
        #endregion

        #region Pour export de piece
        public int Delai { get; set; }
        public double Cout { get; set; }
        public int Stock { get; set; }
        #endregion
        public override string ToString()
        {
            return siret + " " + nom;
        }

        public void Ajout()
        {
            string req = $"insert into fournisseur values ({siret},'{nom}','{contact}','{adresse}',{libelle}); ";
            Controle.Requete(req, false);
        }

        public void Suppression()
        {
            string req = $"delete from fournisseur where siret =  {siret} ; ";
            Controle.Requete(req, false);
            req = $"delete from delivrer where siret =  {siret} ; ";
            Controle.Requete(req, false);
        }


        public void Modif(string attribut, string val)
        {
            string req = $"update fournisseur set {attribut} = '{val}' where siret = {siret}; ";
            Controle.Requete(req, false);
        }

        public void LivrePiece(string no_p, string numpiece, int delai, double prixf, int quantite)
        {

            string req = $"select count(num_piece) as nb from delivrer where siret = {this.siret} and no_p = '{no_p}'; ";
            MySqlDataReader reader = Controle.Requete(req, true);
            if(reader.Read())
            if (reader.GetInt64(0) == 0)
            {
                req = $"insert into delivrer values ('{no_p}',{this.siret},{delai},{prixf},'{numpiece}', {quantite});";
                Controle.Requete(req, false);
            }
            else
            {
                req = $"update delivrer set stock = {quantite}, num_piece = '{numpiece}', delai = {delai}, cout = {prixf} where no_p = '{no_p}' and siret = {this.siret};";
                Controle.Requete(req, false);
            }
        }
    }
}
