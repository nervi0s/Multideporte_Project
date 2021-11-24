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
    class TurnoversCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;
        private bool _visible;


        #region Constructor

        public TurnoversCommand(Momento momento, Jugador jugador)
        {
            Momento = momento;
            _jugador = jugador;
            _equipo = _jugador.Equipo;

            Reset();
        }

        public TurnoversCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
  
            Reset();
        }

        #endregion
        
        public void Execute()
        {
            /*
            _equipo.AttacksComplete.Add(Momento);
            _equipo.Faults.Add(Momento);
            */

            if (_jugador != null)
            {
                _jugador.Turnovers.Add(Momento);
            }
        }

        public string getNameCommand()
        {
            return "TurnoversCommand";
        }

        public void Undo()
        {
            /*
            _equipo.AttacksComplete.Remove(Momento);
            _equipo.Faults.Remove(Momento);
            */

            if (_jugador != null)
            {
                _jugador.Turnovers.Remove(Momento);
            }
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
                    string s = "PlayerIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "',";
                    if (_jugador != null)
                        s += "'" + _jugador.Number + "', '" + _jugador.FullName.Replace("'", "\\'") + "', '" + _jugador.ShortName.Replace("'", "\\'") + "', '" + getPosicion(_jugador, _idioma[i]) + "', '" + _jugador.RutaFoto.Replace(@"\", @"\\") + "'])";
                    else
                        s += "'', '', '', '', ''])";
                    
                    //if (Program.EstaActivado(i))
                    //    _ipfs[i].Envia(s);

                    Console.WriteLine("Método Show 1ª Rama de TurnoversCommand: " + s);
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    //if (Program.EstaActivado(i))
                    //    _ipfs[i].Envia("PlayerOUT()");

                    Console.WriteLine("Método Show 2ª Rama de TurnoversCommand: PlayerOUT()");

                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            string s = Momento + " Cesión de balón\n";

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
