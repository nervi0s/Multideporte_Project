using System.Collections.Generic;

namespace Balonmano_Manager_App.Comandos
{

    /**
     * Comparador de objetos ICommand
     * Compara el Momento de los dos objetos. Sirve para ordenar en base al 
     * momento, situándose los más recientes en primer lugar.
     */
    public class ICommandComparer : IComparer<ICommand>
    {
        public int Compare(ICommand a, ICommand b)
        {
            int orden;

            try
            {
                Momento ma = ((ICommandExecutable)a).Momento;
                Momento mb = ((ICommandExecutable)b).Momento;

                int ordenParte = mb.Parte - ma.Parte;

                if (ordenParte == 0)
                {
                    orden = mb.SegundoAbsoluto - ma.SegundoAbsoluto;
                }
                else
                {
                    orden = ordenParte;
                }
            }
            catch // Si no son ICommandExecutable no tienen Momento por lo que tienen el mismo orden
            {
                orden = 0;
            }

            return orden;
        }

    }
}
