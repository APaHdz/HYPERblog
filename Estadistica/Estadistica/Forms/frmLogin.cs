using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Estadistica
{
    public partial class frmLogin : Form
    {


        public frmLogin()
        {
            InitializeComponent();
            txtUsuario.ContextMenu = txtContraseña.ContextMenu
                = txtNuevaContraseña.ContextMenu = txtRepitaContraseña.ContextMenu = new ContextMenu();

            lblVersión.Text = "v" + DataBaseConn.AppVersion.ToString();
        }




        #region Atributos
        string[] sTextos = { "Usuario", "Contraseña", "Extensión", "Nueva contraseña", "Repita contraseña" };
        delegate void SetObjectCallback(object param);
        delegate void SetDelegateCallback();


        #endregion


        #region Métodos
        public void ValidaContraseña()
        {
            int iTrucoDesbloquea = 0;

            lblMensaje.ForeColor = Color.Red;

            if (int.Parse(lblContador.Text) >= 5)
                lblMensaje.Text = "Ha superado el número de intentos.\r\nCierre y abra la aplicación para intentar nuevamente.";

            else if (txtUsuario.Text == sTextos[0] || txtUsuario.Text.Length < 4)
            {
                txtUsuario.Focus();
                lblMensaje.Text = "Ingrese un usuario válido.";

            }
            else if (txtContraseña.Text.Trim().ToLower() == "local")
            {
                System.Diagnostics.Process.Start(DataBaseConn.LocalFolder + DataBaseConn.AppName);
                Application.Exit();

            }
            else if (txtContraseña.Text == sTextos[1] ||
               (txtContraseña.Text.Length < 8 && !int.TryParse(txtContraseña.Text, out iTrucoDesbloquea))
           )
            {
                txtContraseña.Focus();
                lblMensaje.Text = "Ingrese una contraseña de 8 o más caracteres.";

            }
            else
            {
                lblMensaje.ForeColor = System.Drawing.Color.Gray;
                lblMensaje.Text = "Validando datos...";
                lblMensaje.Visible = true;
                lblMensaje.Refresh();
                lblContador.Text = (int.Parse(lblContador.Text) + 1).ToString();
                btnAcceder.Visible = btnAcceder.Enabled = false;
                picWait.Visible = true;

                Thread hiloValida = new Thread(ValidaUsuario);
                hiloValida.IsBackground = true;
                hiloValida.Start();
            }
        }

        public void ValidaUsuario(object oMensaje = null)
        {
            string sMensaje = "";

            if (oMensaje == null)
            {

                if (!DataBaseConn.CreateConnection())
                    sMensaje = "Falló la conexión a base de datos.";
                else
                    //DataBaseConn.StartThread(Ingresa);
                    sMensaje = Ejecutivo.IniciaSesión(txtUsuario.Text, txtContraseña.Text);

                if (this.InvokeRequired)
                {
                    //this.Invoke(new SetObjectCallback(ValidaUsuario), sMensaje);
                    //return;
                }
            }
            else
                sMensaje = oMensaje.ToString();

            Invoke((Action)delegate() { picWait.Visible = false; btnAcceder.Visible = btnAcceder.Enabled = true; });


            Invoke((Action)delegate() { lblVersión.Text = "z" + DataBaseConn.ServerNum + "\r\nv" + DataBaseConn.AppVersion; });

            if (Ejecutivo.Expiró)
            {
                lblMensaje.Text = sMensaje;
                Thread cambioContraseña = new Thread(PreparaCambioContraseña);
                cambioContraseña.IsBackground = true;
                cambioContraseña.Start();
            }
            else if (Ejecutivo.Datos == null)
            {
                lblMensaje.ForeColor = System.Drawing.Color.Red;
                Invoke((Action)delegate() { lblMensaje.Text = "Problemas al ingresar al sistema, verifique su usuario y/o contraseña."; });
                return;
            }
            else if (Ejecutivo.Datos.Table.Columns.Contains("idEjecutivo"))
            {
                lblMensaje.ForeColor = Color.Gray;
                //lblMensaje.Text = "Su contraseña expirará en " + Ejecutivo.Datos["Días"] + " día(s), \r\n¿desea cambiarla ahora?";

                DataBaseConn.StartThread(Ingresa);

                //Invoke((Action)delegate() { btnSí.Visible = btnNo.Visible = true; });
                //Invoke((Action)delegate() { btnNo.Focus(); });
                //this.AcceptButton = btnNo;
                //Invoke((Action)delegate() { btnAcceder.Visible = btnAcceder.Enabled = false; });
                //Invoke((Action)delegate() { txtContraseña.Visible = txtUsuario.Visible = false; }); 

            }
        }

        public void PreparaCambioContraseña()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new SetDelegateCallback(PreparaCambioContraseña));
                return;
            }

            lblMensaje.Text = "Ingrese su nueva contraseña.";
            btnAcceder.Visible = btnAcceder.Enabled = false;
            txtContraseña.Visible = txtUsuario.Visible = false;
            lblContador.Visible = false;
            pnlValidación.Visible = txtNuevaContraseña.Visible = txtRepitaContraseña.Visible = true;
            btnNo.Enabled = btnSí.Enabled = btnSí.Visible = btnNo.Visible = false;
            btnAcceder.Text = "Cambiar";
            this.AcceptButton = btnAcceder;
        }

        public void ValidaNuevaContraseña()
        {

            bool bVálido = true;


            Label[] lblValidaciones = { lblMayúscula, lblMinúscula, lblNúmero, lblSímbolo, lblIguales, lbl8Dígitos, lblDiferente };
            bool[] bValidaciones = {
                false, false, false, false,
                txtNuevaContraseña.Text == txtRepitaContraseña.Text,
                txtNuevaContraseña.Text.Length >= 8,
                txtNuevaContraseña.Text != txtContraseña.Text
            };


            foreach (char cCaracter in txtNuevaContraseña.Text)
                if (!bValidaciones[0] && char.IsUpper(cCaracter))
                    bValidaciones[0] = true;
                else if (!bValidaciones[1] && char.IsLower(cCaracter))
                    bValidaciones[1] = true;
                else if (!bValidaciones[2] && char.IsNumber(cCaracter))
                    bValidaciones[2] = true;
                else if (!bValidaciones[3] && !char.IsLetterOrDigit(cCaracter))
                    bValidaciones[3] = true;


            for (int i = 0; i < lblValidaciones.Length; i++)
                if (bValidaciones[i])
                {
                    lblValidaciones[i].Text = lblValidaciones[i].Text.Replace("✘", "✔");
                    lblValidaciones[i].ForeColor = Color.Green;
                    ;
                }
                else
                {
                    bVálido = false;
                    lblValidaciones[i].Text = lblValidaciones[i].Text.Replace("✔", "✘");
                    lblValidaciones[i].ForeColor = Color.Red;
                }

            btnAcceder.Enabled = btnAcceder.Visible = bVálido;

        }
        public void CambiaContraseña(object oMensaje)
        {

            if (this.InvokeRequired)
            {

                string sMensaje = "";

                //sMensaje = Ejecutivo.CambiaContraseña(txtUsuario.Text, txtNuevaContraseña.Text, txtContraseña.Tag.ToString());
                //if (sMensaje != "")
                //{
                //    this.BeginInvoke(new SetObjectCallback(CambiaContraseña), sMensaje);
                //    return;
                //}

                sMensaje = Ejecutivo.IniciaSesión(txtUsuario.Text, txtNuevaContraseña.Text);
                if (sMensaje != "")
                {
                    this.BeginInvoke(new SetObjectCallback(CambiaContraseña), sMensaje);
                    return;
                }
                else
                    Ingresa();

            }
            else
            {

                lblMensaje.ForeColor = Colores.Rojo;
                lblMensaje.Text = oMensaje.ToString();
                picWait.Visible = false;
                txtContraseña.Text = txtNuevaContraseña.Text;
                ValidaNuevaContraseña();
            }
        }

        public void Ingresa()
        {

            if (Catálogo.Carteras == null)
            {

                Mensaje("Cargando catálogos...");
                Catálogo.ActualizaTodosCatálogos(this);

                if (this.InvokeRequired)
                    this.Invoke(new SetDelegateCallback(Ingresa));

            }
            else

                this.Close();
        }
        public void Mensaje(object oMensaje)
        {

            if (lblMensaje.InvokeRequired)
                lblMensaje.BeginInvoke(new SetObjectCallback(Mensaje), oMensaje);
            else
                lblMensaje.Text = oMensaje.ToString();
        }
        #endregion


        #region Eventos
        private void txt_Enter(object sender, EventArgs e)
        {

            TextBox txtCampo = (TextBox)sender;
            lblMensaje.Text = "";

            if (
                txtCampo == txtUsuario && txtCampo.Text == sTextos[0] ||
                txtCampo == txtContraseña && txtCampo.Text == sTextos[1] ||
                txtCampo == txtNuevaContraseña && txtCampo.Text == sTextos[3] ||
                txtCampo == txtRepitaContraseña && txtCampo.Text == sTextos[4]
            )
            {
                if (txtCampo == txtContraseña || txtCampo == txtNuevaContraseña || txtCampo == txtRepitaContraseña)
                    txtCampo.PasswordChar = '•';

                txtCampo.Text = "";
                txtCampo.ForeColor = System.Drawing.Color.DimGray;
                txtCampo.BackColor = Color.Gainsboro;
                txtCampo.Focus();
            }

        }
        private void txt_Leave(object sender, EventArgs e)
        {
            TextBox txtCampo = (TextBox)sender;

            if (txtCampo.Text.Length > 0)
            {
                txtCampo.BackColor = Color.White;
                return;
            }

            if (txtCampo == txtUsuario)
                txtCampo.Text = sTextos[0];

            else if (txtCampo == txtContraseña)
            {
                txtCampo.Text = sTextos[1];
                txtCampo.PasswordChar = '\0';
            }
            else if (txtCampo == txtNuevaContraseña)
            {
                txtCampo.Text = sTextos[3];
                txtCampo.PasswordChar = '\0';
            }
            else if (txtCampo == txtRepitaContraseña)
            {
                txtCampo.Text = sTextos[4];
                txtCampo.PasswordChar = '\0';
            }

            txtCampo.ForeColor = System.Drawing.Color.DarkGray;
            txtCampo.BackColor = Color.Gainsboro;

        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsUpper(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Tab);
        }
        private void txtExtensión_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Tab);
        }
        private void btnAcceder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter && e.KeyCode != Keys.Space)
                txtUsuario.Focus();
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            btnAcceder.Focus();

            if (btnAcceder.Text == "Acceder")
                ValidaContraseña();
            else
            {
                btnAcceder.Visible = false;
                picWait.Visible = true;
                if (txtContraseña.Tag == null)
                    txtContraseña.Tag = txtContraseña.Text;

                DataBaseConn.StartThread(CambiaContraseña);
            }

        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            btnNo.Enabled = btnSí.Enabled = btnSí.Visible = btnNo.Visible = false;
            picWait.Visible = true;
            DataBaseConn.StartThread(Ingresa);
        }
        private void btnSí_Click(object sender, EventArgs e)
        {
            PreparaCambioContraseña();
        }
        private void txtNuevaContraseña_TextChanged(object sender, EventArgs e)
        {
            ValidaNuevaContraseña();
        }
        #endregion

        private void picWait_Click(object sender, EventArgs e)
        {

        }
    }
}