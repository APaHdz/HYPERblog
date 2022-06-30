namespace Estadistica
{
    partial class frmMapa
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMapa));
            this.dtpMesAsignacion = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.gbOpciones = new System.Windows.Forms.GroupBox();
            this.cmbCorrInco = new System.Windows.Forms.ComboBox();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblLocalidad = new System.Windows.Forms.Label();
            this.cmbSaldo = new System.Windows.Forms.ComboBox();
            this.chkSCV = new System.Windows.Forms.CheckBox();
            this.chkBSegmento = new System.Windows.Forms.CheckedListBox();
            this.chkBProducto = new System.Windows.Forms.CheckedListBox();
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.chkPaquetes = new System.Windows.Forms.CheckBox();
            this.cmbPaquetes = new System.Windows.Forms.ComboBox();
            this.btnCargar = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.gMap = new GMap.NET.WindowsForms.GMapControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picWaitBulk = new System.Windows.Forms.PictureBox();
            this.gbOpciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpMesAsignacion
            // 
            this.dtpMesAsignacion.CustomFormat = "MM/yyyy";
            this.dtpMesAsignacion.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpMesAsignacion.Location = new System.Drawing.Point(65, 25);
            this.dtpMesAsignacion.Name = "dtpMesAsignacion";
            this.dtpMesAsignacion.Size = new System.Drawing.Size(112, 26);
            this.dtpMesAsignacion.TabIndex = 219;
            // 
            // lblFecha
            // 
            this.lblFecha.AutoSize = true;
            this.lblFecha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFecha.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblFecha.ForeColor = System.Drawing.Color.DimGray;
            this.lblFecha.Location = new System.Drawing.Point(8, 32);
            this.lblFecha.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFecha.Name = "lblFecha";
            this.lblFecha.Size = new System.Drawing.Size(41, 16);
            this.lblFecha.TabIndex = 218;
            this.lblFecha.Text = "Fecha";
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
            this.cmbCarteras.Location = new System.Drawing.Point(65, 57);
            this.cmbCarteras.Name = "cmbCarteras";
            this.cmbCarteras.Size = new System.Drawing.Size(207, 24);
            this.cmbCarteras.TabIndex = 216;
            // 
            // lblCartera
            // 
            this.lblCartera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblCartera.AutoSize = true;
            this.lblCartera.Location = new System.Drawing.Point(8, 65);
            this.lblCartera.Name = "lblCartera";
            this.lblCartera.Size = new System.Drawing.Size(51, 16);
            this.lblCartera.TabIndex = 217;
            this.lblCartera.Text = "Cartera";
            // 
            // gbOpciones
            // 
            this.gbOpciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.gbOpciones.Controls.Add(this.cmbCorrInco);
            this.gbOpciones.Controls.Add(this.lblSaldo);
            this.gbOpciones.Controls.Add(this.lblLocalidad);
            this.gbOpciones.Controls.Add(this.cmbSaldo);
            this.gbOpciones.Controls.Add(this.chkSCV);
            this.gbOpciones.Controls.Add(this.chkBSegmento);
            this.gbOpciones.Controls.Add(this.chkBProducto);
            this.gbOpciones.Controls.Add(this.cmbRegion);
            this.gbOpciones.Controls.Add(this.chkPaquetes);
            this.gbOpciones.Controls.Add(this.cmbPaquetes);
            this.gbOpciones.Controls.Add(this.dtpMesAsignacion);
            this.gbOpciones.Controls.Add(this.cmbCarteras);
            this.gbOpciones.Controls.Add(this.lblCartera);
            this.gbOpciones.Controls.Add(this.lblFecha);
            this.gbOpciones.Location = new System.Drawing.Point(12, -1);
            this.gbOpciones.Name = "gbOpciones";
            this.gbOpciones.Size = new System.Drawing.Size(969, 148);
            this.gbOpciones.TabIndex = 220;
            this.gbOpciones.TabStop = false;
            this.gbOpciones.Text = "- * -";
            // 
            // cmbCorrInco
            // 
            this.cmbCorrInco.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbCorrInco.BackColor = System.Drawing.Color.White;
            this.cmbCorrInco.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCorrInco.DropDownWidth = 200;
            this.cmbCorrInco.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbCorrInco.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbCorrInco.FormattingEnabled = true;
            this.cmbCorrInco.Items.AddRange(new object[] {
            "*",
            "Domicilio Correcto",
            "Domicilio Incorrecto"});
            this.cmbCorrInco.Location = new System.Drawing.Point(397, 52);
            this.cmbCorrInco.Name = "cmbCorrInco";
            this.cmbCorrInco.Size = new System.Drawing.Size(207, 24);
            this.cmbCorrInco.TabIndex = 233;
            // 
            // lblSaldo
            // 
            this.lblSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblSaldo.AutoSize = true;
            this.lblSaldo.Location = new System.Drawing.Point(318, 117);
            this.lblSaldo.Name = "lblSaldo";
            this.lblSaldo.Size = new System.Drawing.Size(39, 16);
            this.lblSaldo.TabIndex = 232;
            this.lblSaldo.Text = "Saldo";
            // 
            // lblLocalidad
            // 
            this.lblLocalidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblLocalidad.AutoSize = true;
            this.lblLocalidad.Location = new System.Drawing.Point(318, 87);
            this.lblLocalidad.Name = "lblLocalidad";
            this.lblLocalidad.Size = new System.Drawing.Size(63, 16);
            this.lblLocalidad.TabIndex = 231;
            this.lblLocalidad.Text = "Localidad";
            // 
            // cmbSaldo
            // 
            this.cmbSaldo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbSaldo.BackColor = System.Drawing.Color.White;
            this.cmbSaldo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaldo.DropDownWidth = 200;
            this.cmbSaldo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbSaldo.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbSaldo.FormattingEnabled = true;
            this.cmbSaldo.Items.AddRange(new object[] {
            "*",
            "< 10,000",
            "> 10,000 y < 50,000",
            "> 50,000 y < 100,000",
            "> 100,000 y < 200,000",
            "> 200,000 y < 300,000",
            "> 300,000 y < 400,000",
            "> 400,000 y < 500,000",
            "> 500,000"});
            this.cmbSaldo.Location = new System.Drawing.Point(397, 109);
            this.cmbSaldo.Name = "cmbSaldo";
            this.cmbSaldo.Size = new System.Drawing.Size(207, 24);
            this.cmbSaldo.TabIndex = 230;
            // 
            // chkSCV
            // 
            this.chkSCV.AutoSize = true;
            this.chkSCV.Location = new System.Drawing.Point(321, 54);
            this.chkSCV.Name = "chkSCV";
            this.chkSCV.Size = new System.Drawing.Size(49, 20);
            this.chkSCV.TabIndex = 229;
            this.chkSCV.Text = "SCV";
            this.chkSCV.UseVisualStyleBackColor = true;
            // 
            // chkBSegmento
            // 
            this.chkBSegmento.FormattingEnabled = true;
            this.chkBSegmento.Items.AddRange(new object[] {
            "VENCIDA",
            "VIGENTE",
            "FALLIDA",
            "CASTIGADA"});
            this.chkBSegmento.Location = new System.Drawing.Point(755, 57);
            this.chkBSegmento.Name = "chkBSegmento";
            this.chkBSegmento.Size = new System.Drawing.Size(113, 88);
            this.chkBSegmento.TabIndex = 228;
            // 
            // chkBProducto
            // 
            this.chkBProducto.FormattingEnabled = true;
            this.chkBProducto.Items.AddRange(new object[] {
            "Auto",
            "Consumo",
            "Tarjeta"});
            this.chkBProducto.Location = new System.Drawing.Point(640, 66);
            this.chkBProducto.Name = "chkBProducto";
            this.chkBProducto.Size = new System.Drawing.Size(88, 67);
            this.chkBProducto.TabIndex = 227;
            // 
            // cmbRegion
            // 
            this.cmbRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbRegion.BackColor = System.Drawing.Color.White;
            this.cmbRegion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegion.DropDownWidth = 200;
            this.cmbRegion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbRegion.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbRegion.FormattingEnabled = true;
            this.cmbRegion.Items.AddRange(new object[] {
            "*",
            "Álvaro Obregón",
            "Azcapotzalco",
            "Benito Juárez",
            "Coyoacán",
            "Cuajimalpa de Morelos",
            "Cuauhtémoc",
            "Gustavo A. Madero",
            "Iztacalco",
            "Iztapalapa",
            "La Magdalena Contreras",
            "Miguel Hidalgo",
            "Milpa Alta",
            "Tláhuac",
            "Tlalpan",
            "Venustiano Carranza",
            "Xochimilco",
            "CDMX",
            "Aguascalientes",
            "Baja California",
            "Baja California Sur",
            "Campeche",
            "Chiapas",
            "Chihuahua",
            "Coahuila",
            "Colima",
            "Durango",
            "Guanajuato",
            "Guerrero",
            "Hidalgo",
            "Jalisco",
            "México",
            "Michoacán",
            "Morelos",
            "Nayarit",
            "Nuevo León",
            "Oaxaca",
            "Puebla",
            "Querétaro",
            "Quintana Roo",
            "San Luis Potosí",
            "Sinaloa",
            "Sonora",
            "Tabasco",
            "Tamaulipas",
            "Tlaxcala",
            "Veracruz",
            "Yucatán",
            "Zacatecas"});
            this.cmbRegion.Location = new System.Drawing.Point(397, 79);
            this.cmbRegion.Name = "cmbRegion";
            this.cmbRegion.Size = new System.Drawing.Size(207, 24);
            this.cmbRegion.TabIndex = 226;
            // 
            // chkPaquetes
            // 
            this.chkPaquetes.AutoSize = true;
            this.chkPaquetes.Location = new System.Drawing.Point(321, 22);
            this.chkPaquetes.Name = "chkPaquetes";
            this.chkPaquetes.Size = new System.Drawing.Size(105, 20);
            this.chkPaquetes.TabIndex = 225;
            this.chkPaquetes.Text = "Código Postal";
            this.chkPaquetes.UseVisualStyleBackColor = true;
            this.chkPaquetes.CheckedChanged += new System.EventHandler(this.chkPaquetes_CheckedChanged);
            // 
            // cmbPaquetes
            // 
            this.cmbPaquetes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbPaquetes.BackColor = System.Drawing.Color.White;
            this.cmbPaquetes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaquetes.DropDownWidth = 200;
            this.cmbPaquetes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPaquetes.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbPaquetes.FormattingEnabled = true;
            this.cmbPaquetes.Location = new System.Drawing.Point(472, 22);
            this.cmbPaquetes.Name = "cmbPaquetes";
            this.cmbPaquetes.Size = new System.Drawing.Size(396, 24);
            this.cmbPaquetes.TabIndex = 220;
            this.cmbPaquetes.Visible = false;
            // 
            // btnCargar
            // 
            this.btnCargar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCargar.BackColor = System.Drawing.Color.SlateGray;
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCargar.Location = new System.Drawing.Point(433, 510);
            this.btnCargar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(111, 27);
            this.btnCargar.TabIndex = 222;
            this.btnCargar.Text = "Localizar";
            this.btnCargar.UseVisualStyleBackColor = false;
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
            this.lblMensajes.Location = new System.Drawing.Point(3, 539);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(978, 20);
            this.lblMensajes.TabIndex = 222;
            this.lblMensajes.Text = "Resultado";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gMap
            // 
            this.gMap.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.gMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMap.AutoScroll = true;
            this.gMap.Bearing = 0F;
            this.gMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gMap.CanDragMap = true;
            this.gMap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMap.GrayScaleMode = false;
            this.gMap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMap.LevelsKeepInMemmory = 5;
            this.gMap.Location = new System.Drawing.Point(12, 151);
            this.gMap.MarkersEnabled = true;
            this.gMap.MaxZoom = 18;
            this.gMap.MinZoom = 1;
            this.gMap.MouseWheelZoomEnabled = true;
            this.gMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMap.Name = "gMap";
            this.gMap.NegativeMode = false;
            this.gMap.PolygonsEnabled = true;
            this.gMap.RetryLoadTile = 0;
            this.gMap.RoutesEnabled = true;
            this.gMap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMap.ShowTileGridLines = false;
            this.gMap.Size = new System.Drawing.Size(969, 315);
            this.gMap.TabIndex = 223;
            this.gMap.Zoom = 15D;
            this.gMap.DoubleClick += new System.EventHandler(this.gMap_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // sfdExcel
            // 
            this.sfdExcel.DefaultExt = "xlsx";
            this.sfdExcel.Filter = "Documents (*.png)|*.png";
            this.sfdExcel.Title = "Guardar libro de Excel";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::Estadistica.Properties.Resources.Colores_Puntos;
            this.pictureBox1.Location = new System.Drawing.Point(42, 472);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(903, 26);
            this.pictureBox1.TabIndex = 224;
            this.pictureBox1.TabStop = false;
            // 
            // picWaitBulk
            // 
            this.picWaitBulk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.picWaitBulk.BackColor = System.Drawing.Color.Transparent;
            this.picWaitBulk.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWaitBulk.Location = new System.Drawing.Point(471, 504);
            this.picWaitBulk.Name = "picWaitBulk";
            this.picWaitBulk.Size = new System.Drawing.Size(32, 32);
            this.picWaitBulk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWaitBulk.TabIndex = 223;
            this.picWaitBulk.TabStop = false;
            this.picWaitBulk.Visible = false;
            // 
            // frmMapa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(993, 568);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.gMap);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.gbOpciones);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.picWaitBulk);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmMapa";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mapa (Google) -  Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMapa_FormClosed);
            this.gbOpciones.ResumeLayout(false);
            this.gbOpciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpMesAsignacion;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.GroupBox gbOpciones;
        private System.Windows.Forms.ComboBox cmbPaquetes;
        private System.Windows.Forms.PictureBox picWaitBulk;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.Label lblMensajes;
        private GMap.NET.WindowsForms.GMapControl gMap;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.CheckBox chkPaquetes;
        private System.Windows.Forms.ComboBox cmbRegion;
        private System.Windows.Forms.CheckBox chkSCV;
        private System.Windows.Forms.CheckedListBox chkBSegmento;
        private System.Windows.Forms.CheckedListBox chkBProducto;
        private System.Windows.Forms.ComboBox cmbSaldo;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblLocalidad;
        private System.Windows.Forms.ComboBox cmbCorrInco;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}