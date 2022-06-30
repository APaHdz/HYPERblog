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
    public partial class frmAsignacionesCJ : Form
    {
        public frmAsignacionesCJ()
        {
            InitializeComponent();
            //dtpMesAsignacion.Format = DateTimePickerFormat.Custom;
            //dtpMesAsignacion.CustomFormat = "MM/yyyy";
            dtpMesAsignacion.Value = DateTime.Today.AddDays(-0);
            dtpMesAsignacion.MaxDate = dtpMesAsignacion.Value;
            dtpMesAsignacion.MinDate = DateTime.Today.AddDays(-365);
            PreparaVentana();
        }

        #region Variables
        int iNumberRecords;
        DataTable tblAsignacion = new DataTable();
        dbFileReader lector;
        string Producto = "";
        string sCuentaError = "";
        string sTipo = "";
        string[] Layout;
        int idCartera = 0;
        int idProducto = 0;
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {
            //Productos
            cmbProducto.ValueMember = "idProducto";
            cmbProducto.DisplayMember = "Producto";
            cmbProducto.DataSource = Catálogo.Productos;

            //Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;

            cmbProducto.SelectedIndex = -1;
            dgvAsignacion.DefaultCellStyle.Font = controlEvents.RowFont;
            Mensaje("Seleccione un producto.", Color.SlateGray, lblMensajes);

            cmbTipo.SelectedIndex = 0;
        }
        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;

            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;            
            cmbProducto.SelectedIndex = -1;

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
            tblAsignacion = new DataTable();

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

                tblAsignacion.Load(lector.Reader);

                //tblPersonal.Columns["Entrada"].DataType = typeof(TimeSpan);

                if (tblAsignacion.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }
                                
                
                Layout = new string[] { "Cuenta1", "Cuenta2", "Cliente", "Saldo", "SaldoCobrar", "SaldoExtra", "Mora", "Segmento", "Codigo1", "Codigo2", "Codigo3", "Codigo4", "Codigo5", "Plaza", "Region", "Division", "Sucursal", "Fecha1", "Fecha2", "Numero1", "Numero2" };
                
                   foreach (string sColumna in Layout)
                    if (!tblAsignacion.Columns.Contains(sColumna)){
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

            //string sErrorFechas = "";
            //Mensaje("Validando fechas del archivo seleccionado, por favor espere...", Color.DimGray, lblMensajes);
            //sErrorFechas = validaFechas(tblAsignacion);
            //if (sErrorFechas != ""){
            //    Mensaje(sErrorFechas, Color.Red, lblMensajes);
            //    lector.Close();
            //    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
            //    return;
            //}

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblAsignacion.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvAsignacion.DataSource = tblAsignacion;
                //dgvAsignacion.Columns["Entrada"].DefaultCellStyle.Format = "t";
                //dgvAsignacion.Columns["Salida"].DefaultCellStyle.Format = "t";
                dgvAsignacion.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvAsignacion.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        //private string validaFechas(DataTable tblAsignacion)
        //{
        //    DateTime dtValidador = new DateTime();
        //    int i = 0;
        //    string Dato = "";
        //    try{
        //        foreach (DataRow drAsignacion in tblAsignacion.Rows){
        //            if (drAsignacion.Table.Columns.Contains("Fecha Pago")){
        //                if (!DateTime.TryParse(drAsignacion["Fecha Pago"].ToString(), out dtValidador)){
        //                    Dato = dtValidador.ToString();
        //                    if (Dato != "01/01/0001 12:00:00 a. m."){
        //                        sCuentaError = drAsignacion["CtaNumero"].ToString();
        //                        return "Falló, la fecha contiene un tipo de dato no válido.";
        //                    }
        //                }
        //            }                                    
        //        }
        //    }
        //    catch (Exception ex){
        //        return "Falló " + ex.Message;
        //    }
        //    return "";
        //}

        public void CargaAsignacion(object oDatos)
        {
            DataTable AsignacionError = new DataTable(), CreaTemporalPag = new DataTable(), tblAsignacion = new DataTable();            

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblAsignacion.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }
           
            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Asig_" + Ejecutivo.Datos["idEjecutivo"] + "'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Asig_" + Ejecutivo.Datos["idEjecutivo"]);

            if (!DataBaseConn.Execute("DropTemporalAsig")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Asig_" + Ejecutivo.Datos["idEjecutivo"] + " ( ";
            for (int i = 0; i < tblAsignacion.Columns.Count; i++){
                if (tblAsignacion.Columns[i].ColumnName.Contains("Fecha"))
                    sCreateTable += ("\r\n [" + tblAsignacion.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblAsignacion.Columns[i] + "] VARCHAR(200) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);

            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalAsig")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Asig_" + Ejecutivo.Datos["idEjecutivo"];
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

                string Fecha = dtpMesAsignacion.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando Asignacion, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.2.Asignacion] @idCartera, @idProducto, @idEjecutivo, @Fecha, @Tipo ");                                
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@Tipo", sTipo);

                if (!DataBaseConn.Execute("InsertaAsignacion")){
                    Mensaje("Falló al insertar Asignacion.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; cmbProducto.SelectedIndex = -1; });
                    return;
                }

                Mensaje("Los Asignación se insertó en el sistema.", Color.DimGray, lblMensajes);                

                lector.Close();

                Mensaje("Insertando domicilios, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.2.1.AsignacionDomicilios] @idCartera, @idEjecutivo, @Fecha ");
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaDomiciliosAsignacion"))
                {
                    Mensaje("Falló al insertar Domicilios Asignacion.", Color.Red, lblMensajes);
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; cmbProducto.SelectedIndex = -1; });
                    return;
                }

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvAsignacion.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                    cmbProducto.SelectedIndex = -1;
                    Mensaje("Listo.", Color.DimGray, lblMensajes);
                });
            }
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Mensaje("Transfiriendo... " + e.RowsCopied.ToString("N0") + "/" + iNumberRecords.ToString("N0") + " registros. " + (e.RowsCopied / (float)iNumberRecords * 100).ToString("N2") + " %", Color.DimGray, lblRegistros);
        }
        #endregion

        #region Eventos
        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiaCartera();
            if (cmbCarteras.Text != "")
            {
                txtRuta.Visible = btnArchivo.Visible = true;
                Producto = cmbProducto.Text;
                dgvAsignacion.DataSource = null;
                dgvAsignacion.Visible = btnCargar.Visible = false;
                txtRuta.Text = "";
                LimpiaMensajes();
                Mensaje("Seleccione un archivo con el layout de Asignacion " + Producto + ".", Color.SlateGray, lblMensajes);
            }
            else
                Mensaje("Seleccione una cartera.", Color.SlateGray, lblMensajes);
        }        

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvAsignacion.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            idProducto = (cmbProducto.Text == "" ? 0 : Convert.ToInt32(cmbProducto.SelectedValue));
            sTipo = cmbTipo.Text;
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
            DataBaseConn.StartThread(CargaAsignacion);
        }

        private void frmAsignacion_FormClosed(object sender, FormClosedEventArgs e)
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
