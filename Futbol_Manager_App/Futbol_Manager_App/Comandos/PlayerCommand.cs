using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class PlayerCommand : ICommandShowable
    {
        public enum Command
        {
            None,
            Posicion,
            FaltasRecibidas,
            FaltaRecibida,
            FaltasCometidas,
            FaltaCometida,
            Goles,
            Gol,
            GolesPartidos,
            GolesPP,
            GolPP,
            TirosTotales,
            TiroTotales,
            TirosAPorteria,
            TiroAPorteria,
            Paradas,
            Parada,
            TAmarillas,
            TAmarilla,
            TRoja,
            TRojas,
            FuerasDeJuego,
            FueraDeJuego,
            GolesPorPartido,
            Partidos,
            GolPorPartido,
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
                    string s = "PlayerIN(['" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "'";

                    if (_stat == Command.None)
                    {
                        //s += _mensaje;
                    }
                    else if (_stat == Command.Posicion)
                    {
                        s += ", '" + getPosicion(_jugador, idioma[i]) + "'";
                    }
                    else if (_stat == Command.GolesPP || _stat == Command.GolPP)
                    {
                        s += ", '" + statName(_stat, idioma[i]) + "'";
                    }
                    else if (_stat == Command.GolesPartidos)
                    {
                        bool isGoals = Convert.ToInt32(getStat(_stat, _jugador)) != 1;

                        // Suma los goles en el partido actual y el partido actual en los partidos jugados con los datos de la base de datos
                        if (Program._PrimerComienzo)
                        {
                            bool isMatches = (_jugador.Matches + 1) > 1;
                            s += ", '" + getStat(_stat, _jugador) + " " + statName(isGoals ? Command.GolesPorPartido : Command.GolPorPartido, idioma[i]) + " " + Convert.ToString((Convert.ToInt32(_jugador.Matches) + 1) + " " + statName(isMatches ? Command.Partidos : Command.Partido, idioma[i])) + "'";
                        }
                        else
                        {
                            bool isMatches = _jugador.Matches > 1;
                            s += ", '" + getStat(_stat, _jugador) + " " + statName(isGoals ? Command.GolesPorPartido : Command.GolPorPartido, idioma[i]) + " " + Convert.ToString((Convert.ToInt32(_jugador.Matches) + 1) + " " + statName(isMatches ? Command.Partidos : Command.Partido, idioma[i])) + "'";
                        }
                    }
                    else
                    {
                        s += ", '" + getStat(_stat, _jugador) + " " + statName(_stat, idioma[i]) + "'";
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
                string golesEnTotal ="" + getStat(Command.Goles, _jugador);
                if (Program._PrimerComienzo)
                    return _jugador.ShortName + " \n " + statName(_stat) + "/Partidos: " + golesEnTotal + "/" + (Convert.ToInt32(getStat(Command.Partidos, _jugador))+1);
                else
                    return _jugador.ShortName + " \n " + statName(_stat) + "/Partidos: " + golesEnTotal + "/" + (Convert.ToInt32(getStat(Command.Partidos, _jugador)) );
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

        private string getStat(Command stat, Jugador jugador)
        {
            switch (stat)
            {
                case Command.FaltasRecibidas:
                case Command.FaltaRecibida:
                    return jugador.FaltasRecibidas.Count.ToString();

                case Command.FaltasCometidas:
                case Command.FaltaCometida:
                    return jugador.FaltasCometidas.Count.ToString();

                case Command.Goles:
                case Command.Gol:
                    return Convert.ToString(jugador.Goles.Count + jugador.GolesPenalty.Count);

                case Command.GolesPartidos:
                    return Convert.ToString(jugador.Goles.Count + jugador.GolesPenalty.Count + jugador.Goals);

                case Command.GolesPP:
                case Command.GolPP:
                    return Convert.ToString(jugador.GolesPP.Count);

                case Command.TirosTotales:
                case Command.TiroTotales:
                    //return jugador.Tirosapuerta.Count.ToString() + "/" + (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count).ToString();
                    return (jugador.Tirosapuerta.Count + jugador.Tirosfuera.Count).ToString();

                case Command.TiroAPorteria:
                case Command.TirosAPorteria:
                    return jugador.Tirosapuerta.Count.ToString();

                case Command.Paradas:
                case Command.Parada:
                    return jugador.Paradas.Count.ToString();

                case Command.TAmarillas:
                case Command.TAmarilla:
                    return jugador.TAmarillas.Count.ToString();

                case Command.TRojas:
                case Command.TRoja:
                    return jugador.TRojas.Count.ToString();

                case Command.FuerasDeJuego:
                case Command.FueraDeJuego:
                    return jugador.Fuerasdejuego.Count.ToString();

                case Command.Partidos:
                case Command.Partido:
                    return Convert.ToString((Convert.ToInt32(_jugador.Matches)));

                default:
                    return "0";
            }
        }
        
        private string statName(Command stat, IdiomaData idioma)
        {
            switch (stat)
            {
                case Command.FaltasRecibidas:
                    return idioma.FoulsR;
                case Command.FaltaRecibida:
                    return idioma.FoulR;
                case Command.FaltasCometidas:
                    return idioma.Fouls;
                case Command.FaltaCometida:
                    return idioma.Foul;
                case Command.Goles:
                case Command.GolesPartidos:
                    return idioma.GoalsMatch;
                case Command.Gol:
                    return idioma.GoalMatch;
                case Command.GolesPP:
                    return idioma.GoalPP;
                case Command.GolPP:
                    return idioma.GoalPP;
                case Command.TirosTotales:
                    return idioma.TotalKicks;
                case Command.TiroTotales:
                    return idioma.TotalKick;
                case Command.TiroAPorteria:
                    return idioma.KickIn;
                case Command.TirosAPorteria:
                    return idioma.KicksIn;
                case Command.Paradas:
                    return idioma.GoalkeeperSaves;
                case Command.Parada:
                    return idioma.GoalkeeperSave;
                case Command.TAmarillas:
                    return idioma.YellowCardsMatch;
                case Command.TAmarilla:
                    return idioma.YellowCardMatch;
                case Command.TRoja:
                    return idioma.RedCardMatch;
                case Command.TRojas:
                    return idioma.RedCardsMatch;
                case Command.FuerasDeJuego:
                    return idioma.Offsides;
                case Command.FueraDeJuego:
                    return idioma.Offside;
                case Command.GolesPorPartido:
                    return idioma.GoalsPerMatch;
                case Command.Partidos:
                    return idioma.Matches;
                case Command.GolPorPartido:
                    return idioma.GoalPerMatch;
                case Command.Partido:
                    return idioma.Match;
                default:
                    return "";
            }
        }        

        private string statName(Command stat)
        {
            switch (stat)
            {
                case Command.FaltaRecibida:
                case Command.FaltasRecibidas:
                    return "Faltas Recibidas";

                case Command.FaltaCometida:
                case Command.FaltasCometidas:
                    return "Faltas Cometidas";

                case Command.Gol:
                case Command.Goles:
                case Command.GolesPartidos:
                    return "Goles";

                case Command.GolesPP:
                case Command.GolPP:
                    return "Goles (PP)";

                case Command.TiroTotales:
                case Command.TirosTotales:
                    return "Tiros totales";

                case Command.TiroAPorteria:
                case Command.TirosAPorteria:
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

                case Command.FuerasDeJuego:
                case Command.FueraDeJuego:
                    return "Fueras de juego";

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

                case Jugador.Defensa:
                    return idioma.Defender;

                case Jugador.Centrocampista:
                    return idioma.Midfielder;

                default:
                    return idioma.Forward;
            }
        }

        private string getPosicion(Jugador jugador)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return "Portero";

                case Jugador.Defensa:
                    return "Defensa";

                case Jugador.Centrocampista:
                    return "Centrocampista";

                default:
                    return "Delantero";
            }
        }
    }
}
