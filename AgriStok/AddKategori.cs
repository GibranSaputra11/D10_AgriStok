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
    public partial class AddKategori : Form
    {
        private DAL dbLogic = new DAL();
        private DataTable dtKategori = new DataTable();

        public AddKategori()
        {
            InitializeComponent();
        }

        private void AddKategori_Load(object sender, EventArgs e)
        {
            dgvKategori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvKategori.MultiSelect = false;
            dgvKategori.ReadOnly = true;
            dgvKategori.AllowUserToAddRows = false;
            dgvKategori.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKategori.CellClick += dgvKategori_CellClick;

            txtKategoriID.ReadOnly = true;
            LoadDataGrid();
            ClearForm();
        }

        private void ClearForm()
        {
            txtKategoriID.Text = dbLogic.GenerateIdKategori();
            txtNamaKategori.Clear();
            txtNamaKategori.Focus();
        }

        private void dgvKategori_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvKategori.Rows.Count)
            {
                DataGridViewRow row = dgvKategori.Rows[e.RowIndex];
                txtKategoriID.Text = row.Cells["Id_Kategori"].Value.ToString();
                txtNamaKategori.Text = row.Cells["Nama_Kategori"].Value.ToString();
            }
        }

        private void LoadDataGrid()
        {
            try
            {
                dtKategori = dbLogic.GetKategori(); 
                bindingSourceKategori.DataSource = dtKategori;
                dgvKategori.DataSource = bindingSourceKategori;
                bindingNavigatorKategori.BindingSource = bindingSourceKategori;
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string input = txtNamaKategori.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtKategoriID.Text))
            {
                MessageBox.Show("ID Kategori tidak valid atau kosong! Sistem akan membuat ulang ID.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtKategoriID.Text = dbLogic.GenerateIdKategori();
                return;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Nama Kategori tidak boleh kosong!");
                return;
            }

            if (input.Length < 2)
            {
                MessageBox.Show("Input terlalu pendek! Masukkan minimal 2 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
            {
                MessageBox.Show("Input tidak valid! Hanya boleh menggunakan huruf, tanpa simbol (@, #, dll) atau angka.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] kategoriValid =
            {
                "pupuk", "bibit", "benih", "pestisida", "alat", "kemasan", "mesin", "hasil panen",

                "pupuk organik", "pupuk kimia", "pupuk cair", "pupuk daun", "kompos",
    
                "insektisida", "herbisida", "fungisida", "bakterisida",
    
                "benih sayuran", "benih buah", "bibit kultur jaringan",
    
                "alat manual", "suku cadang", "alat irigasi",
    
                "media tanam", "mulsa", "polybag", "zpt"
            };

            string inputKategori = txtNamaKategori.Text.Trim().ToLower();

            if (!kategoriValid.Contains(inputKategori))
            {
                MessageBox.Show("Kategori tidak valid atau tidak dikenali dalam sistem pergudangan pertanian!\n" +
                                "Mohon gunakan kategori standar (contoh: 'pupuk organik', 'benih sayuran', 'alat tani manual').",
                                "Validasi Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbLogic.InsertKategori(txtKategoriID.Text, txtNamaKategori.Text);
                MessageBox.Show("Data Kategori berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKategoriID.Text)) return;

            if (txtKategoriID.Text == dbLogic.GenerateIdKategori())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbLogic.UpdateKategori(txtKategoriID.Text, txtNamaKategori.Text);
                MessageBox.Show("Data Kategori berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show("Gagal update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKategoriID.Text)) return;

            if (txtKategoriID.Text == dbLogic.GenerateIdKategori())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Kategori ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    dbLogic.DeleteKategori(txtKategoriID.Text);
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
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }
    }
}
