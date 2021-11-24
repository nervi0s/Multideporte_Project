using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class NextTransmissionCommand : ICommandShowable
    {
        public NextTransmission nextTransmission { get; set; }
        private bool _visible;
        
        public NextTransmissionCommand(NextTransmission nt)
        {
            nextTransmission = nt;

            Reset();
        }

        public string getNameCommand()
        {
            return "NextTransmissionCommand";
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
                        ipf[i].Envia("NextTransmissionIN(['" + nextTransmission.Grupo + "', '" + nextTransmission.Equipo1 + "', '" + nextTransmission.Equipo2 + "', '" + nextTransmission.Lugar.Replace("'", "\\\'") + "', '" + nextTransmission.Hora + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("NextTransmissionOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return nextTransmission.Grupo + "\n" + nextTransmission.Equipo1 + " " + nextTransmission.Equipo2 + " " + nextTransmission.Lugar + " " + nextTransmission.Hora;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }
    }
}
