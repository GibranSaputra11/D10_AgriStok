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

        public TransaksiOut()
        {
            InitializeComponent();
        }

        private void TransaksiOut_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadKelompokTani();
            LoadBarang();


        }

        private void SetupDataGridView()
        {
            dgvKeranjang.Columns.Clear();
            dgvKeranjang.Columns.Add("Id_Barang", "ID Barang");
            dgvKeranjang.Columns.Add("Nama_Barang", "Nama Barang");
            dgvKeranjang.Columns.Add("Jumlah", "Jumlah Keluar");
            dgvKeranjang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvKeranjang.AllowUserToAddRows = false;
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
    }
}
