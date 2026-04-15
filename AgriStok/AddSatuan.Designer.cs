namespace AgriStok
{
    partial class AddSatuan
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
            this.txtSatuanID = new System.Windows.Forms.TextBox();
            this.txtNamaSatuan = new System.Windows.Forms.TextBox();
            this.btnSimpan = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvSatuan = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSatuan)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSatuanID
            // 
            this.txtSatuanID.Location = new System.Drawing.Point(113, 51);
            this.txtSatuanID.Name = "txtSatuanID";
            this.txtSatuanID.Size = new System.Drawing.Size(83, 20);
            this.txtSatuanID.TabIndex = 0;
            // 
            // txtNamaSatuan
            // 
            this.txtNamaSatuan.Location = new System.Drawing.Point(113, 95);
            this.txtNamaSatuan.Name = "txtNamaSatuan";
            this.txtNamaSatuan.Size = new System.Drawing.Size(83, 20);
            this.txtNamaSatuan.TabIndex = 1;
            // 
            // btnSimpan
            // 
            this.btnSimpan.Location = new System.Drawing.Point(230, 51);
            this.btnSimpan.Name = "btnSimpan";
            this.btnSimpan.Size = new System.Drawing.Size(86, 20);
            this.btnSimpan.TabIndex = 2;
            this.btnSimpan.Text = "Simpan";
            this.btnSimpan.UseVisualStyleBackColor = true;
            this.btnSimpan.Click += new System.EventHandler(this.btnSimpan_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(230, 94);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(86, 20);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(230, 139);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(86, 20);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // dgvSatuan
            // 
            this.dgvSatuan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSatuan.Location = new System.Drawing.Point(47, 207);
            this.dgvSatuan.Name = "dgvSatuan";
            this.dgvSatuan.Size = new System.Drawing.Size(269, 189);
            this.dgvSatuan.TabIndex = 5;
            this.dgvSatuan.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSatuan_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Satuan ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Satuan";
            // 
            // AddSatuan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSatuan);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnSimpan);
            this.Controls.Add(this.txtNamaSatuan);
            this.Controls.Add(this.txtSatuanID);
            this.Name = "AddSatuan";
            this.Text = "AddSatuan";
            this.Load += new System.EventHandler(this.AddSatuan_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSatuan)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSatuanID;
        private System.Windows.Forms.TextBox txtNamaSatuan;
        private System.Windows.Forms.Button btnSimpan;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvSatuan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}