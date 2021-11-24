using System.Collections.Generic;

namespace Futbol_Sala_Manager_App.Beans
{

    /**
     * Comparador de objetos Jugador para el GUI
     * Compara los jugadores en base a su posición de juego. Posiciona al
     * portero en primer lugar, los defensas en segundo, los centrocampistas
     * en tercero y a los delanteros los últimos.
     */
    public class JugadorComparerGUI : IComparer<Jugador>
    {

        public int Compare(Jugador a, Jugador b)
        {
            return a.Number.CompareTo(b.Number);
        }

    }
}
