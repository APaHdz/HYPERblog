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
    public partial class frmTablasTemporales : Form
    {
        public frmTablasTemporales()
        {
            InitializeComponent();
            //dtpMesAsistencia.Format = DateTimePickerFormat.Custom;
            //dtpMesAsistencia.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblTablaTemporal = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        string Tablatemp = "";
        string EsquemaTemporal = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvTablaTemporal.DefaultCellStyle.Font = controlEvents.RowFont;
            AgregaEventosTxtKeyPress();
        }
        public void AgregaEventosTxtKeyPress()
        {
            txtTablaTemporal.KeyPress += new KeyPressEventHandler(controlEvents.txtOnlyNumbersLetters_KeyPress);
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
            DataTable tblTablaTemporal = new DataTable();

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

            //lector.Open();
            iNumberRecords = lector.CountRows();
            try{
                lector.Read(100);

                tblTablaTemporal.Load(lector.Reader);

                if (tblTablaTemporal.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                //string[] Layout;
                //Layout = new string[] { "Clave", "Nombre", "Turno", "Supervisor", "Etapa" };
                
                //foreach (string sColumna in Layout)
                //    if (!tblTablaTemporal.Columns.Contains(sColumna))
                //    {
                //        Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                //        lector.Close();
                //        Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //        return;
                //    }
            }
            catch (Exception ex){
                Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblTablaTemporal.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvTablaTemporal.DataSource = tblTablaTemporal;                
                dgvTablaTemporal.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvTablaTemporal.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }        

        public void CargaTablaTemporal(object oDatos)
        {
            DataTable tblTablaTemporal = new DataTable();
            DataTable CreaTemporalTT = new DataTable();         

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblTablaTemporal.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo." + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            

            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
             DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = '" + Tablatemp + "_" + Ejecutivo.Datos["idEjecutivo"] + "'  AND schema_id=" + EsquemaTemporal +  " ) " +
                " DROP TABLE dbEstadistica.Temp." + Tablatemp + "_" + Ejecutivo.Datos["idEjecutivo"]);

            if (!DataBaseConn.Execute("DropTablaTemporal")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp." + Tablatemp + "_" + Ejecutivo.Datos["idEjecutivo"] + " ( ";
            for (int i = 0; i < tblTablaTemporal.Columns.Count; i++){
                if (tblTablaTemporal.Columns[i].ColumnName.Contains("Fecha") || tblTablaTemporal.Columns[i].ColumnName.Contains("FECHA") || tblTablaTemporal.Columns[i].ColumnName.Contains("FECH") || tblTablaTemporal.Columns[i].ColumnName.Contains("Fech") || tblTablaTemporal.Columns[i].ColumnName.Contains("FDFECHAGENPROMESA") || tblTablaTemporal.Columns[i].ColumnName.Contains("FDFECHAPROMESA") || tblTablaTemporal.Columns[i].ColumnName.Contains("FECARRIBO") || tblTablaTemporal.Columns[i].ColumnName.Contains("FDFECHAABONO"))
                    sCreateTable += ("\r\n [" + tblTablaTemporal.Columns[i] + "] DATE NULL, ");
                else if (tblTablaTemporal.Columns[i].ColumnName.Contains("Hora") || tblTablaTemporal.Columns[i].ColumnName.Contains("Segundo"))
                    sCreateTable += ("\r\n [" + tblTablaTemporal.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblTablaTemporal.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalTT, "CreaTablaTemporal")){
                Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Insertando datos en la tabla temporal, por favor espere...", Color.DimGray, lblMensajes);
            using (SqlBulkCopy blkData = DataBaseConn.bulkCopy){{
                    try{
                        DataBaseConn.ConnectSQL(true);
                        blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.NotifyAfter = 317;
                        blkData.DestinationTableName = "dbEstadistica.Temp." + Tablatemp + "_" + Ejecutivo.Datos["idEjecutivo"];
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblTablaTemporal);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);                
            }

            string SQuery = "SET TRAN ISOLATION LEVEL READ UNCOMMITTED \r\n\t" +
                                    "WAITFOR DELAY '00:00:00';  \r\n\t" +                                    
                                    "SELECT   \r\n\t" +
                                    "        T.Cuenta ,\r\n\t" +
                                    "        C.NombreDeudor [CM Name] ,\r\n\t" +
                                    "        T.Telefono [CM Phone number] ,\r\n\t" +
                                    "        T.Mensaje [Type of message  (based on catalogue)],\r\n\t" +
                                    "        'AMX' + RTRIM(LTRIM(STR(C.Expediente))) [Folio] ,\r\n\t" +
                                    "        p.loan_productcode ,\r\n\t" +
                                    "        P.recoveredcode ,\r\n\t" +
                                    "        P.Placement ,\r\n\t" +
                                    "        P.initialbalance ,\r\n\t" +
                                    "        P.CurrentBalanceG ,\r\n\t" +
                                    "        CONVERT(VARCHAR(10),Fecha,103) [Date of message sent (DDMMYY) or Not  Available] ,\r\n\t" +
                                    "        T.Analista [SMS Analyst Name] ,\r\n\t" +
                                    "        CONVERT(VARCHAR(10),T.Respuesta,103) [Date of response from CM] ,\r\n\t" +
                                    "        T.Conteo [Count of Responses received] ,\r\n\t" +
                                    "        T.PII [PII included in Response ? (Y/N)] ,\r\n\t" +
                                    "        T.Complaint [Complaint raised? (Y/N)] ,\r\n\t" +
                                    "        T.CM [CM Response Documentation] ,\r\n\t" +
                                    "        T.Alto [Stop message requested ? (Y/N)] ,\r\n\t" +
                                    "        T.Comments [Comments/Remarks]\r\n\t" +
                                    "        FROM dbEstadistica.Temp.Perc_7745 T\r\n\t" +
                                    "        LEFT JOIN dbCollection..Cuentas C ON T.Cuenta COLLATE DATABASE_DEFAULT = C.idCuenta COLLATE DATABASE_DEFAULT AND C.idCartera=1\r\n\t" +
                                    "        LEFT JOIN dbHistory.Y.ACTL_Producto_1 P ON T.Cuenta COLLATE DATABASE_DEFAULT = P.idCuenta COLLATE DATABASE_DEFAULT AND CONVERT(VARCHAR(10),T.Fecha,112) BETWEEN P.[_VálidoDesde] AND P.[_VálidoHasta] ";

            DataBaseConn.SetCommand(SQuery);

            //DataBaseConn.StartThread(Excel);

            Invoke((Action)delegate(){
                btnArchivo.Visible = true;
                picWaitBulk.Visible = false;
                dgvTablaTemporal.DataSource = null;
                lblInstrucciones.Text = "";
                lblRegistros.Text = "";
                txtRuta.Text = "";
                btnCargar.Visible = false;
                picWait.Visible = false;
                txtTablaTemporal.Text = "";
            });

            Mensaje("La tabla temporal se creó correctamente.", Color.DimGray, lblMensajes);
        }

        public void Excel()
        {
            Mensaje("Consulta en proceso, favor espere unos segundos...", Color.DimGray, lblMensajes);
            DataTable tblDireccion = new DataTable();
            if (!DataBaseConn.Fill(tblDireccion, "ConsultaDirecciones"))
            {
                Invoke((Action)delegate()
                {
                    //btnConsulta.Visible = true;
                    //picWaitBulk.Visible = false;
                    //btnRutas.Visible = true;
                    //picWaitRutas.Visible = false;
                    //gbOpciones.Enabled = true;
                });
                Mensaje("Falló al realizar la consulta.", Color.Red, lblMensajes);
                return;
            }

            string sRutaExcel = "";

            this.Invoke((Action)delegate()
            {
                lblMensajes.Text = "Guardando libro de Excel...";

                if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                    TerminaBúsqueda("Guardado cancelado por usuario.", false);
                else
                    sRutaExcel = sfdExcel.FileName;
            });

            if (sRutaExcel == "")
            {
                tblDireccion.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblDireccion, sRutaExcel);
            TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != ""); 
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
            dgvTablaTemporal.DataSource = null;
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
            dgvTablaTemporal.DataSource = null;

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
            if (txtTablaTemporal.Text.Length <= 3)
            {
                Mensaje("El nombre de la tabla temporal debe ser mayor a 3 caracteres.", Color.Red, lblMensajes);
                return;
            }
            else
            {
                Tablatemp = txtTablaTemporal.Text;
            }

            EsquemaTemporal = "";
            if (DataBaseConn.Server == "192.168.16.11" || DataBaseConn.Server == "192.168.8.97" || DataBaseConn.Server == "192.168.8.126")
            {
                EsquemaTemporal = "6";
            }
            else if (DataBaseConn.Server == "192.168.7.124")
            {
                EsquemaTemporal = "20";
            }
            else if (DataBaseConn.Server == "192.168.8.53")
            {
                EsquemaTemporal = "21";
            }
            else
            {
                Mensaje("El servidor al que se conectó Estadistica debe darse de alta. Consulte al administrador de la aplicación", System.Drawing.Color.Red, lblMensajes);
                return;
            }

            Mensaje("", Color.DimGray, lblInstrucciones);
            btnCargar.Visible = false;
            picWaitBulk.Visible = true;
            btnArchivo.Visible = false;
            DataBaseConn.StartThread(CargaTablaTemporal);
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
