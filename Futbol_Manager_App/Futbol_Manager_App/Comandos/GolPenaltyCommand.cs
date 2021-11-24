using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class GolPenaltyCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        private bool _visible;


        public GolPenaltyCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;

            Reset();
        }
        public GolPenaltyCommand(Momento tiempo, Equipo equipo)
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
            _equipo.GolesPenalty.Add(Momento);
            _equipo.Tirosapuerta.Add(Momento);

            if (_jugador != null)
            {
                _jugador.GolesPenalty.Add(Momento);
                _jugador.Tirosapuerta.Add(Momento);
            }
        }

        public void Undo()
        {
            _equipo.GolesPenalty.Remove(Momento);
            _equipo.Tirosapuerta.Remove(Momento);

            if (_jugador != null)
            {
                _jugador.GolesPenalty.Remove(Momento);
                _jugador.Tirosapuerta.Remove(Momento);
            }
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {           
            if (!_visible)
            {               
                for (int i = 0; i < n; i++)
                {
                    string s = "PlayerGolIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', ";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\")+ "'])";
                    else
                        s += "'', '', '', '', ''])";

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
            string s = Momento + " Gol (Penalty)\n";

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

    }
}
