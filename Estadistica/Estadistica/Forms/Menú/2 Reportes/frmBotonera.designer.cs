namespace Estadistica
{
    partial class frmBotonera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBotonera));
            this.lblInicial = new System.Windows.Forms.Label();
            this.dtpInicial = new System.Windows.Forms.DateTimePicker();
            this.lblFinal = new System.Windows.Forms.Label();
            this.dtpFinal = new System.Windows.Forms.DateTimePicker();
            this.btnConsulta = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInicial
            // 
            this.lblInicial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblInicial.AutoSize = true;
            this.lblInicial.Location = new System.Drawing.Point(37, 136);
            this.lblInicial.Name = "lblInicial";
            this.lblInicial.Size = new System.Drawing.Size(40, 16);
            this.lblInicial.TabIndex = 144;
            this.lblInicial.Text = "Inicial";
            // 
            // dtpInicial
            // 
            this.dtpInicial.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInicial.Location = new System.Drawing.Point(83, 129);
            this.dtpInicial.Name = "dtpInicial";
            this.dtpInicial.Size = new System.Drawing.Size(118, 26);
            this.dtpInicial.TabIndex = 145;
            // 
            // lblFinal
            // 
            this.lblFinal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblFinal.AutoSize = true;
            this.lblFinal.Location = new System.Drawing.Point(207, 136);
            this.lblFinal.Name = "lblFinal";
            this.lblFinal.Size = new System.Drawing.Size(34, 16);
            this.lblFinal.TabIndex = 146;
            this.lblFinal.Text = "Final";
            // 
            // dtpFinal
            // 
            this.dtpFinal.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtpFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFinal.Location = new System.Drawing.Point(247, 129);
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
            this.btnConsulta.Location = new System.Drawing.Point(149, 173);
            this.btnConsulta.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Size = new System.Drawing.Size(111, 27);
            this.btnConsulta.TabIndex = 197;
            this.btnConsulta.Text = "Generar";
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
            this.lblMensajes.Location = new System.Drawing.Point(12, 219);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(379, 42);
            this.lblMensajes.TabIndex = 195;
            this.lblMensajes.Text = "Selecciona un intervalo y da click en Consultar";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWait.Location = new System.Drawing.Point(187, 171);
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
            this.picLogo.Location = new System.Drawing.Point(102, 12);
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
            this.cmbCarteras.Location = new System.Drawing.Point(159, 90);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(187, 24);
            this.cmbCarteras.TabIndex = 198;
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(80, 98);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 199;
            this.lblCartera.Text = "Cartera";
            // 
            // frmBotonera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(403, 270);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.cmbCarteras);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.lblFinal);
            this.Controls.Add(this.dtpFinal);
            this.Controls.Add(this.lblInicial);
            this.Controls.Add(this.dtpInicial);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBotonera";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Botonera - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBotonera_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblInicial;
        private System.Windows.Forms.DateTimePicker dtpInicial;
        private System.Windows.Forms.Label lblFinal;
        private System.Windows.Forms.DateTimePicker dtpFinal;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
    }
}