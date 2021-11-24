using System.Collections.Generic;

namespace Futbol_Manager_App.Beans
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
            int ordenPosicion = getOrdenPosicion(a.Posicion) - getOrdenPosicion(b.Posicion);

            if (ordenPosicion == 0)
            {
                return a.Number - b.Number;
            }
            else
            {
                return ordenPosicion;
            }
        }


        private int getOrdenPosicion(int puesto)
        {
            if (puesto == Jugador.Portero)
            {
                return 1;
            }
            else if (puesto == Jugador.Defensa)
            {
                return 2;
            }
            else if (puesto == Jugador.Centrocampista)
            {
                return 3;
            }
            else
            { // Delantero
                return 4;
            }
        }

    }
}
