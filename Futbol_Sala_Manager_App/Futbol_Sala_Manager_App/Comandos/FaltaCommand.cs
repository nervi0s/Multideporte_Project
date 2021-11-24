using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class FaltaCommand : ICommandExecutable, ICommandImmediateExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;


        public FaltaCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public FaltaCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Faltas.Add(Momento);
            _equipo.FaltasAcumuladas++;

            if (_jugador != null)
                _jugador.Faltas.Add(Momento);
        }

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (Program.EstaActivado(i))
                    ipf[i].Envia("Falta(['" + _equipo.TeamCode.Replace("'", "\\'") + "', '" + _equipo.Faltas.Count + "'])");
            }
        }

        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            for (int i = 0; i < n; i++)
            {
                if (Program.EstaActivado(i))
                    ipf[i].Envia("Falta(['" + _equipo.TeamCode.Replace("'", "\\'") + "', '" + _equipo.Faltas.Count + "'])");
            }
        }

        public void Undo()
        {
            _equipo.Faltas.Remove(Momento);
            _equipo.FaltasAcumuladas--;

            if (_jugador != null)
                _jugador.Faltas.Remove(Momento);
        }        

        override public string ToString()
        {
            string s = Momento + " Falta\n";

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
