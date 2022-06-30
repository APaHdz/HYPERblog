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
    public partial class frmCarga_ArchivosRiesgoBanorte : Form
    {
        public frmCarga_ArchivosRiesgoBanorte()
        {
            InitializeComponent();
            dtpCargaArchivos.Format = DateTimePickerFormat.Custom;
            dtpCargaArchivos.CustomFormat = "MM/yyyy";
            dtpCargaArchivos.Value = DateTime.Today.AddDays(-0);
            dtpCargaArchivos.MaxDate = dtpCargaArchivos.Value;
            dtpCargaArchivos.MinDate = DateTime.Today.AddDays(-730);
            PreparaVentana();
        }

        #region Variables
        int iNumberRecords;
        DataTable tblArchivo = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        int LO = 0;
        string reporte;
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion



        #region Metodos
        public void PreparaVentana()
        {
            dgvArchivo.DefaultCellStyle.Font = controlEvents.RowFont;
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
            tblArchivo = new DataTable();

            if (!File.Exists(sFilePath))
            {
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
            try
            {
                lector.Read(100);

                tblArchivo.Load(lector.Reader);

                if (tblArchivo.Rows.Count == 0)
                {
                    Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                if (reporte == "Pagos")

                //if (rBLayout1.Checked == true)
                {
                    string[] Layout;
                    Layout = new string[] { "CUENTA","Nombre","fecha_posteo","Fecha de pago","COD_TRN",	"DESC_TRN",	"Tipo_Recuperacion", "IMPORTE",	"Fecha Castigo","Pagos Vencidos","Saldo","CVE CYBER","Despacho",	"Etapa","Concepto",	"Tipo",	"Estado","Sucursal","Cartera",	"U0AGNTE",	"Migracion","GC"};


                    foreach (string sColumna in Layout)
                        if (!tblArchivo.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                            lector.Close();
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;
                        }
                    LO = 1;
                }
                if (reporte == "OGR")

                //if (rBLayout2.Checked == true)
                {
                    string[] Layout;
                    Layout = new string[] { "Grupo del producto en Cyber",	"Crédito",	"Número de Cliente","Nombre del Cliente","Estado","Código Postal","Número de Sucursal",	"Fecha de apertura del crédito","Comportamiento de los ultimos 06 meses","Gestiones del Mes","Gestiones Exitosas del Mes","Saldo Vencido", "Saldo Total", "Ultimo pago", 	"Fecha del último pago",	"Fecha en que la cuenta entró a cobranza",	"Días de mora",	"Pagos Vencidos","Monto Promesa de Pago", 	"Fecha Promesa de Pago",	"Estado de Promesa de Pago",	"Promesas de pago rotas",	"Fecha del próximo contacto",	"Prioridad",	"Block Code",	"Block Code 2",	"Fecha HOST",	"Fecha de la ultima accion",	"Tipo de Tarjeta",	 "Pago mínimo total", 	"Fecha Límite de Pago",	"RFC del cliente",	"Segmento 1","Segmento","FechaCastigo","Etapa"};

                    foreach (string sColumna in Layout)
                        if (!tblArchivo.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                            lector.Close();
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;

                        }
                   LO = 2;
                //}
                //if (reporte == "Retenciones")
                ////if (rBLayout3.Checked == true)
                //{
                //    string[] Layout;
                //    Layout = new string[] { "PRÉSTAMO",	"CUENTA","FECHA_PAGO",	"PAGO",	"CONCEPTO",	"PRODUCTO"};

                //    foreach (string sColumna in Layout)
                //        if (!tblArchivo.Columns.Contains(sColumna))
                //        {
                //            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                //            lector.Close();
                //            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //            return;
                //        }
                //    LO = 3;
                //}

                //if (reporte == "Pagos en otras agencias")
                ////if (rBLayout3.Checked == true)
                //{
                //    string[] Layout;
                //    Layout = new string[] {"CLAVE1","CLAVE 2","CUENTA",	"FECHA_PAGO","PAGO","NOMBRE","APELLIDOS","MORA","M_APLC_INI","SDO_APLC_I",	"M_CONT_INI","SDO_CONT_I",	"MORA_ACT",	"SDO_APLC_A",	"SDO_CONT_A",	"SDO_VEN",	"PAG_MIN",	"DESPACHO",	"DIVISIONAL",	"REGIONAL",	"PLAZA"	,"FEC_ASIG","DESP_CERO","DIASMORA",	"CUARTIL",	"CARTERA",	"PRODUCTO",	"SUB_PRO",	"DESCP_PROD",	"DEFINE",	"TRAMITE",	"RANGO",	"FECHA_CS",	"CARTCONT_I",	"GEST",	"NUM_CTE",	"GRUPO","NOTA",	"PRESTAMO",	"Cta"};


                //    foreach (string sColumna in Layout)
                //        if (!tblArchivo.Columns.Contains(sColumna))
                //        {
                //            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                //            lector.Close();
                //            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //            return;
                //        }
                //    LO = 4;
                //}
                //if (reporte == "Avances todas las agencias")

                ////if (rBLayout2.Checked == true)
                //{
                //    string[] Layout;
                //    Layout = new string[] {"MORA",	"PRODUCTO",	"DESPACHO",	"SUBPRODUCTO",	"PLAZA","DIVISIONAL","TRAMITE",	"CTAS_INI",	"SDO_INI",	"CTAS_MC",	"SDO_MC",	"CTAS_SCV",	"SDO_SCV",	"CTAS_BM",	"SDO_BM",	"CTA_SM",	"SDO_SM",	"CTA_MM",	"SDO_MM",	"CTA PGADA",	"CASH",	"META",	"Meta_SCV",	"REGISTRO",	"CARTERA",	"SOLUCIONADO",	"STATUS2",	"RANGO"};

                //    foreach (string sColumna in Layout)
                //        if (!tblArchivo.Columns.Contains(sColumna))
                //        {
                //            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                //            lector.Close();
                //            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //            return;

                //        }
                //    LO = 5;
                //}
                //if (reporte == "OGR")
                //{
                //    string[] Layout;
                //    Layout = new string[] { "DESPACHO",	"PRESTAMO",	"CUENTA","PRESTAMO1","FEC_ASIG","ASIGNACION","RETIROS",	"FECHA DE RE",	"Esta# Domicilios",	"PRIORIDAD","NOMBRE","RFC",	"MORA_APLC","PRODUCTO_OK","SEGMENTO_OK","SALDO_ACT_UCA","SDO_VENCIDO","PRODUCTO","FECHA_CS","FECHA_APERT","CICLO", "NUM_CTE","CP" ,"PLAZA","REGION","DIVISION","TIPO_PERS","RANGO"};

                //    foreach (string sColumna in Layout)
                //        if (!tblArchivo.Columns.Contains(sColumna))
                //        {
                //            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                //            lector.Close();
                //            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                //            return;

                //        }
                //    LO = 6;
                }
            
        }

            catch (Exception ex)
            {
                Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }
            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblArchivo.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate()
            {
                dgvArchivo.DataSource = tblArchivo;
                dgvArchivo.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvArchivo.Visible = true;
            });
        }




        public void LimpiaArchivo(object oDatos)
        {
            DataTable tblArchivo = new DataTable();
            DataTable CreaTemporalPag = new DataTable();

            Mensaje("Leyendo archivo, por favor espere...", Color.Green, lblMensajes);
            try
            {
                lector.Read();
                tblArchivo.Load(lector.Reader);
            }
            catch (Exception ex)
            {
                Mensaje("Falló al leer archivo. " + ex.Message, Color.Red, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Carga_Banorte1" +  "' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Carga_Banorte1");

            if (!DataBaseConn.Execute("DropTemporalArchivo"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Carga_Banorte1" + " ( ";
            for (int i = 0; i < tblArchivo.Columns.Count; i++)
            {
                //if (tblSMS.Columns[i].ColumnName == "Fecha Asig" || tblSMS.Columns[i].ColumnName == "Fecha Prom" || tblSMS.Columns[i].ColumnName == "Fecha Sig Gest" || tblSMS.Columns[i].ColumnName == "Fecha Ult Diag" || tblSMS.Columns[i].ColumnName == "Fecha Ult Gest" || tblSMS.Columns[i].ColumnName == "Fecha Ult Coment" || tblSMS.Columns[i].ColumnName == "Fecha Venc Asig" || tblSMS.Columns[i].ColumnName == "Fecha Visita" || tblSMS.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblSMS.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblSMS.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblSMS.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblSMS.Columns[i].ColumnName == "Fec_Bancomalo" || tblSMS.Columns[i].ColumnName == "F_Alta_Credito" || tblSMS.Columns[i].ColumnName == "Fecha ultimo Pago" || tblSMS.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblSMS.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblArchivo.Columns[i].ColumnName == "FECHA_PAGO" || tblArchivo.Columns[i].ColumnName == "FEC_ASIG")
                    sCreateTable += ("\r\n [" + tblArchivo.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblArchivo.Columns[i] + "] VARCHAR(100) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")";

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalArchivo"))
            {
                Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Insertando datos, por favor espere...", Color.DimGray, lblMensajes);
            using (SqlBulkCopy blkData = DataBaseConn.bulkCopy)
            {
                {
                    try
                    {
                        DataBaseConn.ConnectSQL(true);
                        blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.NotifyAfter = 317;
                        blkData.DestinationTableName = "dbEstadistica.Temp.Carga_Banorte1";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblArchivo);
                    }
                    catch (Exception ex)
                    {
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpCargaArchivos.Value.ToString("yyyy-MM-dd");


                Mensaje("Cargando base, por favor espere...", Color.DimGray, lblMensajes);
                if (LO == 1)
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Riesgos].[Actualización_BasesBanorte] @LO, @Fecha, @idEjecutivo");

                if (LO == 2)
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Riesgos].[Actualización_BasesBanorte1] @LO, @Fecha, @idEjecutivo");

                //if (LO == 3)
                //    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Riesgos].[Actualización_Bases2] @LO, @Fecha, @idEjecutivo");


                //if (LO == 4)
                //    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Riesgos].[Actualización_Bases3] @LO, @Fecha, @idEjecutivo");


                //if (LO == 5)
                //    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Riesgos].[Actualización_Bases4] @LO, @Fecha, @idEjecutivo");

                //if (LO == 6)
                //    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Riesgos].[Actualización_Bases5] @LO, @Fecha, @idEjecutivo");


                DataBaseConn.CommandParameters.AddWithValue("@LO", LO);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);

                DataTable tblResultado = new DataTable();
                if (!DataBaseConn.Execute("Resultado_Archivo"))
                {
                    
                    Mensaje("Falló la carga del archivo.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }
                
                Mensaje("Se actualizó base correctamente.", Color.DimGray, lblMensajes);
              //   string sRutaExcel = "";

              //if (tblResultado.Rows.Count == 0)
              //{
              //    TerminaBúsqueda("Consulta terminada sin registros.", false);
              //    return;
              //}
              //else
              //    this.Invoke((Action)delegate()
              //    {
              //        lblMensajes.Text = "Consulta terminada. Guardando libro de Excel...";
             
              //        if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
              //            TerminaBúsqueda("Guardado cancelado por usuario.", false);
              //        else
              //        {
              //            sRutaExcel = sfdExcel.FileName;
              //          }
              //      });

              //  if (sRutaExcel == "")
              //  {
              //      tblResultado.Dispose();
              //      return;
              //  }

              //  string sResultado = ExcelXML.ExportToExcelSAX(ref tblResultado, sRutaExcel);
              //  TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado. Seleccione otro archivo." : sResultado, sResultado != "");
                
                
                lector.Close();

                Invoke((Action)delegate
                {
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvArchivo.DataSource = null;
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

        public void TerminaBúsqueda(string Mensaje, bool bError)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate() { TerminaBúsqueda(Mensaje, bError); });
                return;
            }

            if (bError)
                lblMensajes.ForeColor = System.Drawing.Color.Crimson;
            else
                lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            lblMensajes.Text = Mensaje;

            btnArchivo.Visible = true;
            picWaitBulk.Visible = false;
            dgvArchivo.DataSource = null;
            lblInstrucciones.Text = "";
            lblRegistros.Text = "";
            txtRuta.Text = "";
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

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            reporte = cmbLayout.Text;
            LimpiaMensajes();
            dgvArchivo.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
            {
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
            DataBaseConn.StartThread(LimpiaArchivo);
        }

        private void frmBlaster_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
            if (lector != null)
                lector.Close();
        }
        #endregion

        private void dtpCargaArchivos_ValueChanged(object sender, EventArgs e)
        {

        }


    }
}