namespace AgriStok
{
    partial class KelolaKelompokTani
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KelolaKelompokTani));
            this.txtTlpKelompok = new System.Windows.Forms.TextBox();
            this.txtAlamatKelompok = new System.Windows.Forms.TextBox();
            this.btnDeleteKelompok = new System.Windows.Forms.Button();
            this.btnUpdateKelompok = new System.Windows.Forms.Button();
            this.dgvKelompokTani = new System.Windows.Forms.DataGridView();
            this.btnAddKelompok = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNamaKelompok = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKelompokID = new System.Windows.Forms.TextBox();
            this.bindingNavigatorKelompokTani = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingSourceKelompokTani = new System.Windows.Forms.BindingSource(this.components);
            this.btnImpExcel = new System.Windows.Forms.Button();
            this.btnImpDb = new System.Windows.Forms.Button();
            this.lblNamaFile = new System.Windows.Forms.Label();
            this.lblStatusGrid = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKelompokTani)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorKelompokTani)).BeginInit();
            this.bindingNavigatorKelompokTani.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceKelompokTani)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTlpKelompok
            // 
            this.txtTlpKelompok.Location = new System.Drawing.Point(135, 197);
            this.txtTlpKelompok.Name = "txtTlpKelompok";
            this.txtTlpKelompok.Size = new System.Drawing.Size(115, 20);
            this.txtTlpKelompok.TabIndex = 38;
            // 
            // txtAlamatKelompok
            // 
            this.txtAlamatKelompok.Location = new System.Drawing.Point(135, 123);
            this.txtAlamatKelompok.Multiline = true;
            this.txtAlamatKelompok.Name = "txtAlamatKelompok";
            this.txtAlamatKelompok.Size = new System.Drawing.Size(173, 55);
            this.txtAlamatKelompok.TabIndex = 37;
            // 
            // btnDeleteKelompok
            // 
            this.btnDeleteKelompok.Location = new System.Drawing.Point(350, 153);
            this.btnDeleteKelompok.Name = "btnDeleteKelompok";
            this.btnDeleteKelompok.Size = new System.Drawing.Size(90, 35);
            this.btnDeleteKelompok.TabIndex = 36;
            this.btnDeleteKelompok.Text = "Delete Kelompok Tani";
            this.btnDeleteKelompok.UseVisualStyleBackColor = true;
            this.btnDeleteKelompok.Click += new System.EventHandler(this.btnDeleteKelompok_Click);
            // 
            // btnUpdateKelompok
            // 
            this.btnUpdateKelompok.Location = new System.Drawing.Point(350, 98);
            this.btnUpdateKelompok.Name = "btnUpdateKelompok";
            this.btnUpdateKelompok.Size = new System.Drawing.Size(90, 39);
            this.btnUpdateKelompok.TabIndex = 35;
            this.btnUpdateKelompok.Text = "Update Kelompok Tani";
            this.btnUpdateKelompok.UseVisualStyleBackColor = true;
            this.btnUpdateKelompok.Click += new System.EventHandler(this.btnUpdateKelompok_Click);
            // 
            // dgvKelompokTani
            // 
            this.dgvKelompokTani.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKelompokTani.Location = new System.Drawing.Point(23, 244);
            this.dgvKelompokTani.Name = "dgvKelompokTani";
            this.dgvKelompokTani.Size = new System.Drawing.Size(634, 180);
            this.dgvKelompokTani.TabIndex = 34;
            this.dgvKelompokTani.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvKelompokTani_CellClick);
            // 
            // btnAddKelompok
            // 
            this.btnAddKelompok.Location = new System.Drawing.Point(350, 46);
            this.btnAddKelompok.Name = "btnAddKelompok";
            this.btnAddKelompok.Size = new System.Drawing.Size(90, 39);
            this.btnAddKelompok.TabIndex = 33;
            this.btnAddKelompok.Text = "Add Kelompok Tani";
            this.btnAddKelompok.UseVisualStyleBackColor = true;
            this.btnAddKelompok.Click += new System.EventHandler(this.btnAddKelompok_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "No Tlp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "Alamat";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Nama Kelompok Tani";
            // 
            // txtNamaKelompok
            // 
            this.txtNamaKelompok.Location = new System.Drawing.Point(135, 69);
            this.txtNamaKelompok.Multiline = true;
            this.txtNamaKelompok.Name = "txtNamaKelompok";
            this.txtNamaKelompok.Size = new System.Drawing.Size(173, 40);
            this.txtNamaKelompok.TabIndex = 29;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 28;
            this.label1.Text = "Kelompok Tani ID";
            // 
            // txtKelompokID
            // 
            this.txtKelompokID.Location = new System.Drawing.Point(135, 31);
            this.txtKelompokID.Name = "txtKelompokID";
            this.txtKelompokID.Size = new System.Drawing.Size(115, 20);
            this.txtKelompokID.TabIndex = 27;
            // 
            // bindingNavigatorKelompokTani
            // 
            this.bindingNavigatorKelompokTani.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigatorKelompokTani.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigatorKelompokTani.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigatorKelompokTani.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigatorKelompokTani.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigatorKelompokTani.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigatorKelompokTani.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigatorKelompokTani.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigatorKelompokTani.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigatorKelompokTani.Name = "bindingNavigatorKelompokTani";
            this.bindingNavigatorKelompokTani.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigatorKelompokTani.Size = new System.Drawing.Size(800, 25);
            this.bindingNavigatorKelompokTani.TabIndex = 39;
            this.bindingNavigatorKelompokTani.Text = "bindingNavigatorKelompokTani";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this.bindingNavigatorAddNewItem_Click);
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnImpExcel
            // 
            this.btnImpExcel.Location = new System.Drawing.Point(483, 80);
            this.btnImpExcel.Name = "btnImpExcel";
            this.btnImpExcel.Size = new System.Drawing.Size(126, 29);
            this.btnImpExcel.TabIndex = 40;
            this.btnImpExcel.Text = "Pilih Excel";
            this.btnImpExcel.UseVisualStyleBackColor = true;
            this.btnImpExcel.Click += new System.EventHandler(this.btnImpExcel_Click);
            // 
            // btnImpDb
            // 
            this.btnImpDb.Enabled = false;
            this.btnImpDb.Location = new System.Drawing.Point(483, 126);
            this.btnImpDb.Name = "btnImpDb";
            this.btnImpDb.Size = new System.Drawing.Size(126, 29);
            this.btnImpDb.TabIndex = 41;
            this.btnImpDb.Text = "Simpan ke Database";
            this.btnImpDb.UseVisualStyleBackColor = true;
            this.btnImpDb.Click += new System.EventHandler(this.btnImpDb_Click);
            // 
            // lblNamaFile
            // 
            this.lblNamaFile.AutoSize = true;
            this.lblNamaFile.Location = new System.Drawing.Point(625, 88);
            this.lblNamaFile.Name = "lblNamaFile";
            this.lblNamaFile.Size = new System.Drawing.Size(54, 13);
            this.lblNamaFile.TabIndex = 42;
            this.lblNamaFile.Text = "Nama File";
            // 
            // lblStatusGrid
            // 
            this.lblStatusGrid.AutoSize = true;
            this.lblStatusGrid.Location = new System.Drawing.Point(353, 216);
            this.lblStatusGrid.Name = "lblStatusGrid";
            this.lblStatusGrid.Size = new System.Drawing.Size(304, 13);
            this.lblStatusGrid.TabIndex = 43;
            this.lblStatusGrid.Text = "MODE PREVIEW: Data Excel (Belum Tersimpan ke Database)";
            // 
            // KelolaKelompokTani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblStatusGrid);
            this.Controls.Add(this.lblNamaFile);
            this.Controls.Add(this.btnImpDb);
            this.Controls.Add(this.btnImpExcel);
            this.Controls.Add(this.bindingNavigatorKelompokTani);
            this.Controls.Add(this.txtTlpKelompok);
            this.Controls.Add(this.txtAlamatKelompok);
            this.Controls.Add(this.btnDeleteKelompok);
            this.Controls.Add(this.btnUpdateKelompok);
            this.Controls.Add(this.dgvKelompokTani);
            this.Controls.Add(this.btnAddKelompok);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNamaKelompok);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKelompokID);
            this.Name = "KelolaKelompokTani";
            this.Text = "KelolaKelompokTani";
            this.Load += new System.EventHandler(this.KelolaKelompokTani_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKelompokTani)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigatorKelompokTani)).EndInit();
            this.bindingNavigatorKelompokTani.ResumeLayout(false);
            this.bindingNavigatorKelompokTani.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceKelompokTani)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTlpKelompok;
        private System.Windows.Forms.TextBox txtAlamatKelompok;
        private System.Windows.Forms.Button btnDeleteKelompok;
        private System.Windows.Forms.Button btnUpdateKelompok;
        private System.Windows.Forms.DataGridView dgvKelompokTani;
        private System.Windows.Forms.Button btnAddKelompok;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNamaKelompok;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKelompokID;
        private System.Windows.Forms.BindingNavigator bindingNavigatorKelompokTani;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.BindingSource bindingSourceKelompokTani;
        private System.Windows.Forms.Button btnImpExcel;
        private System.Windows.Forms.Button btnImpDb;
        private System.Windows.Forms.Label lblNamaFile;
        private System.Windows.Forms.Label lblStatusGrid;
    }
}