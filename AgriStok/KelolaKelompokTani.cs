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
    public partial class KelolaKelompokTani : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        private DataTable dtKelompok = new DataTable();

        public KelolaKelompokTani()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void KelolaKelompokTani_Load(object sender, EventArgs e)
        {
            dataGridViewKelompok.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewKelompok.MultiSelect = false;
            dataGridViewKelompok.ReadOnly = true;
            dataGridViewKelompok.AllowUserToAddRows = false;
            dataGridViewKelompok.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtKelompokID.ReadOnly = true;

            LoadDataGrid();
            ClearForm();
        }

        private string GenerateID()
        {
            string newID = "KL-001"; 

            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                try
                {
                    localConn.Open();
                    string query = "SELECT TOP 1 Id_Kelompok FROM KelompokTani ORDER BY Id_Kelompok DESC";
                    SqlCommand cmd = new SqlCommand(query, localConn);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastID = result.ToString(); 
                        int number = int.Parse(lastID.Split('-')[1]);
                        number++;
                        newID = "KL-" + number.ToString("D3"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal generate ID Kelompok Tani: " + ex.Message);
                }
            }
            return newID;
        }

        private void ClearForm()
        {
            bindingSourceKelompok.AddNew();
            txtKelompokID.Text = GenerateID();
            txtNamaKelompok.Clear();
            txtAlamatKelompok.Clear();
            txtTlpKelompok.Clear();
            txtNamaKelompok.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                string query = "SELECT * FROM vw_KelolaKelompokTani";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    dtKelompok = new DataTable();
                    da.Fill(dtKelompok);

                    bindingSourceKelompok.DataSource = dtKelompok;

                    dataGridViewKelompok.DataSource = bindingSourceKelompok;
                    bindingNavigatorKelompok.BindingSource = bindingSourceKelompok;

                    BindControls();
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void BindControls()
        {
            txtKelompokID.DataBindings.Clear();
            txtNamaKelompok.DataBindings.Clear();
            txtTlpKelompok.DataBindings.Clear();
            txtAlamatKelompok.DataBindings.Clear();

            txtKelompokID.DataBindings.Add("Text", bindingSourceKelompok, "Id_Kelompok");
            txtNamaKelompok.DataBindings.Add("Text", bindingSourceKelompok, "Nama_Kelompok");
            txtTlpKelompok.DataBindings.Add("Text", bindingSourceKelompok, "NoTlp_Kelompok");
            txtAlamatKelompok.DataBindings.Add("Text", bindingSourceKelompok, "Alamat_Kelompok");
        }

        private void dataGridViewKelompok_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewKelompok.Rows[e.RowIndex];

                txtKelompokID.Text = row.Cells["Id_Kelompok"].Value.ToString();
                txtNamaKelompok.Text = row.Cells["Nama_Kelompok"].Value.ToString();
                txtAlamatKelompok.Text = row.Cells["Alamat_Kelompok"].Value.ToString();
                txtTlpKelompok.Text = row.Cells["NoTlp_Kelompok"].Value.ToString();
            }
        }

        private void btnAddKelompok_Click(object sender, EventArgs e)
        {
            string nama = txtNamaKelompok.Text.Trim();
            string noTelp = txtTlpKelompok.Text.Trim();
            string alamat = txtAlamatKelompok.Text.Trim();

            if (string.IsNullOrWhiteSpace(nama) || nama.Length < 3)
            {
                MessageBox.Show("Nama tidak boleh kosong atau terlalu pendek!\nMasukkan minimal 3 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(nama, @"^[a-zA-Z0-9\s\.,]+$"))
            {
                MessageBox.Show("Nama mengandung simbol yang tidak diizinkan!\nHanya gunakan huruf, angka, spasi, titik (.), atau koma (,).", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(noTelp) || !Regex.IsMatch(noTelp, @"^[0-9]{10,13}$"))
            {
                MessageBox.Show("Nomor Telepon tidak valid!\nPastikan hanya berisi angka dan berjumlah 10 hingga 13 digit (Contoh: 081234567890).", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(alamat) || alamat.Length < 5)
            {
                MessageBox.Show("Alamat harus diisi dengan jelas!\nMasukkan detail alamat minimal 5 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO KelompokTani (Id_Kelompok, Nama_Kelompok, Alamat_Kelompok, NoTlp_Kelompok) 
                                     VALUES (@Id, @Nama, @Alamat, @NoTlp)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtKelompokID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaKelompok.Text);
                    cmd.Parameters.AddWithValue("@Alamat", txtAlamatKelompok.Text);
                    cmd.Parameters.AddWithValue("@NoTlp", txtTlpKelompok.Text);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data Kelompok Tani berhasil ditambahkan!");
                        ClearForm();
                        LoadDataGrid();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Terjadi Kesalahan: " + ex.Message); }
            }
        }

        private void btnUpdateKelompok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKelompokID.Text)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"UPDATE KelompokTani 
                                     SET Nama_Kelompok = @Nama, 
                                         Alamat_Kelompok = @Alamat, 
                                         NoTlp_Kelompok = @NoTlp 
                                     WHERE Id_Kelompok = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtKelompokID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaKelompok.Text);
                    cmd.Parameters.AddWithValue("@Alamat", txtAlamatKelompok.Text);
                    cmd.Parameters.AddWithValue("@NoTlp", txtTlpKelompok.Text);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data Kelompok Tani berhasil diupdate!");
                        ClearForm();
                        LoadDataGrid();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Terjadi Kesalahan: " + ex.Message); }
            }
        }

        private void btnDeleteKelompok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKelompokID.Text)) return;

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Kelompok Tani ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM KelompokTani WHERE Id_Kelompok = @Id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", txtKelompokID.Text);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus!");
                            ClearForm();
                            LoadDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data tidak bisa dihapus karena Kelompok Tani ini memiliki riwayat transaksi keluar.\n\nDetail: " + ex.Message);
                    }
                }
            }
        }
    }
}
