using System.Drawing;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{

    /**
     * Control para representar una ronda de penaltis
     * Incluye un número para el orden, y dos jugadores, uno local y otro visitante.
     * Los jugadores se muestran con su nombre y el color de la caja es verde si ha
     * marcado y roja si ha fallado.
     */
    public partial class PenaltisPanelItem : UserControl
    {

        /**
         * Constructor
         * Se establece el número asociado al elemento
         */
        public PenaltisPanelItem(int numero)
        {
            InitializeComponent();

            this.numero.Text = numero.ToString();
        }

        /**
         * Configura un jugador
         * Con 'local' se indica si es el jugador local o el visitante.
         */
        public void SetJugador(bool local, string nombre, bool gol)
        {
            Label label = (local ? this.jugadorL : this.jugadorV);

            label.Text = nombre;

            if (nombre == "")
                label.BackColor = Color.WhiteSmoke;
            else
                label.BackColor = (gol ? Color.DarkSeaGreen : Color.Firebrick);
        }

        /**
         * Indica si tanto el jugador local como el visitante están vacíos
         */
        public bool IsEmpty()
        {
            return this.jugadorL.Text == "" && this.jugadorV.Text == "";
        }


    }
}
