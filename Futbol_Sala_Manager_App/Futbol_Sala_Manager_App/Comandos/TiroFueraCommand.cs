using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class TiroFueraCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


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

        public void Undo()
        {
            _equipo.Tirosfuera.Remove(Momento);

            if (_jugador != null)
                _jugador.Tirosfuera.Remove(Momento);
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

    }
}
