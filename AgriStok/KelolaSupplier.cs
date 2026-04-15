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

        }
    }
}
