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
        private DAL dbLogic = new DAL();
        private DataTable dtSupplier = new DataTable();

        public KelolaSupplier()
        {
            InitializeComponent();
        }

        private void KelolaSupplier_Load(object sender, EventArgs e)
        {
            dgvSupplier.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSupplier.MultiSelect = false;
            dgvSupplier.ReadOnly = true;
            dgvSupplier.AllowUserToAddRows = false;
            dgvSupplier.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSupplier.CellClick += dgvSupplier_CellClick;

            txtSupplierID.ReadOnly = true;

            LoadDataGrid();
            ClearForm();
        }

        
        private void ClearForm()
        {
            bindingSourceSupplier.AddNew();
            txtSupplierID.Text = dbLogic.GenerateIdSupplier();
            txtNamaSupplier.Clear();
            txtAlamatSupplier.Clear();
            txtTlpSupplier.Clear();
            txtNamaSupplier.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                dtSupplier = dbLogic.GetSupplier();
                bindingSourceSupplier.DataSource = dtSupplier;
                dgvSupplier.DataSource = bindingSourceSupplier;
                bindingNavigatorSupplier.BindingSource = bindingSourceSupplier;
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void dgvSupplier_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSupplier.Rows[e.RowIndex];
                txtSupplierID.Text = row.Cells["Id_Supplier"].Value?.ToString();
                txtNamaSupplier.Text = row.Cells["Nama_Supplier"].Value?.ToString();
                txtTlpSupplier.Text = row.Cells["NoTlp_Supplier"].Value?.ToString();
                txtAlamatSupplier.Text = row.Cells["Alamat_Supplier"].Value?.ToString();
            }
        }


        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            string nama = txtNamaSupplier.Text.Trim();
            string noTelp = txtTlpSupplier.Text.Trim();
            string alamat = txtAlamatSupplier.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtSupplierID.Text))
            {
                MessageBox.Show("ID Supplier tidak valid atau kosong! Sistem akan membuat ulang ID.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSupplierID.Text = dbLogic.GenerateIdSupplier(); 
                return; 
            }

            if (string.IsNullOrWhiteSpace(txtNamaSupplier.Text) ||
                string.IsNullOrWhiteSpace(txtTlpSupplier.Text) ||
                string.IsNullOrWhiteSpace(txtAlamatSupplier.Text))
            {
                MessageBox.Show("Harap isi semua kolom dengan lengkap!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                dbLogic.InsertSupplier(txtSupplierID.Text, txtNamaSupplier.Text, txtTlpSupplier.Text, txtAlamatSupplier.Text);
                MessageBox.Show("Data Supplier berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        
        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSupplierID.Text)) return;

            if (txtSupplierID.Text == dbLogic.GenerateIdSupplier())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                dbLogic.UpdateSupplier(txtSupplierID.Text, txtNamaSupplier.Text, txtTlpSupplier.Text, txtAlamatSupplier.Text);
                MessageBox.Show("Data Supplier berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSupplierID.Text)) return;

            if (txtSupplierID.Text == dbLogic.GenerateIdSupplier())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Supplier ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    dbLogic.DeleteSupplier(txtSupplierID.Text);
                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Sistem Menolak:\n\n" + ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.BackupSupplierData();
                MessageBox.Show("Backup data Supplier berhasil diamankan!", "Sistem Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Backup: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestCelah_Click(object sender, EventArgs e)
        {
            try
            {
                int result = dbLogic.SimulateSQLInjection(txtNamaSupplier.Text);

                MessageBox.Show(result + " baris supplier berhasil di-hack!", "Simulasi Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Eksekusi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            try
            {
                dbLogic.RestoreSupplierData();
                MessageBox.Show("Data berhasil di-restore ke kondisi awal!", "Recovery Instan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDataGrid(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Reset gagal: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
