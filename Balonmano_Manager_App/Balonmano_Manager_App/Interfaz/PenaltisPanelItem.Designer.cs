namespace Balonmano_Manager_App.Interfaz
{
    partial class PenaltisPanelItem
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
            this.jugadorV = new System.Windows.Forms.Label();
            this.numero = new System.Windows.Forms.Label();
            this.jugadorL = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.jugadorV, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.numero, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.jugadorL, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(462, 61);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // jugadorV
            // 
            this.jugadorV.AutoSize = true;
            this.jugadorV.BackColor = System.Drawing.Color.WhiteSmoke;
            this.jugadorV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jugadorV.Location = new System.Drawing.Point(248, 2);
            this.jugadorV.Margin = new System.Windows.Forms.Padding(0);
            this.jugadorV.Name = "jugadorV";
            this.jugadorV.Size = new System.Drawing.Size(212, 57);
            this.jugadorV.TabIndex = 2;
            this.jugadorV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numero
            // 
            this.numero.AutoSize = true;
            this.numero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numero.Location = new System.Drawing.Point(2, 2);
            this.numero.Margin = new System.Windows.Forms.Padding(0);
            this.numero.Name = "numero";
            this.numero.Size = new System.Drawing.Size(30, 57);
            this.numero.TabIndex = 1;
            this.numero.Text = "1";
            this.numero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // jugadorL
            // 
            this.jugadorL.AutoSize = true;
            this.jugadorL.BackColor = System.Drawing.Color.WhiteSmoke;
            this.jugadorL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.jugadorL.Location = new System.Drawing.Point(34, 2);
            this.jugadorL.Margin = new System.Windows.Forms.Padding(0);
            this.jugadorL.Name = "jugadorL";
            this.jugadorL.Size = new System.Drawing.Size(212, 57);
            this.jugadorL.TabIndex = 0;
            this.jugadorL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PenaltisPanelItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "PenaltisPanelItem";
            this.Size = new System.Drawing.Size(462, 61);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label jugadorL;
        private System.Windows.Forms.Label jugadorV;
        private System.Windows.Forms.Label numero;
    }
}
