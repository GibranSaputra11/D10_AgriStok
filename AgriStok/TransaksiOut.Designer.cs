namespace AgriStok
{
    partial class TransaksiOut
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
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgvKeranjang = new System.Windows.Forms.DataGridView();
            this.numJumlah = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStokSekarang = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbBarang = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbKelompok = new System.Windows.Forms.ComboBox();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.txtIDTransaksi = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeranjang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numJumlah)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(562, 388);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(149, 24);
            this.btnSimpan.TabIndex = 34;
            this.btnSimpan.Text = "Simpan Transaksi";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(109, 330);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(174, 24);
            this.btnTambah.TabIndex = 33;
            this.btnTambah.Text = "Tambah ke Keranjang";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(464, 388);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(77, 20);
            this.lblTotal.TabIndex = 32;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(354, 391);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 13);
            this.label7.TabIndex = 31;
            this.label7.Text = "Total Semua Barang";
            // 
            // dgvKeranjang
            // 
            this.dgvKeranjang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKeranjang.Location = new System.Drawing.Point(357, 38);
            this.dgvKeranjang.Name = "dgvKeranjang";
            this.dgvKeranjang.Size = new System.Drawing.Size(424, 333);
            this.dgvKeranjang.TabIndex = 30;
            // 
            // numJumlah
            // 
            this.numJumlah.Location = new System.Drawing.Point(109, 286);
            this.numJumlah.Name = "numJumlah";
            this.numJumlah.Size = new System.Drawing.Size(104, 20);
            this.numJumlah.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 290);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Jumlah Keluar";
            // 
            // txtStokSekarang
            // 
            this.txtStokSekarang.Location = new System.Drawing.Point(109, 235);
            this.txtStokSekarang.Name = "txtStokSekarang";
            this.txtStokSekarang.Size = new System.Drawing.Size(104, 20);
            this.txtStokSekarang.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 238);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Stok Saat Ini";
            // 
            // cmbBarang
            // 
            this.cmbBarang.FormattingEnabled = true;
            this.cmbBarang.Location = new System.Drawing.Point(109, 185);
            this.cmbBarang.Name = "cmbBarang";
            this.cmbBarang.Size = new System.Drawing.Size(123, 21);
            this.cmbBarang.TabIndex = 25;
            this.cmbBarang.SelectedIndexChanged += new System.EventHandler(this.cmbBarang_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Pilih Barang";
            // 
            // cmbKelompok
            // 
            this.cmbKelompok.FormattingEnabled = true;
            this.cmbKelompok.Location = new System.Drawing.Point(109, 127);
            this.cmbKelompok.Name = "cmbKelompok";
            this.cmbKelompok.Size = new System.Drawing.Size(199, 21);
            this.cmbKelompok.TabIndex = 23;
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Location = new System.Drawing.Point(109, 83);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(198, 20);
            this.dtpTanggal.TabIndex = 22;
            // 
            // txtIDTransaksi
            // 
            this.txtIDTransaksi.Location = new System.Drawing.Point(109, 44);
            this.txtIDTransaksi.Name = "txtIDTransaksi";
            this.txtIDTransaksi.Size = new System.Drawing.Size(104, 20);
            this.txtIDTransaksi.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Kelompok Tani";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Tanggal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "ID Transaksi";
            // 
            // TransaksiOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.btnTambah);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dgvKeranjang);
            this.Controls.Add(this.numJumlah);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtStokSekarang);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbBarang);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbKelompok);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.txtIDTransaksi);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TransaksiOut";
            this.Text = "TransaksiOut";
            this.Load += new System.EventHandler(this.TransaksiOut_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeranjang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numJumlah)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnTambah;
        private System.Windows.Forms.TextBox lblTotal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dgvKeranjang;
        private System.Windows.Forms.NumericUpDown numJumlah;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtStokSekarang;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbBarang;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbKelompok;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.TextBox txtIDTransaksi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}