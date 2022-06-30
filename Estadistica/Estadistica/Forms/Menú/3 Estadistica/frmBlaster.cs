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
    public partial class frmBlaster : Form
    {
        public frmBlaster()
        {
            InitializeComponent();
            //dtpMesBlaster.Format = DateTimePickerFormat.Custom;
            //dtpMesBlaster.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblBlaster = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvBlaster.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblBlaster = new DataTable();

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

                tblBlaster.Load(lector.Reader);                

                if (tblBlaster.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                string[] Layout;
                //Layout = new string[] { "Contrato", "Folio", "Empresa", "NumCliente", "Nombre", "Corte", "Producto", "Stat", "MVs", "GWO", "Segmento", "Fecha Asig", "Fecha Prom", "Fecha Sig Gest", "Fecha Ult Diag", "Fecha Ult Gest", "Ult Clave Gestion", "Unidad", "Región_Dir", "Gerencia_Dir", "División", "Region", "Zona", "Gestor", "CVE_NOMBRE GESTOR", "NOMBRE GESTOR", "Supervisor", "Coordinador", "Criterio", "Fecha Ult Coment", "Fecha Venc Asig", "Fecha Visita", "Direccionamiento", "Esquema", "Diagnostico Cliente", "Diagnostico Credito", "DatosLocalizacion_CAlle", "DatosLocalizacion_Colonia", "DatosLocalizacion_Ciudad", "DatosLocalizacion_Estado", "DatosLocalizacion_CP", "Tel_Contacto_Fijo", "Tel_Contacto_Celular", "DatosLocalizacion_Fecha", "DatosLocalizacion_CorreoElect", "StatusCredito_Parentesco", "StatusCredito_NombreContacto", "StatusCredito_Status", "StatusCredito_StatusCliente", "StatusCredito_StatusGarantia", "StatusCredito_RazonNoPago", "Domicilio_Calle", "Domicilio_colonia", "Domicilio_municipio", "Domicilio_estado", "Domicilio_cp", "Domicilio_tel1", "Domicilio_tel2", "Domicilio_tel3", "Saldos_Capital", "Saldos_Intereses", "Saldos_SegurosyAcc", "Saldos_SubTotal", "Saldos_Mora_Gastos", "Saldos_Gran_Total", "1a Opcion Mitigante", "Importe 1a Opcion Mitigante", "2a Opcion Mitigante", "Importe 2a Opcion Mitigante", "3a Opcion Mitigante", "Importe 3a Opcion Mitigante", "4a Opcion Mitigante", "Importe 4a Opcion Mitigante", "5a Opcion Mitigante", "Importe 5a Opcion Mitigante", "6a Opcion Mitigante", "Importe 6a Opcion Mitigante", "7a Opcion Mitigante", "Importe 7a Opcion Mitigante", "Mitigante Especial", "Fecha Inicio Vigencia Mitigante", "Fecha Termino Vigencia Mitigante", "Segmento", "Fec_Bancomalo", "Prioridad", "Razon", "C&F", "Marca_Bloqueo", "No de Boletin", "Exclusion", "Abogado Interno", "Abogado_Externo", "Division HPTK", "Región HPTK", "Tipo de Juicio", "Etapa", "SubEtapa", "Estatus Juicio", "Datos Abogado Interno", "Fecha Inicio Demanda", "F_Alta_Credito", "Asignacion_Especial", "AEF", "Fecha ultimo Pago", "Fecha de confirmación de asignación", "Num Tel 1", "Num Tel 2", "Num Tel 3", "Num Tel 4", "Num Tel 5", "Num Tel 6", "Num Tel 7", "Num Tel 8", "Num Tel 9", "Num Tel 10", "Tels" };
                Layout = new string[] { "list_id","campaign_id","call_date","length_in_sec","status","phone_number","user","called_count","Servidor","Equivalencia","Cartera" };
                
                foreach (string sColumna in Layout)
                    if (!tblBlaster.Columns.Contains(sColumna)){
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
            sErrorFechas = validaFechas(tblBlaster);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblBlaster.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvBlaster.DataSource = tblBlaster;                
                dgvBlaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvBlaster.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblBlaster)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drBlaster in tblBlaster.Rows){
                    if (drBlaster.Table.Columns.Contains("call_date")){
                        if (!DateTime.TryParse(drBlaster["call_date"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drBlaster["Segmento"].ToString();
                                return "Falló, verificar la columna call_date del segmento " + drBlaster["phone_number"].ToString() + " contiene un tipo de dato no válido.";
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

        public void CargaBlaster(object oDatos)
        {
            DataTable PagosError = new DataTable(), CreaTemporalPag = new DataTable(), tblBlaster = new DataTable();
            //string sErrorFechas = "";

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblBlaster.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Blaster' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Blaster");

            if (!DataBaseConn.Execute("DropTemporalBlaster")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Blaster ( ";
            for (int i = 0; i < tblBlaster.Columns.Count; i++){
                //if (tblBlaster.Columns[i].ColumnName == "Fecha Asig" || tblBlaster.Columns[i].ColumnName == "Fecha Prom" || tblBlaster.Columns[i].ColumnName == "Fecha Sig Gest" || tblBlaster.Columns[i].ColumnName == "Fecha Ult Diag" || tblBlaster.Columns[i].ColumnName == "Fecha Ult Gest" || tblBlaster.Columns[i].ColumnName == "Fecha Ult Coment" || tblBlaster.Columns[i].ColumnName == "Fecha Venc Asig" || tblBlaster.Columns[i].ColumnName == "Fecha Visita" || tblBlaster.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblBlaster.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblBlaster.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblBlaster.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblBlaster.Columns[i].ColumnName == "Fec_Bancomalo" || tblBlaster.Columns[i].ColumnName == "F_Alta_Credito" || tblBlaster.Columns[i].ColumnName == "Fecha ultimo Pago" || tblBlaster.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblBlaster.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblBlaster.Columns[i].ColumnName == "call_date")
                    sCreateTable += ("\r\n [" + tblBlaster.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblBlaster.Columns[i] + "] VARCHAR(100) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalBlaster")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Blaster";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblBlaster);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpMesBlaster.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando Blaster, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[3.2.Blaster] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaBlaster")){
                    Mensaje("Falló al insertar Blaster.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La Blaster se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvBlaster.DataSource = null;
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
            dgvBlaster.DataSource = null;

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
            DataBaseConn.StartThread(CargaBlaster);
        }

        private void frmBlaster_FormClosed_1(object sender, FormClosedEventArgs e)
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
