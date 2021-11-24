using System;

namespace Futbol_Sala_Manager_App.Beans
{
    [Serializable]
    public class Enfrentamientos
    {
        [Serializable]
        public class Partido
        {
            public string equipoL;
            public string escudoL;
            public string equipoV;
            public string escudoV;
            public string info;
        }

        public string titulo;
        public string division;
        public Partido[] partidos;
    }
}
