using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
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
        private Posesion _posesion;
        
        private bool _visible;


        public StatisticsTableCommand(Momento tiempo,Equipo equipoL, Equipo equipoV, Posesion posesion)
        {
            _equipoL = equipoL;
            _equipoV = equipoV;
            _posesion = posesion;
            this._tiempo = tiempo;
            Reset();
        }
        
        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
            int momento = _tiempo.getParte();

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("StatisticsTableIN(['" +
                            idioma[i].Possesion + "','" + _posesion.getPorcentajeLocal() + "', '" + _posesion.getPorcentajeVisitante() + "', '" +
                            idioma[i].Kicks + "','" + _equipoL.getTirosCompuesto() + "', '" + _equipoV.getTirosCompuesto() + "', '" +
                            idioma[i].KicksintoGoal + "','" + _equipoL.getTirosAPuerta() + "', '" + _equipoV.getTirosAPuerta() + "', '" +
                            idioma[i].GoalkeeperSaves + "','" + _equipoL.getParadas() + "', '" + _equipoV.getParadas() + "', '" +
                            idioma[i].Fouls + "','" + _equipoL.FaltasAcumuladas + "', '" + _equipoV.FaltasAcumuladas + "', '" +
                            idioma[i].Corners + "','" + _equipoL.getCorners() + "', '" + _equipoV.getCorners() + "', '" +
                            //idioma[i].Possesion + "','" + _posesion.getPorcentajeLocal() + "', '" + _posesion.getPorcentajeVisitante() + "', '" +
                            idioma[i].YellowCards + "','" + _equipoL.getTAmarillas() + "', '" + _equipoV.getTAmarillas() + "', '" +
                            idioma[i].RedCards + "','" + _equipoL.getTRojas() + "', '" + _equipoV.getTRojas() + "'" + "])");

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
