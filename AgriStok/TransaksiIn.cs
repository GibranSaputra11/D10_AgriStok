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

        private bool isEditMode = false;
        private int indexKeranjangEdit = -1;

        private List<string> listBarangDihapus = new List<string>();

        public TransaksiIn()
        {
            InitializeComponent();
            dtpTanggal.MinDate = DateTime.Parse("2000-01-01");
            dtpTanggal.MaxDate = DateTime.Now;

            SetupDataGridView();
            LoadSupplier();
            LoadBarang();
        }

        private void TransaksiIn_Load(object sender, EventArgs e)
        {

            if (!isEditMode)
            {
                txtIDTransaksi.Text = GenerateID();
            }

            txtIDTransaksi.ReadOnly = true;
            txtStokSekarang.ReadOnly = true;
            lblTotal.ReadOnly = true;
            numJumlah.Minimum = 1;

            dgvKeranjang.CellClick += dgvKeranjang_CellClick;
        }

        private void SetupDataGridView()
        {
            dgvKeranjang.Columns.Clear();
            dgvKeranjang.Columns.Add("Id_Barang", "ID Barang");
            dgvKeranjang.Columns.Add("Nama_Barang", "Nama Barang");
            dgvKeranjang.Columns.Add("Jumlah", "Jumlah Masuk");
            dgvKeranjang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvKeranjang.AllowUserToAddRows = false;
            dgvKeranjang.AllowUserToDeleteRows = false; 
            dgvKeranjang.ReadOnly = true; 
            dgvKeranjang.SelectionMode = DataGridViewSelectionMode.FullRowSelect; 
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

            if (indexKeranjangEdit >= 0)
            {
                bool adaYangSama = false;

                for (int i = 0; i < dgvKeranjang.Rows.Count; i++)
                {
                    if (i != indexKeranjangEdit && dgvKeranjang.Rows[i].Cells["Id_Barang"].Value.ToString() == idBarang)
                    {
                        dgvKeranjang.Rows[i].Cells["Jumlah"].Value = (int)dgvKeranjang.Rows[i].Cells["Jumlah"].Value + jumlah;
                        dgvKeranjang.Rows.RemoveAt(indexKeranjangEdit);
                        adaYangSama = true;
                        break;
                    }
                }

                if (!adaYangSama)
                {
                    dgvKeranjang.Rows[indexKeranjangEdit].Cells["Id_Barang"].Value = idBarang;
                    dgvKeranjang.Rows[indexKeranjangEdit].Cells["Nama_Barang"].Value = namaBarang;
                    dgvKeranjang.Rows[indexKeranjangEdit].Cells["Jumlah"].Value = jumlah;
                }

                indexKeranjangEdit = -1;
                btnTambah.Text = "Tambah";
                btnHapus.Enabled = false;
                cmbBarang.SelectedIndex = -1;
                numJumlah.Value = 1;
                txtStokSekarang.Clear();

                HitungTotal();
                return; 
            }

            foreach (DataGridViewRow row in dgvKeranjang.Rows)
            {
                if (row.Cells["Id_Barang"].Value.ToString() == idBarang)
                {
                    row.Cells["Jumlah"].Value = (int)row.Cells["Jumlah"].Value + jumlah;
                    HitungTotal();

                    cmbBarang.SelectedIndex = -1;
                    numJumlah.Value = 1;
                    txtStokSekarang.Clear();
                    return;
                }
            }

            dgvKeranjang.Rows.Add(idBarang, namaBarang, jumlah);
            HitungTotal();

            btnHapus.Enabled = false;
            cmbBarang.SelectedIndex = -1;
            numJumlah.Value = 1;
            txtStokSekarang.Clear();
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

            if (!isEditMode) 
            {
                string idKembar = CekTransaksiKembar(cmbSupplier.SelectedValue.ToString(), dtpTanggal.Value);

                if (!string.IsNullOrEmpty(idKembar))
                {
                    DialogResult dialog = MessageBox.Show($"Gagal! Transaksi untuk Supplier ini pada tanggal tersebut sudah dicatat dengan Nomor {idKembar}.\n\nApakah Anda ingin mengedit (Update) transaksi tersebut?", "Duplikasi Ditemukan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (dialog == DialogResult.Yes)
                    {
                        MasukModeUpdate(idKembar); 
                        return;
                    }
                    else
                    {
                        return; 
                    }
                }
            }

            HitungTotal();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    if (isEditMode)
                    {
                        
                        SqlCommand cmdMaster = new SqlCommand("sp_UpdateTransaksiIn", conn, transaction);
                        cmdMaster.CommandType = CommandType.StoredProcedure;
                        cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdMaster.Parameters.AddWithValue("@IdSupplier", cmbSupplier.SelectedValue.ToString());
                        cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value.Date);
                        cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                        cmdMaster.ExecuteNonQuery();

                        foreach (string idHapus in listBarangDihapus)
                        {
                            SqlCommand cmdHapus = new SqlCommand("sp_DeleteDetailInItem", conn, transaction);
                            cmdHapus.CommandType = CommandType.StoredProcedure;
                            cmdHapus.Parameters.AddWithValue("@IdIn", txtIDTransaksi.Text);
                            cmdHapus.Parameters.AddWithValue("@IdBarang", idHapus);
                            cmdHapus.ExecuteNonQuery();
                        }

                        foreach (DataGridViewRow row in dgvKeranjang.Rows)
                        {
                            string idBarang = row.Cells["Id_Barang"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Jumlah"].Value);

                            SqlCommand cmdDetail = new SqlCommand("sp_UpdateDetailIn", conn, transaction);
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.Parameters.AddWithValue("@IdIn", txtIDTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@IdBarang", idBarang);
                            cmdDetail.Parameters.AddWithValue("@QtyBaru", qty);
                            cmdDetail.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Transaksi Barang Masuk Berhasil Di-Update!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        SqlCommand cmdMaster = new SqlCommand("sp_InsertTransaksiIn", conn, transaction);
                        cmdMaster.CommandType = CommandType.StoredProcedure;
                        cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdMaster.Parameters.AddWithValue("@IdSupplier", cmbSupplier.SelectedValue.ToString());
                        cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value.Date);
                        cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                        cmdMaster.ExecuteNonQuery();

                        foreach (DataGridViewRow row in dgvKeranjang.Rows)
                        {
                            string idBarang = row.Cells["Id_Barang"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Jumlah"].Value);

                            SqlCommand cmdDetail = new SqlCommand("sp_InsertDetailIn", conn, transaction);
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.Parameters.AddWithValue("@IdIn", txtIDTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@IdBarang", idBarang);
                            cmdDetail.Parameters.AddWithValue("@Qty", qty);
                            cmdDetail.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Transaksi Barang Masuk Berhasil Disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    isEditMode = false;
                    btnSimpan.Text = "Simpan";
                    btnSimpan.BackColor = SystemColors.Control;
                    listBarangDihapus.Clear(); 

                    dgvKeranjang.Rows.Clear();
                    txtIDTransaksi.Text = GenerateID();
                    lblTotal.Text = "0";
                    cmbSupplier.SelectedIndex = -1;
                    cmbBarang.SelectedIndex = -1;
                    txtStokSekarang.Clear();
                    numJumlah.Value = 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Transaksi Gagal dibatalkan sistem: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
       
        private string CekTransaksiKembar(string idSupplier, DateTime tanggal)
        {
            string idKembar = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id_In FROM Transaksi_In WHERE Id_Supplier = @IdSupplier AND Tgl_In = @Tgl";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdSupplier", idSupplier);
                    cmd.Parameters.AddWithValue("@Tgl", tanggal.Date);

                    object result = cmd.ExecuteScalar();
                    if (result != null) idKembar = result.ToString();
                }
                catch (Exception ex) { MessageBox.Show("Error Radar: " + ex.Message); }
            }
            return idKembar;
        }

       
        private void MasukModeUpdate(string idTransaksiLama)
        {
            isEditMode = true;
            txtIDTransaksi.Text = idTransaksiLama;
            btnSimpan.Text = "Update Transaksi";
            btnSimpan.BackColor = Color.Orange;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT d.Id_Barang, b.Nama_Barang, d.Subtotal_In FROM Detail_In d INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang WHERE d.Id_In = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idTransaksiLama);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string idBarang = reader["Id_Barang"].ToString();
                    string namaBarang = reader["Nama_Barang"].ToString();
                    int qtyLama = Convert.ToInt32(reader["Subtotal_In"]);
                    bool sudahAdaDiLayar = false;

                    foreach (DataGridViewRow row in dgvKeranjang.Rows)
                    {
                        if (row.Cells["Id_Barang"].Value.ToString() == idBarang)
                        {
                            row.Cells["Jumlah"].Value = (int)row.Cells["Jumlah"].Value + qtyLama;
                            sudahAdaDiLayar = true;
                            break;
                        }
                    }

                    if (!sudahAdaDiLayar)
                    {
                        dgvKeranjang.Rows.Add(idBarang, namaBarang, qtyLama);
                    }
                }
            }
            HitungTotal();
            MessageBox.Show("Sistem mendeteksi transaksi yang sama!\n\nForm dialihkan ke [Mode Update]. Data lama telah DIGABUNGKAN dengan data baru Anda. Silakan edit jumlah jika perlu, lalu klik Update.", "Auto-Merge Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dgvKeranjang_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                HitungTotal();
            }
        }

        private void dgvKeranjang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKeranjang.Rows[e.RowIndex];
                cmbBarang.SelectedValue = row.Cells["Id_Barang"].Value.ToString();
                numJumlah.Value = Convert.ToDecimal(row.Cells["Jumlah"].Value);

                indexKeranjangEdit = e.RowIndex;
                btnTambah.Text = "Ubah Item";

                btnHapus.Enabled = true;
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (indexKeranjangEdit >= 0)
            {
                string idBarangYgDihapus = dgvKeranjang.Rows[indexKeranjangEdit].Cells["Id_Barang"].Value.ToString();

                if (isEditMode)
                {
                    listBarangDihapus.Add(idBarangYgDihapus);
                }

                dgvKeranjang.Rows.RemoveAt(indexKeranjangEdit);

                indexKeranjangEdit = -1;
                btnTambah.Text = "Tambah";
                btnHapus.Enabled = false;
                cmbBarang.SelectedIndex = -1;
                numJumlah.Value = 1;
                txtStokSekarang.Clear();

                HitungTotal();
            }
        }

        public void BukaUntukEdit(string idTransaksiLama)
        {
            isEditMode = true;
            txtIDTransaksi.Text = idTransaksiLama;
            btnSimpan.Text = "Update Transaksi";
            btnSimpan.BackColor = Color.Orange;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryMaster = "SELECT Id_Supplier, Tgl_In FROM Transaksi_In WHERE Id_In = @Id";
                SqlCommand cmdMaster = new SqlCommand(queryMaster, conn);
                cmdMaster.Parameters.AddWithValue("@Id", idTransaksiLama);

                using (SqlDataReader reader = cmdMaster.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cmbSupplier.SelectedValue = reader["Id_Supplier"].ToString();
                        dtpTanggal.Value = Convert.ToDateTime(reader["Tgl_In"]);
                    }
                }
            }

            dgvKeranjang.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryDetail = "SELECT d.Id_Barang, b.Nama_Barang, d.Subtotal_In FROM Detail_In d INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang WHERE d.Id_In = @Id";
                SqlCommand cmdDetail = new SqlCommand(queryDetail, conn);
                cmdDetail.Parameters.AddWithValue("@Id", idTransaksiLama);

                using (SqlDataReader reader = cmdDetail.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvKeranjang.Rows.Add(reader["Id_Barang"].ToString(), reader["Nama_Barang"].ToString(), Convert.ToInt32(reader["Subtotal_In"]));
                    }
                }
            }
            HitungTotal();
        }

    }
}
