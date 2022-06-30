using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Estadistica
{
    public partial class frmAsignaciones : Form
    {
        public frmAsignaciones()
        {
            InitializeComponent();            
          }

        #region Variables
        string Carpeta = "";   
        dbFileReader lector;
        string[] Layout;
        DataSet dsTablas = new DataSet();
        DataTable tblAvances = new DataTable();
        int Arc = 0;
        #endregion

        #region Metodos
        public void Archivo()
        {
            DirectoryInfo dir = new DirectoryInfo(Carpeta);
            FileInfo[] files = dir.GetFiles("*.xlsx");

            for (int iFile = 0; iFile < files.Length; iFile++)
            {
                dsTablas = new DataSet();
                tblAvances = new DataTable();
                string ArchivoSinExt = System.IO.Path.GetFileNameWithoutExtension(files[iFile].ToString());
                string ArchivoConExt = System.IO.Path.GetFileName(files[iFile].ToString());
                string RutaArchivo = Path.Combine(Carpeta, ArchivoConExt);
                
                lector = new dbFileReader(RutaArchivo);                
                lector.Open();
                Arc = iFile + 1;
                Mensaje("Archivo: " + Arc.ToString() + " - " + lector.CountRows().ToString() + " registros.", Color.DimGray, lblMensajes);
                try
                {
                    lector.Read();
                    tblAvances.Load(lector.Reader);

                    if (tblAvances.Rows.Count == 0)
                    {
                        Mensaje("El archivo seleccionado no tiene registros.", Color.Red, lblMensajes);
                        lector.Close();
                    }
                    else
                    {
                        bool FileVal = true;
                        Layout = new string[] { "DESPACHO", "PRESTAMO", "CUENTA", "PRESTAMO", "FEC_ASIG", "ASIGNACION", "RETIROS", "FECHA DE RE", "Esta# Domicilios", "PRIORIDAD", "NOMBRE", "RFC", "MORA_APLC", "PRODUCTO_OK", "SEGMENTO_OK", "SALDO_ACT_UCA", "SDO_VENCIDO", "PRODUCTO", "FECHA_CS", "FECHA_APERT", "CICLO", "NUM_CTE", "CP", "PLAZA", "REGION", "DIVISION", "TIPO_PERS", "RANGO", "Mes", "Año" };
                        foreach (string sColumna in Layout)
                            if (!tblAvances.Columns.Contains(sColumna))
                            {
                                FileVal = false;
                                Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                                lector.Close();
                            }

                        if (FileVal == true)
                        {
                            if (Arc == 1)
                            {
                                Mensaje("Verificando tabla temporal, por favor espere...", Color.Green, lblMensajes);
                                DataBaseConn.SetCommand("SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'AsignacionBBVA_OGR' /*AND schema_id=6*/ ");

                                DataTable tblTabla = new DataTable();
                                if (!DataBaseConn.Fill(tblTabla, "VerificaAsignacionBBVA_OGR"))
                                {
                                    Mensaje("Falló consulta de tabla temporal.", Color.Crimson, lblMensajes);
                                    lector.Close();
                                    Invoke((Action)delegate() { btnRuta.Visible = true; picWait.Visible = false; });
                                    return;
                                }

                                if (tblTabla.Rows.Count == 0)
                                {
                                    string sCreateTable = "CREATE TABLE dbEstadistica.Riesgos.AsignacionBBVA_OGR ( ";
                                    for (int i = 0; i < tblAvances.Columns.Count; i++)
                                    {
                                        if (tblAvances.Columns[i].ColumnName == "FEC_ASIG" || tblAvances.Columns[i].ColumnName == "FECHA DE RE" || tblAvances.Columns[i].ColumnName == "FECHA_CS" || tblAvances.Columns[i].ColumnName == "FECHA_APERT")
                                            //sCreateTable += ("\r\n [" + tblAvances.Columns[i] + "] DATE NULL, ");
                                            sCreateTable += ("\r\n [" + tblAvances.Columns[i] + "] VARCHAR(250) NULL, ");
                                        else
                                            sCreateTable += ("\r\n [" + tblAvances.Columns[i] + "] VARCHAR(250) NULL, ");
                                    }

                                    sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + " );";

                                    DataBaseConn.SetCommand(sCreateTable);

                                    if (!DataBaseConn.Execute("CreaAsignacionBBVA_OGR"))
                                    {
                                        Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                                        lector.Close();
                                        Invoke((Action)delegate() { btnRuta.Visible = true; picWait.Visible = false; });
                                        return;
                                    }
                                }                                
                            }
                            //dsTablas.Tables.Add(tblAvances);
                            Mensaje("Insertando datos, por favor espere...", Color.DimGray, lblMensajes);
                            using (SqlBulkCopy blkData = DataBaseConn.bulkCopy)
                            {
                                {
                                    try
                                    {
                                        DataBaseConn.ConnectSQL(true);
                                        //blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                                        //blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                                        blkData.NotifyAfter = 317;
                                        blkData.DestinationTableName = "dbEstadistica.Riesgos.AsignacionBBVA_OGR";
                                        blkData.BulkCopyTimeout = 30 * 60;
                                        blkData.WriteToServer(tblAvances);
                                    }
                                    catch (Exception ex)
                                    {
                                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                                        lector.Close();
                                        //Invoke((Action)delegate() { btnRuta.Visible = true; picWait.Visible = false; });
                                        //return;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                    lector.Close();
                }
                
                
            }
            Mensaje("El proceso terminó", Color.DimGray, lblMensajes);
            lector.Close();
            Invoke((Action)delegate() { btnRuta.Visible = true; picWait.Visible = false; });
        }
        #endregion

        #region Eventos
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
            {
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                btnRuta.Visible = true;
                picWait.Visible = false;
                return;
            }

            btnRuta.Visible = false;
            picWait.Visible = true;
            string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath);
            Mensaje("Cargando archivos, por favor espere...", Color.DimGray, lblMensajes);
            Carpeta = folderBrowserDialog1.SelectedPath;
            DataBaseConn.StartThread(Archivo);
        }

        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Hilos
        public void Mensaje(string sMessage, Color color, Label lblMensajes)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new SetStringCallback(Mensaje), new object[] { sMessage, color, lblMensajes });
            }
            else
            {
                lblMensajes.Text = sMessage;
                lblMensajes.ForeColor = color;
                lblMensajes.Visible = true;
            }
        }
        #endregion

    
    }
}
