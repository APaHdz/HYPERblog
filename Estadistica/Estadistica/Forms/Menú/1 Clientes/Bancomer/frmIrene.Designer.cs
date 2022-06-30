namespace Estadistica
{
    partial class frmIrene
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIrene));
            this.dtpInicial = new System.Windows.Forms.DateTimePicker();
            this.lblInicio = new System.Windows.Forms.Label();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.picWaitBulk = new System.Windows.Forms.PictureBox();
            this.lblInstrucciones = new System.Windows.Forms.Label();
            this.lblRegistros = new System.Windows.Forms.Label();
            this.btnCargar = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.dgvIrene = new System.Windows.Forms.DataGridView();
            this.Quitar = new System.Windows.Forms.DataGridViewImageColumn();
            this.NumReg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prioridad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FGestion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Despacho = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoLlamada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoGestion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.idCuenta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prestamo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoCredito = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Portafolio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ciclo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mora = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreDeudor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Calle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Colonia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Delegacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Entidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TelCasa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TelOficina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsuarioPlantilla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreEjecutivo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupervisorPlantilla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gerencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Region = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Plaza = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fecha_Insert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Segundo_Insert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comentarios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comentario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FechaPP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MontoNegociado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CausaNoPago = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UsuarioPlantilla2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NombreEjecutivo2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupervisorPlantilla2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PlazaPLantilla = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Omitir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpFinal = new System.Windows.Forms.DateTimePicker();
            this.lblFinal = new System.Windows.Forms.Label();
            this.picWaitConsulta = new System.Windows.Forms.PictureBox();
            this.btnConsulta = new System.Windows.Forms.Button();
            this.txtComentario = new System.Windows.Forms.TextBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIrene)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitConsulta)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpInicial
            // 
            this.dtpInicial.CustomFormat = "MM/yyyy";
            this.dtpInicial.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpInicial.Location = new System.Drawing.Point(271, 12);
            this.dtpInicial.Name = "dtpInicial";
            this.dtpInicial.Size = new System.Drawing.Size(112, 26);
            this.dtpInicial.TabIndex = 218;
            // 
            // lblInicio
            // 
            this.lblInicio.AutoSize = true;
            this.lblInicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblInicio.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblInicio.ForeColor = System.Drawing.Color.DimGray;
            this.lblInicio.Location = new System.Drawing.Point(225, 19);
            this.lblInicio.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblInicio.Name = "lblInicio";
            this.lblInicio.Size = new System.Drawing.Size(37, 16);
            this.lblInicio.TabIndex = 217;
            this.lblInicio.Text = "Inicio";
            // 
            // picLogo
            // 
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(11, 12);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(178, 62);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 216;
            this.picLogo.TabStop = false;
            // 
            // picWaitBulk
            // 
            this.picWaitBulk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.picWaitBulk.BackColor = System.Drawing.Color.Transparent;
            this.picWaitBulk.Location = new System.Drawing.Point(456, 538);
            this.picWaitBulk.Name = "picWaitBulk";
            this.picWaitBulk.Size = new System.Drawing.Size(32, 32);
            this.picWaitBulk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWaitBulk.TabIndex = 224;
            this.picWaitBulk.TabStop = false;
            this.picWaitBulk.Visible = false;
            // 
            // lblInstrucciones
            // 
            this.lblInstrucciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInstrucciones.BackColor = System.Drawing.Color.White;
            this.lblInstrucciones.Location = new System.Drawing.Point(8, 548);
            this.lblInstrucciones.Name = "lblInstrucciones";
            this.lblInstrucciones.Size = new System.Drawing.Size(353, 15);
            this.lblInstrucciones.TabIndex = 222;
            // 
            // lblRegistros
            // 
            this.lblRegistros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRegistros.BackColor = System.Drawing.Color.White;
            this.lblRegistros.Location = new System.Drawing.Point(577, 543);
            this.lblRegistros.Name = "lblRegistros";
            this.lblRegistros.Size = new System.Drawing.Size(357, 15);
            this.lblRegistros.TabIndex = 221;
            this.lblRegistros.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCargar
            // 
            this.btnCargar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCargar.BackColor = System.Drawing.Color.SlateGray;
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCargar.Location = new System.Drawing.Point(415, 543);
            this.btnCargar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(111, 27);
            this.btnCargar.TabIndex = 223;
            this.btnCargar.Text = "Cargar";
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
            this.lblMensajes.Location = new System.Drawing.Point(3, 582);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(931, 20);
            this.lblMensajes.TabIndex = 220;
            this.lblMensajes.Text = "Resultado";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvIrene
            // 
            this.dgvIrene.AllowUserToAddRows = false;
            this.dgvIrene.AllowUserToDeleteRows = false;
            this.dgvIrene.AllowUserToResizeRows = false;
            this.dgvIrene.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvIrene.BackgroundColor = System.Drawing.Color.White;
            this.dgvIrene.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvIrene.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvIrene.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSlateGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIrene.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvIrene.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIrene.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Quitar,
            this.NumReg,
            this.Prioridad,
            this.FGestion,
            this.Cliente,
            this.Despacho,
            this.Tipo,
            this.TipoLlamada,
            this.TipoGestion,
            this.idCuenta,
            this.Prestamo,
            this.TipoCredito,
            this.Portafolio,
            this.Ciclo,
            this.Mora,
            this.NombreDeudor,
            this.Calle,
            this.Colonia,
            this.CP,
            this.Delegacion,
            this.Entidad,
            this.TelCasa,
            this.TelOficina,
            this.UsuarioPlantilla,
            this.NombreEjecutivo,
            this.SupervisorPlantilla,
            this.Gerencia,
            this.Region,
            this.Plaza,
            this.Fecha_Insert,
            this.Segundo_Insert,
            this.Comentarios,
            this.Comentario,
            this.RR,
            this.CR,
            this.CA,
            this.FechaPP,
            this.MontoNegociado,
            this.CausaNoPago,
            this.UsuarioPlantilla2,
            this.NombreEjecutivo2,
            this.SupervisorPlantilla2,
            this.PlazaPLantilla,
            this.Omitir});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvIrene.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvIrene.EnableHeadersVisualStyles = false;
            this.dgvIrene.GridColor = System.Drawing.Color.LightGray;
            this.dgvIrene.Location = new System.Drawing.Point(6, 80);
            this.dgvIrene.MultiSelect = false;
            this.dgvIrene.Name = "dgvIrene";
            this.dgvIrene.ReadOnly = true;
            this.dgvIrene.RowHeadersVisible = false;
            this.dgvIrene.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIrene.Size = new System.Drawing.Size(923, 347);
            this.dgvIrene.StandardTab = true;
            this.dgvIrene.TabIndex = 219;
            this.dgvIrene.Visible = false;
            this.dgvIrene.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvIrene_CellContentClick);
            this.dgvIrene.SelectionChanged += new System.EventHandler(this.dgvIrene_SelectionChanged);
            // 
            // Quitar
            // 
            this.Quitar.HeaderText = "Quitar";
            this.Quitar.Image = global::Estadistica.Properties.Resources.Cross_26;
            this.Quitar.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Quitar.Name = "Quitar";
            this.Quitar.ReadOnly = true;
            this.Quitar.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Quitar.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // NumReg
            // 
            this.NumReg.DataPropertyName = "NumReg";
            this.NumReg.HeaderText = "NumReg";
            this.NumReg.Name = "NumReg";
            this.NumReg.ReadOnly = true;
            // 
            // Prioridad
            // 
            this.Prioridad.DataPropertyName = "Prioridad";
            this.Prioridad.HeaderText = "Prioridad";
            this.Prioridad.Name = "Prioridad";
            this.Prioridad.ReadOnly = true;
            // 
            // FGestion
            // 
            this.FGestion.DataPropertyName = "FGestion";
            this.FGestion.HeaderText = "FGestion";
            this.FGestion.Name = "FGestion";
            this.FGestion.ReadOnly = true;
            // 
            // Cliente
            // 
            this.Cliente.DataPropertyName = "Cliente";
            this.Cliente.HeaderText = "Cliente";
            this.Cliente.Name = "Cliente";
            this.Cliente.ReadOnly = true;
            // 
            // Despacho
            // 
            this.Despacho.DataPropertyName = "Despacho";
            this.Despacho.HeaderText = "Despacho";
            this.Despacho.Name = "Despacho";
            this.Despacho.ReadOnly = true;
            // 
            // Tipo
            // 
            this.Tipo.DataPropertyName = "Tipo";
            this.Tipo.HeaderText = "Tipo";
            this.Tipo.Name = "Tipo";
            this.Tipo.ReadOnly = true;
            // 
            // TipoLlamada
            // 
            this.TipoLlamada.DataPropertyName = "TipoLlamada";
            this.TipoLlamada.HeaderText = "TipoLlamada";
            this.TipoLlamada.Name = "TipoLlamada";
            this.TipoLlamada.ReadOnly = true;
            // 
            // TipoGestion
            // 
            this.TipoGestion.DataPropertyName = "TipoGestion";
            this.TipoGestion.HeaderText = "TipoGestion";
            this.TipoGestion.Name = "TipoGestion";
            this.TipoGestion.ReadOnly = true;
            // 
            // idCuenta
            // 
            this.idCuenta.DataPropertyName = "idCuenta";
            this.idCuenta.HeaderText = "idCuenta";
            this.idCuenta.Name = "idCuenta";
            this.idCuenta.ReadOnly = true;
            // 
            // Prestamo
            // 
            this.Prestamo.DataPropertyName = "Prestamo";
            this.Prestamo.HeaderText = "Prestamo";
            this.Prestamo.Name = "Prestamo";
            this.Prestamo.ReadOnly = true;
            // 
            // TipoCredito
            // 
            this.TipoCredito.DataPropertyName = "TipoCredito";
            this.TipoCredito.HeaderText = "TipoCredito";
            this.TipoCredito.Name = "TipoCredito";
            this.TipoCredito.ReadOnly = true;
            // 
            // Portafolio
            // 
            this.Portafolio.DataPropertyName = "Portafolio";
            this.Portafolio.HeaderText = "Portafolio";
            this.Portafolio.Name = "Portafolio";
            this.Portafolio.ReadOnly = true;
            // 
            // Ciclo
            // 
            this.Ciclo.DataPropertyName = "Ciclo";
            this.Ciclo.HeaderText = "Ciclo";
            this.Ciclo.Name = "Ciclo";
            this.Ciclo.ReadOnly = true;
            // 
            // Mora
            // 
            this.Mora.DataPropertyName = "Mora";
            this.Mora.HeaderText = "Mora";
            this.Mora.Name = "Mora";
            this.Mora.ReadOnly = true;
            // 
            // NombreDeudor
            // 
            this.NombreDeudor.DataPropertyName = "NombreDeudor";
            this.NombreDeudor.HeaderText = "NombreDeudor";
            this.NombreDeudor.Name = "NombreDeudor";
            this.NombreDeudor.ReadOnly = true;
            // 
            // Calle
            // 
            this.Calle.DataPropertyName = "Calle";
            this.Calle.HeaderText = "Calle";
            this.Calle.Name = "Calle";
            this.Calle.ReadOnly = true;
            // 
            // Colonia
            // 
            this.Colonia.DataPropertyName = "Colonia";
            this.Colonia.HeaderText = "Colonia";
            this.Colonia.Name = "Colonia";
            this.Colonia.ReadOnly = true;
            // 
            // CP
            // 
            this.CP.DataPropertyName = "CP";
            this.CP.HeaderText = "CP";
            this.CP.Name = "CP";
            this.CP.ReadOnly = true;
            // 
            // Delegacion
            // 
            this.Delegacion.DataPropertyName = "Delegacion";
            this.Delegacion.HeaderText = "Delegacion";
            this.Delegacion.Name = "Delegacion";
            this.Delegacion.ReadOnly = true;
            // 
            // Entidad
            // 
            this.Entidad.DataPropertyName = "Entidad";
            this.Entidad.HeaderText = "Entidad";
            this.Entidad.Name = "Entidad";
            this.Entidad.ReadOnly = true;
            // 
            // TelCasa
            // 
            this.TelCasa.DataPropertyName = "TelCasa";
            this.TelCasa.HeaderText = "TelCasa";
            this.TelCasa.Name = "TelCasa";
            this.TelCasa.ReadOnly = true;
            // 
            // TelOficina
            // 
            this.TelOficina.DataPropertyName = "TelOficina";
            this.TelOficina.HeaderText = "TelOficina";
            this.TelOficina.Name = "TelOficina";
            this.TelOficina.ReadOnly = true;
            // 
            // UsuarioPlantilla
            // 
            this.UsuarioPlantilla.DataPropertyName = "UsuarioPlantilla";
            this.UsuarioPlantilla.HeaderText = "UsuarioPlantilla";
            this.UsuarioPlantilla.Name = "UsuarioPlantilla";
            this.UsuarioPlantilla.ReadOnly = true;
            // 
            // NombreEjecutivo
            // 
            this.NombreEjecutivo.DataPropertyName = "NombreEjecutivo";
            this.NombreEjecutivo.HeaderText = "NombreEjecutivo";
            this.NombreEjecutivo.Name = "NombreEjecutivo";
            this.NombreEjecutivo.ReadOnly = true;
            // 
            // SupervisorPlantilla
            // 
            this.SupervisorPlantilla.DataPropertyName = "SupervisorPlantilla";
            this.SupervisorPlantilla.HeaderText = "SupervisorPlantilla";
            this.SupervisorPlantilla.Name = "SupervisorPlantilla";
            this.SupervisorPlantilla.ReadOnly = true;
            // 
            // Gerencia
            // 
            this.Gerencia.DataPropertyName = "Gerencia";
            this.Gerencia.HeaderText = "Gerencia";
            this.Gerencia.Name = "Gerencia";
            this.Gerencia.ReadOnly = true;
            // 
            // Region
            // 
            this.Region.DataPropertyName = "Region";
            this.Region.HeaderText = "Region";
            this.Region.Name = "Region";
            this.Region.ReadOnly = true;
            // 
            // Plaza
            // 
            this.Plaza.DataPropertyName = "Plaza";
            this.Plaza.HeaderText = "Plaza";
            this.Plaza.Name = "Plaza";
            this.Plaza.ReadOnly = true;
            // 
            // Fecha_Insert
            // 
            this.Fecha_Insert.DataPropertyName = "Fecha_Insert";
            this.Fecha_Insert.HeaderText = "Fecha_Insert";
            this.Fecha_Insert.Name = "Fecha_Insert";
            this.Fecha_Insert.ReadOnly = true;
            // 
            // Segundo_Insert
            // 
            this.Segundo_Insert.DataPropertyName = "Segundo_Insert";
            this.Segundo_Insert.HeaderText = "Segundo_Insert";
            this.Segundo_Insert.Name = "Segundo_Insert";
            this.Segundo_Insert.ReadOnly = true;
            // 
            // Comentarios
            // 
            this.Comentarios.DataPropertyName = "Comentarios";
            this.Comentarios.HeaderText = "Comentarios";
            this.Comentarios.Name = "Comentarios";
            this.Comentarios.ReadOnly = true;
            // 
            // Comentario
            // 
            this.Comentario.DataPropertyName = "Comentario";
            this.Comentario.HeaderText = "Comentario";
            this.Comentario.Name = "Comentario";
            this.Comentario.ReadOnly = true;
            // 
            // RR
            // 
            this.RR.DataPropertyName = "RR";
            this.RR.HeaderText = "RR";
            this.RR.Name = "RR";
            this.RR.ReadOnly = true;
            // 
            // CR
            // 
            this.CR.DataPropertyName = "CR";
            this.CR.HeaderText = "CR";
            this.CR.Name = "CR";
            this.CR.ReadOnly = true;
            // 
            // CA
            // 
            this.CA.DataPropertyName = "CA";
            this.CA.HeaderText = "CA";
            this.CA.Name = "CA";
            this.CA.ReadOnly = true;
            // 
            // FechaPP
            // 
            this.FechaPP.DataPropertyName = "FechaPP";
            this.FechaPP.HeaderText = "FechaPP";
            this.FechaPP.Name = "FechaPP";
            this.FechaPP.ReadOnly = true;
            // 
            // MontoNegociado
            // 
            this.MontoNegociado.DataPropertyName = "MontoNegociado";
            this.MontoNegociado.HeaderText = "MontoNegociado";
            this.MontoNegociado.Name = "MontoNegociado";
            this.MontoNegociado.ReadOnly = true;
            // 
            // CausaNoPago
            // 
            this.CausaNoPago.DataPropertyName = "CausaNoPago";
            this.CausaNoPago.HeaderText = "CausaNoPago";
            this.CausaNoPago.Name = "CausaNoPago";
            this.CausaNoPago.ReadOnly = true;
            // 
            // UsuarioPlantilla2
            // 
            this.UsuarioPlantilla2.DataPropertyName = "UsuarioPlantilla2";
            this.UsuarioPlantilla2.HeaderText = "UsuarioPlantilla2";
            this.UsuarioPlantilla2.Name = "UsuarioPlantilla2";
            this.UsuarioPlantilla2.ReadOnly = true;
            // 
            // NombreEjecutivo2
            // 
            this.NombreEjecutivo2.DataPropertyName = "NombreEjecutivo2";
            this.NombreEjecutivo2.HeaderText = "NombreEjecutivo2";
            this.NombreEjecutivo2.Name = "NombreEjecutivo2";
            this.NombreEjecutivo2.ReadOnly = true;
            // 
            // SupervisorPlantilla2
            // 
            this.SupervisorPlantilla2.DataPropertyName = "SupervisorPlantilla2";
            this.SupervisorPlantilla2.HeaderText = "SupervisorPlantilla2";
            this.SupervisorPlantilla2.Name = "SupervisorPlantilla2";
            this.SupervisorPlantilla2.ReadOnly = true;
            // 
            // PlazaPLantilla
            // 
            this.PlazaPLantilla.DataPropertyName = "PlazaPLantilla";
            this.PlazaPLantilla.HeaderText = "PlazaPLantilla";
            this.PlazaPLantilla.Name = "PlazaPLantilla";
            this.PlazaPLantilla.ReadOnly = true;
            // 
            // Omitir
            // 
            this.Omitir.DataPropertyName = "Omitir";
            this.Omitir.HeaderText = "Omitir";
            this.Omitir.Name = "Omitir";
            this.Omitir.ReadOnly = true;
            // 
            // dtpFinal
            // 
            this.dtpFinal.CustomFormat = "MM/yyyy";
            this.dtpFinal.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFinal.Location = new System.Drawing.Point(271, 44);
            this.dtpFinal.Name = "dtpFinal";
            this.dtpFinal.Size = new System.Drawing.Size(112, 26);
            this.dtpFinal.TabIndex = 226;
            // 
            // lblFinal
            // 
            this.lblFinal.AutoSize = true;
            this.lblFinal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblFinal.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblFinal.ForeColor = System.Drawing.Color.DimGray;
            this.lblFinal.Location = new System.Drawing.Point(225, 51);
            this.lblFinal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFinal.Name = "lblFinal";
            this.lblFinal.Size = new System.Drawing.Size(34, 16);
            this.lblFinal.TabIndex = 225;
            this.lblFinal.Text = "Final";
            // 
            // picWaitConsulta
            // 
            this.picWaitConsulta.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.picWaitConsulta.BackColor = System.Drawing.Color.Transparent;
            this.picWaitConsulta.Location = new System.Drawing.Point(456, 35);
            this.picWaitConsulta.Name = "picWaitConsulta";
            this.picWaitConsulta.Size = new System.Drawing.Size(32, 32);
            this.picWaitConsulta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWaitConsulta.TabIndex = 228;
            this.picWaitConsulta.TabStop = false;
            this.picWaitConsulta.Visible = false;
            // 
            // btnConsulta
            // 
            this.btnConsulta.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConsulta.BackColor = System.Drawing.Color.SlateGray;
            this.btnConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsulta.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsulta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnConsulta.Location = new System.Drawing.Point(415, 40);
            this.btnConsulta.Margin = new System.Windows.Forms.Padding(2);
            this.btnConsulta.Name = "btnConsulta";
            this.btnConsulta.Size = new System.Drawing.Size(111, 27);
            this.btnConsulta.TabIndex = 227;
            this.btnConsulta.Text = "Consultar";
            this.btnConsulta.UseVisualStyleBackColor = false;
            this.btnConsulta.Click += new System.EventHandler(this.btnConsulta_Click);
            // 
            // txtComentario
            // 
            this.txtComentario.Font = new System.Drawing.Font("Lucida Sans Unicode", 10F, System.Drawing.FontStyle.Bold);
            this.txtComentario.Location = new System.Drawing.Point(12, 433);
            this.txtComentario.Multiline = true;
            this.txtComentario.Name = "txtComentario";
            this.txtComentario.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComentario.Size = new System.Drawing.Size(922, 99);
            this.txtComentario.TabIndex = 229;
            this.txtComentario.Visible = false;
            // 
            // sfdExcel
            // 
            this.sfdExcel.DefaultExt = "xlsx";
            this.sfdExcel.Filter = "Libro de Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
            this.sfdExcel.Title = "Guardar libro de Excel";
            // 
            // frmIrene
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(948, 611);
            this.Controls.Add(this.txtComentario);
            this.Controls.Add(this.picWaitConsulta);
            this.Controls.Add(this.btnConsulta);
            this.Controls.Add(this.dtpFinal);
            this.Controls.Add(this.lblFinal);
            this.Controls.Add(this.picWaitBulk);
            this.Controls.Add(this.lblInstrucciones);
            this.Controls.Add(this.lblRegistros);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.dgvIrene);
            this.Controls.Add(this.dtpInicial);
            this.Controls.Add(this.lblInicio);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIrene";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Irene - Estadistica";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIrene)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitConsulta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpInicial;
        private System.Windows.Forms.Label lblInicio;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.PictureBox picWaitBulk;
        private System.Windows.Forms.Label lblInstrucciones;
        private System.Windows.Forms.Label lblRegistros;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.DataGridView dgvIrene;
        private System.Windows.Forms.DateTimePicker dtpFinal;
        private System.Windows.Forms.Label lblFinal;
        private System.Windows.Forms.PictureBox picWaitConsulta;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.TextBox txtComentario;
        private System.Windows.Forms.DataGridViewImageColumn Quitar;
        private System.Windows.Forms.DataGridViewTextBoxColumn NumReg;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prioridad;
        private System.Windows.Forms.DataGridViewTextBoxColumn FGestion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn Despacho;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoLlamada;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoGestion;
        private System.Windows.Forms.DataGridViewTextBoxColumn idCuenta;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prestamo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoCredito;
        private System.Windows.Forms.DataGridViewTextBoxColumn Portafolio;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ciclo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mora;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreDeudor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Calle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Colonia;
        private System.Windows.Forms.DataGridViewTextBoxColumn CP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Delegacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Entidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn TelCasa;
        private System.Windows.Forms.DataGridViewTextBoxColumn TelOficina;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsuarioPlantilla;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreEjecutivo;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupervisorPlantilla;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gerencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Region;
        private System.Windows.Forms.DataGridViewTextBoxColumn Plaza;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fecha_Insert;
        private System.Windows.Forms.DataGridViewTextBoxColumn Segundo_Insert;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comentarios;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comentario;
        private System.Windows.Forms.DataGridViewTextBoxColumn RR;
        private System.Windows.Forms.DataGridViewTextBoxColumn CR;
        private System.Windows.Forms.DataGridViewTextBoxColumn CA;
        private System.Windows.Forms.DataGridViewTextBoxColumn FechaPP;
        private System.Windows.Forms.DataGridViewTextBoxColumn MontoNegociado;
        private System.Windows.Forms.DataGridViewTextBoxColumn CausaNoPago;
        private System.Windows.Forms.DataGridViewTextBoxColumn UsuarioPlantilla2;
        private System.Windows.Forms.DataGridViewTextBoxColumn NombreEjecutivo2;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupervisorPlantilla2;
        private System.Windows.Forms.DataGridViewTextBoxColumn PlazaPLantilla;
        private System.Windows.Forms.DataGridViewTextBoxColumn Omitir;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
    }
}