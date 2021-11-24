using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    public class StatisticsCommand : ICommandShowable
    {
        public const int TirosAPuerta = 6;
        public const int TirosAPuertaDentro = 61;
        public const int TirosAPuertaComp = 63;
        public const int Tiros = 601;
        public const int TirosPenalti = 602;
        public const int TirosContraataque = 603;
        public const int Paradas = 7;
        public const int Perdidas = 150;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int TAzules = 10;
        public const int Goles = 711;
        public const int Goles_field = 712;
        public const int Goles_seven = 713;
        public const int Attacks = 714;
        public const int Fieldthrows = 715;
        public const int Fastbreaks = 716;
        public const int Sevenmthrows = 717;
        public const int Turnovers = 718;
        public const int Exclusions = 722;

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
        public StatisticsCommand(string stat, string localValue, string awayValue, string localName, string awayName)
        {
            _stat = -1;
            _statName = stat;
            _localName = localName;
            _awayName = awayName;
            _localValue = localValue;
            _awayValue = awayValue;

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
                if (_stat == -1)
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("StatisticsIN(['" + _statName.Replace("'", "\\'") + "', '" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + _localValue + "','" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" + _awayValue + "'])");
                    }
                }   
                else
                {
                    for (int i = 0; i < n; i++)
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia("StatisticsIN(['" + statName(_stat, idioma[i]) + "', '" + _equipoL.FullName.Replace("'", "\\'") + "', '" + _equipoL.ShortName.Replace("'", "\\'") + "', '" + _equipoL.TeamCode.Replace("'", "\\'") + "', '" + getStat(_stat, _equipoL) + "','" + _equipoV.FullName.Replace("'", "\\'") + "', '" + _equipoV.ShortName.Replace("'", "\\'") + "', '" + _equipoV.TeamCode.Replace("'", "\\'") + "', '" + getStat(_stat, _equipoV) + "'])");
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

        public Jugador GetJugador()
        {
            return null;
        }

        public string getNameCommand()
        {
            return "StatisticsCommand";
        }

        private string CadenaPorcentajes(int a, int b)
        {
            int porcento;
            //NO SE PUEDE DIVIDIR POR CERO
            if (b == 0)
            {
                porcento = 0;
            }
            else
            {
                porcento = 100 * a / b;
            }

            string s = a.ToString() + "/" + b.ToString() + "\\n" + porcento.ToString() + "%";
            return s;
        }
        
        private string getStat(int stat, Equipo equipo)
        {
            switch (stat)
            {
                case Goles:
                    return equipo.getGoles().ToString();

                case Goles_field:
                    return equipo.getGolesField().ToString();

                case Goles_seven:
                    return equipo.getGolesSeven().ToString();

                case Attacks:
                    return equipo.getAtaques().ToString();

                case Fieldthrows:
                    return equipo.getFieldthrowsTotal().ToString();

                case Fastbreaks:
                    return equipo.getFastbreaksTotal().ToString();

                case Sevenmthrows:
                    return equipo.getSevenMThrowsTotal().ToString();

                case Turnovers:
                    return equipo.getTurnoversTotal().ToString();

                case Exclusions:
                    return (equipo.getExclusiones_Totales()*2).ToString();

                case Tiros:
                    return CadenaPorcentajes(equipo.getGoles(), equipo.getTiros());

                case TirosPenalti:
                    return CadenaPorcentajes(equipo.getGoles7M(), equipo.getTiros7M());

                case TirosContraataque:
                    return CadenaPorcentajes(equipo.getGolesContraataque(), equipo.getTirosContraataque());

                case TirosAPuerta:
                    return equipo.getTirosCompuesto().ToString();

                case TirosAPuertaDentro:
                    return equipo.getTirosAPuerta().ToString();

                case TirosAPuertaComp:
                    return equipo.getTirosAPuerta().ToString() + "/" + equipo.getTirosCompuesto().ToString();

                case Paradas:
                    return equipo.getParadasTiro().ToString();

                case Perdidas:
                    return equipo.getPerdidasTiro().ToString();

                case TAmarillas:
                    return equipo.getTAmarillas().ToString();

                case TRojas:
                    return equipo.getTRojas().ToString();

                case TAzules:
                    return equipo.getTAzules().ToString();

                default:
                    return "0";
            }
        }

        private string statName(int stat, IdiomaData idioma)
        {
            switch (stat)
            {
                case Goles:
                    return idioma.Goals;

                case Goles_field:
                    return idioma.Goals;

                case Goles_seven:
                    return idioma.Goal;

                case Attacks:
                    return idioma.Attacks;

                case Fieldthrows:
                    return idioma.Goal;

                case Fastbreaks:
                    return idioma.Goal;

                case Sevenmthrows:
                    return idioma.Goal;
           
                case Exclusions:
                    return idioma.ExclusionsMins;

                case Tiros:
                    return idioma.Tiros;

                case TirosPenalti:
                    return idioma.Tiros_7M;

                case TirosContraataque:
                    return idioma.Tiros_Contraataque;

                case TirosAPuerta:
                    return idioma.Kicks;

                case TirosAPuertaDentro:
                    return idioma.KicksintoGoal;

                case TirosAPuertaComp:
                    return idioma.Kicks;

                case Paradas:
                    return idioma.GoalkeeperSaves;

                case Perdidas:
                    return idioma.Perdidas;

                case TAmarillas:
                    return idioma.YellowCards;

                case TRojas:
                    return idioma.RedCards;

                case TAzules:
                    return idioma.BlueCards;

                default:
                    return "";
            }
        }

        private string statName(int stat)
        {
            switch (stat)
            {
                case Goles:
                    return "Goles";

                case Goles_field:
                    return "Goles normales";

                case Goles_seven:
                    return "Goles de siete metros";

                case Attacks:
                    return "Ataques";

                case Fieldthrows:
                    return "Tiros a puerta";

                case Fastbreaks:
                    return "Contraataques";

                case Sevenmthrows:
                    return "Lanzamientos de siete metros";

                case Turnovers:
                    return "Cesión de balón";

                case Exclusions:
                    return "Exclusiones";

                case Tiros:
                    return "Tiros";

                case TirosPenalti:
                    return "Tiros 7 Metros";

                case TirosContraataque:
                    return "Tiros de Cnt.Atq.";

                case TirosAPuerta:
                    return "Tiros a puerta";

                case TirosAPuertaDentro:
                    return "Tiros entre palos";

                case TirosAPuertaComp:
                    return "Tiros a puerta";

                case Paradas:
                    return "Paradas";

                case Perdidas:
                    return "Perdidas";

                case TAmarillas:
                    return "T. Amarillas";

                case TRojas:
                    return "T. Rojas";

                case TAzules:
                    return "T. Azules";

                default:
                    return "";
            }
        }
    }
}
