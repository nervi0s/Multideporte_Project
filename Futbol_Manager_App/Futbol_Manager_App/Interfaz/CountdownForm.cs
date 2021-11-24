using System;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Interfaz
{

    /**
     * Formulario de edición de un Countdown
     */
    public partial class CountdownForm : Form
    {

        private Countdown _countdown;

        /**
         * Constructor
         */
        public CountdownForm()
        {
            InitializeComponent();
        }

        /**
         * Constructor
         */
        public CountdownForm(Countdown countdown)
        {
            InitializeComponent();

            this.numericHora.Value = countdown.Hora;
            this.numericMinutos.Value = countdown.Minutos;
            this.numericDesfase.Value = countdown.Desfase;
            this.textBoxReferencia.Text = countdown.Referencia;
        }

        /**
         * Recupera el Countdown representado en el formulario
         * En caso de que el usuario cancele la operación se devuelve 'null'.
         */
        public Countdown GetCountdown()
        {
            return _countdown;
        }
                     
        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            save();
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _countdown = null;
            this.Close();
        }

        private void save()
        {
            _countdown = new Countdown();
            _countdown.Hora = (int) this.numericHora.Value;
            _countdown.Minutos = (int)this.numericMinutos.Value;
            _countdown.Desfase = (int)this.numericDesfase.Value;
            _countdown.Referencia = this.textBoxReferencia.Text;
        }


    }
}
