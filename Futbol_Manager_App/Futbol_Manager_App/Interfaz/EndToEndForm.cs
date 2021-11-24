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
    public partial class EndToEndForm : Form
    {
        private EndToEnd _enToEnd;

        public EndToEndForm()
        {
            InitializeComponent();
        }

        public EndToEndForm(EndToEnd en)
        {
            InitializeComponent();

            this.textBoxHeader.Text = en.Header;
            this.textBoxLinea1.Text = en.Linea1;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _enToEnd = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _enToEnd = new EndToEnd(this.textBoxHeader.Text, this.textBoxLinea1.Text);
            this.Close();
        }

        public EndToEnd GetEndToEnd()
        {
            return _enToEnd;
        }

    }
}
