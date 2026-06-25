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
using System.IO;


namespace AgriStok
{
    public partial class KelolaBarang : Form
    {
        private DAL dbLogic = new DAL();

        private DataTable dtBarang = new DataTable();

        public KelolaBarang()
        {
            InitializeComponent();
        }

        private void KelolaBarang_Load(object sender, EventArgs e)
        {
            dataGridViewBarang.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewBarang.MultiSelect = false;
            dataGridViewBarang.ReadOnly = true;
            dataGridViewBarang.AllowUserToAddRows = false;
            dataGridViewBarang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtBarangID.ReadOnly = true;

            cmbSatuan.Enabled = false;

            LoadComboBoxKategori();
            LoadDataGrid();

            ClearForm();
        }

        private void LoadComboBoxKategori()
        {
            try
            {
                DataTable dt = dbLogic.GetDropdownKategori();
                cmbKategori.DataSource = dt;
                cmbKategori.DisplayMember = "Nama_Kategori";
                cmbKategori.ValueMember = "Id_Kategori";
                cmbKategori.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Error load Kategori: " + ex.Message); }
        }

        private void LoadComboBoxSatuan()
        {
            try
            {
                string kategoriTerpilih = cmbKategori.SelectedIndex != -1 ? cmbKategori.Text : "";
                DataTable dt = dbLogic.GetDropdownSatuan(kategoriTerpilih);

                cmbSatuan.DataSource = dt;
                cmbSatuan.DisplayMember = "Nama_Satuan";
                cmbSatuan.ValueMember = "Id_Satuan";
                cmbSatuan.SelectedIndex = -1;
            }
            catch (Exception ex) { MessageBox.Show("Gagal memuat satuan: " + ex.Message); }
        }

        private void btnSaveBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text))
            {
                MessageBox.Show("ID Barang tidak valid atau kosong! Sistem akan membuat ulang ID.", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBarangID.Text = dbLogic.GenerateIdBarang();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) || cmbSatuan.SelectedValue == null || cmbKategori.SelectedValue == null)
            {
                MessageBox.Show("Harap isi semua data dengan lengkap!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) || txtNamaBarang.Text.Length < 3)
            {
                MessageBox.Show("Nama tidak boleh terlalu pendek!\nMasukkan minimal 3 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtNamaBarang.Text, @"^[a-zA-Z0-9\s\.,]+$"))
            {
                MessageBox.Show("Nama mengandung simbol yang tidak diizinkan!\nHanya gunakan huruf, angka, spasi, titik (.), atau koma (,).", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string namaBarangInput = txtNamaBarang.Text.ToLower();

            List<string> whitelistPertanian = new List<string>
            {
                "benih", "bibit", "biji", "stek", "cangkok", "okulasi", "kultur jaringan", "spora",

                "padi", "jagung", "kedelai", "kacang hijau", "kacang tanah", "kacang panjang", "gandum",
                "sorgum", "singkong", "ubi", "ketela", "porang", "talas", "garut", "edamame",

                "kubis", "sawi", "bayam", "kangkung", "selada", "pakcoy", "caisim", "seledri",
                "daun bawang", "brokoli", "kembang kol", "kemangi", "kelor", "asparagus", "rebung",

                "cabai", "lombok", "tomat", "bawang merah", "bawang putih", "bawang bombay",
                "kentang", "wortel", "terong", "pare", "labu", "oyong", "gambas", "paprika",
                "buncis", "kecipir", "lobak", "bit",

                "melon", "semangka", "mangga", "jeruk", "apel", "alpukat", "durian", "rambutan",
                "manggis", "salak", "pepaya", "pisang", "nanas", "buah naga", "stroberi", "anggur",
                "kelengkeng", "duku", "belimbing", "jambu", "nangka", "sirsak", "markisa", "delima",

                "kelapa sawit", "kelapa", "karet", "kakao", "kopi", "tebu", "tembakau", "cengkeh",
                "teh", "lada", "pala", "kemiri", "vanili", "kapas", "nilam", "sereh wangi", "aren",

                "jahe", "kunyit", "lengkuas", "laos", "kencur", "temulawak", "kapulaga",
                "ketumbar", "merica", "kayu manis", "sereh", "lidah buaya", "mahkota dewa",

                "jati", "sengon", "mahoni", "gaharu", "albasia", "akasia", "balsa", "jabon", "bambu",
                "pupuk", "urea", "npk", "za", "kcl", "tsp", "sp-36", "sp36", "phonska",
                "mutiara", "nitrat", "kalium", "fosfat", "magnesium", "kalsium", "boron",

                "kompos", "kandang", "poc", "organik", "hayati", "mikoriza", "rhizobium",
                "guano", "humat", "asam amino", "ekoenzim", "bokashi", "trichoderma",

                "pestisida", "insektisida", "herbisida", "fungisida", "nematisida",
                "rodentisida", "bakterisida", "akarisida", "moluskisida", "furadan",
                "roundup", "gramoxone", "antracol", "dithane", "regent", "abamektin",
                "glifosat", "paraquat", "mankozeb", "perekat", "penembus", "perata",

                "cangkul", "sabit", "parang", "arit", "garu", "bajak", "sekop", "gembor",
                "gunting dahan", "gergaji", "golok", "linggis", "dodos", "egrek",

                "traktor", "mesin", "pompa air", "sprayer", "combine harvester",
                "transplanter", "thresher", "cultivator", "rotavator", "genset",
                "chopper", "dryer", "seeder", "mist blower", "fogger",

                "mulsa", "polybag", "paranet", "shading net", "waring", "terpal",
                "tali salaran", "ajir", "bambu", "cocopeat", "sekam", "rockwool",
                "hidroponik", "netpot", "ab mix", "selang drip", "emiter", "ph meter",
                "tds meter", "hygrometer", "thermometer",

                "drone", "sensor", "iot", "mikrokontroler", "aktuator", "smart pad",
                "rfid", "barcode", "gps", "cuaca", "telemetri", "otomatisasi"
            };

            bool isSesuaiWhitelist = whitelistPertanian.Any(keyword => namaBarangInput.Contains(keyword));

            if (!isSesuaiWhitelist)
            {
                MessageBox.Show("Barang ditolak!\nSistem mendeteksi barang ini tidak termasuk dalam kategori atau inventaris pertanian.\n\nPastikan nama barang mengandung kata kunci pertanian yang valid (Contoh: 'Pupuk', 'Benih', 'Traktor', dll).",
                                "Di Luar Konteks", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] imgBytes = ConvertImageToBytes(pbFoto);
                dbLogic.InsertBarang(txtBarangID.Text, txtNamaBarang.Text, cmbSatuan.SelectedValue.ToString(), cmbKategori.SelectedValue.ToString(), imgBytes);
                MessageBox.Show("Data Barang berhasil ditambahkan!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ClearForm()
        {
            txtBarangID.Text = dbLogic.GenerateIdBarang();
            txtNamaBarang.Clear();
            cmbKategori.SelectedIndex = -1;
            cmbSatuan.SelectedIndex = -1;
            dataGridViewBarang.ClearSelection();
            pbFoto.Image = null;
            txtNamaBarang.Focus();
        }

        private void LoadDataGrid()
        {
            try
            {
                dtBarang = dbLogic.GetKelolaBarang();
                bindingSourceBarang.DataSource = dtBarang;
                dataGridViewBarang.DataSource = bindingSourceBarang;
                bindingNavigatorBarang.BindingSource = bindingSourceBarang;

                if (dataGridViewBarang.Columns.Contains("Id_Kategori"))
                    dataGridViewBarang.Columns["Id_Kategori"].Visible = false;
                if (dataGridViewBarang.Columns.Contains("Id_Satuan"))
                    dataGridViewBarang.Columns["Id_Satuan"].Visible = false;
            }
            catch (Exception ex) { MessageBox.Show("Gagal Menampilkan Data: " + ex.Message); }
        }

        private void dataGridViewBarang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBarang.Rows[e.RowIndex];
                txtBarangID.Text = row.Cells["Id_Barang"].Value?.ToString();
                txtNamaBarang.Text = row.Cells["Nama_Barang"].Value?.ToString();

                if (row.Cells["Id_Kategori"].Value != null && row.Cells["Id_Kategori"].Value != DBNull.Value)
                    cmbKategori.SelectedValue = row.Cells["Id_Kategori"].Value.ToString();
                else
                    cmbKategori.SelectedIndex = -1;

                if (row.Cells["Id_Satuan"].Value != null && row.Cells["Id_Satuan"].Value != DBNull.Value)
                    cmbSatuan.SelectedValue = row.Cells["Id_Satuan"].Value.ToString();
                else
                    cmbSatuan.SelectedIndex = -1;

                if (row.Cells["Foto"].Value != DBNull.Value && row.Cells["Foto"].Value != null)
                {
                    byte[] imgBytes = (byte[])row.Cells["Foto"].Value;
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        pbFoto.Image = Image.FromStream(ms);
                    }
                }
                else
                {
                    pbFoto.Image = null;
                }
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

       
        private void btnUpdateBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text)) return;

            if (txtBarangID.Text == dbLogic.GenerateIdBarang())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk diubah!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNamaBarang.Text) || txtNamaBarang.Text.Length < 3)
            {
                MessageBox.Show("Nama tidak boleh terlalu pendek!\nMasukkan minimal 3 karakter.", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Regex.IsMatch(txtNamaBarang.Text, @"^[a-zA-Z0-9\s\.,]+$"))
            {
                MessageBox.Show("Nama mengandung simbol yang tidak diizinkan!\nHanya gunakan huruf, angka, spasi, titik (.), atau koma (,).", "Validasi Gagal", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string namaBarangInput = txtNamaBarang.Text.ToLower();

            List<string> whitelistPertanian = new List<string>
            {
                "benih", "bibit", "biji", "stek", "cangkok", "okulasi", "kultur jaringan", "spora",

                "padi", "jagung", "kedelai", "kacang hijau", "kacang tanah", "kacang panjang", "gandum",
                "sorgum", "singkong", "ubi", "ketela", "porang", "talas", "garut", "edamame",

                "kubis", "sawi", "bayam", "kangkung", "selada", "pakcoy", "caisim", "seledri",
                "daun bawang", "brokoli", "kembang kol", "kemangi", "kelor", "asparagus", "rebung",

                "cabai", "lombok", "tomat", "bawang merah", "bawang putih", "bawang bombay",
                "kentang", "wortel", "terong", "pare", "labu", "oyong", "gambas", "paprika",
                "buncis", "kecipir", "lobak", "bit",

                "melon", "semangka", "mangga", "jeruk", "apel", "alpukat", "durian", "rambutan",
                "manggis", "salak", "pepaya", "pisang", "nanas", "buah naga", "stroberi", "anggur",
                "kelengkeng", "duku", "belimbing", "jambu", "nangka", "sirsak", "markisa", "delima",

                "kelapa sawit", "kelapa", "karet", "kakao", "kopi", "tebu", "tembakau", "cengkeh",
                "teh", "lada", "pala", "kemiri", "vanili", "kapas", "nilam", "sereh wangi", "aren",

                "jahe", "kunyit", "lengkuas", "laos", "kencur", "temulawak", "kapulaga",
                "ketumbar", "merica", "kayu manis", "sereh", "lidah buaya", "mahkota dewa",

                "jati", "sengon", "mahoni", "gaharu", "albasia", "akasia", "balsa", "jabon", "bambu",
                "pupuk", "urea", "npk", "za", "kcl", "tsp", "sp-36", "sp36", "phonska",
                "mutiara", "nitrat", "kalium", "fosfat", "magnesium", "kalsium", "boron",

                "kompos", "kandang", "poc", "organik", "hayati", "mikoriza", "rhizobium",
                "guano", "humat", "asam amino", "ekoenzim", "bokashi", "trichoderma",

                "pestisida", "insektisida", "herbisida", "fungisida", "nematisida",
                "rodentisida", "bakterisida", "akarisida", "moluskisida", "furadan",
                "roundup", "gramoxone", "antracol", "dithane", "regent", "abamektin",
                "glifosat", "paraquat", "mankozeb", "perekat", "penembus", "perata",

                "cangkul", "sabit", "parang", "arit", "garu", "bajak", "sekop", "gembor",
                "gunting dahan", "gergaji", "golok", "linggis", "dodos", "egrek",

                "traktor", "mesin", "pompa air", "sprayer", "combine harvester",
                "transplanter", "thresher", "cultivator", "rotavator", "genset",
                "chopper", "dryer", "seeder", "mist blower", "fogger",

                "mulsa", "polybag", "paranet", "shading net", "waring", "terpal",
                "tali salaran", "ajir", "bambu", "cocopeat", "sekam", "rockwool",
                "hidroponik", "netpot", "ab mix", "selang drip", "emiter", "ph meter",
                "tds meter", "hygrometer", "thermometer",

                "drone", "sensor", "iot", "mikrokontroler", "aktuator", "smart pad",
                "rfid", "barcode", "gps", "cuaca", "telemetri", "otomatisasi"
            };

            bool isSesuaiWhitelist = whitelistPertanian.Any(keyword => namaBarangInput.Contains(keyword));

            if (!isSesuaiWhitelist)
            {
                MessageBox.Show("Barang ditolak!\nSistem mendeteksi barang ini tidak termasuk dalam kategori atau inventaris pertanian.\n\nPastikan nama barang mengandung kata kunci pertanian yang valid (Contoh: 'Pupuk', 'Benih', 'Traktor', dll).",
                                "Di Luar Konteks", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] imgBytes = ConvertImageToBytes(pbFoto);
                dbLogic.UpdateBarang(txtBarangID.Text, txtNamaBarang.Text, cmbSatuan.SelectedValue.ToString(), cmbKategori.SelectedValue.ToString(), imgBytes);
                MessageBox.Show("Data Barang berhasil diupdate!");
                LoadDataGrid();
                ClearForm();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void btnDeleteBarang_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBarangID.Text)) return;

            if (txtBarangID.Text == dbLogic.GenerateIdBarang())
            {
                MessageBox.Show("Pilih data yang sudah ada di tabel terlebih dahulu untuk dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirm = MessageBox.Show("Yakin ingin menghapus Barang ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    dbLogic.DeleteBarang(txtBarangID.Text);
                    MessageBox.Show("Data berhasil dihapus!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataGrid();
                    ClearForm();
                }
                catch (Exception ex) { MessageBox.Show("Sistem Menolak:\n\n" + ex.Message, "Peringatan Stok", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void btnAddSatuan_Click(object sender, EventArgs e)
        {
            AddSatuan addSatuan = new AddSatuan();
            addSatuan.ShowDialog();
            LoadComboBoxSatuan();
        }

        private void addKategori_Click(object sender, EventArgs e)
        {
            AddKategori addKategori = new AddKategori();
            addKategori.ShowDialog();
            LoadComboBoxSatuan();
        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKategori.SelectedIndex != -1)
            {
                LoadComboBoxSatuan();
                cmbSatuan.Enabled = true;
            }
            else
            {
                cmbSatuan.DataSource = null;
                cmbSatuan.Enabled = false;
            }
        }

        private void btnUploadFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pbFoto.Image = Image.FromFile(ofd.FileName);
            }
        }

        private byte[] ConvertImageToBytes(PictureBox pb)
        {
            if (pb.Image == null) return null;

            using (MemoryStream ms = new MemoryStream())
            {
                pb.Image.Save(ms, pb.Image.RawFormat);
                return ms.ToArray();
            }
        }
    }
}
