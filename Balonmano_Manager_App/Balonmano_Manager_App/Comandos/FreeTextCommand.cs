using System;
using System.Drawing;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Comandos
{
    [Serializable]
    public class FreeTextCommand : ICommandShowable
    {
        public string Linea1 { get; set; }
        public string Linea2 { get; set; }
        public string Linea3 { get; set; }
        public string Linea4 { get; set; }
        public int Lineas { get; set; }
        public bool _si_es_localizador;
        private bool _visible;


        public FreeTextCommand(bool si_localizador, int numLineas, string linea1, string linea2, string linea3, string linea4)
        {
            Linea1 = linea1;
            Linea2 = linea2;
            Linea3 = linea3;
            Linea4 = linea4;
            Lineas = numLineas;
            _si_es_localizador = si_localizador;
            Reset();
        }

        public FreeTextCommand(int numLineas, string linea1, string linea2, string linea3, string linea4)
        {
            Linea1 = linea1;
            Linea2 = linea2;
            Linea3 = linea3;
            Linea4 = linea4;
            Lineas = numLineas;
            Reset();
        }

        public FreeTextCommand(string linea1, string linea2, string linea3)
        {
            Linea1 = linea1;
            Linea2 = linea2;
            Linea3 = linea3;

            Reset();
        }
        public FreeTextCommand(string linea1, string linea2)
        {
            Linea1 = linea1;
            Linea2 = linea2;
            Linea3 = "";

            Reset();
        }
        public FreeTextCommand(string linea1)
        {
            Linea1 = linea1;
            Linea2 = "";
            Linea3 = "";

            Reset();
        }

        public string getNameCommand()
        {
            return "FreeTextCommand";
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
                        //if (_si_es_localizador)
                        //    ipf[i].Envia("FreeTextIN([1, '" + Lineas + "', '" + Linea1.Replace("'", "\\'") + "', '" + Linea2.Replace("'", "\\'") + "','" + Linea3.Replace("'", "\\'") + "','" + Linea4.Replace("'", "\\'") + "'])");
                        //else
                            ipf[i].Envia("FreeTextIN(['" + Lineas + "', '" + Linea1.Replace("'", "\\'") + "', '" + Linea2.Replace("'", "\\'") + "','" + Linea3.Replace("'", "\\'") + "','" + Linea4.Replace("'", "\\'") + "'])");
                    }
                        
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("FreeTextOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            switch (Lineas)
            {
                case 1:
                    return Linea1;
                case 2:
                    return Linea1 + "\n" + Linea2 ;
                case 3:
                    return Linea1 + "\n" + Linea2 + "\n" + Linea3;
                case 4:
                    return Linea1 + "\n" + Linea2 + "\n" + Linea3 + "\n" + Linea4;
                default:
                    return Linea1;
                
            }
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
