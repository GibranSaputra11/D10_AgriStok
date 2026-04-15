using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriStok
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void btnBarang_Click(object sender, EventArgs e)
        {
            KelolaBarang kelolaBarang = new KelolaBarang();
            kelolaBarang.ShowDialog();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            KelolaSupplier kelolaSupplier = new KelolaSupplier();
            kelolaSupplier.ShowDialog();
        }

        private void btnTani_Click(object sender, EventArgs e)
        {
            KelolaKelompokTani kelolaKelompokTani = new KelolaKelompokTani();
            kelolaKelompokTani.ShowDialog();
        }
    }
}
