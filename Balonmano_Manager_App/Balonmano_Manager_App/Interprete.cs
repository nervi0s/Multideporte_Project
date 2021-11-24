using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.Timers;
//using NauComDotNet;


namespace Futbol_Sala_Manager_App
{
    public abstract class Interprete
    {
        // Atributos comunes
        public Consola mi_consola;
        public int id_deporte;
        public string puerto_com;
        public string nombre_interprete;


        // Constructor
        public Interprete()
        {            
        }
                    

        /*
        * Metodo que conecta/desconecta con el dispositivo que corresponda. Este metodo lo heredaran las consolas de los diferentes deportes
        */
        public abstract bool conecta(Consola mi_consola);
        public abstract void desconecta();


        /*
        * Metodo que decodifica la trama. Este metodo lo heredaran las consolas de los diferentes deportes para decodificarlas correctamente
        */
        public abstract void decod_trama(string trama = "");
    }



    # region Interprete_mondo

    // Interprete de Mondo
    public class Interprete_mondo : Interprete
    {
        // Atributos
        SerialPort serial_port;
        public int estado_decod = -1;
        public byte[] buffer;

        string dorsal = "", set = "";
        string digito0 = "", digito1 = "", digito2 = "", digito3 = "";        
        bool posesion_con_decimal = false, posesion_ultimos_5_sg = false;
        int posesion_num_aa = 0;


        // Constructor
        public Interprete_mondo(int id_deporte, string puerto)
        {
            this.id_deporte = id_deporte;
            this.puerto_com = puerto;
        }
       
        /*
        *  Se conecta al puerto serie
        */
        public override bool conecta(Consola consola)
        {

            bool error = true;
                                   
            this.mi_consola = consola;

            // Parametros consola
            this.serial_port = new System.IO.Ports.SerialPort();
            this.serial_port.BaudRate = 19200;
            this.serial_port.DataBits = 8;
            this.serial_port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
            this.serial_port.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
            this.serial_port.PortName = this.puerto_com;
            this.serial_port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

            try
            {                
                this.serial_port.Open();
            }
            catch (UnauthorizedAccessException e) { error = false; Console.WriteLine("Oh no " + e); }
            catch (IOException r) { error = false; Console.WriteLine("Oh no " + r); }
            catch (ArgumentException t) { error = false; Console.WriteLine("Oh no " + t); }

            //if (!error)
            //    MessageBox.Show("Error al abrir puerto COM, posiblemente este en uso o no disponible", "Puerto COM no disponible", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Console.WriteLine("A ver " + error);

            if (this.serial_port.IsOpen)
            {
                Console.WriteLine("La consola de mondo ha sido reconocida y en principio esta conectada correctamente");
            }

            return error;
        }


        /*
        *  Cierra la conexión con la consola
        */
        public override void desconecta()
        {
            if (this.serial_port.IsOpen)
                this.serial_port.Close();
        }


        /*
         *   Lee los datos recibidos por el puerto serie
         */
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!this.serial_port.IsOpen)
            {
                return;
            }
            int bytes = this.serial_port.BytesToRead;
            buffer = new byte[bytes];
            this.serial_port.Read(buffer, 0, bytes);
            decod_trama();
        }


        /*
       * Lee un byte de la trama 
       */
        public byte LeerByte(byte[] trama, int b)
        {
            byte by = 0x40;
            if (trama.Length > b)
            {
                by = trama[b];
            }
            return by;
        }


        /*
         * Convierte un byte en un string
         */
        public string ByteToHexString(byte data)
        {
            return Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' ');
        }



        /*
         * Convierte un array de Bytes con caracteres en Hexadecimal
         */
        public string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            return sb.ToString().ToUpper();
        }


        /*
         *  Devuelve el valor decimal correspondiente al valor hexadecimal que llega como parametro
         */
        public string traduce_hexadecimal_a_decimal(string valor_hexadecimal)
        {
            string valor_decimal = "";

            if (valor_hexadecimal == "a")
                valor_decimal = "10";
            else if (valor_hexadecimal == "b")
                valor_decimal = "11";
            else if (valor_hexadecimal == "c")
                valor_decimal = "12";
            else if (valor_hexadecimal == "d")
                valor_decimal = "13";
            else if (valor_hexadecimal == "e")
                valor_decimal = "14";
            else if (valor_hexadecimal == "f")
                valor_decimal = "15";
            else
                valor_decimal = valor_hexadecimal;

            return valor_decimal;
        }


        /*
         *  Devuelve el valor decimal correspondiente al valor hexadecimal que llega como parametro (dorsal)
         */
        public string traduce_dorsal(string valor_hexadecimal)
        {
            string valor_decimal = "";

            if (valor_hexadecimal == "a")
                valor_decimal = "10";
            else if (valor_hexadecimal == "b")
                valor_decimal = "11";
            else if (valor_hexadecimal == "c")
                valor_decimal = "12";
            else if (valor_hexadecimal == "d")
                valor_decimal = "13";
            else if (valor_hexadecimal == "e")
                valor_decimal = "14";
            else if (valor_hexadecimal == "f")
                valor_decimal = "15";
            else if (valor_hexadecimal == "0")
                valor_decimal = "16";
            else if (valor_hexadecimal == "1")
                valor_decimal = "17";
            else
                valor_decimal = valor_hexadecimal;

            return valor_decimal;
        }


        /*
         * Metodo que limpia las variables globles de los digitos
         */
        public void limpia_digitos()
        {
            this.digito0 = "";
            this.digito1 = "";
            this.digito2 = "";
            this.digito3 = "";
        }

        /*
        * Compone un string a partir de los 2 digitos por separado
        */
        public string compone_string_de_2_digitos()
        {
            if (this.digito1 == "f" || this.digito1 == "")
                return this.digito0;
            else
                return this.digito1 + this.digito0;
        }


        /*
        * Compone un string a partir de los 3 digitos por separado
        */
        public string compone_string_de_3_digitos()
        {
            if ((this.digito2 == "0" || this.digito2 == "f" | this.digito2 == "") && (this.digito1 == "0" || this.digito1 == "f" || this.digito1 == ""))
                return this.digito0;
            else if (this.digito2 == "0" || this.digito2 == "f")
                return this.digito1 + this.digito0;
            else
                return this.digito2 + this.digito1 + this.digito0;
        }


        /*
        * Compone un string a partir de los 4 digitos por separado
        */
        public string compone_string_de_4_digitos()
        {
            return (this.digito3.Replace("f", "") + this.digito2.Replace("f", "") + ":" + this.digito1.Replace("f", "") + this.digito0.Replace("f", ""));
        }


        /*
         * Traduce los puntos hexdecimal a string y elimina los posibles "0" y "f" que pueden llegar en el valor hexadecimal
         */
        public string compone_string(string cadena)
        {
            if (cadena.Length > 1)
            {
                //if (cadena[0] == '0')
                //{
                if (Convert.ToString(cadena[1]) != "f")
                    return traduce_hexadecimal_a_decimal(Convert.ToString(cadena[1]));
                else
                    return "0";
                //}
            }
            return cadena;
        }


        /*
         * Devuelve el string del crono de la posesion a partir de sus digitos
         */
        private string compone_crono_posesion(bool con_decimal, string crono_sin_traducir)
        {
            if (!con_decimal)
            {
                // Sin decimal
                return crono_sin_traducir.Replace('f', '0');
            }
            else
            {
                // Con decimal
                return crono_sin_traducir[0] + "." + crono_sin_traducir[1];
            }
        }


        /*
         * Metodo que decodifica las tramas dependiendo del deporte
         */
        public override void decod_trama(string trama = "")
        {
            switch (this.id_deporte)
            {
                case 1:     // Baloncesto
                    decod_trama_baloncesto();
                    break;
                case 2:     // Balonmano
                    decod_trama_balonmano();
                    break;
                case 3:     // Futbol sala
                    decod_trama_futbol_sala();
                    break;
                case 4:     // Waterpolo
                    decod_trama_waterpolo();
                    break;
                case 5:     // Hockey hielo
                    decod_trama_hockey_hielo();
                    break;
                case 6:     // Hockey sala
                    decod_trama_hockey_sala();
                    break;
                case 7:     // Voleibol
                    decod_trama_voleibol();
                    break;
                case 8:     // Tenis
                    decod_trama_tenis();
                    break;
                case 9:     // Futbol
                    decod_trama_futbol();
                    break;
                case 10:     // Fronton
                    decod_trama_fronton();
                    break;
                case 11:     // Baloncesto basico
                    decod_trama_baloncesto();
                    break;
                case 12:     // Padel
                    decod_trama_padel();
                    break;
            }
        }


        /*
         * Metodo que decodifica las tramas del baloncesto
         */
        public void decod_trama_baloncesto()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", byte_leido = "", crono_posesion = "";
            

            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Periodo
                                this.estado_decod = 7;
                            else if (byte_leido == "e0 ") // Bocina y flechas
                                this.estado_decod = 8;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 9;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 10;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 11;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 12;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 13;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 14;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 15;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 16;
                            else if (byte_leido == "ab ") // Faltas equipo local
                                this.estado_decod = 17;
                            else if (byte_leido == "ac ") // Faltas equipo visitante
                                this.estado_decod = 18;

                            else if (byte_leido[0] == '8') // Puntos jugador local
                            {
                                //Console.WriteLine("BYTE TRAMA: " + Convert.ToString(byte_leido[1]));
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 19;
                            }

                            else if (byte_leido[0] == '9') // Puntos jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 20;
                            }

                            else if ((byte_leido[0] == '4') || ((byte_leido[0] == 'c') && (byte_leido[1] != '0'))) // Faltas jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 21;
                            }

                            else if ((byte_leido[0] == '5') || ((byte_leido[0] == 'd') && (byte_leido[1] != '0'))) // Faltas jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 22;
                            }

                            else if (byte_leido[0] == '0') // Dorsales jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 23;
                            }

                            else if (byte_leido[0] == '1') // Dorsales jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 24;
                            }

                            else if (byte_leido == "ea ")   // Crono de posesion
                                this.estado_decod = 25;

                            else
                            {
                                this.estado_decod = 2;      // Fin de trama
                            }
                            ByteActual++;
                        }

                        //Console.WriteLine(byte_leido);

                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                //Console.WriteLine("Trama: " + trama);

                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.digito3);
                                //Console.WriteLine(this.digito2);
                                //Console.WriteLine(this.digito1);
                                //Console.WriteLine(this.digito0);
                                //Console.WriteLine(this.digito3 + this.digito2 + ":" + this.digito1 + this.digito0);
                                //Console.WriteLine("===========================");

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3.Replace("f", "") + this.digito2.Replace("f", "") + "." + this.digito1.Replace("f", "");

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    //limpia_digitos();
                                    ((Consola_baloncesto)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Periodo
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_baloncesto)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));

                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Bocina (byte leido = 50) y flechas
                    else if (this.estado_decod == 8)
                    {
                        if (longitud > ByteActual)
                        {
                            // 51: flecha visitante, 52: flecha local, 53: sin flechas
                            if (byte_leido == "51 ")
                                ((Consola_baloncesto)this.mi_consola).update_flechas("visitante");
                            else if (byte_leido == "52 ")
                                ((Consola_baloncesto)this.mi_consola).update_flechas("local");
                            else if (byte_leido == "53 ")
                                ((Consola_baloncesto)this.mi_consola).update_flechas("none");
                            else if (byte_leido == "50 ")
                                ((Consola_baloncesto)this.mi_consola).update_bocina();
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 9) || (this.estado_decod == 10) || (this.estado_decod == 11))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 9)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 11)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 11)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 12) || (this.estado_decod == 13) || (this.estado_decod == 14))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 12)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 14)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 14)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 15) || (this.estado_decod == 16))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 15)
                                    ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }
                        
                    // Faltas local y visitante [EQUIPO]
                    else if ((this.estado_decod == 17) || (this.estado_decod == 18))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 17)
                                    ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("local", (Convert.ToString(byte_leido[1])).Replace("f", "0"));
                                else
                                    ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("visitante", (Convert.ToString(byte_leido[1])).Replace("f", "0"));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 19) || (this.estado_decod == 20))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 15))
                            {
                                //Console.WriteLine("BYTE 0: " + Convert.ToString(byte_leido[0]));
                                //Console.WriteLine("BYTE 1: " + Convert.ToString(byte_leido[1]));
                                //Console.WriteLine("DORSAL " + dorsal);

                                if (this.estado_decod == 19)
                                {
                                    //if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                    if (((Convert.ToString(byte_leido[0]) == "f") || (Convert.ToString(byte_leido[0]) == "0")) && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("local", traduce_dorsal(dorsal), "0");
                                    else
                                        //((Consola_baloncesto)this.mi_consola).update_puntos_jugador("local", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]).Replace("0", "") + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                        ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("local", traduce_dorsal(dorsal), ((Convert.ToString(byte_leido[0])).Replace("0", "") + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                                else
                                {
                                    if (((Convert.ToString(byte_leido[0]) == "f") || (Convert.ToString(byte_leido[0]) == "0")) && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("visitante", traduce_dorsal(dorsal), "0");
                                    else
                                        //((Consola_baloncesto)this.mi_consola).update_puntos_jugador("visitante", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                        ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("visitante", traduce_dorsal(dorsal), ((Convert.ToString(byte_leido[0])).Replace("0", "") + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }
                        
                    // Faltas jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 21) || (this.estado_decod == 22))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 3) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 15))
                            {
                                if (Convert.ToInt32(traduce_dorsal(dorsal)) == 3)
                                    dorsal = "E";
                                else
                                    dorsal = traduce_dorsal(dorsal);

                                if (this.estado_decod == 21)
                                    ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("local", dorsal, Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("visitante", dorsal, Convert.ToString(byte_leido[1]));
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }
                        
                    // Dorsales jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 23) || (this.estado_decod == 24))
                    {
                        if (longitud > ByteActual)
                        {
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);

                            //Console.WriteLine(this.digito0);
                            //Console.WriteLine(this.digito1);

                            if (this.estado_decod == 23)
                                ((Consola_baloncesto)this.mi_consola).update_dorsales_jugadores("local", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                            else
                                ((Consola_baloncesto)this.mi_consola).update_dorsales_jugadores("visitante", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }
                        
                    // Crono de posesion
                    else if (this.estado_decod == 25)
                    {
                        if (longitud > ByteActual)
                        {
                            if (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1]) == "bb")
                            {
                                // Dato siguiente sin decimal
                                this.posesion_con_decimal = false;
                                this.posesion_ultimos_5_sg = false;
                            }
                            else if ((Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1]) == "aa") && (!this.posesion_ultimos_5_sg))
                            {
                                // Dato siguiente con decimal
                                this.posesion_con_decimal = true;
                                this.posesion_ultimos_5_sg = true;
                                this.posesion_num_aa++;
                            }
                            else if ((Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1]) == "aa") && (this.posesion_ultimos_5_sg))
                            {
                                if (this.posesion_num_aa == 1)
                                {
                                    ((Consola_baloncesto)this.mi_consola).update_crono_posesion("4.9");
                                    this.posesion_num_aa++;
                                }
                                else if (this.posesion_num_aa == 2)
                                {
                                    ((Consola_baloncesto)this.mi_consola).update_crono_posesion("4.9");
                                    this.posesion_num_aa++;
                                }
                                else if (this.posesion_num_aa == 3)
                                {
                                    ((Consola_baloncesto)this.mi_consola).update_crono_posesion("4.8");
                                    this.posesion_num_aa++;
                                }
                            }
                            else
                            {
                                crono_posesion = compone_crono_posesion(this.posesion_con_decimal, Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1]));
                                ((Consola_baloncesto)this.mi_consola).update_crono_posesion(crono_posesion);
                            }

                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }


        /*
         * Metodo que decodifica las tramas del balonmano
         */
        public void decod_trama_balonmano()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Periodo
                                this.estado_decod = 7;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 8;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 9;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 10;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 11;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 12;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 13;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 14;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 15;
                            else if (byte_leido[0] == '8') // Goles jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                //Console.WriteLine("[ LOCAL ]: " + dorsal);
                                this.estado_decod = 16;
                            }
                            else if (byte_leido[0] == '9') // Goles jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 17;
                            }
                            else if (byte_leido[0] == '0') // Dorsales jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 18;
                            }

                            else if (byte_leido[0] == '1') // Dorsales jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 19;
                            }

                            else if (byte_leido == "64 ") // Expulsion jugador local
                                this.estado_decod = 20;

                            else if (byte_leido == "65 ") // Expulsion jugador visitante
                                this.estado_decod = 21;

                            else if (byte_leido == "66 ") // Anulacion expulsion jugador local
                                this.estado_decod = 22;

                            else if (byte_leido == "67 ") // Anulacion expulsion jugador visitante
                                this.estado_decod = 23;

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_balonmano)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Periodo
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_balonmano)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 8) || (this.estado_decod == 9) || (this.estado_decod == 10))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 8)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 10)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_balonmano)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 11) || (this.estado_decod == 12) || (this.estado_decod == 13))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 11)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 13)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_balonmano)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 14) || (this.estado_decod == 15))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 14)
                                    ((Consola_balonmano)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_balonmano)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Goles jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 16) || (this.estado_decod == 17))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 17))
                            {
                                if (this.estado_decod == 16)
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_balonmano)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_balonmano)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                                else
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_balonmano)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_balonmano)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Dorsales jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 18) || (this.estado_decod == 19))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 18)
                                ((Consola_balonmano)this.mi_consola).update_dorsales_jugadores("local", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                            else
                                ((Consola_balonmano)this.mi_consola).update_dorsales_jugadores("visitante", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 20) || (this.estado_decod == 21))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 20)
                                ((Consola_balonmano)this.mi_consola).update_suma_expulsion_jugador("local", compone_string_de_2_digitos());
                            else
                                ((Consola_balonmano)this.mi_consola).update_suma_expulsion_jugador("visitante", compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Anulacion expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 22) || (this.estado_decod == 23))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 22)
                                ((Consola_balonmano)this.mi_consola).update_resta_expulsion_jugador("local", compone_string_de_2_digitos());
                            else
                                ((Consola_balonmano)this.mi_consola).update_resta_expulsion_jugador("visitante", compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
         * Metodo que decodifica las tramas del futbol sala
         */
        public void decod_trama_futbol_sala()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Periodo
                                this.estado_decod = 7;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 8;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 9;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 10;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 11;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 12;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 13;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 14;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 15;
                            else if (byte_leido == "ab ") // Falta equipo local
                                this.estado_decod = 16;
                            else if (byte_leido == "ac ") // Falta equipo visitante
                                this.estado_decod = 17;

                            else if (byte_leido[0] == '8') // Goles jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 18;
                            }

                            else if (byte_leido[0] == '9') // Goles jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 19;
                            }

                            else if (byte_leido[0] == '0') // Dorsales jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 22;
                            }

                            else if (byte_leido[0] == '1') // Dorsales jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 23;
                            }

                            /* Esto hay que quitarlo, es una ñapa que hemos hecho para la Copa de España de fútbol sala y usaron la consola */
                            /* como si estuviesen haciendo Balonmano */
                            else if (byte_leido == "64 ") // Expulsion jugador local
                                this.estado_decod = 24;

                            else if (byte_leido == "65 ") // Expulsion jugador visitante
                                this.estado_decod = 25;

                            else if (byte_leido == "66 ") // Anulacion expulsion jugador local
                                this.estado_decod = 26;

                            else if (byte_leido == "67 ") // Anulacion expulsion jugador visitante
                                this.estado_decod = 27;
                            /****************************************************************************************************************/

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                //Console.WriteLine("Trama: " + trama);

                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.crono_digito3 + this.crono_digito2 + ":" + this.crono_digito1 + this.crono_digito0);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_futbol_sala)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Periodo
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_futbol_sala)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 8) || (this.estado_decod == 9) || (this.estado_decod == 10))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 8)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 10)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_futbol_sala)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 11) || (this.estado_decod == 12) || (this.estado_decod == 13))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 11)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 13)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_futbol_sala)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 14) || (this.estado_decod == 15))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 14)
                                    ((Consola_futbol_sala)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_futbol_sala)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Faltas local y visitante [EQUIPO]
                    else if ((this.estado_decod == 16) || (this.estado_decod == 17))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 16)
                                    ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("local", (Convert.ToString(byte_leido[1])).Replace("f", "0"));
                                else
                                    ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("visitante", (Convert.ToString(byte_leido[1])).Replace("f", "0"));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Goles jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 18) || (this.estado_decod == 19))
                    {
                        if (longitud > ByteActual)
                        {
                            /* Hay que dejar la condición que está comentada, es una ñapa que hemos hecho para la Copa de España de fútbol sala y */
                            /* usaron la consola como si estuviesen haciendo Balonmano */
                            //if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 15))
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 17))
                            {
                                if (this.estado_decod == 18)
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_futbol_sala)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_futbol_sala)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                                else
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_futbol_sala)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_futbol_sala)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Dorsales jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 22) || (this.estado_decod == 23))
                    {
                        if (longitud > ByteActual)
                        {
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 22)
                                ((Consola_futbol_sala)this.mi_consola).update_dorsales_jugadores("local", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                            else
                                ((Consola_futbol_sala)this.mi_consola).update_dorsales_jugadores("visitante", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }


                    /* Esto hay que quitarlo, es una ñapa que hemos hecho para la Copa de España de fútbol sala y usaron la consola */
                    /* como si estuviesen haciendo Balonmano */
                    // Expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 24) || (this.estado_decod == 25))
                    {
                        if (longitud > ByteActual)
                        {
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 24)
                                ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("local", compone_string_de_2_digitos());
                            else
                                ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("visitante", compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Anulacion expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 26) || (this.estado_decod == 27))
                    {
                        if (longitud > ByteActual)
                        {
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 26)
                                ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("local", compone_string_de_2_digitos());
                            else
                                ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("visitante", compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }
                    /****************************************************************************************************************/

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
         * Metodo que decodifica las tramas del waterpolo
         */
        public void decod_trama_waterpolo()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Periodo
                                this.estado_decod = 7;
                            else if (byte_leido == "e0 ") // Bocina y flechas
                                this.estado_decod = 8;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 9;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 10;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 11;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 12;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 13;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 14;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 15;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 16;

                            else if (byte_leido[0] == '8') // Goles jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 17;
                            }

                            else if (byte_leido[0] == '9') // Goles jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 18;
                            }

                            else if ((byte_leido[0] == '4') || ((byte_leido[0] == 'c') && (byte_leido[1] != '0'))) // Faltas jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 19;
                            }

                            else if ((byte_leido[0] == '5') || ((byte_leido[0] == 'd') && (byte_leido[1] != '0'))) // Faltas jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 20;
                            }

                            else if (byte_leido[0] == '0') // Dorsales jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 21;
                            }

                            else if (byte_leido[0] == '1') // Dorsales jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 22;
                            }

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }


                        // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                //Console.WriteLine("Trama: " + trama);

                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.crono_digito3 + this.crono_digito2 + ":" + this.crono_digito1 + this.crono_digito0);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_waterpolo)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Periodo
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_waterpolo)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Bocina (byte leido = 50) y flechas
                    else if (this.estado_decod == 8)
                    {
                        if (longitud > ByteActual)
                        {
                            // 51: flecha visitante, 52: flecha local, 53: sin flechas
                            if (byte_leido == "51 ")
                                ((Consola_waterpolo)this.mi_consola).update_flechas("visitante");
                            else if (byte_leido == "52 ")
                                ((Consola_waterpolo)this.mi_consola).update_flechas("local");
                            else if (byte_leido == "53 ")
                                ((Consola_waterpolo)this.mi_consola).update_flechas("none");                           
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 9) || (this.estado_decod == 10) || (this.estado_decod == 11))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 9)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 11)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 11)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_waterpolo)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 12) || (this.estado_decod == 13) || (this.estado_decod == 14))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 12)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 14)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 14)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_waterpolo)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 15) || (this.estado_decod == 16))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 15)
                                    ((Consola_waterpolo)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_waterpolo)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Goles jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 17) || (this.estado_decod == 18))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 15))
                            {
                                if (this.estado_decod == 17)
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_waterpolo)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_waterpolo)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                                else
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_waterpolo)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_waterpolo)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Expulsiones jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 19) || (this.estado_decod == 20))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 3) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 15))
                            {
                                if (Convert.ToInt32(traduce_dorsal(dorsal)) == 3)
                                    dorsal = "E";
                                else
                                    dorsal = traduce_dorsal(dorsal);

                                if (this.estado_decod == 19)
                                    ((Consola_waterpolo)this.mi_consola).update_expulsiones_jugador("local", dorsal, Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_waterpolo)this.mi_consola).update_expulsiones_jugador("visitante", dorsal, Convert.ToString(byte_leido[1]));
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Dorsales jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 21) || (this.estado_decod == 22))
                    {
                        if (longitud > ByteActual)
                        {
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 21)
                                //Console.WriteLine("equipo local : " + traduce_dorsal(dorsal));
                                ((Consola_waterpolo)this.mi_consola).update_dorsales_jugadores("local", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                            else
                                //Console.WriteLine("equipo visitante : " + traduce_dorsal(dorsal));
                                ((Consola_waterpolo)this.mi_consola).update_dorsales_jugadores("visitante", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
         * Metodo que decodifica las tramas del hockey hielo
         */
        public void decod_trama_hockey_hielo()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", minutos = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama + "<----------");

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Periodo
                                this.estado_decod = 7;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 8;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 9;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 10;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 11;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 12;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 13;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 14;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 15;

                            else if (byte_leido[0] == '8') // Goles jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 16;
                            }

                            else if (byte_leido[0] == '9') // Goles jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 17;
                            }

                            else if (byte_leido[0] == '0') // Dorsales jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 18;
                            }

                            else if (byte_leido[0] == '1') // Dorsales jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 19;
                            }

                            else if (byte_leido == "60 ") // Expulsion de 2:00
                                minutos = "2";

                            else if (byte_leido == "61 ") // Expulsion de 5:00
                                minutos = "5";

                            else if (byte_leido == "64 ") // Expulsion jugador local
                                this.estado_decod = 20;

                            else if (byte_leido == "65 ") // Expulsion jugador visitante
                                this.estado_decod = 21;

                            else if (byte_leido == "66 ") // Anulacion expulsion jugador local
                                this.estado_decod = 22;

                            else if (byte_leido == "67 ") // Anulacion expulsion jugador visitante
                                this.estado_decod = 23;

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                //Console.WriteLine("Trama: " + trama);

                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.crono_digito3 + this.crono_digito2 + ":" + this.crono_digito1 + this.crono_digito0);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_hockey_hielo)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Periodo
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_hockey_hielo)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 8) || (this.estado_decod == 9) || (this.estado_decod == 10))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 8)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 10)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_hockey_hielo)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 11) || (this.estado_decod == 12) || (this.estado_decod == 13))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 11)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 13)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_hockey_hielo)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 14) || (this.estado_decod == 15))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 14)
                                    ((Consola_hockey_hielo)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_hockey_hielo)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Goles jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 16) || (this.estado_decod == 17))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 15))
                            {
                                if (this.estado_decod == 16)
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_hockey_hielo)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_hockey_hielo)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                                else
                                {
                                    if ((Convert.ToString(byte_leido[0]) == "f") && (Convert.ToString(byte_leido[1]) == "f"))
                                        ((Consola_hockey_hielo)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), "0");
                                    else
                                        ((Consola_hockey_hielo)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])).Replace("f", ""));
                                }
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Dorsales jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 18) || (this.estado_decod == 19))
                    {
                        if (longitud > ByteActual)
                        {
                            if (this.estado_decod == 18)
                                //actualiza_dorsales_jugadores("local", traduce_dorsal(dorsal), compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                                ((Consola_hockey_hielo)this.mi_consola).update_dorsales_jugadores("local", traduce_dorsal(dorsal), traduce_dorsal(dorsal));
                            else
                                //actualiza_dorsales_jugadores("visitante", traduce_dorsal(dorsal), compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                                ((Consola_hockey_hielo)this.mi_consola).update_dorsales_jugadores("visitante", traduce_dorsal(dorsal), traduce_dorsal(dorsal));
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 20) || (this.estado_decod == 21))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            //Console.WriteLine(trama);

                            if (minutos == "")
                                minutos = "2";

                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 20)
                                ((Consola_hockey_hielo)this.mi_consola).update_suma_expulsion_jugador("local", compone_string_de_2_digitos(), minutos);
                            else
                                ((Consola_hockey_hielo)this.mi_consola).update_suma_expulsion_jugador("visitante", compone_string_de_2_digitos(), minutos);
                            limpia_digitos();
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Anulacion expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 22) || (this.estado_decod == 23))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 22)
                                ((Consola_hockey_hielo)this.mi_consola).update_resta_expulsion_jugador("local", compone_string_de_2_digitos());
                            else
                                ((Consola_hockey_hielo)this.mi_consola).update_resta_expulsion_jugador("visitante", compone_string_de_2_digitos());
                            limpia_digitos();
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
        * Metodo que decodifica las tramas del hockey sala
        */
        public void decod_trama_hockey_sala()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Periodo
                                this.estado_decod = 7;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 8;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 9;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 10;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 11;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 12;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 13;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 14;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 15;

                            else if (byte_leido[0] == '8') // Goles jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 16;
                            }

                            else if (byte_leido[0] == '9') // Goles jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 17;
                            }

                            else if (byte_leido[0] == '0') // Dorsales jugador local
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 18;
                            }

                            else if (byte_leido[0] == '1') // Dorsales jugador visitante
                            {
                                dorsal = Convert.ToString(byte_leido[1]);
                                this.estado_decod = 19;
                            }

                            else if (byte_leido == "64 ") // Expulsion jugador local
                                this.estado_decod = 20;

                            else if (byte_leido == "65 ") // Expulsion jugador visitante
                                this.estado_decod = 21;

                            else if (byte_leido == "66 ") // Anulacion expulsion jugador local
                                this.estado_decod = 22;

                            else if (byte_leido == "67 ") // Anulacion expulsion jugador visitante
                                this.estado_decod = 23;

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                //Console.WriteLine("Trama: " + trama);

                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.crono_digito3 + this.crono_digito2 + ":" + this.crono_digito1 + this.crono_digito0);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_hockey_sala)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Periodo
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_hockey_sala)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 8) || (this.estado_decod == 9) || (this.estado_decod == 10))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 8)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 10)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_hockey_sala)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 11) || (this.estado_decod == 12) || (this.estado_decod == 13))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 11)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 13)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_hockey_sala)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 14) || (this.estado_decod == 15))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 14)
                                    ((Consola_hockey_sala)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_hockey_sala)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos jugador local y visitante [JUGADOR]
                    else if ((this.estado_decod == 16) || (this.estado_decod == 17))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(traduce_dorsal(dorsal)) >= 4) && (Convert.ToInt32(traduce_dorsal(dorsal)) <= 17))
                            {
                                if (this.estado_decod == 16)
                                    ((Consola_hockey_sala)this.mi_consola).update_goles_jugador("local", traduce_dorsal(dorsal), compone_string(Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])));
                                else
                                    ((Consola_hockey_sala)this.mi_consola).update_goles_jugador("visitante", traduce_dorsal(dorsal), compone_string(Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])));
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Dorsales jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 18) || (this.estado_decod == 19))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 18)
                                ((Consola_hockey_sala)this.mi_consola).update_dorsales_jugadores("local", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                            else
                                ((Consola_hockey_sala)this.mi_consola).update_dorsales_jugadores("visitante", traduce_dorsal(dorsal), compone_string_de_2_digitos());
                            limpia_digitos();
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 20) || (this.estado_decod == 21))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 20)
                                ((Consola_hockey_sala)this.mi_consola).update_suma_expulsion_jugador("local", compone_string_de_2_digitos());
                            else
                                ((Consola_hockey_sala)this.mi_consola).update_suma_expulsion_jugador("visitante", compone_string_de_2_digitos());
                            limpia_digitos();
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Anulacion expulsion jugadores local y visitantes [JUGADOR]
                    else if ((this.estado_decod == 22) || (this.estado_decod == 23))
                    {
                        if (longitud > ByteActual)
                        {
                            //Console.WriteLine("DORSAL: " + compone_string_de_2_digitos(Convert.ToString(byte_leido[0]), Convert.ToString(byte_leido[1])));
                            this.digito0 = Convert.ToString(byte_leido[1]);
                            this.digito1 = Convert.ToString(byte_leido[0]);
                            if (this.estado_decod == 22)
                                ((Consola_hockey_sala)this.mi_consola).update_resta_expulsion_jugador("local", compone_string_de_2_digitos());
                            else
                                ((Consola_hockey_sala)this.mi_consola).update_resta_expulsion_jugador("visitante", compone_string_de_2_digitos());
                            limpia_digitos();
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
         * Metodo que decodifica las tramas del voleibol
         */
        public void decod_trama_voleibol()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Sets ganados visitante
                                this.estado_decod = 3;
                            else if (byte_leido == "a2 ") // Sets ganados local
                                this.estado_decod = 4;
                            else if (byte_leido == "aa ") // Set
                                this.estado_decod = 5;
                            else if (byte_leido == "e0 ") // Bocina y flechas
                                this.estado_decod = 6;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 7;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 8;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 9;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 10;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 11;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 12;
                            else if (byte_leido == "c0 ") // Tiempo muerto local
                                this.estado_decod = 13;
                            else if (byte_leido == "d0 ") // Tiempo muerto visitante
                                this.estado_decod = 14;

                            else if (byte_leido[0] == '8') // Sets equipo local
                            {
                                set = Convert.ToString(Convert.ToInt32(traduce_hexadecimal_a_decimal(Convert.ToString(byte_leido[1]))) - 3);
                                this.estado_decod = 15;
                            }

                            else if (byte_leido[0] == '9') // Sets equipo visitante
                            {
                                set = Convert.ToString(Convert.ToInt32(traduce_hexadecimal_a_decimal(Convert.ToString(byte_leido[1]))) - 3);
                                this.estado_decod = 16;
                            }

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Sets local y visitante [EQUIPO]
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 3)
                                    ((Consola_voleibol)this.mi_consola).update_sets_ganados("visitante", Convert.ToString(byte_leido[1]));
                                if (this.estado_decod == 4)
                                    ((Consola_voleibol)this.mi_consola).update_sets_ganados("local", Convert.ToString(byte_leido[1]));
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Set
                    else if (this.estado_decod == 5)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_voleibol)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Bocina (byte leido = 50) y flechas
                    else if (this.estado_decod == 6)
                    {
                        if (longitud > ByteActual)
                        {
                            // 51: flecha visitante, 52: flecha local, 53: sin flechas
                            if (byte_leido == "51 ")
                                ((Consola_voleibol)this.mi_consola).update_flechas("visitante");
                            else if (byte_leido == "52 ")
                                ((Consola_voleibol)this.mi_consola).update_flechas("local");
                            else if (byte_leido == "53 ")
                                ((Consola_voleibol)this.mi_consola).update_flechas("none");     
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 7) || (this.estado_decod == 8) || (this.estado_decod == 9))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 7)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 8)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 9)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_voleibol)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 10) || (this.estado_decod == 11) || (this.estado_decod == 12))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 10)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 11)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 12)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_voleibol)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tiempo muerto local y visitante [EQUIPO]
                    else if ((this.estado_decod == 13) || (this.estado_decod == 14))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 13)
                                    ((Consola_voleibol)this.mi_consola).update_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                                else
                                    ((Consola_voleibol)this.mi_consola).update_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Sets equipo local y visitante [EQUIPO]
                    else if ((this.estado_decod == 15) || (this.estado_decod == 16))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(set) >= 1) && (Convert.ToInt32(set) <= 5))
                            {
                                if (this.estado_decod == 15)
                                {
                                    if (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1]) == "ff")
                                        ((Consola_voleibol)this.mi_consola).update_sets_equipo("local", set, "0");
                                    else
                                        ((Consola_voleibol)this.mi_consola).update_sets_equipo("local", set, Convert.ToString(byte_leido[0]).Replace("f", "") + Convert.ToString(byte_leido[1]));
                                }
                                else
                                {
                                    if (Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1]) == "ff")
                                        ((Consola_voleibol)this.mi_consola).update_sets_equipo("visitante", set, "0");
                                    else
                                        ((Consola_voleibol)this.mi_consola).update_sets_equipo("visitante", set, Convert.ToString(byte_leido[0]).Replace("f", "") + Convert.ToString(byte_leido[1]));
                                }
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
         * Metodo que decodifica las tramas del tenis
         */
        public void decod_trama_tenis()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Sets ganados visitante
                                this.estado_decod = 3;
                            else if (byte_leido == "a2 ") // Sets ganados local
                                this.estado_decod = 4;
                            else if (byte_leido == "aa ") // Set
                                this.estado_decod = 5;
                            else if (byte_leido == "e0 ") // Bocina, flechas y tie break
                                this.estado_decod = 6;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 7;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 8;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 9;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 10;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 11;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 12;

                            else if (byte_leido[0] == '8') // Sets equipo local
                            {
                                set = Convert.ToString(Convert.ToInt32(traduce_hexadecimal_a_decimal(Convert.ToString(byte_leido[1]))) - 3);
                                this.estado_decod = 13;
                            }

                            else if (byte_leido[0] == '9') // Sets equipo visitante
                            {
                                set = Convert.ToString(Convert.ToInt32(traduce_hexadecimal_a_decimal(Convert.ToString(byte_leido[1]))) - 3);
                                this.estado_decod = 14;
                            }

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Sets local y visitante [EQUIPO]
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 3)
                                    ((Consola_tenis)this.mi_consola).update_juegos("visitante", "", Convert.ToString(byte_leido[1]));
                                if (this.estado_decod == 4)
                                    ((Consola_tenis)this.mi_consola).update_juegos("local", "", Convert.ToString(byte_leido[1]));
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Set
                    else if (this.estado_decod == 5)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_tenis)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Bocina (byte leido = 50) y flechas
                    else if (this.estado_decod == 6)
                    {
                        if (longitud > ByteActual)
                        {
                            // 51: flecha visitante, 52: flecha local, 53: sin flechas
                            if (byte_leido == "51 ")
                                ((Consola_tenis)this.mi_consola).update_flechas("visitante");
                            else if (byte_leido == "52 ")
                                ((Consola_tenis)this.mi_consola).update_flechas("local");
                            else if (byte_leido == "53 ")
                                ((Consola_tenis)this.mi_consola).update_flechas("none");    
                            else if ((byte_leido == "60 ") || (byte_leido == "61 "))   // 60: apaga tie break, 61: enciende tie break
                                ((Consola_tenis)this.mi_consola).update_tie_break(Convert.ToString(byte_leido[1]));
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 7) || (this.estado_decod == 8) || (this.estado_decod == 9))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 7)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 8)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 9)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();

                                    if (puntos == "e")      // Llega 40 pero cuando el visitante tambien lleva 40 (irian 40 - 40)
                                        puntos = "40";
                                    else if (puntos == "a") // Advanced
                                        puntos = "50";
                                    else if (puntos == "f") // Llega 40 pero cuando el visitante lleva ventaja (irian 40 - Ad)
                                        puntos = "40";

                                    ((Consola_tenis)this.mi_consola).update_puntos_equipo("local", puntos);
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 10) || (this.estado_decod == 11) || (this.estado_decod == 12))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 10)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 11)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 12)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();

                                    if (puntos == "e")      // Llega 40 cuando el local tambien lleva 40 (irian 40 - 40)
                                        puntos = "40";
                                    else if (puntos == "a") // Advanced
                                        puntos = "50";
                                    else if (puntos == "f") // Llega 40 cuando el local lleva ventaja (irian Ad - 40)
                                        puntos = "40";

                                    ((Consola_tenis)this.mi_consola).update_puntos_equipo("visitante", puntos);
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Sets equipo local y visitante [EQUIPO]
                    else if ((this.estado_decod == 13) || (this.estado_decod == 14))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(set) >= 1) && (Convert.ToInt32(set) <= 5))
                            {
                                if (this.estado_decod == 13)
                                    ((Consola_tenis)this.mi_consola).update_sets_equipo("local", set, compone_string(Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])));
                                else
                                    ((Consola_tenis)this.mi_consola).update_sets_equipo("visitante", set, compone_string(Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])));
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
         * Metodo que decodifica las tramas del futbol
         */
        public void decod_trama_futbol()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", crono = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "aa ") // Parte
                                this.estado_decod = 7;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 8;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 9;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 10;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 11;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 12;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 13;
                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                //Console.WriteLine("Trama: " + trama);

                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.crono_digito3 + this.crono_digito2 + ":" + this.crono_digito1 + this.crono_digito0);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_futbol)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Parte
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                ((Consola_futbol)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 8) || (this.estado_decod == 9) || (this.estado_decod == 10))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 8)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 10)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 10)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_futbol)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 11) || (this.estado_decod == 12) || (this.estado_decod == 13))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 11)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 13)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_futbol)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }


        /*
         * Metodo que decodifica las tramas del fronton
         */
        public void decod_trama_fronton()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string tanteo = "", puntos = "", crono = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Crono digito 0
                                this.estado_decod = 3;
                            else if (byte_leido == "a1 ") // Crono digito 1
                                this.estado_decod = 4;
                            else if (byte_leido == "a2 ") // Crono digito 2
                                this.estado_decod = 5;
                            else if (byte_leido == "a3 ") // Crono digito 3
                                this.estado_decod = 6;
                            else if (byte_leido == "e0 ") // Bocina y flechas
                                this.estado_decod = 7;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 8;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 9;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 10;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 11;
                            else if (byte_leido == "a6 ") // Tanteo necesario para ganar el juego digito 1
                                this.estado_decod = 12;
                            else if (byte_leido == "a9 ") // Tanteo necesario para ganar el juego digito 2                       
                                this.estado_decod = 13;
                            //else if (byte_leido == "c0 ") // Tiempo muerto local
                            //    this.estado_decod = 14;
                            //else if (byte_leido == "d0 ") // Tiempo muerto visitante
                            //    this.estado_decod = 15;
                            else if (byte_leido == "ab ") // Sets equipo local
                                this.estado_decod = 16;
                            else if (byte_leido == "ac ") // Sets equipo visitante
                                this.estado_decod = 17;
                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Crono, los 4 digitos
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4) || (this.estado_decod == 5) || (this.estado_decod == 6))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 3)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 4)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 5)
                                    this.digito2 = Convert.ToString(byte_leido[1]);
                                if (this.estado_decod == 6)
                                    this.digito3 = Convert.ToString(byte_leido[1]);

                                //Console.WriteLine(this.digito3 + this.digito2 + ":" + this.digito1 + this.digito0);

                                if (this.estado_decod == 6)
                                {
                                    if ((digito3 == "f") && (digito2 == "f") && (digito1 == "f") && (digito0 == "f"))
                                        crono = "";

                                    else if (digito0 == "f")    // Ultimo minuto - Formato XX.X
                                        crono = this.digito3 + this.digito2 + "." + this.digito1;

                                    else   // Formato XX:XX
                                        crono = compone_string_de_4_digitos();

                                    limpia_digitos();
                                    ((Consola_fronton)this.mi_consola).update_crono(crono);
                                }
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Bocina (byte leido = 50) y flechas
                    else if (this.estado_decod == 7)
                    {
                        if (longitud > ByteActual)
                        {
                            // 51: flecha visitante, 52: flecha local, 53: sin flechas
                            if (byte_leido == "51 ")
                                ((Consola_fronton)this.mi_consola).update_flechas("visitante");
                            else if (byte_leido == "52 ")
                                ((Consola_fronton)this.mi_consola).update_flechas("local");
                            else if (byte_leido == "53 ")
                                ((Consola_fronton)this.mi_consola).update_flechas("none");
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 8) || (this.estado_decod == 9))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 8)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito1 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 9)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_fronton)this.mi_consola).update_puntos_equipo("local", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 10) || (this.estado_decod == 11))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 10)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 11)
                                    digito1 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 11)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();
                                    ((Consola_fronton)this.mi_consola).update_puntos_equipo("visitante", Convert.ToString(puntos));
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Tanteo necesario para ganar el juego
                    else if ((this.estado_decod == 12) || (this.estado_decod == 13))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 12)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 13)
                                    digito0 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 13)
                                {
                                    tanteo = compone_string_de_2_digitos();
                                    limpia_digitos();
                                    ((Consola_fronton)this.mi_consola).update_tanteo_necesario(tanteo);
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    //// Tiempo muerto local y visitante [EQUIPO]
                    //else if ((this.estado_decod == 14) || (this.estado_decod == 15))
                    //{
                    //    if (longitud > ByteActual)
                    //    {
                    //        //if (byte_leido[0] == '5')
                    //        //{
                    //            //if (this.estado_decod == 14)
                    //            //    actualiza_tiempos_muertos("local", Convert.ToString(byte_leido[1]));
                    //            //else if (this.estado_decod == 15)
                    //            //    actualiza_tiempos_muertos("visitante", Convert.ToString(byte_leido[1]));
                    //        //}
                    //        this.estado_decod = 2;
                    //        ByteActual++;
                    //    }
                    //}

                    // Sets local y visitante [EQUIPO]
                    else if ((this.estado_decod == 16) || (this.estado_decod == 17))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 16)
                                    ((Consola_fronton)this.mi_consola).update_sets_equipo("local", (Convert.ToString(byte_leido[1])).Replace("f", "0"));
                                else if (this.estado_decod == 17)
                                    ((Consola_fronton)this.mi_consola).update_sets_equipo("visitante", (Convert.ToString(byte_leido[1])).Replace("f", "0"));
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }



        /*
        * Metodo que decodifica las tramas del padel
        */
        public void decod_trama_padel()
        {
            int longitud = this.buffer.Length;
            int ByteActual = 0;
            string puntos = "", byte_leido = "";


            string trama = ByteArrayToHexString(this.buffer);
            //Console.WriteLine("Trama: " + trama);

            while (ByteActual < longitud)
            {
                byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));

                if (byte_leido.Length > 1)
                {
                    if (this.estado_decod == -1)
                    {
                        if (longitud > ByteActual)
                        {
                            // Empieza trama
                            if (byte_leido == "02 ")
                                this.estado_decod = 1;
                            ByteActual++;
                        }
                    }

                    // Mira la info que le envia
                    else if (this.estado_decod == 1)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "a0 ")      // Sets ganados visitante
                                this.estado_decod = 3;
                            else if (byte_leido == "a2 ") // Sets ganados local
                                this.estado_decod = 4;
                            else if (byte_leido == "aa ") // Set
                                this.estado_decod = 5;
                            else if (byte_leido == "e0 ") // Bocina, flechas y tie break
                                this.estado_decod = 6;
                            else if (byte_leido == "a4 ") // Marcador local digito 0
                                this.estado_decod = 7;
                            else if (byte_leido == "a5 ") // Marcador local digito 1
                                this.estado_decod = 8;
                            else if (byte_leido == "a6 ") // Marcador local digito 2
                                this.estado_decod = 9;
                            else if (byte_leido == "a7 ") // Marcador visitante digito 0
                                this.estado_decod = 10;
                            else if (byte_leido == "a8 ") // Marcador visitante digito 1
                                this.estado_decod = 11;
                            else if (byte_leido == "a9 ") // Marcador visitante digito 2
                                this.estado_decod = 12;

                            else if (byte_leido[0] == '8') // Sets equipo local
                            {
                                set = Convert.ToString(Convert.ToInt32(traduce_hexadecimal_a_decimal(Convert.ToString(byte_leido[1]))) - 3);
                                this.estado_decod = 13;
                            }

                            else if (byte_leido[0] == '9') // Sets equipo visitante
                            {
                                set = Convert.ToString(Convert.ToInt32(traduce_hexadecimal_a_decimal(Convert.ToString(byte_leido[1]))) - 3);
                                this.estado_decod = 14;
                            }

                            else
                            {
                                this.estado_decod = 2;          // Fin de trama
                            }
                            ByteActual++;
                        }
                    }

                    // Fin de trama
                    else if (this.estado_decod == 2)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido == "03 ")
                                this.estado_decod = -1;
                            ByteActual++;
                        }
                    }

                    // Sets local y visitante [EQUIPO]
                    else if ((this.estado_decod == 3) || (this.estado_decod == 4))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 3)
                                    ((Consola_padel)this.mi_consola).update_juegos("visitante", "", Convert.ToString(byte_leido[1]));
                                if (this.estado_decod == 4)
                                    ((Consola_padel)this.mi_consola).update_juegos("local", "", Convert.ToString(byte_leido[1]));
                            }
                            ByteActual++;
                        }
                        this.estado_decod = 2;
                    }

                    // Set
                    else if (this.estado_decod == 5)
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                                ((Consola_padel)this.mi_consola).update_periodo(Convert.ToString(byte_leido[1]));
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Bocina (byte leido = 50) y flechas
                    else if (this.estado_decod == 6)
                    {
                        if (longitud > ByteActual)
                        {
                            // 51: flecha visitante, 52: flecha local, 53: sin flechas
                            if (byte_leido == "51 ")
                                ((Consola_padel)this.mi_consola).update_flechas("visitante");
                            else if (byte_leido == "52 ")
                                ((Consola_padel)this.mi_consola).update_flechas("local");
                            else if (byte_leido == "53 ")
                                ((Consola_padel)this.mi_consola).update_flechas("none");    
                            else if ((byte_leido == "60 ") || (byte_leido == "61 "))   // 60: apaga tie break, 61: enciende tie break
                                ((Consola_padel)this.mi_consola).update_tie_break(Convert.ToString(byte_leido[1]));
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    // Puntos local, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 7) || (this.estado_decod == 8) || (this.estado_decod == 9))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 7)
                                    this.digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 8)
                                    this.digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 9)
                                    this.digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 9)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();

                                    if (puntos == "e")      // Llega 40 pero cuando el visitante tambien lleva 40 (irian 40 - 40)
                                        puntos = "40";
                                    else if (puntos == "a") // Advanced
                                        puntos = "Ad";
                                    else if (puntos == "f") // Llega 40 pero cuando el visitante lleva ventaja (irian 40 - Ad)
                                        puntos = "-";
                                    
                                    ((Consola_padel)this.mi_consola).update_puntos_equipo("local", puntos);
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Puntos visitante, los 3 digitos [EQUIPO]
                    else if ((this.estado_decod == 10) || (this.estado_decod == 11) || (this.estado_decod == 12))
                    {
                        if (longitud > ByteActual)
                        {
                            if (byte_leido[0] == '5')
                            {
                                if (this.estado_decod == 10)
                                    digito0 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 11)
                                    digito1 = Convert.ToString(byte_leido[1]);
                                else if (this.estado_decod == 12)
                                    digito2 = Convert.ToString(byte_leido[1]);

                                if (this.estado_decod == 12)
                                {
                                    puntos = compone_string_de_3_digitos();
                                    limpia_digitos();

                                    if (puntos == "e")      // Llega 40 cuando el local tambien lleva 40 (irian 40 - 40)
                                        puntos = "40";
                                    else if (puntos == "a") // Advanced
                                        puntos = "Ad";
                                    else if (puntos == "f") // Llega 40 cuando el local lleva ventaja (irian Ad - 40)
                                        puntos = "-";
                                    
                                    ((Consola_padel)this.mi_consola).update_puntos_equipo("visitante", puntos);
                                }
                            }
                            this.estado_decod = 2;
                            ByteActual++;
                        }
                    }

                    // Sets equipo local y visitante [EQUIPO]
                    else if ((this.estado_decod == 13) || (this.estado_decod == 14))
                    {
                        if (longitud > ByteActual)
                        {
                            if ((Convert.ToInt32(set) >= 1) && (Convert.ToInt32(set) <= 5))
                            {
                                if (this.estado_decod == 13)
                                    ((Consola_padel)this.mi_consola).update_sets_equipo("local", set, compone_string(Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])));
                                else
                                    ((Consola_padel)this.mi_consola).update_sets_equipo("visitante", set, compone_string(Convert.ToString(byte_leido[0]) + Convert.ToString(byte_leido[1])));
                            }
                        }
                        this.estado_decod = 2;
                        ByteActual++;
                    }

                    else
                    {
                        ByteActual++;
                    }
                }
            }
        }
    }

    # endregion


    //# region Interprete_stb

    //// Interprete de STB
    //public class Interprete_stb : Interprete
    //{
    //    // Atributos
    //    SerialPort serial_port;
    //    public int estado_decod = -1;
    //    public byte[] buffer;

    //    string dorsal = "", faltas_local = "", faltas_visitante = "", crono = "", periodo = "", marcador_local = "", marcador_visitante = "";
    //    string segundos_digito0 = "", segundos_digito1 = "", digito2 = "", digito3 = "", minutos_digito1 = "", minutos_digito0 = "";
    //    bool posesion_con_decimal = false, posesion_ultimos_5_sg = false;
    //    int posesion_num_aa = 0;


    //    // Constructor
    //    public Interprete_stb(int id_deporte, string puerto)
    //    {
    //        this.id_deporte = id_deporte;
    //        this.puerto_com = puerto;
    //    }


    //    /*
    //    *  Se conecta al puerto serie
    //    */
    //    public override bool conecta(Consola consola)
    //    {
    //        bool error = true;

    //        this.mi_consola = consola;

    //        // Parametros consola
    //        this.serial_port = new System.IO.Ports.SerialPort();
    //        this.serial_port.BaudRate = 19200;
    //        this.serial_port.DataBits = 8;
    //        this.serial_port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "One");
    //        this.serial_port.Parity = (Parity)Enum.Parse(typeof(Parity), "None");
    //        this.serial_port.PortName = this.puerto_com;
    //        this.serial_port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);

    //        try
    //        {
    //            this.serial_port.Open();
    //        }
    //        catch (UnauthorizedAccessException) { error = false; }
    //        catch (IOException) { error = false; }
    //        catch (ArgumentException) { error = false; }

    //        //if (!error)
    //        //    MessageBox.Show("Error al abrir puerto COM, posiblemente este en uso o no disponible", "Puerto COM no disponible", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        return error;
    //    }


    //    /*
    //    *  Cierra la conexión con la consola
    //    */
    //    public override void desconecta()
    //    {
    //        if (this.serial_port.IsOpen)
    //            this.serial_port.Close();
    //    }


    //    /*
    //     *   Lee los datos recibidos por el puerto serie
    //     */
    //    public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
    //    {
    //        if (!this.serial_port.IsOpen)
    //        {
    //            return;
    //        }
    //        int bytes = this.serial_port.BytesToRead;
    //        buffer = new byte[bytes];
    //        this.serial_port.Read(buffer, 0, bytes);
    //        decod_trama();
    //    }


    //    /*
    //   * Lee un byte de la trama 
    //   */
    //    public byte LeerByte(byte[] trama, int b)
    //    {
    //        byte by = 0x40;
    //        if (trama.Length > b)
    //        {
    //            by = trama[b];
    //        }
    //        return by;
    //    }


    //    /*
    //     * Convierte un byte en un string
    //     */
    //    public string ByteToHexString(byte data)
    //    {
    //        return Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' ');
    //    }



    //    /*
    //     * Convierte un array de Bytes con caracteres en Hexadecimal
    //     */
    //    public string ByteArrayToHexString(byte[] data)
    //    {
    //        StringBuilder sb = new StringBuilder(data.Length * 3);
    //        foreach (byte b in data)
    //            sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
    //        return sb.ToString().ToUpper();
    //    }


    //    /*
    //     *  Devuelve el valor decimal correspondiente al valor hexadecimal que llega como parametro
    //     */
    //    public string traduce_hexadecimal_a_decimal(string valor_hexadecimal)
    //    {
    //        string valor_decimal = "";

    //        if (valor_hexadecimal == "a")
    //            valor_decimal = "10";
    //        else if (valor_hexadecimal == "b")
    //            valor_decimal = "11";
    //        else if (valor_hexadecimal == "c")
    //            valor_decimal = "12";
    //        else if (valor_hexadecimal == "d")
    //            valor_decimal = "13";
    //        else if (valor_hexadecimal == "e")
    //            valor_decimal = "14";
    //        else if (valor_hexadecimal == "f")
    //            valor_decimal = "15";
    //        else
    //            valor_decimal = valor_hexadecimal;

    //        return valor_decimal;
    //    }


    //    /*
    //     *  Devuelve el valor decimal correspondiente al valor hexadecimal que llega como parametro (dorsal)
    //     */
    //    public string traduce_dorsal(string valor_hexadecimal)
    //    {
    //        string valor_decimal = "";

    //        if (valor_hexadecimal == "a")
    //            valor_decimal = "10";
    //        else if (valor_hexadecimal == "b")
    //            valor_decimal = "11";
    //        else if (valor_hexadecimal == "c")
    //            valor_decimal = "12";
    //        else if (valor_hexadecimal == "d")
    //            valor_decimal = "13";
    //        else if (valor_hexadecimal == "e")
    //            valor_decimal = "14";
    //        else if (valor_hexadecimal == "f")
    //            valor_decimal = "15";
    //        else if (valor_hexadecimal == "0")
    //            valor_decimal = "16";
    //        else if (valor_hexadecimal == "1")
    //            valor_decimal = "17";
    //        else
    //            valor_decimal = valor_hexadecimal;

    //        return valor_decimal;
    //    }


    //    /*
    //     * Metodo que limpia las variables globles de los digitos
    //     */
    //    public void limpia_digitos()
    //    {
    //        this.segundos_digito0 = "";
    //        this.segundos_digito1 = "";
    //        this.digito2 = "";
    //        this.minutos_digito1 = "";
            
    //        //this.digito3 = "";
    //    }


    //    /*
    //    * Compone un string a partir de los 2 digitos por separado
    //    */
    //    public string compone_string_de_2_digitos()
    //    {
    //        if (this.segundos_digito1 == "f" || this.segundos_digito1 == "" || this.segundos_digito1 == "0")
    //            return this.segundos_digito0;
    //        else
    //            return this.segundos_digito1 + this.segundos_digito0;
    //    }


    //    /*
    //    * Compone un string a partir de los 3 digitos por separado
    //    */
    //    public string compone_string_de_3_digitos()
    //    {
    //        if ((this.digito2 == "0" || this.digito2 == "f" | this.digito2 == "") && (this.segundos_digito1 == "0" || this.segundos_digito1 == "f" || this.segundos_digito1 == ""))
    //            return this.segundos_digito0;
    //        else if (this.digito2 == "0" || this.digito2 == "f")
    //            return this.segundos_digito1 + this.segundos_digito0;
    //        else
    //            return this.digito2 + this.segundos_digito1 + this.segundos_digito0;
    //    }


    //    /*
    //    * Compone un string a partir de los 4 digitos por separado
    //    */
    //    public string compone_string_de_4_digitos()
    //    {
    //        return (this.digito3 + this.digito2 + ":" + this.segundos_digito1 + this.segundos_digito0);
    //    }


    //    /*
    //     * Traduce los puntos hexdecimal a string y elimina los posibles "0" y "f" que pueden llegar en el valor hexadecimal
    //     */
    //    public string compone_string(string cadena)
    //    {
    //        if (cadena.Length > 1)
    //        {
    //            //if (cadena[0] == '0')
    //            //{
    //            if (Convert.ToString(cadena[1]) != "f")
    //                return traduce_hexadecimal_a_decimal(Convert.ToString(cadena[1]));
    //            else
    //                return "0";
    //            //}
    //        }
    //        return cadena;
    //    }


    //    /*
    //     * Devuelve el string del crono de la posesion a partir de sus digitos
    //     */
    //    private string compone_crono_posesion(bool con_decimal, string crono_sin_traducir)
    //    {
    //        if (!con_decimal)
    //        {
    //            // Sin decimal
    //            return crono_sin_traducir.Replace('f', '0');
    //        }
    //        else
    //        {
    //            // Con decimal
    //            return crono_sin_traducir[0] + "." + crono_sin_traducir[1];
    //        }
    //    }


    //    /*
    //     * Metodo que decodifica las tramas dependiendo del deporte
    //     */
    //    public override void decod_trama(string trama = "")
    //    {
    //        switch (this.id_deporte)
    //        {
    //            case 1:     // Baloncesto
    //                decod_trama_baloncesto();
    //                break;
    //            case 4:     // Waterpolo
    //                decod_trama_waterpolo();
    //                break;
    //        }
    //    }


    //    /*
    //     * Metodo que decodifica las tramas del baloncesto
    //     */
    //    public void decod_trama_baloncesto()
    //    {
    //        int longitud = this.buffer.Length;
    //        int ByteActual = 0;
    //        string puntos = "", crono = "", periodo = "", marcador_local = "", marcador_visitante = "", byte_leido = "", crono_posesion = "";
    //        string tanteo_local = "", faltas_local = "", faltas_visitante = "";
    //        bool primer_digito = true, primer_30 = true;


    //        string trama = ByteArrayToHexString(this.buffer);
    //        //Console.WriteLine("Trama interprete STB: " + trama);

    //        while (ByteActual < longitud)
    //        {
    //            byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));
    //            //Console.WriteLine("Byte leido: " + byte_leido);

    //            if (byte_leido.Length > 1)
    //            {
    //                if (this.estado_decod == -1)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        // Empieza trama
    //                        if (byte_leido == "00 ")
    //                        {
    //                            this.estado_decod = 1;
    //                        }
    //                        ByteActual++;
    //                    }
    //                }

    //                // Mira la info que le envia
    //                else if (this.estado_decod == 1)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        //Console.WriteLine("Byte leido: " + byte_leido);

    //                        if (byte_leido == "11 ") // Crono
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 3;
    //                            //Console.WriteLine(">>>>>>>>>> CRONO");
    //                        }
    //                        else if (byte_leido == "12 ") // Crono en el ultimo minuto
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 9;
    //                            //Console.WriteLine(">>>>>>>>>> CRONO ULTIMO MINUTO");
    //                        }
    //                        else if (byte_leido == "15 ") // Periodo
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 17;
    //                            //Console.WriteLine(">>>>>>>>>> PERIODO");
    //                        }
    //                        else if (byte_leido == "17 ") // Marcador local
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 18;
    //                            //Console.WriteLine(">>>>>>>>>> MARCADOR LOCAL");
    //                        }
    //                        else if (byte_leido == "1f ") // Marcador visitante
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 22;
    //                            //Console.WriteLine(">>>>>>>>>> MARCADOR VISITANTE");
    //                        }
    //                        else if (byte_leido == "0e ") // Faltas local
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 26;
    //                            //Console.WriteLine(">>>>>>>>>> FALTAS LOCAL");
    //                        }
    //                        else if (byte_leido == "0f ") // Faltas visitante
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 29;
    //                            //Console.WriteLine(">>>>>>>>>> FALTAS VISITANTE");
    //                        }

    //                        else
    //                        {
    //                            //Console.WriteLine("Byte leido: " + byte_leido);
    //                            ByteActual++;
    //                        }
    //                    }
    //                }

    //                // Fin de trama
    //                else if (this.estado_decod == 2)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        if (byte_leido == "03 ")
    //                            this.estado_decod = -1;
    //                        ByteActual++;
    //                    }
    //                }
                        
    //                else if (this.estado_decod == 3)    // lector delante de 30
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.minutos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 4;
    //                }
    //                else if (this.estado_decod == 4)    // lector delante de 3X (minutos)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.minutos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 5;
    //                }
    //                else if (this.estado_decod == 5)    // lector delante de 3A (:)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 6;
    //                }
    //                else if (this.estado_decod == 6)    // lector delante de 3X (digito 1 de los segundos)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 7;
    //                }
    //                else if (this.estado_decod == 7)    // lector delante de 3X (digito 0 de los segundos)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 8;
    //                }

    //                // Crono
    //                else if (this.estado_decod == 8)
    //                {
    //                    crono = this.minutos_digito1 + this.minutos_digito0 + ":" + this.segundos_digito1 + this.segundos_digito0;
    //                    if (crono != this.crono)
    //                    {
    //                        this.crono = crono;
    //                        //Console.WriteLine(this.crono);

    //                        ((Consola_baloncesto)this.mi_consola).update_crono(this.crono);
    //                        limpia_digitos();
    //                    }

    //                    ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Crono en el ultimo minuto
    //                else if (this.estado_decod == 9)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 10;
    //                }
    //                else if (this.estado_decod == 10)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 11;
    //                }
    //                else if (this.estado_decod == 11)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 12;
    //                }
    //                else if (this.estado_decod == 12)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.digito2 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 13;
    //                }
    //                else if (this.estado_decod == 13)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 14;
    //                }
    //                else if (this.estado_decod == 14)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 15;
    //                }
    //                else if (this.estado_decod == 15)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 16;
    //                }
    //                else if (this.estado_decod == 16)
    //                {
    //                    crono = this.digito2 + this.segundos_digito1 + "." + this.segundos_digito0;
    //                    if (crono != this.crono)
    //                    {
    //                        this.crono = crono;
    //                        //Console.WriteLine("ultimo minuto: " + this.crono);

    //                        ((Consola_baloncesto)this.mi_consola).update_crono(this.crono);
    //                        limpia_digitos();
    //                    }

    //                    ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Periodo
    //                else if (this.estado_decod == 17)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        if (byte_leido[0] == '3')
    //                        {
    //                            periodo = Convert.ToString(byte_leido[1]);
    //                            if (periodo != this.periodo)
    //                            {
    //                                this.periodo = periodo;
    //                                if (this.periodo == "a")
    //                                    this.periodo = "E";
    //                                ((Consola_baloncesto)this.mi_consola).update_periodo(this.periodo);
    //                                //Console.WriteLine("Periodo: " + this.periodo);
    //                            }
    //                        }
    //                        ByteActual++;
    //                        this.estado_decod = 1;
    //                    }
    //                }

    //                // Marcador local
    //                else if (this.estado_decod == 18)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 19;
    //                }
    //                else if (this.estado_decod == 19)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 20;
    //                }
    //                else if (this.estado_decod == 20)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 21;
    //                }
    //                else if (this.estado_decod == 21)
    //                {
    //                    marcador_local = compone_string_de_2_digitos();
    //                    if (marcador_local != this.marcador_local)
    //                    {
    //                        this.marcador_local = marcador_local;
    //                        //Console.WriteLine("Marcador local: "+ this.marcador_local);

    //                        ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("local", this.marcador_local);
    //                        limpia_digitos();
    //                    }

    //                    //ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Marcador visitante
    //                else if (this.estado_decod == 22)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 23;
    //                }
    //                else if (this.estado_decod == 23)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 24;
    //                }
    //                else if (this.estado_decod == 24)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 25;
    //                }
    //                else if (this.estado_decod == 25)
    //                {
    //                    marcador_visitante = compone_string_de_2_digitos();
    //                    if (marcador_visitante != this.marcador_visitante)
    //                    {
    //                        this.marcador_visitante = marcador_visitante;
    //                        //Console.WriteLine("Marcador visitante: " + this.marcador_visitante);

    //                        ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("visitante", this.marcador_visitante);
    //                        limpia_digitos();
    //                    }

    //                    //ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Faltas equipo local
    //                else if (this.estado_decod == 26)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 27;
    //                }
    //                else if (this.estado_decod == 27)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 28;
    //                }
    //                else if (this.estado_decod == 28)
    //                {
    //                    faltas_local = compone_string_de_2_digitos();
    //                    if (faltas_local != this.faltas_local)
    //                    {
    //                        this.faltas_local = faltas_local;
    //                        //Console.WriteLine("Faltas local: "+ this.faltas_local);

    //                        ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("local", this.faltas_local);
    //                        limpia_digitos();
    //                    }

    //                    //ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Faltas equipo visitante
    //                else if (this.estado_decod == 29)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 30;
    //                }
    //                else if (this.estado_decod == 30)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 31;
    //                }
    //                else if (this.estado_decod == 31)
    //                {
    //                    faltas_visitante = compone_string_de_2_digitos();
    //                    if (faltas_visitante != this.faltas_visitante)
    //                    {
    //                        this.faltas_visitante = faltas_visitante;
    //                        //Console.WriteLine("Faltas visitante: "+ this.faltas_visitante);

    //                        ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("visitante", this.faltas_visitante);
    //                        limpia_digitos();
    //                    }

    //                    //ByteActual++;
    //                    this.estado_decod = 1;
    //                }


    //                else
    //                {
    //                    ByteActual++;
    //                }
    //            }
    //        }
    //    }

        
    //    /*
    //     * Metodo que decodifica las tramas del waterpolo
    //     */
    //    public void decod_trama_waterpolo()
    //    {
    //        int longitud = this.buffer.Length;
    //        int ByteActual = 0;
    //        string puntos = "", crono = "", periodo = "", marcador_local = "", marcador_visitante = "", byte_leido = "", crono_posesion = "";
    //        string tanteo_local = "";
    //        bool primer_digito = true, primer_30 = true;
           

    //        string trama = ByteArrayToHexString(this.buffer);
    //        //Console.WriteLine("Trama interprete STB: " + trama);

    //        while (ByteActual < longitud)
    //        {
    //            byte_leido = ByteToHexString(LeerByte(this.buffer, ByteActual));
    //            //Console.WriteLine("Byte leido: " + byte_leido);

    //            if (byte_leido.Length > 1)
    //            {
    //                if (this.estado_decod == -1)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        // Empieza trama
    //                        if (byte_leido == "00 ")
    //                        {
    //                            this.estado_decod = 1;
    //                        }                           
    //                        ByteActual++;
    //                    }
    //                }

    //                // Mira la info que le envia
    //                else if (this.estado_decod == 1)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        //Console.WriteLine("Byte leido: " + byte_leido);

    //                        if (byte_leido == "11 ") // Crono
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 3;
    //                            //Console.WriteLine(">>>>>>>>>> CRONO");
    //                        }
    //                        else if (byte_leido == "12 ") // Crono en el ultimo minuto
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 9;
    //                            //Console.WriteLine(">>>>>>>>>> CRONO ULTIMO MINUTO");
    //                        }
    //                        else if (byte_leido == "15 ") // Periodo
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 17;
    //                            //Console.WriteLine(">>>>>>>>>> PERIODO");
    //                        }
    //                        else if (byte_leido == "17 ") // Marcador local
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 18;
    //                            //Console.WriteLine(">>>>>>>>>> MARCADOR LOCAL");
    //                        }
    //                        else if (byte_leido == "1f ") // Marcador visitante
    //                        {
    //                            ByteActual++;
    //                            this.estado_decod = 22;
    //                            //Console.WriteLine(">>>>>>>>>> MARCADOR VISITANTE");
    //                        }
                                
    //                        else
    //                        {
    //                            ByteActual++;
    //                        }                           
    //                    }
    //                }

    //                // Fin de trama
    //                else if (this.estado_decod == 2)
    //                {
    //                    //Console.WriteLine("************************ estado 2");

    //                    if (longitud > ByteActual)
    //                    {
    //                        if (byte_leido == "03 ")
    //                            this.estado_decod = -1;
    //                        ByteActual++;
    //                    }
    //                }

                        

    //                else if (this.estado_decod == 3)    // lector delante de 30
    //                {
    //                    ByteActual++;
    //                    this.minutos_digito0 = Convert.ToString(byte_leido[1]);
    //                    this.estado_decod = 4;                       
    //                }
    //                else if (this.estado_decod == 4)    // lector delante de 3X (minutos)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.minutos_digito1 = Convert.ToString(byte_leido[1]);
                        
    //                    this.estado_decod = 5;
    //                }
    //                else if (this.estado_decod == 5)    // lector delante de 3A (:)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 6;                        
    //                }
    //                else if (this.estado_decod == 6)    // lector delante de 3X (digito 1 de los segundos)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);
                                                
    //                    this.estado_decod = 7;
    //                }
    //                else if (this.estado_decod == 7)    // lector delante de 3X (digito 0 de los segundos)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);
                        
    //                    this.estado_decod = 8;
    //                }                                        

    //                // Crono
    //                else if (this.estado_decod == 8)
    //                {
    //                    crono = "0" + this.minutos_digito1 + ":" + this.segundos_digito1 + this.segundos_digito0;
    //                    if (crono != this.crono)
    //                    {
    //                        this.crono = crono;
    //                        Console.WriteLine(this.crono);
                                                        
    //                        ((Consola_waterpolo)this.mi_consola).update_crono(this.crono);
    //                        limpia_digitos();
    //                    }
                        
    //                    ByteActual++;
    //                    this.estado_decod = 1;
    //                }
                        
    //                // Crono en el ultimo minuto
    //                else if (this.estado_decod == 9) 
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 10;
    //                }
    //                else if (this.estado_decod == 10)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 11;
    //                }
    //                else if (this.estado_decod == 11)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 12;
    //                }
    //                else if (this.estado_decod == 12)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.digito2 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 13;
    //                }
    //                else if (this.estado_decod == 13)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 14;
    //                }
    //                else if (this.estado_decod == 14)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 15;
    //                }
    //                else if (this.estado_decod == 15)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 16;
    //                }
    //                else if (this.estado_decod == 16)
    //                {
    //                    crono = this.digito2 + this.segundos_digito1 + "." + this.segundos_digito0;
    //                    if (crono != this.crono)
    //                    {
    //                        this.crono = crono;
    //                        //Console.WriteLine("ultimo minuto: " + this.crono);
                                                        
    //                        ((Consola_waterpolo)this.mi_consola).update_crono(this.crono);
    //                        limpia_digitos();
    //                    }

    //                    ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Periodo
    //                else if (this.estado_decod == 17)
    //                {
    //                    if (longitud > ByteActual)
    //                    {
    //                        if (byte_leido[0] == '3')
    //                        {
    //                            periodo = Convert.ToString(byte_leido[1]);
    //                            if (periodo != this.periodo)
    //                            {
    //                                this.periodo = periodo;
    //                                if (this.periodo == "a")
    //                                    this.periodo = "E";
    //                                ((Consola_waterpolo)this.mi_consola).update_periodo(this.periodo);
    //                                //Console.WriteLine("Periodo: " + this.periodo);
    //                            }
    //                        }
    //                        ByteActual++;
    //                        this.estado_decod = 1;
    //                    }
    //                }

    //                // Marcador local
    //                else if (this.estado_decod == 18)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 19;
    //                }
    //                else if (this.estado_decod == 19) 
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);
                                                
    //                    this.estado_decod = 20;
    //                }
    //                else if (this.estado_decod == 20)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);
                        
    //                    this.estado_decod = 21;
    //                }
    //                else if (this.estado_decod == 21)
    //                {
    //                    marcador_local = compone_string_de_2_digitos();
    //                    if (marcador_local != this.marcador_local)
    //                    {
    //                        this.marcador_local = marcador_local;
    //                        //Console.WriteLine("Marcador local: "+ this.marcador_local);
                                                        
    //                        ((Consola_waterpolo)this.mi_consola).update_puntos_equipo("local", this.marcador_local);
    //                        limpia_digitos();
    //                    }
                        
    //                    //ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                // Marcador visitante
    //                else if (this.estado_decod == 22)
    //                {
    //                    ByteActual++;
    //                    this.estado_decod = 23;
    //                }
    //                else if (this.estado_decod == 23)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito1 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 24;
    //                }
    //                else if (this.estado_decod == 24)
    //                {
    //                    ByteActual++;
    //                    if (byte_leido[0] == '3')
    //                        this.segundos_digito0 = Convert.ToString(byte_leido[1]);

    //                    this.estado_decod = 25;
    //                }
    //                else if (this.estado_decod == 25)
    //                {
    //                    marcador_visitante = compone_string_de_2_digitos();
    //                    if (marcador_visitante != this.marcador_visitante)
    //                    {
    //                        this.marcador_visitante = marcador_visitante;
    //                        //Console.WriteLine("Marcador visitante: " + this.marcador_visitante);

    //                        ((Consola_waterpolo)this.mi_consola).update_puntos_equipo("visitante", this.marcador_visitante);
    //                        limpia_digitos();
    //                    }

    //                    //ByteActual++;
    //                    this.estado_decod = 1;
    //                }

    //                else
    //                {
    //                    ByteActual++;
    //                }                
    //            }
    //        }
    //    }

    //}

    //# endregion


    //# region Interprete_nautronic

    //// Interprete de Nautronic
    //public class Interprete_nautronic : Interprete
    //{
    //    // Atributos
    //    private NauCom NC;
    //    //SerialPort serial_port;
    //    public int estado_decod = -1;
    //    public byte[] buffer;

    //    string crono_anterior = "", crono_posesion_anterior = "";
    //    bool primera_trama_dorsales_recibida = false;
    //    //int posesion_num_aa = 0;


    //    // Constructor
    //    public Interprete_nautronic(int id_deporte, string puerto)
    //    {
    //        this.id_deporte = id_deporte;
    //        this.puerto_com = puerto;
    //    }

    //    /*
    //    *  Se conecta al puerto serie
    //    */
    //    public override bool conecta(Consola consola)
    //    {
    //        bool error = true;
    //        //Thread thread_consola;
    //        System.Timers.Timer timer_consola;

    //        this.mi_consola = consola;
    //        try
    //        {
    //            this.NC = new NauCom(this.puerto_com, NauBeeChannels.CHANNEL_0, true);

    //            // Create a timer and set a two second interval.
    //            timer_consola = new System.Timers.Timer();
    //            timer_consola.Interval = 250;

    //            // Hook up the Elapsed event for the timer. 
    //            timer_consola.Elapsed += get_estado_consola;

    //            // Have the timer fire repeated events (true is the default)
    //            timer_consola.AutoReset = true;

    //            // Start the timer
    //            timer_consola.Enabled = true;


    //            //NC = new NauCom(this.puerto_com, NauBeeChannels.CHANNEL_0, true);
    //            //NC.NauBeeDigitDotRelayUpdate += new NauCom.NauBeeDataUpdateEventHandler(NC_NauBeeDigitDotRelayUpdate);
    //            ////NC.NauBeeTextUpdate += new NauCom.NauBeeTextUpdateEventHandler(NC_NauBeeTextUpdate);

    //            //ThreadStart ts = delegate { decod_trama(""); };
    //            //thread_consola = new Thread(ts);
    //            //thread_consola.Start();    
    //        }
           
    //        catch(Exception e)
    //        {
    //            error = false;
    //        }   

    //        return error;
    //    }


    //    /*
    //    *  Cierra la conexión con la consola
    //    */
    //    public override void desconecta()
    //    {
    //        this.NC.Stop();
    //    }


    //    //static void NC_NauBeeTextUpdate(int Adress, string Text)
    //    //{
    //    //    Console.WriteLine("NC_NauBeeTextUpdate: " + Text);
    //    //    //throw new NotImplementedException();
    //    //}

    //    private void NC_NauBeeDigitDotRelayUpdate(EventType Type, int[] Adresses)
    //    {
    //        //bool flash, visible, extraFlash;
    //        int valor_digito = -1;


    //        if (Type == EventType.Digit)
    //        {
    //            string Data = "";
    //            for (int i = 0; i < Adresses.Length; i++)
    //            {
    //                int Value;
    //                bool Flash, ExtraVisble, ExtraFlash;
    //                NC.GetDigit(Adresses[i], out Value, out Flash, out ExtraVisble, out ExtraFlash);

    //                Console.WriteLine(Adresses[i]);

    //                if (Adresses[i] == 137)
    //                    Console.WriteLine(Adresses[i] + " - " + Value);
    //                if (Adresses[i] == 64)
    //                    Console.WriteLine(Adresses[i] + " - " + Value);

    //                //if ((Adresses[i] == 12) || (Adresses[i] == 13) || (Adresses[i] == 14) || (Adresses[i] == 15))
    //                //{
    //                //    valor_digito = Value;
    //                //    //get_crono(Adresses[i], valor_digito);
    //                //}

    //                if (Value != 15)
    //                    Data += Value.ToString();
    //            }
    //            //Console.WriteLine("NC_NauBeeDigitDotRelayUpdate: " + Type.ToString() + "_" + Adresses[0].ToString() + "_" + Data.ToString());
    //        }
    //        //throw new NotImplementedException();
    //    }


                
    //    private void get_estado_consola(Object source, ElapsedEventArgs e)
    //    {
    //        if (!this.primera_trama_dorsales_recibida)
    //        {
    //            get_dorsales_local();
    //            this.primera_trama_dorsales_recibida = true;
    //            get_dorsales_visitante();
    //        }

    //        //get_periodo();
    //        //get_crono();
    //        //get_crono_posesion();
    //        //get_flechas();

    //        //get_tiempos_muertos_local();
    //        //get_tiempos_muertos_visitante();

    //        get_puntos_local();
    //        //get_faltas_local();
    //        get_puntos_jugadores_local();
    //        //get_faltas_jugadores_local();

    //        //get_puntos_visitante();
    //        //get_faltas_visitante();
    //        //get_puntos_jugadores_visitante();
    //        //get_faltas_jugadores_visitante();
    //    }


    //    /*
    //     * Método que consigue el periodo
    //     */ 
    //    private void get_periodo()
    //    {
    //        string periodo = "";
    //        int digito0 = -1, digito1 = -1;
    //        bool Flash, ExtraVisble, ExtraFlash;

    //        this.NC.GetDigit(6, out digito0, out Flash, out ExtraVisble, out ExtraFlash);
    //        this.NC.GetDigit(7, out digito1, out Flash, out ExtraVisble, out ExtraFlash);

    //        digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //        digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //        //Console.WriteLine(digito0);
    //        //Console.WriteLine(digito1);
    //        //Console.WriteLine("===================");

    //        if (digito0 == 0)
    //        {
    //            if (digito1 == 14)
    //                periodo = "E";
    //            else
    //                periodo = Convert.ToString(digito1);
    //        }
    //        else
    //            periodo = Convert.ToString(digito1) + Convert.ToString(digito0);

    //        ((Consola_baloncesto)this.mi_consola).update_periodo(periodo);
    //    }



    //    /*
    //     * Método que consigue el crono general del partido
    //     */ 
    //    private void get_crono()
    //    {
    //        string crono = "";
    //        int digito0 = -1, digito1 = -1, digito2 = -1, digito3 = -1;
    //        bool Flash, ExtraVisble, ExtraFlash;

    //        this.NC.GetDigit(12, out digito0, out Flash, out ExtraVisble, out ExtraFlash);
    //        this.NC.GetDigit(13, out digito1, out Flash, out ExtraVisble, out ExtraFlash);
    //        this.NC.GetDigit(14, out digito2, out Flash, out ExtraVisble, out ExtraFlash);
    //        this.NC.GetDigit(15, out digito3, out Flash, out ExtraVisble, out ExtraFlash);

    //        //Console.WriteLine(digito0);
    //        //Console.WriteLine(digito1);
    //        //Console.WriteLine(digito2);
    //        //Console.WriteLine(digito3);
    //        //Console.WriteLine("=====================================");


    //        if ((digito0 == 15) && (digito3 == 15))   // 09.9
    //            crono = "0" + Convert.ToString(digito1) + "." + Convert.ToString(digito2);
    //        else if (digito0 == 15) // 09:59
    //            crono = "0" + Convert.ToString(digito1) + ":" + Convert.ToString(digito2) + Convert.ToString(digito3);
    //        else
    //        {
    //            if (digito3 != 15)  // 10:00
    //                crono = Convert.ToString(digito0) + Convert.ToString(digito1) + ":" + Convert.ToString(digito2) + Convert.ToString(digito3);
    //            else    // 59.9
    //                crono = digito0 + "" + digito1 + "." + digito2;
    //        }

    //        if (crono != this.crono_anterior)
    //        {
    //            ((Consola_baloncesto)this.mi_consola).update_crono(crono);
    //            this.crono_anterior = crono;

    //            //Console.WriteLine("Enviado crono: " + crono);
    //            //Console.WriteLine("*************************");
    //        }
    //    }


    //    /*
    //     * Método que consigue el crono de posesión
    //     */
    //    private void get_crono_posesion()
    //    {
    //        string crono_posesion = "";
    //        int digito0 = -1, digito1 = -1, digito2 = -1;
    //        bool Flash, ExtraVisble, ExtraFlash;

    //        this.NC.GetDigit(202, out digito0, out Flash, out ExtraVisble, out ExtraFlash);
    //        this.NC.GetDigit(203, out digito1, out Flash, out ExtraVisble, out ExtraFlash);
    //        this.NC.GetDigit(204, out digito2, out Flash, out ExtraVisble, out ExtraFlash);
           
    //        //Console.WriteLine(digito0);
    //        //Console.WriteLine(digito1);
    //        //Console.WriteLine(digito2);
    //        //Console.WriteLine("=====================================");

    //        //digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //        //digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //        if (digito2 == 15)  // 24
    //            crono_posesion = Convert.ToString(digito0).Replace("0", "") + digito1;
    //        else if (digito2 == 1)  // 4.9
    //            crono_posesion = digito0 + "." + digito1;

    //        if (crono_posesion != this.crono_posesion_anterior)
    //        {
    //            ((Consola_baloncesto)this.mi_consola).update_crono_posesion(crono_posesion);
    //            this.crono_posesion_anterior = crono_posesion;

    //            //Console.WriteLine("Enviado crono: " + crono_posesion);
    //            //Console.WriteLine("*************************");
    //        }
    //    }



    //    /*
    //     * Método que consigue la flecha de posesión de balón
    //     */ 
    //    private void get_flechas()
    //    {
    //        int digito0 = -1, flash_bit = -1;

    //        this.NC.GetDot(137, out digito0, out flash_bit);

    //        if (digito0 == 0)
    //            ((Consola_baloncesto)this.mi_consola).update_flechas("none");
    //        else if (digito0 == 243)
    //            ((Consola_baloncesto)this.mi_consola).update_flechas("local");
    //        else if (digito0 == 252)
    //            ((Consola_baloncesto)this.mi_consola).update_flechas("visitante");
    //    }


    //    /*
    //     * Método que consigue los tiempos muertos del equipo local
    //     */
    //    private void get_tiempos_muertos_local()
    //    {
    //        int digito0 = -1, flash_bit = -1;

    //        this.NC.GetDot(0, out digito0, out flash_bit);

    //        if (digito0 == 1)
    //            ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("local", "1");
    //        else if (digito0 == 5)
    //            ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("local", "2");
    //        else if (digito0 == 21)
    //            ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("local", "3");
    //    }


    //    /*
    //     * Método que consigue los tiempos muertos del equipo visitante
    //     */
    //    private void get_tiempos_muertos_visitante()
    //    {
    //        int digito0 = -1, flash_bit = -1;

    //        this.NC.GetDot(128, out digito0, out flash_bit);

    //        if (digito0 == 1)
    //            ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("visitante", "1");
    //        else if (digito0 == 5)
    //            ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("visitante", "2");
    //        else if (digito0 == 21)
    //            ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("visitante", "3");
    //    }



    //    /*
    //     * Método que consigue los puntos del equipo local
    //     */
    //    private void get_puntos_local()
    //    {
    //        string marcador = "";
    //        int digito0 = -1, digito1 = -1, digito2 = -1;
    //        bool flash, visible, extraFlash;

    //        this.NC.GetDigit(0, out digito0, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(1, out digito1, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(2, out digito2, out flash, out visible, out extraFlash);

    //        digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //        digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));
    //        digito2 = Convert.ToInt32(Convert.ToString(digito2).Replace("15", "0"));

    //        //Console.WriteLine("LOCAL");
    //        //Console.WriteLine(Convert.ToString(digito0));
    //        //Console.WriteLine(Convert.ToString(digito1));
    //        //Console.WriteLine(Convert.ToString(digito2));
    //        //Console.WriteLine("=======================");

    //        if (digito0 == 0)
    //        {
    //            if (digito2 == 0)   // Marcador de un dígito
    //                marcador = Convert.ToString(digito1);
    //            else  // Marcador de tres dígitos
    //                marcador = Convert.ToString(digito2) + Convert.ToString(digito0) + Convert.ToString(digito1);
    //        }
    //        else  // Marcador de dos dígitos
    //            marcador = Convert.ToString(digito0) + Convert.ToString(digito1);

    //        ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("local", marcador);
    //    }


    //    /*
    //     * Método que consigue los puntos del equipo visitante
    //     */
    //    private void get_puntos_visitante()
    //    {
    //        string marcador = "";
    //        int digito0 = -1, digito1 = -1, digito2 = -1;
    //        bool flash, visible, extraFlash;

    //        this.NC.GetDigit(128, out digito0, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(129, out digito1, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(130, out digito2, out flash, out visible, out extraFlash);

    //        digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //        digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));
    //        digito2 = Convert.ToInt32(Convert.ToString(digito2).Replace("15", "0"));

    //        //Console.WriteLine("VISITANTE");
    //        //Console.WriteLine(Convert.ToString(digito0));
    //        //Console.WriteLine(Convert.ToString(digito1));
    //        //Console.WriteLine(Convert.ToString(digito2));
    //        //Console.WriteLine("=======================");

    //        if (digito0 == 0)
    //        {
    //            if (digito2 == 0)   // Marcador de un dígito
    //                marcador = Convert.ToString(digito1);
    //            else  // Marcador de tres dígitos
    //                marcador = Convert.ToString(digito2) + Convert.ToString(digito0) + Convert.ToString(digito1);
    //        }
    //        else  // Marcador de dos dígitos
    //            marcador = Convert.ToString(digito0) + Convert.ToString(digito1);

    //        ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("visitante", marcador);
    //    }


    //    /*
    //     * Método que consigue las faltas del equipo local
    //     */
    //    private void get_faltas_local()
    //    {
    //        string dorsal = "", num_faltas = "";
    //        int digito0 = -1, digito1 = -1, digito2 = -1;
    //        bool flash, visible, extraFlash;

    //        this.NC.GetDigit(19, out digito0, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(20, out digito1, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(21, out digito2, out flash, out visible, out extraFlash);

    //        digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //        digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));
    //        digito2 = Convert.ToInt32(Convert.ToString(digito2).Replace("15", "0"));

    //        //Console.WriteLine("FALTAS LOCAL");
    //        //Console.WriteLine(Convert.ToString(digito0));
    //        //Console.WriteLine(Convert.ToString(digito1));
    //        //Console.WriteLine(Convert.ToString(digito2));
    //        //Console.WriteLine("=======================");

    //        if ((digito1 == 0) && (digito2 == 0))
    //        {
    //            // Faltas de equipo (1 0 0 -> 1 Falta de equipo)
    //            num_faltas = Convert.ToString(digito0);
    //            ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("local", num_faltas);
    //        }
    //        //else
    //        //{
    //        //    // Faltas de jugador (1 0 4 -> 1 Falta del jugador con dorsal 4)
    //        //    dorsal = Convert.ToString(digito1).Replace("0", "") + Convert.ToString(digito2);
    //        //    num_faltas = Convert.ToString(digito0);
    //        //    ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("local", dorsal, num_faltas);

    //        //    Console.WriteLine("Dorsal: " + dorsal + " - " + num_faltas + " faltas");
    //        //}
    //    }


    //    /*
    //     * Método que consigue las faltas del equipo visitante
    //     */
    //    private void get_faltas_visitante()
    //    {
    //        string dorsal = "", num_faltas = "";
    //        int digito0 = -1, digito1 = -1, digito2 = -1;
    //        bool flash, visible, extraFlash;

    //        this.NC.GetDigit(147, out digito0, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(148, out digito1, out flash, out visible, out extraFlash);
    //        this.NC.GetDigit(149, out digito2, out flash, out visible, out extraFlash);

    //        digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //        digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));
    //        digito2 = Convert.ToInt32(Convert.ToString(digito2).Replace("15", "0"));

    //        //Console.WriteLine("FALTAS VISITANTE");
    //        //Console.WriteLine(Convert.ToString(digito0));
    //        //Console.WriteLine(Convert.ToString(digito1));
    //        //Console.WriteLine(Convert.ToString(digito2));
    //        //Console.WriteLine("=======================");

    //        if ((digito1 == 0) && (digito2 == 0))
    //        {
    //            // Faltas de equipo (1 0 0 -> 1 Falta de equipo)
    //            num_faltas = Convert.ToString(digito0);
    //            ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("visitante", num_faltas);
    //        }
    //        //else
    //        //{
    //        //    // Faltas de jugador (1 0 4 -> 1 Falta del jugador con dorsal 4)
    //        //    dorsal = Convert.ToString(digito1).Replace("0", "") + Convert.ToString(digito2);
    //        //    num_faltas = Convert.ToString(digito0);
    //        //    ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("visitante", dorsal, num_faltas);
    //        //}
    //    }


    //     /*
    //     * Método que consigue los dorsales del equipo local
    //     */
    //    private void get_dorsales_local()
    //    {
    //        string dorsal = "";
    //        int digito0 = -1, digito1 = -1, direccion = -1, dorsal_anterior = -1;
    //        bool flash, visible, extraFlash;

    //        //Console.WriteLine("LOCALES");

    //        dorsal_anterior = 4;
    //        direccion = 32;
    //        for (int i = 0; i < 12; i++)
    //        {
    //            this.NC.GetDigit(direccion, out digito0, out flash, out visible, out extraFlash);
    //            this.NC.GetDigit(direccion + 1, out digito1, out flash, out visible, out extraFlash);

    //            digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //            digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //            dorsal = Convert.ToString(digito0).Replace("0", "") + digito1;
    //            ((Consola_baloncesto)this.mi_consola).update_dorsales_jugadores("local", Convert.ToString(dorsal_anterior), dorsal);
                                
    //            //Console.WriteLine("local : " + Convert.ToString(dorsal_anterior) + " -> " + dorsal);

    //            dorsal_anterior = dorsal_anterior + 1;
    //            direccion = direccion + 2;
    //        }
    //        //Console.WriteLine("**********************************************");
    //    }


    //    /*
    //     * Método que consigue los dorsales del equipo visitante
    //     */
    //    private void get_dorsales_visitante()
    //    {
    //        string dorsal = "";
    //        int digito0 = -1, digito1 = -1, direccion = -1, dorsal_anterior = -1;
    //        bool flash, visible, extraFlash;

    //        //Console.WriteLine("VISITANTES");

    //        dorsal_anterior = 4;
    //        direccion = 160;
    //        for (int i = 0; i < 12; i++)
    //        {
    //            this.NC.GetDigit(direccion, out digito0, out flash, out visible, out extraFlash);
    //            this.NC.GetDigit(direccion + 1, out digito1, out flash, out visible, out extraFlash);

    //            digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //            digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //            dorsal = Convert.ToString(digito0).Replace("0", "") + digito1;
    //            ((Consola_baloncesto)this.mi_consola).update_dorsales_jugadores("visitante", Convert.ToString(dorsal_anterior), dorsal);

    //            //Console.WriteLine("visitante : " + Convert.ToString(dorsal_anterior) + " -> " + dorsal);

    //            dorsal_anterior = dorsal_anterior + 1;
    //            direccion = direccion + 2;
    //        }
    //        //Console.WriteLine("**********************************************");
    //    }


    //    /*
    //     * Método que consigue los puntos de los jugadores del equipo local
    //     */
    //    private void get_puntos_jugadores_local()
    //    {
    //        string puntos = "";
    //        int digito0 = -1, digito1 = -1, direccion = -1;
    //        bool flash, visible, extraFlash;

    //        direccion = 80;
    //        for (int i = 0; i < 12; i++)
    //        {
    //            this.NC.GetDigit(direccion, out digito0, out flash, out visible, out extraFlash);
    //            this.NC.GetDigit(direccion + 1, out digito1, out flash, out visible, out extraFlash);

    //            digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //            digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //            puntos = Convert.ToString(digito0).Replace("0", "") + digito1;
    //            ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("local", Convert.ToString(i), puntos);

    //            //Console.WriteLine("local : " + Convert.ToString(i) + " -> " + puntos);

    //            direccion = direccion + 2;
    //        }
    //    }


    //    /*
    //     * Método que consigue los puntos de los jugadores del equipo visitante
    //     */
    //    private void get_puntos_jugadores_visitante()
    //    {
    //        string puntos = "";
    //        int digito0 = -1, digito1 = -1, direccion = -1;
    //        bool flash, visible, extraFlash;

    //        direccion = 208;
    //        for (int i = 0; i < 12; i++)
    //        {
    //            this.NC.GetDigit(direccion, out digito0, out flash, out visible, out extraFlash);
    //            this.NC.GetDigit(direccion + 1, out digito1, out flash, out visible, out extraFlash);

    //            digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //            digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //            puntos = Convert.ToString(digito0).Replace("0", "") + digito1;
    //            ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("visitante", Convert.ToString(i), puntos);

    //            //Console.WriteLine("visitante : " + Convert.ToString(i) + " -> " + puntos);

    //            direccion = direccion + 2;
    //        }
    //    }


    //    /*
    //     * Método que consigue las faltas de los jugadores del equipo local
    //     */
    //    private void get_faltas_jugadores_local()
    //    {
    //        string faltas = "";
    //        int digito0 = -1, digito1 = -1, direccion = -1;
    //        bool flash, visible, extraFlash;

    //        //direccion = 64;
    //        //for (int i = 0; i < 12; i++)
    //        for (int i = 64; i < 80; i++)
    //        {
    //            this.NC.GetDigit(i, out digito0, out flash, out visible, out extraFlash);
    //            //this.NC.GetDigit(direccion + 1, out digito1, out flash, out visible, out extraFlash);

    //            //digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //            //digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //            Console.WriteLine(Convert.ToString(i) + ": " + digito0);
                
    //            //faltas = Convert.ToString(digito0).Replace("0", "") + digito1;
    //            //((Consola_baloncesto)this.mi_consola).update_faltas_jugador("local", Convert.ToString(i), faltas);

    //            //Console.WriteLine("local : " + Convert.ToString(i) + " -> " + faltas + " faltas");

    //            //direccion = direccion + 1;
    //        }
    //        Console.WriteLine("=====================================================");
    //    }


    //    /*
    //     * Método que consigue las faltas de los jugadores del equipo visitante
    //     */
    //    private void get_faltas_jugadores_visitante()
    //    {
    //        string faltas = "";
    //        int digito0 = -1, digito1 = -1, direccion = -1;
    //        bool flash, visible, extraFlash;

    //        direccion = 192;
    //        for (int i = 0; i < 12; i++)
    //        {
    //            this.NC.GetDigit(direccion, out digito0, out flash, out visible, out extraFlash);
    //            this.NC.GetDigit(direccion + 1, out digito1, out flash, out visible, out extraFlash);

    //            digito0 = Convert.ToInt32(Convert.ToString(digito0).Replace("15", "0"));
    //            digito1 = Convert.ToInt32(Convert.ToString(digito1).Replace("15", "0"));

    //            faltas = Convert.ToString(digito0).Replace("0", "") + digito1;
    //            ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("visitante", Convert.ToString(i), faltas);

    //            //Console.WriteLine("visitante : " + Convert.ToString(i) + " -> " + faltas + " faltas");

    //            direccion = direccion + 2;
    //        }
    //    }



    //    /*
    //     * Metodo que decodifica las tramas dependiendo del deporte
    //     */
    //    public override void decod_trama(string trama = "")
    //    {
    //        //get_crono();

    //        //while (true)
    //        //{

    //        //}

    //        //switch (this.id_deporte)
    //        //{
    //            //case 1:     // Baloncesto
    //            //    decod_trama_baloncesto();
    //            //    break;
    //            //case 2:     // Balonmano
    //            //    decod_trama_balonmano();
    //            //    break;
    //            //case 3:     // Futbol sala
    //            //    decod_trama_futbol_sala();
    //            //    break;
    //            //case 4:     // Waterpolo
    //            //    decod_trama_waterpolo();
    //            //    break;
    //            //case 5:     // Hockey hielo
    //            //    decod_trama_hockey_hielo();
    //            //    break;
    //            //case 6:     // Hockey sala
    //            //    decod_trama_hockey_sala();
    //            //    break;
    //            //case 7:     // Voleibol
    //            //    decod_trama_voleibol();
    //            //    break;
    //            //case 8:     // Tenis
    //            //    decod_trama_tenis();
    //            //    break;
    //            //case 9:     // Futbol
    //            //    decod_trama_futbol();
    //            //    break;
    //            //case 10:     // Fronton
    //            //    decod_trama_fronton();
    //            //    break;
    //            //case 11:     // Baloncesto basico
    //            //    decod_trama_baloncesto();
    //            //    break;
    //            //case 12:     // Padel
    //            //    decod_trama_padel();
    //            //    break;
    //        //}
    //    }
       
    //}

    //# endregion

    
    //# region Interprete_virtualia

    //// Interprete de Virtualia
    //public class Interprete_virtualia : Interprete
    //{
    //    // Atributos
    //    TcpListener server;
    //    TcpClient client;
    //    public Thread thread_escucha_tramas;
    //    public bool hay_que_cerrar_la_conexion;
        

    //    // Constructor
    //    public Interprete_virtualia(int id_deporte, string puerto)
    //    {
    //        this.id_deporte = id_deporte;
    //        this.puerto_com = puerto;
    //        this.hay_que_cerrar_la_conexion = false;
    //    }


    //    /*
    //    *  Se conecta al puerto tcp
    //    */
    //    public override bool conecta(Consola consola)
    //    {
    //        bool error = true;

            
    //        this.mi_consola = consola;

    //        //this.server = new TcpListener(IPAddress.Any, 1234);
    //        //this.server.Start();
    //        //this.client = this.server.AcceptTcpClient();
    //        //Console.WriteLine("Conexion tcp realizada");                     
            
    //        //this.thread_escucha_tramas = new Thread(decod_trama);
    //        //this.thread_escucha_tramas.Start();

    //        this.thread_escucha_tramas = new Thread(servidor_tramas);
    //        this.thread_escucha_tramas.Start();

    //        Console.WriteLine("Escuchando tramas...");

    //        return error;
    //    }


    //    /*
    //    *  Desconecta del puerto tcp
    //    */
    //    public override void desconecta()
    //    {
    //        this.hay_que_cerrar_la_conexion = true;

    //        if (this.thread_escucha_tramas != null && this.thread_escucha_tramas.IsAlive)
    //            this.thread_escucha_tramas.Abort();
    //    }


    //    /*
    //     * Metodo que decodifica las tramas dependiendo del deporte
    //     */
    //    // State object for reading client data asynchronously
    //    public class StateObject
    //    {
    //        // Client  socket.
    //        public Socket workSocket = null;
    //        // Size of receive buffer.
    //        public const int BufferSize = 1024;
    //        // Receive buffer.
    //        public byte[] buffer = new byte[BufferSize];
    //        // Received data string.
    //        public StringBuilder sb = new StringBuilder();
    //    }


    //    // Thread signal.
    //    public static ManualResetEvent allDone = new ManualResetEvent(false);

    //    public void servidor_tramas()
    //    {
    //        // Data buffer for incoming data.
    //        byte[] bytes = new Byte[1024];

    //        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 1234);

    //        // Create a TCP/IP socket.
    //        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //        // Bind the socket to the local endpoint and listen for incoming connections.
    //        try
    //        {
    //            listener.Bind(localEndPoint);
    //            listener.Listen(100);

    //            while (true)
    //            {
    //                // Set the event to nonsignaled state.
    //                allDone.Reset();

    //                // Start an asynchronous socket to listen for connections.
    //                //Console.WriteLine("Waiting for a connection...");
    //                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

    //                // Wait until a connection is made before continuing.
    //                allDone.WaitOne();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.ToString());
    //        }
    //    }

    //    public void AcceptCallback(IAsyncResult ar)
    //    {
    //        // Signal the main thread to continue.
    //        allDone.Set();

    //        // Get the socket that handles the client request.
    //        Socket listener = (Socket)ar.AsyncState;
    //        Socket handler = listener.EndAccept(ar);  // Acepta la petición

    //        // Create the state object.
    //        StateObject state = new StateObject();
    //        state.workSocket = handler;
    //        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
    //    }

    //    public void ReadCallback(IAsyncResult ar)
    //    {
    //        String content = String.Empty;

    //        // Retrieve the state object and the handler socket
    //        // from the asynchronous state object.
    //        try
    //        {
    //            StateObject state = (StateObject)ar.AsyncState;
    //            Socket handler = state.workSocket;

    //            // Read data from the client socket. 
    //            int bytesRead = handler.EndReceive(ar);

    //            if (bytesRead > 0)
    //            {
    //                // There  might be more data, so store the data received so far.
    //                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
    //                content = state.sb.ToString();
    //                decod_trama(Convert.ToString(content));
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.ToString());
    //        }
    //    }


    //    public override void decod_trama(string trama = "")
    //    //public override void decod_trama()
    //    {
    //        switch (this.id_deporte)
    //        {
    //            case 1:     // Baloncesto
    //                //decod_trama_baloncesto();
    //                decod_trama_baloncesto(trama);
    //                break;
    //            case 2:     // Balonmano
    //                decod_trama_balonmano(trama);
    //                break;
    //            case 3:     // Futbol sala
    //                decod_trama_futbol_sala(trama);
    //                break;
    //            case 4:     // Waterpolo
    //                decod_trama_waterpolo(trama);
    //                break;
    //            case 5:     // Hockey hielo
    //                decod_trama_hockey_hielo(trama);
    //                break;
    //            //case 6:     // Hockey sala
    //            //    decod_trama_hockey_sala();
    //            //    break;
    //            case 7:     // Voleibol
    //                decod_trama_voleibol(trama);
    //                break;
    //            case 8:     // Tenis
    //                decod_trama_tenis(trama);
    //                break;
    //            case 9:     // Futbol
    //                decod_trama_futbol(trama);
    //                break;
    //            //case 10:     // Fronton
    //            //    decod_trama_fronton();
    //            //    break;
    //            case 11:     // Baloncesto basico
    //                decod_trama_baloncesto(trama);
    //                break;
    //            case 12:     // Padel
    //                decod_trama_padel(trama);
    //                break;
    //        }
    //    }


    //    /*
    //     * Metodo que hace un split del string que llega como parametro y lo separa por el delimitador definido
    //     */
    //    public String[] parsea_datos_del_buffer(String datos, string[] delimitador)
    //    {
    //        //Char delimitador = ';';
    //        //String[] array_datos = datos.Split(delimitador);

    //        //string[] delimitador = { "basket;" };
    //        string[] array_datos = datos.Split(delimitador, StringSplitOptions.None);

    //        //String[] delimitador = string.split(Pattern.quote("basket;"));
    //        //foreach (var substring in array_datos)
    //        //    Console.WriteLine(substring);

    //        return array_datos;
    //    }



    //    /*
    //     * Metodo que decodifica las tramas del baloncesto
    //     */

    //    public void decod_trama_baloncesto(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string dorsal = "", puntos_totales = "", tiros_libres = "", canastas_de_2 = "", triples = "", faltas = "", crono = "", dorsal_interno = "";
    //        int num_jugadores = 12;

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "crono":
    //                crono = datos_parseados_mensaje[2].ToString();
    //                ((Consola_baloncesto)this.mi_consola).update_crono(crono);
    //                break;
    //            case "periodo":
    //                ((Consola_baloncesto)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "indicador_posesion":
    //                ((Consola_baloncesto)this.mi_consola).update_flechas(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_local":
    //                ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_visitante":
    //                ((Consola_baloncesto)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "puntos_local":
    //                ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "puntos_visitante":
    //                ((Consola_baloncesto)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "faltas_local":
    //                ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "faltas_visitante":
    //                ((Consola_baloncesto)this.mi_consola).update_faltas_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "puntos_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                puntos_totales = datos_parseados_mensaje[3].ToString();
    //                tiros_libres = datos_parseados_mensaje[4].ToString();
    //                canastas_de_2 = datos_parseados_mensaje[5].ToString();
    //                triples = datos_parseados_mensaje[6].ToString();
    //                ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("local", dorsal, puntos_totales, tiros_libres, canastas_de_2, triples);
    //                break;
    //            case "puntos_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                puntos_totales = datos_parseados_mensaje[3].ToString();
    //                tiros_libres = datos_parseados_mensaje[4].ToString();
    //                canastas_de_2 = datos_parseados_mensaje[5].ToString();
    //                triples = datos_parseados_mensaje[6].ToString();
    //                ((Consola_baloncesto)this.mi_consola).update_puntos_jugador("visitante", dorsal, puntos_totales, tiros_libres, canastas_de_2, triples);
    //                break;
    //            case "faltas_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                faltas = datos_parseados_mensaje[3].ToString();
    //                ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("local", dorsal, faltas);
    //                break;
    //            case "faltas_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                faltas = datos_parseados_mensaje[3].ToString();
    //                ((Consola_baloncesto)this.mi_consola).update_faltas_jugador("visitante", dorsal, faltas);
    //                break;                
    //            case "dorsales_equipo_local":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_baloncesto)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //                }
    //                break;
    //            case "dorsales_equipo_visitante":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_baloncesto)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //                }
    //                ((Consola_baloncesto)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //            case "crono_posesion":
    //                ((Consola_baloncesto)this.mi_consola).update_crono_posesion(datos_parseados_mensaje[2].ToString());
    //                break;
    //            //case "inicia_videomarcador":
    //            //    ((Consola_baloncesto)this.mi_consola).update_inicia_videomarcador();
    //            //    break;
    //        }          
    //    }


    //    /*
    //     * Metodo que decodifica las tramas del balonmano
    //     */
    //    public void decod_trama_balonmano(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string crono = "", estado = "", dorsal = "", goles = "", dorsal_interno = "";
    //        int num_jugadores = 14;

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "crono":
    //                crono = datos_parseados_mensaje[2].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_crono(crono);
    //                break;
    //            case "crono_estado":
    //                estado = datos_parseados_mensaje[2].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_estado_crono(estado);
    //                break;
    //            case "periodo":
    //                ((Consola_balonmano)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_local":
    //                ((Consola_balonmano)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_visitante":
    //                ((Consola_balonmano)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_local":
    //                ((Consola_balonmano)this.mi_consola).update_tiempos_muertos("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_visitante":
    //                ((Consola_balonmano)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_goles_jugador("local", dorsal, goles);
    //                break;
    //            case "goles_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_goles_jugador("visitante", dorsal, goles);
    //                break;                
    //            case "dorsales_equipo_local":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_balonmano)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //                }
    //                break;
    //            case "dorsales_equipo_visitante":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_balonmano)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //                }
    //                ((Consola_balonmano)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //            case "suma_expulsion_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_suma_expulsion_jugador("local", dorsal);
    //                break;
    //            case "suma_expulsion_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_suma_expulsion_jugador("visitante", dorsal);
    //                break;
    //            case "resta_expulsion_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_resta_expulsion_jugador("local", dorsal);
    //                break;
    //            case "resta_expulsion_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                ((Consola_balonmano)this.mi_consola).update_resta_expulsion_jugador("visitante", dorsal);
    //                break;
    //            //case "inicia_videomarcador":
    //            //    ((Consola_balonmano)this.mi_consola).update_inicia_videomarcador();
    //            //    break;
    //        }
    //    }


    //    /*
    //     * Metodo que decodifica las tramas del futbol sala
    //     */
    //    public void decod_trama_futbol_sala(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string crono = "", dorsal = "", goles = "", dorsal_interno = "";
    //        int num_jugadores = 12;
                        
    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "crono":
    //               crono = datos_parseados_mensaje[2].ToString();
    //               ((Consola_futbol_sala)this.mi_consola).update_crono(crono);
    //                break;
    //            case "periodo":
    //                ((Consola_futbol_sala)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_local":
    //                ((Consola_futbol_sala)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_visitante":
    //                ((Consola_futbol_sala)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_local":
    //                ((Consola_futbol_sala)this.mi_consola).update_tiempos_muertos("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_visitante":
    //                ((Consola_futbol_sala)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "faltas_local":
    //                ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "faltas_visitante":
    //                ((Consola_futbol_sala)this.mi_consola).update_faltas_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_futbol_sala)this.mi_consola).update_goles_jugador("local", dorsal, goles);
    //                break;
    //            case "goles_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_futbol_sala)this.mi_consola).update_goles_jugador("visitante", dorsal, goles);
    //                break;                
    //            case "dorsales_equipo_local":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_futbol_sala)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //                }
    //                break;
    //            case "dorsales_equipo_visitante":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_futbol_sala)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //                }
    //                ((Consola_futbol_sala)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //            //case "inicia_videomarcador":
    //            //    //((Consola_futbol_sala)this.mi_consola).update_inicia_videomarcador();
    //            //    break;
    //        }
    //    }
        

    //    /*
    //     * Metodo que decodifica las tramas del waterpolo
    //     */
    //    public void decod_trama_waterpolo(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string crono = "", dorsal = "", goles = "", dorsal_interno = "", num_exclusiones = "";
    //        int num_jugadores = 13;

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "crono":
    //                crono = datos_parseados_mensaje[2].ToString();
    //                ((Consola_waterpolo)this.mi_consola).update_crono(crono);
    //                break;
    //            case "periodo":
    //                ((Consola_waterpolo)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "indicador_posesion":
    //                ((Consola_waterpolo)this.mi_consola).update_flechas(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_local":
    //                ((Consola_waterpolo)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_visitante":
    //                ((Consola_waterpolo)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_local":
    //                ((Consola_waterpolo)this.mi_consola).update_tiempos_muertos("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_visitante":
    //                ((Consola_waterpolo)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_waterpolo)this.mi_consola).update_goles_jugador("local", dorsal, goles);
    //                break;
    //            case "goles_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_waterpolo)this.mi_consola).update_goles_jugador("visitante", dorsal, goles);
    //                break;              
    //            case "dorsales_equipo_local":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_waterpolo)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //                }
    //                break;
    //            case "dorsales_equipo_visitante":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_waterpolo)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //                }
    //                ((Consola_waterpolo)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //            case "expulsiones_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                num_exclusiones = datos_parseados_mensaje[3].ToString();
    //                ((Consola_waterpolo)this.mi_consola).update_expulsiones_jugador("local", dorsal, num_exclusiones);
    //                break;
    //            case "expulsiones_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                num_exclusiones = datos_parseados_mensaje[3].ToString();
    //                ((Consola_waterpolo)this.mi_consola).update_expulsiones_jugador("visitante", dorsal, num_exclusiones);
    //                break;
    //            //case "inicia_videomarcador":
    //            //    ((Consola_waterpolo)this.mi_consola).update_inicia_videomarcador();
    //            //    break;
    //        }
    //    }


    //    /*
    //     * Metodo que decodifica las tramas del hockey hielo
    //     */
    //    public void decod_trama_hockey_hielo(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string crono = "", estado = "", dorsal = "", goles = "", dorsal_interno = "", num_exclusiones = "";
    //        int num_jugadores = 30;

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "crono":
    //                crono = datos_parseados_mensaje[2].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_crono(crono);
    //                break;
    //            case "crono_estado":
    //                estado = datos_parseados_mensaje[2].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_estado_crono(estado);
    //                break;
    //            case "periodo":
    //                ((Consola_hockey_hielo)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_local":
    //                ((Consola_hockey_hielo)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_visitante":
    //                ((Consola_hockey_hielo)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_local":
    //                ((Consola_hockey_hielo)this.mi_consola).update_tiempos_muertos("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_visitante":
    //                ((Consola_hockey_hielo)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_goles_jugador("local", dorsal, goles);
    //                break;
    //            case "goles_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_goles_jugador("visitante", dorsal, goles);
    //                break;
    //            case "suma_expulsion_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                num_exclusiones = datos_parseados_mensaje[3].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_suma_expulsion_jugador("local", dorsal, num_exclusiones);
    //                break;
    //            case "suma_expulsion_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                num_exclusiones = datos_parseados_mensaje[3].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_suma_expulsion_jugador("visitante", dorsal, num_exclusiones);
    //                break;
    //            case "resta_expulsion_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_resta_expulsion_jugador("local", dorsal);
    //                break;
    //            case "resta_expulsion_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                ((Consola_hockey_hielo)this.mi_consola).update_resta_expulsion_jugador("visitante", dorsal);
    //                break;               
    //            case "dorsales_equipo_local":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_hockey_hielo)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //                }
    //                break;
    //            case "dorsales_equipo_visitante":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_hockey_hielo)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //                }
    //                ((Consola_hockey_hielo)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //            //case "inicia_videomarcador":
    //            //    ((Consola_hockey_hielo)this.mi_consola).update_inicia_videomarcador();
    //            //    break;
    //        }
    //    }


    //    ///*
    //    // * Metodo que decodifica las tramas del hockey sala
    //    // */
    //    //public void decod_trama_hockey_sala()
    //    //{
    //    //    NetworkStream stream = this.client.GetStream();
    //    //    String[] datos_parseados;

    //    //    //Console.WriteLine("[Thread] => Estoy dentro del thread");
    //    //    while (true)
    //    //    {
    //    //        Console.WriteLine("[Thread] => Estoy esperando tramas...");
    //    //        while (!stream.DataAvailable) ;
    //    //        //Console.WriteLine("[Thread] => Me ha llegado una");

    //    //        Byte[] bytes = new Byte[client.Available];
    //    //        stream.Read(bytes, 0, bytes.Length);

    //    //        //translate bytes of request to string
    //    //        String data = Encoding.UTF8.GetString(bytes);
    //    //        Console.WriteLine(data);

    //    //        datos_parseados = parsea_datos_del_buffer(data);
    //    //        switch (datos_parseados[1].ToString())
    //    //        {
    //    //            case "crono":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_crono(datos_parseados[2].ToString());
    //    //                break;
    //    //            case "crono_estado":
    //    //                estado = datos_parseados_mensaje[2].ToString();
    //    //                ((Consola_hockey_sala)this.mi_consola).update_estado_crono(estado);
    //    //                break;
    //    //            case "periodo":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_periodo(datos_parseados[2].ToString());
    //    //                break;
    //    //            case "puntos_local":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_puntos_equipo("local", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "puntos_visitante":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_puntos_equipo("visitante", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "tiempo_muerto_local":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_tiempos_muertos("local", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "tiempo_muerto_visitante":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "goles_jugador_local":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_goles_jugador("local", datos_parseados[2].ToString(), datos_parseados[3].ToString());
    //    //                break;
    //    //            case "goles_jugador_visitante":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_goles_jugador("visitante", datos_parseados[2].ToString(), datos_parseados[3].ToString());
    //    //                break;
    //    //            case "suma_expulsion_jugador_local":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_suma_expulsion_jugador("local", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "suma_expulsion_jugador_visitante":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_suma_expulsion_jugador("visitante", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "resta_expulsion_jugador_local":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_resta_expulsion_jugador("local", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "resta_expulsion_jugador_visitante":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_resta_expulsion_jugador("visitante", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "dorsales_local":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_dorsales_jugadores("local", datos_parseados[2].ToString(), datos_parseados[3].ToString());
    //    //                break;
    //    //            case "dorsales_visitante":
    //    //                ((Consola_hockey_sala)this.mi_consola).update_dorsales_jugadores("visitante", datos_parseados[2].ToString(), datos_parseados[3].ToString());
    //    //                break;
    //            //    case "dorsales_equipo_local":
    //            //    for (int i = 0; i < datos_parseados_mensaje.Length - 2; i++)
    //            //    {
    //            //        dorsal_interno = Convert.ToString(i + 1);
    //            //        dorsal = datos_parseados_mensaje[i + 2].ToString();
    //            //        ((Consola_hockey_hielo)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //            //    }
    //            //    break;
    //            //case "dorsales_equipo_visitante":
    //            //    for (int i = 0; i < datos_parseados_mensaje.Length - 2; i++)
    //            //    {
    //            //        dorsal_interno = Convert.ToString(i + 1);
    //            //        dorsal = datos_parseados_mensaje[i + 2].ToString();
    //            //        ((Consola_hockey_hielo)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //            //    }
    //            //    break;
    //    //        }
    //    //    }
    //    //}


    //    /*
    //     * Metodo que decodifica las tramas del voleibol
    //     */
    //    public void decod_trama_voleibol(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string sets_acumulados_local = "", sets_acumulados_visitante = "";

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "tiempo_muerto_local":
    //                ((Consola_voleibol)this.mi_consola).update_tiempos_muertos("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tiempo_muerto_visitante":
    //                ((Consola_voleibol)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set":
    //                ((Consola_voleibol)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "cambio_set":
    //                ((Consola_voleibol)this.mi_consola).update_cambio_set(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "indicador_saque":
    //                ((Consola_voleibol)this.mi_consola).update_flechas(datos_parseados_mensaje[2].ToString());
    //                break;                
    //            case "puntos_local":
    //                ((Consola_voleibol)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "puntos_visitante":
    //                ((Consola_voleibol)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "sets_acumulados":
    //                sets_acumulados_local = datos_parseados_mensaje[2].ToString();
    //                sets_acumulados_visitante = datos_parseados_mensaje[3].ToString();
    //                ((Consola_voleibol)this.mi_consola).update_sets_acumulados(sets_acumulados_local, sets_acumulados_visitante);
    //                break;
    //            case "modo_edicion":
    //                ((Consola_voleibol)this.mi_consola).update_modo_edicion(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "sets_local":
    //                for (int i = 0; i < 5; i++)
    //                    ((Consola_voleibol)this.mi_consola).update_sets_equipo("local", Convert.ToString(i + 1), datos_parseados_mensaje[i + 2].ToString());
    //                break;
    //            case "sets_visitante":
    //                for (int i = 0; i < 5; i++)
    //                    ((Consola_voleibol)this.mi_consola).update_sets_equipo("visitante", Convert.ToString(i + 1), datos_parseados_mensaje[i + 2].ToString());
    //                break;
    //            case "inicia_videomarcador":
    //                ((Consola_voleibol)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //        }
    //    }



    //    /*
    //    * Metodo que decodifica las tramas del tenis
    //    */
    //    public void decod_trama_tenis(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string sets_acumulados_local = "", sets_acumulados_visitante = "", tie_break_activado = "";

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "set1_local":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("local", "1", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set2_local":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("local", "2", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set3_local":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("local", "3", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set4_local":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("local", "4", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set5_local":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("local", "5", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set1_visitante":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("visitante", "1", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set2_visitante":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("visitante", "2", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set3_visitante":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("visitante", "3", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set4_visitante":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("visitante", "4", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set5_visitante":
    //                ((Consola_tenis)this.mi_consola).update_sets_equipo("visitante", "5", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set":
    //                ((Consola_tenis)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "cambio_juego":
    //                ((Consola_tenis)this.mi_consola).update_cambio_juego(datos_parseados_mensaje[2].ToString(), datos_parseados_mensaje[3].ToString());
    //                break;
    //            case "cambio_set":
    //                ((Consola_tenis)this.mi_consola).update_cambio_set(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "indicador_saque":
    //                ((Consola_tenis)this.mi_consola).update_flechas(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tie_break":
    //                tie_break_activado = datos_parseados_mensaje[2].ToString();
    //                if (Convert.ToBoolean(tie_break_activado))
    //                    ((Consola_tenis)this.mi_consola).update_tie_break("1");
    //                else
    //                    ((Consola_tenis)this.mi_consola).update_tie_break("0");
    //                break;
    //            case "puntos_local":
    //                ((Consola_tenis)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "puntos_visitante":
    //                ((Consola_tenis)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "sets_acumulados":
    //                sets_acumulados_local = datos_parseados_mensaje[2].ToString();
    //                sets_acumulados_visitante = datos_parseados_mensaje[3].ToString();
    //                ((Consola_tenis)this.mi_consola).update_sets_acumulados(sets_acumulados_local, sets_acumulados_visitante);
    //                break;
    //            case "modo_edicion":
    //                ((Consola_tenis)this.mi_consola).update_modo_edicion(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "inicia_videomarcador":
    //                ((Consola_tenis)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //        }
    //    }


    //    /*
    //     * Metodo que decodifica las tramas del futbol
    //     */
    //    public void decod_trama_futbol(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string crono = "", dorsal = "", goles = "", dorsal_interno = "", num_tarjetas = "", tipo_falta = "";
    //        int num_jugadores = 20;

    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "crono":
    //                crono = datos_parseados_mensaje[2].ToString();
    //                ((Consola_futbol)this.mi_consola).update_crono(crono);
    //                break;
    //            case "parte":
    //                ((Consola_futbol)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_local":
    //                ((Consola_futbol)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_visitante":
    //                ((Consola_futbol)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "goles_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_futbol)this.mi_consola).update_goles_jugador("local", dorsal, goles);
    //                break;
    //            case "goles_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                goles = datos_parseados_mensaje[3].ToString();
    //                ((Consola_futbol)this.mi_consola).update_goles_jugador("visitante", dorsal, goles);
    //                break;
    //            case "faltas_jugador_local":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                num_tarjetas = datos_parseados_mensaje[3].ToString();
    //                tipo_falta = datos_parseados_mensaje[4].ToString();                    
    //                if (tipo_falta == "amarilla")
    //                    ((Consola_futbol)this.mi_consola).update_tarjetas_jugador("local", dorsal, tipo_falta, num_tarjetas);
    //                else if (tipo_falta == "roja")
    //                    ((Consola_futbol)this.mi_consola).update_tarjetas_jugador("local", dorsal, tipo_falta, num_tarjetas);
    //                break;
    //            case "faltas_jugador_visitante":
    //                dorsal = datos_parseados_mensaje[2].ToString();
    //                num_tarjetas = datos_parseados_mensaje[3].ToString();
    //                tipo_falta = datos_parseados_mensaje[4].ToString();                    
    //                if (tipo_falta == "amarilla")
    //                    ((Consola_futbol)this.mi_consola).update_tarjetas_jugador("visitante", dorsal, tipo_falta, num_tarjetas);
    //                else if (tipo_falta == "roja")
    //                    ((Consola_futbol)this.mi_consola).update_tarjetas_jugador("visitante", dorsal, tipo_falta, num_tarjetas);
    //                break;              
    //            case "dorsales_equipo_local":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_futbol)this.mi_consola).update_dorsales_jugadores("local", dorsal_interno, dorsal);
    //                }
    //                break;
    //            case "dorsales_equipo_visitante":
    //                for (int i = 0; i < num_jugadores; i++)
    //                {
    //                    dorsal_interno = Convert.ToString(i + 1);
    //                    dorsal = datos_parseados_mensaje[i + 2].ToString();
    //                    ((Consola_futbol)this.mi_consola).update_dorsales_jugadores("visitante", dorsal_interno, dorsal);
    //                }
    //                ((Consola_futbol)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //            //case "inicia_videomarcador":
    //            //    ((Consola_futbol)this.mi_consola).update_inicia_videomarcador();
    //            //    break;
    //        }
    //    }


    //    ///*
    //    // * Metodo que decodifica las tramas del fronton
    //    // */
    //    //public void decod_trama_fronton()
    //    //{
    //    //    NetworkStream stream = this.client.GetStream();
    //    //    String[] datos_parseados;

    //    //    //Console.WriteLine("[Thread] => Estoy dentro del thread");
    //    //    while (true)
    //    //    {
    //    //        Console.WriteLine("[Thread] => Estoy esperando tramas...");
    //    //        while (!stream.DataAvailable) ;
    //    //        //Console.WriteLine("[Thread] => Me ha llegado una");

    //    //        Byte[] bytes = new Byte[client.Available];
    //    //        stream.Read(bytes, 0, bytes.Length);

    //    //        //translate bytes of request to string
    //    //        String data = Encoding.UTF8.GetString(bytes);
    //    //        Console.WriteLine(data);

    //    //        datos_parseados = parsea_datos_del_buffer(data);
    //    //        switch (datos_parseados[1].ToString())
    //    //        {
    //    //            case "crono":
    //    //                ((Consola_fronton)this.mi_consola).update_crono(datos_parseados[2].ToString());
    //    //                break;
    //    //            case "indicador_posesion":
    //    //                ((Consola_fronton)this.mi_consola).update_flechas(datos_parseados[2].ToString());
    //    //                break;
    //    //            //case "tiempo_muerto_local":
    //    //            //    ((Consola_fronton)this.mi_consola).update_tiempos_muertos("local", datos_parseados[2].ToString());
    //    //            //    break;
    //    //            //case "tiempo_muerto_visitante":
    //    //            //    ((Consola_fronton)this.mi_consola).update_tiempos_muertos("visitante", datos_parseados[2].ToString());
    //    //            //    break;
    //    //            case "puntos_local":
    //    //                ((Consola_fronton)this.mi_consola).update_puntos_equipo("local", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "puntos_visitante":
    //    //                ((Consola_fronton)this.mi_consola).update_puntos_equipo("visitante", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "tanteo":
    //    //                ((Consola_fronton)this.mi_consola).update_tanteo_necesario(datos_parseados[2].ToString());
    //    //                break;
    //    //            case "sets_local":
    //    //                ((Consola_fronton)this.mi_consola).update_sets_equipo("local", datos_parseados[2].ToString());
    //    //                break;
    //    //            case "sets_visitante":
    //    //                ((Consola_fronton)this.mi_consola).update_sets_equipo("visitante", datos_parseados[2].ToString());
    //    //                break;
    //    //        }
    //    //    }
    //    //}


    //    /*
    //    * Metodo que decodifica las tramas del padel
    //    */
    //    public void decod_trama_padel(string trama)
    //    {
    //        String[] datos_parseados_mensaje;
    //        string sets_acumulados_local = "", sets_acumulados_visitante = "", tie_break_activado = "";
                        
    //        datos_parseados_mensaje = parsea_datos_del_buffer(trama, new string[] { ";" });
    //        switch (datos_parseados_mensaje[1].ToString())
    //        {
    //            case "set1_local":
    //                ((Consola_padel)this.mi_consola).update_juegos("local", "1", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set2_local":
    //                ((Consola_padel)this.mi_consola).update_juegos("local", "2", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set3_local":
    //                ((Consola_padel)this.mi_consola).update_juegos("local", "3", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set1_visitante":
    //                ((Consola_padel)this.mi_consola).update_juegos("visitante", "1", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set2_visitante":
    //                ((Consola_padel)this.mi_consola).update_juegos("visitante", "2", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "set3_visitante":
    //                ((Consola_padel)this.mi_consola).update_juegos("visitante", "3", datos_parseados_mensaje[2].ToString());
    //                break;               
    //            case "set":
    //                ((Consola_padel)this.mi_consola).update_periodo(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "cambio_juego":
    //                ((Consola_padel)this.mi_consola).update_cambio_juego(datos_parseados_mensaje[2].ToString(), datos_parseados_mensaje[3].ToString());
    //                break;
    //            case "cambio_set":
    //                ((Consola_padel)this.mi_consola).update_cambio_set(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "indicador_saque":
    //                ((Consola_padel)this.mi_consola).update_flechas(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "tie_break":
    //                tie_break_activado = datos_parseados_mensaje[2].ToString();
    //                if (Convert.ToBoolean(tie_break_activado))
    //                    ((Consola_padel)this.mi_consola).update_tie_break("1");
    //                else
    //                    ((Consola_padel)this.mi_consola).update_tie_break("0");
    //                break;
    //            case "puntos_local":
    //                ((Consola_padel)this.mi_consola).update_puntos_equipo("local", datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "puntos_visitante":
    //                ((Consola_padel)this.mi_consola).update_puntos_equipo("visitante", datos_parseados_mensaje[2].ToString());                    
    //                break;
    //            case "sets_acumulados":
    //                sets_acumulados_local = datos_parseados_mensaje[2].ToString();
    //                sets_acumulados_visitante = datos_parseados_mensaje[3].ToString();
    //                ((Consola_padel)this.mi_consola).update_sets_acumulados(sets_acumulados_local, sets_acumulados_visitante);
    //                break;
    //            case "modo_edicion":
    //                ((Consola_padel)this.mi_consola).update_modo_edicion(datos_parseados_mensaje[2].ToString());
    //                break;
    //            case "inicia_videomarcador":
    //                ((Consola_padel)this.mi_consola).update_inicia_videomarcador();
    //                break;
    //        }
    //    }

    //}

    //# endregion


    //# region Interprete_manual

    //// Interprete Manual
    //public class Interprete_manual : Interprete
    //{
    //    // Constructor
    //    public Interprete_manual(int id_deporte, string puerto)
    //    {
    //        this.id_deporte = id_deporte;
    //        this.puerto_com = puerto;
    //        //this.hay_que_cerrar_la_conexion = false;
    //    }


    //    /*
    //    *  Se conecta al puerto tcp
    //    */
    //    public override bool conecta(Consola consola)
    //    {
    //        bool error = true;
    //        this.mi_consola = consola;
    //        return error;
    //    }


    //    /*
    //    *  Desconecta del puerto tcp
    //    */
    //    public override void desconecta()
    //    {
    //    }
        
    //    public override void decod_trama(string trama = "")
    //    {
    //    }
    //}

    //# endregion
    

}
