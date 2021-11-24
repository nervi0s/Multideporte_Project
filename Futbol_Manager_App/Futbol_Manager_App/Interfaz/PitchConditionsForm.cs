using System;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Interfaz
{
    public partial class PitchConditionsForm : Form
    {
        private PitchConditions _pitchConditions1;

        public PitchConditionsForm()
        {
            InitializeComponent();
        }

        public PitchConditionsForm(PitchConditions pc)
        {
            InitializeComponent();
            this.textBoxTitle.Text = pc.Title;
            this.textBoxEstadio.Text = pc.Estadio;
            this.textBoxClima.Text = pc.Clima;
            this.textBoxGrados.Text = pc.Temperatura;
            this.textBoxHumedad.Text = pc.Humedad;
            this.textBoxViento.Text = pc.Viento;
        }

        public PitchConditions GetPitchConditions()
        {
            return _pitchConditions1;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _pitchConditions1 = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _pitchConditions1 = new PitchConditions(this.textBoxTitle.Text, this.textBoxEstadio.Text, this.textBoxClima.Text, this.textBoxGrados.Text, this.textBoxHumedad.Text, this.textBoxViento.Text);
            this.Close();
        }
    }
}
