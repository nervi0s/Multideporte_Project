using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
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

        public string getNameCommand()
        {
            return "GroupStandingCommand";
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
                for(int i = 0; i < groupStanding.Teams.Length; ++i)
                {
                    var t = groupStanding.Teams[i];
                    message += t.Equipo + "', '" + t.p + "', '" + t.w + "', '" + t.pts + "', '" + t.d + "', '" + t.l + "', '" + t.gf + "', '" + t.ga;
                    if (i < groupStanding.Teams.Length - 1)
                        message += "', '";
                }
                for (int i = 0; i < groupStanding.Matches.Length; ++i)
                {
                    var m = groupStanding.Matches[i];
                    message += m.Equipo1 + "', '" + m.Result1 + "', '" + m.Equipo2 + "', '" + m.Result2;
                    if (i < groupStanding.Matches.Length - 1)
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

        public Jugador GetJugador()
        {
            return null;
        }
    }
}
