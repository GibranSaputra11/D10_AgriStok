namespace AgriStok
{
    partial class StatistikaTransakiTahunan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dtpTahun = new System.Windows.Forms.DateTimePicker();
            this.btnLoad = new System.Windows.Forms.Button();
            this.chartStatistik = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartStatistik)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpTahun
            // 
            this.dtpTahun.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTahun.Location = new System.Drawing.Point(32, 43);
            this.dtpTahun.Name = "dtpTahun";
            this.dtpTahun.Size = new System.Drawing.Size(84, 20);
            this.dtpTahun.TabIndex = 0;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(122, 42);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(146, 23);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Tampilkan Grafik";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // chartStatistik
            // 
            chartArea2.Name = "ChartArea1";
            this.chartStatistik.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartStatistik.Legends.Add(legend2);
            this.chartStatistik.Location = new System.Drawing.Point(27, 87);
            this.chartStatistik.Name = "chartStatistik";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartStatistik.Series.Add(series2);
            this.chartStatistik.Size = new System.Drawing.Size(744, 326);
            this.chartStatistik.TabIndex = 2;
            this.chartStatistik.Text = "Statistik Transaski Tahunan";
            // 
            // StatistikaTransakiTahunan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chartStatistik);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.dtpTahun);
            this.Name = "StatistikaTransakiTahunan";
            this.Text = "StatistikaTransakiTahunan";
            this.Load += new System.EventHandler(this.StatistikaTransakiTahunan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartStatistik)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpTahun;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartStatistik;
    }
}