using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Comandos
{
    public class PresentationCommand : ICommandShowable
    {
        //private string _mensaje;
        private Equipo _equipoL;
        private Equipo _equipoV;
        private Encuentro _encuentro;
        private bool _visible;


        //public PresentationCommand(string mensaje, Equipo equipoL, Equipo equipoV)
        //{
        //    _mensaje = mensaje;
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
            return "Id Partido";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

    }
}
