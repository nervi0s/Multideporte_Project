using System;

namespace Futbol_Manager_App.Beans
{
    [Serializable]
    public class PitchConditions
    {
        public PitchConditions(string title, string stadium, string clima, string temperatura, string humedad, string viento)
        {
            Title = title;
            Estadio = stadium;
            Temperatura = temperatura;
            Humedad = humedad;
            Clima = clima;
            Viento = viento;
        }
        public string Title { get; set; }

        public string Estadio { get; set; }

        public string Temperatura { get; set; }

        public string Humedad { get; set; }

        public string Clima { get; set; }

        public string Viento { get; set; }
    }
}
