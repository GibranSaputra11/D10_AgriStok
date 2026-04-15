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

namespace AgriStok
{
    public partial class Dashboard : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public Dashboard()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void ConnectDatabase()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                MessageBox.Show("Koneksi berhasil!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi Gagal" + ex.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectDatabase();
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
            DaftarTransakiOut daftarTransakiOut = new DaftarTransakiOut();
            daftarTransakiOut.ShowDialog();
        }
    }
}
