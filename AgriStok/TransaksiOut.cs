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
    public partial class TransaksiOut : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        private bool isEditMode = false;
        private int indexKeranjangEdit = -1;
        private List<string> listBarangDihapus = new List<string>();

        public TransaksiOut()
        {
            InitializeComponent();
            dtpTanggal.MinDate = DateTime.Parse("2000-01-01");
            dtpTanggal.MaxDate = DateTime.Now;

            SetupDataGridView();
            LoadKelompokTani();
            LoadBarang();
        }

        private void TransaksiOut_Load(object sender, EventArgs e)
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
            dgvKeranjang.Columns.Add("Jumlah", "Jumlah Keluar");
            dgvKeranjang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvKeranjang.AllowUserToAddRows = false;
            dgvKeranjang.AllowUserToDeleteRows = false;
            dgvKeranjang.ReadOnly = true;
            dgvKeranjang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadKelompokTani()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Kelompok, Nama_Kelompok FROM KelompokTani", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbKelompok.DataSource = dt;
                cmbKelompok.DisplayMember = "Nama_Kelompok";
                cmbKelompok.ValueMember = "Id_Kelompok";
                cmbKelompok.SelectedIndex = -1;
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
                string query = "SELECT TOP 1 Id_Out FROM Transaksi_Out ORDER BY Id_Out DESC";
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
                        catch (Exception ex) { MessageBox.Show("Gagal memuat stok: " + ex.Message); }
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
            if (cmbBarang.SelectedValue == null || string.IsNullOrEmpty(txtStokSekarang.Text)) return;

            string idBarang = cmbBarang.SelectedValue.ToString();
            string namaBarang = cmbBarang.Text;
            int jumlah = (int)numJumlah.Value;
            int stokTersedia = int.Parse(txtStokSekarang.Text);

            int totalDiminta = jumlah;
            for (int i = 0; i < dgvKeranjang.Rows.Count; i++)
            {
                if (i != indexKeranjangEdit && dgvKeranjang.Rows[i].Cells["Id_Barang"].Value.ToString() == idBarang)
                {
                    totalDiminta += (int)dgvKeranjang.Rows[i].Cells["Jumlah"].Value;
                }
            }

            if (totalDiminta > stokTersedia)
            {
                MessageBox.Show($"Stok tidak mencukupi! Sisa stok {namaBarang} di gudang hanya {stokTersedia}.", "Peringatan Stok", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                if (btnHapus != null) btnHapus.Enabled = false;
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

            if (btnHapus != null) btnHapus.Enabled = false;
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

        private void dgvKeranjang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKeranjang.Rows[e.RowIndex];
                cmbBarang.SelectedValue = row.Cells["Id_Barang"].Value.ToString();
                numJumlah.Value = Convert.ToDecimal(row.Cells["Jumlah"].Value);

                indexKeranjangEdit = e.RowIndex;
                btnTambah.Text = "Ubah Item";

                if (btnHapus != null) btnHapus.Enabled = true;
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            if (cmbKelompok.SelectedValue == null || dgvKeranjang.Rows.Count == 0)
            {
                MessageBox.Show("Lengkapi data Kelompok Tani dan pastikan keranjang tidak kosong!");
                return;
            }

            if (!isEditMode)
            {
                string idKembar = CekTransaksiKembar(cmbKelompok.SelectedValue.ToString(), dtpTanggal.Value);

                if (!string.IsNullOrEmpty(idKembar))
                {
                    DialogResult dialog = MessageBox.Show($"Gagal! Transaksi untuk Kelompok Tani ini pada tanggal tersebut sudah dicatat dengan Nomor {idKembar}.\n\nApakah Anda ingin mengedit (Update) transaksi tersebut?", "Duplikasi Ditemukan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                        SqlCommand cmdMaster = new SqlCommand("sp_UpdateTransaksiOut", conn, transaction);
                        cmdMaster.CommandType = CommandType.StoredProcedure;
                        cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdMaster.Parameters.AddWithValue("@IdKelompok", cmbKelompok.SelectedValue.ToString());
                        cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value.Date);
                        cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                        cmdMaster.ExecuteNonQuery();

                        foreach (string idHapus in listBarangDihapus)
                        {
                            SqlCommand cmdHapus = new SqlCommand("sp_DeleteDetailOutItem", conn, transaction);
                            cmdHapus.CommandType = CommandType.StoredProcedure;
                            cmdHapus.Parameters.AddWithValue("@IdOut", txtIDTransaksi.Text);
                            cmdHapus.Parameters.AddWithValue("@IdBarang", idHapus);
                            cmdHapus.ExecuteNonQuery();
                        }

                        foreach (DataGridViewRow row in dgvKeranjang.Rows)
                        {
                            string idBarang = row.Cells["Id_Barang"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Jumlah"].Value);

                            SqlCommand cmdDetail = new SqlCommand("sp_UpdateDetailOut", conn, transaction);
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.Parameters.AddWithValue("@IdOut", txtIDTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@IdBarang", idBarang);
                            cmdDetail.Parameters.AddWithValue("@QtyBaru", qty);
                            cmdDetail.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Transaksi Barang Keluar Berhasil Di-Update!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        SqlCommand cmdMaster = new SqlCommand("sp_InsertTransaksiOut", conn, transaction);
                        cmdMaster.CommandType = CommandType.StoredProcedure;
                        cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdMaster.Parameters.AddWithValue("@IdKelompok", cmbKelompok.SelectedValue.ToString());
                        cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value.Date);
                        cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                        cmdMaster.ExecuteNonQuery();

                        foreach (DataGridViewRow row in dgvKeranjang.Rows)
                        {
                            string idBarang = row.Cells["Id_Barang"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Jumlah"].Value);

                            SqlCommand cmdDetail = new SqlCommand("sp_InsertDetailOut", conn, transaction);
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.Parameters.AddWithValue("@IdOut", txtIDTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@IdBarang", idBarang);
                            cmdDetail.Parameters.AddWithValue("@Qty", qty);
                            cmdDetail.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Transaksi Barang Keluar Berhasil Disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    isEditMode = false;
                    btnSimpan.Text = "Simpan";
                    btnSimpan.BackColor = SystemColors.Control;
                    listBarangDihapus.Clear();

                    dgvKeranjang.Rows.Clear();
                    txtIDTransaksi.Text = GenerateID();
                    lblTotal.Text = "0";
                    cmbKelompok.SelectedIndex = -1;
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

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            if (cmbKelompok.SelectedValue == null || dgvKeranjang.Rows.Count == 0)
            {
                MessageBox.Show("Lengkapi data Kelompok Tani dan pastikan keranjang tidak kosong!");
                return;
            }

            if (!isEditMode)
            {
                string idKembar = CekTransaksiKembar(cmbKelompok.SelectedValue.ToString(), dtpTanggal.Value);

                if (!string.IsNullOrEmpty(idKembar))
                {
                    DialogResult dialog = MessageBox.Show($"Gagal! Transaksi untuk Kelompok Tani ini pada tanggal tersebut sudah dicatat dengan Nomor {idKembar}.\n\nApakah Anda ingin mengedit (Update) transaksi tersebut?", "Duplikasi Ditemukan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                        SqlCommand cmdMaster = new SqlCommand("sp_UpdateTransaksiOut", conn, transaction);
                        cmdMaster.CommandType = CommandType.StoredProcedure;
                        cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdMaster.Parameters.AddWithValue("@IdKelompok", cmbKelompok.SelectedValue.ToString());
                        cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value.Date);
                        cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                        cmdMaster.ExecuteNonQuery();

                        foreach (string idHapus in listBarangDihapus)
                        {
                            SqlCommand cmdHapus = new SqlCommand("sp_DeleteDetailOutItem", conn, transaction);
                            cmdHapus.CommandType = CommandType.StoredProcedure;
                            cmdHapus.Parameters.AddWithValue("@IdOut", txtIDTransaksi.Text);
                            cmdHapus.Parameters.AddWithValue("@IdBarang", idHapus);
                            cmdHapus.ExecuteNonQuery();
                        }

                        foreach (DataGridViewRow row in dgvKeranjang.Rows)
                        {
                            string idBarang = row.Cells["Id_Barang"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Jumlah"].Value);

                            SqlCommand cmdDetail = new SqlCommand("sp_UpdateDetailOut", conn, transaction);
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.Parameters.AddWithValue("@IdOut", txtIDTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@IdBarang", idBarang);
                            cmdDetail.Parameters.AddWithValue("@QtyBaru", qty);
                            cmdDetail.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Transaksi Barang Keluar Berhasil Di-Update!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        SqlCommand cmdMaster = new SqlCommand("sp_InsertTransaksiOut", conn, transaction);
                        cmdMaster.CommandType = CommandType.StoredProcedure;
                        cmdMaster.Parameters.AddWithValue("@Id", txtIDTransaksi.Text);
                        cmdMaster.Parameters.AddWithValue("@IdKelompok", cmbKelompok.SelectedValue.ToString());
                        cmdMaster.Parameters.AddWithValue("@Tgl", dtpTanggal.Value.Date);
                        cmdMaster.Parameters.AddWithValue("@Total", int.Parse(lblTotal.Text));
                        cmdMaster.ExecuteNonQuery();

                        foreach (DataGridViewRow row in dgvKeranjang.Rows)
                        {
                            string idBarang = row.Cells["Id_Barang"].Value.ToString();
                            int qty = Convert.ToInt32(row.Cells["Jumlah"].Value);

                            SqlCommand cmdDetail = new SqlCommand("sp_InsertDetailOut", conn, transaction);
                            cmdDetail.CommandType = CommandType.StoredProcedure;
                            cmdDetail.Parameters.AddWithValue("@IdOut", txtIDTransaksi.Text);
                            cmdDetail.Parameters.AddWithValue("@IdBarang", idBarang);
                            cmdDetail.Parameters.AddWithValue("@Qty", qty);
                            cmdDetail.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Transaksi Barang Keluar Berhasil Disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    isEditMode = false;
                    btnSimpan.Text = "Simpan";
                    btnSimpan.BackColor = SystemColors.Control;
                    listBarangDihapus.Clear();

                    dgvKeranjang.Rows.Clear();
                    txtIDTransaksi.Text = GenerateID();
                    lblTotal.Text = "0";
                    cmbKelompok.SelectedIndex = -1;
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

        private string CekTransaksiKembar(string idKelompok, DateTime tanggal)
        {
            string idKembar = "";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Id_Out FROM Transaksi_Out WHERE Id_Kelompok = @IdKelompok AND Tgl_Out = @Tgl";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@IdKelompok", idKelompok);
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
                string query = "SELECT d.Id_Barang, b.Nama_Barang, d.Subtotal_Out FROM Detail_Out d INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang WHERE d.Id_Out = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idTransaksiLama);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string idBarang = reader["Id_Barang"].ToString();
                    string namaBarang = reader["Nama_Barang"].ToString();
                    int qtyLama = Convert.ToInt32(reader["Subtotal_Out"]);
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

        public void BukaUntukEdit(string idTransaksiLama)
        {
            isEditMode = true;
            txtIDTransaksi.Text = idTransaksiLama;
            btnSimpan.Text = "Update Transaksi";
            btnSimpan.BackColor = Color.Orange;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryMaster = "SELECT Id_Kelompok, Tgl_Out FROM Transaksi_Out WHERE Id_Out = @Id";
                SqlCommand cmdMaster = new SqlCommand(queryMaster, conn);
                cmdMaster.Parameters.AddWithValue("@Id", idTransaksiLama);

                using (SqlDataReader reader = cmdMaster.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cmbKelompok.SelectedValue = reader["Id_Kelompok"].ToString();
                        dtpTanggal.Value = Convert.ToDateTime(reader["Tgl_Out"]);
                    }
                }
            }

            dgvKeranjang.Rows.Clear();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string queryDetail = "SELECT d.Id_Barang, b.Nama_Barang, d.Subtotal_Out FROM Detail_Out d INNER JOIN Barang b ON d.Id_Barang = b.Id_Barang WHERE d.Id_Out = @Id";
                SqlCommand cmdDetail = new SqlCommand(queryDetail, conn);
                cmdDetail.Parameters.AddWithValue("@Id", idTransaksiLama);

                using (SqlDataReader reader = cmdDetail.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dgvKeranjang.Rows.Add(reader["Id_Barang"].ToString(), reader["Nama_Barang"].ToString(), Convert.ToInt32(reader["Subtotal_Out"]));
                    }
                }
            }
            HitungTotal();
        }
    }
}

