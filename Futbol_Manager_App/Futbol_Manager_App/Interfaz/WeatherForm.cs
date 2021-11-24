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
    public partial class WeatherForm : Form
    {
        private Weather _weather1;

        public WeatherForm()
        {
            InitializeComponent();
        }

        public WeatherForm(Weather we)
        {
            InitializeComponent();
            this.textBoxHeader.Text = we.Header;
            this.textBoxGrados.Text = we.Temperatura;
            this.textBoxHumedad.Text = we.Humedad;
            this.textBoxViento.Text = we.Wind;
            this.comboBoxTiempo.Text = we.Tiempo;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _weather1 = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _weather1= new Weather(this.textBoxHeader.Text,this.textBoxGrados.Text,this.textBoxHumedad.Text,this.textBoxViento.Text,this.comboBoxTiempo.Text);
            this.Close();
        }               

        public  Weather   GetWeather()
        {
            return _weather1;
        }

        private void buttonCancelar_Click_1(object sender, EventArgs e)
        {
            _weather1 = null;
            this.Close();
        }
    }
}
