using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class TeamReservesCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;


        public TeamReservesCommand(Equipo equipo)
        {
            _equipo = equipo;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {            
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string peticion = "TeamReservesIN(['" + idioma[i].Reserves + "', '" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', '" + idioma[i].Coach + "', '" + _equipo.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.RutaFoto.Replace(@"\", @"\\")+ "', " + _equipo.Entrenador.SancionSiAmarilla;
                    peticion += genPeticionJugadores(idioma[i]);
                    peticion += "])";
                    {
                        if (Program.EstaActivado(i))
                            ipf[i].Envia(peticion);
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("TeamReservesOUT()");
                }
                _visible = false;
            }

            return _visible;
        }

        override public string ToString()
        {
            return "Suplentes: " + _equipo.TeamCode;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }
        
        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            _equipo.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipo.Banquillo)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + j.RutaFoto.Replace(@"\", @"\\")+ "', " + j.SancionSiAmarilla + "]";
            }
            // completa hasta los 8 con vacios
            for (int i = 0; i < (8 - _equipo.Banquillo.Count); i++)
            {
                s += ", ['0', '', '']";
            }

            return s;
        }

    }
}
