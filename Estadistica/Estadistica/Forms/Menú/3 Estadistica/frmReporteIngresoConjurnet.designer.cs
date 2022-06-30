namespace Estadistica
{
    partial class frmReporteIngresoConjurnet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReporteIngresoConjurnet));
            this.btnConsulta = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.cmbProductos = new System.Windows.Forms.ComboBox();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblRegistros = new System.Windows.Forms.Label();
            this.chkPorPeriodo = new System.Windows.Forms.CheckBox();
            this.dTPFechaIni = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFechaIni = new System.Windows.Forms.Label();
            this.dTPFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConsulta
            // 
            this.btnConsulta.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConsulta.BackColor = System.Drawing.Color.SlateGray;
            this.btnConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsulta.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsulta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnConsulta.Location = new System.Drawing.Point(88, 275);
            this.btnConsulta.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Size = new System.Drawing.Size(70, 27);
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
            this.lblMensajes.Location = new System.Drawing.Point(8, 307);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(254, 26);
            this.lblMensajes.TabIndex = 195;
            this.lblMensajes.Text = "Selecciona la información a consultar.";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picLogo
            // 
            this.picLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(36, 12);
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
            this.cmbCarteras.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbCarteras.BackColor = System.Drawing.Color.White;
            this.cmbCarteras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCarteras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCarteras.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbCarteras.FormattingEnabled = true;
            this.cmbCarteras.Location = new System.Drawing.Point(73, 83);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(191, 24);
            this.cmbCarteras.TabIndex = 198;
            this.cmbCarteras.SelectedIndexChanged += new System.EventHandler(this.cmbCarteras_SelectedIndexChanged);
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(8, 86);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 199;
            this.lblCartera.Text = "Cartera";
            // 
            // lblProducto
            // 
            this.lblProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(8, 116);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(59, 16);
            this.lblProducto.TabIndex = 221;
            this.lblProducto.Text = "Producto";
            // 
            // cmbProductos
            // 
            this.cmbProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbProductos.BackColor = System.Drawing.Color.White;
            this.cmbProductos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProductos.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new System.Drawing.Point(73, 113);
            this.cmbProductos.Name = "cmbProductos";
            this.cmbProductos.Size = new System.Drawing.Size(191, 24);
            this.cmbProductos.TabIndex = 220;
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Location = new System.Drawing.Point(8, 146);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(0, 16);
            this.lblSaldo.TabIndex = 222;
            // 
            // lblRegistros
            // 
            this.lblRegistros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblRegistros.AutoSize = true;
            this.lblRegistros.Location = new System.Drawing.Point(8, 168);
            this.lblRegistros.Name = "lblRegistros";
            this.lblRegistros.Size = new System.Drawing.Size(0, 16);
            this.lblRegistros.TabIndex = 230;
            // 
            // chkPorPeriodo
            // 
            this.chkPorPeriodo.AutoSize = true;
            this.chkPorPeriodo.Location = new System.Drawing.Point(88, 244);
            this.chkPorPeriodo.Name = "chkPorPeriodo";
            this.chkPorPeriodo.Size = new System.Drawing.Size(95, 20);
            this.chkPorPeriodo.TabIndex = 234;
            this.chkPorPeriodo.Text = "Por periodo";
            this.chkPorPeriodo.UseVisualStyleBackColor = true;
            this.chkPorPeriodo.CheckedChanged += new System.EventHandler(this.chkPorPeriodo_CheckedChanged);
            // 
            // dTPFechaIni
            // 
            this.dTPFechaIni.CustomFormat = "MM/yyyy";
            this.dTPFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPFechaIni.Location = new System.Drawing.Point(88, 146);
            this.dTPFechaIni.Name = "dTPFechaIni";
            this.dTPFechaIni.Size = new System.Drawing.Size(112, 26);
            this.dTPFechaIni.TabIndex = 235;
            this.dTPFechaIni.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 165);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 236;
            // 
            // lblFechaIni
            // 
            this.lblFechaIni.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblFechaIni.AutoSize = true;
            this.lblFechaIni.Location = new System.Drawing.Point(8, 152);
            this.lblFechaIni.Name = "lblFechaIni";
            this.lblFechaIni.Size = new System.Drawing.Size(74, 16);
            this.lblFechaIni.TabIndex = 237;
            this.lblFechaIni.Text = "Fecha Inicio";
            this.lblFechaIni.Visible = false;
            this.lblFechaIni.Click += new System.EventHandler(this.lblFechaIni_Click);
            // 
            // dTPFechaFin
            // 
            this.dTPFechaFin.CustomFormat = "MM/yyyy";
            this.dTPFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dTPFechaFin.Location = new System.Drawing.Point(88, 195);
            this.dTPFechaFin.Name = "dTPFechaFin";
            this.dTPFechaFin.Size = new System.Drawing.Size(112, 26);
            this.dTPFechaFin.TabIndex = 238;
            this.dTPFechaFin.Visible = false;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.Location = new System.Drawing.Point(8, 202);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(61, 16);
            this.lblFechaFin.TabIndex = 239;
            this.lblFechaFin.Text = "Fecha Fin";
            this.lblFechaFin.Visible = false;
            this.lblFechaFin.Click += new System.EventHandler(this.label2_Click);
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWait.Location = new System.Drawing.Point(116, 270);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 196;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            // 
            // frmReporteIngresoConjurnet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(274, 342);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.dTPFechaFin);
            this.Controls.Add(this.lblFechaIni);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dTPFechaIni);
            this.Controls.Add(this.chkPorPeriodo);
            this.Controls.Add(this.lblRegistros);
            this.Controls.Add(this.lblSaldo);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.cmbProductos);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.cmbCarteras);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmReporteIngresoConjurnet";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte Ingreso Conjurnet - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBotonera_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProductos;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblRegistros;
        private System.Windows.Forms.CheckBox chkPorPeriodo;
        private System.Windows.Forms.DateTimePicker dTPFechaIni;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFechaIni;
        private System.Windows.Forms.DateTimePicker dTPFechaFin;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.PictureBox picWait;
    }
}