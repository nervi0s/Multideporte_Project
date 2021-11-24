using System;
using System.Drawing;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;


namespace Futbol_Sala_Manager_App.Comandos
{
    [Serializable]
    public class LocalizadorCommand : ICommandShowable
    {
        public Localizador localizador { get; set; }
        private bool _visible;


        public LocalizadorCommand(Localizador l)
        {
            localizador = l;

            Reset();
        }
        
        public void Reset()
        {
            _visible = false;
        }

        public bool Show(InterfaceIPF[] ipf, IdiomaData[] idioma, int n)
        {
            // Si existe telefono se llama a las funciones PreMatch_2lineas_IN/OUT en lugar de PreMatchIN/OUT
            //string funName = (Prematch.Telefono == "" ? "PreMatch" : "PreMatch_2lineas_");

            if (!_visible)
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("LocalizadorIN(['" + localizador.Title.Replace("'", "\\'") + "', '" + localizador.TextoLocalizador.Replace("'", "\\'") + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("LocalizadorOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return localizador.Title + "\n" + localizador.TextoLocalizador;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
