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
    public partial class frmPlantillaPersonal : Form
    {
        public frmPlantillaPersonal()
        {
            InitializeComponent();
            dtpCargaArchivos.Format = DateTimePickerFormat.Custom;
            dtpCargaArchivos.CustomFormat = "MM/yyyy";
            dtpCargaArchivos.Value = DateTime.Today.AddDays(-0);
            dtpCargaArchivos.MaxDate = dtpCargaArchivos.Value;
            dtpCargaArchivos.MinDate = DateTime.Today.AddDays(-365);
            PreparaVentana();
        }

        #region Variables
        int iNumberRecords;
        DataTable tblArchivo = new DataTable();
        dbFileReader lector;                
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

                string[] Layout;
                Layout = new string[] { "NO# EMPLEADO", "LOGIN", "NOMBRE DEL PERSONAL", "SUPERVISOR" };

                foreach (string sColumna in Layout)
                    if (!tblArchivo.Columns.Contains(sColumna))
                    {
                        Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                        lector.Close();
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
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'PlantillaPersonal" +  "' AND schema_id=21 ) " +
                " DROP TABLE dbEstadistica.Temp.PlantillaPersonal");

            if (!DataBaseConn.Execute("DropTemporalArchivo"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.PlantillaPersonal" + " ( ";
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.PlantillaPersonal";
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

                string Mes = dtpCargaArchivos.Value.ToString("yyyy-MM-dd");


                Mensaje("Cargando base, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Banorte].[1.2.PlantillaPersonal] @Mes, @idEjecutivo");                                
                DataBaseConn.CommandParameters.AddWithValue("@Mes", Mes);
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


    }
}