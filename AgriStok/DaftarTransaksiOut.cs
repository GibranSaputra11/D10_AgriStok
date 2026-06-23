using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AgriStok
{
    public partial class DaftarTransaksiOut : Form
    {
        private DAL dbLogic = new DAL();

        public DaftarTransaksiOut()
        {
            InitializeComponent();
        }

        private void DaftarTransakiOut_Load(object sender, EventArgs e)
        {
            dgvMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMaster.AllowUserToAddRows = false;
            dgvMaster.ReadOnly = true;
            dgvMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDetail.AllowUserToAddRows = false;
            dgvDetail.ReadOnly = true;
            dgvDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            lblItemCount.Text = "Total Jenis Barang: 0";

            dgvMaster.CellClick += dgvMaster_CellClick;

            LoadMasterData();
        }

        private void LoadMasterData()
        {
            try
            {
                bindingSourceMaster.Filter = "";

                DataTable dt = dbLogic.GetDaftarTransaksiOut();

                bindingSourceMaster.DataSource = dt;
                dgvMaster.DataSource = bindingSourceMaster;

                if (bindingNavigatorMaster != null)
                {
                    bindingNavigatorMaster.BindingSource = bindingSourceMaster;
                }

                dgvMaster.ClearSelection();
                dgvDetail.DataSource = null;
                lblItemCount.Text = "Total Jenis Barang: 0";

                if (btnPrintNota != null) btnPrintNota.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat data Master: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDetailData(string idTransaksi)
        {
            try
            {
                DataTable dt = dbLogic.GetDetailTransaksiOut(idTransaksi);

                dgvDetail.DataSource = dt;
                lblItemCount.Text = $"Total Jenis Barang: {dt.Rows.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal memuat detail barang: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string idTransaksi = dgvMaster.Rows[e.RowIndex].Cells["ID Transaksi"].Value.ToString();

                LoadDetailData(idTransaksi);

                if (btnPrintNota != null) btnPrintNota.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                bindingSourceMaster.Filter = string.Format("[ID Transaksi] LIKE '%{0}%' OR [Nama Kelompok] LIKE '%{0}%'", txtSearch.Text);

                dgvMaster.ClearSelection();
                dgvDetail.DataSource = null;
                lblItemCount.Text = "Total Jenis Barang: 0";
                if (btnPrintNota != null) btnPrintNota.Enabled = false;
            }
            catch (Exception) { bindingSourceMaster.Filter = ""; }
        }

        private void btnAddOut_Click(object sender, EventArgs e)
        {
            TransaksiOut transaksiOut = new TransaksiOut();

            transaksiOut.ShowDialog();
            
            txtSearch.Clear();

            LoadMasterData();
        }

        private void btnEditOut_Click(object sender, EventArgs e)
        {
            if (dgvMaster.CurrentRow != null && dgvMaster.SelectedRows.Count > 0)
            {
                string idTransaksiTerpilih = dgvMaster.CurrentRow.Cells["ID Transaksi"].Value.ToString();

                TransaksiOut formTransaksi = new TransaksiOut();
                formTransaksi.BukaUntukEdit(idTransaksiTerpilih);
                formTransaksi.ShowDialog();

                LoadMasterData();
            }
            else
            {
                MessageBox.Show("Silakan pilih transaksi yang ingin diedit dari tabel terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnPrintNota_Click(object sender, EventArgs e)
        {
            if (dgvMaster.CurrentRow != null && dgvMaster.SelectedRows.Count > 0)
            {
                string idTransaksiTerpilih = dgvMaster.CurrentRow.Cells["ID Transaksi"].Value.ToString();

                ReportOut frmCetak = new ReportOut(idTransaksiTerpilih);
                frmCetak.ShowDialog();
            }
        }
    }
}
