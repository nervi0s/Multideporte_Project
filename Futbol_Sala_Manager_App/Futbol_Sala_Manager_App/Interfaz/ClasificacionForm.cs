using Futbol_Sala_Manager_App.Beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class ClasificacionForm : Form
    {
        public class EquipoClasificacion
        {
            public ComboBox equipo = new ComboBox();
            public TextBox pt = new TextBox();
            public TextBox pj = new TextBox();
            public TextBox pg = new TextBox();
            public TextBox pe = new TextBox();
            public TextBox pp = new TextBox();
            public TextBox gf = new TextBox();
            public TextBox gc = new TextBox();
        }

        private Clasificacion _clasificacion;
        private EquipoClasificacion[] _equipos;
        string[] teams;
        string[] escudos;

        public ClasificacionForm()
        {
            InitializeComponent();

            LoadDivisionCombo();
            GetTeamsInfo();
            InitializeTableLayout();
            //CheckText();
        }

        public ClasificacionForm(Clasificacion c)
        {
            InitializeComponent();

            LoadDivisionCombo();
            GetTeamsInfo();
            InitializeTableLayout();
            LoadData(c);
            //CheckText();
        }

        void InitializeTableLayout()
        {
            CleanControls();
            // Son 16 equipos los que hay que crear
            _equipos = new EquipoClasificacion[16];
            // La primera fila no cuenta que es la de los textos que indican que es cada columna
            // Creamos los equipos
            for (int i = 0; i < 16; ++i) // FILAS
            {
                _equipos[i] = new EquipoClasificacion();
                // Equipo
                _equipos[i].equipo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].equipo.Items.AddRange(teams);
                _equipos[i].equipo.SelectedItem = _equipos[i].equipo.Items[0];
                _equipos[i].equipo.DropDownStyle = ComboBoxStyle.DropDownList;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].equipo, 0, i + 1);
                // Puntos totales
                _equipos[i].pt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].pt.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].pt, 1, i + 1);
                // Partidos jugados
                _equipos[i].pj.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].pj.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].pj, 2, i + 1);
                // Partidos ganados
                _equipos[i].pg.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].pg.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].pg, 3, i + 1);
                // Partidos empatados
                _equipos[i].pe.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].pe.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].pe, 4, i + 1);
                // Partidos perdidos
                _equipos[i].pp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].pp.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].pp, 5, i + 1);
                // Goles a favor
                _equipos[i].gf.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].gf.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].gf, 6, i + 1);
                // Goles en contra
                _equipos[i].gc.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _equipos[i].gc.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_equipos.Controls.Add(_equipos[i].gc, 7, i + 1);
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

        void CleanControls()
        {
            if (_equipos != null)
                foreach (var e in _equipos)
                {
                    tableLayoutPanel_equipos.Controls.Remove(e.equipo);
                    tableLayoutPanel_equipos.Controls.Remove(e.pt);
                    tableLayoutPanel_equipos.Controls.Remove(e.pj);
                    tableLayoutPanel_equipos.Controls.Remove(e.pg);
                    tableLayoutPanel_equipos.Controls.Remove(e.pe);
                    tableLayoutPanel_equipos.Controls.Remove(e.pp);
                    tableLayoutPanel_equipos.Controls.Remove(e.gf);
                    tableLayoutPanel_equipos.Controls.Remove(e.gc);
                }
        }

        void GetTeamsInfo()
        {
            try
            {
                string division = comboBox_division.Text.Replace("ª", "").Replace(" ", "_");
                //Console.WriteLine(Path.GetDirectoryName(Application.ExecutablePath) + "/futbol_sala/Equipos_" + division + ".txt");
                string[] lines = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "/futbol_sala/Equipos_" + division + ".txt");
                List<string> equiposLista = new List<string>();
                List<string> escudosLista = new List<string>();
                foreach (var l in lines)
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

        //void CheckText()
        //{
        //    foreach(var e in _equipos)
        //    {
        //        if (string.IsNullOrEmpty(e.pt.Text)) e.pt.Text = "0";
        //        if (string.IsNullOrEmpty(e.pj.Text)) e.pj.Text = "0";
        //        if (string.IsNullOrEmpty(e.pg.Text)) e.pg.Text = "0";
        //        if (string.IsNullOrEmpty(e.pe.Text)) e.pe.Text = "0";
        //        if (string.IsNullOrEmpty(e.pp.Text)) e.pp.Text = "0";
        //        if (string.IsNullOrEmpty(e.gf.Text)) e.gf.Text = "0";
        //        if (string.IsNullOrEmpty(e.gc.Text)) e.gc.Text = "0";
        //    }
        //}

        void LoadData(Clasificacion c)
        {
            if (c == null || c.equipos == null || c.equipos.Length < 0)
                return;

            if (!string.IsNullOrEmpty(c.division))
                comboBox_division.Text = c.division;
            for(int i = 0; i < c.equipos.Length && i < _equipos.Length; ++i)
            {
                EquipoClasificacion ec = _equipos[i];
                Clasificacion.Equipo e = c.equipos[i];
                // Primero el equipo
                ec.equipo.SelectedItem = e.equipo;
                // Luego los datos del equipo
                ec.pt.Text = e.pt;
                ec.pj.Text = e.pj;
                ec.pg.Text = e.pg;
                ec.pe.Text = e.pe;
                ec.pp.Text = e.pp;
                ec.gf.Text = e.gf;
                ec.gc.Text = e.gc;
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _clasificacion = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            //CheckText();
            List<Clasificacion.Equipo> equiposLista = new List<Clasificacion.Equipo>();
            foreach(var ec in _equipos)
            {
                equiposLista.Add(new Clasificacion.Equipo
                {
                    equipo = ec.equipo.SelectedItem.ToString(),
                    pt = ec.pt.Text,
                    pj = ec.pj.Text,
                    pg = ec.pg.Text,
                    pe = ec.pe.Text,
                    pp = ec.pp.Text,
                    gf = ec.gf.Text,
                    gc = ec.gc.Text
                });
            }
            _clasificacion = new Clasificacion
            {
                division = comboBox_division.Text,
                equipos = equiposLista.ToArray()
            };

            this.Close();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTeamsInfo();
            InitializeTableLayout();
        }

        public Clasificacion GetClasificacion()
        {
            return _clasificacion;
        }
    }
}
