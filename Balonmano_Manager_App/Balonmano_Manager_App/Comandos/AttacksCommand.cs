using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    class AttacksCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


        #region Constructor
        public AttacksCommand(Momento momento, Jugador jugador)
        {
            Momento = momento;
            _jugador = jugador;
            _equipo = _jugador.Equipo;
        }

        public AttacksCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        #endregion

        public string getNameCommand()
        {
            return "AttacksCommand";
        }

        public void Execute()
        {
            _equipo.Ataques.Add(Momento);
        }
        public Jugador GetJugador()
        {
            return null;
        }

        public void Undo()
        {
            _equipo.Ataques.Remove(Momento);
        }  

        override public string ToString()
        {
            string s = Momento + " Ataque\n";

            s += _equipo.ShortName;

            return s;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

    }
}
