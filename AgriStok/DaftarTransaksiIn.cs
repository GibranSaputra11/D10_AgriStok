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
    public partial class DaftarTransaksiIn : Form
    {
        private readonly string connectionString = "Data Source=gibran-laptop;Initial Catalog=GudangPertanianDB;Integrated Security=True";
        public DaftarTransaksiIn()
        {
            InitializeComponent();
        }

        private void DaftarTransaksiIn_Load(object sender, EventArgs e)
        {

        }
    }
}
