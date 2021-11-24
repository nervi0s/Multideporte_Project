using System;
using System.Windows.Forms;
using Futbol_Manager_App.Persistencia;

namespace Futbol_Manager_App.Interfaz
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
                this.textBox_foulsCL.Text = _eData.EquipoL.getFaltasCometidas().ToString();
                this.textBox_foulsCV.Text = _eData.EquipoV.getFaltasCometidas().ToString();
                this.textBox_foulsRL.Text = _eData.EquipoL.getFaltasRecibidas().ToString();
                this.textBox_foulsRV.Text = _eData.EquipoV.getFaltasRecibidas().ToString();
                this.textBox_ballPossessionL.Text = _eData.Posesion.getPorcentajeLocal().Replace("%", "");
                this.textBox_ballPossessionV.Text = _eData.Posesion.getPorcentajeVisitante().Replace("%", "");
                this.textBox_attemptsOnTargetL.Text = _eData.EquipoL.getTirosAPuerta().ToString();
                this.textBox_attemptsOnTargetV.Text = _eData.EquipoV.getTirosAPuerta().ToString();
                this.textBox_totalAttemptsL.Text = _eData.EquipoL.getTirosCompuesto().ToString();
                this.textBox_totalAttemptsV.Text = _eData.EquipoV.getTirosCompuesto().ToString();
                this.textBox_cornersL.Text = _eData.EquipoL.getCorners().ToString();
                this.textBox_cornersV.Text = _eData.EquipoV.getCorners().ToString();
                this.textBox_offsidesL.Text = _eData.EquipoL.getFuerasJuego().ToString();
                this.textBox_offsidesV.Text = _eData.EquipoV.getFuerasJuego().ToString();
                this.textBox_passesL.Text = _eData.EquipoL.getPases().ToString();
                this.textBox_passesV.Text = _eData.EquipoV.getPases().ToString();
                this.textBox_completedL.Text = _eData.EquipoL.getPasesCompletados().ToString();
                this.textBox_completedV.Text = _eData.EquipoV.getPasesCompletados().ToString();
                this.textBox_yellowCardsL.Text = _eData.EquipoL.getTAmarillas().ToString();
                this.textBox_yellowCardsV.Text = _eData.EquipoV.getTAmarillas().ToString();
                this.textBox_redCardsL.Text = _eData.EquipoL.getTRojas().ToString();
                this.textBox_redCardsV.Text = _eData.EquipoV.getTRojas().ToString();
                this.textBox_paradasL.Text = _eData.EquipoL.getParadas().ToString();
                this.textBox_paradasV.Text = _eData.EquipoV.getParadas().ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }

            // Añadimos los eventos para que no se llamen nada más abrir el formulario
            this.textBox_foulsCL.TextChanged += GlobalModifications;
            this.textBox_foulsCV.TextChanged += GlobalModifications;
            this.textBox_foulsRL.TextChanged += GlobalModifications;
            this.textBox_foulsRV.TextChanged += GlobalModifications;
            this.textBox_ballPossessionL.TextChanged += GlobalModifications;
            this.textBox_ballPossessionV.TextChanged += GlobalModifications;
            this.textBox_attemptsOnTargetL.TextChanged += GlobalModifications;
            this.textBox_attemptsOnTargetV.TextChanged += GlobalModifications;
            this.textBox_totalAttemptsL.TextChanged += GlobalModifications;
            this.textBox_totalAttemptsV.TextChanged += GlobalModifications;
            this.textBox_cornersL.TextChanged += GlobalModifications;
            this.textBox_cornersV.TextChanged += GlobalModifications;
            this.textBox_offsidesL.TextChanged += GlobalModifications;
            this.textBox_offsidesV.TextChanged += GlobalModifications;
            this.textBox_passesL.TextChanged += GlobalModifications;
            this.textBox_passesV.TextChanged += GlobalModifications;
            this.textBox_completedL.TextChanged += GlobalModifications;
            this.textBox_completedV.TextChanged += GlobalModifications;
            this.textBox_yellowCardsL.TextChanged += GlobalModifications;
            this.textBox_yellowCardsV.TextChanged += GlobalModifications;
            this.textBox_redCardsL.TextChanged += GlobalModifications;
            this.textBox_redCardsV.TextChanged += GlobalModifications;
            this.textBox_paradasL.TextChanged += GlobalModifications;
            this.textBox_paradasV.TextChanged += GlobalModifications;

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
                FoulsCommitted_Local_TextChanged(sender, e);
                FoulsCommitted_Visitante_TextChanged(sender, e);
                FoulsReceived_Local_TextChanged(sender, e);
                FoulsReceived_Visitante_TextChanged(sender, e);
                BallPossession_Local_TextChanged(sender, e);
                BallPossession_Visitante_TextChanged(sender, e);
                AttemptsOnTarget_Local_TextChanged(sender, e);
                AttemptsOnTarget_Visitante_TextChanged(sender, e);
                TotalAttempts_Local_TextChanged(sender, e);
                TotalAttempts_Visitante_TextChanged(sender, e);
                Corners_Local_TextChanged(sender, e);
                Corners_Visitante_TextChanged(sender, e);
                Offsides_Local_TextChanged(sender, e);
                Offsides_Visitante_TextChanged(sender, e);
                Passes_Local_TextChanged(sender, e);
                Passes_Visitante_TextChanged(sender, e);
                Completed_Local_TextChanged(sender, e);
                Completed_Visitante_TextChanged(sender, e);
                YellowCards_Local_TextChanged(sender, e);
                YellowCards_Visitante_TextChanged(sender, e);
                RedCards_Local_TextChanged(sender, e);
                RedCards_Visitante_TextChanged(sender, e);
                Paradas_Local_TextChanged(sender, e);
                Paradas_Visitante_TextChanged(sender, e);
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
            int n;
            if(int.TryParse(textBox_ballPossessionL.Text, out n))
                this._eData.Posesion.setPorcentajeLocal(Convert.ToInt32(textBox_ballPossessionL.Text));
        }
        void BallPossession_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_ballPossessionV.Text, out n))
                this._eData.Posesion.setPorcentajeVisitante(Convert.ToInt32(textBox_ballPossessionV.Text));
        }

        // AttemptsOnTarget
        void AttemptsOnTarget_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_attemptsOnTargetL.Text, out n))
                this._eData.EquipoL.attemptsOnTargetModified = Convert.ToInt32(textBox_attemptsOnTargetL.Text);
        }
        void AttemptsOnTarget_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_attemptsOnTargetV.Text, out n))
                this._eData.EquipoV.attemptsOnTargetModified = Convert.ToInt32(textBox_attemptsOnTargetV.Text);
        }

        // TotalAttempts
        void TotalAttempts_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_totalAttemptsL.Text, out n))
                this._eData.EquipoL.totalAttemptsModified = Convert.ToInt32(textBox_totalAttemptsL.Text);
        }
        void TotalAttempts_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_totalAttemptsV.Text, out n))
                this._eData.EquipoV.totalAttemptsModified = Convert.ToInt32(textBox_totalAttemptsV.Text);
        }

        // Corners
        void Corners_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_cornersL.Text, out n))
                this._eData.EquipoL.cornersModified = Convert.ToInt32(textBox_cornersL.Text);
        }
        void Corners_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_cornersV.Text, out n))
                this._eData.EquipoV.cornersModified = Convert.ToInt32(textBox_cornersV.Text);
        }

        // Offsides
        void Offsides_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_offsidesL.Text, out n))
                this._eData.EquipoL.offsidesModified = Convert.ToInt32(textBox_offsidesL.Text);
        }
        void Offsides_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_offsidesV.Text, out n))
                this._eData.EquipoV.offsidesModified = Convert.ToInt32(textBox_offsidesV.Text);
        }
        // Passes
        void Passes_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_passesL.Text, out n))
                this._eData.EquipoL.passesModified = Convert.ToInt32(textBox_passesL.Text);
        }
        void Passes_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_passesV.Text, out n))
                this._eData.EquipoV.passesModified = Convert.ToInt32(textBox_passesV.Text);
        }
        // Completed
        void Completed_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_completedL.Text, out n))
                this._eData.EquipoL.completedModified = Convert.ToInt32(textBox_completedL.Text);
        }
        void Completed_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_completedV.Text, out n))
                this._eData.EquipoV.completedModified = Convert.ToInt32(textBox_completedV.Text);
        }
        // FoulsCommitted
        void FoulsCommitted_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_foulsCL.Text, out n))
                this._eData.EquipoL.foulsCommittedModified = Convert.ToInt32(textBox_foulsCL.Text);
        }
        void FoulsCommitted_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_foulsCV.Text, out n))
                this._eData.EquipoV.foulsCommittedModified = Convert.ToInt32(textBox_foulsCV.Text);
        }
        // FoulsReceived
        void FoulsReceived_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_foulsRL.Text, out n))
                this._eData.EquipoL.foulsReceivedModified = Convert.ToInt32(textBox_foulsRL.Text);
        }
        void FoulsReceived_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_foulsRV.Text, out n))
                this._eData.EquipoV.foulsReceivedModified = Convert.ToInt32(textBox_foulsRV.Text);
        }
        // YellowCards
        void YellowCards_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_yellowCardsL.Text, out n))
                this._eData.EquipoL.yellowCardsModified = Convert.ToInt32(textBox_yellowCardsL.Text);
        }
        void YellowCards_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_yellowCardsV.Text, out n))
                this._eData.EquipoV.yellowCardsModified = Convert.ToInt32(textBox_yellowCardsV.Text);
        }
        // RedCards
        void RedCards_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_redCardsL.Text, out n))
                this._eData.EquipoL.redCardsModified = Convert.ToInt32(textBox_redCardsL.Text);
        }
        void RedCards_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_redCardsV.Text, out n))
                this._eData.EquipoV.redCardsModified = Convert.ToInt32(textBox_redCardsV.Text);
        }
        // Paradas
        void Paradas_Local_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_paradasL.Text, out n))
                this._eData.EquipoL.paradasModified = Convert.ToInt32(textBox_paradasL.Text);
        }
        void Paradas_Visitante_TextChanged(object sender, EventArgs e)
        {
            int n;
            if (int.TryParse(textBox_paradasV.Text, out n))
                this._eData.EquipoV.paradasModified = Convert.ToInt32(textBox_paradasV.Text);
        }
        #endregion
    }
}
