namespace Estadistica

{
    partial class frmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtContraseña = new System.Windows.Forms.TextBox();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.btnAcceder = new System.Windows.Forms.Button();
            this.picConsorcioJurídico = new System.Windows.Forms.PictureBox();
            this.btnSí = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.txtNuevaContraseña = new System.Windows.Forms.TextBox();
            this.txtRepitaContraseña = new System.Windows.Forms.TextBox();
            this.lbl8Dígitos = new System.Windows.Forms.Label();
            this.lblMayúscula = new System.Windows.Forms.Label();
            this.lblMinúscula = new System.Windows.Forms.Label();
            this.lblNúmero = new System.Windows.Forms.Label();
            this.lblSímbolo = new System.Windows.Forms.Label();
            this.lblDiferente = new System.Windows.Forms.Label();
            this.lblIguales = new System.Windows.Forms.Label();
            this.pnlValidación = new System.Windows.Forms.Panel();
            this.lblContador = new System.Windows.Forms.Label();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.lblVersión = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picConsorcioJurídico)).BeginInit();
            this.pnlValidación.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtUsuario.BackColor = System.Drawing.Color.Gainsboro;
            this.txtUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsuario.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtUsuario.ForeColor = System.Drawing.Color.DarkGray;
            this.txtUsuario.Location = new System.Drawing.Point(194, 93);
            this.txtUsuario.MaxLength = 4;
            this.txtUsuario.Multiline = true;
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(167, 16);
            this.txtUsuario.TabIndex = 1;
            this.txtUsuario.Text = "Usuario";
            this.txtUsuario.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtUsuario.Enter += new System.EventHandler(this.txt_Enter);
            this.txtUsuario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUsuario_KeyPress);
            this.txtUsuario.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtContraseña
            // 
            this.txtContraseña.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtContraseña.BackColor = System.Drawing.Color.Gainsboro;
            this.txtContraseña.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContraseña.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtContraseña.ForeColor = System.Drawing.Color.DarkGray;
            this.txtContraseña.Location = new System.Drawing.Point(194, 129);
            this.txtContraseña.MaxLength = 20;
            this.txtContraseña.Name = "txtContraseña";
            this.txtContraseña.Size = new System.Drawing.Size(167, 15);
            this.txtContraseña.TabIndex = 2;
            this.txtContraseña.Text = "Contraseña";
            this.txtContraseña.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtContraseña.Enter += new System.EventHandler(this.txt_Enter);
            this.txtContraseña.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // lblMensaje
            // 
            this.lblMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensaje.BackColor = System.Drawing.Color.Transparent;
            this.lblMensaje.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.Location = new System.Drawing.Point(13, 190);
            this.lblMensaje.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(529, 32);
            this.lblMensaje.TabIndex = 8;
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAcceder
            // 
            this.btnAcceder.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAcceder.BackColor = System.Drawing.Color.White;
            this.btnAcceder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAcceder.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAcceder.ForeColor = System.Drawing.Color.DimGray;
            this.btnAcceder.Location = new System.Drawing.Point(212, 234);
            this.btnAcceder.Name = "btnAcceder";
            this.btnAcceder.Size = new System.Drawing.Size(130, 28);
            this.btnAcceder.TabIndex = 0;
            this.btnAcceder.Text = "Acceder";
            this.btnAcceder.UseVisualStyleBackColor = false;
            this.btnAcceder.Click += new System.EventHandler(this.btnAcceder_Click);
            this.btnAcceder.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnAcceder_KeyDown);
            // 
            // picConsorcioJurídico
            // 
            this.picConsorcioJurídico.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picConsorcioJurídico.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picConsorcioJurídico.InitialImage = ((System.Drawing.Image)(resources.GetObject("picConsorcioJurídico.InitialImage")));
            this.picConsorcioJurídico.Location = new System.Drawing.Point(134, -1);
            this.picConsorcioJurídico.Name = "picConsorcioJurídico";
            this.picConsorcioJurídico.Size = new System.Drawing.Size(287, 88);
            this.picConsorcioJurídico.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picConsorcioJurídico.TabIndex = 5;
            this.picConsorcioJurídico.TabStop = false;
            // 
            // btnSí
            // 
            this.btnSí.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSí.BackColor = System.Drawing.Color.White;
            this.btnSí.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSí.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSí.ForeColor = System.Drawing.Color.Crimson;
            this.btnSí.Location = new System.Drawing.Point(296, 235);
            this.btnSí.Name = "btnSí";
            this.btnSí.Size = new System.Drawing.Size(100, 25);
            this.btnSí.TabIndex = 5;
            this.btnSí.Text = "&Sí";
            this.btnSí.UseVisualStyleBackColor = false;
            this.btnSí.Visible = false;
            this.btnSí.Click += new System.EventHandler(this.btnSí_Click);
            this.btnSí.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnAcceder_KeyDown);
            // 
            // btnNo
            // 
            this.btnNo.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnNo.BackColor = System.Drawing.Color.White;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNo.ForeColor = System.Drawing.Color.DarkGreen;
            this.btnNo.Location = new System.Drawing.Point(158, 235);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(100, 25);
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "&No";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Visible = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            this.btnNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnAcceder_KeyDown);
            // 
            // txtNuevaContraseña
            // 
            this.txtNuevaContraseña.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtNuevaContraseña.BackColor = System.Drawing.Color.Gainsboro;
            this.txtNuevaContraseña.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNuevaContraseña.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtNuevaContraseña.ForeColor = System.Drawing.Color.DarkGray;
            this.txtNuevaContraseña.Location = new System.Drawing.Point(194, 111);
            this.txtNuevaContraseña.MaxLength = 20;
            this.txtNuevaContraseña.Name = "txtNuevaContraseña";
            this.txtNuevaContraseña.Size = new System.Drawing.Size(167, 15);
            this.txtNuevaContraseña.TabIndex = 6;
            this.txtNuevaContraseña.Text = "Nueva contraseña";
            this.txtNuevaContraseña.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtNuevaContraseña.Visible = false;
            this.txtNuevaContraseña.TextChanged += new System.EventHandler(this.txtNuevaContraseña_TextChanged);
            this.txtNuevaContraseña.Enter += new System.EventHandler(this.txt_Enter);
            this.txtNuevaContraseña.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // txtRepitaContraseña
            // 
            this.txtRepitaContraseña.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtRepitaContraseña.BackColor = System.Drawing.Color.Gainsboro;
            this.txtRepitaContraseña.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRepitaContraseña.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtRepitaContraseña.ForeColor = System.Drawing.Color.DarkGray;
            this.txtRepitaContraseña.Location = new System.Drawing.Point(194, 147);
            this.txtRepitaContraseña.MaxLength = 20;
            this.txtRepitaContraseña.Name = "txtRepitaContraseña";
            this.txtRepitaContraseña.Size = new System.Drawing.Size(167, 15);
            this.txtRepitaContraseña.TabIndex = 7;
            this.txtRepitaContraseña.Text = "Repita contraseña";
            this.txtRepitaContraseña.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtRepitaContraseña.Visible = false;
            this.txtRepitaContraseña.TextChanged += new System.EventHandler(this.txtNuevaContraseña_TextChanged);
            this.txtRepitaContraseña.Enter += new System.EventHandler(this.txt_Enter);
            this.txtRepitaContraseña.Leave += new System.EventHandler(this.txt_Leave);
            // 
            // lbl8Dígitos
            // 
            this.lbl8Dígitos.AutoSize = true;
            this.lbl8Dígitos.Location = new System.Drawing.Point(6, 25);
            this.lbl8Dígitos.Name = "lbl8Dígitos";
            this.lbl8Dígitos.Size = new System.Drawing.Size(97, 14);
            this.lbl8Dígitos.TabIndex = 9;
            this.lbl8Dígitos.Text = "✘ 8 caractéres";
            // 
            // lblMayúscula
            // 
            this.lblMayúscula.AutoSize = true;
            this.lblMayúscula.Location = new System.Drawing.Point(6, 44);
            this.lblMayúscula.Name = "lblMayúscula";
            this.lblMayúscula.Size = new System.Drawing.Size(113, 14);
            this.lblMayúscula.TabIndex = 10;
            this.lblMayúscula.Text = "✘ una mayúscula";
            // 
            // lblMinúscula
            // 
            this.lblMinúscula.AutoSize = true;
            this.lblMinúscula.Location = new System.Drawing.Point(6, 63);
            this.lblMinúscula.Name = "lblMinúscula";
            this.lblMinúscula.Size = new System.Drawing.Size(109, 14);
            this.lblMinúscula.TabIndex = 10;
            this.lblMinúscula.Text = "✘ una minúscula";
            // 
            // lblNúmero
            // 
            this.lblNúmero.AutoSize = true;
            this.lblNúmero.Location = new System.Drawing.Point(6, 82);
            this.lblNúmero.Name = "lblNúmero";
            this.lblNúmero.Size = new System.Drawing.Size(87, 14);
            this.lblNúmero.TabIndex = 10;
            this.lblNúmero.Text = "✘ un número";
            // 
            // lblSímbolo
            // 
            this.lblSímbolo.AutoSize = true;
            this.lblSímbolo.Location = new System.Drawing.Point(6, 101);
            this.lblSímbolo.Name = "lblSímbolo";
            this.lblSímbolo.Size = new System.Drawing.Size(87, 14);
            this.lblSímbolo.TabIndex = 10;
            this.lblSímbolo.Text = "✘ un símbolo";
            // 
            // lblDiferente
            // 
            this.lblDiferente.AutoSize = true;
            this.lblDiferente.Location = new System.Drawing.Point(6, 120);
            this.lblDiferente.Name = "lblDiferente";
            this.lblDiferente.Size = new System.Drawing.Size(157, 14);
            this.lblDiferente.TabIndex = 10;
            this.lblDiferente.Text = "✘ diferente a la anterior";
            // 
            // lblIguales
            // 
            this.lblIguales.AutoSize = true;
            this.lblIguales.Location = new System.Drawing.Point(6, 6);
            this.lblIguales.Name = "lblIguales";
            this.lblIguales.Size = new System.Drawing.Size(142, 14);
            this.lblIguales.TabIndex = 11;
            this.lblIguales.Text = "✘ nueva sean iguales";
            // 
            // pnlValidación
            // 
            this.pnlValidación.Controls.Add(this.lblIguales);
            this.pnlValidación.Controls.Add(this.lbl8Dígitos);
            this.pnlValidación.Controls.Add(this.lblDiferente);
            this.pnlValidación.Controls.Add(this.lblMayúscula);
            this.pnlValidación.Controls.Add(this.lblSímbolo);
            this.pnlValidación.Controls.Add(this.lblMinúscula);
            this.pnlValidación.Controls.Add(this.lblNúmero);
            this.pnlValidación.ForeColor = System.Drawing.Color.Crimson;
            this.pnlValidación.Location = new System.Drawing.Point(380, 80);
            this.pnlValidación.Name = "pnlValidación";
            this.pnlValidación.Size = new System.Drawing.Size(169, 140);
            this.pnlValidación.TabIndex = 12;
            this.pnlValidación.Visible = false;
            // 
            // lblContador
            // 
            this.lblContador.AutoSize = true;
            this.lblContador.Location = new System.Drawing.Point(6, 246);
            this.lblContador.Name = "lblContador";
            this.lblContador.Size = new System.Drawing.Size(15, 14);
            this.lblContador.TabIndex = 11;
            this.lblContador.Text = "0";
            // 
            // picWait
            // 
            this.picWait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Location = new System.Drawing.Point(261, 227);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 13;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            this.picWait.Click += new System.EventHandler(this.picWait_Click);
            // 
            // lblVersión
            // 
            this.lblVersión.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersión.ForeColor = System.Drawing.Color.LightGray;
            this.lblVersión.Location = new System.Drawing.Point(470, 235);
            this.lblVersión.Name = "lblVersión";
            this.lblVersión.Size = new System.Drawing.Size(79, 30);
            this.lblVersión.TabIndex = 15;
            this.lblVersión.Text = "z0\r\nv0.0.1";
            this.lblVersión.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmLogin
            // 
            this.AcceptButton = this.btnAcceder;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(554, 271);
            this.Controls.Add(this.lblVersión);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblContador);
            this.Controls.Add(this.pnlValidación);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.txtRepitaContraseña);
            this.Controls.Add(this.txtNuevaContraseña);
            this.Controls.Add(this.picConsorcioJurídico);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnSí);
            this.Controls.Add(this.btnAcceder);
            this.Controls.Add(this.txtContraseña);
            this.Controls.Add(this.txtUsuario);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Estadística";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.picConsorcioJurídico)).EndInit();
            this.pnlValidación.ResumeLayout(false);
            this.pnlValidación.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtContraseña;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btnAcceder;
        private System.Windows.Forms.PictureBox picConsorcioJurídico;
        private System.Windows.Forms.Button btnSí;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.TextBox txtNuevaContraseña;
        private System.Windows.Forms.TextBox txtRepitaContraseña;
        private System.Windows.Forms.Label lbl8Dígitos;
        private System.Windows.Forms.Label lblMayúscula;
        private System.Windows.Forms.Label lblMinúscula;
        private System.Windows.Forms.Label lblNúmero;
        private System.Windows.Forms.Label lblSímbolo;
        private System.Windows.Forms.Label lblDiferente;
        private System.Windows.Forms.Label lblIguales;
        private System.Windows.Forms.Panel pnlValidación;
        private System.Windows.Forms.Label lblContador;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Label lblVersión;
    }
}

