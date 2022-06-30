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
    public partial class frmPersonalCJ : Form
    {
        public frmPersonalCJ()
        {
            InitializeComponent();
            dtpMesSMS.Format = DateTimePickerFormat.Custom;
            dtpMesSMS.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblPersonalCJ = new DataTable();
        dbFileReader lector;
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvPersonalCJ.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblPersonalCJ = new DataTable();

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

                tblPersonalCJ.Load(lector.Reader);                

                if (tblPersonalCJ.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                string[] Layout;                
                Layout = new string[] { "NO# EMPLEADO", "CLAVE EMPRESA", "SEXO", "NOMBRE", "STATUS", "ALTA NOMINA", "AREA ANTERIOR", "AREA", "FECHA ULTIMO CAMBIO", "FECHA BAJA", "CAUSA DE BAJA", "SUCURSAL", "RFC", "CURP", "SUELDO MENSUAL", "PUESTO ANTERIOR", "PUESTO", "ULTIMO CAMBIO" };
                
                foreach (string sColumna in Layout)
                    if (!tblPersonalCJ.Columns.Contains(sColumna)){
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
            
            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblPersonalCJ.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvPersonalCJ.DataSource = tblPersonalCJ;                
                dgvPersonalCJ.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvPersonalCJ.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }
        
        public void CargaPersonalCJ(object oDatos)
        {
            DataTable tblPersonalCJ = new DataTable() ;            
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblPersonalCJ.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'PerCJ' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.PerCJ");

            if (!DataBaseConn.Execute("DropTemporalPerCJ"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.PerCJ ( ";
            for (int i = 0; i < tblPersonalCJ.Columns.Count; i++){
                if (tblPersonalCJ.Columns[i].ColumnName == "ALTA NOMINA" || tblPersonalCJ.Columns[i].ColumnName == "FECHA ULTIMO CAMBIO" || tblPersonalCJ.Columns[i].ColumnName == "FECHA BAJA" || tblPersonalCJ.Columns[i].ColumnName == "ULTIMO CAMBIO")
                    sCreateTable += ("\r\n [" + tblPersonalCJ.Columns[i] + "] DATE NULL, ");
                else if (tblPersonalCJ.Columns[i].ColumnName == "SUELDO MENSUAL")
                    sCreateTable += ("\r\n [" + tblPersonalCJ.Columns[i] + "] MONEY NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblPersonalCJ.Columns[i] + "] VARCHAR(250) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalPerCJ"))
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.PerCJ";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblPersonalCJ);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpMesSMS.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando Personal, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[4.7.Personal] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaPersonalCJ")){
                    Mensaje("Falló al insertar Personal.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("El personal se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvPersonalCJ.DataSource = null;
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
            dgvPersonalCJ.DataSource = null;

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
            DataBaseConn.StartThread(CargaPersonalCJ);
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
