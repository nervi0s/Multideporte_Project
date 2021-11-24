using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class PreMatchCommand : ICommandShowable
    {
        public Prematch Prematch { get; set; }
        private bool _visible;


        public PreMatchCommand(Prematch prematch)
        {
            Prematch = prematch;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {            
            // Si existe telefono se llama a las funciones PreMatch_2lineas_IN/OUT en lugar de PreMatchIN/OUT
            string funName = (Prematch.Telefono == "" ? "PreMatch" : "PreMatch_2lineas_");

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia(funName + "IN(['" + Prematch.Broadcaster.Replace("'", "\\'") + "', '" + Prematch.Hora + "', '" + Prematch.Referencia + "', " + (Prematch.Tipo == "Pre" ? 0 : 1) + ", '" + Prematch.Telefono + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia(funName + "OUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return Prematch.Tipo + " Match\n" + Prematch.Broadcaster + ", " + Prematch.Hora + " " + Prematch.Referencia + " " + Prematch.Telefono;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

    }
}
