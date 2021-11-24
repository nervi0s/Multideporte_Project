using System;

namespace Balonmano_Manager_App.Beans
{

    /**
     * Bean de datos de un Prematch
     */
    [Serializable]
    public class EndToEnd
    {
        public EndToEnd(string header, string linea)
        {
            Linea1 = linea;
            Header = header;
        }

        public string Header { get; set; }

        public string Linea1 { get; set; }
    }
}
