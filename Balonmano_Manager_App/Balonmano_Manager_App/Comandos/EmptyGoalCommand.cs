using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    public class EmptyGoalCommand : ICommandImmediateExecutable
    {
        private Equipo _equipo;
        private bool _activo;


        public EmptyGoalCommand(Equipo equipo, bool activo)
        {
            _equipo = equipo;
            _activo = activo;
        }

        public void ExecuteImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            if (_activo)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("EmptyGoalIN(['" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "', '" + idioma[i].EmptyGoal + "'])");
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("EmptyGoalOUT(['" + _equipo.TeamCode.Replace("'", "\\'") + "'])");
                }
            }
        }

        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //No existe la posibilidad de borrarlo
        }

        override public string ToString()
        {
            return "EmptyGoal " + _equipo.TeamCode;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        public string getNameCommand()
        {
            return "EmptyGoalCommand";
        }
    }
}
