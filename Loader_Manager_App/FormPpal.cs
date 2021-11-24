using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Loader_Manager_App.Clases;

namespace Loader_Setup_App
{
    public partial class FormPpal : Form
    {
        private Licencia _licencia = new Licencia();


        public FormPpal()
        {
            InitializeComponent();
        }              


        private void FormPpal_Load(object sender, EventArgs e)
        {
            //string codigo_licencia = this._licencia.dame_licencia();
            //if (codigo_licencia.Contains("no licenciado"))
            //{
            //    this.panel_licencia.BackColor = Color.Crimson;
            //    this.tableLayoutPanel_contenedor.Enabled = false;
            //    this.label_codigo_licencia.Text = codigo_licencia;
            //    //Application.Exit();
            //}
            //else
            //{
            //    if (codigo_licencia.Contains("ATENCIÓN"))
            //        this.panel_licencia.BackColor = Color.Orange;
            //    else
            //        this.panel_licencia.BackColor = SystemColors.Control;
            //    this.tableLayoutPanel_contenedor.Enabled = true;
            //    this.label_codigo_licencia.Text = codigo_licencia;
            //}            
        }

        private void button_balonmano_setup_Click(object sender, EventArgs e)
        {
            Balonmano_Manager_App.Interfaz.LoaderForm form = new Balonmano_Manager_App.Interfaz.LoaderForm(false, this);         
            form.Show();
            this.Hide();
        }

        private void button_futbol_setup_Click(object sender, EventArgs e)
        {
            Futbol_Manager_App.Interfaz.LoaderForm form = new Futbol_Manager_App.Interfaz.LoaderForm(false, this);
            form.Show();
            this.Hide();
        }

        private void button_futbol_sala_setup_Click(object sender, EventArgs e)
        {
            Futbol_Sala_Manager_App.Interfaz.LoaderForm form = new Futbol_Sala_Manager_App.Interfaz.LoaderForm(false, this);
            form.Show();
            this.Hide();
        }

    }
}
