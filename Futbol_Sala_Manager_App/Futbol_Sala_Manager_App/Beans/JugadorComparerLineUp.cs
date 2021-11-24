using System.Collections.Generic;

namespace Futbol_Sala_Manager_App.Beans
{

    /**
     * Comparador de objetos Jugador para el LineUp
     * Compara los jugadores en base a su posición de juego. Posiciona al
     * portero en primer lugar, y los demás jugadores después.
     */
    public class JugadorComparerLineUp : IComparer<Jugador>
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
            else
            {
                return 2;
            }
        }

    }
}
