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
    public partial class DaftarTransaksiOut : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public DaftarTransaksiOut()
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

            bindingSourceMaster.PositionChanged += BindingSourceMaster_PositionChanged;

            lblItemCount.Text = "Total Jenis Barang: 0";

            LoadMasterData();
        }

        private void LoadMasterData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    bindingSourceMaster.Filter = "";

                    string query = "SELECT * FROM vw_DaftarTransaksiOut ORDER BY [Tanggal Keluar] DESC, [ID Transaksi] DESC";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    bindingSourceMaster.DataSource = dt;
                    dgvMaster.DataSource = bindingSourceMaster;

                    if (bindingNavigatorMaster != null)
                    {
                        bindingNavigatorMaster.BindingSource = bindingSourceMaster;
                    }

                    BindingSourceMaster_PositionChanged(null, null);
                }
                catch (Exception ex) { MessageBox.Show("Gagal memuat data Master: " + ex.Message); }
            }
        }

        private void LoadDetailData(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT [ID Barang], [Nama Barang], [Jumlah Keluar] FROM vw_DetailTransaksiOut WHERE [ID Transaksi] = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idTransaksi);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvDetail.DataSource = dt;

                    lblItemCount.Text = $"Total Jenis Barang: {dt.Rows.Count}";
                }
                catch (Exception ex) { MessageBox.Show("Gagal memuat detail barang: " + ex.Message); }
            }
        }

        private void BindingSourceMaster_PositionChanged(object sender, EventArgs e)
        {
            if (bindingSourceMaster.Current != null)
            {
                DataRowView currentRow = (DataRowView)bindingSourceMaster.Current;
                string idTransaksi = currentRow["ID Transaksi"].ToString();

                LoadDetailData(idTransaksi);
            }
            else
            {
                dgvDetail.DataSource = null;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSourceMaster.Filter = string.Format("[ID Transaksi] LIKE '%{0}%' OR [Nama Kelompok] LIKE '%{0}%'", txtSearch.Text);
            }
            catch (Exception)
            {
                bindingSourceMaster.Filter = "";
            }
        }

        private void btnAddOut_Click(object sender, EventArgs e)
        {
            TransaksiOut transaksiOut = new TransaksiOut();

            transaksiOut.ShowDialog();
            
            txtSearch.Clear();

            LoadMasterData();
        }

        private void btnEditOut_Click(object sender, EventArgs e)
        {
            if (dgvMaster.CurrentRow != null)
            {
                string idTransaksiTerpilih = dgvMaster.CurrentRow.Cells["ID Transaksi"].Value.ToString();

                TransaksiOut formTransaksi = new TransaksiOut();
                formTransaksi.BukaUntukEdit(idTransaksiTerpilih);
                formTransaksi.ShowDialog();

                LoadMasterData();
            }
            else
            {
                MessageBox.Show("Silakan pilih transaksi yang ingin diedit dari tabel terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
