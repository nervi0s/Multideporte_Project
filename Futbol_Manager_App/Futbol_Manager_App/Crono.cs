using System.Timers;
using Futbol_Manager_App.Interfaz;
using Futbol_Manager_App.Persistencia;
using System.Diagnostics;

namespace Futbol_Manager_App
{
    /**
     * Lógica del cronómetro
     */
    public class Crono
    {
        private InterfaceIPF[] _ipf;
        private IdiomaData[] _idioma;

        private Momento _momento;
        private bool _play;
        
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
            _timer.Elapsed += new ElapsedEventHandler(onTimedEvent);
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

        /**
         * Arranca el cronómetro
         */
        public void Empezar()
        {
            // Si es necesario inicia la nueva parte
            if (_momento.Parte % 2 != 0)
                SetMomento(new Momento(_momento.Parte + 1));

            _stopwatch.Start();

            _play = true;
            _timer.Enabled = true;
            playCronoIpf();

            activaOpciones();
            Program._PrimerComienzo = true;

        }

        /**
         * Finaliza la parte en curso
         */
        public void FinParte()
        {
            _gui.SetCronoLive(false);

            System.Threading.Thread.Sleep(1000);
            finParteAux(_momento.Parte + 1);
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
                finParteAux(Momento.FinPenaltis);
            }
            else
            {
                finParteAux(Momento.FinPartido);
            }
        }

        /**
         * Resetea el cronómetro
         */
        public void Reset()
        {
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
            configCrono(_momento.GetMinuto(), _momento.GetSegundo(),
                _momento.GetNombreParte(_idioma[0]), _momento.GetFinalParte(), genParteAbr(_momento.Parte));

            if (IsPlay())
                playCronoIpf();
            else
                stopCronoIpf();
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

            updateFormAsync();
            
            configCrono(momento.GetMinuto(), momento.GetSegundo(),
                momento.GetNombreParte(_idioma[0]), momento.GetFinalParte(), genParteAbr(momento.Parte));

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

            stopCronoIpf();

            SetMomento(new Momento(parte));
        }

        // Genera el flag ParteAbr a partir de la Parte
        private int genParteAbr(int parte)
        {
            if (parte == Momento.FinPartido)
            {
                return 1;
            }
            else if (parte == Momento.FinPenaltis)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        // Incremente el cronómetro cada segundo
        private void onTimedEvent(object source, ElapsedEventArgs e)
        {
            //_momento.Update(_stopwatch.Elapsed.Minutes, _stopwatch.Elapsed.Seconds);
            _momento.SegundoAbsoluto = _segundoAbsolutoReferencia + _stopwatch.Elapsed.Seconds + _stopwatch.Elapsed.Minutes * 60;

            updateFormAsync();
        }

        // Llamada asincrona para actualizar el GUI
        private delegate void SimpleMethodCall(string texto);
        private void updateFormAsync()
        {
            SimpleMethodCall myMethod = new SimpleMethodCall(_gui.SetCronoTextAsync);
            myMethod.BeginInvoke(_momento.GetTextoCrono(_idioma[0]), null, null);
        }


        // Activa y desactiva las opciones en función al momento del partido
        private void activaOpciones()
        {
            bool kickoff;
            bool finParte;
            bool finPartido;

            kickoff = !_play && _momento.Parte != Momento.FinPartido && _momento.Parte != Momento.FinPenaltis;

            finParte = _play && _momento.Parte != Momento.Penaltis;

            finPartido = _play && (_momento.Parte == Momento.IniParte2 || 
                _momento.Parte == Momento.IniProrroga2 ||
                _momento.Parte == Momento.Penaltis);

            _gui.ActivaOpcionesCrono(kickoff, finParte, finPartido);
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
            for (int i = 0; i < _numipf;i++ )
                _ipf[i].Envia("itemset('CRONOMETRO/PlayP','EXP_EXE')");

        }
        private void stopCronoIpf()
        {
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
}
