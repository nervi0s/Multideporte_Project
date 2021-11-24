using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class StatisticsTableCommand : ICommandShowable
    {
        public const int Corners = 3;
        public const int Faltas = 4;
        public const int Goles = 5;
        public const int TirosAPuerta = 6;
        public const int Paradas = 7;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int FuerasDeJuego = 10;
        public const int Cambios = 11;

        private Equipo _equipoL;
        private Equipo _equipoV;
        private Momento _tiempo;
        private string _posesionLocal;
        private string _posesionVisiante;                      

        private bool _visible;


        public StatisticsTableCommand(Momento tiempo,Equipo equipoL, Equipo equipoV, string posesionLocal, string posesionVisitante)
        {
            _equipoL = equipoL;
            _equipoV = equipoV;
            _posesionLocal = posesionLocal;
            _posesionVisiante = posesionVisitante;
            this._tiempo = tiempo;
            Reset();
        }
        
        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            int momento = _tiempo.getParte();

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                    {
                        string parte =
                            momento == 1 ? idioma[i].FinParte1 :
                            momento == 3 ? idioma[i].FinParte2 :
                            momento == 5 ? idioma[i].FinProrroga1 :
                            momento == 7 ? idioma[i].FinProrroga2 :
                            (momento == 10 || momento == 9) ? idioma[i].FinPartido : "";

                        ipf[i].Envia("StatisticsTableIN([" + "'" +
                            /* POSESIÓN */ idioma[i].Possesion + "', '" + _posesionLocal + "', '" + _posesionVisiante + "', '" +
                            /* ATTEMPTS ON TARGET */ idioma[i].IntoGoal + "','" + _equipoL.getTirosAPuerta() + "', '" + _equipoV.getTirosAPuerta() + "', '" +
                            /* TOTAL ATTEMPTS */ idioma[i].Kicks + "', '" + +_equipoL.getTirosCompuesto() + "', '" + _equipoV.getTirosCompuesto() + "', '" +
                            /* CORNERS */ idioma[i].Corners + "', '" + _equipoL.getCorners() + "', '" + _equipoV.getCorners() + "', '" +
                            /* OFFSIDES */ idioma[i].Offsides + "', '" + _equipoL.getFuerasJuego() + "', '" + _equipoV.getFuerasJuego() + "', '" +
                            /* PASSES */ idioma[i].Passes + "', '" + _equipoL.getPases() + "', '" + _equipoV.getPases() + "', '" +
                            /* PASSES COMPLETED */ "(" + idioma[i].PassesCompleted + ")" + "', '" + "(" + _equipoL.getPasesCompletados() + ")" + "', '" + "(" + _equipoV.getPasesCompletados() + ")" + "', '" +
                            /* SAVES */ idioma[i].SavesMatch + "', '" + _equipoL.getParadas() + "', '" + _equipoV.getParadas() + "', '" +
                            /* FOULS COMMITTED */ idioma[i].Fouls + "','" + _equipoL.getFaltasCometidas() + "', '" + _equipoV.getFaltasCometidas() + "', '" +
                            /* FOULS RECEIVED */ idioma[i].FoulsR + "','" + _equipoL.getFaltasRecibidas() + "', '" + _equipoV.getFaltasRecibidas() + "', '" +
                            /* YELLOW CARDS */ idioma[i].YellowCards + "','" + _equipoL.getTAmarillas() + "', '" + _equipoV.getTAmarillas() + "', '" +
                            /* RED CARDS */ idioma[i].RedCards + "','" + _equipoL.getTRojas() + "', '" + _equipoV.getTRojas() + "', '" + parte + "'])");
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("StatisticsTableOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
                return "Tabla Estadísticas";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
