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
    public partial class frmCargaConjurnetNeg : Form
    {
        public frmCargaConjurnetNeg()
        {
            InitializeComponent();
            //dtpMesNegociacionCN.Format = DateTimePickerFormat.Custom;
            //dtpMesNegociacionCN.CustomFormat = "MM/yyyy";
            dtpDiaNegociacionesinicial.Value = DateTime.Today.AddDays(-1);
            dtpDiaNegociacionesinicial.MaxDate = dtpDiaNegociacionesinicial.Value;
            dtpDiaNegociacionesinicial.MinDate = DateTime.Today.AddDays(-15);
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblNegociacionCN = new DataTable();
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
            dgvNegociaciones.DefaultCellStyle.Font = controlEvents.RowFont;

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
            tblNegociacionCN = new DataTable();

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

                tblNegociacionCN.Load(lector.Reader);                

                if (tblNegociacionCN.Rows.Count == 0){
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }
              
                //string[] Layout;
                if (Cartera == "American Express")
                {
                    Layout = new string[] { "Fecha Negociacion", "Expediente", "Herramienta", "Saldo Negociacion", "Num Pagos", "Descuento", "Periodo Pagos", "Minimomas Atrasado", "fechacorte", "ultimo Interes" };
                }
                else if (Cartera == "BBVA Bancomer")
                {
                    Layout = new string[] { "id Insert", "Fecha Negociacion", "Expediente", "Herramienta", "Saldo Negociacion", "Num Pagos", "Descuento", "FechaPlazo1", "MontoPago1", "FechaPlazo2", "MontoPago2", "FechaPlazo3", "MontoPago3", "FechaPlazo4", "MontoPago4", "FechaPlazo5", "MontoPago5", "FechaPlazo6", "MontoPago6", "FechaPlazo7", "MontoPago7", "FechaPlazo8", "MontoPago8", "FechaPlazo9", "MontoPago9", "FechaPlazo10", "MontoPago10", "FechaPlazo11", "MontoPago11", "FechaPlazo12", "MontoPago12", "FechaPlazo13", "MontoPago13", "FechaPlazo14", "MontoPago14", "FechaPlazo15", "MontoPago15", "FechaPlazo16", "MontoPago16", "FechaPlazo17", "MontoPago17", "FechaPlazo18", "MontoPago18", "FechaPlazo19", "MontoPago19", "FechaPlazo20", "MontoPago20", "FechaPlazo21", "MontoPago21", "FechaPlazo22", "MontoPago22", "FechaPlazo23", "MontoPago23", "FechaPlazo24", "MontoPago24", "FechaPlazo25", "MontoPago25", "FechaPlazo26", "MontoPago26", "FechaPlazo27", "MontoPago27", "FechaPlazo28", "MontoPago28", "FechaPlazo29", "MontoPago29", "FechaPlazo30", "MontoPago30", "FechaPlazo31", "MontoPago31", "FechaPlazo32", "MontoPago32", "FechaPlazo33", "MontoPago33", "FechaPlazo34", "MontoPago34", "FechaPlazo35", "MontoPago35", "FechaPlazo36", "MontoPago36", "FechaPlazo37", "MontoPago37", "FechaPlazo38", "MontoPago38", "FechaPlazo39", "MontoPago39", "FechaPlazo40", "MontoPago40", "FechaPlazo41", "MontoPago41", "FechaPlazo42", "MontoPago42", "FechaPlazo43", "MontoPago43", "FechaPlazo44", "MontoPago44", "FechaPlazo45", "MontoPago45", "FechaPlazo46", "MontoPago46", "FechaPlazo47", "MontoPago47", "FechaPlazo48", "MontoPago48", "FechaPlazo49", "MontoPago49", "FechaPlazo50", "MontoPago50", "FechaPlazo51", "MontoPago51", "FechaPlazo52", "MontoPago52", "FechaPlazo53", "MontoPago53", "FechaPlazo54", "MontoPago54", "FechaPlazo55", "MontoPago55", "FechaPlazo56", "MontoPago56", "FechaPlazo57", "MontoPago57", "FechaPlazo58", "MontoPago58", "FechaPlazo59", "MontoPago59", "FechaPlazo60", "MontoPago60" };
                }
                
                else if (Cartera == "Banamex")
                {
                    Layout = new string[] { "id Insert", "id Cuenta", "Expediente", "Fecha Negociacion", "Herramienta", "Saldo Negociacion", "Num Pagos", "Descuento", "Producto", "Dias WO", "Meses_Vencidos", "Correo", "Segmento_Cobranza_Nivel_Morosidad_Cuenta", "Saldo_Actual", "Compras", "Tasa Onus Offus Settlement", "Tasa Onus Offus Payment Plan", "FechaPago1", "MontoPago1", "FechaPago2", "MontoPago2", "FechaPago3", "MontoPago3", "FechaPago4", "MontoPago4", "FechaPago5", "MontoPago5", "FechaPago6", "MontoPago6", "FechaPago7", "MontoPago7", "FechaPago8", "MontoPago8", "FechaPago9", "MontoPago9", "FechaPago10", "MontoPago10", "FechaPago11", "MontoPago11", "FechaPago12", "MontoPago12", "FechaPago13", "MontoPago13", "FechaPago14", "MontoPago14", "FechaPago15", "MontoPago15", "FechaPago16", "MontoPago16", "FechaPago17", "MontoPago17", "FechaPago18", "MontoPago18", "FechaPago19", "MontoPago19", "FechaPago20", "MontoPago20", "FechaPago21", "MontoPago21", "FechaPago22", "MontoPago22", "FechaPago23", "MontoPago23", "FechaPago24", "MontoPago24", "FechaPago25", "MontoPago25", "FechaPago26", "MontoPago26", "FechaPago27", "MontoPago27", "FechaPago28", "MontoPago28", "FechaPago29", "MontoPago29", "FechaPago30", "MontoPago30", "FechaPago31", "MontoPago31", "FechaPago32", "MontoPago32", "FechaPago33", "MontoPago33", "FechaPago34", "MontoPago34", "FechaPago35", "MontoPago35", "FechaPago36", "MontoPago36", "FechaPago37", "MontoPago37", "FechaPago38", "MontoPago38", "FechaPago39", "MontoPago39", "FechaPago40", "MontoPago40", "FechaPago41", "MontoPago41", "FechaPago42", "MontoPago42", "FechaPago43", "MontoPago43", "FechaPago44", "MontoPago44", "FechaPago45", "MontoPago45", "FechaPago46", "MontoPago46", "FechaPago47", "MontoPago47", "FechaPago48", "MontoPago48", "FechaPago49", "MontoPago49", "FechaPago50", "MontoPago50", "FechaPago51", "MontoPago51", "FechaPago52", "MontoPago52", "FechaPago53", "MontoPago53", "FechaPago54", "MontoPago54", "FechaPago55", "MontoPago55", "FechaPago56", "MontoPago56", "FechaPago57", "MontoPago57", "FechaPago58", "MontoPago58", "FechaPago59", "MontoPago59", "FechaPago60", "MontoPago60" };
                }

                else if (Cartera == "HSBC")
                {
                    Layout = new string[] { "id Insert", "id Cuenta", "Expediente", "Fecha Negociacion", "Herramienta", "Saldo Negociacion", "Num Pagos", "Descuento", "Producto", "Estado Nego", "FechaPago1", "MontoPago1", "FechaPago2", "MontoPago2", "FechaPago3", "MontoPago3", "FechaPago4", "MontoPago4", "FechaPago5", "MontoPago5", "FechaPago6", "MontoPago6", "FechaPago7", "MontoPago7", "FechaPago8", "MontoPago8", "FechaPago9", "MontoPago9", "FechaPago10", "MontoPago10", "FechaPago11", "MontoPago11", "FechaPago12", "MontoPago12", "FechaPago13", "MontoPago13", "FechaPago14", "MontoPago14", "FechaPago15", "MontoPago15", "FechaPago16", "MontoPago16", "FechaPago17", "MontoPago17", "FechaPago18", "MontoPago18", "FechaPago19", "MontoPago19", "FechaPago20", "MontoPago20", "FechaPago21", "MontoPago21", "FechaPago22", "MontoPago22", "FechaPago23", "MontoPago23", "FechaPago24", "MontoPago24", "FechaPago25", "MontoPago25", "FechaPago26", "MontoPago26", "FechaPago27", "MontoPago27", "FechaPago28", "MontoPago28", "FechaPago29", "MontoPago29", "FechaPago30", "MontoPago30", "FechaPago31", "MontoPago31", "FechaPago32", "MontoPago32", "FechaPago33", "MontoPago33", "FechaPago34", "MontoPago34", "FechaPago35", "MontoPago35", "FechaPago36", "MontoPago36", "FechaPago37", "MontoPago37", "FechaPago38", "MontoPago38", "FechaPago39", "MontoPago39", "FechaPago40", "MontoPago40", "FechaPago41", "MontoPago41", "FechaPago42", "MontoPago42", "FechaPago43", "MontoPago43", "FechaPago44", "MontoPago44", "FechaPago45", "MontoPago45", "FechaPago46", "MontoPago46", "FechaPago47", "MontoPago47", "FechaPago48", "MontoPago48", "FechaPago49", "MontoPago49", "FechaPago50", "MontoPago50", "FechaPago51", "MontoPago51", "FechaPago52", "MontoPago52", "FechaPago53", "MontoPago53", "FechaPago54", "MontoPago54", "FechaPago55", "MontoPago55", "FechaPago56", "MontoPago56", "FechaPago57", "MontoPago57", "FechaPago58", "MontoPago58", "FechaPago59", "MontoPago59", "FechaPago60", "MontoPago60" };
                }
                else if (Cartera == "Santander")
                    Layout = new string[] { "Fecha Negociacion", "Expediente", "Herramienta", "Saldo Negociacion", "Descuento", "Num Pagos","FechaPago1", "MontoPago1", "FechaPago2", "MontoPago2", "FechaPago3", "MontoPago3", "FechaPago4", "MontoPago4", "FechaPago5", "MontoPago5", "FechaPago6", "MontoPago6", "FechaPago7", "MontoPago7", "FechaPago8", "MontoPago8", "FechaPago9", "MontoPago9", "FechaPago10", "MontoPago10", "FechaPago11", "MontoPago11", "FechaPago12", "MontoPago12", "FechaPago13", "MontoPago13", "FechaPago14", "MontoPago14", "FechaPago15", "MontoPago15", "FechaPago16", "MontoPago16", "FechaPago17", "MontoPago17", "FechaPago18", "MontoPago18", "FechaPago19", "MontoPago19", "FechaPago20", "MontoPago20", "FechaPago21", "MontoPago21", "FechaPago22", "MontoPago22", "FechaPago23", "MontoPago23", "FechaPago24", "MontoPago24", "FechaPago25", "MontoPago25", "FechaPago26", "MontoPago26", "FechaPago27", "MontoPago27", "FechaPago28", "MontoPago28", "FechaPago29", "MontoPago29", "FechaPago30", "MontoPago30", "FechaPago31", "MontoPago31", "FechaPago32", "MontoPago32", "FechaPago33", "MontoPago33", "FechaPago34", "MontoPago34", "FechaPago35", "MontoPago35", "FechaPago36", "MontoPago36", "FechaPago37", "MontoPago37", "FechaPago38", "MontoPago38", "FechaPago39", "MontoPago39", "FechaPago40", "MontoPago40", "FechaPago41", "MontoPago41", "FechaPago42", "MontoPago42", "FechaPago43", "MontoPago43", "FechaPago44", "MontoPago44", "FechaPago45", "MontoPago45", "FechaPago46", "MontoPago46", "FechaPago47", "MontoPago47", "FechaPago48", "MontoPago48", "FechaPago49", "MontoPago49", "FechaPago50", "MontoPago50", "FechaPago51", "MontoPago51", "FechaPago52", "MontoPago52", "FechaPago53", "MontoPago53", "FechaPago54", "MontoPago54", "FechaPago55", "MontoPago55", "FechaPago56", "MontoPago56", "FechaPago57", "MontoPago57", "FechaPago58", "MontoPago58", "FechaPago59", "MontoPago59", "FechaPago60", "MontoPago60"};

                if (Layout == null)
                {
                    Mensaje("El layout selecionado no coincide con el procedimiento. Favor de Verificarlo", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                foreach (string sColumna in Layout)
                    if (!tblNegociacionCN.Columns.Contains(sColumna)){
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
            
            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblNegociacionCN.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvNegociaciones.DataSource = tblNegociacionCN;                
                dgvNegociaciones.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvNegociaciones.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }
        
        public void CargaNegociacionCN(object oDatos)
        {
            DataTable tblNegociacionCN = new DataTable() ;            
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try{
                lector.Read();
                tblNegociacionCN.Load(lector.Reader);               
            }
            catch (Exception ex){
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }            

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'NegociacionCN' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.NegociacionCN");

            if (!DataBaseConn.Execute("DropTemporalNegociacionCN")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.NegociacionCN ( ";
            for (int i = 0; i < tblNegociacionCN.Columns.Count; i++){
                //if (tblNegociacionCN.Columns[i].ColumnName == "Fecha Asig" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Prom" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Sig Gest" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Ult Diag" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Ult Gest" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Ult Coment" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Venc Asig" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Visita" || tblNegociacionCN.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblNegociacionCN.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblNegociacionCN.Columns[i].ColumnName == "Fec_Bancomalo" || tblNegociacionCN.Columns[i].ColumnName == "F_Alta_Credito" || tblNegociacionCN.Columns[i].ColumnName == "Fecha ultimo Pago" || tblNegociacionCN.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblNegociacionCN.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblNegociacionCN.Columns[i].ColumnName.Contains("fecha") || tblNegociacionCN.Columns[i].ColumnName.Contains("Fecha"))
                    sCreateTable += ("\r\n [" + tblNegociacionCN.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblNegociacionCN.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalNegociacionCN")){
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.NegociacionCN";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblNegociacionCN);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpDiaNegociacionesinicial.Value.ToString("yyyy-MM-dd");
                string Fecha1 = dtpDiaNegociacionesFinal.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando NegociacionCN, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[ConjurNet].[1.0.Negociacion] @idEjecutivo, @idCartera, @idProducto, @Fecha, @Fecha1");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha1", Fecha1);


                if (!DataBaseConn.Execute("InsertaNegociacionCN")){
                    Mensaje("Falló al insertar NegociacionCN.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La NegociacionCN se insertó en el sistema.", Color.DimGray, lblMensajes);

                lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvNegociaciones.DataSource = null;
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
            dgvNegociaciones.DataSource = null;

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
            DataBaseConn.StartThread(CargaNegociacionCN);
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

        private void lblFecha_Click(object sender, EventArgs e)
        {

        }

        

        
    }
}
