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
    public partial class AddKategori : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public AddKategori()
        {
            InitializeComponent();
        }

        private void AddKategori_Load(object sender, EventArgs e)
        {
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
            txtKategoriID.Text = GenerateID();
            txtNamaKategori.Clear();
            txtNamaKategori.Focus();
        }

        private void LoadDataGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Kategori AS [ID], Nama_Kategori AS [Nama Kategori] FROM Kategori", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvKategori.DataSource = dt;
                }
                catch (Exception ex) { MessageBox.Show("Gagal load tabel: " + ex.Message); }
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaKategori.Text))
            {
                MessageBox.Show("Nama Kategori tidak boleh kosong!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Kategori (Id_Kategori, Nama_Kategori) VALUES (@Id, @Nama)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtKategoriID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaKategori.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Kategori berhasil disimpan!");
                    ClearForm();
                    LoadDataGrid();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKategoriID.Text)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Kategori SET Nama_Kategori = @Nama WHERE Id_Kategori = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtKategoriID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaKategori.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Data Kategori berhasil diupdate!");
                    ClearForm();
                    LoadDataGrid();
                }
                catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKategoriID.Text)) return;

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Kategori ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Kategori WHERE Id_Kategori = @Id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", txtKategoriID.Text);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil dihapus!");
                        ClearForm();
                        LoadDataGrid();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Kategori tidak bisa dihapus karena sedang digunakan oleh data Barang di gudang!\n\nDetail: " + ex.Message, "Gagal Hapus", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvKategori_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKategori.Rows[e.RowIndex];
                txtKategoriID.Text = row.Cells["ID"].Value.ToString();
                txtNamaKategori.Text = row.Cells["Nama Kategori"].Value.ToString();
            }
        }
    }
}
