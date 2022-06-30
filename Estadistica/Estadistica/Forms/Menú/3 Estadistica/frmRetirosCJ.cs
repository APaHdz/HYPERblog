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
    public partial class frmRetirosCJ : Form
    {
        public frmRetirosCJ()
        {
            InitializeComponent();
            dtpMesRetiro.Format = DateTimePickerFormat.Custom;
            dtpMesRetiro.CustomFormat = "MM/yyyy";
            dtpMesRetiro.Value = DateTime.Today.AddDays(-0);
            dtpMesRetiro.MaxDate = dtpMesRetiro.Value;
            dtpMesRetiro.MinDate = DateTime.Today.AddDays(-365);
            PreparaVentana();
        }

        #region Variables
        int iNumberRecords;
        DataTable tblRetiros = new DataTable();
        dbFileReader lector;
        string Producto = "";
        string sCuentaError = "";
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
            dgvRetiros.DefaultCellStyle.Font = controlEvents.RowFont;
            Mensaje("Seleccione un producto.", Color.SlateGray, lblMensajes);
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
            tblRetiros = new DataTable();

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

                tblRetiros.Load(lector.Reader);

                //tblPersonal.Columns["Entrada"].DataType = typeof(TimeSpan);

                if (tblRetiros.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }
                                
                
                Layout = new string[] { "Cuenta2", "Fecha_Retiro" };
                
                                foreach (string sColumna in Layout)
                    if (!tblRetiros.Columns.Contains(sColumna)){
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
            sErrorFechas = validaFechas(tblRetiros);
            if (sErrorFechas != "")
            {
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblRetiros.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvRetiros.DataSource = tblRetiros;
                //dgvAsignacion.Columns["Entrada"].DefaultCellStyle.Format = "t";
                //dgvAsignacion.Columns["Salida"].DefaultCellStyle.Format = "t";
                dgvRetiros.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnRetirar.Visible = btnArchivo.Visible = dgvRetiros.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblRetiros)
        {
            DateTime dtValidador = new DateTime();
            int i = 0;
            string Dato = "";
            try
            {
                foreach (DataRow drRetiros in tblRetiros.Rows)
                {
                    if (drRetiros.Table.Columns.Contains("Retiro"))
                    {
                        if (!DateTime.TryParse(drRetiros["Retiro"].ToString(), out dtValidador))
                        {
                            Dato = dtValidador.ToString();
                            //if (Dato != "01/01/0001 12:00:00 a. m.")
                            //{
                                sCuentaError = drRetiros["Cuenta"].ToString();
                                return "Falló, la fecha del registro: " + sCuentaError.ToString() + " contiene un tipo de dato no válido.";
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "Falló " + ex.Message;
            }
            return "";
        }

        public void Retiros(object oDatos)
        {
            DataTable RetirosError = new DataTable(), CreaTemporalRet = new DataTable(), tblRetiros = new DataTable(), tblActualizados = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblRetiros.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }
           
            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Reti_" + Ejecutivo.Datos["idEjecutivo"] + "'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Reti_" + Ejecutivo.Datos["idEjecutivo"]);

            if (!DataBaseConn.Execute("DropTemporalReti")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Reti_" + Ejecutivo.Datos["idEjecutivo"] + " ( ";
            for (int i = 0; i < tblRetiros.Columns.Count; i++){
                if (tblRetiros.Columns[i].ColumnName.Contains("Retiro"))
                    sCreateTable += ("\r\n [" + tblRetiros.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblRetiros.Columns[i] + "] VARCHAR(30) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);

            if (!DataBaseConn.Fill(CreaTemporalRet, "CreaTemporalReti")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Reti_" + Ejecutivo.Datos["idEjecutivo"];
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblRetiros);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpMesRetiro.Value.ToString("yyyy-MM-dd");

                Mensaje("Marcando retiros, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[1.3.Retiros] @idCartera, @idProducto, @idEjecutivo, @Fecha ");                                
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Fill(tblActualizados, "MarcaRetiros")){
                    Mensaje("Falló al marcar retiros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; cmbProducto.SelectedIndex = -1; });
                    return;
                }

                string RA = tblActualizados.Rows[0][0].ToString();

                Mensaje("Se marcaron como retiros " + RA + " registros en el sistema.", Color.DimGray, lblMensajes);                

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvRetiros.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                    cmbProducto.SelectedIndex = -1;
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
                dgvRetiros.DataSource = null;
                dgvRetiros.Visible = btnRetirar.Visible = false;
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
            dgvRetiros.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            idProducto = (cmbProducto.Text == "" ? 0 : Convert.ToInt32(cmbProducto.SelectedValue));
            txtRuta.Text = ofdArchivo.FileName;
            Mensaje("Cargando vista previa del archivo de Excel, por favor espere...", Color.DimGray, lblMensajes);
            btnRetirar.Visible = btnArchivo.Visible = false;
            picWait.Visible = true;
            DataBaseConn.StartThread(Archivo, ofdArchivo.FileName);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            Mensaje("", Color.DimGray, lblInstrucciones);
            btnRetirar.Visible = false;
            picWaitBulk.Visible = true;
            btnArchivo.Visible = false;
            DataBaseConn.StartThread(Retiros);
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
