using System.Timers;
using Futbol_Sala_Manager_App.Interfaz;
using Futbol_Sala_Manager_App.Persistencia;
using System.Diagnostics;
using System;

namespace Futbol_Sala_Manager_App
{
    /**
     * Lógica del cronómetro
     */
    public class Crono
    {
        public InterfaceIPF[] _ipf;
        public IdiomaData[] _idioma;

        public Momento _momento;
        private bool _play;
        private bool _resumed;
        private MainForm _gui;
        private Timer _timer;
        private int _segundoAbsolutoReferencia;
        private Stopwatch _stopwatch;
        private int _numipf;


        /**
         * Constructor
         */
        public Crono(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            _ipf = ipf;
            _idioma = idioma;

            _stopwatch = new Stopwatch();
            _momento = new Momento(0);
            _play = false;

            // Configura el timer
            _timer = new Timer();
            _timer.Elapsed += onTimedEvent;
            _timer.Interval = 1000;
            _numipf = n;
        }


        /**
         * Enlaza el formulario
         * Permite la actualización del cronómetro en el GUI
         */
        public void Bind(MainForm crono)
        {
            _gui = crono;
        }

        private void ResetStopwatch()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        /*
         * Arranca el cronómetro
         */
        private bool firstStart = true;
        public void Empezar()
        {
            // Envía una trama de información sobre la info del crono al IPF ANTES de mandar el comando play(). Si la aplicación está en modo Principal y se pretende ganar el control del cronómetro
            if (!_gui.GetConfigData().isMachineCrono && !_gui.GetConfigData().modeOcrActivated)
                _gui.getControlador().CronoEdit(_momento._cadena_minuto.ToString("00"), _momento._cadena_segundo.ToString("00"), _momento.Parte);

            // Si es necesario inicia la nueva parte
            if (_momento.Parte % 2 != 0)
            {
                Console.WriteLine(">>> Detectada necesidad de iniciar nueva parte.");
                SetMomentoCambioDeParte(new Momento(_momento.Parte + 1));
                Controlador.parteDeReferencia += 1;
            }

            if (_momento.Parte == Momento.Penaltis)
            {
                firstStart = true;
            }

            //Console.WriteLine("1 > " + _momento._cadena_minuto + ":" + _momento._cadena_segundo);
            //Console.WriteLine("2 > " + _momento._cadena_minuto + ":" + _momento._cadena_segundo);
            //Console.WriteLine("3 > " + _momento._cadena_minuto + ":" + _momento._cadena_segundo);

            if (_gui.GetConfigData().isMachineCrono)
            {
                if (firstStart)
                {
                    ResetStopwatch();
                    _timer.Start();
                    _play = true;
                    _gui.GetConfigData().ownedCronoControl = true;
                    firstStart = false;
                }
                else
                {
                    _play = true;
                    _resumed = true;
                    _timer.Interval = RemainingAfterPause;
                    RemainingAfterPause = 0;
                    _timer.Start();
                    _gui.GetConfigData().ownedCronoControl = true;
                }

            }
            else
            {
                if (Controlador.llamadaDesdeCronoApp)
                {
                    _play = true;
                }
                else
                {
                    if (_gui.GetConfigData().modeOcrActivated)
                    {
                        _stopwatch.Start();
                        _play = true;
                        _timer.Enabled = true;
                        configCronoInfoParteWithOcr(_momento.GetNombreParte(_idioma[0]), genParteAbr(_momento.Parte));
                    }
                    else
                    {
                        if (!_gui.GetConfigData().ownedCronoControl)
                        {
                            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
                            System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("¿Quieres tomar el control del cronómetro?", "Advertencia", buttons);
                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {
                                AsynchronousSocketListener.StopListening();

                                ResetStopwatch();
                                _timer.Start();
                                _play = true;
                                _gui.GetConfigData().ownedCronoControl = true;
                            }
                        }
                        else
                        {
                            if (firstStart)
                            {
                                ResetStopwatch();
                                _timer.Start();
                                _play = true;
                                _gui.GetConfigData().ownedCronoControl = true;
                                firstStart = false;
                            }
                            else
                            {
                                AsynchronousSocketListener.StopListening();

                                _play = true;
                                _resumed = true;
                                _timer.Interval = RemainingAfterPause;
                                RemainingAfterPause = 0;
                                _timer.Start();
                                _gui.GetConfigData().ownedCronoControl = true;
                            }

                        }
                    }
                }
            }
            if (!_gui.GetConfigData().isMachineCrono)
            {
                if (_momento.getParte() == Momento.Penaltis)
                    _play = false;
                configCronoInfoEstado();
            }
            activaOpciones();
            Program._PrimerComienzo = true;
        }
        public double RemainingAfterPause { get; private set; }
        /*
         * Para el cronómetro
         */
        public void Parar()
        {
            _timer.Stop();
            _stopwatch.Stop();
            _play = false;
            RemainingAfterPause = _timer.Interval - _stopwatch.Elapsed.TotalMilliseconds;

            if (!_gui.GetConfigData().isMachineCrono)
                configCronoInfoEstado();

            activaOpciones();
        }

        /**
         * Finaliza la parte en curso
         */
        public void FinParte()
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            firstStart = true;

            _gui.SetCronoLive(false);

            finParteAux(_momento.Parte + 1);

            if (!_gui.GetConfigData().isMachineCrono)
                CronoOut();
        }

        /**
         * Finaliza el partido
         */
        public void FinPartido()
        {
            _gui.SetCronoLive(false);

            if (_momento.Parte == Momento.Penaltis)
            {
                finParteAux(Momento.FinPenaltis);
            }
            else
            {
                finParteAux(Momento.FinPartido);
            }
            if (!_gui.GetConfigData().isMachineCrono)
                CronoOut();
        }

        /**
         * Resetea el cronómetro
         */
        public void Reset()
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            firstStart = true;

            int p;
            if (_momento.Parte % 2 == 0)
                p = _momento.Parte;
            else
                p = _momento.Parte - 1;

            finParteAux(p);
        }

        /**
         * Actualiza el estado del Ipf relativo al cronómetro
         */
        public void UpdateIpf()
        {
            //Console.WriteLine("Update IPF > " + _momento.GetMinuto() + ":" + _momento.GetSegundo());

            configCrono(_momento.GetMinutoCrono(), _momento.GetSegundo(), _momento.GetNombreParte(_idioma[0]), _momento.GetFinalParte(), genParteAbr(_momento.Parte));

            //if (!IsPlay())
            //    stopCronoIpf();
        }

        /**
         * Establece el momento actual
         */
        public void SetMomento(Momento momento)
        {
            _momento = momento;
            _segundoAbsolutoReferencia = momento.SegundoAbsoluto;
            _stopwatch.Reset();

            if (IsPlay())
                _stopwatch.Start();

            updateFormSync();

            Console.WriteLine("Momento, en parte -> " + momento.Parte);


            if (!_gui.GetConfigData().isMachineCrono)
            {
                if (_gui.GetConfigData().modeOcrActivated)
                {
                    configCrono(Ocr.minute, Ocr.second, momento.GetNombreParte(_idioma[0]), momento.GetFinalParte(), genParteAbr(momento.Parte));
                }
                else
                {
                    configCrono(momento.GetMinutoCrono(), momento.GetSegundo(), momento.GetNombreParte(_idioma[0]), momento.GetFinalParte(), genParteAbr(momento.Parte));
                }
            }
            activaOpciones();
        }

        public void SetMomentoCambioDeParte(Momento momento)
        {
            _momento = momento;
            _segundoAbsolutoReferencia = momento.SegundoAbsoluto;
            _stopwatch.Reset();

            if (IsPlay())
                _stopwatch.Start();

            updateFormSync();

            Console.WriteLine("Momento, en parte -> " + momento.Parte);


            if (!_gui.GetConfigData().isMachineCrono)
            {
                if (_gui.GetConfigData().modeOcrActivated)
                {
                    configCronoInfoParte(Ocr.minute, Ocr.second, momento.GetNombreParte(_idioma[0]), momento.GetFinalParte(), genParteAbr(momento.Parte));
                }
                else
                {
                    configCronoInfoParte(momento.GetMinutoCrono(), momento.GetSegundo(), momento.GetNombreParte(_idioma[0]), momento.GetFinalParte(), genParteAbr(momento.Parte));
                }
            }
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
         * Indica si el cronómetro está corriendo
         */
        public bool IsPlay()
        {
            return _play;
        }

        // Establece el momento como el final de la parte indicada
        private void finParteAux(int parte)
        {
            _play = false;
            _timer.Enabled = false;

            //if (!_gui.GetConfigData().isMachineCrono)
            //    stopCronoIpf();
            if (parte == 9) // En caso de llegar al momento 9 (Fin de penaltis lo pasamo al momento 10, que es fin de partido)
                parte = 10;
            SetMomentoCambioDeParte(new Momento(parte));
        }

        // Genera el flag ParteAbr a partir de la Parte
        public int genParteAbr(int parte)
        {
            //if ((parte == Momento.IniParte1) || (parte == Momento.FinParte1))
            //    return 0;
            //else if ((parte == Momento.IniParte2) || (parte == Momento.FinParte2))
            //    return 1;
            //else if ((parte == Momento.IniProrroga1) || (parte == Momento.FinProrroga1))
            //    return 2;
            //else if ((parte == Momento.IniProrroga2) || (parte == Momento.FinProrroga2))
            //    return 3;
            //if (parte == Momento.FinPartido)
            //    return 4;
            //else if (parte == Momento.FinPenaltis)
            //    return 5;
            //else
            //    return 0;
            return parte;
        }

        // Incremente el cronómetro cada segundo
        private void onTimedEvent(object source, ElapsedEventArgs e)
        {
            if (_resumed)
            {
                _resumed = false;
                _timer.Stop();
                _timer.Interval = 1000;
                _timer.Start();


            }
            if (_gui.GetConfigData().modeOcrActivated)
            {
                if (!Ocr.decimas_showed) // En caso de que haya décimas en videomarcador
                {
                    _momento._cadena_minuto = Ocr.minute;
                    _momento._cadena_segundo = Ocr.second;
                }
            }
            else
            {
                if (_momento._cadena_minuto > 0)
                {
                    if (_momento._cadena_segundo == 0)
                    {
                        _momento._cadena_segundo = 59;
                        _momento._cadena_minuto = _momento._cadena_minuto - 1;
                    }
                    else
                        _momento._cadena_segundo = _momento._cadena_segundo - 1;
                }
                else
                {
                    if (_momento._cadena_segundo > 0)
                    {
                        if (_momento._cadena_segundo == 0)
                        {
                            _momento._cadena_segundo = 59;
                            _momento._cadena_minuto = _momento._cadena_minuto - 1;
                        }
                        else
                            _momento._cadena_segundo = _momento._cadena_segundo - 1;
                    }
                }

                if (_gui.GetConfigData().isMachineCrono || _gui.GetConfigData().ownedCronoControl)
                {
                    _gui.getControlador().CronoEdit(_momento._cadena_minuto.ToString("00"), _momento._cadena_segundo.ToString("00"));
                    if (_momento._cadena_minuto == 0 && _momento._cadena_segundo == 0)
                    {
                        _stopwatch.Stop();
                        _play = false;
                        _timer.Enabled = false;
                    }
                }
            }

            if (!_gui.GetConfigData().modeOcrActivated)
                updateFormAsync();
            ResetStopwatch();
        }

        // Llamada asincrona para actualizar el GUI
        private delegate void SimpleMethodCall(string texto, string minutes, string seconds);
        private void updateFormAsync()
        {
            SimpleMethodCall myMethod = new SimpleMethodCall(_gui.SetCronoTextAsync);
            myMethod.BeginInvoke(_momento.GetTextoCrono(_idioma[0]), _momento.GetMinutoCrono().ToString("00"), _momento.GetSegundo().ToString("00"), null, null);
        }

        private delegate void SimpleMethodCallSync(string texto, string minutes, string seconds);
        private void updateFormSync()
        {
            if (_gui.GetConfigData().modeOcrActivated)
                _gui.SetCronoTextAsync(_momento.GetTextoCrono(_idioma[0]), Ocr.minute.ToString("00"), Ocr.second.ToString("00"));
            else
                _gui.SetCronoTextAsync(_momento.GetTextoCrono(_idioma[0]), _momento.GetMinutoCrono().ToString("00"), _momento.GetSegundo().ToString("00"));
        }

        // Activa y desactiva las opciones en función al momento del partido
        private void activaOpciones()
        {
            bool kickoff;
            bool finParte;
            bool finPartido;

            kickoff = !_play && _momento.Parte != Momento.FinPartido && _momento.Parte != Momento.FinPenaltis;

            finParte = /*_play && */_momento.Parte != Momento.Penaltis;

            finPartido = /*_play && */(_momento.Parte == Momento.IniParte2 ||
                _momento.Parte == Momento.IniProrroga2 ||
                _momento.Parte == Momento.Penaltis);

            _gui.ActivaOpcionesCrono(kickoff, finParte, finPartido);
        }



        // ====================================== IPF ======================================

        // parteAbr: 0=Sin resumen, 1=resumen fin partido, 2=resumen penaltis 
        public void configCrono(int minutos, int segundos, string parte, int parteDuracion, int parteAbr)
        {
            for (int i = 0; i < _numipf; i++)
            {
                _ipf[i].Envia("itemset('CRONOMETRO/CRONO','MAP_STRING_PAR','" + minutos.ToString("00") + ":" + segundos.ToString("00") + "')");
            }
        }

        public void configCronoInfoParte(int minutos, int segundos, string parte, int parteDuracion, int parteAbr)
        {
            for (int i = 0; i < _numipf; i++)
            {
                _ipf[i].Envia("itemset('CRONOMETRO/CRONO','MAP_STRING_PAR','" + minutos.ToString("00") + ":" + segundos.ToString("00") + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/Parte','MAP_STRING_PAR','" + parte + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/ParteAbr','MAP_STRING_PAR','" + parteAbr + "')");
            }
        }

        public void configCronoInfoEstado()
        {
            int estado = _play ? 1 : 0;
            //for (int i = 0; i < _numipf; i++)
            //{
            //    _ipf[i].Envia("itemset('CRONOMETRO/Active','MAP_INT_PAR'," + estado + ")");
            //}
        }

        public void configCronoWithOcr(string time, string parte, int parteDuracion, int parteAbr)
        {
            for (int i = 0; i < _numipf; i++)
            {
                _ipf[i].Envia("itemset('CRONOMETRO/CRONO','MAP_STRING_PAR','" + time + "')");
            }
        }
        public void configCronoInfoParteWithOcr(string parte, int parteAbr)
        {
            for (int i = 0; i < _numipf; i++)
            {
                _ipf[i].Envia("itemset('CRONOMETRO/Parte','MAP_STRING_PAR','" + parte + "')");
                _ipf[i].Envia("itemset('CRONOMETRO/ParteAbr','MAP_STRING_PAR','" + parteAbr + "')");
            }
        }

        //public void playCronoIpf()
        //{
        //    for (int i = 0; i < _numipf;i++ )
        //        _ipf[i].Envia("itemset('CRONOMETRO/PlayP','EXP_EXE')");

        //}
        //private void stopCronoIpf()
        //{
        //    for (int i = 0; i < _numipf; i++)
        //        _ipf[i].Envia("itemset('CRONOMETRO/StopP','EXP_EXE')");
        //}

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
                CronoOut();
            }
        }

        void CronoOut()
        {
            Console.WriteLine("CronoOut");
            for (int i = 0; i < _numipf; i++)
            {
                if (Program.EstaActivado(i))
                    _ipf[i].Envia("itemset('CRONOMETRO/CronoOUT','EXP_EXE')");
            }
        }
    }
}
