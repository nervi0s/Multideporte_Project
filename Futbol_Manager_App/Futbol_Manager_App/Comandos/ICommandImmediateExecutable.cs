using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public interface ICommandImmediateExecutable : ICommand
    {

        /**
         * Ejecuta las acciones asociadas el evento representado nada mas producirse
         */
        void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n);

        /**
         * Deshace las acciones, ejecutadas previamente, asociadas al evento nada mas producirse.
         */
        void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n);

    }
}
