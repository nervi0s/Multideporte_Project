using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class GolCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;
        private bool _visible;


        public GolCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;

            Reset();
        }
        public GolCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public void Execute()
        {
            _equipo.Goles.Add(Momento);
            _equipo.Tirosapuerta.Add(Momento);

            if (_jugador != null)
            {
                _jugador.Goles.Add(Momento);
                _jugador.Tirosapuerta.Add(Momento);
            }
        }
        public void Undo()
        {
            _equipo.Goles.Remove(Momento);
            _equipo.Tirosapuerta.Remove(Momento);

            if (_jugador != null)
            {
                _jugador.Goles.Remove(Momento);
                _jugador.Tirosapuerta.Remove(Momento);
            }
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string s = "PlayerGolIN(['" + _jugador.Equipo.FullName.Replace("'", "\\'") + "', '" + _jugador.Equipo.ShortName.Replace("'", "\\'") + "', '" + _jugador.Equipo.TeamCode.Replace("'", "\\'") + "', '" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "', '";
                    s += statName(idioma[i]) + " " + getStat(_jugador);
                    s += "', '" + Momento.GetMinuto() + "'])";

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
                        ipf[i].Envia("PlayerGolOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            string s = Momento + " Gol\n";

            if (_jugador != null)
            {
                s += _jugador.Number + " " + _jugador.ShortName;
            }
            else
            {
                s += _equipo.ShortName;
            }

            return s;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

        private string getStat(Jugador jugador)
        {
            return Convert.ToString(jugador.Goles.Count + jugador.GolesPenalty.Count);
        }

        private string statName(IdiomaData idioma)
        {
            return _jugador.Goles.Count + _jugador.GolesPenalty.Count != 1 ?
                idioma.GoalsMatch : idioma.GoalMatch;
        }

        public Jugador GetJugador()
        {
            return _jugador;
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

    }
}
