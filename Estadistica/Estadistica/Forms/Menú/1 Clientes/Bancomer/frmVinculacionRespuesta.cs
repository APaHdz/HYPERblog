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
    public partial class frmVinculacionRespuesta : Form
    {
        public frmVinculacionRespuesta()
        {
            InitializeComponent();
            //dtpVinculacion.Format = DateTimePickerFormat.Custom;
            //dtpVinculacion.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblVinculaciones = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvVinculacion.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblVinculaciones = new DataTable();

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

                tblVinculaciones.Load(lector.Reader);

                if (tblVinculaciones.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                for (int i = 0; i < tblVinculaciones.Columns.Count; i++)
                    tblVinculaciones.Columns[i].ColumnName = tblVinculaciones.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

                string[] Layout;
                Layout = new string[] { "CONTRATO", "CUENTA", "STATUS", "SALDO", "MORA", "AGENCIA", "CICLO", "%", "FECHA Lim. APLICACION", "PAGO INICIAL", "FOLIO", "STATUS", "COMENTARIO" };
                
                foreach (string sColumna in Layout)
                    if (!tblVinculaciones.Columns.Contains(sColumna.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "")))
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
            sErrorFechas = validaFechas(tblVinculaciones);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblVinculaciones.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvVinculacion.DataSource = tblVinculaciones;                
                dgvVinculacion.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvVinculacion.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblConciliacionLeasing)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drConciliacion in tblVinculaciones.Rows)
                {
                    //Fecha
                    if (drConciliacion.Table.Columns.Contains("FECHA"))
                    {
                        if (!DateTime.TryParse(drConciliacion["FECHA"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drConciliacion["CONTRATO"].ToString();
                                return "Falló, verificar la columna Fecha del contrato " + drConciliacion["CONTRATO"].ToString() + " contiene un tipo de dato no válido.";
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

        public void CargaVinculaciones(object oDatos)
        {            
            DataTable tblVinculaciones = new DataTable();
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try
            {
                lector.Read();
                tblVinculaciones.Load(lector.Reader);
            }
            catch (Exception ex)
            {
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            for (int i = 0; i < tblVinculaciones.Columns.Count; i++)
                tblVinculaciones.Columns[i].ColumnName = tblVinculaciones.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'RVin'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.RVin");

            if (!DataBaseConn.Execute("DropTemporalRVin"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.RVin ( ";
            for (int i = 0; i < tblVinculaciones.Columns.Count; i++)
            {
                if (tblVinculaciones.Columns[i].ColumnName == "FECHALIMAPLICACION")
                    sCreateTable += ("\r\n [" + tblVinculaciones.Columns[i] + "] DATE NULL, ");                
                else if (tblVinculaciones.Columns[i].ColumnName.Contains("SALDO"))
                    sCreateTable += ("\r\n [" + tblVinculaciones.Columns[i] + "] MONEY NULL, ");
                else if (tblVinculaciones.Columns[i].ColumnName.Contains("PAGO"))
                    sCreateTable += ("\r\n [" + tblVinculaciones.Columns[i] + "] MONEY NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblVinculaciones.Columns[i] + "] VARCHAR(50) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ");";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "RVin"))
            {
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.RVin";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblVinculaciones);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpVinculacion.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando centros de acopio del mes, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Bancomer].[1.10.0.VinculacionRespuesta] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaVinculaciones"))
                {
                    Mensaje("Falló al insertar Vinculaciones.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La información se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvVinculacion.DataSource = null;
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
            dgvVinculacion.DataSource = null;

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
            if (MessageBox.Show("¿Desea eliminar y cargar la respuesta del día seleccionado?", "Elimina/Carga respuesta vinculación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                txtRuta.Text = "";
                btnArchivo.Visible = true;
                dgvVinculacion.DataSource = null;
                dgvVinculacion.Visible = false;
                LimpiaMensajes();
                Mensaje("Selecciona un archivo.", Color.SlateGray, lblMensajes);
                return;
            }
            else
            {
                Mensaje("", Color.DimGray, lblInstrucciones);
                btnCargar.Visible = false;
                picWaitBulk.Visible = true;
                btnArchivo.Visible = false;
                DataBaseConn.StartThread(CargaVinculaciones);
            }
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
