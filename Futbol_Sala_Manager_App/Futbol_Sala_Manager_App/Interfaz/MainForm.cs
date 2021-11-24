using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Futbol_Sala_Manager_App.Beans;
using Futbol_Sala_Manager_App.Comandos;
using Futbol_Sala_Manager_App.Persistencia;


namespace Futbol_Sala_Manager_App.Interfaz
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
        private int _stat1;
        private int _stat2;
        private int _stat3;
        private int _estord1 = 0; private int _estord2 = 0; private int _estord3 = 0; private int _estord4 = 0; private int _estord5= 0; private int _estord6 = 0;
        private int _estord7 = 0; private int _estord8 = 0; private int _estord9 = 0;
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
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesPresentacion, 1, 0);
            this.panelOpcionesPresentacion.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesRotulos, 1, 0);
            this.panelOpcionesRotulos.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesCrono, 1, 0);
            this.panelOpcionesCrono.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesEstadisticas, 1, 0);
            this.panelOpcionesEstadisticas.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesGoles, 1, 0);
            this.panelOpcionesGoles.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesTiros, 1, 0);
            this.panelOpcionesTiros.Hide();                       

            // Penaltis
            this.tableLayoutPanelCol3.Controls.Add(this.penaltis, 0, 2);
            this.penaltis.Hide();

            // Lista de eventos
            this.tableLayoutPanelCol3.Controls.Add(this.listBox, 0, 2);

            _config = PersistenciaUtil.CargaConfig();

            if (_config.isMachineCrono)
            {
                Console.WriteLine("################ MODO MANUAL CRONÓMETRO ################");
                AsynchronousClient.StartClient(int.Parse(_config.port), _config.ipAddress);
                this.FormClosed += new FormClosedEventHandler((o, e) => { AsynchronousClient.CloseClient(); });
            }
            else if (_config.modeOcrActivated)
            {
                Console.WriteLine("################ MODO OCR ACTIVO ################");
            }
            else if(_config.isMachineCrono == false)
            {
                Console.WriteLine("################ MODO MANUAL PRINCIPAL ################");
                AsynchronousSocketListener.StartListening(int.Parse(_config.puertoEscuchaMaquinaPrincipal), _config.ipAddress);
                this.FormClosed += new FormClosedEventHandler((o, e) => { AsynchronousSocketListener.StopListening(); });
            }
            CheckForIllegalCrossThreadCalls = false;


            //this.KeyPreview = _config.isMachineCrono; // El teclado solo tiene acceso si es el usuario cronómetro
            this.KeyPreview = true; // El teclado esta activo en los dos usuarios (cronometro y principal)

            // Suscripción a los eventos del cliente
            AsynchronousSocketListener.onDataReceived += PosesionClientDataReceived;

            //Carga la configuración del tiempo incial seleccionado en ajustes
            this.numericUpDown_minutes.Value = int.Parse(_config.duracionParte_i);
            this.checkBoxCrono.Text = this.numericUpDown_minutes.Value.ToString("00") + "'" + this.numericUpDown_seconds.Value.ToString("00") + "''";

            //Deshabilitamos controles que no son necesarios si se arranca la aplicación en modo cronómetro
            if (_config.isMachineCrono)
            {
                this.checkBoxConexion.Enabled = false;
                this.checkBoxConexion.BackColor = Color.Gray;
                this.checkBox2.Enabled = false;
                this.checkBox3.Enabled = false;
                this.checkBox4.Enabled = false;
                this.checkBox5.Enabled = false;
                this.checkBox6.Enabled = false;
                this.checkBox7.Enabled = false;
                this.checkBox8.Enabled = false;
                this.checkBox9.Enabled = false;
                this.checkBox10.Enabled = false;
                this.checkBox13.Enabled = false;
                this.posesionStop.Enabled = false;
            }
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
            /*
            if (j.Posicion == Jugador.Portero)
            {
                colorPos = System.Drawing.Color.WhiteSmoke;
            }
            else if (j.Posicion == Jugador.Cierre)
            {
                colorPos = System.Drawing.Color.LightGray;
            }
            else if (j.Posicion == Jugador.Ala)
            {
                colorPos = System.Drawing.Color.DarkGray;
            }
            else // Delantero
            { 
                colorPos = System.Drawing.Color.DimGray;
            }
             */

            colorPos = Color.LightGray;
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
                panel = (eqLocal ? this.panelJugadoresEq1 : this.panelJugadoresEq2);
            }

            // Listener
            controlNombre.Click += delegate { _controlador.ClickOnJugador( j,controlNombre ); };

            panel.Controls.Add(controlNumero);
            panel.Controls.Add(controlNombre);
        }

        private void configMenu(TableLayoutPanel menu)
        {            
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
            //panelJugadoresEq1.RowCount = equipo.Banquillo.Count + equipo.Jugadores.Count;
            panelJugadoresEq1.RowCount = equipo.Jugadores.Count;
            this.checkBoxEqL.Text = equipo.TeamCode;

            this.checkBoxEntrenadorL.Text = equipo.Entrenador.ShortName;
            this.checkBoxEntrenadorL.Click += delegate { _controlador.ClickOnEntrenador(equipo.Entrenador); };
            this.checkBoxAsistenteL.Text = equipo.Asistente.ShortName;
            this.checkBoxAsistenteL.Click += delegate { _controlador.ClickOnAsistente(equipo.Asistente); };

            List<Jugador> players =new List<Jugador>();
            panelJugadoresEq1.Controls.Clear();

            equipo.Banquillo.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Banquillo)
            {
                players.Add(j);
                //addJugador(true, true, j);
            }

            equipo.Jugadores.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Jugadores)
            {
                players.Add(j);
                // addJugador(true, true, j);
            }
            players.Sort(new JugadorComparerGUI());

            foreach (Jugador j in players)
            {              
               addJugador(true, true, j);               
            }

            _colorL = equipo.Color1;
        }
        public void ConfigEquipoVisitante(Equipo equipo)
        {
            //panelJugadoresEq2.RowCount = equipo.Banquillo.Count + equipo.Jugadores.Count;
            panelJugadoresEq2.RowCount = equipo.Jugadores.Count;
            this.checkBoxEqV.Text = equipo.TeamCode;

            this.checkBoxEntrenadorV.Text = equipo.Entrenador.ShortName;
            this.checkBoxEntrenadorV.Click += delegate { _controlador.ClickOnEntrenador(equipo.Entrenador); };
            this.checkBoxAsistenteV.Text = equipo.Asistente.ShortName;
            this.checkBoxAsistenteV.Click += delegate { _controlador.ClickOnAsistente(equipo.Asistente); };

            List<Jugador> players = new List<Jugador>();
            panelJugadoresEq2.Controls.Clear();

            equipo.Banquillo.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Banquillo)
            {
                players.Add(j);
            }

            equipo.Jugadores.Sort(new JugadorComparerGUI());
            foreach (Jugador j in equipo.Jugadores)
            {
                players.Add(j);
            }
            
            players.Sort(new JugadorComparerGUI());
            foreach (Jugador j in players)
            {
                addJugador(false, true, j);
            }

            _colorV = equipo.Color1;
        }

        public void ActivaTabJugadores(bool titulares)
        {
            //this.tabControlPlantilla.SelectedTab = this.tabPageJugadores;
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
        delegate void SetTextCallback(string text, string minutes, string seconds);
        public void SetCronoTextAsync(string text, string minutes, string seconds)
        {
            try
            {
                // InvokeRequired required compares the thread ID of the
                // calling thread to the thread ID of the creating thread.
                // If these threads are different, it returns true.
                if (this.checkBoxCrono.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetCronoText);
                    this.Invoke(d, new object[] { text, minutes, seconds });
                }
                else
                {
                    this.SetCronoText(text, minutes, seconds);
                }
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }
        private void SetCronoText(string text, string minutes, string seconds)
        {
            this.checkBoxCrono.Text = text;
            if (!numericUpDown_minutes.Focused && !numericUpDown_seconds.Focused)
            {
                this.numericUpDown_minutes.Text = minutes;
                this.numericUpDown_seconds.Text = seconds;
            }
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
               // controles = new Control[panelJugadoresEq1.Controls.Count + panelBanquilloEq1.Controls.Count];
                controles = new Control[panelJugadoresEq1.Controls.Count ];

                panelJugadoresEq1.Controls.CopyTo(controles, 0);
                //panelBanquilloEq1.Controls.CopyTo(controles, panelJugadoresEq1.Controls.Count);
            }
            else // eqVisitante
            {
                //controles = new Control[panelJugadoresEq2.Controls.Count + panelBanquilloEq2.Controls.Count];
                controles = new Control[panelJugadoresEq2.Controls.Count ];

                panelJugadoresEq2.Controls.CopyTo(controles, 0);
                //panelBanquilloEq2.Controls.CopyTo(controles, panelJugadoresEq2.Controls.Count);
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
                //controles = new Control[panelJugadoresEq1.Controls.Count + panelBanquilloEq1.Controls.Count];
                controles = new Control[panelJugadoresEq1.Controls.Count ];

                panelJugadoresEq1.Controls.CopyTo(controles, 0);
                //panelBanquilloEq1.Controls.CopyTo(controles, panelJugadoresEq1.Controls.Count);
            }
            else // eqVisitante
            {
                //controles = new Control[panelJugadoresEq2.Controls.Count + panelBanquilloEq2.Controls.Count];
                controles = new Control[panelJugadoresEq2.Controls.Count ];

                panelJugadoresEq2.Controls.CopyTo(controles, 0);
                //panelBanquilloEq2.Controls.CopyTo(controles, panelJugadoresEq2.Controls.Count);
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
        private void activaControl(Control control, bool activo, bool eqLocal, bool posesion = false)
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
                    control.BackColor = posesion ? Color.FromArgb(0, Color.White) : _colorL;
                }
                else
                {
                    control.BackColor = posesion ? Color.FromArgb(0, Color.White) : _colorV;
                }
            }
        }



        // Pinta los botones de la alineación del color correspondiente
        private void pinta_controles_alineaciones()
        {
            checkBoxEqLocal.BackColor = _colorL;
            checkBoxDispLocal.BackColor = _colorL;
            checkBoxEqLocalCrawl.BackColor = _colorL;

            checkBoxEqVisit.BackColor = _colorV;
            checkBoxDispVisit.BackColor = _colorV;
            checkBoxEqVisitCrawl.BackColor = _colorV;
        }


        /**
         * Activa o desactiva los controles de la posesión
         */
        public void ActivaOpcionesPosesion(bool activo)
        {
            activaControl(posesionL, activo, true, true);
            activaControl(posesionV, activo, false, true);

            this.posesionStop.Checked = true;
        }

        /**
         * Activa o desactiva las subsecciones relativas al Crono
         */
        public void ActivaOpcionesCrono(bool kickoff, bool finParte, bool finPartido)
        {
            this.cronoParar.Enabled = !kickoff;
            if (!this.cronoParar.Enabled)
                tableLayout_Crono.Focus();
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

        // ######################## PORTERO JUGADOR #############################
        #region Portero Jugador

        private void checkBoxPorteroJugadorL_Click(object sender, EventArgs e)
        {
            //if (checkBoxPorteroJugadorL.BackColor == Color.Transparent || checkBoxPorteroJugadorL.BackColor == Color.Coral)
            //{
            bool pj_activo =_controlador.PorteroJugador(true);
            if (pj_activo)
                checkBoxPorteroJugadorL.BackColor = Color.LightGreen;
            else
                checkBoxPorteroJugadorL.BackColor = Color.Transparent;

            //}
            //else
            //{
            //    checkBoxPorteroJugadorL.BackColor = Color.Transparent;
            //    _controlador.PorteroJugador(true, false);
            //}
        }

        private void checkBoxPorteroJugadorV_Click(object sender, EventArgs e)
        {
            bool pj_activo = _controlador.PorteroJugador(false);
            if (pj_activo)
                checkBoxPorteroJugadorV.BackColor = Color.LightGreen;
            else
                checkBoxPorteroJugadorV.BackColor = Color.Transparent;

            //if (checkBoxPorteroJugadorV.BackColor == Color.Transparent || checkBoxPorteroJugadorV.BackColor == Color.Coral)
            //{
            //checkBoxPorteroJugadorV.BackColor = Color.LightGreen;
            //_controlador.PorteroJugador(false, true);
            //}
            //else
            //{
            //    checkBoxPorteroJugadorV.BackColor = Color.Transparent;
            //    _controlador.PorteroJugador(false, false);
            //}
        }
        
        #endregion

        // ################################ POSESION #################################
        #region Botones de Posesion

        private void posesionL_CheckedChanged(object sender, EventArgs e)
        {
            PosesionLocal();
        }

        void PosesionLocal()
        {
            if (posesionL.Checked)
            {
                _controlador.PosesionStart(true);
            }
            posesionL.BackColor = posesionL.Checked ? _colorL : Color.FromArgb(0, Color.White);
            // Cambia la imagen enm función del estado
            //this.posesionL.Image = (posesionL.Checked ?
            //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaB_ON :
            //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaB_OFF);


            //this.posesionL.Image = (posesionL.Checked ?
            //    Properties.Resources.BalonBotaB_ON :
            //    Properties.Resources.BalonBotaB_OFF);

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("PosesionLocal");
            }
        }

        private void posesionV_CheckedChanged(object sender, EventArgs e)
        {
            PosesionVisitante();
        }

        void PosesionVisitante()
        {
            if (posesionV.Checked)
            {
                _controlador.PosesionStart(false);
            }

            posesionV.BackColor = posesionV.Checked ? _colorV : Color.FromArgb(0, Color.White);
            //this.posesionV.Image = (posesionV.Checked ?
            //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaA_ON :
            //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaA_OFF);
            // Cambia la imagen enm función del estado
            //this.posesionV.Image = (posesionV.Checked ?
            //    Properties.Resources.BalonBotaA_ON :
            //    Properties.Resources.BalonBotaA_OFF);

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("PosesionVisitante");
            }
        }

        private void posesionStop_Click(object sender, EventArgs e)
        {
            // Solo realiza la acción si el evento viene de un click del usuario
            if (e is MouseEventArgs)
            {
                PosesionStopClick(); // Solucionado bug de pulsar dos veces el botón de mostrar posesión
                _controlador.previewPosesion();
            }
        }

        void PosesionStopClick()
        {
            _controlador.PosesionStop();

            if (AsynchronousClient.isClient)
            {
                AsynchronousClient.Send("PosesionStop");
            }
        }

        void PosesionStop()
        {
            //this.posesionStop.Image = (posesionStop.Checked ?
            //    Futbol_Sala_Manager_App.Properties.Resources.Balon_ON :
            //    Futbol_Sala_Manager_App.Properties.Resources.Balon_OFF);

            // Cambia la imagen enm función del estado
            //this.posesionStop.Image = (posesionStop.Checked ?
            //    Properties.Resources.Balon_ON :
            //    Properties.Resources.Balon_OFF);
        }

        private void posesionStop_CheckedChanged(object sender, EventArgs e)
        {
            PosesionStop();
        }

        void PosesionClientDataReceived(string data)
        {
            //Console.WriteLine("***Ha llegado " + data + "***");
            if (data.Contains("PosesionLocal") && !posesionL.Checked)
            {
                posesionL.Checked = true;
                //_controlador.PosesionStart(true);
                //this.posesionL.Image = Properties.Resources.BalonBotaB_ON;
                //this.posesionStop.Image = Properties.Resources.Balon_OFF;
                //this.posesionV.Image = Properties.Resources.BalonBotaA_OFF;
            }
            else if (data.Contains("PosesionVisitante") && !posesionV.Checked)
            {
                posesionV.Checked = true;
                //_controlador.PosesionStart(false);
                //this.posesionL.Image = Properties.Resources.BalonBotaB_OFF;
                //this.posesionStop.Image = Properties.Resources.Balon_OFF;
                //this.posesionV.Image = Properties.Resources.BalonBotaA_ON;
            }
            else if (data.Contains("PosesionStop") && !posesionStop.Checked)
            {
                posesionStop.Checked = true;
                //_controlador.PosesionStop();
                //this.posesionL.Image = Properties.Resources.BalonBotaB_OFF;
                //this.posesionStop.Image = Properties.Resources.Balon_ON;
                //this.posesionV.Image = Properties.Resources.BalonBotaA_OFF;
            }
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

        private void localizador_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Localizador);
        }

        private void perfilJugador_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.PerfilJugador);
        }

        private void clasificacion_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Clasificacion);
        }

        private void enfrentamientoJornada_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Enfrentamientos);
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
        private void Rachas_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Rachas);
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

        private void NewsExchangeFeed_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.NewsExchangeFeed);
        }

        private void EditEstadisticas_CheckedChanged(object sender, EventArgs e)
        {
            EditEstadisticasForm eef = new EditEstadisticasForm(_controlador._datos);
            eef.ShowDialog();
        }

        private void cronoKickoff_Click(object sender, EventArgs e)
        {
            _controlador.CronoEmpezar();
        }

        private void cronoParar_Click(object sender, EventArgs e)
        {
            _controlador.CronoParar();
        }

        private void cronoFinParte_Click(object sender, EventArgs e)
        {
            string message = "¿Está seguro que quiere finalizar?";
            string caption = "Información";
		    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
		    DialogResult result;
            result = MessageBox.Show(message, caption, buttons);

		    if (result == DialogResult.Yes)
                _controlador.CronoFinParte();
        }

        private void cronoFinPartido_Click(object sender, EventArgs e)
        {
            string message = "¿Está seguro que quiere finalizar?";
            string caption = "Información";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);

            if (result == DialogResult.Yes)
                _controlador.CronoFinPartido();
        }

        private void cronoReset_Click(object sender, EventArgs e)
        {
            _controlador.CronoReset();
        }
               
        private void cronoConfig_Click(object sender, EventArgs e)
        {
            _controlador.CronoEdit();
        }

        private System.Timers.Timer cronoTimer;
        private void cronoEdit_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_minutes.Focused || numericUpDown_seconds.Focused)
            {
                if(cronoTimer != null)
                {
                    cronoTimer.Stop();
                    cronoTimer.Dispose();
                }

                cronoTimer = new System.Timers.Timer(500);
                cronoTimer.Enabled = true;
                cronoTimer.AutoReset = false;
                cronoTimer.Elapsed += EditCrono;
            }
        }

        void EditCrono(Object source, System.Timers.ElapsedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate { tableLayout_Crono.Focus(); });
            if (_config.modeOcrActivated) 
            {
                // Si se ha elegido arrancar en modo OCR la aplicación, al modificar manualmente lo valores numéricos del crono se los asigna a los campos
                // estáticos de minutos y segundos de la clase Ocr para luego usarlos para actualizar la UI y mandarle info al IPF
                Ocr.minute = (int) numericUpDown_minutes.Value;
                Ocr.second = (int)numericUpDown_seconds.Value;
            }
            _controlador.CronoEdit(numericUpDown_minutes.Value.ToString(), numericUpDown_seconds.Value.ToString());
        }

        private void cronoEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void conexion_CheckedChanged(object sender, EventArgs e)
        {
            bool on = ((CheckBox)sender).Checked;
            int numIpfs = _config.NumIpf;
            if (on)
            {
                SeleccionIpf seleccion = new SeleccionIpf(_config.NumIpf);
                seleccion.ShowDialog();

                if (Program.CancelarConexion == false)
                {
                    if (on)
                    {
                        _config.isMachineCrono = false;
                        AsynchronousClient.isClient = false;
                        if (AsynchronousClient.clientStarted)
                            AsynchronousClient.CloseClient();
                        _controlador.EstablecerConexionIpf();

                    }
                    else
                    {
                        _controlador.CerrarConexionIpf();
                        _config.isMachineCrono = true;
                        AsynchronousClient.StartClient(int.Parse(_config.port), _config.ipAddress);
                        this.FormClosed += new FormClosedEventHandler((o, ev) => { AsynchronousClient.CloseClient(); });
                    }
                }
            }
            else
            {
                _controlador.CerrarConexionIpf();
                if (_config.wasStardedMachineCrono)
                {
                    _config.isMachineCrono = true;
                    if (AsynchronousClient.clientStarted == false)
                    {
                        AsynchronousClient.StartClient(int.Parse(_config.port), _config.ipAddress);
                        this.FormClosed += new FormClosedEventHandler((o, ev) => { AsynchronousClient.CloseClient(); });
                    }
                }
            }
            Program.CancelarConexion = false;
        }

        #endregion

        // ################################ SECCIONES ################################
        #region Botones de Secciones
                   
        private void presentacion_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesPresentacion);
            _controlador.SetSeccion(Controlador.Presentacion);

            pinta_controles_alineaciones();
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
            configMenu(null);
            _controlador.SetSeccion(Controlador.Faltas);
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

        //private void fueraJuego_CheckedChanged(object sender, EventArgs e)
        //{
        //    configMenu(null);
        //    _controlador.SetSeccion(Controlador.FuerasDeJuego);
        //}

        //private void cambio_CheckedChanged(object sender, EventArgs e)
        //{
        //    configMenu(panelOpcionesCambios);
        //    _controlador.SetSeccion(Controlador.Cambio);

        //    Program.CambioEscogido = -1;
        //}

        //private void cambioDoble_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.SetSeccion(Controlador.CambioDoble);
        //    Program.CambioEscogido = -1;
        //}
        //private void cambioInfo_CheckedChanged(object sender, EventArgs e)
        //{
        //    Program.CambioEscogido = 204;

        //    _controlador.SetSeccion(Controlador.Cambio);

        //    ICommandShowable comando = _controlador.GetComandoOnAir();

        //    if (comando!=null)
        //        Console.WriteLine(_controlador.GetComandoOnAir());          
        //}
        //private void cambioIn_CheckedChanged(object sender, EventArgs e)
        //{
        //    Program.CambioEscogido = 200;
        //    _controlador.SetSeccion(Controlador.Cambio);
        //}
        //private void cambioOut_CheckedChanged(object sender, EventArgs e)
        //{
        //    Program.CambioEscogido = 201;
        //    _controlador.SetSeccion(Controlador.Cambio);
        //}
        //private void cambioInInfo_CheckedChanged(object sender, EventArgs e)
        //{
        //    Program.CambioEscogido = 202;
        //    _controlador.SetSeccion(Controlador.Cambio);
        //}
        //private void cambioOutInfo_CheckedChanged(object sender, EventArgs e)
        //{
        //    Program.CambioEscogido = 203;
        //    _controlador.SetSeccion(Controlador.Cambio);
        //}

        //public int CapturaCambio()
        //{
        //    int n = 0;
        //    if (cambioIn.Checked)
        //        n = 200;
        //    if (cambioOut.Checked)
        //        n = 201;
        //    if (cambioInInfo.Checked)
        //        n = 202;
        //    if (cambioOutInfo.Checked)
        //        n = 203;
        //    if (cambioInfo.Checked)
        //        n = 204;
        //    return n;                    
        //}

        private int EstadisticasSeleccionadas()
        {
            int n = 0;

            if (posesiones.BackColor == Color.LightSkyBlue) n++;
            if (corners.BackColor == Color.LightSkyBlue) n++;
            if (faltas.BackColor == Color.LightSkyBlue) n++;
            if (tirosPuerta.BackColor == Color.LightSkyBlue) n++;
            if (tirosFuera.BackColor == Color.LightSkyBlue) n++;
            if (paradas.BackColor == Color.LightSkyBlue) n++;
            if (amarillas.BackColor == Color.LightSkyBlue) n++;
            if (rojas.BackColor == Color.LightSkyBlue) n++;
            //if (fuerasJuego.BackColor == Color.LightSkyBlue) n++;
  
            return n;
        }

        private void CapturaEstadisticas()
        {
            bool e1=false;
            bool e2=false;
            bool e3=false;

            if (corners.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.Corners;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.Corners;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.Corners;
                            e3 = true;
                        }
            }

            if (faltas.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.Faltas;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.Faltas;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.Faltas;
                            e3 = true;
                        }
            }
            
            if (tirosPuerta.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.TirosAPuertaDentro;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.TirosAPuertaDentro;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.TirosAPuertaDentro;
                            e3 = true;
                        }
            }

            if (tirosFuera.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.TirosAPuerta;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.TirosAPuerta;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.TirosAPuerta;
                            e3 = true;
                        }
            }

            if (paradas.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.Paradas;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.Paradas;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.Paradas;
                            e3 = true;
                        }
            }

            if (amarillas.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.TAmarillas;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.TAmarillas;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.TAmarillas;
                            e3 = true;
                        }
            }

            if (rojas.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.TRojas;
                    e1 = true;
                }
                else
                    if (e2 == false)
                    {
                        _stat2 = EstadisticasCommand.TRojas;
                        e2 = true;
                    }
                    else
                        if (e3 == false)
                        {
                            _stat3 = EstadisticasCommand.TRojas;
                            e3 = true;
                        }
            }

            //if (fuerasJuego.BackColor == Color.LightSkyBlue)
            //{
            //    if (e1 == false)
            //    {
            //        _stat1 = EstadisticasCommand.FuerasDeJuego;
            //        e1 = true;
            //    }
            //    else
            //        if (e2 == false)
            //    {
            //        _stat2 = EstadisticasCommand.FuerasDeJuego;
            //        e2 = true;
            //    }
            //    else
            //            if (e3 == false)
            //    {
            //        _stat3 = EstadisticasCommand.FuerasDeJuego;
            //        e3 = true;
            //    }
            //}

            if (posesiones.BackColor == Color.LightSkyBlue)
            {
                if (e1 == false)
                {
                    _stat1 = EstadisticasCommand.posesiones;
                    e1 = true;
                }
                else
                    if (e2 == false)
                {
                    _stat2 = EstadisticasCommand.posesiones;
                    e2 = true;
                }
                else
                        if (e3 == false)
                {
                    _stat3 = EstadisticasCommand.posesiones;
                    e3 = true;
                }
            }
        }


        //Cuando se elimina el primero
        private void reordenar1()
        {
            if (_estord1 == 2) _estord1 = 1; if (_estord1 == 3) _estord1 = 2;
            if (_estord2 == 2) _estord2 = 1; if (_estord2 == 3) _estord2 = 2;
            if (_estord3 == 2) _estord3 = 1; if (_estord3 == 3) _estord3 = 2;
            if (_estord4 == 2) _estord4 = 1; if (_estord4 == 3) _estord4 = 2;
            if (_estord5 == 2) _estord5 = 1; if (_estord5 == 3) _estord5 = 2;
            if (_estord6 == 2) _estord6 = 1; if (_estord6 == 3) _estord6 = 2;
            if (_estord7 == 2) _estord7 = 1; if (_estord7 == 3) _estord7 = 2;
            if (_estord8 == 2) _estord8 = 1; if (_estord8 == 3) _estord8 = 2;
            if (_estord9 == 2) _estord9 = 1; if (_estord9 == 3) _estord9 = 2;
        }

        //Cuando se elimina el segundo
        private void reordenar2()
        {
            if (_estord1 == 3) _estord1 = 2; 
            if (_estord2 == 3) _estord2 = 2; 
            if (_estord3 == 3) _estord3 = 2; 
            if (_estord4 == 3) _estord4 = 2; 
            if (_estord5 == 3) _estord5 = 2; 
            if (_estord6 == 3) _estord6 = 2; 
            if (_estord7 == 3) _estord7 = 2; 
            if (_estord8 == 3) _estord8 = 2;
            if (_estord9 == 3) _estord9 = 2; 
        }

        private void OrdenarEstadisticasVariasLineas()
        {
            if (_estord1 == 1) _stat1 = 3; if (_estord1 == 2) _stat2 = 3; if (_estord1 == 3) _stat3 = 3;
            if (_estord2 == 1) _stat1 = 4; if (_estord2 == 2) _stat2 = 4; if (_estord2 == 3) _stat3 = 4;
            if (_estord3 == 1) _stat1 = 61; if (_estord3 == 2) _stat2 = 61; if (_estord3 == 3) _stat3 = 61;
            if (_estord4 == 1) _stat1 = 6; if (_estord4 == 2) _stat2 = 6; if (_estord4 == 3) _stat3 = 6;
            if (_estord5 == 1) _stat1 = 7; if (_estord5 == 2) _stat2 = 7; if (_estord5 == 3) _stat3 = 7;
            if (_estord6 == 1) _stat1 = 8; if (_estord6 == 2) _stat2 = 8; if (_estord6 == 3) _stat3 = 8;
            if (_estord7 == 1) _stat1 = 9; if (_estord7 == 2) _stat2 = 9; if (_estord7 == 3) _stat3 = 9;
            if (_estord8 == 1) _stat1 = 10; if (_estord8 == 2) _stat2 = 10; if (_estord8 == 3) _stat3 = 10;
            if (_estord9 == 1) _stat1 = 64; if (_estord9 == 2) _stat2 = 64; if (_estord9 == 3) _stat3 = 64;
        }

        private void corners_CheckedChanged(object sender, EventArgs e)
        {
            corners.Checked = false;
            if (corners.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                corners.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord1 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord1 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord1 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas()==2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV,_controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                
            }
            else
            {
                corners.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord1 == 1)
                {
                    _estord1 = 0;
                    reordenar1();
                }
                if (_estord1 == 2)
                {
                    _estord1 = 0;
                    reordenar2();
                }
                if (_estord1 == 3)
                    _estord1 = 0;

                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }
        private void faltas_CheckedChanged(object sender, EventArgs e)
        {
            faltas.Checked = false;
            if (faltas.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                faltas.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord2 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord2 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord2 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                faltas.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord2 == 1)
                {
                    _estord2 = 0;
                    reordenar1();
                }
                if (_estord2 == 2)
                {
                    _estord2 = 0;
                    reordenar2();
                }
                if (_estord2 == 3)
                    _estord2 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }

        public int SumaOrdenEstadisticasSeleccionadas()
        {
            return _estord1 + _estord2 + _estord3 + _estord4 + _estord5 + _estord6 + _estord7 + _estord8 + _estord9;
        }

        private void posesiones_CheckedChanged(object sender, EventArgs e)
        {
            posesiones.Checked = false;
            if (posesiones.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                posesiones.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord9 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord9 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord9 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                posesiones.BackColor = Color.Transparent;
                CapturaEstadisticas();

                if (_estord9 == 1)
                {
                    _estord9 = 0;
                    reordenar1();
                }
                if (_estord9 == 2)
                {
                    _estord9 = 0;
                    reordenar2();
                }
                if (_estord9 == 3)
                    _estord9 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }

        private void tirosPuerta_CheckedChanged(object sender, EventArgs e)
        {
            tirosPuerta.Checked = false;
            if (tirosPuerta.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                tirosPuerta.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord3 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord3 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord3 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                tirosPuerta.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord3 == 1)
                {
                    _estord3 = 0;
                    reordenar1();
                }
                if (_estord3 == 2)
                {
                    _estord3 = 0;
                    reordenar2();
                }
                if (_estord3 == 3)
                    _estord3 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }
        private void tirosFuera_CheckedChanged(object sender, EventArgs e)
        {
            tirosFuera.Checked = false;
            if (tirosFuera.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                tirosFuera.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord4 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord4 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord4 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                tirosFuera.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord4 == 1)
                {
                    _estord4 = 0;
                    reordenar1();
                }
                if (_estord4 == 2)
                {
                    _estord4= 0;
                    reordenar2();
                }
                if (_estord4 == 3)
                    _estord4 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }
        private void paradas2_CheckedChanged(object sender, EventArgs e)
        {
            paradas.Checked = false;
            if (paradas.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                paradas.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord5 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord5 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord5 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                paradas.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord5 == 1)
                {
                    _estord5 = 0;
                    reordenar1();
                }
                if (_estord5 == 2)
                {
                    _estord5 = 0;
                    reordenar2();
                }
                if (_estord5 == 3)
                    _estord5 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }
        private void amarillas_CheckedChanged(object sender, EventArgs e)
        {
            amarillas.Checked = false;
            if (amarillas.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                amarillas.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord6 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord6 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord6 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                amarillas.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord6 == 1)
                {
                    _estord6 = 0;
                    reordenar1();
                }
                if (_estord6 == 2)
                {
                    _estord6 = 0;
                    reordenar2();
                }
                if (_estord6 == 3)
                    _estord6 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }
        private void rojas_CheckedChanged(object sender, EventArgs e)
        {
            rojas.Checked = false;
            if (rojas.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
            {
                rojas.BackColor = Color.LightSkyBlue;
                CapturaEstadisticas();

                switch (SumaOrdenEstadisticasSeleccionadas())
                {
                    case 0:
                        {
                            _estord7 = 1;
                            break;
                        }
                    case 1:
                        {
                            _estord7 = 2;
                            break;
                        }

                    case 3:
                        {
                            _estord7 = 3;
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
            else
            {
                rojas.BackColor = Color.Transparent;
                CapturaEstadisticas();
                if (_estord7 == 1)
                {
                    _estord7 = 0;
                    reordenar1();
                }
                if (_estord7 == 2)
                {
                    _estord7= 0;
                    reordenar2();
                }
                if (_estord7 == 3)
                    _estord7 = 0;
                OrdenarEstadisticasVariasLineas();
                if (EstadisticasSeleccionadas() == 2)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
                if (EstadisticasSeleccionadas() == 3)
                    _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            }
        }
        //private void fuerasJuego_CheckedChanged(object sender, EventArgs e)
        //{
        //    fuerasJuego.Checked = false;
        //    if (fuerasJuego.BackColor == Color.Transparent && EstadisticasSeleccionadas() < 3)
        //    {
        //        fuerasJuego.BackColor = Color.LightSkyBlue;
        //        CapturaEstadisticas();

        //        switch (SumaOrdenEstadisticasSeleccionadas())
        //        {
        //            case 0:
        //                {
        //                    _estord8 = 1;
        //                    break;
        //                }
        //            case 1:
        //                {
        //                    _estord8 = 2;
        //                    break;
        //                }

        //            case 3:
        //                {
        //                    _estord8= 3;
        //                    break;
        //                }

        //            default:
        //                {
        //                    break;
        //                }
        //        }
        //        OrdenarEstadisticasVariasLineas();
        //        if (EstadisticasSeleccionadas() == 2)
        //            _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
        //        if (EstadisticasSeleccionadas() == 3)
        //            _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
        //    }
        //    else
        //    {
        //        fuerasJuego.BackColor = Color.Transparent;
        //        CapturaEstadisticas();

        //        if (_estord8 == 1)
        //        {
        //            _estord8 = 0;
        //            reordenar1();
        //        }
        //        if (_estord8== 2)
        //        {
        //            _estord8 = 0;
        //            reordenar2();
        //        }
        //        if (_estord8 == 3)
        //            _estord8 = 0;
        //        OrdenarEstadisticasVariasLineas();
        //        if (EstadisticasSeleccionadas() == 2)
        //            _controlador.OnAirSetup(new EstadisticasCommand(_stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
        //        if (EstadisticasSeleccionadas() == 3)
        //            _controlador.OnAirSetup(new EstadisticasCommand(_stat3, _stat2, _stat1, _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
        //    }
        //}

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

        private void estadisticas_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.setEstadisticasSeleccionadas(EstadisticasSeleccionadas());
            configMenu(panelOpcionesEstadisticas);
            CapturaEstadisticas();
            _controlador.setEstadisticas(_stat3, _stat2, _stat1);
            _controlador.SetSeccion(Controlador.Estadisticas);


        }

        //private void checkBox12_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (checkBox12.Checked == false)
        //        Program.CambioEscogido = -1;


        //}

        private void checkBoxEqLocalCrawl_Click(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUpCrawl(true);
        }

        private void checkBoxEqVisitCrawl_Click(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUpCrawl(false);
        }

        private void TimeOutButton_Click(object sender, EventArgs e)
        {
            _controlador.TimeOut();
        }

        private void LocalTimeOutButton_Click(object sender, EventArgs e)
        {
            _controlador.LocalTimeOut();
        }

        private void VisitorTimeOutButton_Click(object sender, EventArgs e)
        {
            _controlador.VisitorTimeOut();
        }

        private void cronoEdit_TextChanged(object sender, EventArgs e)
        {

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.A))
                _controlador.CronoEmpezar();
            else if (e.KeyCode.Equals(Keys.S))
                _controlador.CronoParar();
            else if(e.KeyCode.Equals(Keys.J) && posesionL.Enabled)
            {
                posesionL.Checked = posesionL.Enabled;
                PosesionLocal();
            }
            else if (e.KeyCode.Equals(Keys.K) && posesionStop.Enabled)
            {
                posesionStop.Checked = posesionStop.Enabled;
                PosesionStopClick();
                PosesionStop();
            }
            else if (e.KeyCode.Equals(Keys.L) && posesionV.Enabled)
            {
                posesionV.Checked = posesionV.Enabled;
                PosesionVisitante();
            }
        }
            

        public void AbrirRotulosJugador()
        {
            this.checkBox3.PerformClick();
        }

        // Devuelve los datos de configuración asocidados a la instancia de este formulario
        // Se usa para ver desde la Clase crono si estamos ante un máquina controlador de IPF o solo Crono
        public ConfigData GetConfigData()
        {
            return _config;
        }

        public string GetNumericUpDown_minutesText()
        {
            return numericUpDown_minutes.Value.ToString();
        }

        public string GetNumericUpDown_secondsText()
        {
            return numericUpDown_seconds.Value.ToString();
        }

        public Controlador getControlador()
        {
            return _controlador;
        }
    }
}
