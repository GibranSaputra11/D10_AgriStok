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
    public partial class KelolaBarang : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        private DataTable dtBarang = new DataTable();

        public KelolaBarang()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void KelolaBarang_Load(object sender, EventArgs e)
        {
            dataGridViewBarang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBarang.MultiSelect = false;
            dataGridViewBarang.ReadOnly = true;
            dataGridViewBarang.AllowUserToAddRows = false;
            dataGridViewBarang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtBarangID.ReadOnly = true;

            cmbSatuan.Enabled = false;

            LoadComboBoxKategori();
            LoadDataGrid();

            ClearForm();
        }

        private void LoadComboBoxKategori()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT Id_Kategori, Nama_Kategori FROM Kategori";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbKategori.DataSource = dt;
                    cmbKategori.DisplayMember = "Nama_Kategori";
                    cmbKategori.ValueMember = "Id_Kategori";

                    cmbKategori.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error load Kategori: " + ex.Message);
                }
            }
        }

        private void LoadComboBoxSatuan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id_Satuan, Nama_Satuan FROM Satuan";

                    string kategoriTerpilih = cmbKategori.Text.ToLower();


                    if (kategoriTerpilih.Contains("pupuk") || kategoriTerpilih.Contains("bibit") || kategoriTerpilih.Contains("benih"))
                    {
                        query += " WHERE Nama_Satuan IN ('Kg', 'Gram', 'Sak', 'Karung', 'Ton', 'Pack')";
                    }
                    else if (kategoriTerpilih.Contains("obat") || kategoriTerpilih.Contains("pestisida") || kategoriTerpilih.Contains("herbisida"))
                    {
                        query += " WHERE Nama_Satuan IN ('Liter', 'Botol', 'Mililiter', 'Pack')";
                    }
                    else if (kategoriTerpilih.Contains("alat") || kategoriTerpilih.Contains("kemasan"))
                    {
                        query += " WHERE Nama_Satuan IN ('Unit', 'Pcs', 'Box', 'Buah')";
                    }

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbSatuan.DataSource = dt;
                    cmbSatuan.DisplayMember = "Nama_Satuan";
                    cmbSatuan.ValueMember = "Id_Satuan";

                    cmbSatuan.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal memuat satuan: " + ex.Message);
                }
            }
        }

        private string GenerateID()
        {
            string newID = "BR-001";

            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                try
                {
                    localConn.Open();
                    string query = "SELECT TOP 1 Id_Barang FROM Barang ORDER BY Id_Barang DESC";
                    SqlCommand cmd = new SqlCommand(query, localConn);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastID = result.ToString(); 
                        int number = int.Parse(lastID.Split('-')[1]);
                        number++;
                        newID = "BR-" + number.ToString("D3"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal generate ID: " + ex.Message);
                }
            }
            return newID;
        }

        private void btnSaveBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text))
            {
                MessageBox.Show("ID Barang tidak valid atau kosong! Sistem akan membuat ulang ID.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarangID.Text = GenerateID();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) || cmbSatuan.SelectedValue == null || cmbKategori.SelectedValue == null)
            {
                MessageBox.Show("Harap isi semua data dengan lengkap!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) || txtNamaBarang.Text.Length < 3)
            {
                MessageBox.Show("Nama tidak boleh terlalu pendek!\nMasukkan minimal 3 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtNamaBarang.Text, @"^[a-zA-Z0-9\s\.,]+$"))
            {
                MessageBox.Show("Nama mengandung simbol yang tidak diizinkan!\nHanya gunakan huruf, angka, spasi, titik (.), atau koma (,).", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertBarang", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", txtBarangID.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNamaBarang.Text);
                cmd.Parameters.AddWithValue("@IdSatuan", cmbSatuan.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@IdKategori", cmbKategori.SelectedValue.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Data Barang berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show("Terjadi Kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }
        }

        private void ClearForm()
        {
            bindingSourceBarang.AddNew();
            txtBarangID.Text = GenerateID();

            txtNamaBarang.Clear();
            cmbSatuan.SelectedIndex = -1;
            cmbKategori.SelectedIndex = -1;

            txtNamaBarang.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                string query = "SELECT * FROM vw_KelolaBarang";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    dtBarang = new DataTable();
                    da.Fill(dtBarang); 

                    bindingSourceBarang.DataSource = dtBarang;

                    dataGridViewBarang.DataSource = bindingSourceBarang;
                    bindingNavigatorBarang.BindingSource = bindingSourceBarang;

                    BindControls();
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void BindControls()
        {
            txtBarangID.DataBindings.Clear();
            txtNamaBarang.DataBindings.Clear();
            cmbKategori.DataBindings.Clear();
            cmbSatuan.DataBindings.Clear();

            //txtBarangID.DataBindings.Add("Text", bindingSourceBarang, "Id_Barang");
            //txtNamaBarang.DataBindings.Add("Text", bindingSourceBarang, "Nama_Barang");

            //cmbKategori.DataBindings.Add("SelectedValue", bindingSourceBarang, "Id_Kategori");
            //cmbSatuan.DataBindings.Add("SelectedValue", bindingSourceBarang, "Id_Satuan");
        }

       
        private void btnUpdateBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text)) return;

            if (txtBarangID.Text == GenerateID())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_UpdateBarang", conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", txtBarangID.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNamaBarang.Text);
                cmd.Parameters.AddWithValue("@IdSatuan", cmbSatuan.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@IdKategori", cmbKategori.SelectedValue.ToString());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Barang berhasil diupdate!");

                LoadDataGrid();
                ClearForm();

            }
            catch (Exception ex) 
            {
                MessageBox.Show("Gagal memperbarui data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { conn.Close(); }
        }

        private void btnDeleteBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text)) return;

            if (txtBarangID.Text == GenerateID())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Barang ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    SqlCommand cmd = new SqlCommand("sp_DeleteBarang", conn);

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Id", txtBarangID.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                    ClearForm();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show("Sistem Menolak:\n\n" + ex.Message, "Peringatan Stok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally { conn.Close(); }
            }
        }

        private void btnAddSatuan_Click(object sender, EventArgs e)
        {
            AddSatuan addSatuan = new AddSatuan();
            addSatuan.ShowDialog();
        }

        private void addKategori_Click(object sender, EventArgs e)
        {
            AddKategori addKategori = new AddKategori();
            addKategori.ShowDialog();
        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKategori.SelectedIndex != -1)
            {
                string kategori = cmbKategori.Text.ToLower();

                LoadComboBoxSatuan();

                cmbSatuan.Enabled = true;
            }
            else
            {
                cmbSatuan.DataSource = null;
                cmbSatuan.Enabled = false;
            }
        }
    }
}
