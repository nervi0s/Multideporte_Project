using System.Collections.Generic;
using System;

namespace Futbol_Sala_Manager_App.Beans
{

    /**
     * Bean de datos de un Equipo
     */
    [Serializable]
    public class Equipo
    {

        public bool Local { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public string TeamCode { get; set; }

        public string Badge { get; set; }

        public System.Drawing.Color Color1 { get; set; }
        public System.Drawing.Color Color2 { get; set; }

        //public string Campo { get; set; }

        //public string Ciudad { get; set; }

        public int FaltasAcumuladas { get; set; }

        private List<Jugador> _jugadores;
        public List<Jugador> Jugadores
        {
            get { return _jugadores; }
            set { _jugadores = value; }
        }

        private List<Jugador> _banquillo;
        public List<Jugador> Banquillo
        {
            get { return _banquillo; }
            set { _banquillo = value; }
        }

        public Jugador Entrenador { get; set; }

        public Jugador Asistente { get; set; }


        #region ------- Datos Normales -------
        private List<Momento> _corners = new List<Momento>();
        public List<Momento> Corners
        {
            get { return _corners; }
            set { _corners = value; }
        }

        private List<Momento> _faltas = new List<Momento>();
        public List<Momento> Faltas
        {
            get { return _faltas; }
            set { _faltas = value; }
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

        private List<Momento> _cambios = new List<Momento>();
        public List<Momento> Cambios
        {
            get { return _cambios; }
            set { _cambios = value; }
        }
        #endregion

        #region ------- Datos Modificados -------
        public bool _modified = false;
        public int attemptsOnTargetModified;//
        public int attemptsModified;//
        public int cornersModified;//
        public int offsidesModified;
        public int foulsModified;//
        public int yellowCardsModified;//
        public int redCardsModified;//
        public int paradasModified;
        #endregion

        #region ------- Getters -------
        public int getCorners()
        {
            return _modified ? cornersModified : Corners.Count;
        }

        public int getFaltas()
        {
            return _modified ? foulsModified : Faltas.Count;
        }
        public int getFaltasAcumuladas()
        {
            return this.FaltasAcumuladas;
        }
        public int getGoles()
        {
            return Goles.Count;
        }
        public int getGolesPP()
        {
            return GolesPP.Count;
        }
        public int getGolesPenalty()
        {
            return GolesPenalty.Count;
        }

        public int getTirosAPuerta()
        {
            return _modified ? attemptsOnTargetModified : Tirosapuerta.Count;
        }

        public int getTirosFuera()
        {
            return Tirosfuera.Count;
        }
        public int getTirosCompuesto()
        {
            return _modified ? attemptsModified : (Tirosapuerta.Count + Tirosfuera.Count);
        }

        public int getParadas()
        {
            return _modified ? paradasModified : Paradas.Count;
        }

        public int getTAmarillas()
        {
            return _modified ? yellowCardsModified : TAmarillas.Count;
        }

        public int getTRojas()
        {
            return _modified ? redCardsModified : TRojas.Count;
        }

        public int getFuerasJuego()
        {
            return _modified ? offsidesModified : Fuerasdejuego.Count;
        }

        public int getCambios()
        {
            return Cambios.Count;
        }
        #endregion
    }
}
