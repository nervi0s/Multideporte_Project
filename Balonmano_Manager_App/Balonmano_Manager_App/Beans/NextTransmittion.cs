using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Balonmano_Manager_App.Beans
{
    [Serializable]
    public class NextTransmission
    {
        public NextTransmission(string grupo, string equipo1, string equipo2, string lugar, string hora)
        {
            Grupo = grupo;
            Equipo1 = equipo1;
            Equipo2 = equipo2;
            Lugar = lugar;
            Hora = hora;
        }
        public string Grupo { get; set; }

        public string Equipo1 { get; set; }

        public string Equipo2 { get; set; }

        public string Lugar { get; set; }

        public string Hora { get; set; }
    }
}
