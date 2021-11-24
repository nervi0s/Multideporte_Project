using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Futbol_Sala_Manager_App
{
    public partial class CustomNumeric : NumericUpDown
    {
        public CustomNumeric()
        {
            InitializeComponent();
        }

        protected override void UpdateEditText()
        {
            this.Text = Value.ToString("00");
            // base.UpdateEditText();
        }
    }
}
