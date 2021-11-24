using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class ParadaCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


        public ParadaCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public ParadaCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Paradas.Add(Momento);
            if (_jugador!=null)
                _jugador.Paradas.Add(Momento);
        }

        public void Undo()
        {
            _equipo.Paradas.Remove(Momento);
            if (_jugador!=null)
                _jugador.Paradas.Remove(Momento);
        }

        override public string ToString()
        {
            if (_jugador!=null)
                return Momento + " Parada\n" + _jugador.Number + " " + _jugador.ShortName;
            else
                return Momento + " Parada\n" + _equipo.ShortName;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

    }
}
