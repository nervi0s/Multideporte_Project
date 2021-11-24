using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class TiroCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;
        private bool _visible;
        private int tipo_tiro;

        //CONSTANTES TIPO TIRO
        private const int Tiro_Normal = 0;
        private const int Tiro_7_M = 1;
        private const int Tiro_Contraataque = 2;

        public TiroCommand(Momento tiempo, Jugador jugador, int tipo_throw)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
            tipo_tiro = tipo_throw;
        }
        public TiroCommand(Momento tiempo, Equipo equipo, int tipo_throw)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
            tipo_tiro = tipo_throw;
        }

        public void Execute()
        {
            //SE SUMA UN TIRO AL EQUIPO
            _equipo.Tiros.Add(Momento);

            //DEPENDIENDO DEL TIPO DE TIRO SE SUMA SU ESTADÍSTICA
            switch (tipo_tiro)
            {
                case Tiro_7_M:
                    _equipo.Tiros7M.Add(Momento);
                    break;

                case Tiro_Contraataque:
                    _equipo.TirosContraataque.Add(Momento);
                    break;

                default:
                    break;
            }
        }
        public void Undo()
        {
            //SE RESTA UN TIRO AL EQUIPO
            _equipo.Tiros.Remove(Momento);

            //DEPENDIENDO DEL TIPO DE TIRO SE RESTA SU ESTADÍSTICA
            switch (tipo_tiro)
            {
                case Tiro_7_M:
                    _equipo.Tiros7M.Remove(Momento);
                    break;

                case Tiro_Contraataque:
                    _equipo.TirosContraataque.Remove(Momento);
                    break;

                default:
                    break;
            }
        }

        override public string ToString()
        {
            string s;

            switch (tipo_tiro)
            {
                case Tiro_Normal:
                    s = Momento + " Tiro\n";
                    break;

                case Tiro_7_M:
                    s = Momento + " Tiro 7 M.\n";
                    break;

                case Tiro_Contraataque:
                    s = Momento + " Tiro CntAtq.\n";
                    break;

                default:
                    s = Momento + " Gol\n";
                    break;
            }

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

        public string getNameCommand()
        {
            return "TiroCommand";
        }
    }
}
