using Futbol_Sala_Manager_App.Beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class EnfrentamientosForm : Form
    {
        public class EnfrentamientoData
        {
            public ComboBox equipoL = new ComboBox();
            public TextBox info = new TextBox();
            public ComboBox equipoV = new ComboBox();
        }

        private Enfrentamientos _enfrentamientos;
        private EnfrentamientoData[] _eData;
        string[] teams;
        string[] escudos;

        public EnfrentamientosForm()
        {
            InitializeComponent();

            LoadDivisionCombo();
            GetTeamsInfo();
            InitializeTableLayout();
        }

        public EnfrentamientosForm(Enfrentamientos e)
        {
            InitializeComponent();

            LoadDivisionCombo();
            GetTeamsInfo();
            InitializeTableLayout();
            LoadData(e);
        }

        void InitializeTableLayout()
        {
            CleanControls();

            // Son 16 equipos los que hay que crear
            _eData = new EnfrentamientoData[8];
            // Cogemos los equipos del fichero

            // La primera fila no cuenta que es la de los textos que indican que es cada columna
            // Creamos los equipos
            for (int i = 0; i < 8; ++i) // FILAS
            {
                _eData[i] = new EnfrentamientoData();

                // Equipo Local
                _eData[i].equipoL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _eData[i].equipoL.Items.AddRange(teams);
                _eData[i].equipoL.SelectedItem = _eData[i].equipoL.Items[0];
                _eData[i].equipoL.DropDownStyle = ComboBoxStyle.DropDownList;
                tableLayoutPanel_enfrentamientos.Controls.Add(_eData[i].equipoL, 0, i + 1);
                
                // Informacion
                _eData[i].info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _eData[i].info.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_enfrentamientos.Controls.Add(_eData[i].info, 1, i + 1);
                
                // Equipo Visitante
                _eData[i].equipoV.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _eData[i].equipoV.Items.AddRange(teams);
                _eData[i].equipoV.SelectedItem = _eData[i].equipoV.Items[0];
                _eData[i].equipoV.DropDownStyle = ComboBoxStyle.DropDownList;
                tableLayoutPanel_enfrentamientos.Controls.Add(_eData[i].equipoV, 2, i + 1);
            }
        }

        void CleanControls()
        {
            if (_eData != null)
                foreach (var t in _eData)
                {
                    tableLayoutPanel_enfrentamientos.Controls.Remove(t.equipoL);
                    tableLayoutPanel_enfrentamientos.Controls.Remove(t.info);
                    tableLayoutPanel_enfrentamientos.Controls.Remove(t.equipoV);
                }
        }

        void LoadDivisionCombo()
        {
            try
            {
                comboBox_division.Items.Clear();
                comboBox_division.Items.AddRange(new string[] { "1 Division", "2 Division" });
                comboBox_division.SelectedItem = comboBox_division.Items[0];
                comboBox_division.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }

        void GetTeamsInfo()
        {
            try
            {
                string division = comboBox_division.Text.Replace("ª", "").Replace(" ", "_");
                //Console.WriteLine(division);
                string[] lines = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "/futbol_sala/Equipos_" + division + ".txt");
                List<string> equiposLista = new List<string>();
                List<string> escudosLista = new List<string>();
                foreach(var l in lines)
                {
                    string[] s = l.Split(';');
                    if (s.Length > 0)
                        equiposLista.Add(s[0]);
                    if (s.Length > 1)
                        escudosLista.Add(s[1]);
                }
                teams = equiposLista.ToArray();
                escudos = escudosLista.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }

        void LoadData(Enfrentamientos e)
        {
            if (e == null || e.partidos == null || e.partidos.Length < 0)
                return;

            textBox_titulo.Text = e.titulo;
            if(!string.IsNullOrEmpty(e.division))
                comboBox_division.Text = e.division;
            for (int i = 0; i < e.partidos.Length && i < _eData.Length; ++i)
            {
                EnfrentamientoData ed = _eData[i];
                Enfrentamientos.Partido p = e.partidos[i];
                // Equipo Local
                ed.equipoL.SelectedItem = p.equipoL;
                // Informacion
                ed.info.Text = p.info;
                // Equipo Visitante
                ed.equipoV.SelectedItem = p.equipoV;
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _enfrentamientos = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            List<Enfrentamientos.Partido> partidosLista = new List<Enfrentamientos.Partido>();
            foreach (var ec in _eData)
            {
                partidosLista.Add(new Enfrentamientos.Partido
                {
                    equipoL = ec.equipoL.Text,
                    escudoL = getEscudo(ec.equipoL.Text),
                    info = ec.info.Text,
                    equipoV = ec.equipoV.Text,
                    escudoV = getEscudo(ec.equipoV.Text)
                });
            }
            _enfrentamientos = new Enfrentamientos
            {
                titulo = textBox_titulo.Text,
                division = comboBox_division.Text,
                partidos = partidosLista.ToArray()
            };

            this.Close();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTeamsInfo();
            InitializeTableLayout();
        }

        string getEscudo(string equipo)
        {
            for(int i = 0; i < teams.Length; ++i)
            {
                if(teams[i].Equals(equipo))
                {
                    return escudos[i];
                }
            }
            return "";
        }

        public Enfrentamientos GetEnfrentamientos()
        {
            return _enfrentamientos;
        }
    }
}
