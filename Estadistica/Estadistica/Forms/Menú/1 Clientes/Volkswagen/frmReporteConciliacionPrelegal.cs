using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estadistica
{
    public partial class frmReporteConciliacionPrelegal : Form
    {
        public frmReporteConciliacionPrelegal()
        {
            InitializeComponent();
            dtpInicial.Value = DateTime.Today.AddDays(-0);
            dtpInicial.MaxDate = dtpInicial.Value;
            dtpInicial.MinDate = DateTime.Today.AddDays(-90);

            dtpFinal.Value = DateTime.Today.AddDays(-0);
            dtpFinal.MaxDate = dtpFinal.Value;
            dtpFinal.MinDate = DateTime.Today.AddDays(-90); 
        }

        #region Variables    
        string sAgencia = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void Consulta()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);

            string Inicial = dtpInicial.Value.ToString("yyyy-MM-dd");
            string Final = dtpFinal.Value.ToString("yyyy-MM-dd");

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Volkswagen.[2.3.1.VW_Saldo_Dia] @Agencia, @Inicial, @Final ");
            DataBaseConn.CommandParameters.AddWithValue("@Agencia", sAgencia);
            DataBaseConn.CommandParameters.AddWithValue("@Inicial", Inicial);
            DataBaseConn.CommandParameters.AddWithValue("@Final", Final);

            DataTable tblSaldoDia = new DataTable();

            if (!DataBaseConn.Fill(tblSaldoDia, "2.3.1.VW_Saldo_Dia"))
            {
                TerminaBúsqueda("Falló proceso 2.3.1.VW_Saldo_Dia.", true);
                return;
            }

            for (int i = 0; i < tblSaldoDia.Rows.Count; i++)
            {
                string Columna = "[" + tblSaldoDia.Rows[i]["Día"].ToString() + "]";
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
                  "ALTER TABLE  dbEstadistica.Temp.VWP_Saldo_Dia \r\n\t" +
                  "DROP COLUMN " + Columna);                

                if (!DataBaseConn.Execute("Elimina columna."))
                {
                    Mensaje("Falló al eliminar columna.", System.Drawing.Color.Crimson, lblMensajes);
                    TerminaBúsqueda("Falló proceso.", true);
                    return;
                }
            }

            DataTable tblConsulta = new DataTable();
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; SELECT * FROM dbEstadistica.Temp.VWP_Saldo_Dia");

            if (!DataBaseConn.Fill(tblConsulta,"C_VWP_Saldo_Dia."))
            {                
                TerminaBúsqueda("Falló consulta.", true);
                return;
            }

            for (int i = 1; i < tblConsulta.Columns.Count; i++)
            {                
                    string NombreColumna = "[" + tblConsulta.Columns[i].ColumnName.ToString() + "]";
                    if (NombreColumna != "[26]" && NombreColumna != "[27]" && NombreColumna != "[28]" && NombreColumna != "[29]" && NombreColumna != "[30]" && NombreColumna != "[31]")
                    {
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
                      "USE dbEstadistica; EXEC sp_rename 'Temp.VWP_Saldo_Dia." + NombreColumna + "','" + (i).ToString() + "'");

                    if (!DataBaseConn.Execute("Nombre columna."))
                    {
                        Mensaje("Falló al renombrar columna.", System.Drawing.Color.Crimson, lblMensajes);
                        TerminaBúsqueda("Falló al renombrar columna.", true);
                        return;
                    }
                }
            }

            tblConsulta = new DataTable();
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; SELECT * FROM dbEstadistica.Temp.VWP_Saldo_Dia");

            if (!DataBaseConn.Fill(tblConsulta, "C_VWP_Saldo_Dia."))
            {
                TerminaBúsqueda("Falló consulta.", true);
                return;
            }

            for (int i = 1; i < 32; i++)
            {
                string NombreColumna = "[" + i.ToString() + "]";

                if (!tblConsulta.Columns.Contains(NombreColumna.Replace("[", "").Replace("]", "")))
                {
                    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
                      "USE dbEstadistica; ALTER TABLE [Temp].[VWP_Saldo_Dia] ADD " + NombreColumna + " decimal(3,2) DEFAULT 0.00 NOT NULL ");

                    if (!DataBaseConn.Execute("Agrega columna."))
                    {
                        Mensaje("Falló al agregar columna.", System.Drawing.Color.Crimson, lblMensajes);
                        TerminaBúsqueda("Falló al agregar columna.", true);
                        return;
                    }
                }
            }

            //for (int i = tblConsulta.Columns.Count; i < 32; i++)
            //{
            //    string NombreColumna = "[" + i.ToString() + "]";
            //    DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; \r\n" +
            //      "USE dbEstadistica; ALTER TABLE [Temp].[VWP_Saldo_Dia] ADD " + NombreColumna + " decimal(3,2) DEFAULT 0.00 NOT NULL ");

            //    if (!DataBaseConn.Execute("Agrega columna."))
            //    {
            //        Mensaje("Falló al agregar columna.", System.Drawing.Color.Crimson, lblMensajes);
            //        TerminaBúsqueda("Falló al agregar columna.", true);
            //        return;
            //    }
            //}


            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Volkswagen.[2.3.Consolidado_PrelegalDia] @Agencia, @Inicial, @Final ");
            DataBaseConn.CommandParameters.AddWithValue("@Agencia", sAgencia);
            DataBaseConn.CommandParameters.AddWithValue("@Inicial", Inicial);
            DataBaseConn.CommandParameters.AddWithValue("@Final", Final);
            //DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);

            //Invoke((Action)delegate() { btnConsulta.Visible = false; picWait.Visible = true; });
            DataSet tblReporte = new DataSet();           

            if (!DataBaseConn.Fill(tblReporte, "Exportas_Reporte_LeasingDia"))
            {
                TerminaBúsqueda("Falló la generación del reporte.", true);
                return;
            }
            
            if (tblReporte.Tables[0].Columns.Contains("Mensaje"))
            {
                Mensaje(tblReporte.Tables[0].Rows[0]["Mensaje"].ToString(), Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnConsulta.Visible = true; picWait.Visible = false; });
                return;
            }

            string sRutaExcel = "";
            
            if (tblReporte.Tables.Count == 0 || tblReporte.Tables[0].Rows.Count == 0)
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
                tblReporte.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblReporte, sRutaExcel);
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
            picWait.Visible = false;

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnConsulta_Click(object sender, EventArgs e)
        {
            if (dtpInicial.Value > dtpFinal.Value)
            {
                Mensaje("La fecha inicial tiene que ser menor o igual a la fecha final.", Color.Red, lblMensajes);            
                return;
            }
                
                sAgencia = cmbAgencia.Text;
                picWait.Visible = true;
                btnConsulta.Visible = false;                                           

                DataBaseConn.StartThread(Consulta);            
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
