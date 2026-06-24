using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace AgriStok
{
    public partial class FormKonfigurasiDB : Form
    {
        public FormKonfigurasiDB()
        {
            InitializeComponent();

            try
            {
                if (ConfigurationManager.ConnectionStrings["GudangDbConn"] != null)
                {
                    string connStringLama = ConfigurationManager.ConnectionStrings["GudangDbConn"].ConnectionString;

                    if (!string.IsNullOrEmpty(connStringLama))
                    {
                        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connStringLama);

                        txtServer.Text = builder.DataSource;
                        txtUser.Text = builder.UserID;
                        txtPassword.Text = builder.Password;
                    }
                }
                else
                {
                    txtUser.Text = "sa"; 
                }
            }
            catch { }
        }

       
        private void btnGetIP_Click(object sender, EventArgs e)
        {
            txtServer.Text = GetLocalIPAddress();
        }

        private string GetLocalIPAddress()
        {
            string localIP = string.Empty;
            try
            {
                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.OperationalStatus == OperationalStatus.Up &&
                        (item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                         item.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                    {
                        string deskripsi = item.Description.ToLower();
                        if (deskripsi.Contains("vmware") || deskripsi.Contains("virtual") || deskripsi.Contains("pseudo"))
                        {
                            continue; 
                        }

                        var gateways = item.GetIPProperties().GatewayAddresses;
                        if (gateways.Count == 0)
                        {
                            continue; 
                        }

                        foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                localIP = ip.Address.ToString();
                                return localIP;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error mendeteksi IP Wi-Fi/LAN: " + ex.Message, "Error Jaringan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (string.IsNullOrEmpty(localIP))
            {
                localIP = "127.0.0.1"; 
            }

            return localIP;
        }

        private void btnGetServerLokal_Click(object sender, EventArgs e)
        {
            try
            {
                string serverDitemukan = ".";

                RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

                if (key != null && key.GetValueNames().Length > 0)
                {
                    string namaInstance = key.GetValueNames()[0];

                    if (namaInstance != "MSSQLSERVER")
                    {
                        serverDitemukan = $@".\{namaInstance}";
                    }
                }

                txtServer.Text = serverDitemukan;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal mendeteksi server SQL lokal.\nDetail: " + ex.Message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                txtServer.Text = ".";
            }
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            string ipServer = txtServer.Text;
            string userDb = txtUser.Text;
            string passDb = txtPassword.Text;

            string koneksiBaru = $"Data Source={ipServer};Initial Catalog=GudangPertanianDB;User ID={userDb};Password={passDb};";

            try
            {
                using (SqlConnection conn = new SqlConnection(koneksiBaru))
                {
                    conn.Open();
                }

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (config.ConnectionStrings.ConnectionStrings["GudangDbConn"] != null)
                {
                    config.ConnectionStrings.ConnectionStrings["GudangDbConn"].ConnectionString = koneksiBaru;
                }
                else
                {
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("GudangDbConn", koneksiBaru, "System.Data.SqlClient"));
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");

                MessageBox.Show("Konfigurasi Database berhasil disimpan! Aplikasi siap digunakan.", "Koneksi Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                Dashboard formUtama = new Dashboard();
                formUtama.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Koneksi Gagal! Pastikan IP dan Password benar, serta SQL Server/Firewall sudah diizinkan.\n\nDetail: " + ex.Message, "Error Koneksi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
