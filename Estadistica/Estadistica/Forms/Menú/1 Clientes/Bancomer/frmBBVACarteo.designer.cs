namespace Estadistica
{
    partial class frmBBVACarteo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBBVACarteo));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.dtpFecha = new System.Windows.Forms.DateTimePicker();
            this.lblAsignacion = new System.Windows.Forms.Label();
            this.dtpPago1 = new System.Windows.Forms.DateTimePicker();
            this.lblPago1 = new System.Windows.Forms.Label();
            this.dtpPago2 = new System.Windows.Forms.DateTimePicker();
            this.lblPago2 = new System.Windows.Forms.Label();
            this.cmbSegmento = new System.Windows.Forms.ComboBox();
            this.lblSegmento = new System.Windows.Forms.Label();
            this.cmbCartera = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(54, 12);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(178, 62);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 120;
            this.picLogo.TabStop = false;
            // 
            // dtpFecha
            // 
            this.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFecha.Location = new System.Drawing.Point(90, 96);
            this.dtpFecha.Name = "dtpFecha";
            this.dtpFecha.Size = new System.Drawing.Size(103, 26);
            this.dtpFecha.TabIndex = 165;
            // 
            // lblAsignacion
            // 
            this.lblAsignacion.AutoSize = true;
            this.lblAsignacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAsignacion.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblAsignacion.Location = new System.Drawing.Point(87, 77);
            this.lblAsignacion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAsignacion.Name = "lblAsignacion";
            this.lblAsignacion.Size = new System.Drawing.Size(106, 16);
            this.lblAsignacion.TabIndex = 164;
            this.lblAsignacion.Text = "Fecha Asignación";
            // 
            // dtpPago1
            // 
            this.dtpPago1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPago1.Location = new System.Drawing.Point(25, 146);
            this.dtpPago1.Name = "dtpPago1";
            this.dtpPago1.Size = new System.Drawing.Size(100, 26);
            this.dtpPago1.TabIndex = 167;
            // 
            // lblPago1
            // 
            this.lblPago1.AutoSize = true;
            this.lblPago1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPago1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblPago1.Location = new System.Drawing.Point(24, 127);
            this.lblPago1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPago1.Name = "lblPago1";
            this.lblPago1.Size = new System.Drawing.Size(101, 16);
            this.lblPago1.TabIndex = 166;
            this.lblPago1.Text = "Fecha 1er. Pago";
            // 
            // dtpPago2
            // 
            this.dtpPago2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPago2.Location = new System.Drawing.Point(156, 146);
            this.dtpPago2.Name = "dtpPago2";
            this.dtpPago2.Size = new System.Drawing.Size(100, 26);
            this.dtpPago2.TabIndex = 169;
            // 
            // lblPago2
            // 
            this.lblPago2.AutoSize = true;
            this.lblPago2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPago2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblPago2.Location = new System.Drawing.Point(153, 127);
            this.lblPago2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPago2.Name = "lblPago2";
            this.lblPago2.Size = new System.Drawing.Size(104, 16);
            this.lblPago2.TabIndex = 168;
            this.lblPago2.Text = "Fecha 2do. Pago";
            // 
            // cmbSegmento
            // 
            this.cmbSegmento.FormattingEnabled = true;
            this.cmbSegmento.Items.AddRange(new object[] {
            "Auto",
            "Consumo",
            "Pyme",
            "Tdc Banco",
            "Tdc Finanzia"});
            this.cmbSegmento.Location = new System.Drawing.Point(25, 194);
            this.cmbSegmento.Name = "cmbSegmento";
            this.cmbSegmento.Size = new System.Drawing.Size(100, 24);
            this.cmbSegmento.TabIndex = 171;
            // 
            // lblSegmento
            // 
            this.lblSegmento.AutoSize = true;
            this.lblSegmento.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSegmento.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblSegmento.Location = new System.Drawing.Point(42, 175);
            this.lblSegmento.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSegmento.Name = "lblSegmento";
            this.lblSegmento.Size = new System.Drawing.Size(64, 16);
            this.lblSegmento.TabIndex = 170;
            this.lblSegmento.Text = "Segmento";
            // 
            // cmbCartera
            // 
            this.cmbCartera.FormattingEnabled = true;
            this.cmbCartera.Items.AddRange(new object[] {
            "Castigada",
            "Fallida",
            "Vencida",
            "Vigente"});
            this.cmbCartera.Location = new System.Drawing.Point(156, 194);
            this.cmbCartera.Name = "cmbCartera";
            this.cmbCartera.Size = new System.Drawing.Size(100, 24);
            this.cmbCartera.TabIndex = 173;
            // 
            // lblCartera
            // 
            this.lblCartera.AutoSize = true;
            this.lblCartera.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblCartera.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblCartera.Location = new System.Drawing.Point(181, 175);
            this.lblCartera.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 172;
            this.lblCartera.Text = "Cartera";
            // 
            // lblMensajes
            // 
            this.lblMensajes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensajes.BackColor = System.Drawing.Color.White;
            this.lblMensajes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblMensajes.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblMensajes.Location = new System.Drawing.Point(-2, 283);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(285, 48);
            this.lblMensajes.TabIndex = 176;
            this.lblMensajes.Text = "Selecciona los parametros y clic en Generar.";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWait.Location = new System.Drawing.Point(126, 237);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 175;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnGenerar.BackColor = System.Drawing.Color.SlateGray;
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnGenerar.Location = new System.Drawing.Point(90, 237);
            this.btnGenerar.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(110, 25);
            this.btnGenerar.TabIndex = 174;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // sfdExcel
            // 
            this.sfdExcel.DefaultExt = "xlsx";
            this.sfdExcel.Filter = "Libro de Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
            this.sfdExcel.Title = "Guardar libro de Excel";
            // 
            // frmBBVACarteo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(284, 330);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.cmbCartera);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.cmbSegmento);
            this.Controls.Add(this.lblSegmento);
            this.Controls.Add(this.dtpPago2);
            this.Controls.Add(this.lblPago2);
            this.Controls.Add(this.dtpPago1);
            this.Controls.Add(this.lblPago1);
            this.Controls.Add(this.dtpFecha);
            this.Controls.Add(this.lblAsignacion);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBBVACarteo";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Carteo - Estadistica";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.DateTimePicker dtpFecha;
        private System.Windows.Forms.Label lblAsignacion;
        private System.Windows.Forms.DateTimePicker dtpPago1;
        private System.Windows.Forms.Label lblPago1;
        private System.Windows.Forms.DateTimePicker dtpPago2;
        private System.Windows.Forms.Label lblPago2;
        private System.Windows.Forms.ComboBox cmbSegmento;
        private System.Windows.Forms.Label lblSegmento;
        private System.Windows.Forms.ComboBox cmbCartera;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
    }
}