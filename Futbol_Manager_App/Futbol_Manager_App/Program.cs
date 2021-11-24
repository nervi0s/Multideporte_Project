using System;
using System.Windows.Forms;
using Futbol_Manager_App.Interfaz;

namespace Futbol_Manager_App
{
    static class Program
    {

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        /// 
        public static bool _PrimerComienzo = false;
        public static bool[] IpfsSeleccionados= new bool[10];
        public static bool CancelarConexion = false;
        public static int CambioEscogido = -1;

        [STAThread]
        static void Main(string[] args)
        {
            InicializaIpfsSeleccionados();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cuando se llama a la aplicación con el parámetro "-DummyData" se utilizan datos de prueba
            // En caso contrario se usa la conexión a la base de datos
            bool dummyData = (args.Length > 0 && args[0] == "-DummyData");

            try
            {
                // Inicia el formulario Loader
                Application.Run(new LoaderForm(dummyData));

            }
            catch { }
        }

        public static bool EstaActivado(int i)
        {
            return IpfsSeleccionados[i];
        }

        public static void InicializaIpfsSeleccionados()
        {
            for (int i = 0; i < 10; i++)
                IpfsSeleccionados[i] = true;
        }


    }
}
