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
    public partial class TransaksiIn : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public TransaksiIn()
        {
            InitializeComponent();
        }

        private void TransaksiIn_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadSupplier();
            LoadBarang();
            txtIDTransaksi.Text = GenerateID();
            txtIDTransaksi.ReadOnly = true;
            txtStokSekarang.ReadOnly = true;
            numJumlah.Minimum = 1;
        }

        private void SetupDataGridView()
        {
            dgvKeranjang.Columns.Clear();
            dgvKeranjang.Columns.Add("Id_Barang", "ID Barang");
            dgvKeranjang.Columns.Add("Nama_Barang", "Nama Barang");
            dgvKeranjang.Columns.Add("Jumlah", "Jumlah Masuk");
            dgvKeranjang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKeranjang.AllowUserToAddRows = false;
        }

        private void LoadSupplier()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Supplier, Nama_Supplier FROM Supplier", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbSupplier.DataSource = dt;
                cmbSupplier.DisplayMember = "Nama_Supplier";
                cmbSupplier.ValueMember = "Id_Supplier";
                cmbSupplier.SelectedIndex = -1;
            }
        }

        private void LoadBarang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Barang, Nama_Barang FROM Barang", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbBarang.DataSource = dt;
                cmbBarang.DisplayMember = "Nama_Barang";
                cmbBarang.ValueMember = "Id_Barang";
                cmbBarang.SelectedIndex = -1;
            }
        }

        private string GenerateID()
        {
            string newID = "TR-001";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 Id_In FROM Transaksi_In ORDER BY Id_In DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                if (result != null)
                {
                    int number = int.Parse(result.ToString().Split('-')[1]);
                    newID = "TR-" + (number + 1).ToString("D3");
                }
            }
            return newID;
        }

        private void cmbBarang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBarang.SelectedIndex != -1 && cmbBarang.SelectedValue != null)
            {
                if (cmbBarang.SelectedValue is string idBarang)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        try
                        {
                            conn.Open();
                            string query = "SELECT Stok_Barang FROM Barang WHERE Id_Barang = @Id";
                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@Id", idBarang);

                            object result = cmd.ExecuteScalar();
                            if (result != null)
                            {
                                txtStokSekarang.Text = result.ToString();
                            }
                            else
                            {
                                txtStokSekarang.Text = "0";
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Gagal memuat stok: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                txtStokSekarang.Clear();
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            if (cmbBarang.SelectedValue == null) return;

            string idBarang = cmbBarang.SelectedValue.ToString();
            string namaBarang = cmbBarang.Text;
            int jumlah = (int)numJumlah.Value;

            foreach (DataGridViewRow row in dgvKeranjang.Rows)
            {
                if (row.Cells["Id_Barang"].Value.ToString() == idBarang)
                {
                    row.Cells["Jumlah"].Value = (int)row.Cells["Jumlah"].Value + jumlah;
                    HitungTotal();
                    return;
                }
            }

            dgvKeranjang.Rows.Add(idBarang, namaBarang, jumlah);
            HitungTotal();
        }

        private void HitungTotal()
        {
            int total = 0;
            foreach (DataGridViewRow row in dgvKeranjang.Rows)
            {
                total += Convert.ToInt32(row.Cells["Jumlah"].Value);
            }
            lblTotal.Text = total.ToString();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cmbSupplier.SelectedValue == null || dgvKeranjang.Rows.Count == 0)
            {
                MessageBox.Show("Lengkapi data supplier dan keranjang!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string queryMaster = "INSERT INTO Transaksi_In (Id_In, Id_Supplier, Tgl_In, Total_Barang_In) VALUES (@Id, @IdSup, @Tgl, @Total)";
                    SqlCommand cmdMaster = new SqlCommand(queryMaster, conn, transaction);
                    cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                    cmdMaster.Parameters.AddWithValue("@IdSup", cmbSupplier.SelectedValue.ToString());
                    cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value);
                    cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                    cmdMaster.ExecuteNonQuery();

                    foreach (DataGridViewRow row in dgvKeranjang.Rows)
                    {
                        string idBarang = row.Cells["Id_Barang"].Value.ToString();
                        int qty = (int)row.Cells["Jumlah"].Value;

                        string queryDetail = "INSERT INTO Detail_In (Id_In, Id_Barang, Subtotal_In) VALUES (@Id, @IdB, @Qty)";
                        SqlCommand cmdDetail = new SqlCommand(queryDetail, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdDetail.Parameters.AddWithValue("@IdB", idBarang);
                        cmdDetail.Parameters.AddWithValue("@Qty", qty);
                        cmdDetail.ExecuteNonQuery();

                        string queryStok = "UPDATE Barang SET Stok_Barang = Stok_Barang + @Qty WHERE Id_Barang = @IdB";
                        SqlCommand cmdStok = new SqlCommand(queryStok, conn, transaction);
                        cmdStok.Parameters.AddWithValue("@Qty", qty);
                        cmdStok.Parameters.AddWithValue("@IdB", idBarang);
                        cmdStok.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Transaksi Berhasil Disimpan!");

                    dgvKeranjang.Rows.Clear();
                    txtIDTransaksi.Text = GenerateID();
                    lblTotal.Text = "0";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Transaksi Gagal: " + ex.Message);
                }
            }
        }
    }
}
