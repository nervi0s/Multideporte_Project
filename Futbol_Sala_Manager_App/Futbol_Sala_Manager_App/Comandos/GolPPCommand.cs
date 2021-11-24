using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class GolPPCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


        public GolPPCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public GolPPCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.GolesPP.Add(Momento);

            if (_jugador != null)
            {
                _jugador.GolesPP.Add(Momento);
            }
        }

        public void Undo()
        {
            _equipo.GolesPP.Remove(Momento);

            if (_jugador != null)
            {
                _jugador.GolesPP.Remove(Momento);
            }
        }
        
        override public string ToString()
        {
            string s = Momento + " Gol (PP)\n";

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
            return _jugador;
        }

    }
}
