using System;
using System.Data;
using System.Windows.Forms;

namespace Estadistica
{
    public partial class frmMenú : Form
    {
        public frmMenú()
        {
            InitializeComponent();

            lblVersión.Text = "z" + DataBaseConn.ServerNum + "\r\nv" + DataBaseConn.AppVersion;
        }

        private void frmMenú_Shown(object sender, EventArgs e)
        {
            //if (Ejecutivo.Datos["idEjecutivo"].ToString() != "7745")
            //{
            //    int jerarquia = Convert.ToInt32(Ejecutivo.Datos["Jerarquía"]);
            //    if (jerarquia < 0)
            //    {
            //        MessageBox.Show("Pertenece a una área que no tiene permisos para utilizar esta aplicación.", "Acceso a Procesos Cj");
            //        Application.Exit();
            //    }

            //    //sToolStripMenuItem.Visible = false;
            //}
        }

        //Clientes
        //CityBanamex
        private void summaryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void remesaPymeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmRemesaPyme formRemesaPyme = new frmRemesaPyme())
                formRemesaPyme.ShowDialog(this);
        }
        private void asistenciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAsistencia formAsistencia = new frmAsistencia())
                formAsistencia.ShowDialog(this);
        }
        private void summaryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (frmSummary formSummary = new frmSummary())
                formSummary.ShowDialog(this);
        }
        private void negociacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmNegociaciones formNegociaciones = new frmNegociaciones())
                formNegociaciones.ShowDialog(this);
        }

        //Bancomer
        private void ireneToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void iVRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmIVR formIVR = new frmIVR())
                formIVR.ShowDialog(this);
        }
        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmIrene formIrene = new frmIrene())
                formIrene.ShowDialog(this);
        }
        private void avancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAvances formAvances = new frmAvances())
                formAvances.ShowDialog(this);
        }
        private void cuentasÚnicasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmIreneCuentas formIreneCuentas = new frmIreneCuentas())
                formIreneCuentas.ShowDialog(this);
        }

        private void cuentasIVRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmIreneIVR formIreneIVR = new frmIreneIVR())
                formIreneIVR.ShowDialog(this);
        }
        private void carteoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmBBVACarteo formBBVACarteo = new frmBBVACarteo())
                formBBVACarteo.ShowDialog(this);
        }
        private void visitasToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmVisitasMapeo formVisitasMapeo = new frmVisitasMapeo())
                formVisitasMapeo.ShowDialog(this);
        }
        private void respuestaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmVinculacionRespuesta formVinculacionRespuesta = new frmVinculacionRespuesta())
                formVinculacionRespuesta.ShowDialog(this);
        }
        private void metasSCVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMetasSCV formMetasSCV = new frmMetasSCV())
                formMetasSCV.ShowDialog(this);
        }
        //Nissan
        private void centrosAcopioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCentroAcopio formCentroAcopio = new frmCentroAcopio())
                formCentroAcopio.ShowDialog(this);
        }
        private void dacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDaciones formDaciones = new frmDaciones())
                formDaciones.ShowDialog(this);
        }
        //Volkswagen
        private void conciliaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmConciliaciónLeasing formConciliaciónLeasing = new frmConciliaciónLeasing())
                formConciliaciónLeasing.ShowDialog(this);
        }
        private void pagosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmPagosLeasing formPagosLeasing = new frmPagosLeasing())
                formPagosLeasing.ShowDialog(this);
        }
        private void conciliaciónPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmReporteConciliacion formReporteConciliacion = new frmReporteConciliacion())
                formReporteConciliacion.ShowDialog(this);
        }
        private void barridoPagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmBarridoPagos formBarridoPagos = new frmBarridoPagos())
                formBarridoPagos.ShowDialog(this);
        }
        private void conciliaciónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmConciliaciónPrelegal formConciliaciónPrelegal = new frmConciliaciónPrelegal())
                formConciliaciónPrelegal.ShowDialog(this);
        }
        private void barridoPagosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmBarridoPagosPrelegal formBarridoPagosPrelegal = new frmBarridoPagosPrelegal())
                formBarridoPagosPrelegal.ShowDialog(this);
        }
        private void pagosToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            using (frmPagosPrelegal formPagosPrelegal = new frmPagosPrelegal())
                formPagosPrelegal.ShowDialog(this);
        }
        private void conciliaciónPagosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmReporteConciliacionPrelegal formReporteConciliacionPrelegal = new frmReporteConciliacionPrelegal())
                formReporteConciliacionPrelegal.ShowDialog(this);
        }
        private void segurosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmSegurosLeasing formSegurosLeasing = new frmSegurosLeasing())
                formSegurosLeasing.ShowDialog(this);
        }

        private void fBL5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmFBL5Leasing formFBL5Leasing = new frmFBL5Leasing())
                formFBL5Leasing.ShowDialog(this);
        }
        private void conciliacionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmConciliaciónTelBank formConciliaciónTelBank = new frmConciliaciónTelBank())
                formConciliaciónTelBank.ShowDialog(this);
        }
        private void pagosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmPagosTelBank formPagosTelBank = new frmPagosTelBank())
                formPagosTelBank.ShowDialog(this);
        }
        private void conciliaciónPagosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (frmReporteConciliacionTelBank formReporteConciliacionTelBank = new frmReporteConciliacionTelBank())
                formReporteConciliacionTelBank.ShowDialog(this);
        }

        //Libertad
        private void actualizaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAsignaciónLibertad formAsignaciónLibertad = new frmAsignaciónLibertad())
                formAsignaciónLibertad.ShowDialog(this);
        }

        //Daimler
        private void gestionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaGestiones formCargaGestiones = new frmCargaGestiones())
                formCargaGestiones.ShowDialog(this);
        }
        private void inboundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaInbound formCargaInbound = new frmCargaInbound())
                formCargaInbound.ShowDialog(this);
        }
        private void saldosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaSaldos formCargaSaldos = new frmCargaSaldos())
                formCargaSaldos.ShowDialog(this);
        }
        private void asistenciaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmCargaAsistencia formCargaAsistencia = new frmCargaAsistencia())
                formCargaAsistencia.ShowDialog(this);
        }
        private void eficienciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaEficiencia formCargaEficiencia = new frmCargaEficiencia())
                formCargaEficiencia.ShowDialog(this);
        }

        //Inbursa
        private void validaciónDomiciliosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmValidacionDomicilios formValidacionDomicilios = new frmValidacionDomicilios())
                formValidacionDomicilios.ShowDialog(this);
        }

        //Reportes
        private void alClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmReportesAlCliente formReportesAlCliente = new frmReportesAlCliente())
                formReportesAlCliente.ShowDialog(this);
        }
        private void botoneraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmBotonera formBotonera = new frmBotonera())
                formBotonera.ShowDialog(this);
        }
        private void detalleAsignaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDetalleAsignacion formDetalleAsignacion = new frmDetalleAsignacion())
                formDetalleAsignacion.ShowDialog(this);
        }
        private void movimientosAsignaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMovimientoAsignacion formMovimientoAsignacion = new frmMovimientoAsignacion())
                formMovimientoAsignacion.ShowDialog(this);
        }
        private void telefonosAsignaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmTelefonosAsignacion formTelefonosAsignacion = new frmTelefonosAsignacion())
                formTelefonosAsignacion.ShowDialog(this);
        }
        private void paqueteDomiciliosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmPaqueteDomicilios formPaqueteDomicilios = new frmPaqueteDomicilios())
                formPaqueteDomicilios.ShowDialog(this);
        }
        //Estadistica
        private void tablasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmTablasTemporales formTablasTemporales = new frmTablasTemporales())
                formTablasTemporales.ShowDialog(this);
        }

        private void pagosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (frmPagosCJ formPagosCJ = new frmPagosCJ())
                formPagosCJ.ShowDialog(this);
        }

        private void geolocalizaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmGeolocalizacion formGeolocalizacion = new frmGeolocalizacion())
                formGeolocalizacion.ShowDialog(this);
        }

        private void asignaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAsignacionesCJ formAsignacionesCJ = new frmAsignacionesCJ())
                formAsignacionesCJ.ShowDialog(this);
        }
        private void asignacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAsignacionesEstadistica formAsignacionesEstadistica = new frmAsignacionesEstadistica())
                formAsignacionesEstadistica.ShowDialog(this);
        }
        private void retirosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmRetirosCJ formRetirosCJ = new frmRetirosCJ())
                formRetirosCJ.ShowDialog(this);
        }

        private void marcarCuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMarcarCJ formMarcarCJ = new frmMarcarCJ())
                formMarcarCJ.ShowDialog(this);
        }

        private void mapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMapa formMapa = new frmMapa())
                formMapa.ShowDialog(this);
        }

        private void rutasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmRutas formRutas = new frmRutas())
                formRutas.ShowDialog(this);
        }
        private void metasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMetas formMetas = new frmMetas())
                formMetas.ShowDialog(this);
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void cuentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDictamen formDictamen = new frmDictamen())
                formDictamen.ShowDialog(this);
        }
        private void intentosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmIntentos formIntentos = new frmIntentos())
                formIntentos.ShowDialog(this);
        }
        private void blasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmBlaster formBlaster = new frmBlaster())
                formBlaster.ShowDialog(this);
        }
        //Facturación
        private void arancelesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAranceles formAranceles = new frmAranceles())
                formAranceles.ShowDialog(this);
        }
        private void facturaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmFactura formFactura = new frmFactura())
                formFactura.ShowDialog(this);
        }

        private void contactoRecuperaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmContactoRecuperacion formContactoRecuperacion = new frmContactoRecuperacion())
                formContactoRecuperacion.ShowDialog(this);
        }

        //Score
        private void analisisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAnalisis formAnalisis = new frmAnalisis())
                formAnalisis.ShowDialog(this);
        }

        private void contraseñaLogErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Estadistica].[Contraseña_LogError] @idEjecutivo");
            DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);


            DataSet dtContraseñaLogError = new DataSet();

            if (!DataBaseConn.Fill(dtContraseñaLogError, "ContraseñaLogError"))
            {
                return;


                string sRutaExcel = "";

                if (dtContraseñaLogError.Tables.Count == 0 || dtContraseñaLogError.Tables[0].Rows.Count == 0)
                {
                    TerminaBúsqueda("Consulta terminada sin registros.", false);
                    return;
                }
                else
                    this.Invoke((Action)delegate()
                    {

                        if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                            TerminaBúsqueda("Guardado cancelado por usuario.", false);
                        else
                            sRutaExcel = sfdExcel.FileName;
                    });

                if (sRutaExcel == "")
                {
                    dtContraseñaLogError.Dispose();
                    return;
                }

                string sResultado = ExcelXML.ExportToExcelSAX(ref dtContraseñaLogError, sRutaExcel);
                TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");
            }
        }

        public void TerminaBúsqueda(string Mensaje, bool bError)
        {

            if (this.InvokeRequired)
            {
                this.BeginInvoke((Action)delegate() { TerminaBúsqueda(Mensaje, bError); });
                return;
            }
            this.ControlBox = true;
        }

        private void corteCierreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCorteCierre formCorteCierre = new frmCorteCierre())
                formCorteCierre.ShowDialog(this);
        }

        private void rMTSSUCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmRmtsSuc formRmtsSuc = new frmRmtsSuc())
                formRmtsSuc.ShowDialog(this);
        }

        private void metasAsesorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMetasAsesor formMetasAsesor = new frmMetasAsesor())
                formMetasAsesor.ShowDialog(this);
        }

        private void teléfonosContactoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmTelefonosContacto formTelefonosContacto = new frmTelefonosContacto())
                formTelefonosContacto.ShowDialog(this);
        }

        private void sMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmSMS formSMS = new frmSMS())
                formSMS.ShowDialog(this);
        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmEmail formEmail = new frmEmail())
                formEmail.ShowDialog(this);
        }

        private void extraeTelEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmExtraeTelMail formExtraeTelMail = new frmExtraeTelMail())
                formExtraeTelMail.ShowDialog(this);
        }

        private void validaciónDomiciliosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmValidacionDomiciliosNissan formValidacionDomiciliosNissan = new frmValidacionDomiciliosNissan())
                formValidacionDomiciliosNissan.ShowDialog(this);
        }

        private void teléfonosCarterasConContactoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void teléfonosConContactoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmTelefonosCarterasConContacto formTelefonosCarterasConContacto = new frmTelefonosCarterasConContacto())
                formTelefonosCarterasConContacto.ShowDialog(this);
        }

        private void analisisCarteraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAnalisisCartera formAnalisisCartera = new frmAnalisisCartera())
                formAnalisisCartera.ShowDialog(this);
        }

        private void estudioCarteraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmEstudioCartera formEstudioCartera = new frmEstudioCartera())
                formEstudioCartera.ShowDialog(this);
        }

        private void limpiaArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmLimpia_Archivo formLimpia_Archivo = new frmLimpia_Archivo())
                formLimpia_Archivo.ShowDialog(this);
        }

        private void foliosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmFolios formFolios = new frmFolios())
                formFolios.ShowDialog(this);
        }

        private void asignaciónBMXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaConjurnetAsignacion formCargaConjurnetAsignacion = new frmCargaConjurnetAsignacion())
                formCargaConjurnetAsignacion.ShowDialog(this);
        }

        private void actividadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaConjurnet formCargaConjurnet = new frmCargaConjurnet())
                formCargaConjurnet.ShowDialog(this);
        }

        private void negociacionesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmCargaConjurnetNeg formCargaConjurnetNeg = new frmCargaConjurnetNeg())
                formCargaConjurnetNeg.ShowDialog(this);
        }

        private void domiciliosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDomicilios formDomicilios = new frmDomicilios())
                formDomicilios.ShowDialog(this);
        }

        private void pagosMBXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmConjurnetPagosbmx formConjurnetPagosbmx = new frmConjurnetPagosbmx())
                formConjurnetPagosbmx.ShowDialog(this);
        }

        private void reporteIngresosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmReporteIngresoConjurnet formReporteIngresoConjurnet = new frmReporteIngresoConjurnet())
                formReporteIngresoConjurnet.ShowDialog(this);
        }

        private void asigTDCGraciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAsignacionTDCGracia formAsignacionTDCGracia = new frmAsignacionTDCGracia())
                formAsignacionTDCGracia.ShowDialog(this);
        }

        private void entradasA1PVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAsignacionTDCGraciaEntradas formAsignacionTDCGraciaEntradas = new frmAsignacionTDCGraciaEntradas())
                formAsignacionTDCGraciaEntradas.ShowDialog(this);
        }

        private void calendarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCalendario formCalendario = new frmCalendario())
                formCalendario.ShowDialog(this);
        }

        private void gestionesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmFallabelaGestiones formFallabelaGestiones = new frmFallabelaGestiones())
                formFallabelaGestiones.ShowDialog(this);
        }

        private void intentosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmFallabelaIntentos formFallabelaIntentos = new frmFallabelaIntentos())
                formFallabelaIntentos.ShowDialog(this);
        }

        private void accionamientosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmFallabelaAccionamientos formFallabelaAccionamientos = new frmFallabelaAccionamientos())
                formFallabelaAccionamientos.ShowDialog(this);
        }

        private void desasignacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmDesasignacion formDesasignacion = new frmDesasignacion())
                formDesasignacion.ShowDialog(this);
        }

        private void generoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmGenero formGenero = new frmGenero())
                formGenero.ShowDialog(this);
        }

        private void mejorContactoFallidoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMejorContactoFallido formMejorContactoFallido = new frmMejorContactoFallido())
                formMejorContactoFallido.ShowDialog(this);
        }

        private void asignacionesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (frmAsignaciones formAsignaciones = new frmAsignaciones())
                formAsignaciones.ShowDialog(this);
        }

        private void facturaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmCargaFacturas formCargaFacturas = new frmCargaFacturas())
                formCargaFacturas.ShowDialog(this);
        }

        private void riesgoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCarga_ArchivosRiesgo formCarga_ArchivosRiesgo = new frmCarga_ArchivosRiesgo())
                formCarga_ArchivosRiesgo.ShowDialog(this);
        }

        private void visitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmCargaAsigVisitas formCargaAsigVisitas = new frmCargaAsigVisitas())
                formCargaAsigVisitas.ShowDialog(this);
        }

        private void personalCJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmPersonalCJ formPersonalCJ = new frmPersonalCJ())
                formPersonalCJ.ShowDialog(this);
        }

        private void plantillaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmPlantillaPersonal formPlantillaPersonal = new frmPlantillaPersonal())
                formPlantillaPersonal.ShowDialog(this);
        }

        private void riesgosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmCarga_ArchivosRiesgo formCarga_ArchivosRiesgo = new frmCarga_ArchivosRiesgo())
                formCarga_ArchivosRiesgo.ShowDialog(this);
        }

        private void retencionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmRetenciones formRetenciones = new frmRetenciones())
                formRetenciones.ShowDialog(this);
        }

        private void scoreToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmScore formScore = new frmScore())
                formScore.ShowDialog(this);
        }

        private void banorteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (frmCarga_ArchivosRiesgoBanorte formCarga_ArchivosRiesgoBanorte = new frmCarga_ArchivosRiesgoBanorte())
                formCarga_ArchivosRiesgoBanorte.ShowDialog(this);
        }

        private void listaNegraCorreosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmListaNegraCorreos formListaNegraCorreos = new frmListaNegraCorreos())
                formListaNegraCorreos.ShowDialog(this);
        }

        private void maxMinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmMax_Min formMax_Min = new frmMax_Min())
                formMax_Min.ShowDialog(this);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            using (frmvwfac formvwfac = new frmvwfac())
                formvwfac.ShowDialog(this);
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            using (frmRepFacLeasing formRepFacLeasing = new frmRepFacLeasing())
                formRepFacLeasing.ShowDialog(this);
        }


        private void toolStripMenuItem8_Click_1(object sender, EventArgs e)
        {
            using (frmPortFacLeas formPortFacLeas = new frmPortFacLeas())
                formPortFacLeas.ShowDialog(this);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {

            using (frmDirecFacLeas formDirecFacLeas = new frmDirecFacLeas())
                formDirecFacLeas.ShowDialog(this);


        }

        private void telecomToolStripMenuItem_Click(object sender, EventArgs e)
        {

            using (frmTelecom formTelecom = new frmTelecom())
                formTelecom.ShowDialog(this);
        }
    }
}

