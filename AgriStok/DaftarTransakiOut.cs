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
    public partial class DaftarTransakiOut : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public DaftarTransakiOut()
        {
            InitializeComponent();
        }

        private void DaftarTransakiOut_Load(object sender, EventArgs e)
        {
            LoadMasterData();
        }

        private void LoadMasterData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT t.Id_Out AS [ID Transaksi], 
                                            k.Nama_Kelompok AS [Nama Kelompok Tani], 
                                            t.Tgl_Out AS [Tanggal Keluar], 
                                            t.Total_Barang_Out AS [Total Item]
                                     FROM Transaksi_Out t
                                     INNER JOIN KelompokTani k ON t.Id_Kelompok = k.Id_Kelompok
                                     ORDER BY t.Tgl_Out DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMaster.DataSource = dt;

                    dgvDetail.DataSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data Transaksi Keluar: " + ex.Message);
                }
            }
        }
    }
}
