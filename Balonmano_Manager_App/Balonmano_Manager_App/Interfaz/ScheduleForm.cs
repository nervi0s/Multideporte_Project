using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Balonmano_Manager_App.Beans;

namespace Balonmano_Manager_App.Interfaz
{
    public partial class ScheduleForm : Form
    {
        private Schedule _schedule;


        public ScheduleForm()
        {
            InitializeComponent();
        }

        public ScheduleForm(Schedule s)
        {
            InitializeComponent();

            textBox_E1_equipo1.Text = s.Schedules[0].Equipo1;
            textBox_E1_equipo2.Text = s.Schedules[0].Equipo2;
            textBox_E1_info.Text = s.Schedules[0].Info;
            textBox_E2_equipo1.Text = s.Schedules[1].Equipo1;
            textBox_E2_equipo2.Text = s.Schedules[1].Equipo2;
            textBox_E2_info.Text = s.Schedules[1].Info;            
        }

          
        private void Aceptar_Click(object sender, EventArgs e)
        {
            List<Schedule.SchedulesInfo> scheduleLista = new List<Schedule.SchedulesInfo>();
            scheduleLista.Add(new Schedule.SchedulesInfo
            {
                Equipo1 = textBox_E1_equipo1.Text,
                Equipo2 = textBox_E1_equipo2.Text,
                Info = textBox_E1_info.Text,
            });
            scheduleLista.Add(new Schedule.SchedulesInfo
            {
                Equipo1 = textBox_E2_equipo1.Text,
                Equipo2 = textBox_E2_equipo2.Text,
                Info = textBox_E2_info.Text,
            });

            _schedule = new Schedule
            {
                Schedules = scheduleLista.ToArray(),
            };

            this.Close();
        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            _schedule = null;
            this.Close();
        }

        public Schedule GetSchedule()
        {
            return _schedule;
        }
    }
}
