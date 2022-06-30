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


namespace Estadistica
{
    public partial class frmDesasignacion : Form
    {
        public frmDesasignacion()
        {
            InitializeComponent();
            //dtpMesRemesa.Format = DateTimePickerFormat.Custom;
            //dtpMesRemesa.CustomFormat = "MM/yyyy";
            PreparaVentana();
        }
        
        #region Variables
        int iNumberRecords;
        //DataTable tblGestiones = new DataTable();
        //dbFileReader lector;
        //string sCuentaError = "";
        string separadorArchivos = "";
        DataTable workTable = new DataTable();
        #endregion

        #region Delegados
        delegate void SetStringCallback(string text, Color color, Label label);
        #endregion

        #region Metodos
        public void PreparaVentana()
        {           
            dgvDia.DefaultCellStyle.Font = controlEvents.RowFont;
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
            //tblGestiones = new DataTable();

            if (!File.Exists(sFilePath)){
                Mensaje("No existe el archivo para leer.", Color.Red, lblMensajes);
                Invoke((Action)delegate() { picWait.Visible = false; btnArchivo.Visible = true; });
                return;
            }            

            
            if (rb1PV.Checked == true)
            {
                workTable = new DataTable();
                DataColumn CONSECUTIVO = new DataColumn("CONSECUTIVO");
                DataColumn NO_CUENTA = new DataColumn("NO_CUENTA");
                DataColumn TRASCODIFICADA = new DataColumn("TRASCODIFICADA");
                DataColumn NOMBRE_CLIENTE = new DataColumn("NOMBRE_CLIENTE");
                DataColumn RFC = new DataColumn("RFC");
                DataColumn AGENCIA = new DataColumn("AGENCIA");
                DataColumn FECHA_ASIGNACION = new DataColumn("FECHA_ASIGNACION");
                DataColumn MONTO_AGENCIA = new DataColumn("MONTO_AGENCIA");
                DataColumn DIA_CORTE = new DataColumn("DIA_CORTE");
                DataColumn PAGOS_VENCIDOS = new DataColumn("PAGOS_VENCIDOS");
                DataColumn DIAS_MOROSOS = new DataColumn("DIAS_MOROSOS");
                DataColumn TOTAL_DEUDOR = new DataColumn("TOTAL_DEUDOR");
                DataColumn MONTO_MOROSO = new DataColumn("MONTO_MOROSO");
                DataColumn PAGO_MINIMO = new DataColumn("PAGO_MINIMO");
                DataColumn CODIGO_BLOQUEO = new DataColumn("CODIGO_BLOQUEO");
                DataColumn FECHA_COD_BLOQUEO = new DataColumn("FECHA_COD_BLOQUEO");
                DataColumn STATUS_CLI = new DataColumn("STATUS_CLI");
                DataColumn BUC = new DataColumn("BUC");
                DataColumn FEC_ULT_GESTION = new DataColumn("FEC_ULT_GESTION");
                DataColumn COD_ACCION = new DataColumn("COD_ACCION");
                DataColumn COD_RESULTADO = new DataColumn("COD_RESULTADO");
                DataColumn MONTO_ULTIMO_PAGO = new DataColumn("MONTO_ULTIMO_PAGO");
                DataColumn FECHA_ULTIMO_PAGO = new DataColumn("FECHA_ULTIMO_PAGO");
                DataColumn PLAZO_PACTADO = new DataColumn("PLAZO_PACTADO");
                DataColumn TASA = new DataColumn("TASA");
                DataColumn MENSUALIDAD = new DataColumn("MENSUALIDAD");
                DataColumn FECHA_ALTA_REFIN = new DataColumn("FECHA_ALTA_REFIN");
                DataColumn ORIGEN_REFIN = new DataColumn("ORIGEN_REFIN");
                DataColumn STATUS_CUMPLIMIENTO = new DataColumn("STATUS_CUMPLIMIENTO");
                DataColumn GRUPO_ECONOMICO = new DataColumn("GRUPO_ECONOMICO");
                DataColumn DIAS_ASIGNACION = new DataColumn("DIAS_ASIGNACION");
                DataColumn FECHA_DESASIGNACION = new DataColumn("FECHA_DESASIGNACION");
                DataColumn CLASIF = new DataColumn("CLASIF");
                DataColumn C_P = new DataColumn("C_P");
                DataColumn LADA1 = new DataColumn("LADA1");
                DataColumn TEL1 = new DataColumn("TEL1");
                DataColumn LADA2 = new DataColumn("LADA2");
                DataColumn TEL2 = new DataColumn("TEL2");
                DataColumn LADA3 = new DataColumn("LADA3");
                DataColumn TEL3 = new DataColumn("TEL3");
                DataColumn ULTIMO_SALDO_MES_ANTERIOR = new DataColumn("ULTIMO_SALDO_MES_ANTERIOR");
                DataColumn BHSCOR = new DataColumn("BHSCOR");
                DataColumn PV_1MES_ATRAS = new DataColumn("PV_1MES_ATRAS");
                DataColumn PV_2MES_ATRAS = new DataColumn("PV_2MES_ATRAS");
                DataColumn PV_3MES_ATRAS = new DataColumn("PV_3MES_ATRAS");
                DataColumn PV_4MES_ATRAS = new DataColumn("PV_4MES_ATRAS");
                DataColumn PV_5MES_ATRAS = new DataColumn("PV_5MES_ATRAS");
                DataColumn PV_6MES_ATRAS = new DataColumn("PV_6MES_ATRAS");
                DataColumn FECHA_CASTIGO = new DataColumn("FECHA_CASTIGO");
                DataColumn OBSERVACIONES = new DataColumn("OBSERVACIONES");
                DataColumn SALDO_VENCIDO_30_DIAS = new DataColumn("SALDO_VENCIDO_30_DIAS");
                DataColumn SALDO_VENCIDO_60_DIAS = new DataColumn("SALDO_VENCIDO_60_DIAS");
                DataColumn SALDO_VENCIDO_90_DIAS = new DataColumn("SALDO_VENCIDO_90_DIAS");
                DataColumn SALDO_VENCIDO_120_DIAS = new DataColumn("SALDO_VENCIDO_120_DIAS");

                workTable.Columns.Add(CONSECUTIVO);
                workTable.Columns.Add(NO_CUENTA);
                workTable.Columns.Add(TRASCODIFICADA);
                workTable.Columns.Add(NOMBRE_CLIENTE);
                workTable.Columns.Add(RFC);
                workTable.Columns.Add(AGENCIA);
                workTable.Columns.Add(FECHA_ASIGNACION);
                workTable.Columns.Add(MONTO_AGENCIA);
                workTable.Columns.Add(DIA_CORTE);
                workTable.Columns.Add(PAGOS_VENCIDOS);
                workTable.Columns.Add(DIAS_MOROSOS);
                workTable.Columns.Add(TOTAL_DEUDOR);
                workTable.Columns.Add(MONTO_MOROSO);
                workTable.Columns.Add(PAGO_MINIMO);
                workTable.Columns.Add(CODIGO_BLOQUEO);
                workTable.Columns.Add(FECHA_COD_BLOQUEO);
                workTable.Columns.Add(STATUS_CLI);
                workTable.Columns.Add(BUC);
                workTable.Columns.Add(FEC_ULT_GESTION);
                workTable.Columns.Add(COD_ACCION);
                workTable.Columns.Add(COD_RESULTADO);
                workTable.Columns.Add(MONTO_ULTIMO_PAGO);
                workTable.Columns.Add(FECHA_ULTIMO_PAGO);
                workTable.Columns.Add(PLAZO_PACTADO);
                workTable.Columns.Add(TASA);
                workTable.Columns.Add(MENSUALIDAD);
                workTable.Columns.Add(FECHA_ALTA_REFIN);
                workTable.Columns.Add(ORIGEN_REFIN);
                workTable.Columns.Add(STATUS_CUMPLIMIENTO);
                workTable.Columns.Add(GRUPO_ECONOMICO);
                workTable.Columns.Add(DIAS_ASIGNACION);
                workTable.Columns.Add(FECHA_DESASIGNACION);
                workTable.Columns.Add(CLASIF);
                workTable.Columns.Add(C_P);
                workTable.Columns.Add(LADA1);
                workTable.Columns.Add(TEL1);
                workTable.Columns.Add(LADA2);
                workTable.Columns.Add(TEL2);
                workTable.Columns.Add(LADA3);
                workTable.Columns.Add(TEL3);
                workTable.Columns.Add(ULTIMO_SALDO_MES_ANTERIOR);
                workTable.Columns.Add(BHSCOR);
                workTable.Columns.Add(PV_1MES_ATRAS);
                workTable.Columns.Add(PV_2MES_ATRAS);
                workTable.Columns.Add(PV_3MES_ATRAS);
                workTable.Columns.Add(PV_4MES_ATRAS);
                workTable.Columns.Add(PV_5MES_ATRAS);
                workTable.Columns.Add(PV_6MES_ATRAS);
                workTable.Columns.Add(FECHA_CASTIGO);
                workTable.Columns.Add(OBSERVACIONES);
                workTable.Columns.Add(SALDO_VENCIDO_30_DIAS);
                workTable.Columns.Add(SALDO_VENCIDO_60_DIAS);
                workTable.Columns.Add(SALDO_VENCIDO_90_DIAS);
                workTable.Columns.Add(SALDO_VENCIDO_120_DIAS);


                separadorArchivos = ",";

                string FileToRead = sFilePath;
                using (StreamReader ReaderObject = new StreamReader(FileToRead))
                {
                    string line;
                    // ReaderObject reads a single line, stores it in Line string variable and then displays it on console
                    while ((line = ReaderObject.ReadLine()) != null)
                    {
                        try
                        {
                            //line = sr.ReadLine();

                            line = line.Replace("?", "");
                            line = line.Replace("\"", "");
                            line = line.Replace("\t", separadorArchivos);
                            string[] valores = line.Split(Convert.ToChar(","));
                            DataRow row1 = workTable.NewRow();
                            row1["CONSECUTIVO"] = valores[0].ToString();
                            row1["NO_CUENTA"] = valores[1].ToString();
                            row1["TRASCODIFICADA"] = valores[2].ToString();
                            row1["NOMBRE_CLIENTE"] = valores[3].ToString();
                            row1["RFC"] = valores[4].ToString();
                            row1["AGENCIA"] = valores[5].ToString();
                            row1["FECHA_ASIGNACION"] = valores[6].ToString();
                            row1["MONTO_AGENCIA"] = valores[7].ToString();
                            row1["DIA_CORTE"] = valores[8].ToString();
                            row1["PAGOS_VENCIDOS"] = valores[9].ToString();
                            row1["DIAS_MOROSOS"] = valores[10].ToString();
                            row1["TOTAL_DEUDOR"] = valores[11].ToString();
                            row1["MONTO_MOROSO"] = valores[12].ToString();
                            row1["PAGO_MINIMO"] = valores[13].ToString();
                            row1["CODIGO_BLOQUEO"] = valores[14].ToString();
                            row1["FECHA_COD_BLOQUEO"] = valores[15].ToString();
                            row1["STATUS_CLI"] = valores[16].ToString();
                            row1["BUC"] = valores[17].ToString();
                            row1["FEC_ULT_GESTION"] = valores[18].ToString();
                            row1["COD_ACCION"] = valores[19].ToString();
                            row1["COD_RESULTADO"] = valores[20].ToString();
                            row1["MONTO_ULTIMO_PAGO"] = valores[21].ToString();
                            row1["FECHA_ULTIMO_PAGO"] = valores[22].ToString();
                            row1["PLAZO_PACTADO"] = valores[23].ToString();
                            row1["TASA"] = valores[24].ToString();
                            row1["MENSUALIDAD"] = valores[25].ToString();
                            row1["FECHA_ALTA_REFIN"] = valores[26].ToString();
                            row1["ORIGEN_REFIN"] = valores[27].ToString();
                            row1["STATUS_CUMPLIMIENTO"] = valores[28].ToString();
                            row1["GRUPO_ECONOMICO"] = valores[29].ToString();
                            row1["DIAS_ASIGNACION"] = valores[30].ToString();
                            row1["FECHA_DESASIGNACION"] = valores[31].ToString();
                            row1["CLASIF"] = valores[32].ToString();
                            row1["C_P"] = valores[33].ToString();
                            row1["LADA1"] = valores[34].ToString();
                            row1["TEL1"] = valores[35].ToString();
                            row1["LADA2"] = valores[36].ToString();
                            row1["TEL2"] = valores[37].ToString();
                            row1["LADA3"] = valores[38].ToString();
                            row1["TEL3"] = valores[39].ToString();
                            row1["ULTIMO_SALDO_MES_ANTERIOR"] = valores[40].ToString();
                            row1["BHSCOR"] = valores[41].ToString();
                            row1["PV_1MES_ATRAS"] = valores[42].ToString();
                            row1["PV_2MES_ATRAS"] = valores[43].ToString();
                            row1["PV_3MES_ATRAS"] = valores[44].ToString();
                            row1["PV_4MES_ATRAS"] = valores[45].ToString();
                            row1["PV_5MES_ATRAS"] = valores[46].ToString();
                            row1["PV_6MES_ATRAS"] = valores[47].ToString();
                            row1["FECHA_CASTIGO"] = valores[48].ToString();
                            row1["OBSERVACIONES"] = valores[49].ToString();
                            row1["SALDO_VENCIDO_30_DIAS"] = valores[50].ToString();
                            row1["SALDO_VENCIDO_60_DIAS"] = valores[51].ToString();
                            row1["SALDO_VENCIDO_90_DIAS"] = valores[52].ToString();
                            row1["SALDO_VENCIDO_120_DIAS"] = valores[53].ToString();

                            workTable.Rows.Add(row1);
                        }
                        catch { }
                    }
                }
            }
            else
            {
                workTable = new DataTable();
                DataColumn CONSECUTIVO = new DataColumn("CONSECUTIVO");
                DataColumn NO_CUENTA = new DataColumn("NO_CUENTA");
                DataColumn TRASCODIFICADA = new DataColumn(" TRASCODIFICADA");
                DataColumn NOMBRE_CLIENTE = new DataColumn(" NOMBRE_CLIENTE");
                DataColumn RFC = new DataColumn(" RFC");
                DataColumn AGENCIA = new DataColumn(" AGENCIA");
                DataColumn FECHA_ASIGNACION = new DataColumn(" FECHA_ASIGNACION");
                DataColumn FECHA_DESASIGNACION = new DataColumn(" FECHA_DESASIGNACION");
                DataColumn DIAS_AGENCIA = new DataColumn(" DIAS_AGENCIA");
                DataColumn MONTO_AGENCIA = new DataColumn(" MONTO_AGENCIA");
                DataColumn TOTAL_DEUDOR = new DataColumn(" TOTAL_DEUDOR");
                DataColumn SALDO_MES = new DataColumn(" SALDO_MES");
                DataColumn SALDO_VENCIDO = new DataColumn(" SALDO_VENCIDO");
                DataColumn CUENTA_CHEQUES = new DataColumn(" CUENTA_CHEQUES");
                DataColumn PAGOS_VENCIDOS = new DataColumn(" PAGOS_VENCIDOS");
                DataColumn DIAS_MORA = new DataColumn(" DIAS_MORA");
                DataColumn STATUS_CLI = new DataColumn(" STATUS_CLI");
                DataColumn TIPO_FACTURACION = new DataColumn(" TIPO_FACTURACION");
                DataColumn BUC = new DataColumn(" BUC");
                DataColumn FEC_ULT_GESTION = new DataColumn(" FEC_ULT_GESTION");
                DataColumn COD_ACCION = new DataColumn(" COD_ACCION");
                DataColumn COD_RESULTADO = new DataColumn(" COD_RESULTADO");
                DataColumn PROGRAMA_ESPECIAL = new DataColumn(" PROGRAMA_ESPECIAL");
                DataColumn FECHA_PROXIMO_VENCIMIENTO = new DataColumn(" FECHA_PROXIMO_VENCIMIENTO");
                DataColumn FECHA_ULTIMO_PAGO = new DataColumn(" FECHA_ULTIMO_PAGO");
                DataColumn MONTO_ULTIMO_PAGO = new DataColumn(" MONTO_ULTIMO_PAGO");
                DataColumn DIAS_ASIGNACION = new DataColumn(" DIAS_ASIGNACION");
                DataColumn STACTECA = new DataColumn(" STACTECA");
                DataColumn FECSTACA = new DataColumn(" FECSTACA");
                DataColumn FECHA_CASTIGO = new DataColumn(" FECHA_CASTIGO");
                DataColumn CLASIF = new DataColumn(" CLASIF");
                DataColumn C_P = new DataColumn(" C_P");
                DataColumn CLAVE_PRODUCTO = new DataColumn(" CLAVE_PRODUCTO");
                DataColumn PRODUCTO = new DataColumn(" PRODUCTO");
                DataColumn OBSERVACIONES = new DataColumn("OBSERVACIONES");
                DataColumn SALDO_VENCIDO_30_DIAS = new DataColumn("SALDO_VENCIDO_30_DIAS");
                DataColumn SALDO_VENCIDO_60_DIAS = new DataColumn("SALDO_VENCIDO_60_DIAS");
                DataColumn SALDO_VENCIDO_90_DIAS = new DataColumn("SALDO_VENCIDO_90_DIAS");
                DataColumn SALDO_VENCIDO_120_DIAS = new DataColumn("SALDO_VENCIDO_120_DIAS");


                workTable.Columns.Add(CONSECUTIVO);
                workTable.Columns.Add(NO_CUENTA);
                workTable.Columns.Add(TRASCODIFICADA);
                workTable.Columns.Add(NOMBRE_CLIENTE);
                workTable.Columns.Add(RFC);
                workTable.Columns.Add(AGENCIA);
                workTable.Columns.Add(FECHA_ASIGNACION);
                workTable.Columns.Add(FECHA_DESASIGNACION);
                workTable.Columns.Add(DIAS_AGENCIA);
                workTable.Columns.Add(MONTO_AGENCIA);
                workTable.Columns.Add(TOTAL_DEUDOR);
                workTable.Columns.Add(SALDO_MES);
                workTable.Columns.Add(SALDO_VENCIDO);
                workTable.Columns.Add(CUENTA_CHEQUES);
                workTable.Columns.Add(PAGOS_VENCIDOS);
                workTable.Columns.Add(DIAS_MORA);
                workTable.Columns.Add(STATUS_CLI);
                workTable.Columns.Add(TIPO_FACTURACION);
                workTable.Columns.Add(BUC);
                workTable.Columns.Add(FEC_ULT_GESTION);
                workTable.Columns.Add(COD_ACCION);
                workTable.Columns.Add(COD_RESULTADO);
                workTable.Columns.Add(PROGRAMA_ESPECIAL);
                workTable.Columns.Add(FECHA_PROXIMO_VENCIMIENTO);
                workTable.Columns.Add(FECHA_ULTIMO_PAGO);
                workTable.Columns.Add(MONTO_ULTIMO_PAGO);
                workTable.Columns.Add(DIAS_ASIGNACION);
                workTable.Columns.Add(STACTECA);
                workTable.Columns.Add(FECSTACA);
                workTable.Columns.Add(FECHA_CASTIGO);
                workTable.Columns.Add(CLASIF);
                workTable.Columns.Add(C_P);
                workTable.Columns.Add(CLAVE_PRODUCTO);
                workTable.Columns.Add(PRODUCTO);
                workTable.Columns.Add(OBSERVACIONES);
                workTable.Columns.Add(SALDO_VENCIDO_30_DIAS);
                workTable.Columns.Add(SALDO_VENCIDO_60_DIAS);
                workTable.Columns.Add(SALDO_VENCIDO_90_DIAS);
                workTable.Columns.Add(SALDO_VENCIDO_120_DIAS);



                separadorArchivos = ",";

                string FileToRead = sFilePath;
                using (StreamReader ReaderObject = new StreamReader(FileToRead))
                {
                    string line;
                    // ReaderObject reads a single line, stores it in Line string variable and then displays it on console
                    while ((line = ReaderObject.ReadLine()) != null)
                    {
                        try
                        {
                            line = line.Replace("?", "");
                            line = line.Replace("\"", "");
                            line = line.Replace("\t", separadorArchivos);
                            string[] valores = line.Split(Convert.ToChar(","));
                            DataRow row1 = workTable.NewRow();

                            row1["CONSECUTIVO"] = valores[0].ToString();
                            row1["NO_CUENTA"] = valores[1].ToString();
                            row1[" TRASCODIFICADA"] = valores[2].ToString().Trim();
                            row1[" NOMBRE_CLIENTE"] = valores[3].ToString();
                            row1[" RFC"] = valores[4].ToString();
                            row1[" AGENCIA"] = valores[5].ToString();
                            row1[" FECHA_ASIGNACION"] = valores[6].ToString();
                            row1[" FECHA_DESASIGNACION"] = valores[7].ToString();
                            row1[" DIAS_AGENCIA"] = valores[8].ToString();
                            row1[" MONTO_AGENCIA"] = valores[9].ToString();
                            row1[" TOTAL_DEUDOR"] = valores[10].ToString();
                            row1[" SALDO_MES"] = valores[11].ToString();
                            row1[" SALDO_VENCIDO"] = valores[12].ToString();
                            row1[" CUENTA_CHEQUES"] = valores[13].ToString();
                            row1[" PAGOS_VENCIDOS"] = valores[14].ToString();
                            row1[" DIAS_MORA"] = valores[15].ToString();
                            row1[" STATUS_CLI"] = valores[16].ToString();
                            row1[" TIPO_FACTURACION"] = valores[17].ToString();
                            row1[" BUC"] = valores[18].ToString();
                            row1[" FEC_ULT_GESTION"] = valores[19].ToString();
                            row1[" COD_ACCION"] = valores[20].ToString();
                            row1[" COD_RESULTADO"] = valores[21].ToString();
                            row1[" PROGRAMA_ESPECIAL"] = valores[22].ToString();
                            row1[" FECHA_PROXIMO_VENCIMIENTO"] = valores[23].ToString();
                            row1[" FECHA_ULTIMO_PAGO"] = valores[24].ToString();
                            row1[" MONTO_ULTIMO_PAGO"] = valores[25].ToString();
                            row1[" DIAS_ASIGNACION"] = valores[26].ToString();
                            row1[" STACTECA"] = valores[27].ToString();
                            row1[" FECSTACA"] = valores[28].ToString();
                            row1[" FECHA_CASTIGO"] = valores[29].ToString();
                            row1[" CLASIF"] = valores[30].ToString();
                            row1[" C_P"] = valores[31].ToString();
                            row1[" CLAVE_PRODUCTO"] = valores[32].ToString();
                            row1[" PRODUCTO"] = valores[33].ToString();
                            row1["OBSERVACIONES"] = valores[34].ToString();
                            row1["SALDO_VENCIDO_30_DIAS"] = valores[35].ToString();
                            row1["SALDO_VENCIDO_60_DIAS"] = valores[36].ToString();
                            row1["SALDO_VENCIDO_90_DIAS"] = valores[37].ToString();
                            row1["SALDO_VENCIDO_120_DIAS"] = valores[38].ToString();


                            workTable.Rows.Add(row1);
                        }
                        catch (IOException e) { }
                    }

                }

            }

            string[] Layout;

            if (rb1PV.Checked == true)
                Layout = new string[] { "CONSECUTIVO","NO_CUENTA","TRASCODIFICADA","NOMBRE_CLIENTE","RFC","AGENCIA","FECHA_ASIGNACION","MONTO_AGENCIA","DIA_CORTE","PAGOS_VENCIDOS","DIAS_MOROSOS","TOTAL_DEUDOR","MONTO_MOROSO","PAGO_MINIMO","CODIGO_BLOQUEO","FECHA_COD_BLOQUEO","STATUS_CLI","BUC","FEC_ULT_GESTION","COD_ACCION","COD_RESULTADO","MONTO_ULTIMO_PAGO","FECHA_ULTIMO_PAGO","PLAZO_PACTADO","TASA","MENSUALIDAD","FECHA_ALTA_REFIN","ORIGEN_REFIN","STATUS_CUMPLIMIENTO","GRUPO_ECONOMICO","DIAS_ASIGNACION","FECHA_DESASIGNACION","CLASIF","C_P","LADA1","TEL1","LADA2","TEL2","LADA3","TEL3","ULTIMO_SALDO_MES_ANTERIOR","BHSCOR","PV_1MES_ATRAS","PV_2MES_ATRAS","PV_3MES_ATRAS","PV_4MES_ATRAS","PV_5MES_ATRAS","PV_6MES_ATRAS","FECHA_CASTIGO","OBSERVACIONES","SALDO_VENCIDO_30_DIAS","SALDO_VENCIDO_60_DIAS","SALDO_VENCIDO_90_DIAS","SALDO_VENCIDO_120_DIAS" };
            else
                Layout = new string[] { "CONSECUTIVO","NO_CUENTA","TRASCODIFICADA","NOMBRE_CLIENTE","RFC","AGENCIA","FECHA_ASIGNACION","FECHA_DESASIGNACION","DIAS_AGENCIA","MONTO_AGENCIA","TOTAL_DEUDOR","SALDO_MES","SALDO_VENCIDO","CUENTA_CHEQUES","PAGOS_VENCIDOS","DIAS_MORA","STATUS_CLI","TIPO_FACTURACION","BUC","FEC_ULT_GESTION","COD_ACCION","COD_RESULTADO","PROGRAMA_ESPECIAL","FECHA_PROXIMO_VENCIMIENTO","FECHA_ULTIMO_PAGO","MONTO_ULTIMO_PAGO","DIAS_ASIGNACION","STACTECA","FECSTACA","FECHA_CASTIGO","CLASIF","C_P","CLAVE_PRODUCTO","PRODUCTO","OBSERVACIONES","SALDO_VENCIDO_30_DIAS","SALDO_VENCIDO_60_DIAS","SALDO_VENCIDO_90_DIAS","SALDO_VENCIDO_120_DIAS"};

            for (int i = 0; i < workTable.Columns.Count; i++)
                workTable.Columns[i].ColumnName = workTable.Columns[i].ColumnName.Replace(" ", "").Trim();


            foreach (string sColumna in Layout)
                if (!workTable.Columns.Contains(sColumna))
                {
                    Mensaje("Falta la columna " + sColumna + " en el archivo. Verifique el layout.", Color.Red, lblMensajes);
                    Invoke((Action)delegate () { picWait.Visible = false; btnArchivo.Visible = true; });
                    return;
                }

            iNumberRecords = workTable.Rows.Count;
            Mensaje("Archivo con " + iNumberRecords.ToString("#,0") + " registros y " + workTable.Columns.Count.ToString("#,0") + " columnas.", Color.DimGray, lblRegistros);
            Mensaje("Verifique la información y presione Cargar.", Color.SlateGray, lblInstrucciones);


            Invoke((Action)delegate(){
                dgvDia.DataSource = workTable;                
                dgvDia.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                picWait.Visible = false;
                btnCargar.Visible = btnArchivo.Visible = dgvDia.Visible = true;
            });

            Mensaje("", Color.DimGray, lblMensajes);
        }

       
        public void CargaRemesa(object oDatos)
        {
            string Layout = (rb1PV.Checked == true ? "1PV" : "AUTO");
            Mensaje("Creando tabla temporal, por favor espere...", Color.Green, lblMensajes);
            DataBaseConn.SetCommand("IF EXISTS (SELECT 1 FROM dbEstadistica.sys.tables WHERE name = 'Layout" + Layout + "' AND schema_id=6 ) " +
                " DROP TABLE dbEstadistica.Temp.Layout" + Layout);

            if (!DataBaseConn.Execute("DropTemporalRemesa")){
                Mensaje("Falló al eliminar tabla temporal.", Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            string sCreateTable = "CREATE TABLE dbEstadistica.Temp.Layout" + Layout + " ( ";
            for (int i = 0; i < workTable.Columns.Count; i++){
                //if (tblGestiones.Columns[i].ColumnName == "Fecha Asig" || tblGestiones.Columns[i].ColumnName == "Fecha Prom" || tblGestiones.Columns[i].ColumnName == "Fecha Sig Gest" || tblGestiones.Columns[i].ColumnName == "Fecha Ult Diag" || tblGestiones.Columns[i].ColumnName == "Fecha Ult Gest" || tblGestiones.Columns[i].ColumnName == "Fecha Ult Coment" || tblGestiones.Columns[i].ColumnName == "Fecha Venc Asig" || tblGestiones.Columns[i].ColumnName == "Fecha Visita" || tblGestiones.Columns[i].ColumnName == "DatosLocalizacion_Fecha" || tblGestiones.Columns[i].ColumnName == "Fecha Inicio Vigencia Mitigante" || tblGestiones.Columns[i].ColumnName == "Fecha Termino Vigencia Mitigante" || tblGestiones.Columns[i].ColumnName == "Fecha Inicio Demanda" || tblGestiones.Columns[i].ColumnName == "Fec_Bancomalo" || tblGestiones.Columns[i].ColumnName == "F_Alta_Credito" || tblGestiones.Columns[i].ColumnName == "Fecha ultimo Pago" || tblGestiones.Columns[i].ColumnName == "Fecha de confirmación de asignación" || tblGestiones.Columns[i].ColumnName == "DatosLocalizacion_Fecha")
                if (workTable.Columns[i].ColumnName == "Fecha Asig")
                    sCreateTable += ("\r\n [" + workTable.Columns[i] + "] DATETIME NULL, ");
                else
                    sCreateTable += ("\r\n [" + workTable.Columns[i] + "] VARCHAR(8000) NULL, ");
            }

            //sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ", idRegistro INT NOT NULL IDENTITY(1,1) PRIMARY KEY CLUSTERED );";
            sCreateTable = sCreateTable.TrimEnd(new char[] { ' ', ',' }) + ")" ;

            DataBaseConn.SetCommand(sCreateTable);
            if (!DataBaseConn.Execute("CreaTemporalRem")){
                Mensaje("Falló al crear tabla temporal.", Color.Crimson, lblMensajes);
                Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                return;
            }

            Mensaje("Insertando datos, por favor espere...", Color.DimGray, lblMensajes);
            using (SqlBulkCopy blkData = DataBaseConn.bulkCopy){{
                    try{
                        DataBaseConn.ConnectSQL(true);
                        blkData.SqlRowsCopied -= new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);
                        blkData.NotifyAfter = 317;
                        blkData.DestinationTableName = "dbEstadistica.Temp.Layout" + Layout;
                        blkData.BulkCopyTimeout = 30 * 60;
                        blkData.WriteToServer(workTable);
                    }
                    catch (Exception ex){
                        Mensaje("Falló al insertar los datos, " + ex.ToString(), Color.Red, lblMensajes);
                        Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                        return;
                    }
                }
                Mensaje("Se han transferido " + iNumberRecords.ToString("N0") + " / " + iNumberRecords.ToString("N0") + ". 100 %", Color.DimGray, lblRegistros);

                string Fecha = dtpDia.Value.ToString("yyyy-MM-dd");

                Mensaje("Insertando gestiones, por favor espere...", Color.DimGray, lblMensajes);
                DataBaseConn.SetCommand("WAITFOR DELAY '00:00:00'; EXEC dbEstadistica.Santander.[1.8.Desasignaciones] @idEjecutivo, @Fecha, @Producto");
                DataBaseConn.CommandParameters.AddWithValue("@idEjecutivo", Ejecutivo.Datos["idEjecutivo"]);
                DataBaseConn.CommandParameters.AddWithValue("@Fecha", Fecha);
                DataBaseConn.CommandParameters.AddWithValue("@Producto", Layout);

                if (!DataBaseConn.Execute("InsertaDesasignacion")){
                    Mensaje("Falló al insertar la información.", Color.Red, lblMensajes);
                    //lector.Close();
                    Invoke((Action)delegate() { btnArchivo.Visible = true; picWaitBulk.Visible = false; });
                    return;
                }

                Mensaje("La desasignación se insertó en el sistema.", Color.DimGray, lblMensajes);

                //lector.Close();

                Invoke((Action)delegate{
                    btnArchivo.Visible = true;
                    picWaitBulk.Visible = false;
                    dgvDia.DataSource = null;
                    lblInstrucciones.Text = "";
                    lblRegistros.Text = "";
                    txtRuta.Text = "";
                });
            }
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            Mensaje("Transfiriendo... " + e.RowsCopied.ToString("N0") + "/" + iNumberRecords.ToString("N0") + " registros. " + (e.RowsCopied / (float)iNumberRecords * 100).ToString("N2") + " %", Color.DimGray, lblRegistros);
        }
        #endregion

        #region Eventos
        private void btnArchivo_Click(object sender, EventArgs e)
        {
            LimpiaMensajes();
            dgvDia.DataSource = null;

            if (ofdArchivo.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel){
                Mensaje("Proceso cancelado.", Color.DimGray, lblMensajes);
                return;
            }
            txtRuta.Text = ofdArchivo.FileName;
            Mensaje("Cargando vista previa del archivo de texto, por favor espere...", Color.DimGray, lblMensajes);
            btnCargar.Visible = btnArchivo.Visible = false;
            picWait.Visible = true;
            DataBaseConn.StartThread(Archivo, ofdArchivo.FileName);
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            Mensaje("", Color.DimGray, lblInstrucciones);
            btnCargar.Visible = false;
            picWaitBulk.Visible = true;
            btnArchivo.Visible = false;
            DataBaseConn.StartThread(CargaRemesa);
        }

        private void frmRemesa_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBaseConn.CancelRunningQuery();
            //if (lector != null)
            //    lector.Close();
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
