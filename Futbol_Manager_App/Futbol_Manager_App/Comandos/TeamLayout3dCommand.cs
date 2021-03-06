using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class TeamLayout3dCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;


        public TeamLayout3dCommand(Equipo equipo)
        {
            _equipo = equipo;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {

                    string peticion = "TeamLayout3DIN(['" + _equipo.TeamCode + "', '" +
                        idioma[i].Coach + " " + _equipo.Entrenador.FullName + "', '" + _equipo.Entrenador.ShortName + "'";
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
                        ipf[i].Envia("TeamLayoutOUT3D()");
                }
                _visible = false;
            }

            return _visible;
        }

        override public string ToString()
        {
            return "Disposición 3D: " + _equipo.TeamCode;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            foreach (Jugador j in _equipo.Jugadores)
            {
                Console.WriteLine(String.Format("{0,2} {1,-20} [{2:+0.00;-0.00} ; {3:+0.00;-0.00}] -> [{4:0.00} ; {5:0.00}]",
                    j.Number, j.ShortName, (Convert.ToSingle(j.PosX) * 2 - 1), (Convert.ToSingle(j.PosY) * 2 - 1), Convert.ToSingle(j.PosX), Convert.ToSingle(j.PosY)));

                string c = (j.Capitan ? " " + idioma.CP : "");
                s += ", ['" + j.Number + "', '" + j.ShortName + c + "', '" + j.PosX + "', '" + j.PosY + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }

            return s;
        }
    }
}
