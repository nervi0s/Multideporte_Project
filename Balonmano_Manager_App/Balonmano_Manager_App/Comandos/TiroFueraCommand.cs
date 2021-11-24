using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class TiroFueraCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;
        private bool _visible;

        public TiroFueraCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public TiroFueraCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Tirosfuera.Add(Momento);

            if (_jugador != null)
                _jugador.Tirosfuera.Add(Momento);
        }

        public string getNameCommand()
        {
            return "TiroFueraCommand";
        }
        public void Undo()
        {
            _equipo.Tirosfuera.Remove(Momento);

            if (_jugador != null)
                _jugador.Tirosfuera.Remove(Momento);
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string s = "TiroIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', ";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + getPosicion(_jugador, idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "', ";
                    if (Momento != null)
                        s += "'" + Momento + ", ";

                    if (Program.EstaActivado(i))
                        ipf[i].Envia(s + " '" + idioma[i].Attempt + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("TiroOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        public void Reset()
        {
            _visible = false;
        }

        override public string ToString()
        {
            string s = Momento + " Tiro fuera\n";

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
