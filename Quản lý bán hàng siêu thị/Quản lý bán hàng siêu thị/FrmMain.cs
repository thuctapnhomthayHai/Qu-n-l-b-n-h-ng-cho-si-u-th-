﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quản_lý_bán_hàng_siêu_thị
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void quảnLýKháchHàngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmKH KH = new frmKH();
            KH.Show();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {

        }

        private void khoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongKe TK = new ThongKe();
            TK.Show();
        }

        private void bánHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
