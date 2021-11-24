namespace Balonmano_Manager_App.Interfaz
{
    partial class GolForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GolForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.GolContraataqueButton = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Gol7MButton = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.GolNormalButton = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tableLayout_Crono = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_minutes = new Balonmano_Manager_App.CustomNumeric();
            this.numericUpDown_seconds = new Balonmano_Manager_App.CustomNumeric();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayout_Crono.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_seconds)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 221);
            this.panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.78261F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.73913F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.73913F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 21.73913F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(179, 221);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.GolContraataqueButton);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 175);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(173, 43);
            this.panel4.TabIndex = 2;
            // 
            // GolContraataqueButton
            // 
            this.GolContraataqueButton.AutoSize = true;
            this.GolContraataqueButton.Location = new System.Drawing.Point(9, 11);
            this.GolContraataqueButton.Name = "GolContraataqueButton";
            this.GolContraataqueButton.Size = new System.Drawing.Size(156, 21);
            this.GolContraataqueButton.TabIndex = 1;
            this.GolContraataqueButton.TabStop = true;
            this.GolContraataqueButton.Text = "GOL CONTRAATAQUE";
            this.GolContraataqueButton.UseVisualStyleBackColor = true;
            this.GolContraataqueButton.CheckedChanged += new System.EventHandler(this.GolContraataqueButton_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Gol7MButton);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 127);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(173, 42);
            this.panel3.TabIndex = 1;
            // 
            // Gol7MButton
            // 
            this.Gol7MButton.AutoSize = true;
            this.Gol7MButton.Location = new System.Drawing.Point(31, 12);
            this.Gol7MButton.Name = "Gol7MButton";
            this.Gol7MButton.Size = new System.Drawing.Size(117, 21);
            this.Gol7MButton.TabIndex = 1;
            this.Gol7MButton.TabStop = true;
            this.Gol7MButton.Text = "GOL 7 METROS";
            this.Gol7MButton.UseVisualStyleBackColor = true;
            this.Gol7MButton.CheckedChanged += new System.EventHandler(this.Gol7MButton_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.GolNormalButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 79);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(173, 42);
            this.panel2.TabIndex = 0;
            // 
            // GolNormalButton
            // 
            this.GolNormalButton.AutoSize = true;
            this.GolNormalButton.Location = new System.Drawing.Point(36, 10);
            this.GolNormalButton.Name = "GolNormalButton";
            this.GolNormalButton.Size = new System.Drawing.Size(110, 21);
            this.GolNormalButton.TabIndex = 0;
            this.GolNormalButton.Text = "GOL NORMAL";
            this.GolNormalButton.UseVisualStyleBackColor = true;
            this.GolNormalButton.CheckedChanged += new System.EventHandler(this.GolNormalButton_CheckedChanged);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tableLayout_Crono);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(179, 76);
            this.panel5.TabIndex = 3;
            // 
            // tableLayout_Crono
            // 
            this.tableLayout_Crono.ColumnCount = 3;
            this.tableLayout_Crono.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_Crono.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout_Crono.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_Crono.Controls.Add(this.label1, 1, 0);
            this.tableLayout_Crono.Controls.Add(this.numericUpDown_minutes, 0, 0);
            this.tableLayout_Crono.Controls.Add(this.numericUpDown_seconds, 2, 0);
            this.tableLayout_Crono.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayout_Crono.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_Crono.Name = "tableLayout_Crono";
            this.tableLayout_Crono.RowCount = 1;
            this.tableLayout_Crono.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayout_Crono.Size = new System.Drawing.Size(179, 76);
            this.tableLayout_Crono.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(82, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = ":";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDown_minutes
            // 
            this.numericUpDown_minutes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDown_minutes.Cursor = System.Windows.Forms.Cursors.Default;
            this.numericUpDown_minutes.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_minutes.Location = new System.Drawing.Point(10, 15);
            this.numericUpDown_minutes.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown_minutes.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown_minutes.Name = "numericUpDown_minutes";
            this.numericUpDown_minutes.Size = new System.Drawing.Size(59, 46);
            this.numericUpDown_minutes.TabIndex = 2;
            this.numericUpDown_minutes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_minutes.ValueChanged += new System.EventHandler(this.numericUpDown_seconds_ValueChanged);
            // 
            // numericUpDown_seconds
            // 
            this.numericUpDown_seconds.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDown_seconds.Font = new System.Drawing.Font("Segoe UI Semibold", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_seconds.Location = new System.Drawing.Point(109, 15);
            this.numericUpDown_seconds.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown_seconds.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown_seconds.Name = "numericUpDown_seconds";
            this.numericUpDown_seconds.Size = new System.Drawing.Size(59, 46);
            this.numericUpDown_seconds.TabIndex = 2;
            this.numericUpDown_seconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_seconds.ValueChanged += new System.EventHandler(this.numericUpDown_seconds_ValueChanged);
            // 
            // GolForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 221);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(195, 260);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(195, 260);
            this.Name = "GolForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gol";
            this.Load += new System.EventHandler(this.GolForm_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.tableLayout_Crono.ResumeLayout(false);
            this.tableLayout_Crono.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_minutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_seconds)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton GolNormalButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton Gol7MButton;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton GolContraataqueButton;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TableLayoutPanel tableLayout_Crono;
        private System.Windows.Forms.Label label1;
        private CustomNumeric numericUpDown_minutes;
        private CustomNumeric numericUpDown_seconds;
    }
}