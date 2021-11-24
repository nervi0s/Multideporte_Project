using System;

namespace Balonmano_Manager_App.Beans
{
    [Serializable]
    public class GroupStanding
    {
        public string League;
        public string Group;
        public TeamInfo[] Teams;
        public MatchesInfo[] Matches;

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


        [Serializable]
        public class MatchesInfo
        {
            public string Equipo1;
            public string Result1;
            public string Equipo2;
            public string Result2;
        }
    }
}
