using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velomax_GUI
{
    interface IElement
    {
        void Ajout();
        void Suppression();
        void Modif(string attribut, string valeur);
    }
}
