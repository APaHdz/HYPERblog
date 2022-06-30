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
    public partial class frmCargaConjurnetAsignacion : Form
    {
        public frmCargaConjurnetAsignacion()
        {
            InitializeComponent();
            dtpDiaAsignacionesinicial.Format = DateTimePickerFormat.Custom;
            dtpDiaAsignacionesinicial.CustomFormat = "MM/yyyy";
            dtpDiaAsignacionesinicial.Value = DateTime.Today.AddDays(-1);
            dtpDiaAsignacionesinicial.MaxDate = dtpDiaAsignacionesinicial.Value;
            dtpDiaAsignacionesinicial.MinDate = DateTime.Today.AddDays(-15);
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblAsignacionCN = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        string[] Layout;
        string Cartera = "";
        string Producto = "";
        int idCartera;
        int idProducto;

        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvAsignaciones.DefaultCellStyle.Font = controlEvents.RowFont;

            //Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;

            //Productos
            cmbProductos.ValueMember = "idProducto";
            cmbProductos.DisplayMember = "Producto";
            cmbProductos.DataSource = Catálogo.Productos;

            cmbProductos.SelectedIndex = -1;
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
            tblAsignacionCN = new DataTable();

            if (!File.Exists(sFilePath)){
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                Invoke((Action)delegate(){
                    picWait.Visible = false; btnArchivo.Visible = true;
                    cmbProductos.Enabled = true;
                    cmbCarteras.Enabled = true;              
                });
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

                tblAsignacionCN.Load(lector.Reader);                

                if (tblAsignacionCN.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }
              
                //string[] Layout;
                string[] Layout;  
                {
                    Layout = new string[] { "NumeroControl", "NumeroDeCuenta", "cuenta", "Etapa", "SaldoVencido", "FechaAsignacion", "TipoDeCredito", "Credito", "Contacto en el ultimo mes", "Ultimo contacto", "Contacto previo al acceso", "Con pago posterior a Cobranza", "Con pago posterior a asignacion a Consorcio ", "Sin pago anterior al acceso", "Con pago posterior a Herremienta", "RegionAsignacion", "Estado", "Edad", "RangoEdad", "Status previo al envio de invitacion", "Status actual", "correo", "rfc" };
                }
                               

                if (Layout == null)
                {
                    Mensaje("El layout selecionado no coincide con el procedimiento. Favor de Verificarlo", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                foreach (string sColumna in Layout)
                    if (!tblAsignacionCN.Columns.Contains(sColumna)){
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
            
            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblAsignacionCN.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvAsignaciones.DataSource = tblAsignacionCN;                
                dgvAsignaciones.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvAsignaciones.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }
        
        public void CargaAsignacionCN(object oDatos)
        {
            DataTable tblAsignacionCN = new DataTable() ;            
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblAsignacionCN.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'AsignacionCN' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.AsignacionCN");

            if (!DataBaseConn.Execute("DropTemporalAsignacionCN")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.AsignacionCN ( ";
            for (int i = 0; i < tblAsignacionCN.Columns.Count; i++){
                //if (tblAsignacionCN.Columns[i].ColumnName == "Fecha Asig" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Prom" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Sig Gest" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Ult Diag" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Ult Gest" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Ult Coment" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Venc Asig" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Visita" || tblAsignacionCN.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblAsignacionCN.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblAsignacionCN.Columns[i].ColumnName == "Fec_Bancomalo" || tblAsignacionCN.Columns[i].ColumnName == "F_Alta_Credito" || tblAsignacionCN.Columns[i].ColumnName == "Fecha ultimo Pago" || tblAsignacionCN.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblAsignacionCN.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblAsignacionCN.Columns[i].ColumnName.Contains("fecha") || tblAsignacionCN.Columns[i].ColumnName.Contains("Fecha"))
                    sCreateTable += ("\r\n [" + tblAsignacionCN.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblAsignacionCN.Columns[i] + "] VARCHAR(250) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalAsignacionCN")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.AsignacionCN";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblAsignacionCN);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpDiaAsignacionesinicial.Value.ToString("yyyy-MM-dd");
                //string Fecha1 = dtpDiaAsignacionesFinal.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando AsignacionCN, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.ConjurNet.[1.0.Asignacion] @idEjecutivo, @idCartera, @Fecha");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
               // DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                //DataBaseConn.CommandParameters.AddWithValue("@Fecha1", Fecha1);


                if (!DataBaseConn.Execute("InsertaAsignacionCN")){
                    Mensaje("Falló al insertar AsignacionCN.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La AsignacionCN se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvAsignaciones.DataSource = null;
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

        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;

            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;
            cmbProductos.SelectedIndex = -1;

            //if (Convert.ToInt32(cmbCarteras.SelectedValue) == 7 )
            //{
            //    cmbProductos.Enabled = true;
            //}
            //else { cmbProductos.Enabled = false; }

            cmbProductos.Enabled = false;
        }
        #endregion

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {  //
           Cartera = cmbCarteras.Text;
           Producto = cmbProductos.Text;
           idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
           // idProducto = (cmbProductos.Text == "" ? 0 : Convert.ToInt32(cmbProductos.SelectedValue));
           // if (idCartera == 7 && idProducto ==0)
           // {
           //     Mensaje("Seleccione un producto.", Color.Crimson, lblMensajes);
           //     return;
           // }

            LimpiaMensajes();
            dgvAsignaciones.DataSource = null;

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
            DataBaseConn.StartThread(CargaAsignacionCN);
        }

        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiaCartera();
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
