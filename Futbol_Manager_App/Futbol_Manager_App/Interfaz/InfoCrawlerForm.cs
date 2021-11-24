using System;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;

namespace Futbol_Manager_App.Interfaz
{
    public partial class InfoCrawlerForm : Form
    {
        private InfoCrawler _infoCrawler;

        public InfoCrawlerForm()
        {
            InitializeComponent();
        }

        public InfoCrawlerForm(InfoCrawler ic)
        {
            InitializeComponent();
            this.richTextBox_info.Text = ic.Title;
        }

        public InfoCrawler GetInfoCrawler()
        {
            return _infoCrawler;
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            _infoCrawler = null;
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            _infoCrawler = new InfoCrawler(this.richTextBox_info.Text);
            this.Close();
        }
    }
}
