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
    public partial class TransaksiOut : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public TransaksiOut()
        {
            InitializeComponent();
        }

        private void TransaksiOut_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadKelompokTani();
            LoadBarang();
            txtIDTransaksi.Text = GenerateID();
            txtIDTransaksi.ReadOnly = true;
            txtStokSekarang.ReadOnly = true;

        }

        private void SetupDataGridView()
        {
            dgvKeranjang.Columns.Clear();
            dgvKeranjang.Columns.Add("Id_Barang", "ID Barang");
            dgvKeranjang.Columns.Add("Nama_Barang", "Nama Barang");
            dgvKeranjang.Columns.Add("Jumlah", "Jumlah Keluar");
            dgvKeranjang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKeranjang.AllowUserToAddRows = false;
        }

        private void LoadKelompokTani()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Kelompok, Nama_Kelompok FROM KelompokTani", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbKelompok.DataSource = dt;
                cmbKelompok.DisplayMember = "Nama_Kelompok";
                cmbKelompok.ValueMember = "Id_Kelompok";
                cmbKelompok.SelectedIndex = -1;
            }
        }

        private void LoadBarang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Barang, Nama_Barang FROM Barang", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbBarang.DataSource = dt;
                cmbBarang.DisplayMember = "Nama_Barang";
                cmbBarang.ValueMember = "Id_Barang";
                cmbBarang.SelectedIndex = -1;
            }
        }

        private string GenerateID()
        {
            string newID = "TRO-001";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 Id_Out FROM Transaksi_Out ORDER BY Id_Out DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int number = int.Parse(result.ToString().Split('-')[1]);
                    newID = "TRO-" + (number + 1).ToString("D3");
                }
            }
            return newID;
        }

        private void cmbBarang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBarang.SelectedIndex != -1 && cmbBarang.SelectedValue != null)
            {
                if (cmbBarang.SelectedValue is string idBarang)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            string query = "SELECT Stok_Barang FROM Barang WHERE Id_Barang = @Id";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Id", idBarang);

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                txtStokSekarang.Text = result.ToString();
                            }
                            else
                            {
                                txtStokSekarang.Text = "0";
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Gagal memuat stok: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                txtStokSekarang.Clear();
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (cmbBarang.SelectedValue == null || string.IsNullOrEmpty(txtStokSekarang.Text)) return;

            string idBarang = cmbBarang.SelectedValue.ToString();
            string namaBarang = cmbBarang.Text;
            int jumlah = (int)numJumlah.Value;
            int stokTersedia = int.Parse(txtStokSekarang.Text);

            int totalDiminta = jumlah;
            foreach (DataGridViewRow row in dgvKeranjang.Rows)
            {
                if (row.Cells["Id_Barang"].Value.ToString() == idBarang)
                {
                    totalDiminta += (int)row.Cells["Jumlah"].Value;
                }
            }

            if (totalDiminta > stokTersedia)
            {
                MessageBox.Show($"Stok tidak mencukupi! Sisa stok {namaBarang} di gudang hanya {stokTersedia}.", "Peringatan Stok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            foreach (DataGridViewRow row in dgvKeranjang.Rows)
            {
                if (row.Cells["Id_Barang"].Value.ToString() == idBarang)
                {
                    row.Cells["Jumlah"].Value = (int)row.Cells["Jumlah"].Value + jumlah;
                    HitungTotal();
                    return;
                }
            }

            dgvKeranjang.Rows.Add(idBarang, namaBarang, jumlah);
            HitungTotal();
        }

        private void HitungTotal()
        {
            int total = 0;
            foreach (DataGridViewRow row in dgvKeranjang.Rows)
            {
                total += Convert.ToInt32(row.Cells["Jumlah"].Value);
            }
            lblTotal.Text = total.ToString();
        }
    }
}
