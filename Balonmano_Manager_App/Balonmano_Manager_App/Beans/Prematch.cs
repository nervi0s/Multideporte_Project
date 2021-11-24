using System;

namespace Balonmano_Manager_App.Beans
{

    /**
     * Bean de datos de un Prematch
     */
    [Serializable]
    public class Prematch
    {
        public string Hora { get; set; }

        public string Referencia { get; set; }

        public string Broadcaster { get; set; }

        public string Telefono { get; set; }

        public string Tipo { get; set; }

    }
}
