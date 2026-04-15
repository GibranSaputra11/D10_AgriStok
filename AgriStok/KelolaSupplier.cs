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
    public partial class KelolaSupplier : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public KelolaSupplier()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void KelolaSupplier_Load(object sender, EventArgs e)
        {
            txtSupplierID.ReadOnly = true;

            LoadDataGrid();
            ClearForm();
        }

        private string GenerateID()
        {
            string newID = "SP-001";

            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                try
                {
                    localConn.Open();
                    string query = "SELECT TOP 1 Id_Supplier FROM Supplier ORDER BY Id_Supplier DESC";
                    SqlCommand cmd = new SqlCommand(query, localConn);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastID = result.ToString(); 
                        int number = int.Parse(lastID.Split('-')[1]);
                        number++;
                        newID = "SP-" + number.ToString("D3"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal generate ID Supplier: " + ex.Message);
                }
            }
            return newID;
        }

        private void ClearForm()
        {
            txtSupplierID.Text = GenerateID();
            txtNamaSupplier.Clear();
            txtAlamatSupplier.Clear();
            txtTlpSupplier.Clear();
            txtNamaSupplier.Focus();
        }

        private void LoadDataGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                   
                    string query = "SELECT Id_Supplier, Nama_Supplier, Alamat_Supplier, NoTlp_Supplier FROM Supplier";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridViewSupplier.DataSource = dt;

                    dataGridViewSupplier.Columns["Id_Supplier"].HeaderText = "ID Supplier";
                    dataGridViewSupplier.Columns["Nama_Supplier"].HeaderText = "Nama Supplier";
                    dataGridViewSupplier.Columns["Alamat_Supplier"].HeaderText = "Alamat";
                    dataGridViewSupplier.Columns["NoTlp_Supplier"].HeaderText = "No. Telepon";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal Menampilkan Data: " + ex.Message);
                }
            }
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNamaSupplier.Text))
            {
                MessageBox.Show("Nama Supplier harus diisi!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO Supplier (Id_Supplier, Nama_Supplier, Alamat_Supplier, NoTlp_Supplier) 
                                     VALUES (@Id, @Nama, @Alamat, @NoTlp)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", txtSupplierID.Text);
                    cmd.Parameters.AddWithValue("@Nama", txtNamaSupplier.Text);
                    cmd.Parameters.AddWithValue("@Alamat", txtAlamatSupplier.Text);
                    cmd.Parameters.AddWithValue("@NoTlp", txtTlpSupplier.Text);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Data Supplier berhasil ditambahkan!");
                        ClearForm();
                        LoadDataGrid();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Terjadi Kesalahan: " + ex.Message); }
            }
        }
    }
}
