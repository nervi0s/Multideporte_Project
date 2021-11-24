using Balonmano_Manager_App.Persistencia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Balonmano_Manager_App.Interfaz
{
    public partial class EditEstadisticasForm : Form
    {
        private EncuentroData _eData;
        private Controlador _controlador;
        private bool manual = false;

        public EditEstadisticasForm(EncuentroData eData, Controlador _control)
        {
            InitializeComponent();
            this._eData = eData;
            this._controlador = _control;
        }

        private void EditEstadisticasForm_Load(object sender, EventArgs e)
        {
            //RELLENAMOS LOS CAMPOS CON LA INFORMACIÓN DEL ENCUENTRO
            CargaDatosAutomaticos();
        }

        private void CargaDatosAutomaticos()
        {
            //ESTABLECEMOS EL IDIOMA
            IdiomaData[] idioma = _controlador.getIdiomas();
            int n = _controlador.getNumIpfs();

            for (int i = 0; i < n; i++)
            {
                //TIROS TOTALES
                this.textBox_medio_1.Text = idioma[i].Tiros.ToString();

                this.textBox_local_1.Text = CadenaPorcentajes(_eData.EquipoL.getGoles(), _eData.EquipoL.getTiros());
                this.textBox_visitante_1.Text = CadenaPorcentajes(_eData.EquipoV.getGoles(), _eData.EquipoV.getTiros());

                //TIROS CAMPO
                this.textBox_medio_2.Text = idioma[i].Tiros_Campo.ToString();

                this.textBox_local_2.Text = CadenaPorcentajes(_eData.EquipoL.getGoles()- _eData.EquipoL.getGoles7M()- _eData.EquipoL.getGolesContraataque(), _eData.EquipoL.getTiros()- _eData.EquipoL.getTiros7M()- _eData.EquipoL.getTirosContraataque());
                this.textBox_visitante_2.Text = CadenaPorcentajes(_eData.EquipoV.getGoles()- _eData.EquipoV.getGoles7M() - _eData.EquipoV.getGolesContraataque(), _eData.EquipoV.getTiros()- _eData.EquipoV.getTiros7M()- _eData.EquipoV.getTirosContraataque());

                //TIROS 7 M
                this.textBox_medio_3.Text = idioma[i].Tiros_7M.ToString();

                this.textBox_local_3.Text = CadenaPorcentajes(_eData.EquipoL.getGoles7M(), _eData.EquipoL.getTiros7M());
                this.textBox_visitante_3.Text = CadenaPorcentajes(_eData.EquipoV.getGoles7M(), _eData.EquipoV.getTiros7M());

                //TIROS CONTRAATAQUE
                this.textBox_medio_4.Text = idioma[i].Tiros_Contraataque.ToString();

                this.textBox_local_4.Text = CadenaPorcentajes(_eData.EquipoL.getGolesContraataque(), _eData.EquipoL.getTirosContraataque());
                this.textBox_visitante_4.Text = CadenaPorcentajes(_eData.EquipoV.getGolesContraataque(), _eData.EquipoV.getTirosContraataque());

                //PARADAS
                this.textBox_medio_5.Text = idioma[i].GoalkeeperSaves.ToString();

                this.textBox_local_5.Text = _eData.EquipoL.getParadasTiro().ToString();
                this.textBox_visitante_5.Text = _eData.EquipoV.getParadasTiro().ToString();

                //PERDIDAS
                this.textBox_medio_6.Text = idioma[i].Perdidas.ToString();

                this.textBox_local_6.Text = _eData.EquipoL.getPerdidasTiro().ToString();
                this.textBox_visitante_6.Text = _eData.EquipoV.getPerdidasTiro().ToString();

                //EXCLUSIONES
                this.textBox_medio_7.Text = idioma[i].ExclusionsMins.ToString();

                this.textBox_local_7.Text = (_eData.EquipoL.getExclusiones_Totales()*2).ToString();
                this.textBox_visitante_7.Text = (_eData.EquipoV.getExclusiones_Totales()*2).ToString();

                //POSESION
                this.textBox_medio_8.Text = idioma[i].Attacks.ToString();

                this.textBox_local_8.Text = _eData.EquipoL.getAtaques().ToString();
                this.textBox_visitante_8.Text = _eData.EquipoV.getAtaques().ToString();

                ////POSESION
                //this.textBox_medio_8.Text = idioma[i].Possesion.ToString();

                //this.textBox_local_8.Text = _eData.Posesion.getPorcentajeLocal();
                //this.textBox_visitante_8.Text = _eData.Posesion.getPorcentajeVisitante();
            }
        }

        private string CadenaPorcentajes(int a, int b)
        {
            int porcento;
            //NO SE PUEDE DIVIDIR POR CERO
            if (b == 0)
            {
                porcento = 0;
            }
            else
            {
                porcento = 100 * a / b;
            }
            
            string s = a.ToString()+"/"+b.ToString()+"   "+porcento.ToString()+"%";
            return s;
        }

        private void EditEstadisticasForm_Activated(object sender, EventArgs e)
        {
            if (!manual)
                CargaDatosAutomaticos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (manual)
            {
                string message = "¿Está seguro que quiere cambiar a AUTOMÁTICO?";
                string caption = "Información";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    button1.BackColor = Color.Tomato;
                    button1.Text = "AUTOMÁTICO";

                    tabla_button.Visible = false;
                    linea1_button.Visible = false;
                    linea2_button.Visible = false;
                    linea3_button.Visible = false;
                    linea4_button.Visible = false;
                    linea5_button.Visible = false;
                    linea6_button.Visible = false;
                    linea7_button.Visible = false;
                    linea8_button.Visible = false;

                    manual = false;
                }
            }
            else
            {
                string message = "¿Está seguro que quiere cambiar a MANUAL?";
                string caption = "Información";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    button1.BackColor = Color.LightGreen;
                    button1.Text = "MANUAL";

                    tabla_button.Visible = true;
                    linea1_button.Visible = true;
                    linea2_button.Visible = true;
                    linea3_button.Visible = true;
                    linea4_button.Visible = true;
                    linea5_button.Visible = true;
                    linea6_button.Visible = true;
                    linea7_button.Visible = true;
                    linea8_button.Visible = true;

                    manual = true;
                }
            }
        }

        /* STATISTICS IN */
        private void linea1_button_Click(object sender, EventArgs e)
        {
            if (linea1_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_1.Text, textBox_local_1.Text, textBox_visitante_1.Text);

                linea1_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea1_button.BackColor = Color.DarkGray;
            }
        }

        private void linea2_button_Click(object sender, EventArgs e)
        {
            if (linea2_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_2.Text, textBox_local_2.Text, textBox_visitante_2.Text);

                linea2_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea2_button.BackColor = Color.DarkGray;
            }
        }

        private void linea3_button_Click(object sender, EventArgs e)
        {
            if (linea3_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_3.Text, textBox_local_3.Text, textBox_visitante_3.Text);

                linea3_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea3_button.BackColor = Color.DarkGray;
            }
        }

        private void linea4_button_Click(object sender, EventArgs e)
        {
            if (linea4_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_4.Text, textBox_local_4.Text, textBox_visitante_4.Text);

                linea4_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea4_button.BackColor = Color.DarkGray;
            }
        }

        private void linea5_button_Click(object sender, EventArgs e)
        {
            if (linea5_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_5.Text, textBox_local_5.Text, textBox_visitante_5.Text);

                linea5_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea5_button.BackColor = Color.DarkGray;
            }
        }

        private void linea6_button_Click(object sender, EventArgs e)
        {
            if (linea6_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_6.Text, textBox_local_6.Text, textBox_visitante_6.Text);

                linea6_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea6_button.BackColor = Color.DarkGray;
            }
        }

        private void linea7_button_Click(object sender, EventArgs e)
        {
            if (linea7_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_7.Text, textBox_local_7.Text, textBox_visitante_7.Text);

                linea7_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea7_button.BackColor = Color.DarkGray;
            }
        }

        private void linea8_button_Click(object sender, EventArgs e)
        {
            if (linea8_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inStaticsManual(textBox_medio_8.Text, textBox_local_8.Text, textBox_visitante_8.Text);

                linea8_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outStaticsManual();

                linea8_button.BackColor = Color.DarkGray;
            }
        }

        private void tabla_button_Click(object sender, EventArgs e)
        {
            if (tabla_button.BackColor == Color.DarkGray)
            {
                //MANDA IN STADISTIC
                _controlador.inTableManual(
                    textBox_medio_1.Text, textBox_local_1.Text, textBox_visitante_1.Text,
                    textBox_medio_2.Text, textBox_local_2.Text, textBox_visitante_2.Text,
                    textBox_medio_3.Text, textBox_local_3.Text, textBox_visitante_3.Text,
                    textBox_medio_4.Text, textBox_local_4.Text, textBox_visitante_4.Text,
                    textBox_medio_5.Text, textBox_local_5.Text, textBox_visitante_5.Text,
                    textBox_medio_6.Text, textBox_local_6.Text, textBox_visitante_6.Text,
                    textBox_medio_7.Text, textBox_local_7.Text, textBox_visitante_7.Text,
                    textBox_medio_8.Text, textBox_local_8.Text, textBox_visitante_8.Text);

                tabla_button.BackColor = Color.LightCoral;
            }
            else
            {
                //MANDA OUT STADISTIC
                _controlador.outTableManual();

                tabla_button.BackColor = Color.DarkGray;
            }
        }
    }
}
