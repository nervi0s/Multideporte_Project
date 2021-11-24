using System;
using System.Collections.Generic;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    public class TeamLayoutCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;
        
        public TeamLayoutCommand(Equipo equipo)
        {
            _equipo = equipo;

            Reset();
        }

        public string getNameCommand()
        {
            return "TeamLayoutCommand";
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
                    string peticion = "TeamLayoutIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', '" + idioma[i].Coach + "', '" + _equipo.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.RutaFoto.Replace(@"\", @"\\") + "', " + _equipo.Entrenador.SancionSiAmarilla;
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
                        ipf[i].Envia("TeamLayoutOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Disposición: " + _equipo.TeamCode;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            foreach (Jugador j in _equipo.Jugadores)
            {
                //Console.WriteLine(String.Format("{0,2} {1,-20} [{2:+0.00;-0.00} ; {3:+0.00;-0.00}] -> [{4:0.00} ; {5:0.00}]",
                //j.Number, j.ShortName, j.RutaFoto.Replace(@"\", @"\\"), (Convert.ToSingle(j.PosX) * 2 - 1), (Convert.ToSingle(j.PosY) * 2 - 1), Convert.ToSingle(j.PosX), Convert.ToSingle(j.PosY)));

                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.ShortName.Replace("'", "\\'") + p + c + "', '" + j.RutaFoto.Replace(@"\", @"\\") + "', " + j.SancionSiAmarilla + ", '" + j.PosX + "', '" + j.PosY + "']";
            }

            return s;


            //string s = "";
            //List<Jugador> jugadores = new List<Jugador>(_equipo.Jugadores);
            //Jugador portero = jugadores.Find(j => j.Posicion.Equals(Jugador.Portero));
            //if(portero != null)
            //{
            //    s += ", ['" + portero.Number + "', '" + portero.ShortName + "', '" + portero.PosX + "', '" + portero.PosY + "']";
            //    jugadores.Remove(portero);
            //}
            //foreach (Jugador j in jugadores)
            //{
            //    //Console.WriteLine(String.Format("{0,2} {1,-20} [{2:+0.00;-0.00} ; {3:+0.00;-0.00}] -> [{4:0.00} ; {5:0.00}]",
            //    //    j.Number, j.ShortName, (Convert.ToSingle(j.PosX) * 2 - 1), (Convert.ToSingle(j.PosY) * 2 - 1), Convert.ToSingle(j.PosX), Convert.ToSingle(j.PosY)));

            //    s += ", ['" + j.Number + "', '" + j.ShortName + "', '" + j.PosX + "', '" + j.PosY + "']";
            //}

            //return s;
        }


    }
}
