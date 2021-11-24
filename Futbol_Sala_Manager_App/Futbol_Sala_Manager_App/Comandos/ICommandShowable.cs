using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{

    /**
     * Interfaz de Comando Visualizable
     */
    public interface ICommandShowable : ICommand
    {

        /** 
         * Muestra el comando mediante el IPF
         * El comando se muestra en pasos sucesivos. Mientras
         * queden pasos por mostrar devuelve true.
         * Utiliza idioma para los textos.
         */
       // bool Show(InterfaceIPF ipf, IdiomaData idioma);

        bool Show(InterfaceIPF[] _ipfs, IdiomaData[] _idioma, int n);
        // n es el numero de ipfs

        /**
         * Reestablece el estado actual al inicial
         * Se utiliza para reiniciar un comando que se encuentra
         * en un estado intermedio de dibujado.
         */
        void Reset();

    }
}
