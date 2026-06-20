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
        public void InsertBarang(string id, string nama, string idSatuan, string idKategori)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_InsertBarang", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nama", nama);
                cmd.Parameters.AddWithValue("@IdSatuan", idSatuan);
                cmd.Parameters.AddWithValue("@IdKategori", idKategori);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateBarang(string id, string nama, string idSatuan, string idKategori)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateBarang", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nama", nama);
                cmd.Parameters.AddWithValue("@IdSatuan", idSatuan);
                cmd.Parameters.AddWithValue("@IdKategori", idKategori);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteBarang(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteBarang", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}
