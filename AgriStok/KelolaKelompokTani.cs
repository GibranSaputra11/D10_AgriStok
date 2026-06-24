using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriStok
{
    public partial class KelolaKelompokTani : Form
    {
        private DAL dbLogic = new DAL();
        private DataTable dtKelompok = new DataTable();

        public KelolaKelompokTani()
        {
            InitializeComponent();
        }

        private void KelolaKelompokTani_Load(object sender, EventArgs e)
        {
            dgvKelompokTani.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKelompokTani.MultiSelect = false;
            dgvKelompokTani.ReadOnly = true;
            dgvKelompokTani.AllowUserToAddRows = false;
            dgvKelompokTani.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKelompokTani.CellClick += dgvKelompokTani_CellClick;

            lblNamaFile.Visible = false;
            lblStatusGrid.Visible = false;
            txtKelompokID.ReadOnly = true;
            LoadDataGrid();
            ClearForm();
        }

        private void ClearForm()
        {
            bindingSourceKelompokTani.AddNew();
            txtKelompokID.Text = dbLogic.GenerateIdKelompokTani();
            txtNamaKelompok.Clear();
            txtAlamatKelompok.Clear();
            txtTlpKelompok.Clear();
            txtNamaKelompok.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                dtKelompok = dbLogic.GetKelompokTani();
                bindingSourceKelompokTani.DataSource = dtKelompok;
                dgvKelompokTani.DataSource = bindingSourceKelompokTani;
                bindingNavigatorKelompokTani.BindingSource = bindingSourceKelompokTani;
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void dgvKelompokTani_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvKelompokTani.Rows[e.RowIndex];
                txtKelompokID.Text = row.Cells["Id_Kelompok"].Value?.ToString();
                txtNamaKelompok.Text = row.Cells["Nama_Kelompok"].Value?.ToString();
                txtTlpKelompok.Text = row.Cells["NoTlp_Kelompok"].Value?.ToString();
                txtAlamatKelompok.Text = row.Cells["Alamat_Kelompok"].Value?.ToString();
            }
        }

               
        private void btnAddKelompok_Click(object sender, EventArgs e)
        {
            string nama = txtNamaKelompok.Text.Trim();
            string noTelp = txtTlpKelompok.Text.Trim();
            string alamat = txtAlamatKelompok.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtKelompokID.Text))
            {
                MessageBox.Show("ID Kelompok tidak valid atau kosong! Sistem akan membuat ulang ID.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKelompokID.Text = dbLogic.GenerateIdKelompokTani();
                return;
            }

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
                dbLogic.InsertKelompokTani(txtKelompokID.Text, txtNamaKelompok.Text, txtTlpKelompok.Text, txtAlamatKelompok.Text);
                MessageBox.Show("Data Kelompok Tani berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnUpdateKelompok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKelompokID.Text)) return;

            if (txtKelompokID.Text == dbLogic.GenerateIdKelompokTani())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbLogic.UpdateKelompokTani(txtKelompokID.Text, txtNamaKelompok.Text, txtTlpKelompok.Text, txtAlamatKelompok.Text);
                MessageBox.Show("Data Kelompok Tani berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show("Gagal update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnDeleteKelompok_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKelompokID.Text)) return;

            if (txtKelompokID.Text == dbLogic.GenerateIdKelompokTani())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Kelompok Tani ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    dbLogic.DeleteKelompokTani(txtKelompokID.Text);
                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                    ClearForm();
                }
                catch (Exception ex) { MessageBox.Show("Sistem Menolak:\n\n" + ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnImpExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog { Filter = "Excel Files|*.xls;*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                                {
                                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                                });

                                DataTable dtExcel = result.Tables[0];

                                dgvKelompokTani.DataSource = null;
                                dgvKelompokTani.DataSource = dtExcel;

                                lblStatusGrid.Text = "MODE PREVIEW: Data Excel (Belum Tersimpan ke Database)";
                                lblStatusGrid.ForeColor = System.Drawing.Color.DarkOrange;
                                lblStatusGrid.Visible = true; 

                                lblNamaFile.Text = "File siap: " + Path.GetFileName(openFileDialog.FileName);
                                lblNamaFile.ForeColor = System.Drawing.Color.Green;
                                lblNamaFile.Visible = true;


                                btnImpDb.Enabled = true;     
                                btnAddKelompok.Enabled = false;  
                                btnUpdateKelompok.Enabled = false;
                                btnDeleteKelompok.Enabled = false;
                            }
                        }
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("File Excel sedang terbuka di aplikasi lain! Tolong tutup dahulu.", "Akses Ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal membaca Excel: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnImpDb_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = dgvKelompokTani.DataSource as DataTable;

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diimport. Pilih file Excel terlebih dahulu!");
                    return;
                }

                int sukses = 0;
                int gagal = 0;

                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        string nama = row["Nama_Kelompok"].ToString().Trim();
                        string noTlp = row["NoTlp_Kelompok"].ToString().Trim();
                        string alamat = row["Alamat_Kelompok"].ToString().Trim();

                        if (string.IsNullOrWhiteSpace(nama)) continue;

                        string idBaru = dbLogic.GenerateIdKelompokTani();
                        dbLogic.InsertKelompokTani(idBaru, nama, noTlp, alamat);

                        sukses++;
                    }
                    catch
                    {
                        gagal++; 
                    }
                }

                MessageBox.Show($"Import selesai!\nBerhasil masuk database: {sukses} baris\nGagal/Dilewati: {gagal} baris", "Status Laporan", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblNamaFile.Visible = false; 
                lblStatusGrid.Visible = false;
                btnImpDb.Enabled = false;    
                btnAddKelompok.Enabled = true;   
                btnUpdateKelompok.Enabled = true;
                btnDeleteKelompok.Enabled = true;

                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saat memproses data: " + ex.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
