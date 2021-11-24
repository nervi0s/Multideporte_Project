using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    public class WeatherCommand : ICommandShowable
    {
        public Weather weather { get; set; }
        private bool _visible;


        public WeatherCommand(Weather we)
        {
            weather = we;

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
                        ipf[i].Envia("WeatherIN(['" + weather.Header.Replace("'", "\\'") + "', '" + weather.Temperatura + "', '" + weather.Humedad + "', '" + weather.Wind + "', '" + weather.Tiempo + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("WeatherOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return weather.Header + "\n" + weather.Temperatura + " " + weather.Humedad + " " + weather.Wind + " " + weather.Tiempo;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
