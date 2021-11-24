﻿using System;
using System.Drawing;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class FaltaRecibidaCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        public FaltaRecibidaCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public FaltaRecibidaCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.FaltasRecibidas.Add(Momento);

            if (_jugador != null)
                _jugador.FaltasRecibidas.Add(Momento);
        }

        public void Undo()
        {
            _equipo.FaltasRecibidas.Remove(Momento);

            if (_jugador != null)
                _jugador.FaltasRecibidas.Remove(Momento);
        }

        override public string ToString()
        {
            string s = Momento + " Falta Recibida\n";

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
