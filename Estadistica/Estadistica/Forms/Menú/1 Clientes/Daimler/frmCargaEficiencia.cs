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
    public partial class frmCargaEficiencia : Form
    {
        public frmCargaEficiencia()
        {
            InitializeComponent();
            //dtpFechaGestiones.Format = DateTimePickerFormat.Custom;
            //dtpFechaGestiones.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblCargaEficiencia = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvEficiencia.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblCargaEficiencia = new DataTable();

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

                tblCargaEficiencia.Load(lector.Reader);

                if (tblCargaEficiencia.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                for (int i = 0; i < tblCargaEficiencia.Columns.Count; i++)
                    tblCargaEficiencia.Columns[i].ColumnName = tblCargaEficiencia.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

                string[] Layout;
                Layout = new string[] { "AREA", "EMPRESA", "NUMERODECLIENTE", "RFC", "NUMERODECONTRATO", "REFERENCIA", "NOMBREDELCLIENTE", "MONTOVENCIDOXCOBRAR", "DIAPAGO", "Bucket", "DIASMORA", "ESTATUS", "TOTALDEMONTOENRIESGO", "030P", "3160P", "6190P", "91120P", "121180P", "181360P", "RANGOSV", "COBRARMORATORIO", "MORATORIOCOBRADA", "BucketActual", "ESTADO", "PRIORIDAD", "AGENCIA", "INICIO", "TERMINO", "CURRENT", "MORAASIG", "BucketASIG", "DIASDETERIORO", "ESTATUSDEDETERIORO", "DOCUMENTOS", "ACE", "USUARIOS" };
                
                foreach (string sColumna in Layout)
                    if (!tblCargaEficiencia.Columns.Contains(sColumna.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "")))
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
            sErrorFechas = validaFechas(tblCargaEficiencia);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblCargaEficiencia.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvEficiencia.DataSource = tblCargaEficiencia;                
                dgvEficiencia.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvEficiencia.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblConciliacionLeasing)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drGestion in tblCargaEficiencia.Rows)
                {
                    //Fecha
                    if (drGestion.Table.Columns.Contains("INICIO"))
                    {
                        if (!DateTime.TryParse(drGestion["INICIO"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drGestion["NUMERODECLIENTE"].ToString();
                                return "Falló, verificar la columna INICIO del contrato " + drGestion["NUMERODECLIENTE"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }               
                    }

                    if (drGestion.Table.Columns.Contains("TERMINO"))
                    {
                        if (!DateTime.TryParse(drGestion["TERMINO"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drGestion["NUMERODECLIENTE"].ToString();
                                return "Falló, verificar la columna TERMINO del contrato " + drGestion["NUMERODECLIENTE"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }
                    }

                    if (drGestion.Table.Columns.Contains("CURRENT"))
                    {
                        if (!DateTime.TryParse(drGestion["CURRENT"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drGestion["NUMERODECLIENTE"].ToString();
                                return "Falló, verificar la columna CURRENT del contrato " + drGestion["NUMERODECLIENTE"].ToString() + " contiene un tipo de dato no válido.";
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

        public void CargaEfectividad(object oDatos)
        {            
            DataTable tblCargaEficiencia = new DataTable();
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try
            {
                lector.Read();
                tblCargaEficiencia.Load(lector.Reader);
            }
            catch (Exception ex)
            {
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            for (int i = 0; i < tblCargaEficiencia.Columns.Count; i++)
                tblCargaEficiencia.Columns[i].ColumnName = tblCargaEficiencia.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'DaEf'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.DaEf");

            if (!DataBaseConn.Execute("DropTemporalDaEf")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.DaEf ( ";
            for (int i = 0; i < tblCargaEficiencia.Columns.Count; i++)
            {
                if (tblCargaEficiencia.Columns[i].ColumnName == "INICIO" || tblCargaEficiencia.Columns[i].ColumnName == "TERMINO" || tblCargaEficiencia.Columns[i].ColumnName == "CURRENT")
                    sCreateTable += ("\r\n [" + tblCargaEficiencia.Columns[i] + "] DATE NULL, ");
                //else if (tblCargaEficiencia.Columns[i].ColumnName.Contains("HORAINICIO"))
                //    sCreateTable += ("\r\n [" + tblCargaEficiencia.Columns[i] + "] DATETIME NULL, ");
                else if (tblCargaEficiencia.Columns[i].ColumnName.Contains("MONTOVENCIDOXCOBRAR") || tblCargaEficiencia.Columns[i].ColumnName.Contains("TOTALDEMONTOENRIESGO") || tblCargaEficiencia.Columns[i].ColumnName.Contains("030P") || tblCargaEficiencia.Columns[i].ColumnName.Contains("3160P") || tblCargaEficiencia.Columns[i].ColumnName.Contains("6190P") || tblCargaEficiencia.Columns[i].ColumnName.Contains("91120P") || tblCargaEficiencia.Columns[i].ColumnName.Contains("121180P") || tblCargaEficiencia.Columns[i].ColumnName.Contains("181360P"))
                    sCreateTable += ("\r\n [" + tblCargaEficiencia.Columns[i] + "] MONEY NULL, ");                
                else
                    sCreateTable += ("\r\n [" + tblCargaEficiencia.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ");";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalDaEf")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.DaEf";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblCargaEficiencia);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpFechaEficiencia.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando cuentas únicas, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Daimler].[1.4.Eficiencia] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaDaimlerEficiencia"))
                {
                    Mensaje("Falló al insertar eficiencia Daimler.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La información se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvEficiencia.DataSource = null;
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
            dgvEficiencia.DataSource = null;

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
            DataBaseConn.StartThread(CargaEfectividad);
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
