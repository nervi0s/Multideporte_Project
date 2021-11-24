using Futbol_Sala_Manager_App.Interfaz;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Futbol_Sala_Manager_App
{
    public class Ocr
    {
        private bool active;
        public bool isRunnig;
        private string ip;
        private int port;
        private Crono crono;
        private Thread threadTCPClient;
        private TCPClientReceiver tcpClientReceiver;

        private MainForm _gui;
        public int min;
        public int sec;
        string time;
        public bool pauseDataProcessing = false;

        public Ocr(string ip, string port, bool active, MainForm _gui, Crono crono)
        {
            this.active = active;
            this.ip = ip;
            int.TryParse(port, out this.port);
            this._gui = _gui;
            this.crono = crono;
        }

        public bool isActive()
        {
            return active;
        }

        public void start()
        {
            isRunnig = true;
            threadTCPClient = new Thread(() =>
            {
                try
                {
                    tcpClientReceiver = new TCPClientReceiver(ip, port);
                    tcpClientReceiver.onReceivedData += dataRecieved;
                    tcpClientReceiver.getData();
                }
                catch (SocketException)
                {
                    Console.WriteLine("Error de inicio, compruebe la IP y el puerto");
                }
            });

            threadTCPClient.Name = "Subproceso de escucha hacia el servidor OCR";
            threadTCPClient.Start();
        }

        private void dataRecieved(string data)
        {
            Console.WriteLine(data);
            JsonDocument jsonDocument;
            try
            {
                jsonDocument = JsonDocument.Parse(data);
            }
            catch (Exception)
            {
                jsonDocument = JsonDocument.Parse("{\"type\":\"ocr\",\"values\":{\"formattedTime\":\"[00, 00, doubleDot]\",\"time\":\"0:00\"}}");
            }

            StringBuilder formattedTime;
            try
            {
                formattedTime = new StringBuilder(jsonDocument.RootElement.GetProperty("values").GetProperty("formattedTime").GetString());
                if (formattedTime.ToString() == " ")
                {
                    formattedTime = new StringBuilder("[00, 00, doubleDot]");
                }
            }
            catch (Exception)
            {
                time = "0:00";
                formattedTime = new StringBuilder("[00, 00, doubleDot]");
            }

            formattedTime.Replace("[", "").Replace("]", "").Replace(" ", "");
            string[] splittedTimeInfo = formattedTime.ToString().Split(',');
            string format = splittedTimeInfo[2]; // Puede ser "dot" o "doubleDot" o "noData"
            time = splittedTimeInfo[0] + (splittedTimeInfo[2] == "doubleDot" ? ":" : ".") + splittedTimeInfo[1];
            Console.WriteLine(time + " -- " + crono.GetMomento().Parte);

            if (time.Contains(":"))
            {
                decimas_showed = false;
                string[] s = time.Split(':');
                min = int.Parse(s[0]);
                minute = min;
                sec = int.Parse(s[1]);
                second = sec;
            }
            else if (time.Contains("."))
            {
                decimas_showed = true;
                string[] s = time.Split('.');
                min = int.Parse(s[0]);
                minute = min;
                sec = int.Parse(s[1]);
                second = sec;
            }

            if (!pauseDataProcessing) // Si el procesamiento de datos no se ha marcado como pausado, procesa los datos
                processData(format);
        }

        // Modifica la UI al recbir datos desde el OCR y envía datos al IPF
        private void processData(string format)
        {
            if (format == "doubleDot")
            {
                _gui.SetCronoTextAsync(crono.GetMomento().GetTextoCronoWithOCR(crono._idioma[0], format), min.ToString("00"), sec.ToString("00"));
                crono.configCronoWithOcr(time, crono._momento.GetNombreParte(crono._idioma[0]), crono._momento.GetFinalParte(), crono.genParteAbr(crono._momento.Parte));
            }
            else if (format == "dot")
            {
                _gui.SetCronoTextAsync(crono.GetMomento().GetTextoCronoWithOCR(crono._idioma[0], format), min.ToString("00"), sec.ToString());
                crono.configCronoWithOcr(time, crono._momento.GetNombreParte(crono._idioma[0]), crono._momento.GetFinalParte(), crono.genParteAbr(crono._momento.Parte));
            }
        }

        public static int minute;
        public static int second;
        public static bool decimas_showed;
    }
}
