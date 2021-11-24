using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class CountdownCommand : ICommandShowable
    {
        public Countdown Countdown { get; set; }
        private bool _visible;


        public CountdownCommand(Countdown countdown)
        {
            Countdown = countdown;

            Reset();
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
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("CountdownIN(['" + formatHora() + "', '" + Countdown.Desfase + "', '" + Countdown.Referencia + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("CountdownOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Countdown\n" + formatHora() + " (" + String.Format("{0:+0;-0}", Countdown.Desfase) + ") " + Countdown.Referencia;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        private string formatHora()
        {
            return Countdown.Hora.ToString("00") + ":" + Countdown.Minutos.ToString("00");
        }

    }
}
