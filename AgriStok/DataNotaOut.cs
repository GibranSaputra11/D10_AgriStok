using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriStok
{
    internal class DataNotaOut
    {
        public string ID_Transaksi { get; set; }
        public string Nama_Kelompok { get; set; } 
        public DateTime Tanggal { get; set; }
        public string Nama_Barang { get; set; }
        public int Jumlah { get; set; }
    }
}
