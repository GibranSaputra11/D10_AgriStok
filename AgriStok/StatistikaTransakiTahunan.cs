using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgriStok
{
    public partial class StatistikaTransakiTahunan : Form
    {
        private DAL dbLogic = new DAL();
        public StatistikaTransakiTahunan()
        {
            InitializeComponent();
        }
    }
}
