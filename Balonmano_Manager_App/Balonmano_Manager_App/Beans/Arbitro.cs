using System;

namespace Balonmano_Manager_App.Beans
{
    /**
     * Bean de datos de un Árbitro
     */
    [Serializable]
    public class Arbitro
    {
        public const int Arbitro1 = 1;
        public const int Arbitro2 = 2;
      
        public string ShortName { get; set; }

        public string FullName { get; set; }

        public int Cargo { get; set; }

        public string Nacionalidad { get; set; }

        public string Colegio { get; set; }
    }
}
