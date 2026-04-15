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
        }

        private void KelolaBarang_Load(object sender, EventArgs e)
        {

        }
    }
}
