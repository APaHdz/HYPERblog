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
    public partial class frmRetenciones : Form
    {
        public frmRetenciones()
        {
            InitializeComponent();
            dtpMesPagos.Format = DateTimePickerFormat.Custom;
            dtpMesPagos.CustomFormat = "MM/yyyy";
            dtpMesPagos.Value = DateTime.Today.AddDays(-0);
            dtpMesPagos.MaxDate = dtpMesPagos.Value;
            dtpMesPagos.MinDate = DateTime.Today.AddDays(-365);
            PreparaVentana();
        }

        #region Variables
        int iNumberRecords;
        DataTable tblPagos = new DataTable();
        dbFileReader lector;
        string Producto = "";
        string sCuentaError = "";
        string[] Layout;
        int idCartera = 0;

        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {
        

            //Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;

             string servidor =  DataBaseConn.Server;
             string VerCarteras = "";

             if (servidor.Contains("8.53") || servidor.Contains ( "7.65"))
             {
                 VerCarteras = "43,5,50";
             }
             if (servidor.Contains("8.126") || servidor.Contains ("7.30"))
             {
                 VerCarteras = "7,36,23,31,41";

             }

             if (servidor.Contains("16.11"))
             {
                 VerCarteras = "1,4";
             }

             if (servidor.Contains("7.124"))
             {
                 VerCarteras = "8,9,10,12,13,15,17,49,39,28,33";
             }
             if (servidor.Contains("8.97"))
             {
                 return;
             }
             

             


             if (VerCarteras != "*")
             {
                 cmbCarteras.DataSource = Catálogo.Carteras;
                 DataTable tblAreas = (DataTable)cmbCarteras.DataSource;
                 tblAreas.DefaultView.RowFilter = "idCartera in (" + VerCarteras + ")";
             }


        }
        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;

            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;            
           

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
            tblPagos = new DataTable();

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

                tblPagos.Load(lector.Reader);

                //tblPersonal.Columns["Entrada"].DataType = typeof(TimeSpan);

                if (tblPagos.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }
                                
                //switch(idCartera)
                //{
                //    case 23:
                //        Layout = new string[] { "Fecha Pago", "Plan Pagos", "Pago Real", "Segmento" };
                //        break;                    
                //}

                Layout = new string[] { "idCuenta", "Cuenta2", "FechaPago", "MontoPago", "Tipo"};

                //if(tblPagos.Columns.Count!=Layout.Length){
                //    Mensaje("El layout del archivo no corresponde para el tipo de producto " + Producto + ". Verifique el layout.", Color.Red, lblMensajes);
                //    lector.Close();
                //    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; txtRuta.Text = ""; });
                //    return;
                //}


                //for (int i = 0; i < Layout.Length; i++)
                //    tblPagos.Columns[i].ColumnName = Layout[i];

                foreach (string sColumna in Layout)
                    if (!tblPagos.Columns.Contains(sColumna)){
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
            sErrorFechas = validaFechas(tblPagos);
            if (sErrorFechas != ""){
                Mensaje(sErrorFechas, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblPagos.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvPagos.DataSource = tblPagos;
                //dgvPagos.Columns["Entrada"].DefaultCellStyle.Format = "t";
                //dgvPagos.Columns["Salida"].DefaultCellStyle.Format = "t";
                dgvPagos.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvPagos.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

        private string validaFechas(DataTable tblPagos)
        {
            DateTime dtValidador = new DateTime();
            int i = 0;
            string Dato = "";
            try{
                foreach (DataRow drPagos in tblPagos.Rows){
                    if (drPagos.Table.Columns.Contains("FechaPago")){
                        if (!DateTime.TryParse(drPagos["FechaPago"].ToString(), out dtValidador)){
                            Dato = dtValidador.ToString();
                            if (Dato != "01/01/0001 12:00:00 a. m."){
                                sCuentaError = drPagos["Cuenta1"].ToString();
                                return "Falló, la fecha contiene un tipo de dato no válido.";
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

        public void CargaPagos(object oDatos)
        {
            DataTable PagosError = new DataTable(), CreaTemporalPag = new DataTable(), tblPagos = new DataTable();            

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblPagos.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }
           
            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Retenciones' AND schema_id=21 ) " +
                " DROP TABLE dbEstadistica.Temp.Retenciones");

            if (!DataBaseConn.Execute("DropTemporalRetenciones")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Retenciones ( ";
            for (int i = 0; i < tblPagos.Columns.Count; i++){
                if (tblPagos.Columns[i].ColumnName == "FechaPago")
                    sCreateTable += ("\r\n [" + tblPagos.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblPagos.Columns[i] + "] VARCHAR(8000) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idPago INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);

            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalPag")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Retenciones";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblPagos);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpMesPagos.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando pagos, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Riesgos.[Retenciones] @idCartera, @idEjecutivo, @Fecha ");                                
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                if (!DataBaseConn.Execute("InsertaRetenciones")){
                    Mensaje("Falló al insertar retenciones.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("Las retenciones se insertarón en el sistema.", Color.DimGray, lblMensajes);                

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvPagos.DataSource = null;
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
        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiaCartera();
            if (cmbCarteras.Text != "")
            {
                txtRuta.Visible = btnArchivo.Visible = true;
               // Producto = cmbProducto.Text;
                dgvPagos.DataSource = null;
                dgvPagos.Visible = btnCargar.Visible = false;
                txtRuta.Text = "";
                LimpiaMensajes();
                Mensaje("Seleccione un archivo con el layout de Retenciones " + Producto + ".", Color.SlateGray, lblMensajes);
            }
            else
                Mensaje("Seleccione una cartera.", Color.SlateGray, lblMensajes);
        }        

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvPagos.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
          //  idProducto = (cmbProducto.Text == "" ? 0 : Convert.ToInt32(cmbProducto.SelectedValue));
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
            DataBaseConn.StartThread(CargaPagos);
        }

        private void frmPagos_FormClosed(object sender, FormClosedEventArgs e)
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

        private void frmRetenciones_Load(object sender, EventArgs e)
        {

        }

        

        
                                
    }
}
