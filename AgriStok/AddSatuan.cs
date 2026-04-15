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

    public partial class AddSatuan : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public AddSatuan()
        {
            InitializeComponent();
        }

        private void AddSatuan_Load(object sender, EventArgs e)
        {
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
            txtSatuanID.Text = GenerateID();
            txtNamaSatuan.Clear();
            txtNamaSatuan.Focus();
        }

        private void LoadDataGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Satuan AS [ID], Nama_Satuan AS [Nama Satuan] FROM Satuan", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvSatuan.DataSource = dt;
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaSatuan.Text)) return;

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
    }
}
