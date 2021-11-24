using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Interfaz
{
    public partial class EndOfMultiFlashInterviewForm : Form
    {
        private EndOfInterview _endOfInterview;

        public EndOfMultiFlashInterviewForm()
        {
            InitializeComponent();
        }

        public EndOfMultiFlashInterviewForm(EndOfInterview en)
        {
            InitializeComponent();

            this.textBoxLinea1.Text = en.Linea1;
            this.textBoxLinea2.Text = en.Linea2;
            this.textBoxLinea3.Text = en.Linea3;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            this._endOfInterview = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            this._endOfInterview = new EndOfInterview(this.textBoxLinea1.Text, this.textBoxLinea2.Text, this.textBoxLinea3.Text);
            this.Close();
        }

        public EndOfInterview GetEndOfInterview()
        {
            return this._endOfInterview;
        }

    }
}
