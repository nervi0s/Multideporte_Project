using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;
using System;
using System.Drawing;

namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class PerfilJugadorCommand : ICommandShowable
    {
        public PerfilJugador perfilJugador { get; set; }
        private bool _visible;


        public PerfilJugadorCommand(PerfilJugador pj)
        {
            perfilJugador = pj;

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
                        ipf[i].Envia("PerfilJugadorIN(['" +
                            perfilJugador.photoPath.Replace(@"\", @"\\")+ "', '" +
                            perfilJugador.stat1 + "', '" +
                            perfilJugador.stat2 + "', '" +
                            perfilJugador.stat3 + "', '" +
                            perfilJugador.stat4 + "', '" +
                            perfilJugador.stat5 + "', '" +
                            perfilJugador.stat6 + "', '" +
                            perfilJugador.stat7 + "', '" +
                            perfilJugador.stat8 + 
                            "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("PerfilJugadorOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return perfilJugador.stat1 + "\n" + perfilJugador.stat2 + "\n" + perfilJugador.stat3;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
