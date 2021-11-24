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
    public partial class ExchangeForm : Form
    {
        private Exchange _exchange;

        public ExchangeForm()
        {
            InitializeComponent();
        }

        public ExchangeForm(Exchange ex)
        {
            InitializeComponent();


            this.textBoxHeader.Text = ex.Header;
            this.textBoxLinea1.Text = ex.Hora;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _exchange = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _exchange = new Exchange(this.textBoxHeader.Text, this.textBoxLinea1.Text);

            this.Close();
        }

        public Exchange GetExchange()
        {
            return _exchange;
        }

        private void ExchangeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
