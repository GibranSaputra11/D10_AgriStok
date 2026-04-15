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
            dgvMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaster.AllowUserToAddRows = false;
            dgvMaster.ReadOnly = true;
            dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetail.AllowUserToAddRows = false;
            dgvDetail.ReadOnly = true;
            dgvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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

        private void LoadDetailData(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT b.Id_Barang AS [ID Barang], 
                                            b.Nama_Barang AS [Nama Barang], 
                                            d.Subtotal_Out AS [Jumlah Keluar]
                                     FROM Detail_Out d
                                     INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang
                                     WHERE d.Id_Out = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idTransaksi);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvDetail.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat detail barang: " + ex.Message);
                }
            }
        }

        private void dgvMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMaster.Rows[e.RowIndex];

                string idTransaksi = row.Cells["ID Transaksi"].Value.ToString();

                LoadDetailData(idTransaksi);
            }
        }

        private void btnAddOut_Click(object sender, EventArgs e)
        {
            TransaksiOut transaksiOut = new TransaksiOut();

            transaksiOut.ShowDialog();

            LoadMasterData();
        }

    }
}
