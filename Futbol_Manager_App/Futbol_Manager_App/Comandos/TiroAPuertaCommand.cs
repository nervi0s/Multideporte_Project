using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class TiroAPuertaCommand : ICommandExecutable, ICommandShowable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;
        private bool _visible;

        public TiroAPuertaCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public TiroAPuertaCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.Tirosapuerta.Add(Momento);

            if (_jugador != null)
                _jugador.Tirosapuerta.Add(Momento);
        }
        
        public void Undo()
        {
            _equipo.Tirosapuerta.Remove(Momento);

            if (_jugador != null)
                _jugador.Tirosapuerta.Remove(Momento);
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                string s = "TiroIN(['" + _equipo.TeamCode + "',";
                if (_jugador != null)
                {
                    s += "'" + _jugador.Number + "', '" + _jugador.FullName + "', '" + _jugador.ShortName + "',";
                }
                if (Momento != null)
                {
                    s += " '" + Momento + ",";
                }
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia(s + " '" + idioma[i].AttemptIn + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("TiroOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        public void Reset()
        {
            _visible = false;
        }
        
        override public string ToString()
        {
            string s = Momento + " Tiro a puerta\n";

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
