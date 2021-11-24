using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    public class TeamsReservesCommand : ICommandShowable
    {
        private Equipo _equipoL;
        private Equipo _equipoV;
        private bool _visible;
        
        public TeamsReservesCommand(Equipo equipoL, Equipo equipoV)
        {
            _equipoL = equipoL;
            _equipoV = equipoV;

            Reset();
        }

        public string getNameCommand()
        {
            return "TeamsReservesCommand";
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
                    string peticion = "TeamsReservesIN(['" + idioma[i].Reserves + "', '" + idioma[i].Coaches + "', '" + _equipoL.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipoL.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipoL.Entrenador.RutaFoto.Replace(@"\", @"\\") + "', " + _equipoL.Entrenador.SancionSiAmarilla + ", '" + _equipoV.Entrenador.FullName + "', '" + _equipoV.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipoV.Entrenador.RutaFoto.Replace(@"\", @"\\") + "', " + _equipoV.Entrenador.SancionSiAmarilla;
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
                        ipf[i].Envia("TeamsReservesOUT()");
                }
                _visible = false;
            }

            return _visible;
        }

        override public string ToString()
        {
            return "Suplentes";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            // locales
            _equipoL.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipoL.Banquillo)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", [-1, '" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + j.RutaFoto.Replace(@"\", @"\\") + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            // visitantes
            _equipoV.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipoV.Banquillo)
            {
                //string p = (j.Posicion == Jugador.Portero ? idioma.GK + " " : "");
                // string c = (j.Capitan ? idioma.CP + " " : "");

                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", [0, '" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + j.RutaFoto.Replace(@"\", @"\\") + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            // completa hasta los 16 con vacios
            for (int i = 0; i < (16 - _equipoL.Banquillo.Count - _equipoV.Banquillo.Count); i++)
            {
                s += ", [0, '0', '']";
            } 

            return s;
        }

    }
}
