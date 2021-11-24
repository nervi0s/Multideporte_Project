using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class TimeOutCommand : ICommandShowable
    {
        private Equipo _equipo;
        private bool _visible;


        public TimeOutCommand(Equipo  equipo)
        {
            _equipo = equipo;
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
                    {
                        if (_equipo != null)
                            ipf[i].Envia("TimeOutIN(['" + idioma[i].TimeOut + "', '" + _equipo.FullName.Replace("'", "\\'") + "', '" + _equipo.ShortName.Replace("'", "\\'") + "', '" + _equipo.TeamCode.Replace("'", "\\'") + "'])");
                        //else
                        //    ipf[i].Envia("TimeOutIN(['" + idioma[i].TimeOut + "', '', ''])");
                    }
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("TimeOutOUT()");
                }
                _visible = false;

            }
            return _visible;
        }

        override public string ToString()
        {
            if (_equipo == null)
                return "Tiempo Muerto" ;
            else
                return "Tiempo Muerto " + _equipo.TeamCode;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
