using System;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App
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

        // 0: 1a parte        0 -  20  (20)
        // 1: 2a parte       20 -  40  (20)
        // 2: 1a prorroga    90 - 105  (15)
        // 3: 2a prorroga   105 - 120  (15)

        //private static int[] minuto = { 0, 20, 20, 40, 40, 45, 45, 50, 50, 50, 50, 50 };
        public static int[] minuto = { 20, 0, 20, 0, 3, 0, 3, 0, 0, 0, 0, 0 };
        public static int[] Minuto { private set { } get { return minuto; } }
        private static int[] minutoAdd = { 0, 0, 20, 0, 40, 0, 45, 0, 50, 40, 40, 40 };
        public static int[] MinutoAdd { private set { } get { return minutoAdd; } }

        public bool isStart { get { return minuto[Parte].Equals(GetMinutoCrono() + minutoAdd[_parte]); }  set { } }

        public int _cadena_minuto;
        public int _cadena_segundo;

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

            _cadena_minuto = minuto[parte];
        }

        /**
         * Constructor
         */
        public Momento(Momento momento)
        {
            _parte = momento.Parte;
            _segundoAbsoluto = momento.SegundoAbsoluto;
            _cadena_minuto = minuto[_parte] - momento._cadena_minuto;
            _cadena_segundo = 60 - momento._cadena_segundo;
        }


        /**
         * Reconfigura el objeto con los nuevos valores
         */
        public void Update(int parte, int minutos, int segundos)
        {
            _parte = parte;
            //_segundoAbsoluto = minutos * 60 + segundos;

            _cadena_minuto = minutos;
            _cadena_segundo = segundos;

            //Console.WriteLine("Segundo absoluto 1: " + _segundoAbsoluto);
        }

        /**
         * Reconfigura el objeto con los nuevos valores
         */
        public void Update(int minutos, int segundos)
        {
            _segundoAbsoluto = (minuto[_parte] + minutos) * 60 + segundos;

            //Console.WriteLine("Segundo absoluto 2: " + _segundoAbsoluto);
        }

        /**
         * Devuelve el minuto actual
         */
        public int GetMinutoCrono()
        {
            //Console.WriteLine("GetMinuto: " + _cadena_minuto);

            //return (_segundoAbsoluto - (_segundoAbsoluto % 60)) / 60;
            return _cadena_minuto;
        }

        public int GetMinuto()
        {
            return _cadena_minuto + minutoAdd[_parte];
        }

        /**
         * Devuelve el segundo actual
         */
        public int GetSegundo()
        {
            //Console.WriteLine("GetSegundo: " + _cadena_segundo);
                        
            //return _segundoAbsoluto % 60;
            return _cadena_segundo;
        }

        public int GetSegundoAbsoluto()
        {
            return GetMinuto() * 60 + GetSegundo();
        }

        /*
         * Devuelve cadena que representa el tiempo para el Cronometro
         */
        public string GetTextoCrono(IdiomaData idioma)
        {
            if (ConfigData.ModeOcrActivated)
            {
                if (Parte % 2 == 1) 
                    return GetNombreParte(idioma) + "\n " + GetMinutoCrono().ToString("00") + "' " + GetSegundo().ToString("00") + "''";
                else
                    return GetNombreParte(idioma) + "\n " + Ocr.minute.ToString("00") + "' " + Ocr.second.ToString("00") + "''";
            }
            else
            {
                return GetNombreParte(idioma) + "\n " + GetMinutoCrono().ToString("00") + "' " + GetSegundo().ToString("00") + "''";
            }
        }

        public string GetTextoCronoWithOCR(IdiomaData idioma, string format)
        {
            if (format == "doubleDot")
            {
                return GetNombreParte(idioma) + "\n " + Ocr.minute.ToString("00") + "' " + Ocr.second.ToString("00") + "''";
            }
            else if (format == "dot")
            {
                return GetNombreParte(idioma) + "\n " + Ocr.minute.ToString("00") + "'' " + Ocr.second.ToString() + "";
            }
            else
            {
                return GetNombreParte(idioma) + "\n " + Ocr.minute.ToString("00") + "' " + Ocr.second.ToString("00") + "''";
            }
        }

        /*
         * Devuelve una cadena que representa el minuto de juego
         */
        public string GetMinutoJuego()
        {
            int m = GetMinutoCrono() + minutoAdd[_parte];
            return m + "'";
        }

        /*
         * Devuelve los minutos de descuento transcurrido
         */
        public string GetMinutoDescuento()
        {
            return "0'";
            //int m;

            //// Si estamos en un descanso no se devuelve descuento
            //if (_parte % 2 != 0)
            //    m = 0;

            //else // Calcula el descuento
            //    m = Math.Max(0, GetMinuto() + 1 - minuto[_parte + 1]);
            
            //return m + "'";
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
