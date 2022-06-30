namespace Estadistica
{
    partial class frmFactura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFactura));
            this.lblFinal = new System.Windows.Forms.Label();
            this.dtpFinal = new System.Windows.Forms.DateTimePicker();
            this.btnConsulta = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.lbxAvances = new System.Windows.Forms.ListBox();
            this.lblFechas = new System.Windows.Forms.Label();
            this.lblFechaInicial = new System.Windows.Forms.Label();
            this.dtpFechaInicial = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFinal = new System.Windows.Forms.Label();
            this.dtpFechaFinal = new System.Windows.Forms.DateTimePicker();
            this.btnIntervalo = new System.Windows.Forms.Button();
            this.picWaitIntervalo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitIntervalo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFinal
            // 
            this.lblFinal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFinal.AutoSize = true;
            this.lblFinal.Location = new System.Drawing.Point(9, 148);
            this.lblFinal.Name = "lblFinal";
            this.lblFinal.Size = new System.Drawing.Size(55, 16);
            this.lblFinal.TabIndex = 146;
            this.lblFinal.Text = "Avances";
            // 
            // dtpFinal
            // 
            this.dtpFinal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpFinal.CustomFormat = "MM/yyyy";
            this.dtpFinal.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFinal.Location = new System.Drawing.Point(12, 167);
            this.dtpFinal.Name = "dtpFinal";
            this.dtpFinal.Size = new System.Drawing.Size(118, 26);
            this.dtpFinal.TabIndex = 147;
            // 
            // btnConsulta
            // 
            this.btnConsulta.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConsulta.BackColor = System.Drawing.Color.SlateGray;
            this.btnConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsulta.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsulta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnConsulta.Location = new System.Drawing.Point(138, 169);
            this.btnConsulta.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Size = new System.Drawing.Size(73, 27);
            this.btnConsulta.TabIndex = 197;
            this.btnConsulta.Text = "Buscar";
            this.btnConsulta.UseVisualStyleBackColor = false;
            this.btnConsulta.Click += new System.EventHandler(this.btnConsulta_Click);
            // 
            // lblMensajes
            // 
            this.lblMensajes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensajes.BackColor = System.Drawing.Color.White;
            this.lblMensajes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblMensajes.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblMensajes.Location = new System.Drawing.Point(12, 348);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(348, 25);
            this.lblMensajes.TabIndex = 195;
            this.lblMensajes.Text = "Selecciona una cartera y un mes de avances.";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWait.Location = new System.Drawing.Point(154, 164);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 196;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            // 
            // picLogo
            // 
            this.picLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(87, 12);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(195, 62);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 143;
            this.picLogo.TabStop = false;
            // 
            // sfdExcel
            // 
            this.sfdExcel.DefaultExt = "xlsx";
            this.sfdExcel.Filter = "Libro de Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
            this.sfdExcel.Title = "Guardar libro de Excel";
            // 
            // cmbCarteras
            // 
            this.cmbCarteras.FormattingEnabled = true;
            this.cmbCarteras.Location = new System.Drawing.Point(15, 117);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(196, 24);
            this.cmbCarteras.TabIndex = 198;
            this.cmbCarteras.SelectedIndexChanged += new System.EventHandler(this.cmbCarteras_SelectedIndexChanged);
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(17, 98);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 199;
            this.lblCartera.Text = "Cartera";
            // 
            // lbxAvances
            // 
            this.lbxAvances.DisplayMember = "FechaAvance";
            this.lbxAvances.FormattingEnabled = true;
            this.lbxAvances.ItemHeight = 16;
            this.lbxAvances.Location = new System.Drawing.Point(228, 117);
            this.lbxAvances.Name = "lbxAvances";
            this.lbxAvances.Size = new System.Drawing.Size(120, 228);
            this.lbxAvances.TabIndex = 201;
            this.lbxAvances.ValueMember = "FechaAvance";
            this.lbxAvances.Visible = false;
            this.lbxAvances.Click += new System.EventHandler(this.lbxAvances_Click);
            this.lbxAvances.SelectedIndexChanged += new System.EventHandler(this.lbxAvances_SelectedIndexChanged);
            // 
            // lblFechas
            // 
            this.lblFechas.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFechas.AutoSize = true;
            this.lblFechas.Location = new System.Drawing.Point(261, 98);
            this.lblFechas.Name = "lblFechas";
            this.lblFechas.Size = new System.Drawing.Size(47, 16);
            this.lblFechas.TabIndex = 202;
            this.lblFechas.Text = "Fechas";
            this.lblFechas.Visible = false;
            // 
            // lblFechaInicial
            // 
            this.lblFechaInicial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFechaInicial.AutoSize = true;
            this.lblFechaInicial.Location = new System.Drawing.Point(12, 205);
            this.lblFechaInicial.Name = "lblFechaInicial";
            this.lblFechaInicial.Size = new System.Drawing.Size(40, 16);
            this.lblFechaInicial.TabIndex = 203;
            this.lblFechaInicial.Text = "Inicial";
            // 
            // dtpFechaInicial
            // 
            this.dtpFechaInicial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpFechaInicial.CustomFormat = "MM/yyyy";
            this.dtpFechaInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicial.Location = new System.Drawing.Point(15, 224);
            this.dtpFechaInicial.Name = "dtpFechaInicial";
            this.dtpFechaInicial.Size = new System.Drawing.Size(118, 26);
            this.dtpFechaInicial.TabIndex = 204;
            // 
            // lblFechaFinal
            // 
            this.lblFechaFinal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFechaFinal.AutoSize = true;
            this.lblFechaFinal.Location = new System.Drawing.Point(12, 253);
            this.lblFechaFinal.Name = "lblFechaFinal";
            this.lblFechaFinal.Size = new System.Drawing.Size(34, 16);
            this.lblFechaFinal.TabIndex = 205;
            this.lblFechaFinal.Text = "Final";
            // 
            // dtpFechaFinal
            // 
            this.dtpFechaFinal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpFechaFinal.CustomFormat = "MM/yyyy";
            this.dtpFechaFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFinal.Location = new System.Drawing.Point(15, 272);
            this.dtpFechaFinal.Name = "dtpFechaFinal";
            this.dtpFechaFinal.Size = new System.Drawing.Size(118, 26);
            this.dtpFechaFinal.TabIndex = 206;
            // 
            // btnIntervalo
            // 
            this.btnIntervalo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnIntervalo.BackColor = System.Drawing.Color.SlateGray;
            this.btnIntervalo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIntervalo.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIntervalo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnIntervalo.Location = new System.Drawing.Point(138, 248);
            this.btnIntervalo.Margin = new System.Windows.Forms.Padding(2);
            this.btnIntervalo.Name = "btnIntervalo";
            this.btnIntervalo.Size = new System.Drawing.Size(73, 27);
            this.btnIntervalo.TabIndex = 208;
            this.btnIntervalo.Text = "Generar";
            this.btnIntervalo.UseVisualStyleBackColor = false;
            this.btnIntervalo.Click += new System.EventHandler(this.btnIntervalo_Click);
            // 
            // picWaitIntervalo
            // 
            this.picWaitIntervalo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWaitIntervalo.BackColor = System.Drawing.Color.Transparent;
            this.picWaitIntervalo.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWaitIntervalo.Location = new System.Drawing.Point(154, 243);
            this.picWaitIntervalo.Name = "picWaitIntervalo";
            this.picWaitIntervalo.Size = new System.Drawing.Size(32, 32);
            this.picWaitIntervalo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWaitIntervalo.TabIndex = 207;
            this.picWaitIntervalo.TabStop = false;
            this.picWaitIntervalo.Visible = false;
            // 
            // frmFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(372, 382);
            this.Controls.Add(this.btnIntervalo);
            this.Controls.Add(this.picWaitIntervalo);
            this.Controls.Add(this.lblFechaFinal);
            this.Controls.Add(this.dtpFechaFinal);
            this.Controls.Add(this.lblFechaInicial);
            this.Controls.Add(this.dtpFechaInicial);
            this.Controls.Add(this.lblFechas);
            this.Controls.Add(this.lbxAvances);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.cmbCarteras);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.lblFinal);
            this.Controls.Add(this.dtpFinal);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFactura";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Factura - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBotonera_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitIntervalo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblFinal;
        private System.Windows.Forms.DateTimePicker dtpFinal;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.ListBox lbxAvances;
        private System.Windows.Forms.Label lblFechas;
        private System.Windows.Forms.Label lblFechaInicial;
        private System.Windows.Forms.DateTimePicker dtpFechaInicial;
        private System.Windows.Forms.Label lblFechaFinal;
        private System.Windows.Forms.DateTimePicker dtpFechaFinal;
        private System.Windows.Forms.Button btnIntervalo;
        private System.Windows.Forms.PictureBox picWaitIntervalo;
    }
}