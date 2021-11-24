using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class GolPPCommand : ICommandExecutable
    {
        public Momento Momento { get; set; }
        private Jugador _jugador;
        private Equipo _equipo;

        //private bool _visible;

        public GolPPCommand(Momento tiempo, Jugador jugador)
        {
            Momento = tiempo;
            _jugador = jugador;
            _equipo = jugador.Equipo;
        }
        public GolPPCommand(Momento tiempo, Equipo equipo)
        {
            Momento = tiempo;
            _jugador = null;
            _equipo = equipo;
        }

        public void Execute()
        {
            _equipo.GolesPP.Add(Momento);

            if (_jugador != null)
            {
                _jugador.GolesPP.Add(Momento);
            }
        }

        public void Undo()
        {
            _equipo.GolesPP.Remove(Momento);

            if (_jugador != null)
            {
                _jugador.GolesPP.Remove(Momento);
            }
        }

        //public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        //{

        //    if (!_visible)
        //    {
        //        string s = "PlayerPPIN(['" + _equipo.TeamCode + "',";
        //        if (_jugador != null)
        //        {
        //            s += "'" + _jugador.Number + "', '" + _jugador.FullName + "', '" + _jugador.ShortName + "',";
        //        }
        //        else
        //        {
        //            s += "'', '" + _equipo.ShortName + "', '',";
        //        }
        //        s += "' ', 1])";
        //        for (int i = 0; i < n; i++)
        //        {
        //            if (Program.EstaActivado(i))
        //                ipf[i].Envia(s);
        //        }
        //        _visible = true;
        //    }
        //    else
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            if (Program.EstaActivado(i))
        //                ipf[i].Envia("PlayerPPOUT()");
        //        }
        //        _visible = false;
        //    }
        //    return _visible;
        //}

        //public void Reset()
        //{
        //    _visible = false;
        //}

        override public string ToString()
        {
            string s = Momento + " Gol (PP)\n";

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
