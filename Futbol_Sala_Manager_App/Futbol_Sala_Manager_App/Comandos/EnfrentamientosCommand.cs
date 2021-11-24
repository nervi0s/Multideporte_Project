using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class EnfrentamientosCommand : ICommandShowable
    {
        public Enfrentamientos enfrentamientos { get; set; }
        private bool _visible;


        public EnfrentamientosCommand(Enfrentamientos e)
        {
            enfrentamientos = e;

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
                        string message = "EnfrentamientosIN(['" + enfrentamientos.titulo.Replace("'", "\\'") + "', '" + GetDivisionName(idioma[0], enfrentamientos.division.Replace("'", "\\'")) + "'";

                        // Mitad del mensaje
                        foreach (var p in enfrentamientos.partidos)
                        {
                            message += ", '" +
                                p.equipoL.Replace("'", "\\'") + "', '" +
                                p.escudoL.Replace("'", "\\'") + "', '" +
                                p.info.Replace("'", "\\'") + "', '" +
                                p.equipoV.Replace("'", "\\'") + "', '" +
                                p.escudoV.Replace("'", "\\'") + "'";
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
                        ipf[i].Envia("EnfrentamientosOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return enfrentamientos.titulo + "\n" + enfrentamientos.division;
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

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
