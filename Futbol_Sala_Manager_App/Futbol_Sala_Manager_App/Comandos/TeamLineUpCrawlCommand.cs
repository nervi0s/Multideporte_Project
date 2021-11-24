using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class TeamLineUpCrawlCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;


        public TeamLineUpCrawlCommand(Equipo equipo)
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
                    string peticion = "TeamLineUpCrawlIN(['" + _equipo.TeamCode + "', '" +
                        idioma[i].Coach + " " + _equipo.Entrenador.FullName.Replace("'", "\\'") + "', '" + _equipo.Entrenador.ShortName.Replace("'", "\\'") + "', '" + idioma[i].Starters + "'"; 
                    peticion += genPeticionJugadoresTitulares(idioma[i]);
                    peticion +=  ", '" + idioma[i].Reserves + "'";
                    peticion += genPeticionJugadoresReservas(idioma[i]);
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
                        ipf[i].Envia("TeamLineUpCrawlOUT()");
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
        
        private string genPeticionJugadoresTitulares(IdiomaData idioma)
        {
            string s = "";
            //int pos_barra = 0;
            //string ruta_foto = "";

            _equipo.Jugadores.Sort(new JugadorComparerLineUp());
            foreach (Jugador j in _equipo.Jugadores)
            {
                string p = (j.Posicion == Jugador.Portero ? " " + idioma.GK : "");
                string c = (j.Capitan ? " " + idioma.CP : "");
                //pos_barra = j.PhotoPath.LastIndexOf(@"\");
                //ruta_foto = j.PhotoPath.Substring(pos_barra + 1, (j.PhotoPath.Length - pos_barra - 1));

                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
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

                s += ", ['" + j.Number + "', '" + j.FullName.Replace("'", "\\'") + p + c + "', '" + j.ShortName.Replace("'", "\\'") + "', " + (j.SancionSiAmarilla ? 1 : 0) + "]";
            }


            return s;
        }

    }
}
