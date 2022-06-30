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
    public partial class frmVisitasMapeo : Form
    {
        public frmVisitasMapeo()
        {
            InitializeComponent();
            dtpMesVisitasMapeo.Format = DateTimePickerFormat.Custom;
            dtpMesVisitasMapeo.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblVisitasMapeo = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvVisitasMapeo.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblVisitasMapeo = new DataTable();

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

                tblVisitasMapeo.Load(lector.Reader);

                if (tblVisitasMapeo.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                for (int i = 0; i < tblVisitasMapeo.Columns.Count; i++)
                    tblVisitasMapeo.Columns[i].ColumnName = tblVisitasMapeo.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

                string[] Layout;
                Layout = new string[] { "PRODUCTO", "CUENTA", "CONTRATO18DIGITOS)", "CAPTURISTA", "EJECUTIVO", "SUPERVISORGERENTE", "FCAPTURA", "FVISITA", "HORAAMPM)", "GESTION", "CA", "CA", "CR", "FECHPP", "IMPORTE", "MAPEOSOLUCION", "MAPEODELAUTOMOVIL", "CAUSASDENOPAGO", "OBSERVACIONESGRAL", "CALLE", "NOEXTYÓINT.", "COL", "DELMUN", "TELALTERNO", "CONFIRMADOESDELTITULAR", "TIPODEDOMICILIO", "CORREOELECTRONICO", "ESTADÍO", "DIVISION", "REGIONAL", "PLAZA", "NOVIS", "DOMICILIOINCOMPLETO", "CA", "CR", "RR", "CONTACTO", "PRIORIDAD", "GRUPO", "LOGIN", "FECREPO", "PLAZA", "ANTIGÜEDAD", "1ERILO", "EN200" };
                
                foreach (string sColumna in Layout)
                    if (!tblVisitasMapeo.Columns.Contains(sColumna.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "")))
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

            string sErrorFechas = "";

            Mensaje("Validando fechas del archivo seleccionado, por favor espere...", Color.DimGray, lblMensajes);
            sErrorFechas = validaFechas(tblVisitasMapeo);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblVisitasMapeo.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvVisitasMapeo.DataSource = tblVisitasMapeo;
                dgvVisitasMapeo.Columns["HORAAMPM"].DefaultCellStyle.Format = "hh:mm:ss tt";
                dgvVisitasMapeo.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvVisitasMapeo.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblConciliacionLeasing)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drConciliacion in tblVisitasMapeo.Rows)
                {
                    //Fecha
                    if (drConciliacion.Table.Columns.Contains("FCAPTURA") || drConciliacion.Table.Columns.Contains("FVISITA") || drConciliacion.Table.Columns.Contains("FECHPP") || drConciliacion.Table.Columns.Contains("FECREPO") || drConciliacion.Table.Columns.Contains("ANTIGÜEDAD"))
                    {
                        if (!DateTime.TryParse(drConciliacion["FCAPTURA"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drConciliacion["CUENTA"].ToString();
                                return "Falló, verificar la columna Fecha de la cuenta " + drConciliacion["CUENTA"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }

                        if (!DateTime.TryParse(drConciliacion["FVISITA"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drConciliacion["CUENTA"].ToString();
                                return "Falló, verificar la columna Fecha de la cuenta " + drConciliacion["CUENTA"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }

                        if (!DateTime.TryParse(drConciliacion["FECHPP"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drConciliacion["CUENTA"].ToString();
                                return "Falló, verificar la columna Fecha de la cuenta " + drConciliacion["CUENTA"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }

                        if (!DateTime.TryParse(drConciliacion["FECREPO"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drConciliacion["CUENTA"].ToString();
                                return "Falló, verificar la columna Fecha de la cuenta " + drConciliacion["CUENTA"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }

                        if (!DateTime.TryParse(drConciliacion["ANTIGÜEDAD"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drConciliacion["CUENTA"].ToString();
                                return "Falló, verificar la columna Fecha de la cuenta " + drConciliacion["CUENTA"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }
                    }                    
                }
            }
            catch (Exception ex){
                return "Falló " + ex.Message;
            }
            return "";
        }

        public void CargaVisitasMapeo(object oDatos)
        {            
            DataTable tblVisitasMapeo = new DataTable();
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try
            {
                lector.Read();
                tblVisitasMapeo.Load(lector.Reader);
            }
            catch (Exception ex)
            {
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            for (int i = 0; i < tblVisitasMapeo.Columns.Count; i++)
                tblVisitasMapeo.Columns[i].ColumnName = tblVisitasMapeo.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Vima'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Vima");

            if (!DataBaseConn.Execute("DropTemporalVima")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Vima ( ";
            for (int i = 0; i < tblVisitasMapeo.Columns.Count; i++)
            {
                if (tblVisitasMapeo.Columns[i].ColumnName == "FCAPTURA" || tblVisitasMapeo.Columns[i].ColumnName == "FVISITA" || tblVisitasMapeo.Columns[i].ColumnName == "FECHPP" || tblVisitasMapeo.Columns[i].ColumnName == "FECREPO" || tblVisitasMapeo.Columns[i].ColumnName == "ANTIGÜEDAD")
                    sCreateTable += ("\r\n [" + tblVisitasMapeo.Columns[i] + "] DATE NULL, ");
                else if (tblVisitasMapeo.Columns[i].ColumnName.Contains("HORAAMPM"))
                    sCreateTable += ("\r\n [" + tblVisitasMapeo.Columns[i] + "] DATETIME NULL, ");
                else if (tblVisitasMapeo.Columns[i].ColumnName.Contains("IMPORTE"))
                    sCreateTable += ("\r\n [" + tblVisitasMapeo.Columns[i] + "] MONEY NULL, ");                
                else
                    sCreateTable += ("\r\n [" + tblVisitasMapeo.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ");";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalVima")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Vima";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblVisitasMapeo);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpMesVisitasMapeo.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando cuentas únicas, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Bancomer].[1.4.0.VisitasMapeo] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaVisitasMapeo"))
                {
                    Mensaje("Falló al insertar visitas mapeo.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La información se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvVisitasMapeo.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                });
            }
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Mensaje("Transfiriendo... " + e.RowsCopied.ToString("N0") + "/" + iNumberRecords.ToString("N0") + " registros. " + (e.RowsCopied / (float)iNumberRecords * 100).ToString("N2") + " %", Color.DimGray, lblRegistros);
        }
        #endregion

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvVisitasMapeo.DataSource = null;

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
            DataBaseConn.StartThread(CargaVisitasMapeo);
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
