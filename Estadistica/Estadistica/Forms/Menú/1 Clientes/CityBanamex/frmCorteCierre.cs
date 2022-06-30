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
    public partial class frmCorteCierre : Form
    {
        public frmCorteCierre()
        {
            InitializeComponent();
            //dtpCierre.Format = DateTimePickerFormat.Custom;
            //dtpCierre.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblNegociaciones = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvCierre.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblNegociaciones = new DataTable();

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

                tblNegociaciones.Load(lector.Reader);                

                if (tblNegociaciones.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                //for (int i = 0; i < tblNegociaciones.Columns.Count; i++)
                //    tblNegociaciones.Columns[i].ColumnName = tblNegociaciones.Columns[i].ColumnName.Replace(" ", "").Replace(".", "").Replace("/", "").Replace("*", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("$", "").Replace("¿", "").Replace("?", "").Replace("#", "").ToUpper();

                string[] Layout;                
                Layout = new string[] { "Nombre", "Clave", "Supervisor", "4TO CORTE", "Settlement (pt)", "Settlement a Plazo (cp)", "Payment Plan (lp)", "Saldo Negociado" };
                
                foreach (string sColumna in Layout)
                    if (!tblNegociaciones.Columns.Contains(sColumna)){
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
            sErrorFechas = validaFechas(tblNegociaciones);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblNegociaciones.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvCierre.DataSource = tblNegociaciones;                
                dgvCierre.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvCierre.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblNegociaciones)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drNegociaciones in tblNegociaciones.Rows){
                    if (drNegociaciones.Table.Columns.Contains("FECHADERECEPCION"))
                    {
                        if (!DateTime.TryParse(drNegociaciones["FECHADERECEPCION"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drNegociaciones["CUENTAS"].ToString();
                                return "Falló, verificar la columna FECHADERECEPCION del segmento " + drNegociaciones["CUENTAS"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }                    
                    }

                    if (drNegociaciones.Table.Columns.Contains("FECHADEVENCIMIENTO"))
                    {                        
                        if (!DateTime.TryParse(drNegociaciones["FECHADEVENCIMIENTO"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drNegociaciones["CUENTAS"].ToString();
                                return "Falló, verificar la columna FECHADEVENCIMIENTO del segmento " + drNegociaciones["CUENTAS"].ToString() + " contiene un tipo de dato no válido.";
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

        public void CargaNegociaciones(object oDatos)
        {
            DataTable SummaryError = new DataTable(), CreaTemporalPag = new DataTable(), tblNegociaciones = new DataTable();
            //string sErrorFechas = "";

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblNegociaciones.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'BaCC' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.BaCC");

            if (!DataBaseConn.Execute("DropTemporalCorteCierre")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.BaCC ( ";
            for (int i = 0; i < tblNegociaciones.Columns.Count; i++){
                if (tblNegociaciones.Columns[i].ColumnName == "Fecha de recepcion" || tblNegociaciones.Columns[i].ColumnName == "Fecha de Vencimiento")
                    sCreateTable += ("\r\n [" + tblNegociaciones.Columns[i] + "] DATE NULL, ");
                else if (tblNegociaciones.Columns[i].ColumnName == "Saldo Negociado" || tblNegociaciones.Columns[i].ColumnName == "Importe Pago inicial")
                    sCreateTable += ("\r\n [" + tblNegociaciones.Columns[i] + "] MONEY NULL, ");
                else if (tblNegociaciones.Columns[i].ColumnName == "Num de Pagos")
                    sCreateTable += ("\r\n [" + tblNegociaciones.Columns[i] + "] SMALLINT NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblNegociaciones.Columns[i] + "] VARCHAR(300) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalCorteCierre")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.BaCC";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblNegociaciones);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpCierre.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando cierre corte, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.CityBanamex.[2.3.CorteCierre] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaSummary")){
                    Mensaje("Falló al insertar corte cierre CityBanamex.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La información se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvCierre.DataSource = null;
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
            dgvCierre.DataSource = null;

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
            DataBaseConn.StartThread(CargaNegociaciones);
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
