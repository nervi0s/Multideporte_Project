
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public interface ICommandImmediateExecutable : ICommand
    {
        /**
         * Ejecuata las acciones asociadas al evento representado inmediatamente
         */
        void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n);
        /**
         * Deshace las acciones asociadas al evento representado inmediatamente
         */
        void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n);

    }
}
