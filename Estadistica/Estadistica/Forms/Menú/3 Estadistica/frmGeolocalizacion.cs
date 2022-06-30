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
using System.Net;
using System.Text.RegularExpressions;

namespace Estadistica
{
    public partial class frmGeolocalizacion : Form
    {
        public frmGeolocalizacion()
        {
            InitializeComponent();
            PreparaVentana();
        }        

        #region Variables
        DataTable tblArchivo = new DataTable();
        dbFileReader lector;
        int iNumberRecords;
        int idCartera;
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        delegate void SetBoolCallback(bool var);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            //Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;
            //cmbCarteras.SelectedIndex = -1;
        }
        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;            
        }
        public void ETL_Archivo(object oFilePath)
        {
            lblMensajes.Text = "Leyendo libro de Excel...";
            string sFilePath = oFilePath.ToString().Trim();

            if (!File.Exists(sFilePath))
            {
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                btnArchivo.BeginInvoke((Action)delegate() { btnArchivo.Visible = true; });
                return;
            }

            BeginFinsihProcess(true);
            Mensaje("", Color.DimGray, lblMensajes);

            lector = new dbFileReader(sFilePath);

            string sResultado = lector.Open();
            if (sResultado != "")
            {
                Mensaje(sResultado, Color.Red, lblMensajes);
                Invoke((Action)delegate { picWait.Visible = false; });
                return;
            }

            lector.Open();
            iNumberRecords = lector.CountRows();
            try
            {
                lector.Read(0);
                tblArchivo = new DataTable();
                tblArchivo.Load(lector.Reader);

                string[] LayoutArchivo;
                LayoutArchivo = new string[9];
                LayoutArchivo[0] = "idCuenta";
                LayoutArchivo[1] = "Calle";
                LayoutArchivo[2] = "ColoniaLocalidad";
                LayoutArchivo[3] = "DelegaciónMunicipio";
                LayoutArchivo[4] = "Estado";
                LayoutArchivo[5] = "CódigoPostal";
                LayoutArchivo[6] = "División";
                LayoutArchivo[7] = "Latitud";
                LayoutArchivo[8] = "Longitud";

                for (int lay = 0; lay < LayoutArchivo.Length; lay++)
                {
                    if (tblArchivo.Columns[lay].ColumnName != LayoutArchivo[lay])
                    {
                        Mensaje("Falta la columna " + LayoutArchivo[lay] + " en el archivo. Seleccione el archivo con el layout correcto.", Color.Red, lblInstrucciones);
                        lector.Close();
                        Invoke((Action)delegate { btnArchivo.Visible = true; });
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje("Error " + ex.Message, Color.Red, lblMensajes);
                return;
            }

            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'GeoN_" + Ejecutivo.Datos["idEjecutivo"] + "'  AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.GeoN_" + Ejecutivo.Datos["idEjecutivo"]);

            if (!DataBaseConn.Execute("DropTemporalGeoN")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWait.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.GeoN_" + Ejecutivo.Datos["idEjecutivo"] + " ( ";
            for (int i = 0; i < tblArchivo.Columns.Count; i++){
                if (tblArchivo.Columns[i].ColumnName.Contains("Fecha"))
                    sCreateTable += ("\r\n [" + tblArchivo.Columns[i] + "] DATE NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblArchivo.Columns[i] + "] VARCHAR(200) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);
            
            DataTable CreaTemporalGeoN = new DataTable();
           
            if (!DataBaseConn.Fill(CreaTemporalGeoN, "CreaTemporalGeoN")){
                Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                lector.Close();
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWait.Visible = false; });
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
                        blkData.DestinationTableName = "dbEstadistica.Temp.GeoN_" + Ejecutivo.Datos["idEjecutivo"];
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblArchivo);
                    }
                    catch (Exception ex)
                    {
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        lector.Close();
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWait.Visible = false; });
                        return;
                    }
                }

                Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + tblArchivo.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
                Mensaje("El archivo se cargó correctamente.", Color.SlateGray, lblInstrucciones);

                Invoke((Action)delegate()
                {
                    dgvGeolocalizacion.DataSource = tblArchivo;
                    dgvGeolocalizacion.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                    picWait.Visible = false;
                    btnCargar.Visible = btnArchivo.Visible = dgvGeolocalizacion.Visible = true;
                });

            }
            BeginFinsihProcess(false);
        }

        public void Geolocalizacion()
        {
            Mensaje("Determinando cuentas a geolocalizar, por favor espere...", Color.DimGray, lblMensajes);
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Estadistica.[2.1.Geolocalización] @idCartera, @idEjecutivo ");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);            
            DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);

            DataTable tblCuentasGeolocalizacion = new DataTable();

            if (!DataBaseConn.Fill(tblCuentasGeolocalizacion, "CuentasGeolocalizacion"))
            {
                TerminaBúsqueda("Falló la consulta.", true);
                return;
            }

            if (tblCuentasGeolocalizacion.Rows.Count > 0)
            {
                Invoke((Action)delegate { txtRuta.Visible = btnArchivo.Visible = btnCargar.Visible = false; });
                Invoke((Action)delegate { picWait.Visible = true; });

                LimpiaMensajes();
                string sCuenta = "", sCalle = "", sColoniaLocalidad = "", sDelegaciónMunicipio = "", sEstado = "", sCódigoPostal = "", sDivisión = "", sLatitud = "", sLongitud = "", sCP = "", sResultado="", sInformaciónEncontrada="";
                Boolean ValsourceCode = false;
                string sSourceCode = "";

                string sFecha_Insert = DateTime.Now.ToString("yyy-MM-dd");
                string idEjecutivo = Ejecutivo.Datos["idEjecutivo"].ToString();

                for (int i = 0; i < tblCuentasGeolocalizacion.Rows.Count; i++)
                {

                    Mensaje(i + 1 + " registro de " + tblCuentasGeolocalizacion.Rows.Count, Color.DimGray, lblMensajes);

                    System.Threading.Thread.Sleep(1000);

                    sCuenta = tblCuentasGeolocalizacion.Rows[i]["idCuenta"].ToString().Trim();
                    sCalle = tblCuentasGeolocalizacion.Rows[i]["Calle"].ToString().Trim();
                    sColoniaLocalidad = tblCuentasGeolocalizacion.Rows[i]["ColoniaLocalidad"].ToString().Trim();
                    sDelegaciónMunicipio = tblCuentasGeolocalizacion.Rows[i]["DelegaciónMunicipio"].ToString().Trim();
                    sEstado = tblCuentasGeolocalizacion.Rows[i]["Estado"].ToString().Trim();
                    sCódigoPostal = tblCuentasGeolocalizacion.Rows[i]["CódigoPostal"].ToString().Trim();
                    sDivisión = tblCuentasGeolocalizacion.Rows[i]["División"].ToString().Trim();
                    sLatitud = tblCuentasGeolocalizacion.Rows[i]["Latitud"].ToString().Trim();
                    sLongitud = tblCuentasGeolocalizacion.Rows[i]["Longitud"].ToString().Trim();


                    string DirWeb = "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + sLatitud + "," + sLongitud + "&AIzaSyCuLvKJRqYn92rmDmRGzsJKDWQgWprxb38";

                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(DirWeb);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        StreamReader sr = new StreamReader(response.GetResponseStream());
                        sSourceCode = sr.ReadToEnd();
                        sr.Close();
                        response.Close();
                        sSourceCode = sSourceCode.ToString().Trim().Replace("\"", "<").Replace("  ", " ");
                        ValsourceCode = true;
                    }
                    catch (Exception ex)
                    {
                        ValsourceCode = false;                        
                    }

                    if(ValsourceCode==false)
                    {
                        sResultado = "";
                        sInformaciónEncontrada = "";
                    }
                    else
                    {
                        if (sSourceCode.LastIndexOf("OVER_QUERY_LIMIT") > 0){
                            sResultado = "Excediste tu límite de búsqueda";
                            sInformaciónEncontrada = "";}

                        if (sSourceCode.LastIndexOf("REQUEST_DENIED") > 0){
                            sResultado = "Tu solicitud fue rechazada";
                            sInformaciónEncontrada = "";}

                        if (sSourceCode.LastIndexOf("INVALID_REQUEST") > 0){
                            sResultado = "La consulta resulta incompleta";
                            sInformaciónEncontrada = "";}

                        if (sSourceCode.LastIndexOf("UNKNOWN_ERROR") > 0){
                            sResultado = "Error al procesar solicitud al servidor";
                            sInformaciónEncontrada = "";}

                        if (sSourceCode.LastIndexOf("ZERO_RESULTS") > 0){
                            sResultado = "No existe resultado para su búsqueda";
                            sInformaciónEncontrada = "";}

                        if (sSourceCode.LastIndexOf("OK") > 0){
                            int idxDirIni = sSourceCode.IndexOf("<formatted_address<");
                            int idxDirFin = sSourceCode.IndexOf("<geometry<");

                            int idxCpFin = sSourceCode.IndexOf("<postal_code<");
                            int idxCpIni = idxCpFin - 50;

                            string DomicilioEncontrado = "";

                            try{
                                DomicilioEncontrado = sSourceCode.Substring(idxDirIni, idxDirFin - idxDirIni).ToString();
                                DomicilioEncontrado = DomicilioEncontrado.ToString().Replace("<formatted_address<", "").Replace("<", "").Replace(":", "").Trim();}
                            catch { DomicilioEncontrado = ""; }

                            //string CpEncontrado = "";
                            //string resultCpEncontrado = "";

                            //try{
                            //    CpEncontrado = sSourceCode.Substring(idxCpIni, idxCpFin - idxCpIni).ToString();
                            //    CpEncontrado = CpEncontrado.ToString().Replace("<long_name<", "").Replace("<", "").Replace("types", "").Replace(":", "").Trim();
                            //    string input = CpEncontrado;
                            //    string pattern = "[^0-9]";
                            //    string replacement = "";
                            //    Regex rgx = new Regex(pattern);
                            //    resultCpEncontrado = rgx.Replace(input, replacement);}
                            //catch { CpEncontrado = ""; }

                            //if (CpEncontrado == "" && sCP != "")
                            //    CpEncontrado = sCP;

                            sResultado = "Geocódigo correcto";
                            sInformaciónEncontrada = DomicilioEncontrado.ToString();
                            //tblArchivo.Rows[i]["CpEncontrado"] = resultCpEncontrado;
                        }
                    }

                    System.Threading.Thread.Sleep(1000);

                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Estadistica].[2.2.Geolocalización_Inserta] @idCartera, @idCuenta, @Calle, @ColoniaLocalidad, @DelegaciónMunicipio, @Estado, @CódigoPostal, @División, @Latitud, @Longitud, @Resultado, @InformaciónEncontrada, @Fecha_Insert, @idEjecutivo ");
                    DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                    DataBaseConn.CommandParameters.AddWithValue("@idCuenta", sCuenta);
                    DataBaseConn.CommandParameters.AddWithValue("@Calle", sCalle);
                    DataBaseConn.CommandParameters.AddWithValue("@ColoniaLocalidad", sColoniaLocalidad);
                    DataBaseConn.CommandParameters.AddWithValue("@DelegaciónMunicipio", sDelegaciónMunicipio);
                    DataBaseConn.CommandParameters.AddWithValue("@Estado", sEstado);
                    DataBaseConn.CommandParameters.AddWithValue("@CódigoPostal", sCódigoPostal);
                    DataBaseConn.CommandParameters.AddWithValue("@División", sDivisión);
                    DataBaseConn.CommandParameters.AddWithValue("@Latitud", sLatitud);
                    DataBaseConn.CommandParameters.AddWithValue("@Longitud", sLongitud);
                    DataBaseConn.CommandParameters.AddWithValue("@Resultado", sResultado);
                    DataBaseConn.CommandParameters.AddWithValue("@InformaciónEncontrada", sInformaciónEncontrada);
                    DataBaseConn.CommandParameters.AddWithValue("@Fecha_Insert", sFecha_Insert);
                    DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", idEjecutivo);

                    if (!DataBaseConn.Execute("InsertaGeolocalización"))
                    {
                        Mensaje("Falló al insertar Geolocalización.", Color.Crimson, lblMensajes);
                        //picWait.Visible = false;
                        //btnAgregar.Visible = true;
                        //return;
                    }
                }
                Invoke((Action)delegate { txtRuta.Visible = btnArchivo.Visible = true; });
                Invoke((Action)delegate { picWait.Visible = false; });
                Invoke((Action)delegate { txtRuta.Text = ""; });
                LimpiaMensajes();
                Mensaje("El proceso terminó.", Color.Green, lblMensajes);
            }
            else
            {
                Mensaje("No existen cuentas para geolocalizar.", Color.Red, lblInstrucciones);                
            }

                Invoke((Action)delegate
                {
                    btnArchivo.Visible = true;
                    picWait.Visible = false;
                    dgvGeolocalizacion.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                });
        }
        

        public void BeginFinsihProcess(bool Start)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetBoolCallback(BeginFinsihProcess), Start);
                return;
            }

            btnArchivo.Visible = !Start;
            txtRuta.ReadOnly = Start;
            
            if (tblArchivo.Rows.Count > 0)
            {
                btnCargar.Visible = true;
                Mensaje("Presione el botón iniciar.", Color.DimGray, lblMensajes);
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
            picWait.Visible = false;
            dgvGeolocalizacion.DataSource = null;
            lblInstrucciones.Text = "";
            lblRegistros.Text = "";
            txtRuta.Text = "";
            btnCargar.Visible = false;
            picWait.Visible = false;

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CambiaCartera();
            if (cmbCarteras.Text != "")
            {
                txtRuta.Visible = btnArchivo.Visible = true;
                //Producto = cmbProducto.Text;
                //dgvPagos.DataSource = null;
                //dgvPagos.Visible = btnCargar.Visible = false;
                txtRuta.Text = "";
                LimpiaMensajes();
                Mensaje("Seleccione un archivo con el layout de correcto.", Color.SlateGray, lblMensajes);
            }
            else
                Mensaje("Seleccione una cartera.", Color.SlateGray, lblMensajes);
        }
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            btnCargar.Visible = false;
            tblArchivo.Clear();            

            try
            {
                SeleccionarArchivo.Title = "Seleccione Archivo";
                SeleccionarArchivo.Filter = "Archivos .xls y .xlsx (*.xls, *.xlsx)|*.xls;*.xlsx";
                SeleccionarArchivo.ShowDialog(this);
                txtRuta.Text = SeleccionarArchivo.FileName;
                int UltimaPosicionPunto = txtRuta.Text.LastIndexOf(".");
                int Extension = txtRuta.Text.Length - UltimaPosicionPunto;
                lblMensajes.Text = "Leyendo libro de Excel...";

                Thread hiloETL = new Thread(ETL_Archivo);
                hiloETL.IsBackground = true;
                hiloETL.Start(txtRuta.Text);
            }
            catch
            {
                Mensaje("Seleccione un archivo para realizar proceso de Geolocalización.", Color.DimGray, lblMensajes);
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            Mensaje("", Color.DimGray, lblInstrucciones);
            btnCargar.Visible = false;
            picWait.Visible = true;
            btnArchivo.Visible = false;
            DataBaseConn.StartThread(Geolocalizacion);
        }
        #endregion

        #region Hilos
        protected void LimpiaMensajes()
        {
            Mensaje("", Color.DimGray, lblRegistros);
            Mensaje("", Color.DimGray, lblInstrucciones);
            Mensaje("", Color.DimGray, lblMensajes);
        }
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

        private void frmGeolocalizacion_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
            if (lector != null)
                lector.Close();
        }        
        
    }
}
