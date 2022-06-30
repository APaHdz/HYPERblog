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
    public partial class frmAsignacionesEstadistica : Form
    {
        public frmAsignacionesEstadistica()
        {
            InitializeComponent();
            //dtpMesAsignacion.Format = DateTimePickerFormat.Custom;
            //dtpMesAsignacion.CustomFormat = "MM/yyyy";            
            dtpMesAsignacion.Value = DateTime.Today.AddDays(-0);
            dtpMesAsignacion.MaxDate = dtpMesAsignacion.Value;
            dtpMesAsignacion.MinDate = DateTime.Today.AddDays(-3650);
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
        string Tipo = "";
        int idColumnas = 0;
        string Select = "";
        string Tabla = "";
        string Layouts = "";
        string Insert = "";
        string TablaCampos = "";
        DataTable tblConsultas = new DataTable();
        DataTable tblCampos = new DataTable();
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

                //if (idCartera == 7 && idProducto == 3 && tblAsignacion.Columns.Count!=41)
                //{
                //    Mensaje("Este archivo debe tener 41 columnas, tiene " + tblAsignacion.Columns.Count + ".", Color.Red, lblMensajes);
                //    lector.Close();
                //    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //    return;
                //}

                //if(idCartera==7 && idProducto==3)
                //    Layout = new string[] { "AGNREF" ,"TIPO" ,"PAGO_LLAVE" ,"PGO_KEYVIN" ,"PERAPLC" ,"PORQUITA" ,"CUENTA" ,"RFC" ,"MORA_APLC" ,"MRACONT" ,"CART_INI" ,"M_APLC_INI" ,"SDO_APLC_I" ,"CART_ACT" ,"REC_VDO" ,"SDO_APLC" ,"SDO_VEN" ,"PAG_MIN" ,"PAG_FIJO" ,"PRODUCTO" ,"SUB_PRODUC" ,"CICLO" ,"MONTO_APER" ,"FECHA_APER" ,"FECHA_TERM" ,"FECHA_CS" ,"CUARTIL" ,"ANTIG_CTA" ,"F_UL_PAGO" ,"MTO_PAGO" ,"F_UL_COD" ,"ESTADO" ,"DIVISION" ,"BUCKET1" ,"BUCKET2" ,"BUCKET3" ,"BUCKET4" ,"BUCKET5" ,"BUCKET6" ,"BUCKET7" ,"TIPO2"  };
                //else
                //{
                //    Mensaje("No existe código para este proceso.", Color.Red, lblMensajes);
                //    lector.Close();
                //    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //    return;
                //}

                //Layout = new string[] { Layouts  };
                string phrase = tblConsultas.Rows[0]["Layout"].ToString();
                string[] Layout = phrase.Split(',');

                for (int i = 0; i < Layout.Length; i++)
                    tblAsignacion.Columns[i].ColumnName = Layout[i];
                
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

            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblAsignacion.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);

            Invoke((Action)delegate(){
                dgvAsignacion.DataSource = tblAsignacion;                
                dgvAsignacion.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvAsignacion.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }
        
        public void CargaAsignacion(object oDatos)
        {
            //DataTable CreaTemporalPag = new DataTable();
            DataTable AsignacionError = new DataTable(), CreaTemporalPag = new DataTable(), tblAsignacion = new DataTable();

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
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Asig_" + idCartera + "_" + idProducto + "_" + Ejecutivo.Datos["idEjecutivo"] + "'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Asig_" + idCartera + "_" + idProducto + "_" + Ejecutivo.Datos["idEjecutivo"]);

            if (!DataBaseConn.Execute("DropTemporalAsig")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "";
            //if (idCartera == 7 && idProducto == 3)
            //{
                sCreateTable = "CREATE TABLE dbEstadistica.Temp.Asig_" + idCartera + "_" + idProducto + "_" + Ejecutivo.Datos["idEjecutivo"] + " ( ";
                //for (int i = 0; i < tblCampos.Rows.Count; i++)
                //{
                //    sCreateTable += "\r\n [" + tblCampos.Rows[i]["Campo"] + "] "  + tblCampos.Rows[i]["Tipo"].ToString() ;
                //    //if (tblAsignacion.Columns[i].ColumnName.ToString() == "AGNREF" || tblAsignacion.Columns[i].ColumnName.ToString() == "FECHA_APER" || tblAsignacion.Columns[i].ColumnName.ToString() == "FECHA_TERM" || tblAsignacion.Columns[i].ColumnName.ToString() == "F_UL_PAGO" || tblAsignacion.Columns[i].ColumnName.ToString() == "F_UL_COD")
                //    //    sCreateTable += ("\r\n [" + tblAsignacion.Columns[i] + "] DATE NULL, ");
                //    //else if (tblAsignacion.Columns[i].ColumnName.ToString() == "PAGO_LLAVE" || tblAsignacion.Columns[i].ColumnName.ToString() == "PGO_KEYVIN" || tblAsignacion.Columns[i].ColumnName.ToString() == "SDO_APLC_I" || tblAsignacion.Columns[i].ColumnName.ToString() == "SDO_APLC" || tblAsignacion.Columns[i].ColumnName.ToString() == "SDO_VEN" || tblAsignacion.Columns[i].ColumnName.ToString() == "PAG_MIN" || tblAsignacion.Columns[i].ColumnName.ToString() == "PAG_FIJO" || tblAsignacion.Columns[i].ColumnName.ToString() == "MONTO_APER" || tblAsignacion.Columns[i].ColumnName.ToString() == "MTO_PAGO" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET1" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET2" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET3" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET4" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET5" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET6" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET7")
                //    //    sCreateTable += ("\r\n [" + tblAsignacion.Columns[i] + "] MONEY NULL, ");
                //    //else if (tblAsignacion.Columns[i].ColumnName.ToString() == "PERAPLC" || tblAsignacion.Columns[i].ColumnName.ToString() == "PORQUITA" || tblAsignacion.Columns[i].ColumnName.ToString() == "MORA_APLC" || tblAsignacion.Columns[i].ColumnName.ToString() == "MRACONT" || tblAsignacion.Columns[i].ColumnName.ToString() == "M_APLC_INI" || tblAsignacion.Columns[i].ColumnName.ToString() == "REC_VDO" || tblAsignacion.Columns[i].ColumnName.ToString() == "CICLO" || tblAsignacion.Columns[i].ColumnName.ToString() == "ANTIG_CTA" || tblAsignacion.Columns[i].ColumnName.ToString() == "MTO_PAGO" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET1" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET2" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET3" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET4" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET5" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET6" || tblAsignacion.Columns[i].ColumnName.ToString() == "BUCKET7")
                //    //    sCreateTable += ("\r\n [" + tblAsignacion.Columns[i] + "] SMALLINT NULL, ");
                //    //else
                //    //    sCreateTable += ("\r\n [" + tblAsignacion.Columns[i] + "] VARCHAR(MAX) NULL, ");
                //}
            //}

                sCreateTable = sCreateTable + TablaCampos;

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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Asig_" + idCartera + "_" + idProducto + "_" + Ejecutivo.Datos["idEjecutivo"];
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
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Asignaciones.[1.1.Inserta_Asignaciones] @idCartera, @idProducto, @idEjecutivo, @Fecha, @Select, @Tabla, @Insert ");
                DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@Select", Select);
                DataBaseConn.CommandParameters.AddWithValue("@Tabla", Tabla);
                DataBaseConn.CommandParameters.AddWithValue("@Insert", Insert);


                if (!DataBaseConn.Execute("InsertaAsignacion")){
                    Mensaje("Falló al insertar Asignacion.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; cmbProducto.SelectedIndex = -1; });
                    return;
                }

                Mensaje("Los Asignación se insertó en el sistema.", Color.DimGray, lblMensajes);                

                lector.Close();                

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvAsignacion.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                    cmbProducto.SelectedIndex = -1;
                    Mensaje("Listo caaarnaal.", Color.DimGray, lblMensajes);
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
                Mensaje("Seleccione un archivo con el layout de asignación" + Producto + ".", Color.SlateGray, lblMensajes);
            }
            else
                Mensaje("Seleccione una cartera.", Color.SlateGray, lblMensajes);
        }        

        private void btnArchivo_Click(object sender, EventArgs e)
        {
            if (cmbCarteras.Text == "")
            {
                Mensaje("Selecciona una cartera.", Color.Crimson, lblMensajes);
                return;
            }

            if (cmbProducto.Text == "")
            {
                Mensaje("Selecciona un producto.", Color.Crimson, lblMensajes);
                return;
            }

            if (cmbTipo.Text == "")
            {
                Mensaje("Selecciona tipo.", Color.Crimson, lblMensajes);
                return;
            }

            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            idProducto = (cmbProducto.Text == "" ? 0 : Convert.ToInt32(cmbProducto.SelectedValue));
            Tipo = cmbCarteras.Text.ToString();

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; SELECT * FROM dbEstadistica.[Asignaciones].[AsignacionConsultas] WHERE Activo = 1 AND idCartera = " + idCartera + " AND idProducto = '" + idProducto + "' AND Tipo = '" + cmbTipo.Text + "'");

            tblConsultas = new DataTable();

            if (!DataBaseConn.Fill(tblConsultas, "TablaAsignacionConsultas"))
            {
                Mensaje("Error en la consulta a la tabla AsignacionConsultas", Color.Crimson, lblMensajes);
                return;
            }

            if(tblConsultas.Rows.Count<=0)
            {
                Mensaje("No existe un layout dado de alta para la cartera y producto seleccionado", Color.Crimson, lblMensajes);
                return;
            }

            LimpiaMensajes();
            dgvAsignacion.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            
            idColumnas = Convert.ToInt32(tblConsultas.Rows[0]["Columnas"].ToString());
            Select = tblConsultas.Rows[0]["Select"].ToString();
            Tabla = tblConsultas.Rows[0]["Tabla"].ToString();
            Layouts = tblConsultas.Rows[0]["Layout"].ToString();
            Insert = tblConsultas.Rows[0]["Insert"].ToString();
            TablaCampos = tblConsultas.Rows[0]["TablaCampos"].ToString();

            //DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00';  SELECT  COLUMN_NAME Campo ,  \r\n\t " +
            //                                                    "        CASE WHEN DATA_TYPE = 'varchar' THEN 'Varchar(' + REPLACE(RTRIM(LTRIM(CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR))), '-1', 'MAX') + ')' \r\n\t " +
            //                                                    "             ELSE DATA_TYPE \r\n\t " +
            //                                                    "        END + CASE WHEN IS_NULLABLE = 'YES' THEN ' NULL, ' \r\n\t " +
            //                                                    "                   ELSE ' NOT NULL, ' \r\n\t " +
            //                                                    "              END Tipo \r\n\t " +
            //                                                    "FROM    INFORMATION_SCHEMA.Columns \r\n\t " +
            //                                                    "WHERE   TABLE_NAME = '" + TablaCampos + "' \r\n\t AND COLUMN_NAME NOT IN ('MesAsignacion','Fecha_Insert','idEjecutivo')" +
            //                                                    "ORDER BY ORDINAL_POSITION \r\n\t "
            //    );

            //tblCampos = new DataTable();

            //if (!DataBaseConn.Fill(tblCampos, "TablaCampos"))
            //{
            //    Mensaje("Error en la consulta a la tabla de campos.", Color.Crimson, lblMensajes);
            //    return;
            //}

            txtRuta.Text = ofdArchivo.FileName;
            Mensaje("Cargando vista previa del archivo de Excel, por favor espere...", Color.DimGray, lblMensajes);
            btnCargar.Visible = btnArchivo.Visible = false;
            picWait.Visible = true;
            DataBaseConn.StartThread(Archivo, ofdArchivo.FileName);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            if(cmbCarteras.Text=="")
            {
                Mensaje("Selecciona una cartera.", Color.Crimson, lblMensajes);
                return;
            }

            if (cmbProducto.Text == "")
            {
                Mensaje("Selecciona un producto.", Color.Crimson, lblMensajes);
                return;
            }

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
