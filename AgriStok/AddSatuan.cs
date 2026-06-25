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

    public partial class AddSatuan : Form
    {
        private DAL dbLogic = new DAL();
        private DataTable dtSatuan = new DataTable();

        public AddSatuan()
        {
            InitializeComponent();
        }

        private void AddSatuan_Load(object sender, EventArgs e)
        {
            dgvSatuan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSatuan.MultiSelect = false;
            dgvSatuan.ReadOnly = true;
            dgvSatuan.AllowUserToAddRows = false;
            dgvSatuan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSatuan.CellClick += dgvSatuan_CellClick; 

            txtSatuanID.ReadOnly = true;
            LoadDataGrid();
            ClearForm();
        }

      
        private void ClearForm()
        {
            txtSatuanID.Text = dbLogic.GenerateIdSatuan();
            txtNamaSatuan.Clear();
            txtNamaSatuan.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                dtSatuan = dbLogic.GetSatuan();
                bindingSourceSatuan.DataSource = dtSatuan;
                dgvSatuan.DataSource = bindingSourceSatuan;
                bindingNavigatorSatuan.BindingSource = bindingSourceSatuan;
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void dgvSatuan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSatuan.Rows[e.RowIndex];
                txtSatuanID.Text = row.Cells["Id_Satuan"].Value?.ToString();
                txtNamaSatuan.Text = row.Cells["Nama_Satuan"].Value?.ToString();
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {

            string input = txtNamaSatuan.Text.Trim();

            if (string.IsNullOrWhiteSpace(txtSatuanID.Text))
            {
                MessageBox.Show("ID Satuan tidak valid atau kosong! Sistem akan membuat ulang ID.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSatuanID.Text = dbLogic.GenerateIdSatuan();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaSatuan.Text)) return;

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

            string[] satuanValid =
            {
                "kg", "kilogram", "g", "gram", "kuintal", "kwintal", "ton", "ons", "pon",

                "l", "liter", "ml", "mililiter", "cc", "jerigen", "dirigen", "drum", "galon", "tangki", "vial", "ampul",

                "sak", "karung", "pcs", "pieces", "botol", "pack", "pak", "bungkus", "bks", "sachet", "saset",
                "dus", "kardus", "carton", "karton", "box", "boks", "kaleng", "peti", "palet", "pallet", "bal", "roll", "rol",

                "buah", "biji", "ikat", "tandan", "sisir", "keranjang", "polybag", "polibag", "tray", "lembar"
            };

            string inputSatuan = txtNamaSatuan.Text.Trim().ToLower();

            if (!satuanValid.Contains(inputSatuan))
            {
                MessageBox.Show("Satuan tidak dikenali! Mohon gunakan standar penamaan satuan gudang (contoh: kg, liter, sak, karung, pcs).",
                                "Validasi Gagal",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbLogic.InsertSatuan(txtSatuanID.Text, txtNamaSatuan.Text);
                MessageBox.Show("Data Satuan berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSatuanID.Text)) return;

            if (txtSatuanID.Text == dbLogic.GenerateIdSatuan())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbLogic.UpdateSatuan(txtSatuanID.Text, txtNamaSatuan.Text);
                MessageBox.Show("Data Satuan berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

                      
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSatuanID.Text)) return;

            if (txtSatuanID.Text == dbLogic.GenerateIdSatuan())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Satuan ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    dbLogic.DeleteSatuan(txtSatuanID.Text);
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
