using System.Collections.Generic;
using System;

namespace Balonmano_Manager_App.Beans
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

        private List<Jugador> _dorsales;
        public List<Jugador> Dorsales
        {
            get { return _dorsales; }
            set { _dorsales = value; }
        }

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

        public Jugador EntrenadorAsistente { get; set; }



        #region ------- Datos Normales -------

        //private List<Momento> _corners = new List<Momento>();
        //public List<Momento> Corners
        //{
        //    get { return _corners; }
        //    set { _corners = value; }
        //}

        //private List<Momento> _faltasRecibidas = new List<Momento>();
        //public List<Momento> FaltasRecibidas
        //{
        //    get { return _faltasRecibidas; }
        //    set { _faltasRecibidas = value; }
        //}

        //private List<Momento> _faltasCometidas = new List<Momento>();
        //public List<Momento> FaltasCometidas
        //{
        //    get { return _faltasCometidas; }
        //    set { _faltasCometidas = value; }
        //}                

        //private List<Momento> _golesPP = new List<Momento>();
        //public List<Momento> GolesPP
        //{
        //    get { return _golesPP; }
        //    set { _golesPP = value; }
        //}

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

        private List<Momento> _tAzules = new List<Momento>();
        public List<Momento> TAzules
        {
            get { return _tAzules; }
            set { _tAzules = value; }
        }

        //private List<Momento> _fuerasdejuego = new List<Momento>();
        //public List<Momento> Fuerasdejuego
        //{
        //    get { return _fuerasdejuego; }
        //    set { _fuerasdejuego = value; }
        //}

        //private List<Momento> _cambios = new List<Momento>();
        //public List<Momento> Cambios
        //{
        //    get { return _cambios; }
        //    set { _cambios = value; }
        //}                                 


        // Tiros
        private List<Momento> _fieldThrowsTotal = new List<Momento>();
        public List<Momento> FieldThrowsTotal
        {
            get { return _fieldThrowsTotal; }
            set { _fieldThrowsTotal = value; }
        }

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

        private List<Momento> _sevenMthrowsComplete = new List<Momento>();
        public List<Momento> SevenMthrowsComplete
        {
            get { return _sevenMthrowsComplete; }
            set { _sevenMthrowsComplete = value; }
        }
        public int _sevenMThrowsPercentage { get; set; }


        // Goles
        private List<Momento> _goles = new List<Momento>();
        public List<Momento> Goles
        {
            get { return _goles; }
            set { _goles = value; }
        }

        // Goles desde 7M
        private List<Momento> _goles7M = new List<Momento>();
        public List<Momento> Goles7M
        {
            get { return _goles7M; }
            set { _goles7M = value; }
        }

        // Goles de Contraataque
        private List<Momento> _golesContraataque = new List<Momento>();
        public List<Momento> GolesContraataque
        {
            get { return _golesContraataque; }
            set { _golesContraataque = value; }
        }

        // Tiros
        private List<Momento> _tiros = new List<Momento>();
        public List<Momento> Tiros
        {
            get { return _tiros; }
            set { _tiros = value; }
        }

        // Tiros desde 7M
        private List<Momento> _tiros7M = new List<Momento>();
        public List<Momento> Tiros7M
        {
            get { return _tiros7M; }
            set { _tiros7M = value; }
        }

        // Tiros de Contraataque
        private List<Momento> _tirosContraataque = new List<Momento>();
        public List<Momento> TirosContraataque
        {
            get { return _tirosContraataque; }
            set { _tirosContraataque = value; }
        }

        // Paradas de tiros
        private List<Momento> _paradasTiro = new List<Momento>();
        public List<Momento> ParadasTiro
        {
            get { return _paradasTiro; }
            set { _paradasTiro = value; }
        }

        // Perdidas de tiros
        private List<Momento> _perdidasTiro = new List<Momento>();
        public List<Momento> PerdidasTiro
        {
            get { return _perdidasTiro; }
            set { _perdidasTiro = value; }
        }

        // Ataques
        private List<Momento> _ataques = new List<Momento>();
        public List<Momento> Ataques
        {
            get { return _ataques; }
            set { _ataques = value; }
        }

        // Blocajes
        private List<Momento> _blocajes = new List<Momento>();
        public List<Momento> Blocajes
        {
            get { return _blocajes; }
            set { _blocajes = value; }
        }


        public void setPercentage(int completados, int total, string _dataPercentage)
        {

            Console.WriteLine("setPercentage " + _dataPercentage + ", " + completados + ", " + total);

            switch (_dataPercentage)
            {

                case "fieldPercentage":
                    _fieldThrowsPercentage = (completados / total) * 100;
                    break;

                case "sevenPercentage":
                    _sevenMThrowsPercentage = (completados / total) * 100;
                    break;

                case "attackPercentage":
                    _attacksPercentage = (completados / total) * 100;
                    break;

                case "fastbreakPercentage":
                    break;

                default:
                    Console.WriteLine("Error at setPecentage, _dataPercentage invalid");
                   break;

            }

        }

               
        // Cesion de balon
        private List<Momento> _turnovers = new List<Momento>();
        public List<Momento> Turnovers
        {
            get { return _turnovers; }
            set { _turnovers = value; }
        }


        // Attacks
        private List<Momento> _attacksComplete = new List<Momento>();
        public List<Momento> AttacksComplete
        {
            get { return _attacksComplete; }
            set { _attacksComplete = value; }
        }

        private List<Momento> _attacksTotal = new List<Momento>();
        public List<Momento> AttacksTotal
        {
            get { return _attacksTotal; }
            set { _attacksTotal = value; }
        }

        public int _attacksPercentage { get; set; }


        //// Faltas de equipo
        //private List<Momento> _faults = new List<Momento>();
        //public List<Momento> Faults
        //{
        //    get { return _faults; }
        //    set { _faults = value; }
        //}


        // Contraataque
        private List<Momento> _fastBreaks = new List<Momento>();
        public List<Momento> Fastbreaks
        {
            get { return _fastBreaks; }
            set { _fastBreaks = value; }
        }

        // Exclusion
        private List<Momento> _exclusion = new List<Momento>();
        public List<Momento> Exclusion
        {
            get { return _exclusion; }
            set { _exclusion = value; }
        }


        //// Timeout
        //private List<Momento> _timeout = new List<Momento>();
        //public List<Momento> Timeout
        //{
        //    get { return _timeout; }
        //    set { _timeout = value; }
        //}






        //// Tarjetas 
        //private List<Momento> _tarjetasAzules = new List<Momento>();
        //public List<Momento> TarjetasAzules
        //{
        //    get { return _tarjetasAzules; }
        //    set { _tarjetasAzules = value; }
        //}

        //private List<Momento> _tarjetasAmarillas = new List<Momento>();
        //public List<Momento> TarjetasAmarillas
        //{
        //    get { return _tarjetasAmarillas; }
        //    set { _tarjetasAmarillas = value; }
        //}


        //private List<Momento> _tarjetasRojas = new List<Momento>();
        //public List<Momento> TarjetasRojas
        //{
        //    get { return _tarjetasRojas; }
        //    set { _tarjetasRojas = value; }
        //}



        


        #endregion


        private List<Momento> _penalti = new List<Momento>();
        public List<Momento> Penalti
        {
            get { return _penalti; }
            set { _penalti = value; }
        }
      


        #region ------- Datos Modificados -------
        public bool _modified = false;
        public int attemptsOnTargetModified;//
        public int totalAttemptsModified;//
        public int cornersModified;//
        public int offsidesModified;
        public int passesModified;
        public int completedModified;
        public int foulsCommittedModified;//
        public int foulsReceivedModified;//
        public int yellowCardsModified;//
        public int redCardsModified;//
        public int paradasModified;
        public int exclusiones_Totales = 0;

        // Handball Statistics
        public int attacksCompletedModified;
        public int attacksTotalModified;
        public int attacksPercentageModified;

        //public int fastbreaksCompleteModified;
        public int fastbreaksTotalModified;
        //public int fastbreaksPercentageModified;

        public int turnoverModified;    // Total, como fastbreak

        public int fieldthrowsCompleteModified;
        public int fieldthrowsTotalModified;
        public int fieldthrowsPercentageModified;

        public int sevenMthrowsCompleteModified;
        public int sevenMthrowsTotalModified;
        public int sevenMthrowsPercentageModified;

        //
        public int blueCardsModified;
        public int handball_yellowCardsModified;
        public int handball_redCardsModified;

        //
        public int handball_timeoutModified;


        public int penaltiModified;


        #endregion

        #region ------- Getters -------
        //public int getCorners()
        //{
        //    return _modified ? cornersModified : Corners.Count;
        //}

        //public int getfaltasRecibidas()
        //{
        //    return _modified ? foulsReceivedModified : FaltasRecibidas.Count;
        //}

        //public int getFaltasCometidas()
        //{
        //    return _modified ? foulsCommittedModified : FaltasCometidas.Count;
        //}
        
        //public int getGolesPP()
        //{
        //    return GolesPP.Count;
        //}
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
            return _modified ? totalAttemptsModified : (Tirosapuerta.Count + Tirosfuera.Count);
        }

        public int getParadas()
        {
            return _modified ? paradasModified : Paradas.Count;
        }

        public int getParadasTiro()
        {
            return ParadasTiro.Count;
        }

        public int getPerdidasTiro()
        {
            return PerdidasTiro.Count;
        }

        public int getAtaques()
        {
            return Ataques.Count;
        }

        public int getBlocajes()
        {
            return Blocajes.Count;
        }

        public int getTAmarillas()
        {
            return _modified ? yellowCardsModified : TAmarillas.Count;
        }

        public int getTRojas()
        {
            return _modified ? redCardsModified : TRojas.Count;
        }

        public int getTAzules()
        {
            return _modified ? blueCardsModified : TAzules.Count;
        }

        //public int getFuerasJuego()
        //{
        //    return _modified ? offsidesModified : Fuerasdejuego.Count;
        //}

        //public int getCambios()
        //{
        //    return Cambios.Count;
        //}

        //public int getPases()
        //{
        //    return passesModified;
        //}

        //public int getPasesCompletados()
        //{
        //    return completedModified;
        //}


        /////////////////////       HANDBALL        /////////////////////////////

        // Ataques ( Exito / Totales / Porcentaje)
        public int getAttacksComplete()
        {
            return _modified ? attacksCompletedModified : AttacksComplete.Count;
        }

        public int getAttacksTotal()
        {
            return _modified ? attacksTotalModified : AttacksTotal.Count;
        }

        public int getAttacksPercentage()
        {
            return _modified ? attacksPercentageModified : _attacksPercentage;
        }

        // Contraataques totales
        public int getFastbreaksTotal()
        {
            return _modified ? fastbreaksTotalModified : Fastbreaks.Count;
        }

        // Cesiones totales
        public int getTurnoversTotal()
        {
            return _modified ? turnoverModified : Turnovers.Count;
        }


        // Tiros (Exito / Totales / Porcentaje)
        // No se valoran los tiros de siete metros
        public int getFieldthrowsComplete()
        {
            // Aqui seria cosa de coger total de tiros 
            return _modified ? fieldthrowsCompleteModified : FieldThrowsComplete.Count;
        }

        public int getFieldthrowsTotal()
        {
            return _modified ? fieldthrowsTotalModified : FieldThrowsTotal.Count;
        }

        public int getFieldThrowsPercentage()
        {
            return _modified ? fieldthrowsPercentageModified : _fieldThrowsPercentage;
        }


        // Tiros de siete metros ( Exito / Totales / Porcentaje)
        public int getSevenMThrowsComplete()
        {
            // Aqui seria cosa de coger total de tiros 
            return _modified ? fieldthrowsCompleteModified : SevenMthrowsComplete.Count;
        }

        public int getSevenMThrowsTotal()
        {
            return _modified ? fieldthrowsTotalModified : SevenMthrowsTotal.Count;
        }

        public int getSevenMThrowsPercentage()
        {
            return _modified ? attacksPercentageModified : _sevenMThrowsPercentage;
        }


        //Exclusiones
        public int getExclusiones_Totales()
        {
            return exclusiones_Totales;
        }


        // Goles (Fieldthrows / 7 Meters)
        public int getGoles()
        {
            return Goles.Count;
        }

        public int getGoles7M()
        {
            return Goles7M.Count;
        }

        public int getGolesContraataque()
        {
            return GolesContraataque.Count;
        }

        public int getTiros()
        {
            return Tiros.Count;
        }

        public int getTiros7M()
        {
            return Tiros7M.Count;
        }

        public int getTirosContraataque()
        {
            return TirosContraataque.Count;
        }


        public int getGolesField()
        {
            return FieldThrowsComplete.Count;
        }

        public int getGolesSeven()
        {
            return SevenMthrowsComplete.Count;
        }


        // Cesion de balon
        public int getTurnovers()
        {
            return _modified ? turnoverModified : Turnovers.Count;
        }


        // Tarjetas
        //public int getTarjetasAzules()
        //{
        //    return _modified ? blueCardsModified : TAzules.Count;
        //}

        //public int getTarjetasAmarillas()
        //{
        //    return _modified ? handball_yellowCardsModified : TAmarillas.Count;
        //}

        //public int getTarjetasRojas()
        //{
        //    return _modified ? handball_redCardsModified : TRojas.Count;
        //}



        //public int getTimeout()
        //{
        //    return _modified ? handball_timeoutModified : Timeout.Count;
        //}




        public int getPenalti()
        {
            return _modified ? penaltiModified : Penalti.Count;
        }

        



        #endregion
    }
}
