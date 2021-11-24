using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
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

        public string getNameCommand()
        {
            return "GolPenaltyCommand";
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

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {           
            if (!_visible)
            {               
                for (int i = 0; i < n; i++)
                {
                    string s = "PlayerIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', ";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "'])";
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
                        ipf[i].Envia("PlayerOUT()");
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

        public Jugador GetJugador()
        {
            return null;
        }

        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;
                case Jugador.Extremo:
                    return idioma.Extremo;
                case Jugador.ExtremoDerecho:
                    return idioma.ExtremoDerecho;
                case Jugador.ExtremoIzquierdo:
                    return idioma.ExtremoIzquierdo;
                case Jugador.Lateral:
                    return idioma.Lateral;
                case Jugador.LateralDerecho:
                    return idioma.LateralDerecho;
                case Jugador.LateralIzquierdo:
                    return idioma.LateralIzquierdo;
                case Jugador.Central:
                    return idioma.Central;
                default:
                    return idioma.Pivote;
            }
        }

    }
}
