using System;

namespace Futbol_Sala_Manager_App.Beans
{
    [Serializable]
    public class Clasificacion
    {
        public string division;
        public Equipo[] equipos;

        [Serializable]
        public class Equipo
        {
            public string equipo;

            public string pt;
            public string pj;
            public string pg;
            public string pe;
            public string pp;
            public string gf;
            public string gc;
        }
        
    }
}
