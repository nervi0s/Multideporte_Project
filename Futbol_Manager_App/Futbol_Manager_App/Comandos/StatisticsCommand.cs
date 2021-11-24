using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class StatisticsCommand : ICommandShowable
    {
        public const int Corners = 3;
        public const int FaltasRecibidas = 4;
        public const int FaltasCometidas = 41;
        public const int TirosAPuerta = 6;
        public const int TirosAPuertaDentro = 61;
        public const int TirosAPuertaComp = 63;
        public const int Paradas = 7;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int FuerasDeJuego = 10;
        public const int Cambios = 11;
        public const int PasesCompletados = 70;
        public const int PasesTotales = 71;
        
        private int _stat;
        private Equipo _equipoL;
        private Equipo _equipoV;

        private string _statName;
        private string _localValue;
        private string _awayValue;
        private string _localName;
        private string _awayName;

        private bool _visible;


        public StatisticsCommand(int stat, Equipo equipoL, Equipo equipoV)
        {
            _stat = stat;
            _equipoL = equipoL;
            _equipoV = equipoV;

            Reset();
        }
        public StatisticsCommand(string stat, string localValue, string awayValue, Equipo local, Equipo away)
        {
            _stat = -1;
            _statName = stat;
            _equipoL = local;
            _equipoV = away;
            _localValue = localValue;
            _awayValue = awayValue;

            Reset();
        }
        //public StatisticsCommand(string stat, string localValue, string awayValue, string localName, string awayName)
        //{
        //    _stat = -1;
        //    _statName = stat;
        //    _localName = localName;
        //    _awayName = awayName;
        //    _localValue = localValue;
        //    _awayValue = awayValue;

        //    Reset();
        //}

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {            
            if (!_visible)
            {
                if (_stat == -1)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("StatisticsIN(['" + _statName + "', '" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + _localValue + "', '" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" + _awayValue + "'])");
                    }
                }   
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("StatisticsIN(['" + statName(_stat, idioma[i]) + "', '" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + getStat(_stat, _equipoL) + "', '" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" + getStat(_stat, _equipoV) + "'])");
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("StatisticsOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            if (_stat == -1)
            {
                return "Estadísticas: " + _statName + " \n " + _localValue + " - " + _awayValue;
            }
            else
            {
                return "Estadísticas: " + statName(_stat) + " \n " + getStat(_stat, _equipoL) + " - " + getStat(_stat, _equipoV);
            }
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
               
        private string getStat(int stat, Equipo equipo)
        {
            switch (stat)
            {
                case PasesCompletados:
                    return equipo.getPasesCompletados().ToString();

                case PasesTotales:
                    return equipo.getPases().ToString();

                case Corners:
                    return equipo.getCorners().ToString();

                case FaltasRecibidas:
                    return equipo.getFaltasRecibidas().ToString();

                case FaltasCometidas:
                    return equipo.getFaltasCometidas().ToString();

                case TirosAPuerta:
                    return equipo.getTirosCompuesto().ToString();

                case TirosAPuertaDentro:
                    return equipo.getTirosAPuerta().ToString();

                case TirosAPuertaComp:
                    return equipo.getTirosAPuerta().ToString() + "/" + equipo.getTirosCompuesto().ToString();

                case Paradas:
                    return equipo.getParadas().ToString();

                case TAmarillas:
                    return equipo.getTAmarillas().ToString();

                case TRojas:
                    return equipo.getTRojas().ToString();

                case FuerasDeJuego:
                    return equipo.getFuerasJuego().ToString();

                case Cambios:
                    return equipo.getCambios().ToString();

                default:
                    return "0";
            }
        }

        private string statName(int stat, IdiomaData idioma)
        {
            switch (stat)
            {
                case PasesCompletados:
                    return idioma.PassesCompletedFull;

                case PasesTotales:
                    return idioma.Passes;

                case Corners:
                    return idioma.Corners;

                case FaltasRecibidas:
                    return idioma.FoulsR;

                case FaltasCometidas:
                    return idioma.Fouls;

                case TirosAPuerta:
                    return idioma.Kicks;

                case TirosAPuertaDentro:
                    return idioma.KicksintoGoal;

                case TirosAPuertaComp:
                    return idioma.Kicks;

                case Paradas:
                    return idioma.GoalkeeperSaves;

                case TAmarillas:
                    return idioma.YellowCards;

                case TRojas:
                    return idioma.RedCards;

                case FuerasDeJuego:
                    return idioma.Offsides;

                case Cambios:
                    return idioma.Change;

                default:
                    return "";
            }
        }

        private string statName(int stat)
        {
            switch (stat)
            {
                case PasesCompletados:
                    return "Pases Completados";

                case PasesTotales:
                    return "Pases Totales";

                case Corners:
                    return "Corners";

                case FaltasRecibidas:
                    return "Faltas Recibidas";

                case FaltasCometidas:
                    return "Faltas Cometidas";

                case TirosAPuerta:
                    return "Tiros a puerta";

                case TirosAPuertaDentro:
                    return "Tiros entre palos";

                case TirosAPuertaComp:
                    return "Tiros a puerta";

                case Paradas:
                    return "Paradas";

                case TAmarillas:
                    return "T. Amarillas";

                case TRojas:
                    return "T. Rojas";

                case FuerasDeJuego:
                    return "Fueras de Juego";

                case Cambios:
                    return "Cambios";

                default:
                    return "";
            }
        }
    }
}
