using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
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
                    string peticion = "TeamsReservesIN(['" + idioma[i].Reserves + "', '" + idioma[i].Coaches + "', '" + _equipoL.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipoV.Entrenador.ShortName.Replace("'", "\\'") + "'";
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

        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            // locales
            _equipoL.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipoL.Banquillo)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", [-1, '" + j.Number + "', '" + j.ShortName.Replace("'", "\\'") + p + c + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            // visitantes
            _equipoV.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipoV.Banquillo)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");
                
                s += ", [0, '" + j.Number + "', '" + j.ShortName.Replace("'", "\\'") + p + c + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";

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
