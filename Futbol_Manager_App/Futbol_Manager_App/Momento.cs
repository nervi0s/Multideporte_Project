using System;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App
{

    /**
     * Representación de un Momento
     * Especifica un momento concreto dentro del periodo de juego.
     * Viene dado por la la parte del juego (Primera, Segunda, 1ª Prórroga, 
     * 2ª Prorroga o Penaltis) y dentro de a misma el minuto concreto (con 
     * precisión de segundos)
     */
    [Serializable]
    public class Momento
    {
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
                case IniProrroga1:
                    return idioma.Prorroga1;
                case FinProrroga1:
                    return idioma.FinProrroga1;
                case IniProrroga2:
                    return idioma.Prorroga2;
                case FinProrroga2:
                    return idioma.FinProrroga2;
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
