namespace Estadistica
{
    partial class frmAnalisis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnalisis));
            this.btnGenerar = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.lblFechaFin = new System.Windows.Forms.Label();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblProducto = new System.Windows.Forms.Label();
            this.lblCartera = new System.Windows.Forms.Label();
            this.cmbProductos = new System.Windows.Forms.ComboBox();
            this.lblSegmento = new System.Windows.Forms.Label();
            this.cmbSegmentos = new System.Windows.Forms.ComboBox();
            this.chkBase = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerar
            // 
            this.btnGenerar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnGenerar.BackColor = System.Drawing.Color.SlateGray;
            this.btnGenerar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnGenerar.Location = new System.Drawing.Point(180, 252);
            this.btnGenerar.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(111, 27);
            this.btnGenerar.TabIndex = 197;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnConsulta_Click);
            // 
            // lblMensajes
            // 
            this.lblMensajes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensajes.BackColor = System.Drawing.Color.White;
            this.lblMensajes.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblMensajes.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            this.lblMensajes.Location = new System.Drawing.Point(12, 299);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(285, 42);
            this.lblMensajes.TabIndex = 195;
            this.lblMensajes.Text = "Selecciona la información a generar.";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWait.Location = new System.Drawing.Point(219, 252);
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
            this.picLogo.Location = new System.Drawing.Point(55, 12);
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
            // dtpFechaFin
            // 
            this.dtpFechaFin.CustomFormat = "MM/yyyy";
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(84, 101);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(112, 26);
            this.dtpFechaFin.TabIndex = 221;
            // 
            // lblFechaFin
            // 
            this.lblFechaFin.AutoSize = true;
            this.lblFechaFin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFechaFin.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblFechaFin.ForeColor = System.Drawing.Color.DimGray;
            this.lblFechaFin.Location = new System.Drawing.Point(19, 101);
            this.lblFechaFin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFechaFin.Name = "lblFechaFin";
            this.lblFechaFin.Size = new System.Drawing.Size(41, 16);
            this.lblFechaFin.TabIndex = 220;
            this.lblFechaFin.Text = "Fecha";
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
            this.cmbCarteras.Location = new System.Drawing.Point(84, 133);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(207, 24);
            this.cmbCarteras.TabIndex = 216;
            this.cmbCarteras.SelectedIndexChanged += new System.EventHandler(this.cmbCarteras_SelectedIndexChanged);
            // 
            // lblProducto
            // 
            this.lblProducto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblProducto.AutoSize = true;
            this.lblProducto.Location = new System.Drawing.Point(19, 171);
            this.lblProducto.Name = "lblProducto";
            this.lblProducto.Size = new System.Drawing.Size(59, 16);
            this.lblProducto.TabIndex = 219;
            this.lblProducto.Text = "Producto";
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(18, 141);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 218;
            this.lblCartera.Text = "Cartera";
            // 
            // cmbProductos
            // 
            this.cmbProductos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbProductos.BackColor = System.Drawing.Color.White;
            this.cmbProductos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProductos.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbProductos.FormattingEnabled = true;
            this.cmbProductos.Location = new System.Drawing.Point(84, 163);
            this.cmbProductos.Name = "cmbProductos";
            this.cmbProductos.Size = new System.Drawing.Size(207, 24);
            this.cmbProductos.TabIndex = 217;
            // 
            // lblSegmento
            // 
            this.lblSegmento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblSegmento.AutoSize = true;
            this.lblSegmento.Location = new System.Drawing.Point(19, 201);
            this.lblSegmento.Name = "lblSegmento";
            this.lblSegmento.Size = new System.Drawing.Size(64, 16);
            this.lblSegmento.TabIndex = 223;
            this.lblSegmento.Text = "Segmento";
            // 
            // cmbSegmentos
            // 
            this.cmbSegmentos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbSegmentos.BackColor = System.Drawing.Color.White;
            this.cmbSegmentos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSegmentos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSegmentos.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbSegmentos.FormattingEnabled = true;
            this.cmbSegmentos.Location = new System.Drawing.Point(84, 193);
            this.cmbSegmentos.Name = "cmbSegmentos";
            this.cmbSegmentos.Size = new System.Drawing.Size(207, 24);
            this.cmbSegmentos.TabIndex = 222;
            // 
            // chkBase
            // 
            this.chkBase.AutoSize = true;
            this.chkBase.Location = new System.Drawing.Point(203, 109);
            this.chkBase.Name = "chkBase";
            this.chkBase.Size = new System.Drawing.Size(98, 20);
            this.chkBase.TabIndex = 224;
            this.chkBase.Text = "dbCollection";
            this.chkBase.UseVisualStyleBackColor = true;
            // 
            // frmAnalisis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(309, 350);
            this.Controls.Add(this.chkBase);
            this.Controls.Add(this.lblSegmento);
            this.Controls.Add(this.cmbSegmentos);
            this.Controls.Add(this.dtpFechaFin);
            this.Controls.Add(this.lblFechaFin);
            this.Controls.Add(this.cmbCarteras);
            this.Controls.Add(this.lblProducto);
            this.Controls.Add(this.lblCartera);
            this.Controls.Add(this.cmbProductos);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAnalisis";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Analisis - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAnalisis_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.Label lblFechaFin;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.ComboBox cmbProductos;
        private System.Windows.Forms.Label lblSegmento;
        private System.Windows.Forms.ComboBox cmbSegmentos;
        private System.Windows.Forms.CheckBox chkBase;
    }
}