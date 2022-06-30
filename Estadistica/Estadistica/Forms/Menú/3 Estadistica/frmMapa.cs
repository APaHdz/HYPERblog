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
    public partial class frmMapa : Form
    {
        public frmMapa()
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
            Mensaje("Seleccione mes de asignación y cartera.", Color.SlateGray, lblMensajes);

            Ping ping = new Ping();

            PingReply pingStatus = ping.Send(IPAddress.Parse("208.69.34.231"));

            cmbRegion.SelectedIndex = 0;
            cmbSaldo.SelectedIndex = 0;
            cmbCorrInco.SelectedIndex = 0;

            if (pingStatus.Status == IPStatus.Success)
            {
                lblMensajes.Text = "Ping a Google listo, seleccione una opción.";
                gMap.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;                
                GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;                                
                gMap.Position = new PointLatLng(19.384664, -99.148815);
            }
            else
            {
                lblMensajes.Text = "En espera de internet.";
                gbOpciones.Enabled = false;
                return;
            }
        }
        public void PorCP()
        {
            Fecha = dtpMesAsignacion.Value.ToString("yyyy-MM-dd");
            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            idEjecutivo = Ejecutivo.Datos["idEjecutivo"].ToString();
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; " +
                                    " SELECT  Paquete ," +
                                    "        COUNT(Paquete) Direcciones ," +
                                    "        RTRIM(LTRIM(STR(CódigoPostal))) + ' - ' + RTRIM(LTRIM(STR(COUNT(Paquete)))) + ' - ' + Estado + ' - ' + DelegaciónMunicipio Dato" +
                                    " FROM    dbEstadistica.Estadistica.Domicilios" +
                                    " WHERE   idCartera = @idCartera" +
                                    "        AND YEAR(Fecha_Asignacion) = YEAR(@Fecha)" +
                                    "        AND MONTH(Fecha_Asignacion) = MONTH(@Fecha)" +
                                    " GROUP BY Paquete ," +
                                    "        CódigoPostal ," +
                                    "        Estado ," +
                                    "        DelegaciónMunicipio" +
                                    " ORDER BY Paquete DESC");
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
            DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

            DataBaseConn.StartThread(ConsultaPaquetes);
        }
        public void ConsultaPaquetes()
        {
            DataTable tblPaquete = new DataTable();
            if (!DataBaseConn.Fill(tblPaquete,"ConsultaPaquete"))
            {
                Mensaje("Falló al consultar paquetes.", Color.Red, lblMensajes);
                Invoke((Action)delegate()
                {
                    btnCargar.Visible = true;
                    picWaitBulk.Visible = false;
                    dtpMesAsignacion.Enabled = true;
                    cmbCarteras.Enabled = true;
                    cmbPaquetes.Visible = false;
                    chkPaquetes.Visible = false;
                });
                return;
            }

            Invoke((Action)delegate()
                {
                    cmbPaquetes.DataSource = tblPaquete;
                    cmbPaquetes.DisplayMember = "Dato";
                    cmbPaquetes.ValueMember = "Paquete";
                    btnCargar.Visible = true;
                    picWaitBulk.Visible = false;
                    dtpMesAsignacion.Enabled = true;
                    cmbCarteras.Enabled = true;
                    cmbPaquetes.Visible = true;
                    chkPaquetes.Visible = true;
                    Mensaje("Seleccione un paquete.", Color.SlateGray, lblMensajes);
                });
        }

        public void PaqueteBusca()
        {
            tblDireccion = new DataTable();
            if (!DataBaseConn.Fill(tblDireccion, "ConsultaDireccionesPaquete"))
            {                
                Invoke((Action)delegate()
                {
                    btnCargar.Visible = true;
                    picWaitBulk.Visible = false;
                    dtpMesAsignacion.Enabled = true;
                    cmbCarteras.Enabled = true;
                    cmbPaquetes.Enabled = false;
                });
                Mensaje("Falló al consultar paquetes.", Color.Red, lblMensajes);
                return;
            }

            if (tblDireccion.Rows.Count <= 0)
            {
                Paquetes();                       
            }
            else
            {
                Invoke((Action)delegate()
                {
                    btnCargar.Visible = true;
                    picWaitBulk.Visible = false;
                    dtpMesAsignacion.Enabled = true;
                    cmbCarteras.Enabled = true;
                    cmbPaquetes.Enabled = false;
                    
                    chkSCV.Enabled = true;
                    cmbRegion.Enabled = true;
                    chkBProducto.Enabled = true;
                    chkBSegmento.Enabled = true;
                    cmbSaldo.Visible = true;
                    lblLocalidad.Visible = true;
                    lblSaldo.Visible = true;
                    cmbCorrInco.Visible = true;
                    gMap.Enabled = true;
                });
                Mensaje("No existen registros con estos parametros.", Color.DimGray, lblMensajes);
            }
        }
        public void Paquetes()
        {                                         
                int idDomicilio = 0;
                string Direccion = "";
                string Res = "";
                string Marca = "";
                string Producto = "";

                string sSourceCode = "";
                bool ValsourceCode = false;
                string sLatitudDestino = "";
                string sLongitudDestino = "";
                string sDireccion = "";
                string Resultado = "";
                string Estatus = "";

                double dLatitudOrigen = 0;
                double dLongitudOrigen = 0;
                double distancia = 0;
                double KM = 0;

                string Dis = "";
                string Dur = "";

                string KMok = "";
                string MNok = "";

                string DistanciaTiempo = "";

                GMapOverlay polygons = new GMapOverlay("polygons");                            
                List<PointLatLng> points = new List<PointLatLng>();
                int Zoom = 5;
                for (int i = 0; i <= tblDireccion.Rows.Count; i++)
                {
                    if (Zoom == 20)
                        Zoom = 5;

                    System.Threading.Thread.Sleep(1000);
                    Mensaje(i + 1 + " de " + tblDireccion.Rows.Count.ToString(), Color.DimGray, lblMensajes);

                    //idDomicilio = int.Parse(tblDireccion.Rows[i]["idDomicilio"].ToString());
                    //Direccion = tblDireccion.Rows[i]["Direccion"].ToString().Replace("#","Ñ").ToUpper();
                    //Direccion = Direccion.Replace("EDIF", "").Replace("EDIFICIO", "").Replace("DEPTO", "").Replace("DEPARTAMENTO", "").Replace("CDA","").Replace("MZ","").Replace("LT","");
                    //Direccion = Direccion.Replace("  ", " ");
                    //Res = tblDireccion.Rows[i]["Resultado"].ToString().Trim();
                    //Marca = tblDireccion.Rows[i]["Marca_Especial"].ToString().Trim();
                    //Estatus = tblDireccion.Rows[i]["Estatus"].ToString().Trim();
                    //Producto = tblDireccion.Rows[i]["Producto"].ToString().Trim();

                    //string urlbusqueda = "https://maps.google.com/maps/api/geocode/xml?address=" + Direccion + "&key=AIzaSyDQa2s-GksYOHS8d-OxvgEz1M579zwwPC0";
                    string urlbusqueda = "https://maps.google.com/maps/api/geocode/xml?address=" + "andalucia 245 colonia alamos benito juarez" + "&key=AIzaSyDQa2s-GksYOHS8d-OxvgEz1M579zwwPC0";
                    //string urlbusqueda = "https://maps.google.com/maps/api/geocode/xml?address=" + Direccion + "&key=AIzaSyAh1lbZZK-ylXwN-LQR9JLFGGwDnOoCuwk";
                    
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlbusqueda);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        StreamReader sr = new StreamReader(response.GetResponseStream());
                        sSourceCode = sr.ReadToEnd();
                        sr.Close();
                        response.Close();
                        ValsourceCode = true;
                    }
                    catch (Exception ex)
                    {
                        ValsourceCode = false;
                        this.Invoke((Action)delegate { Mensaje("Error: " + ex.ToString(), Color.Red, lblMensajes); });                        
                    }

                    if (ValsourceCode == true)
                    {
                        sLatitudDestino = "";
                        sLongitudDestino = "";
                        sDireccion = "";
                        distancia = 0;
                        DistanciaTiempo = "";
                        KM = 0;

                        if (sSourceCode.LastIndexOf("OVER_QUERY_LIMIT") > 0)
                        {
                            Resultado = "Excediste tu límite de búsqueda.";
                            this.Invoke((Action)delegate { Mensaje("Excediste tu límite de búsqueda.", Color.Red, lblMensajes); });
                        }

                        if (sSourceCode.LastIndexOf("REQUEST_DENIED") > 0)
                        {
                            Resultado = "Tu solicitud fue rechazada.";
                            this.Invoke((Action)delegate { Mensaje("Tu solicitud fue rechazada.", Color.Red, lblMensajes); });
                        }

                        if (sSourceCode.LastIndexOf("INVALID_REQUEST") > 0)
                        {
                            Resultado = "La consulta resulta incompleta.";
                            this.Invoke((Action)delegate { Mensaje("La consulta resulta incompleta.", Color.Red, lblMensajes); });
                        }

                        if (sSourceCode.LastIndexOf("UNKNOWN_ERROR") > 0)
                        {
                            Resultado = "Error al procesar solicitud al servidor de google maps.";
                            this.Invoke((Action)delegate { Mensaje("Error al procesar solicitud al servidor de google maps.", Color.Red, lblMensajes); });
                        }

                        if (sSourceCode.LastIndexOf("ZERO_RESULTS") > 0)
                        {
                            Resultado = "No existe resultado para su búsqueda.";
                            this.Invoke((Action)delegate { Mensaje("No existe resultado para su búsqueda.", Color.Red, lblMensajes); });
                        }

                        if (sSourceCode.LastIndexOf("OK") > 0)
                        {
                            int idxLatitudIni = sSourceCode.IndexOf("<lat>");
                            int idxLatitudFin = sSourceCode.IndexOf("</lat>");

                            int idxLongitudIni = sSourceCode.IndexOf("<lng>");
                            int idxLongitudFin = sSourceCode.IndexOf("</lng>");

                            int idxDireccionIni = sSourceCode.IndexOf("<formatted_address>");
                            int idxDireccionFin = sSourceCode.IndexOf("</formatted_address>");

                            sLatitudDestino = sSourceCode.Substring(idxLatitudIni, idxLatitudFin - idxLatitudIni).ToString();
                            sLatitudDestino = sLatitudDestino.ToString().Replace("<lat>", "").Trim();

                            sLongitudDestino = sSourceCode.Substring(idxLongitudIni, idxLongitudFin - idxLongitudIni).ToString();
                            sLongitudDestino = sLongitudDestino.ToString().Replace("<lng>", "").Trim();

                            sDireccion = sSourceCode.Substring(idxDireccionIni, idxDireccionFin - idxDireccionIni).ToString();
                            sDireccion = sDireccion.ToString().Replace("<formatted_address>", "").Trim();

                            if (i > 0)
                            {                                
                                string urlbusquedaDistancia = "https://maps.googleapis.com/maps/api/directions/json?origin=" + dLatitudOrigen.ToString() + "," + dLongitudOrigen.ToString() + "&destination=" + sLatitudDestino.ToString() + "," + sLongitudDestino.ToString() + "&key=AIzaSyCuLvKJRqYn92rmDmRGzsJKDWQgWprxb38";
                                //string urlbusquedaDistancia = "https://maps.googleapis.com/maps/api/directions/json?origin=" + dLatitudOrigen.ToString() + "," + dLongitudOrigen.ToString() + "&destination=" + sLatitudDestino.ToString() + "," + sLongitudDestino.ToString() + "&key=AIzaSyAh1lbZZK-ylXwN-LQR9JLFGGwDnOoCuwk";
                                try
                                {                                    
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlbusquedaDistancia);
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    StreamReader sr = new StreamReader(response.GetResponseStream());
                                    string sDisTiem = sr.ReadToEnd();
                                    sDisTiem = sDisTiem.Trim().Replace("\"", "").Replace(" ","").Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                    sr.Close();
                                    response.Close();

                                    if (sDisTiem.LastIndexOf("OK") > 0)
                                    {
                                        Dis = "";
                                        Dur = "";
                                        int idxDisIni = sDisTiem.IndexOf("distance:{");
                                        int idxDisFin = sDisTiem.IndexOf("duration:{");

                                        int idxDurIni = sDisTiem.IndexOf("duration:{");
                                        int idxDurFin = sDisTiem.IndexOf("end_address");

                                        Dis = sDisTiem.Substring(idxDisIni, idxDisFin - idxDisIni).ToString();
                                        Dis = Dis.ToString().Replace("{", "").Replace("}", "").Replace(" ", "").Trim();
                                        int idxKMini = Dis.IndexOf("distance:");
                                        int idxKMFin = Dis.IndexOf("value");
                                        KMok = Dis.Substring(idxKMini, idxKMFin - idxKMini).ToString();
                                        KMok = KMok.ToString().Replace("distance:text:", "").Replace(",", "").Replace(" ", "").Trim();
                                        KMok = KMok.ToLower();

                                        Dur = sDisTiem.Substring(idxDurIni, idxDurFin - idxDurIni).ToString();
                                        Dur = Dur.ToString().Replace("{", "").Replace("}", "").Replace(" ", "").Trim();
                                        int idxMNini = Dur.IndexOf("duration:");
                                        int idxMNFin = Dur.IndexOf("value");
                                        MNok = Dur.Substring(idxMNini, idxMNFin - idxMNini).ToString();
                                        MNok = MNok.ToString().Replace("duration:text:", "").Replace(",", "").Replace(" ", "").Trim();

                                        string x = "";
                                        x= KMok.Replace("a", "").Replace("b", "").Replace("c", "").Replace("d", "").Replace("e", "").Replace("f", "").Replace("g", "").Replace("h", "").Replace("i", "").Replace("j", "").Replace("k", "").Replace("l", "").Replace("m", "").Replace("n", "").Replace("ñ", "").Replace("o", "").Replace("p", "").Replace("q", "").Replace("r", "").Replace("s", "").Replace("t", "").Replace("u", "").Replace("v", "").Replace("w", "").Replace("x", "").Replace("y", "").Replace("z", "");

                                        DistanciaTiempo = "Distancia: " + KMok.ToString() + " Tiempo: " + MNok.ToString();

                                        if (KMok.Contains("mi"))
                                            KM = 10;
                                        else if (KMok.Contains("k"))
                                            KM = Convert.ToDouble(x);
                                        else
                                            KM = 0.9;


                                        
                                    }
                                    else
                                    {
                                        KM = 10;
                                    }
                                }
                                catch (Exception ex)
                                {                                    
                                }
                            }                            
                            Resultado = "Geocódigo correcto.";

                            //if (i == 0)
                            //{
                                dLatitudOrigen = Convert.ToDouble(sLatitudDestino);
                                dLongitudOrigen = Convert.ToDouble(sLongitudDestino);
                            //}                                                            

                            GMapOverlay markersOverlay = new GMapOverlay("markers");

                            if(Producto=="Auto")
                            {
                                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.green_small);
                                markersOverlay.Markers.Add(marker);
                                gMap.Overlays.Add(markersOverlay);
                                marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            }
                            else if(Producto=="Consumo")
                            {
                                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.yellow_small);
                                markersOverlay.Markers.Add(marker);
                                gMap.Overlays.Add(markersOverlay);
                                marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            }
                            else if (Producto=="Tarjeta")
                            {
                                GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.blue_small);
                                markersOverlay.Markers.Add(marker);
                                gMap.Overlays.Add(markersOverlay);
                                marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            }

                            //if (i == 0 && idDomicilio==1)
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.black_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (Estatus == "BAJA MORA")
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.orange);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (Estatus=="MORA CERO")
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.white_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (Estatus == "PAGO PARCIAL")
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.purple_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i == 0 && Marca == "SCV")
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.green);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i == 0 && Marca != "SCV")
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.blue);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i >= 1 && Marca == "SCV")
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.green_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}                            
                            //else if (i >= 1 && Marca != "SCV" && KM < 1)
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.blue_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i >= 1 && KM >= 1 && KM < 2)
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.yellow_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i >= 1 && KM > 2 && KM < 3)
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.gray_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i >= 1 && KM > 3 && KM < 5)
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.brown_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                            //else if (i >= 1 && KM > 5)
                            //{
                            //    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)), GMarkerGoogleType.red_small);
                            //    markersOverlay.Markers.Add(marker);
                            //    gMap.Overlays.Add(markersOverlay);
                            //    marker.ToolTipText = Direccion + Environment.NewLine + sDireccion + Environment.NewLine + "Latitud: " + sLatitudDestino + Environment.NewLine + "Longitud: " + sLongitudDestino + Environment.NewLine + DistanciaTiempo;
                            //}
                                                        
                            points.Add(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)));

                            this.Invoke((Action)delegate { gMap.Position = new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)); });
                            //this.Invoke((Action)delegate { gMap.Zoom = 15; gMap.Position = new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)); });

                            

                            //if (KM <= 2 && i>1)
                            //{
                            //    //GMapOverlay polyOverlay = new GMapOverlay("polygons");
                            //    //points.Add(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)));
                            //    //GMapPolygon polygon = new GMapPolygon(points, "mypolygon");
                            //    //polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                            //    //polygon.Stroke = new Pen(Color.Red, 1);
                            //    //polyOverlay.Polygons.Add(polygon);
                            //    //gMap.Overlays.Add(polyOverlay);

                            //    //polygon.Fill = new SolidBrush(Color.FromArgb(50, Color.Red));
                            //    //polygon.Stroke = new Pen(Color.Red, 1);


                            //    //GMapOverlay routes = new GMapOverlay("routes");
                            //    //points.Add(new PointLatLng(Convert.ToDouble(sLatitudDestino), Convert.ToDouble(sLongitudDestino)));
                            //    //GMapRoute route = new GMapRoute(points, "Ruta");
                            //    ////GMapRoute route = new GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(points, false, false, 15);
                            //    //route.Stroke = new Pen(Color.Blue, 3);
                            //    //routes.Routes.Add(route);
                            //    //gMap.Overlays.Add(routes);
                            //}
                            


                            //if (Res != "Geocódigo correcto.")
                            //{
                            //    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
                            //                          "UPDATE dbEstadistica.[Estadistica].[Domicilios] SET \r\n\t" +
                            //                          "Latitud = '" + sLatitudDestino + "',  \r\n\t" +
                            //                          "Longitud = '" + sLongitudDestino + "',  \r\n\t" +
                            //                          "Resultado = '" + Resultado + "',  \r\n\t" +
                            //                          "Informacion = '" + sDireccion.Replace("'","") + "', \r\n" +
                            //                          "idEjecutivo = " + idEjecutivo + " \r\n" +
                            //                          "WHERE idDomicilio = " + idDomicilio);

                            //    Mensaje("Actualizando información, favor espere unos segundos...", System.Drawing.Color.SlateGray, lblMensajes);
                            //    DataBaseConn.StartThread(ActualizandoInformacion);
                            //}                            
                            //this.Invoke((Action)delegate { Mensaje("Geocódigo correcto.", Color.DimGray, lblMensajes); });
                        }
                    }
                    if (Res != "Geocódigo correcto.")
                    {
                        DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
                                              "UPDATE dbEstadistica.[Estadistica].[Domicilios] SET \r\n\t" +
                                              "Latitud = '" + sLatitudDestino + "',  \r\n\t" +
                                              "Longitud = '" + sLongitudDestino + "',  \r\n\t" +
                                              "Resultado = '" + Resultado + "',  \r\n\t" +
                                              "Informacion = '" + sDireccion.Replace("'", "") + "', \r\n" +
                                              "idEjecutivo = " + idEjecutivo + " \r\n" +
                                              "WHERE idDomicilio = " + idDomicilio);

                        Mensaje("Actualizando información, favor espere unos segundos...", System.Drawing.Color.SlateGray, lblMensajes);
                        DataBaseConn.StartThread(ActualizandoInformacion);
                    }
                    Zoom = Zoom + 1;
                }                

                Invoke((Action)delegate
                {
                    if (chkPaquetes.Checked == true)
                    {
                        btnCargar.Visible = true;
                        picWaitBulk.Visible = false;
                        dtpMesAsignacion.Enabled = true;
                        cmbCarteras.Enabled = true;
                        chkPaquetes.Enabled = true;
                        cmbPaquetes.Enabled = true;                        
                        gMap.Enabled = true;
                    }
                    else
                    {
                        btnCargar.Visible = true;
                        picWaitBulk.Visible = false;
                        dtpMesAsignacion.Enabled = true;
                        cmbCarteras.Enabled = true;
                        chkSCV.Enabled = true;
                        cmbRegion.Enabled = true;
                        chkBProducto.Enabled = true;
                        chkBSegmento.Enabled = true;
                        cmbSaldo.Visible = true;
                        lblLocalidad.Visible = true;
                        lblSaldo.Visible = true;
                        cmbCorrInco.Visible = true;
                        gMap.Enabled = true;
                    }
                    Mensaje("El proceso terminó.", System.Drawing.Color.SlateGray, lblMensajes);
                });     
                
        }   
        public void ActualizandoInformacion()
        {
            if (!DataBaseConn.Execute("ActualizandoInformacion"))            
                Mensaje("Falló al actualizar información.", System.Drawing.Color.Crimson, lblMensajes);
        }

        public void LimpiaParámetros()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate()
                {                    
                    btnCargar.Visible = true;
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

            gMap.Enabled = false;

            if (chkPaquetes.Checked == true)
            {
                btnCargar.Visible = false;
                picWaitBulk.Visible = true;
                dtpMesAsignacion.Enabled = false;
                cmbCarteras.Enabled = false;
                chkPaquetes.Enabled = false;
                cmbPaquetes.Enabled = false;                

                if (cmbPaquetes.Visible == true)
                {
                    Paquete = Convert.ToString(cmbPaquetes.SelectedValue);
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n\t" +
                                            "UPDATE dbEstadistica.Estadistica.Domicilios SET Latitud=NULL, Longitud=NULL, Resultado='Geocódigo incorrecto' WHERE Informacion NOT LIKE '%Mex%' AND Resultado='Geocódigo correcto.' \r\n\t" +
                                            "SELECT  idDomicilio , \r\n\t" +
                                            "D.Resultado , \r\n\t" +
                                            "A.Marca_Especial , \r\n\t" +
                                            "P.Estatus , \r\n\t" +
                                            "R.Producto , \r\n\t" +
                                            "REPLACE(REPLACE(REPLACE(DelegaciónMunicipio, ' ', '<>'), '><', ''), '<>', ' ') + ', ' + " +
                                            "REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') + ', ' + " +
                                            "REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Calle, ' ', '<>'), '><', ''), '<>', ' '), 'NUM.EXT.', ''), 'NUM.INT.', '') + ', ' + " +
                                            "CódigoPostal Direccion \r\n\t" +
                                            "FROM    dbEstadistica.Estadistica.Domicilios D \r\n\t" +
                                            "INNER JOIN dbEstadistica.Estadistica.Asignaciones A ON D.Fecha_Asignacion = A.Fecha_Asignacion \r\n\t" +
                                            "                                                       AND D.idCartera = A.idCartera \r\n\t" +
                                            "                                                       AND D.idProducto = A.idProducto \r\n\t" +
                                            "                                                       AND D.Cuenta1 = A.Cuenta1 \r\n\t" +
                                            "LEFT JOIN ( SELECT  idCuenta , \r\n\t" +
                                            "                    idCartera , \r\n\t" +
                                            "                    MAX(Fecha_Pago) Fecha , \r\n\t" +
                                            "                    MAX(Status) Estatus \r\n\t" +
                                            "            FROM    dbEstadistica.Estadistica.Pagos \r\n\t" +
                                            "            WHERE   idCartera = @idCartera \r\n\t" +
                                            "                    AND YEAR(Fecha_Pago) = YEAR(@Fecha) \r\n\t" +
                                            "                    AND MONTH(Fecha_Pago) = MONTH(@Fecha) \r\n\t" +
                                            "            GROUP BY idCuenta , \r\n\t" +
                                            "                    idCartera \r\n\t" +
                                            "          ) P ON A.idCartera = P.idCartera \r\n\t" +
                                            "                 AND A.Cuenta1 = P.idCuenta \r\n\t" +
                                            "                 AND YEAR(Fecha) = YEAR(@Fecha) \r\n\t" +
                                            "                 AND MONTH(Fecha) = MONTH(@Fecha) \r\n\t" +
                                            "INNER JOIN dbHistory..Productos R ON A.idProducto = R.idProducto \r\n\t" +
                                            "WHERE   D.idCartera = @idCartera \r\n\t" +
                                            "AND YEAR(D.Fecha_Asignacion) = YEAR(@Fecha) \r\n\t" +
                                            "AND MONTH(D.Fecha_Asignacion) = MONTH(@Fecha) \r\n\t" +
                                            "AND D.CódigoPostal IS NOT NULL \r\n\t" +
                                            "AND D.Paquete = " + Paquete +
                                            " ORDER BY D.Paquete ASC , " +
                                            "Marca_Especial DESC, EstatusDomicilioVisita ASC, Direccion ASC");
                    DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
                    DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);

                    btnCargar.Visible = false;
                    picWaitBulk.Visible = true;
                    dtpMesAsignacion.Enabled = false;
                    cmbCarteras.Enabled = false;

                    DataBaseConn.StartThread(PaqueteBusca);
                }                   
            }
            else
            {
                btnCargar.Visible = false;
                picWaitBulk.Visible = true;
                dtpMesAsignacion.Enabled = false;
                cmbCarteras.Enabled = false;
                chkSCV.Enabled = false;
                cmbRegion.Enabled = false;
                chkBProducto.Enabled = false;
                chkBSegmento.Enabled = false;

                if (chkSCV.Visible == true)
                {
                    Paquete = Convert.ToString(cmbPaquetes.SelectedValue);

                    string SCV = "";
                    string Region = "";
                    string Prod = "";
                    string Producto = "";
                    string Segmento = "";
                    string Segm = "";
                    string Saldo = "";
                    string CorrInc = "";

                    if(chkSCV.Checked==true)
                        SCV = " AND Dom.Marca_Especial = 'SCV'";

                    if (cmbRegion.Text == "*")
                        Region = " ";
                    else if (cmbRegion.Text == "CDMX")
                        Region = " AND Dom.Paquete = 'Distrito Federal'";
                    else 
                        Region = " AND CASE WHEN Dom.Paquete = 'Distrito Federal' THEN DelegaciónMunicipio ELSE Dom.Estado END = '" + cmbRegion.Text + "'";

                    if (cmbCorrInco.Text != "*")
                        CorrInc = " AND EstatusDomicilioVisita = '" + cmbCorrInco.Text + "'" ;
                    
                    for (int x = 0; x <= chkBProducto.CheckedItems.Count - 1; x++)
                    {
                        Prod = Prod + "'" + chkBProducto.CheckedItems[x].ToString() + "' ,";
                    }
                    if (Prod.Length > 0)
                    {
                        Prod = Prod.Substring(0, Prod.Length - 1);
                        Producto = " AND P.Producto IN (" + Prod + ")";
                    }

                    for (int x = 0; x <= chkBSegmento.CheckedItems.Count - 1; x++)
                    {
                        Segm = Segm + "'" + chkBSegmento.CheckedItems[x].ToString() + "' ,";
                    }
                    if (Segm.Length > 0)
                    {
                        Segm = Segm.Substring(0, Segm.Length - 1);
                        Segmento = " AND Dom.Segmento IN (" + Segm + ")";
                    }

                    if(cmbSaldo.Text=="< 10,000"){
                        Saldo = " AND Dom.Saldo < 10000"; }
                    else if (cmbSaldo.Text == "> 10,000 y < 50,000"){
                        Saldo = " AND Dom.Saldo BETWEEN 10000 AND 49999.99";}
                    else if (cmbSaldo.Text == "> 50,000 y < 100,000"){
                        Saldo = " AND Dom.Saldo BETWEEN 50000 AND 100000.99";}
                    else if (cmbSaldo.Text == "> 100,000 y < 200,000"){
                        Saldo = " AND Dom.Saldo BETWEEN 100000 AND 200000.99";}
                    else if (cmbSaldo.Text == "> 200,000 y < 300,000"){
                        Saldo = " AND Dom.Saldo BETWEEN 200000 AND 300000.99";}
                    else if (cmbSaldo.Text == "> 300,000 y < 400,000"){
                        Saldo = " AND Dom.Saldo BETWEEN 300000 AND 400000.99";}
                    else if (cmbSaldo.Text == "> 400,000 y < 500,000"){
                        Saldo = " AND Dom.Saldo BETWEEN 400000 AND 500000.99";}
                    else if (cmbSaldo.Text == "> 500,000"){
                        Saldo = " AND Dom.Saldo > 500000";}
                    else{
                        Saldo = "";}

                    string SQuery = "SET TRAN ISOLATION LEVEL READ UNCOMMITTED  \r\n\t" +
                                    "WAITFOR DELAY '00:00:00';  \r\n\t" +
                                    "UPDATE dbEstadistica.Estadistica.Domicilios SET Latitud=NULL, Longitud=NULL, Resultado='Geocódigo incorrecto' WHERE Informacion NOT LIKE '%Mex%' AND Resultado='Geocódigo correcto.' \r\n\t" +
                                    "SELECT  Dom.idDomicilio ,  \r\n\t" +
                                    "        Dom.idProducto ,  \r\n\t" +
                                    "        P.Producto ,  \r\n\t" +
                                    "        Dom.Segmento ,  \r\n\t" +
                                    "        Dom.Resultado ,  \r\n\t" +
                                    "        Dom.Latitud ,  \r\n\t" +
                                    "        Dom.Longitud ,  \r\n\t" +
                                    "        Dom.CódigoPostal ,  \r\n\t" +
                                    "        Dom.Estatus ,  \r\n\t" +
                                    "        Dom.Saldo ,  \r\n\t" +
                                    "        Dom.Marca_Especial ,  \r\n\t" +
                                    "        CASE WHEN Dom.Paquete = 'Distrito Federal' THEN DelegaciónMunicipio ELSE Paquete END Paquete ,  \r\n\t" +
                                    "        Dom.EstatusDomicilioVisita ,  \r\n\t" +
                                    "        Calle + ', ' + DelegaciónMunicipio + ', ' + Paquete + ', ' + CódigoPostal Direccion  \r\n\t" +
                                    "FROM    ( SELECT    idDomicilio ,  \r\n\t" +
                                    "                    A.Cuenta1 ,  \r\n\t" +
                                    "                    A.idProducto ,  \r\n\t" +
                                    "                    A.Segmento ,  \r\n\t" +
                                    "                    D.Resultado ,  \r\n\t" +
                                    "                    P.Estatus ,  \r\n\t" +
                                    "                    A.Saldo ,  \r\n\t" +
                                    "                    A.Marca_Especial ,  \r\n\t" +
                                    "                    D.EstatusDomicilioVisita ,  \r\n\t" +
                                    "                    D.Latitud ,  \r\n\t" +
                                    "                    D.Longitud ,  \r\n\t" +
                                    "                    CódigoPostal ,  \r\n\t" +
                                    "                    REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(Calle, ' ', '<>'), '><', ''), '<>', ' '), 'NUM.EXT.', ''), 'NUM.INT.', ''),'+','Ñ'),'#','Ñ'),'¿','Ñ'),'.',''),'\"',''),'=',''),'-','') Calle ,  \r\n\t" +
                                    "                    REPLACE(REPLACE(REPLACE(DelegaciónMunicipio, ' ', '<>'), '><', ''), '<>', ' ') DelegaciónMunicipio ,  \r\n\t" +
                                    "                    REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') Estado ,  \r\n\t" +
                                    "                    CASE WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'AG' THEN 'Aguascalientes'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'BN' THEN 'Baja California'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'BS' THEN 'Baja California Sur'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CA' THEN 'Campeche'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CH' THEN 'Chihuahua'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CO' THEN 'Colima'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CS' THEN 'Chiapas'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'CU' THEN 'Coahuila'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'COAHUILA DE ZARAGOZA' THEN 'Coahuila'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'DF' THEN 'Distrito Federal'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'DISTRITO FEDERAL' THEN 'Distrito Federal'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'DU' THEN 'Durango'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'EM' THEN 'México'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MEXICO' THEN 'México'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'GO' THEN 'Guerrero'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'GU' THEN 'Guanajuato'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'HI' THEN 'Hidalgo'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'HIDALGO' THEN 'Hidalgo'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'JA' THEN 'Jalisco'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MI' THEN 'Michoacán'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MICHOACAN DE OCAMPO' THEN 'Michoacán'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'MO' THEN 'Morelos'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'NA' THEN 'Nayarit'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'NL' THEN 'Nuevo León'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'OA' THEN 'Oaxaca'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'PU' THEN 'Puebla'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'QR' THEN 'Quintana Roo'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'QU' THEN 'Querétaro'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'QUERETARO' THEN 'Querétaro'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'SI' THEN 'Sinaloa'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'SL' THEN 'San Luis Potosí'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'SO' THEN 'Sonora'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'TA' THEN 'Tabasco'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'TL' THEN 'Tlaxcala'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'TM' THEN 'Tamaulipas'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'VE' THEN 'Veracruz'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'VERACRUZ DE IGNACIO' THEN 'Veracruz'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'YU' THEN 'Yucatán'  \r\n\t" +
                                    "                         WHEN REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ') = 'ZA' THEN 'Zacatecas'  \r\n\t" +
                                    "                         ELSE REPLACE(REPLACE(REPLACE(Estado, ' ', '<>'), '><', ''), '<>', ' ')  \r\n\t" +
                                    "                    END Paquete  \r\n\t" +
                                    "          FROM      dbEstadistica.Estadistica.Domicilios D  \r\n\t" +
                                    "                    INNER JOIN dbEstadistica.Estadistica.Asignaciones A ON D.Fecha_Asignacion = A.Fecha_Asignacion  \r\n\t" +
                                    "                                                                           AND D.idCartera = A.idCartera  \r\n\t" +
                                    "                                                                           AND D.idProducto = A.idProducto  \r\n\t" +
                                    "                                                                           AND D.Cuenta1 = A.Cuenta1  \r\n\t" +
                                    "                    LEFT JOIN (  \r\n\t" +
                                    "                                SELECT  idCuenta ,  \r\n\t" +
                                    "	                                    idCartera ,  \r\n\t" +
                                    "	                                    MAX(Fecha_Pago) Fecha ,  \r\n\t" +
                                    "	                                    MAX(Status) Estatus  \r\n\t" +
                                    "                                FROM    dbEstadistica.Estadistica.Pagos  \r\n\t" +
                                    "                                WHERE   idCartera = " + idCartera + "  \r\n\t" +
                                    "	                                    AND YEAR(Fecha_Pago) = YEAR('" + Fecha.Replace(" ", "") + "')  \r\n\t" +
                                    "	                                    AND MONTH(Fecha_Pago) = MONTH('" + Fecha.Replace(" ", "") + "')  \r\n\t" +
                                    "                                GROUP BY idCuenta , idCartera  \r\n\t" +
                                    "                    ) P ON A.idCartera = P.idCartera AND A.Cuenta1 = P.idCuenta AND YEAR(Fecha) = YEAR(A.Fecha_Asignacion) AND MONTH(Fecha) = MONTH(A.Fecha_Asignacion)  \r\n\t" +
                                    "          WHERE     D.idCartera = " + idCartera + "  \r\n\t" +
                                    "                    AND YEAR(D.Fecha_Asignacion) = YEAR('" + Fecha.Replace(" ", "") + "')  \r\n\t" +
                                    "                    AND MONTH(D.Fecha_Asignacion) = MONTH('" + Fecha.Replace(" ", "") + "')  \r\n\t" +
                                    "                    AND D.CódigoPostal IS NOT NULL					  \r\n\t" +
                                    "        ) Dom  \r\n\t" +
                                    "        INNER JOIN dbHistory..Productos P ON Dom.idProducto = P.idProducto  \r\n\t" +
                                    "WHERE P.idProducto IS NOT NULL AND (LEN(Latitud)<=0 OR Latitud IS NULL) " + SCV + Producto + Segmento + Region + Saldo + CorrInc + " ORDER BY Dom.CódigoPostal ASC, Dom.Latitud ASC";

                    DataBaseConn.SetCommand(SQuery);                    

                    btnCargar.Visible = false;
                    picWaitBulk.Visible = true;
                    dtpMesAsignacion.Enabled = false;
                    cmbCarteras.Enabled = false;
                    //cmbPaquetes.Enabled = false;

                    DataBaseConn.StartThread(PaqueteBusca);
                }
            }

        }

        private void chkPaquetes_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPaquetes.Checked == true)
            {
                PorCP();
                cmbPaquetes.Visible = true;
                chkSCV.Visible = false;
                cmbRegion.Visible = false;
                chkBProducto.Visible = false;
                chkBSegmento.Visible = false;
                cmbSaldo.Visible = false;
                lblLocalidad.Visible = false;
                lblSaldo.Visible = false;
                cmbCorrInco.Visible = false;
                Proceso = 0;

            }
            else
            {
                cmbRegion.SelectedIndex = 0;
                cmbPaquetes.Visible = false;
                chkSCV.Visible = true;
                cmbRegion.Visible = true;
                chkBProducto.Visible = true;
                chkBSegmento.Visible = true;
                cmbSaldo.Visible = true;
                lblLocalidad.Visible = true;
                lblSaldo.Visible = true;
                cmbCorrInco.Visible = true;
                Proceso = 1;
            }
        }

        private void gMap_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string sRuta = "";
                if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                    Mensaje("Mapa no exportado.", System.Drawing.Color.SlateGray, lblMensajes);
                else
                {
                    sRuta = sfdExcel.FileName;
                    var _tmpImage = gMap.ToImage();
                    if (_tmpImage == null) return;
                    _tmpImage.Save(sRuta);
                }
            }
            catch { }
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

        private void frmMapa_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
        }


    }
}
