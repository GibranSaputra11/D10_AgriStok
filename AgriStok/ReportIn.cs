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
    public partial class ReportIn : Form
    {
        private DAL dbLogic = new DAL();
        private string idTransaksi;

        public ReportIn(string id)
        {
            InitializeComponent();
            idTransaksi = id;
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtNota = dbLogic.GetNotaTransaksiIn(idTransaksi);

                CrystalReportIn laporanNota = new CrystalReportIn();

                laporanNota.SetDataSource(dtNota);

                crystalReportViewer1.ReportSource = laporanNota;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat Nota: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
