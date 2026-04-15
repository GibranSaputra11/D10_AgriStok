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
    public partial class KelolaBarang : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public KelolaBarang()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void KelolaBarang_Load(object sender, EventArgs e)
        {
            LoadComboBoxKategori();
            LoadComboBoxSatuan();
        }

        private void LoadComboBoxKategori()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT Id_Kategori, Nama_Kategori FROM Kategori";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbKategori.DataSource = dt;
                    cmbKategori.DisplayMember = "Nama_Kategori";
                    cmbKategori.ValueMember = "Id_Kategori";

                    cmbKategori.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error load Kategori: " + ex.Message);
                }
            }
        }

        private void LoadComboBoxSatuan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT Id_Satuan, Nama_Satuan FROM Satuan";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbSatuan.DataSource = dt;
                    cmbSatuan.DisplayMember = "Nama_Satuan"; 
                    cmbSatuan.ValueMember = "Id_Satuan";     

                    cmbSatuan.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error load Satuan: " + ex.Message);
                }
            }
        }


    }
}
