using System;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{

    /**
    * Formulario de edición de un Minutos de descuento
    */
    public partial class ExtraForm : Form
    {

        private int _minutos;

        /**
         * Constructor
         */
        public ExtraForm()
        {
            InitializeComponent();
        }

        /**
         * Constructor
         */
        public ExtraForm(int minutos)
        {
            InitializeComponent();

            this.numericUpDown.Value = minutos;
        }

        /**
         * Recupera los minutos representados en el formulario
         * En caso de que el usuario cancele la operación se devuelve '-1'.
         */
        public int getMinutos()
        {
            return _minutos;
        }



        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _minutos = (int) this.numericUpDown.Value;
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _minutos = -1;
            this.Close();
        }


    }
}
