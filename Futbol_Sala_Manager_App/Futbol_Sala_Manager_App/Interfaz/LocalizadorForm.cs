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
    public partial class LocalizadorForm : Form
    {
        private Localizador _localizador;

        public LocalizadorForm()
        {
            InitializeComponent();
        }

        public LocalizadorForm(Localizador l)
        {
            InitializeComponent();
            this.textBox_title.Text = l.Title;
            this.textBox_localizador.Text = l.TextoLocalizador;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _localizador = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _localizador = new Localizador(this.textBox_title.Text, this.textBox_localizador.Text);
            this.Close();
        }

        public Localizador GetLocalizador()
        {
            return _localizador;
        }
    }
}
