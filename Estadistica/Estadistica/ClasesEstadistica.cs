using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.Cryptography;



namespace Estadistica {
    
    #region DataBaseConn    
    /// Clase para conexión a la base de datos.    
    static class DataBaseConn {

        public static Version AppVersion = new Version(
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major,
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor,
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Build
            );

        
        private static string _AppName = "Estadistica";
        public static string AppName { get { return _AppName; } }
        public static string TempFileName { get { return "~" + _AppName + ".temp"; } }
        public static string TempFilePath { get { return AppDomain.CurrentDomain.BaseDirectory + TempFileName; } }
        public static string LocalFolder { get { return System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CJ\"; } }
        private static string _NetworkFolder;
        public static string NetworkFolder { get { return _NetworkFolder; } set { _NetworkFolder = value; } }

        private static string sDataBaseName = "";
        public static string DataBaseName { get { return sDataBaseName; } }

        private static string sServer = "";        
        public static string Server { get { return sServer; } }
        //public static string _sOdiServidor = (sServer.Length > 0 ? sServer.Substring(sServer.Length - 1, 1) : "");
        public static string ServerNum { get { return sServer.Length > 0 ? sServer.Substring(sServer.Length - 1, 1) : ""; } }
        //string ServerNumOdi = (sServer.Length > 0 ? sServer.Substring(sServer.Length - 1, 1) : "");

        

        private static int _idLogError = 0;
        public static int idLogError { get { return _idLogError; } }

        private static string _ErrorString = "";

        private static DateTime _Hoy = DateTime.Today;

        public static bool EjecuciónConHilos = true;

        private static SqlConnection _conDataBase = null;

        //string sServidor = "";
        
        /// Bulk copy para inserción a base de datos. 
        public static SqlBulkCopy bulkCopy { get { return new SqlBulkCopy(_conDataBase); } }

        private static bool _EnEjecución = false;        
        /// Indica si la conexión está en ejecución.
        public static bool EnEjecución { get { return _EnEjecución; } }

        private static object _scalar;        
        /// Objeto escalar del último resultado de la ejecución.        
        public static object Escalar { get { return _scalar; } }

        private static SqlCommand _command;
        private static SqlDataAdapter _dataAdapter;        
        /// Parámetros de la consulta.        
        public static SqlParameterCollection CommandParameters { get { return _command.Parameters; } }

        #region ErrorLog
        static string _Module = "",
                        _Domain = "",
                        _User = "",
                        _Machine = "",
                        _IP = "",
                        _Query = "",
                        _idEjecutivo = "0"
                        ;

        /// <summary>
        /// Ejecuta el hilo que guardar el error.
        /// </summary>
        /// <param name="Module">Módulo en donde se generó el error.</param>
        /// <param name="ErrorString">Texto con el error a guardar.</param>
        static void LogError(string Module, string ErrorString, string Query) {

            _Module = "Estadistica: v" + DataBaseConn.AppVersion + ": " + Module;
            _ErrorString = ErrorString;
            _Query = Query;
            _idLogError = 0;

            if ( Ejecutivo.Datos != null && Ejecutivo.Datos.Table.Columns.Contains("idEjecutivo") )
                _idEjecutivo = Ejecutivo.Datos["idEjecutivo"].ToString();

            //Thread hiloError = new Thread(SaveError);
            //hiloError.IsBackground = true;
            //hiloError.Start();
            SaveError();
        }
        static void SaveError() {

            string sDataBaseError = "";

            if ( _IP == "" )
                _IP = Funciones.GetIP();
            if ( _User == "" ) {
                _Domain = Environment.UserDomainName;
                _Machine = Environment.MachineName;
                _User = Environment.UserName;
            }


            try {
                ConnectSQL(true);

                SqlCommand comInsertError = new SqlCommand("USE dbEstadistica; INSERT INTO dbEstadistica..LogErrorApp ( Dominio, Computadora, Usuario, idEjecutivo, IP, FechaHora, Módulo, Query, Error) VALUES  ( '" +
                    _Domain + "','" +
                    _Machine + "','" +
                    _User + "'," +
                    _idEjecutivo + ",'" +
                    _IP + "','" +
                     DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" +
                    _Module + "', " +
                    " @Query, " +
                    " @ErrorString ) SELECT @@IDENTITY"
                , _conDataBase);
                comInsertError.Parameters.AddWithValue("@ErrorString", _ErrorString);
                comInsertError.Parameters.AddWithValue("@Query", _Query);
                _idLogError = Convert.ToInt32(comInsertError.ExecuteScalar());
            } catch ( Exception ex ) {
                sDataBaseError = ex.Message;
            }

            try {
                System.IO.Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CJ\Estadistica\");
                System.IO.File.AppendAllText(System.Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\CJ\Estadistica\" + "Error_Estadistica_" + DateTime.Now.ToString("dd-MM-yyyy") + ".log",
                    "\r\n" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                    + "\r\n Computadora: " + _Domain + @"\" + _Machine + @" - " + _IP
                    + "\r\n Usuario: " + _User
                    + "\r\n Módulo: " + _Module
                    + "\r\n Error: " + _ErrorString
                    + "\r\n DB Log Error: " + sDataBaseError + "\r\n");
            } catch ( Exception ex ) {
                ex.Message.ToString();
                //MessageBox.Show("Falló al guardar Error Log.\r\n" + ex.Message, "Error Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        #endregion

        /// <summary>
        /// Desencripta el archivo de conexión.
        /// </summary>
        /// <param name="FilePath">Ruta donde se encuentra el archivo temp.</param>
        /// <returns></returns>
        public static string DecriptTempFileConnection(string FilePath) {

            string textoEncriptado = "";
            try {
                textoEncriptado = System.IO.File.ReadAllText(FilePath);
                string sLlave = "oicrosnoC";
                byte[] keyArray;
                byte[] ArregloACifrar = Convert.FromBase64String(textoEncriptado);

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(System.Text.UTF8Encoding.UTF8.GetBytes(sLlave));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider triple = new TripleDESCryptoServiceProvider();

                triple.Key = keyArray;
                triple.Mode = CipherMode.ECB;
                triple.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = triple.CreateDecryptor();

                byte[] resultado = cTransform.TransformFinalBlock(ArregloACifrar, 0, ArregloACifrar.Length);

                triple.Clear();
                textoEncriptado = System.Text.UTF8Encoding.UTF8.GetString(resultado);                

            } catch ( Exception ex ) {
                ex.Message.ToString();
            }
            //sServidor = textoEncriptado;
            return textoEncriptado;
        }

        /// <summary>
        /// Establece la conexión por primera vedbMemory.AMS.
        /// </summary>
        // string sConection = 
        public static bool CreateConnection() {
            if ( _conDataBase != null )
                return true;

            string sConection = DecriptTempFileConnection(TempFilePath) + AppVersion;

            if ( sConection == AppVersion.ToString() )
                return false;

            SqlConnection conDB = new SqlConnection(sConection);
            _EnEjecución = true;
            try {
                conDB.Open();
            } catch ( SqlException exSQL ) {
                LogError("Conexión", "(" + conDB.DataSource + ") " + exSQL.Message, "");
                _EnEjecución = false;
                return false;
            }
            _EnEjecución = false;

            _conDataBase = conDB;
            sServer = _conDataBase.DataSource;
            sDataBaseName = _conDataBase.Database;
            return true;
        }

        /// <summary>
        /// Maneja la conexión con la base de datos, si está abierta la mantiene.
        /// </summary>
        /// <param name="Open">true para abrir conexión, false para cerrarla.</param>
        public static void ConnectSQL(bool Open) {

            if ( Open && _conDataBase.State == System.Data.ConnectionState.Closed )
                _conDataBase.Open();

            if ( !Open && _conDataBase.State == System.Data.ConnectionState.Open )
                try {
                    _conDataBase.Close();
                } catch ( SqlException ex ) {
                    LogError("Conexión", _conDataBase.DataSource + " - " + ex.Message, "");
                }

        }

        /// <summary>
        /// Cambia la base de datos a la que está conectado.
        /// </summary>
        /// <param name="Database">Base de datos a la que se conectará.</param>
        public static bool ChangeDatabase(string Database) {
            try {
                if ( _conDataBase.State == ConnectionState.Open && _conDataBase.Database != Database )
                    _conDataBase.ChangeDatabase(Database);
            } catch ( Exception ex ) {
                LogError("ChangeDatabase", ex.Message, Database);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Establece el query para ejecutar el comando.
        /// </summary>
        /// <param name="Command">Comando que se va a ejecutar.</param>
        public static void SetCommand(string Command) {
            if ( _Hoy != DateTime.Today ) {
                _EnEjecución = true;
                for ( int i = Application.OpenForms.Count - 1; i >= 0; i-- )
                    Application.OpenForms[i].Invoke((Action)delegate() { Application.OpenForms[i].Hide(); });

                MessageBox.Show("¿Usted dejó " + DataBaseConn._AppName + " abierto desde ayer?\r\n\r\nLe invitamos a que diario lo cierre.\r\nGracias por su cooperación.", DataBaseConn._AppName + " - Aplicación al día siguiente", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                Application.Exit();
            }

            _ErrorString = "";
            if ( Ejecutivo.Datos != null && Ejecutivo.Datos["NombreEjecutivo"] != null )
                Command = "\r\n/* " + Ejecutivo.Datos["NombreEjecutivo"] + " */"
                    + ( Command.ToLower().Contains("dbmemory.") ? "SET TRAN ISOLATION LEVEL READ COMMITTED" : "\tSET TRAN ISOLATION LEVEL READ UNCOMMITTED" )
                    + "\r\n" + Command;
            _command = new SqlCommand(Command, _conDataBase);
            _command.CommandTimeout = 3600;
            _scalar = null;
        }
        /// <summary>
        /// Ejecuta el commando previamente establecido y guarda el primer objeto leído en Escalar.
        /// </summary>
        /// <param name="Módulo">El módulo en donde se está ejecutando el comando.</param>
        /// <returns>True si fue correcta la ejecución.</returns>
        public static bool Execute(string Módulo) {

            if ( _EnEjecución )
                return false;


            _EnEjecución = true;
            try {
                ConnectSQL(true);
                _scalar = _command.ExecuteScalar();
            } catch ( Exception ex ) {
                LogError(Módulo, ex.Message, _command.CommandText);
                _EnEjecución = false;
                return false;
            }

            _EnEjecución = false;

            return true;
        }
        /// <summary>
        /// Realiza el llenado de la tabla con el comando previamente establecido.
        /// </summary>
        /// <param name="Módulo">Módulo en donde se ejecutó el llenado.</param>
        /// <param name="Tabla">Tabla que se llenará.</param>
        /// <returns>Tabla resultado.</returns>
        public static bool Fill(DataTable Tabla, string Módulo) {
            return Fill(Tabla, Módulo, true);
        }
        /// <summary>
        /// Realiza el llenado del dataset con el comando previamente establecido.
        /// </summary>
        /// <param name="Módulo">Módulo en donde se ejecutó el llenado.</param>
        /// <param name="Tabla">Tabla que se llenará.</param>
        /// <returns>Tabla resultado.</returns>
        public static bool Fill(DataSet Tablas, string Módulo) {
            return Fill(Tablas, Módulo, false);
        }
        private static bool Fill(object Tabla, string Módulo, bool bTabla) {
            _dataAdapter = new SqlDataAdapter();
            _dataAdapter.SelectCommand = _command;

            //if ( _EnEjecución )
            //    return false;

            _EnEjecución = true;
            try {
                ConnectSQL(true);
                if ( bTabla )
                    _dataAdapter.Fill((DataTable)Tabla);
                else
                    _dataAdapter.Fill((DataSet)Tabla);
            } catch ( InvalidOperationException ioEx ) {  //Error de data reader abierto.
                ioEx.ToString();
                return false;
            } catch ( Exception SqlEx ) {
                //return true;
                //if ( !( SqlEx.Class == 11 && SqlEx.Number == 0 ) )  // Error de cancelación por usuario. 
                //if(SqlEx.ToString()!="An invalid floating point operation occurred.")
                LogError(Módulo, SqlEx.Message, _command.CommandText);
                _EnEjecución = false;

                return false;
            }

            _dataAdapter.Dispose();
            _dataAdapter = null;

            _EnEjecución = false;

            return true;
        }

        /// <summary>
        /// Cancel el query que se esté ejecutando en la conexión.
        /// </summary>
        public static void CancelRunningQuery() {
            if ( _conDataBase == null )
                return;

            if ( _command != null ) {
                Thread _hiloCancelar = new Thread(_command.Cancel);
                _hiloCancelar.IsBackground = true;
                _hiloCancelar.Start();
                _EnEjecución = false;
            }

        }

 

        /// <summary>
        /// Empieza a ejecutar el método en un hilo.
        /// </summary>
        /// <param name="Método">Método que se ejecutará dentro del hilo.</param>
        /// <param name="Objeto">Parámetro que se le pasa al hilo.</param>
        public static void StartThread(ParameterizedThreadStart Método, object Objeto = null) {

            if ( EjecuciónConHilos ) {
                Thread hiloQuery = new Thread(Método);
                hiloQuery.IsBackground = true;
                hiloQuery.Start(Objeto);
            } else
                Método.Invoke(Objeto);
        }
        public static void StartThread(ThreadStart Método) {

            if ( EjecuciónConHilos ) {
                Thread hiloQuery = new Thread(Método);
                hiloQuery.IsBackground = true;
                hiloQuery.Start();
            } else
                Método.Invoke();

        }
    }
    #endregion

    #region Ejecutivo
    /// <summary>
    /// Clase para manejo del ejecutivo.
    /// Mediante esta clase se interactua con la base de datos.
    /// </summary>
    static class Ejecutivo {

        /* Atributos */
        static DataRow _drDatos;

        static int _Bloqueo = 0;
        static bool _Expiró = false;
        static bool _Complemento = false;

        static DataTable _Descendientes;
        static DataSet _Consultas;

        public enum Resultado { Contar, ContarCuentas, Detalle, Cuentas, FilaDeTrabajo };        

        /// <summary>
        /// Indica si la contraseña ya expiró después de una validación de usuario.
        /// </summary>
        public static bool Expiró { get { return _Expiró; } }
        /// <summary>
        /// Indica si el usuario tiene activado o desactivado complemento.
        /// </summary>
        public static bool Complemento { get { return _Complemento; } set { _Complemento = value; } }

        /// <summary>
        /// Datos del ejecutivo.
        /// </summary>
        public static DataRow Datos { get { return _drDatos; } }

        /// <summary>
        /// Tabla con los ejecutivos descendintes.
        /// </summary>
        public static DataTable Descendientes { get { return _Descendientes; } }

        /// <summary>
        /// Tabla con las consultas.
        /// </summary>
        public static DataTable Consultas { get { return _Consultas.Tables["Consultas"]; } }

        /// <summary>
        /// Tabla limpia de parámetros para usarse en las consultas.
        /// </summary>
        public static DataTable TablaParámetros {
            get {
                DataTable tblParámetros = new DataTable();
                tblParámetros.Columns.Add("Concepto");
                tblParámetros.Columns.Add("Campo");
                tblParámetros.Columns.Add("Valores");
                tblParámetros.Columns.Add("Parámetros");
                tblParámetros.Columns.Add("Dato");
                tblParámetros.Columns.Add("idConsulta", typeof(int));
                tblParámetros.PrimaryKey = new DataColumn[] { tblParámetros.Columns["Concepto"], tblParámetros.Columns["Campo"] };
                return tblParámetros;
            }
        }
        /// <summary>
        /// Tabla limpia de agrupación para usarse en las consultas.
        /// </summary>
        public static DataTable TablaAgrupar {
            get {
                DataTable tblAgrupar = new DataTable();
                tblAgrupar.Columns.Add("Concepto");
                tblAgrupar.Columns.Add("Campo");
                tblAgrupar.Columns.Add("idConsulta", typeof(int));
                tblAgrupar.PrimaryKey = new DataColumn[] { tblAgrupar.Columns["Concepto"], tblAgrupar.Columns["Campo"] };
                return tblAgrupar;
            }
        }

        /* Métodos */

        // Validación
        /// <summary>
        /// Valida los datos del usuario con la base de datos.
        /// </summary>
        /// <param name="Usuario">Usuario, 4 mayúsculas.</param>
        /// <param name="Contraseña">Contraseña de 8 o más dígitos.</param>
        /// <param name="Extensión">Extensión de 3 a 5 dígitos</param>
        /// <returns>Mensajes de resultado de la validación.</returns>
        public static string IniciaSesión(string Usuario, string Contraseña) {

            _drDatos = null;

            DataTable tblResultado = new DataTable();

            string sServidores = DataBaseConn.Server;

            if (sServidores == "192.168.7.18" || sServidores == "192.168.8.97")
            {
                DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM [192.168.7.30].dbCollection.dbo.Ejecutivos WHERE Usuario='" + Usuario + "' AND CAST(DECRYPTBYPASSPHRASE(Usuario, Contraseña, 1, CONVERT(VARBINARY(64), idEjecutivo)) AS VARCHAR(MAX)) ='" + Contraseña + "'");
            }
            else
            {
                DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM dbCollection.dbo.Ejecutivos WHERE Usuario='" + Usuario + "' AND CAST(DECRYPTBYPASSPHRASE(Usuario, Contraseña, 1, CONVERT(VARBINARY(64), idEjecutivo)) AS VARCHAR(MAX)) ='" + Contraseña + "'");
            }

            if (!DataBaseConn.Fill(tblResultado, "ValidaEjecutivo"))
                return "No se pudo validar el usuario, falló la solicitud con el servidor.";

            //DataBaseConn.SetCommand(
            //    "EXEC dbMemory.PS.IniciaSesión @Usuario, @Contraseña, @Extensión, @Bloqueo, @Dominio, @Computadora, @UsuarioWindows, @IP, @Aplicación, @Versión"
            //);            

            if (tblResultado.Columns.Contains("Expiró"))
                _Expiró = true;

            //if ( tblResultado.Columns.Contains("Jerarquía") && tblResultado.Rows.Count > 0 && Convert.ToInt32(tblResultado.Rows[0]["Jerarquía"]) == 0 )
            //    return "Sus privilegios son insuficientes para acceder a esta aplicación.";

            if ( tblResultado.Columns.Contains("Mensaje") )
                return tblResultado.Rows[0]["Mensaje"].ToString();
            else if ( tblResultado.Columns.Contains("idEjecutivo") && tblResultado.Rows.Count == 1 )
                _drDatos = tblResultado.Rows[0];


            //if ( !ObtieneEjecutivosPropios() )
            //    return "Falló al obtener los ejecutivos propios.";

            //if ( !ObtieneConsultas() )
            //    return "Falló al obtener las consultas del ejecutivo.";

            return "";
        }                 
    }
    #endregion



    #region dbFileReader
    /// <summary>
    /// Lee un libro de Excel.
    /// </summary>
    public class dbFileReader : IDisposable {
        bool disposed;

        IDbConnection _connection = null;
        IDataReader _dataReader = null;
        IDbCommand _command = null;

        DataTable _tblTabs;
        DataTable _tblColumns = null;

        TempDBF _TempDBF;
        protected class TempDBF {
            string _NewName,
                   _Directory,
                   _OriginalFileName,
                   _TxtFileName;
            int _RandomNumber;

            public TempDBF(string FilePath) {
                _Directory = Path.GetDirectoryName(FilePath) + "\\";
                _OriginalFileName = Path.GetFileName(FilePath);

                Random rndNum = new Random();
                _RandomNumber = rndNum.Next(1, 99);
                _NewName = "Temp" + _RandomNumber + ".dbf";
            }

            public string NewName { get { return _NewName; } }

            public string Rename() {

                if ( _OriginalFileName.Length <= 12 || System.IO.File.Exists(_Directory + _NewName) ) {
                    _NewName = _OriginalFileName;
                    return _NewName;
                }

                try {
                    // Create a txt with the original file name. 
                    _TxtFileName = ( _NewName + " " + _OriginalFileName + ".txt" ).ToLower().Replace(".dbf", "");
                    File.WriteAllText(_Directory + _TxtFileName, "El archivo \"" + _OriginalFileName + "\" fue renombrado a \"" + _NewName + "\" para ser usado por el ODBC dBASE Driver 64-bit que solo permite 8 caractéres en el nombre del archivo.");

                    // Rename
                    System.IO.File.Move(_Directory + _OriginalFileName, _Directory + _NewName);
                } catch ( IOException ex ) {
                    ex.ToString();
                    return _OriginalFileName;
                }

                return _NewName;
            }

            public void RestoreName() {
                try {
                    if ( File.Exists(_Directory + _NewName) )
                        File.Move(_Directory + _NewName, _Directory + _OriginalFileName);

                    if ( File.Exists(_Directory + _TxtFileName) )
                        File.Delete(_Directory + _TxtFileName);
                } catch ( Exception ex ) {
                    ex.Message.ToString();
                }
            }

            ~TempDBF() {
                RestoreName();
            }
        }

        string
            _FilePath,
            _ActiveTableName = "",
            _SchemaFilePath = "";
        bool _Headers = false;

        /// <summary>
        /// Ruta copmleta del archivo.
        /// </summary>
        public string FilePath { get { return _FilePath; } }
        /// <summary>
        /// Directorio del archivo.
        /// </summary>
        public string Directory { get { return System.IO.Path.GetDirectoryName(_FilePath) + "\\"; } }
        /// <summary>
        /// Nombre del archivo.
        /// </summary>
        public string FileName { get { return System.IO.Path.GetFileName(_FilePath); } }
        /// <summary>
        /// Extensión del archivo.
        /// </summary>
        public string Extension { get { return System.IO.Path.GetExtension(_FilePath).ToLower(); } }
        /// <summary>
        /// Lector del archivo.
        /// </summary>
        public IDataReader Reader { get { return _dataReader; } }
        /// <summary>
        /// Nombre de la tabla o pestaña activa.
        /// </summary>
        public string ActiveTable { get { return _ActiveTableName; } }

        /// <summary>
        /// Lee un archivo que contenga una tabla ( .dbf, .xls, .xlsx, .txt (tab) y .csv(,) ).
        /// </summary>
        /// <param name="FilePath">Ruta del archivo.</param>
        public dbFileReader(string FilePath) {
            _FilePath = FilePath;
        }
        /// <summary>
        /// Lee un archivo que contenga una tabla ( .dbf, .xls, .xlsx, .txt (tab) y .csv(,) ).
        /// </summary>
        /// <param name="FilePath">Ruta del archivo.</param> 
        /// <param name="Encabezados">Si la primera fila del archivo cuenta con encabezados.</param>
        public dbFileReader(string FilePath, bool Encabezados) {
            _FilePath = FilePath;
            _Headers = Encabezados;
        }

        /// <summary>
        /// Abre la conexión. Devuelve el mensaje de error.
        /// </summary>
        /// <returns></returns>
        public string Open() {
            string oleDbStringConection = "",
                    odbcStringConection = "";

            switch ( Extension ) {
                case ".dbf":
                //odbcStringConection = @"Driver={Microsoft Access dBASE Driver (*.dbf, *.ndx, *.mdx)};Dbq=" + Directory + ";DriverID=277;";
                oleDbStringConection = @"Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='dBASE IV';Data Source=" + Directory;
                break;

                case ".xls":
                case ".xlsx":
                //odbcStringConection = @"Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};ReadOnly=False;DBQ=" + _FilePath;
                oleDbStringConection = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='Excel 12.0 Xml;IMEX=0;HDR=YES';Data Source=" + _FilePath;
                break;

                case ".txt":
                case ".csv":
                //odbcStringConection = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + _FilePath;
                oleDbStringConection = "Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='text';Data Source=" + Directory + ";";                
                break;
            }

            if ( !_Headers )
                oleDbStringConection = oleDbStringConection.Replace("HDR=YES", "HDR=NO");

            if ( oleDbStringConection != "" ) {
                _connection = new OleDbConnection(oleDbStringConection);
                _command = new OleDbCommand();
            } else {
                _connection = new OdbcConnection(odbcStringConection);
                _command = new OdbcCommand();
            }

            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            CreateCSVSchema();

            return "";
        }
        private void CreateCSVSchema() {
            if ( Extension != ".csv" && Extension != ".txt" )
                return;

            string sTableName = Table_Name(),
                   sDelimiter = Extension == ".csv" ? "CSVDelimited" : "TabDelimited",
                   sFirstLine = "";

            _SchemaFilePath = Directory + "schema.ini";

            using ( StreamReader srFile = new StreamReader(_FilePath) )
                sFirstLine = srFile.ReadLine();

            if (sFirstLine.Contains(","))
                sDelimiter = "CSVDelimited";
            if (sFirstLine.Contains("\t"))
                sDelimiter = "TabDelimited";
            if (sFirstLine.Contains("|"))
                sDelimiter = "Delimited(|)";
            if (sFirstLine.Contains("¦"))
                sDelimiter = "Delimited(¦)";
            if (sFirstLine.Contains(";"))
                sDelimiter = "Delimited(;)";

            _Headers = true;

            string[] sLines = {
                sTableName,                
                "ColNameHeader=" + _Headers,            
                "MaxScanRows=5",
                "Format=" + sDelimiter
            };

            try {
                if ( File.Exists(_SchemaFilePath) )
                    File.Delete(_SchemaFilePath);
                File.WriteAllLines(_SchemaFilePath, sLines);
            } catch ( Exception ) {
                return;
            }

            if ( _dataReader == null ) {
                _command.Connection = _connection;
                _command.CommandText = "SELECT TOP 5 * FROM " + sTableName;
                _dataReader = _command.ExecuteReader(CommandBehavior.SingleRow);
            }

            _tblColumns = _dataReader.GetSchemaTable();
            _dataReader.Close();

            string[] sColumns = new string[_tblColumns.Rows.Count];

            for ( int i = 0; i < sColumns.Length; i++ )
                sColumns[i] = "Col" + ( i + 1 ) + "=\"" + _tblColumns.Rows[i]["ColumnName"] + "\" Text ";

            try {
                File.AppendAllLines(_SchemaFilePath, sColumns);
            } catch ( Exception ) {
                return;
            }
        }

        public string CreateFile() {

            try {
                if ( File.Exists(_FilePath) )
                    File.Delete(_FilePath);
            } catch ( Exception ex ) {
                return ex.Message;
            }

            while ( File.Exists(_FilePath) )
                Thread.Sleep(100);

            return Open();
        }

        /// <summary>
        /// Crea una pestaña en el libro de excel con el nombre y la información de la tabla.
        /// </summary>
        /// <param name="Nombre">Nombre de la pestaña.</param>
        /// <param name="Tabla">Tabla con los datos que se pasarán.</param>
        public string CreateTab(string Nombre, DataTable Tabla) {

            Nombre = Nombre.Replace("$", "").Replace(" ", "_");

            if ( Extension != ".xlsx" && Extension != ".xls" )
                return "Solo se puede realizar la creación a un libro de Excel.";

            if ( _connection == null || _connection.State != ConnectionState.Open )
                return "Se requiere una conexión abierta a un libro de Excel.";

            if ( Nombre.Trim().Length == 0 )
                return "Indique un nombre de la tabla.";

            if ( Tabla.Rows.Count == 0 )
                return "";

            string sCreateTable = "CREATE TABLE [" + Nombre + "] ( \r\n";
            string sInsert = "INSERT INTO [" + Nombre + "] ( ";


            // Crea tabla            
            foreach ( DataColumn col in Tabla.Columns ) {
                string sTipo = "NUMBER";
                if ( col.DataType.Name == "String" )
                    sTipo = "TEXT";
                else if ( col.DataType.Name == "DateTime" )
                    sTipo = "DATETIME";
                else if ( col.DataType.Name == "TimeSpan" )
                    sTipo = "TIMESTAMP";

                sCreateTable += "[" + col.ColumnName.Replace('.', '_') + "] " + sTipo + ", ";
                sInsert += "[" + col.ColumnName.Replace('.', '_') + "],  ";
            }
            sCreateTable = sCreateTable.TrimEnd(new char[] { ',', ' ' }) + " )";

            _command.CommandText = sCreateTable;
            _command.Connection = _connection;
            try {
                if ( _dataReader != null && !_dataReader.IsClosed )
                    _dataReader.Close();

                _command.ExecuteNonQuery();
            } catch ( Exception ex ) {
                return ex.Message;
            }


            ////////////////
            /*OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + Nombre + "]", (OleDbConnection)_connection);
            da.SelectCommand.CommandText = sInsert.TrimEnd(new char[] { ',', ' ' }) + " ) SELECT * FROM @Tabla";
            da.SelectCommand.Parameters.AddWithValue("@Tabla", Tabla);
            da.SelectCommand.Parameters["@Tabla"].DbType = DbType.Object;            
            da.SelectCommand.ExecuteNonQuery();*/
            /////////


            OleDbCommand oldbCom = new OleDbCommand("", (OleDbConnection)_connection);
            string sCommand = sInsert.TrimEnd(new char[] { ',', ' ' }) + " ) VALUES ( ";

            for ( int iColumnas = 0; iColumnas < Tabla.Columns.Count; iColumnas++ ) {
                oldbCom.Parameters.AddWithValue("@Param" + iColumnas, Tabla.Rows[0][iColumnas]);
                sCommand += "@Param" + iColumnas + ",";
            }
            oldbCom.CommandText = sCommand.TrimEnd(',') + ") ";


            for ( int iFilas = 0; iFilas < Tabla.Rows.Count; iFilas++ ) {
                for ( int iColumnas = 0; iColumnas < Tabla.Columns.Count; iColumnas++ )
                    oldbCom.Parameters[iColumnas].Value = Tabla.Rows[iFilas][iColumnas];

                try {
                    oldbCom.ExecuteNonQuery();
                } catch ( Exception ex ) {
                    return ex.Message;
                }
            }


            return "";
        }

        /// <summary>
        /// Obtiene un arreglo con el nombre de las pestañas del libro de Excel.
        /// </summary>
        /// <returns></returns>
        public string[] GetTabs() {

            if ( _tblTabs == null ) {
                if ( _connection.GetType() == typeof(OleDbConnection) )
                    _tblTabs = ( (OleDbConnection)_connection ).GetSchema("Tables");
                else
                    _tblTabs = ( (OdbcConnection)_connection ).GetSchema("Tables");
            }

            switch ( Extension ) {

                case ".xls":
                case ".xlsx":
                string sTabs = "";

                for ( int i = 0; i < _tblTabs.Rows.Count; i++ )
                    if ( !_tblTabs.Rows[i]["TABLE_NAME"].ToString().Contains("#") )
                        sTabs += _tblTabs.Rows[i]["TABLE_NAME"].ToString() + "/";

                if ( sTabs.EndsWith("/") ) {
                    sTabs = sTabs.TrimEnd('/');
                    return sTabs.Split('/');
                } else
                    return new string[0];

                default:
                return new string[] { FileName };
            }

        }

        /// <summary>
        /// Devulve un arreglo con los nombres de las columnas. 
        /// Necesita un reader abierto.
        /// </summary>
        /// <returns></returns>
        public string[] GetColumns() {
            // Validates connection.
            //if ( _connection == null || _dataReader == null)
            //  return new string[0];

            if ( _tblColumns == null )
                _tblColumns = _dataReader.GetSchemaTable();
            string[] sColumns = new string[_tblColumns.Rows.Count];

            for ( int i = 0; i < sColumns.Length; i++ )
                sColumns[i] = _tblColumns.Rows[i]["ColumnName"].ToString();

            return sColumns;
        }


        private string Table_Name(string sTableName = "") {

            sTableName = sTableName.Replace("[", "").Replace("]", "");

            if ( sTableName == "" && _ActiveTableName != "" )
                return _ActiveTableName;

            switch ( Extension ) {
                case ".xls":
                case ".xlsx":
                if ( sTableName == "" )
                    sTableName = GetTabs()[0];
                break;

                case ".dbf":
                if ( _TempDBF == null ) {
                    _TempDBF = new TempDBF(_FilePath);
                    sTableName = _TempDBF.Rename();
                } else
                    sTableName = _TempDBF.NewName;
                break;

                default:
                sTableName = FileName;
                break;
            }

            sTableName = "[" + sTableName + "]";

            _ActiveTableName = sTableName;
            return sTableName;
        }

        /// <summary>
        /// Establece la hoja activa del Libro de Excel.
        /// </summary>
        /// <param name="SheetName">Nombre de la hoja de Excel.</param>
        public void SetActiveSheet(string SheetName) {
            Table_Name(SheetName);
        }

        /// <summary>
        /// Devuelve el número de filas en la tabla.
        /// </summary>        
        /// <returns></returns>
        public int CountRows() {

            int iCount = 0;

            string sTableName = Table_Name();
            _command.CommandText = "SELECT COUNT(*) AS REGISTROS FROM " + sTableName;
            _command.Connection = _connection;
            iCount = int.Parse(_command.ExecuteScalar().ToString());

            return iCount;
        }


        /// <summary>
        /// Carga el archivo al Reader.
        /// </summary>
        public void Read() {
            Read(0, "*");
        }
        /// <summary>
        /// Carga el archivo al Reader.
        /// </summary>        
        /// <param name="TopRows">Número de filas, 0 obtiene todas.</param>
        public void Read(int TopRows) {
            Read(TopRows, "*");
        }
        /// <summary>
        /// Carga el archivo al Reader.
        /// </summary>        
        /// <param name="TopRows">Número de filas, 0 obtiene todas.</param>       
        /// <param name="Columns">Nombres de columnas que desea obtener.</param>
        public void Read(int TopRows, string[] Columns) {

            string sColumns = "";
            for ( int i = 0; i < Columns.Length; i++ )
                sColumns += ( Columns[i] == "" ? "NULL" : Columns[i] ) + ", ";
            sColumns = sColumns.TrimEnd(' ', ',');

            Read(TopRows, sColumns);
        }
        private void Read(int TopRows, string Columns) {
            if ( _connection == null || _connection.State == ConnectionState.Closed )
                return;

            string sTableName = Table_Name();

            _command.CommandText = "SELECT \r\n " + ( TopRows > 0 ? "TOP " + TopRows : "" ) + " " + Columns + " \r\n FROM " + sTableName;
            _command.Connection = _connection;
            _dataReader = _command.ExecuteReader(CommandBehavior.SequentialAccess);

        }

        /// <summary>
        /// Cierra el archivo y libera los recursos.
        /// </summary>
        public void Close() {

            try {
                if ( _dataReader != null )
                    _dataReader.Close();
                _dataReader.Dispose();
            } catch ( Exception ) { }

            try {
                if ( _connection != null )
                    _connection.Close();
                _connection.Dispose();
            } catch ( Exception ) { }

            try {
                if ( _TempDBF != null )
                    _TempDBF.RestoreName();
            } catch ( Exception ) { }

            try {
                if ( File.Exists(_SchemaFilePath) )
                    File.Delete(_SchemaFilePath);
            } catch ( Exception ) { }

            _command = null;
            _dataReader = null;
            _connection = null;

            CleanMemory();
        }

        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "SetProcessWorkingSetSize")]
        internal static extern bool SetProcessWorkingSetSize(IntPtr pProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);
        private static void CleanMemory() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            int iMinMB = 15 * ( 1024 ^ 3 );
            int iMaxMB = 230 * ( 1024 ^ 3 );

            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, iMinMB, iMaxMB);
        }

        protected virtual void Dispose(bool disposing) {
            if ( !disposed && disposing ) {
                Close();
            }

            _tblTabs = null;
            _tblColumns = null;
            _TempDBF = null;
            _ActiveTableName = null;
            _SchemaFilePath = null;
            _FilePath = null;

            disposed = true;
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~dbFileReader() {
            Dispose(true);
        }
    }
    #endregion

    #region Catálogos
    /// <summary>
    /// Clase que contiene todas los catálogos y métodos para acceder a ellos.  
    /// </summary>
    static class Catálogo {

        /* Atributos */

        static DataSet _dsTablas = new DataSet();

        static public DataTable Carteras {
            get {
                if ( _dsTablas.Tables.Contains("Carteras") )
                    return _dsTablas.Tables["Carteras"];
                else
                    return null;
            }
        }        
        static public DataTable Productos {
            get {
                if ( _dsTablas.Tables.Contains("Productos") )
                    return _dsTablas.Tables["Productos"];
                else
                    return null;
            }
        }     
        static public DataTable Segmentos
        {
            get
            {
                if (_dsTablas.Tables.Contains("Segmentos"))
                    return _dsTablas.Tables["Segmentos"];
                else
                    return null;
            }
        }
        static public DataTable CJClientes
        {
            get
            {
                if (_dsTablas.Tables.Contains("CJClientes"))
                    return _dsTablas.Tables["CJClientes"];
                else
                    return null;
            }
        }      
        static public DataTable Catálogos {
            get {
                if ( _dsTablas.Tables.Contains("Catálogos") )
                    return _dsTablas.Tables["Catálogos"];
                else
                    return null;
            }
        }
        static public DataTable Valores {
            get {
                if ( _dsTablas.Tables.Contains("ValoresCatálogo") )
                    return _dsTablas.Tables["ValoresCatálogo"];
                else
                    return null;
            }
        }
        static public DataTable Relaciones {
            get {
                if ( _dsTablas.Tables.Contains("RelacionesCatálogos") )
                    return _dsTablas.Tables["RelacionesCatálogos"];
                else
                    return null;
            }
        }
        static public DataTable Versionamiento {
            get {
                if ( _dsTablas.Tables.Contains("Versionamiento") )
                    return _dsTablas.Tables["Versionamiento"];
                else
                    return null;
            }
        }

        public static void ActualizaTodosCatálogos(IWin32Window owner) {

            if (
                !CargaCarterasProductos() ||
                !CargaCatálogos()  ||                
                !CargaVersionamiento() 
            ) {
                if ( ( (Form)owner ).InvokeRequired )
                    ( (Form)owner ).Invoke((Action)delegate {
                        MessageBox.Show(owner, "Falló al obtener información fundamental para el funcionamiento de la aplicación, por lo cual se cerrará.\r\n\r\nPuede dirigirse al área de sistemas para que le den solución con el id " + DataBaseConn.idLogError.ToString("N0") + ".",
                        "Catálogos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                Application.Exit();

            }

        }
        public static bool CargaCarterasProductos() {

            

            // Carteras
            DataTable tblCarteras = new DataTable();

            string sServidores = DataBaseConn.Server;

            if (sServidores == "192.168.7.18" || sServidores == "192.168.8.97")
            {
                DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM [192.168.7.30].dbHistory.dbo.Carteras");
            }
            else
            {
                DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM dbHistory.dbo.Carteras");
            }
            
            if ( !DataBaseConn.Fill(tblCarteras, "CargaCarteras") )
                return false;

            tblCarteras.TableName = "Carteras";
            tblCarteras.PrimaryKey = new DataColumn[] { tblCarteras.Columns["idCartera"] };
            _dsTablas.Tables.Add(tblCarteras);
            _dsTablas.Tables["Carteras"].DefaultView.Sort = "Cartera";

            // Productos
            DataTable tblProductos = new DataTable();
            //string sServidores = DataBaseConn.Server;

            if (sServidores == "192.168.7.18" || sServidores == "192.168.8.97")
            {
                DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM [192.168.7.30].dbCollection.dbo.Productos");
            }
            else
            {
                DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM dbCollection.dbo.Productos");
            }

            if ( !DataBaseConn.Fill(tblProductos, "CargaProductos") )
                return false;

            tblProductos.TableName = "Productos";
            tblProductos.PrimaryKey = new DataColumn[] { tblProductos.Columns["idProducto"] };
            _dsTablas.Tables.Add(tblProductos);
            _dsTablas.Tables["Productos"].DefaultView.Sort = "Producto";

            //Segmentos
            DataTable tblSegmentos = new DataTable();
            DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM Segmentos");
            if (!DataBaseConn.Fill(tblSegmentos, "CargaSegmentos"))
                return false;

            tblSegmentos.TableName = "Segmentos";
            tblSegmentos.PrimaryKey = new DataColumn[] { tblProductos.Columns["idSegmento"] };
            _dsTablas.Tables.Add(tblSegmentos);
            _dsTablas.Tables["Segmentos"].DefaultView.Sort = "Segmento";

            //CJClientes
            DataTable tblCJClientes = new DataTable();
            DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM dbEstadistica.Contabilidad.CJClientes");
            if (!DataBaseConn.Fill(tblCJClientes, "CargaCJClientes"))
                return false;

            tblCJClientes.TableName = "CJClientes";
            tblCJClientes.PrimaryKey = new DataColumn[] { tblCJClientes.Columns["Clave"] };
            _dsTablas.Tables.Add(tblCJClientes);
            _dsTablas.Tables["CJClientes"].DefaultView.Sort = "Clave";   

            // Crea relación
            //DataRelation drCarterasProducto = new DataRelation("CarterasProducto", tblCarteras.Columns["idCartera"], tblProductos.Columns["idCartera"], false);
            //_dsTablas.Relations.Add(drCarterasProducto);

            return true;
        }
        public static bool CargaCatálogos() {

            // Catálogos
            DataTable tblCatálogos = new DataTable();
            DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM Catálogos");
            if ( !DataBaseConn.Fill(tblCatálogos, "CargaValoresCatálogos") )
                return false;

            tblCatálogos.TableName = "Catálogos";
            tblCatálogos.PrimaryKey = new DataColumn[] { tblCatálogos.Columns["idCatálogo"] };
            _dsTablas.Tables.Add(tblCatálogos);


            // Valores
            DataTable tblValores = new DataTable();
            DataBaseConn.SetCommand("USE dbEstadistica; SELECT idValor, idCatálogo, Valor, Detalle, Orden FROM ValoresCatálogo ");
            if ( !DataBaseConn.Fill(tblValores, "CargaValoresCatálogos") )
                return false;

            tblValores.TableName = "ValoresCatálogo";
            tblValores.PrimaryKey = new DataColumn[] { tblValores.Columns["idValor"] };
            tblValores.Rows.Remove(tblValores.Rows.Find(0));
            _dsTablas.Tables.Add(tblValores);            

            // FK Catálogos - Valores
            DataRelation drCatálogos = new DataRelation("Catálogos", tblCatálogos.Columns["idCatálogo"], tblValores.Columns["idCatálogo"], false);
            _dsTablas.Relations.Add(drCatálogos);


            // Relaciones
            DataTable tblRelaciones = new DataTable();
            DataBaseConn.SetCommand("USE dbEstadistica; SELECT * FROM vw_Relaciones ");
            if ( !DataBaseConn.Fill(tblRelaciones, "CargaRelacionesCatálogos") )
                return false;

            tblRelaciones.TableName = "RelacionesCatálogos";
            tblRelaciones.PrimaryKey = new DataColumn[] { tblRelaciones.Columns["idValor1"], tblRelaciones.Columns["idValor2"] };
            _dsTablas.Tables.Add(tblRelaciones);

            // FK Relaciones
            DataRelation drRelación1 = new DataRelation("Relación1", tblValores.Columns["idValor"], tblRelaciones.Columns["idValor1"], false);
            _dsTablas.Relations.Add(drRelación1);

            DataRelation drRelación2 = new DataRelation("Relación2", tblValores.Columns["idValor"], tblRelaciones.Columns["idValor2"], false);
            _dsTablas.Relations.Add(drRelación2);

            return true;

        }  
        public static bool CargaVersionamiento() {
            // Versionamiento
            DataTable tblVersiones = new DataTable();
            DataBaseConn.SetCommand("USE dbEstadistica; SELECT Fecha, CONCAT(Mayor,'.',Minor, '.',Build) Versión, Descripción FROM Versionamiento WHERE Aplicación = 'Estadistica' ");
            if ( !DataBaseConn.Fill(tblVersiones, "CargaVersionamientoEstadistica") )
                return false;

            tblVersiones.DefaultView.Sort = "Fecha DESC, Versión DESC";

            tblVersiones.TableName = "Versionamiento";
            _dsTablas.Tables.Add(tblVersiones);            

            return true;
        }

        public static void CargaColumnasProducto(object idProducto, object idCartera = null) {

            if ( idProducto == null && idCartera == null )
                return;

            DataTable tblColumnasProducto = new DataTable();
            if ( idCartera != null )
                tblColumnasProducto.TableName = "Cartera_" + idCartera;
            else
                tblColumnasProducto.TableName = "Producto_" + idProducto;


            if ( _dsTablas.Tables.Contains(tblColumnasProducto.TableName) ) 
                _dsTablas.Tables.Remove(tblColumnasProducto.TableName);


            DataBaseConn.SetCommand("SELECT LOWER(name) name FROM dbEstadistica.sys.columns WHERE OBJECT_ID('Y." + tblColumnasProducto.TableName + "') = [object_id]");

            DataBaseConn.Fill(tblColumnasProducto, "CargaColumnasProducto");

            if ( tblColumnasProducto.Columns.Contains("name") && tblColumnasProducto.Rows.Count > 0 ) {
                _dsTablas.Tables.Add(tblColumnasProducto);
                _dsTablas.Tables[tblColumnasProducto.TableName].DefaultView.Sort = "name";
                _dsTablas.Tables[tblColumnasProducto.TableName].DefaultView.RowFilter = "name <> 'idCuenta'";
            }
        }

        public static Hashtable RelacionesCatálogo(int idValor2) {
            Hashtable htRelaciones = new Hashtable();

            DataRow[] drFilas = _dsTablas.Tables["ValoresCatálogo"].Rows.Find(idValor2).GetChildRows("Relación2");

            foreach ( DataRow fila in drFilas )
                htRelaciones.Add(fila["idValor1"].ToString(), fila["Valor1"].ToString());

            return htRelaciones;
        }

        public static string idsRelaciones(int idValor2) {
            Hashtable htRelación = RelacionesCatálogo(idValor2);
            string sIds = "";
            foreach ( string idValor in htRelación.Keys )
                sIds += idValor + ",";

            return sIds.TrimEnd(',');        
        }

        /* Funciones */

        public static DataTable TablaBit() {
            DataTable tblBit = new DataTable();
            tblBit.Columns.Add("idValor", typeof(int));
            tblBit.Columns.Add("Valor");

            tblBit.Rows.Add(1, "Sí");
            tblBit.Rows.Add(0, "No");

            return tblBit;
        }

        public static DataTable ValoresDelCatálogo(string Catálogo) {
            DataTable tblValores = new DataTable();

            DataRow[] drFilas = Catálogos.Select("Catálogo = " + Catálogo);

            if ( drFilas.Length == 1 )
                tblValores = drFilas[0].GetChildRows("Catálogos").CopyToDataTable();

            return tblValores;
        }

        public static DataTable ValoresDelCatálogo(int idCatálogo) {

            DataTable tblValores = Catálogos.Rows.Find(idCatálogo).GetChildRows("Catálogos").CopyToDataTable();
            tblValores.DefaultView.Sort = "Orden";
            return tblValores;
        }
    
        public static DataTable ValoresDelCatálogo(int idCatálogo1, int idValor2Relación) {

            DataTable tblRelación = Valores.Clone();

            DataRow[] drRelaciones = Relaciones.Select("idValor2 = " + idValor2Relación);
            foreach ( DataRow drRelación in drRelaciones ) {
                DataRow[] drValores = drRelación.GetParentRows("Relación1");
                foreach ( DataRow drValor in drValores )
                    if ( Convert.ToInt32(drValor["idCatálogo"]) == idCatálogo1 )
                        tblRelación.ImportRow(drValor);
            }

            return tblRelación;
        }
  
        public static DataTable ProductoColumnas(object idProducto, object idCartera) {

            //if ( !_dsTablas.Tables.Contains("Producto_" + idProducto) && !_dsTablas.Tables.Contains("Cartera_" + idCartera) )
                CargaColumnasProducto(idProducto, idCartera);


            if ( _dsTablas.Tables.Contains("Cartera_" + idCartera) )
                return _dsTablas.Tables["Cartera_" + idCartera];

            if ( _dsTablas.Tables.Contains("Producto_" + idProducto) )
                return _dsTablas.Tables["Producto_" + idProducto];

            else
                return new DataTable();
        }
       
        public static int idCartera(int idProducto) {

            DataRow drProducto = _dsTablas.Tables["Productos"].Rows.Find(idProducto);
            if ( drProducto != null )
                return Convert.ToInt32(drProducto["idCartera"]);

            return 0;
        }

        public static string Cartera(int idCartera) {

            DataRow drCartera = _dsTablas.Tables["Carteras"].Rows.Find(idCartera);
            if ( drCartera != null )
                return drCartera["Cartera"].ToString();

            return "";
        }

        public static string Producto(int idProducto) {

            DataRow drProducto = _dsTablas.Tables["Productos"].Rows.Find(idProducto);
            if ( drProducto != null )
                return drProducto["Producto"].ToString();

            return "";
        }

        public static string idProductos(int idCartera) {
            string sProductos = "0";

            foreach ( DataRow drProducto in _dsTablas.Tables["Productos"].Rows )
                if ( Convert.ToInt32(drProducto["idCartera"]) == idCartera )
                    sProductos += "," + drProducto["idProducto"];

            return sProductos;
        }

        public static string Valor(object idValor) {
            DataRow drValor = Valores.Rows.Find(idValor);

            if ( drValor == null )
                return "";
            else
                return drValor["Valor"].ToString();
        }
    }
    #endregion


    #region Funciones
    /// <summary>
    /// Funciones para facilitar la vida.
    /// </summary>    
    static class Funciones {

        /* Funciones */

        


   
        public static bool CopyApp(string[] args) {
            string RutaCjpad = @"C:\Procest\AcuerdosEst";


            if (File.Exists(@"\\192.168.4.9\estadistica\BANCOMER\TDAG\" + DateTime.Now.Year.ToString() + @"\Base.xlsx"))
            {
                DateTime FechaCreacion = File.GetCreationTime(@"\\192.168.4.9\estadistica\BANCOMER\TDAG\" + DateTime.Now.Year.ToString()+ @"\Base.xlsx");
                DateTime FechaModif = File.GetLastWriteTime(@"\\192.168.4.9\estadistica\BANCOMER\TDAG\" + DateTime.Now.Year.ToString() + @"\Base.xlsx");

                SqlCommand command = new  SqlCommand("SELECT MAX(FechaCreacion) FROM dbEstadistica.Riesgos.FechasTDAG");

               // DataTable dt = new DataTable();
                //dt = command;
            }

            
            



           /// if (DataBaseConn.SetCommand("SELECT MAX(FechaCreacion) FROM dbEstadistica.Riesgos.fechasTDAG") < FechaCreacion
           ///     
           ///     
           ///     );




            if (!Directory.Exists(RutaCjpad))
                Directory.CreateDirectory(RutaCjpad);

            if (File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO AUTO.docx"))
                File.Delete(RutaCjpad + "\\FO ACUERDO DE PAGO AUTO.docx");
            if (!File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO AUTO.docx"))
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\FO ACUERDO DE PAGO AUTO.docx", RutaCjpad + "\\FO ACUERDO DE PAGO AUTO.docx");

            if (File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO.docx"))
                File.Delete(RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO.docx");
            if (!File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO.docx"))
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\FO ACUERDO DE PAGO CONSUMO.docx", RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO.docx");

            if (File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO TDC.docx"))
                File.Delete(RutaCjpad + "\\FO ACUERDO DE PAGO TDC.docx");
            if (!File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO TDC.docx"))
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\FO ACUERDO DE PAGO TDC.docx", RutaCjpad + "\\FO ACUERDO DE PAGO TDC.docx");

            if (File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO AUTO_METRO.docx"))
                File.Delete(RutaCjpad + "\\FO ACUERDO DE PAGO AUTO_METRO.docx");
            if (!File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO AUTO_METRO.docx"))
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\FO ACUERDO DE PAGO AUTO_METRO.docx", RutaCjpad + "\\FO ACUERDO DE PAGO AUTO_METRO.docx");

            if (File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO_METRO.docx"))
                File.Delete(RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO_METRO.docx");
            if (!File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO_METRO.docx"))
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\FO ACUERDO DE PAGO CONSUMO_METRO.docx", RutaCjpad + "\\FO ACUERDO DE PAGO CONSUMO_METRO.docx");

            if (File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO TDC_METRO.docx"))
                File.Delete(RutaCjpad + "\\FO ACUERDO DE PAGO TDC_METRO.docx");
            if (!File.Exists(RutaCjpad + "\\FO ACUERDO DE PAGO TDC_METRO.docx"))
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + "\\FO ACUERDO DE PAGO TDC_METRO.docx", RutaCjpad + "\\FO ACUERDO DE PAGO TDC_METRO.docx");
            return true;
            string sLocalDirectory = DataBaseConn.LocalFolder + DataBaseConn.AppName + "\\";
            string sFileName =
                DataBaseConn.AppName + "Local " +
                ( Application.ExecutablePath.Contains("x86") ? "x86" : "" ) +
                " v" + DataBaseConn.AppVersion + ".exe";

            if ( Application.ExecutablePath == sLocalDirectory + sFileName && args.Length >= 2 && args[0] == "Carpeta" ) {
                DataBaseConn.NetworkFolder = args[1];
                return true;
            }

            if ( !RunningFromNetwork() )
                return false;
            
            System.IO.Directory.CreateDirectory(sLocalDirectory);
            try {
                // Copy Temp 
                CopyFile(AppDomain.CurrentDomain.BaseDirectory + DataBaseConn.TempFileName, sLocalDirectory + DataBaseConn.TempFileName);

                // Copy OpenXML dll
                if ( !File.Exists(sLocalDirectory + "DocumentFormat.OpenXml.dll") )
                    CopyFile(AppDomain.CurrentDomain.BaseDirectory + "DocumentFormat.OpenXml.dll", sLocalDirectory + "DocumentFormat.OpenXml.dll");

                


                // Check current local version                
                string[] sLocalFiles = Directory.GetFiles(sLocalDirectory, DataBaseConn.AppName + "Local*", SearchOption.TopDirectoryOnly);
                foreach ( string sFile in sLocalFiles )
                    if ( sFile == sLocalDirectory + sFileName && new FileInfo(sFile).Length == new FileInfo(Application.ExecutablePath).Length ) {
                        Process.Start(sFile, "Carpeta " + AppDomain.CurrentDomain.BaseDirectory);
                        return false;
                    } else 
                        File.Delete(sFile);

                // Copy .exe and execute
                File.Delete(sLocalDirectory + sFileName);
                CopyFile(Application.ExecutablePath, sLocalDirectory + sFileName);                
                Process.Start(sLocalDirectory + sFileName, "Carpeta " + AppDomain.CurrentDomain.BaseDirectory);
            } catch ( Exception ex ) {
                MessageBox.Show("Se tuvo un problema con el autocopiado del archivo. Revise que no tenga abierta otra instancia de la aplicación o consulte a soporte técnico para resolver problemas de escritura local.\r\n" + ex.Message,
                DataBaseConn.AppName + "v " + DataBaseConn.AppVersion + " - Autocopiado local", MessageBoxButtons.OK);
            }
            return false;
        }
        private static void CopyFile(string Source, string Destination) {

            if ( File.Exists(Destination) ) {
                FileInfo hiddenFile = new FileInfo(Destination);
                hiddenFile.Attributes &= ~FileAttributes.Hidden;
                hiddenFile.Attributes &= ~FileAttributes.System;
            }

            File.Copy(Source, Destination, true);
        }


        public static bool RunningFromNetwork() {
            return true;
            if ( Application.StartupPath.StartsWith(@"\\") )
                return true;
            else {
                DriveInfo info = new DriveInfo(Application.StartupPath.Substring(0, 1));
                if ( info.DriveType == DriveType.Network )
                    return true;
            }

            MessageBox.Show("La aplicación no debe ejecutarse o copiarse localmente a la máquina. Debe ejecutarse desde un servidor en la red.\r\n\r\nLa aplicación se cerrará.",
                "Estadistica - Ejecución local", MessageBoxButtons.OK);

            return false;

        }

        public static string GetIP() {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach ( IPAddress ip in host.AddressList )
                if ( ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                    && ip.ToString().StartsWith("192.168.") )
                    return ip.ToString();

            return "";
        }

        // Campos Pantalla

        public static Hashtable _htProducto;

        static public bool TryParseDate(string Text, out DateTime Date) {
            if ( DateTime.TryParse(Text, out Date) ||
                        DateTime.TryParseExact(Text, new string[] { "yyyyMMdd" }, null, System.Globalization.DateTimeStyles.None, out Date) )
                return true;
            return false;
        }

        /// <summary>
        /// Cambia el formato del texto
        /// </summary>
        /// <param name="Texto">Texto que va cambiar el formato.</param>
        /// <param name="Formato">1 Texto, 2 Número, 3 Moneda, 4 Fecha</param>
        static public string Formato(object Texto, object Formato) {

            if ( Texto == null )
                return "";

            string sTexto = Texto.ToString();

            switch ( Formato.ToString() ) {
                case "2":
                float fTexto;
                sTexto = float.TryParse(sTexto, out fTexto) ? fTexto.ToString("#,#") : sTexto;
                break;

                case "3":
                double dTexto;
                sTexto = double.TryParse(sTexto, out dTexto) ? dTexto.ToString("$ #,#.00") : sTexto; //C2
                break;

                case "4":
                sTexto = sTexto.Replace("12:00:00 a.m.", "");
                DateTime dtTexto = new DateTime();
                if ( TryParseDate(sTexto, out dtTexto) )
                    sTexto = dtTexto.ToString("dd/MM/yyyy");
                break;

                case "5":
                float pTexto;
                sTexto = float.TryParse(sTexto, out pTexto) ? Math.Round(pTexto * 100, 0) + " %" : sTexto;
                break;
            }

            return sTexto;
        }

        /// <summary>
        /// Evalua una expresión aritmética y devuelve el resultado, 0 si fue incorrecta.
        /// </summary>
        /// <param name="Tabla">Expresión aritmética.</param>  
        /// <param name="Número">Indica si se va a devolver un número</param>
        /// <param name="Fecha">Indica si se va a evaluar una fecha</param>
        static public double Evaluate(string expression) {

            if ( Regex.Matches(expression, @"[a-zA-Z]").Count > 0 )
                return 0;

            DataTable dtExpression = new DataTable();
            double dEvaluation = 0;
            expression = expression.Replace("%", "/100");

            try {
                dtExpression.Columns.Add(new DataColumn("Eval", typeof(double), expression));
                dtExpression.Rows.Add(dtExpression.NewRow());
                double.TryParse(dtExpression.Rows[0]["Eval"].ToString(), out dEvaluation);
            } catch ( Exception ) {
                //ErrorLogClass.LogError("Evaluate", expression);
            }

            return dEvaluation;
        }

        static public object EvaluateDate(string expression) {

            if ( Regex.Matches(expression, @"[a-zA-Z]").Count > 0 )
                return new DateTime(0);

            DateTime dtPrimero = new DateTime();
            DateTime dtSegundo = new DateTime();
            int iDías = 0;

            string[] sExpresión = expression.Trim().Split(' ');


            if ( sExpresión.Length != 3 || !TryParseDate(sExpresión[0], out dtPrimero) )
                return "";

            switch ( sExpresión[1] ) {

                case "+":
                if ( int.TryParse(sExpresión[2], out iDías) )
                    return dtPrimero.AddDays(iDías);
                break;

                case "-":
                if ( TryParseDate(sExpresión[2], out dtSegundo) )
                    return ( dtPrimero - dtSegundo ).TotalDays;
                else if ( int.TryParse(sExpresión[2], out iDías) )
                    return dtPrimero.AddDays(-iDías);
                break;
            }

            return sExpresión[0];
        }

        /// <summary>
        /// Revisa si el texto son letras.
        /// </summary>
        /// <param name="Text">Texto a revisar.</param>
        /// <returns></returns>
        static public bool AreLetters(string Text) {
            foreach ( char c in Text )
                if ( !Char.IsLetter(c) )
                    return false;
            return true;
        }

        /// <summary>
        /// Traduce el nombre de los campos por su valor y opera entre ellos para obtener un resultado.
        /// </summary>
        /// <param name="Expresión">Expresión que contiene los nombres de los campos y sus operaciones.</param>        
        public static object CampoCalculado(string Expresión) {

            string[] sCampos = Expresión.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            string sResult = Expresión;

            for ( int i = 0; i < sCampos.Length; i++ ) {
                if ( _htProducto[sCampos[i]] != null )
                    sResult = sResult.Replace("[" + sCampos[i] + "]", _htProducto[sCampos[i]].ToString().Trim());
            }

            if ( Expresión.StartsWith("#") )
                return EvaluateDate(sResult.Replace("#", ""));

            else if ( sCampos.Length > 1 && ( Expresión.Contains("+") || Expresión.Contains("-") || Expresión.Contains("*") || Expresión.Contains("/") || Expresión.Contains("^") ) )
                return Evaluate(sResult);

            return sResult;
        }

        static public Hashtable ConvertRowToHashTable(DataTable Tabla, int iFila) {
            Hashtable htRow = new Hashtable();

            if ( Tabla.Rows.Count <= iFila )
                return htRow;

            for ( int iCol = 0; iCol < Tabla.Columns.Count; iCol++ ) {
                htRow.Add(
                    Tabla.Columns[iCol].ColumnName.ToString().Trim(),
                    Tabla.Rows[iFila][iCol].ToString().Trim()
                );
            }
            return htRow;
        }

        /// <summary>
        /// Agrega la fila de totales a la tabla.
        /// </summary>
        /// <param name="Tabla">Tabla a la que se le agrega la fila de totales.</param>
        static public void FilaTotales(ref DataTable Tabla) {
            string sIndicador = Tabla.TableName.Split('_')[0];

            string sTotal = "Total";

            if ( sIndicador == "Sesiones" )
                return;

            if ( !Tabla.Columns.Contains("SortColumn") )
                Tabla.Columns.Add("SortColumn");

            DataRow drTotales = Tabla.NewRow();

            for ( int iCols = 0; iCols < Tabla.Columns.Count; iCols++ )
                switch ( Tabla.Columns[iCols].DataType.Name ) {
                    case "Int32":  // Conteos
                    drTotales[iCols] = Tabla.Compute("SUM([" + Tabla.Columns[iCols].ColumnName + "])", "");
                    break;
                    case "Single":  // Porcentaje
                    drTotales[iCols] = Tabla.Compute("AVG([" + Tabla.Columns[iCols].ColumnName + "])", "");
                    sTotal = "Promedio";
                    break;
                    case "Double":  // Dinero
                    case "Decimal":  // Dinero
                    drTotales[iCols] = Tabla.Compute("SUM([" + Tabla.Columns[iCols].ColumnName + "])", "");
                    break;
                    case "TimeSpan": //Tiempo                        
                    try {
                        object oTiempo = Tabla.Compute("AVG([" + Tabla.Columns[iCols].ColumnName + "])", "");
                        drTotales[iCols] = oTiempo == null ? DBNull.Value : oTiempo;
                    } catch ( Exception ) {
                        // Código para Windows XP
                        long lTiempo = 0;
                        for ( int i = 0; i < Tabla.Rows.Count; i++ )
                            if ( Tabla.Rows[i][iCols] != null && Tabla.Rows[i][iCols].GetType() == typeof(TimeSpan) )
                                lTiempo += ( (TimeSpan)Tabla.Rows[i][iCols] ).Ticks;
                        if ( Tabla.Rows.Count > 0 )
                            drTotales[iCols] = new TimeSpan(lTiempo / Tabla.Rows.Count);
                        else
                            drTotales[iCols] = new TimeSpan(0);
                    }
                    sTotal = "Promedio";
                    break;

                }

            if ( Tabla.Columns.Contains("Ejecutivo") )
                drTotales["Ejecutivo"] = sTotal;
            else if ( Tabla.Columns.Contains("Hora") )
                drTotales["Hora"] = 0;


            drTotales["SortColumn"] = ".";
            Tabla.DefaultView.Sort = ( "SortColumn, " + Tabla.DefaultView.Sort ).TrimEnd(',', ' ');

            Tabla.Rows.Add(drTotales);
        }

        /// <summary>
        /// Agrega una columna de porcentaje sobre el total de filas.
        /// </summary>
        /// <param name="tblTabla">Tabla a la que se le agregará la columna</param>
        /// <param name="NombreColumna">Nombre de la columna que utilizará para calcular el %.</param>
        static public void ColumnaPorcentaje(ref DataTable tblTabla, string NombreColumna) {

            if ( !tblTabla.Columns.Contains(NombreColumna) )
                return;

            string sTotal = tblTabla.Compute("SUM(" + NombreColumna + ")", "").ToString();
            DataColumn colPorcentaje = tblTabla.Columns.Add(
                NombreColumna + " %",
                typeof(double),
                sTotal.Length == 0 || Convert.ToDouble(sTotal) == 0 ? "0" : NombreColumna + " / " + sTotal
                );
        }

        /// <summary>
        /// Traduce las palabras en inglés del versionamiento. 
        /// </summary>
        /// <param name="ChangeLog">Cambios hechos a la aplicación.</param>
        /// <returns></returns>
        static public string TraduceVersionamiento(string ChangeLog) {
            return ChangeLog
                .Replace("Added:", "Añadido:")
                .Replace("Changed:", "Cambiado:")
                .Replace("Improved:", "Mejorado:")
                .Replace("Fixed:", "Reparado:")
                .Replace("New:", "Nuevo:")
                .Replace("Removed:", "Quitado:")
                .Replace("\t", " ")
                .Replace("  ", " ");
        }


        // Scripts

        static DataRow _drInfo,
            _drCuenta;

        public static DataRow ScriptsInfo { set { _drInfo = value; } }
        public static DataRow ScriptsCuenta { set { _drCuenta = value; } }

        /// <summary>
        /// Obtiene el formato del txt para guardarlo en el Script.
        /// </summary>
        static public string ExtraeFormatoScript(RichTextBox txtScript) {
            bool bNegrita = false, bColor = false;
            string sScript = "";

            for ( int iPosición = 0; iPosición < txtScript.Text.Length; iPosición++ ) {
                //seleccionas caracter
                txtScript.Select(iPosición, 1);

                if ( txtScript.SelectionFont.Bold ) {
                    if ( !bNegrita )
                        sScript += "*";

                    bNegrita = true;
                } else if ( bNegrita ) {
                    sScript += "*";

                    bNegrita = false;
                }

                if ( txtScript.SelectionColor == System.Drawing.Color.GreenYellow ) {
                    if ( !bColor )
                        sScript += "&";

                    bColor = true;
                } else if ( bColor ) {
                    sScript += "&";

                    bColor = false;
                }

                sScript += txtScript.SelectedText;
            }
            if ( bNegrita )
                sScript += "*";
            if ( bColor )
                sScript += "&";

            return sScript;
        }
        /// <summary>
        /// Le da formato al script reemplazando "*" y "&" por texto resaltado y coloreado respectivamente
        /// </summary>
        /// <param name="txtScript">RichTextBox al cual se le pasará el texto ya formateado</param>
        /// <param name="Script">Script a formatear</param>
        /// <param name="bValores">Reemplaza los campos calculados</param>
        static public void FormatoScript(RichTextBox txtScript, string Script, bool bValores) {
            string sLlave = "";

            bool bLlave = false;

            int iLlave = 0,
               iAsteriscoInicio = -1,
               iAmpersantInicio = -1,
               iSaltos = 0;

            ArrayList lstAsteriscos = new ArrayList();
            ArrayList lsAmpersants = new ArrayList();
            for ( int i = 0; i < Script.Length; i++ ) {
                // Reemplazo de []
                if ( Script[i] == ']' && bValores == true ) {
                    bLlave = false;
                    Script = Script.Replace("[" + sLlave + "]", ReemplazaInfo(sLlave));
                    sLlave = "";
                    i = iLlave;
                    if ( i >= Script.Length )
                        break;
                }

                if ( bLlave )
                    sLlave += Script[i];

                if ( Script[i] == '[' && bValores == true ) {
                    iLlave = i;
                    bLlave = true;
                    sLlave = "";
                }

                if ( Script[i] == '*' )
                    if ( iAsteriscoInicio >= 0 ) {
                        iAsteriscoInicio -= iSaltos;
                        lstAsteriscos.Add(new int[] { iAsteriscoInicio, i - iAsteriscoInicio - iSaltos });
                        Script = Script.Remove(i--, 1);
                        iAsteriscoInicio = -1;
                    } else {
                        iAsteriscoInicio = i;
                        Script = Script.Remove(i--, 1);
                    } else if ( Script[i] == '&' )
                    if ( iAmpersantInicio >= 0 ) {
                        iAmpersantInicio -= iSaltos;
                        lsAmpersants.Add(new int[] { iAmpersantInicio, i - iAmpersantInicio - iSaltos });
                        Script = Script.Remove(i--, 1);
                        iAmpersantInicio = -1;
                    } else {
                        iAmpersantInicio = i;
                        Script = Script.Remove(i--, 1);
                    } else if ( Script[i] == '\r' )
                    iSaltos++;
            }


            txtScript.Text = Script;

            txtScript.Select(0, txtScript.TextLength);
            txtScript.SelectionColor = Colores.Blanco;
            txtScript.SelectionFont = new System.Drawing.Font(txtScript.Font.FontFamily, txtScript.Font.Size, System.Drawing.FontStyle.Regular);

            foreach ( int[] iInicioLargo in lstAsteriscos ) {
                txtScript.Select(iInicioLargo[0], iInicioLargo[1]);
                txtScript.SelectionFont = new System.Drawing.Font(txtScript.Font.FontFamily, txtScript.Font.Size, System.Drawing.FontStyle.Bold);
            }
            foreach ( int[] iInicioLargo in lsAmpersants ) {
                txtScript.Select(iInicioLargo[0], iInicioLargo[1]);
                txtScript.SelectionColor = Colores.Resaltado2;
            }

            txtScript.Select(0, 0);
        }
        /// <summary>
        /// Convierte los valores por campos calculados.
        /// </summary>
        /// <param name="txtScript">TextBox al cual se le pasará el texto ya convertido.</param>
        /// <param name="Script">Script a formatear</param>        
        static public void FormatoScript(TextBox txtScript, string Script) {
            string sLlave = "";

            bool bLlave = false;

            int iLlave = 0;

            ArrayList lstAsteriscos = new ArrayList();
            ArrayList lsAmpersants = new ArrayList();
            for ( int i = 0; i < Script.Length; i++ ) {
                // Reemplazo de []
                if ( Script[i] == ']' ) {
                    bLlave = false;
                    Script = Script.Replace("[" + sLlave + "]", ReemplazaInfo(sLlave));
                    sLlave = "";
                    i = iLlave;
                    if ( i >= Script.Length )
                        break;
                }

                if ( bLlave )
                    sLlave += Script[i];

                if ( Script[i] == '[' ) {
                    iLlave = i;
                    bLlave = true;
                    sLlave = "";
                }

            }

            txtScript.Text = Script;
        }
        /// <summary>
        /// Reemplaza la informacion por el contenido en las tablas de cuenta, producto o el nombre del ejecutivo, según sea el caso
        /// </summary>
        /// <param name="Reemplazo">String a reemplazar</param>
        /// <returns></returns>
        static public string ReemplazaInfo(string Reemplazo) {
            // Reemplaza [] por los info.   
            string sInfo = "";

            if ( _drInfo.Table.Columns.Contains(Reemplazo) ) //Tabla cuenta
                sInfo = _drInfo[Reemplazo].ToString().Trim();
            else if ( Reemplazo == "NombreEjecutivo" )
                sInfo = Ejecutivo.Datos["NombreEjecutivo"].ToString();
            else if ( _drCuenta.Table.Columns.Contains(Reemplazo) )
                sInfo = _drCuenta[Reemplazo].ToString().Trim();
            if ( Reemplazo == "idCuenta" )
                sInfo = sInfo.Substring(sInfo.Length - 4);

            return sInfo;
        }


        // Comentarios
        /// <summary>
        /// Encuentra teléfonos y los reemplaza por X.
        /// </summary>
        /// <param name="Comentario">Texto con el comentario a reemplazar.</param>
        /// <returns>Comentario sin teléfonos.</returns>
        static public string QuitaTeléfonos(string Comentario) {
            string sComentarioOriginal = Comentario, sTelefonoSinModificar = "", sTelefonoModificado = "";
            int iContador = 0;
            Regex validador = new Regex("^([+\\d!/!\\\\$*!#%&/()=?¡_<>{}]|-)");
            for ( int i = 0; i < Comentario.Length; i++ ) {
                char s = Comentario[i];
                if ( validador.IsMatch(s.ToString()) || char.IsWhiteSpace(s) ) {
                    sTelefonoSinModificar += s;
                    if ( char.IsNumber(s) )
                        iContador++;
                }

                if ( ( !validador.IsMatch(s.ToString()) && !char.IsWhiteSpace(s) ) || i == Comentario.Length - 1 ) {
                    if ( iContador >= 10 ) {
                        sTelefonoModificado = " " + sTelefonoSinModificar.Replace(" ", "") + " ";
                        sTelefonoModificado = sTelefonoModificado.Replace("-", "");
                        sTelefonoModificado = Regex.Replace(sTelefonoModificado, "[+|-|/|\\\\|!|#|$|%|&|/|(|)|=|?|¡'|¿|*|$|<|>|_|;|:|[|]|{|}|]", "");
                        sComentarioOriginal = sComentarioOriginal.Replace(sTelefonoSinModificar, " XXXX-" + sTelefonoModificado.Substring(sTelefonoModificado.Length - 5, 4) + " ");
                        iContador = 0;
                        sTelefonoSinModificar = "";
                    }
                    if ( iContador < 10 ) {
                        iContador = 0;
                        sTelefonoSinModificar = "";
                    }
                }

            }

            return sComentarioOriginal;
        }
    }
    #endregion

    #region colores
    static class Colores {

        static public System.Drawing.Color Escritura { get { return System.Drawing.Color.DarkSlateGray; } }
        static public System.Drawing.Color Fondo { get { return System.Drawing.Color.FromArgb(64, 64, 64); } }

        static public System.Drawing.Color Blanco { get { return System.Drawing.Color.WhiteSmoke; } }
        static public System.Drawing.Color GrisClaro { get { return System.Drawing.Color.Gainsboro; } }
        static public System.Drawing.Color Gris { get { return System.Drawing.Color.Silver; } }
        static public System.Drawing.Color GrisOscuro { get { return System.Drawing.Color.Gray; } }

        static public System.Drawing.Color Rojo { get { return System.Drawing.Color.Crimson; } }
        static public System.Drawing.Color Verde { get { return System.Drawing.Color.SeaGreen; } }

        static public System.Drawing.Color Resaltado1 { get { return System.Drawing.Color.Yellow; } }
        static public System.Drawing.Color Resaltado2 { get { return System.Drawing.Color.GreenYellow; } }

        static public System.Drawing.Color InstruccionesBarra { get { return System.Drawing.Color.FromArgb(192, 255, 255); } }
        static public System.Drawing.Color ResultadoBarra { get { return System.Drawing.Color.Aqua; } }
        static public System.Drawing.Color ErrorBarra { get { return System.Drawing.Color.FromArgb(255, 128, 128); } }

        static public System.Drawing.Color MenúGris { get { return System.Drawing.Color.Silver; } }
        static public System.Drawing.Color MenuRojo { get { return System.Drawing.Color.IndianRed; } }
        static public System.Drawing.Color MenúVerde { get { return System.Drawing.Color.DarkSeaGreen; } }

        static public System.Drawing.Color Localización { get { return System.Drawing.Color.WhiteSmoke; } }
        static public System.Drawing.Color NegociaciónClaro { get { return System.Drawing.Color.LightCoral; } }
        static public System.Drawing.Color NegociaciónOscuro { get { return System.Drawing.Color.DarkRed; } }
        static public System.Drawing.Color SeguimientoClaro { get { return System.Drawing.Color.LightGreen; } }
        static public System.Drawing.Color SeguimientoOscuro { get { return System.Drawing.Color.ForestGreen; } }
        static public System.Drawing.Color DefiniciónClaro { get { return System.Drawing.Color.DarkGray; } }
        static public System.Drawing.Color DefiniciónOscuro { get { return System.Drawing.Color.Gray; } }
    }
    #endregion

    #region controlEvents

    static class controlEvents {

        static public System.Drawing.Font RowFont = new System.Drawing.Font("Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular);

        // TextBox Events
        static public void txtOnlyChars_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !( char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Tab );
        }
        static public void txtOnlyCharsPeriod_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !( char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Tab
                || e.KeyChar == '.' );
        }
        static public void txtOnlyCharsCommaPeriod_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !( char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Tab
                || e.KeyChar == '.' || e.KeyChar == ',' );
        }
        static public void txtOnlyMail_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !(
                char.IsLetterOrDigit(e.KeyChar) ||
                e.KeyChar == (char)Keys.Back ||
                e.KeyChar == (char)Keys.Tab ||
                e.KeyChar == '.' ||
                e.KeyChar == '-' ||
                e.KeyChar == '_'
            );
        }
        static public void txtOnlyEMail_KeyPress(object sender, KeyPressEventArgs e) {

            if ( e.KeyChar == '@' ) {
                Control txtSender = (Control)sender;
                e.Handled = txtSender.Text.Contains("@");
                return;
            }


            e.Handled = !(
                char.IsLetterOrDigit(e.KeyChar) ||
                e.KeyChar == (char)Keys.Back ||
                e.KeyChar == (char)Keys.Tab ||
                e.KeyChar == '.' ||
                e.KeyChar == '-' ||
                e.KeyChar == '_' ||
                e.KeyChar == '@'
            );
        }
        static public void txtOnlyNumbers_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !( char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Tab );
        }
        static public void txtOnlyNumbersLetters_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = !( char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || e.KeyChar == (char)Keys.Tab );
        }
        static public void txtOnlyNumbersPoint_KeyPress(object sender, KeyPressEventArgs e) {
            Control txtSender = (Control)sender;
            e.Handled = !( char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Tab || e.KeyChar == '.' );

            if ( e.KeyChar == '.' && txtSender.Text.Contains(".") )
                e.Handled = true;
        }

        static public void txtCampo_Focus(object sender, EventArgs e) {
            TextBox txtCampo = (TextBox)sender;

            if ( txtCampo.SelectionLength == txtCampo.Text.Length ) {
                txtCampo.SelectionStart = txtCampo.Text.Length;
                txtCampo.SelectionLength = 0;
            }

            if ( txtCampo.ForeColor != Colores.Escritura && txtCampo.ContainsFocus ) {
                txtCampo.TextAlign = HorizontalAlignment.Left;
                txtCampo.ForeColor = Colores.Escritura;
                txtCampo.Text = "";
            }

        }
        static public void txtCampo_Desfocus(object sender, EventArgs e) {
            TextBox txtCampo = (TextBox)sender;

            if ( txtCampo.Text.Trim() == "" && !txtCampo.ContainsFocus ) {
                txtCampo.TextAlign = HorizontalAlignment.Center;
                txtCampo.ForeColor = System.Drawing.Color.DarkGray;
                txtCampo.Text = txtCampo.Tag == null ? txtCampo.Name.Replace("txt", "") : txtCampo.Tag.ToString();
            }
        }
        static public void txtCampo_TextChanged(object sender, EventArgs e) {
            TextBox txtCampo = (TextBox)sender;

            int iSel = txtCampo.SelectionStart,
                iSímbolo = 0;

            string sTextoNuevo = "";
            foreach ( char caracter in txtCampo.Text )
                if ( char.IsLetterOrDigit(caracter) || char.IsWhiteSpace(caracter) )
                    sTextoNuevo += caracter;
                else
                    iSímbolo++;

            txtCampo.Text = sTextoNuevo.Trim() + ( txtCampo.Text.EndsWith(" ") ? " " : "" );
            txtCampo.SelectionStart = iSel - iSímbolo;
        }

        // Datagrid
        /// <summary>
        /// Agrega la fila de total al data grid view.
        /// </summary>
        /// <param name="dgvSorted"></param>
        static public void SortTotalRow(this DataGridView dgvSorted) {

            if ( dgvSorted.DataSource == null || dgvSorted.Rows.Count == 0 )
                return;

            DataTable tblSorted = (DataTable)dgvSorted.DataSource;
            string sSort = tblSorted.DefaultView.Sort;

            if ( dgvSorted.Columns.Contains("SortColumn") ) {
                dgvSorted.Columns["SortColumn"].Visible = false;
                dgvSorted.Rows[dgvSorted.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font(dgvSorted.DefaultCellStyle.Font.FontFamily, 9, System.Drawing.FontStyle.Bold); ;
                if ( sSort.Contains(",") )
                    dgvSorted.Columns[sSort.Split(',')[1].Trim().Replace("[", "").Replace("]", "").Replace(" DESC", "")].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
            } else
                if ( sSort != "" && sSort.Contains(",") )
                    if ( sSort.Split(',')[0].Contains(" DESC") )
                        dgvSorted.Columns[sSort.Split(',')[0].Replace(" DESC", "").Trim().Replace("[", "").Replace("]", "")].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
                    else
                        dgvSorted.Columns[sSort.Split(',')[0].Replace(" ASC", "").Trim().Replace("[", "").Replace("]", "")].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Descending;

            for ( int i = 0; i < dgvSorted.ColumnCount; i++ ) {
                dgvSorted.Columns[i].SortMode = DataGridViewColumnSortMode.Programmatic;
                if ( dgvSorted.Columns[i].Name.Contains("%") ) {
                    dgvSorted.Columns[i].DefaultCellStyle.Format = "P0";
                    dgvSorted.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                } else if ( dgvSorted.Columns[i].ValueType.Name == "Decimal" )
                    dgvSorted.Columns[i].DefaultCellStyle.Format = "C0";

            }

            dgvSorted.DefaultCellStyle.Font = RowFont;
            dgvSorted.ColumnHeaderMouseClick -= new DataGridViewCellMouseEventHandler(dgvSort_ColumnHeaderMouseClick);
            dgvSorted.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(dgvSort_ColumnHeaderMouseClick);
        }
        static private void dgvSort_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {

            DataGridView dgvSender = (DataGridView)sender;
            DataTable tblSort = (DataTable)dgvSender.DataSource;

            if ( tblSort == null )
                return;

            string sSort = tblSort.DefaultView.Sort;
            string sColumn = dgvSender.Columns[e.ColumnIndex].Name;
            bool bAscending = true,
                bSortColumn = tblSort.Columns.Contains("SortColumn");

            if ( sSort.Contains("DESC") ) {
                sSort = sColumn;
                bAscending = false;
            } else {
                sSort = sColumn + " DESC";
                bAscending = true;
            }

            tblSort.DefaultView.Sort =
                ( bSortColumn ? "SortColumn, " : "" )
                + sSort;

            if ( bSortColumn )
                dgvSender.Rows[dgvSender.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font(dgvSender.DefaultCellStyle.Font.FontFamily, 9, System.Drawing.FontStyle.Bold);
            dgvSender.Columns[e.ColumnIndex].HeaderCell.SortGlyphDirection = bAscending ? System.Windows.Forms.SortOrder.Ascending : System.Windows.Forms.SortOrder.Descending;
        }


        public class ComboboxItem {
            public string Text { get; set; }
            public int Value { get; set; }

            public ComboboxItem(string sText, int iValue) {
                this.Text = sText;
                this.Value = iValue;
            }
        }
    }

    #endregion

    
}


