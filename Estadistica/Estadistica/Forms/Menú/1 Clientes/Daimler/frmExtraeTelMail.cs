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
    public partial class frmExtraeTelMail : Form
    {
        public frmExtraeTelMail()
        {
            InitializeComponent();
            //dtpMesBlaster.Format = DateTimePickerFormat.Custom;
            //dtpMesBlaster.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblLayout = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvLayout.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblLayout = new DataTable();

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

                tblLayout.Load(lector.Reader);                

                if (tblLayout.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                string[] Layout;
                //Layout = new string[] { "Contrato", "Folio", "Empresa", "NumCliente", "Nombre", "Corte", "Producto", "Stat", "MVs", "GWO", "Segmento", "Fecha Asig", "Fecha Prom", "Fecha Sig Gest", "Fecha Ult Diag", "Fecha Ult Gest", "Ult Clave Gestion", "Unidad", "Región_Dir", "Gerencia_Dir", "División", "Region", "Zona", "Gestor", "CVE_NOMBRE GESTOR", "NOMBRE GESTOR", "Supervisor", "Coordinador", "Criterio", "Fecha Ult Coment", "Fecha Venc Asig", "Fecha Visita", "Direccionamiento", "Esquema", "Diagnostico Cliente", "Diagnostico Credito", "DatosLocalizacion_CAlle", "DatosLocalizacion_Colonia", "DatosLocalizacion_Ciudad", "DatosLocalizacion_Estado", "DatosLocalizacion_CP", "Tel_Contacto_Fijo", "Tel_Contacto_Celular", "DatosLocalizacion_Fecha", "DatosLocalizacion_CorreoElect", "StatusCredito_Parentesco", "StatusCredito_NombreContacto", "StatusCredito_Status", "StatusCredito_StatusCliente", "StatusCredito_StatusGarantia", "StatusCredito_RazonNoPago", "Domicilio_Calle", "Domicilio_colonia", "Domicilio_municipio", "Domicilio_estado", "Domicilio_cp", "Domicilio_tel1", "Domicilio_tel2", "Domicilio_tel3", "Saldos_Capital", "Saldos_Intereses", "Saldos_SegurosyAcc", "Saldos_SubTotal", "Saldos_Mora_Gastos", "Saldos_Gran_Total", "1a Opcion Mitigante", "Importe 1a Opcion Mitigante", "2a Opcion Mitigante", "Importe 2a Opcion Mitigante", "3a Opcion Mitigante", "Importe 3a Opcion Mitigante", "4a Opcion Mitigante", "Importe 4a Opcion Mitigante", "5a Opcion Mitigante", "Importe 5a Opcion Mitigante", "6a Opcion Mitigante", "Importe 6a Opcion Mitigante", "7a Opcion Mitigante", "Importe 7a Opcion Mitigante", "Mitigante Especial", "Fecha Inicio Vigencia Mitigante", "Fecha Termino Vigencia Mitigante", "Segmento", "Fec_Bancomalo", "Prioridad", "Razon", "C&F", "Marca_Bloqueo", "No de Boletin", "Exclusion", "Abogado Interno", "Abogado_Externo", "Division HPTK", "Región HPTK", "Tipo de Juicio", "Etapa", "SubEtapa", "Estatus Juicio", "Datos Abogado Interno", "Fecha Inicio Demanda", "F_Alta_Credito", "Asignacion_Especial", "AEF", "Fecha ultimo Pago", "Fecha de confirmación de asignación", "Num Tel 1", "Num Tel 2", "Num Tel 3", "Num Tel 4", "Num Tel 5", "Num Tel 6", "Num Tel 7", "Num Tel 8", "Num Tel 9", "Num Tel 10", "Tels" };
                Layout = new string[] { "CLASIFICACION","CONTRATO","FECHA GESTION","FECHA SEGUIMIENTO","HORA INICIO","HORA TERMINO","MINUTOS DE GESTION","MONTO_SEGUIMIENTO","MOROSIDAD","ACTIVIDAD","RESULTADO","ACCION","GESTOR","OBSERVACIONES","contacto efectivo","Area","Agencia" };
                
                foreach (string sColumna in Layout)
                    if (!tblLayout.Columns.Contains(sColumna)){
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
            sErrorFechas = validaFechas(tblLayout);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblLayout.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvLayout.DataSource = tblLayout;                
                dgvLayout.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvLayout.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblLayout)
        {
            DateTime dtValidador = new DateTime();
            //int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drLayout in tblLayout.Rows){
                    if (drLayout.Table.Columns.Contains("FECHA GESTION")){
                        if (!DateTime.TryParse(drLayout["FECHA GESTION"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drLayout["CONTRATO"].ToString();
                                return "Falló, verificar la columna FECHA GESTION del CONTRATO " + drLayout["CONTRATO"].ToString() + " contiene un tipo de dato no válido.";
                            }
                        }
                    }

                    if (drLayout.Table.Columns.Contains("FECHA SEGUIMIENTO"))
                    {
                        if (!DateTime.TryParse(drLayout["FECHA SEGUIMIENTO"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m.")
                            {
                                sCuentaError = drLayout["CONTRATO"].ToString();
                                return "Falló, verificar la columna FECHA SEGUIMIENTO del CONTRATO " + drLayout["CONTRATO"].ToString() + " contiene un tipo de dato no válido.";
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

        public void CargaLayout(object oDatos)
        {
            DataTable PagosError = new DataTable(), CreaTemporalPag = new DataTable(), tblLayout = new DataTable();
            //string sErrorFechas = "";

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblLayout.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'DaimlerTelEMail' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.DaimlerTelEMail");

            if (!DataBaseConn.Execute("DropTemporalDaimlerTelEMail"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.DaimlerTelEMail ( ";
            for (int i = 0; i < tblLayout.Columns.Count; i++){
                //if (tblLayout.Columns[i].ColumnName == "Fecha Asig" || tblLayout.Columns[i].ColumnName == "Fecha Prom" || tblLayout.Columns[i].ColumnName == "Fecha Sig Gest" || tblLayout.Columns[i].ColumnName == "Fecha Ult Diag" || tblLayout.Columns[i].ColumnName == "Fecha Ult Gest" || tblLayout.Columns[i].ColumnName == "Fecha Ult Coment" || tblLayout.Columns[i].ColumnName == "Fecha Venc Asig" || tblLayout.Columns[i].ColumnName == "Fecha Visita" || tblLayout.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblLayout.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblLayout.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblLayout.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblLayout.Columns[i].ColumnName == "Fec_Bancomalo" || tblLayout.Columns[i].ColumnName == "F_Alta_Credito" || tblLayout.Columns[i].ColumnName == "Fecha ultimo Pago" || tblLayout.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblLayout.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblLayout.Columns[i].ColumnName == "FECHA GESTION" || tblLayout.Columns[i].ColumnName == "FECHA SEGUIMIENTO")
                    sCreateTable += ("\r\n [" + tblLayout.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblLayout.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalDaimlerTelEMail"))
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.DaimlerTelEMail";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblLayout);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                //string Fecha = dtpMesBlaster.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando Layout, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Daimler.[1.7.ExtraeTelEmail]");
                //DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                //DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                DataSet tblReporte = new DataSet();

                if (!DataBaseConn.Fill(tblReporte, "Exportas_Reporte_TelBankDia"))
                {
                    TerminaBúsqueda("Falló la generación del reporte.", true);
                    return;
                }

                if (tblReporte.Tables[0].Columns.Contains("Mensaje"))
                {
                    Mensaje(tblReporte.Tables[0].Rows[0]["Mensaje"].ToString(), Color.Crimson, lblMensajes);
                    Invoke((Action)delegate() { btnCargar.Visible = true; picWait.Visible = false; });
                    return;
                }

                string sRutaExcel = "";

                if (tblReporte.Tables.Count == 0 || tblReporte.Tables[0].Rows.Count == 0)
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
                    tblReporte.Dispose();
                    return;
                }

                string sResultado = ExcelXML.ExportToExcelSAX(ref tblReporte, sRutaExcel);
                TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado. Seleccione otro archivo." : sResultado, sResultado != "");
            }
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

            btnCargar.Visible = false;
            picWait.Visible = false;
            picWaitBulk.Visible = false;
            btnArchivo.Visible = true;
            txtRuta.Text = "Seleccione Archivo";
            lblRegistros.Text = "";

            dgvLayout.DataSource = null;

            this.ControlBox = true;
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
            dgvLayout.DataSource = null;

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
            DataBaseConn.StartThread(CargaLayout);
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
