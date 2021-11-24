namespace Balonmano_Manager_App.Interfaz
{
    partial class PenaltisPanel
    {
        /// <summary> 
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar 
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonFallo = new System.Windows.Forms.RadioButton();
            this.buttonGol = new System.Windows.Forms.RadioButton();
            this.marcador = new System.Windows.Forms.CheckBox();
            this.panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.marcador, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(581, 386);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.buttonFallo, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonGol, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 336);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(581, 50);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // buttonFallo
            // 
            this.buttonFallo.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonFallo.BackColor = System.Drawing.Color.Firebrick;
            this.buttonFallo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFallo.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonFallo.Location = new System.Drawing.Point(291, 0);
            this.buttonFallo.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.buttonFallo.Name = "buttonFallo";
            this.buttonFallo.Size = new System.Drawing.Size(290, 50);
            this.buttonFallo.TabIndex = 1;
            this.buttonFallo.Text = "Fallo";
            this.buttonFallo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonFallo.UseVisualStyleBackColor = false;
            this.buttonFallo.Click += new System.EventHandler(this.buttonFallo_Click);
            // 
            // buttonGol
            // 
            this.buttonGol.Appearance = System.Windows.Forms.Appearance.Button;
            this.buttonGol.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.buttonGol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGol.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonGol.Location = new System.Drawing.Point(0, 0);
            this.buttonGol.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.buttonGol.Name = "buttonGol";
            this.buttonGol.Size = new System.Drawing.Size(289, 50);
            this.buttonGol.TabIndex = 0;
            this.buttonGol.Tag = "";
            this.buttonGol.Text = "Gol";
            this.buttonGol.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.buttonGol.UseVisualStyleBackColor = false;
            this.buttonGol.Click += new System.EventHandler(this.buttonGol_Click);
            // 
            // marcador
            // 
            this.marcador.Appearance = System.Windows.Forms.Appearance.Button;
            this.marcador.AutoSize = true;
            this.marcador.BackColor = System.Drawing.Color.WhiteSmoke;
            this.marcador.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marcador.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.marcador.Location = new System.Drawing.Point(0, 0);
            this.marcador.Margin = new System.Windows.Forms.Padding(0);
            this.marcador.Name = "marcador";
            this.marcador.Size = new System.Drawing.Size(581, 50);
            this.marcador.TabIndex = 6;
            this.marcador.Text = "Penaltis: 0 - 0";
            this.marcador.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.marcador.UseVisualStyleBackColor = false;
            this.marcador.CheckedChanged += new System.EventHandler(this.marcador_CheckedChanged);
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel.Location = new System.Drawing.Point(3, 53);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(575, 280);
            this.panel.TabIndex = 8;
            // 
            // PenaltisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PenaltisPanel";
            this.Size = new System.Drawing.Size(581, 386);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox marcador;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.RadioButton buttonFallo;
        private System.Windows.Forms.RadioButton buttonGol;
        private System.Windows.Forms.Panel panel;

    }
}
