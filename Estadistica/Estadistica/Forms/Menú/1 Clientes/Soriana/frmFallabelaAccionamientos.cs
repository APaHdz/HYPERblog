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
    public partial class frmFallabelaAccionamientos : Form
    {
        public frmFallabelaAccionamientos()
        {
            InitializeComponent();
            //dtpMesRemesa.Format = DateTimePickerFormat.Custom;
            //dtpMesRemesa.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }
        
        #region Variables
        int iNumberRecords;
        //DataTable tblGestiones = new DataTable();
        //dbFileReader lector;
        //string sCuentaError = "";
        string separadorArchivos = "";
        DataTable workTable = new DataTable();
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvDiaAccionamientos.DefaultCellStyle.Font = controlEvents.RowFont;
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
            //tblGestiones = new DataTable();

            if (!File.Exists(sFilePath)){
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }            

            workTable = new DataTable();
            DataColumn idcuenta = new DataColumn("idcuenta");
            DataColumn Producto = new DataColumn("Producto");
            DataColumn Segmento = new DataColumn("Segmento");
            DataColumn Fecha = new DataColumn("Fecha");
            DataColumn Tipo = new DataColumn("Tipo");
            
            workTable.Columns.Add(idcuenta);
            workTable.Columns.Add(Producto);
            workTable.Columns.Add(Segmento);
            workTable.Columns.Add(Fecha);
            workTable.Columns.Add(Tipo);

            separadorArchivos = "|";

            string FileToRead = sFilePath;
            using (StreamReader ReaderObject = new StreamReader(FileToRead))
            {
                string line;
                // ReaderObject reads a single line, stores it in Line string variable and then displays it on console
                while ((line = ReaderObject.ReadLine()) != null)
                {                    
                    try
                    {
                        //line = sr.ReadLine();

                        line = line.Replace("?", "");
                        line = line.Replace("\"", "");
                        line = line.Replace("\t", separadorArchivos);
                        string[] valores = line.Split(Convert.ToChar("|"));
                        DataRow row1 = workTable.NewRow();
                        row1["idcuenta"] = valores[0].ToString();
                        row1["Producto"] = valores[1].ToString();
                        row1["Segmento"] = valores[2].ToString();
                        row1["Fecha"] = valores[3].ToString();
                        row1["Tipo"] = valores[4].ToString();
                        workTable.Rows.Add(row1);
                    }
                    catch { }
                }
            }            

            iNumberRecords = workTable.Rows.Count;
            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + workTable.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvDiaAccionamientos.DataSource = workTable;                
                dgvDiaAccionamientos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvDiaAccionamientos.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

       
        public void CargaRemesa(object oDatos)
        {
            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'AccSF' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.AccSF");

            if (!DataBaseConn.Execute("DropTemporalRemesa")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.AccSF ( ";
            for (int i = 0; i < workTable.Columns.Count; i++){
                //if (tblGestiones.Columns[i].ColumnName == "Fecha Asig" || tblGestiones.Columns[i].ColumnName == "Fecha Prom" || tblGestiones.Columns[i].ColumnName == "Fecha Sig Gest" || tblGestiones.Columns[i].ColumnName == "Fecha Ult Diag" || tblGestiones.Columns[i].ColumnName == "Fecha Ult Gest" || tblGestiones.Columns[i].ColumnName == "Fecha Ult Coment" || tblGestiones.Columns[i].ColumnName == "Fecha Venc Asig" || tblGestiones.Columns[i].ColumnName == "Fecha Visita" || tblGestiones.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblGestiones.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblGestiones.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblGestiones.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblGestiones.Columns[i].ColumnName == "Fec_Bancomalo" || tblGestiones.Columns[i].ColumnName == "F_Alta_Credito" || tblGestiones.Columns[i].ColumnName == "Fecha ultimo Pago" || tblGestiones.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblGestiones.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (workTable.Columns[i].ColumnName == "Fecha Asig")
                    sCreateTable += ("\r\n [" + workTable.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + workTable.Columns[i] + "] VARCHAR(8000) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Execute("CreaTemporalRem")){
                Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.AccSF";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(workTable);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpDiaAccionamientos.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando accionamientos, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Soriana.[4.3.AccionamientosFalabella] @idEjecutivo, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaAccionamientosFalabella")){
                    Mensaje("Falló al insertar accionamientos Falabella.", Color.Red, lblMensajes);
                    //lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("Los accionamientos se insertarón en el sistema.", Color.DimGray, lblMensajes);

                //lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvDiaAccionamientos.DataSource = null;
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
            dgvDiaAccionamientos.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            txtRuta.Text = ofdArchivo.FileName;
            Mensaje("Cargando vista previa del archivo de texto, por favor espere...", Color.DimGray, lblMensajes);
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
            DataBaseConn.StartThread(CargaRemesa);
        }

        private void frmRemesa_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
            //if (lector != null)
            //    lector.Close();
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
