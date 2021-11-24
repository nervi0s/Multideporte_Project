using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Futbol_Sala_Manager_App;
using Balonmano_Manager_App.AccesoBD;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Interfaz;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App
{
    /**
     * Controlador del Loader
     */
    public class Loader
    {
        private LoaderForm _gui;
        private IAccesoBD _bd;

        // Ref. a consola e intérprete
        private Consola_balonmano _handballConsole;
        private Interprete_mondo _mondoInterprete;
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
        public void CargaEncuentro(int id, Form form)
        {
            _gui.Hide();

            _bd.AbreConexion();
            EncuentroData datos = _bd.DatosEncuentro(id);
            _bd.CierraConexion();


            // Consola e intérprete
            if (PersistenciaUtil.CargaConfig().puertoCOM != null)
            {
                Console.WriteLine("Veamos " + PersistenciaUtil.CargaConfig().puertoCOM);

                _handballConsole = new Consola_balonmano("mondo", PersistenciaUtil.CargaConfig().puertoCOM);
                _mondoInterprete = new Interprete_mondo(2, PersistenciaUtil.CargaConfig().puertoCOM);
                //_handballConsole = new Consola_balonmano("mondo", "COM3");
                //_mondoInterprete = new Interprete_mondo(2, "COM3");
                _mondoInterprete.conecta(_handballConsole);

                new Controlador(datos, _handballConsole, _mondoInterprete);                
            }
            else
            {
                Console.WriteLine("No has seleccionado ninguna consola valida");
            }
        }


        /**
         * Carga el encuentro de Backup
         */
        public void CargaEncuentroBackup()
        {
            _gui.Hide();

            EncuentroData datos = PersistenciaUtil.CargaBackup();

            if (PersistenciaUtil.CargaConfig().puertoCOM != null)
            {
                // Consola e intérprete
                _handballConsole = new Consola_balonmano("mondo", PersistenciaUtil.CargaConfig().puertoCOM);
                _mondoInterprete = new Interprete_mondo(2, PersistenciaUtil.CargaConfig().puertoCOM);
                //_handballConsole = new Consola_balonmano("mondo", "COM3");
                //_mondoInterprete = new Interprete_mondo(2, "COM3");
                _mondoInterprete.conecta(_handballConsole);


                new Controlador(datos, _handballConsole, _mondoInterprete);
            }
            else
            {
                Console.WriteLine("No has seleccionado ninguna consola valida");
            }

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
            if (PersistenciaUtil.CargaIdioma(@"balonmano\" + PersistenciaUtil.CargaConfig().IdiomaFichero) == null)
            {
                //Console.WriteLine(">>>> Fichero de idioma vacío");

                new ConfigForm().ShowDialog(_gui);
                // Vuelve a comprobar para ver si se ha corregido
                if (PersistenciaUtil.CargaIdioma(@"balonmano\" + PersistenciaUtil.CargaConfig().IdiomaFichero) == null)
                    return false;
            }

            return true;
        }


    }
}
