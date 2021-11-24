using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class TiroAPuertaCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


        public TiroAPuertaCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public TiroAPuertaCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Tirosapuerta.Add(Momento);

            if (_jugador != null)
                _jugador.Tirosapuerta.Add(Momento);
        }
        
        public void Undo()
        {
            _equipo.Tirosapuerta.Remove(Momento);

            if (_jugador != null)
                _jugador.Tirosapuerta.Remove(Momento);
        }
        
        override public string ToString()
        {
            string s = Momento + " Tiro a puerta\n";

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
