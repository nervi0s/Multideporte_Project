using System;
using System.Drawing;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class FueraDeJuegoCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        public FueraDeJuegoCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public FueraDeJuegoCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Fuerasdejuego.Add(Momento);

            if (_jugador != null)
                _jugador.Fuerasdejuego.Add(Momento);
        }
        
        public void Undo()
        {
            _equipo.Fuerasdejuego.Remove(Momento);

            if (_jugador != null)
                _jugador.Fuerasdejuego.Remove(Momento);
        }

        override public string ToString()
        {
            string s = Momento + " Fuera de juego\n";

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

    }
}
