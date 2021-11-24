using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Comandos;
using Futbol_Sala_Manager_App.Interfaz;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App
{
    /**
     * Controlador principal de la aplicación
     */
    public class Controlador
    {
        public int numEstadisticas=0;
        public int _stat1 = 0;
        public int _stat2 = 0;        
        public int _stat3 = 0;
        public int _cambioEscogido = -1;


        #region Constantes para Secciones
        public const int Clear = -1;
        public const int Previa = 0;
        public const int Presentacion = 1;
        public const int Rotulos = 2;
        public const int Corners = 3;
        public const int Faltas = 4;
        public const int Goles = 5;
        public const int GolesPP = 51;
        public const int GolesPenalti = 52;
        public const int TirosAPuerta = 6;
        public const int TirosAPuertaDentro = 61;
        public const int TirosAPuertaFuera = 62;
        public const int TirosAPuertaComp = 63;
        public const int Paradas = 7;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int FuerasDeJuego = 10;
        public const int Cambio = 11;
        public const int CambioIn = 111;
        public const int CambioOut = 112;
        public const int CambioInInfo = 113;
        public const int CambioOutInfo = 114;
        public const int CambioInfo = 115;
        public const int CoolingBreak = 116;
        public const int CambioDoble = 12;
        public const int Crono = 13;
        public const int Estadisticas = 14;
        #endregion

        #region Constantes para ListBox
        public const int Historial = 15;
        public const int Plantillas = 16;
        public const int Arbitros = 17;
        public const int Comentaristas = 18;
        public const int TiemposExtra = 19;
        public const int PreMatchs = 20;
        public const int Countdowns = 21;
        public const int Penaltis = 22;
        public const int Vacio = 23;
        public const int NewsExchangeFeed = 24;
        public const int EndToEndTest = 25;
        public const int Weather = 26;
        public const int PostMultiFlashInterview = 27;
        public const int EndOfPostMultiFlashInterview = 28;
        public const int EmergencyCaptions= 29;
        public const int Localizador = 30;
        public const int PerfilJugador = 31;
        public const int Clasificacion = 32;
        public const int Enfrentamientos = 33;
        public const int Rachas = 50;
        #endregion

        #region Constantes botones cambios
        #endregion



        private MainForm _gui;

        private InterfaceIPF[] _ipfs = new InterfaceIPF[10];
        private IdiomaData[] _idiomas = new IdiomaData[10];
        private Penaltis _penaltis;

        private int _seccionActual;
        private Jugador[] _cambio;

        private Crono _crono;

        private bool _onAir;
        private ICommandShowable _comandoOnAir;
        private ICommandShowable _comandoNext;
        private List<ICommand> _onListBox;
        private List<ICommand> _tiemposExtra;

        public EncuentroData _datos;
        private ConfigData _config;
        private PlantillasData _plantillas;
        private bool PJ_onAir = false;

        private Ocr ocr;
        /**
         * Constructor
         * Recibe los datos relativos al encuentro
         */

        public Controlador(EncuentroData datos)
        {

            // Datos
            _datos = datos;
            
           // Setup & Init   
            setup();
            _gui.Show();
            SetSeccion(Clear);

            // exchange.Add(command);

            if (_config.modeOcrActivated) // Se comprueba si al arrancar la aplicación se ha elegido el modo OCR y de ser así se instancia un nuevo objeto de tipo OCR
                ocr = new Ocr(_config.ipOcrServer, _config.portOcrServer, _config.modeOcrActivated, _gui, _crono);

            // Suscripción a los eventos del cliente
            AsynchronousSocketListener.onDataReceived += CronoClientDataReceived;
        }
                     

        /**
         * Devuelve el Controlador de los Penaltis
         */
        public Penaltis GetPenaltis()
        {
            return _penaltis;
        }

        public ICommandShowable GetComandoOnAir()
        {
            return this._comandoOnAir;
        }

        public void setCambio(int n)
        {
            this._cambioEscogido = n;
        }

        public void setEstadisticas(int e1, int e2, int e3)
        {
            this._stat1 = e1;
            this._stat2 = e2;
            this._stat3 = e3;
        }

        public void setEstadisticasSeleccionadas(int n)
        {
            this.numEstadisticas = n;
        }


        // Configuración inicial de los atributos
        private void setup()
        {
            // GUI
            _gui = new MainForm(this);

            // Carga configuración
            _config = PersistenciaUtil.CargaConfig();

            if (_config.isMachineCrono) // Si es la máquina crono en la que se ejecuta este código necesitamos obviar el número de IPFs
            {

                // Carga los ficheros de idiomas de los Ipfs configurados
                for (int i = 0; i < 1; i++)
                {
                    switch (i)
                    {
                        case 0: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero); break;
                        case 1: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero2); break;
                        case 2: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero3); break;
                        case 3: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero4); break;
                        case 4: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero5); break;
                        case 5: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero6); break;
                        case 6: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero7); break;
                        case 7: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero8); break;
                        case 8: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero9); break;
                        case 9: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero10); break;
                        default: break;
                    }
                }
            }
            else
            {
                // Carga los ficheros de idiomas de los Ipfs configurados
                for (int i = 0; i < _config.NumIpf; i++)
                {
                    switch (i)
                    {
                        case 0: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero); break;
                        case 1: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero2); break;
                        case 2: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero3); break;
                        case 3: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero4); break;
                        case 4: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero5); break;
                        case 5: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero6); break;
                        case 6: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero7); break;
                        case 7: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero8); break;
                        case 8: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero9); break;
                        case 9: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol_sala\" + _config.IdiomaFichero10); break;
                        default: break;
                    }
                }
            }

            // Carga las plantillas
            _plantillas = PersistenciaUtil.CargaPlantillas();

            // Configura el GUI con los jugadores
            _gui.ConfigEquipoLocal(_datos.EquipoL);
            _gui.ConfigEquipoVisitante(_datos.EquipoV);

            // Genera los Tiempos Extra por defecto
            configTiemposExtra();
            
            // Carga la configuración de los Ipfs
            for (int i = 0; i < _config.NumIpf; i++)
            {
                switch (i)
                {
                    case 0: _ipfs[i] = new InterfaceIPF(_config.IpfIp); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida);  break;
                    case 1: _ipfs[i] = new InterfaceIPF(_config.IpfIp2); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 2: _ipfs[i] = new InterfaceIPF(_config.IpfIp3); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 3: _ipfs[i] = new InterfaceIPF(_config.IpfIp4); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 4: _ipfs[i] = new InterfaceIPF(_config.IpfIp5); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 5: _ipfs[i] = new InterfaceIPF(_config.IpfIp6); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 6: _ipfs[i] = new InterfaceIPF(_config.IpfIp7); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 7: _ipfs[i] = new InterfaceIPF(_config.IpfIp8); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 8: _ipfs[i] = new InterfaceIPF(_config.IpfIp9); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;
                    case 9: _ipfs[i] = new InterfaceIPF(_config.IpfIp10); _ipfs[i].ConexionPerdida += new System.EventHandler(onConexionPerdida); break;

                    default: break;
                }
            }
            // Settings duración partes del Momento
            Momento.minuto[0] = int.Parse(_config.duracionParte_i);
            Momento.minuto[2] = int.Parse(_config.duracionParte_i);
            Momento.minuto[4] = int.Parse(_config.duracionProrroga_i);
            Momento.minuto[6] = int.Parse(_config.duracionProrroga_i);

            // Cronometro
            _crono = new Crono(_ipfs, _idiomas,_config.NumIpf);
            _crono.Bind(_gui);
            PosesionActivar(false);
 
            // Penaltis
            _penaltis = new Penaltis(_ipfs, _gui,_config.NumIpf);

            // Variables intermedias para los cambios
            _cambio = new Jugador[3];

            
        }

        // Configura el Marcador. Calcula los goles de cada equipo y lo refleja en el GUI y en el IPF
        private void configMarcador()
        {
            int n = _config.NumIpf;
            int golesL = _datos.EquipoL.Goles.Count + _datos.EquipoL.GolesPenalty.Count + _datos.EquipoV.GolesPP.Count;
            int golesV = _datos.EquipoV.Goles.Count + _datos.EquipoV.GolesPenalty.Count + _datos.EquipoL.GolesPP.Count;

            _gui.SetMarcador(golesL + " - " + golesV);
            for (int i = 0; i < n; i++)
                _ipfs[i].ConfigGoles(golesL, golesV);

        }

        // Genera los Tiempos Extra por defecto
        private void configTiemposExtra()
        {
            _tiemposExtra = new List<ICommand>();
            for (int i = 1; i <= 6; i++)
                _tiemposExtra.Add(new ExtraTimeCommand(i));
        }

        /**
         * Cierra la aplicación
         * Desconecta del IPF y guarda un backup de los datos.
         */
        public void CerrarAplicacion()
        {
            for (int i = 0; i < _config.NumIpf;i++ )
                _ipfs[i].Desconectar();
            PersistenciaUtil.GuardaBackup(_datos);

            /*
            List<ICommand> exchange= new List<ICommand>();
            EmergencyCaptionsCommand command = new EmergencyCaptionsCommand(new EmergencyCaptions("EMERGENCY", "Linea1"));
            exchange.Add(command);
            _plantillas.emergencyCaptions = exchange;
             */
             
            PersistenciaUtil.GuardaPlantillas(_plantillas);
            Application.Exit();
        }


        // ================================ SECCION ================================
        #region Seccion

        /**
         * Establece la Sección actual
         */
        public void SetSeccion(int seccion)
        {
            Console.WriteLine("Click en sección: " + seccion );
            _seccionActual = seccion;

            // Desactiva todos los controles
            _gui.ActivaEquipos(false);
            _gui.ActivaEntrenadores(false);
            _gui.ActivaAsistentes(false);
            _gui.ActivaJugadores(true, new List<int>());
            _gui.ActivaJugadores(false, new List<int>());

            switch (_seccionActual)
            {
                case Rotulos:
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaAsistentes(true);
                    _gui.ActivaTodosJugadores(true);
                    _gui.ActivaTodosJugadores(false);
                    break;

                case Corners:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Corners, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                    }
                    break;
              
                case Faltas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Faltas, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay() || _gui.GetConfigData().ownedCronoControl)
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getJugadoresNoExpulsados(true));
                        _gui.ActivaJugadores(false, getJugadoresNoExpulsados(false));
                        //_gui.ActivaTodosJugadores(true);
                       // _gui.ActivaTodosJugadores(false);
                    }
                    break;

                case Goles:
                case GolesPenalti:
                case GolesPP:
                    //OnAirSetup(new StatisticsCommand(StatisticsCommand.Goles, _datos.EquipoL, _datos.EquipoV));
                    OnAirSetup(new StatisticsEfficiencyCommand(StatisticsCommand.Goles, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay() || _gui.GetConfigData().ownedCronoControl)
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getJugadoresNoExpulsados(true));
                        _gui.ActivaJugadores(false, getJugadoresNoExpulsados(false));

                        //_gui.ActivaTodosJugadores(true);
                       // _gui.ActivaTodosJugadores(false);
                        
                    }
                    break;
                
                case TirosAPuerta:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuerta, _datos.EquipoL, _datos.EquipoV));
                    break;

                case TirosAPuertaDentro:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuertaDentro, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay() || _gui.GetConfigData().ownedCronoControl)
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getJugadoresNoExpulsados(true));
                        _gui.ActivaJugadores(false, getJugadoresNoExpulsados(false));
                        //_gui.ActivaTodosJugadores(true);
                        //_gui.ActivaTodosJugadores(false);
                    }
                    break;

                case TirosAPuertaFuera:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuertaFuera, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay() || _gui.GetConfigData().ownedCronoControl)
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getJugadoresNoExpulsados(true));
                        _gui.ActivaJugadores(false, getJugadoresNoExpulsados(false));
                       // _gui.ActivaTodosJugadores(true);
                        //_gui.ActivaTodosJugadores(false);
                    }
                    break;

                case TirosAPuertaComp:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuertaComp, _datos.EquipoL, _datos.EquipoV));
                    break;

                case Paradas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Paradas, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay() || _gui.GetConfigData().ownedCronoControl)
                    {
                        //Activa porteros
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getPorteros(true));
                        _gui.ActivaJugadores(false, getPorteros(false));
                    }
                    break;

                case TAmarillas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TAmarillas, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaAsistentes(true);
                    _gui.ActivaJugadores(true, getJugadoresNoExpulsados(true));
                        _gui.ActivaJugadores(false, getJugadoresNoExpulsados(false));
                    //   _gui.ActivaTodosJugadores(true);
                       // _gui.ActivaTodosJugadores(false);
                    break;

                case TRojas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TRojas, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaAsistentes(true);
                    _gui.ActivaJugadores(true, getJugadoresNoExpulsados(true));
                        _gui.ActivaJugadores(false, getJugadoresNoExpulsados(false));
                        //                    _gui.ActivaTodosJugadores(true);
                       // _gui.ActivaTodosJugadores(false);
                    break;               

                case Estadisticas:
                    if (numEstadisticas == 2)
                    {
                        
                        OnAirSetup(new EstadisticasCommand(_stat2,_stat1, _datos.EquipoL, _datos.EquipoV, _datos));
                    }
                    if (numEstadisticas == 3)
                    {

                        OnAirSetup(new EstadisticasCommand(_stat3, _stat2,_stat1, _datos.EquipoL, _datos.EquipoV, _datos));
                    }
                    

                    break;

                case Crono:
                    if (_crono.GetMomento().Parte == Momento.Penaltis && IsPlay() || _crono.GetMomento().Parte == Momento.FinPenaltis)
                    {
                        ListBoxConfig(Penaltis);
                    }
                    else
                    {
                        ListBoxConfig(Vacio);
                    }
                    break;
            }


            // Carga el historial en el listBox para todas las secciones excepto la del crono
            if (seccion != Crono)
            {
                ListBoxConfig(Historial);
            }
        }


        // Devuelve los jugadores titulares no expulsados del equipo indicado
        private List<int> getTitulares(bool eqLocal)
        {
            Equipo equipo = (eqLocal ? _datos.EquipoL : _datos.EquipoV);
            List<int> dorsales = new List<int>();

            foreach (Jugador j in equipo.Jugadores)
            {
                if (j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
                    dorsales.Add(j.Number);
            }

            return dorsales;
        }

        private List<int> getJugadoresNoExpulsados(bool eqLocal)
        {
            Equipo equipo = (eqLocal ? _datos.EquipoL : _datos.EquipoV);
            List<int> dorsales = new List<int>();

            foreach (Jugador j in equipo.Banquillo)
            {
                if (j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
                    dorsales.Add(j.Number);
            }

            foreach (Jugador j in equipo.Jugadores)
            {
                if (j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
                    dorsales.Add(j.Number);
            }

            return dorsales;
        }
        
        // Devuelve los porteros titulares no expulsados del equipo indicado
        private List<int> getPorteros(bool eqLocal)
        {
            Equipo equipo = (eqLocal ? _datos.EquipoL : _datos.EquipoV);
            List<int> dorsales = new List<int>();

            foreach (Jugador j in equipo.Banquillo)
            {
                if (j.Posicion == Jugador.Portero && j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
                    dorsales.Add(j.Number);
            }

            foreach (Jugador j in equipo.Jugadores)
            {
                if (j.Posicion == Jugador.Portero && j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
                    dorsales.Add(j.Number);
            }
            
            return dorsales;
        }

        // Devuelve los jugadores reserva no expulsados del equipo indicado
        private List<int> getBanquillo(bool eqLocal)
        {
            Equipo equipo = (eqLocal ? _datos.EquipoL : _datos.EquipoV);
            List<int> dorsales = new List<int>();

            foreach (Jugador j in equipo.Banquillo)
            {
                if (j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
                    dorsales.Add(j.Number);
            }

            return dorsales;
        }

        // Devuelve los jugadores titulares o reservas no expulsados del equipo indicado
        private List<int> getJugadores(bool eqLocal)
        {
            List<int> dorsales = new List<int>();

            dorsales.AddRange(getTitulares(eqLocal));
            dorsales.AddRange(getBanquillo(eqLocal));

            return dorsales;
        }

        #endregion

        // ================================== IPF ==================================
        #region IPF

        /**
         * Establece conexión con el IPF
         * Tras conectar se envian todos los comandos necesarios para reflejar 
         * el estado actual del partido.
         */
        public void EstablecerConexionIpf()
        {
            int n=_config.NumIpf;
            bool conectado = _ipfs[0].Conectar();

            //Console.WriteLine("1");

            for (int i = 1; i < n; i++)
                conectado = conectado && _ipfs[1].Conectar();

                if (conectado)
                {
                    for (int i = 0; i < n; i++)
                        switch (i)
                        {
                        //case 0: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast, _datos.ResultadoIda); break;
                        //case 1: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast2, _datos.ResultadoIda); break;
                        //case 2: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast3, _datos.ResultadoIda); break;
                        //case 3: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast4, _datos.ResultadoIda); break;
                        //case 4: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast5, _datos.ResultadoIda); break;
                        //case 5: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast6, _datos.ResultadoIda); break;
                        //case 6: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast7, _datos.ResultadoIda); break;
                        //case 7: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast8, _datos.ResultadoIda); break;
                        //case 8: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast9, _datos.ResultadoIda); break;
                        //case 9: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast10, _datos.ResultadoIda); break;

                        case 0: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast, _datos.ResultadoIda); break;
                        case 1: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast2, _datos.ResultadoIda); break;
                        case 2: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast3, _datos.ResultadoIda); break;
                        case 3: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast4, _datos.ResultadoIda); break;
                        case 4: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast5, _datos.ResultadoIda); break;
                        case 5: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast6, _datos.ResultadoIda); break;
                        case 6: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast7, _datos.ResultadoIda); break;
                        case 7: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast8, _datos.ResultadoIda); break;
                        case 8: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast9, _datos.ResultadoIda); break;
                        case 9: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _config.Multicast10, _datos.ResultadoIda); break;

                        default: break;
                        }

                    //Console.WriteLine("2");
                    _crono.UpdateIpf();
                    //Console.WriteLine("3");
                    configMarcador();
                    _penaltis.UpdateIpf();

                    _gui.SetConexionEstado(true);
                    _gui.ActivaCrono(true);
                    _gui.GetPenaltisGui().ActivaMarcador(true);
                }
                else
                {
                    _gui.SetConexionEstado(false);
                }

                //Console.WriteLine("4");
        }

        /**
         * Cierra la conexión con el IPF
         */
        public void CerrarConexionIpf()
        {
            for (int i = 0; i < _config.NumIpf; i++)
                _ipfs[i].Desconectar();

            _gui.SetConexionEstado(false);

            _gui.SetOnAirLive(false);
            _onAir = false;
            OnAirClear();

            _gui.ActivaCrono(false);
            _gui.GetPenaltisGui().ActivaMarcador(false);
        }

        // Manejador de evento de conexión perdida
        private void onConexionPerdida(object sender, EventArgs e)
        {
            CerrarConexionIpf();
        }

        #endregion

        // =============================== POSESION ================================
        #region Posesion

        /**
         * Inicia on periodo de posesión del equipo indicado
         */
        public void PosesionStart(bool local)
        {
            _datos.Posesion.posesionStart(local);

            
        }

        /**
         * Finaliza el periodo de posesión
         */
        public void PosesionStop()
        {
            _datos.Posesion.posesionStop();

            
        }

        /**
         * Activa o desactiva los controles de posesión
         */
        public void PosesionActivar(bool activo)
        {
            if (!activo)
                PosesionStop();

            _gui.ActivaOpcionesPosesion(activo);
        }

        #endregion

        // ======================= CLICK ON JUGADOR & EQUIPO =======================
        #region ClickOn Jugador y Equipo

        public void ClickOnJugador(Jugador jugador, Button botonJugador)
        {            
            switch (_seccionActual){
                case Rotulos:
                    ShowRotulosJugador(jugador);
                    break;

                case Faltas:
                    addListBoxItem(new FaltaCommand(_crono.GetMomento(), jugador));
                    break;

                case Goles:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), jugador), true);

                    _gui.AbrirRotulosJugador();
                    botonJugador.PerformClick();
                    break;

                case GolesPP:
                    addListBoxItem(new GolPPCommand(_crono.GetMomento(), jugador), true);
                    break;

                case GolesPenalti:
                    addListBoxItem(new GolPenaltyCommand(_crono.GetMomento(), jugador), true);

                    _gui.AbrirRotulosJugador();
                    botonJugador.PerformClick();
                    break;

                case TirosAPuertaDentro:
                    addListBoxItem(new TiroAPuertaCommand(_crono.GetMomento(), jugador));
                    break;

                case TirosAPuertaFuera:
                    addListBoxItem(new TiroFueraCommand(_crono.GetMomento(), jugador));
                    break;

                case Paradas:
                    addListBoxItem(new ParadaCommand(_crono.GetMomento(), jugador));
                    break;

                case TAmarillas:
                    addListBoxItem(new YellowCardCommand(_crono.GetMomento(), jugador));
                    break;

                case TRojas:
                    addListBoxItem(new RedCardCommand(_crono.GetMomento(), jugador));
                    List<int> dorsales; 
                    if (jugador.Equipo.Local)
                        dorsales=getJugadoresNoExpulsados(true);
                    else
                        dorsales=getJugadoresNoExpulsados(false);
                    dorsales.Remove(jugador.Number);
                    _gui.ActivaJugadores(jugador.Equipo.Local, dorsales);
                    break;
                                  
                case Crono:
                    _penaltis.AddPenalti(jugador);
                    break;

            }
        }

        public void ClickOnEntrenador(Jugador entrenador)
        {
            switch (_seccionActual)
            {
                case TAmarillas:
                    addListBoxItem(new YellowCardCommand(_crono.GetMomento(), entrenador));
                    break;

                case TRojas:
                    addListBoxItem(new RedCardCommand(_crono.GetMomento(), entrenador));
                    break;

                case Rotulos:
                    OnAirSetup(new CoachCommand(entrenador));
                    break;
            }
        }

        public void ClickOnAsistente(Jugador asistente)
        {
            switch (_seccionActual)
            {
                case TAmarillas:
                    addListBoxItem(new YellowCardCommand(_crono.GetMomento(), asistente));
                    break;

                case TRojas:
                    addListBoxItem(new RedCardCommand(_crono.GetMomento(), asistente));
                    break;

                case Rotulos:
                    OnAirSetup(new AssistantCoachCommand(asistente));
                    break;
            }
        }


        public void ClickOnEquipo(bool local)
        {
            Equipo equipo = (local ? _datos.EquipoL : _datos.EquipoV);
            switch (_seccionActual)
            {
                case Corners:
                    addListBoxItem(new CornerCommand(_crono.GetMomento(), equipo));
                    break;

                case Faltas:
                    addListBoxItem(new FaltaCommand(_crono.GetMomento(), equipo));
                    break;

                case Goles:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), equipo));
                    break;

                case Paradas:
                    addListBoxItem(new ParadaCommand(_crono.GetMomento(), equipo));
                    break;

                case GolesPP:
                    addListBoxItem(new GolPPCommand(_crono.GetMomento(), equipo));
                    break;

                case GolesPenalti:
                    addListBoxItem(new GolPenaltyCommand(_crono.GetMomento(), equipo));
                    break;

                case TirosAPuertaDentro:
                    addListBoxItem(new TiroAPuertaCommand(_crono.GetMomento(), equipo));
                    break;

                case TirosAPuertaFuera:
                    addListBoxItem(new TiroFueraCommand(_crono.GetMomento(), equipo));
                    break;

                case TAmarillas:
                    addListBoxItem(new YellowCardTeamCommand(_crono.GetMomento(), equipo));
                    break;

                case TRojas:
                    addListBoxItem(new RedCardTeamCommand(_crono.GetMomento(), equipo));
                    break;         
            }
        }       
      
        #endregion

        // ================================= ONAIR =================================
        #region OnAir

        public void OnAirSetup(ICommandShowable command)
        {
            bool conectado = _ipfs[0].Conectado();
            for (int i = 1; i < _config.NumIpf; i++)
            {
                if (Program.IpfsSeleccionados[i] == true)
                    conectado = conectado && _ipfs[i].Conectado();
            }
            if (conectado)
            {
                command.Reset();

                if (!_onAir)
                {
                    _comandoOnAir = command;
                    _gui.SetOnAirText(command.ToString());
                }
                else
                {
                    _comandoNext = command;
                    _gui.SetNextAirText(command.ToString());
                }
            }
        }

        public void OnAirRun()
        {

            if (_comandoOnAir != null)
            {
                _gui.SetOnAirLive(true);
                _onAir = _comandoOnAir.Show(_ipfs, _idiomas, _config.NumIpf);
                if (!_onAir)
                {
                    _gui.SetOnAirLive(false);
                    OnAirNext();
                }
            }
        }

        public void OnAirNext()
        {
            _comandoOnAir = _comandoNext;
            if (_comandoOnAir == null)
            {
                _gui.SetOnAirText("");
            }
            else
            {
                _gui.SetOnAirText(_comandoOnAir.ToString());
            }
            _comandoNext = null;
            _gui.SetNextAirText("");
        }

        public void OnAirClear()
        {
            if (_comandoOnAir != null && !_onAir)
            {
                _gui.SetOnAirText("");
                _comandoOnAir = null;
            }
            if (_comandoNext != null)
            {
                _gui.SetNextAirText("");
                _comandoNext = null;
            }
        }

        #endregion

        // ================================ LISTBOX ================================
        #region ListBox

        public void ListBoxNewItem()
        {
            if (_onListBox == _plantillas.Plantillas) // Plantillas
            {
                PlantillaForm pf = new PlantillaForm();
                pf.ShowDialog();

                if (pf.getPlantilla() != null)
                    addListBoxItem(pf.getPlantilla());
            }
            else if (_onListBox == _plantillas.Clasificacion)
            {
                ClasificacionForm cf = new ClasificacionForm();
                cf.ShowDialog();

                if (cf.GetClasificacion() != null)
                    addListBoxItem(new ClasificacionCommand(cf.GetClasificacion()));
            }
            else if (_onListBox == _plantillas.Enfrentamientos)
            {
                EnfrentamientosForm cf = new EnfrentamientosForm();
                cf.ShowDialog();

                if (cf.GetEnfrentamientos() != null)
                    addListBoxItem(new EnfrentamientosCommand(cf.GetEnfrentamientos()));
            }
            else if (_onListBox == _plantillas.Rachas)
            {
                RachasForm rf = new RachasForm();
                rf.ShowDialog();

                if (rf.GetRachas() != null)
                    addListBoxItem(new RachasCommand(rf.GetRachas()));
            }
            else if (_onListBox == _plantillas.Localizador)
            {
                LocalizadorForm lf = new LocalizadorForm();
                lf.ShowDialog();

                if (lf.GetLocalizador() != null)
                    addListBoxItem(new LocalizadorCommand(lf.GetLocalizador()));
            }
            else if (_onListBox == _plantillas.PerfilJugador)
            {
                PerfilJugadorForm pjf = new PerfilJugadorForm();
                pjf.ShowDialog();

                if (pjf.GetPerfilJugador() != null)
                    addListBoxItem(new PerfilJugadorCommand(pjf.GetPerfilJugador()));
            }
            else // Tiempo Extra
            {
                ExtraForm ef = new ExtraForm();
                ef.ShowDialog();

                if (ef.getMinutos() != -1)
                    addListBoxItem(new ExtraTimeCommand(ef.getMinutos()));
            }
        }

        private void executeItem(ICommand command)
        {
            // Si es ejecutable lo ejecuta
            if (command is ICommandExecutable)
            {
                ((ICommandExecutable)command).Execute();

                //if (command is GolCommand)
                //    configMarcador();               

                // Refresca los comandos OnAir
                if (_comandoOnAir != null && !_onAir)
                    _gui.SetOnAirText(_comandoOnAir.ToString());
                if (_comandoNext != null)
                    _gui.SetNextAirText(_comandoNext.ToString());
            }

            if (command is ICommandImmediateExecutable)
            {
                ((ICommandImmediateExecutable)command).ExecuteImmediate(_ipfs, _idiomas, _config.NumIpf);
            }

            // Si se puede mostrar lo pone OnAir
            if (command is ICommandShowable)
            {
                OnAirSetup((ICommandShowable)command);
            }
        }

        private void addListBoxItem(ICommand command, bool nuevoGol = false)
        {
            _onListBox.Insert(0, command);
            _onListBox.Sort(new ICommandComparer());
            _gui.ConfigListBox(_onListBox.ToArray());

            // Si es ejecutable lo ejecuta
            if (command is ICommandExecutable)
            {
                ((ICommandExecutable)command).Execute();

                if (command is GolCommand || command is GolPPCommand || command is GolPenaltyCommand)
                    configMarcador();

                if (command is YellowCardCommand || command is RedCardCommand)
                {
                    // Refresca para posible desactivación de un jugador
                    SetSeccion(_seccionActual);
                }

                if(command is GolCommand || command is GolPenaltyCommand || command is GolPPCommand)
                {
                    ListBoxEditItem(command, nuevoGol);
                    //ListBoxEditItem();
                    //Jugador jugador = 
                    //    command is GolCommand ? (command as GolCommand).GetJugador() :
                    //    command is GolPenaltyCommand ? (command as GolPenaltyCommand).GetJugador():
                    //    command is GolPPCommand ? (command as GolPPCommand).GetJugador() : 
                    //    null;
                    
                    //if (jugador != null)
                    //{
                        //SetSeccion(Rotulos);
                        //ShowRotulosJugador(jugador);
                        //ListBoxEditItem(command);
                        //if (command is ICommandShowable)
                        //    OnAirSetup(command as ICommandShowable);
                    //}
                }

                // Refresca los comandos OnAir
                if (_comandoOnAir != null && !_onAir)
                    _gui.SetOnAirText(_comandoOnAir.ToString());
                if (_comandoNext != null)
                    _gui.SetNextAirText(_comandoNext.ToString());
            }

            // Si nada más producirse el evento se manda información
            if (command is ICommandImmediateExecutable)
            {
                (command as ICommandImmediateExecutable).ExecuteImmediate(_ipfs, _idiomas, _config.NumIpf);
            }

            // Si se puede mostrar lo pone OnAir
            if (command is ICommandShowable)
            {
                OnAirSetup((ICommandShowable)command);
            }

            // Backup
            PersistenciaUtil.GuardaBackup(_datos);
        }

        public void ListBoxRemoveItem(object item)
        {
            // Si la seccion actual es el crono elimina un penalti
            if (_seccionActual == Crono)
            {
                _penaltis.DelPenalti();
            }
            else if (item != null)
            {
                // Dialogo de confirmacion
                DialogResult reply = MessageBox.Show(
                  "Se va a eliminar el elemento:\n\n" + item + "\n\nEsta acción no se puede deshacer. ¿Estás seguro?",
                  "Borrado",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (reply == DialogResult.Yes)
                {
                    bool posible = true;
                    string msgError = "";

                    // Si no es posible se muestra un mensaje
                    if (!posible)
                    {
                        MessageBox.Show(msgError, "Borrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else // Procede a eleminarlo
                    {
                        removeListBoxItem((ICommand)item);
                    }

                }
            }

        }

        private void removeListBoxItem(ICommand command)
        {
            // Quita el elemento de la lista y recarga la lista en el gui
            _onListBox.Remove(command);
            _gui.ConfigListBox(_onListBox.ToArray());

            // Si es ejecutable lo deshace
            if (command is ICommandExecutable)
            {
                ((ICommandExecutable)command).Undo();
                if (command is GolCommand || command is GolPPCommand || command is GolPenaltyCommand)
                {
                    configMarcador();
                }               
                if (command is YellowCardCommand || command is RedCardCommand)
                {
                    // Refresca para posible reactivación de un jugador
                    SetSeccion(_seccionActual);
                }

            }

            if(command is ICommandImmediateExecutable)
            {
                (command as ICommandImmediateExecutable).UndoImmediate(_ipfs, _idiomas, _config.NumIpf);
            }

            // Backup
            PersistenciaUtil.GuardaBackup(_datos);
        }

        public void ListBoxEditItem(ICommand command, bool nuevoGol = false)
        {
         
            if (command is FreeTextCommand)
            {
                FreeTextCommand ftc = (FreeTextCommand)command;

                PlantillaForm pf = new PlantillaForm(ftc);
                pf.ShowDialog();

                if (pf.getPlantilla() != null)
                {
                    removeListBoxItem(command);
                    addListBoxItem(pf.getPlantilla());
                }
            }
            else if (command is ExtraTimeCommand)
            {
                ExtraTimeCommand etc = (ExtraTimeCommand)command;

                ExtraForm ef = new ExtraForm(etc.Minutos);
                ef.ShowDialog();

                if (ef.getMinutos() != -1)
                {
                    etc.Minutos = ef.getMinutos();
                    _gui.Refresh();
                }

            }
            else if(command is LocalizadorCommand)
            {
                LocalizadorCommand exc = command as LocalizadorCommand;

                LocalizadorForm efm = new LocalizadorForm(exc.localizador);
                efm.ShowDialog();

                if (efm.GetLocalizador() != null)
                {
                    exc.localizador = efm.GetLocalizador();
                    _gui.Refresh();
                }
            }
            else if (command is PerfilJugadorCommand)
            {
                PerfilJugadorCommand exc = command as PerfilJugadorCommand;

                PerfilJugadorForm efm = new PerfilJugadorForm(exc.perfilJugador);
                efm.ShowDialog();

                if (efm.GetPerfilJugador() != null)
                {
                    exc.perfilJugador = efm.GetPerfilJugador();
                    _gui.Refresh();
                }
            }
            else if (command is ClasificacionCommand)
            {
                ClasificacionCommand exc = command as ClasificacionCommand;

                ClasificacionForm efm = new ClasificacionForm(exc.clasificacion);
                efm.ShowDialog();

                if (efm.GetClasificacion() != null)
                {
                    exc.clasificacion = efm.GetClasificacion();
                    _gui.Refresh();
                }
            }
            else if(command is EnfrentamientosCommand)
            {
                EnfrentamientosCommand exc = command as EnfrentamientosCommand;

                EnfrentamientosForm efm = new EnfrentamientosForm(exc.enfrentamientos);
                efm.ShowDialog();

                if (efm.GetEnfrentamientos() != null)
                {
                    exc.enfrentamientos = efm.GetEnfrentamientos();
                    _gui.Refresh();
                }
            }
            else if (command is RachasCommand)
            {
                RachasCommand exc = command as RachasCommand;

                RachasForm efm = new RachasForm(exc.rachas);
                efm.ShowDialog();

                if (efm.GetRachas() != null)
                {
                    exc.rachas = efm.GetRachas();
                    _gui.Refresh();
                }
            }           
            else // ICommandExecutable
            {
                ICommandExecutable ce = (ICommandExecutable)command;

                TiempoForm tf = new TiempoForm(ce.Momento, _config.duracionParte_i, _config.duracionProrroga_i);
                
                if (nuevoGol)
                {
                    tf.getBotonAceptar().PerformClick();
                }
                else
                {
                    tf.ShowDialog(_gui);
                }

                if (tf.getMomento() != null)
                {
                    _onListBox.Sort(new ICommandComparer());
                    _gui.ConfigListBox(_onListBox.ToArray());
                }
            }

            // Backup
            PersistenciaUtil.GuardaBackup(_datos);
        }


        public void ListBoxConfig(int listBox)
        {
            switch (listBox)
            {
                case (Historial):
                    _onListBox = _datos.Historial;
                    _gui.activaAcciones(false, true, true);
                    break;

                case (Plantillas):
                    _onListBox = _plantillas.Plantillas;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Localizador):
                    if (_plantillas.Localizador == null)
                        _plantillas.Localizador = new List<ICommand>();
                    _onListBox = _plantillas.Localizador;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (PerfilJugador):
                    if (_plantillas.PerfilJugador == null)
                        _plantillas.PerfilJugador = new List<ICommand>();
                    _onListBox = _plantillas.PerfilJugador;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Clasificacion):
                    if (_plantillas.Clasificacion == null)
                        _plantillas.Clasificacion = new List<ICommand>();
                    _onListBox = _plantillas.Clasificacion;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Enfrentamientos):
                    if (_plantillas.Enfrentamientos == null)
                        _plantillas.Enfrentamientos = new List<ICommand>();
                    _onListBox = _plantillas.Enfrentamientos;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Rachas):
                    if (_plantillas.Rachas == null)
                        _plantillas.Rachas = new List<ICommand>();
                    _onListBox = _plantillas.Rachas;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Arbitros):
                    _gui.activaAcciones(false, false, false);
                    _onListBox = _datos.Arbitros;
                    break;

                case (Comentaristas):
                    _onListBox = _datos.Comentaristas;
                    _gui.activaAcciones(false, false, false);
                    break;
             
                case (Penaltis):
                    _onListBox = new List<ICommand>();
                    _gui.activaAcciones(false, false, true);
                    break;

                case (Vacio):
                    _onListBox = new List<ICommand>();
                    _gui.activaAcciones(false, false, false);
                    break;
            }

            _gui.ConfigListBox(_onListBox.ToArray());

            // Muestra u oculta el panel de penaltis
            _gui.ShowPenaltis(listBox == Penaltis);
        }

        public void ShowRotulosJugador(Jugador jugador)
        {
            List<ICommandShowable> rotulos = new List<ICommandShowable>();

            rotulos.Add(new PlayerCommand(jugador, " "));
            rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Command.Posicion));
            //rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Goles, PlayerCommand.Partidos, PlayerCommand.PartidosEnd));
            //rotulos.Add(new PlayerCommand(jugador, jugador.Goals, jugador.Matches));
            rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Command.GolesPartidos));

            if (jugador.Faltas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Faltas.Count != 1 ?
                    PlayerCommand.Command.Faltas : PlayerCommand.Command.Falta));

            if ((jugador.Goles.Count + jugador.GolesPenalty.Count) > 0)
            {
                rotulos.Add(new PlayerCommand(jugador, (jugador.Goles.Count + jugador.GolesPenalty.Count) != 1 ?
                    PlayerCommand.Command.Goles : PlayerCommand.Command.Gol));
                rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Command.GolesTiros));
                rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Command.GolesTirosEficiencia));
            }

            if (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count > 0)
            {
                rotulos.Add(new PlayerCommand(jugador, jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count != 1 ?
                    PlayerCommand.Command.TirosAPuerta : PlayerCommand.Command.TiroAPuerta));
                rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Command.TirosAPuertaSolo));
                rotulos.Add(new PlayerCommand(jugador, PlayerCommand.Command.TirosTotales));
            }

            if (jugador.Paradas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Paradas.Count != 1 ?
                    PlayerCommand.Command.Paradas : PlayerCommand.Command.Parada));

            if (jugador.TAmarillas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.TAmarillas.Count != 1 ?
                    PlayerCommand.Command.TAmarillas : PlayerCommand.Command.TAmarilla));

            if (jugador.TRojas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.TRojas.Count != 1 ?
                    PlayerCommand.Command.TRojas : PlayerCommand.Command.TRoja));

            if (jugador.SancionSiAmarilla)
                rotulos.Add(new PlayerCommand(jugador, _idiomas[0].MissesNextMatch));

            _gui.activaAcciones(false, false, false);
            _gui.ConfigListBox(rotulos.ToArray());
        }

        #endregion

        // ================================= CRONO =================================
        #region Crono

        public void CronoShow(bool mostrar)
        {
            _crono.ShowCronoIpf(mostrar);
        }

        // LLamada al hacer click en Empezar
        public void CronoEmpezar()
        {
            if (_config.modeOcrActivated)           // Si se ha elegido arrancar en modo OCR la aplicación
            {
                ocr.pauseDataProcessing = false;    // Despausa el procesamiento de datos del OCR en caso de estar pausado
                if (!ocr.isRunnig)
                    ocr.start();
            }

            _crono.Empezar();

            if (_crono.GetMomento().Parte == Momento.IniParte2 && _crono.GetMomento().isStart && !_gui.GetConfigData().isMachineCrono)
            {
                _datos.EquipoL.Faltas.Clear();
                _datos.EquipoV.Faltas.Clear();

                for (int i = 0; i < _config.NumIpf; i++)
                {
                    if (Program.EstaActivado(i))
                    {
                        _ipfs[i].Envia("Falta(['" + _datos.EquipoL.TeamCode + "', '" + _datos.EquipoL.Faltas.Count + "'])");
                        _ipfs[i].Envia("Falta(['" + _datos.EquipoV.TeamCode + "', '" + _datos.EquipoV.Faltas.Count + "'])");
                    }
                }
            }

            if (_crono.GetMomento().Parte == Momento.Penaltis)
            {
                ListBoxConfig(Penaltis);
            }
            else // En el juego normal se activa el control de posesion
            {
                PosesionActivar(true);
            }

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("CronoStart\n");
            }
        }

        public void CronoParar()
        {
            if (_config.modeOcrActivated)           // Si se ha elegido arrancar en modo OCR la aplicación
                ocr.pauseDataProcessing = true;     // Pausa el procesamiento de datos del OCR en caso de estar en marcha

            _crono.Parar();

            PosesionActivar(false);

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("CronoStop\n");
            }
        }

        public void CronoFinParte()
        {
            if (_config.modeOcrActivated)           // Si se ha elegido arrancar en modo OCR la aplicación
                ocr.pauseDataProcessing = true;     // Pausa el procesamiento de datos del OCR en caso de estar en marcha

            PosesionActivar(false);
            _crono.FinParte();

            // Genera los Tiempos Extra por defecto
            configTiemposExtra();

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("CronoEndPart\n");
            }
        }

        public void CronoFinPartido()
        {
            if (_config.modeOcrActivated)           // Si se ha elegido arrancar en modo OCR la aplicación
                ocr.pauseDataProcessing = true;     // Pausa el procesamiento de datos del OCR en caso de estar en marcha

            PosesionActivar(false);
            _crono.FinPartido();

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("CronoEndGame\n");
            }
        }

        public void CronoReset()
        {
            PosesionActivar(false);
            _crono.Reset();

            if (_crono.GetMomento().Parte == Momento.Penaltis)
            {
                ListBoxConfig(Vacio);
            }

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("CronoReset\n");
            }
        }

        public void CronoEdit() // Llamada al intentar configurar el crono con su botón destinado a ello
        {
            TiempoForm tf = new TiempoForm(_crono.GetMomento(), _config.duracionParte_i, _config.duracionProrroga_i, true);
            tf.ShowDialog(_gui);

            if (tf.getMomento() != null)
                CronoEdit(tf.getMomento(), true);
        }

        public void CronoEdit(string minutes, string seconds)
        {
            int min = 0;
            int sec = 0;
            int.TryParse(minutes, out min);
            int.TryParse(seconds, out sec);
            Momento cronoMomento = _crono.GetMomento();
            cronoMomento._cadena_minuto = min;
            cronoMomento._cadena_segundo = sec;
            CronoEdit(cronoMomento, false);
        }

        public static int parteDeReferencia = -1;
        public void CronoEdit(string minutes, string seconds, int parte)
        {
            int min = 0;
            int sec = 0;
            int.TryParse(minutes, out min);
            int.TryParse(seconds, out sec);
            Momento cronoMomento = _crono.GetMomento();
            cronoMomento._cadena_minuto = min;
            cronoMomento._cadena_segundo = sec;
            cronoMomento.Parte = parte;
            if (parteDeReferencia != parte)
                CronoEdit(cronoMomento, true);
            else if (parteDeReferencia == parte)
                CronoEdit(cronoMomento, false);

            parteDeReferencia = parte;
        }

        void CronoEdit(Momento momento, bool seguimientoParte)
        {
            if (seguimientoParte)
                _crono.SetMomentoCambioDeParte(momento);
            else
                _crono.SetMomento(momento);

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("CronoChanged," + momento.GetMinutoCrono() + "," + momento.GetSegundo() + "," + momento.Parte + "\n");
            }
        }

        public int CronoMinutes()
        {
            Momento cronoMomento = _crono.GetMomento();
            return (Momento.Minuto[cronoMomento.Parte] - cronoMomento.GetMinutoCrono());
        }

        public int CronoSeconds()
        {
            Momento cronoMomento = _crono.GetMomento();
            return (60 - cronoMomento.GetSegundo());
        }

        public bool IsPlay()
        {
            return _crono.IsPlay() || AsynchronousSocketListener.hasClient;
        }

        public static bool llamadaDesdeCronoApp;  // Varaible estática usada para saber si la función Empezar() de la clase Crono ha sido llamda desde la aplicación arrancada en modo Principal o en modo Cronómeto
        void CronoClientDataReceived(string data)
        {
            Console.WriteLine("*** Ha llegado: " + data + " ***");
            
            if (data.Contains("CronoStop"))
            {
                CronoParar();
            }
            else if (data.Contains("CronoStart"))
            {
                llamadaDesdeCronoApp = true;
                if ((_crono.GetMomento().Parte + 1) != Momento.Penaltis && _crono.GetMomento().Parte != Momento.Penaltis)
                    CronoEmpezar();
                llamadaDesdeCronoApp = false;
            }
            else if (data.Contains("CronoEndPart"))
            {
                CronoFinParte();
            }
            else if (data.Contains("CronoEndGame"))
            {
                CronoFinPartido();
            }
            else if (data.Contains("CronoReset"))
            {
                CronoReset();
            }
            else if (data.Contains("CronoChanged"))
            {
                string[] dataSplit = data.Split(',');
                string min = dataSplit[1];
                string sec = dataSplit[2];
                int parte = int.Parse(dataSplit[3]);
                llamadaDesdeCronoApp = true;
                CronoEdit(min, sec, parte);
                llamadaDesdeCronoApp = false;
            }
        }

        #endregion

        // =============================== PREVIEWS ================================
        #region Métodos Preview

        public void previewScore()
        {
            OnAirSetup(new ScoreBoardCommand(_datos.EquipoL, _datos.EquipoV));
        }


        public void previewTeamLineUpCrawl(bool local)
        {
            if (local)
            {
                OnAirSetup(new TeamLineUpCrawlCommand(_datos.EquipoL));
            }
            else
            {
                OnAirSetup(new TeamLineUpCrawlCommand(_datos.EquipoV));
            }
        }

        public void previewTeamLineUp(bool local)
        {
            if (local)
            {
                OnAirSetup(new TeamLineUpCommand(_datos.EquipoL));
            }
            else
            {
                OnAirSetup(new TeamLineUpCommand(_datos.EquipoV));
            }
        }

        public void previewTeamLayout(bool local)
        {
            if (local)
            {
                OnAirSetup(new TeamLayoutCommand(_datos.EquipoL));
            }
            else
            {
                OnAirSetup(new TeamLayoutCommand(_datos.EquipoV));
            }
        }

        public void previewTeamLayout3d(bool local)
        {
            if (local)
            {
                OnAirSetup(new TeamLayout3dCommand(_datos.EquipoL));
            }
            else
            {
                OnAirSetup(new TeamLayout3dCommand(_datos.EquipoV));
            }
        }

        public void previewTeamReserves(bool local)
        {
            if (local)
            {
                OnAirSetup(new TeamReservesCommand(_datos.EquipoL));
            }
            else
            {
                OnAirSetup(new TeamReservesCommand(_datos.EquipoV));
            }
        }

        public void previewTeamsReserves()
        {
            OnAirSetup(new TeamsReservesCommand(_datos.EquipoL, _datos.EquipoV));
        }

        public void previewStatisticsTable()
        {
            OnAirSetup(new StatisticsTableCommand(_crono.GetMomento(), _datos.EquipoL, _datos.EquipoV, _datos.Posesion));
        }

        public void previewPresentation()
        {
            ListBoxConfig(Controlador.Historial);
            //OnAirSetup(new PresentationCommand(_datos.EquipoL.Campo, _datos.EquipoL, _datos.EquipoV));
            OnAirSetup(new PresentationCommand(_datos.DatosEncuentro, _datos.EquipoL, _datos.EquipoV));
        }

        public void TimeOut()
        {
            OnAirSetup(new TimeOutCommand(null));
        }

        public void LocalTimeOut()
        {
            OnAirSetup(new TimeOutCommand(_datos.EquipoL));
        }

        public void VisitorTimeOut()
        {
            OnAirSetup(new TimeOutCommand(_datos.EquipoV));
        }

        public void previewPosesion()
        {
            OnAirSetup(new StatisticsCommand(_idiomas[0].Possesion,
                 _datos.Posesion.getPorcentajeLocal(),
                 _datos.Posesion.getPorcentajeVisitante(),
                 _datos.EquipoL, _datos.EquipoV));
        }

        public bool PorteroJugador(bool local)
        {
            PJ_onAir = !PJ_onAir;
            if (local)
                executeItem(new PorteroJugadorCommand(_datos.EquipoL, PJ_onAir));
            else
                executeItem(new PorteroJugadorCommand(_datos.EquipoV, PJ_onAir));
           
            return PJ_onAir;
        }


        #endregion

    }
}
