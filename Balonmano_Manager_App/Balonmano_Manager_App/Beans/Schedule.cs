using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Balonmano_Manager_App.Beans
{
    [Serializable]
    public class Schedule
    {
        public SchedulesInfo[] Schedules;

        [Serializable]
        public class SchedulesInfo
        {
            public string Equipo1;
            public string Equipo2;
            public string Info;
        }
    }
}
