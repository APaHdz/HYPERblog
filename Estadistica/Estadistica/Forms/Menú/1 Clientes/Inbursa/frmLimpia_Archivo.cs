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
    public partial class frmLimpia_Archivo : Form
    {
        public frmLimpia_Archivo()
        {
            InitializeComponent();
            //dtpMesArchivo.Format = DateTimePickerFormat.Custom;
            //dtpMesArchivo.CustomFormat = "MM/yyyy";
            //PreparaVentana();
        }

        #region Variables
        int iNumberRecords;
        DataTable tblArchivo = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        int LO = 0;
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

                if (rBLayout1.Checked == true)
                {
                    string[] Layout;
                    Layout = new string[] { "num_cred", "GESTOR", "Col", "Delega", "Ciudad", "Edo", "CP", "Tel" };


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
                if (rBLayout2.Checked == true)
                {
                    string[] Layout;
                    Layout = new string[] { "num_cred", "GESTOR", "TipoRef", "Contacto", "Tels", "Mail", "contacto1", "dir_c1", "tel_c1", "contacto2", "dir_c2", "tel_c2", "telc", "tell" };

                    foreach (string sColumna in Layout)
                        if (!tblArchivo.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                            lector.Close();
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;

                        }
                    LO = 2;
                }

                if (rBLayout3.Checked == true)
                {
                    string[] Layout;
                    Layout = new string[] { "num_cred", "gestor", "RefeTelefono" };

                    foreach (string sColumna in Layout)
                        if (!tblArchivo.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                            lector.Close();
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;
                        }
                    LO = 3;
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
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Archivo" + LO.ToString() + "' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Archivo" + LO.ToString());

            if (!DataBaseConn.Execute("DropTemporalArchivo"))
            {
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Archivo"+ LO.ToString() + " ( ";
            for (int i = 0; i < tblArchivo.Columns.Count; i++)
            {
                //if (tblSMS.Columns[i].ColumnName == "Fecha Asig" || tblSMS.Columns[i].ColumnName == "Fecha Prom" || tblSMS.Columns[i].ColumnName == "Fecha Sig Gest" || tblSMS.Columns[i].ColumnName == "Fecha Ult Diag" || tblSMS.Columns[i].ColumnName == "Fecha Ult Gest" || tblSMS.Columns[i].ColumnName == "Fecha Ult Coment" || tblSMS.Columns[i].ColumnName == "Fecha Venc Asig" || tblSMS.Columns[i].ColumnName == "Fecha Visita" || tblSMS.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblSMS.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblSMS.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblSMS.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblSMS.Columns[i].ColumnName == "Fec_Bancomalo" || tblSMS.Columns[i].ColumnName == "F_Alta_Credito" || tblSMS.Columns[i].ColumnName == "Fecha ultimo Pago" || tblSMS.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblSMS.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (tblArchivo.Columns[i].ColumnName == "fecha")
                    sCreateTable += ("\r\n [" + tblArchivo.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblArchivo.Columns[i] + "] VARCHAR(MAX) NULL, ");
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.Archivo" + LO.ToString();
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



                Mensaje("Limpiando Archivo, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[INBURSA].[1.0LimpiaArchivo] @LO");
                DataBaseConn.CommandParameters.AddWithValue("@LO", LO);

                DataTable tblResultado = new DataTable();
                if (!DataBaseConn.Fill(tblResultado,"Resultado_Archivo"))
                {
                    
                    Mensaje("Falló la limpieza del archivo.", Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }
                
                Mensaje("Se limpió el archivo satisfactoriamente.", Color.DimGray, lblMensajes);
                 string sRutaExcel = "";

                if (tblResultado.Rows.Count == 0)
                {
                    TerminaBúsqueda("Consulta terminada sin registros.", false);
                    return;
                }
                else
                    this.Invoke((Action)delegate()
                    {
                        lblMensajes.Text = "Consulta terminada. Guardando libro de Excel...";

                        if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                            TerminaBúsqueda("Guardado cancelado por usuario.", false);
                        else
                        {
                            sRutaExcel = sfdExcel.FileName;
                        }
                    });

                if (sRutaExcel == "")
                {
                    tblResultado.Dispose();
                    return;
                }

                string sResultado = ExcelXML.ExportToExcelSAX(ref tblResultado, sRutaExcel);
                TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado. Seleccione otro archivo." : sResultado, sResultado != "");
                lector.Close();

                //Invoke((Action)delegate
                //{
                //    btnArchivo.Visible = true;
                //    picWaitBulk.Visible = false;
                //    dgvArchivo.DataSource = null;
                //    lblInstrucciones.Text = "";
                //    lblRegistros.Text = "";
                //    txtRuta.Text = "";
                //});
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