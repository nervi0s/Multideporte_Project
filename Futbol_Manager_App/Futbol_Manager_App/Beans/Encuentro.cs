using System;

namespace Futbol_Manager_App.Beans
{

    /**
     * Bean de datos de un Encuentro
     */
    [Serializable]
    public class Encuentro
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public bool Ida { get; set; }
        public string NombreCompeticion { get; set; }
        public string NombrePabellon { get; set; }
        public string Ciudad { get; set; }
        public string Fecha { get; set; }
    }
}
