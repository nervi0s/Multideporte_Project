using System;

namespace Futbol_Sala_Manager_App.Beans
{
    [Serializable]
    public class Rachas
    {
        [Serializable]
        public class Partido
        {
            public string info;
            public string equipoLocal;
            public string puntosLocal;
            public string puntosVisitante;
            public string equipoVisitante;
            public string puntosInfo;
        }

        public string photoPath;
        public string equipo;
        public Partido[] partidos;
    }
}
