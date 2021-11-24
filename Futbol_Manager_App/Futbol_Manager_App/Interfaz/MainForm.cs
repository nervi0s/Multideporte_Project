using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Futbol_Manager_App.Beans;
using Futbol_Manager_App.Comandos;
using Futbol_Manager_App.Persistencia;


namespace Futbol_Manager_App.Interfaz
{
    /**
     * Formulario Principal
     */
    public partial class MainForm : Form
    {
        private Controlador _controlador = null;
        private Color _colorL, _colorV;
        private TableLayoutPanel _menuActual;
        private ConfigData _config;

        private List<int> _stats = new List<int>();
        private bool _orden_de_cerrar = false;



        /**
         * Constructor
         */
        public MainForm(Controlador controlador)
        {
            InitializeComponent();     
            
            _controlador = controlador;
            this.penaltis.BindControlador(controlador);

            // Añade y oculta los submenus
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesPrevia, 1, 0);
            this.panelOpcionesPrevia.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesPresentacion, 1, 0);
            this.panelOpcionesPresentacion.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesRotulos, 1, 0);
            this.panelOpcionesRotulos.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesCrono, 1, 0);
            this.panelOpcionesCrono.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesCambios, 1, 0);
            this.panelOpcionesCambios.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesEstadisticas, 1, 0);
            this.panelOpcionesEstadisticas.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesGoles, 1, 0);
            this.panelOpcionesGoles.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesTiros, 1, 0);
            this.panelOpcionesTiros.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesFaltas, 1, 0);
            this.panelOpcionesFaltas.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesPases, 1, 0);
            this.panelOpcionesPases.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesStats, 1, 0);
            this.panelOpcionesStats.Hide();

            // Penaltis
            this.tableLayoutPanelCol3.Controls.Add(this.penaltis, 0, 2);
            this.penaltis.Hide();

            // Lista de eventos
            this.tableLayoutPanelCol3.Controls.Add(this.listBox, 0, 2);

            _config = Persistencia.PersistenciaUtil.CargaConfig();
        }

        // Captura el evento de cerrar el formulario
        override protected void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_orden_de_cerrar)
            {
                string mensaje = "¿ Está seguro de cerrar la aplicación ?";
                MessageBoxButtons botones = MessageBoxButtons.YesNo;
                string caption = "Información";
                DialogResult result;
                result = MessageBox.Show(mensaje, caption, botones);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    _orden_de_cerrar = true;
                    _controlador.CerrarAplicacion();
                }
                else
                    e.Cancel = true;
            }
        }


        // ################################# AUXILIAR ################################
        #region Metodos Auxiliares

        private void addJugador(bool eqLocal, bool titular, Jugador j)
        {
            Button controlNombre = new Button();
            controlNombre.Text = j.ShortName;
            controlNombre.Dock = System.Windows.Forms.DockStyle.Fill;
            controlNombre.Margin = new System.Windows.Forms.Padding(0);
            controlNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            controlNombre.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Label controlNumero = new Label();
            controlNumero.Text = Convert.ToString(j.Number);
            Color colorPos;
            if (j.Posicion == Jugador.Portero)
            {
                colorPos = System.Drawing.Color.WhiteSmoke;
            }
            else if (j.Posicion == Jugador.Defensa)
            {
                colorPos = System.Drawing.Color.LightGray;
            }
            else if (j.Posicion == Jugador.Centrocampista)
            {
                colorPos = System.Drawing.Color.DarkGray;
            }
            else // Delantero
            { 
                colorPos = System.Drawing.Color.DimGray;
            }
            controlNumero.BackColor = colorPos;
            controlNumero.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            controlNumero.Dock = System.Windows.Forms.DockStyle.Fill;
            controlNumero.Margin = new System.Windows.Forms.Padding(0);
            controlNumero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            TableLayoutPanel panel;
            if (titular)
            {
                panel = (eqLocal ? this.panelJugadoresEq1 : this.panelJugadoresEq2);
            }
            else
            {
                panel = (eqLocal ? this.panelBanquilloEq1 : this.panelBanquilloEq2);
            }

            // Listener
            controlNombre.Click += delegate { _controlador.ClickOnJugador( j,controlNombre ); };

            panel.Controls.Add(controlNumero);
            panel.Controls.Add(controlNombre);
        }

        private void configMenu(TableLayoutPanel menu)
        {
            //Console.WriteLine("{0}", menu.Name);
            if (_menuActual != null)
            {
                _menuActual.Hide();
            }

            if (menu != null)
            {
                // Desactiva todas las opciones del submenu
                foreach (Control c in menu.Controls)
                {
                    if (c is RadioButton)
                        ((RadioButton)c).Checked = false;

                    if (c is CheckBox)
                        ((CheckBox)c).Checked = false;
                }

                menu.Show();
                //Console.WriteLine("Show");
            }
            
            // Si se activa el menu del Crono o se hace un Clear, desactiva las opciones principales
            if (menu == panelOpcionesCrono)
            {
                foreach (Control c in panelOpcionesMain.Controls)
                {
                    ((RadioButton)c).Checked = false;
                }
            }
            else // Desactiva la seccion Crono cuando se selecciona cualquier otra
            {
                buttonCrono.Checked = false;
            }
            
            _menuActual = menu;
        }

        #endregion

        // ################################# PUBLICOS ################################
        #region Metodos Publicos

        public void ConfigEquipoLocal(Equipo equipo)
        {
            this.checkBoxEqL.Text = equipo.TeamCode;
            this.checkBoxEntrenadorL.Text = equipo.Entrenador.ShortName;
            this.checkBoxEntrenadorL.Click += delegate { _controlador.ClickOnEntrenador(equipo.Entrenador); };
            this.checkBoxAsistenteL.Text = equipo.Asistente.ShortName;
            this.checkBoxAsistenteL.Click += delegate { _controlador.ClickOnAsistente(equipo.Asistente); };

            panelJugadoresEq1.Controls.Clear();
            equipo.Jugadores.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Jugadores)
            {
                addJugador(true, true, j);
            }
            panelBanquilloEq1.Controls.Clear();
            equipo.Banquillo.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Banquillo)
            {
                addJugador(true, false, j);
            }
            
            if (equipo.Color1.A < 255)
            {
                if (equipo.Color1.B <= 125 && equipo.Color1.G <= 125 && equipo.Color1.R <= 125)
                    equipo.Color1 = Color.FromArgb(255, 255, 255, 255);
                else
                    equipo.Color1 = Color.FromArgb(255, equipo.Color1.R, equipo.Color1.G, equipo.Color1.B);
            }
            _colorL = equipo.Color1;
        }
        public void ConfigEquipoVisitante(Equipo equipo)
        {
            this.checkBoxEqV.Text = equipo.TeamCode;
            this.checkBoxEntrenadorV.Text = equipo.Entrenador.ShortName;
            this.checkBoxEntrenadorV.Click += delegate { _controlador.ClickOnEntrenador(equipo.Entrenador); };

            this.checkBoxAsistenteV.Text = equipo.Asistente.ShortName;
            this.checkBoxAsistenteV.Click += delegate { _controlador.ClickOnAsistente(equipo.Asistente); };

            panelJugadoresEq2.Controls.Clear();
            equipo.Jugadores.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Jugadores)
            {
                addJugador(false, true, j);
            }
            panelBanquilloEq2.Controls.Clear();
            equipo.Banquillo.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Banquillo)
            {
                addJugador(false, false, j);
            }

            if (equipo.Color1.A < 255)
            {
                if(equipo.Color1.B <= 125 && equipo.Color1.G <= 125 && equipo.Color1.R <= 125)
                    equipo.Color1 = Color.FromArgb(255, 255, 255, 255);
                else
                    equipo.Color1 = Color.FromArgb(255, equipo.Color1.R, equipo.Color1.G, equipo.Color1.B);
            }
            _colorV = equipo.Color1;
        }

        public void ActivaTabJugadores(bool titulares)
        {
            this.tabControlPlantilla.SelectedTab = 
                (titulares ? this.tabPageJugadores : this.tabPageBanquillo);
        }

        public void ConfigListBox(Object[] lista)
        {
            listBox.Items.Clear();
            listBox.Items.AddRange(lista);
        }

        public void SetOnAirText(string text)
        {
            this.onAir.Text = text;
            if (text.Equals("")) this.onAir.Checked = false;
            this.onAir.Enabled = (!text.Equals(""));
        }
        public void SetOnAirLive(bool live)
        {
            this.onAir.Checked = live;
            this.onAir.BackColor = (live ? Color.LightCoral : Color.WhiteSmoke);
        }

        public void SetNextAirText(string text)
        {
            text = text.Replace("\n", ": ");
            this.nextAir.Text = text;
            this.nextAir.Enabled = (!text.Equals(""));
        }

        public void SetMarcador(string marcador)
        {
            this.marcador.Text = marcador;
        }

        public void SetCronoLive(bool live)
        {
            this.checkBoxCrono.Checked = live;
        }

        public void SetConexionEstado(bool on)
        {
            this.checkBoxConexion.Checked = on;
            this.checkBoxConexion.BackColor = (on ? Color.DarkSeaGreen : Color.Firebrick);
        }

        // This delegate enables asynchronous calls
        delegate void SetTextCallback(string text);
        public void SetCronoTextAsync(string text)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.checkBoxCrono.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetCronoText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.SetCronoText(text);
                }
            }
            catch { }
        }
        private void SetCronoText(string text)
        {
            this.checkBoxCrono.Text = text;
        }

        public PenaltisPanel GetPenaltisGui()
        {
            return this.penaltis;
        }

        public void ShowPenaltis(bool visible)
        {
            this.penaltis.Visible = visible;
            this.listBox.Visible = !visible;
        }

        #endregion

        // ################################ HISTORIAL ################################
        #region Historial

        private void onAir_Click(object sender, EventArgs e)
        {
            _controlador.OnAirRun();
        }
        private void nextAir_Click(object sender, EventArgs e)
        {
            _controlador.OnAirNext();
            _controlador.OnAirRun();
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            object command = listBox.SelectedItem;

            if (command != null && command is ICommandShowable)
            {
                _controlador.OnAirSetup((ICommandShowable)command);
            }
        }

        private void buttonBorrar_Click(object sender, EventArgs e)
        {
            _controlador.ListBoxRemoveItem(listBox.SelectedItem);
        }
        private void buttonNuevo_Click(object sender, EventArgs e)
        {
            _controlador.ListBoxNewItem();
        }
        private void buttonEditar_Click(object sender, EventArgs e)
        {
            if (listBox.SelectedItem != null)
                _controlador.ListBoxEditItem((ICommand)listBox.SelectedItem);
            
        }

        public void activaAcciones(bool nuevo, bool editar, bool borrar)
        {
            this.buttonNuevo.Enabled = nuevo;
            this.buttonEditar.Enabled = editar;
            this.buttonBorrar.Enabled = borrar;
        }

        #endregion
        
        // ############################ ACTIVACION BOTONES ###########################
        #region Activacion Botones

        /**
         * Activa los botones de los jugadores indicados, desactivando los demás
         */
        public void ActivaJugadores(bool eqLocal, List<int> dorsales)
        {
            activaTodosJugadores(eqLocal, false);

            foreach (int dorsal in dorsales)
            {
                activaJugador(eqLocal, dorsal, true);
            }
        }

        /**
         * Activa los botones de todos los jugadores excepto
         */
        public void ActivaTodosJugadores(bool eqLocal)
        {
            activaTodosJugadores(eqLocal, true);
        }

        // Activa o desactiva todos los jugadores
        private void activaTodosJugadores(bool eqLocal, bool activo)
        {
            // Algutinado en un array de todos los controles de jugadores
            Control[] controles;
            if (eqLocal)
            {
                controles = new Control[panelJugadoresEq1.Controls.Count + panelBanquilloEq1.Controls.Count];

                panelJugadoresEq1.Controls.CopyTo(controles, 0);
                panelBanquilloEq1.Controls.CopyTo(controles, panelJugadoresEq1.Controls.Count);
            }
            else // eqVisitante
            {
                controles = new Control[panelJugadoresEq2.Controls.Count + panelBanquilloEq2.Controls.Count];

                panelJugadoresEq2.Controls.CopyTo(controles, 0);
                panelBanquilloEq2.Controls.CopyTo(controles, panelJugadoresEq2.Controls.Count);
            }

            // Recorre los jugadores y cuando lo encuentra lo desactiva
            for (int i = 1; i < controles.Length; i = i + 2)
            {
                activaControl(controles[i], activo, eqLocal);
            }
        }

        // Activa o desactiva un jugador concreto
        private void activaJugador(bool eqLocal, int dorsal, bool activo)
        {
            // Algutinado en un array de todos los controles de jugadores
            Control[] controles;
            if (eqLocal)
            {
                controles = new Control[panelJugadoresEq1.Controls.Count + panelBanquilloEq1.Controls.Count];

                panelJugadoresEq1.Controls.CopyTo(controles, 0);
                panelBanquilloEq1.Controls.CopyTo(controles, panelJugadoresEq1.Controls.Count);
            }
            else // eqVisitante
            {
                controles = new Control[panelJugadoresEq2.Controls.Count + panelBanquilloEq2.Controls.Count];

                panelJugadoresEq2.Controls.CopyTo(controles, 0);
                panelBanquilloEq2.Controls.CopyTo(controles, panelJugadoresEq2.Controls.Count);
            }

            // Recorre los jugadores y cuando lo encuentra lo desactiva
            for (int i = 0; i < controles.Length - 1; i = i + 2)
            {
                if (((Label)controles[i]).Text.Equals(dorsal.ToString()))
                {
                    activaControl(controles[i + 1], activo, eqLocal);
                    break;
                }
            }
        }


        /**
         * Activa o desactiva los botones de los equipos
         */
        public void ActivaEquipos(bool activo)
        {
            activaControl(checkBoxEqL, activo, true);
            activaControl(checkBoxEqV, activo, false);
        }

        /**
         * Activa o desactiva los botones de los equipos
         */
        public void ActivaEntrenadores(bool activo)
        {
            activaControl(checkBoxEntrenadorL, activo, true);
            activaControl(checkBoxEntrenadorV, activo, false);
        }
        public void ActivaAsistentes(bool activo)
        {
            activaControl(checkBoxAsistenteL, activo, true);
            activaControl(checkBoxAsistenteV, activo, false);
        }


        // Activa o desactiva un control. Lo pinta en funcion de si es del equipo local o visitante
        private void activaControl(Control control, bool activo, bool eqLocal)
        {
            control.Enabled = activo;
            
            if (!activo)
            {
                control.BackColor = Color.DarkGray;
            }
            else
            {
                if (eqLocal)
                {
                    control.BackColor = _colorL;
                }
                else
                {
                    control.BackColor = _colorV;
                }
            }
        }


        /**
         * Activa o desactiva los controles de la posesión
         */
        public void ActivaOpcionesPosesion(bool activo)
        {
            activaControl(posesionL, activo, true);
            activaControl(posesionV, activo, false);

            this.posesionStop.Checked = true;
        }

        /**
         * Activa o desactiva las subsecciones relativas al Crono
         */
        public void ActivaOpcionesCrono(bool kickoff, bool finParte, bool finPartido)
        {
            this.cronoKickoff.Enabled = kickoff;
            this.cronoFinParte.Enabled = finParte;
            this.cronoFinPartido.Enabled = finPartido;
        }

        /**
         * Activa o desactiva el Crono
         */
        public void ActivaCrono(bool activo)
        {
            this.checkBoxCrono.Enabled = activo;
            if (!activo)
                SetCronoLive(false);
        }

        #endregion

        // ################################ POSESION #################################
        #region Botones de Posesion

        private void posesionL_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _controlador.PosesionStart(true);
            }

            // Cambia la imagen enm función del estado
            this.posesionL.Image = (((RadioButton)sender).Checked ?
                Properties.Resources.BalonBotaB_ON :
                Properties.Resources.BalonBotaB_OFF);
        }

        private void posesionV_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _controlador.PosesionStart(false);
            }

            // Cambia la imagen enm función del estado
            this.posesionV.Image = (((RadioButton)sender).Checked ?
                Properties.Resources.BalonBotaA_ON :
                Properties.Resources.BalonBotaA_OFF);
        }

        private void posesionStop_Click(object sender, EventArgs e)
        {
            // Solo realiza la acción si el evento viene de un click del usuario
            if (e is MouseEventArgs)
            {
                _controlador.PosesionStop();
                _controlador.previewPosesion();
            }
        }
        private void posesionStop_CheckedChanged(object sender, EventArgs e)
        {
            // Cambia la imagen enm función del estado
            this.posesionStop.Image = (((RadioButton)sender).Checked ?
                Properties.Resources.Balon_ON :
                Properties.Resources.Balon_OFF);
        }

        #endregion

        // ################################ OPCIONES #################################
        #region Botones de Opciones

        private void marcador_Click(object sender, EventArgs e)
        {
            _controlador.previewScore();
        }

        private void crono_CheckedChanged(object sender, EventArgs e)
        {
            bool on = ((CheckBox)sender).Checked;

            _controlador.CronoShow(on);

            checkBoxCrono.BackColor = (on ? Color.LightCoral : Color.WhiteSmoke);
        }

        private void equipoL_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ClickOnEquipo(true);
        }
        private void equipoV_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ClickOnEquipo(false);
        }

        private void arbitros_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Arbitros);
        }

        private void comentaristas_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Comentaristas);
        }

        private void plantillas_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Plantillas);
        }

        private void encuentro_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewPresentation();
        }

        private void alineacionEquipoL_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUp(true);
        }

        private void disposicionEquipoL_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLayout(true);
        }

        private void disposicionEquipoL3d_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLayout3d(true);
        }

        private void checkBoxEqLocalSuplentes_Click(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUpSubstitutes(true);
        }

        private void alineacionEquipoV_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUp(false);
        }

        private void disposicionEquipoV_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLayout(false);
        }

        private void disposicionEquipoV3d_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLayout3d(false);
        }

        private void checkBoxEqVisitSuplentes_Click(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUpSubstitutes(false);
        }

        private void suplentesL_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamReserves(true);
        }

        private void suplentesV_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamReserves(false);
        }

        private void suplentes_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamsReserves();
        }

        private void tablaStats_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewStatisticsTable();
        }

        private void prematchs_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.PreMatchs);
        }

        private void countdowns_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Countdowns);
        }
        private void Weather_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Weather);
        }
        private void EndToEndTest_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.EndToEndTest);
        }

        private void PostMultiFlashInterview_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.PostMultiFlashInterview);
        }

        private void EndOfPostMultiFlashInterview_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.EndOfPostMultiFlashInterview);
        }

        private void Highlights_Click(object sender, EventArgs e)
        {
            _controlador.previewHighlights();
        }

        private void NewsExchangeFeed_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.NewsExchangeFeed);
        }

        private void PitchConditions_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.PitchConditions);
        }

        private void InfoCrwaler_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.InfoCrawler);
        }

        private void EditStatistics_CheckedChanged(object sender, EventArgs e)
        {
            // Aquí abrimos directamente el Form. Dónde se editan las estadísticas
            // ¿¿ Hay que comprobar que ya está abierto ??
            EditEstadisticasForm exf = new EditEstadisticasForm(_controlador._datos);
            exf.Show();
        }

        private void GroupStanding_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.GroupStanding);
        }

        private void cronoKickoff_Click(object sender, EventArgs e)
        {
            _controlador.CronoEmpezar();
        }

        private void cronoFinParte_Click(object sender, EventArgs e)
        {
            _controlador.CronoFinParte();
        }

        private void cronoFinPartido_Click(object sender, EventArgs e)
        {
            _controlador.CronoFinPartido();
        }

        private void cronoReset_Click(object sender, EventArgs e)
        {
            _controlador.CronoReset();
        }

        private void cronoExtra_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.TiemposExtra);
        }

        private void cronoConfig_Click(object sender, EventArgs e)
        {
            _controlador.CronoEdit();
        }

        private void faltasRecibidas_Click(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.FaltasRecibidas);
        }

        private void faltasCometidas_Click(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.FaltasCometidas);
        }

        private void pasesCompletados_Click(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.PasesCompletados);
        }

        private void pasesTotales_Click(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.PasesTotales);
        }

        private void conexion_CheckedChanged(object sender, EventArgs e)
        {
            bool on = ((CheckBox)sender).Checked;
            int numIpfs = _config.NumIpf;
            if (on )
            {
                SeleccionIpf seleccion = new SeleccionIpf(_config.NumIpf);
                seleccion.ShowDialog();

                if (Program.CancelarConexion == false)
                {
                    if (on)
                    {
                        _controlador.EstablecerConexionIpf();

                    }
                    else
                    {
                        _controlador.CerrarConexionIpf();
                    }
                }
            }
            else
            {
                _controlador.CerrarConexionIpf();
            }
            Program.CancelarConexion = false;
        }

        #endregion

        // ################################ SECCIONES ################################
        #region Botones de Secciones

        private void previa_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesPrevia);
            _controlador.SetSeccion(Controlador.Previa);
        }

        private void presentacion_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesPresentacion);
            _controlador.SetSeccion(Controlador.Presentacion);
        }

        private void rotulos_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesRotulos);
            _controlador.SetSeccion(Controlador.Rotulos);
        }

        private void emergency_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.EmergencyCaptions);
        }

        private void corner_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Corners);

        }

        private void falta_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesFaltas);
            //_controlador.SetSeccion(Controlador.Faltas);
        }

        private void gol_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesGoles);
            _controlador.SetSeccion(Controlador.Goles);
        }

        private void golPP_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.GolesPP);
        }

        private void golPenalti_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.GolesPenalti);
        }

        private void tiroPuerta_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesTiros);
            _controlador.SetSeccion(Controlador.TirosAPuerta);
        }

        private void tiroDentro_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.TirosAPuertaDentro);
        }

        private void tiroFuera_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.TirosAPuertaFuera);
        }

        private void tiroComp_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.TirosAPuertaComp);
        }

        private void paradas_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Paradas);
        }

        private void coolingBreak_CheckedChanged(object sender, EventArgs e)
        {   
            _controlador.SetSeccion(Controlador.CoolingBreak);
        }

        private void drinksBreak_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.DrinksBreak);
        }


        private void tAmarilla_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.TAmarillas);
        }

        private void tRoja_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.TRojas);
        }

        private void fueraJuego_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.FuerasDeJuego);
        }

        private void cambio_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesCambios);
            _controlador.SetSeccion(Controlador.Cambio);

            Program.CambioEscogido = -1;
        }

        private void cambioDoble_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.CambioDoble);
            Program.CambioEscogido = -1;
        }
        private void cambioInfo_CheckedChanged(object sender, EventArgs e)
        {
            Program.CambioEscogido = 204;

            _controlador.SetSeccion(Controlador.Cambio);

            ICommandShowable comando = _controlador.GetComandoOnAir();

            if (comando!=null)
                Console.WriteLine(_controlador.GetComandoOnAir());
            
            
        }
        private void cambioIn_CheckedChanged(object sender, EventArgs e)
        {
            Program.CambioEscogido = 200;
            _controlador.SetSeccion(Controlador.Cambio);
        }
        private void cambioOut_CheckedChanged(object sender, EventArgs e)
        {
            Program.CambioEscogido = 201;
            _controlador.SetSeccion(Controlador.Cambio);
        }
        private void cambioInInfo_CheckedChanged(object sender, EventArgs e)
        {
            Program.CambioEscogido = 202;
            _controlador.SetSeccion(Controlador.Cambio);
        }
        private void cambioOutInfo_CheckedChanged(object sender, EventArgs e)
        {
            Program.CambioEscogido = 203;
            _controlador.SetSeccion(Controlador.Cambio);
        }

        public int CapturaCambio()
        {
            int n = 0;
            if (cambioIn.Checked)
                n = 200;
            if (cambioOut.Checked)
                n = 201;
            if (cambioInInfo.Checked)
                n = 202;
            if (cambioOutInfo.Checked)
                n = 203;
            if (cambioInfo.Checked)
                n = 204;
            return n;                    
        }

        void UpdateStats()
        {
            if (_stats.Count <= 1)
                _controlador.OnAirClear();
            else if (_stats.Count == 2)
                _controlador.OnAirSetup(new EstadisticasCommand(_stats[1], _stats[0], _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            else if (_stats.Count == 3)
                _controlador.OnAirSetup(new EstadisticasCommand(_stats[2], _stats[1], _stats[0], _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
        }

        private void corners_CheckedChanged(object sender, EventArgs e)
        {
            corners.Checked = false;
            if (corners.BackColor == Color.Transparent && _stats.Count < 3)
            {
                corners.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.Corners);}
            else if(corners.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.Corners);
                corners.BackColor = Color.Transparent;
            }

            UpdateStats();
        }
        private void faltas_CheckedChanged(object sender, EventArgs e)
        {
            faltas.Checked = false;
            if (faltas.BackColor == Color.Transparent && _stats.Count < 3)
            {
                faltas.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.FaltasCometidas);
            }
            else if (faltas.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.FaltasCometidas);
                faltas.BackColor = Color.Transparent;
            }

            UpdateStats();
        }

        private void faltasRecibidas_CheckChanged(object sender, EventArgs e)
        {
            faltasR.Checked = false;
            if (faltasR.BackColor == Color.Transparent && _stats.Count < 3)
            {
                faltasR.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.FaltasRecibidas);
            }
            else if (faltasR.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.FaltasRecibidas);
                faltasR.BackColor = Color.Transparent;
            }

            UpdateStats();
        }

        private void posesiones_CheckedChanged(object sender, EventArgs e)
        {
            posesiones.Checked = false;
            if (posesiones.BackColor == Color.Transparent && _stats.Count < 3)
            {
                posesiones.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.posesiones);
            }
            else if (posesiones.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.posesiones);
                posesiones.BackColor = Color.Transparent;
            }

            UpdateStats();
        }

        private void tirosPuerta_CheckedChanged(object sender, EventArgs e)
        {
            tirosPuerta.Checked = false;
            if (tirosPuerta.BackColor == Color.Transparent && _stats.Count < 3)
            {
                tirosPuerta.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.TirosAPuertaDentro);
            }
            else if (tirosPuerta.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.TirosAPuertaDentro);
                tirosPuerta.BackColor = Color.Transparent;
            }

            UpdateStats();
        }
        private void tirosFuera_CheckedChanged(object sender, EventArgs e)
        {
            tirosFuera.Checked = false;
            if (tirosFuera.BackColor == Color.Transparent && _stats.Count < 3)
            {
                tirosFuera.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.TirosAPuerta);
            }
            else if (tirosFuera.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.TirosAPuerta);
                tirosFuera.BackColor = Color.Transparent;
            }

            UpdateStats();
        }
        private void paradas2_CheckedChanged(object sender, EventArgs e)
        {
            paradas.Checked = false;
            if (paradas.BackColor == Color.Transparent && _stats.Count < 3)
            {
                paradas.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.Paradas);
            }
            else if (paradas.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.Paradas);
                paradas.BackColor = Color.Transparent;
            }

            UpdateStats();
        }
        private void amarillas_CheckedChanged(object sender, EventArgs e)
        {
            amarillas.Checked = false;
            if (amarillas.BackColor == Color.Transparent && _stats.Count < 3)
            {
                amarillas.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.TAmarillas);
            }
            else if (amarillas.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.TAmarillas);
                amarillas.BackColor = Color.Transparent;
            }

            UpdateStats();
        }
        private void rojas_CheckedChanged(object sender, EventArgs e)
        {
            rojas.Checked = false;
            if (rojas.BackColor == Color.Transparent && _stats.Count < 3)
            {
                rojas.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.TAmarillas);
            }
            else if (rojas.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.TAmarillas);
                rojas.BackColor = Color.Transparent;
            }

            UpdateStats();
        }
        private void fuerasJuego_CheckedChanged(object sender, EventArgs e)
        {
            fuerasJuego.Checked = false;
            if (fuerasJuego.BackColor == Color.Transparent && _stats.Count < 3)
            {
                fuerasJuego.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.FuerasDeJuego);
            }
            else if (fuerasJuego.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.FuerasDeJuego);
                fuerasJuego.BackColor = Color.Transparent;
            }

            UpdateStats();
        }

        private void buttonCrono_Click(object sender, EventArgs e)
        {
     
            configMenu(panelOpcionesCrono);
            _controlador.SetSeccion(Controlador.Crono);
        }

        private void clear_Click(object sender, EventArgs e)
        {
            configMenu(null);

            // Desactiva todas las opciones principales
            foreach (Control c in panelOpcionesMain.Controls)
            {
                ((RadioButton)c).Checked = false;
            }

            _controlador.SetSeccion(Controlador.Clear);
            _controlador.OnAirClear();
        }
        #endregion

        // ############################# LOGO BACKGROUND #############################
        #region Dibujado del logo Virtualia

        // Dibujado del fondo Panel raiz
        private void tableLayoutPanelRoot_Paint(object sender, PaintEventArgs e)
        {
            int col_w = tableLayoutPanelRoot.Width - panelOpcionesMain.Width - tableLayoutPanelCol3.Width - tableLayoutPanelCol4.Width;
            int col_h = tableLayoutPanelRoot.Height;

            Image img = generar_fondoVert(col_w, col_h);

            int x = panelOpcionesMain.Width;
            e.Graphics.DrawImageUnscaled(img, x, 0);
        }

        // Variables para el buffer de dibujado
        private Image _fondo_buf = null;
        private int _col_w_buf = 0;
        private int _col_h_buf = 0;

        // Genera la imagen de fondo creando un mosaico en el que se repite una imagen N veces
        private Image generar_fondo(int col_w, int col_h)
        {
            const int NUM_IMG = 5;

            if (_fondo_buf == null || _col_w_buf != col_w || _col_h_buf != col_h)
            {
                try
                {
                    Bitmap bmp = new Bitmap(col_w, col_h);
                    Graphics g = Graphics.FromImage((Image)bmp);

                    Image logo = resizeImage(Properties.Resources.logo_bg, col_w);

                    int pad_h = (col_h - NUM_IMG * logo.Height) / (NUM_IMG - 1) + logo.Height;

                    for (int i = 0; i < NUM_IMG; i++)
                    {
                        g.DrawImageUnscaled(logo, 0, i * pad_h);
                    }

                    g.Dispose();
                    _fondo_buf = (Image)bmp;
                    _col_w_buf = col_w;
                    _col_h_buf = col_h;
                }
                catch { }
            }

            return _fondo_buf;
        }

        // Genera la imagen de fondo con un logo
        private Image generar_fondoVert(int col_w, int col_h)
        {
            if (_fondo_buf == null || _col_w_buf != col_w || _col_h_buf != col_h)
            {
                try
                {
                    Bitmap bmp = new Bitmap(col_w, col_h);
                    Graphics g = Graphics.FromImage((Image)bmp);

                    Image logo = resizeImage(Properties.Resources.logo_bg_v, col_w);
                   
                    int pad_h = (col_h - logo.Height) / 2;

                    g.DrawImageUnscaled(logo, 0, pad_h);

                    g.Dispose();
                    _fondo_buf = (Image)bmp;
                    _col_w_buf = col_w;
                    _col_h_buf = col_h;
                }
                catch { }
            }

            return _fondo_buf;
        }

        // Redimensiona la imagen para que tenga el ancho indicado y mantenga la relación alto/ancho
        private Image resizeImage(Image img, int resizedW)
        {
            //get the height and width of the image
            int originalW = img.Width;
            int originalH = img.Height;

            //get the new size based on the percentage change
            int resizedH = (int)(originalH * resizedW / originalW);

            //create a new Bitmap the size of the new image
            Bitmap bmp = new Bitmap(resizedW, resizedH);
            //create a new graphic from the Bitmap
            Graphics graphic = Graphics.FromImage((Image)bmp);

            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
            graphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;                 

            //draw the newly resized image
            graphic.DrawImage(img, 0, 0, resizedW, resizedH);
            //dispose and free up the resources
            graphic.Dispose();
            //return the image
            return (Image)bmp;
        }

        #endregion

        private void pases_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesPases);
            //_controlador.SetSeccion(Controlador.Pases);
        }

        private void stats_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesStats);
            _controlador.SetSeccion(Controlador.Stats);
        }

        private void estadisticas_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesEstadisticas);
            UpdateStats();
            _controlador.SetSeccion(Controlador.Estadisticas);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == false)
                Program.CambioEscogido = -1;
        }

        private void checkBoxEqLocalCrawl_Click(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUpCrawl(true);
        }

        private void checkBoxEqVisitCrawl_Click(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUpCrawl(false);
        }
      
        public void AbrirRotulosJugador()
        {
            this.checkBox3.PerformClick();
        }           
    }
}
