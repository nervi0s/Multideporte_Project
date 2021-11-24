using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class ParadaTiroCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


        public ParadaTiroCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public ParadaTiroCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public string getNameCommand()
        {
            return "ParadaTiroCommand";
        }

        public void Execute()
        {
            //SE SUMA UNA PARADA
            _equipo.ParadasTiro.Add(Momento);

            if (_jugador != null)
            {
                _jugador.ParadasTiro.Add(Momento);
            }
        }
        public void Undo()
        {
            //SE RESTA UNA PARADA
            _equipo.ParadasTiro.Remove(Momento);

            if (_jugador != null)
            {
                _jugador.ParadasTiro.Remove(Momento);
            }
        }

        override public string ToString()
        {
            string s = Momento +" Parada\n";

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

        public Jugador GetJugador()
        {
            return null;
        }
    }
}
