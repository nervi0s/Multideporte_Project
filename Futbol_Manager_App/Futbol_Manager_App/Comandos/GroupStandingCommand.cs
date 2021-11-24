using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class GroupStandingCommand : ICommandShowable
    {
        public GroupStanding groupStanding { get; set; }

        private bool _visible;


        public GroupStandingCommand(GroupStanding gs)
        {
            groupStanding = gs;

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
                string message = "GroupStandingIN(['";
                message += groupStanding.League + "', '" + groupStanding.Group + "', '";
                for (int i = 0; i < groupStanding.Teams.Length; ++i)
                {
                    var t = groupStanding.Teams[i];
                    message += t.Equipo + "', '" + t.pts + "', '" + t.p + "', '" + t.w + "', '" + t.d + "', '" + t.l + "', '" + t.gf + "', '" + t.ga;
                    if (i < groupStanding.Teams.Length - 1)
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
                        ipf[i].Envia("GroupStandingOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return groupStanding.League + "\n" + groupStanding.Group;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

    }
}
