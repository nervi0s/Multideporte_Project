using System;
using System.Collections.Generic;

namespace Balonmano_Manager_App.Beans
{

    /**
     * Bean de datos de un Jugador
     */
    [Serializable]
    public class Jugador
    {
        public const int Portero = 1;
        public const int Extremo = 2;
        public const int ExtremoDerecho = 3;
        public const int ExtremoIzquierdo = 4;
        public const int Lateral = 5;
        public const int LateralDerecho = 6;
        public const int LateralIzquierdo = 7;
        public const int Central = 8;
        public const int Pivote = 9;


        public Equipo Equipo { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string RutaFoto { get; set; }

        public int Dorsal_Interno { get; set; }

        public int Number { get; set; }

        public int Posicion { get; set; }

        public int Goals { get; set; }

        public int Matches { get; set; }
        
        public bool Capitan { get; set; }

        public bool SancionSiAmarilla { get; set; }

        public string PosX { get; set; }

        public string PosY { get; set; }

        public int exclusiones_Totales = 0;

        
        public int getExclusiones_Totales()
        {
            return exclusiones_Totales;
        }

        
        // Goles de penalty (goles de 7 metros)
        private List<Momento> _golesPenalty = new List<Momento>();
        public List<Momento> GolesPenalty
        {
            get { return _golesPenalty; }
            set { _golesPenalty = value; }
        }

        // Goles
        private List<Momento> _goles = new List<Momento>();
        public List<Momento> Goles
        {
            get { return _goles; }
            set { _goles = value; }
        }
                          
        // Tiros a puerta
        private List<Momento> _tirosapuerta = new List<Momento>();
        public List<Momento> Tirosapuerta
        {
            get { return _tirosapuerta; }
            set { _tirosapuerta = value; }
        }

        // Tiros fuera
        private List<Momento> _tirosfuera = new List<Momento>();
        public List<Momento> Tirosfuera
        {
            get { return _tirosfuera; }
            set { _tirosfuera = value; }
        }

        // Tiros totales
        private List<Momento> _fieldThrowsTotal = new List<Momento>();
        public List<Momento> FieldThrowsTotal
        {
            get { return _fieldThrowsTotal; }
            set { _fieldThrowsTotal = value; }
        }

        // Tiros de campo
        private List<Momento> _fieldThrowsComplete = new List<Momento>();
        public List<Momento> FieldThrowsComplete
        {
            get { return _fieldThrowsComplete; }
            set { _fieldThrowsComplete = value; }
        }
        public int _fieldThrowsPercentage { get; set; }

        // Tiros de siete metros
        private List<Momento> _sevenMthrowsTotal = new List<Momento>();
        public List<Momento> SevenMthrowsTotal
        {
            get { return _sevenMthrowsTotal; }
            set { _sevenMthrowsTotal = value; }
        }

        // Tiros de 7 metros conseguidos
        private List<Momento> _sevenMthrowsComplete = new List<Momento>();
        public List<Momento> SevenMthrowsComplete
        {
            get { return _sevenMthrowsComplete; }
            set { _sevenMthrowsComplete = value; }
        }
        public int _sevenMThrowsPercentage { get; set; }                                  

        // Tarjetas amarillas
        private List<Momento> _tAmarillas = new List<Momento>();
        public List<Momento> TAmarillas
        {
            get { return _tAmarillas; }
            set { _tAmarillas = value; }
        }

        // Tarjetas rojas
        private List<Momento> _tRojas = new List<Momento>();
        public List<Momento> TRojas
        {
            get { return _tRojas; }
            set { _tRojas = value; }
        }

        // Tarjetas azules
        private List<Momento> _tAzules = new List<Momento>();
        public List<Momento> TAzules
        {
            get { return _tAzules; }
            set { _tAzules = value; }
        }

        // Paradas
        private List<Momento> _paradas = new List<Momento>();
        public List<Momento> Paradas
        {
            get { return _paradas; }
            set { _paradas = value; }
        }

        // Paradas de tiros
        private List<Momento> _paradasTiro = new List<Momento>();
        public List<Momento> ParadasTiro
        {
            get { return _paradasTiro; }
            set { _paradasTiro = value; }
        }

        // Blocajes
        private List<Momento> _blocajes = new List<Momento>();
        public List<Momento> Blocajes
        {
            get { return _blocajes; }
            set { _blocajes = value; }
        }

        // Ataques completados (goles de contraataque)
        private List<Momento> _attacksComplete = new List<Momento>();
        public List<Momento> AttacksComplete
        {
            get { return _attacksComplete; }
            set { _attacksComplete = value; }
        }

        // Ataques totales
        private List<Momento> _attacksTotal = new List<Momento>();
        public List<Momento> AttacksTotal
        {
            get { return _attacksTotal; }
            set { _attacksTotal = value; }
        }
        public int _attacksPercentage { get; set; }

        // Contraataques
        private List<Momento> _fastBreaks = new List<Momento>();
        public List<Momento> Fastbreaks
        {
            get { return _fastBreaks; }
            set { _fastBreaks = value; }
        }

        // Pérdidas
        private List<Momento> _turnovers = new List<Momento>();
        public List<Momento> Turnovers
        {
            get { return _turnovers; }
            set { _turnovers = value; }
        }

        // Exclusiones
        private List<Momento> _exclusion = new List<Momento>();
        public List<Momento> Exclusion
        {
            get { return _exclusion; }
            set { _exclusion = value; }
        }
               
        public int setPercentage(int completados, int total, int _dataPercentage)
        {
            return _dataPercentage = (completados / total) * 100;
        }
        
        public int getGoles()
        {
            return Goles.Count;
        }

        public int getParadasTiro()
        {
            return ParadasTiro.Count;
        }

        public int getBlocajes()
        {
            return Blocajes.Count;
        }
    }
}
