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
    public partial class KelolaKelompokTani : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

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
            txtKelompokID.Text = GenerateID();
            txtNamaKelompok.Clear();
            txtAlamatKelompok.Clear();
            txtTlpKelompok.Clear();
            txtNamaKelompok.Focus();
        }

        private void LoadDataGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id_Kelompok, Nama_Kelompok, Alamat_Kelompok, NoTlp_Kelompok FROM KelompokTani";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewKelompok.DataSource = dt;

                    dataGridViewKelompok.Columns["Id_Kelompok"].HeaderText = "ID Kelompok";
                    dataGridViewKelompok.Columns["Nama_Kelompok"].HeaderText = "Nama Kelompok Tani";
                    dataGridViewKelompok.Columns["Alamat_Kelompok"].HeaderText = "Alamat";
                    dataGridViewKelompok.Columns["NoTlp_Kelompok"].HeaderText = "No. Telepon";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Menampilkan Data: " + ex.Message);
                }
            }
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
            if (string.IsNullOrWhiteSpace(txtNamaKelompok.Text))
            {
                MessageBox.Show("Nama Kelompok Tani harus diisi!");
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
