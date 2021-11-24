using System;
using System.Collections.Generic;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Comandos;

namespace Balonmano_Manager_App.Persistencia
{

    /**
     * Bean que contiene todos los datos de un Encuentro
     */
    [Serializable]
    public class EncuentroData
    {
        public Encuentro DatosEncuentro { get; set; }

        public Posesion Posesion { get; set; }

        public Equipo EquipoL { get; set; }

        public Equipo EquipoV { get; set; }

        private List<ICommand> _historial;
        public List<ICommand> Historial
        {
            get { return _historial; }
            set { _historial = value; }
        }

        private List<ICommand> _arbitros;
        public List<ICommand> Arbitros
        {
            get { return _arbitros; }
            set { _arbitros = value; }
        }

        private List<ICommand> _comentaristas;
        public List<ICommand> Comentaristas
        {
            get { return _comentaristas; }
            set { _comentaristas = value; }
        }

        public string ResultadoIda { get; set; }

        /**
         * Constructor
         */
        public EncuentroData()
        {
            Historial = new List<ICommand>();
            Posesion = new Posesion();
        }

    }
}