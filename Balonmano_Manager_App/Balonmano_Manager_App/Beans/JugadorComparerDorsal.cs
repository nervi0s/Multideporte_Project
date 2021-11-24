using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Balonmano_Manager_App.Beans
{
    class JugadorComparerDorsal : IComparer<Jugador>
    {
        public int Compare(Jugador a, Jugador b)
        {
            return a.Number - b.Number;
        }
    }
}
