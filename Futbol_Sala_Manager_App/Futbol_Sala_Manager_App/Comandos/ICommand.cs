using System.Drawing;

namespace Futbol_Sala_Manager_App.Comandos
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

    }
}
