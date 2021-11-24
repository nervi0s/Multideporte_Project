using System;

namespace Futbol_Manager_App.Beans
{

    /**
     * Bean de datos de un Árbitro
     */
    [Serializable]
    public class Arbitro
    {
        public const int arbitro1 = 1;
        public const int arbitro2 = 2;
        public const int arbitro3 = 3;
        public const int arbitro4 = 4;
    

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public int Cargo { get; set; }

        public string Colegio { get; set; }

        public string Nacionalidad { get; set; }


    }
}
