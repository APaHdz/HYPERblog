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

using ExcelDataReader;

namespace Estadistica
{
    public partial class frmCargaFacturas : Form
    {
        public frmCargaFacturas()
        {
            InitializeComponent();
            //dtpInicial.Format = DateTimePickerFormat.Custom;
            //dtpInicial.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }        

        #region Variables        
        DataTable tblFactura = new DataTable();
        DataTable tblFile = new DataTable();
        DataTable tblTab = new DataTable();
        string Tabla = "";
        string[] Layout;
        int Campos = 0;
        string NombreColumna = "";
        int iNumberRecords = 0;        
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvFactura.DefaultCellStyle.Font = controlEvents.RowFont;
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
            string fileName = Path.GetFileName(sFilePath);
            string Inicial = dtpInicial.Value.ToString("yyyy-MM-dd");
            string Final = dtpFinal.Value.ToString("yyyy-MM-dd");
            string Fecha_Insert = DateTime.Now.ToString("yyyy-MM-dd");
            string Segundo_Insert = DateTime.Now.ToString("HH:mm:ss");

            tblFile = new DataTable();
            DataColumn Archivos = new DataColumn("Archivos");
            Archivos.DataType = System.Type.GetType("System.String");
            DataColumn Tab = new DataColumn("Pestañas");
            Tab.DataType = System.Type.GetType("System.String");
            DataColumn Cargas = new DataColumn("Cargas");
            Cargas.DataType = System.Type.GetType("System.Boolean");
            DataColumn Observacion = new DataColumn("Observacion");
            Observacion.DataType = System.Type.GetType("System.String");
            tblFile.Columns.Add(Archivos);
            tblFile.Columns.Add(Tab);
            tblFile.Columns.Add(Cargas);
            tblFile.Columns.Add(Observacion);
            
            tblFactura = new DataTable();

            tblTab = new DataTable();
            DataColumn Archivoss = new DataColumn("Archivos");
            Archivoss.DataType = System.Type.GetType("System.String");
            DataColumn Tabs = new DataColumn("Pestañas");
            Tabs.DataType = System.Type.GetType("System.String");
            tblTab.Columns.Add(Archivoss);
            tblTab.Columns.Add(Tabs);

            if (!File.Exists(sFilePath)){
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }

            //Ruta del fichero Excel
            string filePath = sFilePath;            
            string path = filePath;
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                var result = reader.AsDataSet();
                

                int Pestañas = result.Tables.Count;

                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; " +
                    "SELECT DISTINCT Archivo FROM dbEstadistica.Amex.[Factura] WHERE Archivo = @Archivo \n UNION ALL \n" +
                    "SELECT DISTINCT Archivo FROM dbEstadistica.Amex.[Factura_BonusRR] WHERE Archivo = @Archivo \n UNION ALL \n" +
                    "SELECT DISTINCT Archivo FROM dbEstadistica.Amex.[Factura_Discounts] WHERE Archivo = @Archivo \n"                    
                    );
                DataBaseConn.CommandParameters.AddWithValue("@Archivo", fileName);
                DataTable tblArchivo = new DataTable();
                if (!DataBaseConn.Fill(tblArchivo, "ValidaArchivo" + fileName))
                {
                    Mensaje("Falló al validar archivo(" + Tabla + "), intente nuevamente", Color.Crimson, lblMensajes);
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                if (tblArchivo.Rows.Count > 0)
                {
                    if (MessageBox.Show("El archivo ya fue cargado anteriormente, ¿Desea eliminar la información y cargarla nuevamente?", "Archivo existente", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        Invoke((Action)delegate()
                        {
                            Mensaje("La carga ha sido cancelada por el usuario ya que el archivo ya estaba en el sistema.", Color.DimGray, lblMensajes);
                            btnCargar.Visible = false;
                            btnArchivo.Visible = true;
                            picWait.Visible = false;
                            txtRuta.Text = "";
                        });
                        return;
                    }
                }

                for (int i = 0; i < Pestañas; i++)
                {
                    //Campos = result.Tables[i].Columns.Count;
                    Tabla = result.Tables[i].TableName.Trim();
                    DataRow row1 = tblTab.NewRow();
                    row1["Archivos"] = fileName;
                    row1["Pestañas"] = Tabla;
                    tblTab.Rows.Add(row1);
                }

                Invoke((Action)delegate() { dgvPestañas.DataSource = tblTab; });

                for(int i = 0; i < Pestañas; i++)
                {
                    Campos = result.Tables[i].Columns.Count;
                    Tabla = result.Tables[i].TableName.Trim();
                    if (Tabla.Contains("CHARGE"))
                        Tabla = "CHARGE";                    

                    Tabla = Tabla.ToUpper();

                    try
                    {
                        if (result.Tables[i].Rows[0][0].ToString() == "" || result.Tables[i].Rows[0][1].ToString() == "" || result.Tables[i].Rows[0][2].ToString() == "")
                            result.Tables[i].Rows.RemoveAt(0);
                    }
                    catch
                    {

                    }

                    for (int x = 0; x < Campos; x++)
                    {
                        NombreColumna = result.Tables[i].Rows[0][x].ToString();                        

                        if(NombreColumna!="")
                        {
                            if (NombreColumna.Contains("REP AXIOM"))
                                NombreColumna = "REP AXIOM";

                            NombreColumna = NombreColumna.ToUpper();

                            result.Tables[i].Columns[x].ColumnName = NombreColumna.ToString().Trim();
                        }
                        else
                        {
                            result.Tables[i].Columns.RemoveAt(x);
                            Campos = Campos - 1;
                            x = x - 1;
                        }
                    }

                    if(result.Tables[i].Rows.Count>0)
                        result.Tables[i].Rows.RemoveAt(0);

                    if (result.Tables[i].Rows.Count == 0)
                    {
                        result.Tables.Remove(Tabla);
                        Tabla = "";
                    }
                    //DataRow row1 = tblFile.NewRow();
                    if (Tabla != "")
                    {                                                                        
                        if (Tabla == "LP Y MP")
                        {
                            Layout = new string[] { "8 BYTE CODE", "TRANSACTION DATE", "ACCOUNT NUMBER", "TRANSACTION CODE", "DESCRIPTION CODE", "TRANSACTION AMOUNT", "MIN DUE PAYMENT", "AGENCY RATE 1%", "COMMISSION1", "MINDUE OVERAGE", "OVERAGE %", "COMMISSION 2", "TOTAL COMMISSION" };
                            foreach (string sColumna in Layout)
                                if (!result.Tables[i].Columns.Contains(sColumna))
                                {
                                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                        }
                        else if (Tabla == "GRCC" || Tabla == "RCP" || Tabla == "CORPORATE" || Tabla == "SBS")
                        {
                            Layout = new string[] { "REP AXIOM", "TRANSACTION DATE", "ACCOUNT NUMBER", "TRANSACTION CODE", "DESCRIPTION CODE", "TRANSACTION AMOUNT", "AGENCY RATE %", "COMMISSION AMOUNT" };
                            foreach (string sColumna in Layout)
                                if (!result.Tables[i].Columns.Contains(sColumna))
                                {
                                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                        }
                        else if (Tabla == "CORPORATE CONSORCIO" || Tabla.Contains("GDC"))
                        {
                            Layout = new string[] { "TRANSACTION DATE", "ACCOUNT NUMBER", "CONCEPT", "TRANSACTION AMOUNT USD", "TRANSACTION AMOUNT MEX", "AGENCY RATE %", "COMMISSION AMOUNT" };
                            foreach (string sColumna in Layout)
                                if (!result.Tables[i].Columns.Contains(sColumna))
                                {
                                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                        }
                        else if (Tabla.Contains("CHARGE") || Tabla.Contains("IDC"))
                        {
                            Layout = new string[] { "REP AXIOM", "DATE PLACED", "TRANSACTION DATE", "ACCOUNT NUMBER", "TRANSACTION CODE", "DESCRIPTION CODE", "TRANSACTION AMOUNT USD", "TRANSACTION AMOUNT MEX", "AGENCY RATE %", "COMMISSION AMOUNT" };
                            foreach (string sColumna in Layout)
                                if (!result.Tables[i].Columns.Contains(sColumna))
                                {
                                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                        }
                        else if (Tabla.Contains("DISCOUNTS CONSORCIO"))
                        {
                            Layout = new string[] { "MARKET", "AGENCY", "AGENCY ID", "TRANSACTION DATE", "ACCOUNT DETAILS", "TRANSACTION CODE", "DESCRIPTION CODE", "TRANSACTION AMOUNT", "MIN DUE", "R1", "COMM-MINDUE", "OVERAGE", "R2", "COMM-OVG", "TOTAL COMMISSION AMOUNT", "PRODUCT", "LP-MP / Legales" };
                            foreach (string sColumna in Layout)
                                if (!result.Tables[i].Columns.Contains(sColumna))
                                {
                                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                        }
                        else if (result.Tables[0].TableName == "Summary" && result.Tables[1].TableName == "Base")
                        {
                            result.Tables.Remove("Summary");
                            Campos = result.Tables[0].Columns.Count;

                            for (int x = 0; x < Campos; x++)
                            {
                                NombreColumna = result.Tables[0].Rows[0][x].ToString();

                                if (NombreColumna != "")
                                {                                  
                                    NombreColumna = NombreColumna.ToUpper();

                                    result.Tables[0].Columns[x].ColumnName = NombreColumna.ToString().Trim();
                                }
                                else
                                {
                                    result.Tables[0].Columns.RemoveAt(x);
                                    Campos = Campos - 1;
                                    x = x - 1;
                                }
                            }
                            result.Tables[0].Rows.RemoveAt(0);
                            Tabla = result.Tables[0].TableName.Trim();
                            Layout = new string[] { "last_month_balance", "CM15", "rcode", "Placement", "Agency", "last_age", "current_age", "Formula 1", "Formula 2", "Rollback", "CONS", ">1000", "Bucket", "Not be prev", ">= 60 and NOT WO", "Aply", "$ Bono", "Plac", "A Name"};
                            foreach (string sColumna in Layout)
                                if (!result.Tables[i].Columns.Contains(sColumna))
                                {
                                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falta la columna " + sColumna + " en el archivo. Verifique el layout(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                            Pestañas = Pestañas - 1;
                            Tabla = "BonusRR";
                        }
                        else
                        {
                            Mensaje("El layout no corresponde a ninguna pestaña establecida como layout(" + Tabla + ").", Color.Red, lblMensajes);
                            DataRow row1 = tblFile.NewRow();
                            row1["Archivos"] = fileName;
                            row1["Pestañas"] = Tabla;
                            row1["Cargas"] = false;
                            row1["Observacion"] = "El layout no corresponde a ninguna pestaña establecida como layout(" + Tabla + ").";
                            tblFile.Rows.Add(row1);
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                            return;
                        }

                        Mensaje("Creando tabla temporal(" + Tabla + "), por favor espere...", Color.Green, lblMensajes);
                        DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Factureishon'  AND schema_id = 6 ) " +
                            " DROP TABLE dbEstadistica.Temp.[Factureishon]");

                        if (!DataBaseConn.Execute("DropTemporal" + Tabla))
                        {
                            Mensaje("Falló al eliminar tabla temporal(" + Tabla + ")", Color.Crimson, lblMensajes);
                            DataRow row1 = tblFile.NewRow();
                            row1["Archivos"] = fileName;
                            row1["Pestañas"] = Tabla;
                            row1["Cargas"] = false;
                            row1["Observacion"] = "Falló al eliminar tabla temporal(" + Tabla + ")";
                            tblFile.Rows.Add(row1);
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                            return;
                        }

                        string sCreateTable = "CREATE TABLE dbEstadistica.Temp.[Factureishon] ( ";
                        for (int y = 0; y < result.Tables[i].Columns.Count; y++)
                        {
                            sCreateTable += ("\r\n [" + result.Tables[i].Columns[y] + "] VARCHAR(100) NULL, ");
                        }
                        sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";


                        DataBaseConn.SetCommand(sCreateTable);
                        if (!DataBaseConn.Execute("CreaTemporal(" + Tabla + ")"))
                        {
                            Mensaje("Falló al crear tabla temporal(" + Tabla + ")", Color.Crimson, lblMensajes);
                            DataRow row1 = tblFile.NewRow();
                            row1["Archivos"] = fileName;
                            row1["Pestañas"] = Tabla;
                            row1["Cargas"] = false;
                            row1["Observacion"] = "Falló al crear tabla temporal(" + Tabla + ")";
                            tblFile.Rows.Add(row1);
                            Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                            return;
                        }

                        Mensaje("Insertando datos a tabla(" + Tabla + "), por favor espere...", Color.DimGray, lblMensajes); 
 
                        using (SqlBulkCopy blkData = DataBaseConn.bulkCopy)
                        {
                            {
                                try
                                {
                                    DataBaseConn.ConnectSQL(true);
                                    blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                                    blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                                    blkData.NotifyAfter = 317;
                                    blkData.DestinationTableName = "dbEstadistica.Temp.[Factureishon]";
                                    blkData.BulkCopyTimeout = 30 * 60;
                                    blkData.WriteToServer(result.Tables[i]);
                                }
                                catch (Exception ex)
                                {
                                    Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falló al insertar los datos, " + ex.ToString();
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }

                                Mensaje("Insertando información(" + Tabla + "), por favor espere...", Color.DimGray, lblMensajes);
                                if (Tabla == "LP Y MP")
                                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.1.Factura_LPyMP] @Inicial, @Final, @Archivo, @Pestaña, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");
                                else if (Tabla == "CORPORATE CONSORCIO" || Tabla == "GDC")
                                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.1.Factura_CorporateConsorcio] @Inicial, @Final, @Archivo, @Pestaña, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");
                                else if (Tabla == "CHARGE" || Tabla == "IDC")
                                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.1.Factura_Charge] @Inicial, @Final, @Archivo, @Pestaña, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");
                                else if (Tabla == "DISCOUNTS CONSORCIO")
                                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.1.Factura_Discounts] @Inicial, @Final, @Archivo, @Pestaña, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");
                                else if (Tabla == "BonusRR")
                                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.1.Factura_BonusRR] @Inicial, @Final, @Archivo, @Pestaña, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");
                                else
                                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:01'; EXEC dbEstadistica.[Amex].[1.1.Factura] @Inicial, @Final, @Archivo, @Pestaña, @Fecha_Insert, @Segundo_Insert, @idEjecutivo");

                                DataBaseConn.CommandParameters.AddWithValue("@Inicial", Inicial);
                                DataBaseConn.CommandParameters.AddWithValue("@Final", Final);
                                DataBaseConn.CommandParameters.AddWithValue("@Archivo", fileName);
                                DataBaseConn.CommandParameters.AddWithValue("@Pestaña", Tabla);
                                DataBaseConn.CommandParameters.AddWithValue("@Fecha_Insert", Fecha_Insert);
                                DataBaseConn.CommandParameters.AddWithValue("@Segundo_Insert", Segundo_Insert);
                                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);


                                if (!DataBaseConn.Execute("InsertaFactura"))
                                {
                                    DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Factureishon'  AND schema_id = 6 ) " +
                                                " DROP TABLE dbEstadistica.Temp.[Factureishon]");

                                    if (!DataBaseConn.Execute("DropTemporal" + Tabla))
                                    {
                                        Mensaje("Falló al eliminar tabla temporal(" + Tabla + ")", Color.Crimson, lblMensajes);
                                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                                        //return;
                                    }

                                    Mensaje("Falló al insertar factura a la base(" + Tabla + ").", Color.Red, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = false;
                                    row1["Observacion"] = "Falló al insertar factura a la base(" + Tabla + ").";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; dgvFactura.DataSource = tblFile; });
                                    return;
                                }
                                else
                                {
                                    Mensaje("La información se insertó a la base(" + Tabla + ").", Color.DimGray, lblMensajes);
                                    DataRow row1 = tblFile.NewRow();
                                    row1["Archivos"] = fileName;
                                    row1["Pestañas"] = Tabla;
                                    row1["Cargas"] = true;
                                    row1["Observacion"] = "La pestaña se subió correctamente.";
                                    tblFile.Rows.Add(row1);
                                    Invoke((Action)delegate() { dgvFactura.DataSource = tblFile; });
                                }

                                

                                //////Mensaje("La información se ingreso en el sistema(" + Tabla + ").", Color.DimGray, lblMensajes);
                            }
                            Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);
                            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Factureishon'  AND schema_id = 6 ) " +
                            " DROP TABLE dbEstadistica.Temp.[Factureishon]");

                            if (!DataBaseConn.Execute("DropTemporal" + Tabla))
                            {
                                Mensaje("Falló al eliminar tabla temporal(" + Tabla + ")", Color.Crimson, lblMensajes);
                                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                                //return;
                            }
                        }                        
                    }                    
                    //Mensaje("La información se ingreso en el sistema.", Color.DimGray, lblMensajes);
                }
                Console.Read();                
            }                        
            Mensaje("", Color.DimGray, lblMensajes);
                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvFactura.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                    dgvFactura.DataSource = tblFile;
                });
                LimpiaMensajes();
                
        }        
                
        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            //Mensaje("Transfiriendo... " + e.RowsCopied.ToString("N0") + "/" + iNumberRecords.ToString("N0") + " registros. " + (e.RowsCopied / (float)iNumberRecords * 100).ToString("N2") + " %", Color.DimGray, lblRegistros);
            Mensaje("Transfiriendo registros", Color.DimGray, lblRegistros);
        }
        #endregion

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvFactura.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            txtRuta.Text = ofdArchivo.FileName;
            if (MessageBox.Show("¿Las fechas y el archivo seleccionado son correctos? \n\n" + 
                "Inicial: " + dtpInicial.Value.ToString("yyyy-MM-dd") + "\n\n" + 
                "Final: " + dtpFinal.Value.ToString("yyyy-MM-dd") + "\n\n" +
                "Archivo: " + Path.GetFileName(ofdArchivo.FileName.ToString().Trim()).ToString() + "\n\n" 
                , "Validación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                Mensaje("La carga ha sido cancelada por el usuario.", Color.DimGray, lblMensajes);
                btnCargar.Visible = false;
                btnArchivo.Visible = true;
                picWait.Visible = false;
                txtRuta.Text = "";
            }
            else
            {
                Mensaje("Cargando archivo de Excel, por favor espere...", Color.DimGray, lblMensajes);
                btnCargar.Visible = btnArchivo.Visible = false;
                picWait.Visible = true;
                DataBaseConn.StartThread(Archivo, ofdArchivo.FileName);
            }

            btnCargar.Visible = false;
            btnArchivo.Visible = true;
            picWait.Visible = false;
            txtRuta.Text = "";
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            //Mensaje("", Color.DimGray, lblInstrucciones);
            //btnCargar.Visible = false;
            //picWaitBulk.Visible = true;
            //btnArchivo.Visible = false;
            //DataBaseConn.StartThread(CargaFactura);
        }

        private void frmCargaFacturas_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();      
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
