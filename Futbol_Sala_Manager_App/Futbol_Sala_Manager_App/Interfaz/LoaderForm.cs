using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Persistencia;
using System.Reflection;
using System.IO;

namespace Futbol_Sala_Manager_App.Interfaz
{
    /**
     * Formulario de carga
     */
    public partial class LoaderForm : Form
    {
        private Form _form;
        private Loader _controlador;
        private bool _cargarBackup = false;

        /**
         * Constructor
         * Crea un controlador Loader y le cede el control
         */
        public LoaderForm(bool dummyDB)
        {
            InitializeComponent();

            Program.InicializaIpfsSeleccionados();

            // Version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.labelVersion.Text = "Versión " + version.ToString();
            this.labelVersion.Visible = false;

            // Carga el controlador
            _controlador = new Loader(this, dummyDB);
        }
        public LoaderForm(bool dummyDB, Form form)
        {
            InitializeComponent();

            Program.InicializaIpfsSeleccionados();

            // Version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            this.labelVersion.Text = "Versión " + version.ToString();

            // Carga el controlador
            _controlador = new Loader(this, dummyDB);

            this._form = form;
        }

        private void LoaderForm_Shown(object sender, EventArgs e)
        {
            // Preguntamos al usuario si quiere cargar el backup (para evitar que se le olvide)
            string mensaje = "¿ Quiere cargar el último backup ?";
            MessageBoxButtons botones = MessageBoxButtons.YesNo;
            string caption = "Información";
            DialogResult result;
            result = MessageBox.Show(mensaje, caption, botones);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //buttonBackup.PerformClick();
                _cargarBackup = true;
                deshabilita_botones_encuentros();
            }
        }

        // Método que deshabilita los botones de todos los encuentros cargados en la interfaz
        private void deshabilita_botones_encuentros()
        {
            // Deshabilitamos los botones de todos los encuentros
            for (int i = 0; i < listBox.Controls.Count; i++)
            {
                listBox.Controls[i].Enabled = false;
            }
        }


        /**
         * Carga la lista de encuentros en el interfaz
         */
        public void CargaListaPartidos(List<Encuentro> encuentros)
        {
            listBox.Controls.Clear();
            if (encuentros != null)
            {
                foreach (Encuentro s in encuentros)
                {
                    Button b = new Button();
                    b.Dock = DockStyle.Top;
                    b.Height = 45;
                    b.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    b.Text = s.Descripcion;
                    b.ImageAlign = ContentAlignment.MiddleRight;
                    b.Image = (s.Ida ? Properties.Resources.ida : Properties.Resources.vuelta);
                    
                    int id = s.Id;
                    b.Click += delegate { _controlador.CargaEncuentro(id); };

                    listBox.Controls.Add(b);
                }

                if (_cargarBackup)
                    deshabilita_botones_encuentros();
            }
            else // Error en carga de encuentros
            {
                Label b = new Label();
                b.Dock = DockStyle.Fill;
                b.Height = 45;
                b.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                b.ForeColor = Color.Red;
                b.Text = "No se puede establecer conexión con la Base de Datos";

                listBox.Controls.Add(b);
            }
        }

        /**
         * Carga los datos del Backup en el interfaz
         */
        public void CargaBackup(EncuentroData datos)
        {
            if (datos != null)
            {
                string partido = datos.EquipoL.FullName + " - " + datos.EquipoV.FullName;
                DateTime fecha = PersistenciaUtil.GetBackupFecha();
                string strfecha = fecha.ToShortDateString() + " " + fecha.ToLongTimeString();
                this.buttonBackup.Text = "Backup " + partido + " (" + strfecha + ")";
            }
            else // No hay backup
            {
                this.buttonBackup.Text = "Backup no disponible";
                this.buttonBackup.Enabled = false;
            }
        }


        // ****************************** EVENTOS *****************************

        private void buttonConf_Click(object sender, EventArgs e)
        {
            _controlador.AbrirConfiguracion();
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            _controlador.CargaEncuentroBackup();
        }

        private void LoaderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}
