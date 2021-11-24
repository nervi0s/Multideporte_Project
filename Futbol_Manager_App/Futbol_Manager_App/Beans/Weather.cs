using System;

namespace Futbol_Manager_App.Beans
{

    [Serializable]
    public class Weather
    {
        public Weather(string header, string temperatura, string humedad, string wind, string tiempo)
        {
            Header= header;
            Temperatura = temperatura;
            Humedad = humedad;
            Wind = wind;
            Tiempo = tiempo;
        }
        public string Header { get; set; }

        public string Temperatura{ get; set; }

        public string Humedad { get; set; }

        public string Wind{ get; set; }

        public string Tiempo { get; set; }

    }
}
