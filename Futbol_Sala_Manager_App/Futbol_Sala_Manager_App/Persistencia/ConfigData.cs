
namespace Futbol_Sala_Manager_App.Persistencia
{

    /**
     * Bean de datos para la configuración de la aplicación
     */
    public class ConfigData
    {
        //Numero de IPFs
        public int NumIpf;

        public string BaseDatos;

        public bool isMachineCrono;

        public bool wasStardedMachineCrono;

        public bool ownedCronoControl;

        public string ipAddress;

        public string port;

        public  bool modeOcrActivated;                  // Indica si la aplicación ha sido configurada por el usuario para arrancar en modo OCR

        public static bool ModeOcrActivated;            // Indica si la aplicación ha sido configurada por el usuario para arrancar en modo OCR (estática para ser accesible desde otras clases)

        public string ipOcrServer;                      // IP del servidor que manda los datos del OCR

        public string portOcrServer;                    // Puerto del servidor que manda los datos del OCR

        public string puertoEscuchaMaquinaPrincipal;    // Puerto de escucha de la máquina Principal donde recibirá los datos del máquina Cronómetro

        public  string duracionParte_i;                 // Indica la duración que va a tener la primera y segunda parte del partido
        
        public  string duracionProrroga_i;              // Indica la duración que va a tener cada parte de la prórroga del partido

        // Ipf 1
        public string IpfIp;

        public string IdiomaFichero;

        public bool Multicast;
       

        // Ipf 2
        public string IpfIp2;

        public string IdiomaFichero2;

        public bool Multicast2;

        // Ipf 3
        public string IpfIp3;

        public string IdiomaFichero3;

        public bool Multicast3;

        // Ipf 4
        public string IpfIp4;

        public string IdiomaFichero4;

        public bool Multicast4;

        // Ipf 5
        public string IpfIp5;

        public string IdiomaFichero5;

        public bool Multicast5;

        // Ipf 6
        public string IpfIp6;

        public string IdiomaFichero6;

        public bool Multicast6;


        // Ipf 7
        public string IpfIp7;

        public string IdiomaFichero7;

        public bool Multicast7;

        // Ipf 8
        public string IpfIp8;

        public string IdiomaFichero8;

        public bool Multicast8;


        // Ipf 9
        public string IpfIp9;

        public string IdiomaFichero9;

        public bool Multicast9;

        // Ipf 10
        public string IpfIp10;

        public string IdiomaFichero10;

        public bool Multicast10;


    }
}