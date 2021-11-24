
namespace Balonmano_Manager_App.Comandos
{

    /**
     * Interfaz de Comando Ejecutable
     */
    public interface ICommandExecutable : ICommand
    {

        /**
         * Ejecuta las acciones asociadas el evento representado.
         */
        void Execute();

        /**
         * Deshace las acciones, ejecutadas previamente, asociadas al evento.
         */
        void Undo();

        /**
         * Momento del partido en que ha tenido lugar el evento.
         */
        Momento Momento { get; set; }

    }
}
