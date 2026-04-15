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
    }
}
