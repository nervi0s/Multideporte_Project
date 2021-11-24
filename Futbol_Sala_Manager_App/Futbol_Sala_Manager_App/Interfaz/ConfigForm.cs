using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.AccesoBD;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Interfaz
{

    /**
     * Formulario de edición de la Configuración
     */
    public partial class ConfigForm : Form
    {

        private const int OK = 0;
        private const int Error = 1;
        private const int Untested = 2;
        private const int Testing = 3;


        /**
         * Constructor
         * Carga la configuración actual, empleando Persistencia.
         */
        public ConfigForm()
        {
            InitializeComponent();

            configInicialInterfaz();
            cargaComboIdioma();
            cargaComboDifusion();

            // Si existe configuración actual se carga en el form
            ConfigData configActual = PersistenciaUtil.CargaConfig();
            if (configActual != null)
                setConfig(configActual);
            //comboBox_mode.SelectedIndex = 1;                                    // Se elige como opción predeterminada el modo Manual de la aplicación
            ActiveControl = this.label1;
        }

        /**
         * Recupera la Configuración representada en el formulario
         */
        private ConfigData config = new ConfigData();
        public ConfigData getConfig()
        {
            // Comun
            config.NumIpf = Convert.ToInt32(this.numericUpDown_num_ipfs.Value);
            config.BaseDatos = this.textBoxBaseDatos.Text;

            if (this.comboBox_mode.SelectedIndex == 0) // Seleccionado Modo OCR en el formulario de configuración
            {
                config.modeOcrActivated = true;
                config.isMachineCrono = false;
                config.wasStardedMachineCrono = false;
            }
            else // Seleccionado Modo Manual en el formulario de configuración
            {
                config.modeOcrActivated = false;
                config.isMachineCrono = this.comboBox_machine.SelectedIndex != 0;
                config.wasStardedMachineCrono = config.isMachineCrono;
            }

            config.ipAddress = this.textBox_ip.Text;
            config.port = this.portOcrServer.Text;
            config.puertoEscuchaMaquinaPrincipal = this.textBox_PuertoPrincipalEscuchaCrono.Text;
            ConfigData.ModeOcrActivated = config.modeOcrActivated;              // Se asigna un valor a la variable estática para poder conocer el estado del modo OCR desde otras clases (Momento.cs)
            config.ipOcrServer = this.textBox_ip_ocr_server.Text;
            config.portOcrServer = this.textBox_port_ocr_server.Text;

            config.duracionParte_i = this.textBox_duracion_parte.Text;
            config.duracionProrroga_i = this.textBox_duracion_prorroga.Text;
            // ipf 1
            config.IpfIp = this.textBox_IP_1.Text;
            config.IdiomaFichero = this.comboBox_idioma_1.SelectedItem.ToString();
            if (comboBox_difusion_1.Text == "Unilateral")
                config.Multicast = false;
            else
                config.Multicast = true;

            // ipf 2
            config.IpfIp2 = this.textBox_IP_2.Text;
            config.IdiomaFichero2 = this.comboBox_idioma_2.SelectedItem.ToString();
            if (comboBox_difusion_2.Text == "Unilateral")
                config.Multicast2 = false;
            else
                config.Multicast2 = true;

            // ipf 3
            config.IpfIp3 = this.textBox_IP_3.Text;
            config.IdiomaFichero3 = this.comboBox_idioma_3.SelectedItem.ToString();
            if (comboBox_difusion_3.Text == "Unilateral")
                config.Multicast3 = false;
            else
                config.Multicast3 = true;

            // ipf 4
            config.IpfIp4 = this.textBox_IP_4.Text;
            config.IdiomaFichero4 = this.comboBox_idioma_4.SelectedItem.ToString();
            if (comboBox_difusion_4.Text == "Unilateral")
                config.Multicast4 = false;
            else
                config.Multicast4 = true;

            // ipf 5
            config.IpfIp5 = this.textBox_IP_5.Text;
            config.IdiomaFichero5 = this.comboBox_idioma_5.SelectedItem.ToString();
            if (comboBox_difusion_5.Text == "Unilateral")
                config.Multicast5 = false;
            else
                config.Multicast5 = true;

            // ipf 6
            config.IpfIp6 = this.textBox_IP_6.Text;
            config.IdiomaFichero6 = this.comboBox_idioma_6.SelectedItem.ToString();
            if (comboBox_difusion_6.Text == "Unilateral")
                config.Multicast6 = false;
            else
                config.Multicast6 = true;


            // ipf 7
            config.IpfIp7 = this.textBox_IP_7.Text;
            config.IdiomaFichero7 = this.comboBox_idioma_7.SelectedItem.ToString();
            if (comboBox_difusion_7.Text == "Unilateral")
                config.Multicast7 = false;
            else
                config.Multicast7 = true;


            // ipf 8
            config.IpfIp8 = this.textBox_IP_8.Text;
            config.IdiomaFichero8 = this.comboBox_idioma_8.SelectedItem.ToString();
            if (comboBox_difusion_8.Text == "Unilateral")
                config.Multicast8 = false;
            else
                config.Multicast8 = true;


            // ipf 9
            config.IpfIp9 = this.textBox_IP_9.Text;
            config.IdiomaFichero9 = this.comboBox_idioma_9.SelectedItem.ToString();
            if (comboBox_difusion_9.Text == "Unilateral")
                config.Multicast9 = false;
            else
                config.Multicast9 = true;


            // ipf 10
            config.IpfIp10 = this.textBox_IP_10.Text;
            config.IdiomaFichero10 = this.comboBox_idioma_10.SelectedItem.ToString();
            if (comboBox_difusion_10.Text == "Unilateral")
                config.Multicast10 = false;
            else
                config.Multicast10 = true;


            return config;
        }

        // Representa en el formulario la información de la Configuración indicada
        private void setConfig(ConfigData config)
        {

            if (!config.isMachineCrono)
            {
                if (config.modeOcrActivated)
                {
                    this.comboBox_mode.SelectedIndex = 0;
                }
                else
                {
                    this.comboBox_mode.SelectedIndex = 1;
                }
            }
            else
            {
                this.label_puertoPrincipalEscuchaCrono.Visible = false;
                this.textBox_PuertoPrincipalEscuchaCrono.Visible = false;
            }

            //this.textBox_PuertoPrincipalEscuchaCrono.Text = config.puertoEscuchaMaquinaPrincipal;
            this.textBox_duracion_parte.Text = config.duracionParte_i;
            this.textBox_duracion_prorroga.Text = config.duracionProrroga_i;

            if (config.NumIpf > 0)
                this.numericUpDown_num_ipfs.Value = config.NumIpf;
            this.textBoxBaseDatos.Text = config.BaseDatos;
            this.comboBox_machine.SelectedIndex = config.isMachineCrono ? 1 : 0;
            this.textBox_ip.Text = config.ipAddress;
            this.textBox_port_ocr_server.Text = config.portOcrServer;
            this.portOcrServer.Text = config.port;
            this.textBox_PuertoPrincipalEscuchaCrono.Text = config.puertoEscuchaMaquinaPrincipal;

            // Ipf 1
            this.textBox_IP_1.Text = config.IpfIp;
            this.comboBox_idioma_1.SelectedItem = config.IdiomaFichero;
            if (config.Multicast)
                this.comboBox_difusion_1.Text = "Multilateral";
            else
                this.comboBox_difusion_1.Text = "Unilateral";


            // Ipf 2
            this.textBox_IP_2.Text = config.IpfIp2;
            this.comboBox_idioma_2.SelectedItem = config.IdiomaFichero2;
            if (config.Multicast2)
                this.comboBox_difusion_2.Text = "Multilateral";
            else
                this.comboBox_difusion_2.Text = "Unilateral";

            // Ipf 3
            this.textBox_IP_3.Text = config.IpfIp3;
            this.comboBox_idioma_3.SelectedItem = config.IdiomaFichero3;
            if (config.Multicast3)
                this.comboBox_difusion_3.Text = "Multilateral";
            else
                this.comboBox_difusion_3.Text = "Unilateral";

            // Ipf 4
            this.textBox_IP_4.Text = config.IpfIp4;
            this.comboBox_idioma_4.SelectedItem = config.IdiomaFichero4;
            if (config.Multicast4)
                this.comboBox_difusion_4.Text = "Multilateral";
            else
                this.comboBox_difusion_4.Text = "Unilateral";

            // Ipf 5
            this.textBox_IP_5.Text = config.IpfIp5;
            this.comboBox_idioma_5.SelectedItem = config.IdiomaFichero5;
            if (config.Multicast5)
                this.comboBox_difusion_5.Text = "Multilateral";
            else
                this.comboBox_difusion_5.Text = "Unilateral";

            // Ipf 6
            this.textBox_IP_6.Text = config.IpfIp6;
            this.comboBox_idioma_6.SelectedItem = config.IdiomaFichero6;
            if (config.Multicast6)
                this.comboBox_difusion_6.Text = "Multilateral";
            else
                this.comboBox_difusion_6.Text = "Unilateral";

            // Ipf 7
            this.textBox_IP_7.Text = config.IpfIp7;
            this.comboBox_idioma_7.SelectedItem = config.IdiomaFichero7;
            if (config.Multicast7)
                this.comboBox_difusion_7.Text = "Multilateral";
            else
                this.comboBox_difusion_7.Text = "Unilateral";

            // Ipf 8
            this.textBox_IP_8.Text = config.IpfIp8;
            this.comboBox_idioma_8.SelectedItem = config.IdiomaFichero8;
            if (config.Multicast8)
                this.comboBox_difusion_8.Text = "Multilateral";
            else
                this.comboBox_difusion_8.Text = "Unilateral";

            // Ipf 9
            this.textBox_IP_9.Text = config.IpfIp9;
            this.comboBox_idioma_9.SelectedItem = config.IdiomaFichero9;
            if (config.Multicast9)
                this.comboBox_difusion_9.Text = "Multilateral";
            else
                this.comboBox_difusion_9.Text = "Unilateral";

            // Ipf 10
            this.textBox_IP_10.Text = config.IpfIp10;
            this.comboBox_idioma_10.SelectedItem = config.IdiomaFichero10;
            if (config.Multicast10)
                this.comboBox_difusion_10.Text = "Multilateral";
            else
                this.comboBox_difusion_10.Text = "Unilateral";
        }


        // ****************************** EVENTOS *****************************
        private void buttonBuscarBD_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Base de Datos|*.sqlite";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                textBoxBaseDatos.Text = ofd.FileName;
                configLabelTest(this.labelTestBD, Untested);
            }
        }

        private void buttonTestBD_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestBD, Testing);

            IAccesoBD bd = new AccessData(this.textBoxBaseDatos.Text);
            bool ok = bd.TestConexion();

            configLabelTest(this.labelTestBD, (ok ? OK : Error));
        }

        private void buttonTestIP_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_1, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_1.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_1, (ok ? OK : Error));
        }

        private void buttonGuardar_Click(object sender, EventArgs e)
        {
            if (checkConfig())
            {
                PersistenciaUtil.GuardaConfig(getConfig());
                this.Close();
            }
        }

        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_1, Untested);
        }
        // ********************************************************************


        // Comprueba que los datos actuales son correctos
        private bool checkConfig()
        {
            if (this.textBox_IP_1.Text.Equals(""))
            {
                MessageBox.Show("Se debe definir la IP de conexión a Brainstorm.", "Campo requerido", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        // Puebla la lista de idiomas
        private void cargaComboIdioma()
        {
            FileInfo[] idiomas = PersistenciaUtil.GetListaFicherosIdioma();

            foreach (FileInfo fi in idiomas)
            {
                comboBox_idioma_1.Items.Add(fi.Name);
                comboBox_idioma_2.Items.Add(fi.Name);
                comboBox_idioma_3.Items.Add(fi.Name);
                comboBox_idioma_4.Items.Add(fi.Name);
                comboBox_idioma_5.Items.Add(fi.Name);
                comboBox_idioma_6.Items.Add(fi.Name);
                comboBox_idioma_7.Items.Add(fi.Name);
                comboBox_idioma_8.Items.Add(fi.Name);
                comboBox_idioma_9.Items.Add(fi.Name);
                comboBox_idioma_10.Items.Add(fi.Name);
            }

            comboBox_idioma_1.SelectedIndex = 0;
            comboBox_idioma_2.SelectedIndex = 0;
            comboBox_idioma_3.SelectedIndex = 0;
            comboBox_idioma_4.SelectedIndex = 0;
            comboBox_idioma_5.SelectedIndex = 0;
            comboBox_idioma_6.SelectedIndex = 0;
            comboBox_idioma_7.SelectedIndex = 0;
            comboBox_idioma_8.SelectedIndex = 0;
            comboBox_idioma_9.SelectedIndex = 0;
            comboBox_idioma_10.SelectedIndex = 0;
        }

        //Carga opciones de difusion
        private void cargaComboDifusion()
        {
            comboBox_difusion_1.SelectedIndex = 0;
        }

        // Cambia el texto y el color de una etiqueta en función de su estado
        private void configLabelTest(Label label, int estado)
        {
            switch (estado)
            {
                case OK:
                    label.BackColor = Color.Green;
                    label.Text = "Correcto";
                    break;

                case Error:
                    label.BackColor = Color.Red;
                    label.Text = "Error";
                    break;

                case Untested:
                    label.BackColor = Color.Gray;
                    label.Text = "";
                    break;

                case Testing:
                    label.BackColor = Color.Gray;
                    label.Text = "Conectando...";
                    break;
            }
            label.Update();
        }

        // Prepara la interfaz para el Número de Ipf's seleccionados.
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_num_ipfs.Value == 0)
            {
                this.Size = new System.Drawing.Size(1050, 345);
                ocultaControlesInterfaz(0);
            }

            else if (numericUpDown_num_ipfs.Value == 1)
            {
                this.Size = new System.Drawing.Size(1050, 345);
                presentaControlesInterfaz(1);
                ocultaControlesInterfaz(1);
            }

            else if (numericUpDown_num_ipfs.Value == 2)
            {
                this.Size = new System.Drawing.Size(1050, 320);
                presentaControlesInterfaz(2);
                ocultaControlesInterfaz(2);
            }

            else if (numericUpDown_num_ipfs.Value == 3)
            {
                this.Size = new System.Drawing.Size(1050, 355);
                presentaControlesInterfaz(3);
                ocultaControlesInterfaz(3);
            }

            else if (numericUpDown_num_ipfs.Value == 4)
            {
                this.Size = new System.Drawing.Size(1050, 390);
                presentaControlesInterfaz(4);
                ocultaControlesInterfaz(4);
            }

            else if (numericUpDown_num_ipfs.Value == 5)
            {
                this.Size = new System.Drawing.Size(1050, 425);
                presentaControlesInterfaz(5);
                ocultaControlesInterfaz(5);
            }

            else if (numericUpDown_num_ipfs.Value == 6)
            {
                this.Size = new System.Drawing.Size(1050, 460);
                presentaControlesInterfaz(6);
                ocultaControlesInterfaz(6);
            }

            else if (numericUpDown_num_ipfs.Value == 7)
            {
                this.Size = new System.Drawing.Size(1050, 495);
                presentaControlesInterfaz(7);
                ocultaControlesInterfaz(7);
            }

            else if (numericUpDown_num_ipfs.Value == 8)
            {
                this.Size = new System.Drawing.Size(1050, 530);
                presentaControlesInterfaz(8);
                ocultaControlesInterfaz(8);
            }

            else if (numericUpDown_num_ipfs.Value == 9)
            {
                this.Size = new System.Drawing.Size(1050, 565);
                presentaControlesInterfaz(9);
                ocultaControlesInterfaz(9);
            }

            else if (numericUpDown_num_ipfs.Value == 10)
            {
                this.Size = new System.Drawing.Size(1050, 600);
                presentaControlesInterfaz(10);
                ocultaControlesInterfaz(10);
            }
        }

        // Configuracion inicial de la interfaz
        private void configInicialInterfaz()
        {
            this.Size = new System.Drawing.Size(1050, 345);
        }

        // Oculta controles de la interfaz
        private void ocultaControlesInterfaz(int n)
        {
            if (1 > n)
            {
                label_IP_1.Visible = false;
                textBox_IP_1.Visible = false;
                label_idioma_1.Visible = false;
                comboBox_idioma_1.Visible = false;
                label_difusion_1.Visible = false;
                comboBox_difusion_1.Visible = false;
                button_test_IP_1.Visible = false;
                label_test_IPF_1.Visible = false;
            }

            if (2 > n)
            {
                label_IP_2.Visible = false;
                textBox_IP_2.Visible = false;
                label_idioma_2.Visible = false;
                comboBox_idioma_2.Visible = false;
                label_difusion_2.Visible = false;
                comboBox_difusion_2.Visible = false;
                button_test_IP_2.Visible = false;
                label_test_IPF_2.Visible = false;
            }

            if (3 > n)
            {
                label_IP_3.Visible = false;
                textBox_IP_3.Visible = false;
                label_idioma_3.Visible = false;
                comboBox_idioma_3.Visible = false;
                label_difusion_3.Visible = false;
                comboBox_difusion_3.Visible = false;
                button_test_IP_3.Visible = false;
                label_test_IPF_3.Visible = false;
            }

            if (4 > n)
            {
                label_IP_4.Visible = false;
                textBox_IP_4.Visible = false;
                label_idioma_4.Visible = false;
                comboBox_idioma_4.Visible = false;
                label_difusion_4.Visible = false;
                comboBox_difusion_4.Visible = false;
                button_test_IP_4.Visible = false;
                label_test_IPF_4.Visible = false;
            }

            if (5 > n)
            {
                label_IP_5.Visible = false;
                textBox_IP_5.Visible = false;
                label_idioma_5.Visible = false;
                comboBox_idioma_5.Visible = false;
                label_difusion_5.Visible = false;
                comboBox_difusion_5.Visible = false;
                button_test_IP_5.Visible = false;
                label_test_IPF_5.Visible = false;
            }

            if (6 > n)
            {
                label_IP_6.Visible = false;
                textBox_IP_6.Visible = false;
                label_idioma_6.Visible = false;
                comboBox_idioma_6.Visible = false;
                label_difusion_6.Visible = false;
                comboBox_difusion_6.Visible = false;
                button_test_IP_6.Visible = false;
                label_test_IPF_6.Visible = false;
            }

            if (7 > n)
            {
                label_IP_7.Visible = false;
                textBox_IP_7.Visible = false;
                label_idioma_7.Visible = false;
                comboBox_idioma_7.Visible = false;
                label_difusion_7.Visible = false;
                comboBox_difusion_7.Visible = false;
                button_test_IP_7.Visible = false;
                label_test_IPF_7.Visible = false;
            }

            if (8 > n)
            {
                label_IP_8.Visible = false;
                textBox_IP_8.Visible = false;
                label_idioma_8.Visible = false;
                comboBox_idioma_8.Visible = false;
                label_difusion_8.Visible = false;
                comboBox_difusion_8.Visible = false;
                button_test_IP_8.Visible = false;
                label_test_IPF_8.Visible = false;
            }

            if (9 > n)
            {
                label_IP_9.Visible = false;
                textBox_IP_9.Visible = false;
                label_idioma_9.Visible = false;
                comboBox_idioma_9.Visible = false;
                label_difusion_9.Visible = false;
                comboBox_difusion_9.Visible = false;
                button_test_IP_9.Visible = false;
                label_test_IPF_9.Visible = false;
            }

            if (10 > n)
            {
                label_IP_10.Visible = false;
                textBox_IP_10.Visible = false;
                label_idioma_10.Visible = false;
                comboBox_idioma_10.Visible = false;
                label_difusion_10.Visible = false;
                comboBox_difusion_10.Visible = false;
                button_test_IP_10.Visible = false;
                label_test_IPF_10.Visible = false;
            }


        }

        // Presenta controles de la interfaz
        private void presentaControlesInterfaz(int n)
        {
            if (n >= 1)
            {
                label_IP_1.Visible = true;
                textBox_IP_1.Visible = true;
                label_idioma_1.Visible = true;
                comboBox_idioma_1.Visible = true;
                label_difusion_1.Visible = true;
                comboBox_difusion_1.Visible = true;
                button_test_IP_1.Visible = true;
                label_test_IPF_1.Visible = true;
            }

            if (n >= 2)
            {
                label_IP_2.Visible = true;
                textBox_IP_2.Visible = true;
                label_idioma_2.Visible = true;
                comboBox_idioma_2.Visible = true;
                label_difusion_2.Visible = true;
                comboBox_difusion_2.Visible = true;
                button_test_IP_2.Visible = true;
                label_test_IPF_2.Visible = true;
            }

            if (n >= 3)
            {
                label_IP_3.Visible = true;
                textBox_IP_3.Visible = true;
                label_idioma_3.Visible = true;
                comboBox_idioma_3.Visible = true;
                label_difusion_3.Visible = true;
                comboBox_difusion_3.Visible = true;
                button_test_IP_3.Visible = true;
                label_test_IPF_3.Visible = true;
            }

            if (n >= 4)
            {
                label_IP_4.Visible = true;
                textBox_IP_4.Visible = true;
                label_idioma_4.Visible = true;
                comboBox_idioma_4.Visible = true;
                label_difusion_4.Visible = true;
                comboBox_difusion_4.Visible = true;
                button_test_IP_4.Visible = true;
                label_test_IPF_4.Visible = true;
            }

            if (n >= 5)
            {
                label_IP_5.Visible = true;
                textBox_IP_5.Visible = true;
                label_idioma_5.Visible = true;
                comboBox_idioma_5.Visible = true;
                label_difusion_5.Visible = true;
                comboBox_difusion_5.Visible = true;
                button_test_IP_5.Visible = true;
                label_test_IPF_5.Visible = true;
            }

            if (n >= 6)
            {
                label_IP_6.Visible = true;
                textBox_IP_6.Visible = true;
                label_idioma_6.Visible = true;
                comboBox_idioma_6.Visible = true;
                label_difusion_6.Visible = true;
                comboBox_difusion_6.Visible = true;
                button_test_IP_6.Visible = true;
                label_test_IPF_6.Visible = true;
            }

            if (n >= 7)
            {
                label_IP_7.Visible = true;
                textBox_IP_7.Visible = true;
                label_idioma_7.Visible = true;
                comboBox_idioma_7.Visible = true;
                label_difusion_7.Visible = true;
                comboBox_difusion_7.Visible = true;
                button_test_IP_7.Visible = true;
                label_test_IPF_7.Visible = true;
            }

            if (n >= 8)
            {
                label_IP_8.Visible = true;
                textBox_IP_8.Visible = true;
                label_idioma_8.Visible = true;
                comboBox_idioma_8.Visible = true;
                label_difusion_8.Visible = true;
                comboBox_difusion_8.Visible = true;
                button_test_IP_8.Visible = true;
                label_test_IPF_8.Visible = true;
            }

            if (n >= 9)
            {
                label_IP_9.Visible = true;
                textBox_IP_9.Visible = true;
                label_idioma_9.Visible = true;
                comboBox_idioma_9.Visible = true;
                label_difusion_9.Visible = true;
                comboBox_difusion_9.Visible = true;
                button_test_IP_9.Visible = true;
                label_test_IPF_9.Visible = true;
            }

            if (n >= 10)
            {
                label_IP_10.Visible = true;
                textBox_IP_10.Visible = true;
                label_idioma_10.Visible = true;
                comboBox_idioma_10.Visible = true;
                label_difusion_10.Visible = true;
                comboBox_difusion_10.Visible = true;
                button_test_IP_10.Visible = true;
                label_test_IPF_10.Visible = true;
            }
        }

        private void buttonTestIP2_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_2, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_2.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_2, (ok ? OK : Error));
        }

        private void buttonTestIP3_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_3, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_3.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_3, (ok ? OK : Error));
        }

        private void buttonTestIP4_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_4, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_4.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_4, (ok ? OK : Error));
        }

        private void buttonTestIP5_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_5, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_5.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_5, (ok ? OK : Error));
        }

        private void buttonTestIP6_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_6, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_6.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_6, (ok ? OK : Error));
        }

        private void buttonTestIP7_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_7, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_7.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_7, (ok ? OK : Error));
        }

        private void buttonTestIP8_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_8, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_8.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_8, (ok ? OK : Error));
        }

        private void buttonTestIP9_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_9, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_9.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_9, (ok ? OK : Error));
        }

        private void buttonTestIP10_Click(object sender, EventArgs e)
        {
            configLabelTest(this.label_test_IPF_10, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBox_IP_10.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.label_test_IPF_10, (ok ? OK : Error));
        }

        private void comboBox_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ((ComboBox)sender).SelectedIndex;

            if (selectedIndex == 0)         // OCR
            {
                comboBox_machine.Visible = false;
                label_manual_ip.Visible = false;
                textBox_ip.Visible = false;
                label_manual_puerto.Visible = false;
                portOcrServer.Visible = false;

                label5.Visible = true;
                textBox_ip_ocr_server.Visible = true;
                label3.Visible = true;
                textBox_port_ocr_server.Visible = true;

                config.isMachineCrono = false;
                config.wasStardedMachineCrono = false;

                this.panel_config_modo_arranque.Visible = false;
                this.panel_config_ocr.Visible = true;
            }
            else if (selectedIndex == 1)    // Manual
            {
                comboBox_machine.Visible = true;

                if (config.isMachineCrono)
                {
                    label_manual_ip.Visible = true;
                    textBox_ip.Visible = true;
                    label_manual_puerto.Visible = true;
                    portOcrServer.Visible = true;
                }

                label5.Visible = false;
                textBox_ip_ocr_server.Visible = false;
                label3.Visible = false;
                textBox_port_ocr_server.Visible = false;

                config.modeOcrActivated = false;

                this.panel_config_modo_arranque.Visible = true;
                this.panel_config_ocr.Visible = false;
            }
        }

        private void comboBox_machine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox_machine.SelectedIndex == 0)
            {
                // Seleccionado modo "Principal"
                this.label_manual_ip.Visible = false;
                this.textBox_ip.Visible = false;
                this.label_manual_puerto.Visible = false;
                this.portOcrServer.Visible = false;
                this.numericUpDown_num_ipfs.Value = 1;
                this.label_puertoPrincipalEscuchaCrono.Visible = true;
                this.textBox_PuertoPrincipalEscuchaCrono.Visible = true;
            }
            else
            {
                // Seleccionado modo "Cronómetro"
                this.label_manual_ip.Visible = true;
                this.textBox_ip.Visible = true;
                this.label_manual_puerto.Visible = true;
                this.portOcrServer.Visible = true;
                this.numericUpDown_num_ipfs.Value = 0;
                this.label_puertoPrincipalEscuchaCrono.Visible = false;
                this.textBox_PuertoPrincipalEscuchaCrono.Visible = false;
            }
        }

    }
}
