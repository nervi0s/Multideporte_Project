using System;
using System.Drawing;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class CornerCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Equipo _equipo;
        
        
        public CornerCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Corners.Add(Momento);
        }

        public void Undo()
        {
            _equipo.Corners.Remove(Momento);
        }
        
        override public string ToString()
        {
            return Momento + " Corner\n" + _equipo.ShortName;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

    }
}
