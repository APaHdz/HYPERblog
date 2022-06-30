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
    public partial class frmReporteIngresoConjurnet : Form
    {
        public frmReporteIngresoConjurnet()
        {
            InitializeComponent();                        
            PreparaVentana();
            AgregaEventosTxtKeyPress();
        }

        private void AgregaEventosTxtKeyPress()
        {
            //throw new NotImplementedException();
        }

        #region Variables    
        int idCartera;
        int idProducto ;
        int idEjecutivo = 0;
        bool Valida = false;
        
        
        
        
         
        //int Saldo = 0;
        //int Top = 0;
       //bool Negociacion = false;
        //string Nivel = "";
        //int Correos = 0;
        //int Domicilios = 0;
        //int Telefonos = 0;
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

            //Productos
            cmbProductos.ValueMember = "idProducto";
            cmbProductos.DisplayMember = "Producto";
            cmbProductos.DataSource = Catálogo.Productos;

            cmbProductos.SelectedIndex = -1;

          //  for (int i = 0; i < chkNivel.Items.Count; i++)
          //  {
          //      chkNivel.SetItemChecked(i, true);
          //  }
        }
        public void CambiaCartera()
        {
            if (cmbCarteras.DataSource == null)
                return;

            Catálogo.Productos.DefaultView.RowFilter = "idCartera = " + cmbCarteras.SelectedValue;
            cmbProductos.SelectedIndex = -1;}

            //for (int i = 0; i < chkNivel.Items.Count; i++)
            //{
            //    chkNivel.SetItemChecked(i, true);
            //}
       // }
       // public void AgregaEventosTxtKeyPress();
       // {
       //     txtSaldo.KeyPress += new KeyPressEventHandler(controlEvents.txtOnlyNumbers_KeyPress);
       //     txtRegistros.KeyPress += new KeyPressEventHandler(controlEvents.txtOnlyNumbers_KeyPress);
       

        public void Consulta()
        {
            Mensaje("Consulta en proceso, por favor espere...", Color.DimGray, lblMensajes);


            string Fecha = dTPFechaIni.Value.ToString("yyyy-MM-dd");
            string Fecha1 = dTPFechaFin.Value.ToString("yyyy-MM-dd");

            DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.[ConjurNet].[1.1.Reporte Ingresos] @idCartera,@idProducto,@idEjecutivo,@Valida,@Fecha, @Fecha1" );
            DataBaseConn.CommandParameters.AddWithValue("@idCartera", idCartera);
            DataBaseConn.CommandParameters.AddWithValue("@idProducto", idProducto);
            DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", idEjecutivo);
            DataBaseConn.CommandParameters.AddWithValue("@Valida", Valida);
            DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
            DataBaseConn.CommandParameters.AddWithValue("@Fecha1", Fecha1);
            // DataBaseConn.CommandParameters.AddWithValue("@Producto", Producto);
            // DataBaseConn.CommandParameters.AddWithValue("@Producto", Producto);
           // DataBaseConn.CommandParameters.AddWithValue("@Producto", Producto);
           // DataBaseConn.CommandParameters.AddWithValue("@Productos", Productos);
            //DataBaseConn.CommandParameters.AddWithValue("@Saldo", Saldo);
            //DataBaseConn.CommandParameters.AddWithValue("@Top", Top);
            //DataBaseConn.CommandParameters.AddWithValue("@Negociacion", Negociacion);
            //DataBaseConn.CommandParameters.AddWithValue("@Nivel", Nivel);
            //DataBaseConn.CommandParameters.AddWithValue("@Correos", Correos);
            //DataBaseConn.CommandParameters.AddWithValue("@Domicilios", Domicilios);
            //DataBaseConn.CommandParameters.AddWithValue("@Telefonos", Telefonos);

            DataSet tblAnalisis = new DataSet();
            if (!DataBaseConn.Fill(tblAnalisis, "ReporteIngresoConjurNet"))
            {
                Invoke((Action)delegate() {
                    btnConsulta.Visible = true;
                    picWait.Visible = false;
                    cmbProductos.Enabled = true;
                    cmbCarteras.Enabled = true;
                    //txtSaldo.Enabled = true;
                    //txtRegistros.Enabled = true;
                    //chkNegociadas.Enabled = true;
                    //chkNivel.Enabled = true;
                    //chkCorreos.Enabled = true;
                   // chkPorPeriodo.Enabled = true;
                    //chkTelefonos.Enabled = true;
                });
                TerminaBúsqueda("Falló la generación del reporte.", true);                
                return;
            }
                        
            string sRutaExcel = "";

            if (tblAnalisis.Tables.Count == 0 || tblAnalisis.Tables[0].Rows.Count == 0)
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
                tblAnalisis.Dispose();
                return;
            }

            string sResultado = ExcelXML.ExportToExcelSAX(ref tblAnalisis, sRutaExcel);
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
            cmbProductos.Enabled = true;
            cmbCarteras.Enabled = true;
            //txtSaldo.Enabled = true;
           // txtRegistros.Enabled = true;
            //chkNegociadas.Enabled = true;
            //chkNivel.Enabled = true;
            //chkCorreos.Enabled = true;
            //chkPorPeriodo.Enabled = true;
            //chkTelefonos.Enabled = true;      

            this.ControlBox = true;
        }
        #endregion

        #region Eventos
        private void btnConsulta_Click(object sender, EventArgs e)
        {
            //if (chkCorreos.Checked == false && chkDomicilios.Checked == false && chkTelefonos.Checked == false)
           // {
           //     Mensaje("Debe seleccionar al menos un proceso (Correos/Domicilios/Teléfonos).", Color.Crimson, lblMensajes);
           //     return;
           // }

            idCartera = Convert.ToInt32(cmbCarteras.SelectedValue);
            idProducto = Convert.ToInt32(cmbProductos.SelectedValue);
            idEjecutivo = Convert.ToInt32(Ejecutivo.Datos["idEjecutivo"].ToString());
           // Saldo = (txtSaldo.Text == "" ? 0 : int.Parse(txtSaldo.Text));
           // Top = (txtRegistros.Text == "" ? 0 : int.Parse(txtRegistros.Text));

            //if (chkNegociadas.Checked == true)
            //    Negociacion = true;
            //else
            //    Negociacion = false;

            //Nivel = "";
            //for (int x = 0; x <= chkNivel.CheckedItems.Count - 1; x++)
            //    Nivel = Nivel + "'" + chkNivel.CheckedItems[x].ToString() + "' ,";

           // if (Nivel.Length > 0)
           // {
           //     Nivel = Nivel.Substring(0, Nivel.Length - 1);                
           // }

           // if (chkCorreos.Checked == true)
           //     Correos = 1;
            //else
            //    Correos = 0;
            //
            //if (chkDomicilios.Checked == true)
            //    Domicilios = 1;
            //else
            //    Domicilios = 0;
            //
            //if (chkTelefonos.Checked == true)
            //    Telefonos = 1;
            //else
            //    Telefonos = 0;

            picWait.Visible = true;
            btnConsulta.Visible = false;
            cmbCarteras.Enabled = false;
            cmbProductos.Enabled = false;
            //txtSaldo.Enabled = false;
            //txtRegistros.Enabled = false;
            //chkNegociadas.Enabled = false;
            //chkNivel.Enabled = false;
            //chkCorreos.Enabled = false;
            //chkDomicilios.Enabled = false;
            //chkTelefonos.Enabled = false;


            DataBaseConn.StartThread(Consulta);
        }
        private void cmbCarteras_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiaCartera();
        }
        private void frmBotonera_FormClosed(object sender, FormClosedEventArgs e)
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

        //private void chkNivel_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //
        //}
        //
        //private void chkDomicilios_CheckedChanged(object sender, EventArgs e)
        //{
        //    if(chkPorPeriodo.Checked == true)
        //    {
        //        dTPFechaFin.Visible = true;
        //    }
        //    if(chkPorPeriodo.Checked == true)
        //    {
        //        dTPFechaIni.Visible = true;
        //    }
       // }

        private void lblFechaIni_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void chkPorPeriodo_CheckedChanged(object sender, EventArgs e)
        {
            if(chkPorPeriodo.Checked == true)
            {
                dTPFechaFin.Visible= true;
                dTPFechaIni.Visible = true;
                lblFechaIni.Visible = true;
                lblFechaFin.Visible = true;
                Valida = true;
            }
            else
            {
                dTPFechaIni.Visible = false;
                dTPFechaFin.Visible = false;
                lblFechaIni.Visible = false;
                lblFechaFin.Visible = false;
                Valida = false;

            }
        }







        public object Fecha { get; set; }
    }
}
