namespace Loader_Setup_App
{
    partial class FormPpal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPpal));
            this.tableLayoutPanel_contenedor = new System.Windows.Forms.TableLayoutPanel();
            this.button_balonmano_setup = new System.Windows.Forms.Button();
            this.button_futbol_setup = new System.Windows.Forms.Button();
            this.button_futbol_sala_setup = new System.Windows.Forms.Button();
            this.panel_licencia = new System.Windows.Forms.Panel();
            this.label_codigo_licencia = new System.Windows.Forms.Label();
            this.tableLayoutPanel_contenedor.SuspendLayout();
            this.panel_licencia.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel_contenedor
            // 
            this.tableLayoutPanel_contenedor.ColumnCount = 1;
            this.tableLayoutPanel_contenedor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel_contenedor.Controls.Add(this.button_balonmano_setup, 0, 0);
            this.tableLayoutPanel_contenedor.Controls.Add(this.button_futbol_setup, 0, 1);
            this.tableLayoutPanel_contenedor.Controls.Add(this.button_futbol_sala_setup, 0, 2);
            this.tableLayoutPanel_contenedor.Controls.Add(this.panel_licencia, 0, 3);
            this.tableLayoutPanel_contenedor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel_contenedor.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel_contenedor.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel_contenedor.Name = "tableLayoutPanel_contenedor";
            this.tableLayoutPanel_contenedor.RowCount = 4;
            this.tableLayoutPanel_contenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_contenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_contenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel_contenedor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel_contenedor.Size = new System.Drawing.Size(434, 331);
            this.tableLayoutPanel_contenedor.TabIndex = 0;
            // 
            // button_balonmano_setup
            // 
            this.button_balonmano_setup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.button_balonmano_setup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_balonmano_setup.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_balonmano_setup.ForeColor = System.Drawing.Color.White;
            this.button_balonmano_setup.Image = global::Loader_Manager_App.Properties.Resources.balonmano_no_licenciado;
            this.button_balonmano_setup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_balonmano_setup.Location = new System.Drawing.Point(3, 3);
            this.button_balonmano_setup.Name = "button_balonmano_setup";
            this.button_balonmano_setup.Padding = new System.Windows.Forms.Padding(15);
            this.button_balonmano_setup.Size = new System.Drawing.Size(428, 97);
            this.button_balonmano_setup.TabIndex = 0;
            this.button_balonmano_setup.Text = "BALONMANO";
            this.button_balonmano_setup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_balonmano_setup.UseVisualStyleBackColor = false;
            this.button_balonmano_setup.Click += new System.EventHandler(this.button_balonmano_setup_Click);
            // 
            // button_futbol_setup
            // 
            this.button_futbol_setup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.button_futbol_setup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_futbol_setup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_futbol_setup.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_futbol_setup.ForeColor = System.Drawing.Color.White;
            this.button_futbol_setup.Image = global::Loader_Manager_App.Properties.Resources.futbol_no_licenciado;
            this.button_futbol_setup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_futbol_setup.Location = new System.Drawing.Point(3, 106);
            this.button_futbol_setup.Name = "button_futbol_setup";
            this.button_futbol_setup.Padding = new System.Windows.Forms.Padding(15);
            this.button_futbol_setup.Size = new System.Drawing.Size(428, 97);
            this.button_futbol_setup.TabIndex = 1;
            this.button_futbol_setup.Text = "FÚTBOL";
            this.button_futbol_setup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_futbol_setup.UseVisualStyleBackColor = false;
            this.button_futbol_setup.Click += new System.EventHandler(this.button_futbol_setup_Click);
            // 
            // button_futbol_sala_setup
            // 
            this.button_futbol_sala_setup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(58)))), ((int)(((byte)(138)))));
            this.button_futbol_sala_setup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_futbol_sala_setup.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_futbol_sala_setup.ForeColor = System.Drawing.Color.White;
            this.button_futbol_sala_setup.Image = global::Loader_Manager_App.Properties.Resources.futbolsal_no_licenciado;
            this.button_futbol_sala_setup.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_futbol_sala_setup.Location = new System.Drawing.Point(3, 209);
            this.button_futbol_sala_setup.Name = "button_futbol_sala_setup";
            this.button_futbol_sala_setup.Padding = new System.Windows.Forms.Padding(15);
            this.button_futbol_sala_setup.Size = new System.Drawing.Size(428, 97);
            this.button_futbol_sala_setup.TabIndex = 2;
            this.button_futbol_sala_setup.Text = "FÚTBOL SALA";
            this.button_futbol_sala_setup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_futbol_sala_setup.UseVisualStyleBackColor = false;
            this.button_futbol_sala_setup.Click += new System.EventHandler(this.button_futbol_sala_setup_Click);
            // 
            // panel_licencia
            // 
            this.panel_licencia.Controls.Add(this.label_codigo_licencia);
            this.panel_licencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_licencia.Location = new System.Drawing.Point(0, 309);
            this.panel_licencia.Margin = new System.Windows.Forms.Padding(0);
            this.panel_licencia.Name = "panel_licencia";
            this.panel_licencia.Size = new System.Drawing.Size(434, 22);
            this.panel_licencia.TabIndex = 3;
            // 
            // label_codigo_licencia
            // 
            this.label_codigo_licencia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_codigo_licencia.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_codigo_licencia.Location = new System.Drawing.Point(0, 0);
            this.label_codigo_licencia.Name = "label_codigo_licencia";
            this.label_codigo_licencia.Size = new System.Drawing.Size(434, 22);
            this.label_codigo_licencia.TabIndex = 0;
            this.label_codigo_licencia.Text = "label código licencia";
            this.label_codigo_licencia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormPpal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 331);
            this.Controls.Add(this.tableLayoutPanel_contenedor);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormPpal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loader Manager Deportes";
            this.Load += new System.EventHandler(this.FormPpal_Load);
            this.tableLayoutPanel_contenedor.ResumeLayout(false);
            this.panel_licencia.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel_contenedor;
        private System.Windows.Forms.Button button_balonmano_setup;
        private System.Windows.Forms.Button button_futbol_setup;
        private System.Windows.Forms.Button button_futbol_sala_setup;
        private System.Windows.Forms.Panel panel_licencia;
        private System.Windows.Forms.Label label_codigo_licencia;
    }
}

