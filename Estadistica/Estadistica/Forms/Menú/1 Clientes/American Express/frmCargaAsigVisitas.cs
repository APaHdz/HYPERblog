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

using ExcelDataReader;

namespace Estadistica
{
    public partial class frmCargaAsigVisitas : Form
    {
        public frmCargaAsigVisitas()
        {
            InitializeComponent();
            //dtpInicial.Format = DateTimePickerFormat.Custom;
            //dtpInicial.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables        
        DataTable tblAsignacion = new DataTable();
        string Tabla = "";
        string[] Layout;
        int Campos = 0;
        string NombreColumna = "";
        int iNumberRecords = 0;
        dbFileReader lector;
        string sFilePath = "";
        string fileName = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvAsignacion.DefaultCellStyle.Font = controlEvents.RowFont;
            cmbSegmento.SelectedIndex = 0;
        }

        protected void LimpiaMensajes()
        {
            Mensaje("", Color.DimGray, lblRegistros);
            Mensaje("", Color.DimGray, lblInstrucciones);
            Mensaje("", Color.DimGray, lblMensajes);
        }

        public void Archivo(object oFilePath)
        {
            sFilePath = oFilePath.ToString().Trim();
            fileName = Path.GetFileName(sFilePath);
            tblAsignacion = new DataTable();

            if (!File.Exists(sFilePath)){
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            lector = new dbFileReader(sFilePath);

            string sResultado = lector.Open();

            if (sResultado != "")
            {
                Mensaje(sResultado, Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            lector.Open();
            iNumberRecords = lector.CountRows();

            //Ruta del fichero Excel
            string filePath = sFilePath;            
            string path = filePath;
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; " +
                    "SELECT DISTINCT Archivo FROM dbEstadistica.Amex.[AsignacionVisitas_D2D] WHERE Archivo = @Archivo \n" +
                    "UNION ALL \n SELECT DISTINCT Archivo FROM dbEstadistica.Amex.[AsignacionVisitas_HIR] WHERE Archivo = @Archivo \n"
                    );
            DataBaseConn.CommandParameters.AddWithValue("@Archivo", fileName);
            DataTable tblArchivo = new DataTable();
            if (!DataBaseConn.Fill(tblArchivo, "ValidaArchivo" + fileName))
            {
                Mensaje("Falló al validar archivo(" + Tabla + "), intente nuevamente", Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            if (tblArchivo.Rows.Count > 0)
            {
                if (MessageBox.Show("El archivo ya fue cargado anteriormente, ¿Desea eliminar la información y cargarla nuevamente?", "Archivo existente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    Invoke((Action)delegate()
                    {
                        Mensaje("La carga ha sido cancelada por el usuario ya que el archivo ya estaba en el sistema.", Color.DimGray, lblMensajes);
                        btnCargar.Visible = false;
                        btnArchivo.Visible = true;
                        picWait.Visible = false;
                        txtRuta.Text = "";
                    });
                    return;
                }
            }

            try
            {
                lector.Read(100);

                tblAsignacion.Load(lector.Reader);

                if (tblAsignacion.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                if (Tabla == "D2D")
                {
                    Layout = new string[] { "Cuenta", "Ctacompleta", "Age Charge", "Age Lending", "Last Action", "WL actual", "PTP", "date last action", "Notes", "expo charge", "expo lend", "Due Charge", "due lending", "lift date", "cb score", "NAME", "ADD1", "ADD2", "ADD3", "ADD4", "ADD5", "ADD6", "ZIP", "Fecha de asignación", "Termino de gestión" };
                    foreach (string sColumna in Layout)
                        if (!tblAsignacion.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;
                        }
                }
                else if (Tabla.Contains("HIR"))
                {
                    Layout = new string[] { "CS", "Cuenta", "Cta completa", "Age Charge", "Age Lending", "Last Action", "WL actual", "PTP", "date last action", "Notes", "expo charge", "expo lend", "Due Charge", "due lending", "DPD", "TOTAL x EXP", "lift date", "cb score", "NAME", "ADD1", "ADD2", "ADD3", "ADD4", "ADD5", "ADD6", "ZIP", "Portafolio", "Direccion Completa", "Datos adicionales", "Fecha de asignación", "Termino de gestión" };
                    foreach (string sColumna in Layout)
                        if (!tblAsignacion.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;
                        }
                }
                else
                {
                    Mensaje("El layout no corresponde a ninguna pestaña establecida como layout(" + Tabla + ").", Color.Red, lblMensajes);
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }                
            }
            catch (Exception ex)
            {
                Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblAsignacion.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate()
            {
                dgvAsignacion.DataSource = tblAsignacion;
                dgvAsignacion.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvAsignacion.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);            
        }  
      
        public void CargaVisitas(object oDatos)
        {
            tblAsignacion = new DataTable();
            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try
            {
                lector.Read();
                tblAsignacion.Load(lector.Reader);
            }
            catch (Exception ex)
            {
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'AmexVisitasOGR'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.AmexVisitasOGR");

            if (!DataBaseConn.Execute("DropTemporalAmexVisitasOGR"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.[AmexVisitasOGR] ( ";
            for (int y = 0; y < tblAsignacion.Columns.Count; y++)
            {
                if (tblAsignacion.Columns[y].ColumnName == "Fecha de asignación" || tblAsignacion.Columns[y].ColumnName == "Termino de gestión")
                    sCreateTable += ("\r\n [" + tblAsignacion.Columns[y] + "] DATE NULL, ");
                else if (tblAsignacion.Columns[y].ColumnName == "expo charge" || tblAsignacion.Columns[y].ColumnName == "expo lend" || tblAsignacion.Columns[y].ColumnName == "Due charge" || tblAsignacion.Columns[y].ColumnName == "due lending" || tblAsignacion.Columns[y].ColumnName == "TOTAL x EXP")
                    sCreateTable += ("\r\n [" + tblAsignacion.Columns[y] + "] MONEY NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblAsignacion.Columns[y] + "] VARCHAR(250) NULL, ");
            }
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Execute("CreaTemporal(" + Tabla + ")"))
            {
                Mensaje("Falló al crear tabla temporal(" + Tabla + ")", Color.Crimson, lblMensajes);
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.AmexVisitasOGR";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblAsignacion);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                Mensaje("Insertando información(" + Tabla + "), por favor espere...", Color.DimGray, lblMensajes);
                if (Tabla == "D2D")
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.2.AsignacionVisitas_D2D] @FechaProceso, @Archivo, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");
                else
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.2.AsignacionVisitas_HIR] @FechaProceso, @Archivo, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");

                string FechaProceso = dtpAsignacion.Value.ToString("yyyy-MM-dd");            
                string Fecha_Insert = DateTime.Now.ToString("yyyy-MM-dd");
                string Segundo_Insert = DateTime.Now.ToString("HH:mm:ss");
                DataBaseConn.CommandParameters.AddWithValue("@FechaProceso", FechaProceso);
                DataBaseConn.CommandParameters.AddWithValue("@Archivo", fileName);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha_Insert", Fecha_Insert);
                DataBaseConn.CommandParameters.AddWithValue("@Segundo_Insert", Segundo_Insert);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);

                if (!DataBaseConn.Execute("InsertaAmexVisitasOGR"))
                {
                    Mensaje("Falló al insertar visitas.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La información se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate
                {
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvAsignacion.DataSource = null;
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
            dgvAsignacion.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            txtRuta.Text = ofdArchivo.FileName;
            if (MessageBox.Show("¿La fecha de asignación, segmento y el archivo seleccionado son correctos? \n\n" + 
                "Inicial: " + dtpAsignacion.Value.ToString("yyyy-MM-dd") + "\n\n" +
                "Segmento: " + cmbSegmento.Text + "\n\n" +
                "Archivo: " + Path.GetFileName(ofdArchivo.FileName.ToString().Trim()).ToString() + "\n\n" 
                , "Validación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Mensaje("La carga ha sido cancelada por el usuario.", Color.DimGray, lblMensajes);
                btnCargar.Visible = false;
                btnArchivo.Visible = true;
                picWait.Visible = false;
                txtRuta.Text = "";
                Tabla = "";
            }
            else
            {
                Mensaje("Cargando archivo de Excel, por favor espere...", Color.DimGray, lblMensajes);
                btnCargar.Visible = btnArchivo.Visible = false;
                picWait.Visible = true;
                Tabla = cmbSegmento.Text;
                DataBaseConn.StartThread(Archivo, ofdArchivo.FileName);
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            Mensaje("", Color.DimGray, lblInstrucciones);
            btnCargar.Visible = false;
            picWaitBulk.Visible = true;
            btnArchivo.Visible = false;
            DataBaseConn.StartThread(CargaVisitas);
        }

        private void frmCargaFacturas_FormClosed(object sender, FormClosedEventArgs e)
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
