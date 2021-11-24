using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class RedCardTeamCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Equipo _equipo;


        public RedCardTeamCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _equipo = equipo;
        }
        
        public void Execute()
        {
            _equipo.TRojas.Add(Momento);
        }
        public void Undo()
        {
            _equipo.TRojas.Remove(Momento);
        }
        
        override public string ToString()
        {
            return Momento + " T. Roja\n" + _equipo.ShortName;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

    }
}
