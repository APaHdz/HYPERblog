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
    public partial class frmEstudioCartera : Form
    {
        public frmEstudioCartera()
        {
            InitializeComponent();
            dtpFecha.Value = DateTime.Today.AddDays(-0);
            dtpFecha.MaxDate = dtpFecha.Value;
            dtpFecha.MinDate = DateTime.Today.AddDays(-365);     
            PreparaVentana();
        }

        #region Variables    
        int idCartera = 0;
        int idProducto = 0;
        string Fecha = "";
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
        }

        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;

            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;
            //cmbProductos.SelectedIndex = -1;
        }

        public void Consulta()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);                       

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.10.EstudioCartera] @idCartera, @idProducto, @Fecha");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
            DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
            DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

            DataSet tblEstudioCartera = new DataSet();
            if (!DataBaseConn.Fill(tblEstudioCartera, "EstudioCartera"))
            {
                Invoke((Action)delegate()
                {
                    btnConsulta.Visible = true; picWait.Visible = false; cmbCarteras.Enabled = true; cmbProductos.Enabled = true; dtpFecha.Enabled = true;
                });
                TerminaBúsqueda("Falló la generación del reporte.", true);                
                return;
            }
                        
            string sRutaExcel = "";

            if (tblEstudioCartera.Tables.Count == 0 || tblEstudioCartera.Tables[0].Rows.Count == 0)
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
                tblEstudioCartera.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblEstudioCartera, sRutaExcel);
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
            cmbProductos.Enabled = true;
            dtpFecha.Enabled = true;      

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnConsulta_Click(object sender, EventArgs e)
        {            
                idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
                idProducto = Convert.ToInt32(cmbProductos.SelectedValue);
                Fecha = dtpFecha.Value.ToString("yyyy-MM-dd");
                picWait.Visible = true;
                btnConsulta.Visible = false;
                cmbCarteras.Enabled = false;
                cmbProductos.Enabled = false;
                dtpFecha.Enabled = false;


                DataBaseConn.StartThread(Consulta);            
        }
        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiaCartera();
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
