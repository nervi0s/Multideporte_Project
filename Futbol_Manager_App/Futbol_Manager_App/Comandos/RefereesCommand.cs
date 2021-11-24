using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class RefereesCommand : ICommandShowable
    {
        private Arbitro _arbitro1;
        private Arbitro _arbitro2;
        private Arbitro _arbitro3;
        private Arbitro _arbitro4;
        //private Arbitro _auxiliar2;
        private bool _visible;


        public RefereesCommand(Arbitro arbitro1, Arbitro arbitro2, Arbitro arbitro3, Arbitro arbitro4)
        {
            _arbitro1 = arbitro1;
            _arbitro2 = arbitro2;
            _arbitro3 = arbitro3;
            _arbitro4 = arbitro4;
            //_auxiliar2 = auxiliar2;

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
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))

                        //if (numReferees() > 1)
                        //{
                            ipf[i].Envia("RefereesIN(['" + idioma[i].Officials + "', '" + 
                                idioma[i].Referee + "', '" + _arbitro1.FullName.Replace("'", "\\'") + "', '" + _arbitro1.ShortName.Replace("'", "\\'") + "', '" + _arbitro1.Nacionalidad.Replace("'", "\\'") + "', '" +
                                idioma[i].Assistants + "', '" +
                                _arbitro2.FullName.Replace("'", "\\'") + "', '" + _arbitro2.ShortName.Replace("'", "\\'") + "', '" +
                                _arbitro3.FullName.Replace("'", "\\'") + "', '" + _arbitro3.ShortName.Replace("'", "\\'") + "', '" +
                                _arbitro4.FullName.Replace("'", "\\'") + "', '" + _arbitro4.ShortName.Replace("'", "\\'") + "'])");
                            //_auxiliar2.FullName + "', '" + _auxiliar2.ShortName + "'])");
                        //}
                        //else
                        //{
                        //    ipf[i].Envia("RefereesIN(['" + idioma[i].Officials + "', '" +
                        //        idioma[i].Referee + "', '" + _arbitro1.FullName.Replace("'", "\\'") + "', '" + _arbitro1.ShortName.Replace("'", "\\'") + "', '" + _arbitro1.Nacionalidad.Replace("'", "\\'") + "', '" +
                        //        idioma[i].Referee2 + "', '" +
                        //        _arbitro2.FullName + "', '" + _arbitro2.ShortName + "', '" +
                        //        _arbitro3.FullName + "', '" + _arbitro3.ShortName + "', '" +
                        //        _arbitro4.FullName + "', '" + _arbitro4.ShortName + "'])");
                        //    //_auxiliar2.FullName + "', '" + _auxiliar2.ShortName + "'])");
                        //}
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
        
        public int numReferees()
        {
            int n = 0;

            if (_arbitro2.FullName.Length > 0)
                n++;
            if (_arbitro3.FullName.Length > 0)
                n++;
            if (this._arbitro4.FullName.Length > 0)
                n++;
            //if (this._auxiliar2.FullName.Length > 0)
            //    n++;

            return n;
        }

    }
}
