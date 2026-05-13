using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AgriStok
{

    public partial class AddSatuan : Form
    {
        private SqlConnection conn;
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        private DataTable dtSatuan = new DataTable();

        public AddSatuan()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void AddSatuan_Load(object sender, EventArgs e)
        {
            dgvSatuan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSatuan.MultiSelect = false;
            dgvSatuan.ReadOnly = true;
            dgvSatuan.AllowUserToAddRows = false;
            dgvSatuan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtSatuanID.ReadOnly = true;
            LoadDataGrid();
            ClearForm();
        }

        private string GenerateID()
        {
            string newID = "ST-001";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TOP 1 Id_Satuan FROM Satuan ORDER BY Id_Satuan DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int number = int.Parse(result.ToString().Split('-')[1]);
                        newID = "ST-" + (number + 1).ToString("D3");
                    }
                }
                catch (Exception ex) { MessageBox.Show("Gagal Generate ID: " + ex.Message); }
            }
            return newID;
        }

        private void ClearForm()
        {
            bindingSourceSatuan.AddNew();
            txtSatuanID.Text = GenerateID();
            txtNamaSatuan.Clear();
            txtNamaSatuan.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                string query = "SELECT * FROM vw_Satuan";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    dtSatuan = new DataTable();
                    da.Fill(dtSatuan);

                    bindingSourceSatuan.DataSource = dtSatuan;
                    dgvSatuan.DataSource = bindingSourceSatuan;
                    bindingNavigatorSatuan.BindingSource = bindingSourceSatuan;

                    BindControls();
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void BindControls()
        {
            txtSatuanID.DataBindings.Clear();
            txtNamaSatuan.DataBindings.Clear();

            txtSatuanID.DataBindings.Add("Text", bindingSourceSatuan, "Id_Satuan");
            txtNamaSatuan.DataBindings.Add("Text", bindingSourceSatuan, "Nama_Satuan");
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {

            string input = txtNamaSatuan.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtNamaSatuan.Text)) return;

            if (input.Length < 2)
            {
                MessageBox.Show("Input terlalu pendek! Masukkan minimal 2 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Input tidak valid! Hanya boleh menggunakan huruf, tanpa simbol (@, #, dll) atau angka.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] kataTerlarang = { 
                "pupuk", "bibit", "alat", "pestisida", "herbisida", "obat", "benih", "kemasan",
        
                "hektar", "hek", "meter", "cm", "km", "mil", "are",
        
                "jam", "hari", "bulan", "tahun", "minggu", "detik",
        
                "test", "testing", "coba", "admin", "user", "dummy", "asdf"
            };

            if (kataTerlarang.Any(kata => input.ToLower().Contains(kata)))
            {
                MessageBox.Show("Input mengandung kata terlarang! Mohon masukkan kategori yang valid.",
                                "Validasi Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Satuan (Id_Satuan, Nama_Satuan) VALUES (@Id, @Nama)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtSatuanID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaSatuan.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Satuan berhasil disimpan!");
                    ClearForm();
                    LoadDataGrid();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Satuan SET Nama_Satuan = @Nama WHERE Id_Satuan = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtSatuanID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaSatuan.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Satuan berhasil diupdate!");
                    ClearForm();
                    LoadDataGrid();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }



        private void dgvSatuan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSatuan.Rows[e.RowIndex];
                txtSatuanID.Text = row.Cells["ID"].Value.ToString();
                txtNamaSatuan.Text = row.Cells["Nama Satuan"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSatuanID.Text)) return;

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Satuan ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Satuan WHERE Id_Satuan = @Id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", txtSatuanID.Text);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil dihapus!");
                        ClearForm();
                        LoadDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Satuan tidak bisa dihapus karena sedang digunakan oleh data Barang di gudang!\n\nDetail: " + ex.Message, "Gagal Hapus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
