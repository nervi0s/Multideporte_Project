using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    public class EstadisticasCommand : ICommandShowable
    {
        public const int TirosAPuerta = 6;
        public const int TirosAPuertaDentro = 61;
        public const int TirosAPuertaComp = 63;
        public const int posesiones = 64;
        public const int Paradas = 7;
        public const int TAmarillas = 8;
        public const int TRojas = 9;
        public const int TAzules = 10;

        private int _nEstadisticas;
        private int _stat;
        private int _stat2;
        private int _stat3;
        private Equipo _equipoL;
        private Equipo _equipoV;

        private string _statName;
        private string _statName2;
        private string _statName3;
        private string _localValue;
        private string _awayValue;
        private string _localName;
        private string _awayName;

        private bool _visible;
        public EncuentroData _datos;


        public EstadisticasCommand(int stat, int stat2, Equipo equipoL, Equipo equipoV, EncuentroData datos)
        {
            _stat = stat;
            _stat2 = stat2;
            _equipoL = equipoL;
            _equipoV = equipoV;
            _nEstadisticas = 2;
            _datos = datos;

            Reset();
        }

        public EstadisticasCommand(int stat, int stat2,int stat3, Equipo equipoL, Equipo equipoV, EncuentroData datos)
        {
            _stat = stat;
            _stat2 = stat2;
            _stat3 = stat3;
            _equipoL = equipoL;
            _equipoV = equipoV;
            _nEstadisticas = 3;

            _datos = datos;

            Reset();
        }

        public string getNameCommand()
        {
            return "EstadisticasCommand";
        }

        public void Reset()
        {
            _visible = false;
        }

        private bool AttemptsAndOnTarget()
        {
            bool estaApuerta = ((_stat == 6) || (_stat2 == 6) || (_stat3 == 6));
            bool estaDentro = ((_stat == 61) || (_stat2 == 61) || (_stat3 == 61));
            return estaApuerta && estaDentro;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {

                    if (Program.EstaActivado(i))
                        if (_nEstadisticas == 2)
                        {
                            if (AttemptsAndOnTarget())
                            {
                                ipf[i].Envia("Statistics2linesIN(['" + statName(_stat, idioma[i]) + "', '" +
                                getStat(_stat, _equipoL) + "','" + getStat(_stat, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']"
                                + ","
                                + "['" + statName(_stat2, idioma[i]) + "', '" + getStat(_stat2, _equipoL) + "','" + getStat(_stat2, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']" + ")");
                            }
                            else
                            {
                                ipf[i].Envia("Statistics2linesIN(['" + statName(_stat, idioma[i]) + "', '" +
                                getStat(_stat, _equipoL) + "','" + getStat(_stat, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']"
                                + ","
                                + "['" + statName(_stat2, idioma[i]) + "', '" + getStat(_stat2, _equipoL) + "','" + getStat(_stat2, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']" + ")");

                            }
                        }
                        else
                        {
                            if (AttemptsAndOnTarget())
                            {
                                ipf[i].Envia("Statistics3linesIN(['" + statName(_stat, idioma[i]) + "', '" +
                                getStat(_stat, _equipoL) + "','" + getStat(_stat, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']"
                                + ","
                                + "['" + statName(_stat2, idioma[i]) + "', '" + getStat(_stat2, _equipoL) + "','" + getStat(_stat2, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']" +
                                ","
                                + "['" + statName(_stat3, idioma[i]) + "', '" + getStat(_stat3, _equipoL) + "','" + getStat(_stat3, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']" +
                                ")");
                            }
                            else
                            {
                                ipf[i].Envia("Statistics3linesIN(['" + statName(_stat, idioma[i]) + "', '" +
                                getStat(_stat, _equipoL) + "','" + getStat(_stat, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']"
                                + ","
                                + "['" + statName(_stat2, idioma[i]) + "', '" + getStat(_stat2, _equipoL) + "','" + getStat(_stat2, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']" +
                                ","
                                + "['" + statName(_stat3, idioma[i]) + "', '" + getStat(_stat3, _equipoL) + "','" + getStat(_stat3, _equipoV) + "', '" + _equipoL.TeamCode + "','" + _equipoV.TeamCode + "']" +
                                ")");
                            }
                        }
                }

                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        if (_nEstadisticas == 2)
                            ipf[i].Envia("Statistics2linesOUT()");
                        else
                            ipf[i].Envia("Statistics3linesOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            //string s = "" + statName(_stat2) + "  " + getStat(_stat2, _equipoL) +
            //                                                " - " + getStat(_stat2, _equipoV) + "\n" + statName(_stat) + "  " + getStat(_stat, _equipoL) +
            //                                                " - " + getStat(_stat, _equipoV);
            //System.Console.WriteLine(s);
            //System.Console.WriteLine(_stat2);
            //System.Console.WriteLine(_stat);
            if (_nEstadisticas == 2)
                return "" + statName(_stat2) + "  " + getStat(_stat2, _equipoL) +
                                                            " - " + getStat(_stat2, _equipoV) + "\n" + statName(_stat) + "  " + getStat(_stat, _equipoL) +
                                                            " - " + getStat(_stat, _equipoV);
            else
                return "" + statName(_stat3) + "  " + getStat(_stat3, _equipoL) +
                                            " - " + getStat(_stat3, _equipoV) + "\n" + statName(_stat2) + "  " + getStat(_stat2, _equipoL) +
                                            " - " + getStat(_stat2, _equipoV) + "\n" + statName(_stat) + "  " + getStat(_stat, _equipoL) +
                                            " - " + getStat(_stat, _equipoV);
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        private string getStat(int stat, Equipo equipo)
        {
            switch (stat)
            {
                case posesiones:
                    if (equipo.Local) 
                        return _datos.Posesion.getPorcentajeLocal();
                    else
                        return _datos.Posesion.getPorcentajeVisitante();
              
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
                case posesiones:
                    return idioma.Possesion;

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
                case posesiones:
                    return "Posesion";

                case TirosAPuerta:
                    return "T.Puerta";

                case TirosAPuertaDentro:
                    return "T.Dentro";

                case TirosAPuertaComp:
                    return "T.Puerta";

                case Paradas:
                    return "Paradas";

                case TAmarillas:
                    return "T.Amarillas";

                case TRojas:
                    return "T.Rojas";

                case TAzules:
                    return "T.Azules";

                default:
                    return "";
            }
        }

        public Jugador GetJugador()
        {
            return null;
        }

    }
}
