using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Balonmano_Manager_App.Beans;
using Balonmano_Manager_App.Comandos;
using Balonmano_Manager_App.Persistencia;


namespace Balonmano_Manager_App.Interfaz
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

        private bool escuchar_crono = true;
        private bool escuchar_exclusion = true;
        private bool escuchar_goles = false;
        private bool escuchar_timeOut = false;
        private bool escuchar_marcador = false;
        private bool escuchar_dorsales = false;



        /**
         * Constructor
         */
        public MainForm(Controlador controlador)
        {
            InitializeComponent();           

            _controlador = controlador;
            this.penaltis.BindControlador(controlador);
        
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesPresentacion, 1, 0);
            this.panelOpcionesPresentacion.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesRotulos, 1, 0);
            this.panelOpcionesRotulos.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesCrono, 1, 0);
            this.panelOpcionesCrono.Hide();            
          
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesGoles, 1, 0);
            this.panelOpcionesGoles.Hide();
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesTiros, 1, 0);
            this.panelOpcionesTiros.Hide();
            
            // Tarjetas
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesTarjetas, 1, 0);
            this.panelOpcionesTarjetas.Hide();

            // Tabla estadisticas
            this.tableLayoutPanelRoot.Controls.Add(this.panelOpcionesStats, 1, 0);
            this.panelOpcionesStats.Hide();

            // Penaltis
            this.tableLayoutPanelCol3.Controls.Add(this.penaltis, 0, 2);
            this.penaltis.Hide();

            // Lista de eventos
            this.tableLayoutPanelCol3.Controls.Add(this.listBox, 0, 2);

            _config = Persistencia.PersistenciaUtil.CargaConfig();

            this.KeyPreview = true;
        }

        // Captura el evento de cerrar el formulario
        override protected void OnFormClosing(FormClosingEventArgs e)
        {
            _controlador.CerrarAplicacion();
        }


        // ################################# AUXILIAR ################################
        #region Metodos Auxiliares

        private void addJugador(bool eqLocal, bool titular, Jugador j)
        {
            Button controlNombre = new Button();
            controlNombre.Text = j.ShortName;
            controlNombre.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            controlNombre.Dock = System.Windows.Forms.DockStyle.Fill;
            controlNombre.Margin = new System.Windows.Forms.Padding(0);
            controlNombre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            Label controlNumero = new Label();
            controlNumero.Text = Convert.ToString(j.Number);
            //Color colorPos;
            //if (j.Posicion == Jugador.Portero)
            //{
            //    colorPos = System.Drawing.Color.WhiteSmoke;
            //}
            //else if (j.Posicion == Jugador.Extremo)
            //{
            //    colorPos = System.Drawing.Color.LightGray;
            //}
            //else if (j.Posicion == Jugador.ExtremoDerecho)
            //{
            //    colorPos = System.Drawing.Color.LightGray;
            //}
            //else if (j.Posicion == Jugador.ExtremoIzquierdo)
            //{
            //    colorPos = System.Drawing.Color.LightGray;
            //}
            //else if (j.Posicion == Jugador.Lateral)
            //{
            //    colorPos = System.Drawing.Color.Silver;
            //}
            //else if (j.Posicion == Jugador.LateralDerecho)
            //{
            //    colorPos = System.Drawing.Color.Silver;
            //}
            //else if (j.Posicion == Jugador.LateralIzquierdo)
            //{
            //    colorPos = System.Drawing.Color.Silver;
            //}
            //else if (j.Posicion == Jugador.Central)
            //{
            //    colorPos = System.Drawing.Color.DarkGray;
            //}
            //else
            //{
            //    colorPos = System.Drawing.Color.DimGray;
            //}
            //controlNumero.BackColor = colorPos;
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
                //panel = (eqLocal ? this.panelBanquilloEq1 : this.panelBanquilloEq2);
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

        #region BOOLEANOS CONTROL TRAMAS
        public void set_escuchar_crono(bool b)
        {
            escuchar_crono=b;
        }
        public bool get_escuchar_crono()
        {
            return escuchar_crono;
        }

        public void set_escuchar_exclusion(bool b)
        {
            escuchar_exclusion=b;
        }
        public bool get_escuchar_exclusion()
        {
            return escuchar_exclusion;
        }

        public void set_escuchar_goles(bool b)
        {
            escuchar_goles=b;
        }
        public bool get_escuchar_goles()
        {
            return escuchar_goles;
        }

        public void set_escuchar_timeOut(bool b)
        {
            escuchar_timeOut=b;
        }
        public bool get_escuchar_timeOut()
        {
            return escuchar_timeOut;
        }

        public void set_escuchar_marcador(bool b)
        {
            escuchar_marcador = b;
        }
        public bool get_escuchar_marcador()
        {
            return escuchar_marcador;
        }

        public void set_escuchar_dorsales(bool b)
        {
            escuchar_dorsales=b;
        }
        public bool get_escuchar_dorsales()
        {
            return escuchar_dorsales;
        }
        #endregion


        public void ConfigEquipoLocal(Equipo equipo)
        {
            panelJugadoresEq1.RowCount = equipo.Jugadores.Count;
            this.checkBoxEqL.Text = equipo.TeamCode;

            // Entrenador
            this.checkBoxEntrenadorL.Text = equipo.Entrenador.ShortName;
            this.checkBoxEntrenadorL.Click += delegate { _controlador.ClickOnEntrenador(equipo.Entrenador); };

            // Entrenador asistente
            this.checkBoxEntrenadorAsistenteL.Text = equipo.EntrenadorAsistente.ShortName;
            this.checkBoxEntrenadorAsistenteL.Click += delegate { _controlador.ClickOnEntrenadorAsistente(equipo.EntrenadorAsistente); };

            List<Jugador> players = new List<Jugador>();
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

            //panelJugadoresEq1.Controls.Clear();
            //equipo.Jugadores.Sort(new JugadorComparerDorsal());         //equipo.Jugadores.Sort(new JugadorComparerGUI());  ORDENAR POR POSICION
            //foreach (Jugador j in equipo.Jugadores)
            //{
            //    addJugador(true, true, j);
            //    Console.WriteLine(j.ShortName);
            //}
            ////panelBanquilloEq1.Controls.Clear();
            ////equipo.Banquillo.Sort(new JugadorComparerDorsal());         //equipo.Jugadores.Sort(new JugadorComparerGUI());
            ////foreach (Jugador j in equipo.Banquillo)
            ////{
            ////    addJugador(true, false, j);
            ////}

            //if (equipo.Color1.A < 255)
            //{
            //    if (equipo.Color1.B <= 125 && equipo.Color1.G <= 125 && equipo.Color1.R <= 125)
            //        equipo.Color1 = Color.FromArgb(255, 255, 255, 255);
            //    else
            //        equipo.Color1 = Color.FromArgb(255, equipo.Color1.R, equipo.Color1.G, equipo.Color1.B);
            //}
            _colorL = equipo.Color1;
        }
        public void ConfigEquipoVisitante(Equipo equipo)
        {
            panelJugadoresEq2.RowCount = equipo.Jugadores.Count;
            this.checkBoxEqV.Text = equipo.TeamCode;

            // Entrenador
            this.checkBoxEntrenadorV.Text = equipo.Entrenador.ShortName;
            this.checkBoxEntrenadorV.Click += delegate { _controlador.ClickOnEntrenador(equipo.Entrenador); };

            // Entrenador asistente
            this.checkBoxEntrenadorAsistenteV.Text = equipo.EntrenadorAsistente.ShortName;
            this.checkBoxEntrenadorAsistenteV.Click += delegate { _controlador.ClickOnEntrenadorAsistente(equipo.EntrenadorAsistente); };

            List<Jugador> players = new List<Jugador>();
            panelJugadoresEq2.Controls.Clear();

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
                addJugador(false, true, j);
            }
            

            //panelJugadoresEq2.Controls.Clear();
            //equipo.Jugadores.Sort(new JugadorComparerDorsal());         //equipo.Jugadores.Sort(new JugadorComparerGUI());
            //foreach (Jugador j in equipo.Jugadores)
            //{
            //    addJugador(false, true, j);
            //}
            ////panelBanquilloEq2.Controls.Clear();
            ////equipo.Banquillo.Sort(new JugadorComparerDorsal());         //equipo.Jugadores.Sort(new JugadorComparerGUI());
            ////foreach (Jugador j in equipo.Banquillo)
            ////{
            ////    addJugador(false, false, j);
            ////}

            //if (equipo.Color1.A < 255)
            //{
            //    if(equipo.Color1.B <= 125 && equipo.Color1.G <= 125 && equipo.Color1.R <= 125)
            //        equipo.Color1 = Color.FromArgb(255, 255, 255, 255);
            //    else
            //        equipo.Color1 = Color.FromArgb(255, equipo.Color1.R, equipo.Color1.G, equipo.Color1.B);
            //}

            _colorV = equipo.Color1;
        }

        public void ActivaTabJugadores(bool titulares)
        {
            this.tabControlPlantilla.SelectedTab = (titulares ? this.tabPageJugadores : this.tabPageJugadores);
                //(titulares ? this.tabPageJugadores : this.tabPageBanquillo);
        }

        public void ConfigListBox(Object[] lista)
        {
            try
            {
                CheckForIllegalCrossThreadCalls = false;

                listBox.Items.Clear();
                listBox.Items.AddRange(lista);
            }
            catch (Exception e)
            {
                //se llama fuera del main
                Console.WriteLine("*** " + e);
            }
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
            catch(Exception e) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Source);
                Console.WriteLine(e.StackTrace);
            }
        }
        private void SetCronoText(string text)
        {
            this.checkBoxCrono.Text = text.Replace("*", "");
            if (!numericUpDown_minutes.Focused && !numericUpDown_seconds.Focused)
            {
                Console.WriteLine("*** "+text);
                string[] subs = text.Split('*');
                this.numericUpDown_minutes.Text = subs[1].Split('\'')[0];
                this.numericUpDown_seconds.Text = subs[1].Split('\'')[1];
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
                controles = new Control[panelJugadoresEq1.Controls.Count];       //Control[panelJugadoresEq1.Controls.Count + panelBanquilloEq1.Controls.Count];

                panelJugadoresEq1.Controls.CopyTo(controles, 0);
                //panelBanquilloEq1.Controls.CopyTo(controles, panelJugadoresEq1.Controls.Count);
            }
            else // eqVisitante
            {
                controles = new Control[panelJugadoresEq2.Controls.Count];       //Control[panelJugadoresEq2.Controls.Count + panelBanquilloEq2.Controls.Count];

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
                controles = new Control[panelJugadoresEq1.Controls.Count];       //Control[panelJugadoresEq1.Controls.Count + panelBanquilloEq1.Controls.Count];

                panelJugadoresEq1.Controls.CopyTo(controles, 0);
                //panelBanquilloEq1.Controls.CopyTo(controles, panelJugadoresEq1.Controls.Count);
            }
            else // eqVisitante
            {
                controles = new Control[panelJugadoresEq2.Controls.Count];       //Control[panelJugadoresEq2.Controls.Count + panelBanquilloEq2.Controls.Count];

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
            activaControl(checkBoxEntrenadorAsistenteL, activo, true);
            activaControl(checkBoxEntrenadorV, activo, false);
            activaControl(checkBoxEntrenadorAsistenteV, activo, false);
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
        public void ActivaOpcionesCrono()
        {
            this.cronoKickoff.Enabled = true;
            this.cronoFinParte.Enabled = true;
            this.cronoFinPartido.Enabled = true;
        }

        public void DesactivaOpcionesCrono()
        {
            this.cronoKickoff.Enabled = false;
            this.cronoFinParte.Enabled = false;
            this.cronoFinPartido.Enabled = false;
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


        // ######################## EMPTY GOAL #############################
        #region Empty Goal

        private void emptyGoalL_CheckedChanged(object sender, EventArgs e)
        {
            //if (((CheckBox)sender).Checked)
            //{
            //    _controlador.emptyGoal(true);
            //}
            //else
            //{
            //    _controlador.non_emptyGoal(true);
            //}

            //// Cambia la imagen enm función del estado
            //this.emptyGoalL.Image = (((CheckBox)sender).Checked ?
            //    Properties.Resources.BalonBotaB_ON :
            //    Properties.Resources.BalonBotaB_OFF);

            if(emptyGoalL.BackColor == Color.Transparent || emptyGoalL.BackColor == Color.Coral)
            {
                emptyGoalL.BackColor = Color.LightGreen;
                _controlador.EmptyGoal(true, true);
            }
            else
            {
                emptyGoalL.BackColor = Color.Transparent;
                _controlador.EmptyGoal(true, false);
            }

            
        }

        private void emptyGoalV_CheckedChanged(object sender, EventArgs e)
        {
            //if (((CheckBox)sender).Checked)
            //{
            //    _controlador.emptyGoal(false);
            //}
            //else
            //{
            //    _controlador.non_emptyGoal(false);
            //}

            //// Cambia la imagen enm función del estado
            //this.emptyGoalV.Image = (((CheckBox)sender).Checked ?
            //    Properties.Resources.BalonBotaA_ON :
            //    Properties.Resources.BalonBotaA_OFF);


            if (emptyGoalV.BackColor == Color.Transparent || emptyGoalV.BackColor == Color.Coral)
            {
                emptyGoalV.BackColor = Color.LightGreen;
                _controlador.EmptyGoal(false, true);
            }
            else
            {
                emptyGoalV.BackColor = Color.Transparent;
                _controlador.EmptyGoal(false, false);
            }
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

        //void PosesionLocal()
        //{
        //    if (posesionL.Checked)
        //    {
        //        _controlador.PosesionStart(true);
        //    }
        //    posesionL.BackColor = posesionL.Checked ? _colorL : Color.FromArgb(0, Color.White);
        //    // Cambia la imagen enm función del estado
        //    //this.posesionL.Image = (posesionL.Checked ?
        //    //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaB_ON :
        //    //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaB_OFF);


        //    //this.posesionL.Image = (posesionL.Checked ?
        //    //    Properties.Resources.BalonBotaB_ON :
        //    //    Properties.Resources.BalonBotaB_OFF);

        //    if (AsynchronousClient.isClient)
        //    {
        //        AsynchronousClient.Send("PosesionLocal");
        //    }
        //}

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

        //void PosesionVisitante()
        //{
        //    if (posesionV.Checked)
        //    {
        //        _controlador.PosesionStart(false);
        //    }

        //    posesionV.BackColor = posesionV.Checked ? _colorV : Color.FromArgb(0, Color.White);
        //    //this.posesionV.Image = (posesionV.Checked ?
        //    //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaA_ON :
        //    //    Futbol_Sala_Manager_App.Properties.Resources.BalonBotaA_OFF);
        //    // Cambia la imagen enm función del estado
        //    //this.posesionV.Image = (posesionV.Checked ?
        //    //    Properties.Resources.BalonBotaA_ON :
        //    //    Properties.Resources.BalonBotaA_OFF);

        //    if (AsynchronousClient.isClient)
        //    {
        //        AsynchronousClient.Send("PosesionVisitante");
        //    }
        //}

        //void PosesionStopClick()
        //{
        //    _controlador.PosesionStop();

        //    if (AsynchronousClient.isClient)
        //    {
        //        AsynchronousClient.Send("PosesionStop");
        //    }
        //}

        //void PosesionStop()
        //{
        //    this.posesionStop.Image = (posesionStop.Checked ?
        //        Futbol_Sala_Manager_App.Properties.Resources.Balon_ON :
        //        Futbol_Sala_Manager_App.Properties.Resources.Balon_OFF);

        //    Cambia la imagen enm función del estado
        //    this.posesionStop.Image = (posesionStop.Checked ?
        //        Properties.Resources.Balon_ON :
        //        Properties.Resources.Balon_OFF);
        //}

        private void posesionStop_CheckedChanged(object sender, EventArgs e)
        {
            // Cambia la imagen enm función del estado
            this.posesionStop.Image = (((RadioButton)sender).Checked ?
                Properties.Resources.Balon_ON :
                Properties.Resources.Balon_OFF);
        }

        //void PosesionClientDataReceived(string data)
        //{
        //    //Console.WriteLine("***Ha llegado " + data + "***");
        //    if (data.Contains("PosesionLocal") && !posesionL.Checked)
        //    {
        //        posesionL.Checked = true;
        //        //_controlador.PosesionStart(true);
        //        //this.posesionL.Image = Properties.Resources.BalonBotaB_ON;
        //        //this.posesionStop.Image = Properties.Resources.Balon_OFF;
        //        //this.posesionV.Image = Properties.Resources.BalonBotaA_OFF;
        //    }
        //    else if (data.Contains("PosesionVisitante") && !posesionV.Checked)
        //    {
        //        posesionV.Checked = true;
        //        //_controlador.PosesionStart(false);
        //        //this.posesionL.Image = Properties.Resources.BalonBotaB_OFF;
        //        //this.posesionStop.Image = Properties.Resources.Balon_OFF;
        //        //this.posesionV.Image = Properties.Resources.BalonBotaA_ON;
        //    }
        //    else if (data.Contains("PosesionStop") && !posesionStop.Checked)
        //    {
        //        posesionStop.Checked = true;
        //        //_controlador.PosesionStop();
        //        //this.posesionL.Image = Properties.Resources.BalonBotaB_OFF;
        //        //this.posesionStop.Image = Properties.Resources.Balon_ON;
        //        //this.posesionV.Image = Properties.Resources.BalonBotaA_OFF;
        //    }
        //}

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
            _controlador.ListBoxConfig(Controlador.Localizadores);
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

        //private void checkBoxEqLocalSuplentes_Click(object sender, EventArgs e)
        //{
        //    _controlador.previewTeamLineUpSubstitutes(true);
        //}

        private void alineacionEquipoV_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLineUp(false);
        }

        private void disposicionEquipoV_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamLayout(false);
        }            

        //private void checkBoxEqVisitSuplentes_Click(object sender, EventArgs e)
        //{
        //    _controlador.previewTeamLineUpSubstitutes(false);
        //}      

        private void suplentes_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewTeamsReserves();
        }

        private void tablaStats_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.previewStatisticsTable();
        }

        //private void prematchs_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.PreMatchs);
        //}

        //private void countdowns_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.Countdowns);
        //}
        //private void Weather_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.Weather);
        //}
        //private void EndToEndTest_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.EndToEndTest);
        //}

        //private void PostMultiFlashInterview_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.PostMultiFlashInterview);
        //}

        //private void EndOfPostMultiFlashInterview_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.EndOfPostMultiFlashInterview);
        //}

        //private void Highlights_Click(object sender, EventArgs e)
        //{
        //    _controlador.previewHighlights();
        //}

        //private void NewsExchangeFeed_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.NewsExchangeFeed);
        //}

        //private void PitchConditions_CheckedChanged(object sender, EventArgs e)
        //{
        //    _controlador.ListBoxConfig(Controlador.PitchConditions);
        //}

        private void InfoCrwaler_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.InfoCrawler);
        }

        private void EditStatistics_CheckedChanged(object sender, EventArgs e)
        {
            // Aquí abrimos directamente el Form. Dónde se editan las estadísticas
            // ¿¿ Hay que comprobar que ya está abierto ??
            EditEstadisticasForm exf = new EditEstadisticasForm(_controlador._datos, _controlador);
            exf.Show();
        }

        private void GroupStanding_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.GroupStanding);
        }

        private void NextTransmission_Click(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.NextTransmission);
        }

        private void Schedule_Click(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.Schedule);
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

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                
                this.numericUpDown_minutes.Text = "00";
                this.numericUpDown_seconds.Text = "00";

                //DEJAMOS DE ESCUCHAR DE LA CONSOLA
                cambia_escuchar_crono(false);

                _controlador.CronoFinParte();
            }
                
        }

        private void cronoFinPartido_Click(object sender, EventArgs e)
        {
            string message = "¿Está seguro que quiere finalizar?";
            string caption = "Información";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //DEJAMOS DE ESCUCHAR DE LA CONSOLA
                cambia_escuchar_crono(false);

                _controlador.CronoFinPartido();
            }  
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

        private System.Timers.Timer cronoTimer;
        private void cronoEdit_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_minutes.Focused || numericUpDown_seconds.Focused)
            {
                if (cronoTimer != null)
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
            //System.Threading.Thread.Sleep(500);
            this.BeginInvoke((MethodInvoker)delegate { tableLayout_Crono.Focus(); });
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

        //private void previa_CheckedChanged(object sender, EventArgs e)
        //{
        //    configMenu(panelOpcionesPrevia);
        //    _controlador.SetSeccion(Controlador.Previa);
        //}

        //private void presentacion_CheckedChanged(object sender, EventArgs e)
        //{
        //    configMenu(panelOpcionesPresentacion);
        //    _controlador.SetSeccion(Controlador.Presentacion);
        //}

        //private void rotulos_CheckedChanged(object sender, EventArgs e)
        //{
        //    configMenu(panelOpcionesRotulos);
        //    _controlador.SetSeccion(Controlador.Rotulos);
        //}

        private void emergency_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.ListBoxConfig(Controlador.EmergencyCaptions);
        }

        private void paradas_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Paradas);
        }

        private void coolingBreak_CheckedChanged(object sender, EventArgs e)
        {   
            //_controlador.SetSeccion(Controlador.CoolingBreak);
        }


        private void fueraJuego_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            //_controlador.SetSeccion(Controlador.FuerasDeJuego);
        }

        //private void cambio_CheckedChanged(object sender, EventArgs e)
        //{
        //    //configMenu(panelOpcionesCambios);
        //    //_controlador.SetSeccion(Controlador.Cambio);

        //    //Program.CambioEscogido = -1;
        //}

        void UpdateStats()
        {
            if (_stats.Count <= 1)
                _controlador.OnAirClear();
            else if (_stats.Count == 2)
                _controlador.OnAirSetup(new EstadisticasCommand(_stats[1], _stats[0], _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
            else if (_stats.Count == 3)
                _controlador.OnAirSetup(new EstadisticasCommand(_stats[2], _stats[1], _stats[0], _controlador._datos.EquipoL, _controlador._datos.EquipoV, _controlador._datos));
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
                
        
        // #########################  SECCIONES BALONMANO  ############################
        #region Botones seccion Balonmano
        private void presentacionHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesPresentacion);
            _controlador.SetSeccion(Controlador.Handball_Presentacion);
        }

        private void tEstadisticasHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesStats);
            _controlador.SetSeccion(Controlador.Handball_Testadisticas);
        }
        
        private void rotulosHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesRotulos);
            _controlador.SetSeccion(Controlador.Rotulos);
        }

        //GOL
        private void golesHandball_CheckedChanged(object sender, EventArgs e)
        {

            configMenu(panelOpcionesGoles);

            // Disable player click
            _controlador.SetSeccion(Controlador.Goles);
        }

        //GOL DE PENALTI
        private void golesHandballField_CheckedChanged(object sender, EventArgs e)
        {
            //configMenu(panelOpcionesGoles);
            _controlador.SetSeccion(Controlador.GolesPenalti);
        }

        //GOL DE CONTRAATAQUE
        private void golesHandballSeven_CheckedChanged(object sender, EventArgs e)
        {
            //configMenu(panelOpcionesGoles);
            _controlador.SetSeccion(Controlador.GolesContraataque);
        }



        private void attacksHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Attacks);
        }

        //TIRO NORMAL
        private void fieldThrowsHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesTiros);
            _controlador.SetSeccion(Controlador.Tiros);
        }

        //TIRO 7 METROS
        private void tiroDentro_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.TirosPenalti);
        }

        //TIROS CONTRAATAQUE
        private void tiroComp_CheckedChanged(object sender, EventArgs e)
        {
            _controlador.SetSeccion(Controlador.TirosContraataque);
        }

        private void fastbreaksHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Fastbreaks);
        }

        private void sevenMeterThrowsHandball_CheckedChanged(object sender, EventArgs e)
        {
            //configMenu(null);
            //_controlador.SetSeccion(Controlador.Handball_Sevenmthrows);
        }

        //PERDIDAS
        private void turnoversHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Perdidas);
        }

        private void tarjetasHandball_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesTarjetas);
            //_controlador.SetSeccion(Controlador.Handball_Tarjetas);
        }

        private void exclusions_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(null);
            _controlador.SetSeccion(Controlador.Exclusions);
        }

        private void tarjetaAzul_CheckedChanged(object sender, EventArgs e)
        {

            _controlador.SetSeccion(Controlador.TAzules);

            /*tarjetaAzul.Checked = false;
            if (tarjetaAzul.BackColor == Color.Transparent && _stats.Count < 3)
            {
                tarjetaAzul.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.);
            }
            else if (tarjetaAzul.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.);
                tarjetaAzul.BackColor = Color.Transparent;
            }

            UpdateStats();*/
        }

        private void tarjetaAmarilla_CheckedChanged(object sender, EventArgs e)
        {

            _controlador.SetSeccion(Controlador.TAmarillas);

            /*tarjetaAmarilla.Checked = false;
            if (tarjetaAmarilla.BackColor == Color.Transparent && _stats.Count < 3)
            {
                tarjetaAmarilla.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.);
            }
            else if (tarjetaAmarilla.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.);
                tarjetaAmarilla.BackColor = Color.Transparent;
            }

            UpdateStats();*/
        }

        private void tarjetaRoja_CheckedChanged(object sender, EventArgs e)
        {

            _controlador.SetSeccion(Controlador.TRojas);

            /*tarjetaRoja.Checked = false;
            if (tarjetaRoja.BackColor == Color.Transparent && _stats.Count < 3)
            {
                tarjetaRoja.BackColor = Color.LightSkyBlue;

                // Se añade al stack
                _stats.Add(EstadisticasCommand.);
            }
            else if (tarjetaRoja.BackColor == Color.LightSkyBlue)
            {
                _stats.Remove(EstadisticasCommand.);
                tarjetaRoja.BackColor = Color.Transparent;
            }

            UpdateStats();*/
        }



















        #endregion
                     
       

        private void stats_CheckedChanged(object sender, EventArgs e)
        {
            configMenu(panelOpcionesStats);
            _controlador.SetSeccion(Controlador.Stats);
        }

        private void estadisticas_CheckedChanged(object sender, EventArgs e)
        {
            //configMenu(panelOpcionesEstadisticas);
            //UpdateStats();
            //_controlador.SetSeccion(Controlador.Estadisticas);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == false)
                Program.CambioEscogido = -1;
        }
                
        private void checkBoxConecta_Consola_CheckedChanged(object sender, EventArgs e)
        {
            Console_Conexions_Branch panle_conexion_consola = new Console_Conexions_Branch(this, escuchar_crono, escuchar_exclusion, escuchar_goles, escuchar_timeOut, escuchar_marcador, escuchar_dorsales);
            panle_conexion_consola.ShowDialog();
        }

        public void AbrirRotulosJugador()
        {
            this.checkBox3.PerformClick();
        }

       
        /* CONSOLE CONEXIONS BRANCH */
        public void cambia_escuchar_crono(bool b)
        {
            escuchar_crono = b;

            if (b)
            {
                checkBoxCrono.BackColor = Color.Transparent;
            }
            else
            {
                checkBoxCrono.BackColor = Color.Coral;
            }
        }
        

        public void cambia_escuchar_exclusion(bool b)
        {
            escuchar_exclusion = b;
        }

        public void cambia_escuchar_goles(bool b)
        {
            escuchar_goles = b;
        }
        

        private void LocalTimeOutButton_Click(object sender, EventArgs e)
        {
            _controlador.LocalTimeOut();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Z)
            //{
            //    //POSESION EQ LOCAL
            //    Console.WriteLine("........................................................LOCAL");
            //    posesionL.PerformClick();
            //}
            //else if (e.KeyCode == Keys.X)
            //{
            //    //POSESION STOP
            //    Console.WriteLine("........................................................STOP");
            //    posesionStop.PerformClick();
            //}
            //else if (e.KeyCode == Keys.C)
            //{
            //    //POSESION EQ VISITANTE
            //    Console.WriteLine("........................................................VISITANTE");
            //    posesionV.PerformClick();
            //}
            //else
            //{
            //    Console.WriteLine("........................................................KeyCode: "+ e.KeyCode);
            //}

            if (e.KeyCode.Equals(Keys.Z) && posesionL.Enabled)
            {
                posesionL.Checked = posesionL.Enabled;
                posesionL.PerformClick();
            }
            else if (e.KeyCode.Equals(Keys.X) && posesionStop.Enabled)
            {
                posesionStop.Checked = posesionStop.Enabled;
                posesionStop.PerformClick();
            }
            else if (e.KeyCode.Equals(Keys.C) && posesionV.Enabled)
            {
                posesionV.Checked = posesionV.Enabled;
                posesionV.PerformClick();
            }
        }

        private void VisitorTimeOutButton_Click(object sender, EventArgs e)
        {
            _controlador.VisitorTimeOut();
        }

   
        //TERCER ESTADO DE EMPTY GOAL
        public void emptyNaranja(bool local)
        {
            if (local)
            {
                if (emptyGoalL.BackColor == Color.Transparent)
                {
                    emptyGoalL.BackColor = Color.Coral;
                }
            }
            else
            {
                if (emptyGoalV.BackColor == Color.Transparent)
                {
                    emptyGoalV.BackColor = Color.Coral;
                }
            }
        }

    }
}
