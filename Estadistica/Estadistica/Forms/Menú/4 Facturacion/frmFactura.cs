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
    public partial class frmFactura : Form
    {
        public frmFactura()
        {
            InitializeComponent();
            dtpFinal.Format = DateTimePickerFormat.Custom;
            dtpFinal.CustomFormat = "MM/yyyy";
            dtpFinal.Value = DateTime.Today.AddDays(-0);
            dtpFinal.MaxDate = dtpFinal.Value;
            dtpFinal.MinDate = DateTime.Today.AddDays(-365);

            dtpFechaInicial.Value = DateTime.Today.AddDays(-0);
            dtpFechaInicial.MaxDate = dtpFechaInicial.Value;
            dtpFechaInicial.MinDate = DateTime.Today.AddDays(-365);
            
            dtpFechaFinal.Value = DateTime.Today.AddDays(-0);
            dtpFechaFinal.MaxDate = dtpFechaFinal.Value;
            dtpFechaFinal.MinDate = DateTime.Today.AddDays(-365);

            PreparaVentana();
        }

        #region Variables    
        int idCartera = 0;
        DateTime FechaAvance;
        DataTable tblAvances = new DataTable();
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

        public void ConsultaAvances()
        {
            Mensaje("Consulta de avances del mes seleccionado, por favor espere...", Color.DimGray, lblMensajes);

            string Final = dtpFinal.Value.ToString("yyyy-MM-dd");
            string Año = dtpFinal.Value.Year.ToString();
            string Mes = dtpFinal.Value.Month.ToString();

            //DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; SELECT CONVERT(VARCHAR(10),Avance,112) FechaAvance FROM dbEstadistica.Bancomer.Avances WHERE idCartera=" + idCartera.ToString() + "AND YEAR(Avance) = " + Año.ToString() + " AND MONTH(Avance) = " + Mes.ToString() + " GROUP BY Avance ORDER BY Avance ASC");
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:05'; SELECT Avance FechaAvance FROM dbEstadistica.Bancomer.Avances WHERE idCartera=" + idCartera.ToString() + "AND YEAR(Avance) = " + Año.ToString() + " AND MONTH(Avance) = " + Mes.ToString() + " GROUP BY Avance ORDER BY Avance ASC");

            tblAvances = new DataTable();

            if (!DataBaseConn.Fill(tblAvances, "AvancesMes"))
            {
                this.Invoke((Action)delegate
                {
                    btnConsulta.Enabled = true;
                    picWait.Enabled = false;
                    Mensaje("Falló la consulta de los avances del mes.", System.Drawing.Color.Crimson, lblMensajes);
                });
                return;
            }

            if (tblAvances.Rows.Count > 0)
            {
                //lbxAvances.DisplayMember="FechaAvance";
                //lbxAvances.Items.Clear();
                //lbxAvances.DataSource = null;
                //lbxAvances.DataSource = tblAvances;
                //for (int i = 0; i < tblAvances.Rows.Count; i++)
                //{
                //    lbxAvances.Items.Add(tblAvances.Rows[i].ToString());
                //}
                Invoke((Action)delegate()
                {
                    lbxAvances.DisplayMember = "FechaAvance";
                    lbxAvances.DataSource = tblAvances;
                    lbxAvances.Visible = true;
                    lblFechas.Visible = true;
                    btnConsulta.Visible = true;
                    picWait.Visible = false;
                    cmbCarteras.Enabled = true;
                    dtpFinal.Enabled = true;
                    lbxAvances.Focus();
                });
                Mensaje("Selecciona una fecha.", System.Drawing.Color.Crimson, lblMensajes);                
            }
            else
            {
                Invoke((Action)delegate()
                {
                    //lbxAvances.Items.Clear();
                    lbxAvances.Visible = false;
                    lblFechas.Visible = false;
                    btnConsulta.Visible = true;
                    picWait.Visible = false;
                    cmbCarteras.Enabled = true;
                    dtpFinal.Enabled = true;
                });
                    Mensaje("No existen avances en el mes seleccionado.", System.Drawing.Color.Crimson, lblMensajes);
            }            
        }

        public void Consulta()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);
            
            //string Final = dtpFinal.Value.ToString("yyyy-MM-dd");
            string Final = FechaAvance.ToString("yyyy-MM-dd");

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Facturacion.[1.2.DetalleFactura] @idCartera, @Fin ");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
            DataBaseConn.CommandParameters.AddWithValue("@Fin", Final);

            DataSet tblAvances = new DataSet();
            if (!DataBaseConn.Fill(tblAvances, "DetalleAsignacion")){
                TerminaBúsqueda("Falló la generación del reporte.", true);
                Invoke((Action)delegate() { btnConsulta.Visible = true; picWait.Visible = false; cmbCarteras.Enabled = true; dtpFinal.Enabled = true; });
                return;}
                        
            string sRutaExcel = "";

            if (tblAvances.Tables.Count == 0 || tblAvances.Tables[0].Rows.Count == 0){
                TerminaBúsqueda("Consulta terminada sin registros.", false);
                return;}
            else
                this.Invoke((Action)delegate(){
                    lblMensajes.Text = "Consulta terminada. Guardando libro de Excel...";

                    if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                        TerminaBúsqueda("Guardado cancelado por usuario.", false);
                    else
                        sRutaExcel = sfdExcel.FileName;});

            if (sRutaExcel == ""){
                tblAvances.Dispose();
                return;}

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblAvances, sRutaExcel);
            TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");
        }

        public void ConsultaIntervalo()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);

            string FechaInicial = dtpFechaInicial.Value.ToString("yyyy-MM-dd");
            string FechaFinal = dtpFechaFinal.Value.ToString("yyyy-MM-dd");

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Facturacion.[1.3.DetalleFactura] @idCartera, @FechaIni, @FechaFin ");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
            DataBaseConn.CommandParameters.AddWithValue("@FechaIni", FechaInicial);
            DataBaseConn.CommandParameters.AddWithValue("@FechaFin", FechaFinal);

            DataSet tblFactura = new DataSet();
            if (!DataBaseConn.Fill(tblFactura, "DetalleFactura"))
            {
                TerminaBúsquedaIntervalo("Falló la generación del reporte.", true);
                Invoke((Action)delegate() {
                    dtpFechaInicial.Enabled = true;
                    dtpFechaFinal.Enabled = true;
                    cmbCarteras.Enabled = true;

                    picWaitIntervalo.Visible = false;
                    btnIntervalo.Visible = true;
                });
                return;
            }

            string sRutaExcel = "";

            if (tblFactura.Tables.Count == 0 || tblFactura.Tables[0].Rows.Count == 0)
            {
                TerminaBúsquedaIntervalo("Consulta terminada sin registros.", false);
                return;
            }
            else
                this.Invoke((Action)delegate()
                {
                    lblMensajes.Text = "Consulta terminada. Guardando libro de Excel...";

                    if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                        TerminaBúsquedaIntervalo("Guardado cancelado por usuario.", false);
                    else
                        sRutaExcel = sfdExcel.FileName;
                });

            if (sRutaExcel == "")
            {
                tblFactura.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblFactura, sRutaExcel);
            TerminaBúsquedaIntervalo(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");
        }
        public void TerminaBúsqueda(string Mensaje, bool bError)
        {
            if (this.InvokeRequired){
                this.BeginInvoke((Action)delegate() { TerminaBúsqueda(Mensaje, bError); });
                return;}

            if (bError)
                lblMensajes.ForeColor = System.Drawing.Color.Crimson;
            else
                lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            lblMensajes.Text = Mensaje;

            btnConsulta.Visible = true;
            picWait.Visible = false;
            cmbCarteras.Enabled = true;            
            dtpFinal.Enabled = true;
            btnConsulta.Enabled = true;
            lbxAvances.Enabled = true;

            this.ControlBox = true;
        }

        public void TerminaBúsquedaIntervalo(string Mensaje, bool bError)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate() { TerminaBúsquedaIntervalo(Mensaje, bError); });
                return;
            }

            if (bError)
                lblMensajes.ForeColor = System.Drawing.Color.Crimson;
            else
                lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            lblMensajes.Text = Mensaje;

            dtpFechaInicial.Enabled = true;
            dtpFechaFinal.Enabled = true;
            cmbCarteras.Enabled = true;

            picWaitIntervalo.Visible = false;
            btnIntervalo.Visible = true;

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnAvances_Click(object sender, EventArgs e)
        {

        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
                //lbxAvances.Items.Clear();       
                tblAvances = new DataTable();
                idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);                
                picWait.Visible = true;
                btnConsulta.Visible = false;
                cmbCarteras.Enabled = false;                
                dtpFinal.Enabled = false;
                idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
                DataBaseConn.StartThread(ConsultaAvances);            
        }
        private void lbxAvances_SelectedIndexChanged(object sender, EventArgs e)
        {
            //FechaAvance = lbxAvances.Text;
        }
        private void lbxAvances_Click(object sender, EventArgs e)
        {
            FechaAvance = Convert.ToDateTime(lbxAvances.Text);
            btnConsulta.Enabled = false;
            lbxAvances.Enabled = false;
            DataBaseConn.StartThread(Consulta);
        }
        private void frmBotonera_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
        }

        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbCarteras.Text=="BBVA Bancomer")
            {
                lblFinal.Visible = true;
                dtpFinal.Visible = true;
                lblFinal.Visible = true;
                lbxAvances.Visible = true;
                picWait.Visible = false;
                btnConsulta.Visible = true;

                lblFechaInicial.Visible = false;
                dtpFechaInicial.Visible = false;

                lblFechaFinal.Visible = false;
                dtpFechaFinal.Visible = false;

                picWaitIntervalo.Visible = false;
                btnIntervalo.Visible = false;
            }
            else
            {
                lblFinal.Visible = false;
                dtpFinal.Visible = false;
                lblFinal.Visible = false;
                lbxAvances.Visible = false;
                picWait.Visible = false;
                btnConsulta.Visible = false;

                lblFechaInicial.Visible = true;
                dtpFechaInicial.Visible = true;

                lblFechaFinal.Visible = true;
                dtpFechaFinal.Visible = true;

                picWaitIntervalo.Visible = false;
                btnIntervalo.Visible = true;
            }
        }

        private void btnIntervalo_Click(object sender, EventArgs e)
        {
            dtpFechaInicial.Enabled = false;
            dtpFechaFinal.Enabled = false;
            cmbCarteras.Enabled = false;

            picWaitIntervalo.Visible = true;
            btnIntervalo.Visible = false;

            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);

            DataBaseConn.StartThread(ConsultaIntervalo);
        }
        #endregion

        #region Hilos
        public void Mensaje(string sMessage, Color color, Label lblMensaje)
        {
            if (this.InvokeRequired){
                this.BeginInvoke(new SetStringCallback(Mensaje), new object[] { sMessage, color, lblMensaje });}
            else{
                lblMensaje.Text = sMessage;
                lblMensaje.ForeColor = color;
                lblMensaje.Visible = true;}
        }
        #endregion

    }
}
