using System.Drawing;
using System.Windows.Forms;

namespace Futbol_Manager_App.Interfaz
{
    /**
     * 
     */
    public partial class PenaltisPanel : UserControl
    {
        private Controlador _controlador;

        private int _penaltisL;
        private int _penaltisV;

        private ControlCollection _items;


        /**
         * Constructor
         */
        public PenaltisPanel()
        {
            InitializeComponent();
            
            _penaltisL = 0;
            _penaltisV = 0;

            _items = this.panel.Controls;
        }

        /**
         * Establece el Controlador
         */
        public void BindControlador(Controlador controlador)
        {
            _controlador = controlador;
        }

        /**
         * Añade un penalti
         */
        public void AddPenalti(bool local, string jugador, bool gol)
        {
            int penaltis = (local ? _penaltisL : _penaltisV);

            if (_items.Count <= penaltis)
            {
                addItem();
            }

            ((PenaltisPanelItem)_items[penaltis]).SetJugador(local, jugador, gol);

            if (local)
                _penaltisL++;
            else
                _penaltisV++;
        }

        /**
         * Borra el último penalti del equipo indicado
         */
        public void DelPenalti(bool local)
        {
            if (local)
                _penaltisL--;
            else
                _penaltisV--;

            int penaltis = (local ? _penaltisL : _penaltisV);
            PenaltisPanelItem item = (PenaltisPanelItem)_items[penaltis];

            item.SetJugador(local, "", false);
            if (item.IsEmpty())
                _items.Remove(item);
        }


        /**
         * Establece el texto del marcador
         */
        public void SetMarcador(string marcador)
        {
            this.marcador.Text = marcador;
        }

        /**
         * Activa o desactiva el marcador
         */
        public void ActivaMarcador(bool activo)
        {
            this.marcador.Enabled = activo;
            if (!activo)
                this.marcador.Checked = false;
        }

 



        private void addItem()
        {
            PenaltisPanelItem item = new PenaltisPanelItem(_items.Count + 1);
            item.Dock = DockStyle.Top;

            _items.Add(item);
        }



        private void buttonGol_Click(object sender, System.EventArgs e)
        {
            _controlador.GetPenaltis().SetFlagAcierto(true);
        }

        private void buttonFallo_Click(object sender, System.EventArgs e)
        {
            _controlador.GetPenaltis().SetFlagAcierto(false);
        }

        private void marcador_CheckedChanged(object sender, System.EventArgs e)
        {
            bool on = ((CheckBox)sender).Checked;

            _controlador.GetPenaltis().showPenaltisIpf(on);

            this.marcador.BackColor = (on ? Color.LightCoral : Color.WhiteSmoke);
        }

    }
}
