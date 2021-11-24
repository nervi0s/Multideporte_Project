using System;
using System.Drawing;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class DrinksBreakCommand : ICommandShowable
    {               
        private bool _visible;
        
        public DrinksBreakCommand()
        {
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
                        ipf[i].Envia("DrinksBreakIN(['" + idioma[i].DrinksBreak + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("DrinksBreakOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return "Drinks Break";
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }


    }
}