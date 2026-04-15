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
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public KelolaBarang()
        {
            InitializeComponent();
            LoadComboBoxKategori();
        }

        private void KelolaBarang_Load(object sender, EventArgs e)
        {

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

                    cmbSatuan.DataSource = dt;
                    cmbSatuan.DisplayMember = "Nama_Kategori";
                    cmbSatuan.ValueMember = "Id_Kategori";

                    cmbSatuan.SelectedIndex = -1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error load Kategori: " + ex.Message);
                }
            }
        }
    }
}
