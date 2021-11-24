using System;
using System.Net;
using System.Net.Sockets;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App
{

    /**
     * Interfaz de conexión con el IPF
     */
    public class InterfaceIPF
    {
        private const int Port = 5123;

        private Socket _socket;
        private IPEndPoint _ipf;


        /**
         * Evento para notificar la perdida de la conexion
         */
        public event EventHandler ConexionPerdida;

        // Invoca el evento ConexionPerdida
        protected virtual void OnConexionPerdida()
        {
            if (ConexionPerdida != null)
                ConexionPerdida(this, EventArgs.Empty);
        }


        /**
         * Constructor
         * Recibe la IP en la que conectar con el IPF
         */
        public InterfaceIPF(string ip)
        {
            configSocket(ip);
        }

        /**
         * Realiza la configuración inicial del IPF
         */
        public void ConfigInicial(Equipo local, Equipo visitante, string labelExtraTime, bool multicast, string aggregate)
        {
            //configBadgesPath(badgesPath);

            mapsIniciales(local, visitante, labelExtraTime, multicast, aggregate);
        }
        public void ConfigInicial(Equipo local, Equipo visitante, bool multicast, string aggregate)
        {
            //configBadgesPath(badgesPath);

            mapsIniciales(local, visitante, multicast, aggregate);
        }

        /**
         * Establece los goles que han marcado los equipos local y visitante
         */
        public void ConfigGoles(int golesLocal, int golesVisitante)
        {
            configGoles(true, golesLocal);
            configGoles(false, golesVisitante);
        }

        /*
         * Envia una instrucción al IPF
         */
        public bool Envia(string cadena)
        {
            byte[] msg;

            try
            {
                msg = System.Text.Encoding.Default.GetBytes(cadena + ";");
                _socket.Send(msg);

                return true;
            }
            catch (Exception e)
            {
                OnConexionPerdida();
                Console.WriteLine("Error en el envio de la cadena: " + cadena + ". Error: " + e.Message);
                return false;
            }
        }

        /**
         * Establece la conexión con el IPF
         */
        public bool Conectar()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_ipf);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al conectar: " + e.Message);
                return false;
            }
        }

        /**
         * Desconecta la conexión con el IPF
         */
        public void Desconectar()
        {
            try
            {
                if (_socket != null)
                    _socket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al desconectar: " + e.Message);
            }
        }

        /**
         * Indica si el IPF es accesible
         */
        public bool Ping()
        {
            bool correcto;

            correcto = Conectar();
            if (correcto)
                Desconectar();

            return correcto;
        }

        /**
         * Indica si la conexión está activa
         */
        public bool Conectado()
        {
            bool conectado = (_socket != null && _socket.Connected == true);

            if (!conectado)
                OnConexionPerdida();

            return conectado;
        }


        // =============================== METODOS PRIVADOS =======================================

        // Configuración inicial del IPF
        private void mapsIniciales(Equipo local, Equipo visitante, string labelExtraTime, bool multicast, string aggregate)
        {
            //Envia("MapsIniciales(['" + local.FullName + "', '" + visitante.FullName + "', '" +
            //    local.ShortName + "', '" + visitante.ShortName + "', '" +
            //    local.TeamCode + "', '" + visitante.TeamCode + "', '" +
            //    local.Badge + "', '" + visitante.Badge + "', '" +
            //    local.Color1.R + "', '" + local.Color1.G + "', '" + local.Color1.B + "', '" + local.Color2.R + "', '" + local.Color2.G + "', '" + local.Color2.B + "', '" + visitante.Color1.R + "', '" + visitante.Color1.G + "', '" + visitante.Color1.B + "', '" + visitante.Color2.R + "', '" + visitante.Color2.G + "', '" + visitante.Color2.B + "', '" +
            //    labelExtraTime + "', '" +
            //    (multicast ? "M" : "U") + "', '" +
            //    (aggregate == "-" ? "" : aggregate) + "'])");

            Envia("Maps_Iniciales(['" + local.FullName.Replace("'", "\\'") + "', '" + visitante.FullName.Replace("'", "\\'") + "', '" +
                     local.ShortName.Replace("'", "\\'") + "', '" + visitante.ShortName.Replace("'", "\\'") + "', '" +
                     local.TeamCode.Replace("'", "\\'") + "', '" + visitante.TeamCode.Replace("'", "\\'") + "', '" +
                     local.Badge.Replace(@"\", @"\\") + "', '" + visitante.Badge.Replace(@"\", @"\\") + "', '" +
                     local.Color1.R + "', '" + local.Color1.G + "', '" + local.Color1.B + "', '" + local.Color2.R + "', '" + local.Color2.G + "', '" + local.Color2.B + "', '" + visitante.Color1.R + "', '" + visitante.Color1.G + "', '" + visitante.Color1.B + "', '" + visitante.Color2.R + "', '" + visitante.Color2.G + "', '" + visitante.Color2.B + "', '" +
                     local.getFaltas() + "', '" + visitante.getFaltas() + "'])");
        }
        private void mapsIniciales(Equipo local, Equipo visitante, bool multicast, string aggregate)
        {
            //Envia("MapsIniciales(['" + local.FullName + "', '" + visitante.FullName + "', '" +
            //    local.ShortName + "', '" + visitante.ShortName + "', '" +
            //    local.TeamCode + "', '" + visitante.TeamCode + "', '" +
            //    local.Badge + "', '" + visitante.Badge + "', '" +
            //    local.Color1.R + "', '" + local.Color1.G + "', '" + local.Color1.B + "', '" + local.Color2.R + "', '" + local.Color2.G + "', '" + local.Color2.B + "', '" + visitante.Color1.R + "', '" + visitante.Color1.G + "', '" + visitante.Color1.B + "', '" + visitante.Color2.R + "', '" + visitante.Color2.G + "', '" + visitante.Color2.B + "', '" +
            //    (multicast ? "M" : "U") + "', '" +
            //    (aggregate == "-" ? "" : aggregate) + "'])");

            Envia("Maps_Iniciales(['" + local.FullName.Replace("'", "\\'") + "', '" + visitante.FullName.Replace("'", "\\'") + "', '" +
                     local.ShortName.Replace("'", "\\'") + "', '" + visitante.ShortName.Replace("'", "\\'") + "', '" +
                     local.TeamCode.Replace("'", "\\'") + "', '" + visitante.TeamCode.Replace("'", "\\'") + "', '" +
                     local.Badge.Replace(@"\", @"\\") + "', '" + visitante.Badge.Replace(@"\", @"\\") + "', '" +
                     local.Color1.R + "', '" + local.Color1.G + "', '" + local.Color1.B + "', '" + local.Color2.R + "', '" + local.Color2.G + "', '" + local.Color2.B + "', '" + visitante.Color1.R + "', '" + visitante.Color1.G + "', '" + visitante.Color1.B + "', '" + visitante.Color2.R + "', '" + visitante.Color2.G + "', '" + visitante.Color2.B + "', '" +
                     local.getFaltas() + "', '" + visitante.getFaltas() + "'])");
        }

        //// Establece la ruta de los escudos
        //private void configBadgesPath(string path)
        //{
        //    // Escapa los caracteres '\'
        //    path = path.Replace(@"\", @"\\");

        //    //Envia("itemset('BadgesPath', 'MAP_STRING_PAR', '" + path + "/')");
        //}

        // Establece los goles que ha marcado un equipo
        private void configGoles(bool local, int goles)
        {
            if (local)
            {
                Envia("itemset('MAPS_INICIALES/GolesLocal','MAP_STRING_PAR','" + goles + "')");
            }
            else
            {
                Envia("itemset('MAPS_INICIALES/GolesVisitante','MAP_STRING_PAR','" + goles + "')");
            }
        }


        // Confgiura el socket con la ip indicada
        private void configSocket(string ip)
        {
            try
            {
                IPAddress ipAdd = IPAddress.Parse(ip);
                _ipf = new IPEndPoint(ipAdd, Port);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error configurando el socket: " + e.Message);
            }
        }


    }
}
