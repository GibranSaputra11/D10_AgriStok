using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriStok
{
    internal class DAL
    {
        private readonly string connectionString;

        public DAL()
        {
            connectionString = GetConnectionString();
        }

        public string GetConnectionString()
        {
            string localIP = GetLocalIPAddress();
            return $"Data Source={localIP};Initial Catalog=GudangPertanianDB;User ID=sa;Password=12345678;";
        }

        public static string GetLocalIPAddress()
        {
            string localIP = string.Empty;
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        localIP = ip.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting local IP address: " + ex.Message);
            }
            return localIP;
        }

        public void InsertLogError(string pesan)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertLogError", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Pesan", pesan);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                }
            }
        }

        #region Form Barang
        public string GenerateIdBarang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GenerateIdBarang", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "BR-001";
            }
        }

        public DataTable GetDropdownKategori()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_DropdownKategori", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetDropdownSatuan(string namaKategori)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetDropdownSatuan", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                if (string.IsNullOrEmpty(namaKategori))
                    cmd.Parameters.AddWithValue("@NamaKategori", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@NamaKategori", namaKategori);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetKelolaBarang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_KelolaBarang", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public void InsertBarang(string id, string nama, string idSatuan, string idKategori, byte[] foto)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertBarang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@IdSatuan", idSatuan);
                    cmd.Parameters.AddWithValue("@IdKategori", idKategori);

                    if (foto != null)
                        cmd.Parameters.AddWithValue("@Foto", foto);
                    else
                        cmd.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = DBNull.Value;

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback(); 

                    InsertLogError("Gagal Insert Barang [" + id + "]: " + ex.Message);

                    throw ex; 
                }
            }
        }

        public void UpdateBarang(string id, string nama, string idSatuan, string idKategori, byte[] foto)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateBarang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@IdSatuan", idSatuan);
                    cmd.Parameters.AddWithValue("@IdKategori", idKategori);

                    if (foto != null)
                        cmd.Parameters.AddWithValue("@Foto", foto);
                    else
                        cmd.Parameters.Add("@Foto", SqlDbType.VarBinary).Value = DBNull.Value;

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Update Barang [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void DeleteBarang(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();

                try
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteBarang", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    InsertLogError("Gagal Delete Barang [" + id + "]: " + ex.Message);

                    throw ex;
                }
            }
        }
        #endregion

        #region Form Satuan
        public string GenerateIdSatuan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GenerateIdSatuan", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "ST-001";
            }
        }

        public DataTable GetSatuan()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_Satuan", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertSatuan(string id, string nama)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertSatuan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Insert Satuan [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void UpdateSatuan(string id, string nama)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateSatuan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Update Satuan [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void DeleteSatuan(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteSatuan", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Delete Satuan [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }
        #endregion

        #region Form Kategori
        public string GenerateIdKategori()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GenerateIdKategori", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "KT-001";
            }
        }

        public DataTable GetKategori()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_Kategori", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertKategori(string id, string nama)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertKategori", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Insert Kategori [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void UpdateKategori(string id, string nama)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateKategori", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Update Kategori [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void DeleteKategori(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteKategori", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Delete Kategori [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }
        #endregion

        #region Form Supplier
        public string GenerateIdSupplier()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GenerateIdSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "SP-001";
            }
        }

        public DataTable GetSupplier()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_KelolaSupplier", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertSupplier(string id, string nama, string noTlp, string alamat)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertSupplier", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@NoTlp", noTlp);
                    cmd.Parameters.AddWithValue("@Alamat", alamat);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Insert Supplier [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void UpdateSupplier(string id, string nama, string noTlp, string alamat)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateSupplier", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@NoTlp", noTlp);
                    cmd.Parameters.AddWithValue("@Alamat", alamat);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Update Supplier [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void DeleteSupplier(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteSupplier", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Delete Supplier [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }
        #region Sekenario SQL Inject
        public void BackupSupplierData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    IF OBJECT_ID('dbo.Supplier_Backup') IS NOT NULL DROP TABLE dbo.Supplier_Backup;
                    SELECT * INTO dbo.Supplier_Backup FROM dbo.Supplier;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public int SimulateSQLInjection(string inputNamaSupplier)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Supplier SET Nama_Supplier = 'HACKED_BY_GIBRAN' WHERE Nama_Supplier = '" + inputNamaSupplier + "'";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public void RestoreSupplierData()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    IF OBJECT_ID('dbo.Supplier_Backup') IS NOT NULL
                    BEGIN
                        UPDATE s 
                        SET s.Nama_Supplier = b.Nama_Supplier
                        FROM dbo.Supplier s
                        INNER JOIN dbo.Supplier_Backup b ON s.Id_Supplier = b.Id_Supplier;
                    END";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
        #endregion
        #endregion

        #region Form Kelompok Tani
        public string GenerateIdKelompokTani()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GenerateIdKelompok", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "KTN-001";
            }
        }

        public DataTable GetKelompokTani()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM vw_KelolaKelompokTani", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertKelompokTani(string id, string nama, string noTlp, string alamat)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertKelompokTani", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@NoTlp", noTlp);
                    cmd.Parameters.AddWithValue("@Alamat", alamat);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Insert Kelompok Tani [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void UpdateKelompokTani(string id, string nama, string noTlp, string alamat)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_UpdateKelompokTani", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Nama", nama);
                    cmd.Parameters.AddWithValue("@NoTlp", noTlp);
                    cmd.Parameters.AddWithValue("@Alamat", alamat);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Update Kelompok Tani [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }

        public void DeleteKelompokTani(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_DeleteKelompokTani", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = trans;

                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    InsertLogError("Gagal Delete Kelompok Tani [" + id + "]: " + ex.Message);
                    throw ex;
                }
            }
        }
        #endregion

        #region Statistika
        public DataTable GetStatistikaTransaksiTahunan(int tahun)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetStatistikaTransaksiTahunan", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Tahun", tahun);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region Daftar Transaksi Masuk
        public DataTable GetDaftarTransaksiIn()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM vw_DaftarTransaksiIn ORDER BY [Tanggal Masuk] DESC, [ID Transaksi] DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable GetDetailTransaksiIn(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT [ID Barang], [Nama Barang], [Jumlah Masuk] FROM vw_DetailTransaksiIn WHERE [ID Transaksi] = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idTransaksi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        public DataTable GetNotaTransaksiIn(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CetakNotaIn", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdIn", idTransaksi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region Daftar Transaksi Keluar
        public DataTable GetDaftarTransaksiOut()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM vw_DaftarTransaksiOut ORDER BY [Tanggal Keluar] DESC, [ID Transaksi] DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetDetailTransaksiOut(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT [ID Barang], [Nama Barang], [Jumlah Keluar] FROM vw_DetailTransaksiOut WHERE [ID Transaksi] = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", idTransaksi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public DataTable GetNotaTransaksiOut(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_CetakNotaOut", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdOut", idTransaksi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }
        #endregion

        #region Transaksi Masuk
        public DataTable GetDropdownSupplier()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Supplier, Nama_Supplier FROM vw_DropdownSupplier", conn);
                DataTable dt = new DataTable(); da.Fill(dt); return dt;
            }
        }

        public DataTable GetDropdownBarang()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT Id_Barang, Nama_Barang FROM vw_DropdownBarang", conn);
                DataTable dt = new DataTable(); da.Fill(dt); return dt;
            }
        }

        public string GenerateIdTransaksiIn()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GenerateIdIn", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "TR-001";
            }
        }

        public int GetStokBarang(string idBarang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Stok_Barang FROM vw_DropdownBarang WHERE Id_Barang = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", idBarang);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

        public string CekTransaksiKembarIn(string idSupplier, DateTime tanggal)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id_In FROM vw_MasterTransaksiIn WHERE Id_Supplier = @IdSupplier AND Tgl_In = @Tgl", conn);
                cmd.Parameters.AddWithValue("@IdSupplier", idSupplier);
                cmd.Parameters.AddWithValue("@Tgl", tanggal.Date);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
        }

        public DataTable GetMasterTransaksiIn(string idTransaksi)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Id_Supplier, Tgl_In FROM vw_MasterTransaksiIn WHERE Id_In = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", idTransaksi);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public void InsertTransaksiIn(string id, string idSupplier, DateTime tgl, int total, DataTable keranjang)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmdMaster = new SqlCommand("sp_InsertTransaksiIn", conn, trans);
                    cmdMaster.CommandType = CommandType.StoredProcedure;
                    cmdMaster.Parameters.AddWithValue("@Id", id);
                    cmdMaster.Parameters.AddWithValue("@IdSupplier", idSupplier);
                    cmdMaster.Parameters.AddWithValue("@Tgl", tgl);
                    cmdMaster.Parameters.AddWithValue("@Total", total);
                    cmdMaster.ExecuteNonQuery();

                    foreach (DataRow row in keranjang.Rows)
                    {
                        SqlCommand cmdDetail = new SqlCommand("sp_InsertDetailIn", conn, trans);
                        cmdDetail.CommandType = CommandType.StoredProcedure;
                        cmdDetail.Parameters.AddWithValue("@IdIn", id);
                        cmdDetail.Parameters.AddWithValue("@IdBarang", row["Id_Barang"]);
                        cmdDetail.Parameters.AddWithValue("@Qty", row["Qty"]);
                        cmdDetail.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex) { trans.Rollback(); throw ex; }
            }
        }

        public void UpdateTransaksiIn(string id, string idSupplier, DateTime tgl, int total, DataTable keranjang, List<string> listHapus)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmdMaster = new SqlCommand("sp_UpdateTransaksiIn", conn, trans);
                    cmdMaster.CommandType = CommandType.StoredProcedure;
                    cmdMaster.Parameters.AddWithValue("@Id", id);
                    cmdMaster.Parameters.AddWithValue("@IdSupplier", idSupplier);
                    cmdMaster.Parameters.AddWithValue("@Tgl", tgl);
                    cmdMaster.Parameters.AddWithValue("@Total", total);
                    cmdMaster.ExecuteNonQuery();

                    foreach (string idHapus in listHapus)
                    {
                        SqlCommand cmdHapus = new SqlCommand("sp_DeleteDetailInItem", conn, trans);
                        cmdHapus.CommandType = CommandType.StoredProcedure;
                        cmdHapus.Parameters.AddWithValue("@IdIn", id);
                        cmdHapus.Parameters.AddWithValue("@IdBarang", idHapus);
                        cmdHapus.ExecuteNonQuery();
                    }

                    foreach (DataRow row in keranjang.Rows)
                    {
                        SqlCommand cmdDetail = new SqlCommand("sp_UpdateDetailIn", conn, trans);
                        cmdDetail.CommandType = CommandType.StoredProcedure;
                        cmdDetail.Parameters.AddWithValue("@IdIn", id);
                        cmdDetail.Parameters.AddWithValue("@IdBarang", row["Id_Barang"]);
                        cmdDetail.Parameters.AddWithValue("@QtyBaru", row["Qty"]);
                        cmdDetail.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex) { trans.Rollback(); throw ex; }
            }
        }
        #endregion
    }
}
