using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Futbol_Sala_Manager_App;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Comandos;
using Balonmano_Manager_App.Interfaz;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App
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
        //public const int Corners = 3;
        //public const int FaltasRecibidas = 4;
        //public const int FaltasCometidas = 41;
        public const int Goles = 5;
        public const int GolesContraataque = 51;
        public const int GolesPenalti = 52;
        public const int TirosAPuerta = 6;
        public const int TirosAPuertaDentro = 61;
        public const int TirosAPuertaFuera = 62;
        public const int TirosAPuertaComp = 63;
        public const int Tiros = 601;
        public const int TirosPenalti = 602;
        public const int TirosContraataque = 603;
        public const int Paradas = 7;
        public const int Perdidas = 150;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int TAzules = 10;
        //public const int FuerasDeJuego = 10;
        //public const int Cambio = 11;
        //public const int CambioIn = 111;
        //public const int CambioOut = 112;
        //public const int CambioInInfo = 113;
        //public const int CambioOutInfo = 114;
        //public const int CambioInfo = 115;
        //public const int CoolingBreak = 116;
        //public const int CambioDoble = 12;
        public const int Crono = 13;
        public const int Estadisticas = 14;
        public const int Stats = 90;
        public const int PasesCompletados = 70;
        public const int PasesTotales = 71;


        // Secciones Balonmano
        public const int Handball_Presentacion  = 700;
        public const int Handball_Testadisticas = 701;
        public const int Handball_Rotulos       = 702;

        public const int Handball_Goles         = 703;
        public const int Attacks       = 704;
        public const int Handball_Fieldthrows   = 705;
        public const int Handball_FieldGoal     = 706;
        public const int Handball_Sevenmthrows  = 707;
        public const int Handball_SevenGoal     = 708;
        public const int Fastbreaks    = 709;
        public const int Turnovers     = 710;
        //public const int Handball_Tarjetas      = 711;
        //public const int Handball_Tarjetas_B    = 712;
        //public const int Handball_Tarjetas_R    = 713;
        //public const int Handball_Tarjetas_Y    = 714;
        public const int Exclusions    = 715;

        #endregion

        #region Constantes para ListBox

        public const int Historial = 15;
        public const int Plantillas = 16;
        public const int Arbitros = 17;
        public const int Comentaristas = 18;
        public const int TiemposExtra = 19;
        //public const int PreMatchs = 20;
        //public const int Countdowns = 21;
        public const int Penaltis = 22;
        public const int Vacio = 23;
        //public const int NewsExchangeFeed = 24;
        //public const int EndToEndTest = 25;
        //public const int Weather = 26;
        //public const int PostMultiFlashInterview = 27;
        //public const int EndOfPostMultiFlashInterview = 28;
        public const int EmergencyCaptions = 29;
        //public const int PitchConditions = 30;
        public const int InfoCrawler = 31;
        public const int GroupStanding = 32;
        //public const int Highligths = 33;
        public const int NextTransmission = 34;
        public const int Schedule = 35;
        public const int Localizadores = 36;


        #endregion

        #region Constantes botones cambios
        #endregion


        //CONSTANTES TIPO GOL
        private const int Gol_Normal = 0;
        private const int Gol_7_M = 1;
        private const int Gol_Contraataque = 2;


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
        private PlantillasData _localizadores;


        // Ref. a consola e intérprete
        private Consola_balonmano _handballConsole;
        private Interprete_mondo _mondoInterprete;

        // Ref. a los diccionarios para los jugadores
        public Dictionary<int, int> _dorsalesLocal = new Dictionary<int, int>();
        public Dictionary<int, int> _dorsalesVisitante = new Dictionary<int, int>();


        // Ref. a los cronos de exclusión
        //public List<Crono_exclusion> cronoExclusion_List = new List<Crono_exclusion>();
        //public Dictionary<string, Handball_ExclusionCommand> command_List = new Dictionary<string, Handball_ExclusionCommand>(); 

        //List<ICommand> exchange= new List<ICommand>();
        //NewsExchangeFeed command = new NewsExchangeFeed(new Exchange("News Exchange Feed","Linea1"));



        /**
         * Constructor
         * Recibe los datos relativos al encuentro
         */

        public Controlador(EncuentroData datos, Consola_balonmano _handBallConsole, Interprete_mondo _mondoInterprete )
        {
            // Datos
            _datos = datos;
                 
            // Setup & Init
            setup();
            _gui.Show();
            SetSeccion(Clear);


            // Inicializamos el interprete
            this._handballConsole = _handBallConsole;
            this._mondoInterprete = _mondoInterprete;

            try
            {
                start_ConsoleListener();
            }
            catch (Exception e)
            {
                Console.WriteLine("Tramas recibidas sin tener objetos creados " + e);
            }

            // exchange.Add(command);
        }

        public Crono getCrono()
        {
            return _crono;
        }

        public List<ICommand> getListaCmd()
        {
            return _onListBox;
        }

        #region Consola Mondo

        /**
         * Arrancamos los eventos de la consola de mondo 
         */
        public void start_ConsoleListener()
        {
            //
            this._handballConsole.actualiza_crono += new Consola_balonmano.dispara_crono(_Crono_console);
            
            // Mejor evitarlas
            // Periodo lo manejamos manual
            // Los tiempos muertos se incluyen de forma manual
            //this._handballConsole.actualiza_periodo += new Consola_balonmano.dispara_periodo(_Periodo_console);
            //this._handballConsole.actualiza_tiempos_muertos += new Consola_balonmano.dispara_tiempos_muertos(_tiempoMuerto);
            this._handballConsole.actualiza_periodo += new Consola_balonmano.dispara_periodo(_Periodo_console);
            this._handballConsole.actualiza_tiempos_muertos += new Consola_balonmano.dispara_tiempos_muertos(_tiempoMuerto);

            // Sin interes
            this._handballConsole.actualiza_estado_crono += new Consola_balonmano.dispara_estado_crono(trama_unArgumento);
            this._handballConsole.actualiza_puntos_equipo += new Consola_balonmano.dispara_puntos_equipo(trama_ConteoGoles);

            // Goles y expulsiones
            this._handballConsole.actualiza_goles_jugador += new Consola_balonmano.dispara_goles_jugador(suma_gol);
            this._handballConsole.suma_expulsion_jugador += new Consola_balonmano.dispara_suma_expulsion_jugador(suma_expulsion);
            this._handballConsole.resta_expulsion_jugador += new Consola_balonmano.dispara_resta_expulsion_jugador(resta_expulsion);

            // Sin interes
            this._handballConsole.actualiza_dorsales_jugadores += new Consola_balonmano.dispara_dorsales_jugadores(load_dorsales);
            this._handballConsole.actualiza_inicia_videomarcador += new Consola_balonmano.dispara_inicia_videomarcador(trama_ceroArgumento);
        }


        #region  Eventos de consola


        // Crono
        public void _Crono_console (string crono) 
        {
            if(_gui.get_escuchar_crono())                  //Escucha de la consola activa?
            {
                try
                {
                    Console.WriteLine("TIEMPO: " + crono + ", PARTE: " + _crono.GetPeriodo()+" Porque ya no escuchamos de la consola");
                    _crono.SetTempo_String(crono);
                    if (!_crono.get_iniciado())
                    {
                        _crono.bol_iniciar(crono);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error _crono no inicializado: " + ex);
                }

                if (crono != null)
                {
                    //Console.WriteLine("TIEMPO: " + crono + ", PARTE: " + _crono.GetPeriodo());
                    run_crono(crono);
                }
            }
        }

        // Periodo
        public void _Periodo_console(string periodo)
        {
            Console.WriteLine("PERIODO: " + periodo);

            _crono.SetPeriodo(Int32.Parse(periodo));


            //if (int.TryParse(periodo, out _))
            //{
            //    _crono.SetMomento_console(new Momento(Int32.Parse(periodo)));
            //}

        }


        public void _tiempoMuerto(string equipo, string num_tiemposMuertos)
        {
            //Console.WriteLine("Ha llegado un tiempo muerto, o bueno, " + num_tiemposMuertos + ", del equipo " + equipo);

            //try
            //{

            //    addCommandBoxList commandAddBoxListItem = new addCommandBoxList(addBoxList);

            //    Equipo e = getEquipo(equipo);

            //    if (Int32.Parse(num_tiemposMuertos) - e.Timeout.Count > 0)
            //    {
            //        commandAddBoxListItem.BeginInvoke(new TimeOutCommand(_crono.GetMomento(), e), null, null);
            //    }

            //}
            //catch (Exception timeout)
            //{
            //    Console.WriteLine("Error at _tiempoMuerto " + timeout);
            //}

        }


        // Añadir gol al jugador

        //DESDE LA CONSOLA
        public void suma_gol (string equipo, string dorsal, string num_goles_totales) 
        {
            //Console.WriteLine("Eq: "+ equipo +" , Dorsal: "+dorsal+" , Goles: "+ Int16.Parse(num_goles_totales));
            //SI GOLES POR CONSOLA ESTA ACTIVADO
            if (_gui.get_escuchar_goles())
            {
                try
                {
                    // Get jugador
                    Jugador jugador = getJugadorByDorsalInterno(equipo, dorsal);  

                    if (jugador != null)
                    {
                        // IF num_goles_totales == jugador.getGoles() => NO HACEMOS NADA
                        //SUMAMOS UN GOL
                        if (Int16.Parse(num_goles_totales) > jugador.getGoles())
                        {
                            addListBoxItem(new GolCommand(_crono.GetMomento(), jugador, Gol_Normal));

                            //Abre los rótulos del jugador
                            //_gui.AbrirRotulosJugador();

                            //ShowRotulosJugador(jugador);
                        }
                        else
                        //RESTAMOS UN GOL
                        if (Int16.Parse(num_goles_totales) > jugador.getGoles())
                        {
                            removeListBoxItem(_onListBox.Find(x => x.GetJugador() == jugador && x.getNameCommand().Equals("GolCommand")));
                        }

                    }
                }
                catch (Exception player_notFound)
                {
                    Console.WriteLine("Error at suma_expulsion " + player_notFound);
                }
            }

            //////  SEGURAMNETE HAY QUE CAMBIARLO TODO LO DE ABAJO PORQUE LO HIZO JORGE
            //// Convertir string equipo y dorsal en un jugador existente
            //try
            //{

            //    //// Add / Remove command 
            //    //addCommandBoxList commandAddBoxListItem = new addCommandBoxList(addBoxList);
            //    //removeCommandBoxList commandRemoveBoxListItem = new removeCommandBoxList(removeBoxList);

            //    //// Get jugador
            //    //Jugador jugador = getJugadorByDorsal(equipo, dorsal);

            //    //Console.WriteLine("Gol marcado, diferencia " + (Int32.Parse(num_goles_totales) - jugador.Goles.Count) + ", el dorsal es el " + dorsal);

            //    //// Check +1 / -1
            //    //if (Int32.Parse(num_goles_totales) - jugador.Goles.Count > 0)
            //    //{
            //    //    for (int i = 0; i < (Int32.Parse(num_goles_totales) - jugador.Goles.Count); i++)
            //    //    {
            //    //        //commandAddBoxListItem.BeginInvoke(new GolCommand(_crono.GetMomento(), jugador), null, null);
            //    //    }
            //    //}
            //    //else if (Int32.Parse(num_goles_totales) - jugador.Goles.Count < 0)
            //    //{
            //    //    for (int i = 0; i > (Int32.Parse(num_goles_totales) - jugador.Goles.Count); i--)
            //    //    {
            //    //        //commandRemoveBoxListItem.BeginInvoke(new Handball_GolesCommand(_crono.GetMomento(), jugador), null, null);
            //    //    }
            //    //}


            //    /*
            //    switch (Int32.Parse(num_goles_totales) - jugador.Handball_Goles.Count)
            //    {

            //        case 1:
            //            commandAddBoxListItem.BeginInvoke(new Handball_GolesCommand(_crono.GetMomento(), jugador), null, null);
            //            break;

            //        default:
            //            Console.WriteLine("Error al sumar los puntos del jugador " + jugador.FullName + ", no se han sumado adecuadamente, valor recibido: " 
            //                + (Int32.Parse(num_goles_totales) - jugador.Handball_Goles.Count) 
            //                + ", ya que puntos totales " + Int32.Parse(num_goles_totales) 
            //                + " y los que tiene el jugador ya apuntados " 
            //                + jugador.Handball_Goles.Count);

            //            break;
            //    }
            //    */

            //}
            //catch (Exception player_notFound)
            //{
            //    Console.WriteLine("Error at suma_gol " + player_notFound);
            //}

        }

        public void trama_ConteoGoles(string equipo, string goles)
        {

        }


        // Añadir expulsion al jugador
        public void suma_expulsion (string equipo, string dorsal)
        {
            //SI EXCLUSIONES POR CONSOLA ESTA ACTIVADO
            if (_gui.get_escuchar_exclusion())
            {
                try
                {
                    // Get jugador
                    Jugador jugador = getJugadorByDorsalReal(equipo, dorsal);

                    if (jugador != null)
                    {
                        addListBoxItem(new ExclusionCommand(_crono.GetMomento(), jugador));

                        //ACTIVA TERCER ESTADO DE EMPTY GOAL
                        if (equipo.Equals("local"))
                        {
                            _gui.emptyNaranja(true);
                        }
                        else
                        {
                            _gui.emptyNaranja(false);
                        }
                    }
                }
                catch (Exception player_notFound)
                {
                    Console.WriteLine("Error at suma_expulsion " + player_notFound);
                }
            }

                ////Console.WriteLine("Suma " + equipo + ", " + dorsal);

                //// Add command 
                //addCommandBoxList commandAddBoxListItem = new addCommandBoxList(addBoxList);

                //try
                //{
                //    // Get jugador
                //    Jugador jugador = getJugadorByDorsalReal(equipo, dorsal);

                //    if(jugador != null)
                //    {
                //        Handball_ExclusionCommand exc = new Handball_ExclusionCommand(_crono.GetMomento(), jugador);
                //        command_List.Add(equipo+dorsal,exc);

                //        commandAddBoxListItem.BeginInvoke(exc, null, null);
                //        cronoExclusion_List.Add(new Crono_exclusion(_crono.GetMomento(), _crono, _crono.GetPeriodo(), _idiomas, jugador, equipo));


                //        //IPF 1

                //        for (int i = 0; i < _config.NumIpf; i++)
                //        {
                //            if (Program.EstaActivado(i))
                //            {
                //                _ipfs[i].Envia("Crono_Exclusion(['" + jugador.Equipo.TeamCode + "', '1'])");
                //                Console.WriteLine("****Crono_Exclusion(['" + jugador.Equipo.TeamCode + "', '1'])");
                //            }
                //        }
                //    }

                //}
                //catch (Exception player_notFound)
                //{
                //    Console.WriteLine("Error at suma_expulsion " + player_notFound);
                //}
            }

        // Quitar expulsion al jugador
        public void resta_expulsion(string equipo, string dorsal) {

            if (_gui.get_escuchar_exclusion())                  //Escucha de la consola activa?
            {
                try
                {
                    // Get jugador
                    Jugador jugador = getJugadorByDorsalReal(equipo, dorsal);

                    if (jugador != null)
                    {
                        removeListBoxItem(_onListBox.Find(x => x.GetJugador() == jugador && x.getNameCommand().Equals("ExclusionCommand")));
                    }
                }
                catch (Exception player_notFound)
                {
                    Console.WriteLine("Error at suma_expulsion " + player_notFound);
                }
            }

            //// Remove command
            //removeCommandBoxList commandRemoveBoxListItem = new removeCommandBoxList(removeBoxList);

            //try
            //{
            //    // Get jugador
            //    Jugador jugador = getJugadorByDorsalReal(equipo, dorsal);

            //    if (jugador != null)
            //    {
            //        //Handball_ExclusionCommand exc = new Handball_ExclusionCommand(_crono.GetMomento(), jugador);
            //        commandRemoveBoxListItem.BeginInvoke(command_List[equipo + dorsal], null, null);
            //        cronoExclusion_List.Remove(new Crono_exclusion(_crono.GetMomento(), _crono, _crono.GetPeriodo(), _idiomas, jugador, equipo));

            //        command_List.Remove(equipo + dorsal);

            //        //IPF 1

            //        for (int i = 0; i < _config.NumIpf; i++)
            //        {
            //            if (Program.EstaActivado(i))
            //            {
            //                _ipfs[i].Envia("Crono_Exclusion(['" + jugador.Equipo.TeamCode + "', '0'])");
            //                Console.WriteLine("****Crono_Exclusion(['" + jugador.Equipo.TeamCode + "', '0'])");
            //            }
            //        }
            //    }
            //}
            //catch (Exception player_notFound)
            //{
            //    Console.WriteLine("Error at resta_expulsion " + player_notFound);
            //}

        }

        // Recoger tramas innecesarias
        public void trama_ceroArgumento() { } 
        public void trama_unArgumento(string s) { }
        public void trama_tresArgumento(string s1, string s2, string s3) { }




        // Gestor de dorsales entre datos de consola y reales - NO NECESARIO 
        public void load_dorsales(string equipo, string dorsal_anterior, string dorsal_actual) {
            _convertDorsales(equipo, dorsal_anterior, dorsal_actual);
        }

        ///////////////////////////////////         CONVERSOR DORSALES          ///////////////////////////////////////

        public void _convertDorsales(string equipo, string dorsal_interno, string dorsal_actual)
        {
            Console.WriteLine("Oh vaya nos ha llegado un equipo " + equipo + ", " + dorsal_interno + ", " + dorsal_actual);

            if (equipo.Equals("local"))
            {
                if (!_dorsalesLocal.ContainsKey(Int32.Parse(dorsal_interno)))
                {
                    if (int.TryParse(dorsal_actual, out _))
                        _dorsalesLocal.Add(Int32.Parse(dorsal_interno), Int32.Parse(dorsal_actual));
                    
                }
            }
            else if (equipo.Equals("visitante"))
            {
                if (!_dorsalesVisitante.ContainsKey(Int32.Parse(dorsal_interno)))
                {
                    if (int.TryParse(dorsal_actual, out _))
                        _dorsalesVisitante.Add(Int32.Parse(dorsal_interno), Int32.Parse(dorsal_actual));
                }
            }
        }

        // Recibe el dorsal de la consola y acorde al diccionario creado en convertDorsales
        // podemos extraer el dorsal real del jugador
        public int _getRealDorsal(string equipo, string dorsal)
        {
            int dorsal_consola = Int32.Parse(dorsal);
            int dorsal_real;


            if (equipo.Equals("local"))
            {
                _dorsalesLocal.TryGetValue(dorsal_consola, out dorsal_real);
                return dorsal_real;

            }
            else if(equipo.Equals("visitante"))
            {
                _dorsalesVisitante.TryGetValue(dorsal_consola, out dorsal_real);
                return dorsal_real;
            }
            else
            {
                return 0;
            }


        }



        public Jugador getJugadorByDorsal(string equipo, string dorsal)
        {
            //////////////////////////////
            //String str = "Dorsales locales: ";
            //foreach (int key in _dorsalesLocal.Values)
            //{
            //    str += (key + ", ");
            //}
            //Console.WriteLine(str);

            //str = "Dorsales visitantes: ";
            //foreach (int key in _dorsalesVisitante.Values)
            //{
            //    str += (key + ", ");
            //}
            ////Console.WriteLine(str);
            ///////////////////////////////


            switch (dorsal)
            {
                case "E":
                    if (equipo.Equals("local"))
                        return _datos.EquipoL.Entrenador;
                    else
                        return _datos.EquipoV.Entrenador;

                default:
                    try
                    {
                        int dorsalNumb = Int32.Parse(dorsal);

                        if (equipo.Equals("local"))
                        {
                            foreach (Jugador j in _datos.EquipoL.Jugadores)
                            {
                                if (j.Dorsal_Interno == dorsalNumb)
                                    return j;
                            }
                        }
                        else
                        {
                            foreach (Jugador j in _datos.EquipoV.Jugadores)
                            {
                                if (j.Number == dorsalNumb)
                                    return j;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error parsing string dorsal at getJugadorByDorsal: " + e);
                    }

                    return null;
            }
        }


        public Jugador getJugadorByDorsalReal(string equipo, string dorsal)
        {
            switch (dorsal)
            {
                case "E":
                    if (equipo.Equals("local"))
                        return _datos.EquipoL.Entrenador;
                    else
                        return _datos.EquipoV.Entrenador;

                default:
                    try
                    {
                        int dorsalNumb = Int32.Parse(dorsal);

                        if (equipo.Equals("local"))
                        {

                            foreach (Jugador j in _datos.EquipoL.Jugadores)
                            {
                                if (j.Number == dorsalNumb)
                                    return j;
                            }

                        }
                        else
                        {
                            foreach (Jugador j in _datos.EquipoV.Jugadores)
                            {
                                if (j.Number == dorsalNumb)
                                    return j;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error parsing string dorsal at getJugadorByDorsal: " + e);
                    }

                    return null;
            }
        }

        public Jugador getJugadorByDorsalInterno(string equipo, string dorsal)
        {
            switch (dorsal)
            {
                case "E":
                    if (equipo.Equals("local"))
                        return _datos.EquipoL.Entrenador;
                    else
                        return _datos.EquipoV.Entrenador;

                default:
                    try
                    {
                        int dorsalNumb = Int16.Parse(dorsal) - 4 ;

                        if (equipo.Equals("local"))
                        {

                            foreach (Jugador j in _datos.EquipoL.Jugadores)
                            {
                                if (j.Number == _datos.EquipoL.Dorsales[dorsalNumb].Number)
                                    return j;
                            }

                        }
                        else
                        {
                            foreach (Jugador j in _datos.EquipoV.Jugadores)
                            {
                                if (j.Number == _datos.EquipoV.Dorsales[dorsalNumb].Number)
                                    return j;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error parsing string dorsal at getJugadorByDorsal: " + e);
                    }

                    return null;
            }
        }




        public Equipo getEquipo(string equipo)
        {
            switch (equipo)
            {
                case "local":
                    return _datos.EquipoL;
                case "visitante":
                    return _datos.EquipoV;
                default:
                    return null;
            }
        }





        delegate void addCommandBoxList(ICommand command);
        public void addBoxList(ICommand command)
        {
            try
            {
                if (this._gui.InvokeRequired)
                {
                    addCommandBoxList d = new addCommandBoxList(addListBoxItem);
                    this._gui.Invoke(d, new object[] { command });
                }
                else
                    addListBoxItem(command);
            }
            catch
            {
                Console.WriteLine("Error at addBoxList");
            }
        }

        
        delegate void removeCommandBoxList(ICommand command);

        public void removeBoxList(ICommand command)
        {

            Console.WriteLine("Desaparecido en combate");

            try
            {
                if (this._gui.InvokeRequired)
                {
                    removeCommandBoxList d = new removeCommandBoxList(ListBoxRemoveItem);
                    this._gui.Invoke(d, new object[] { command });
                }
                else
                    removeBoxList(command);
            }
            catch
            {
                Console.WriteLine("Error at removeBoxList");
            }
        }

        #endregion



        #endregion

        /**
         * Devuelve el Controlador de los Penaltis
         */
        public Penaltis GetPenaltis()
        {
            return _penaltis;
        }

        public IdiomaData[] getIdiomas()
        {
            return _idiomas;
        }

        public int getNumIpfs()
        {
            return _config.NumIpf;
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

        //Statistics MANUAL
        public void inStaticsManual(string name, string datoL, string datoV)
        {
            for (int i = 0; i < _config.NumIpf; i++)
            {
                if (Program.EstaActivado(i))                    
                    _ipfs[i].Envia("StatisticsIN(['" + name + "', '" + _datos.EquipoL.FullName + "', '" + _datos.EquipoL.ShortName.Replace("'", "\\'") + "', '" + _datos.EquipoL.TeamCode + "', '" + datoL + "','" + _datos.EquipoV.FullName + "', '" + _datos.EquipoV.ShortName.Replace("'", "\\'") + "', '" + _datos.EquipoV.TeamCode + "', '" + datoV + "'])");
            }
        }

        public void inTableManual(
            string name1, string datoL1, string datoV1,
            string name2, string datoL2, string datoV2,
            string name3, string datoL3, string datoV3,
            string name4, string datoL4, string datoV4,
            string name5, string datoL5, string datoV5,
            string name6, string datoL6, string datoV6,
            string name7, string datoL7, string datoV7,
            string name8, string datoL8, string datoV8)
        {
            for (int i = 0; i < _config.NumIpf; i++)
            {
                if (Program.EstaActivado(i))
                    _ipfs[i].Envia("StatisticsTableIN(['" +
                        name1 + "', '" + datoL1 + "','" + datoV1 + "', '" +
                        name2 + "', '" + datoL2 + "','" + datoV2 + "', '" +
                        name3 + "', '" + datoL3 + "','" + datoV3 + "', '" +
                        name4 + "', '" + datoL4 + "','" + datoV4 + "', '" +
                        name5 + "', '" + datoL5 + "','" + datoV5 + "', '" +
                        name6 + "', '" + datoL6 + "','" + datoV6 + "', '" +
                        name7 + "', '" + datoL7 + "','" + datoV7 + "', '" +
                        name8 + "', '" + datoL8 + "','" + datoV8 + "', '" +
                        _crono.GetMomento().GetNombreParte(_idiomas[i], _crono.Descanso) + "'])");
            }
        }

        public void outStaticsManual()
        {
            for (int i = 0; i < _config.NumIpf; i++)
            {
                if (Program.EstaActivado(i))
                    _ipfs[i].Envia("StatisticsOUT()");
            }
        }

        public void outTableManual()
        {
            for (int i = 0; i < _config.NumIpf; i++)
            {
                if (Program.EstaActivado(i))
                    _ipfs[i].Envia("StatisticsTableOUT()");
            }
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
                    case 0: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero); break;
                    case 1: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero2); break;
                    case 2: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero3); break;
                    case 3: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero4); break;
                    case 4: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero5); break;
                    case 5: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero6); break;
                    case 6: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero7); break;
                    case 7: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero8); break;
                    case 8: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero9); break;
                    case 9: _idiomas[i] = PersistenciaUtil.CargaIdioma(@"balonmano\" + _config.IdiomaFichero10); break;
                    default: break;
                }

            }

            // Carga las plantillas
            _plantillas = PersistenciaUtil.CargaPlantillas();

            // Carga los localizadores
            _localizadores = PersistenciaUtil.CargaLocalizadores();

            // Configura el GUI con los jugadores
            _gui.ConfigEquipoLocal(_datos.EquipoL);
            _gui.ConfigEquipoVisitante(_datos.EquipoV);

            //Lista con los dorsales reales
            _datos.EquipoL.Dorsales = _datos.EquipoL.Jugadores;
            _datos.EquipoV.Dorsales = _datos.EquipoV.Jugadores;

            _datos.EquipoL.Dorsales.Sort(new JugadorComparerDorsal());
            _datos.EquipoV.Dorsales.Sort(new JugadorComparerDorsal());


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
            _crono = new Crono(_ipfs, _idiomas,_config.NumIpf, this);
            _crono.Bind(_gui);
            //PosesionActivar(false);
 
            // Penaltis
            _penaltis = new Penaltis(_ipfs, _gui,_config.NumIpf);

            // Variables intermedias para los cambios
            _cambio = new Jugador[3];

            
        }

        // Configura el Marcador. Calcula los goles de cada equipo y lo refleja en el GUI y en el IPF
        private void configMarcador()
        {
            int n = _config.NumIpf;
            
            // Futbol
            /*int golesL = _datos.EquipoL.Goles.Count + _datos.EquipoL.GolesPenalty.Count + _datos.EquipoV.GolesPP.Count;
            int golesV = _datos.EquipoV.Goles.Count + _datos.EquipoV.GolesPenalty.Count + _datos.EquipoL.GolesPP.Count;
            */

            // Balonmano
            int golesL = _datos.EquipoL.Goles.Count;
            int golesV = _datos.EquipoV.Goles.Count;

            Console.WriteLine("Aqui llegas vaya, pero claro " + _datos.EquipoL.Goles.Count);


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
            _gui.ActivaJugadores(true, new List<int>());
            _gui.ActivaJugadores(false, new List<int>());

            switch (_seccionActual)
            {
                case Handball_Presentacion:
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaTodosJugadores(true);
                    _gui.ActivaTodosJugadores(false);
                    break;

                case Handball_Testadisticas:
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaTodosJugadores(true);
                    _gui.ActivaTodosJugadores(false);
                    break;

                case Handball_Rotulos:
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaTodosJugadores(true);
                    _gui.ActivaTodosJugadores(false);
                    break;

                case Handball_Goles:
                    /*OnAirSetup(new StatisticsCommand(StatisticsCommand.Handball_Goles, _datos.EquipoL, _datos.EquipoV));*/
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(false);
                        /*_gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));*/
                    }
                    break;

                case Handball_Fieldthrows:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Fieldthrows, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Handball_FieldGoal:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Goles_field, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(false);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Handball_Sevenmthrows:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Sevenmthrows, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Handball_SevenGoal:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Goles_seven, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(false);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Attacks:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Attacks, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getJugadores(true));
                        _gui.ActivaJugadores(false, getJugadores(false));
                    }
                    break;

                case Fastbreaks:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Fastbreaks, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Turnovers:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Turnovers, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Exclusions:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Exclusions, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Rotulos:
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaTodosJugadores(true);
                    _gui.ActivaTodosJugadores(false);
                    break;

                case Goles:
                case GolesPenalti:
                case GolesContraataque:
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case Tiros:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Tiros, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case TirosPenalti:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosPenalti, _datos.EquipoL, _datos.EquipoV));
                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getTitulares(true));
                        _gui.ActivaJugadores(false, getTitulares(false));
                    }
                    break;

                case TirosContraataque:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TirosContraataque, _datos.EquipoL, _datos.EquipoV));
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
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getPorteros(true));
                        _gui.ActivaJugadores(false, getPorteros(false));
                    }
                    break;

                case Perdidas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.Perdidas, _datos.EquipoL, _datos.EquipoV));

                    if (IsPlay())
                    {
                        _gui.ActivaEquipos(true);
                        _gui.ActivaJugadores(true, getJugadores(true));
                        _gui.ActivaJugadores(false, getJugadores(false));
                    }
                    break;

                case TAmarillas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TAmarillas, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaJugadores(true, getJugadores(true));
                    _gui.ActivaJugadores(false, getJugadores(false));
                    break;

                case TRojas:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TRojas, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaJugadores(true, getJugadores(true));
                    _gui.ActivaJugadores(false, getJugadores(false));
                    break;

                case TAzules:
                    OnAirSetup(new StatisticsCommand(StatisticsCommand.TAzules, _datos.EquipoL, _datos.EquipoV));

                    _gui.ActivaEquipos(true);
                    _gui.ActivaEntrenadores(true);
                    _gui.ActivaJugadores(true, getJugadores(true));
                    _gui.ActivaJugadores(false, getJugadores(false));
                    break;

                case Estadisticas:
                    if (numEstadisticas == 2)
                    {

                        OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _datos.EquipoL, _datos.EquipoV, _datos));
                    }
                    if (numEstadisticas == 3)
                    {

                        OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _datos.EquipoL, _datos.EquipoV, _datos));
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
                //AYUDA PARA EL OPERADOR, CONFIRMAR LAS EXPULSIONES CON LAS TARJETAS    luis
                //if (j.TAmarillas.Count < 2 && j.TRojas.Count < 1)
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
                            case 0: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast, _datos.ResultadoIda); break;
                            case 1: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast2, _datos.ResultadoIda); break;
                            case 2: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast3, _datos.ResultadoIda); break;
                            case 3: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast4, _datos.ResultadoIda); break;
                            case 4: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast5, _datos.ResultadoIda); break;
                            case 5: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast6, _datos.ResultadoIda); break;
                            case 6: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast7, _datos.ResultadoIda); break;
                            case 7: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast8, _datos.ResultadoIda); break;
                            case 8: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast9, _datos.ResultadoIda); break;
                            case 9: _ipfs[i].ConfigInicial(_config.EscudosRuta, _datos.EquipoL, _datos.EquipoV, _config.Multicast10, _datos.ResultadoIda); break;
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
            Console.WriteLine("Actualmente estamos en la seccion " + _seccionActual);

            switch (_seccionActual) {

                case Handball_Rotulos:
                    ShowRotulosJugador(jugador);
                    break;

                case Handball_Goles:
                    //addListBoxItem(new Handball_GolesCommand(_crono.GetMomento(), jugador));  No se usa
                    break;

                case Attacks:
                    addListBoxItem(new AttacksCommand(_crono.GetMomento(), jugador));
                    break;                

                //case Fastbreaks:
                //    addListBoxItem(new FastbreaksCommand(_crono.GetMomento(), jugador));
                //    break;
                              
                //case Turnovers:
                //    addListBoxItem(new TurnoversCommand(_crono.GetMomento(), jugador));
                //    break;
                                  
                case Exclusions:
                    addListBoxItem(new ExclusionCommand(_crono.GetMomento(), jugador));

                    //ACTIVA TERCER ESTADO DE EMPTY GOAL
                    if (jugador.Equipo == _datos.EquipoL)
                    {
                        _gui.emptyNaranja(true);
                    }
                    else
                    {
                        _gui.emptyNaranja(false);
                    }
                    break;               

                case Rotulos:
                    ShowRotulosJugador(jugador);     
                    break;

                case Goles:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), jugador, Gol_Normal));

                    _gui.AbrirRotulosJugador();
                    botonJugador.PerformClick();
                    break;

                case GolesContraataque:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), jugador, Gol_Contraataque));

                    _gui.AbrirRotulosJugador();
                    botonJugador.PerformClick();
                    break;

                case GolesPenalti:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), jugador, Gol_7_M));

                    _gui.AbrirRotulosJugador();
                    botonJugador.PerformClick();
                    break;

                case Tiros:
                    addListBoxItem(new TiroCommand(_crono.GetMomento(), jugador, Gol_Normal));
                    break;

                case TirosPenalti:
                    addListBoxItem(new TiroCommand(_crono.GetMomento(), jugador, Gol_7_M));
                    break;

                case TirosContraataque:
                    addListBoxItem(new TiroCommand(_crono.GetMomento(), jugador, Gol_Contraataque));
                    break;

                case TirosAPuertaDentro:
                    addListBoxItem(new TiroAPuertaCommand(_crono.GetMomento(), jugador));
                    break;

                case TirosAPuertaFuera:
                    addListBoxItem(new TiroFueraCommand(_crono.GetMomento(), jugador));
                    break;

                case Paradas:
                    addListBoxItem(new ParadaTiroCommand(_crono.GetMomento(), jugador));
                    break;

                case Perdidas:
                    addListBoxItem(new PerdidaCommand(_crono.GetMomento(), jugador));
                    break;

                case TAmarillas:
                    addListBoxItem(new YellowCardCommand(_crono.GetMomento(), jugador));
                    break;

                case TRojas:
                    addListBoxItem(new RedCardCommand(_crono.GetMomento(), jugador));
                    break;

                case TAzules:
                    addListBoxItem(new BlueCardCommand(_crono.GetMomento(), jugador));
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

        public void ClickOnEntrenadorAsistente(Jugador entrenador)
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
                    OnAirSetup(new AssistantCoachCommand(entrenador));
                    break;
            }
        }

        public void ClickOnEquipo(bool local)
        {
            Equipo equipo = (local ? _datos.EquipoL : _datos.EquipoV);
            switch (_seccionActual)
            {
                case Exclusions:
                    addListBoxItem(new ExclusionCommand(_crono.GetMomento(), equipo));

                    //ACTIVA TERCER ESTADO DE EMPTY GOAL
                    if (local)
                    {
                        _gui.emptyNaranja(true);
                    }
                    else
                    {
                        _gui.emptyNaranja(false);
                    }
                    break;

                //case Corners:
                //    addListBoxItem(new CornerCommand(_crono.GetMomento(), equipo));
                //    break;Handball_Goles

                case Handball_Goles:
                    //addListBoxItem(new AttacksCommand(_crono.GetMomento(), equipo));
                    break;

                //case FaltasRecibidas:
                //    addListBoxItem(new FaltaRecibidaCommand(_crono.GetMomento(), equipo));
                //    break;

                //case FaltasCometidas:
                //    addListBoxItem(new FaltaCometidaCommand(_crono.GetMomento(), equipo));
                //    break;

                case Goles:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), equipo, Gol_Normal)); 
                    //addListBoxItem(new GolCommand(_crono.GetMomento(), equipo));
                    break;

                case GolesContraataque:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), equipo, Gol_Contraataque));
                    break;

                case GolesPenalti:
                    addListBoxItem(new GolCommand(_crono.GetMomento(), equipo, Gol_7_M));
                    break;

                case Tiros:
                    addListBoxItem(new TiroCommand(_crono.GetMomento(), equipo, Gol_Normal));
                    break;

                case TirosPenalti:
                    addListBoxItem(new TiroCommand(_crono.GetMomento(), equipo, Gol_7_M));
                    break;

                case TirosContraataque:
                    addListBoxItem(new TiroCommand(_crono.GetMomento(), equipo, Gol_Contraataque));
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

                case TAzules:
                    addListBoxItem(new BlueCardTeamCommand(_crono.GetMomento(), equipo));
                    break;

                case Paradas:
                    addListBoxItem(new ParadaTiroCommand(_crono.GetMomento(), equipo));
                    break;

                case Perdidas:
                    addListBoxItem(new PerdidaCommand(_crono.GetMomento(), equipo));
                    break;

                case Attacks:
                    addListBoxItem(new AttacksCommand(_crono.GetMomento(), equipo));
                    break;

                    //case FuerasDeJuego:
                    //    addListBoxItem(new FueraDeJuegoCommand(_crono.GetMomento(), equipo));
                    //    break;
            }
        }

        //public void HacerCambio(Jugador jugador)
        //{
            
        //    if (_cambio[0] == null)
        //    {
        //        _cambio[0] = jugador;
                
        //        //Activa solo plantilla del equipo
        //        _gui.ActivaJugadores(jugador.Equipo.Local, getTitulares(jugador.Equipo.Local));
        //        _gui.ActivaJugadores(!jugador.Equipo.Local, new List<int>());
        //    }
        //    else
        //    {
        //        addListBoxItem(new ChangeCommandOptions(_crono.GetMomento(), _cambio[0], jugador,this._cambioEscogido));
                    
        //        if (jugador.Equipo.Local)
        //        {
        //            _gui.ConfigEquipoLocal(jugador.Equipo);
        //        }
        //        else
        //        {
        //            _gui.ConfigEquipoVisitante(jugador.Equipo);
        //        }
        //        ResetCambio();
        //        _gui.ActivaJugadores(jugador.Equipo.Local, new List<int>());
        //    }
        //    _gui.ActivaTabJugadores(true);
        //}
        
        //public void HacerCambioDoble(Jugador jugador)
        //{
        //    if (_cambio[0] == null)
        //    {
        //        _cambio[0] = jugador;

        //        // Desactiva el jugador
        //        List<int> dorsales = getBanquillo(jugador.Equipo.Local);
        //        dorsales.Remove(jugador.Number);
        //        _gui.ActivaJugadores(jugador.Equipo.Local, dorsales);

        //        // Desactiva el otro equipo
        //        _gui.ActivaJugadores(!jugador.Equipo.Local, new List<int>());
        //    }
        //    else if (_cambio[1] == null)
        //    {
        //        _cambio[1] = jugador;

        //        //Activa solo plantilla del equipo
        //        _gui.ActivaJugadores(jugador.Equipo.Local, getTitulares(jugador.Equipo.Local));
        //        _gui.ActivaJugadores(!jugador.Equipo.Local, new List<int>());

        //        _gui.ActivaTabJugadores(true);
        //    }
        //    else if (_cambio[2] == null)
        //    {
        //        _cambio[2] = jugador;

        //        // Desactiva el jugador
        //        List<int> dorsales = getTitulares(jugador.Equipo.Local);
        //        dorsales.Remove(jugador.Number);
        //        _gui.ActivaJugadores(jugador.Equipo.Local, dorsales);
        //    }
        //    else
        //    {
        //        addListBoxItem(new DoubleChangeCommand(_crono.GetMomento(), _cambio[0], _cambio[1], _cambio[2], jugador));
        //        if (jugador.Equipo.Local)
        //        {
        //            _gui.ConfigEquipoLocal(jugador.Equipo);
        //        }
        //        else
        //        {
        //            _gui.ConfigEquipoVisitante(jugador.Equipo);
        //        }
        //        ResetCambio();
        //        _gui.ActivaJugadores(jugador.Equipo.Local, new List<int>());
        //    }
        //}
        
        //public void ResetCambio()
        //{
        //    _cambio[0] = null;
        //    _cambio[1] = null;
        //    _cambio[2] = null;
        //}

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
            else if (_onListBox == _plantillas.Localizadores)
            {
                LocalizadorForm lf = new LocalizadorForm();
                lf.ShowDialog();

                if (lf.GetLocalizador() != null)
                    addListBoxItem(new LocalizadorCommand(lf.GetLocalizador()));
            }
            else if (_onListBox == _plantillas.GroupStanding)
            {
                GroupStandingForm exf = new GroupStandingForm();
                exf.ShowDialog();

                if (exf.GetGroupStanding() != null)
                    addListBoxItem(new GroupStandingCommand(exf.GetGroupStanding()));
            }
            //else if (_onListBox == _plantillas.NextTransmission)
            //{
            //    NextTransmissionForm exf = new NextTransmissionForm();
            //    exf.ShowDialog();

            //    if (exf.GetNextTransmission() != null)
            //        addListBoxItem(new NextTransmissionCommand(exf.GetNextTransmission()));
            //}
            //else if (_onListBox == _plantillas.Schedule)
            //{
            //    ScheduleForm exf = new ScheduleForm();
            //    exf.ShowDialog();

            //    if (exf.GetSchedule() != null)
            //        addListBoxItem(new ScheduleCommand(exf.GetSchedule()));
            //}            
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

                if (command is GolCommand)
                    configMarcador();

                /*if (command is YellowCardCommand || command is RedCardCommand)
                {
                    // Refresca para posible desactivación de un jugador
                    SetSeccion(_seccionActual);
                }*/

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

            // Backup
            //PersistenciaUtil.GuardaBackup(_datos);
        }

        private void addListBoxItem(ICommand command)
        {
            _onListBox.Insert(0, command);
            _gui.ConfigListBox(_onListBox.ToArray());

            // Si es ejecutable lo ejecuta
            if (command is ICommandExecutable)
            {
                ((ICommandExecutable)command).Execute();

                if (command is GolCommand)
                    configMarcador();

                /*if (command is YellowCardCommand || command is RedCardCommand)
                {
                    // Refresca para posible desactivación de un jugador
                    SetSeccion(_seccionActual);
                }*/

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
                if (command is GolCommand)
                {
                    configMarcador();
                }              
                if (command is YellowCardCommand || command is RedCardCommand || command is BlueCardCommand)
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
            else if (command is LocalizadorCommand)
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
            else if (command is NextTransmissionCommand)
            {
                NextTransmissionCommand exc = (NextTransmissionCommand)command;
                NextTransmissionForm efm = new NextTransmissionForm(exc.nextTransmission);

                efm.ShowDialog();

                if (efm.GetNextTransmission() != null)
                {
                    exc.nextTransmission = efm.GetNextTransmission();
                    _gui.Refresh();
                }
            }
            else if (command is ScheduleCommand)
            {
                ScheduleCommand exc = (ScheduleCommand)command;
                ScheduleForm efm = new ScheduleForm(exc.schedule);

                efm.ShowDialog();

                if (efm.GetSchedule() != null)
                {
                    exc.schedule = efm.GetSchedule();
                    _gui.Refresh();
                }
            }
            else if (command is GolCommand)
            {
                GolCommand gol_comand = (GolCommand)command;
                GolForm gol_form = new GolForm(gol_comand.getTipoGol(), gol_comand.getNuevoMomento());

                gol_form.ShowDialog();

                //ESTABLECE EL NUEVO TIPO DE GOL
                gol_comand.cambiaTipoGol(gol_form.getNuevoTipo());

                // ESTABLECE EL NUEVO MOMENTO
                gol_comand.setNuevoMomento(gol_form.getMomento());

                _onListBox.Sort(new ICommandComparer());
                _gui.ConfigListBox(_onListBox.ToArray());

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

                case (Localizadores):
                    if (_plantillas.Localizadores == null)
                        _plantillas.Localizadores = new List<ICommand>();

                    _onListBox = _plantillas.Localizadores;
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

                case (NextTransmission):
                    if (_plantillas.NextTransmission == null)
                        _plantillas.NextTransmission = new List<ICommand>();

                    _onListBox = _plantillas.NextTransmission;
                    _gui.activaAcciones(true, true, true);
                    break;

                case (Schedule):
                    if (_plantillas.Schedule == null)
                        _plantillas.Schedule = new List<ICommand>();

                    _onListBox = _plantillas.Schedule;
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

            //if (jugador.FaltasRecibidas.Count > 0)
            //    rotulos.Add(new PlayerCommand(jugador, jugador.FaltasRecibidas.Count != 1 ? 
            //        PlayerCommand.Command.FaltasRecibidas : PlayerCommand.Command.FaltaRecibida));

            //if (jugador.FaltasCometidas.Count > 0)
            //    rotulos.Add(new PlayerCommand(jugador, jugador.FaltasCometidas.Count != 1 ? 
            //        PlayerCommand.Command.FaltasCometidas : PlayerCommand.Command.FaltaCometida));

            if ((jugador.Goles.Count) > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.Goles.Count != 1 ? 
                    PlayerCommand.Command.Goles : PlayerCommand.Command.Gol));

            //if (jugador.GolesPP.Count > 0)
            //    rotulos.Add(new PlayerCommand(jugador, jugador.GolesPP.Count != 1 ? 
            //        PlayerCommand.Command.GolesPP : PlayerCommand.Command.GolPP));

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

            if (jugador.TAzules.Count > 0)
                rotulos.Add(new PlayerCommand(jugador, jugador.TAzules.Count != 1 ?
                    PlayerCommand.Command.TAzules : PlayerCommand.Command.TAzul));

            //if (jugador.Fuerasdejuego.Count > 0)
            //    rotulos.Add(new PlayerCommand(jugador, jugador.Fuerasdejuego.Count != 1 ? 
            //        PlayerCommand.Command.FuerasDeJuego : PlayerCommand.Command.FueraDeJuego));

            //if (jugador.SancionSiAmarilla)
            //    rotulos.Add(new PlayerCommand(jugador, _idiomas[0].MissesNextMatch));

            _gui.activaAcciones(false, false, false);
            _gui.ConfigListBox(rotulos.ToArray());
        }

        #endregion

        // ================================= CRONO =================================

        #region Crono de la consola

        // Inicia el crono y lo actualiza acorde a la consola 
        public void run_crono(string timeStamp)
        {
            // GUI Update
            Momento _momentoAnterior = _crono.GetMomento();

           
            //Console.WriteLine("EmpezarCrono " + timeStamp);

            if (!timeStamp.Equals(_momentoAnterior.ToString()))
            {

                _crono.cronoRun(timeStamp, true);


                // 
                //Console.WriteLine("EXCLUSIONES PRE: " + cronoExclusion_List.Count);

                //List<Crono_exclusion> cronoExclusion_List_eliminar = new List<Crono_exclusion>();

                //foreach (Crono_exclusion cex in cronoExclusion_List)
                //{
                //    if(cex.get_parte_momento_final() == _crono.GetPeriodo() && cex.get_seg_ab_final() == _crono.GetMomento().SegundoAbsoluto)
                //    {
                //        Console.WriteLine("*** FINALIZA EXCLUSION ***");

                //        cronoExclusion_List_eliminar.Add(cex);
                //    }
                //}

                //foreach (Crono_exclusion cex_delete in cronoExclusion_List_eliminar)
                //{
                //    cronoExclusion_List.Remove(cex_delete);
                //}

                //Console.WriteLine("EXCLUSIONES POST: " + cronoExclusion_List.Count);

                /*if (!_crono.IsPlayManual())
                {
                    _crono.setupCrono_Manual(_momentoAnterior);
                    _crono.startCrono_Manual();
                }*/
            }
            else
            {
                Console.WriteLine("Non repetitive data allowed");
            }


            if (!_crono.IsPlay())
            {
                if (_crono.GetMomento().Parte == Momento.Penaltis)
                {
                    ListBoxConfig(Penaltis);
                }
                else // En el juego normal se activa el control de posesion
                {
                    PosesionActivar(true);
                }
            }

            

            //PosesionActivar(true);

        }

        //
        public void CronoShow(bool mostrar)
        {
            _crono.ShowCronoIpf(mostrar);
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

        public void CronoEdit(string minutes, string seconds)
        {
            int min = 0;
            int sec = 0;
            int.TryParse(minutes, out min);
            int.TryParse(seconds, out sec);

            //Console.WriteLine("-- "+min+ " : " + sec);
            //Momento cronoMomento = _crono.GetMomento();
            //cronoMomento.Update(min, sec);
            //CronoEdit(cronoMomento);
            _crono.set_segundoAbsolutoReferencia(sec+(min*60));
        }

        public void CronoEdit(string minutes, string seconds, int parte)
        {
            int min = 0;
            int sec = 0;
            int.TryParse(minutes, out min);
            int.TryParse(seconds, out sec);
            Momento cronoMomento = _crono.GetMomento();
            cronoMomento.Update(parte, min, sec);
            CronoEdit(cronoMomento);
        }

        void CronoEdit(Momento momento)
        {
            _crono.SetMomento(momento);
        }

        public bool IsPlay()
        {
            return _crono.IsPlay();
        }

        /***    MANUALLY    ***/

        public void CronoEmpezar()
        {
            //_controlador._Crono_console("0:01");

            if (!_gui.get_escuchar_crono())                  //Escucha de la consola activa?
            {
                // Crono Manual
                _crono.Empezar();

                //if (_crono.GetMomento().Parte == Momento.IniParte2 && _crono.GetMomento().isStart)
                //{
                //    _datos.EquipoL.Faltas.Clear();
                //    _datos.EquipoV.Faltas.Clear();

                //    for (int i = 0; i < _config.NumIpf; i++)
                //    {
                //        if (Program.EstaActivado(i))
                //        {
                //            _ipfs[i].Envia("Falta(['" + _datos.EquipoL.TeamCode + "', '" + _datos.EquipoL.Faltas.Count + "'])");
                //            _ipfs[i].Envia("Falta(['" + _datos.EquipoV.TeamCode + "', '" + _datos.EquipoV.Faltas.Count + "'])");
                //        }
                //    }
                //}

                if (_crono.GetMomento().Parte == Momento.Penaltis)
                {
                    ListBoxConfig(Penaltis);
                }
                else // En el juego normal se activa el control de posesion
                {
                    PosesionActivar(true);
                }
            }
                
        }

        public void CronoParar()
        {
            _crono.Parar();

            PosesionActivar(false);
        }


        #endregion


        #region Crono

        /*public void CronoShow(bool mostrar)
        {
            _crono.ShowCronoIpf(mostrar);
        }

        public void CronoEmpezar()
        {

            Console.WriteLine("Pero de puto manual");

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



        public void CronoStop()
        {

        }


        public void CronoResume()
        {

        }

    
    */
        #endregion

        // =============================== PREVIEWS ================================
        #region Métodos Preview

        public void previewScore()
        {
            OnAirSetup(new ScoreBoardCommand(_datos.EquipoL, _datos.EquipoV));
        }


        //public void previewTeamLineUpCrawl(bool local)
        //{
        //    if (local)
        //    {
        //        OnAirSetup(new TeamLineUpCrawlCommand(_datos.EquipoL));
        //    }
        //    else
        //    {
        //        OnAirSetup(new TeamLineUpCrawlCommand(_datos.EquipoV));
        //    }
        //}

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

        //public void previewTeamLineUpSubstitutes(bool local)
        //{
        //    if (local)
        //    {
        //        OnAirSetup(new TeamLineUpSubstitutesCommand(_datos.EquipoL));
        //    }
        //    else
        //    {
        //        OnAirSetup(new TeamLineUpSubstitutesCommand(_datos.EquipoV));
        //    }
        //}

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
            OnAirSetup(new StatisticsTableCommand(_crono.GetMomento(), _crono.Descanso, _datos.EquipoL, _datos.EquipoV,_datos.Posesion.getPorcentajeLocal(),_datos.Posesion.getPorcentajeVisitante()));
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
                 _datos.EquipoL.TeamCode, _datos.EquipoV.TeamCode));
        }

        //public void previewHighlights()
        //{
        //    ListBoxConfig(Controlador.Highligths);
        //    OnAirSetup(new HighligthsCommand());
        //}

        public void LocalTimeOut()
        {
            OnAirSetup(new TimeOutCommand(_datos.EquipoL));
        }

        public void VisitorTimeOut()
        {
            OnAirSetup(new TimeOutCommand(_datos.EquipoV));
        }

        
        public void EmptyGoal(bool local, bool activo)
        {
            if (local)
                executeItem(new EmptyGoalCommand(_datos.EquipoL, activo));
            else
                executeItem(new EmptyGoalCommand(_datos.EquipoV, activo));
        }

        #endregion

    }
}
