using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTSOFTMAIN.优梯_行政部;

namespace UTSOFTMAIN
{
    public partial class UT_AdminPart : Form
    {
        public UT_AdminPart()
        {
            InitializeComponent();
        }
        private void buttonX4_Click(object sender, EventArgs e)//订单匹配
        {
            Admin_DataMatch adm = new Admin_DataMatch();
            adm.Text = this.buttonX4.Text;
            adm.Show();
        }

        private void buttonX5_Click(object sender, EventArgs e)//数据导入
        {
            UT_ImprotDate utd = new UT_ImprotDate();
            utd.Text = this.buttonX5.Text;
            utd.Show();
        }

        private void buttonX6_Click(object sender, EventArgs e)//明细统计表
        {
            UT_SerialDetail utis = new UT_SerialDetail();
            utis.Text = this.buttonX6.Text;
            utis.Show();
        }

        private void buttonX7_Click(object sender, EventArgs e)//账户往来统计表
        {
            Admin_AcountDetailCount aadc = new Admin_AcountDetailCount();
            aadc.Text = this.buttonX7.Text;
            aadc.Show();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            Admin_CardSerialDetail acsd = new Admin_CardSerialDetail();
            acsd.Text = this.buttonX1.Text;
            acsd.Show();
        }
    }
}
