using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    public class PresentationCommand : ICommandShowable
    {
        private Equipo _equipoL, _equipoV;
        private Encuentro _encuentro;
        private bool _visible;


        //public PresentationCommand(Equipo equipoL, Equipo equipoV)
        //{
        //    _equipoL = equipoL;
        //    _equipoV = equipoV;

        //    Reset();
        //}
        public PresentationCommand(Encuentro encuentro, Equipo equipoL, Equipo equipoV)
        {
            _encuentro = encuentro;
            _equipoL = equipoL;
            _equipoV = equipoV;

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
                        ipf[i].Envia("PresentationIN(['" + _encuentro.NombreCompeticion.Replace("'", "\\'") + "', '" + _encuentro.NombrePabellon.Replace("'", "\\'") + "', '" + _encuentro.Ciudad.Replace("'", "\\'") + "', '" + _encuentro.Fecha.Replace("'", "\\'") + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("PresentationOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Presentación";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

    }
}
