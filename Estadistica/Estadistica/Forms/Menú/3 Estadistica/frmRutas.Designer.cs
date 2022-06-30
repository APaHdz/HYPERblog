namespace Estadistica
{
    partial class frmRutas
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRutas));
            this.dtpMesAsignacion = new System.Windows.Forms.DateTimePicker();
            this.lblFecha = new System.Windows.Forms.Label();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.gbOpciones = new System.Windows.Forms.GroupBox();
            this.lblKm = new System.Windows.Forms.Label();
            this.cmbKilometros = new System.Windows.Forms.ComboBox();
            this.cmbCorrInco = new System.Windows.Forms.ComboBox();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.lblLocalidad = new System.Windows.Forms.Label();
            this.cmbSaldo = new System.Windows.Forms.ComboBox();
            this.chkSCV = new System.Windows.Forms.CheckBox();
            this.chkBSegmento = new System.Windows.Forms.CheckedListBox();
            this.chkBProducto = new System.Windows.Forms.CheckedListBox();
            this.cmbRegion = new System.Windows.Forms.ComboBox();
            this.btnConsulta = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.picWaitBulk = new System.Windows.Forms.PictureBox();
            this.dgvGeolocalizacion = new System.Windows.Forms.DataGridView();
            this.btnRutas = new System.Windows.Forms.Button();
            this.picWaitRutas = new System.Windows.Forms.PictureBox();
            this.gbOpciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeolocalizacion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitRutas)).BeginInit();
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
            this.gbOpciones.Controls.Add(this.lblKm);
            this.gbOpciones.Controls.Add(this.cmbKilometros);
            this.gbOpciones.Controls.Add(this.cmbCorrInco);
            this.gbOpciones.Controls.Add(this.lblSaldo);
            this.gbOpciones.Controls.Add(this.lblLocalidad);
            this.gbOpciones.Controls.Add(this.cmbSaldo);
            this.gbOpciones.Controls.Add(this.chkSCV);
            this.gbOpciones.Controls.Add(this.chkBSegmento);
            this.gbOpciones.Controls.Add(this.chkBProducto);
            this.gbOpciones.Controls.Add(this.cmbRegion);
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
            // lblKm
            // 
            this.lblKm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblKm.AutoSize = true;
            this.lblKm.Location = new System.Drawing.Point(318, 30);
            this.lblKm.Name = "lblKm";
            this.lblKm.Size = new System.Drawing.Size(36, 16);
            this.lblKm.TabIndex = 235;
            this.lblKm.Text = "Km\'s";
            // 
            // cmbKilometros
            // 
            this.cmbKilometros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cmbKilometros.BackColor = System.Drawing.Color.White;
            this.cmbKilometros.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKilometros.DropDownWidth = 200;
            this.cmbKilometros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbKilometros.ForeColor = System.Drawing.Color.SlateGray;
            this.cmbKilometros.FormattingEnabled = true;
            this.cmbKilometros.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbKilometros.Location = new System.Drawing.Point(397, 22);
            this.cmbKilometros.Name = "cmbKilometros";
            this.cmbKilometros.Size = new System.Drawing.Size(94, 24);
            this.cmbKilometros.TabIndex = 234;
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
            // btnConsulta
            // 
            this.btnConsulta.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConsulta.BackColor = System.Drawing.Color.SlateGray;
            this.btnConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsulta.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsulta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnConsulta.Location = new System.Drawing.Point(433, 510);
            this.btnConsulta.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Size = new System.Drawing.Size(111, 27);
            this.btnConsulta.TabIndex = 222;
            this.btnConsulta.Text = "Consultar";
            this.btnConsulta.UseVisualStyleBackColor = false;
            this.btnConsulta.Click += new System.EventHandler(this.btnCargar_Click);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // sfdExcel
            // 
            this.sfdExcel.DefaultExt = "xlsx";
            this.sfdExcel.Filter = "Libro de Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
            this.sfdExcel.Title = "Guardar libro de Excel";
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
            this.dgvGeolocalizacion.Location = new System.Drawing.Point(12, 153);
            this.dgvGeolocalizacion.MultiSelect = false;
            this.dgvGeolocalizacion.Name = "dgvGeolocalizacion";
            this.dgvGeolocalizacion.ReadOnly = true;
            this.dgvGeolocalizacion.RowHeadersVisible = false;
            this.dgvGeolocalizacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvGeolocalizacion.Size = new System.Drawing.Size(969, 345);
            this.dgvGeolocalizacion.StandardTab = true;
            this.dgvGeolocalizacion.TabIndex = 224;
            this.dgvGeolocalizacion.Visible = false;
            // 
            // btnRutas
            // 
            this.btnRutas.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRutas.BackColor = System.Drawing.Color.SeaGreen;
            this.btnRutas.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRutas.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRutas.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnRutas.Location = new System.Drawing.Point(870, 510);
            this.btnRutas.Margin = new System.Windows.Forms.Padding(2);
            this.btnRutas.Name = "btnRutas";
            this.btnRutas.Size = new System.Drawing.Size(111, 27);
            this.btnRutas.TabIndex = 225;
            this.btnRutas.Text = "Generar Rutas";
            this.btnRutas.UseVisualStyleBackColor = false;
            this.btnRutas.Visible = false;
            this.btnRutas.Click += new System.EventHandler(this.btnRutas_Click);
            // 
            // picWaitRutas
            // 
            this.picWaitRutas.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.picWaitRutas.BackColor = System.Drawing.Color.Transparent;
            this.picWaitRutas.Image = global::Estadistica.Properties.Resources.Wait_flower;
            this.picWaitRutas.Location = new System.Drawing.Point(908, 504);
            this.picWaitRutas.Name = "picWaitRutas";
            this.picWaitRutas.Size = new System.Drawing.Size(32, 32);
            this.picWaitRutas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWaitRutas.TabIndex = 226;
            this.picWaitRutas.TabStop = false;
            this.picWaitRutas.Visible = false;
            // 
            // frmRutas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(993, 568);
            this.Controls.Add(this.btnRutas);
            this.Controls.Add(this.picWaitRutas);
            this.Controls.Add(this.dgvGeolocalizacion);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.gbOpciones);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.picWaitBulk);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRutas";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rutas Visitas -  Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRutas_FormClosed);
            this.gbOpciones.ResumeLayout(false);
            this.gbOpciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGeolocalizacion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitRutas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpMesAsignacion;
        private System.Windows.Forms.Label lblFecha;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.GroupBox gbOpciones;
        private System.Windows.Forms.PictureBox picWaitBulk;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ComboBox cmbRegion;
        private System.Windows.Forms.CheckBox chkSCV;
        private System.Windows.Forms.CheckedListBox chkBSegmento;
        private System.Windows.Forms.CheckedListBox chkBProducto;
        private System.Windows.Forms.ComboBox cmbSaldo;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.Label lblLocalidad;
        private System.Windows.Forms.ComboBox cmbCorrInco;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.DataGridView dgvGeolocalizacion;
        private System.Windows.Forms.Button btnRutas;
        private System.Windows.Forms.PictureBox picWaitRutas;
        private System.Windows.Forms.Label lblKm;
        private System.Windows.Forms.ComboBox cmbKilometros;
    }
}