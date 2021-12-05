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
    /// Logique d'interaction pour page_accueil.xaml
    /// </summary>
    public partial class page_accueil : Page
    {
        public page_accueil()
        {
            InitializeComponent();
            lblconnect.Content = Controle.uid;
        }

        private void change_color(object sender, MouseEventArgs e)
        {
            if (button.IsMouseOver)
                button.Background = Brushes.Azure;
            else button.Background = Brushes.Transparent;
        }
    }
}
