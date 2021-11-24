using System;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Persistencia;

namespace Futbol_Sala_Manager_App.Interfaz
{
    public partial class EditEstadisticasForm : Form
    {
        private EncuentroData _eData;

        public EditEstadisticasForm()
        {
            InitializeComponent();
        }

        public EditEstadisticasForm(EncuentroData eData)
        {
            InitializeComponent();
            this._eData = eData;
        }

        void LoadData(object sender, EventArgs e)
        {
            try
            {
                label_localTeam.Text = _eData.EquipoL.ShortName;
                label_visitorTeam.Text = _eData.EquipoV.ShortName;
                this.textBox_posesionL.Text = _eData.Posesion.getPorcentajeLocal().Replace("%", "");
                this.textBox_posesionV.Text = _eData.Posesion.getPorcentajeVisitante().Replace("%", "");
                this.textBox_foulsL.Text = _eData.EquipoL.getFaltas().ToString();
                this.textBox_foulsV.Text = _eData.EquipoV.getFaltas().ToString();
                this.textBox_attemptsOnTargetL.Text = _eData.EquipoL.getTirosAPuerta().ToString();
                this.textBox_attemptsOnTargetV.Text = _eData.EquipoV.getTirosAPuerta().ToString();
                this.textBox_attemptsL.Text = _eData.EquipoL.getTirosCompuesto().ToString();
                this.textBox_attemptsV.Text = _eData.EquipoV.getTirosCompuesto().ToString();
                this.textBox_cornerL.Text = _eData.EquipoL.getCorners().ToString();
                this.textBox_cornerV.Text = _eData.EquipoV.getCorners().ToString();
                this.textBox_yellowCardsL.Text = _eData.EquipoL.getTAmarillas().ToString();
                this.textBox_yellowCardsV.Text = _eData.EquipoV.getTAmarillas().ToString();
                this.textBox_redCardsL.Text = _eData.EquipoL.getTRojas().ToString();
                this.textBox_redCardsV.Text = _eData.EquipoV.getTRojas().ToString();
                this.textBox_stopsL.Text = _eData.EquipoL.getParadas().ToString();
                this.textBox_stopsV.Text = _eData.EquipoV.getParadas().ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }

            // Añadimos los eventos para que no se llamen nada más abrir el formulario
            this.textBox_foulsL.TextChanged += GlobalModifications;
            this.textBox_foulsV.TextChanged += GlobalModifications;
            this.textBox_attemptsL.TextChanged += GlobalModifications;
            this.textBox_attemptsV.TextChanged += GlobalModifications;
            this.textBox_attemptsOnTargetL.TextChanged += GlobalModifications;
            this.textBox_attemptsOnTargetV.TextChanged += GlobalModifications;
            this.textBox_stopsL.TextChanged += GlobalModifications;
            this.textBox_stopsV.TextChanged += GlobalModifications;
            this.textBox_foulsL.TextChanged += GlobalModifications;
            this.textBox_foulsV.TextChanged += GlobalModifications;
            this.textBox_cornerL.TextChanged += GlobalModifications;
            this.textBox_cornerV.TextChanged += GlobalModifications;
            this.textBox_posesionL.TextChanged += GlobalModifications;
            this.textBox_posesionV.TextChanged += GlobalModifications;
            this.textBox_yellowCardsL.TextChanged += GlobalModifications;
            this.textBox_yellowCardsV.TextChanged += GlobalModifications;
            this.textBox_redCardsL.TextChanged += GlobalModifications;
            this.textBox_redCardsV.TextChanged += GlobalModifications;

            CheckIfModifications();
        }

        void CheckIfModifications()
        {
            this.checkBox_useModifications.Checked = _eData.EquipoL._modified || _eData.Posesion._modificado;
        }

        #region ------ Eventos ------
        void GlobalModifications(object sender, EventArgs e)
        {
            this._eData.EquipoL._modified = true;
            this._eData.EquipoV._modified = true;
            this._eData.Posesion._modificado = true;

            try
            {
                Fouls_Local_TextChanged(sender, e);
                Fouls_Visitante_TextChanged(sender, e);
                BallPossession_Local_TextChanged(sender, e);
                BallPossession_Visitante_TextChanged(sender, e);
                AttemptsOnTarget_Local_TextChanged(sender, e);
                AttemptsOnTarget_Visitante_TextChanged(sender, e);
                Attempts_Local_TextChanged(sender, e);
                Attempts_Visitante_TextChanged(sender, e);
                Corners_Local_TextChanged(sender, e);
                Corners_Visitante_TextChanged(sender, e);
                YellowCards_Local_TextChanged(sender, e);
                YellowCards_Visitante_TextChanged(sender, e);
                RedCards_Local_TextChanged(sender, e);
                RedCards_Visitante_TextChanged(sender, e);
                Stops_Local_TextChanged(sender, e);
                Stops_Visitante_TextChanged(sender, e);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }

            CheckIfModifications();
        }

        void Modifications_CheckedChanged(object sender, EventArgs e)
        {
            _eData.EquipoL._modified = checkBox_useModifications.Checked;
            _eData.EquipoV._modified = checkBox_useModifications.Checked;
            _eData.Posesion._modificado = checkBox_useModifications.Checked;
        }

        // BallPossession
        void BallPossession_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.Posesion.setPorcentajeLocal(Convert.ToInt32(textBox_posesionL.Text));
        }
        void BallPossession_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.Posesion.setPorcentajeVisitante(Convert.ToInt32(textBox_posesionV.Text));
        }

        // AttemptsOnTarget
        void AttemptsOnTarget_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.attemptsOnTargetModified = Convert.ToInt32(textBox_attemptsOnTargetL.Text);
        }
        void AttemptsOnTarget_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.attemptsOnTargetModified = Convert.ToInt32(textBox_attemptsOnTargetV.Text);
        }

        // TotalAttempts
        void Attempts_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.attemptsModified = Convert.ToInt32(textBox_attemptsL.Text);
        }
        void Attempts_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.attemptsModified = Convert.ToInt32(textBox_attemptsV.Text);
        }

        // Corners
        void Corners_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.cornersModified = Convert.ToInt32(textBox_cornerL.Text);
        }
        void Corners_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.cornersModified = Convert.ToInt32(textBox_cornerV.Text);
        }
        
        // FoulsCommitted
        void Fouls_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.foulsModified = Convert.ToInt32(textBox_foulsL.Text);
        }
        void Fouls_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.foulsModified = Convert.ToInt32(textBox_foulsV.Text);
        }

        // YellowCards
        void YellowCards_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.yellowCardsModified = Convert.ToInt32(textBox_yellowCardsL.Text);
        }
        void YellowCards_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.yellowCardsModified = Convert.ToInt32(textBox_yellowCardsV.Text);
        }
        // RedCards
        void RedCards_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.redCardsModified = Convert.ToInt32(textBox_redCardsL.Text);
        }
        void RedCards_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.redCardsModified = Convert.ToInt32(textBox_redCardsV.Text);
        }
        // Stops
        void Stops_Local_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoL.paradasModified = Convert.ToInt32(textBox_stopsL.Text);
        }
        void Stops_Visitante_TextChanged(object sender, EventArgs e)
        {
            this._eData.EquipoV.paradasModified = Convert.ToInt32(textBox_stopsV.Text);
        }
        #endregion
    }
}
