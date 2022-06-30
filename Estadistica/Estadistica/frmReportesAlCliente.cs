using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estadistica {
    public partial class frmReportesAlCliente : Form {
        public frmReportesAlCliente() {
            InitializeComponent();

            dtpHasta.Value = DateTime.Today.AddDays(-1);
            //dtpHasta.MaxDate = dtpHasta.Value;
            dtpHasta.MaxDate = DateTime.Today.AddDays(+5);
            //dtpHasta.MinDate = DateTime.Today.AddDays(-30);
            dtpHasta.MinDate = DateTime.Today.AddYears(-2);

            PreparaVentana();
        }

        // Atributos.
        DataTable _Reportes = new DataTable();
        delegate void SetStringCallback(string text, Color color, Label label);

        public void Texto(){
            string Texto = "Polcal" +
            "Consorcio Juridico de Cobranza Especializada S.A. de C.V. es una empresa cuyo personal esta consciente que la pasion, " +
            "imaginacion, inteligencia, talento y esfuerzo son los instrumentos necesarios para satisfacer nuestras propias necesidades, " +
            "consolidar nuestro liderazgo en el mercado cumpliendo con los requisitos que nos marcan nuestros clientes y regulaciones " +
            "vigentes, mejorando continuamente la eficacia de nuestro Sistema de Gestion de Calidad. " +

            "Objcal " +
            "1. Posicionarse en los primeros lugares del mercado " +
            "2. Realizar nuestras gestiones en apego a las regulaciones vigentes y requisitos que determinan los clientes " +
            "3. Cumplir satisfactoriamente las auditorias de nuestros clientes " +
            "4. Mejorar continuamente la eficacia del sistema de gestion de calidad por medio de las mediciones de indicadores. " +

            "Mis " +
            "Consolidar el liderazgo en el mercado desarrollando integralmente nuestros recursos humanos, asi como nuevos sistemas de cobranza. " +

            "Vis " +
            "En consorcio juridico de cobranza especializada S.A. de C.V., el trabajo se desarrolla a traves de la pasion, imaginacion, " +
            "inteligencia, talento y esfuerzo, y con ello satisfacemos nuestras propias necesidades y las que requieren nuestros clientes " +

            "Val " +
            "Esfuerzo, Pasion, Solidaridad, Imaginacion, Honradez, Inteligencia, Sencillez y Talento " +

            "Consecuencias" +
            "1. Insatisfaccion o perdida de nuestros clientes " +
            "2. No mantener controles en nuestros procesos " +
            "3. Procesos independientes y no interrelacionados para el logro de los objetivos " +
            "4. No planificacion de los procesos " +
            "5. Perdida de reconocimiento como empresa certificada en calidad " +

            "Beneficios" +
            "1. Ofrecer servicios que satisfagan los requisitos del cliente, legales y reglamentarios " +
            "2. Abordar riesgos y oportunidades " +
            "3. Obtencion de nuevos clientes " +
            "4. Mejorar los procesos de organizacion " +
            "5. Cumplir los objetivos de la organizacion";

            Texto = Texto + "Texto";
        }

        // Métodos
        public void PreparaVentana() {

            dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpHasta.Value = DateTime.Today.AddDays(-1);
            dtpHasta.MaxDate = dtpDesde.MaxDate = DateTime.Today.AddDays(+5);
            

            // Productos
            cmbProductos.ValueMember = "idProducto";
            cmbProductos.DisplayMember = "Producto";
            cmbProductos.DataSource = Catálogo.Productos;
            if ( Ejecutivo.Datos["idProducto"].ToString() != "" ) {
                cmbProductos.SelectedValue = Ejecutivo.Datos["idProducto"];
                cmbProductos.Visible = false;
                lblProducto.Text = "Producto        " + cmbProductos.Text;
            }

            // Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;
            if ( Ejecutivo.Datos["idCartera"].ToString() != "" ) {
                cmbCarteras.SelectedValue = Ejecutivo.Datos["idCartera"];
                cmbCarteras.Visible = false;
                lblCartera.Text = "Cartera         " + cmbCarteras.Text;
            }

            CambiaCartera();
        }
        public void ObtieneReportes() {
            _Reportes = new DataTable("Reportes");
            if ( !DataBaseConn.Fill(_Reportes, "ObtieneReportes") ) {
                Mensaje("Falló al crear reportes", Color.Crimson, lblMensajes);
                return;
            }

            _Reportes.PrimaryKey = new DataColumn[] { _Reportes.Columns["idReporte"] };
            _Reportes.DefaultView.Sort = "idCartera, Reporte";

            this.Invoke((Action)delegate() {
                cmbReportes.ValueMember = "idReporte";
                cmbReportes.DisplayMember = "Reporte";
                cmbReportes.DataSource = _Reportes;

                CambiaCartera();
            });
        }

        public void CambiaCartera() {
            if ( cmbReportes.DataSource == null )
                return;

            _Reportes.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;
            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;

            CambiaReporte();

            lblMensajes.Text = "Indique el nombre del reporte, sus parámetros y presione Generar.";
        }
        public void CambiaReporte() {
            DataRow drReporte = _Reportes.Rows.Find(cmbReportes.SelectedValue);

            if ( drReporte == null ) {
                lblDesde.Visible = dtpDesde.Visible =
                lblHasta.Visible = dtpHasta.Visible =
                lblProducto.Visible = cmbProductos.Visible =
                false;
                btnGenerar.Visible = btnGenerar.Enabled = false;
                lblDescripción.Text = "";
                return;
            } else
                btnGenerar.Visible = btnGenerar.Enabled = true;

            lblDesde.Visible = dtpDesde.Visible = (bool)drReporte["ParamDesde"];
            lblHasta.Visible = dtpHasta.Visible = (bool)drReporte["ParamHasta"];
            if ( !(bool)drReporte["ParamDesde"] && (bool)drReporte["ParamHasta"] )
                lblHasta.Text = "Fecha";
            else
                lblHasta.Text = "Hasta";

            lblProducto.Visible = (bool)drReporte["ParamProducto"];
            cmbProductos.Visible = ( lblProducto.Visible && Ejecutivo.Datos["idProducto"].ToString() == "" );

            lblDescripción.Text = drReporte["Descripción"].ToString();
        }

        private void GeneraReporte() {

            DataSet dsReportes = new DataSet();

            if ( !DataBaseConn.Fill(dsReportes, "GeneraReporteAlCliente") ) {
                TerminaReporte("Falló la generación del reporte.", true);
                return;
            }

            string sNombresTablas = DataBaseConn.CommandParameters["@NombresParam"].Value.ToString();
            if ( sNombresTablas != "" ) { 
                string [] sNombres = sNombresTablas.Split(new char [] {'|'}, StringSplitOptions.RemoveEmptyEntries);
                for ( int i = 0; i < Math.Min(sNombres.Length, dsReportes.Tables.Count); i++ )
                    dsReportes.Tables[i].TableName = sNombres[i];
            
            }

            string sRutaExcel = "";

            if ( dsReportes.Tables.Count == 0 || dsReportes.Tables[0].Rows.Count == 0 ) {
                TerminaReporte("Reporte sin registros.", false);
                return;
            } else
                this.Invoke((Action)delegate() {
                    Mensaje("Consulta terminada. Guardando libro de Excel...", Color.SlateGray, lblMensajes);

                    if ( sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel )
                        TerminaReporte("Guardado cancelado por usuario.", false);
                    else
                        sRutaExcel = sfdExcel.FileName;
                });

            if ( sRutaExcel == "" ) {
                dsReportes.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref dsReportes, sRutaExcel);
            TerminaReporte(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");

        }
        public void TerminaReporte(string Mensaje, bool bError) {

            if ( this.InvokeRequired ) {
                this.BeginInvoke((Action)delegate() { TerminaReporte(Mensaje, bError); });
                return;
            }

            if ( bError )
                lblMensajes.ForeColor = System.Drawing.Color.Crimson;
            else
                lblMensajes.ForeColor = System.Drawing.Color.SlateGray;
            lblMensajes.Text = Mensaje;

            btnGenerar.Enabled = btnGenerar.Visible = true;
            picWait.Visible = false;

            this.ControlBox = true;
        }


        #region ThreadEnds
        public void Mensaje(string sMessage, Color color, Label lblMensaje) {
            if ( this.InvokeRequired ) {
                this.BeginInvoke(new SetStringCallback(Mensaje), new object[] { sMessage, color, lblMensaje });
            } else {
                lblMensaje.Text = sMessage;
                lblMensaje.ForeColor = color;
                lblMensaje.Visible = true;
            }
        }
        #endregion


        // Eventos
        private void frmReportesAlCliente_Shown(object sender, EventArgs e) {

            DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM ReportesAlCliente WHERE Activo = 1 AND Esquema NOT IN ('Contabilidad')");
            DataBaseConn.StartThread(ObtieneReportes);
        }

        private void cmbCarteras_SelectionChangeCommitted(object sender, EventArgs e) {
            CambiaCartera();
        }
        private void cmbReportes_SelectionChangeCommitted(object sender, EventArgs e) {
            CambiaReporte();
        }

        private void btnGenerar_Click(object sender, EventArgs e) {
            if ( Convert.ToInt32(Ejecutivo.Datos["Jerarquía"]) < 1 ) {
                lblMensajes.Text = "Carece de permisos para generar reportes.";
                return;
            }



            DataRow drReporte = _Reportes.Rows.Find(cmbReportes.SelectedValue);
            if ( drReporte == null )
                return;

            string sQuery = "SET DATEFORMAT DMY\r\n";

            if ( (bool)drReporte["EsProcedimientoOFunción"] )  // Es store
                sQuery = "EXEC " + drReporte["BaseDatos"] + "." + drReporte["Esquema"] + ".[" + drReporte["FunciónStore"] + "] ";
            else
                sQuery = "SELECT * FROM " + drReporte["BaseDatos"] + "." + drReporte["Esquema"] + "." + drReporte["FunciónStore"] + "( ";

            /*
            sQuery +=
                ( (bool)drReporte["ParamDesde"] ? "'" + dtpDesde.Value.ToString("yyyy-MM-dd") + "', " : "" ) +
                ( (bool)drReporte["ParamHasta"] ? "'" + dtpHasta.Value.ToString("yyyy-MM-dd") + "', " : "" ) +
                ( (bool)drReporte["ParamProducto"] ? cmbProductos.SelectedValue + ", " : "" ) +
                ( (bool)drReporte["NombresTabla"] ? " @NombresTablas = @NombresParam OUTPUT, ": "" ) 
                ;
            */
            sQuery +=
                ( (bool)drReporte["ParamDesde"] ? "@Fecha_Desde, " : "" ) +
                ( (bool)drReporte["ParamHasta"] ? "@Fecha_Hasta, " : "" ) +
                ( (bool)drReporte["ParamProducto"] ? "@iProducto, " : "" ) +
                ( (bool)drReporte["NombresTabla"] ? " @NombresTablas = @NombresParam OUTPUT, " : "" )
                ;

            sQuery = sQuery.TrimEnd(',', ' ');

            if ( !(bool)drReporte["EsProcedimientoOFunción"] )  // Es función
                sQuery += " )";

            DataBaseConn.SetCommand(sQuery);
            DataBaseConn.CommandParameters.AddWithValue("@Fecha_Desde", dtpDesde.Value);
            DataBaseConn.CommandParameters.AddWithValue("@Fecha_Hasta", dtpHasta.Value);
            DataBaseConn.CommandParameters.AddWithValue("@iProducto", cmbProductos.SelectedValue);

            DataBaseConn.CommandParameters.Add("@NombresParam", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output;            
            
            picWait.Visible = true;
            btnGenerar.Enabled = btnGenerar.Visible = false;

            Mensaje("Generando reporte, por favor espere...", Color.SlateGray, lblMensajes);
            DataBaseConn.StartThread(GeneraReporte);
        }

        private void frmDevoluciones_FormClosed(object sender, FormClosedEventArgs e) {
            DataBaseConn.CancelRunningQuery();
            DataBaseConn.ChangeDatabase("dbEstadistica");
        }


    }
}
