using System;
using System.Collections.Generic;

namespace Futbol_Manager_App.Beans
{

    /**
     * Bean de datos de un Jugador
     */
    [Serializable]
    public class Jugador
    {
        public const int Asistente = -1;
        public const int Entrenador = 0;
        public const int Portero = 1;
        public const int Defensa = 2;
        public const int Centrocampista = 3;
        public const int Delantero = 4;


        public Equipo Equipo { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string RutaFoto { get; set; }

        public int Number { get; set; }

        public int Posicion { get; set; }

        public int Goals { get; set; }

        public int Matches { get; set; }
        
        public bool Capitan { get; set; }

        public bool SancionSiAmarilla { get; set; }

        public string PosX { get; set; }

        public string PosY { get; set; }

        private List<Momento> _faltasRecibidas = new List<Momento>();
        public List<Momento> FaltasRecibidas
        {
            get { return _faltasRecibidas; }
            set { _faltasRecibidas = value; }
        }

        private List<Momento> _faltasCometidas = new List<Momento>();
        public List<Momento> FaltasCometidas
        {
            get { return _faltasCometidas; }
            set { _faltasCometidas = value; }
        }

        private List<Momento> _goles = new List<Momento>();
        public List<Momento> Goles
        {
            get { return _goles; }
            set { _goles = value; }
        }

        private List<Momento> _golesPP = new List<Momento>();
        public List<Momento> GolesPP
        {
            get { return _golesPP; }
            set { _golesPP = value; }
        }

        private List<Momento> _golesPenalty = new List<Momento>();
        public List<Momento> GolesPenalty
        {
            get { return _golesPenalty; }
            set { _golesPenalty = value; }
        }

        private List<Momento> _tirosapuerta = new List<Momento>();
        public List<Momento> Tirosapuerta
        {
            get { return _tirosapuerta; }
            set { _tirosapuerta = value; }
        }

        private List<Momento> _tirosfuera = new List<Momento>();
        public List<Momento> Tirosfuera
        {
            get { return _tirosfuera; }
            set { _tirosfuera = value; }
        }

        private List<Momento> _paradas = new List<Momento>();
        public List<Momento> Paradas
        {
            get { return _paradas; }
            set { _paradas = value; }
        }

        private List<Momento> _tAmarillas = new List<Momento>();
        public List<Momento> TAmarillas
        {
            get { return _tAmarillas; }
            set { _tAmarillas = value; }
        }

        private List<Momento> _tRojas = new List<Momento>();
        public List<Momento> TRojas
        {
            get { return _tRojas; }
            set { _tRojas = value; }
        }

        private List<Momento> _fuerasdejuego = new List<Momento>();
        public List<Momento> Fuerasdejuego
        {
            get { return _fuerasdejuego; }
            set { _fuerasdejuego = value; }
        }
    }
}
