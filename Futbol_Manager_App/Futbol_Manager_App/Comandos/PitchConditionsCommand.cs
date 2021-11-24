using System;
using System.Drawing;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Comandos
{
    [Serializable]
    class PitchConditionsCommand : ICommandShowable
    {
        public PitchConditions pitchConditions { get; set; }
        private bool _visible;


        public PitchConditionsCommand(PitchConditions pc)
        {
            pitchConditions = pc;

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
                        ipf[i].Envia("PitchConditionsIN(['" + pitchConditions.Title.Replace("'", "\\'") + "', '" + pitchConditions.Estadio.Replace("'", "\\'") + "', '" + pitchConditions.Clima + "', '" + pitchConditions.Temperatura + "', '" + pitchConditions.Humedad + "', '" + pitchConditions.Viento + "'])");
                }
                _visible = true;
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    if (Program.EstaActivado(i))
                        ipf[i].Envia("PitchConditionsOUT()");
                }
                _visible = false;
            }
            return _visible;
        }

        override public string ToString()
        {
            return pitchConditions.Title + "\n" + pitchConditions.Estadio + " " + pitchConditions.Clima + " " + pitchConditions.Temperatura + " " + pitchConditions.Humedad + " " + pitchConditions.Viento;
        }

        public Color GetColor()
        {
            return Color.SlateGray;
        }
    }
}
