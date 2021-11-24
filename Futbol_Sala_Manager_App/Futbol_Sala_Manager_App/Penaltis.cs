using System.Collections.Generic;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Interfaz;

namespace Futbol_Sala_Manager_App
{

    /**
     * Lógica de los penaltis
     */
    public class Penaltis
    {
        private InterfaceIPF[] _ipfs;
        private MainForm _gui;
        private List<Penalti> _penaltis;
        private bool _flagPenaltiAcierto;
        private int _numIpf;


        /**
         * Constructor
         */
        public Penaltis(InterfaceIPF[] ipf, MainForm gui, int n)
        {
            _ipfs = ipf;
            _gui = gui;
            _numIpf = n;

            _penaltis = new List<Penalti>();
        }

        /**
         * Añade un penalti
         * El penalti se asigna al jugador indicado. Se considera acierto o fallo 
         * en función de lo indicado en la última llamada a SetFlagPenaltiAcierto()
         */
        public void AddPenalti(Jugador jugador)
        {
            Penalti p = new Penalti(jugador, _flagPenaltiAcierto);
            _penaltis.Add(p);

            _gui.GetPenaltisGui().AddPenalti(jugador.Equipo.Local, jugador.ShortName, _flagPenaltiAcierto);
            _gui.GetPenaltisGui().SetMarcador(genMarcador());
            updatePenaltisIpf(_penaltis);
        }

        /**
         * Elimina el último penalti añadido
         */
        public void DelPenalti()
        {
            if (_penaltis.Count > 0)
            {
                bool localLast = _penaltis[_penaltis.Count - 1].Jugador.Equipo.Local;

                _penaltis.RemoveAt(_penaltis.Count - 1);

                _gui.GetPenaltisGui().DelPenalti(localLast);
                _gui.GetPenaltisGui().SetMarcador(genMarcador());

                UpdateIpf();
            }
        }

        /**
         * Establece el valor de acierto o fallo para el siguiente penalti
         */
        public void SetFlagAcierto(bool acierto)
        {
            _flagPenaltiAcierto = acierto;

            _gui.ActivaTodosJugadores(true);
            _gui.ActivaTodosJugadores(false);
        }

        /**
         * Actualiza el estado del IPF
         */
        public void UpdateIpf()
        {
            resetIpf();
            // Ejecuta todos los penaltis desde el principio
            for (int i = 1; i <= _penaltis.Count; i++)
                updatePenaltisIpf(_penaltis.GetRange(0, i));
        }

        // ================================== AUXILIARES ===================================

        private int goles(bool local)
        {
            int goles = 0;

            foreach (Penalti p in _penaltis)
            {
                if (p.Jugador.Equipo.Local == local && p.Acierto)
                {
                    goles++;
                }
            }
            return goles;
        }

        private string teamCode(bool local)
        {
            string code = "";

            foreach (Penalti p in _penaltis)
            {
                if (p.Jugador.Equipo.Local == local)
                {
                    code = p.Jugador.Equipo.TeamCode;
                    break;
                }
            }
            return code;
        }

        private string genMarcador()
        {
            return "Penaltis: " + goles(true) + " - " + goles(false);
        }

        // ===================================== IPF ======================================

        private void updatePenaltisIpf(List<Penalti> penaltis)
        {
            bool local = penaltis[penaltis.Count - 1].Jugador.Equipo.Local;

            string progreso = "";
            int cont = 0;

            foreach (Penalti p in penaltis)
            {
                if (p.Jugador.Equipo.Local == local)
                {
                    progreso += (p.Acierto ? ",1" : ",0");
                    cont++;
                }
            }
            for (int i = 0; i < 20 - cont; i++)
            {
                progreso += ",-1";
            }

            string s = "SetPenalty(['" + teamCode(local) + "', '', " + goles(local) + progreso + "])";
            for (int i = 0; i < _numIpf; i++)
            {
                if (Program.EstaActivado(i))
                    _ipfs[i].Envia(s);
            }
        }

        private void resetIpf()
        {

            for (int i = 0; i < _numIpf; i++)
            {
                if (Program.EstaActivado(i))
                    _ipfs[i].Envia("ResetPenalties()");
            }
        }

        public void showPenaltisIpf(bool visible)
        {
            if (visible)
            {
                for (int i = 0; i < _numIpf; i++)
                {
                    if (Program.EstaActivado(i))
                        _ipfs[i].Envia("PenaltiesIN()");
                        //_ipfs[i].Envia("itemset('MarcadorPenalties/PenaltiesIN','EXP_EXE')");
                }
            }
            else
            {
                for (int i = 0; i < _numIpf; i++)
                {
                    if (Program.EstaActivado(i))
                        _ipfs[i].Envia("PenaltiesOUT()");
                        //_ipfs[i].Envia("itemset('MarcadorPenalties/PenaltiesOUT','EXP_EXE')");
                }
            }
        }


        // ================================= Bean Penalti =================================
        /**
         * Bean privado para un Penalti
         */
        class Penalti
        {
            public Jugador Jugador { get; set; }
            public bool Acierto { get; set; }

            public Penalti(Jugador jugador, bool acierto)
            {
                this.Jugador = jugador;
                this.Acierto = acierto;
            }
        }


    }
}
