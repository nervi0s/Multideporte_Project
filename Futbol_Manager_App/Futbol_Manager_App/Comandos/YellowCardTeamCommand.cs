using System;
using System.Drawing;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class YellowCardTeamCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Equipo _equipo;


        public YellowCardTeamCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.TAmarillas.Add(Momento);
        }

        public void Undo()
        {
            _equipo.TAmarillas.Remove(Momento);
        }
        
        override public string ToString()
        {
            return Momento + " T. Amarilla\n" + _equipo.ShortName;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

    }
}
