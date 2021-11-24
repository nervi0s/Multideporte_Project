using System;

namespace Futbol_Sala_Manager_App.Beans
{

    /**
     * Bean de datos de un Countdown
     */
    [Serializable]
    public class Countdown
    {
        public int Hora { get; set; }

        public int Minutos { get; set; }

        public int Desfase { get; set; }

        public string Referencia { get; set; }
    }
}
