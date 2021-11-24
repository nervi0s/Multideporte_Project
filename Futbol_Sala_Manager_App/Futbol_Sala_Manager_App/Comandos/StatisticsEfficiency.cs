using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class StatisticsEfficiencyCommand : ICommandShowable
    {
    //    public const int Corners = 3;
    //    public const int Faltas = 4;
    //    public const int TirosAPuerta = 6;
        //public const int TirosAPuertaDentro = 61;
        //public const int TirosAPuertaComp = 63;
        //public const int TirosAPuertaFuera = 62;
        //public const int Paradas = 7;
        //public const int TAmarillas = 8;
        //public const int TRojas = 9;
        //public const int FuerasDeJuego = 10;
        //public const int Cambios = 11;
        public const int Goles = 5;

        private int _stat;
        private Equipo _equipoL;
        private Equipo _equipoV;

        private string _statName;
        private string _localValue;
        private string _awayValue;

        private bool _visible;


        public StatisticsEfficiencyCommand(int stat, Equipo equipoL, Equipo equipoV)
        {
            _stat = stat;
            _equipoL = equipoL;
            _equipoV = equipoV;

            Reset();
        }
        public StatisticsEfficiencyCommand(string stat, string localValue, string awayValue, Equipo local, Equipo away)
        {
            _stat = -1;
            _statName = stat;
            _equipoL = local;
            _equipoV = away;
            _localValue = localValue;
            _awayValue = awayValue;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                if (_stat == -1)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("StatisticsEfficiencyIN(['" + _statName + "', '" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + _localValue + "', '" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" + _awayValue + "'])");
                    }
                }
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("StatisticsEfficiencyIN(['" + statName(_stat, idioma[i]) + "', '" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + getStat(_stat, _equipoL) + "', '" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" + getStat(_stat, _equipoV) + "'])");
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("StatisticsEfficiencyOUT()");
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
                //case Corners:
                //    return equipo.getCorners().ToString();

                //case Faltas:
                //    return equipo.getFaltasAcumuladas().ToString();
               
                //case TirosAPuerta:
                //    return equipo.getTirosCompuesto().ToString();

                //case TirosAPuertaDentro:
                //    return equipo.getTirosAPuerta().ToString();

                //case TirosAPuertaFuera:
                //    return equipo.getTirosFuera().ToString();

                //case TirosAPuertaComp:
                //    return equipo.getTirosAPuerta().ToString() + "/" + equipo.getTirosCompuesto().ToString();

                //case Paradas:
                //    return equipo.getParadas().ToString();

                //case TAmarillas:
                //    return equipo.getTAmarillas().ToString();

                //case TRojas:
                //    return equipo.getTRojas().ToString();

                //case FuerasDeJuego:
                //    return equipo.getFuerasJuego().ToString();

                //case Cambios:
                //    return equipo.getCambios().ToString();

                case Goles:
                    return equipo.getGoles().ToString() + "/" + (equipo.getTirosAPuerta() + equipo.getTirosFuera()) + "   " + getEfectividadGoles(equipo.getGoles(), (equipo.getTirosAPuerta() + equipo.getTirosFuera()));
                default:
                    return "0";
            }
        }

        private string getEfectividadGoles(double goles, double tirosTotales)
        {
            if (tirosTotales == 0)
            {
                return "0%";
            }
            return (goles / tirosTotales).ToString("0%");
        }

        private string statName(int stat, IdiomaData idioma)
        {
            switch (stat)
            {
                //case Corners:
                //    return idioma.Corners;

                //case Faltas:
                //    return idioma.Fouls;

                //case TirosAPuerta:
                //    return idioma.Kicks;

                //case TirosAPuertaDentro:
                //    return idioma.KicksintoGoal;

                //case TirosAPuertaFuera:
                //    return idioma.KicksOut;

                //case TirosAPuertaComp:
                //    return idioma.Kicks;

                //case Paradas:
                //    return idioma.GoalkeeperSaves;

                //case TAmarillas:
                //    return idioma.YellowCards;

                case Goles:
                    return idioma.GolesTirosEficiencia;

                default:
                    return "";
            }
        }
        private string statName(int stat)
        {
            switch (stat)
            {
                //case Corners:
                //    return "Corners";

                //case Faltas:
                //    return "Faltas";

                //case TirosAPuerta:
                //    return "Tiros a puerta";

                //case TirosAPuertaDentro:
                //    return "Tiros entre palos";

                //case TirosAPuertaFuera:
                //    return "Tiros a fuera";

                //case TirosAPuertaComp:
                //    return "Tiros a puerta";

                //case Paradas:
                //    return "Paradas";

                //case TAmarillas:
                //    return "T. Amarillas";

                //case TRojas:
                //    return "T. Rojas";

                case Goles:
                    return "Goles";
                default:
                    return "";
            }
        }

    }
}

