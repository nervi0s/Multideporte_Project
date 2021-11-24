using System;
using System.Collections.Generic;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    public class ScoreBoardCommand : ICommandShowable
    {
        private Equipo _equipoL;
        private Equipo _equipoV;
        private bool _visible;

        public ScoreBoardCommand(Equipo equipoL, Equipo equipoV)
        {
            _equipoL = equipoL;
            _equipoV = equipoV;

            Reset();
        }

        public string getNameCommand()
        {
            return "ScoreBoardCommand";
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
                    {
                        if (Program.EstaActivado(i))
                            //ipf[i].Envia("ResetTexts()");
                            ipf[i].Envia(genPeticion(idioma[i]));
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ScoreBoardOUT()");
                }
                _visible = false;
            }

            return _visible;
        }

        override public string ToString()
        {
            return "Marcador";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }
        
        private string genPeticion(IdiomaData idioma)
        {
            List<Gol> lista = new List<Gol>();
            
            // Locales
            List<Jugador> jugadoresL = new List<Jugador>();
            jugadoresL.AddRange(_equipoL.Jugadores);
            jugadoresL.AddRange(_equipoL.Banquillo);

            foreach (Jugador jugador in jugadoresL)
            {
                foreach (Momento gol in jugador.FieldThrowsComplete)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.NORMAL, idioma));
                }
                foreach (Momento gol in jugador.SevenMthrowsComplete)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.PENALTY, idioma));
                }
               
            }
            
            /*foreach (Jugador jugador in jugadoresL)
            {
                foreach (Momento gol in jugador.Goles)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.NORMAL, idioma));
                }
                foreach (Momento gol in jugador.GolesPenalty)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.PENALTY, idioma));
                }
                foreach (Momento gol in jugador.GolesPP)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.PP, idioma));
                }
            }*/

            // Visitantes
            List<Jugador> jugadoresV = new List<Jugador>();
            jugadoresV.AddRange(_equipoV.Jugadores);
            jugadoresV.AddRange(_equipoV.Banquillo);

            foreach (Jugador jugador in jugadoresV)
            {

                foreach (Momento gol in jugador.FieldThrowsComplete)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.NORMAL, idioma));
                }
                foreach (Momento gol in jugador.SevenMthrowsComplete)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, true, Gol.PENALTY, idioma));
                }


                /*foreach (Momento gol in jugador.Goles)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, false, Gol.NORMAL, idioma));
                }
                foreach (Momento gol in jugador.GolesPenalty)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, false, Gol.PENALTY, idioma));
                }
                foreach (Momento gol in jugador.GolesPP)
                {
                    lista.Add(new Gol(gol, jugador.ShortName, false, Gol.PP, idioma));
                }*/
            }

            // Ordena los goles por el momento en el que se han marcado
            lista.Sort();

            bool primero = true;
            string s = "ScoreBoardIN([";

            foreach (Gol g in lista)
            {
                if (primero)
                    primero = false;
                else
                    s += ",";

                s += g.GetCadena();
            }
            s += "])";

            return s;
        }


        class Gol : IComparable<Gol>
        {
            public const int NORMAL = 1;
            public const int PP = 2;
            public const int PENALTY = 3;

            private int segundo;
            private string cadena;

            public Gol(Momento momento, string jugador, bool local, int tipo, IdiomaData idioma)
            {
                this.segundo = momento.SegundoAbsoluto;

                // Si es en propia puerta se cambia de equipo
                if (tipo == PP)
                    local = !local;

                string minutos = momento.GetMinutoJuego();
                string descuento = momento.GetMinutoDescuento();

                // Se coloca el texto de gol especial segun proceda
                if (momento.GetMinutoDescuento() == "0'")
                {
                    minutos += getTipo(tipo, idioma);
                }
                else
                {
                    descuento += getTipo(tipo, idioma);
                }

                this.cadena = "['" + (local ? "-1" : "0") + "', '" +
                      jugador + "', \"" +
                      minutos + "\", \"" +
                      descuento + "\"]";
            }

            public int CompareTo(Gol obj)
            {
                return this.segundo - obj.segundo;
            }

            public string GetCadena()
            {
                return this.cadena;
            }

            private string getTipo(int tipo, IdiomaData idioma)
            {
                switch (tipo)
                {
                    //case PP:
                    //    return " " + idioma.OG;

                    case PENALTY:
                        return " " + idioma.P;

                    default:
                        return "";
                }
            }
        }
    }
}
