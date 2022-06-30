using System;
using System.Threading;
using System.Windows.Forms;

namespace Estadistica
{
    static class MainEstadistica
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread.Sleep(100);  // Espera para que no se empalmen las ejecuciones.
            using (Mutex mutex = new Mutex(false, "Global\\" + appGuid))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("Ya existe otra instancia de la aplicación abierta.\r\nSolo puede abrir esta aplicación una a la vez.",
                   "Estadistica - Aplicación doblemente abierta.", MessageBoxButtons.OK);
                    return;
                }

                if (!Funciones.CopyApp(args))
                    return;

                Application.Run(new frmLogin());

                if (Ejecutivo.Datos != null && Ejecutivo.Datos.Table.Columns.Contains("idEjecutivo") && Ejecutivo.Datos["idEjecutivo"].ToString() != "0" && Catálogo.Valores != null)
                    Application.Run(new frmMenú());

                //Ejecutivo.CierraSesión();

            }
        }

        private static string appGuid = "Estadistica - CJ";
    }
}
