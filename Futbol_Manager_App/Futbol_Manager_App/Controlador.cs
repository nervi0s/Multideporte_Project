using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Comandos;
using Futbol_Manager_App.Interfaz;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App
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
        public const int FaltasRecibidas = 4;
        public const int FaltasCometidas = 41;
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
        public const int DrinksBreak = 117;
        public const int CambioDoble = 12;
        public const int Crono = 13;
        public const int Estadisticas = 14;
        public const int Stats = 90;
        public const int PasesCompletados = 70;
        public const int PasesTotales = 71;
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
        public const int PitchConditions = 30;
        public const int InfoCrawler = 31;
        public const int GroupStanding = 32;
        public const int Highligths = 33;
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


        //List<ICommand> exchange= new List<ICommand>();
        //NewsExchangeFeed command = new NewsExchangeFeed(new Exchange("News Exchange Feed","Linea1"));



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

        // Configuración inicial de los atributos
        private void setup()
        {
            // GUI
            _gui = new MainForm(this);

            // Carga configuración
            _config = PersistenciaUtil.CargaConfig();

            // Carga los ficheros de idiomas de los Ipfs configurados
            for (int i = 0; i < _config.NumIpf; i++)
            {
                switch(i)
                {
                    case 0: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero); break;
                    case 1: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero2); break;
                    case 2: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero3); break;
                    case 3: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero4); break;
                    case 4: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero5); break;
                    case 5: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero6); break;
                    case 6: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero7); break;
                    case 7: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero8); break;
                    case 8: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero9); break;
                    case 9: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"futbol\" + _config.IdiomaFichero10); break;
                    default: break;
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

                case CoolingBreak:
                    OnAirSetup(new CoolingBreakCommand());
                    break;

                case DrinksBreak:
                    OnAirSetup(new DrinksBreakCommand());
                    break;

                case PasesCompletados:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.PasesCompletados, _datos.EquipoL, _datos.EquipoV));
                    break;

                case PasesTotales:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.PasesTotales, _datos.EquipoL, _datos.EquipoV));
                    break;

                case FaltasCometidas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.FaltasCometidas, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case FaltasRecibidas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.FaltasRecibidas, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Goles:
                case GolesPenalti:
                case GolesPP:
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;
                
                case TirosAPuerta:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuerta, _datos.EquipoL, _datos.EquipoV));
                    break;

                case TirosAPuertaDentro:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuertaDentro, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case TirosAPuertaFuera:
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case TirosAPuertaComp:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosAPuertaComp, _datos.EquipoL, _datos.EquipoV));
                    break;

                case Paradas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Paradas, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        //Activa porteros
                        _gui.ActivaJugadores(true, getPorteros(true));
                        _gui.ActivaJugadores(false, getPorteros(false));
                    }
                    break;

                case TAmarillas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TAmarillas, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaAsistentes(true);
                    _gui.ActivaJugadores(true, getJugadores(true));
                    _gui.ActivaJugadores(false, getJugadores(false));
                    break;

                case TRojas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TRojas, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaAsistentes(true);
                    _gui.ActivaJugadores(true, getJugadores(true));
                    _gui.ActivaJugadores(false, getJugadores(false));
                    break;

                case FuerasDeJuego:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.FuerasDeJuego, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Cambio:
                    ResetCambio();

                    if (Program.CambioEscogido==-1)
                        OnAirSetup(new StatisticsCommand(StatisticsCommand.Cambios, _datos.EquipoL, _datos.EquipoV));

                    //Activa banquillos
                    _gui.ActivaJugadores(true, getBanquillo(true));
                    _gui.ActivaJugadores(false, getBanquillo(false));

                    _gui.ActivaTabJugadores(false);
                    break;

                case CambioDoble:
                    ResetCambio();

                    //Activa banquillos
                    _gui.ActivaJugadores(false, getBanquillo(false));
                    _gui.ActivaJugadores(true, getBanquillo(true));

                    _gui.ActivaTabJugadores(false);
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
        
        // Devuelve los porteros titulares no expulsados del equipo indicado
        private List<int> getPorteros(bool eqLocal)
        {
            Equipo equipo = (eqLocal ? _datos.EquipoL : _datos.EquipoV);
            List<int> dorsales = new List<int>();

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

            for (int i = 1; i < n; i++)
                conectado = conectado && _ipfs[1].Conectar();

                if (conectado)
                {
                    for (int i = 0; i < n; i++)
                        switch (i)
                        {
                            case 0: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast, _datos.ResultadoIda); break;
                            case 1: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast2, _datos.ResultadoIda); break;
                            case 2: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast3, _datos.ResultadoIda); break;
                            case 3: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast4, _datos.ResultadoIda); break;
                            case 4: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast5, _datos.ResultadoIda); break;
                            case 5: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast6, _datos.ResultadoIda); break;
                            case 6: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast7, _datos.ResultadoIda); break;
                            case 7: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast8, _datos.ResultadoIda); break;
                            case 8: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast9, _datos.ResultadoIda); break;
                            case 9: _ipfs[i].ConfigInicial(_datos.EquipoL, _datos.EquipoV, _idiomas[i].ExtraTime, _config.Multicast10, _datos.ResultadoIda); break;
                            default: break;
                        }

                    _crono.UpdateIpf();
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

                case FaltasRecibidas:
                    addListBoxItem(new FaltaRecibidaCommand(_crono.GetMomento(), jugador));
                    break;

                case FaltasCometidas:
                    addListBoxItem(new FaltaCometidaCommand(_crono.GetMomento(), jugador));
                    break;

                case Goles:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), jugador));

                    _gui.AbrirRotulosJugador();
                    botonJugador.PerformClick();
                    break;

                case GolesPP:
                    addListBoxItem(new GolPPCommand(_crono.GetMomento(), jugador));
                    break;

                case GolesPenalti:
                    addListBoxItem(new GolPenaltyCommand(_crono.GetMomento(), jugador));

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
                    break;

                case FuerasDeJuego:
                    addListBoxItem(new FueraDeJuegoCommand(_crono.GetMomento(), jugador));
                    break;

                case Cambio:
                    HacerCambio(jugador);
                    break;

                case CambioDoble:
                    HacerCambioDoble(jugador);
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

                case FaltasRecibidas:
                    addListBoxItem(new FaltaRecibidaCommand(_crono.GetMomento(), equipo));
                    break;

                case FaltasCometidas:
                    addListBoxItem(new FaltaCometidaCommand(_crono.GetMomento(), equipo));
                    break;

                case Goles:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), equipo));
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

                case FuerasDeJuego:
                    addListBoxItem(new FueraDeJuegoCommand(_crono.GetMomento(), equipo));
                    break;
            }
        }

        public void HacerCambio(Jugador jugador)
        {
            
            if (_cambio[0] == null)
            {
                _cambio[0] = jugador;
                
                //Activa solo plantilla del equipo
                _gui.ActivaJugadores(jugador.Equipo.Local, getTitulares(jugador.Equipo.Local));
                _gui.ActivaJugadores(!jugador.Equipo.Local, new List<int>());
            }
            else
            {
                addListBoxItem(new ChangeCommandOptions(_crono.GetMomento(), _cambio[0], jugador,this._cambioEscogido));
                    
                if (jugador.Equipo.Local)
                {
                    _gui.ConfigEquipoLocal(jugador.Equipo);
                }
                else
                {
                    _gui.ConfigEquipoVisitante(jugador.Equipo);
                }
                ResetCambio();
                _gui.ActivaJugadores(jugador.Equipo.Local, new List<int>());
            }
            _gui.ActivaTabJugadores(true);
        }
        
        public void HacerCambioDoble(Jugador jugador)
        {
            if (_cambio[0] == null)
            {
                _cambio[0] = jugador;

                // Desactiva el jugador
                List<int> dorsales = getBanquillo(jugador.Equipo.Local);
                dorsales.Remove(jugador.Number);
                _gui.ActivaJugadores(jugador.Equipo.Local, dorsales);

                // Desactiva el otro equipo
                _gui.ActivaJugadores(!jugador.Equipo.Local, new List<int>());
            }
            else if (_cambio[1] == null)
            {
                _cambio[1] = jugador;

                //Activa solo plantilla del equipo
                _gui.ActivaJugadores(jugador.Equipo.Local, getTitulares(jugador.Equipo.Local));
                _gui.ActivaJugadores(!jugador.Equipo.Local, new List<int>());

                _gui.ActivaTabJugadores(true);
            }
            else if (_cambio[2] == null)
            {
                _cambio[2] = jugador;

                // Desactiva el jugador
                List<int> dorsales = getTitulares(jugador.Equipo.Local);
                dorsales.Remove(jugador.Number);
                _gui.ActivaJugadores(jugador.Equipo.Local, dorsales);
            }
            else
            {
                addListBoxItem(new DoubleChangeCommand(_crono.GetMomento(), _cambio[0], _cambio[1], _cambio[2], jugador));
                if (jugador.Equipo.Local)
                {
                    _gui.ConfigEquipoLocal(jugador.Equipo);
                }
                else
                {
                    _gui.ConfigEquipoVisitante(jugador.Equipo);
                }
                ResetCambio();
                _gui.ActivaJugadores(jugador.Equipo.Local, new List<int>());
            }
        }
        
        public void ResetCambio()
        {
            _cambio[0] = null;
            _cambio[1] = null;
            _cambio[2] = null;
        }

        #endregion

        // ================================= ONAIR =================================
        #region OnAir

        public void OnAirSetup(ICommandShowable command)
        {
            bool conectado = _ipfs[0].Conectado();
            for (int i = 1; i < _config.NumIpf; i++)
            {
                if (Program.IpfsSeleccionados[i]==true)
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
                //Console.WriteLine(_comandoOnAir.GetType());
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
            if (_onListBox == _plantillas.Plantillas)
            {
                PlantillaForm pf = new PlantillaForm();
                pf.ShowDialog();

                if (pf.getPlantilla() != null)
                    addListBoxItem(pf.getPlantilla());
            }

            else if (_onListBox == _plantillas.Countdowns)  // Countdowns
            {
                CountdownForm cdf = new CountdownForm();
                cdf.ShowDialog();

                if (cdf.GetCountdown() != null)
                    addListBoxItem(new CountdownCommand(cdf.GetCountdown()));
            }

            else if (_onListBox == _plantillas.Prematchs)  // PreMatchs
            {
                PrematchForm cdf = new PrematchForm();
                cdf.ShowDialog();

                if (cdf.GetPrematch() != null)
                    addListBoxItem(new PreMatchCommand(cdf.GetPrematch()));
            }

            else if (_onListBox == _plantillas.Exchange)  
            {
                ExchangeForm exf = new ExchangeForm();
                exf.ShowDialog();

                if (exf.GetExchange() != null)
                    addListBoxItem(new NewsExchangeFeed(exf.GetExchange()));
            }

            else if (_onListBox == _plantillas.emergencyCaptions)
            {
                EmergencyCaptionsForm exf = new EmergencyCaptionsForm();
                exf.ShowDialog();

                if (exf.GetEmergencyCaption() != null)
                    addListBoxItem(new EmergencyCaptionsCommand(exf.GetEmergencyCaption()));
            }

            else if (_onListBox == _plantillas.EndToEndTest)
            {
                EndToEndForm exf = new EndToEndForm();
                exf.ShowDialog();

                if (exf.GetEndToEnd() != null)
                    addListBoxItem(new EndToEndCommand(exf.GetEndToEnd()));
            }

            else if (_onListBox == _plantillas.postInterview)
            {
                PostMultiFlashInterviewForm exf = new PostMultiFlashInterviewForm();
                exf.ShowDialog();

                if (exf.GetPostInterview() != null)
                    addListBoxItem(new PostInterviewCommand(exf.GetPostInterview()));
            }

            else if (_onListBox == _plantillas.endPostInterview)
            {
                EndOfMultiFlashInterviewForm exf = new EndOfMultiFlashInterviewForm();
                exf.ShowDialog();

                if (exf.GetEndOfInterview() != null)
                    addListBoxItem(new EndOfPostInterviewCommand(exf.GetEndOfInterview()));
            }

            else if (_onListBox == _plantillas.Weather)
            {
                WeatherForm exf = new WeatherForm();
                exf.ShowDialog();

                if (exf.GetWeather() != null)
                    addListBoxItem(new WeatherCommand(exf.GetWeather()));
            }

            else if (_onListBox == _plantillas.PitchConditions)
            {
                PitchConditionsForm exf = new PitchConditionsForm();
                exf.ShowDialog();

                if (exf.GetPitchConditions() != null)
                    addListBoxItem(new PitchConditionsCommand(exf.GetPitchConditions()));
            }

            else if (_onListBox == _plantillas.InfoCrawler)
            {
                InfoCrawlerForm exf = new InfoCrawlerForm();
                exf.ShowDialog();

                if (exf.GetInfoCrawler() != null)
                    addListBoxItem(new InfoCrawlerCommand(exf.GetInfoCrawler()));
            }

            else if (_onListBox == _plantillas.GroupStanding)
            {
                GroupStandingForm exf = new GroupStandingForm();
                exf.ShowDialog();

                if (exf.GetGroupStanding() != null)
                    addListBoxItem(new GroupStandingCommand(exf.GetGroupStanding()));
            }

            else // Tiempo Extra
            {
                ExtraForm ef = new ExtraForm();
                ef.ShowDialog();

                if (ef.getMinutos() != -1)
                    addListBoxItem(new ExtraTimeCommand(ef.getMinutos()));
            }
        }

        private void addListBoxItem(ICommand command)
        {
            _onListBox.Insert(0, command);
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

                // Refresca los comandos OnAir
                if (_comandoOnAir != null && !_onAir)
                    _gui.SetOnAirText(_comandoOnAir.ToString());
                if (_comandoNext != null)
                    _gui.SetNextAirText(_comandoNext.ToString());
            }

            if(command is ICommandImmediateExecutable)
            {
                ((ICommandImmediateExecutable)command).ExecuteImmediate(_ipfs, _idiomas, _config.NumIpf);
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

                    // Si es un cambio se comprueba que sea posible deshacerlo
                    if (item is ChangeCommand)
                    {
                        posible = ((ChangeCommand)item).CheckUndo(out msgError);
                    }
                    if (item is DoubleChangeCommand)
                    {
                        posible = ((DoubleChangeCommand)item).CheckUndo(out msgError);
                    }

                    if (item is ChangeCommandOptions)
                    {
                        posible = ((ChangeCommandOptions)item).CheckUndo(out msgError);
                    }

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
                if (command is ChangeCommand || command is DoubleChangeCommand || command is ChangeCommandOptions)
                {
                    _gui.ConfigEquipoLocal(_datos.EquipoL);
                    _gui.ConfigEquipoVisitante(_datos.EquipoV);
                    // Refresca
                    SetSeccion(_seccionActual);
                }
                if (command is YellowCardCommand || command is RedCardCommand)
                {
                    // Refresca para posible reactivación de un jugador
                    SetSeccion(_seccionActual);
                }

            }

            if (command is ICommandImmediateExecutable)
            {
                ((ICommandImmediateExecutable)command).UndoImmediate(_ipfs, _idiomas, _config.NumIpf);
            }

            // Backup
            PersistenciaUtil.GuardaBackup(_datos);
        }

        public void ListBoxEditItem(ICommand command)
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
            else if (command is CountdownCommand)
            {
                CountdownCommand cdc = (CountdownCommand)command;

                CountdownForm cdf = new CountdownForm(cdc.Countdown);
                cdf.ShowDialog();

                if (cdf.GetCountdown() != null)
                {
                    cdc.Countdown = cdf.GetCountdown();
                    _gui.Refresh();
                }

            }
            else if (command is PreMatchCommand)
            {
                PreMatchCommand pmc = (PreMatchCommand)command;

                PrematchForm pmf = new PrematchForm(pmc.Prematch);
                pmf.ShowDialog();

                if (pmf.GetPrematch() != null)
                {
                    pmc.Prematch = pmf.GetPrematch();
                    _gui.Refresh();
                }

            }
            else if (command is PostInterviewCommand)
            {
                PostInterviewCommand pmc = (PostInterviewCommand)command;

                PostMultiFlashInterviewForm pmf = new PostMultiFlashInterviewForm(pmc.postInterview);
                pmf.ShowDialog();

                if (pmf.GetPostInterview() != null)
                {
                    pmc.postInterview = pmf.GetPostInterview();
                    _gui.Refresh();
                }

            }
            else if (command is EndOfPostInterviewCommand)
            {
                EndOfPostInterviewCommand pmc = (EndOfPostInterviewCommand)command;

                EndOfMultiFlashInterviewForm pmf = new EndOfMultiFlashInterviewForm(pmc.EndOfpostInterview);
                pmf.ShowDialog();

                if (pmf.GetEndOfInterview() != null)
                {
                    pmc.EndOfpostInterview = pmf.GetEndOfInterview();
                    _gui.Refresh();
                }

            }


            else if (command is EmergencyCaptionsCommand)
            {
                EmergencyCaptionsCommand pmc = (EmergencyCaptionsCommand)command;

                EmergencyCaptionsForm pmf = new EmergencyCaptionsForm(pmc.emergencyCaption);
                pmf.ShowDialog();

                if (pmf.GetEmergencyCaption() != null)
                {
                    pmc.emergencyCaption = pmf.GetEmergencyCaption();
                    _gui.Refresh();
                }

            }
            else if (command is NewsExchangeFeed)
            {
                
               NewsExchangeFeed exc = (NewsExchangeFeed)command;

               //Exchange e = exc.Exchange;
               ExchangeForm efm = new ExchangeForm(exc.Exchange);

               efm.ShowDialog();

               if (efm.GetExchange()!= null)
               {
                  exc.Exchange = efm.GetExchange();
                   _gui.Refresh();
               }
            }

            else if (command is WeatherCommand)
            {

                WeatherCommand exc = (WeatherCommand)command;

                WeatherForm efm = new WeatherForm(exc.weather);

                efm.ShowDialog();

                if (efm.GetWeather() != null)
                {
                    exc.weather = efm.GetWeather();
                    _gui.Refresh();
                }
            }

            else if (command is PitchConditionsCommand)
            {

                PitchConditionsCommand exc = (PitchConditionsCommand)command;

                PitchConditionsForm efm = new PitchConditionsForm(exc.pitchConditions);

                efm.ShowDialog();

                if (efm.GetPitchConditions() != null)
                {
                    exc.pitchConditions = efm.GetPitchConditions();
                    _gui.Refresh();
                }
            }

            else if (command is InfoCrawlerCommand)
            {

                InfoCrawlerCommand exc = (InfoCrawlerCommand)command;

                InfoCrawlerForm efm = new InfoCrawlerForm(exc.infoCrawler);

                efm.ShowDialog();

                if (efm.GetInfoCrawler() != null)
                {
                    exc.infoCrawler = efm.GetInfoCrawler();
                    _gui.Refresh();
                }
            }
            else if (command is GroupStandingCommand)
            {

                GroupStandingCommand exc = (GroupStandingCommand)command;

                GroupStandingForm efm = new GroupStandingForm(exc.groupStanding);

                efm.ShowDialog();

                if (efm.GetGroupStanding() != null)
                {
                    exc.groupStanding = efm.GetGroupStanding();
                    _gui.Refresh();
                }
            }

            else if (command is EndToEndCommand)
            {

                EndToEndCommand exc = (EndToEndCommand)command;

                //Exchange e = exc.Exchange;
                EndToEndForm efm = new EndToEndForm(exc.endToEnd);

                efm.ShowDialog();

                if (efm.GetEndToEnd() != null)
                {
                    exc.endToEnd = efm.GetEndToEnd();
                    _gui.Refresh();
                }
            }
            else // ICommandExecutable
            {
                ICommandExecutable ce = (ICommandExecutable)command;

                TiempoForm tf = new TiempoForm(ce.Momento);
                tf.ShowDialog(_gui);

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

                case (PreMatchs):
                    _onListBox = _plantillas.Prematchs;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (NewsExchangeFeed):
                    _onListBox = _plantillas.Exchange;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (EndToEndTest):

                    _onListBox = _plantillas.EndToEndTest;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (EmergencyCaptions):

                    _onListBox = _plantillas.emergencyCaptions;
                    _gui.activaAcciones(true, true, true);
                    break;
                    
                case (Weather):
                    _onListBox = _plantillas.Weather;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (PostMultiFlashInterview):

                    _onListBox = _plantillas.postInterview;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (EndOfPostMultiFlashInterview):

                    _onListBox = _plantillas.endPostInterview;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (PitchConditions):
                    if (_plantillas.PitchConditions == null)
                        _plantillas.PitchConditions = new List<ICommand>();
                    _onListBox = _plantillas.PitchConditions;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (InfoCrawler):
                    if (_plantillas.InfoCrawler == null)
                        _plantillas.InfoCrawler = new List<ICommand>();
                    _onListBox = _plantillas.InfoCrawler;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (GroupStanding):
                    if (_plantillas.GroupStanding == null)
                        _plantillas.GroupStanding = new List<ICommand>();

                    _onListBox = _plantillas.GroupStanding;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Countdowns):
                    _onListBox = _plantillas.Countdowns;
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

                case (TiemposExtra):
                    _onListBox = _tiemposExtra;
                    if (_onListBox.Count == 0)
                        ListBoxNewItem();
                    else
                        ListBoxEditItem(_onListBox[0]);
                    if(_onListBox[0] is ICommandShowable)
                        OnAirSetup(_onListBox[0] as ICommandShowable);
                    _gui.activaAcciones(false, true, false);
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

            if (jugador.FaltasRecibidas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.FaltasRecibidas.Count != 1 ? 
                    PlayerCommand.Command.FaltasRecibidas : PlayerCommand.Command.FaltaRecibida));

            if (jugador.FaltasCometidas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.FaltasCometidas.Count != 1 ? 
                    PlayerCommand.Command.FaltasCometidas : PlayerCommand.Command.FaltaCometida));

            if ((jugador.Goles.Count + jugador.GolesPenalty.Count) > 0)
                rotulos.Add(new PlayerCommand(jugador, (jugador.Goles.Count + jugador.GolesPenalty.Count) != 1 ? 
                    PlayerCommand.Command.Goles : PlayerCommand.Command.Gol));

            if (jugador.GolesPP.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.GolesPP.Count != 1 ? 
                    PlayerCommand.Command.GolesPP : PlayerCommand.Command.GolPP));

            if (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count != 1 ? 
                    PlayerCommand.Command.TirosTotales : PlayerCommand.Command.TiroTotales));

            if (jugador.Tirosapuerta.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Tirosapuerta.Count != 1 ?
                    PlayerCommand.Command.TirosAPorteria : PlayerCommand.Command.TiroAPorteria));

            if (jugador.Paradas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Paradas.Count != 1 ? 
                    PlayerCommand.Command.Paradas : PlayerCommand.Command.Parada));

            if (jugador.TAmarillas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.TAmarillas.Count != 1 ? 
                    PlayerCommand.Command.TAmarillas : PlayerCommand.Command.TAmarilla));

            if (jugador.TRojas.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.TRojas.Count != 1 ? 
                    PlayerCommand.Command.TRojas : PlayerCommand.Command.TRoja));

            if (jugador.Fuerasdejuego.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Fuerasdejuego.Count != 1 ? 
                    PlayerCommand.Command.FuerasDeJuego : PlayerCommand.Command.FueraDeJuego));

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

        public void CronoEmpezar()
        {
            _crono.Empezar();

            if (_crono.GetMomento().Parte == Momento.Penaltis)
            {
                ListBoxConfig(Penaltis);
            }
            else // En el juego normal se activa el control de posesion
            {
                PosesionActivar(true);
            }
        }
        public void CronoFinParte()
        {
            PosesionActivar(false);
            _crono.FinParte();

            // Genera los Tiempos Extra por defecto
            configTiemposExtra();
        }
        public void CronoFinPartido()
        {
            PosesionActivar(false);
            _crono.FinPartido();
        }
        public void CronoReset()
        {
            PosesionActivar(false);
            _crono.Reset();

            if (_crono.GetMomento().Parte == Momento.Penaltis)
            {
                ListBoxConfig(Vacio);
            }

        }

        public void CronoEdit()
        {
            TiempoForm tf = new TiempoForm(_crono.GetMomento());
            tf.ShowDialog(_gui);

            if (tf.getMomento() != null)
                _crono.SetMomento(tf.getMomento());
        }

        public bool IsPlay()
        {
            return _crono.IsPlay();
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

        public void previewTeamLineUpSubstitutes(bool local)
        {
            if (local)
            {
                OnAirSetup(new TeamLineUpSubstitutesCommand(_datos.EquipoL));
            }
            else
            {
                OnAirSetup(new TeamLineUpSubstitutesCommand(_datos.EquipoV));
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
            OnAirSetup(new StatisticsTableCommand(_crono.GetMomento(), _datos.EquipoL, _datos.EquipoV,_datos.Posesion.getPorcentajeLocal(),_datos.Posesion.getPorcentajeVisitante()));
        }

        public void previewPresentation()
        {
            ListBoxConfig(Controlador.Historial);
            OnAirSetup(new PresentationCommand(_datos.DatosEncuentro, _datos.EquipoL, _datos.EquipoV));
        }

        public void previewPosesion()
        {
            OnAirSetup(new StatisticsCommand(_idiomas[0].Possesion,
                 _datos.Posesion.getPorcentajeLocal(),
                 _datos.Posesion.getPorcentajeVisitante(),
                 _datos.EquipoL, _datos.EquipoV));
        }

        public void previewHighlights()
        {
            ListBoxConfig(Controlador.Highligths);
            OnAirSetup(new HighligthsCommand());
        }

        #endregion

    }
}
