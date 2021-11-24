using Futbol_Sala_Manager_App.Beans;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class PerfilJugadorForm : Form
    {
        private PerfilJugador _perfilJugador;
        string photoPath = "";

        public PerfilJugadorForm()
        {
            InitializeComponent();
        }

        public PerfilJugadorForm(PerfilJugador pj)
        {
            InitializeComponent();
            LoadData(pj);
        }

        void LoadData(PerfilJugador pj)
        {
            textBox_stat1.Text = pj.stat1;
            textBox_stat2.Text = pj.stat2;
            textBox_stat3.Text = pj.stat3;
            textBox_stat4.Text = pj.stat4;
            textBox_stat5.Text = pj.stat5;
            textBox_stat6.Text = pj.stat6;
            textBox_stat7.Text = pj.stat7;
            textBox_stat8.Text = pj.stat8;
            photoPath = pj.photoPath;
            try
            {
                if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                    pictureBox1.Image = new Bitmap(photoPath);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine(e.Source);
            }
        }

        private void buttonFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png";

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    photoPath = dlg.FileName;
                    pictureBox1.Image = new Bitmap(dlg.FileName);
                }

                dlg.Dispose();
            }
        }
        
        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _perfilJugador = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _perfilJugador = new PerfilJugador
            {
                stat6 = textBox_stat6.Text,
                stat7 = textBox_stat7.Text,
                stat8 = textBox_stat8.Text,
                stat1 = textBox_stat1.Text,
                stat2 = textBox_stat2.Text,
                stat3 = textBox_stat3.Text,
                stat4 = textBox_stat4.Text,
                stat5 = textBox_stat5.Text,
                photoPath = photoPath
            };
            this.Close();
        }

        public PerfilJugador GetPerfilJugador()
        {
            return _perfilJugador;
        }
    }
}
