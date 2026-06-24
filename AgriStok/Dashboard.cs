using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriStok
{
    public partial class Dashboard : Form
    {
        private string GetConnectionString()
        {
            if (ConfigurationManager.ConnectionStrings["GudangDbConn"] != null)
            {
                return ConfigurationManager.ConnectionStrings["GudangDbConn"].ConnectionString;
            }
            return "";
        }

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
        }

        private void btnPutusKoneksi_Click(object sender, EventArgs e) 
        {
            DialogResult dialogResult = MessageBox.Show("Yakin ingin memutuskan koneksi dan mengganti pengaturan server database?", "Konfirmasi Putus Koneksi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    if (config.ConnectionStrings.ConnectionStrings["GudangDbConn"] != null)
                    {
                        config.ConnectionStrings.ConnectionStrings.Remove("GudangDbConn");
                        config.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }

                    Application.Restart();
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memutuskan koneksi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                try
                {
                    conn.Open();
                    MessageBox.Show("Koneksi ke Database Stabil!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Koneksi Gagal!\n\nDetail: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnBarang_Click(object sender, EventArgs e)
        {
            KelolaBarang kelolaBarang = new KelolaBarang();
            kelolaBarang.ShowDialog();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            KelolaSupplier kelolaSupplier = new KelolaSupplier();
            kelolaSupplier.ShowDialog();
        }

        private void btnTani_Click(object sender, EventArgs e)
        {
            KelolaKelompokTani kelolaKelompokTani = new KelolaKelompokTani();
            kelolaKelompokTani.ShowDialog();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            DaftarTransaksiIn daftarTransaksiIn = new DaftarTransaksiIn();
            daftarTransaksiIn.ShowDialog();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            DaftarTransaksiOut daftarTransakiOut = new DaftarTransaksiOut();
            daftarTransakiOut.ShowDialog();
        }

        private void btnStatistik_Click(object sender, EventArgs e)
        {
            StatistikaTransakiTahunan statistikTransakiTahunan = new StatistikaTransakiTahunan();
            statistikTransakiTahunan.ShowDialog();
        }
    }
}
