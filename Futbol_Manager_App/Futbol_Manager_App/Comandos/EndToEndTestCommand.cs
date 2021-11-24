using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class EndToEndCommand : ICommandShowable
    {
        public EndToEnd endToEnd { get; set; }
        private bool _visible;

        public EndToEndCommand(EndToEnd toEnd)
        {
            endToEnd = toEnd;

            Reset();
        }

        public EndToEndCommand()
        {
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
                        ipf[i].Envia("EndToEndTestIN(['" + endToEnd.Header.Replace("'", "\\'") + "', '" + endToEnd.Linea1.Replace("'", "\\'") + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("EndToEndTestOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return endToEnd.Header + "\n" + endToEnd.Linea1;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
