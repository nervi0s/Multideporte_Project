using Futbol_Sala_Manager_App.Beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class RachasForm : Form
    {
        public class PartidosData
        {
            public TextBox info = new TextBox();
            public ComboBox equipoL = new ComboBox();
            public TextBox puntosL = new TextBox();
            public TextBox puntosV = new TextBox();
            public ComboBox equipoV = new ComboBox();
            public TextBox puntosInfo = new TextBox();
        }

        private Rachas _rachas;
        private PartidosData[] _pData;
        string[] teams;
        string[] escudos;

        public RachasForm()
        {
            InitializeComponent();
            GetTeamsInfo();
            InitializeTableLayout();
            InitializeComboTeam();
        }

        public RachasForm(Rachas r)
        {
            InitializeComponent();
            GetTeamsInfo();
            InitializeTableLayout();
            InitializeComboTeam();
            LoadData(r);
        }

        void InitializeTableLayout()
        {
            // Son 16 equipos los que hay que crear
            _pData = new PartidosData[4];
            // Cogemos los equipos del fichero

            // La primera fila no cuenta que es la de los textos que indican que es cada columna
            // Creamos los equipos
            for (int i = 0; i < 4; ++i) // FILAS
            {
                _pData[i] = new PartidosData();
                // Info
                _pData[i].info.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                tableLayoutPanel_rachas.Controls.Add(_pData[i].info, 0, i + 1);
                // Equipo Local
                _pData[i].equipoL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _pData[i].equipoL.Items.AddRange(teams);
                _pData[i].equipoL.SelectedItem = _pData[i].equipoL.Items[0];
                _pData[i].equipoL.DropDownStyle = ComboBoxStyle.DropDownList;
                tableLayoutPanel_rachas.Controls.Add(_pData[i].equipoL, 1, i + 1);
                // Puntos Local
                _pData[i].puntosL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                tableLayoutPanel_rachas.Controls.Add(_pData[i].puntosL, 2, i + 1);
                // Puntos Visitante
                _pData[i].puntosV.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                tableLayoutPanel_rachas.Controls.Add(_pData[i].puntosV, 3, i + 1);
                // Equipo Visitante
                _pData[i].equipoV.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _pData[i].equipoV.Items.AddRange(teams);
                _pData[i].equipoV.SelectedItem = _pData[i].equipoV.Items[0];
                _pData[i].equipoV.DropDownStyle = ComboBoxStyle.DropDownList;
                tableLayoutPanel_rachas.Controls.Add(_pData[i].equipoV, 4, i + 1);
                // Puntos Info
                _pData[i].puntosInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                tableLayoutPanel_rachas.Controls.Add(_pData[i].puntosInfo, 5, i + 1);
            }
        }

        void InitializeComboTeam()
        {
            comboBox_team.Items.AddRange(teams);
            comboBox_team.SelectedItem = comboBox_team.Items[0];
        }

        void GetTeamsInfo()
        {
            try
            {
                //Console.WriteLine(division);
                string[] lines = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "/" + "Equipos_1_Division" + ".txt");
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

        void LoadData(Rachas r)
        {
            if (r == null || r.partidos == null || r.partidos.Length < 0)
                return;
            
            if (!string.IsNullOrEmpty(r.equipo))
                comboBox_team.Text = r.equipo;
            for (int i = 0; i < r.partidos.Length && i < _pData.Length; ++i)
            {
                PartidosData pd = _pData[i];
                Rachas.Partido md = r.partidos[i];
                // Informacion
                pd.info.Text = md.info;
                // Equipo Local
                pd.equipoL.SelectedItem = md.equipoLocal;
                // Puntos Local
                pd.puntosL.Text = md.puntosLocal;
                // Puntos Visitante
                pd.puntosV.Text = md.puntosVisitante;
                // Equipo Visitante
                pd.equipoV.SelectedItem = md.equipoVisitante;
                // Puntos Info
                pd.puntosInfo.Text = md.puntosInfo;
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _rachas = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            List<Rachas.Partido> partidosLista = new List<Rachas.Partido>();
            foreach (var ec in _pData)
            {
                partidosLista.Add(new Rachas.Partido
                {
                    info = ec.info.Text,
                    equipoLocal = ec.equipoL.Text,
                    puntosLocal = ec.puntosL.Text,
                    puntosVisitante = ec.puntosV.Text,
                    equipoVisitante = ec.equipoV.Text,
                    puntosInfo = ec.puntosInfo.Text
                });
            }
            _rachas = new Rachas
            {
                equipo = comboBox_team.Text,
                photoPath = getEscudo(comboBox_team.Text),
                partidos = partidosLista.ToArray()
            };

            this.Close();
        }

        string getEscudo(string equipo)
        {
            for (int i = 0; i < teams.Length; ++i)
            {
                if (teams[i].Equals(equipo))
                {
                    return escudos[i];
                }
            }
            return "";
        }

        public Rachas GetRachas()
        {
            return _rachas;
        }
    }
}
