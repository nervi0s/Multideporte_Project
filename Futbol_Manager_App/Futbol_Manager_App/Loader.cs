using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Futbol_Manager_App.AccesoBD;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Interfaz;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App
{
    /**
     * Controlador del Loader
     */
    public class Loader
    {
        private LoaderForm _gui;
        private IAccesoBD _bd;


        /**
         * Constructor
         * Recibe la instancia del Interfaz y si se deben emplear o no datos de prueba
         */
        public Loader(LoaderForm gui, bool dummyDB)
        {
            _gui = gui;

            // Si la configuraci es correcta se cargan los datos y se muestran
            if (checkConfiguracion())
            {

                // Se utilizan datos de prueba
                if (dummyDB)
                {
                    _bd = new DummyData();
                }
                else // Se usa la conexión a la base de datos
                {
                    _bd = new AccessData(PersistenciaUtil.CargaConfig().BaseDatos);
                }

                cargaListaPartidos();
                cargaBackup();
            }
            else // Si existe algún problema de configuración se cierra
            {
                _gui.Close();
            }
        }
        

        /**
         * Abre el formulario de Configuración
         */
        public void AbrirConfiguracion()
        {
            new ConfigForm().ShowDialog(_gui);
            cargaListaPartidos();
        }

        /**
         * Carga el encuentro indicado
         */
        public void CargaEncuentro(int id)
        {
            _gui.Hide();

            _bd.AbreConexion();
            EncuentroData datos = _bd.DatosEncuentro(id);
            _bd.CierraConexion();

            new Controlador(datos);
        }

        /**
         * Carga el encuentro de Backup
         */
        public void CargaEncuentroBackup()
        {
            _gui.Hide();

            EncuentroData datos = PersistenciaUtil.CargaBackup();

            new Controlador(datos);
        }



        // ****************************** PRIVADOS *****************************

        // Carga la lista de partidos en el formulario
        private void cargaListaPartidos()
        {
            List<Encuentro> encuentros;
            try
            {
                _bd.AbreConexion();
                encuentros = _bd.ListaEncuentros();
                _bd.CierraConexion();
            }
            catch
            {
                encuentros = null;
            }

            _gui.CargaListaPartidos(encuentros);
        }

        // Carga el backup en el formulario
        private void cargaBackup()
        {
            EncuentroData datos = PersistenciaUtil.CargaBackup();

            _gui.CargaBackup(datos);
        }

        // Comprueba que exista al menos un fichero de Idioma, que exista configuración y que el idioma elegido exista
        private bool checkConfiguracion()
        {
            // Comprueba que exista al menos un fichero de idioma
            FileInfo[] idiomas = PersistenciaUtil.GetListaFicherosIdioma();
            if (idiomas.Length == 0)
            {
                MessageBox.Show("No se han encontrado ficheros de idioma. Los ficheros (*.xml) se deben encontrar en el mismo directorio que el ejecutable.", "Fichero requerido",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }

            // Comprueba que exista fichero de configuración
            if (PersistenciaUtil.CargaConfig() == null)
            {
                new ConfigForm().ShowDialog(_gui);
                // Vuelve a comprobar para ver si se ha corregido
                if (PersistenciaUtil.CargaConfig() == null)
                    return false;
            }

            // Comprueba que el Fichero de Idioma exista
            if (PersistenciaUtil.CargaIdioma(@"futbol\" + PersistenciaUtil.CargaConfig().IdiomaFichero) == null)
            {
                new ConfigForm().ShowDialog(_gui);
                // Vuelve a comprobar para ver si se ha corregido
                if (PersistenciaUtil.CargaIdioma(@"futbol\" + PersistenciaUtil.CargaConfig().IdiomaFichero) == null)
                    return false;
            }

            return true;
        }


    }
}
