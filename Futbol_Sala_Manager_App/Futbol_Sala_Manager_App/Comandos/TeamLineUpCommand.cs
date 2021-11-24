using System.Drawing;
using System.Collections.Generic;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;
using System;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class TeamLineUpCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;
        private List<Jugador> _convocados = new List<Jugador> { };
       

        public TeamLineUpCommand(Equipo equipo)
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
                _convocados.Clear();
                dame_jugadores_titulares();
                dame_jugadores_suplentes();
                _convocados.Sort(new JugadorComparerLineUp());
      
                for (int i = 0; i < n; i++)
                {
                    string peticion = "TeamLineUpIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode + "', '" + 
                        idioma[i].Coach + "', '" + _equipo.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.RutaFoto.Replace(@"\", @"\\")+ "', " + _equipo.Entrenador.SancionSiAmarilla;

                    peticion += gen_cadena_jugadores(idioma[i]);
                    peticion += "])";

                    //Console.WriteLine(peticion);

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
                        ipf[i].Envia("TeamLineUpOUT()");
                }
                _visible = false;
            }

            return _visible;
            


            /*
            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string peticion = "TeamLineUpIN(['" + _equipo.TeamCode + "', '" +
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
                        ipf[i].Envia("TeamLineUpOUT()");
                }
                _visible = false;
            }

            return _visible;
            */ 
        }

        override public string ToString()
        {
            return "Equipo: " + _equipo.TeamCode;
        }

        public Color GetColor()
        {
            return _equipo.Color1;
        }

        private void dame_jugadores_titulares()
        {
            //Console.WriteLine("--->>>> Jugadores titulares: " + _equipo.Jugadores.Count);
            for (int i = 0; i < _equipo.Jugadores.Count; i++)
                _convocados.Add(_equipo.Jugadores[i]);
        }
        private void dame_jugadores_suplentes()
        {
            for (int i = 0; i < _equipo.Banquillo.Count; i++)
                _convocados.Add(_equipo.Banquillo[i]);
            //Console.WriteLine("--->>>> Jugadores suplentes: " + _equipo.Banquillo.Count);
        }

        private string gen_cadena_jugadores(IdiomaData idioma)
        {
            string s = "";
                                   
            foreach (Jugador j in _convocados)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");
                        
                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + getPosicion(j, idioma) + "', '" + j.RutaFoto.Replace(@"\", @"\\")+ "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";

                //Console.WriteLine(j.FullName);
            }
            //Console.WriteLine("==============================");

            return s;
        }


        /*
        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            _equipo.Jugadores.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipo.Jugadores)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.FullName + p + c + "', '" + j.ShortName + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
           
            return s;
        }
        
        private string genPeticionJugadoresReservas(IdiomaData idioma)
        {
            string s = "";

            _equipo.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipo.Banquillo)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.FullName + p + c + "', '" + j.ShortName + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            
            return s;
        }
        */

        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;

                case Jugador.Cierre:
                    return idioma.Cierre;

                case Jugador.AlaCierre:
                    return idioma.AlaCierre;

                case Jugador.Ala:
                    return idioma.Ala;

                case Jugador.AlaPivot:
                    return idioma.AlaPivot;

                case Jugador.Pivot:
                    return idioma.Pivot;

                default:
                    return idioma.Universal;
            }
        }
    }
}
