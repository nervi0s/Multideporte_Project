using System;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{

    /**
     * Formulario de edición de un Momento
     */
    public partial class TiempoForm : Form
    {
        private int _parte;
        private Momento _momento;
        private bool _isCrono;
        private string defaultMinsParte;
        private string defaultMinsProrroga;

        /**
         * Constructor
         */
        public TiempoForm(Momento momento, string defaultMinsParte, string defaultMinsProrroga, bool isCrono = false)
        {
            InitializeComponent();
            _parte = Momento.IniParte1;
            _isCrono = isCrono;

            this.defaultMinsParte = defaultMinsParte;
            this.defaultMinsProrroga = defaultMinsProrroga;

            _momento = momento;
            cargaMomento(momento);
        }

        /**
         * Recupera el Momento representado en el formulario
         * En caso de que el usuario cancele la operación se devuelve 'null'.
         */
        public Momento getMomento()
        {
            return _momento;
        }


        // ============================== PRIVADOS ======================================
        
        private void cargaMomento(Momento momento)
        {
            switch (momento.Parte)
            {
                case Momento.IniParte2:
                    radioButton2.Checked = true;
                    break;
                case Momento.IniProrroga1:
                    radioButton3.Checked = true;
                    break;
                case Momento.IniProrroga2:
                    radioButton4.Checked = true;
                    break;
                case Momento.Penaltis:
                    radioButton5.Checked = true;
                    break;
                default:
                    radioButton1.Checked = true;
                    break;
            }

            ////Console.WriteLine("Tiempo Crono: " + momento.GetMinutoCrono());
            ////Console.WriteLine("Tiempo Crono: " + momento.GetMinuto());
            //this.segundos.Text = _isCrono ?
            //                     (60 - momento.GetSegundo()).ToString() :
            //                     momento.GetSegundo().ToString();
            //this.minutos.Text = _isCrono ?
            //                    (Momento.Minuto[_parte] - momento.GetMinutoCrono()).ToString() :
            //                    momento.GetMinuto().ToString();

            if (!_isCrono)
            {
                this.segundos.Text = momento.GetSegundo().ToString();
                this.minutos.Text = momento.GetMinuto().ToString();
            }
            else
            {
                if (momento.Parte >= Momento.IniParte1 && momento.Parte <= Momento.FinParte2)
                {
                    this.minutos.Text = this.defaultMinsParte;
                    this.segundos.Text = "00";
                }
                else if (momento.Parte >= Momento.IniProrroga1 && momento.Parte <= Momento.FinProrroga2)
                {
                    this.minutos.Text = this.defaultMinsProrroga;
                    this.segundos.Text = "00";
                }
                else
                {
                    this.minutos.Text = "00";
                    this.segundos.Text = "00";
                }
            }
        }

        private void addDigito(int d)
        {
            int digitos = getDigitos();

            if (digitos < 10000)
            {
                digitos *= 10;
                digitos += d;

                updateDigitos(digitos);
            }
        }
        private void delDigito()
        {
            int digitos = getDigitos();

            if (digitos > 9)
            {
                digitos /= 10;
            }
            else
            {
                digitos = 0;
            }
            updateDigitos(digitos);
        }
        private void updateDigitos(int digitos)
        {
            int m = digitos / 100;
            int s = digitos - m * 100;

            segundos.Text = s.ToString();
            minutos.Text = m.ToString();
        }
        private int getDigitos()
        {
            return getMinutos() * 100 + getSegundos();
        }

        private int getMinutos()
        {
            try
            {
                return int.Parse(this.minutos.Text);
            }
            catch
            {
                return 0;
            }
        }
        private int getSegundos()
        {
            try
            {
                return int.Parse(this.segundos.Text);
            }
            catch
            {
                return 0;
            }
        }

        public Button getBotonAceptar()
        {
            return buttonAceptar;
        }

        // ============================== EVENTOS ======================================
        private void buttonAceptar_Click(object sender, EventArgs e)
        {

            //Console.WriteLine(getMinutos());
            //Console.WriteLine(getSegundos());
            int minutos = getMinutos();
            if (!_isCrono)
            {
                // Comprobación del tiempo según la parte que se tiene seleccionada
                // Esto queda un poco chapuza ponerlo aquí, pero funciona.
                int minTiempoParte = Momento.MinutoAdd[_parte];
                //Console.WriteLine("Min: " + minTiempoParte);
                int maxTiempoParte = Momento.MinutoAdd[_parte + 2];
                //Console.WriteLine("Max: " + maxTiempoParte);
                //Console.WriteLine("Minutos: " + minutos);
                if (minutos > maxTiempoParte)
                    minutos = maxTiempoParte;
                else if (minutos < minTiempoParte)
                    minutos = minTiempoParte;
                minutos -= minTiempoParte;
                //Console.WriteLine("Minutos Changed: " + minutos);
            }

            _momento.Update(_parte, minutos, getSegundos());
            this.Close();
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _momento = null;
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _parte = Momento.IniParte1;
            this.minutos.Text = this.defaultMinsParte;
            this.segundos.Text = "00";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _parte = Momento.IniParte2;
            this.minutos.Text = this.defaultMinsParte;
            this.segundos.Text = "00";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _parte = Momento.IniProrroga1;
            this.minutos.Text = this.defaultMinsProrroga;
            this.segundos.Text = "00";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            _parte = Momento.IniProrroga2;
            this.minutos.Text = this.defaultMinsProrroga;
            this.segundos.Text = "00";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            _parte = Momento.Penaltis;
            this.minutos.Text = "00";
            this.segundos.Text = "00";
        }


        private void button0_Click(object sender, EventArgs e)
        {
            addDigito(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addDigito(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addDigito(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addDigito(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addDigito(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            addDigito(5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            addDigito(6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            addDigito(7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            addDigito(8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            addDigito(9);
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            delDigito();
        }


    }
}
