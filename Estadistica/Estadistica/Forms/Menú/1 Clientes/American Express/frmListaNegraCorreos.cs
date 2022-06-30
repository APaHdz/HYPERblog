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
    public partial class frmListaNegraCorreos : Form
    {
        public frmListaNegraCorreos()
        {
            InitializeComponent();
            //dtpMesBlaster.Format = DateTimePickerFormat.Custom;
            //dtpMesBlaster.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblListaNegra = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvListanegra.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblListaNegra = new DataTable();

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

                tblListaNegra.Load(lector.Reader);                

                if (tblListaNegra.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                string[] Layout;
                //Layout = new string[] { "Contrato", "Folio", "Empresa", "NumCliente", "Nombre", "Corte", "Producto", "Stat", "MVs", "GWO", "Segmento", "Fecha Asig", "Fecha Prom", "Fecha Sig Gest", "Fecha Ult Diag", "Fecha Ult Gest", "Ult Clave Gestion", "Unidad", "Región_Dir", "Gerencia_Dir", "División", "Region", "Zona", "Gestor", "CVE_NOMBRE GESTOR", "NOMBRE GESTOR", "Supervisor", "Coordinador", "Criterio", "Fecha Ult Coment", "Fecha Venc Asig", "Fecha Visita", "Direccionamiento", "Esquema", "Diagnostico Cliente", "Diagnostico Credito", "DatosLocalizacion_CAlle", "DatosLocalizacion_Colonia", "DatosLocalizacion_Ciudad", "DatosLocalizacion_Estado", "DatosLocalizacion_CP", "Tel_Contacto_Fijo", "Tel_Contacto_Celular", "DatosLocalizacion_Fecha", "DatosLocalizacion_CorreoElect", "StatusCredito_Parentesco", "StatusCredito_NombreContacto", "StatusCredito_Status", "StatusCredito_StatusCliente", "StatusCredito_StatusGarantia", "StatusCredito_RazonNoPago", "Domicilio_Calle", "Domicilio_colonia", "Domicilio_municipio", "Domicilio_estado", "Domicilio_cp", "Domicilio_tel1", "Domicilio_tel2", "Domicilio_tel3", "Saldos_Capital", "Saldos_Intereses", "Saldos_SegurosyAcc", "Saldos_SubTotal", "Saldos_Mora_Gastos", "Saldos_Gran_Total", "1a Opcion Mitigante", "Importe 1a Opcion Mitigante", "2a Opcion Mitigante", "Importe 2a Opcion Mitigante", "3a Opcion Mitigante", "Importe 3a Opcion Mitigante", "4a Opcion Mitigante", "Importe 4a Opcion Mitigante", "5a Opcion Mitigante", "Importe 5a Opcion Mitigante", "6a Opcion Mitigante", "Importe 6a Opcion Mitigante", "7a Opcion Mitigante", "Importe 7a Opcion Mitigante", "Mitigante Especial", "Fecha Inicio Vigencia Mitigante", "Fecha Termino Vigencia Mitigante", "Segmento", "Fec_Bancomalo", "Prioridad", "Razon", "C&F", "Marca_Bloqueo", "No de Boletin", "Exclusion", "Abogado Interno", "Abogado_Externo", "Division HPTK", "Región HPTK", "Tipo de Juicio", "Etapa", "SubEtapa", "Estatus Juicio", "Datos Abogado Interno", "Fecha Inicio Demanda", "F_Alta_Credito", "Asignacion_Especial", "AEF", "Fecha ultimo Pago", "Fecha de confirmación de asignación", "Num Tel 1", "Num Tel 2", "Num Tel 3", "Num Tel 4", "Num Tel 5", "Num Tel 6", "Num Tel 7", "Num Tel 8", "Num Tel 9", "Num Tel 10", "Tels" };
                Layout = new string[] { "Correo", "RESULTADO", "ORIGEN" };
                
                foreach (string sColumna in Layout)
                    if (!tblListaNegra.Columns.Contains(sColumna)){
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
            sErrorFechas = validaFechas(tblListaNegra);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblListaNegra.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvListanegra.DataSource = tblListaNegra;                
                dgvListanegra.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvListanegra.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblListaNegra)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drBlaster in tblListaNegra.Rows){
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

        public void CargaListaNegra(object oDatos)
        {
            DataTable PagosError = new DataTable(), CreaTemporalPag = new DataTable(), tblListaNegra = new DataTable();
            //string sErrorFechas = "";

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblListaNegra.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'LNAMXCORREO' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.LNAMXCORREO");

            if (!DataBaseConn.Execute("DropTemporalLNAMXCORREO"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.LNAMXCORREO ( ";
            for (int i = 0; i < tblListaNegra.Columns.Count; i++){
                //if (tblListaNegra.Columns[i].ColumnName == "Fecha Asig" || tblListaNegra.Columns[i].ColumnName == "Fecha Prom" || tblListaNegra.Columns[i].ColumnName == "Fecha Sig Gest" || tblListaNegra.Columns[i].ColumnName == "Fecha Ult Diag" || tblListaNegra.Columns[i].ColumnName == "Fecha Ult Gest" || tblListaNegra.Columns[i].ColumnName == "Fecha Ult Coment" || tblListaNegra.Columns[i].ColumnName == "Fecha Venc Asig" || tblListaNegra.Columns[i].ColumnName == "Fecha Visita" || tblListaNegra.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblListaNegra.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblListaNegra.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblListaNegra.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblListaNegra.Columns[i].ColumnName == "Fec_Bancomalo" || tblListaNegra.Columns[i].ColumnName == "F_Alta_Credito" || tblListaNegra.Columns[i].ColumnName == "Fecha ultimo Pago" || tblListaNegra.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblListaNegra.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblListaNegra.Columns[i].ColumnName == "call_date")
                    sCreateTable += ("\r\n [" + tblListaNegra.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblListaNegra.Columns[i] + "] VARCHAR(200) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalLNAMXCORREO"))
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.LNAMXCORREO";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblListaNegra);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpFecha.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando Blaster, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Amex.[3.1.ListaNegraCorreos] @Fecha, @idEjecutivo");
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);                

                if (!DataBaseConn.Execute("InsertaListaNegra")){
                    Mensaje("Falló al insertar Lista negra de correos.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La lista negra de correos se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvListanegra.DataSource = null;
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
            dgvListanegra.DataSource = null;

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
            DataBaseConn.StartThread(CargaListaNegra);
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
