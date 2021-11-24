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
    public partial class GolForm : Form
    {
        private int tipo_gol;

        //CONSTANTES TIPO GOL
        private const int Gol_Normal = 0;
        private const int Gol_7_M = 1;
        private const int Gol_Contraataque = 2;

        //MOMENTO
        private Momento _momento;
        private bool _form_cargado = false;

        public GolForm(int tipo, Momento momento)
        {
            InitializeComponent();

            tipo_gol = tipo;
            _momento = momento;
        }

        private void GolForm_Load(object sender, EventArgs e)
        {
            switch (tipo_gol)
            {
                case Gol_Normal:

                    GolNormalButton.Checked = true;
                    Gol7MButton.Checked = false;
                    GolContraataqueButton.Checked = false;

                    break;

                case Gol_7_M:

                    GolNormalButton.Checked = false;
                    Gol7MButton.Checked = true;
                    GolContraataqueButton.Checked = false;

                    break;

                case Gol_Contraataque:

                    GolNormalButton.Checked = false;
                    Gol7MButton.Checked = false;
                    GolContraataqueButton.Checked = true;

                    break;

                default:
                    break;

            }

            cargaMomento(_momento);
            _form_cargado = true;
        }

        private void cargaMomento(Momento momento)
        {
            switch (momento.Parte)
            {
                //PARA CUANDO SE EDITE LA PARTE DEL TIEMPO
            }

            Console.WriteLine("min: " + momento.GetMinuto() + " sec: " + momento.GetSegundo());
            this.numericUpDown_seconds.Value = momento.GetSegundo();
            this.numericUpDown_minutes.Value = momento.GetMinuto();
        }

        public int getMomento()
        {
            return _momento.SegundoAbsoluto;
        }

        public int getNuevoTipo()
        {
            return tipo_gol;
        }

        private void GolNormalButton_CheckedChanged(object sender, EventArgs e)
        {
            if (GolNormalButton.Checked)
            {
                tipo_gol = Gol_Normal;

                Gol7MButton.Checked = false;
                GolContraataqueButton.Checked = false;
            }
            else
            {
                if(!Gol7MButton.Checked && !GolContraataqueButton.Checked)
                {
                    GolNormalButton.Checked = true;
                }
            }
        }

        private void Gol7MButton_CheckedChanged(object sender, EventArgs e)
        {
            if (Gol7MButton.Checked)
            {
                tipo_gol = Gol_7_M;

                GolNormalButton.Checked = false;
                GolContraataqueButton.Checked = false;
            }
            else
            {
                if (!GolNormalButton.Checked && !GolContraataqueButton.Checked)
                {
                    Gol7MButton.Checked = true;
                }
            }
        }

        private void GolContraataqueButton_CheckedChanged(object sender, EventArgs e)
        {
            if (GolContraataqueButton.Checked)
            {
                tipo_gol = Gol_Contraataque;

                GolNormalButton.Checked = false;
                Gol7MButton.Checked = false;
            }
            else
            {
                if (!GolNormalButton.Checked && !Gol7MButton.Checked)
                {
                    GolContraataqueButton.Checked = true;
                }
            }
        }

        private void numericUpDown_seconds_ValueChanged(object sender, EventArgs e)
        {
            if (_form_cargado)
                _momento.SegundoAbsoluto = (Int16.Parse(this.numericUpDown_minutes.Value.ToString())*60)+ Int16.Parse(this.numericUpDown_seconds.Value.ToString());
        }
    }
}
