using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgriStok
{
    public partial class StatistikaTransakiTahunan : Form
    {
        private DAL dbLogic = new DAL();
        public StatistikaTransakiTahunan()
        {
            InitializeComponent();
            SetupUIFilter();
        }

        private void StatistikaTransakiTahunan_Load(object sender, EventArgs e)
        {
            LoadDataChart();
        }

        private void SetupUIFilter()
        {
            dtpTahun.Format = DateTimePickerFormat.Custom;
            dtpTahun.CustomFormat = "yyyy";
            dtpTahun.ShowUpDown = true;
            dtpTahun.MaxDate = DateTime.Now;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadDataChart();
        }

        private void LoadDataChart()
        {
            int tahunDipilih = dtpTahun.Value.Year;

            chartStatistik.Series.Clear();
            chartStatistik.Titles.Clear();
            chartStatistik.Legends.Clear();
            chartStatistik.ChartAreas.Clear();

            ChartArea ca = new ChartArea("MainArea");
            ca.AxisX.Title = "Bulan";
            ca.AxisY.Title = "Total Barang";
            ca.AxisX.LabelStyle.Angle = -45; 
            ca.AxisX.Interval = 1;
            ca.BackColor = Color.WhiteSmoke;
            chartStatistik.ChartAreas.Add(ca);

            try
            {
                DataTable dt = dbLogic.GetStatistikaTransaksiTahunan(tahunDipilih);

                Series sMasuk = new Series("Transaksi Masuk");
                sMasuk.ChartType = SeriesChartType.Line;
                sMasuk.BorderWidth = 3;
                sMasuk.Color = Color.ForestGreen;
                sMasuk.MarkerStyle = MarkerStyle.Circle; 
                sMasuk.MarkerSize = 7;
                sMasuk.IsValueShownAsLabel = true; 

                Series sKeluar = new Series("Transaksi Keluar");
                sKeluar.ChartType = SeriesChartType.Line;
                sKeluar.BorderWidth = 3;
                sKeluar.Color = Color.Crimson;
                sKeluar.MarkerStyle = MarkerStyle.Square;
                sKeluar.MarkerSize = 7;
                sKeluar.IsValueShownAsLabel = true; 

                foreach (DataRow row in dt.Rows)
                {
                    string bulan = row["Bulan"].ToString();
                    int qtyMasuk = Convert.ToInt32(row["Total_Masuk"]);
                    int qtyKeluar = Convert.ToInt32(row["Total_Keluar"]);

                    sMasuk.Points.AddXY(bulan, qtyMasuk);
                    sKeluar.Points.AddXY(bulan, qtyKeluar);
                }

                chartStatistik.Series.Add(sMasuk);
                chartStatistik.Series.Add(sKeluar);

                Title title = new Title($"Transaksi Gudang Tahun {tahunDipilih}", Docking.Top, new Font("Segoe UI", 16, FontStyle.Bold), Color.Black);
                chartStatistik.Titles.Add(title);

                Legend legend = new Legend("MainLegend");
                legend.Docking = Docking.Bottom;
                legend.Alignment = StringAlignment.Center;
                chartStatistik.Legends.Add(legend);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat grafik statistik: " + ex.Message, "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
