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
    public partial class AddKategori : Form
    {
        private SqlConnection conn;
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        private DataTable dtKategori = new DataTable();

        public AddKategori()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void AddKategori_Load(object sender, EventArgs e)
        {
            dgvKategori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKategori.MultiSelect = false;
            dgvKategori.ReadOnly = true;
            dgvKategori.AllowUserToAddRows = false;
            dgvKategori.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtKategoriID.ReadOnly = true;

            LoadDataGrid();
            ClearForm();
        }

        private string GenerateID()
        {
            string newID = "KT-001";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT TOP 1 Id_Kategori FROM Kategori ORDER BY Id_Kategori DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        int number = int.Parse(result.ToString().Split('-')[1]);
                        newID = "KT-" + (number + 1).ToString("D3");
                    }
                }
                catch (Exception ex) { MessageBox.Show("Gagal Generate ID: " + ex.Message); }
            }
            return newID;
        }

        private void ClearForm()
        {
            bindingSourceKategori.AddNew();
            txtKategoriID.Text = GenerateID();
            txtNamaKategori.Clear();
            txtNamaKategori.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                string query = "SELECT * FROM vw_Kategori";
                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    dtKategori = new DataTable();
                    da.Fill(dtKategori);

                    bindingSourceKategori.DataSource = dtKategori;
                    dgvKategori.DataSource = bindingSourceKategori;
                    bindingNavigatorKategori.BindingSource = bindingSourceKategori;

                    BindControls();
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void BindControls()
        {
            txtKategoriID.DataBindings.Clear();
            txtNamaKategori.DataBindings.Clear();

            txtKategoriID.DataBindings.Add("Text", bindingSourceKategori, "Id_Kategori");
            txtNamaKategori.DataBindings.Add("Text", bindingSourceKategori, "Nama_Kategori");
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string input = txtNamaKategori.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Nama Kategori tidak boleh kosong!");
                return;
            }

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
                    "hektar", "hek", "meter", "kilo", "kilogram", "kg", "liter", "ton", "gram",
                    "kuintal", "lusin", "kodi", "pcs", "box", "sak", "karung", "botol", "ons",

                    "baju", "celana", "sepatu", "elektronik", "laptop", "hp", "handphone",
                    "meja", "kursi", "motor", "mobil", "kendaraan", "skincare", "kosmetik",
                    "makanan", "minuman", "snack",

                    "jam", "hari", "bulan", "tahun", "minggu", "detik", "km", "cm", "mil",

                    "test", "testing", "coba", "admin", "user", "dummy", "asdf", "qwer"
                };

            if (kataTerlarang.Any(kata => input.ToLower().Contains(kata)))
            {
                MessageBox.Show("Input mengandung kata terlarang! Mohon masukkan kategori yang valid.",
                                "Validasi Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_InsertKategori", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", txtKategoriID.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNamaKategori.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Kategori berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            finally 
            { 
                conn.Close(); 
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKategoriID.Text)) return;

            if (txtKategoriID.Text == GenerateID())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SqlCommand cmd = new SqlCommand("sp_UpdateKategori", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", txtKategoriID.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNamaKategori.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Kategori berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show("Gagal update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            finally 
            { 
                conn.Close(); 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKategoriID.Text)) return;

            if (txtKategoriID.Text == GenerateID())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Kategori ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DeleteKategori", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", txtKategoriID.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                    ClearForm();
                }
                catch (Exception ex) 
                { 
                    MessageBox.Show("Sistem Menolak:\n\n" + ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
                }
                finally 
                { 
                    conn.Close(); 
                }
            }
        }
            
    }
}
