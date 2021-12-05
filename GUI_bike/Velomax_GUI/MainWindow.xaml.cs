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

namespace Velomax_GUI
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void login(object sender, MouseButtonEventArgs e)
        {
            if ((box_uid.Text == "root" && box_password.Password == "root") || (box_uid.Text == "bozo" && box_password.Password == "bozo"))
            {
                Controle.uid = box_uid.Text;
                Controle.password = box_password.Password;
                new Window_Accueil().Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Incoherent user !");
            }
        }

        private void change_color(object sender, MouseEventArgs e)
        {
            Border b = (Border)sender;
            if (b.IsMouseOver)
                b.Background = Brushes.CadetBlue;
            else b.Background = Brushes.Transparent;
        }
    }
}
