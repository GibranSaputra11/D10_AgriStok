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
            this.txtNamaBarang = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSatuan = new System.Windows.Forms.ComboBox();
            this.cmbKategori = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSaveBarang = new System.Windows.Forms.Button();
            this.dataGridViewBarang = new System.Windows.Forms.DataGridView();
            this.btnUpdateBarang = new System.Windows.Forms.Button();
            this.btnDeleteBarang = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBarang)).BeginInit();
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
            // txtNamaBarang
            // 
            this.txtNamaBarang.Location = new System.Drawing.Point(153, 97);
            this.txtNamaBarang.Multiline = true;
            this.txtNamaBarang.Name = "txtNamaBarang";
            this.txtNamaBarang.Size = new System.Drawing.Size(115, 40);
            this.txtNamaBarang.TabIndex = 2;
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
            this.btnSaveBarang.Location = new System.Drawing.Point(338, 78);
            this.btnSaveBarang.Name = "btnSaveBarang";
            this.btnSaveBarang.Size = new System.Drawing.Size(96, 23);
            this.btnSaveBarang.TabIndex = 9;
            this.btnSaveBarang.Text = "Simpan Barang";
            this.btnSaveBarang.UseVisualStyleBackColor = true;
            this.btnSaveBarang.Click += new System.EventHandler(this.btnSaveBarang_Click);
            // 
            // dataGridViewBarang
            // 
            this.dataGridViewBarang.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBarang.Location = new System.Drawing.Point(63, 245);
            this.dataGridViewBarang.Name = "dataGridViewBarang";
            this.dataGridViewBarang.Size = new System.Drawing.Size(425, 180);
            this.dataGridViewBarang.TabIndex = 10;
            this.dataGridViewBarang.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBarang_CellContentClick);
            // 
            // btnUpdateBarang
            // 
            this.btnUpdateBarang.Location = new System.Drawing.Point(338, 121);
            this.btnUpdateBarang.Name = "btnUpdateBarang";
            this.btnUpdateBarang.Size = new System.Drawing.Size(96, 23);
            this.btnUpdateBarang.TabIndex = 11;
            this.btnUpdateBarang.Text = "Update Barang";
            this.btnUpdateBarang.UseVisualStyleBackColor = true;
            this.btnUpdateBarang.Click += new System.EventHandler(this.btnUpdateBarang_Click);
            // 
            // btnDeleteBarang
            // 
            this.btnDeleteBarang.Location = new System.Drawing.Point(338, 170);
            this.btnDeleteBarang.Name = "btnDeleteBarang";
            this.btnDeleteBarang.Size = new System.Drawing.Size(96, 23);
            this.btnDeleteBarang.TabIndex = 12;
            this.btnDeleteBarang.Text = "Delete Barang";
            this.btnDeleteBarang.UseVisualStyleBackColor = true;
            this.btnDeleteBarang.Click += new System.EventHandler(this.btnDeleteBarang_Click);
            // 
            // KelolaBarang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 453);
            this.Controls.Add(this.btnDeleteBarang);
            this.Controls.Add(this.btnUpdateBarang);
            this.Controls.Add(this.dataGridViewBarang);
            this.Controls.Add(this.btnSaveBarang);
            this.Controls.Add(this.cmbKategori);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbSatuan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNamaBarang);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBarangID);
            this.Name = "KelolaBarang";
            this.Text = "KelolaBarang";
            this.Load += new System.EventHandler(this.KelolaBarang_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBarang)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBarangID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNamaBarang;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSatuan;
        private System.Windows.Forms.ComboBox cmbKategori;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSaveBarang;
        private System.Windows.Forms.DataGridView dataGridViewBarang;
        private System.Windows.Forms.Button btnUpdateBarang;
        private System.Windows.Forms.Button btnDeleteBarang;
    }
}