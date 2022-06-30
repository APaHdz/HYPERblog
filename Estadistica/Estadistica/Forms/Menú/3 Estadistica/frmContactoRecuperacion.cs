using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Estadistica
{
    public partial class frmContactoRecuperacion : Form
    {
        public frmContactoRecuperacion()
        {
            InitializeComponent();
            dtpFinal.Value = DateTime.Today.AddDays(-0);
            dtpFinal.MaxDate = dtpFinal.Value;
            dtpFinal.MinDate = DateTime.Today.AddDays(-700);
            PreparaVentana();
        }

        #region Variables    
        int idCartera = 0;

        string Servidor = "";
        int iNumberRecords;
        string Ser = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            Ser = DataBaseConn.ServerNum;

            if (Ser == "9")
            {
                cmbCarteras.Items.Add("Piso 8");
                cmbCarteras.Items.Add("Andalucia");
            }
            else
            {
                cmbCarteras.Items.Add("Piso 5");
            }
        }

        public void Consulta()
        {
            Mensaje("Proceso en ejecución, por favor espere...", Color.DimGray, lblMensajes);
                      
            string Final = dtpFinal.Value.ToString("yyyy-MM-dd");

            if(Servidor=="Piso 5")
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.16.6.ContactoRecuperacion_P5] @Fin, @idEjecutivo ");

            if (Servidor == "Piso 8")
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.16.6.ContactoRecuperacion_P8] @Fin, @idEjecutivo ");

            if (Servidor == "Andalucia")
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.16.6.ContactoRecuperacion_An] @Fin, @idEjecutivo ");
            
            DataBaseConn.CommandParameters.AddWithValue("@Fin", Final);
            DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
            
            if (!DataBaseConn.Execute("ConRec"))
            {
                TerminaBúsqueda("Falló la ejecución del proceso.", true);
                Invoke((Action)delegate() { btnConsulta.Visible = true; picWait.Visible = false; cmbCarteras.Enabled = true; dtpFinal.Enabled = true; });
                return;
            }

            Invoke((Action)delegate() { btnConsulta.Visible = true; picWait.Visible = false; cmbCarteras.Enabled = true; dtpFinal.Enabled = true; });
            Mensaje("El proceso terminó.", Color.DimGray, lblMensajes);
        }

        public void TerminaBúsqueda(string Mensaje, bool bError)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate() { TerminaBúsqueda(Mensaje, bError); });
                return;
            }

            if (bError)
                lblMensajes.ForeColor = System.Drawing.Color.Crimson;
            else
                lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            lblMensajes.Text = Mensaje;

            btnConsulta.Visible = true;
            picWait.Visible = false;
            cmbCarteras.Enabled = true;
            dtpFinal.Enabled = true;

            this.ControlBox = true;
        }
       
        #endregion

        #region Eventos
        private void btnConsulta_Click(object sender, EventArgs e)
        {            
                idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);                
                picWait.Visible = true;
                btnConsulta.Visible = false;
                cmbCarteras.Enabled = false;
                dtpFinal.Enabled = false;                

                if (cmbCarteras.Text == "Piso 5")
                    Servidor = "Piso 5";

                if (cmbCarteras.Text == "Piso 8")
                    Servidor = "Piso 8";

                if (cmbCarteras.Text == "Andalucia")
                    Servidor = "Andalucia";

                DataBaseConn.StartThread(Consulta);            
        }
        #endregion

        #region Hilos
        public void Mensaje(string sMessage, Color color, Label lblMensaje)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetStringCallback(Mensaje), new object[] { sMessage, color, lblMensaje });
            }
            else
            {
                lblMensaje.Text = sMessage;
                lblMensaje.ForeColor = color;
                lblMensaje.Visible = true;
            }
        }
        #endregion

        private void frmBotonera_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();    
        }

        
    }
}
