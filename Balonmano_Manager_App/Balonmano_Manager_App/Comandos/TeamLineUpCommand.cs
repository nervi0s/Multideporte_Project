using System.Drawing;
using System.Collections.Generic;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;
using System;

namespace Balonmano_Manager_App.Comandos
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

        public string getNameCommand()
        {
            return "TeamLineUpCommand";
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {
            _convocados.Clear();
            dame_jugadores_titulares();
            dame_jugadores_suplentes();
            _convocados.Sort(new JugadorComparerLineUp());

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    string peticion = "TeamLineUpIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', '" + idioma[i].Coach + "', '" + _equipo.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.RutaFoto.Replace(@"\", @"\\") + "', " + _equipo.Entrenador.SancionSiAmarilla;
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

        public Jugador GetJugador()
        {
            return null;
        }
        
        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            //_equipo.Jugadores.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _convocados)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + getPosicion(j, idioma) + "', '" + j.RutaFoto.Replace(@"\", @"\\") + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            return s;
        }

        //private string genPeticionJugadores(IdiomaData idioma)
        //{
        //    string s = "";

        //    _equipo.Jugadores.Sort(new JugadorComparerLineUp());
        //    foreach (Jugador j in _equipo.Jugadores)
        //    {
        //        string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
        //        string c = (j.Capitan ? " " + idioma.CP : "");

        //        s += ", ['" + j.Number + "', '" + j.FullName + p + c + "', '" + j.ShortName + "', '" + getPosicion(j, idioma) + "', '" + j.RutaFoto + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
        //    }
        //    return s;
        //}

        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;

                case Jugador.Extremo:
                    return idioma.Extremo;

                case Jugador.ExtremoDerecho:
                    return idioma.ExtremoDerecho;

                case Jugador.ExtremoIzquierdo:
                    return idioma.ExtremoIzquierdo;

                case Jugador.Lateral:
                    return idioma.Lateral;

                case Jugador.LateralDerecho:
                    return idioma.LateralDerecho;

                case Jugador.LateralIzquierdo:
                    return idioma.LateralIzquierdo;

                case Jugador.Central:
                    return idioma.Central;

                default:
                    return idioma.Pivote;
            }
        }
    }
}
