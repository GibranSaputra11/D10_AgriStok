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
    public partial class DaftarTransaksiIn : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";
        public DaftarTransaksiIn()
        {
            InitializeComponent();
        }

        private void DaftarTransaksiIn_Load(object sender, EventArgs e)
        {
            dgvMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaster.AllowUserToAddRows = false;
            dgvMaster.ReadOnly = true;
            dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadMasterData();

        }

        private void LoadMasterData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT t.Id_In AS [ID Transaksi], 
                                            s.Nama_Supplier AS [Nama Supplier], 
                                            t.Tgl_In AS [Tanggal Masuk], 
                                            t.Total_Barang_In AS [Total Item]
                                     FROM Transaksi_In t
                                     INNER JOIN Supplier s ON t.Id_Supplier = s.Id_Supplier
                                     ORDER BY t.Tgl_In DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMaster.DataSource = dt;

                    dgvDetail.DataSource = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat data Transaksi Masuk: " + ex.Message);
                }
            }
        }
    }
}
