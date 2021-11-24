using System;

namespace Futbol_Manager_App.Beans
{

    /**
     * Bean de datos de un Prematch
     */
    [Serializable]
    public class EndOfInterview
    {
        public EndOfInterview(string l1, string l2, string l3)
        {
            Linea1 = l1;
            Linea2 = l2;
            Linea3 = l3;
        }       

        public string Linea1 { get; set; }

        public string Linea2 { get; set; }

        public string Linea3 { get; set; }
    }
}
