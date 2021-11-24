using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class HighligthsCommand : ICommandShowable
    {
        //public Highligths High { get; set; }
        private bool _visible;


        public HighligthsCommand()
        {
            //High = h;
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
                        ipf[i].Envia("HighligthsIN()");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("HighligthsOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Highlights";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

    }
}
