using System;
using Futbol_Manager_App.Comandos;
using System.Collections.Generic;

namespace Futbol_Manager_App.Persistencia
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

        private List<ICommand> _countdowns;
        public List<ICommand> Countdowns
        {
            get { return _countdowns; }
            set { _countdowns = value; }
        }

        private List<ICommand> _prematchs;
        public List<ICommand> Prematchs
        {
            get { return _prematchs; }
            set { _prematchs = value; }
        }


        private List<ICommand> _exchange;
        public List<ICommand> Exchange
        {
            get { return _exchange; }
            set { _exchange = value; }
        }


        private List<ICommand> _endToEnd;
        public List<ICommand> EndToEndTest
        {
            get { return _endToEnd; }
            set { _endToEnd = value; }
        }


        private List<ICommand> _weather;
        public List<ICommand> Weather
        {
            get { return _weather; }
            set { _weather = value; }
        }

        private List<ICommand> _pitchConditions;
        public List<ICommand> PitchConditions
        {
            get { return _pitchConditions; }
            set { _pitchConditions = value; }
        }

        private List<ICommand> _infoCrawler;
        public List<ICommand> InfoCrawler
        {
            get { return _infoCrawler; }
            set { _infoCrawler = value; }
        }

        private List<ICommand> _groupStanding;
        public List<ICommand> GroupStanding
        {
            get { return _groupStanding; }
            set { _groupStanding = value; }
        }

        private List<ICommand> _postInterview;
        public List<ICommand> postInterview
        {
            get { return _postInterview; }
            set { _postInterview = value; }
        }

        private List<ICommand> _endPostInterview;
        public List<ICommand> endPostInterview
        {
            get { return _endPostInterview; }
            set { _endPostInterview = value; }
        }

        private List<ICommand> _emergencyCaptions;
        public List<ICommand> emergencyCaptions
        {
            get { return _emergencyCaptions; }
            set { _emergencyCaptions = value; }
        }


        /**
         * Constructor
         */
        public PlantillasData()
        {
            Plantillas = new List<ICommand>();
            Countdowns = new List<ICommand>();
            Prematchs = new List<ICommand>();
            Exchange = new List<ICommand>();
            EndToEndTest = new List<ICommand>();
            Weather = new List<ICommand>();
            postInterview = new List<ICommand>();     
            endPostInterview = new List<ICommand>();
            emergencyCaptions = new List<ICommand>();
            PitchConditions = new List<ICommand>();
            InfoCrawler = new List<ICommand>();
        }

    }
}
