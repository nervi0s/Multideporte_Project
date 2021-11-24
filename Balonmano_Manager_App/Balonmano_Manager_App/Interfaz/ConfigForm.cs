using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Balonmano_Manager_App.AccesoBD;
using Balonmano_Manager_App.Persistencia;

namespace Balonmano_Manager_App.Interfaz
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

            cargaComboIdioma();
            cargaComboDifusion();
            configInicialInterfaz();

            // Si existe configuración actual se carga en el form
            ConfigData configActual = PersistenciaUtil.CargaConfig();
            if (configActual != null)
                setConfig(configActual);

            ActiveControl = this.label1;
        }

        /**
         * Recupera la Configuración representada en el formulario
         */
        public ConfigData getConfig()
        {
            ConfigData config = new ConfigData();

            // Comun
            config.NumIpf = Convert.ToInt32(this.numericUpDown1.Value);
            config.BaseDatos = this.textBoxBaseDatos.Text;
            config.puertoCOM = this.comboBox_puertoCOM.SelectedItem.ToString();

            // ipf 1
            config.IpfIp = this.textBoxIP.Text;            
            config.IdiomaFichero = this.comboBoxIdioma.SelectedItem.ToString();
            if (comboBox1Difusion.Text=="Unilateral")
                config.Multicast = false;
            else
                config.Multicast = true;

            // ipf 2
            config.IpfIp2 = this.textBoxIP2.Text;
            config.IdiomaFichero2 = this.comboBoxIdioma2.SelectedItem.ToString();
            if (comboBox2Difusion.Text == "Unilateral")
                config.Multicast2 = false;
            else
                config.Multicast2 = true;

            // ipf 3
            config.IpfIp3 = this.textBoxIP3.Text;
            config.IdiomaFichero3 = this.comboBoxIdioma3.SelectedItem.ToString();
            if (comboBox3Difusion.Text == "Unilateral")
                config.Multicast3 = false;
            else
                config.Multicast3 = true;

            // ipf 4
            config.IpfIp4 = this.textBoxIP4.Text;
            config.IdiomaFichero4 = this.comboBoxIdioma4.SelectedItem.ToString();
            if (comboBox4Difusion.Text == "Unilateral")
                config.Multicast4 = false;
            else
                config.Multicast4 = true;

            // ipf 5
            config.IpfIp5 = this.textBoxIP5.Text;
            config.IdiomaFichero5 = this.comboBoxIdioma5.SelectedItem.ToString();
            if (comboBox5Difusion.Text == "Unilateral")
                config.Multicast5 = false;
            else
                config.Multicast5 = true;

            // ipf 6
            config.IpfIp6 = this.textBoxIP6.Text;
            config.IdiomaFichero6 = this.comboBoxIdioma6.SelectedItem.ToString();
            if (comboBox6Difusion.Text == "Unilateral")
                config.Multicast6 = false;
            else
                config.Multicast6 = true;
                        
            // ipf 7
            config.IpfIp7 = this.textBoxIP7.Text;
            config.IdiomaFichero7 = this.comboBoxIdioma7.SelectedItem.ToString();
            if (comboBox7Difusion.Text == "Unilateral")
                config.Multicast7 = false;
            else
                config.Multicast7 = true;
            
            // ipf 8
            config.IpfIp8 = this.textBoxIP8.Text;
            config.IdiomaFichero8 = this.comboBoxIdioma8.SelectedItem.ToString();
            if (comboBox8Difusion.Text == "Unilateral")
                config.Multicast8 = false;
            else
                config.Multicast8 = true;
            
            // ipf 9
            config.IpfIp9 = this.textBoxIP9.Text;
            config.IdiomaFichero9 = this.comboBoxIdioma9.SelectedItem.ToString();
            if (comboBox9Difusion.Text == "Unilateral")
                config.Multicast9 = false;
            else
                config.Multicast9 = true;
            
            // ipf 10
            config.IpfIp10 = this.textBoxIP10.Text;
            config.IdiomaFichero10 = this.comboBoxIdioma10.SelectedItem.ToString();
            if (comboBox10Difusion.Text == "Unilateral")
                config.Multicast10 = false;
            else
                config.Multicast10 = true;

                        return config;
        }

        // Representa en el formulario la información de la Configuración indicada
        private void setConfig(ConfigData config)
        {
            if (config.NumIpf>0)
                this.numericUpDown1.Value = config.NumIpf;
            this.textBoxBaseDatos.Text = config.BaseDatos;

            // Puertos COM
            this.comboBox_puertoCOM.Items.AddRange(new object[] {
                "COM1",
                "COM2",
                "COM3",
                "COM4",
                "COM5",
                "COM6",
                "COM7",
                "COM8"
            });

            this.comboBox_puertoCOM.Text = config.puertoCOM;

            // Ipf 1
            this.textBoxIP.Text = config.IpfIp;
            this.comboBoxIdioma.SelectedItem = config.IdiomaFichero;
            if (config.Multicast)
                this.comboBox1Difusion.Text = "Multilateral";
            else
                this.comboBox1Difusion.Text = "Unilateral";
            
            // Ipf 2
            this.textBoxIP2.Text = config.IpfIp2;
            this.comboBoxIdioma2.SelectedItem = config.IdiomaFichero2;
            if (config.Multicast2)
                this.comboBox2Difusion.Text = "Multilateral";
            else
                this.comboBox2Difusion.Text = "Unilateral";

            // Ipf 3
            this.textBoxIP3.Text = config.IpfIp3;
            this.comboBoxIdioma3.SelectedItem = config.IdiomaFichero3;
            if (config.Multicast3)
                this.comboBox3Difusion.Text = "Multilateral";
            else
                this.comboBox3Difusion.Text = "Unilateral";

            // Ipf 4
            this.textBoxIP4.Text = config.IpfIp4;
            this.comboBoxIdioma4.SelectedItem = config.IdiomaFichero4;
            if (config.Multicast4)
                this.comboBox4Difusion.Text = "Multilateral";
            else
                this.comboBox4Difusion.Text = "Unilateral";

            // Ipf 5
            this.textBoxIP5.Text = config.IpfIp5;
            this.comboBoxIdioma5.SelectedItem = config.IdiomaFichero5;
            if (config.Multicast5)
                this.comboBox5Difusion.Text = "Multilateral";
            else
                this.comboBox5Difusion.Text = "Unilateral";

            // Ipf 6
            this.textBoxIP6.Text = config.IpfIp6;
            this.comboBoxIdioma6.SelectedItem = config.IdiomaFichero6;
            if (config.Multicast6)
                this.comboBox6Difusion.Text = "Multilateral";
            else
                this.comboBox6Difusion.Text = "Unilateral";

            // Ipf 7
            this.textBoxIP7.Text = config.IpfIp7;
            this.comboBoxIdioma7.SelectedItem = config.IdiomaFichero7;
            if (config.Multicast7)
                this.comboBox7Difusion.Text = "Multilateral";
            else
                this.comboBox7Difusion.Text = "Unilateral";

            // Ipf 8
            this.textBoxIP8.Text = config.IpfIp8;
            this.comboBoxIdioma8.SelectedItem = config.IdiomaFichero8;
            if (config.Multicast8)
                this.comboBox8Difusion.Text = "Multilateral";
            else
                this.comboBox8Difusion.Text = "Unilateral";

            // Ipf 9
            this.textBoxIP9.Text = config.IpfIp9;
            this.comboBoxIdioma9.SelectedItem = config.IdiomaFichero9;
            if (config.Multicast9)
                this.comboBox9Difusion.Text = "Multilateral";
            else
                this.comboBox9Difusion.Text = "Unilateral";

            // Ipf 10
            this.textBoxIP10.Text = config.IpfIp10;
            this.comboBoxIdioma10.SelectedItem = config.IdiomaFichero10;
            if (config.Multicast10)
                this.comboBox10Difusion.Text = "Multilateral";
            else
                this.comboBox10Difusion.Text = "Unilateral";
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
            configLabelTest(this.labelTestIPF, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF, (ok ? OK : Error));
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
            configLabelTest(this.labelTestIPF, Untested);
        }
        // ********************************************************************


        // Comprueba que los datos actuales son correctos
        private bool checkConfig()
        {
            if (this.textBoxIP.Text.Equals(""))
            {
                MessageBox.Show("Se debe definir la IP de conexión a Brainstorm.", "Campo requerido",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

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
                comboBoxIdioma.Items.Add(fi.Name);
                comboBoxIdioma2.Items.Add(fi.Name);
                comboBoxIdioma3.Items.Add(fi.Name);
                comboBoxIdioma4.Items.Add(fi.Name);
                comboBoxIdioma5.Items.Add(fi.Name);
                comboBoxIdioma6.Items.Add(fi.Name);
                comboBoxIdioma7.Items.Add(fi.Name);
                comboBoxIdioma8.Items.Add(fi.Name);
                comboBoxIdioma9.Items.Add(fi.Name);
                comboBoxIdioma10.Items.Add(fi.Name);
            }

            comboBoxIdioma.SelectedIndex = 0;
            comboBoxIdioma2.SelectedIndex = 0;
            comboBoxIdioma3.SelectedIndex = 0;
            comboBoxIdioma4.SelectedIndex = 0;
            comboBoxIdioma5.SelectedIndex = 0;
            comboBoxIdioma6.SelectedIndex = 0;
            comboBoxIdioma7.SelectedIndex = 0;
            comboBoxIdioma8.SelectedIndex = 0;
            comboBoxIdioma9.SelectedIndex = 0;
            comboBoxIdioma10.SelectedIndex = 0;
        }

        //Carga opciones de difusion
        private void cargaComboDifusion()
        {
            comboBox1Difusion.SelectedIndex = 0;            
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
            if (numericUpDown1.Value == 1)
            {
                this.Size = new System.Drawing.Size(1050, 265);
                ocultaControlesInterfaz(1);
            }

            if (numericUpDown1.Value == 2)
            {
                this.Size = new System.Drawing.Size(1050, 300);
                presentaControlesInterfaz(2);
                ocultaControlesInterfaz(2);
            }

            if (numericUpDown1.Value == 3)
            {
                this.Size = new System.Drawing.Size(1050, 335);
                presentaControlesInterfaz(3);
                ocultaControlesInterfaz(3);
            }

            if (numericUpDown1.Value == 4)
            {
                this.Size = new System.Drawing.Size(1050, 370);
                presentaControlesInterfaz(4);
                ocultaControlesInterfaz(4);
            }

            if (numericUpDown1.Value == 5)
            {
                this.Size = new System.Drawing.Size(1050, 405);
                presentaControlesInterfaz(5);
                ocultaControlesInterfaz(5);
            }

            if (numericUpDown1.Value == 6)
            {
                this.Size = new System.Drawing.Size(1050, 440);
                presentaControlesInterfaz(6);
                ocultaControlesInterfaz(6);
            }

            if (numericUpDown1.Value == 7)
            {
                this.Size = new System.Drawing.Size(1050, 475);
                presentaControlesInterfaz(7);
                ocultaControlesInterfaz(7);
            }

            if (numericUpDown1.Value == 8)
            {
                this.Size = new System.Drawing.Size(1050, 510);
                presentaControlesInterfaz(8);
                ocultaControlesInterfaz(8);
            }

            if (numericUpDown1.Value == 9)
            {
                this.Size = new System.Drawing.Size(1050, 545);
                presentaControlesInterfaz(9);
                ocultaControlesInterfaz(9);
            }

            if (numericUpDown1.Value == 10)
            {
                this.Size = new System.Drawing.Size(1050, 570);
                presentaControlesInterfaz(10);
                ocultaControlesInterfaz(10);
            }
        }

        // Configuracion inicial de la interfaz

        private void configInicialInterfaz()
        {
            this.Size = new System.Drawing.Size(1050, 265);
        }


        // Oculta controles de la interfaz
        private void ocultaControlesInterfaz(int n)
        {
            if (2 > n)
            {
                labelIP2.Visible = false;
                textBoxIP2.Visible = false;
                labelidioma2.Visible = false;
                comboBoxIdioma2.Visible = false;
                labelDifusion2.Visible = false;
                comboBox2Difusion.Visible = false;
                buttonTestIP2.Visible = false;
                labelTestIPF2.Visible = false;
            }

            if (3 > n)
            {
                labelIP3.Visible = false;
                textBoxIP3.Visible = false;
                labelidioma3.Visible = false;
                comboBoxIdioma3.Visible = false;
                labelDifusion3.Visible = false;
                comboBox3Difusion.Visible = false;
                buttonTestIP3.Visible = false;
                labelTestIPF3.Visible = false;
            }

            if (4 > n)
            {
                labelIP4.Visible = false;
                textBoxIP4.Visible = false;
                labelidioma4.Visible = false;
                comboBoxIdioma4.Visible = false;
                labelDifusion4.Visible = false;
                comboBox4Difusion.Visible = false;
                buttonTestIP4.Visible = false;
                labelTestIPF4.Visible = false;
            }

            if (5 > n)
            {
                labelIP5.Visible = false;
                textBoxIP5.Visible = false;
                labelidioma5.Visible = false;
                comboBoxIdioma5.Visible = false;
                labelDifusion5.Visible = false;
                comboBox5Difusion.Visible = false;
                buttonTestIP5.Visible = false;
                labelTestIPF5.Visible = false;
            }

            if (6 > n)
            {
                labelIP6.Visible = false;
                textBoxIP6.Visible = false;
                labelidioma6.Visible = false;
                comboBoxIdioma6.Visible = false;
                labelDifusion6.Visible = false;
                comboBox6Difusion.Visible = false;
                buttonTestIP6.Visible = false;
                labelTestIPF6.Visible = false;
            }

            if (7 > n)
            {
                labelIP7.Visible = false;
                textBoxIP7.Visible = false;
                labelidioma7.Visible = false;
                comboBoxIdioma7.Visible = false;
                labelDifusion7.Visible = false;
                comboBox7Difusion.Visible = false;
                buttonTestIP7.Visible = false;
                labelTestIPF7.Visible = false;
            }

            if (8 > n)
            {
                labelIP8.Visible = false;
                textBoxIP8.Visible = false;
                labelidioma8.Visible = false;
                comboBoxIdioma8.Visible = false;
                labelDifusion8.Visible = false;
                comboBox8Difusion.Visible = false;
                buttonTestIP8.Visible = false;
                labelTestIPF8.Visible = false;
            }

            if (9 > n)
            {
                labelIP9.Visible = false;
                textBoxIP9.Visible = false;
                labelidioma9.Visible = false;
                comboBoxIdioma9.Visible = false;
                labelDifusion9.Visible = false;
                comboBox9Difusion.Visible = false;
                buttonTestIP9.Visible = false;
                labelTestIPF9.Visible = false;
            }

            if (10 > n)
            {
                labelIP10.Visible = false;
                textBoxIP10.Visible = false;
                labelidioma10.Visible = false;
                comboBoxIdioma10.Visible = false;
                labelDifusion10.Visible = false;
                comboBox10Difusion.Visible = false;
                buttonTestIP10.Visible = false;
                labelTestIPF10.Visible = false;
            }


        }

        // Presenta controles de la interfaz
        private void presentaControlesInterfaz(int n)
        {
            if (n >= 2)
            {
                labelIP2.Visible = true;
                textBoxIP2.Visible = true;
                labelidioma2.Visible = true;
                comboBoxIdioma2.Visible = true;
                labelDifusion2.Visible = true;
                comboBox2Difusion.Visible = true;
                buttonTestIP2.Visible = true;
                labelTestIPF2.Visible = true;
            }

            if (n >= 3)
            {
                labelIP3.Visible = true;
                textBoxIP3.Visible = true;
                labelidioma3.Visible = true;
                comboBoxIdioma3.Visible = true;
                labelDifusion3.Visible = true;
                comboBox3Difusion.Visible = true;
                buttonTestIP3.Visible = true;
                labelTestIPF3.Visible = true;
            }

            if (n >= 4)
            {
                labelIP4.Visible = true;
                textBoxIP4.Visible = true;
                labelidioma4.Visible = true;
                comboBoxIdioma4.Visible = true;
                labelDifusion4.Visible = true;
                comboBox4Difusion.Visible = true;
                buttonTestIP4.Visible = true;
                labelTestIPF4.Visible = true;
            }

            if (n >= 5)
            {
                labelIP5.Visible = true;
                textBoxIP5.Visible = true;
                labelidioma5.Visible = true;
                comboBoxIdioma5.Visible = true;
                labelDifusion5.Visible = true;
                comboBox5Difusion.Visible = true;
                buttonTestIP5.Visible = true;
                labelTestIPF5.Visible = true;
            }

            if (n >= 6)
            {
                labelIP6.Visible = true;
                textBoxIP6.Visible = true;
                labelidioma6.Visible = true;
                comboBoxIdioma6.Visible = true;
                labelDifusion6.Visible = true;
                comboBox6Difusion.Visible = true;
                buttonTestIP6.Visible = true;
                labelTestIPF6.Visible = true;
            }

            if (n >= 7)
            {
                labelIP7.Visible = true;
                textBoxIP7.Visible = true;
                labelidioma7.Visible = true;
                comboBoxIdioma7.Visible = true;
                labelDifusion7.Visible = true;
                comboBox7Difusion.Visible = true;
                buttonTestIP7.Visible = true;
                labelTestIPF7.Visible = true;
            }

            if (n >= 8)
            {
                labelIP8.Visible = true;
                textBoxIP8.Visible = true;
                labelidioma8.Visible = true;
                comboBoxIdioma8.Visible = true;
                labelDifusion8.Visible = true;
                comboBox8Difusion.Visible = true;
                buttonTestIP8.Visible = true;
                labelTestIPF8.Visible = true;
            }

            if (n >= 9)
            {
                labelIP9.Visible = true;
                textBoxIP9.Visible = true;
                labelidioma9.Visible = true;
                comboBoxIdioma9.Visible = true;
                labelDifusion9.Visible = true;
                comboBox9Difusion.Visible = true;
                buttonTestIP9.Visible = true;
                labelTestIPF9.Visible = true;
            }

            if (n >= 10)
            {
                labelIP10.Visible = true;
                textBoxIP10.Visible = true;
                labelidioma10.Visible = true;
                comboBoxIdioma10.Visible = true;
                labelDifusion10.Visible = true;
                comboBox10Difusion.Visible = true;
                buttonTestIP10.Visible = true;
                labelTestIPF10.Visible = true;
            }
        }

        private void buttonTestIP2_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF2, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP2.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF2, (ok ? OK : Error));
        }

        private void buttonTestIP3_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF3, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP3.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF3, (ok ? OK : Error));
        }

        private void buttonTestIP4_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF4, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP4.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF4, (ok ? OK : Error));
        }

        private void buttonTestIP5_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF5, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP5.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF5, (ok ? OK : Error));
        }

        private void buttonTestIP6_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF6, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP6.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF6, (ok ? OK : Error));
        }

        private void buttonTestIP7_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF7, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP7.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF7, (ok ? OK : Error));
        }

        private void buttonTestIP8_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF8, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP8.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF8, (ok ? OK : Error));
        }

        private void buttonTestIP9_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF9, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP9.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF9, (ok ? OK : Error));
        }

        private void buttonTestIP10_Click(object sender, EventArgs e)
        {
            configLabelTest(this.labelTestIPF10, Testing);

            InterfaceIPF ipf = new InterfaceIPF(this.textBoxIP10.Text);
            bool ok = ipf.Ping();

            configLabelTest(this.labelTestIPF10, (ok ? OK : Error));
        }



    }
}
