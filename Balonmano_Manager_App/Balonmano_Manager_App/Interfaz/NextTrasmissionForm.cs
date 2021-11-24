using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Balonmano_Manager_App.Beans;

namespace Balonmano_Manager_App.Interfaz
{
    public partial class NextTransmissionForm : Form
    {
        private NextTransmission _nextTransmittion;

        public NextTransmissionForm()
        {
            InitializeComponent();
        }

        public NextTransmissionForm(NextTransmission nt)
        {
            InitializeComponent();

            this.textBox_grupo.Text = nt.Grupo;
            this.textBox_equipo1.Text = nt.Equipo1;
            this.textBox_equipo2.Text = nt.Equipo2;
            this.textBox_lugar.Text = nt.Lugar;
            this.textBox_hora.Text = nt.Hora;
        }

        private void button_cancelar_Click(object sender, EventArgs e)
        {
            _nextTransmittion = null;
            this.Close();
        }

        private void button_aceptar_Click(object sender, EventArgs e)
        {
            _nextTransmittion = new NextTransmission(this.textBox_grupo.Text, this.textBox_equipo1.Text, this.textBox_equipo2.Text, this.textBox_lugar.Text, this.textBox_hora.Text);
            this.Close();
        }

        public NextTransmission GetNextTransmission()
        {
            return _nextTransmittion;
        }
    }
}
