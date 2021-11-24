using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class BlueCardTeamCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Equipo _equipo;
        
        public BlueCardTeamCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _equipo = equipo;
        }

        public string getNameCommand()
        {
            return "BlueCardTeamCommand";
        }

        public void Execute()
        {
            _equipo.TAzules.Add(Momento);
        }
        public void Undo()
        {
            _equipo.TAzules.Remove(Momento);
        }


        override public string ToString()
        {
            return Momento + " T. Azul\n" + _equipo.ShortName;
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

