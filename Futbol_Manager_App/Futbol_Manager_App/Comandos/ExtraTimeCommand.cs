using System.Drawing;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class ExtraTimeCommand : ICommandShowable
    {
        public int Minutos { get; set; }
        private bool _visible;
        private bool _usado;


        public ExtraTimeCommand(int minutos)
        {
            Minutos = minutos;
            _usado = false;

            Reset();
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma,int n)
        {            
            _usado = true;

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ExtraTimeIN(['" + Minutos + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ExtraTimeOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Tiempo extra\n" + Minutos + " minuto" + (Minutos > 1 ? "s" : "");
        }

        public Color GetColor()
        {
            return (_usado ? Color.AliceBlue : Color.SlateGray);
        }


    }
}
