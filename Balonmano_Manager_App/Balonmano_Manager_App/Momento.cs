using System;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App
{

    /*
     * Representación de un momento para Balonmano
     * Viene dado por la la parte del juego (Primera, Segunda, 1º Prórroga - parte 1 y 2,
     * 2º Prórroga - parte 1 y 2, Penaltis) y dentro de a misma el minuto concreto (con 
     * precisión de segundos)
     * 
     */
    [Serializable]
    public class Momento
    {
        public const int Extra = 0;
        public const int IniParte1 = 1;
        public const int FinParte1 = 2;
        public const int IniParte2 = 3;
        public const int FinParte2 = 4;
        public const int IniProrroga1_parte1 = 5;
        public const int FinProrroga1_parte1 = 6;
        public const int IniProrroga1_parte2 = 7;
        public const int FinProrroga1_parte2 = 8;
        public const int IniProrroga2_parte1 = 9;
        public const int FinProrroga2_parte1 = 10;
        public const int IniProrroga2_parte2 = 11;
        public const int FinProrroga2_parte2 = 12;
        public const int Penaltis = 13;
        public const int FinPenaltis = 14;
        public const int FinPartido = 15;
        public const int FinPartidoDescanso = 16;
        public const int FinPartidoDescansoAux = 17;

        // 0: 1a parte       00 - 30  (30)
        // 1: 2a parte       30 - 60  (30)
        // 2: 1a prorroga 1  60 - 65  (5)
        // 3: 1a prorroga 2  65 - 70  (5)
        // 2: 2a prorroga 1  70 - 75  (5)
        // 3: 2a prorroga 2  75 - 80  (5)

        private static int[] minuto = {0, 0, 30, 0, 30, 0, 5, 0, 5, 0, 5, 0, 5, 80, 80, 80, 80, 80 }; //Añadido elemento 16 y 17 para evitar el out of Range en getFinalParte


        private int _parte;

        public int Parte
        {
            get { return _parte; }
            set { _parte = value; }
        }

        private int _segundoAbsoluto;
        public int SegundoAbsoluto
        {
            get { return _segundoAbsoluto; }
            set { _segundoAbsoluto = value; }
        }


        /**
         * Constructor
         */
        public Momento(int parte)
        {
            _parte = parte;
            _segundoAbsoluto = minuto[parte] * 60;
        }

        /**
         * Constructor
         */
        public Momento(Momento momento)
        {
            _parte = momento.Parte;
            _segundoAbsoluto = momento.SegundoAbsoluto;
        }


        /**
         * Constructor
         */
        public Momento(int segundo, Momento momento)
        {
            _parte = momento.Parte;
            _segundoAbsoluto = segundo;
        }

        /**
         * Reconfigura el objeto con los nuevos valores
         */
        public void Update(int parte, int minutos, int segundos)
        {
            _parte = parte;
            _segundoAbsoluto = minutos * 60 + segundos;
        }

        /**
         * Reconfigura el objeto con los nuevos valores
         */
        public void Update(int minutos, int segundos)
        {
            _segundoAbsoluto = (minuto[_parte] + minutos) * 60 + segundos;
        }

        /**
         * Devuelve el minuto actual
         */
        public int GetMinuto()
        {
            return (_segundoAbsoluto - (_segundoAbsoluto % 60)) / 60;
        }

        /**
         * Devuelve el segundo actual
         */
        public int GetSegundo()
        {
            return _segundoAbsoluto % 60;
        }


        /**
         * Devuelve el tiempo de la parte actual
         */
        public int GetSegundosParte(int parte)
        {
            return minuto[parte] * 60;
        }


        /*
         * Devuelve cadena que representa el tiempo para el Cronometro
         */
        public string GetTextoCrono(IdiomaData idioma, bool descanso)
        {
            return GetNombreParte(idioma, descanso) + "\n " + " *" + GetMinuto().ToString("00") + "' " + GetSegundo().ToString("00") + "''";
        }

        /*
         * Devuelve una cadena que representa el minuto de juego
         */
        public string GetMinutoJuego()
        {
            int m = Math.Min(GetMinuto() + 1, minuto[_parte + 1]);
            return m + "'";
        }

        /*
         * Devuelve los minutos de descuento transcurrido
         */
        public string GetMinutoDescuento()
        {
            int m;

            // Si estamos en un descanso no se devuelve descuento
            if (_parte % 2 != 0)
                m = 0;

            else // Calcula el descuento
                m = Math.Max(0, GetMinuto() + 1 - minuto[_parte + 1]);

            return m + "'";
        }

        /**
         * Devuelve el final (último minuto) de la parte en curso
         */
        public int GetFinalParte()
        {
            return minuto[_parte + 1];
        }

        /**
         * Devuelve el nombre de la parte en curso
         */
        public string GetNombreParte(IdiomaData idioma, bool descanso)
        {
            int selecctor;

            if (descanso)
            {
                selecctor = _parte - 1;
            }
            else
            {
                selecctor = _parte;
            }

            switch (selecctor)
            {
                case IniParte1:
                    return idioma.Parte1;
                case FinParte1:
                    return idioma.FinParte1;
                case IniParte2:
                    return idioma.Parte2;
                case FinParte2:
                    return idioma.FinParte2;
                case IniProrroga1_parte1:
                    return idioma.Prorroga1_parte1;
                case FinProrroga1_parte1:
                    return idioma.FinProrroga1_parte1;
                case IniProrroga1_parte2:
                    return idioma.Prorroga1_parte2;
                case FinProrroga1_parte2:
                    return idioma.FinProrroga1_parte2;
                case IniProrroga2_parte1:
                    return idioma.Prorroga2_parte1;
                case FinProrroga2_parte1:
                    return idioma.FinProrroga2_parte1;
                case IniProrroga2_parte2:
                    return idioma.Prorroga2_parte2;
                case FinProrroga2_parte2:
                    return idioma.FinProrroga2_parte2;
                case Penaltis:
                    return idioma.Penaltis;
                case FinPenaltis:
                    return idioma.FinPartido;
                case FinPartido:
                case FinPartidoDescanso:
                case FinPartidoDescansoAux:
                    return idioma.FinPartido;
                default:
                    return "";
            }
        }

        /**
         * Representación del minuto actual
         * Ejemplos:
         *   9'
         *   45'+2'
         */
        override public string ToString()
        {
            string m = GetMinutoJuego();
            string d = GetMinutoDescuento();
            d = (d.Equals("0'") ? "" : "+" + d);

            return m + d;
        }

        /**
        * Devuelde la parte
        */

        public int getParte()
        {
            return this._parte;
        }

        public bool EsDescanso()
        {
            return (this.Parte == 1 || this.Parte == 3 || this.Parte == 5);
        }

        public bool EsfinalPrimeraParte()
        {
            return this.Parte == 1;
        }

        public bool EsfinalPartido()
        {
            return this.Parte > 2;
        }
    }



    /**
    * Representación de un Momento
    * Especifica un momento concreto dentro del periodo de juego.
    * Viene dado por la la parte del juego (Primera, Segunda, 1ª Prórroga, 
    * 2ª Prorroga o Penaltis) y dentro de a misma el minuto concreto (con 
    * precisión de segundos)
    */
    public class Momento_futbol { 

        public const int IniParte1 = 0;
        public const int FinParte1 = 1;
        public const int IniParte2 = 2;
        public const int FinParte2 = 3;
        public const int IniProrroga1 = 4;
        public const int FinProrroga1 = 5;
        public const int IniProrroga2 = 6;
        public const int FinProrroga2 = 7;
        public const int Penaltis = 8;
        public const int FinPenaltis = 9;
        public const int FinPartido = 10;
 
        // 0: 1a parte        0 -  45  (45)
        // 1: 2a parte       45 -  90  (45)
        // 2: 1a prorroga    90 - 105  (15)
        // 3: 2a prorroga   105 - 120  (15)

        private static int[] minuto = { 0, 45, 45, 90, 90, 105, 105, 120, 120, 120, 120, 120 };


        private int _parte;
        public int Parte
        {
            get { return _parte; }
            set { _parte = value; }
        }

        private int _segundoAbsoluto;
        public int SegundoAbsoluto
        {
            get { return _segundoAbsoluto; }
            set { _segundoAbsoluto = value; }
        }


        /**
         * Constructor
         */
        public Momento_futbol(int parte)
        {
            _parte = parte;
            _segundoAbsoluto = minuto[parte] * 60;
        }

        /**
         * Constructor
         */
        public Momento_futbol(Momento_futbol momento)
        {
            _parte = momento.Parte;
            _segundoAbsoluto = momento.SegundoAbsoluto;
        }


        /**
         * Constructor
         */
        public Momento_futbol(int segundo, Momento_futbol momento)
        {
            _parte = momento.Parte;
            _segundoAbsoluto = segundo;
        }



        /**
         * Reconfigura el objeto con los nuevos valores
         */
        public void Update(int parte, int minutos, int segundos)
        {
            _parte = parte;
            _segundoAbsoluto = minutos * 60 + segundos;
        }

        /**
         * Reconfigura el objeto con los nuevos valores
         */
        public void Update(int minutos, int segundos)
        {
            _segundoAbsoluto = (minuto[_parte] + minutos) * 60 + segundos;
        }

        /**
         * Devuelve el minuto actual
         */
        public int GetMinuto()
        {
            return (_segundoAbsoluto - (_segundoAbsoluto % 60)) / 60;
        }

        /**
         * Devuelve el segundo actual
         */
        public int GetSegundo()
        {
            return _segundoAbsoluto % 60;
        }

        /*
         * Devuelve cadena que representa el tiempo para el Cronometro
         */
        public string GetTextoCrono(IdiomaData idioma)
        {
            return GetNombreParte(idioma) + "\n " + GetMinuto().ToString("00") + "' " + GetSegundo().ToString("00") + "''";
        }

        /*
         * Devuelve una cadena que representa el minuto de juego
         */
        public string GetMinutoJuego()
        {
            int m = Math.Min(GetMinuto() + 1, minuto[_parte + 1]);
            return m + "'";
        }

        /*
         * Devuelve los minutos de descuento transcurrido
         */
        public string GetMinutoDescuento()
        {
            int m;

            // Si estamos en un descanso no se devuelve descuento
            if (_parte % 2 != 0)
                m = 0;

            else // Calcula el descuento
                m = Math.Max(0, GetMinuto() + 1 - minuto[_parte + 1]);
            
            return m + "'";
        }

        /**
         * Devuelve el final (último minuto) de la parte en curso
         */
        public int GetFinalParte()
        {
            return minuto[_parte + 1];
        }

        /**
         * Devuelve el nombre de la parte en curso
         */
        public string GetNombreParte(IdiomaData idioma)
        {
            switch (_parte)
            {
                case IniParte1:
                    return idioma.Parte1;
                case FinParte1:
                    return idioma.FinParte1;
                case IniParte2:
                    return idioma.Parte2;
                case FinParte2:
                    return idioma.FinParte2;
                /*case IniProrroga1:
                    return idioma.Prorroga1;
                case FinProrroga1:
                    return idioma.FinProrroga1;
                case IniProrroga2:
                    return idioma.Prorroga2;
                case FinProrroga2:
                    return idioma.FinProrroga2;*/
                case Penaltis:
                    return idioma.Penaltis;
                case FinPenaltis:
                    return idioma.FinPartido;
                case FinPartido:
                    return idioma.FinPartido;
                default:
                    return "";
            }
        }

        /**
         * Representación del minuto actual
         * Ejemplos:
         *   9'
         *   45'+2'
         */
        override public string ToString()
        {
            string m = GetMinutoJuego();
            string d = GetMinutoDescuento();
            d = (d.Equals("0'") ? "" : "+" + d);

            return m + d;
        }

        /**
        * Devuelde la parte
        */

        public int getParte()
        {
            return this._parte;
        }

        public bool EsDescanso()
        {
            return (this.Parte == 1 || this.Parte == 3 || this.Parte == 5);
        }

        public bool EsfinalPrimeraParte()
        {
            return this.Parte == 1;
        }

        public bool EsfinalPartido()
        {
            return this.Parte > 2;
        }
    }
}
