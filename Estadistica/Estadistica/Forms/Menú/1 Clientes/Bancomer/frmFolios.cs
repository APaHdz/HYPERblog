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
    public partial class frmFolios : Form
    {
        public frmFolios()
        {
            InitializeComponent();
            dtpInicial.Value = DateTime.Today.AddDays(-1);
            dtpInicial.MaxDate = dtpInicial.Value;
            dtpInicial.MinDate = DateTime.Today.AddDays(-700);

            dtpFinal.Value = DateTime.Today.AddDays(-1);
            dtpFinal.MaxDate = dtpFinal.Value;
            dtpFinal.MinDate = DateTime.Today.AddDays(-700);
          }

        #region Variables
        
        Microsoft.Office.Interop.Word.Application _objWordApplication; //Objeto Word
        Microsoft.Office.Interop.Word.Document _objWordDocument;  //Objeto Document
        Object oMissing = System.Reflection.Missing.Value;

        System.IO.FileStream fsXml;
        string Documento = "";
        DataTable tblCuentaError = new DataTable();
        string NamePdf = "";
        string idCuenta = "";
        DataTable tblConsulta;
        string sIdentificador = "";
        string sruta = "";
        string sarc = "";
        string sarco = "";
        string snombre = "";
        string Productos = "";
        string Prestamos = "";
        string Titulares = "";
        string Saldos = "";
        string Descuentos = "";
        string Montos = "";
        string Fechas = "";
        string Fecha_Insert = "";
        string FechasConvert = "";
        string Mes = "";
        string Dia = "";
        string Año = "";
        string MontosDescuentos = "";
        string Divisiones = "";
        string Regiones = "";
        string Correos = "";
        string Modelos = "";
        string SubBancs = "";
        string Republica = "";
        string Folios = "";
        string Firmado ="";
        string Carpeta1 ="";
        string Carpeta2 ="";
        string Carpeta3 ="";
        string Carpeta4 = "";
        string Carpeta5 = "";
        string Carpeta6 = "";
        string CarpCon = "";
        string Inicial = "";
        string Final = "";
        int Pdf = 0;
        int PdfReg = 0;
        

        #endregion

        #region Metodos
        public void Consulta()
        {
            Invoke((Action)delegate()
            {
                btnGenerar.Visible = false;
                picWait.Visible = true;
                dtpInicial.Enabled = false;
                dtpFinal.Enabled = false;
            });

            Inicial = dtpInicial.Value.ToString("yyyy-MM-dd");
            Final = dtpFinal.Value.ToString("yyyy-MM-dd");
            tblConsulta = new DataTable();

            Mensaje("Generando Consulta...", Color.SlateGray, lblMensajes);
            //DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; select Folios,[Fecha de emision], Producto, Titular, Prestamo, CASE WHEN LEN(Prestamo)>18 THEN LEFT (Prestamo,8)+RIGHT(Prestamo,10) ELSE Prestamo end Prestamo18, Cuenta, [Saldo total], [Monto a pagar], Descuento, [Fecha de pago], Sub_Banc, Division, Monto_Descuento, Region, Correo, Tipo, EstadoRep, F.Consecutivo, CASE WHEN FF.Consecutivo IS NULL THEN 'NO' ELSE 'SI' end Firmado, FF.PDF_FIRMADO from dbEstadistica.Bancomer.Folios F left join dbEstadistica.Bancomer.Folios_Firmados FF ON F.Consecutivo = FF.Consecutivo where F.Fecha_Insert between @Ini and @Fin and Cuenta='4772133033596302' ");
            //DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; select Folios,[Fecha de emision], Producto, Titular, Prestamo, CASE WHEN LEN(Prestamo)>18 THEN LEFT (Prestamo,8)+RIGHT(Prestamo,10) ELSE Prestamo end Prestamo18, Cuenta, [Saldo total], [Monto a pagar], Descuento, [Fecha de pago], Sub_Banc, Division, Monto_Descuento, Region, Correo, Tipo, EstadoRep, F.Consecutivo, CASE WHEN FF.Consecutivo IS NULL THEN 'NO' ELSE 'SI' end Firmado, FF.PDF_FIRMADO from dbEstadistica.Bancomer.Folios F left join dbEstadistica.Bancomer.Folios_Firmados FF ON F.Consecutivo = FF.Consecutivo where F.Fecha_Insert between @Ini and @Fin ");
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[Bancomer].[3.1.Folios_Estadistica] @Ini,@Fin ");            
            DataBaseConn.CommandParameters.AddWithValue("@Ini", Inicial);
            DataBaseConn.CommandParameters.AddWithValue("@Fin", Final);

            if (!DataBaseConn.Fill(tblConsulta, "Consulta"))
            {
                Mensaje("Falló la consulta a la base de datos, Intente más tarde.", System.Drawing.Color.Crimson, lblMensajes);
                picWait.Visible = false;
                btnGenerar.Visible = true;
                dtpInicial.Enabled = true;
                dtpFinal.Enabled = true;
                return;
            }
            DataBaseConn.StartThread(Generar);

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

           // btnConsulta.Visible = true;
            picWait.Visible = false;
            btnGenerar.Visible = true;
            dtpInicial.Enabled = true;
            dtpFinal.Enabled = true;

            this.ControlBox = true;
        }
        public void PDFs()
        {
            try
            {
                foreach (Process proceso in Process.GetProcesses())
                {
                    if (proceso.ProcessName == "WINWORD")
                    {
                        proceso.Kill();
                    }
                }
            }
            catch
            {
            }
       
            sarco = "";
            sarc = "";
       
            if (Productos == "TDC BANCO" && SubBancs == "Julio César Ángeles Sotuyo")
            {
                sarco = "FO ACUERDO DE PAGO TDC_METRO.docx";
                sarc = "FO ACUERDO DE PAGO TDC_METRO_Copy.docx";
                CarpCon = Carpeta1;
            }
            else if (Productos == "TDC BANCO" && SubBancs == "Jaime López Pérez")
            {
                sarco = "FO ACUERDO DE PAGO TDC.docx";
                sarc = "FO ACUERDO DE PAGO TDC_Copy.docx";
                CarpCon = Carpeta1;
            }
            else if (Productos == "AUTO FINANZIA" && SubBancs == "Julio César Ángeles Sotuyo")
            {
                sarco = "FO ACUERDO DE PAGO AUTO_METRO.docx";
                sarc = "FO ACUERDO DE PAGO AUTO_METRO_Copy.docx";
                CarpCon = Carpeta3;
            }
            else if (Productos == "AUTO FINANZIA" && SubBancs == "Jaime López Pérez")
            {
                sarco = "FO ACUERDO DE PAGO AUTO.docx";
                sarc = "FO ACUERDO DE PAGO AUTO_Copy.docx";
                CarpCon = Carpeta3;
            }
            else if (Productos == "CONSUMO QUINCENAL" && SubBancs == "Julio César Ángeles Sotuyo")
            {
                sarco = "FO ACUERDO DE PAGO CONSUMO_METRO.docx";
                sarc = "FO ACUERDO DE PAGO CONSUMO_METRO_Copy.docx";
                CarpCon = Carpeta2;
            }
            else if (Productos == "CONSUMO QUINCENAL" && SubBancs == "Jaime López Pérez")
            {
                sarco = "FO ACUERDO DE PAGO CONSUMO.docx";
                sarc = "FO ACUERDO DE PAGO CONSUMO_Copy.docx";
                CarpCon = Carpeta2;
            }
            else if (Productos == "CONSUMO MENSUAL" && SubBancs == "Julio César Ángeles Sotuyo")
            {
                sarco = "FO ACUERDO DE PAGO CONSUMO_METRO.docx";
                sarc = "FO ACUERDO DE PAGO CONSUMO_METRO_Copy.docx";
                CarpCon = Carpeta2;
            }
            else if (Productos == "CONSUMO MENSUAL" && SubBancs == "Jaime López Pérez")
            {
                sarco = "FO ACUERDO DE PAGO CONSUMO.docx";
                sarc = "FO ACUERDO DE PAGO CONSUMO_Copy.docx";
                CarpCon = Carpeta2;
            }
       
            if (System.IO.File.Exists(@"C:\Procest\AcuerdosEst\" + sarc))
            {
                try
                {
                    System.IO.File.Delete(@"C:\Procest\AcuerdosEst\" + sarc);
                }
                catch { }
            }
            if (System.IO.File.Exists(CarpCon + "\\" + sarc))
            {
                try
                {
                    System.IO.File.Delete(CarpCon + "\\" + sarc);
                }
                catch { }
            }

           

            string archivo = @"C:\Procest\AcuerdosEst\" + sarco;
            if (!File.Exists(archivo))
            {
              Mensaje ("No existe el documento en la ruta especificada.", System.Drawing.Color.Crimson, lblMensajes);
            }

            /// consulta el producto para saber en que carpeta guardar el word
            if (Productos == "TDC BANCO")
            {
                 File.Copy(@"C:\Procest\AcuerdosEst\" + sarco,  Carpeta1 + "\\"+ sarc);
            }

            else if (Productos == "AUTO FINANZIA")
            {
                File.Copy(@"C:\Procest\AcuerdosEst\" + sarco, Carpeta3 + "\\" + sarc);
             
            }
            else if (Productos == "CONSUMO QUINCENAL")
            {
                File.Copy(@"C:\Procest\AcuerdosEst\" + sarco, Carpeta2 + "\\" + sarc);
               
            }
            else if (Productos == "CONSUMO MENSUAL")
            {
                File.Copy(@"C:\Procest\AcuerdosEst\" + sarco, Carpeta2 + "\\" + sarc);
               
            }
           
       
            snombre = NamePdf + Folios.ToString() + " " + Titulares.ToString();
            Word();
        }
        public void Word()
        {
            if (Productos == "TDC BANCO")
            {
                sIdentificador = idCuenta;
            }
            else
                sIdentificador = Prestamos;
            try
            {
                LoadWordFile(CarpCon + "\\" + sarc);
                FindReplace(CarpCon +"\\" + sarc, "<folio>", Folios);
                FindReplace(CarpCon +"\\" + sarc, "<producto>", Productos);
                FindReplace(CarpCon +"\\" + sarc, "<titular>", Titulares);
                FindReplace(CarpCon +"\\" + sarc, "<préstamo>", Prestamos);
                FindReplace(CarpCon +"\\" + sarc, "<cuenta>", sIdentificador);
                FindReplace(CarpCon +"\\" + sarc, "<saldo>", Saldos);
                FindReplace(CarpCon +"\\" + sarc, "<pago>", Montos);
                FindReplace(CarpCon +"\\" + sarc, "<por_descuento>", Descuentos);
                FindReplace(CarpCon +"\\" + sarc, "<desc_monto>", MontosDescuentos);
                FindReplace(CarpCon +"\\" + sarc, "<fec_pago>", FechasConvert);
                FindReplace(CarpCon + "\\" + sarc, "<día>", Dia);
                FindReplace(CarpCon + "\\" + sarc, "<mes>", Mes);
                FindReplace(CarpCon + "\\" + sarco, "<año>", Año);
                FindReplace(CarpCon + "\\" + sarc, "<desc_auto>", Modelos);
                FindReplace(CarpCon + "\\" + sarc, "<dir_banc>", SubBancs);
                FindReplace(CarpCon + "\\" + sarc, "<estado>", Republica);


                FindReplace(CarpCon + "\\" + sarc, "<folio>", "");
                FindReplace(CarpCon + "\\" + sarc, "<producto>", "");
                FindReplace(CarpCon + "\\" + sarc, "<titular>", "");
                FindReplace(CarpCon + "\\" + sarc, "<préstamo>", "");
                FindReplace(CarpCon + "\\" + sarc, "<cuenta>", "");
                FindReplace(CarpCon + "\\" + sarc, "<saldo>", "");
                FindReplace(CarpCon + "\\" + sarc, "<pago>", "");
                FindReplace(CarpCon + "\\" + sarc, "<por_descuento>", "");
                FindReplace(CarpCon + "\\" + sarc, "<desc_monto>", "");
                FindReplace(CarpCon + "\\" + sarc, "<fec_pago>", "");
                FindReplace(CarpCon + "\\" + sarc, "<día>", "");
                FindReplace(CarpCon + "\\" + sarc, "<mes>", "");
                FindReplace(CarpCon + "\\" + sarc, "<año>", "");
                FindReplace(CarpCon + "\\" + sarc, "<desc_auto>", "");
                FindReplace(CarpCon + "\\" + sarc, "<dir_banc>", "");
                FindReplace(CarpCon + "\\" + sarc, "<estado>", "");

                CloseWord();
            }
            catch 
            {
                try
                {
                    foreach (Process proceso in Process.GetProcesses())
                    {
                        if (proceso.ProcessName == "WINWORD")
                        {
                            proceso.Kill();
                        }
                    }
                }
                catch { }


                if (System.IO.File.Exists(CarpCon + "\\" + sarc))
                {
                    try
                    {
                        System.IO.File.Delete(CarpCon + "\\" + sarc);
                    }
                    catch { }
                }
            }
        }
        private void LoadWordFile(string _filename)
        {
            _objWordApplication = new Microsoft.Office.Interop.Word.Application();
            _objWordDocument = _objWordApplication.Documents.Open(_filename, false, false, false, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            _objWordApplication.Visible = false;
        }
        public void FindReplace(string _filename, string _find, string _replace)
        {
            Microsoft.Office.Interop.Word.Find findObject = _objWordApplication.Selection.Find;
            findObject.ClearFormatting();
            findObject.Text = _find;
            findObject.Replacement.ClearFormatting();
            findObject.Replacement.Text = _replace;
       
            object replaceAll = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
            findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }
        public void CloseWord()
       {
          
           Microsoft.Office.Interop.Word._Document doc = _objWordApplication.Documents[CarpCon + "\\" + sarc] as Microsoft.Office.Interop.Word._Document;
           object fileFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF;
           Documento = CarpCon + "\\" + snombre + ".pdf";
           doc.SaveAs(Documento, ref fileFormat, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
           doc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
       
           try
           {
               foreach (Process proceso in Process.GetProcesses())
               {
                   if (proceso.ProcessName == "WINWORD")
                   {
                       proceso.Kill();
                   }
               }
           }
           catch
           {
           }

           if (System.IO.File.Exists(CarpCon + "\\" + sarc))
           {
               try
               {
                   System.IO.File.Delete(CarpCon + "\\" + sarc);
               }
               catch { }
           }
       }

        public void Generar()
        {
            int i = 0;
            for (i = 0; i < tblConsulta.Rows.Count; i++)
            {
                Pdf = i + 1;
                PdfReg = tblConsulta.Rows.Count;
                Productos = "";
                Prestamos = "";
                Titulares = "";
                Saldos = "";
                Descuentos = "";
                Montos = "";
                Fechas = "";
                FechasConvert = "";
                Fecha_Insert = "";
                Mes = "";
                Dia = "";
                Año = "";
                MontosDescuentos = "";
                Divisiones = "";
                Regiones = "";
                Correos = "";
                Modelos = "";
                Republica = "";
                Folios = "";
                Firmado = "";


                idCuenta = tblConsulta.Rows[i]["Cuenta"].ToString();
                Productos = tblConsulta.Rows[i]["Producto"].ToString();
                Prestamos = tblConsulta.Rows[i]["Prestamo"].ToString().ToUpper();
                Titulares = tblConsulta.Rows[i]["Titular"].ToString();
                Titulares = Titulares.Replace("?", "");
                Saldos = tblConsulta.Rows[i]["Saldo total"].ToString();
                Saldos = string.Format("{0:C2}", tblConsulta.Rows[i]["Saldo total"]);
                Descuentos = tblConsulta.Rows[i]["Descuento"].ToString();
                Descuentos = (Decimal.Parse(Descuentos) * 100).ToString("##.##");
                Montos = tblConsulta.Rows[i]["Monto a pagar"].ToString();
                Montos = string.Format("{0:C2}", tblConsulta.Rows[i]["Monto a pagar"]);
                Fechas = tblConsulta.Rows[i]["Fecha de pago"].ToString();
                Fecha_Insert = tblConsulta.Rows[i]["Fecha de emision"].ToString();
                MontosDescuentos = tblConsulta.Rows[i]["Monto_Descuento"].ToString();
                MontosDescuentos = string.Format("{0:C2}", tblConsulta.Rows[i]["Monto_Descuento"]);
                Divisiones = tblConsulta.Rows[i]["Division"].ToString();
                Regiones = tblConsulta.Rows[i]["Region"].ToString();
                Correos = tblConsulta.Rows[i]["Correo"].ToString();
                Modelos = tblConsulta.Rows[i]["Tipo"].ToString();
                SubBancs = tblConsulta.Rows[i]["Sub_Banc"].ToString();
                Republica = tblConsulta.Rows[i]["EstadoRep"].ToString();
                Folios = tblConsulta.Rows[i]["Folios"].ToString();
                FechasConvert = Fechas.Substring(0, 2) + " de " + DateTime.Parse(Fechas).ToString("MMMM", CultureInfo.CreateSpecificCulture("Es")) + " del " + Fechas.Substring(6, 4);
                Mes = DateTime.Parse(Fecha_Insert).ToString("MMMM", CultureInfo.CreateSpecificCulture("Es"));
                Dia = Fecha_Insert.Substring(0, 2).Length == 1 ? "0" + Fecha_Insert.Substring(0, 2) : Fecha_Insert.Substring(0, 2);
                Año = Fecha_Insert.Substring(6, 4);
                Firmado = tblConsulta.Rows[i]["Firmado"].ToString();
           
                
                if (Firmado == "SI" )
                {
                    DataRow row = tblConsulta.Rows[i];
                    byte[] documentofir = (byte[])row["PDF_FIRMADO"];
                  
                    if (Productos == "TDC BANCO")
                    {
                        File.WriteAllBytes(Carpeta4 + "\\" + Folios + " " + Titulares + " " + "(Firmado)" + ".pdf", documentofir);
                    }
                    else if (Productos == "CONSUMO MENSUAL")
                    {
                        File.WriteAllBytes(Carpeta5 + "\\" + Folios + " " + Titulares + " " + "(Firmado)" + ".pdf", documentofir);
                    }
                    else if (Productos == "CONSUMO QUINCENAL")
                    {
                        File.WriteAllBytes(Carpeta5 + "\\" + Folios + " " + Titulares + " " + "(Firmado)" + ".pdf", documentofir);
                    }
                    else if (Productos == "AUTO FINANZIA")
                    {
                        File.WriteAllBytes(Carpeta6 + "\\" + Folios + " " + Titulares + " " + "(Firmado)" + ".pdf", documentofir);
                    }
                 }
                else
                {
                    PDFs();
                    Invoke((Action)delegate()
                    {
                        picWait.Visible = true;
                        btnGenerar.Visible = false;
                        dtpInicial.Enabled = false;
                        dtpFinal.Enabled = false;

                    });
                    Mensaje("Generando PDF " + " " + (i + 1) + " " + "de" + " " + tblConsulta.Rows.Count, Color.DimGray, lblMensajes);
                }

            }
            Invoke((Action)delegate()
            {
                ExportarExcel();
            });

                                      
            Invoke((Action)delegate()
            {
                picWait.Visible = false;
                Mensaje("El proceso concluyó, los PDF's se guardarón en la carpeta seleccionada.", Color.DimGray, lblMensajes);
            });
        }



        public void ExportarExcel ()
        {
            string sRutaExcel ="";             
            if (sfdExcel.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                TerminaBúsqueda("Guardado cancelado por usuario.", false);
            else
                sRutaExcel = sfdExcel.FileName;
            if (sRutaExcel == "")
            {
                tblConsulta.Dispose();
                return;
            }
            string sResultado = ExcelXML.ExportToExcelSAX(ref tblConsulta, sRutaExcel);
           TerminaBúsqueda(sResultado == "" ? "Libro de Excel guardado." : sResultado, sResultado != "");

        }
                
                
        #endregion

        #region Eventos
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            //Invoke((Action)delegate()
            //{
            //    btnGenerar.Visible = false;
            //    picWait.Visible = true;
            //    dtpInicial.Enabled = false;
            //    dtpFinal.Enabled = false;
            //});
            if (dtpInicial.Value > dtpFinal.Value)
            {
                Mensaje("La fecha inicial tiene que ser menor o igual a la fecha final.", Color.Red, lblMensajes);
                return;
            }
           
            ///seleccionar ruta y crear las 6 carpetas
            using (var seleccion = new FolderBrowserDialog())
            {
                MessageBox.Show(this,"Seleccione ruta para descargar archivos.","Validacion",MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (seleccion.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(seleccion.SelectedPath))
                {
                     sruta = seleccion.SelectedPath;
                }

                Carpeta1 = sruta + "\\" + "Generados TDC";
                if (!Directory.Exists(Carpeta1))
                    Directory.CreateDirectory(Carpeta1);
                
                Carpeta2 = sruta + "\\" + "Generados Consumo";
                if (!Directory.Exists(Carpeta2))
                    Directory.CreateDirectory(Carpeta2);
                
                Carpeta3 = sruta + "\\" + "Generados Auto";
                if (!Directory.Exists(Carpeta3))
                    Directory.CreateDirectory(Carpeta3);
                
                Carpeta4 = sruta + "\\" + "Firmados TDC";
                if (!Directory.Exists(Carpeta4))
                    Directory.CreateDirectory(Carpeta4);

                Carpeta5 = sruta + "\\" + "Firmados Consumo";
                if (!Directory.Exists(Carpeta5))
                    Directory.CreateDirectory(Carpeta5);
                
                Carpeta6 = sruta + "\\" + "Firmados Auto";
                if (!Directory.Exists(Carpeta6))
                    Directory.CreateDirectory(Carpeta6);
              }
            Consulta();
            //DataBaseConn.StartThread(Generar);
            //Generar();
           
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
