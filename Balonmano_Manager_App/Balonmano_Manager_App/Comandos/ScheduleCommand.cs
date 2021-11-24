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
    public class ScheduleCommand : ICommandShowable
    {
        public Schedule schedule { get; set; }
        private bool _visible;
        
        public ScheduleCommand(Schedule sch)
        {
            schedule = sch;

            Reset();
        }

        public string getNameCommand()
        {
            return "ScheduleCommand";
        }

        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (!_visible)
            {
                string message = "ScheduleIN(['";
                for (int i = 0; i < schedule.Schedules.Length; ++i)
                {
                    var t = schedule.Schedules[i];
                    message += t.Equipo1 + "', '" + t.Equipo2 + "', '" + t.Info;
                    if (i < schedule.Schedules.Length - 1)
                        message += "', '";
                }
                message += "'])";

                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia(message);
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("ScheduleOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Schedule";
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

