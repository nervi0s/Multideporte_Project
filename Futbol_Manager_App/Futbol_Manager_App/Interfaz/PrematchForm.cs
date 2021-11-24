using System;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Interfaz
{

    /**
     * Formulario de edición de un Prematch
     */
    public partial class PrematchForm : Form
    {
        private Prematch _prematch;


        /**
        * Constructor
        */
        public PrematchForm()
        {
            InitializeComponent();
        }

        /**
         * Constructor
         */
        public PrematchForm(Prematch prematch)
        {
            InitializeComponent();

            this.textBoxHora.Text = prematch.Hora;
            this.textBoxBroadcaster.Text = prematch.Broadcaster;
            this.textBoxReferencia.Text = prematch.Referencia;
            this.textBoxTelefono.Text = prematch.Telefono;

            if (prematch.Tipo == "Pre")
            {
                this.radioButtonPre.Checked = true;
            }
            else
            {
                this.radioButtonPost.Checked = true;
            }
        }

        /**
         * Recupera el Prematch representado en el formulario
         * En caso de que el usuario cancele la operación se devuelve 'null'.
         */
        public Prematch GetPrematch()
        {
            return _prematch;
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            save();
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _prematch = null;
            this.Close();
        }

        private void save()
        {
            _prematch = new Prematch();
            _prematch.Hora = this.textBoxHora.Text;
            _prematch.Broadcaster = this.textBoxBroadcaster.Text;
            _prematch.Referencia = this.textBoxReferencia.Text;
            _prematch.Telefono = this.textBoxTelefono.Text;

            if (this.radioButtonPre.Checked)
            {
                _prematch.Tipo = "Pre";
            }
            else
            {
                _prematch.Tipo = "Post";
            }
        }
    }
}
