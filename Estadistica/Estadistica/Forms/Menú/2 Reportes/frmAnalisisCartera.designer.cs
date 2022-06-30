namespace Estadistica
{
    partial class frmAnalisisCartera
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnalisisCartera));
            this.btnConsulta = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.sfdExcel = new System.Windows.Forms.SaveFileDialog();
            this.cmbCarteras = new System.Windows.Forms.ComboBox();
            this.lblCartera = new System.Windows.Forms.Label();
            this.lblProducto = new System.Windows.Forms.Label();
            this.cmbProductos = new System.Windows.Forms.ComboBox();
            this.lblSaldo = new System.Windows.Forms.Label();
            this.txtSaldo = new System.Windows.Forms.TextBox();
            this.chkNivel = new System.Windows.Forms.CheckedListBox();
            this.txtRegistros = new System.Windows.Forms.TextBox();
            this.lblRegistros = new System.Windows.Forms.Label();
            this.chkNegociadas = new System.Windows.Forms.CheckBox();
            this.chkCorreos = new System.Windows.Forms.CheckBox();
            this.chkDomicilios = new System.Windows.Forms.CheckBox();
            this.chkTelefonos = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConsulta
            // 
            this.btnConsulta.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConsulta.BackColor = System.Drawing.Color.SlateGray;
            this.btnConsulta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsulta.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsulta.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnConsulta.Location = new System.Drawing.Point(94, 270);
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
            this.lblSaldo.Size = new System.Drawing.Size(39, 16);
            this.lblSaldo.TabIndex = 222;
            this.lblSaldo.Text = "Saldo";
            // 
            // txtSaldo
            // 
            this.txtSaldo.BackColor = System.Drawing.Color.Gainsboro;
            this.txtSaldo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSaldo.Location = new System.Drawing.Point(65, 143);
            this.txtSaldo.Name = "txtSaldo";
            this.txtSaldo.Size = new System.Drawing.Size(69, 19);
            this.txtSaldo.TabIndex = 223;
            this.txtSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkNivel
            // 
            this.chkNivel.BackColor = System.Drawing.Color.Gainsboro;
            this.chkNivel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkNivel.CheckOnClick = true;
            this.chkNivel.FormattingEnabled = true;
            this.chkNivel.Items.AddRange(new object[] {
            "Localización",
            "Convencimiento",
            "Acuerdo",
            "Definición"});
            this.chkNivel.Location = new System.Drawing.Point(140, 143);
            this.chkNivel.MultiColumn = true;
            this.chkNivel.Name = "chkNivel";
            this.chkNivel.Size = new System.Drawing.Size(122, 84);
            this.chkNivel.TabIndex = 229;
            // 
            // txtRegistros
            // 
            this.txtRegistros.BackColor = System.Drawing.Color.Gainsboro;
            this.txtRegistros.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRegistros.Location = new System.Drawing.Point(65, 168);
            this.txtRegistros.Name = "txtRegistros";
            this.txtRegistros.Size = new System.Drawing.Size(69, 19);
            this.txtRegistros.TabIndex = 231;
            this.txtRegistros.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblRegistros
            // 
            this.lblRegistros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lblRegistros.AutoSize = true;
            this.lblRegistros.Location = new System.Drawing.Point(8, 168);
            this.lblRegistros.Name = "lblRegistros";
            this.lblRegistros.Size = new System.Drawing.Size(61, 16);
            this.lblRegistros.TabIndex = 230;
            this.lblRegistros.Text = "Registros";
            // 
            // chkNegociadas
            // 
            this.chkNegociadas.AutoSize = true;
            this.chkNegociadas.BackColor = System.Drawing.Color.Gainsboro;
            this.chkNegociadas.Location = new System.Drawing.Point(40, 193);
            this.chkNegociadas.Name = "chkNegociadas";
            this.chkNegociadas.Size = new System.Drawing.Size(94, 20);
            this.chkNegociadas.TabIndex = 232;
            this.chkNegociadas.Text = "Negociadas";
            this.chkNegociadas.UseVisualStyleBackColor = false;
            // 
            // chkCorreos
            // 
            this.chkCorreos.AutoSize = true;
            this.chkCorreos.Location = new System.Drawing.Point(13, 235);
            this.chkCorreos.Name = "chkCorreos";
            this.chkCorreos.Size = new System.Drawing.Size(72, 20);
            this.chkCorreos.TabIndex = 233;
            this.chkCorreos.Text = "Correos";
            this.chkCorreos.UseVisualStyleBackColor = true;
            // 
            // chkDomicilios
            // 
            this.chkDomicilios.AutoSize = true;
            this.chkDomicilios.Location = new System.Drawing.Point(91, 235);
            this.chkDomicilios.Name = "chkDomicilios";
            this.chkDomicilios.Size = new System.Drawing.Size(85, 20);
            this.chkDomicilios.TabIndex = 234;
            this.chkDomicilios.Text = "Domicilios";
            this.chkDomicilios.UseVisualStyleBackColor = true;
            // 
            // chkTelefonos
            // 
            this.chkTelefonos.AutoSize = true;
            this.chkTelefonos.Location = new System.Drawing.Point(182, 235);
            this.chkTelefonos.Name = "chkTelefonos";
            this.chkTelefonos.Size = new System.Drawing.Size(83, 20);
            this.chkTelefonos.TabIndex = 235;
            this.chkTelefonos.Text = "Teléfonos";
            this.chkTelefonos.UseVisualStyleBackColor = true;
            // 
            // frmAnalisisCartera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(274, 342);
            this.Controls.Add(this.chkTelefonos);
            this.Controls.Add(this.chkDomicilios);
            this.Controls.Add(this.chkCorreos);
            this.Controls.Add(this.chkNegociadas);
            this.Controls.Add(this.txtRegistros);
            this.Controls.Add(this.lblRegistros);
            this.Controls.Add(this.chkNivel);
            this.Controls.Add(this.txtSaldo);
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
            this.Name = "frmAnalisisCartera";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Analisis cartera - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBotonera_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Button btnConsulta;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.SaveFileDialog sfdExcel;
        private System.Windows.Forms.ComboBox cmbCarteras;
        private System.Windows.Forms.Label lblCartera;
        private System.Windows.Forms.Label lblProducto;
        private System.Windows.Forms.ComboBox cmbProductos;
        private System.Windows.Forms.Label lblSaldo;
        private System.Windows.Forms.TextBox txtSaldo;
        private System.Windows.Forms.CheckedListBox chkNivel;
        private System.Windows.Forms.TextBox txtRegistros;
        private System.Windows.Forms.Label lblRegistros;
        private System.Windows.Forms.CheckBox chkNegociadas;
        private System.Windows.Forms.CheckBox chkCorreos;
        private System.Windows.Forms.CheckBox chkDomicilios;
        private System.Windows.Forms.CheckBox chkTelefonos;
    }
}