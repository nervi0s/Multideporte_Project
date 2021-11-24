using System;

namespace Futbol_Manager_App.Beans
{

    /**
     * Bean de datos de un Prematch
     */
    [Serializable]
    public class Exchange
    {
        public Exchange(string h, string horas)
        {
            Hora = horas;
            Header = h;
        }
        public string Hora { get; set; }

        public string Header { get; set; }
    }
}
