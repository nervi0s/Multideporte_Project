using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol_Sala_Manager_App
{
    public class MomentoOcr
    {
        public const int INICIO_PARTE_1 = 0;
        public const int FIN_PARTE_1 = 1;
        public const int INICIO_PARTE_2 = 2;
        public const int FIN_PARTE_2 = 3;
        public const int INICIO_PRORROGA_1 = 4;
        public const int FIN_PRORROGA_1 = 5;
        public const int INICIO_PRORROGA_2 = 6;
        public const int FIN_PRORROGA_2 = 7;
        public const int INICIO_PENALTIS = 8;
        public const int FIN_PENALTIS = 9;
        public const int FIN_PARTIDO = 10;

        private int momentoActual;

        public delegate void ReceivedDataDelegate(int parte);   // Delegado par manejar eventos al recibir un dato del sv
        public event ReceivedDataDelegate onReceivedData;       // Manejador del evento al recibir un dato del sv

        public MomentoOcr()
        {
            momentoActual = INICIO_PARTE_1;
        }

        public void aumentarMomento()
        {
            if (this.momentoActual <= FIN_PARTIDO)
                this.momentoActual++;
            else
                this.momentoActual = FIN_PARTIDO;
            onReceivedData(this.momentoActual);
        }

        public void cambiarMomento(int momento)
        {
            this.momentoActual = momento;
            onReceivedData(this.momentoActual);
        }

        public int getMomentoActual()
        {
            return this.momentoActual;
        }
    }
}
