using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Data.SqlClient;

namespace Estadistica
{
    public partial class frmDomicilios : Form
    {
        public frmDomicilios()
        {
            InitializeComponent();
            //dtpMesDomicilios.Format = DateTimePickerFormat.Custom;
            //dtpMesDomicilios.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblDomicilios = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvDomicilios.DefaultCellStyle.Font = controlEvents.RowFont;
        }

        protected void LimpiaMensajes()
        {
            Mensaje("", Color.DimGray, lblRegistros);
            Mensaje("", Color.DimGray, lblInstrucciones);
            Mensaje("", Color.DimGray, lblMensajes);
        }

        public void Archivo(object oFilePath)
        {
            string sFilePath = oFilePath.ToString().Trim();
            tblDomicilios = new DataTable();

            if (!File.Exists(sFilePath)){
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            lector = new dbFileReader(sFilePath);

            string sResultado = lector.Open();

            if (sResultado != ""){
                Mensaje(sResultado, Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            lector.Open();
            iNumberRecords = lector.CountRows();
            try{
                lector.Read(100);

                tblDomicilios.Load(lector.Reader);

                if (tblDomicilios.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                string[] Layout;
                Layout = new string[] { "idCuenta","Calle","Colonia","Municipio","Estado","CP" };
                
                foreach (string sColumna in Layout)
                    if (!tblDomicilios.Columns.Contains(sColumna))
                    {
                        Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                        return;
                    }
            }
            catch (Exception ex){
                Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }            

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblDomicilios.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvDomicilios.DataSource = tblDomicilios;                
                dgvDomicilios.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvDomicilios.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }        

        public void CargaDomicilios(object oDatos)
        {            
            DataTable tblDomicilios = new DataTable();
            DataTable CreaTemporalAsi = new DataTable();         

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblDomicilios.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'DomiVal'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.DomiVal");

            if (!DataBaseConn.Execute("DropTemporalDomiVal")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.DomiVal ( ";
            for (int i = 0; i < tblDomicilios.Columns.Count; i++)
            {
                if (tblDomicilios.Columns[i].ColumnName == "Fecha")
                    sCreateTable += ("\r\n [" + tblDomicilios.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblDomicilios.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalAsi, "CreaTemporalDomiVal")){
                Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Insertando datos, por favor espere...", Color.DimGray, lblMensajes);
            using (SqlBulkCopy blkData = DataBaseConn.bulkCopy){{
                    try{
                        DataBaseConn.ConnectSQL(true);
                        blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.NotifyAfter = 317;
                        blkData.DestinationTableName = "dbEstadistica.Temp.DomiVal";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblDomicilios);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);
                

                Mensaje("Domicilios en proceso, por favor espere...", Color.DimGray, lblMensajes);

                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Estadistica].[6.0.ValidacionDomicilios]");                
                //DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);               

                DataTable tblReporteDomicilios = new DataTable();

                if (!DataBaseConn.Fill(tblReporteDomicilios, "ExcelDomicilios"))
                {
                    TerminaBúsqueda("Falló la generación del reporte.", true);
                    return;
                }

                if (tblReporteDomicilios.Columns.Contains("Mensaje"))
                {
                    Mensaje(tblReporteDomicilios.Rows[0]["Mensaje"].ToString(), Color.Crimson, lblMensajes);
                    Invoke((Action)delegate() { btnCargar.Visible = true; picWait.Visible = false; });
                    return;
                }

                string sRutaExcel = "";

                if (tblReporteDomicilios.Rows.Count == 0)
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
                    tblReporteDomicilios.Dispose();
                    return;
                }

                string sResultado = ExcelXML.ExportToExcelSAX(ref tblReporteDomicilios, sRutaExcel);
                TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");               
            }
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Mensaje("Transfiriendo... " + e.RowsCopied.ToString("N0") + "/" + iNumberRecords.ToString("N0") + " registros. " + (e.RowsCopied / (float)iNumberRecords * 100).ToString("N2") + " %", Color.DimGray, lblRegistros);
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

            btnArchivo.Visible = true;
            picWaitBulk.Visible = false;
            dgvDomicilios.DataSource = null;
            lblInstrucciones.Text = "";
            lblRegistros.Text = "";
            txtRuta.Text = "";
            btnCargar.Visible = false;
            picWait.Visible = false;

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvDomicilios.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            txtRuta.Text = ofdArchivo.FileName;
            Mensaje("Cargando vista previa del archivo de Excel, por favor espere...", Color.DimGray, lblMensajes);
            btnCargar.Visible = btnArchivo.Visible = false;
            picWait.Visible = true;
            DataBaseConn.StartThread(Archivo, ofdArchivo.FileName);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            Mensaje("", Color.DimGray, lblInstrucciones);
            btnCargar.Visible = false;
            picWaitBulk.Visible = true;
            btnArchivo.Visible = false;
            DataBaseConn.StartThread(CargaDomicilios);
        }

        private void frmRemesa_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
            if (lector != null)
                lector.Close();
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
