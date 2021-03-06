using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class RefereesCommand : ICommandShowable
    {
        private Arbitro _arbitro1;
        private Arbitro _arbitro2;       
        private bool _visible;
        
        public RefereesCommand(Arbitro arbitro1, Arbitro arbitro2)
        {
            _arbitro1 = arbitro1;
            _arbitro2 = arbitro2;
       
            Reset();
        }

        public string getNameCommand()
        {
            return "RefereesCommand";
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

                        ipf[i].Envia("RefereesIN(['" + idioma[i].Officials + "', '" +
                            _arbitro1.FullName.Replace("'", "\\'") + "', '" + _arbitro1.ShortName.Replace("'", "\\'") + "', '" + _arbitro1.Nacionalidad.Replace("'", "\\'") + "', '" + _arbitro1.Colegio.Replace("'", "\\'") + "', '" +
                            _arbitro2.FullName.Replace("'", "\\'") + "', '" + _arbitro2.ShortName.Replace("'", "\\'") + "', '" + _arbitro2.Nacionalidad.Replace("'", "\\'") + "', '" + _arbitro2.Colegio.Replace("'", "\\'") + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("RefereesOUT()");
                }
                _visible = false;
            }

            return _visible;
        }

        override public string ToString()
        {
            return "Tabla de Árbitros";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }

        public Jugador GetJugador()
        {
            return null;
        }

        public int numReferees()
        {
            int n = 0;

            if (_arbitro2.FullName.Length > 0)
                n++;
           
            return n;
        }

    }
}
