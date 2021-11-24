using System;
using System.Drawing;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Comandos
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

        public void Execute()
        {
            _equipo.Paradas.Add(Momento);
            _jugador.Paradas.Add(Momento);
        }

        public void Undo()
        {
            _equipo.Paradas.Remove(Momento);
            _jugador.Paradas.Remove(Momento);
        }

        override public string ToString()
        {
            return Momento + " Parada\n" + _jugador.Number + " " + _jugador.ShortName;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

    }
}
