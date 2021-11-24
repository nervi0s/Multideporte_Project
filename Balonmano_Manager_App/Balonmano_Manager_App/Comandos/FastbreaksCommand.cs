using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    class FastbreaksCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        private bool _visible;

        #region Constructor
        
        public FastbreaksCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public FastbreaksCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }


        #endregion

        public string getNameCommand()
        {
            return "FastbreaksCommand";
        }

        public void Execute()
        {
            _equipo.Fastbreaks.Add(Momento);

            if (_jugador != null)
                _jugador.Fastbreaks.Add(Momento);
        }

        public void Undo()
        {
            _equipo.Fastbreaks.Remove(Momento);

            if (_jugador != null)
                _jugador.Fastbreaks.Remove(Momento);
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] _ipfs, IdiomaData[] _idioma, int n)
        {
            if (!_visible)
            {               
                for (int i = 0; i < n; i++)
                {
                    string s = "TiroIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "',";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, _idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "',";
                    if (Momento != null)
                        s += " '" + Momento + ",";

                    //if (Program.EstaActivado(i))
                    //    _ipfs[i].Envia(s + " '" + _idioma[i].AttemptIn + "'])");

                    Console.WriteLine("Método Show 1ª Rama de FastbreaksCommand: " + s + " '" + _idioma[i].AttemptIn + "'])");

                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        _ipfs[i].Envia("TiroOUT()");

                    Console.WriteLine("Método Show 2ª Rama de FastbreaksCommand: TiroOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        override public string ToString()
        {
            string s = Momento + " Contraataques que han acabado en gol\n";

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
