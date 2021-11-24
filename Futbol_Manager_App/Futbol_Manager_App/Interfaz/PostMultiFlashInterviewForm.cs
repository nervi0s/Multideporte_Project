using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Interfaz
{
    public partial class PostMultiFlashInterviewForm : Form
    {
        private PostInterview _postInterview;

        public PostMultiFlashInterviewForm()
        {
            InitializeComponent();
        }

        public PostMultiFlashInterviewForm(PostInterview po)
        {
            InitializeComponent();

            this.textBoxHeader.Text = po.Header;
            this.textBoxLinea1.Text = po.Linea1;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _postInterview = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _postInterview = new PostInterview(this.textBoxHeader.Text, this.textBoxLinea1.Text);
            this.Close();
        }

        public PostInterview GetPostInterview()
        {
            return _postInterview;
        }

        private void buttonCancelar_Click_1(object sender, EventArgs e)
        {
            _postInterview = null;
            this.Close();
        }

        private void buttonAceptar_Click_1(object sender, EventArgs e)
        {
            _postInterview = new PostInterview(this.textBoxHeader.Text, this.textBoxLinea1.Text);
            this.Close();
        }
    }
}
