using System.Timers;
using Balonmano_Manager_App.Interfaz;
using Balonmano_Manager_App.Persistencia;
using System.Diagnostics;
using System;
using System.Globalization;
using Balonmano_Manager_App.Beans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Balonmano_Manager_App
{
    /**
     * Lógica del cronómetro
     */
    public class Crono
    {

        #region Variables

        // Console
        private InterfaceIPF[] _ipf;
        private IdiomaData[] _idioma;

        private Momento _momento;
        private bool _play;

        private MainForm _gui;
        private int _segundoAbsolutoReferencia = 0;
        private int _numipf;

        private Controlador _controller;

        // Manual
        private Momento _momentoManual;

        private Timer _timer;
        private Stopwatch _stopwatch;

        private bool _playManual;

        private int parte_partido = 1;
        private string tempo_string;

        private Timer timer_pause;

        private bool _descanso;

        public bool Descanso
        {
            get { return _descanso; }
            set { _descanso = value; }
        }


        #endregion

        /**
         * Constructor
         */
        public Crono(InterfaceIPF[] ipf, IdiomaData[] idioma, int n, Controlador controlador)
        {

            // Console Crono
            _ipf = ipf;
            _idioma = idioma;

            _momento = new Momento(1);
            _play = false;

            _numipf = n;

            _controller = controlador;


            // Manual Crono
            _momentoManual = new Momento(1);

            Descanso = false;

            _stopwatch = new Stopwatch();           

            _timer = new Timer();
            //_timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
            _timer.Elapsed += new ElapsedEventHandler(onTimedEvent_Manual);
            _timer.Interval = 1000;

            _playManual = false;

            timer_pause = new Timer(1200);

        }


        #region Crono Consola

        /**
         * Enlaza el formulario
         * Permite la actualización del cronómetro en el GUI
         */
        public void Bind(MainForm crono)
        {
            _gui = crono;
        }

        public void set_segundoAbsolutoReferencia(int segundos)
        {
            _segundoAbsolutoReferencia = segundos;
            _momento.SegundoAbsoluto = segundos;
            updateFormAsync();

            configCrono(_momento.GetMinuto(), _momento.GetSegundo(), _momento.GetNombreParte(_idioma[0], Descanso), _momento.GetFinalParte(), genParteAbr(_momento.Parte));
        }


        public void cronoRun(string timeStamp, bool consola)
        {

            // Crono empezar
            if (!_play)
            {
                _play = true;

                //playCronoIpf();

                activaOpciones();
                Program._PrimerComienzo = true;
            }

            try
            {

                // Convert timeStamp
                int segundos_actual = timeStampToInt(timeStamp);

                // Update Momento
                //SI VIENE DEL CRONO HAY QUE AÑADIRLE EL PERIODO PORQUE LA CONSOLA VA DE 0 a 30 CADA PARTE
                if (consola)
                {
                    _momento.SegundoAbsoluto = (segundos_actual + addTimePeriod());
                }
                else
                {
                    _momento.SegundoAbsoluto = (segundos_actual);
                }
                

                updateFormAsync();
            }
            catch (Exception crono_error)
            {
                Console.WriteLine("Error at cronoRun " + crono_error);
            }


        }


        private int timeStampToInt(string date)
        {
            string[] timeformats = { @"m\:ss", @"mm\:ss", @"h\:mm\:ss" };
            TimeSpan duration = TimeSpan.ParseExact(date, timeformats, CultureInfo.InvariantCulture);

            return (int)duration.TotalSeconds;
        }



        // Llamada asincrona para actualizar el GUI
        private delegate void SimpleMethodCall(string texto);
        private void updateFormAsync()
        {
            SimpleMethodCall myMethod = new SimpleMethodCall(_gui.SetCronoTextAsync);
            myMethod.BeginInvoke(_momento.GetTextoCrono(_idioma[0], Descanso), null, null);
        }



        public int addTimePeriod()
        {
            int tiempoPeriodo = _momento.GetSegundosParte(_momento.getParte());

            //Console.WriteLine("Veamos el tiempo de esta aprte en cocnreto " 
            //    + tiempoPeriodo
            //    + ", es la parte "
            //    + _momento.getParte());

            return tiempoPeriodo;

        }


        public void SetMomento_console(Momento momento)
        {
            _momento = momento;
            //_segundoAbsolutoReferencia = momento.GetSegundosParte(momento.getParte());


            //configCrono(momento.GetMinuto(), momento.GetSegundo(),
            //    momento.GetNombreParte(_idioma[0]), momento.GetFinalParte(), genParteAbr(momento.Parte));
        }





        /**
         * Indica si el cronómetro está corriendo
         */
        public bool IsPlay()
        {
            return _play;
        }



        /**
         * Actualiza el estado del Ipf relativo al cronómetro
         */
        public void UpdateIpf()
        {
            configCrono(_momento.GetMinuto(), _momento.GetSegundo(),
                _momento.GetNombreParte(_idioma[0], Descanso), _momento.GetFinalParte(), genParteAbr(_momento.Parte));

            if (IsPlay())
                playCronoIpf();
            else
                stopCronoIpf();
        }








        #endregion


        #region Crono manual

        /*
         * Manual crono which updates each second, compares values
         * with console crono and let us know when the game has stopped
         * 
         */
        //public void CronoManualEmpezar()
        //{
        //    // Si es necesario inicia la nueva parte
        //    if (_momento.Parte % 2 != 0)
        //        SetMomento(new Momento(_momento.Parte + 1));

        //    //_stopwatch.Start();

        //    _play = true;
        //    _timer.Enabled = true;
        //    playCronoIpf();

        //    activaOpciones();
        //    Program._PrimerComienzo = true;
        //}

        //public void startCrono_Manual()
        //{

        //    if (!_playManual)
        //    {
        //        _playManual = true;

        //        //_stopwatch.Restart();
        //        _timer.Enabled = true;

        //        // Lista de cronos de exclusion
        //        /*
        //         * foreach (Crono_exclusion c in lista_cronos){
        //         * 
        //         *      c.start();
        //         * 
        //         * }
        //         * 
        //         */
        //    }

        //}

        private void onTimedEvent_Manual(object source, ElapsedEventArgs e)
        {

            if (!_gui.get_escuchar_crono())
            {
                // MANUAL
                if (true)//if (_playManual)
                {
                    //SEGUNDO DESDE EL QUE SE PARTE, SE VA ACTUALIZANDO
                    _momento.SegundoAbsoluto = _segundoAbsolutoReferencia +_stopwatch.Elapsed.Seconds + _stopwatch.Elapsed.Minutes * 60;

                    //SI SUPERA EL TIEMPO MÁXIMO DE LA PARTE SE PARA AUTOMÁTICAMENTE
                    if (_momento.SegundoAbsoluto <= _momento.GetSegundosParte(_momento.Parte+1))
                    {
                        if (_momento.GetMinuto()>59)
                        {

                            cronoRun("01:" + (_momento.GetMinuto()%60).ToString("00") + ":" + _momento.GetSegundo().ToString("00"), false);
                        }
                        else
                        {
                            cronoRun(_momento.GetMinuto().ToString("00") + ":" + _momento.GetSegundo().ToString("00"), false);
                        }
                    }
                    else
                    {
                        _momento.SegundoAbsoluto = _momento.GetSegundosParte(_momento.Parte + 1);
                        Parar();
                    }

                    //int manual_absolute = _momentoManual.GetSegundo() + _momentoManual.GetMinuto() * 60;
                    //int console_absolute = _momento.GetSegundo() + _momento.GetMinuto() * 60;

                    //Console.WriteLine("*** "+manual_absolute + ", " + console_absolute);

                    //if (manual_absolute >= console_absolute)
                    //{
                    //    Console.WriteLine("Paramos el tiempo en " + manual_absolute);
                    //    stopCrono_Manual();
                    //}

                    //updateTimeAsync(_momento.GetMinuto().ToString("00") + "'" + _momento.GetSegundo().ToString("00") + "''");
                    
                }
            }
            else
            {
                
            }
        }

        private void updateTimeAsync(string text)
        {
            SimpleMethodCall myMethod = new SimpleMethodCall(_gui.SetCronoTextAsync);
            myMethod.BeginInvoke(text, null, null);
        }



        //public void setupCrono_Manual(Momento _momentoAnterior)
        //{
        //    //_segundoAbsolutoReferencia = _momentoAnterior.SegundoAbsoluto;
        //    _momentoManual = _momentoAnterior;

        //    //
        //    Console.WriteLine("El momento actual es " + _momento.SegundoAbsoluto 
        //                    + ", " + _momentoManual.SegundoAbsoluto 
        //                    + ", " + _momentoAnterior.SegundoAbsoluto);
        //}


        public void stopCrono_Manual()
        {
            //_stopwatch.Stop();
            _timer.Enabled = false;

            _playManual = false;


            // Lista de cronos de exclusion
            /*
             * foreach (Crono_exclusion c in lista_cronos){
             * 
             *      c.stop();
             * 
             * }
             * 
             */

            // Si el crono se está actualizando, vamos corriendo.
            // En el momento en que se pare
            /*if (_datos.EmptyGoal.Local_empty)
                _datos.EmptyGoal.stopEmptyGoal(true);

            if (_datos.EmptyGoal.Visitante_empty)
                _datos.EmptyGoal.stopEmptyGoal(false);*/


        }


        /**
         * Indica si el cronómetro está corriendo
         */
        public bool IsPlayManual()
        {
            return _playManual;
        }

        #endregion
        


        /**
         * Arranca el cronómetro
         */
        public void Empezar()
        {

            //// Si es necesario inicia la nueva parte
            //if (_momento.Parte % 2 != 0)
            //    SetMomento(new Momento(_momento.Parte + 1));

            _stopwatch.Start();

            _play = true;
            _timer.Enabled = true;
            Descanso = false;
            playCronoIpf();

            //_timer.Start();

            activaOpciones();
            Program._PrimerComienzo = true;

        }

        public void Parar()
        {
            //if (_play)
            //{
            _stopwatch.Reset();

            //_play = false;
            _timer.Enabled = false;
            stopCronoIpf();

            _segundoAbsolutoReferencia = _momento.SegundoAbsoluto;

            //}
            //else
            //{
            //    _stopwatch.Start();

            //    _play = true;
            //    _timer.Enabled = true;
            //    playCronoIpf();
            //}
        }

        /**
         * Finaliza la parte en curso
         */
        public void FinParte()
        {
            _gui.SetCronoLive(false);

            System.Threading.Thread.Sleep(1000);

            Descanso = true;

            finParteAux(_momento.Parte + 2);

            //stopCronoIpf();
        }

        /**
         * Finaliza el partido
         */
        public void FinPartido()
        {
            _gui.SetCronoLive(false);

            System.Threading.Thread.Sleep(1000);

            if (_momento.Parte == Momento.Penaltis)
            {
                //finParteAux(Momento.FinPenaltis);
            }
            else
            {
                //finParteAux(Momento.FinPartido);
            }

            _momento.Parte = Momento.FinPartidoDescanso;

            stopCronoIpf();

            //config
            configCrono(_momento.GetMinuto(), _momento.GetSegundo(),
                _momento.GetNombreParte(_idioma[0], Descanso), _momento.GetFinalParte(), 7);    //es un 7 porque es el final del partido: 1,2,3,4,5,6,7
        }

        /**
         * Resetea el cronómetro
         */
        public void Reset()
        {
            //int p;
            //if (_momento.Parte % 2 == 0)
            //    p = _momento.Parte;
            //else
            //    p = _momento.Parte - 1;

            finParteAux(_momento.Parte);
        }

        

        /**
         * Establece el momento actual
         */
        public void SetMomento(Momento momento)
        {
            
            _momento = momento;
            //SetPeriodo(momento.Parte);
            //_segundoAbsolutoReferencia = momento.SegundoAbsoluto;
            _momento.SegundoAbsoluto = momento.SegundoAbsoluto;
            _segundoAbsolutoReferencia = momento.SegundoAbsoluto;
            _stopwatch.Reset();
            //if (IsPlay())
            //    _stopwatch.Start();

            updateFormAsync();

            Console.WriteLine(_momento.GetMinuto() + ", " + _momento.GetSegundo());

            configCrono(_momento.GetMinuto(), _momento.GetSegundo(),
                _momento.GetNombreParte(_idioma[0], Descanso), _momento.GetFinalParte(), genParteAbr(_momento.Parte));

            activaOpciones();
        }

        /**
         * Devuelve el momento actual
         */
        public Momento GetMomento()
        {
            return new Momento(_momento);
        }

        
        /**
         * Establece el PERIODO actual
         */
        public void SetPeriodo(int periode)
        {
            //NO ESCUCHAMOS DE LA CONSOLA LA PARTE, LA LLEVAMOS MANUAL
            //parte_partido = periode;
            //_momento.Parte = (periode*2)-1; //Periodo según la Clase Momento (sus constantes);
        }

        /**
         * Devuelve el PERIODO actual
         */
        public int GetPeriodo()
        {
            return -1; //parte_partido;
        }

        /**
         * Establece el TIEMPO (string) actual
         */
        private bool iniciado = false;

        private string momento_anterior = "";

        public void bol_iniciar(string time_)
        {
            iniciado = true;

            Descanso = false;

            //CONFIGURAR CRONO
            configCrono(Int16.Parse(time_.Split(':')[0]), Int16.Parse(time_.Split(':')[1]), _momento.GetNombreParte(_idioma[0], Descanso), _momento.GetFinalParte(), genParteAbr(_momento.Parte));

            _controller.PosesionActivar(true);

            //Empezar();
            playCronoIpf();
        }
        public bool get_iniciado()
        {
            return iniciado;
        }


        private void HandleTimer(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("PAUSA");
            timer_pause.Stop();
            iniciado = false;
            _gui.ActivaOpcionesCrono();
            //Parar();
            //ipf
            _controller.PosesionActivar(false);
            stopCronoIpf();
        }


        public void SetTempo_String(string time_)
        {
            tempo_string = time_;

            _momentoManual.Update(Int16.Parse(time_.Split(':')[0]), Int16.Parse(time_.Split(':')[1]));

            timer_pause.Stop();

            timer_pause = new Timer(1100);
            timer_pause.Elapsed += new ElapsedEventHandler(HandleTimer);

            timer_pause.Start();
        }

        private ElapsedEventHandler HandleTimer()
        {
            throw new NotImplementedException();
        }

        /**
         * Devuelve el TIEMPO (string) actual
         */
        public string GetTempo_String()
        {
            return tempo_string;
        }



        // Establece el momento como el final de la parte indicada
        private void finParteAux(int parte)
        {
            _play = false;
            _timer.Enabled = false;

            stopCronoIpf();

            _stopwatch.Reset();
         

           // _gui.SetCronoText("");

            SetMomento(new Momento(parte));
        }

        // Genera el flag ParteAbr a partir de la Parte
        private int genParteAbr(int parte)
        {
            int r = (parte + 1) / 2;
            r--;
            return r;
        }


        //// Incremente el cronómetro cada segundo
        //private void onTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    //_momento.Update(_stopwatch.Elapsed.Minutes, _stopwatch.Elapsed.Seconds);
        //    _momento.SegundoAbsoluto = _segundoAbsolutoReferencia + _stopwatch.Elapsed.Seconds + _stopwatch.Elapsed.Minutes * 60;

        //    updateFormAsync();
        //}

        

        // Activa y desactiva las opciones en función al momento del partido
        private void activaOpciones()
        {
            bool kickoff;
            bool finParte;
            bool finPartido;

            /*kickoff = !_play && _momento.Parte != Momento.FinPartido && _momento.Parte != Momento.FinPenaltis;

            finParte = _play && _momento.Parte != Momento.Penaltis;

            finPartido = _play && (_momento.Parte == Momento.IniParte2 ||
                _momento.Parte == Momento.IniProrroga1_parte2 ||
                _momento.Parte == Momento.IniProrroga2_parte2 ||
                _momento.Parte == Momento.Penaltis);

            _gui.ActivaOpcionesCrono(kickoff, finParte, finPartido);*/
        }



        // ====================================== IPF ======================================

        // parteAbr: 0=Sin resumen, 1=resumen fin partido, 2=resumen penaltis 
        private void configCrono(int minutos, int segundos, string parte, int parteDuracion, int parteAbr)
        {
            for (int i = 0; i < _numipf; i++)
            {
                _ipf[i].Envia("itemset('CRONOMETRO/MinP','MAP_STRING_PAR','" + minutos + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/SecP','MAP_STRING_PAR','" + segundos + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/Parte','MAP_STRING_PAR','" + parte + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/ParteAbr','MAP_STRING_PAR','" + parteAbr + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/Duracion','MAP_STRING_PAR','" + parteDuracion + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/SetP','EXP_EXE')");
            }
        }

        private void playCronoIpf()
        {
            _gui.DesactivaOpcionesCrono();

            for (int i = 0; i < _numipf; i++)
                _ipf[i].Envia("itemset('CRONOMETRO/PlayP','EXP_EXE')");

        }
        private void stopCronoIpf()
        {
            _gui.ActivaOpcionesCrono();

            for (int i = 0; i < _numipf; i++)
                _ipf[i].Envia("itemset('CRONOMETRO/StopP','EXP_EXE')");
        }

        /**
         * Muestra u oculta el Cronómetro
         */
        public void ShowCronoIpf(bool visible)
        {
            if (visible)
            {
                for (int i = 0; i < _numipf; i++)
                {
                    if (Program.EstaActivado(i))
                        _ipf[i].Envia("itemset('CRONOMETRO/CronoIN','EXP_EXE')");
                }
            }
            else
            {
                for (int i = 0; i < _numipf; i++)
                {
                    if (Program.EstaActivado(i))
                        _ipf[i].Envia("itemset('CRONOMETRO/CronoOUT','EXP_EXE')");
                }
            }
        }


    }

    #region Crono Exclusión

    public class Crono_exclusion {

        // Cuando a un jugador se le excluya, se creará un crono individual de dos minutos
        // de modo que para crear estos objetos, necesitamos saber el dorsal del jugador y 
        // el tiempo de inicio de la exclusión, y tener acceso al tiempo que maneja la consola

        private Momento inicio;
        private Momento final_exclusion;

        private Crono ref_crono_consola;

        private string jugador_dorsal;
        private Jugador jugador;
        private string equipo;

        private int parte_momento_inicio;

        private int seg_ab_final;
        private int parte_momento_final;

        private IdiomaData[] _idioma;



        public Crono_exclusion(Momento momento_inicial, Crono crono, int parte_actual, IdiomaData[] idioma, Jugador _jugador, string equipo)
        {

            this.jugador = _jugador;
            this.equipo = equipo;


            this.ref_crono_consola = crono;

            this.inicio = momento_inicial;

            this.parte_momento_inicio = parte_actual;



            Console.WriteLine("INICIO DE EXCLUSION: " + this.inicio.SegundoAbsoluto + " , PARTE: " + this.parte_momento_inicio);


            /* CALCULAMOS EL FINAL */
            switch (parte_actual)
            {
                case 1:

                    /* EXCLUSION PRODUCIDA EN EL PRIMER TIEMPO */

                    if (this.inicio.SegundoAbsoluto+120 > 1800)
                    {
                        /* LA EXCLUSION TERMINA EN LA SIGUIENTE PARTE */

                        this.seg_ab_final = this.inicio.SegundoAbsoluto + 120 - 1800;
                        this.parte_momento_final = 2;
                    }
                    else
                    {
                        this.seg_ab_final = this.inicio.SegundoAbsoluto + 120;
                        this.parte_momento_final = 1;
                    }

                    break;

                case 2:

                    this.seg_ab_final = this.inicio.SegundoAbsoluto + 120;
                    this.parte_momento_final = 2;

                    break;

                default:
                    break;
            }

            Console.WriteLine("FINAL DE EXCLUSION: " + this.seg_ab_final + " , PARTE: " + this.parte_momento_final);



            /*

            // Calculate end time
            int fin_parteInicial = inicio.GetFinalParte();
            int segundoAbsoluto_finParte = fin_parteInicial * 60;

            int segundoAbsoluto_inicio = inicio.SegundoAbsoluto;
            int segundoAbsoluto_fin = segundoAbsoluto_inicio + 120;

            if (segundoAbsoluto_finParte >= segundoAbsoluto_fin)
            {
                // Podemos marcar como fin del periodo de exclusion
                // segundoAbsoluto_fin
                
                final_exclusion = new Momento(segundoAbsoluto_fin, inicio);
                Console.WriteLine("Mira que bien, entra en esta parte " + segundoAbsoluto_fin + ", parte: " + inicio.getParte());

            }
            else
            {
                // Recalculamos en la siguiente parte
                int parte_inicio = inicio.getParte();

                if (parte_inicio + 2 >= 14)
                {

                    // Ya estamos en la última 
                    // Fin de exclusion en esa parte al segundo final
                    final_exclusion = new Momento(segundoAbsoluto_fin, new Momento(parte_inicio));

                    Console.WriteLine("Estamos ya en la última parte " +
                        + final_exclusion.SegundoAbsoluto
                        + ", "
                        + final_exclusion.getParte() 
                        + ", "
                        + final_exclusion.GetNombreParte(idioma[0]));
                }
                else
                {
                    // Vamos a la siguiente parte
                    final_exclusion = new Momento(segundoAbsoluto_fin, new Momento(parte_inicio + 2));

                    Console.WriteLine("Tenemos que seguir a la siguiente parte " + 
                        + final_exclusion.SegundoAbsoluto
                        + ", "
                        + final_exclusion.getParte()
                        + ", " 
                        + final_exclusion.GetNombreParte(idioma[0]));
                }

            }

            */

        }

        public int get_seg_ab_final()
        {
            return this.seg_ab_final;
        }

        public int get_parte_momento_final()
        {
            return this.parte_momento_final;
        }


        public bool check_exclusion(Momento actual)
        {
            if (actual.SegundoAbsoluto == this.final_exclusion.SegundoAbsoluto)
            {
                //
                Console.WriteLine("Se ha terminado la exclusion del jugador " + jugador.Number 
                    + " del equipo " + equipo
                    + ", llamado " + jugador.FullName);

                return true;
            }

            return false;
        }

        public Jugador get_player_ex()
        {
            return jugador;
        }


        //private InterfaceIPF[] _ipf;
        //private int _numipf;
        /*private Crono _crono;
        private IdiomaData[] _idioma;

        private Momento _momento;
        private Momento _momento_final;
        private bool _play;

        private MainForm _gui;
        private Timer _timer;
        private int _segundoAbsolutoReferencia;
        private Stopwatch _stopwatch;

        private string _id_jugador;
        private string _equipo;


        // Constructor
        public Crono_exclusion(Crono crono, IdiomaData[] idioma, string dorsal_jugador, string equipo, Momento momento)
        {
            _crono = crono;

            _idioma = idioma;

            _stopwatch = new Stopwatch();

            // Determinamos en base al momento actual el tiempo y calculamos cuál sería el momento final
            _momento = momento;
            _momento_final = getFinTiempoExclusion(_momento);

            // Configura el timer
            _timer = new Timer();
            _timer.Elapsed += new ElapsedEventHandler(onTimedEvent_CronoExclusion);
            _timer.Interval = 1000;

            _id_jugador = dorsal_jugador;
            _equipo = equipo;


            iniciarCronoExclusion();

        }

        // Iniciar crono de exclusión
        public void iniciarCronoExclusion()
        {
            // Se chequea si el crono general está activado
            if (_crono.IsPlay())
            {
                _stopwatch.Start();

                _play = true;
                _timer.Enabled = true;
            }
        }

        // Fin de crono de exclusión
        public void finCronoExclusion()
        {
            _play = false;
            _timer.Enabled = false;

            System.Console.WriteLine("Se ha detenido el crono de exclusion del jugador " + _id_jugador + " del equipo " + _equipo);
        }

        //
        public Momento getFinTiempoExclusion(Momento momento_inicial)
        {
            int tiempo_inicial = momento_inicial.SegundoAbsoluto;
            int tiempo_final = tiempo_inicial + 120;

            return new Momento(tiempo_final, momento_inicial);
        }

        // Incremente el cronómetro cada segundo
        private void onTimedEvent_CronoExclusion(object source, ElapsedEventArgs e)
        {
            //_momento.Update(_stopwatch.Elapsed.Minutes, _stopwatch.Elapsed.Seconds);
            _momento.SegundoAbsoluto = _segundoAbsolutoReferencia + _stopwatch.Elapsed.Seconds + _stopwatch.Elapsed.Minutes * 60;

            System.Console.WriteLine("El cronometro de la exclusion va por " + _momento.SegundoAbsoluto + ", " + _momento_final.SegundoAbsoluto);
            
            if (checkTiempoExclusion())
            {
                System.Console.WriteLine("Oh vaya");
                finCronoExclusion();
            }
        }

        private bool checkTiempoExclusion()
        {
            if (_momento.SegundoAbsoluto == _momento_final.SegundoAbsoluto)
                return true;
            else
                return false;
        }

        public bool exclusion_IsPlay()
        {
            return _play;
        }
        */


       
    }

    #endregion

}