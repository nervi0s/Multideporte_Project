namespace Balonmano_Manager_App.Interfaz
{
    partial class ScheduleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScheduleForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_E1_info = new System.Windows.Forms.TextBox();
            this.textBox_E1_equipo2 = new System.Windows.Forms.TextBox();
            this.textBox_E1_equipo1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_E2_info = new System.Windows.Forms.TextBox();
            this.textBox_E2_equipo2 = new System.Windows.Forms.TextBox();
            this.textBox_E2_equipo1 = new System.Windows.Forms.TextBox();
            this.Aceptar = new System.Windows.Forms.Button();
            this.Cancelar = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 210);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox_E1_info);
            this.panel1.Controls.Add(this.textBox_E1_equipo2);
            this.panel1.Controls.Add(this.textBox_E1_equipo1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(578, 97);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(260, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "VS";
            // 
            // textBox_E1_info
            // 
            this.textBox_E1_info.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_E1_info.Location = new System.Drawing.Point(86, 55);
            this.textBox_E1_info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_E1_info.Name = "textBox_E1_info";
            this.textBox_E1_info.Size = new System.Drawing.Size(395, 25);
            this.textBox_E1_info.TabIndex = 2;
            // 
            // textBox_E1_equipo2
            // 
            this.textBox_E1_equipo2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_E1_equipo2.Location = new System.Drawing.Point(302, 12);
            this.textBox_E1_equipo2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_E1_equipo2.Name = "textBox_E1_equipo2";
            this.textBox_E1_equipo2.Size = new System.Drawing.Size(209, 25);
            this.textBox_E1_equipo2.TabIndex = 1;
            // 
            // textBox_E1_equipo1
            // 
            this.textBox_E1_equipo1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_E1_equipo1.Location = new System.Drawing.Point(33, 10);
            this.textBox_E1_equipo1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_E1_equipo1.Name = "textBox_E1_equipo1";
            this.textBox_E1_equipo1.Size = new System.Drawing.Size(209, 25);
            this.textBox_E1_equipo1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox_E2_info);
            this.panel2.Controls.Add(this.textBox_E2_equipo2);
            this.panel2.Controls.Add(this.textBox_E2_equipo1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 109);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(578, 97);
            this.panel2.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(260, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "VS";
            // 
            // textBox_E2_info
            // 
            this.textBox_E2_info.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_E2_info.Location = new System.Drawing.Point(86, 55);
            this.textBox_E2_info.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_E2_info.Name = "textBox_E2_info";
            this.textBox_E2_info.Size = new System.Drawing.Size(395, 25);
            this.textBox_E2_info.TabIndex = 2;
            // 
            // textBox_E2_equipo2
            // 
            this.textBox_E2_equipo2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_E2_equipo2.Location = new System.Drawing.Point(302, 12);
            this.textBox_E2_equipo2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_E2_equipo2.Name = "textBox_E2_equipo2";
            this.textBox_E2_equipo2.Size = new System.Drawing.Size(209, 25);
            this.textBox_E2_equipo2.TabIndex = 1;
            // 
            // textBox_E2_equipo1
            // 
            this.textBox_E2_equipo1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_E2_equipo1.Location = new System.Drawing.Point(33, 10);
            this.textBox_E2_equipo1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBox_E2_equipo1.Name = "textBox_E2_equipo1";
            this.textBox_E2_equipo1.Size = new System.Drawing.Size(209, 25);
            this.textBox_E2_equipo1.TabIndex = 0;
            // 
            // Aceptar
            // 
            this.Aceptar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Aceptar.Location = new System.Drawing.Point(160, 235);
            this.Aceptar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Aceptar.Name = "Aceptar";
            this.Aceptar.Size = new System.Drawing.Size(110, 35);
            this.Aceptar.TabIndex = 0;
            this.Aceptar.Text = "Aceptar";
            this.Aceptar.UseVisualStyleBackColor = true;
            this.Aceptar.Click += new System.EventHandler(this.Aceptar_Click);
            // 
            // Cancelar
            // 
            this.Cancelar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancelar.Location = new System.Drawing.Point(320, 235);
            this.Cancelar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Cancelar.Name = "Cancelar";
            this.Cancelar.Size = new System.Drawing.Size(110, 35);
            this.Cancelar.TabIndex = 1;
            this.Cancelar.Text = "Cancelar";
            this.Cancelar.UseVisualStyleBackColor = true;
            this.Cancelar.Click += new System.EventHandler(this.Cancelar_Click);
            // 
            // ScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 281);
            this.Controls.Add(this.Cancelar);
            this.Controls.Add(this.Aceptar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(600, 320);
            this.MinimumSize = new System.Drawing.Size(600, 320);
            this.Name = "ScheduleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Balonmano Manager App - Schedule";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Aceptar;
        private System.Windows.Forms.Button Cancelar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_E1_info;
        private System.Windows.Forms.TextBox textBox_E1_equipo2;
        private System.Windows.Forms.TextBox textBox_E1_equipo1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_E2_info;
        private System.Windows.Forms.TextBox textBox_E2_equipo2;
        private System.Windows.Forms.TextBox textBox_E2_equipo1;
    }
}