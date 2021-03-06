using System;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Comandos;

namespace Futbol_Sala_Manager_App.Interfaz
{

    /**
     * Formulario de edición de una Plantilla de texto
     */
    public partial class PlantillaForm : Form
    {

        private FreeTextCommand _plantilla;

        /**
         * Constructor
         */
        public PlantillaForm()
        {
            InitializeComponent();

            this.radioButton2.Checked = true;
        }

        /**
         * Constructor
         */
        public PlantillaForm(FreeTextCommand plantilla)
        {
            InitializeComponent();

            if (!plantilla.Linea3.Equals(""))
            {
                this.radioButton3.Checked = true;
            }
            else if (!plantilla.Linea2.Equals(""))
            {
                this.radioButton2.Checked = true;
            }
            else
            {
                this.radioButton1.Checked = true;
            }

            this.linea1.Text = plantilla.Linea1;
            this.linea2.Text = plantilla.Linea2;
            this.linea3.Text = plantilla.Linea3;
        }


        /**
         * Recupera la plantilla representada en el formulario
         * En caso de que el usuario cancele la operación se devuelve 'null'.
         */
        public FreeTextCommand getPlantilla()
        {
            return _plantilla;
        }




        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _plantilla = new FreeTextCommand(this.numLineas(), linea1.Text, linea2.Text, linea3.Text, linea4.Text);
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _plantilla = null;
            this.Close();
        }

        private int numLineas()
        {
            if (this.radioButton1.Checked)
                return 1;
            else if (this.radioButton2.Checked)
                return 2;
            else if (this.radioButton3.Checked)
                return 3;
            else
                return 4;
        }


        private void configLineas(int lineas)
        {
            switch (lineas)
            {
                case 1:
                    linea2.Visible = false;
                    linea2.Text = "";
                    linea3.Visible = false;
                    linea3.Text = "";
                    linea4.Visible = false;
                    linea4.Text = "";
                    break;
                case 2:
                    linea2.Visible = true;
                    linea3.Visible = false;
                    linea4.Visible = false;
                    linea3.Text = "";
                    linea4.Text = "";
                    break;
                case 3:
                    linea2.Visible = true;
                    linea3.Visible = true;
                    linea4.Visible = false;
                    linea4.Text = "";
                    break;

                case 4:
                    linea1.Visible = true;
                    linea2.Visible = true;
                    linea3.Visible = true;
                    linea4.Visible = true;
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            configLineas(1);
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            configLineas(2);
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            configLineas(3);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            configLineas(4);
        }


    }
}
