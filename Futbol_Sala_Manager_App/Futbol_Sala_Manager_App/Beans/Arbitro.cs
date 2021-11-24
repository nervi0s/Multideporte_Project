using System;

namespace Futbol_Sala_Manager_App.Beans
{

    /**
     * Bean de datos de un Árbitro
     */
    [Serializable]
    public class Arbitro
    {
        public const int arbitro1 = 1;
        public const int arbitro2 = 2;
        //public const int Linier2 = 3;
        //public const int CuartoArbitro = 4;
        //public const int Auxiliar1 = 5;
        //public const int Auxiliar2 = 6;


        public string ShortName { get; set; }

        public string FullName { get; set; }

        public int Cargo { get; set; }

        public string Nacionalidad { get; set; }

        public string Colegio { get; set; }

    }
}
