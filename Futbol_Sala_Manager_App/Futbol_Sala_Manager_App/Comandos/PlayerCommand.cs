using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class PlayerCommand : ICommandShowable
    {
        public enum Command
        {
            None,
            Posicion,
            Falta,
            Faltas,
            Goles,
            Gol,
            GolesPartidos,
            GolesTiros,
            GolesTirosEficiencia,
            TirosAPuerta,
            TiroAPuerta,
            TirosAPuertaSolo,
            TirosTotales,
            Paradas,
            Parada,
            TAmarillas,
            TAmarilla,
            TRoja,
            TRojas,
            GolPorPartido,
            GolesPorPartido,
            Partidos,
            Partido
        }

        private Command _stat;
        private Jugador _jugador;
        private string _mensaje;
        private bool _visible;


        public PlayerCommand(Jugador jugador, Command stat)
        {
            _stat = stat;
            _jugador = jugador;

            Reset();
        }
        /*
        public PlayerCommand(Jugador jugador, int stat, int partidos)
        {
            _stat = stat;
            _partidos = partidos;
            _jugador = jugador;

            Reset();
        }*/

        public PlayerCommand(Jugador jugador, string mensaje)
        {
            _stat = Command.None;
            _jugador = jugador;
            _mensaje = mensaje;

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
                for (int i = 0; i < n; i++)
                {
                    string s = "PlayerIN(['" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "'";

                    if (_stat == Command.None)
                    {
                        //s += _mensaje;
                    }
                    else if (_stat == Command.Posicion)
                    {
                        s += ", '" + getPosicion(_jugador, idioma[i]) + "'";
                    }
                    else if (_stat == Command.GolesPartidos)
                    {
                        bool isGoals = Convert.ToInt32(getStat(_stat, _jugador)) != 1;

                        // Suma los goles en el partido actual y el partido actual en los partidos jugados con los datos de la base de datos
                        if (Program._PrimerComienzo)
                        {
                            bool isMatches = (_jugador.Matches + 1) > 1;
                            s += ", '" + getPosicion(_jugador, idioma[i]) + "', '" + statName(isGoals ? Command.GolesPorPartido : Command.GolPorPartido, idioma[i]).Replace(":", "") + "/" + (statName(isMatches ? Command.Partidos : Command.Partido, idioma[i])) + " " + getStat(_stat, _jugador) + "/" + Convert.ToString((Convert.ToInt32(_jugador.Matches) + 1) + "'");

                        }
                        else
                        {
                            bool isMatches = _jugador.Matches > 1;
                            s += ", '" + getPosicion(_jugador, idioma[i]) + "', '" + statName(isGoals ? Command.GolesPorPartido : Command.GolPorPartido, idioma[i]).Replace(":", "") + "/" + (statName(isMatches ? Command.Partidos : Command.Partido, idioma[i])) + " " + getStat(_stat, _jugador) + "/" + Convert.ToString((Convert.ToInt32(_jugador.Matches) + 1) + "'");
                        }
                    }
                    else
                    {
                        s += ", '" + getPosicion(_jugador, idioma[i]) + "', '" + statName(_stat, idioma[i]) + " " + getStat(_stat, _jugador) + "'";
                    }

                    s += "])";

                    //Console.WriteLine(s);

                    if (Program.EstaActivado(i))
                        ipf[i].Envia(s);

                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("PlayerOUT()");
                }

                _visible = false;
            }

            return _visible;
        }

        override public string ToString()
        {
            if (_stat == Command.None)
            {
                return _jugador.ShortName + "\n" + _mensaje;
            }
            else if (_stat == Command.Posicion)
            {
                return _jugador.ShortName + "\n" + getPosicion(_jugador);
            }
            else if (_stat == Command.GolesPartidos)
            {
                string golesEnTotal = "" + getStat(Command.Goles, _jugador);
                if (Program._PrimerComienzo)
                    return _jugador.ShortName + " \n " + statName(_stat) + "/Partidos: " + golesEnTotal + "/" + (Convert.ToInt32(getStat(Command.Partidos, _jugador)) + 1);
                else
                    return _jugador.ShortName + " \n " + statName(_stat) + "/Partidos: " + golesEnTotal + "/" + (Convert.ToInt32(getStat(Command.Partidos, _jugador)));
            }
            else
            {
                return _jugador.ShortName + " \n " + statName(_stat) + ": " + getStat(_stat, _jugador);
            }
        }

        public Color GetColor()
        {
            return _jugador.Equipo.Color1;
        }

        private string getEfectividadGoles(double goles, double tirosTotales)
        {
            if (tirosTotales == 0)
            {
                return "0%";
            }
            return (goles / tirosTotales).ToString("0%");
        }

        private string getStat(Command stat, Jugador jugador)
        {
            switch (stat)
            {
                case Command.Faltas:
                case Command.Falta:
                    return jugador.Faltas.Count.ToString();

                case Command.Goles:
                case Command.Gol:
                    return Convert.ToString(jugador.Goles.Count + jugador.GolesPenalty.Count);

                case Command.GolesPartidos:
                    return Convert.ToString(jugador.Goles.Count + jugador.GolesPenalty.Count + jugador.Goals);

                case Command.TirosAPuerta:
                case Command.TiroAPuerta:
                    return jugador.Tirosapuerta.Count.ToString() + "/" + (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count).ToString();

                case Command.Paradas:
                case Command.Parada:
                    return jugador.Paradas.Count.ToString();

                case Command.TAmarillas:
                case Command.TAmarilla:
                    return jugador.TAmarillas.Count.ToString();

                case Command.TRojas:
                case Command.TRoja:
                    return jugador.TRojas.Count.ToString();

                case Command.Partidos:
                case Command.Partido:
                    return Convert.ToString((Convert.ToInt32(_jugador.Matches)));
                case Command.GolesTiros:
                    return jugador.Goles.Count + "/" + (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count).ToString();
                case Command.GolesTirosEficiencia:
                    return jugador.Goles.Count + "/" + (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count).ToString() + " - " + getEfectividadGoles(jugador.Goles.Count, jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count);
                case Command.TirosAPuertaSolo:
                    return jugador.Tirosapuerta.Count + "";
                case Command.TirosTotales:
                    return (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count) + "";
                default:
                    return "0";
            }
        }

        private string statName(Command stat, IdiomaData idioma)
        {
            switch (stat)
            {
                case Command.Faltas:
                    return idioma.FoulsMatch;
                case Command.Falta:
                    return idioma.FoulMatch;
                case Command.Goles:
                case Command.GolesPartidos:
                    return idioma.GoalsMatch;
                case Command.Gol:
                    return idioma.GoalMatch;
                case Command.TirosAPuerta:
                    return idioma.ShotsMatch;
                case Command.TiroAPuerta:
                    return idioma.ShotMatch;
                case Command.Paradas:
                    return idioma.SaveMatch;
                case Command.Parada:
                    return idioma.SavesMatch;
                case Command.TAmarillas:
                    return idioma.YellowCardsMatch;
                case Command.TAmarilla:
                    return idioma.YellowCardMatch;
                case Command.TRoja:
                    return idioma.RedCardMatch;
                case Command.TRojas:
                    return idioma.RedCardsMatch;
                case Command.GolesPorPartido:
                    return idioma.GoalsPerMatch;
                case Command.Partidos:
                    return idioma.Matches;
                case Command.GolPorPartido:
                    return idioma.GoalPerMatch;
                case Command.Partido:
                    return idioma.Match;
                case Command.GolesTiros:
                    return idioma.GolesTiros;
                case Command.GolesTirosEficiencia:
                    return idioma.GolesTirosEficiencia;
                case Command.TirosAPuertaSolo:
                    return idioma.TirosAPuerta;
                case Command.TirosTotales:
                    return idioma.TirosTotales;
                default:
                    return "";
            }
        }

        private string statName(Command stat)
        {
            switch (stat)
            {
                case Command.Falta:
                case Command.Faltas:
                    return "Faltas";

                case Command.Gol:
                case Command.Goles:
                case Command.GolesPartidos:
                    return "Goles";

                case Command.TiroAPuerta:
                case Command.TirosAPuerta:
                    return "Tiros a puerta";

                case Command.Paradas:
                case Command.Parada:
                    return "Paradas";

                case Command.TAmarillas:
                case Command.TAmarilla:
                    return "T. Amarillas";

                case Command.TRojas:
                case Command.TRoja:
                    return "T. Rojas";
                case Command.GolesTiros:
                    return "Goles/Tiros totales";
                case Command.GolesTirosEficiencia:
                    return "Goles/Tiros totales eficiencia";
                case Command.TirosAPuertaSolo:
                    return "Tiros a puerta";
                case Command.TirosTotales:
                    return "Tiros totales";
                default:
                    return "";
            }
        }

        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;

                case Jugador.Cierre:
                    return idioma.Cierre;

                case Jugador.AlaCierre:
                    return idioma.AlaCierre;

                case Jugador.Ala:
                    return idioma.Ala;

                case Jugador.AlaPivot:
                    return idioma.AlaPivot;

                case Jugador.Pivot:
                    return idioma.Pivot;

                default:
                    return idioma.Universal;
            }
        }

        private string getPosicion(Jugador jugador)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return "Portero";

                case Jugador.Cierre:
                    return "Cierre";

                case Jugador.AlaCierre:
                    return "Ala Cierre";

                case Jugador.Ala:
                    return "Ala";

                case Jugador.AlaPivot:
                    return "Ala Pívot";

                case Jugador.Pivot:
                    return "Pívot";

                default:
                    return "Universal";
            }
        }
    }
}
