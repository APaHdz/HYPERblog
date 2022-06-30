using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Estadistica
{
    public partial class frmIrene : Form
    {
        public frmIrene()
        {
            InitializeComponent();
            dtpInicial.Value = DateTime.Today.AddDays(-0);
            dtpInicial.MaxDate = dtpInicial.Value;
            dtpInicial.MinDate = DateTime.Today.AddDays(-45);

            dtpFinal.Value = DateTime.Today.AddDays(-0);
            dtpFinal.MaxDate = dtpFinal.Value;
            dtpFinal.MinDate = DateTime.Today.AddDays(-45);
        }
        

        #region Variables
        DataTable tblIreneQuery;        
        int iNumberRecords;
        string Inicial = "";
        string Final = "";
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void IreneQuery()
        {
            if (!DataBaseConn.Fill(tblIreneQuery, "IreneQuery"))
            {
                Mensaje("Falló la consulta.", Color.Crimson, lblMensajes);
                return;
            }

            if (tblIreneQuery.Rows.Count > 0)
            {
                tblIreneQuery.DefaultView.Sort = "NumReg";
                tblIreneQuery.PrimaryKey = new DataColumn[] { tblIreneQuery.Columns["NumReg"] };

                Invoke((Action)delegate
                {
                    dgvIrene.DataSource = tblIreneQuery;
                    dgvIrene.Columns["MontoNegociado"].DefaultCellStyle.Format = "C2";                
                    dgvIrene.Visible = true;
                    btnConsulta.Visible = true;
                    picWaitConsulta.Visible = false;
                    dtpInicial.Enabled = true;
                    dtpFinal.Enabled = true;
                    txtComentario.Visible = true;
                    dgvIrene.Visible = true;
                    btnCargar.Visible = true;
                });
                Mensaje("Consulta terminada con " + tblIreneQuery.Rows.Count.ToString() + " registros.", Color.DimGray, lblMensajes);
            }
            else
            {                
                Invoke((Action)delegate
                {
                    dgvIrene.DataSource = tblIreneQuery;
                    dgvIrene.Visible = false;
                    btnConsulta.Visible = true;
                    picWaitConsulta.Visible = false;
                    dtpInicial.Enabled = true;
                    dtpFinal.Enabled = true;
                    txtComentario.Visible = false;                    
                    btnCargar.Visible = false;
                });
                Mensaje("No existen solicitudes pendientes.", Color.DimGray, lblMensajes);
            }
        }

        public void IreneReporte()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);
            
            DataSet tblReporte = new DataSet();
            if (!DataBaseConn.Fill(tblReporte, "IreneReporte"))
            {
                TerminaBúsqueda("Falló la generación del reporte.", true);
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

            btnConsulta.Visible = true;
            btnCargar.Visible = false;
            picWaitBulk.Visible = false;

            dgvIrene.DataSource = null;
            dgvIrene.Visible = false;
            txtComentario.Text = "";
            txtComentario.Visible = false;
            lblInstrucciones.Text = "";
            lblRegistros.Text = "";

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnConsulta_Click(object sender, EventArgs e)
        {
            if(dtpInicial.Value>dtpFinal.Value)
            {
                Mensaje("La fecha de inicio no puede ser mayor a la fecha final.", Color.Crimson, lblMensajes);
                return;
            }

            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);

            Inicial = dtpInicial.Value.ToString("yyyy-MM-dd");
            Final = dtpFinal.Value.ToString("yyyy-MM-dd");

            tblIreneQuery = new DataTable();
            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Bancomer.[1.2.IreneQuery] @Inicial, @Final ");            
            DataBaseConn.CommandParameters.AddWithValue("@Inicial", Inicial);
            DataBaseConn.CommandParameters.AddWithValue("@Final", Final);

            btnConsulta.Visible = false;
            picWaitConsulta.Visible = true;
            txtComentario.Visible = false;
            txtComentario.Text = "";
            dgvIrene.Visible = false;
            dtpInicial.Enabled = false;
            dtpFinal.Enabled = false;
            btnCargar.Visible = false;
            picWaitBulk.Visible = false;

            DataBaseConn.StartThread(IreneQuery);
        }

        private void dgvIrene_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIrene.SelectedRows.Count == 0)
                return;

            string NumReg = dgvIrene.SelectedRows[0].Cells["NumReg"].Value.ToString();
            string Comentarios = dgvIrene.SelectedRows[0].Cells["Comentarios"].Value.ToString();
            string Omite = dgvIrene.SelectedRows[0].Cells["Omitir"].Value.ToString();

            if (Comentarios.Contains("AMIGA") || Comentarios.Contains("AMIGO") || Comentarios.Contains("AMIG") 
                 || Comentarios.Contains("ATENDER") || Comentarios.Contains("ATENDE") || Comentarios.Contains("ATIENDE")
                 || Comentarios.Contains("ATENDIERON") || Comentarios.Contains("BANCOMER")
                 || Comentarios.Contains("BBVA") || Comentarios.Contains("BUZON") || Comentarios.Contains("BUSON")
                 || Comentarios.Contains("COLGADO") || Comentarios.Contains("COLGANDO") || Comentarios.Contains("COLGAR")
                 || Comentarios.Contains("COLGO") || Comentarios.Contains("CUELGA") || Comentarios.Contains("COLGARO")
                 || Comentarios.Contains("CONDUCEF") || Comentarios.Contains("CONDUSEF") || Comentarios.Contains("CONOCIDO")
                 || Comentarios.Contains("CORREO") || Comentarios.Contains("MAIL") || Comentarios.Contains("CORTA")
                 || Comentarios.Contains("CORTADA") || Comentarios.Contains("CORTARA") || Comentarios.Contains("CORTO")
                 || Comentarios.Contains("CORTARON") || Comentarios.Contains("CUELGA") || Comentarios.Contains("CYBER")
                 || Comentarios.Contains("CIBER") || Comentarios.Contains("DEJA EN LA LINEA") || Comentarios.Contains("DEJA EN LINEA")
                 || Comentarios.Contains("DESCUELGA") || Comentarios.Contains("DESCOLGADO") || Comentarios.Contains("DESCOLGANDO")
                 || Comentarios.Contains("DESCONOCE ADEUDO") || Comentarios.Contains("DESCONOCE") || Comentarios.Contains("DESK")
                 || Comentarios.Contains("ESPOSA") || Comentarios.Contains("ESPOSO") || Comentarios.Contains("FINALIZA")
                 || Comentarios.Contains("FINALISA") || Comentarios.Contains("FINALIZO") || Comentarios.Contains("FINALISO")
                 || Comentarios.Contains("GESTION ANTERIOR") || Comentarios.Contains("GROSERIAS") || Comentarios.Contains("GROCERIAS")
                 || Comentarios.Contains("GROSERIA") || Comentarios.Contains("GROCERIA") || Comentarios.Contains("HD")
                 || Comentarios.Contains("HELP") || Comentarios.Contains("HERMANA") || Comentarios.Contains("HERMANO")
                 || Comentarios.Contains("HIJA") || Comentarios.Contains("HIJO") || Comentarios.Contains("INSULTA")
                 || Comentarios.Contains("LE CONOCE") || Comentarios.Contains("CONOCE") || Comentarios.Contains("LLAMAR")
                 || Comentarios.Contains("MAIL") || Comentarios.Contains("MAMA") || Comentarios.Contains("MANDA PROPUESTA")
                 || Comentarios.Contains("MANEJANDO") || Comentarios.Contains("MARCANDO") || Comentarios.Contains("MARCAR")
                 || Comentarios.Contains("MARQUE") || Comentarios.Contains("MAS TARDE") || Comentarios.Contains("MENSAJE")
                 || Comentarios.Contains("MSJ") || Comentarios.Contains("NO CONTESTA") || Comentarios.Contains("NO RECONOCE ADEUDO")
                 || Comentarios.Contains("NO RESPONDE") || Comentarios.Contains("NO SE ESCUCHA") || Comentarios.Contains("OCUPADA")
                 || Comentarios.Contains("OCUPADO") || Comentarios.Contains("OCUPADO") || Comentarios.Contains("PAPA")
                 || Comentarios.Contains("RECHAZA DEUDA") || Comentarios.Contains("RECHAZA DEUDA") || Comentarios.Contains("RESPONSABILIZA")
                 || Comentarios.Contains("RESPONSABLE")  || Comentarios.Contains("SE NIEGA A PAGAR") || Comentarios.Contains("SEÑOR")
                 || Comentarios.Contains("SMS") || Comentarios.Contains("SR") || Comentarios.Contains("TERCERO") || Comentarios.Contains("TERMINA")
                 || Comentarios.Contains("TERMINADA") || Comentarios.Contains("TERMINO") || Comentarios.Contains("VECINA")
                 || Comentarios.Contains("VESINA") || Comentarios.Contains("VECINO") || Comentarios.Contains("VESINO")
                 || Comentarios.Contains("VINCULA") || Comentarios.Contains("VISITA") || Comentarios.Contains("WAHTSAP")
                 || Comentarios.Contains("WAPSAP")  || Comentarios.Contains("WASSAP") || Comentarios.Contains("WAT SAP")
                 || Comentarios.Contains("WATHS") || Comentarios.Contains("WATHS") || Comentarios.Contains("WATS")
                 || Comentarios.Contains("WATS")  || Comentarios.Contains("WATSAP") || Comentarios.Contains("WATTS")
                 || Comentarios.Contains("WH") || Comentarios.Contains("WHAT") || Comentarios.Contains("WHATHS")
                 || Comentarios.Contains("WHATS") || Comentarios.Contains("WHATSAPP") || Comentarios.Contains("WSP")
                 || Comentarios.Contains("WA ") || Comentarios.Contains("APGO") || Comentarios.Contains("LIQUIDIAR")
                 || Comentarios.Contains("DNCIA") || Comentarios.Contains("DICTA") || Comentarios.Contains("SUEGIMIENTO")
                 || Comentarios.Contains("ACLARACIN") || Comentarios.Contains("RESULVE") ||Comentarios.Contains("BONICA")
                 || Comentarios.Contains("DIDCTAMINA") || Comentarios.Contains("CUATGRO") || Comentarios.Contains("VUELGA")
                 || Comentarios.Contains("CUNATO") || Comentarios.Contains("SINERO") || Comentarios.Contains("CUENLGA")
                 || Comentarios.Contains("NO ESCUCHA") || Comentarios.Contains("CUALGA") || Comentarios.Contains("MJA")
                 || Comentarios.Contains("MENSAJUE") || Comentarios.Contains("CUNETA") || Comentarios.Contains("PUAGO")
                 || Comentarios.Contains("NOSCUCHA") || Comentarios.Contains("ACLARACON") || Comentarios.Contains("GROCERO")
                 || Comentarios.Contains("NO PRAGARA") || Omite.Contains("1")
                )
            {
                txtComentario.ForeColor = Color.Red;
            }
            else
            {
                txtComentario.ForeColor = Color.Blue;
            }

            txtComentario.Text = Comentarios;
        }

        private void dgvIrene_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvIrene.Rows.Count <= 0)
                return;

            if (e.RowIndex >= 0 && dgvIrene.Columns[e.ColumnIndex].Name == "Quitar")
            {
                string NumReg = dgvIrene.SelectedRows[0].Cells["NumReg"].Value.ToString();
                int Reg = e.RowIndex;
                dgvIrene.Rows.RemoveAt(Reg);
                Mensaje("Registro eliminado correctamente.", Color.DimGray, lblMensajes);
            }

            //if(e.RowIndex>=0 && dgvIrene.Columns[e.ColumnIndex].Name == "Quitar")
            //{
            //    if(MessageBox.Show(this, "¿Está seguro de eliminar el registro seleccionado?","Eliminar registro",MessageBoxButtons.YesNo,MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            //    {
            //        Mensaje(tblIreneQuery.Rows.Count.ToString() + " registros.", Color.DimGray, lblMensajes);
            //        return;
            //    }
            //    else
            //    {
            //        string NumReg = dgvIrene.SelectedRows[0].Cells["NumReg"].Value.ToString();
            //        int Reg = e.RowIndex;
            //        dgvIrene.Rows.RemoveAt(Reg);

            //        Mensaje("Registro eliminado correctamente.", Color.DimGray, lblMensajes);
            //    }
            //}
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            DataTable CreaTemporalPag = new DataTable();
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Irene' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Irene");

            if (!DataBaseConn.Execute("DropTemporalIrene"))
            {
                Mensaje("Falló al eliminar tabla temporal IRENE.", Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnCargar.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Irene ( ";
            for (int i = 0; i < tblIreneQuery.Columns.Count; i++)
            {
                if (tblIreneQuery.Columns[i].ColumnName.Contains("FGestion") || tblIreneQuery.Columns[i].ColumnName.Contains("FechaPP") || tblIreneQuery.Columns[i].ColumnName.Contains("Fecha_Insert"))
                    sCreateTable += ("\r\n [" + tblIreneQuery.Columns[i] + "] DATE NULL, ");
                else if (tblIreneQuery.Columns[i].ColumnName.Contains("MontoNegociado"))
                    sCreateTable += ("\r\n [" + tblIreneQuery.Columns[i] + "] MONEY NULL, ");
                else if (tblIreneQuery.Columns[i].ColumnName.Contains("Segundo_Insert"))
                    sCreateTable += ("\r\n [" + tblIreneQuery.Columns[i] + "] TIME(0) NULL, ");
                else
                    sCreateTable += ("\r\n [" + tblIreneQuery.Columns[i] + "] VARCHAR(MAX) NULL, ");
            }

            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";

            DataBaseConn.SetCommand(sCreateTable);

            if (!DataBaseConn.Fill(CreaTemporalPag, "CreaTemporalIrene"))
            {
                Mensaje("Falló al crear tabla temporal de IRENE.", Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnCargar.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Insertando datos a Irene, por favor espere...", Color.DimGray, lblMensajes);
            using (SqlBulkCopy blkData = DataBaseConn.bulkCopy)
            {
                {
                    try
                    {
                        DataBaseConn.ConnectSQL(true);
                        blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.NotifyAfter = 317;
                        blkData.DestinationTableName = "dbEstadistica.Temp.Irene";
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(tblIreneQuery);
                    }
                    catch (Exception ex)
                    {
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        Invoke((Action)delegate() { btnCargar.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                //Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                //tblIreneReporte = new DataSet();
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Bancomer.[1.2.IreneReporte] @Inicial, @Final ");
                DataBaseConn.CommandParameters.AddWithValue("@Inicial", Inicial);
                DataBaseConn.CommandParameters.AddWithValue("@Final", Final);

                DataBaseConn.StartThread(IreneReporte);
            }
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
