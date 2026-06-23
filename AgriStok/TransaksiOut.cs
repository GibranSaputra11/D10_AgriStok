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
        private DAL dbLogic = new DAL(); 

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
            try
            {
                if (!isEditMode)
                {
                    txtIDTransaksi.Text = dbLogic.GenerateIdTransaksiOut();
                }

                txtIDTransaksi.ReadOnly = true;
                txtStokSekarang.ReadOnly = true;
                lblTotal.ReadOnly = true;
                numJumlah.Minimum = 1;

                dgvKeranjang.CellClick += dgvKeranjang_CellClick;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat memuat form: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            try
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
            catch (Exception ex) { MessageBox.Show("Gagal mengatur tabel: " + ex.Message); }
        }

        private void LoadKelompokTani()
        {
            try
            {
                cmbKelompok.DataSource = dbLogic.GetDropdownKelompokTani();
                cmbKelompok.DisplayMember = "Nama_Kelompok";
                cmbKelompok.ValueMember = "Id_Kelompok";
                cmbKelompok.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data Kelompok Tani: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBarang()
        {
            try
            {
                cmbBarang.DataSource = dbLogic.GetDropdownBarang();
                cmbBarang.DisplayMember = "Nama_Barang";
                cmbBarang.ValueMember = "Id_Barang";
                cmbBarang.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data Barang: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        private void cmbBarang_SelectedIndexChanged(object sender, EventArgs e)  
        {
            if (cmbBarang.SelectedIndex != -1 && cmbBarang.SelectedValue != null)
            {
                if (cmbBarang.SelectedValue is string idBarang)
                {
                    try
                    {
                        int stok = dbLogic.GetStokBarang(idBarang);
                        txtStokSekarang.Text = stok.ToString();
                    }
                    catch (Exception ex) { MessageBox.Show("Gagal memuat stok: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            else
            {
                txtStokSekarang.Clear();
            }
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan saat menambah keranjang: " + ex.Message, "Error Keranjang", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HitungTotal()
        {
            try
            {
                int total = 0;
                foreach (DataGridViewRow row in dgvKeranjang.Rows)
                {
                    total += Convert.ToInt32(row.Cells["Jumlah"].Value);
                }
                lblTotal.Text = total.ToString();
            }
            catch (Exception) { lblTotal.Text = "error"; }
        }

        private void dgvKeranjang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
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
            catch (Exception ex) { MessageBox.Show("Error seleksi keranjang: " + ex.Message); }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            try
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
                    if (btnHapus != null) btnHapus.Enabled = false;
                    cmbBarang.SelectedIndex = -1;
                    numJumlah.Value = 1;
                    txtStokSekarang.Clear();

                    HitungTotal();
                }
            }
            catch (Exception ex) { MessageBox.Show("Gagal menghapus item keranjang: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbKelompok.SelectedValue == null || dgvKeranjang.Rows.Count == 0)
                {
                    MessageBox.Show("Lengkapi data Kelompok Tani dan pastikan keranjang tidak kosong!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!isEditMode)
                {
                    string idKembar = dbLogic.CekTransaksiKembarOut(cmbKelompok.SelectedValue.ToString(), dtpTanggal.Value);

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

                DataTable dtKeranjang = new DataTable();
                dtKeranjang.Columns.Add("Id_Barang", typeof(string));
                dtKeranjang.Columns.Add("Qty", typeof(int));

                foreach (DataGridViewRow row in dgvKeranjang.Rows)
                {
                    dtKeranjang.Rows.Add(row.Cells["Id_Barang"].Value.ToString(), Convert.ToInt32(row.Cells["Jumlah"].Value));
                }

                if (isEditMode)
                {
                    dbLogic.UpdateTransaksiOut(txtIDTransaksi.Text, cmbKelompok.SelectedValue.ToString(), dtpTanggal.Value.Date, int.Parse(lblTotal.Text), dtKeranjang, listBarangDihapus);
                    MessageBox.Show("Transaksi Barang Keluar Berhasil Di-Update!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    dbLogic.InsertTransaksiOut(txtIDTransaksi.Text, cmbKelompok.SelectedValue.ToString(), dtpTanggal.Value.Date, int.Parse(lblTotal.Text), dtKeranjang);
                    MessageBox.Show("Transaksi Barang Keluar Berhasil Disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                isEditMode = false;
                btnSimpan.Text = "Simpan";
                btnSimpan.BackColor = SystemColors.Control;
                listBarangDihapus.Clear();

                dgvKeranjang.Rows.Clear();
                txtIDTransaksi.Text = dbLogic.GenerateIdTransaksiOut();
                lblTotal.Text = "0";
                cmbKelompok.SelectedIndex = -1;
                cmbBarang.SelectedIndex = -1;
                txtStokSekarang.Clear();
                numJumlah.Value = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Transaksi Gagal dibatalkan sistem: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void MasukModeUpdate(string idTransaksiLama)
        {
            try
            {
                isEditMode = true;
                txtIDTransaksi.Text = idTransaksiLama;
                btnSimpan.Text = "Update Transaksi";
                btnSimpan.BackColor = Color.Orange;

                DataTable dtMaster = dbLogic.GetMasterTransaksiOut(idTransaksiLama);
                if (dtMaster.Rows.Count > 0)
                {
                    cmbKelompok.SelectedValue = dtMaster.Rows[0]["Id_Kelompok"].ToString();
                    dtpTanggal.Value = Convert.ToDateTime(dtMaster.Rows[0]["Tgl_Out"]);
                }

                DataTable dtDetail = dbLogic.GetDetailTransaksiOut(idTransaksiLama);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    string idBarang = dr["ID Barang"].ToString();
                    string namaBarang = dr["Nama Barang"].ToString();
                    int qtyLama = Convert.ToInt32(dr["Jumlah Keluar"]);
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
                HitungTotal();
                MessageBox.Show("Sistem mendeteksi transaksi yang sama!\n\nForm dialihkan ke [Mode Update]. Data lama telah DIGABUNGKAN dengan data baru Anda. Silakan edit jumlah jika perlu, lalu klik Update.", "Auto-Merge Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Gagal memuat data update: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public void BukaUntukEdit(string idTransaksiLama)
        {
            try
            {
                isEditMode = true;
                txtIDTransaksi.Text = idTransaksiLama;
                btnSimpan.Text = "Update Transaksi";
                btnSimpan.BackColor = Color.Orange;

                DataTable dtMaster = dbLogic.GetMasterTransaksiOut(idTransaksiLama);
                if (dtMaster.Rows.Count > 0)
                {
                    cmbKelompok.SelectedValue = dtMaster.Rows[0]["Id_Kelompok"].ToString();
                    dtpTanggal.Value = Convert.ToDateTime(dtMaster.Rows[0]["Tgl_Out"]);
                }

                dgvKeranjang.Rows.Clear();
                DataTable dtDetail = dbLogic.GetDetailTransaksiOut(idTransaksiLama);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    dgvKeranjang.Rows.Add(dr["ID Barang"].ToString(), dr["Nama Barang"].ToString(), Convert.ToInt32(dr["Jumlah Keluar"]));
                }
                HitungTotal();
            }
            catch (Exception ex) { MessageBox.Show("Gagal memuat form edit: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}

