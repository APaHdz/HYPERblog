using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net.NetworkInformation;

using System.IO;
using System.Reflection;

using System.Net;

using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms.ToolTips;

namespace Estadistica
{
    public partial class frmRutas : Form
    {
        public frmRutas()
        {
            InitializeComponent();
            dtpMesAsignacion.Format = DateTimePickerFormat.Custom;
            dtpMesAsignacion.CustomFormat = "MM/yyyy";
            dtpMesAsignacion.Value = DateTime.Today.AddDays(-0);
            dtpMesAsignacion.MaxDate = dtpMesAsignacion.Value;
            dtpMesAsignacion.MinDate = DateTime.Today.AddDays(-365);
            PreparaVentana();
        }

        #region Variables
        string Fecha = "";
        int idCartera = 0;
        string Paquete = "";
        string idEjecutivo = "";
        DataTable tblDireccion;
        DataTable tblCarteras;
        int Proceso = 0;
        int Reg = 0;
        int Km = 0;
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {            
            //Carteras
            cmbCarteras.ValueMember = "idCartera";
            cmbCarteras.DisplayMember = "Cartera";
            cmbCarteras.DataSource = Catálogo.Carteras;
            //tblCarteras = (DataTable)cmbCarteras.DataSource;            
            //tblCarteras.DefaultView.RowFilter = "idCartera in (7)";
            //Mensaje("Seleccione mes de asignación y cartera.", Color.SlateGray, lblMensajes);            

            cmbRegion.SelectedIndex = 0;
            cmbSaldo.SelectedIndex = 0;
            cmbCorrInco.SelectedIndex = 0;

            cmbKilometros.SelectedIndex = 0;
        }        

        public void Ruta()
        {
            Mensaje("Consulta en proceso, favor espere unos segundos...", Color.DimGray, lblMensajes);
            tblDireccion = new DataTable();
            if (!DataBaseConn.Fill(tblDireccion, "ConsultaDirecciones"))
            {                
                Invoke((Action)delegate()
                {
                    btnConsulta.Visible = true;
                    picWaitBulk.Visible = false;
                    btnRutas.Visible = true;
                    picWaitRutas.Visible = false;
                    gbOpciones.Enabled = true;
                });
                Mensaje("Falló al realizar la consulta.", Color.Red, lblMensajes);
                return;
            }
            
            Reg = tblDireccion.Rows.Count;
            Invoke((Action)delegate()
            {
                if (tblDireccion.Rows.Count > 0)
                {
                    dgvGeolocalizacion.DataSource = tblDireccion;
                    dgvGeolocalizacion.Visible = true;
                    btnConsulta.Visible = true;
                    picWaitBulk.Visible = false;
                    gbOpciones.Enabled = true;
                    dgvGeolocalizacion.Visible = true;
                    btnRutas.Visible = true;
                    picWaitRutas.Visible = false;
                }
                else
                {
                    dgvGeolocalizacion.Visible = false;
                    btnConsulta.Visible = true;
                    picWaitBulk.Visible = false;
                    gbOpciones.Enabled = true;
                    dgvGeolocalizacion.Visible = false;
                    btnRutas.Visible = false;
                    picWaitRutas.Visible = false;
                }
                
            });
            Mensaje("La consulta terminó, se obtuvierón: " + Reg.ToString() + " registro(s).", Color.DimGray, lblMensajes);
        }
        public void DeterminaRuta()
        {
            string LATO = "";
            string LONO = "";
            string LATD = "";
            string LOND = "";

            tblDireccion.Columns.Add("Distancia");
            
            tblDireccion.Columns.Add("Ruta1");
            tblDireccion.Columns.Add("Ruta2");
            tblDireccion.Columns.Add("Ruta3");
            tblDireccion.Columns.Add("Ruta4");
            tblDireccion.Columns.Add("Ruta5");
         
            int Ruta1 = 1;
            int Ruta2 = 1;
            int Ruta3 = 1;
            int Ruta4 = 1;
            int Ruta5 = 1;            

            double Distancia = 0;

            for (int i = 0; i < tblDireccion.Rows.Count; i++)
            {
                Distancia = 0;
                if (i > 0)               
                {
                    LATO = tblDireccion.Rows[i-1]["Latitud"].ToString().Trim();
                    LONO = tblDireccion.Rows[i-1]["Longitud"].ToString().Trim();

                    LATD = tblDireccion.Rows[i]["Latitud"].ToString().Trim();
                    LOND = tblDireccion.Rows[i]["Longitud"].ToString().Trim();

                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
                                             "SELECT (ACOS(COS(RADIANS(90 - " + LATO + ")) * COS(RADIANS(90 - " + LATD + ")) + SIN(RADIANS(90 - " + LATO + ")) * SIN(RADIANS(90 - " + LATD + ")) * COS(RADIANS(" + LONO + " - " + LOND + "))) * 6371) DistanciaKms ");

                    DataTable tblDistancia = new DataTable();
                    if (!DataBaseConn.Fill(tblDistancia, "ActualizandoInformacion"))
                        Mensaje("Falló al calcular distancia.", System.Drawing.Color.Crimson, lblMensajes);
                        
                    if (tblDistancia.Rows.Count > 0)
                        Distancia = double.Parse(tblDistancia.Rows[0][0].ToString());
                    else
                        Distancia = 6;

                    tblDireccion.Rows[i]["Distancia"] = Distancia.ToString("N2");

                    //if(Distancia>Km)
                        //Ruta = Ruta + 1;

                    if (Distancia > 1)
                    {
                        Ruta1 = Ruta1 + 1;
                        tblDireccion.Rows[i]["Ruta1"] = Ruta1;                        
                    }
                    else
                    {
                        tblDireccion.Rows[i]["Ruta1"] = Ruta1;
                    }

                    if (Distancia > 2)
                    {
                        Ruta2 = Ruta2 + 1;
                        tblDireccion.Rows[i]["Ruta2"] = Ruta2;                        
                    }
                    else
                    {
                        tblDireccion.Rows[i]["Ruta2"] = Ruta2;
                    }

                    if (Distancia > 3)
                    {
                        Ruta3 = Ruta3 + 1;
                        tblDireccion.Rows[i]["Ruta3"] = Ruta3;                        
                    }
                    else
                    {
                        tblDireccion.Rows[i]["Ruta3"] = Ruta3;
                    }

                    if (Distancia > 4)
                    {
                        Ruta4 = Ruta4 + 1;
                        tblDireccion.Rows[i]["Ruta4"] = Ruta4;                        
                    }
                    else
                    {
                        tblDireccion.Rows[i]["Ruta4"] = Ruta4;
                    }

                    if (Distancia > 5)
                    {
                        Ruta5 = Ruta5 + 1;
                        tblDireccion.Rows[i]["Ruta5"] = Ruta5;                        
                    }
                    else
                    {
                        tblDireccion.Rows[i]["Ruta5"] = Ruta5;
                    }
                    
                    Mensaje("Calculando distancia (" + (i + 1).ToString() + " - " + Reg.ToString() + ") , este proceso puede tardar varios minutos, por favor espere...", System.Drawing.Color.ForestGreen, lblMensajes);                    
                }
                else
                {
                    tblDireccion.Rows[i]["Distancia"] = Distancia;
                    
                    tblDireccion.Rows[i]["Ruta1"] = Ruta1;
                    tblDireccion.Rows[i]["Ruta2"] = Ruta2;
                    tblDireccion.Rows[i]["Ruta3"] = Ruta3;
                    tblDireccion.Rows[i]["Ruta4"] = Ruta4;
                    tblDireccion.Rows[i]["Ruta5"] = Ruta5;                    
                }                
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

            btnConsulta.Visible = true;
            picWaitBulk.Visible = false;
            dgvGeolocalizacion.DataSource = null;            
            btnRutas.Visible = false;
            picWaitRutas.Visible = false;
            gbOpciones.Enabled = true;

            this.ControlBox = true;
        }
        public void ActualizandoInformacion()
        {
            DataTable tblDistancia = new DataTable();
            if (!DataBaseConn.Fill(tblDistancia,"ActualizandoInformacion"))            
                Mensaje("Falló al calcular distancia.", System.Drawing.Color.Crimson, lblMensajes);

            int Distancia = int.Parse(tblDistancia.Rows[0][0].ToString());

            int D = Distancia;
        }

        public void LimpiaParámetros()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate()
                {                    
                    btnConsulta.Visible = true;
                    picWaitBulk.Visible = false;
                });
                return;
            }
            
        }
        #endregion
       
        #region Eventos       
        private void btnCargar_Click(object sender, EventArgs e)
        {
            Fecha = dtpMesAsignacion.Value.ToString("yyyy-MM-dd");
            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            idEjecutivo = Ejecutivo.Datos["idEjecutivo"].ToString();
           
            btnConsulta.Visible = false;
            picWaitBulk.Visible = true;                          

            gbOpciones.Enabled = false;

                    string SCV = "";
                    string Region = "";
                    string Prod = "";
                    string Producto = "";
                    string Segmento = "";
                    string Segm = "";
                    string Saldo = "";
                    string CorrInc = "";

                    if (chkSCV.Checked == true)
                        SCV = " AND Dom.Marca_Especial = 'SCV'";

                    if (cmbRegion.Text == "*")
                        Region = " ";
                    else if (cmbRegion.Text == "CDMX")
                        Region = " AND Dom.Estado = 'Distrito Federal'";
                    else
                        Region = " AND CASE WHEN Dom.Estado = 'Distrito Federal' THEN DelegaciónMunicipio ELSE Dom.Estado END = '" + cmbRegion.Text + "'";

                    if (cmbCorrInco.Text != "*")
                        CorrInc = " AND Dom.EstatusDomicilioVisita = '" + cmbCorrInco.Text + "'";

                    for (int x = 0; x <= chkBProducto.CheckedItems.Count - 1; x++)                    
                        Prod = Prod + "'" + chkBProducto.CheckedItems[x].ToString() + "' ,";                    

                    if (Prod.Length > 0){
                        Prod = Prod.Substring(0, Prod.Length - 1);
                        Producto = " AND Dom.Producto IN (" + Prod + ")";}

                    for (int x = 0; x <= chkBSegmento.CheckedItems.Count - 1; x++)                    
                        Segm = Segm + "'" + chkBSegmento.CheckedItems[x].ToString() + "' ,";                    

                    if (Segm.Length > 0){
                        Segm = Segm.Substring(0, Segm.Length - 1);
                        Segmento = " AND Dom.Segmento IN (" + Segm + ")";}

                    if (cmbSaldo.Text == "< 10,000")
                        Saldo = " AND Dom.Saldo < 10000";
                    else if (cmbSaldo.Text == "> 10,000 y < 50,000")
                        Saldo = " AND Dom.Saldo BETWEEN 10000 AND 49999.99";
                    else if (cmbSaldo.Text == "> 50,000 y < 100,000")
                        Saldo = " AND Dom.Saldo BETWEEN 50000 AND 100000.99";
                    else if (cmbSaldo.Text == "> 100,000 y < 200,000")
                        Saldo = " AND Dom.Saldo BETWEEN 100000 AND 200000.99";
                    else if (cmbSaldo.Text == "> 200,000 y < 300,000")
                        Saldo = " AND Dom.Saldo BETWEEN 200000 AND 300000.99";
                    else if (cmbSaldo.Text == "> 300,000 y < 400,000")
                        Saldo = " AND Dom.Saldo BETWEEN 300000 AND 400000.99";
                    else if (cmbSaldo.Text == "> 400,000 y < 500,000")
                        Saldo = " AND Dom.Saldo BETWEEN 400000 AND 500000.99";
                    else if (cmbSaldo.Text == "> 500,000")
                        Saldo = " AND Dom.Saldo > 500000";
                    else
                        Saldo = "";

                    string SQuery = "SET TRAN ISOLATION LEVEL READ UNCOMMITTED \r\n\t" +
                                    "WAITFOR DELAY '00:00:00';  \r\n\t" +
                                    "UPDATE dbEstadistica.Estadistica.Domicilios SET Latitud=NULL, Longitud=NULL, Resultado='Geocódigo incorrecto' WHERE Informacion NOT LIKE '%Mex%' AND Resultado='Geocódigo correcto.'  \r\n\t" +
                                    "SELECT * FROM (  \r\n\t" +
                                    "SELECT  A.Fecha_Asignacion , \r\n\t" +
                                    "        C.Cartera , \r\n\t" +
                                    "        P.Producto , \r\n\t" +
                                    "        A.Codigo1 , \r\n\t" +
                                    "        A.Cuenta1 idCuenta , \r\n\t" +
                                    "        A.Cuenta2 Prestamo , \r\n\t" +
                                    "        A.Saldo , \r\n\t" +
                                    "        A.Mora , \r\n\t" +
                                    "        A.Segmento , \r\n\t" +
                                    "        A.Division , \r\n\t" +
                                    "        A.Region , \r\n\t" +
                                    "        A.Plaza , \r\n\t" +
                                    "        A.Marca_Especial , \r\n\t" +                                    
                                    "        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(D.Calle, ' ', '<>'), '><', ''), '<>', ' '), 'NUM.EXT.', ''), 'NUM.INT.', ''), '+', 'Ñ'), '#', 'Ñ'), '¿', 'Ñ'), '.', ''), '\"', ''), '=', ''), '-', '') Calle , \r\n\t" +
		                            "        REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(D.ColoniaLocalidad, ' ', '<>'), '><', ''), '<>', ' '), 'NUM.EXT.', ''), 'NUM.INT.', ''), '+', 'Ñ'), '#', 'Ñ'), '¿', 'Ñ'), '.', ''), '\"', ''), '=', ''), '-', '') ColoniaLocalidad , \r\n\t" +
                                    "        REPLACE(REPLACE(REPLACE(DelegaciónMunicipio, ' ', '<>'), '><', ''), '<>', ' ') DelegaciónMunicipio , \r\n\t" +
                                    "        CASE WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'AG' THEN 'Aguascalientes' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'BN' THEN 'Baja California' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'BS' THEN 'Baja California Sur' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CA' THEN 'Campeche' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CH' THEN 'Chihuahua' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CO' THEN 'Colima' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CS' THEN 'Chiapas' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CU' THEN 'Coahuila' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'COAHUILA DE ZARAGOZA' THEN 'Coahuila' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'DF' THEN 'Distrito Federal' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'DISTRITO FEDERAL' THEN 'Distrito Federal' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'DU' THEN 'Durango' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'EM' THEN 'México' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MEXICO' THEN 'México' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'GO' THEN 'Guerrero' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'GU' THEN 'Guanajuato' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'HI' THEN 'Hidalgo' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'HIDALGO' THEN 'Hidalgo' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'JA' THEN 'Jalisco' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MI' THEN 'Michoacán' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MICHOACAN DE OCAMPO' THEN 'Michoacán' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MO' THEN 'Morelos' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'NA' THEN 'Nayarit' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'NL' THEN 'Nuevo León' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'OA' THEN 'Oaxaca' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'PU' THEN 'Puebla' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'QR' THEN 'Quintana Roo' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'QU' THEN 'Querétaro' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'QUERETARO' THEN 'Querétaro' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'SI' THEN 'Sinaloa' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'SL' THEN 'San Luis Potosí' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'SO' THEN 'Sonora' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'TA' THEN 'Tabasco' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'TL' THEN 'Tlaxcala' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'TM' THEN 'Tamaulipas' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'VE' THEN 'Veracruz' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'VERACRUZ DE IGNACIO' THEN 'Veracruz' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'YU' THEN 'Yucatán' \r\n\t" +
                                    "             WHEN REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'ZA' THEN 'Zacatecas' \r\n\t" +
                                    "             ELSE REPLACE(REPLACE(REPLACE(D.Estado, ' ', '<>'), '><', ''), '<>', ' ') \r\n\t" +
                                    "        END Estado , \r\n\t" +
                                    "        D.CódigoPostal , \r\n\t" +
                                    "        CP.Periferia , \r\n\t" +
                                    "        D.Paquete , \r\n\t" +
                                    "        D.EstatusDomicilioVisita , \r\n\t" +
                                    "        D.ObservaciónVisita , \r\n\t" +
                                    "        D.DomicilioCarteo EstatusDomicilioCarteo , \r\n\t" +
                                    "        D.ObservaciónCarteo , \r\n\t" +
                                    "        V.Valor EstatusDomicilioGespa , \r\n\t" +
                                    "        CASE WHEN N.idCuenta IS NOT NULL THEN 'Domicilio Lista Negra' ELSE NULL END ListaNegra , \r\n\t" +
			                        "        ISNULL(F.Visitas,0) [Visitas Real] , \r\n\t" +
                                    "        F.[Contacto Real] , \r\n\t" +
                                    "        F.[Sin contacto Real] , \r\n\t" +
                                    "        F.[Ilocalizable Real] , \r\n\t" +
                                    "        ISNULL(FC.Visitas,0) [Visitas Complemento] , \r\n\t" +
                                    "        FC.[Contacto Complemento] , \r\n\t" +
                                    "        FC.[Sin contacto Complemento] , \r\n\t" +
                                    "        FC.[Ilocalizable Complemento] , \r\n\t" +
                                    "        D.Latitud , \r\n\t" +
                                    "        D.Longitud --, \r\n\t" +
                                    "        --D.Informacion Geolocalización \r\n\t" +
                                    "FROM    dbEstadistica.Estadistica.Asignaciones A \r\n\t" +
                                    "        INNER JOIN dbEstadistica.Estadistica.Domicilios D ON A.idCartera = D.idCartera \r\n\t" +
                                    "                                                             AND A.idProducto = D.idProducto \r\n\t" +
                                    "                                                             AND A.Cuenta1 = D.Cuenta1 \r\n\t" +
                                    "                                                             AND A.Fecha_Asignacion = D.Fecha_Asignacion \r\n\t" +
                                    "        INNER JOIN dbHistory..Carteras C ON A.idCartera = C.idCartera \r\n\t" +
                                    "        INNER JOIN dbHistory..Productos P ON A.idProducto = P.idProducto \r\n\t" +
                                    "        INNER JOIN dbHistory..ValoresCatálogo V ON D.idInformación = V.idValor \r\n\t" +
                                    "        LEFT JOIN ( SELECT  V.idCartera , \r\n\t" +
                                    "                                V.idCuenta , \r\n\t" +
                                    "                                COUNT(V.idCuenta) Visitas , \r\n\t" +
                                    "                                MAX(CASE WHEN Contacto = 'Contacto' THEN V.Fecha ELSE NULL END) [Contacto Real] , \r\n\t" +
                                    "                                MAX(CASE WHEN Contacto = 'Sin contacto' THEN V.Fecha ELSE NULL END) [Sin Contacto Real] , \r\n\t" +
                                    "                                MAX(CASE WHEN Contacto = 'Ilocalizable' THEN V.Fecha ELSE NULL END) [Ilocalizable Real] \r\n\t" +
                                    "                        FROM    ( SELECT    idCartera , \r\n\t" +
                                    "                                            idCuenta , \r\n\t" +
                                    "                                            Fecha_Visita , \r\n\t" +
                                    "                                            Segundo_Visita , \r\n\t" +
                                    "                                            CAST(CONVERT(VARCHAR(10), Fecha_Visita) + ' ' + CONVERT(VARCHAR(8), Segundo_Visita) AS DATETIME) Fecha , \r\n\t" +
                                    "                                            CASE  \r\n\t" +
									"					                            --Contacto -- Titular, Le conoce  \r\n\t" +
                                    "                                                 WHEN idContacto IN ( 1101, 1102 ) THEN 'Contacto'  \r\n\t" +
									"					                            --Sin contacto -- No le conoce, No corresponde, Existe domicilio, Desocupado, Sin acceso a domicilio, Zona de riesgo, Notificación en domicilio, No trabaja ahí, Menor de edad  \r\n\t" +
                                    "                                                 WHEN idContacto IN ( 1103, 1110, 1112, 1114, 1116, 1117, 1118, 1122, 1129 ) THEN 'Sin contacto'  \r\n\t" +
									"					                            --Ilocalizable -- Domicilio no existe, Domicilio no localizado, Cambio domicilio, Direccion insuficiente  \r\n\t" +
                                    "                                                 WHEN idContacto IN ( 1113, 1115, 1127, 1128 ) THEN 'Ilocalizable' \r\n\t" +
                                    "                                                 ELSE NULL \r\n\t" +
                                    "                                            END Contacto , \r\n\t" +
                                    "                                            'R' Base \r\n\t" +
                                    "                                  FROM      dbHistory..GestionesDomiciliarias \r\n\t" +
                                    "                                  WHERE     idCartera = " + idCartera + " \r\n\t" +
                                    "                                ) V \r\n\t" +
                                    "                        GROUP BY V.idCartera , \r\n\t" +
                                    "                                V.idCuenta \r\n\t" +
                                    "                      ) F ON A.idCartera = F.idCartera \r\n\t" +
                                    "                             AND A.Cuenta1 COLLATE DATABASE_DEFAULT = F.idCuenta COLLATE DATABASE_DEFAULT \r\n\t" +
                                    "            LEFT JOIN ( SELECT  V.idCartera , \r\n\t" +
                                    "                                V.idCuenta , \r\n\t" +
                                    "                                COUNT(V.idCuenta) Visitas , \r\n\t" +
                                    "                                MAX(CASE WHEN Contacto = 'Contacto' THEN V.Fecha ELSE NULL END) [Contacto Complemento] , \r\n\t" +
                                    "                                MAX(CASE WHEN Contacto = 'Sin contacto' THEN V.Fecha ELSE NULL END) [Sin Contacto Complemento] , \r\n\t" +
                                    "                                MAX(CASE WHEN Contacto = 'Ilocalizable' THEN V.Fecha ELSE NULL END) [Ilocalizable Complemento] \r\n\t" +
                                    "                        FROM    ( SELECT    idCartera , \r\n\t" +
                                    "                                            idCuenta , \r\n\t" +
                                    "                                            Fecha_Visita , \r\n\t" +
                                    "                                            Segundo_Visita , \r\n\t" +
                                    "                                            CAST(CONVERT(VARCHAR(10), Fecha_Visita) + ' ' + CONVERT(VARCHAR(8), Segundo_Visita) AS DATETIME) Fecha , \r\n\t" +
                                    "                                            CASE  \r\n\t" +
									"					                            --Contacto -- Titular, Le conoce  \r\n\t" +
                                    "                                                 WHEN idContacto IN ( 1101, 1102 ) THEN 'Contacto'  \r\n\t" +
									"					                            --Sin contacto -- No le conoce, No corresponde, Existe domicilio, Desocupado, Sin acceso a domicilio, Zona de riesgo, Notificación en domicilio, No trabaja ahí, Menor de edad  \r\n\t" +
                                    "                                                 WHEN idContacto IN ( 1103, 1110, 1112, 1114, 1116, 1117, 1118, 1122, 1129 ) THEN 'Sin contacto'  \r\n\t" +
									"					                            --Ilocalizable -- Domicilio no existe, Domicilio no localizado, Cambio domicilio, Direccion insuficiente  \r\n\t" +
                                    "                                                 WHEN idContacto IN ( 1113, 1115, 1127, 1128 ) THEN 'Ilocalizable' \r\n\t" +
                                    "                                                 ELSE NULL \r\n\t" +
                                    "                                            END Contacto , \r\n\t" +
                                    "                                            'C' Base \r\n\t" +
                                    "                                  FROM      dbComplemento..GestionesDomiciliarias \r\n\t" +
                                    "                                  WHERE     idCartera = " + idCartera + " \r\n\t" +
                                    "                                ) V \r\n\t" +
                                    "                        GROUP BY V.idCartera , \r\n\t" +
                                    "                                V.idCuenta \r\n\t" +
                                    "                      ) FC ON A.idCartera = FC.idCartera \r\n\t" +
                                    "                              AND A.Cuenta1 COLLATE DATABASE_DEFAULT = FC.idCuenta COLLATE DATABASE_DEFAULT \r\n\t" +
                                    "                        LEFT JOIN ( SELECT  CódigoPostal , \r\n\t" +
                                    "                                            Periferia \r\n\t" +
                                    "                                    FROM    dbEstadistica..CódigosPostales \r\n\t" +
                                    "                                    GROUP BY CódigoPostal , \r\n\t" +
                                    "                                            Periferia \r\n\t" +
                                    "                                  ) CP ON D.CódigoPostal = CP.CódigoPostal \r\n\t" +
                                    "                        LEFT JOIN ( SELECT  DISTINCT idCartera , \r\n\t" +
									"			                        idCuenta , \r\n\t" +
									"			                        CalleNum , \r\n\t" +
									"			                        Colonia , \r\n\t" +
									"			                        Municipio , \r\n\t" +
									"			                        CP \r\n\t" + 
									"	                        FROM    dbCollection..ListaNegraDomicilios  \r\n\t" +
                                    "	                        WHERE  idCartera =" + idCartera + "  \r\n\t" +
                                    "                                  --AND YEAR(FechaListaNegra) = YEAR('" + Fecha.Replace(" ", "") + "') \r\n\t" +
                                    "		                           --AND MONTH(FechaListaNegra) = MONTH('" + Fecha.Replace(" ", "") + "') \r\n\t" +
                                    "                          ) N ON D.idCartera = N.idCartera AND D.Cuenta1 COLLATE DATABASE_DEFAULT = N.idCuenta COLLATE DATABASE_DEFAULT --AND D.Calle = N.CalleNum AND D.ColoniaLocalidad = N.Colonia AND D.DelegaciónMunicipio = N.Municipio AND D.CódigoPostal = N.CP \r\n\t" +
                                    "          WHERE     A.idCartera = " + idCartera + "  \r\n\t" +
                                    "                    AND YEAR(A.Fecha_Asignacion) = YEAR('" + Fecha.Replace(" ", "") + "') \r\n\t" +
                                    "                    AND MONTH(A.Fecha_Asignacion) = MONTH('" + Fecha.Replace(" ", "") + "') \r\n\t" +
                                    "                    AND LEN(D.Latitud) > 0 \r\n\t" +
                                    "                    AND A.Fecha_Retiro IS NULL ) Dom  WHERE Cartera IS NOT NULL \r\n\t" + SCV + Producto + Segmento + Region + Saldo + CorrInc + " ORDER BY Dom.CódigoPostal ASC , Dom.Paquete ASC , Dom.Latitud ASC ";

                    DataBaseConn.SetCommand(SQuery);

                    btnConsulta.Visible = false;
                    picWaitBulk.Visible = true;
                    btnRutas.Visible = false;
                    picWaitRutas.Visible = false;                    

                    DataBaseConn.StartThread(Ruta);
        }

        private void btnRutas_Click(object sender, EventArgs e)
        {
            btnConsulta.Visible = false;
            btnRutas.Visible = false;
            picWaitRutas.Visible = true;
            gbOpciones.Enabled = false;
            dgvGeolocalizacion.Enabled = false;

            Km = int.Parse(cmbKilometros.Text.ToString());

            DataBaseConn.StartThread(DeterminaRuta); 
        }                
        #endregion

        #region Hilos
        protected void LimpiaMensajes()
        {            
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

        private void frmRutas_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        
       

    }
}
