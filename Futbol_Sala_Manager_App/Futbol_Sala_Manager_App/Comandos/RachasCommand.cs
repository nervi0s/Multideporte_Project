using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class RachasCommand : ICommandShowable
    {
        public Rachas rachas { get; set; }
        private bool _visible;


        public RachasCommand(Rachas r)
        {
            rachas = r;

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
                    if (Program.EstaActivado(i))
                    {
                        // Comienzo del mensaje
                        string message = "RachasIN(['" + rachas.photoPath.Replace(@"\", @"\\")+ "'";

                        // Mitad del mensaje
                        foreach (var m in rachas.partidos)
                        {
                            message += ", '" +
                                m.info + "', '" +
                                m.equipoLocal + "', '" +
                                m.puntosLocal + "', '" +
                                m.puntosVisitante + "', '" +
                                m.equipoVisitante + "', '" +
                                m.puntosInfo + "'";
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
                        ipf[i].Envia("RachasOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Rachas \n" + rachas.equipo;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
