using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml.Serialization;
using Balonmano_Manager_App.Beans;

namespace Balonmano_Manager_App.Persistencia
{
    /**
     * Funciones de Persistencia
     * Permite grabar hacia y cargar desde el disco duro.
     * Carga y guarda ficheros de configuración, de backup, de idioma y de plantillas.
     */
    public class PersistenciaUtil
    {
        private const string FileConfig = "Balonmano_Manager_App.cfg";
        private const string FileBackup = "Balonmano_Manager_App.bak";
        private const string FilePlantillas = "Balonmano_Manager_App.tem";

        /**
         * Carga el fichero de configuración
         */
        public static ConfigData CargaConfig()
        {
            return cargaXml<ConfigData>(addRuta(FileConfig));
        }

        /**
         * Guarda el fichero de configuración
         */
        public static void GuardaConfig(ConfigData config)
        {
            creaXml(config, addRuta(FileConfig));
        }

        /**
         * Carga el fichero de textos
         */
        public static IdiomaData CargaIdioma(string filename)
        {
            return cargaXml<IdiomaData>(addRuta(filename));
        }

        /**
         * Obtiene una lista de los ficheros de idioma disponibles
         */
        public static FileInfo[] GetListaFicherosIdioma()
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(Application.ExecutablePath) + @"\balonmano");
            FileInfo[] rgFiles = di.GetFiles("*.xml");

            //Console.WriteLine("* * * * * * * * * * * * * "+ rgFiles.Length);

            return rgFiles;
        }

        /**
         * Almacena en disco todos los datos del encuentro
         */
        public static void GuardaBackup(EncuentroData backup)
        {
            creaBin(backup, addRuta(FileBackup));
        }

        /**
        * Recupera de disco todos los datos del encuentro
        */
        public static EncuentroData CargaBackup()
        {
            return cargaBin<EncuentroData>(addRuta(FileBackup));
        }

        /**
         * Devuelve la fecha del backup
         */
        public static DateTime GetBackupFecha()
        {
            return System.IO.File.GetLastWriteTime(addRuta(FileBackup));
        }

        /**
         * Almacena en disco las plantillas
         */
        public static void GuardaPlantillas(PlantillasData historico)
        {
            creaBin(historico, addRuta(FilePlantillas));
        }

        /**
        * Recupera de disco las plantillas
        */
        public static PlantillasData CargaPlantillas()
        {
            PlantillasData plantillas = cargaBin<PlantillasData>(addRuta(FilePlantillas));

            if (plantillas == null)
            {
                plantillas = new PlantillasData();
            }

            return plantillas;
        }

        /**
        * Recupera de disco los localizadores
        */
        public static PlantillasData CargaLocalizadores()
        {
            PlantillasData localizadores = cargaBin<PlantillasData>(addRuta(FilePlantillas));

            if (localizadores == null)
            {
                localizadores = new PlantillasData();
            }

            return localizadores;
        }

        // ============================================================================================

        private static Tipo cargaBin<Tipo>(string filename)
        {
            Tipo obj = default(Tipo);
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                obj = (Tipo)formatter.Deserialize(stream);
                stream.Close();
            }
            catch { }
            return obj;
        }

        private static void creaBin(Object obj, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, obj);
            stream.Close();
        }


        private static Tipo cargaXml<Tipo>(string filename)
        {
            // Declare an object variable of the type to be deserialized.
            Tipo obj;

            try
            {
                // Create an instance of the XmlSerializer class;
                // specify the type of object to be deserialized.
                XmlSerializer serializer = new XmlSerializer(typeof(Tipo));
                /* If the XML document has been altered with unknown 
                nodes or attributes, handle them with the 
                UnknownNode and UnknownAttribute events.*/
                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                // A FileStream is needed to read the XML document.
                FileStream fs = new FileStream(filename, FileMode.Open);

                /* Use the Deserialize method to restore the object's state with
                data from the XML document. */
                obj = (Tipo)serializer.Deserialize(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error cargando XML: " + e.Message);

                obj = default(Tipo);
            }

            // Return the object
            return obj;
        }

        private static void creaXml(Object obj, string filename)
        {
            try
            {
                // Create an instance of the XmlSerializer class;
                // specify the type of object to serialize.
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                TextWriter writer = new StreamWriter(filename);

                // Serialize the purchase order, and close the TextWriter.
                serializer.Serialize(writer, obj);
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error creando XML: " + e.Message);
            }
        }

        private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }
        private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }


        private static string addRuta(string file)
        {
            //Console.WriteLine(Application.ExecutablePath);
            //Console.WriteLine(Path.GetDirectoryName(Application.ExecutablePath));
            //return "C:/" + file;

            //Console.WriteLine(">>>" + Application.ExecutablePath + "<<<");
            //Console.WriteLine("***" + file + "***");
            //Console.WriteLine("========================================================================================================================================");

            return Path.GetDirectoryName(Application.ExecutablePath) + "/" + file;
        }

    }
}