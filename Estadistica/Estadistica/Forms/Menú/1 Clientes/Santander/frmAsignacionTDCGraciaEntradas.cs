﻿using System;
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
    public partial class frmAsignacionTDCGraciaEntradas : Form
    {
        public frmAsignacionTDCGraciaEntradas()
        {
            InitializeComponent();
            //dtpMesRemesa.Format = DateTimePickerFormat.Custom;
            //dtpMesRemesa.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblTDCGracia = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        string ArchivoExtension = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvTDCGracia.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblTDCGracia = new DataTable();

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

                tblTDCGracia.Load(lector.Reader);

                if (tblTDCGracia.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                string[] Layout;
                Layout = new string[] { "PRODUCTO","GRUPO","BUC","NOMBRE_CLIENTE","NO_CUENTA","GRACIA","TERMINACION","DIAS_MORA","BLOQUE_DIAS_MORA","PV","TOTAL_DEUDOR","BANDERA_TOTAL_DEUDOR","PAGO_MINIMO_ACT","BANDERA_PAGO_MIN_ACT","FILA","ESTATUS_FILA","FILA_PREVIA","FEC CAMBIO FILA","CANAL","AGENCIA","FEC_ASIGN_AGENCIA","AGENCIA_PREVIA","FEC_AGENCIA_PREVIA","CODIGO_BLOQUEO","COND_ECO","PV_HIST","PV_3","PV_2","PV_1","MIGRACIONES","BEHAVIOR_SCORE","DESCRIP_PROD","FEC_APERTURA_CTA","AÑO","MES","MARCA_LE","FEC_CORTE","DIA_CORTE","STATUS_CASTIGO","PROGRAMA_ESPECIAL","BANDERA_MOROSO","BANDERA_PROMESA","FEC_PROMESA","FEC_PAGO_PROMESA","LOCATION","MARCA_UNIVERSIDADES","CENTRO_ALTA","CONTRATO","SALDO30","SALDO60","SALDO90","REMANENTES","PROD","SUBPRODUCTO","CLAVE_PRODUCTO","MAX_CR_CC","CALIFICACION_CC","SUBCALIFICACION_CC","TOQUES_CC","BANDERA_TELEFONOS","TELEFONOS","EMAIL","U6DOMACT2","LIMITE_CREDITO" };

                for (int i = 0; i < tblTDCGracia.Columns.Count; i++)
                    tblTDCGracia.Columns[i].ColumnName = tblTDCGracia.Columns[i].ColumnName.Replace("+", "").Trim();
                

                foreach (string sColumna in Layout)
                    if (!tblTDCGracia.Columns.Contains(sColumna))
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

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblTDCGracia.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvTDCGracia.DataSource = tblTDCGracia;                
                dgvTDCGracia.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvTDCGracia.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

       

        public void CargaTDCGracia(object oDatos)
        {
            //DataTable PagosError = new DataTable(), CreaTemporalPag = new DataTable(), tblTDCGracia = new DataTable();
            DataTable tblTDCGracia = new DataTable();
            DataTable CreaTemporalPag = new DataTable();
            //string sErrorFechas = "";

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblTDCGracia.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'TDCGraciaEntradas'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.TDCGraciaEntradas");

            if (!DataBaseConn.Execute("DropTemporalTDCGraciaEntradas"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.TDCGraciaEntradas ( ";
            for (int i = 0; i < tblTDCGracia.Columns.Count; i++)
            {
                if (tblTDCGracia.Columns[i].ColumnName == "FEC CAMBIO FILA" || tblTDCGracia.Columns[i].ColumnName == "FEC_ASIGN_AGENCIA" || tblTDCGracia.Columns[i].ColumnName == "FEC_AGENCIA_PREVIA" || tblTDCGracia.Columns[i].ColumnName == "FEC_APERTURA_CTA" || tblTDCGracia.Columns[i].ColumnName == "FEC_CORTE" || tblTDCGracia.Columns[i].ColumnName == "FEC_PROMESA" || tblTDCGracia.Columns[i].ColumnName == "FEC_PAGO_PROMESA")
                    sCreateTable += ("\r\n [" + tblTDCGracia.Columns[i] + "] DATE NULL, ");
                else if (tblTDCGracia.Columns[i].ColumnName == "TOTAL_DEUDOR" || tblTDCGracia.Columns[i].ColumnName == "PAGO_MINIMO_ACT" || tblTDCGracia.Columns[i].ColumnName == "SALDO30" || tblTDCGracia.Columns[i].ColumnName == "SALDO60" || tblTDCGracia.Columns[i].ColumnName == "SALDO90" || tblTDCGracia.Columns[i].ColumnName == "LIMITE_CREDITO")
                    sCreateTable += ("\r\n [" + tblTDCGracia.Columns[i] + "] MONEY NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblTDCGracia.Columns[i] + "] VARCHAR(250) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalTDCGraciaEntradas")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.TDCGraciaEntradas";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblTDCGracia);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpDiaAsignacion.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando información, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC [Santander].[AsigTDCGraciaEntradas] @Fecha_Proceso, @idEjecutivo, @ArchivoExtension");
                DataBaseConn.CommandParameters.AddWithValue("@Fecha_Proceso", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@ArchivoExtension", ArchivoExtension);


                if (!DataBaseConn.Execute("InsertaTDCGracia"))
                {
                    Mensaje("Falló al insertar información del día de hoy.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La información se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvTDCGracia.DataSource = null;
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
            dgvTDCGracia.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            txtRuta.Text = ofdArchivo.FileName;
            ArchivoExtension = ofdArchivo.SafeFileName.ToString();
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
            DataBaseConn.StartThread(CargaTDCGracia);
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
