using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Estadistica
{
    public partial class frmBBVACarteo : Form
    {
        public frmBBVACarteo()
        {
            InitializeComponent();

            cmbSegmento.SelectedIndex = 0;
            cmbCartera.SelectedIndex = 0;

            dtpFecha.MinDate = DateTime.Today.AddDays(-60);
            dtpFecha.Value = DateTime.Today.AddDays(-0);
            dtpFecha.MaxDate = DateTime.Today.AddDays(+0);

            dtpPago1.MinDate = DateTime.Today.AddDays(-0);
            dtpPago1.Value = DateTime.Today.AddDays(-0);
            dtpPago1.MaxDate = DateTime.Today.AddDays(+30);

            dtpPago2.MinDate = DateTime.Today.AddDays(-0);
            dtpPago2.Value = DateTime.Today.AddDays(-0);
            dtpPago2.MaxDate = DateTime.Today.AddDays(+30);
            
        }

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);        
        #endregion

        #region Variables
        DataSet Carteo = new DataSet();
        #endregion


        #region Eventos
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Ejecutivo.Datos["Jerarquía"]) < 2){
                Mensajes("Carece de permisos para realizar un reporte.", Color.Crimson, lblMensajes);
                return;
            }

            int idEjecutivo = Convert.ToInt32(Ejecutivo.Datos["idEjecutivo"]);

            Mensajes("Generando consulta...", Color.SlateGray, lblMensajes);
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:02'; EXEC dbEstadistica.Bancomer.[1.8.Carteo] @FechaAsignacion, @FechaPago1, @FechaPago2, @Segmento, @Cartera, @idEjecutivo ");
            DataBaseConn.CommandParameters.AddWithValue("@FechaAsignacion", dtpFecha.Value);
            DataBaseConn.CommandParameters.AddWithValue("@FechaPago1", dtpPago1.Value);
            DataBaseConn.CommandParameters.AddWithValue("@FechaPago2", dtpPago2.Value);
            DataBaseConn.CommandParameters.AddWithValue("@Segmento", cmbSegmento.Text);
            DataBaseConn.CommandParameters.AddWithValue("@Cartera", cmbCartera.Text);
            DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", idEjecutivo);

            btnGenerar.Visible = false;
            picWait.Visible = true;

            DataBaseConn.StartThread(GenerarCarteo);
        }
        #endregion

        #region Metodos
        public void GenerarCarteo()
        {
            Carteo = new DataSet();
            if (!DataBaseConn.Fill(Carteo, "Generar Carteo BBVA")){
                Mensajes("Falló la consulta para generar carteo.", Color.Crimson, lblMensajes);
                return;
            }

            this.Invoke((Action)delegate{
                if (Carteo.Tables[0].Rows.Count <= 0){
                    Mensajes("No existen cuentas para cartear.", Color.Crimson, lblMensajes);
                    btnGenerar.Visible = true;
                    picWait.Visible = false;
                    return;
                }
                else if (Carteo.Tables[0].Columns.Contains("Mensaje")){
                    Mensajes(Carteo.Tables[0].Rows[0]["Mensaje"].ToString(), Color.Crimson, lblMensajes);
                    btnGenerar.Visible = true;
                    picWait.Visible = false;
                    return;
                }
                else{
                    lblMensajes.Text = "Guardando libro de Excel...";

                    if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                        TerminaBúsqueda("Guardado cancelado por usuario.", false);
                    else{                        
                        DataBaseConn.StartThread(GuardaExcel, sfdExcel.FileName);
                    }
                }
            });
        }

        public void TerminaBúsqueda(string Mensaje, bool bError)
        {
            if (this.InvokeRequired){
                this.BeginInvoke((Action)delegate() { TerminaBúsqueda(Mensaje, bError); });
                return;
            }

            if (bError)
                lblMensajes.ForeColor = System.Drawing.Color.Crimson;
            else
                lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
                lblMensajes.Text = Mensaje;
                picWait.Visible = false;
                btnGenerar.Visible = true;
                this.ControlBox = true;
        }
        public void GuardaExcel(object oRutaExcel)
        {
            string sResultado = ExcelXML.ExportToExcelSAX(ref Carteo, oRutaExcel.ToString(), false);
            TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");
        }
        #endregion

        #region ThreadEnds
        protected void LimpiaMensajes()
        {
            Mensajes("", Color.DimGray, lblMensajes);
        }

        public void Mensajes(string sMessage, Color color, Label label)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new SetStringCallback(Mensajes), new object[] { sMessage, color, label });
            else{
                label.Text = sMessage;
                label.ForeColor = color;
                label.Visible = true;
            }
        }
        #endregion 
    }
}
