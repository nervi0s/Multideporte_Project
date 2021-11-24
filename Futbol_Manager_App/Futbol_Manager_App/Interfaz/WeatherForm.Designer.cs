namespace Futbol_Manager_App.Interfaz
{
    partial class WeatherForm
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

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeatherForm));
            this.label1 = new System.Windows.Forms.Label();
            this.buttonAceptar = new System.Windows.Forms.Button();
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxGrados = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxHumedad = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxViento = new System.Windows.Forms.TextBox();
            this.comboBoxTiempo = new System.Windows.Forms.ComboBox();
            this.buttonCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 17);
            this.label1.TabIndex = 44;
            this.label1.Text = "Cº";
            // 
            // buttonAceptar
            // 
            this.buttonAceptar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAceptar.Location = new System.Drawing.Point(185, 160);
            this.buttonAceptar.Name = "buttonAceptar";
            this.buttonAceptar.Size = new System.Drawing.Size(110, 35);
            this.buttonAceptar.TabIndex = 5;
            this.buttonAceptar.Text = "Aceptar";
            this.buttonAceptar.UseVisualStyleBackColor = true;
            this.buttonAceptar.Click += new System.EventHandler(this.buttonAceptar_Click);
            // 
            // textBoxHeader
            // 
            this.textBoxHeader.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHeader.Location = new System.Drawing.Point(86, 27);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.Size = new System.Drawing.Size(504, 25);
            this.textBoxHeader.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(29, 30);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 17);
            this.label11.TabIndex = 41;
            this.label11.Text = "Header";
            // 
            // textBoxGrados
            // 
            this.textBoxGrados.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxGrados.Location = new System.Drawing.Point(86, 67);
            this.textBoxGrados.Name = "textBoxGrados";
            this.textBoxGrados.Size = new System.Drawing.Size(110, 25);
            this.textBoxGrados.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(222, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 46;
            this.label2.Text = "Humedad";
            // 
            // textBoxHumedad
            // 
            this.textBoxHumedad.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxHumedad.Location = new System.Drawing.Point(293, 70);
            this.textBoxHumedad.Name = "textBoxHumedad";
            this.textBoxHumedad.Size = new System.Drawing.Size(110, 25);
            this.textBoxHumedad.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(429, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 17);
            this.label3.TabIndex = 48;
            this.label3.Text = "Viento";
            // 
            // textBoxViento
            // 
            this.textBoxViento.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxViento.Location = new System.Drawing.Point(480, 70);
            this.textBoxViento.Name = "textBoxViento";
            this.textBoxViento.Size = new System.Drawing.Size(110, 25);
            this.textBoxViento.TabIndex = 3;
            // 
            // comboBoxTiempo
            // 
            this.comboBoxTiempo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxTiempo.FormattingEnabled = true;
            this.comboBoxTiempo.Items.AddRange(new object[] {
            "CLEAN NIGHT",
            "CLOUDY",
            "FOGGY",
            "HAIL",
            "PARTLY CLOUD NIGHT",
            "PARTLY CLOUDY",
            "RAINY",
            "SNOW",
            "STORM",
            "SUNNY",
            "WINDY"});
            this.comboBoxTiempo.Location = new System.Drawing.Point(86, 112);
            this.comboBoxTiempo.Name = "comboBoxTiempo";
            this.comboBoxTiempo.Size = new System.Drawing.Size(317, 25);
            this.comboBoxTiempo.TabIndex = 4;
            // 
            // buttonCancelar
            // 
            this.buttonCancelar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancelar.Location = new System.Drawing.Point(325, 160);
            this.buttonCancelar.Name = "buttonCancelar";
            this.buttonCancelar.Size = new System.Drawing.Size(110, 35);
            this.buttonCancelar.TabIndex = 6;
            this.buttonCancelar.Text = "Cancelar";
            this.buttonCancelar.UseVisualStyleBackColor = true;
            this.buttonCancelar.Click += new System.EventHandler(this.buttonCancelar_Click_1);
            // 
            // WeatherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 211);
            this.Controls.Add(this.buttonCancelar);
            this.Controls.Add(this.comboBoxTiempo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxViento);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxHumedad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAceptar);
            this.Controls.Add(this.textBoxHeader);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxGrados);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(635, 250);
            this.MinimumSize = new System.Drawing.Size(635, 250);
            this.Name = "WeatherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fútbol Manager App - Weather";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAceptar;
        private System.Windows.Forms.TextBox textBoxHeader;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxGrados;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxHumedad;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxViento;
        private System.Windows.Forms.ComboBox comboBoxTiempo;
        private System.Windows.Forms.Button buttonCancelar;
    }
}