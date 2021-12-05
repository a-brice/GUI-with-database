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

namespace ProjetWPFV2
{
    /// <summary>
    /// Logique d'interaction pour PageStatistiques.xaml
    /// </summary>
    public partial class PageStatistiques : Page
    {
        public Pizzeria pizzeria;
        public PageStatistiques(Pizzeria pizzeria)
        {
            InitializeComponent();
            this.pizzeria = pizzeria;
            fillliste();
            fillliste2();
        }

        public void fillliste()
        {
            List<Commande> lstc = pizzeria.LstCommande;
            List<Personne> lstp = pizzeria.LstPersonne<Commis>();
            int i = 0;

            List<int> lstnb = new List<int>();

            lstc.Sort((x, y) => x.NomCommis.CompareTo(y.NomCommis));
            lstp.ForEach(delegate (Personne pers)
            {
                i = 0;

                lstc.ForEach(delegate (Commande com)
                {
                    if (pers.Nom == com.NomCommis) i++;

                });

                lstnb.Add(i);
            });

            lstboxCommis.ItemsSource = lstp;
            lstboxnbCommis.ItemsSource = lstnb;

        }
        public void fillliste2()
        {
            List<Commande> lstc = pizzeria.LstCommande;
            List<Personne> lstp = pizzeria.LstPersonne<Livreur>();
            int i = 0;

            List<int> lstnb = new List<int>();

            lstc.Sort((x, y) => x.NomLivreur.CompareTo(y.NomLivreur));

            lstp.ForEach(delegate (Personne pers)
            {
                i = 0;

                lstc.ForEach(delegate (Commande com)
                {
                    if (pers.Nom == com.NomLivreur) i++;

                });

                lstnb.Add(i);
            });

            lstboxLivreur.ItemsSource = lstp;
            lstboxnbLivreur.ItemsSource = lstnb;
        }

        private void Btngoback_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage(pizzeria));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            double somme = 0;
            pizzeria.LstCommande.ForEach(x => somme += x.Prix());
            lbl3.Content = somme / pizzeria.LstCommande.Count + " €";

        }
    }
}
