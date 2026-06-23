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
    public partial class ReportOut : Form
    {
        private DAL dbLogic = new DAL();
        private string idTransaksi;
        public ReportOut(string id)
        {
            InitializeComponent();
            idTransaksi = id;
        }

        private void ReportOut_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dtNota = dbLogic.GetNotaTransaksiOut(idTransaksi);

                CrystalReportOut laporanNota = new CrystalReportOut();

                laporanNota.SetDataSource(dtNota);

                crystalReportViewer1.ReportSource = laporanNota;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat Nota Keluar: " + ex.Message, "Error Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
