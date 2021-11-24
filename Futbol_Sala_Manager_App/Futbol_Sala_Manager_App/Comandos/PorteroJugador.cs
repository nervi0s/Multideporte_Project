using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class PorteroJugadorCommand : ICommandImmediateExecutable
    {
        private Equipo _equipo;
        private bool _activo;


        public PorteroJugadorCommand(Equipo equipo, bool activo)
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
                        ipf[i].Envia("PorteroJugadorIN(['" + idioma[i].PorteroJugador + "', '" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "'])");
                }
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("PorteroJugadorOUT(['" + _equipo.TeamCode.Replace("'", "\\'") + "'])");
                }
            }
        }

        public void UndoImmediate(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            //No existe la posibilidad de borrarlo
        }

        override public string ToString()
        {
            return "PorteroJugador " + _equipo.TeamCode;
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
            return "PorteroJugadorCommand";
        }
    }
}

