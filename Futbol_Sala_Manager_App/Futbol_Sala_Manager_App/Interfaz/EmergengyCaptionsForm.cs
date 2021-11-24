using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Beans;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class EmergencyCaptionsForm : Form
    {
        private EmergencyCaptions _emergencyCaption;

        public EmergencyCaptionsForm()
        {
            InitializeComponent();
        }

        public EmergencyCaptionsForm(EmergencyCaptions em)
        {
            InitializeComponent();
            this.textBoxHeader.Text = em.Header;
            this.textBoxLinea1.Text = em.Linea1;

            
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            this._emergencyCaption = new EmergencyCaptions(this.textBoxHeader.Text, this.textBoxLinea1.Text);
            this.Close();
        }

        public EmergencyCaptions GetEmergencyCaption()
        {
            return this._emergencyCaption;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _emergencyCaption = null;
            this.Close();
        }



    }
}
