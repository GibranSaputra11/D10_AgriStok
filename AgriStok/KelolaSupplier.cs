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
    public partial class KelolaSupplier : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        private DataTable dtSupplier = new DataTable();

        public KelolaSupplier()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void KelolaSupplier_Load(object sender, EventArgs e)
        {
            dataGridViewSupplier.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSupplier.MultiSelect = false;
            dataGridViewSupplier.ReadOnly = true;
            dataGridViewSupplier.AllowUserToAddRows = false;
            dataGridViewSupplier.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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
            bindingSourceSupplier.AddNew();
            txtSupplierID.Text = GenerateID();
            txtNamaSupplier.Clear();
            txtAlamatSupplier.Clear();
            txtTlpSupplier.Clear();
            txtNamaSupplier.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                string query = "SELECT * FROM vw_KelolaSupplier";

                using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                {
                    dtSupplier = new DataTable();
                    da.Fill(dtSupplier);

                    bindingSourceSupplier.DataSource = dtSupplier;

                    dataGridViewSupplier.DataSource = bindingSourceSupplier;
                    bindingNavigatorSupplier.BindingSource = bindingSourceSupplier;

                    BindControls();
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void BindControls()
        {
            txtSupplierID.DataBindings.Clear();
            txtNamaSupplier.DataBindings.Clear();
            txtTlpSupplier.DataBindings.Clear();
            txtAlamatSupplier.DataBindings.Clear();

            txtSupplierID.DataBindings.Add("Text", bindingSourceSupplier, "Id_Supplier");
            txtNamaSupplier.DataBindings.Add("Text", bindingSourceSupplier, "Nama_Supplier");
            txtTlpSupplier.DataBindings.Add("Text", bindingSourceSupplier, "NoTlp_Supplier");
            txtAlamatSupplier.DataBindings.Add("Text", bindingSourceSupplier, "Alamat_Supplier");
        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            string nama = txtNamaSupplier.Text.Trim();
            string noTelp = txtTlpSupplier.Text.Trim();
            string alamat = txtAlamatSupplier.Text.Trim();

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

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_InsertSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure; 

                cmd.Parameters.AddWithValue("@Id", txtSupplierID.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNamaSupplier.Text);
                cmd.Parameters.AddWithValue("@NoTlp", txtTlpSupplier.Text);
                cmd.Parameters.AddWithValue("@Alamat", txtAlamatSupplier.Text);

                cmd.ExecuteNonQuery(); 

                MessageBox.Show("Data Supplier berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show("Terjadi Kesalahan: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            {
                conn.Close();
            }
        }

        
        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSupplierID.Text)) return;

            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                SqlCommand cmd = new SqlCommand("sp_UpdateSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", txtSupplierID.Text);
                cmd.Parameters.AddWithValue("@Nama", txtNamaSupplier.Text);
                cmd.Parameters.AddWithValue("@NoTlp", txtTlpSupplier.Text);
                cmd.Parameters.AddWithValue("@Alamat", txtAlamatSupplier.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Data Supplier berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show("Gagal memperbarui data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { conn.Close(); }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSupplierID.Text)) return;

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Supplier ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Supplier WHERE Id_Supplier = @Id";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Id", txtSupplierID.Text);

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Data berhasil dihapus!");
                            ClearForm();
                            LoadDataGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Data tidak bisa dihapus karena Supplier ini memiliki riwayat transaksi masuk.\n\nDetail: " + ex.Message);
                    }
                }
            }
        }
    }
}
