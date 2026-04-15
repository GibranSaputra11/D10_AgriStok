namespace AgriStok
{
    partial class KelolaBarang
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
            this.txtBarangID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textNamaBarang = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSatuan = new System.Windows.Forms.ComboBox();
            this.cmbKategori = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSaveBarang = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtBarangID
            // 
            this.txtBarangID.Location = new System.Drawing.Point(153, 59);
            this.txtBarangID.Name = "txtBarangID";
            this.txtBarangID.Size = new System.Drawing.Size(115, 20);
            this.txtBarangID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Barang ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Nama Barang";
            // 
            // textNamaBarang
            // 
            this.textNamaBarang.Location = new System.Drawing.Point(153, 97);
            this.textNamaBarang.Multiline = true;
            this.textNamaBarang.Name = "textNamaBarang";
            this.textNamaBarang.Size = new System.Drawing.Size(115, 40);
            this.textNamaBarang.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(60, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Satuan";
            // 
            // cmbSatuan
            // 
            this.cmbSatuan.FormattingEnabled = true;
            this.cmbSatuan.Location = new System.Drawing.Point(153, 157);
            this.cmbSatuan.Name = "cmbSatuan";
            this.cmbSatuan.Size = new System.Drawing.Size(115, 21);
            this.cmbSatuan.TabIndex = 6;
            // 
            // cmbKategori
            // 
            this.cmbKategori.FormattingEnabled = true;
            this.cmbKategori.Location = new System.Drawing.Point(153, 197);
            this.cmbKategori.Name = "cmbKategori";
            this.cmbKategori.Size = new System.Drawing.Size(115, 21);
            this.cmbKategori.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(60, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Kategori";
            // 
            // btnSaveBarang
            // 
            this.btnSaveBarang.Location = new System.Drawing.Point(125, 245);
            this.btnSaveBarang.Name = "btnSaveBarang";
            this.btnSaveBarang.Size = new System.Drawing.Size(96, 23);
            this.btnSaveBarang.TabIndex = 9;
            this.btnSaveBarang.Text = "Simpan Barang";
            this.btnSaveBarang.UseVisualStyleBackColor = true;
            // 
            // KelolaBarang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 298);
            this.Controls.Add(this.btnSaveBarang);
            this.Controls.Add(this.cmbKategori);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbSatuan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textNamaBarang);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBarangID);
            this.Name = "KelolaBarang";
            this.Text = "KelolaBarang";
            this.Load += new System.EventHandler(this.KelolaBarang_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBarangID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textNamaBarang;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSatuan;
        private System.Windows.Forms.ComboBox cmbKategori;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSaveBarang;
    }
}