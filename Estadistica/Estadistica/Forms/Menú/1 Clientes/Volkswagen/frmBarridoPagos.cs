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
    public partial class frmBarridoPagos : Form
    {
        public frmBarridoPagos()
        {
            InitializeComponent();
            //dtpMesAsistencia.Format = DateTimePickerFormat.Custom;
            //dtpMesAsistencia.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables
        int iNumberRecords;
        DataTable tblTablaTemporal = new DataTable();
        dbFileReader lector;
        string sCuentaError = "";
        string Tablatemp = "";

        string Carpeta = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvTablaTemporal.DefaultCellStyle.Font = controlEvents.RowFont;
            AgregaEventosTxtKeyPress();
        }
        public void AgregaEventosTxtKeyPress()
        {
            txtTablaTemporal.KeyPress += new KeyPressEventHandler(controlEvents.txtOnlyNumbersLetters_KeyPress);
        }

        protected void LimpiaMensajes()
        {
            Mensaje("", Color.DimGray, lblRegistros);
            Mensaje("", Color.DimGray, lblInstrucciones);
            Mensaje("", Color.DimGray, lblMensajes);
        }

        public void Proceso()
        {
            DirectoryInfo dir = new DirectoryInfo(Carpeta);
            FileInfo[] files = dir.GetFiles("*.xlsx");

            //DataTable tblErrorArchivo = new DataTable();
            //tblErrorArchivo.Columns.Add("idSolicitud");
            //tblErrorArchivo.Columns.Add("Archivo");
            //tblErrorArchivo.Columns.Add("Error");
            string RutaArchivo = "";
            string Archivo = "";
            string NombreArchivo = "";
            for (int iFile = 0; iFile < files.Length; iFile++)
            {
                Archivo = files[iFile].Name.ToLower();
                NombreArchivo = System.IO.Path.GetFileNameWithoutExtension(files[iFile].ToString());
                RutaArchivo = Carpeta + "\\" + Archivo;

                tblTablaTemporal = new DataTable();

                lector = new dbFileReader(RutaArchivo);

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
                    lector.Read(0);
                    tblTablaTemporal.Load(lector.Reader);

                    if (tblTablaTemporal.Rows.Count == 0)
                    {
                        Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                        lector.Close();
                        //Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                        //return;
                    }
                    
                    for(int i = 0; i<tblTablaTemporal.Columns.Count;i++){
                        string NombreColumna = tblTablaTemporal.Rows[8][i].ToString();

                        if(NombreColumna!="")
                            tblTablaTemporal.Columns[i].ColumnName = NombreColumna.ToString().Trim().Replace(" ", "").Replace("º", "").Replace(".","");
                    }

                    for (int i = 0; i < tblTablaTemporal.Columns.Count; i++){
                        string NombreColumna = tblTablaTemporal.Rows[8][i].ToString();

                        if (NombreColumna.Trim() == "")
                            tblTablaTemporal.Columns.RemoveAt(i);}

                    for (int i = 0; i < tblTablaTemporal.Columns.Count; i++){
                        string NombreColumna = tblTablaTemporal.Rows[8][i].ToString();

                        if (NombreColumna.Trim() == "")
                            tblTablaTemporal.Columns.RemoveAt(i);}
    

                    string[] Layout;
                    Layout = new string[] { "St","Asignación","Referencia","Cuenta","BP","Ndoc","Cla","Fechabase","Fechadoc","Ve","ImporteenMD","Mon","Doccomp","Compens","ML","Texto","Fecontab"};

                    foreach (string sColumna in Layout)
                        if (!tblTablaTemporal.Columns.Contains(sColumna))
                        {
                            Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                            lector.Close();
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                            return;
                        }

                    Invoke((Action)delegate() { txtRuta.Text = RutaArchivo; });                    
                }
                catch (Exception ex)
                {
                    Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                    lector.Close();
                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

                Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblTablaTemporal.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);                

                if(iFile==0)
                {
                    Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
                    DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = '" + Tablatemp + "'  AND schema_id=6 ) " +
                       " DROP TABLE dbEstadistica.Temp." + Tablatemp);

                    if (!DataBaseConn.Execute("DropTablaTemporal"))
                    {
                        Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }

                    string sCreateTable = "CREATE TABLE dbEstadistica.Temp." + Tablatemp + " ( ";
                    for (int i = 0; i < tblTablaTemporal.Columns.Count; i++)
                    {
                        if (tblTablaTemporal.Columns[i].ColumnName.Contains("Fechaxxx"))
                            sCreateTable += ("\r\n [" + tblTablaTemporal.Columns[i] + "] DATE NULL, ");
                        else
                            sCreateTable += ("\r\n [" + tblTablaTemporal.Columns[i] + "] VARCHAR(100) NULL, ");
                    }

                    sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

                    DataBaseConn.SetCommand(sCreateTable);
                    if (!DataBaseConn.Fill(tblTablaTemporal, "CreaTablaTemporal"))
                    {
                        Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }

                Mensaje("Insertando datos en la tabla temporal, por favor espere...", Color.DimGray, lblMensajes);
                using (SqlBulkCopy blkData = DataBaseConn.bulkCopy)
                {
                    {
                        try
                        {
                            DataBaseConn.ConnectSQL(true);
                            blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                            blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                            blkData.NotifyAfter = 317;
                            blkData.DestinationTableName = "dbEstadistica.Temp." + Tablatemp ;
                            blkData.BulkCopyTimeout = 30 * 60;
                            blkData.WriteToServer(tblTablaTemporal);
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
                }
            }            

            Mensaje("Los archivos se subierón a la tabla.", Color.DimGray, lblMensajes);

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Volkswagen.[1.4.Barrido_Pagos] ");
            //DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);            

            DataSet tblBarridoPagos = new DataSet();
            if (!DataBaseConn.Fill(tblBarridoPagos, "BarridoPagos"))
            {
                TerminaBúsqueda("Falló la generación del reporte.", true);                
                return;
            }

            string sRutaExcel = "";

            if (tblBarridoPagos.Tables.Count == 0 || tblBarridoPagos.Tables[0].Rows.Count == 0)
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
                        sRutaExcel = sfdExcel.FileName;
                });

            if (sRutaExcel == "")
            {
                tblBarridoPagos.Dispose();
                return;
            }

            string sResultadoE = ExcelXML.ExportToExcelSAX(ref tblBarridoPagos, sRutaExcel);
            TerminaBúsqueda(sResultadoE == "" ? "Libro de Excel guardado." : sResultadoE, sResultadoE != "");

            Invoke((Action)delegate()
            {
                btnArchivo.Visible = true;
                picWaitBulk.Visible = false;
                dgvTablaTemporal.DataSource = null;
                lblInstrucciones.Text = "";
                lblRegistros.Text = "";
                txtRuta.Text = "";
                btnCargar.Visible = false;
                picWait.Visible = false;
            });
        }
                    

        public void Excel()
        {
            Mensaje("Consulta en proceso, favor espere unos segundos...", Color.DimGray, lblMensajes);
            DataTable tblDireccion = new DataTable();
            if (!DataBaseConn.Fill(tblDireccion, "ConsultaDirecciones"))
            {
                Invoke((Action)delegate()
                {
                    //btnConsulta.Visible = true;
                    //picWaitBulk.Visible = false;
                    //btnRutas.Visible = true;
                    //picWaitRutas.Visible = false;
                    //gbOpciones.Enabled = true;
                });
                Mensaje("Falló al realizar la consulta.", Color.Red, lblMensajes);
                return;
            }

            string sRutaExcel = "";

            this.Invoke((Action)delegate()
            {
                lblMensajes.Text = "Guardando libro de Excel...";

                if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                    TerminaBúsqueda("Guardado cancelado por usuario.", false);
                else
                    sRutaExcel = sfdExcel.FileName;
            });

            if (sRutaExcel == "")
            {
                tblDireccion.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblDireccion, sRutaExcel);
            TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != ""); 
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
            dgvTablaTemporal.DataSource = null;
            lblInstrucciones.Text = "";
            lblRegistros.Text = "";
            txtRuta.Text = "";
            btnCargar.Visible = false;
            picWait.Visible = false;

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvTablaTemporal.DataSource = null;

            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
            {
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            } 
            
            txtRuta.Text = "";
            Mensaje("Cargando archivo de Excel, por favor espere...", Color.DimGray, lblMensajes);
            btnCargar.Visible = btnArchivo.Visible = false;
            picWait.Visible = true;
            Tablatemp = txtTablaTemporal.Text;

            string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);

            Carpeta = folderBrowserDialog1.SelectedPath;

            DataBaseConn.StartThread(Proceso);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (txtTablaTemporal.Text.Length <= 3)
            {
                Mensaje("El nombre de la tabla temporal debe ser mayor a 3 caracteres.", Color.Red, lblMensajes);
                return;
            }
            else
            {
                Tablatemp = txtTablaTemporal.Text;
            }

            Mensaje("", Color.DimGray, lblInstrucciones);
            btnCargar.Visible = false;
            picWaitBulk.Visible = true;
            btnArchivo.Visible = false;
            //DataBaseConn.StartThread(CargaTablaTemporal);
        }

        private void frmRemesa_FormClosed(object sender, FormClosedEventArgs e)
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
