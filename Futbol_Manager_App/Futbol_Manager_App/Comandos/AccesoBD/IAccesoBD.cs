using System.Collections.Generic;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.AccesoBD
{
    /**
     * Interfaz de Acceso a la Base de Datos
     */
    public interface IAccesoBD
    {
        /**
         * Testea la conexión a la base de datos
         * Devuelve true si es posible conectar con la base de datos
         */
        bool TestConexion();

        /**
         * Abre la conexión
         * Devuelve true si la conexión se ha abirto con éxito
         */
        bool AbreConexion();

        /**
         * Cierra la conexión
         */
        void CierraConexion();

        /**
         * Lista de encuentros
         * Recopilación del id y descripción de todos los encuentros
         */
        List<Encuentro> ListaEncuentros();

        /**
         * Datos de un encuentro
         * Recupera todos los datos relacionados a un encuentro
         */
        EncuentroData DatosEncuentro(int idEncuentro);
    }
}
