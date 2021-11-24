using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
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
        private bool _descanso;
        private string _posesionLocal;
        private string _posesionVisitante;       

        private bool _visible;


        public StatisticsTableCommand(Momento tiempo, bool descanso, Equipo equipoL, Equipo equipoV, string posesionLocal, string posesionVisitante)
        {
            _equipoL = equipoL;
            _equipoV = equipoV;
            _posesionLocal = posesionLocal;
            _posesionVisitante = posesionVisitante;
            this._tiempo = tiempo;
            _descanso = descanso;
            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                    {
                        string parte = _tiempo.GetNombreParte(idioma[i], _descanso);
                        
                        ipf[i].Envia("StatisticsTableIN(['" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" +
                            /* TIROS TOTALES */         idioma[i].Tiros.ToString()              + "', '" + CadenaPorcentajes(_equipoL.getGoles(), _equipoL.getTiros())                                                  + "', '" +  CadenaPorcentajes(_equipoV.getGoles(), _equipoV.getTiros())                         + "', '" +
                            /* TIROS  CAMPO */          idioma[i].Tiros_Campo.ToString()        + "', '" + CadenaPorcentajes(_equipoL.getGoles()- _equipoL.getGoles7M()- _equipoL.getGolesContraataque(), _equipoL.getTiros()- _equipoL.getTiros7M()- _equipoL.getTirosContraataque()) + "', '" + CadenaPorcentajes(_equipoV.getGoles()- _equipoV.getGoles7M()- _equipoV.getGolesContraataque(), _equipoV.getTiros()- _equipoV.getTiros7M()- _equipoV.getTirosContraataque()) + "', '" +
                            /* TIROS 7M */              idioma[i].Tiros_7M.ToString()           + "', '" + CadenaPorcentajes(_equipoL.getGoles7M(), _equipoL.getTiros7M())                                              + "', '" +  CadenaPorcentajes(_equipoV.getGoles7M(), _equipoV.getTiros7M())                      + "', '" +
                            /* TIROS C.ATQ */           idioma[i].Tiros_Contraataque.ToString() + "', '" + CadenaPorcentajes(_equipoL.getGolesContraataque(), _equipoL.getTirosContraataque())                          + "', '" +  CadenaPorcentajes(_equipoV.getGolesContraataque(), _equipoV.getTirosContraataque())  + "', '" +
                            /* PARADAS */               idioma[i].GoalkeeperSaves.ToString()    + "', '" + _equipoL.getParadasTiro().ToString()             + "', '" + _equipoV.getParadasTiro().ToString()             + "', '" +
                            /* PERDIDAS */              idioma[i].Perdidas.ToString()           + "', '" + _equipoL.getPerdidasTiro().ToString()            + "', '" + _equipoV.getPerdidasTiro().ToString()            + "', '" +
                            /* EXCLUSIONES */           idioma[i].ExclusionsMins.ToString()     + "', '" + (_equipoL.getExclusiones_Totales()*2).ToString()     + "', '" + (_equipoV.getExclusiones_Totales()*2).ToString()     + "', '" +
                            /* ATAQUES */               idioma[i].Attacks.ToString()            + "', '" + _equipoL.getAtaques().ToString()                 + "', '" + _equipoV.getAtaques().ToString() + "', '" +
                            ///* POSESION */              idioma[i].Possesion.ToString()          + "', '" + _posesionLocal                                   + "', '" + _posesionVisiante                                + "', '" + 
                                                        parte + "'])");
                        
                        ///* PASSES COMPLETED */ "(" + idioma[i].PassesCompleted + ")" + "', '" + "(" + _equipoL.getPasesCompletados() + ")" + "', '" + "(" + _equipoV.getPasesCompletados() + ")" + "', '" +
                        ///* FOULS COMMITTED */ idioma[i].Fouls + "','" + _equipoL.getFaltasCometidas() + "', '" + _equipoV.getFaltasCometidas() + "', '" +
                        ///* YELLOW CARDS */ idioma[i].YellowCards + "','" + _equipoL.getTAmarillas() + "', '" + _equipoV.getTAmarillas() + "', '" +
                        ///* RED CARDS */ idioma[i].RedCards + "','" + _equipoL.getTRojas() + "', '" + _equipoV.getTRojas() + "', '" +
                        ///* PARADAS */ idioma[i].SavesMatch + "','" + _equipoL.getParadas() + "', '" + _equipoV.getParadas() + "', '" + parte + "'])");
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

        private string CadenaPorcentajes(int a, int b)
        {
            int porcento;
            // NO SE PUEDE DIVIDIR POR CERO
            if (b == 0)
            {
                porcento = 0;
            }
            else
            {
                porcento = 100 * a / b;
            }

            string s = a.ToString() + "/" + b.ToString() + "   " + porcento.ToString() + "%";
            return s;
        }
        
        override public string ToString()
        {
                return "Tabla Estadísticas";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        public string getNameCommand()
        {
            return "StatisticsTableCommand";
        }

    }
}
