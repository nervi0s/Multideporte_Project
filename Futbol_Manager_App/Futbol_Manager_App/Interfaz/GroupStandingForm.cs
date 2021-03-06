using Futbol_Manager_App.Beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Futbol_Manager_App.Interfaz
{
    public partial class GroupStandingForm : Form
    {
        public class TeamData
        {
            public ComboBox equipo = new ComboBox();
            public TextBox pts = new TextBox();
            public TextBox p = new TextBox();
            public TextBox w = new TextBox();
            public TextBox d = new TextBox();
            public TextBox l = new TextBox();
            public TextBox gf = new TextBox();
            public TextBox ga = new TextBox();
        }

        private GroupStanding _groupStanding;
        private TeamData[] _tData;
        string[] teams;
        //string[] escudos;

        public GroupStandingForm()
        {
            InitializeComponent();

            LoadLeagueGroupCombos();
            GetTeamsInfo();
            InitializeTableLayout();
        }

        public GroupStandingForm(GroupStanding gs)
        {
            InitializeComponent();

            LoadLeagueGroupCombos();
            GetTeamsInfo();
            InitializeTableLayout();
            LoadData(gs);
        }

        void LoadLeagueGroupCombos()
        {
            try
            {
                // League 
                comboBox_League.Items.Clear();
                comboBox_League.Items.AddRange(new string[] { "League A", "League B", "League C", "League D" });
                comboBox_League.SelectedItem = comboBox_League.Items[0];
                comboBox_League.SelectedIndexChanged += comboBox_SelectedIndexChanged;
                // Group
                comboBox_Group.Items.Clear();
                comboBox_Group.Items.AddRange(new string[] { "Group 1", "Group 2", "Group 3", "Group 4" });
                comboBox_Group.SelectedItem = comboBox_Group.Items[0];
                comboBox_Group.SelectedIndexChanged += comboBox_SelectedIndexChanged;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }

        void InitializeTableLayout()
        {
            CleanControls();

            int length = teams.Length > 6 ? 6 : teams.Length;
            // Son 16 equipos los que hay que crear
            _tData = new TeamData[length];
            // Cogemos los equipos del fichero

            // La primera fila no cuenta que es la de los textos que indican que es cada columna
            // Creamos los equipos
            for (int i = 0; i < length; ++i) // FILAS
            {
                _tData[i] = new TeamData();
                // Equipo
                _tData[i].equipo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].equipo.Items.AddRange(teams);
                _tData[i].equipo.SelectedItem = _tData[i].equipo.Items[i];
                _tData[i].equipo.DropDownStyle = ComboBoxStyle.DropDownList;
                tableLayoutPanel_teams.Controls.Add(_tData[i].equipo, 0, i + 1);
                // pts
                _tData[i].pts.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].pts.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].pts, 1, i + 1);
                // p
                _tData[i].p.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].p.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].p, 2, i + 1);
                // w
                _tData[i].w.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].w.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].w, 3, i + 1);
                // d
                _tData[i].d.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].d.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].d, 4, i + 1);
                // l
                _tData[i].l.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].l.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].l, 5, i + 1);
                // gf
                _tData[i].gf.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].gf.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].gf, 6, i + 1);
                // ga
                _tData[i].ga.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                _tData[i].ga.TextAlign = HorizontalAlignment.Center;
                tableLayoutPanel_teams.Controls.Add(_tData[i].ga, 7, i + 1);
            }
        }

        void CleanControls()
        {
            if(_tData != null)
            foreach(var t in _tData)
            {
                tableLayoutPanel_teams.Controls.Remove(t.equipo);
                tableLayoutPanel_teams.Controls.Remove(t.pts);
                tableLayoutPanel_teams.Controls.Remove(t.p);
                tableLayoutPanel_teams.Controls.Remove(t.w);
                tableLayoutPanel_teams.Controls.Remove(t.d);
                tableLayoutPanel_teams.Controls.Remove(t.l);
                tableLayoutPanel_teams.Controls.Remove(t.gf);
                tableLayoutPanel_teams.Controls.Remove(t.ga);
            }
        }

        void GetTeamsInfo()
        {
            try
            {
                string st = comboBox_League.Text.Replace(" ", "") + "_" + comboBox_Group.Text.Replace(" ", "") + ".txt";
                //Console.WriteLine(Path.GetDirectoryName(Application.ExecutablePath) + "/" + st);
                string[] lines = File.ReadAllLines(Path.GetDirectoryName(Application.ExecutablePath) + "/futbol/" + st);                                

                //List<string> equiposLista = new List<string>();
                //List<string> escudosLista = new List<string>();
                //foreach (var l in lines)
                //{
                //    string[] s = l.Split(';');
                //    if (s.Length > 0)
                //        equiposLista.Add(s[0]);
                //    if (s.Length > 1)
                //        escudosLista.Add(s[1]);
                //}

                teams = lines;// equiposLista.ToArray();
                //escudos = escudosLista.ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }

        void LoadData(GroupStanding gs)
        {
            if (gs == null || gs.Teams == null || gs.Teams.Length < 0)
                return;

            comboBox_League.Text = gs.League;
            comboBox_Group.Text = gs.Group;
            for (int i = 0; i < gs.Teams.Length && i < _tData.Length; ++i)
            {
                TeamData td = _tData[i];
                GroupStanding.TeamInfo ti = gs.Teams[i];
                // Equipo
                td.equipo.Text = ti.Equipo;
                td.pts.Text = ti.pts;
                td.p.Text = ti.p;
                td.w.Text = ti.w;
                td.d.Text = ti.d;
                td.l.Text = ti.l;
                td.gf.Text = ti.gf;
                td.ga.Text = ti.ga;
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _groupStanding = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            List<GroupStanding.TeamInfo> equiposLista = new List<GroupStanding.TeamInfo>();
            foreach (var ec in _tData)
            {
                equiposLista.Add(new GroupStanding.TeamInfo
                {
                    Equipo = ec.equipo.Text,
                    pts = ec.pts.Text,
                    p = ec.p.Text,
                    w = ec.w.Text,
                    d = ec.d.Text,
                    l = ec.l.Text,
                    gf = ec.gf.Text,
                    ga = ec.ga.Text
                    //PhotoPath = getEscudo(ec.equipo.Text)
                });
            }
            _groupStanding = new GroupStanding
            {
                Teams = equiposLista.ToArray(),
                League = comboBox_League.Text,
                Group = comboBox_Group.Text
            };

            this.Close();
        }

        //string getEscudo(string equipo)
        //{
        //    for (int i = 0; i < teams.Length; ++i)
        //    {
        //        if (teams[i].Equals(equipo))
        //        {
        //            return escudos[i];
        //        }
        //    }
        //    return "";
        //}

        public GroupStanding GetGroupStanding()
        {
            return _groupStanding;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTeamsInfo();
            InitializeTableLayout();
        }
    }
}
