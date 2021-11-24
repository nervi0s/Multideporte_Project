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
    public partial class Console_Conexions_Branch : Form
    {
        private bool escuchar_crono = true;
        private bool escuchar_exclusion = true;
        private bool escuchar_goles = false;
        private bool escuchar_timeOut = false;
        private bool escuchar_marcador = false;
        private bool escuchar_dorsales = false;

        MainForm _gui;

        public Console_Conexions_Branch(MainForm reference, bool crono, bool exclusion, bool goles, bool timeOut, bool marcador, bool dorsales)
        {
            escuchar_crono = crono;
            escuchar_exclusion = exclusion;
            escuchar_goles = goles;
            escuchar_timeOut = timeOut;
            escuchar_marcador = marcador;
            escuchar_dorsales = dorsales;

            _gui = reference;

            InitializeComponent();
        }

        private void Console_Conexions_Branch_Load(object sender, EventArgs e)
        {
            //COLORES

            if (escuchar_crono)
            {
                button_Crono.BackColor = Color.LawnGreen;
            }
            else
            {
                button_Crono.BackColor = Color.Tomato;
            }

            if (escuchar_exclusion)
            {
                button_Exclusion.BackColor = Color.LawnGreen;
            }
            else
            {
                button_Exclusion.BackColor = Color.Tomato;
            }

            if (escuchar_goles)
            {
                button_Goles.BackColor = Color.LawnGreen;
            }
            else
            {
                button_Goles.BackColor = Color.Tomato;
            }

            if (escuchar_timeOut)
            {
                button_TimeOut.BackColor = Color.LawnGreen;
            }
            else
            {
                button_TimeOut.BackColor = Color.Tomato;
            }

            if (escuchar_marcador)
            {
                button_Marcador.BackColor = Color.LawnGreen;
            }
            else
            {
                button_Marcador.BackColor = Color.Tomato;
            }

            if (escuchar_dorsales)
            {
                button_Dorsales.BackColor = Color.LawnGreen;
            }
            else
            {
                button_Dorsales.BackColor = Color.Tomato;
            }
        }


        /***    CLICK BUTTON    ***/

        private void button_Crono_Click(object sender, EventArgs e)
        {
            if (escuchar_crono)
            {
                escuchar_crono = false;
                button_Crono.BackColor = Color.Tomato;

                _gui.cambia_escuchar_crono(escuchar_crono);
            }
            else
            {
                escuchar_crono = true;
                button_Crono.BackColor = Color.LawnGreen;

                _gui.cambia_escuchar_crono(escuchar_crono);
            }
        }

        private void button_Exclusion_Click(object sender, EventArgs e)
        {
            if (escuchar_exclusion)
            {
                escuchar_exclusion = false;
                button_Exclusion.BackColor = Color.Tomato;

                _gui.cambia_escuchar_exclusion(escuchar_exclusion);
            }
            else
            {
                escuchar_exclusion = true;
                button_Exclusion.BackColor = Color.LawnGreen;

                _gui.cambia_escuchar_exclusion(escuchar_exclusion);
            }
        }

        private void button_Goles_Click(object sender, EventArgs e)
        {
            if (escuchar_goles)
            {
                escuchar_goles = false;
                button_Goles.BackColor = Color.Tomato;

                _gui.cambia_escuchar_goles(escuchar_goles);
            }
            else
            {
                escuchar_goles = true;
                button_Goles.BackColor = Color.LawnGreen;

                _gui.cambia_escuchar_goles(escuchar_goles);
            }
        }

        private void button_TimeOut_Click(object sender, EventArgs e)
        {
            if (escuchar_timeOut)
            {
                escuchar_timeOut = false;
                button_TimeOut.BackColor = Color.Tomato;
            }
            else
            {
                escuchar_timeOut = true;
                button_TimeOut.BackColor = Color.LawnGreen;
            }
        }

        private void button_Marcador_Click(object sender, EventArgs e)
        {
            if (escuchar_marcador)
            {
                escuchar_marcador = false;
                button_Marcador.BackColor = Color.Tomato;
            }
            else
            {
                escuchar_marcador = true;
                button_Marcador.BackColor = Color.LawnGreen;
            }
        }

        private void button_Dorsales_Click(object sender, EventArgs e)
        {
            if (escuchar_dorsales)
            {
                escuchar_dorsales = false;
                button_Dorsales.BackColor = Color.Tomato;
            }
            else
            {
                escuchar_dorsales = true;
                button_Dorsales.BackColor = Color.LawnGreen;
            }
        }
    }
}
