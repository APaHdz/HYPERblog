using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estadistica
{
    public partial class frmMovimientoAsignacion : Form
    {
        public frmMovimientoAsignacion()
        {
            InitializeComponent();            
            dtpFinal.Value = DateTime.Today.AddDays(-0);
            dtpFinal.MaxDate = dtpFinal.Value;
            dtpFinal.MinDate = DateTime.Today.AddDays(-365);
            PreparaVentana();
        }

        #region Variables    
        int idCartera = 0;
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            //Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;            
        }

        public void Consulta()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);
            
            string Final = dtpFinal.Value.ToString("yyyy-MM-dd");

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.6.MovimientosAsignacion] @idCartera, @Fin ");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);            
            DataBaseConn.CommandParameters.AddWithValue("@Fin", Final);

            DataSet tblMovAsig = new DataSet();
            if (!DataBaseConn.Fill(tblMovAsig, "MovimientosAsignacion"))
            {
                TerminaBúsqueda("Falló la generación del reporte.", true);
                Invoke((Action)delegate() { btnConsulta.Visible = true; picWait.Visible = false; cmbCarteras.Enabled = true; dtpFinal.Enabled = true; });
                return;
            }
                        
            string sRutaExcel = "";

            if (tblMovAsig.Tables.Count == 0 || tblMovAsig.Tables[0].Rows.Count == 0)
            {
                TerminaBúsqueda("Consulta terminada sin registros.", false);
                return;
            }
            else
                this.Invoke((Action)delegate()
                {
                    lblMensajes.Text = "Consulta terminada. Guardando libro de Excel...";

                    if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                        TerminaBúsqueda("Guardado cancelado por usuario.", false);
                    else
                        sRutaExcel = sfdExcel.FileName;
                });

            if (sRutaExcel == "")
            {
                tblMovAsig.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblMovAsig, sRutaExcel);
            TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");
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

                DataBaseConn.StartThread(Consulta);            
        }

        private void frmBotonera_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
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

        

        
    }
}
