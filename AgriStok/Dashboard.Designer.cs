namespace AgriStok
{
    partial class Dashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.btnBarang = new System.Windows.Forms.Button();
            this.btnSupplier = new System.Windows.Forms.Button();
            this.btnTani = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnKoneksiDB = new System.Windows.Forms.Button();
            this.btnStatistik = new System.Windows.Forms.Button();
            this.btnPutusKoneksi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBarang
            // 
            this.btnBarang.Location = new System.Drawing.Point(217, 364);
            this.btnBarang.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBarang.Name = "btnBarang";
            this.btnBarang.Size = new System.Drawing.Size(219, 49);
            this.btnBarang.TabIndex = 0;
            this.btnBarang.Text = "Kelola Barang";
            this.btnBarang.UseVisualStyleBackColor = true;
            this.btnBarang.Click += new System.EventHandler(this.btnBarang_Click);
            // 
            // btnSupplier
            // 
            this.btnSupplier.Location = new System.Drawing.Point(471, 364);
            this.btnSupplier.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSupplier.Name = "btnSupplier";
            this.btnSupplier.Size = new System.Drawing.Size(219, 49);
            this.btnSupplier.TabIndex = 1;
            this.btnSupplier.Text = "Kelola Supplier";
            this.btnSupplier.UseVisualStyleBackColor = true;
            this.btnSupplier.Click += new System.EventHandler(this.btnSupplier_Click);
            // 
            // btnTani
            // 
            this.btnTani.Location = new System.Drawing.Point(733, 364);
            this.btnTani.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTani.Name = "btnTani";
            this.btnTani.Size = new System.Drawing.Size(219, 49);
            this.btnTani.TabIndex = 2;
            this.btnTani.Text = "Kelola Kelompok Tani";
            this.btnTani.UseVisualStyleBackColor = true;
            this.btnTani.Click += new System.EventHandler(this.btnTani_Click);
            // 
            // btnIn
            // 
            this.btnIn.Location = new System.Drawing.Point(355, 452);
            this.btnIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(219, 49);
            this.btnIn.TabIndex = 3;
            this.btnIn.Text = "Transaksi Masuk";
            this.btnIn.UseVisualStyleBackColor = true;
            this.btnIn.Click += new System.EventHandler(this.btnIn_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(601, 452);
            this.btnOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(219, 49);
            this.btnOut.TabIndex = 4;
            this.btnOut.Text = "Transaksi Keluar";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AgriStok.Properties.Resources.AgristockLogo;
            this.pictureBox1.Location = new System.Drawing.Point(343, 75);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(491, 250);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // btnKoneksiDB
            // 
            this.btnKoneksiDB.Location = new System.Drawing.Point(1024, 578);
            this.btnKoneksiDB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnKoneksiDB.Name = "btnKoneksiDB";
            this.btnKoneksiDB.Size = new System.Drawing.Size(165, 23);
            this.btnKoneksiDB.TabIndex = 6;
            this.btnKoneksiDB.Text = "Koneksi Database";
            this.btnKoneksiDB.UseVisualStyleBackColor = true;
            this.btnKoneksiDB.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnStatistik
            // 
            this.btnStatistik.Location = new System.Drawing.Point(471, 526);
            this.btnStatistik.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStatistik.Name = "btnStatistik";
            this.btnStatistik.Size = new System.Drawing.Size(219, 49);
            this.btnStatistik.TabIndex = 7;
            this.btnStatistik.Text = "Statistik Data Transaksi";
            this.btnStatistik.UseVisualStyleBackColor = true;
            this.btnStatistik.Click += new System.EventHandler(this.btnStatistik_Click);
            // 
            // btnPutusKoneksi
            // 
            this.btnPutusKoneksi.Location = new System.Drawing.Point(1024, 547);
            this.btnPutusKoneksi.Margin = new System.Windows.Forms.Padding(4);
            this.btnPutusKoneksi.Name = "btnPutusKoneksi";
            this.btnPutusKoneksi.Size = new System.Drawing.Size(165, 23);
            this.btnPutusKoneksi.TabIndex = 8;
            this.btnPutusKoneksi.Text = "Putus Koneksi Database";
            this.btnPutusKoneksi.UseVisualStyleBackColor = true;
            this.btnPutusKoneksi.Click += new System.EventHandler(this.btnPutusKoneksi_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 617);
            this.Controls.Add(this.btnPutusKoneksi);
            this.Controls.Add(this.btnStatistik);
            this.Controls.Add(this.btnKoneksiDB);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnTani);
            this.Controls.Add(this.btnSupplier);
            this.Controls.Add(this.btnBarang);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBarang;
        private System.Windows.Forms.Button btnSupplier;
        private System.Windows.Forms.Button btnTani;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnKoneksiDB;
        private System.Windows.Forms.Button btnStatistik;
        private System.Windows.Forms.Button btnPutusKoneksi;
    }
}

