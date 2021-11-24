using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Futbol_Sala_Manager_App
{
    class TCPClientReceiver
    {
        private readonly int port;              // Puerto del servidor al que este cliente se va a conectar
        private readonly string ip;             // Ip del servidor al que este cliente se va conectar
        private bool connectionStatus;          // True-> Conexión establecida con el sv. False -> No hay conexión con el sv
        private TcpClient tcpClient;            // Se usa para manejar conexiones del cliente
        private NetworkStream networkStream;    // Se usa para enviar y recibir datos a través de la conexión establecida con el sv

        public delegate void ReceivedDataDelegate(string data); // Delegado par manejar eventos al recibir un dato del sv
        public event ReceivedDataDelegate onReceivedData;       // Manejador del evento al recibir un dato del sv

        public delegate void ConnectionLossedDelegate();
        public event ConnectionLossedDelegate onLostConnection;

        // Constructor para establecer una conexión con el sv. Lanza una execepción en caso de no poder conectarse a dicho sv
        public TCPClientReceiver(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            try
            {
                startConnection();
            }
            catch (SocketException e)
            {
                throw e;
            }
        }
        // Comprueba si existe datos disponibles enviados por el servidor
        private bool checkDataAvailable()
        {
            return networkStream.DataAvailable;
        }
        // Empieza a esuchar y a obtener los datos enviados por el servidor. Lanza una execepción en caso de haber perdido la conexión
        public void getData()
        {
            try
            {
                while (connectionStatus)
                {
                    if (checkDataAvailable())
                    {
                        byte[] data = new Byte[256];
                        int bytes = networkStream.Read(data, 0, data.Length);
                        string responseData = string.Empty;
                        responseData = Encoding.ASCII.GetString(data, 0, bytes);
                        onReceivedData(responseData);
                    }
                    if (!socketConnected(tcpClient.Client))
                    {
                        connectionStatus = false;
                        retryConnection();
                    }
                }
            }
            catch (Exception e)
            {
                connectionStatus = false;
                //onLostConnection();
                throw e;
            }
        }
        // Se usa para parar la escuha de datos recibidos por el servidor
        public void stopListenData()
        {
            connectionStatus = false;
        }
        // Se intenta establecer conexión cono el servidor. Lanza una execepción en caso de no poder conectarse a dicho sv
        private void startConnection()
        {
            try
            {
                this.tcpClient = new TcpClient(ip, port);
                this.networkStream = tcpClient.GetStream();
                connectionStatus = true;
            }
            catch (SocketException e)
            {
                connectionStatus = false;
                throw e;
            }
        }
        // Intenta realizar la conexión
        public void retryConnection()
        {
            while (!connectionStatus)
            {
                Console.WriteLine("intentado reconexion");
                try
                {
                    this.tcpClient = new TcpClient(ip, port);
                    this.networkStream = tcpClient.GetStream();
                    connectionStatus = true;
                }
                catch (SocketException e)
                {
                    connectionStatus = false;
                }
            }
        }

        public bool getConnectionStatus()
        {
            return connectionStatus;
        }

        // Comprueba que exista conexión con el socket del servidor
        bool socketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
    }
}
