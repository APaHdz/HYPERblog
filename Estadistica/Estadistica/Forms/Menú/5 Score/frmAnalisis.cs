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
    public partial class frmAnalisis : Form
    {
        public frmAnalisis()
        {
            InitializeComponent();
            dtpFechaFin.Value = DateTime.Today.AddDays(-0);
            dtpFechaFin.MaxDate = dtpFechaFin.Value;
            dtpFechaFin.MinDate = DateTime.Today.AddDays(-700);
            PreparaVentana();
        }

        #region Variables    
        int idCartera = 0;
        int idProducto = 0;
        int idSegmento = 0;
        string Segmento = "";
        string FechaFin = "";
        
        string Ser = "";
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

            //Productos
            cmbProductos.ValueMember = "idProducto";
            cmbProductos.DisplayMember = "Producto";
            cmbProductos.DataSource = Catálogo.Productos;

            //PSegmentos
            cmbSegmentos.ValueMember = "idSegmento";
            cmbSegmentos.DisplayMember = "Segmento";
            cmbSegmentos.DataSource = Catálogo.Segmentos;

            Ser = DataBaseConn.ServerNum;

            CambiaCartera();
        }

        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;

            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;
            cmbProductos.SelectedIndex = -1;

            Catálogo.Segmentos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;
            cmbSegmentos.SelectedIndex = -1;

        }

        public void Consulta()
        {
            Mensaje("Proceso en ejecución, por favor espere...", Color.DimGray, lblMensajes);

            string Base = "";
            if (chkBase.Checked == true)
                Base = "1";
            else
                Base = "0";


            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; SELECT * FROM dbEstadistica.[Score].[Analisis] WHERE Activo=1 AND idCartera = " + idCartera + " AND idProducto = " + idProducto + " AND Base = '" + Base + "'" );
            //DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; SELECT * FROM dbEstadistica.[Score].[Analisis] WHERE Activo=1 AND idCartera = " + idCartera + " AND idProducto = " + idProducto + " AND idSegmento = " + idSegmento);

            DataTable dtStore = new DataTable();

            if(!DataBaseConn.Fill(dtStore,"Consulta de SP Score"))
            {
                Invoke((Action)delegate()
                {
                    btnGenerar.Visible = true;
                    picWait.Visible = false;
                    cmbCarteras.Enabled = true;
                    cmbProductos.Enabled = true;
                    cmbSegmentos.Enabled = true;
                    dtpFechaFin.Enabled = true;
                });
                return;
            }

            if(dtStore.Rows.Count<=0)
            {
                Mensaje("No existe información con los parametros seleccionados.", Color.DimGray, lblMensajes);
                return;
            }

            Mensaje("En proceso el resultado de la consulta, por favor espere...", Color.DimGray, lblMensajes);
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00';USE dbEstadistica EXEC " + dtStore.Rows[0]["BaseDatos"] + ".[" + dtStore.Rows[0]["Esquema"] + "].[" + dtStore.Rows[0]["FunciónStore"] + "] @idCartera, @idProducto, @idSegmento, @Segmento, @FechaFin ");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
            DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
            DataBaseConn.CommandParameters.AddWithValue("@idSegmento", idSegmento);
            DataBaseConn.CommandParameters.AddWithValue("@Segmento", Segmento);
            DataBaseConn.CommandParameters.AddWithValue("@FechaFin", FechaFin);

            DataSet dtAnalisis = new DataSet();

            if (!DataBaseConn.Fill(dtAnalisis,"AnalisisScore"))
            {
                Mensaje("Falló al generar analisis.", Color.Red, lblMensajes);
                Invoke((Action)delegate()
                {
                    btnGenerar.Visible = true;
                    picWait.Visible = false;
                    cmbCarteras.Enabled = true;
                    cmbProductos.Enabled = true;
                    cmbSegmentos.Enabled = true;
                    dtpFechaFin.Enabled = true;
                });
                return;
            }            

            string sRutaExcel = "";

            if (dtAnalisis.Tables.Count == 0 || dtAnalisis.Tables[0].Rows.Count == 0)
            {
                TerminaBúsqueda("Consulta terminada sin registros.", false);
                Invoke((Action)delegate()
                {
                    btnGenerar.Visible = true;
                    picWait.Visible = false;
                    cmbCarteras.Enabled = true;
                    cmbProductos.Enabled = true;
                    cmbSegmentos.Enabled = true;
                    dtpFechaFin.Enabled = true;
                });
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
                dtAnalisis.Dispose();
                return;
            }

            Invoke((Action)delegate()
            {
                btnGenerar.Visible = true;
                picWait.Visible = false;
                cmbCarteras.Enabled = true;
                cmbProductos.Enabled = true;
                cmbSegmentos.Enabled = true;
                dtpFechaFin.Enabled = true;
            });
            

            string sResultado = ExcelXML.ExportToExcelSAX(ref dtAnalisis, sRutaExcel);
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

            btnGenerar.Visible = true;
            picWait.Visible = false;
            cmbCarteras.Enabled = true;
            dtpFechaFin.Enabled = true;

            this.ControlBox = true;
        }
       
        #endregion

        #region Eventos
        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiaCartera();
        }  

        private void btnConsulta_Click(object sender, EventArgs e)
        {            
             idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
             idProducto = Convert.ToInt32(cmbProductos.SelectedValue);
             idSegmento = Convert.ToInt32(cmbSegmentos.SelectedValue);
             Segmento = cmbSegmentos.Text.ToString();
             FechaFin = dtpFechaFin.Value.ToString("yyyy-MM-dd");
             picWait.Visible = true;
             btnGenerar.Visible = false;
             cmbCarteras.Enabled = false;
             cmbProductos.Enabled = false;
             cmbSegmentos.Enabled = false;
             dtpFechaFin.Enabled = false;                               
             
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

        private void frmAnalisis_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();    
        }

                      
    }
}
