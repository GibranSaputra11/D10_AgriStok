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
    public partial class KelolaBarang : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

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
            LoadComboBoxSatuan();
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
            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) || cmbSatuan.SelectedValue == null || cmbKategori.SelectedValue == null)
            {
                MessageBox.Show("Harap isi semua data dengan lengkap!");
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
                ClearForm();
                LoadDataGrid();
            }
            catch (Exception ex) { MessageBox.Show("Terjadi Kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }
        }

        private void ClearForm()
        {
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
                string query = @"SELECT 
                                    b.Id_Barang, b.Nama_Barang, 
                                    b.Id_Satuan, s.Nama_Satuan, 
                                    b.Id_Kategori, k.Nama_Kategori, 
                                    b.Stok_Barang 
                                 FROM Barang b
                                 INNER JOIN Satuan s ON b.Id_Satuan = s.Id_Satuan
                                 INNER JOIN Kategori k ON b.Id_Kategori = k.Id_Kategori";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dataGridViewBarang.DataSource = dt;
                dataGridViewBarang.Columns["Id_Satuan"].Visible = false;
                dataGridViewBarang.Columns["Id_Kategori"].Visible = false;
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void dataGridViewBarang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBarang.Rows[e.RowIndex];

                txtBarangID.Text = row.Cells["Id_Barang"].Value.ToString();
                txtNamaBarang.Text = row.Cells["Nama_Barang"].Value.ToString();
                cmbSatuan.SelectedValue = row.Cells["Id_Satuan"].Value.ToString();
                cmbKategori.SelectedValue = row.Cells["Id_Kategori"].Value.ToString();
            }
        }

        private void btnUpdateBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text)) return;

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

                ClearForm();
                LoadDataGrid();

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

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Barang ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string query = "DELETE FROM Barang WHERE Id_Barang = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtBarangID.Text);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data berhasil dihapus!");
                        ClearForm(); 
                        LoadDataGrid();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Data gagal dihapus: " + ex.Message); }
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
