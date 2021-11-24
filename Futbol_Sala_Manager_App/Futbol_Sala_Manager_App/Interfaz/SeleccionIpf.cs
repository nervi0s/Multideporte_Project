using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class SeleccionIpf : Form
    {

        ConfigData configActual = PersistenciaUtil.CargaConfig();

        public SeleccionIpf(int numIpf)
        {
            InitializeComponent();

            cargaComboIdioma();
            CargaConfiguracionIpf();
            ConfiguracionInterfaz(numIpf);
        }

        private void buttonConectar_Click(object sender, EventArgs e)
        {
            GuardarConfiguracion();
            this.Close();
        }

        private void GuardarConfiguracion()
        {
            for (int i = 0; i < 10; i++)
            {
                switch (i)
                {
                    case 0: Program.IpfsSeleccionados[i] = checkBox1.Checked; break;
                    case 1: Program.IpfsSeleccionados[i] = checkBox2.Checked; break;
                    case 2: Program.IpfsSeleccionados[i] = checkBox3.Checked; break;
                    case 3: Program.IpfsSeleccionados[i] = checkBox4.Checked; break;
                    case 4: Program.IpfsSeleccionados[i] = checkBox5.Checked; break;
                    case 5: Program.IpfsSeleccionados[i] = checkBox6.Checked; break;
                    case 6: Program.IpfsSeleccionados[i] = checkBox7.Checked; break;
                    case 7: Program.IpfsSeleccionados[i] = checkBox8.Checked; break;
                    case 8: Program.IpfsSeleccionados[i] = checkBox9.Checked; break;
                    case 9: Program.IpfsSeleccionados[i] = checkBox10.Checked; break;
                    default: break;
                }
            }
        }

        private void CargaConfiguracionIpf()
        {
            // IPF 1
            this.textBoxIP.Text = configActual.IpfIp;
            if (configActual.Multicast)
                this.comboBox1Difusion.Text = "Multilateral";
            else
                this.comboBox1Difusion.Text = "unilateral";
            this.comboBoxIdioma.Text = configActual.IdiomaFichero;

            // IPF 2
            this.textBoxIP2.Text = configActual.IpfIp2;
            if (configActual.Multicast2)
                this.comboBox2Difusion.Text = "Multilateral";
            else
                this.comboBox2Difusion.Text = "unilateral";
            this.comboBoxIdioma2.Text = configActual.IdiomaFichero2;
            
            // IPF 3
            this.textBoxIP3.Text = configActual.IpfIp3;
            if (configActual.Multicast3)
                this.comboBox3Difusion.Text = "Multilateral";
            else
                this.comboBox3Difusion.Text = "unilateral";
            this.comboBoxIdioma3.Text = configActual.IdiomaFichero3;

            // IPF 4
            this.textBoxIP4.Text = configActual.IpfIp4;
            if (configActual.Multicast4)
                this.comboBox4Difusion.Text = "Multilateral";
            else
                this.comboBox4Difusion.Text = "unilateral";
            this.comboBoxIdioma4.Text = configActual.IdiomaFichero4;

            // IPF 5
            this.textBoxIP5.Text = configActual.IpfIp5;
            if (configActual.Multicast5)
                this.comboBox5Difusion.Text = "Multilateral";
            else
                this.comboBox5Difusion.Text = "unilateral";
            this.comboBoxIdioma5.Text = configActual.IdiomaFichero5;

            // IPF 6
            this.textBoxIP6.Text = configActual.IpfIp6;
            if (configActual.Multicast6)
                this.comboBox6Difusion.Text = "Multilateral";
            else
                this.comboBox6Difusion.Text = "unilateral";
            this.comboBoxIdioma6.Text = configActual.IdiomaFichero6;

            // IPF 7
            this.textBoxIP7.Text = configActual.IpfIp7;
            if (configActual.Multicast7)
                this.comboBox7Difusion.Text = "Multilateral";
            else
                this.comboBox7Difusion.Text = "unilateral";
            this.comboBoxIdioma7.Text = configActual.IdiomaFichero7;

            // IPF 8
            this.textBoxIP8.Text = configActual.IpfIp8;
            if (configActual.Multicast8)
                this.comboBox8Difusion.Text = "Multilateral";
            else
                this.comboBox8Difusion.Text = "unilateral";
            this.comboBoxIdioma8.Text = configActual.IdiomaFichero8;

            // IPF 9
            this.textBoxIP9.Text = configActual.IpfIp9;
            if (configActual.Multicast9)
                this.comboBox9Difusion.Text = "Multilateral";
            else
                this.comboBox9Difusion.Text = "unilateral";

            // IPF 10
            this.textBoxIP10.Text = configActual.IpfIp10;
            if (configActual.Multicast10)
                this.comboBox10Difusion.Text = "Multilateral";
            else
                this.comboBox10Difusion.Text = "unilateral";
            this.comboBoxIdioma10.Text = configActual.IdiomaFichero10;

            checkBox1.Checked = Program.IpfsSeleccionados[0];
            checkBox2.Checked = Program.IpfsSeleccionados[1];
            checkBox3.Checked = Program.IpfsSeleccionados[2];
            checkBox4.Checked = Program.IpfsSeleccionados[3];
            checkBox5.Checked = Program.IpfsSeleccionados[4];
            checkBox6.Checked = Program.IpfsSeleccionados[5];
            checkBox7.Checked = Program.IpfsSeleccionados[6];
            checkBox8.Checked = Program.IpfsSeleccionados[7];
            checkBox9.Checked = Program.IpfsSeleccionados[8];
            checkBox10.Checked = Program.IpfsSeleccionados[9];
        }

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

        private void ConfiguracionInterfaz(int numIpf)
        {
            switch (numIpf)
            {
                case 1:
                    {
                        this.Size = new System.Drawing.Size(880, 200);
                        this.buttonConectar.Location = new Point(295, 110);
                        this.buttonCancelar.Location = new Point(455, 110);
                        break;
                    }

                case 2:
                    {
                        this.Size = new System.Drawing.Size(880, 230);
                        this.buttonConectar.Location = new Point(295, 140);
                        this.buttonCancelar.Location = new Point(455, 140);
                        MostrarIpf2();
                        break;
                    }

                case 3:
                    {
                        this.Size = new System.Drawing.Size(880, 260);
                        this.buttonConectar.Location = new Point(295, 170);
                        this.buttonCancelar.Location = new Point(455, 170);
                        MostrarIpf2(); MostrarIpf3();

                        break;
                    }

                case 4:
                    {
                        this.Size = new System.Drawing.Size(880, 290);
                        this.buttonConectar.Location = new Point(295, 200);
                        this.buttonCancelar.Location = new Point(455, 200);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4();

                        break;
                    }

                case 5:
                    {
                        this.Size = new System.Drawing.Size(880, 320);
                        this.buttonConectar.Location = new Point(295, 230);
                        this.buttonCancelar.Location = new Point(455, 230);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4(); MostrarIpf5();

                        break;
                    }

                case 6:
                    {
                        this.Size = new System.Drawing.Size(880, 350);
                        this.buttonConectar.Location = new Point(295, 260);
                        this.buttonCancelar.Location = new Point(455, 260);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4(); MostrarIpf5(); MostrarIpf6();

                        break;
                    }

                case 7:
                    {
                        this.Size = new System.Drawing.Size(880, 380);
                        this.buttonConectar.Location = new Point(295, 290);
                        this.buttonCancelar.Location = new Point(455, 290);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4(); MostrarIpf5(); MostrarIpf6(); MostrarIpf7();

                        break;
                    }

                case 8:
                    {
                        this.Size = new System.Drawing.Size(880, 410);
                        this.buttonConectar.Location = new Point(295, 320);
                        this.buttonCancelar.Location = new Point(455, 320);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4(); MostrarIpf5(); MostrarIpf6(); MostrarIpf7(); MostrarIpf8();

                        break;
                    }

                case 9:
                    {
                        this.Size = new System.Drawing.Size(880, 440);
                        this.buttonConectar.Location = new Point(295, 350);
                        this.buttonCancelar.Location = new Point(455, 350);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4(); MostrarIpf5(); MostrarIpf6(); MostrarIpf7(); MostrarIpf8(); MostrarIpf9();

                        break;
                    }

                case 10:
                    {
                        this.Size = new System.Drawing.Size(880, 470);
                        this.buttonConectar.Location = new Point(295, 380);
                        this.buttonCancelar.Location = new Point(455, 380);
                        MostrarIpf2(); MostrarIpf3(); MostrarIpf4(); MostrarIpf5(); MostrarIpf6(); MostrarIpf7(); MostrarIpf8(); MostrarIpf9(); MostrarIpf10();

                        break;
                    }

                default: break;
            }
        }

        private void MostrarIpf2()
        {
            checkBox2.Visible = true;
            labelIP2.Visible = true;
            textBoxIP2.Visible = true;
            labelidioma2.Visible = true;
            comboBoxIdioma2.Visible = true;
            labelDifusion2.Visible = true;
            comboBox2Difusion.Visible = true;
        }

        private void MostrarIpf3()
        {
            checkBox3.Visible = true;
            labelIP3.Visible = true;
            textBoxIP3.Visible = true;
            labelidioma3.Visible = true;
            comboBoxIdioma3.Visible = true;
            labelDifusion3.Visible = true;
            comboBox3Difusion.Visible = true;
        }

        private void MostrarIpf4()
        {
            checkBox4.Visible = true;
            labelIP4.Visible = true;
            textBoxIP4.Visible = true;
            labelidioma4.Visible = true;
            comboBoxIdioma4.Visible = true;
            labelDifusion4.Visible = true;
            comboBox4Difusion.Visible = true;
        }

        private void MostrarIpf5()
        {
            checkBox5.Visible = true;
            labelIP5.Visible = true;
            textBoxIP5.Visible = true;
            labelidioma5.Visible = true;
            comboBoxIdioma5.Visible = true;
            labelDifusion5.Visible = true;
            comboBox5Difusion.Visible = true;
        }

        private void MostrarIpf6()
        {
            checkBox6.Visible = true;
            labelIP6.Visible = true;
            textBoxIP6.Visible = true;
            labelidioma6.Visible = true;
            comboBoxIdioma6.Visible = true;
            labelDifusion6.Visible = true;
            comboBox6Difusion.Visible = true;
        }

        private void MostrarIpf7()
        {
            checkBox7.Visible = true;
            labelIP7.Visible = true;
            textBoxIP7.Visible = true;
            labelidioma7.Visible = true;
            comboBoxIdioma7.Visible = true;
            labelDifusion7.Visible = true;
            comboBox7Difusion.Visible = true;
        }

        private void MostrarIpf8()
        {
            checkBox8.Visible = true;
            labelIP8.Visible = true;
            textBoxIP8.Visible = true;
            labelidioma8.Visible = true;
            comboBoxIdioma8.Visible = true;
            labelDifusion8.Visible = true;
            comboBox8Difusion.Visible = true;
        }

        private void MostrarIpf9()
        {
            checkBox9.Visible = true;
            labelIP9.Visible = true;
            textBoxIP9.Visible = true;
            labelidioma9.Visible = true;
            comboBoxIdioma9.Visible = true;
            labelDifusion9.Visible = true;
            comboBox9Difusion.Visible = true;
        }

        private void MostrarIpf10()
        {
            checkBox10.Visible = true;
            labelIP10.Visible = true;
            textBoxIP10.Visible = true;
            labelidioma10.Visible = true;
            comboBoxIdioma10.Visible = true;
            labelDifusion10.Visible = true;
            comboBox10Difusion.Visible = true;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            Program.CancelarConexion = true;
            this.Close();
        }
            
    }
}
