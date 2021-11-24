using System;
using Futbol_Sala_Manager_App.Comandos;
using System.Collections.Generic;

namespace Futbol_Sala_Manager_App.Persistencia
{

    /**
     * Bean de datos con los elementos introducidos por el usuario y que son persistentes
     */
    [Serializable]
    public class PlantillasData
    {

        private List<ICommand> _plantillas;
        public List<ICommand> Plantillas
        {
            get { return _plantillas; }
            set { _plantillas = value; }
        }

        private List<ICommand> _localizador;
        public List<ICommand> Localizador
        {
            get { return _localizador; }
            set { _localizador = value; }
        }

        private List<ICommand> _perfilJugador;
        public List<ICommand> PerfilJugador
        {
            get { return _perfilJugador; }
            set { _perfilJugador = value; }
        }

        private List<ICommand> _clasificacion;
        public List<ICommand> Clasificacion
        {
            get { return _clasificacion; }
            set { _clasificacion = value; }
        }

        private List<ICommand> _enfrentamientos;
        public List<ICommand> Enfrentamientos
        {
            get { return _enfrentamientos; }
            set { _enfrentamientos = value; }
        }      

        private List<ICommand> _emergencyCaptions;
        public List<ICommand> emergencyCaptions
        {
            get { return _emergencyCaptions; }
            set { _emergencyCaptions = value; }
        }

        private List<ICommand> _rachas;
        public List<ICommand> Rachas
        {
            get { return _rachas; }
            set { _rachas = value; }
        }


        /**
         * Constructor
         */
        public PlantillasData()
        {
            Plantillas = new List<ICommand>();           
            emergencyCaptions = new List<ICommand>();
            Localizador = new List<ICommand>();
            PerfilJugador = new List<ICommand>();
            Clasificacion = new List<ICommand>();
            Rachas = new List<ICommand>();
        }

    }
}
