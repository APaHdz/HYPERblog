﻿namespace Estadistica
{
    partial class frmCalendario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalendario));
            this.lblInstrucciones = new System.Windows.Forms.Label();
            this.lblRegistros = new System.Windows.Forms.Label();
            this.btnCargar = new System.Windows.Forms.Button();
            this.lblMensajes = new System.Windows.Forms.Label();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.btnArchivo = new System.Windows.Forms.Button();
            this.dgvCalendario = new System.Windows.Forms.DataGridView();
            this.picWaitBulk = new System.Windows.Forms.PictureBox();
            this.picWait = new System.Windows.Forms.PictureBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.ofdArchivo = new System.Windows.Forms.OpenFileDialog();
            this.dtpCalendario = new System.Windows.Forms.DateTimePicker();
            this.lblMesCalendario = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendario)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInstrucciones
            // 
            this.lblInstrucciones.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblInstrucciones.BackColor = System.Drawing.Color.White;
            this.lblInstrucciones.Location = new System.Drawing.Point(16, 559);
            this.lblInstrucciones.Name = "lblInstrucciones";
            this.lblInstrucciones.Size = new System.Drawing.Size(353, 15);
            this.lblInstrucciones.TabIndex = 190;
            // 
            // lblRegistros
            // 
            this.lblRegistros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRegistros.BackColor = System.Drawing.Color.White;
            this.lblRegistros.Location = new System.Drawing.Point(583, 559);
            this.lblRegistros.Name = "lblRegistros";
            this.lblRegistros.Size = new System.Drawing.Size(357, 15);
            this.lblRegistros.TabIndex = 189;
            this.lblRegistros.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCargar
            // 
            this.btnCargar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCargar.BackColor = System.Drawing.Color.SlateGray;
            this.btnCargar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCargar.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCargar.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnCargar.Location = new System.Drawing.Point(423, 547);
            this.btnCargar.Margin = new System.Windows.Forms.Padding(2);
            this.btnCargar.Name = "btnCargar";
            this.btnCargar.Size = new System.Drawing.Size(111, 27);
            this.btnCargar.TabIndex = 191;
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
            this.lblMensajes.Location = new System.Drawing.Point(9, 583);
            this.lblMensajes.Name = "lblMensajes";
            this.lblMensajes.Size = new System.Drawing.Size(931, 20);
            this.lblMensajes.TabIndex = 188;
            this.lblMensajes.Text = "Resultado";
            this.lblMensajes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRuta
            // 
            this.txtRuta.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRuta.BackColor = System.Drawing.Color.Gainsboro;
            this.txtRuta.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRuta.Enabled = false;
            this.txtRuta.ForeColor = System.Drawing.Color.DimGray;
            this.txtRuta.Location = new System.Drawing.Point(16, 79);
            this.txtRuta.MaxLength = 10000;
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.ReadOnly = true;
            this.txtRuta.Size = new System.Drawing.Size(808, 19);
            this.txtRuta.TabIndex = 186;
            this.txtRuta.Text = "Seleccione Archivo";
            // 
            // btnArchivo
            // 
            this.btnArchivo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnArchivo.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.btnArchivo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnArchivo.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnArchivo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnArchivo.Location = new System.Drawing.Point(829, 74);
            this.btnArchivo.Margin = new System.Windows.Forms.Padding(2);
            this.btnArchivo.Name = "btnArchivo";
            this.btnArchivo.Size = new System.Drawing.Size(111, 27);
            this.btnArchivo.TabIndex = 187;
            this.btnArchivo.Text = "Archivo";
            this.btnArchivo.UseVisualStyleBackColor = false;
            this.btnArchivo.Click += new System.EventHandler(this.btnArchivo_Click);
            // 
            // dgvCalendario
            // 
            this.dgvCalendario.AllowUserToAddRows = false;
            this.dgvCalendario.AllowUserToDeleteRows = false;
            this.dgvCalendario.AllowUserToResizeRows = false;
            this.dgvCalendario.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCalendario.BackgroundColor = System.Drawing.Color.White;
            this.dgvCalendario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCalendario.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCalendario.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSlateGray;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCalendario.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCalendario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCalendario.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCalendario.EnableHeadersVisualStyles = false;
            this.dgvCalendario.GridColor = System.Drawing.Color.LightGray;
            this.dgvCalendario.Location = new System.Drawing.Point(17, 106);
            this.dgvCalendario.MultiSelect = false;
            this.dgvCalendario.Name = "dgvCalendario";
            this.dgvCalendario.ReadOnly = true;
            this.dgvCalendario.RowHeadersVisible = false;
            this.dgvCalendario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvCalendario.Size = new System.Drawing.Size(923, 436);
            this.dgvCalendario.StandardTab = true;
            this.dgvCalendario.TabIndex = 185;
            this.dgvCalendario.Visible = false;
            // 
            // picWaitBulk
            // 
            this.picWaitBulk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.picWaitBulk.BackColor = System.Drawing.Color.Transparent;
            this.picWaitBulk.Location = new System.Drawing.Point(463, 548);
            this.picWaitBulk.Name = "picWaitBulk";
            this.picWaitBulk.Size = new System.Drawing.Size(32, 32);
            this.picWaitBulk.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWaitBulk.TabIndex = 193;
            this.picWaitBulk.TabStop = false;
            this.picWaitBulk.Visible = false;
            // 
            // picWait
            // 
            this.picWait.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picWait.BackColor = System.Drawing.Color.Transparent;
            this.picWait.Location = new System.Drawing.Point(463, 297);
            this.picWait.Name = "picWait";
            this.picWait.Size = new System.Drawing.Size(32, 32);
            this.picWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picWait.TabIndex = 192;
            this.picWait.TabStop = false;
            this.picWait.Visible = false;
            // 
            // picLogo
            // 
            this.picLogo.Image = global::Estadistica.Properties.Resources.ConsorcioLetras;
            this.picLogo.Location = new System.Drawing.Point(15, 8);
            this.picLogo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(178, 62);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 184;
            this.picLogo.TabStop = false;
            // 
            // ofdArchivo
            // 
            this.ofdArchivo.FileName = " ";
            this.ofdArchivo.Filter = "Libro de Excel (*.xls, *.xlsx)|*.xls;*.xlsx";
            this.ofdArchivo.Title = "Archivo con Cuentas";
            // 
            // dtpCalendario
            // 
            this.dtpCalendario.CustomFormat = "MM/yyyy";
            this.dtpCalendario.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpCalendario.Location = new System.Drawing.Point(500, 44);
            this.dtpCalendario.Name = "dtpCalendario";
            this.dtpCalendario.Size = new System.Drawing.Size(112, 26);
            this.dtpCalendario.TabIndex = 195;
            // 
            // lblMesCalendario
            // 
            this.lblMesCalendario.AutoSize = true;
            this.lblMesCalendario.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMesCalendario.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.lblMesCalendario.ForeColor = System.Drawing.Color.DimGray;
            this.lblMesCalendario.Location = new System.Drawing.Point(436, 51);
            this.lblMesCalendario.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMesCalendario.Name = "lblMesCalendario";
            this.lblMesCalendario.Size = new System.Drawing.Size(41, 16);
            this.lblMesCalendario.TabIndex = 194;
            this.lblMesCalendario.Text = "Fecha";
            // 
            // frmCalendario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(948, 611);
            this.Controls.Add(this.dtpCalendario);
            this.Controls.Add(this.lblMesCalendario);
            this.Controls.Add(this.picWaitBulk);
            this.Controls.Add(this.picWait);
            this.Controls.Add(this.lblInstrucciones);
            this.Controls.Add(this.lblRegistros);
            this.Controls.Add(this.btnCargar);
            this.Controls.Add(this.lblMensajes);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.btnArchivo);
            this.Controls.Add(this.dgvCalendario);
            this.Controls.Add(this.picLogo);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F);
            this.ForeColor = System.Drawing.Color.DimGray;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCalendario";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calendario - Estadística";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRemesa_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendario)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWaitBulk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picWaitBulk;
        private System.Windows.Forms.PictureBox picWait;
        private System.Windows.Forms.Label lblInstrucciones;
        private System.Windows.Forms.Label lblRegistros;
        private System.Windows.Forms.Button btnCargar;
        private System.Windows.Forms.Label lblMensajes;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Button btnArchivo;
        private System.Windows.Forms.DataGridView dgvCalendario;
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.OpenFileDialog ofdArchivo;
        private System.Windows.Forms.DateTimePicker dtpCalendario;
        private System.Windows.Forms.Label lblMesCalendario;
    }
}