namespace Estadistica
{
    partial class frmGeolocalizacion
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGeolocalizacion));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnArchivo = new System.Windows.Forms.Button();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.btnCargar = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.lblRegistros = new System.Windows.Forms.Label();
            this.lblInstrucciones = new System.Windows.Forms.Label();
            this.SeleccionarArchivo = new System.Windows.Forms.OpenFileDialog();
            this.dgvGeolocalizacion = new System.Windows.Forms.DataGridView();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeolocalizacion)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(11, 12);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(178, 62);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 148;
            this.picLogo.TabStop = false;
            // 
            // btnArchivo
            // 
            this.btnArchivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnArchivo.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnArchivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArchivo.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArchivo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnArchivo.Location = new System.Drawing.Point(824, 50);
            this.btnArchivo.Margin = new System.Windows.Forms.Padding(2);
            this.btnArchivo.Name = "btnArchivo";
            this.btnArchivo.Size = new System.Drawing.Size(111, 27);
            this.btnArchivo.TabIndex = 151;
            this.btnArchivo.Text = "Archivo";
            this.btnArchivo.UseVisualStyleBackColor = false;
            this.btnArchivo.Visible = false;
            this.btnArchivo.Click += new System.EventHandler(this.btnArchivo_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRuta.BackColor = System.Drawing.Color.Gainsboro;
            this.txtRuta.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRuta.Enabled = false;
            this.txtRuta.ForeColor = System.Drawing.Color.DimGray;
            this.txtRuta.Location = new System.Drawing.Point(194, 55);
            this.txtRuta.MaxLength = 10000;
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(625, 19);
            this.txtRuta.TabIndex = 150;
            this.txtRuta.Text = "Seleccione Archivo";
            this.txtRuta.Visible = false;
            // 
            // picWait
            // 
            this.picWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Location = new System.Drawing.Point(475, 539);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 152;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            // 
            // btnCargar
            // 
            this.btnCargar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCargar.BackColor = System.Drawing.Color.SlateGray;
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCargar.Location = new System.Drawing.Point(429, 544);
            this.btnCargar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(111, 27);
            this.btnCargar.TabIndex = 154;
            this.btnCargar.Text = "Iniciar";
            this.btnCargar.UseVisualStyleBackColor = false;
            this.btnCargar.Visible = false;
            this.btnCargar.Click += new System.EventHandler(this.btnCargar_Click);
            // 
            // lblMensajes
            // 
            this.lblMensajes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensajes.BackColor = System.Drawing.Color.White;
            this.lblMensajes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblMensajes.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblMensajes.Location = new System.Drawing.Point(2, 582);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(946, 20);
            this.lblMensajes.TabIndex = 153;
            this.lblMensajes.Text = "Resultado";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRegistros
            // 
            this.lblRegistros.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblRegistros.Location = new System.Drawing.Point(555, 556);
            this.lblRegistros.Name = "lblRegistros";
            this.lblRegistros.Size = new System.Drawing.Size(357, 15);
            this.lblRegistros.TabIndex = 156;
            this.lblRegistros.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblInstrucciones
            // 
            this.lblInstrucciones.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblInstrucciones.Location = new System.Drawing.Point(47, 556);
            this.lblInstrucciones.Name = "lblInstrucciones";
            this.lblInstrucciones.Size = new System.Drawing.Size(353, 15);
            this.lblInstrucciones.TabIndex = 155;
            // 
            // dgvGeolocalizacion
            // 
            this.dgvGeolocalizacion.AllowUserToAddRows = false;
            this.dgvGeolocalizacion.AllowUserToDeleteRows = false;
            this.dgvGeolocalizacion.AllowUserToResizeRows = false;
            this.dgvGeolocalizacion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGeolocalizacion.BackgroundColor = System.Drawing.Color.White;
            this.dgvGeolocalizacion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGeolocalizacion.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvGeolocalizacion.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSlateGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGeolocalizacion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGeolocalizacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvGeolocalizacion.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGeolocalizacion.EnableHeadersVisualStyles = false;
            this.dgvGeolocalizacion.GridColor = System.Drawing.Color.LightGray;
            this.dgvGeolocalizacion.Location = new System.Drawing.Point(13, 87);
            this.dgvGeolocalizacion.MultiSelect = false;
            this.dgvGeolocalizacion.Name = "dgvGeolocalizacion";
            this.dgvGeolocalizacion.ReadOnly = true;
            this.dgvGeolocalizacion.RowHeadersVisible = false;
            this.dgvGeolocalizacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvGeolocalizacion.Size = new System.Drawing.Size(923, 436);
            this.dgvGeolocalizacion.StandardTab = true;
            this.dgvGeolocalizacion.TabIndex = 176;
            this.dgvGeolocalizacion.Visible = false;
            // 
            // cmbCarteras
            // 
            this.cmbCarteras.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbCarteras.BackColor = System.Drawing.Color.White;
            this.cmbCarteras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCarteras.DropDownWidth = 200;
            this.cmbCarteras.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCarteras.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbCarteras.FormattingEnabled = true;
            this.cmbCarteras.Location = new System.Drawing.Point(500, 25);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(207, 24);
            this.cmbCarteras.TabIndex = 213;
            this.cmbCarteras.SelectedIndexChanged += new System.EventHandler(this.cmbCarteras_SelectedIndexChanged);
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(415, 28);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 214;
            this.lblCartera.Text = "Cartera";
            // 
            // frmGeolocalizacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(948, 611);
            this.Controls.Add(this.cmbCarteras);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.dgvGeolocalizacion);
            this.Controls.Add(this.lblRegistros);
            this.Controls.Add(this.lblInstrucciones);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.btnArchivo);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGeolocalizacion";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geolocalizacion";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmGeolocalizacion_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeolocalizacion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnArchivo;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.Label lblRegistros;
        private System.Windows.Forms.Label lblInstrucciones;
        private System.Windows.Forms.OpenFileDialog SeleccionarArchivo;
        private System.Windows.Forms.DataGridView dgvGeolocalizacion;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
    }
}