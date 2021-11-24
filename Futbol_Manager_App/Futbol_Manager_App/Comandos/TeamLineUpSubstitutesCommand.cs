﻿using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class TeamLineUpSubstitutesCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;


        public TeamLineUpSubstitutesCommand(Equipo equipo)
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
                    string peticion = "TeamLineUpSubstitutesIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', '" +
                        idioma[i].Coach + " " + _equipo.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.ShortName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.RutaFoto.Replace(@"\", "/");
                    peticion += genPeticionJugadores(idioma[i]);
                    peticion += genPeticionSuplentes(idioma[i]);
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
                        ipf[i].Envia("TeamLineUpSubstitutesOUT()");
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

        private string genPeticionJugadores(IdiomaData idioma)
        {
            string s = "";

            _equipo.Jugadores.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipo.Jugadores)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + getPosicion(j, idioma) + "', '" + j.RutaFoto.Replace(@"\", @"\\")+ "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            return s;
        }

        private string genPeticionSuplentes(IdiomaData idioma)
        {
            string s = "";

            _equipo.Banquillo.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipo.Banquillo)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");

                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', '" + getPosicion(j, idioma) + "', '" + j.RutaFoto.Replace(@"\", @"\\")+ "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }
            // completa hasta los 8 con vacios
            for (int i = 0; i < (8 - _equipo.Banquillo.Count); i++)
            {
                s += ", ['0', '', '', 0]";
            }

            return s;
        }

        private string getPosicion(Jugador jugador, IdiomaData idioma)
        {
            switch (jugador.Posicion)
            {
                case Jugador.Portero:
                    return idioma.Goalkeeper;

                case Jugador.Defensa:
                    return idioma.Defender;

                case Jugador.Centrocampista:
                    return idioma.Midfielder;

                default:
                    return idioma.Forward;
            }
        }

    }
}
