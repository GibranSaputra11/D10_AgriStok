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
    public partial class KelolaSupplier : Form
    {
        private SqlConnection conn;
        private string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";

        public KelolaSupplier()
        {
            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void KelolaSupplier_Load(object sender, EventArgs e)
        {
            txtSupplierID.ReadOnly = true;
        }

        private string GenerateID()
        {
            string newID = "SP-001";

            using (SqlConnection localConn = new SqlConnection(connectionString))
            {
                try
                {
                    localConn.Open();
                    string query = "SELECT TOP 1 Id_Supplier FROM Supplier ORDER BY Id_Supplier DESC";
                    SqlCommand cmd = new SqlCommand(query, localConn);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastID = result.ToString(); 
                        int number = int.Parse(lastID.Split('-')[1]);
                        number++;
                        newID = "SP-" + number.ToString("D3"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal generate ID Supplier: " + ex.Message);
                }
            }
            return newID;
        }
    }
}
