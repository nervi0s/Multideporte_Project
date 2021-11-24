using Balonmano_Manager_App.Beans;
using System.Drawing;

namespace Balonmano_Manager_App.Comandos
{

    /**
     * Interfaz de Comando
     * Se trata de una interfaz genérica que aglutina a todos los comandos de diferetes características.
     * Los comandos concretos implementarán ICommandShowable, ICommandExecutable o ambos.
     */
    public interface ICommand
    {
        /**
         * Color representativo
         */
        Color GetColor();

        /**
         * Representación en texto
         */
        string ToString();

        Jugador GetJugador();

        string getNameCommand();

    }
}
