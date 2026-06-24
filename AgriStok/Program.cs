using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriStok
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string connString = "";
            try
            {
                if (ConfigurationManager.ConnectionStrings["GudangDbConn"] != null)
                {
                    connString = ConfigurationManager.ConnectionStrings["GudangDbConn"].ConnectionString;
                }
            }
            catch { }

            bool isKoneksiAman = false;

            if (!string.IsNullOrEmpty(connString))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connString))
                    {
                        conn.Open();
                        isKoneksiAman = true; 
                    }
                }
                catch (Exception)
                {
                    isKoneksiAman = false;
                }
            }

            if (isKoneksiAman)
            {
                Application.Run(new Dashboard());
            }
            else
            {
                Application.Run(new FormKonfigurasiDB());
            }
        }
    }
}
