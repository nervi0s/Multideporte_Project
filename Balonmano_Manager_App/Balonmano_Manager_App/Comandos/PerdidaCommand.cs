using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Balonmano_Manager_App.Beans;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    class PerdidaCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        public PerdidaCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public PerdidaCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public string getNameCommand()
        {
            return "PerdidaCommand";
        }

        public void Execute()
        {
            //SE SUMA UNA PERDIDA
            _equipo.PerdidasTiro.Add(Momento);

        }
        public void Undo()
        {
            //SE RESTA UNA PERDIDA
            _equipo.PerdidasTiro.Remove(Momento);

        }

        override public string ToString()
        {
            string s = Momento + " Perdida\n";

            s += _equipo.ShortName;

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
    }
}
