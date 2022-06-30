namespace Estadistica
{
    partial class frmReportesAlCliente
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReportesAlCliente));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.cmbProductos = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.lblDesde = new System.Windows.Forms.Label();
            this.lblHasta = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbReportes = new System.Windows.Forms.ComboBox();
            this.lblDescripción = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(218, 12);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(195, 62);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 133;
            this.picLogo.TabStop = false;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGenerar.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnGenerar.Location = new System.Drawing.Point(264, 215);
            this.btnGenerar.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(108, 27);
            this.btnGenerar.TabIndex = 11;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // lblMensajes
            // 
            this.lblMensajes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensajes.BackColor = System.Drawing.Color.White;
            this.lblMensajes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblMensajes.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblMensajes.Location = new System.Drawing.Point(3, 257);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(630, 42);
            this.lblMensajes.TabIndex = 12;
            this.lblMensajes.Text = "Obteniendo reportes, por favor espere...";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Location = new System.Drawing.Point(302, 215);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 141;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            // 
            // sfdExcel
            // 
            this.sfdExcel.DefaultExt = "xlsx";
            this.sfdExcel.Filter = "Libro de Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
            this.sfdExcel.Title = "Guardar libro de Excel";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(460, 154);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(118, 26);
            this.dtpHasta.TabIndex = 10;
            this.dtpHasta.Visible = false;
            // 
            // cmbCarteras
            // 
            this.cmbCarteras.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbCarteras.BackColor = System.Drawing.Color.White;
            this.cmbCarteras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCarteras.DropDownWidth = 250;
            this.cmbCarteras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCarteras.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbCarteras.FormattingEnabled = true;
            this.cmbCarteras.Location = new System.Drawing.Point(134, 96);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(161, 24);
            this.cmbCarteras.Sorted = true;
            this.cmbCarteras.TabIndex = 1;
            this.cmbCarteras.SelectionChangeCommitted += new System.EventHandler(this.cmbCarteras_SelectionChangeCommitted);
            // 
            // cmbProductos
            // 
            this.cmbProductos.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbProductos.BackColor = System.Drawing.Color.White;
            this.cmbProductos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProductos.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new System.Drawing.Point(416, 96);
            this.cmbProductos.Name = "cmbProductos";
            this.cmbProductos.Size = new System.Drawing.Size(161, 24);
            this.cmbProductos.Sorted = true;
            this.cmbProductos.TabIndex = 6;
            this.cmbProductos.Visible = false;
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(51, 99);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 0;
            this.lblCartera.Text = "Cartera";
            // 
            // lblProducto
            // 
            this.lblProducto.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(333, 99);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(59, 16);
            this.lblProducto.TabIndex = 5;
            this.lblProducto.Text = "Producto";
            this.lblProducto.Visible = false;
            // 
            // dtpDesde
            // 
            this.dtpDesde.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(331, 155);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(118, 26);
            this.dtpDesde.TabIndex = 8;
            this.dtpDesde.Visible = false;
            // 
            // lblDesde
            // 
            this.lblDesde.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDesde.AutoSize = true;
            this.lblDesde.Location = new System.Drawing.Point(368, 136);
            this.lblDesde.Name = "lblDesde";
            this.lblDesde.Size = new System.Drawing.Size(45, 16);
            this.lblDesde.TabIndex = 7;
            this.lblDesde.Text = "Desde";
            this.lblDesde.Visible = false;
            // 
            // lblHasta
            // 
            this.lblHasta.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHasta.AutoSize = true;
            this.lblHasta.Location = new System.Drawing.Point(499, 136);
            this.lblHasta.Name = "lblHasta";
            this.lblHasta.Size = new System.Drawing.Size(41, 16);
            this.lblHasta.TabIndex = 9;
            this.lblHasta.Text = "Hasta";
            this.lblHasta.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(51, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Reporte";
            // 
            // cmbReportes
            // 
            this.cmbReportes.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbReportes.BackColor = System.Drawing.Color.White;
            this.cmbReportes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbReportes.DropDownWidth = 250;
            this.cmbReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbReportes.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbReportes.FormattingEnabled = true;
            this.cmbReportes.Location = new System.Drawing.Point(134, 133);
            this.cmbReportes.Name = "cmbReportes";
            this.cmbReportes.Size = new System.Drawing.Size(161, 24);
            this.cmbReportes.TabIndex = 3;
            this.cmbReportes.SelectionChangeCommitted += new System.EventHandler(this.cmbReportes_SelectionChangeCommitted);
            // 
            // lblDescripción
            // 
            this.lblDescripción.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDescripción.BackColor = System.Drawing.Color.White;
            this.lblDescripción.Font = new System.Drawing.Font("Lucida Sans Unicode", 8F);
            this.lblDescripción.ForeColor = System.Drawing.Color.LightSlateGray;
            this.lblDescripción.Location = new System.Drawing.Point(51, 160);
            this.lblDescripción.Name = "lblDescripción";
            this.lblDescripción.Size = new System.Drawing.Size(244, 53);
            this.lblDescripción.TabIndex = 4;
            // 
            // frmReportesAlCliente
            // 
            this.AcceptButton = this.btnGenerar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(636, 308);
            this.Controls.Add(this.cmbCarteras);
            this.Controls.Add(this.cmbReportes);
            this.Controls.Add(this.cmbProductos);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.lblHasta);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDesde);
            this.Controls.Add(this.lblDescripción);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.dtpDesde);
            this.Controls.Add(this.dtpHasta);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReportesAlCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reportes al cliente - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDevoluciones_FormClosed);
            this.Shown += new System.EventHandler(this.frmReportesAlCliente_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.ComboBox cmbProductos;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.Label lblDesde;
        private System.Windows.Forms.Label lblHasta;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbReportes;
        private System.Windows.Forms.Label lblDescripción;
    }
}