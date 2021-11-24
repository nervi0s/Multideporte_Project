using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;
using System;
using System.Drawing;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class ClasificacionCommand : ICommandShowable
    {
        public Clasificacion clasificacion { get; set; }
        private bool _visible;

        public ClasificacionCommand(Clasificacion c)
        {
            clasificacion = c;

            Reset();
        }
        
        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            // Si existe telefono se llama a las funciones PreMatch_2lineas_IN/OUT en lugar de PreMatchIN/OUT
            //string funName = (Prematch.Telefono == "" ? "PreMatch" : "PreMatch_2lineas_");

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                    {
                        // Comienzo del mensaje
                        string message = "ClasificacionIN(['" + GetDivisionName(idioma[0], clasificacion.division).Replace("'", "\\'") + "', ";

                        // Mitad del mensaje
                        for(int j = 0; j < clasificacion.equipos.Length; ++j)
                        {
                            message += (j == 0 ? "'" : ", '") +
                                clasificacion.equipos[j].equipo.Replace("'", "\\'") + "', '" +
                                clasificacion.equipos[j].pt + "', '" +
                                clasificacion.equipos[j].pj + "', '" +
                                clasificacion.equipos[j].pg + "', '" +
                                clasificacion.equipos[j].pe + "', '" +
                                clasificacion.equipos[j].pp + "', '" +
                                clasificacion.equipos[j].gf + "', '" +
                                clasificacion.equipos[j].gc + "'";
                        }

                        // Final del mensaje
                        message += "])";
                        
                        ipf[i].Envia(message);
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ClasificacionOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        string GetDivisionName(IdiomaData idioma, string division)
        {
            switch (division)
            {
                case "1 Division":
                    return idioma.Division1;
                case "2 Division":
                    return idioma.Division2;
                default:
                    return "";
            }
        }

        override public string ToString()
        {
            return "Clasificación"  + "\n" + clasificacion.division;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
