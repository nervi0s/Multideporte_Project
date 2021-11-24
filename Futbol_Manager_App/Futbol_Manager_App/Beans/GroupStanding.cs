using System;

namespace Futbol_Manager_App.Beans
{
    [Serializable]
    public class GroupStanding
    {
        public string League;
        public string Group;
        public TeamInfo[] Teams;

        [Serializable]
        public class TeamInfo
        {
            public string Equipo;
            public string pts;
            public string p;
            public string w;
            public string d;
            public string l;
            public string gf;
            public string ga;
        }
    }
}
