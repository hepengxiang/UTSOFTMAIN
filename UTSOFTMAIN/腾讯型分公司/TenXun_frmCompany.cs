using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using UTSOFTMAIN.腾讯型分公司;

namespace UTSOFTMAIN
{
    public partial class TenXun_frmCompany : Form
    {
        public TenXun_frmCompany()
        {
            InitializeComponent();
        }

        private void buttonX8_Click(object sender, EventArgs e)//预定记录
        {
            TX_YD_VIPMessage tyv = new TX_YD_VIPMessage();
            tyv.Text = this.buttonX8.Text;
            tyv.Show();
        }

        private void buttonX2_Click(object sender, EventArgs e)//日常收支
        {
            TX_SerialDetail tsd = new TX_SerialDetail();
            tsd.Text = this.buttonX2.Text;
            tsd.Show();
        }

        private void buttonX3_Click(object sender, EventArgs e)//内部转账
        {
            TX_InsideSerial tis = new TX_InsideSerial();
            tis.Text = this.buttonX3.Text;
            tis.Show();
        }

        private void buttonX4_Click(object sender, EventArgs e)//报名记录
        {
            TX_BM_VIPMessage tbv = new TX_BM_VIPMessage();
            tbv.Text = this.buttonX4.Text;
            tbv.Show();
        }

        private void buttonX7_Click(object sender, EventArgs e)//投诉退款记录
        {
            TX_TSTK_Message ttm = new TX_TSTK_Message();
            ttm.Text = this.buttonX7.Text;
            ttm.Show();
        }

        private void buttonX9_Click(object sender, EventArgs e)//腾讯转移记录
        {
            TX_ZY_Message tzm = new TX_ZY_Message();
            tzm.Text = this.buttonX9.Text;
            tzm.Show();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            TX_FlushList tfl = new TX_FlushList();
            tfl.Text = this.buttonX6.Text;
            tfl.Show();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        private void buttonX5_Click(object sender, EventArgs e)//资源添加
        {
            TX_ZYTable_ResourceAddShow tzra = new TX_ZYTable_ResourceAddShow();
            tzra.groupName = "早间组";
            tzra.qqType = "营销QQ";
            tzra.Text = this.buttonX5.Text;
            tzra.Show();
        }

        private void buttonX10_Click(object sender, EventArgs e)//资源开发
        {
            TX_ZYTable_ResourceDevelopShow tzra = new TX_ZYTable_ResourceDevelopShow();
            tzra.groupName = "早间组";
            tzra.qqType = "营销QQ";
            tzra.Text = this.buttonX10.Text;
            tzra.Show();
        }

        private void buttonX11_Click(object sender, EventArgs e)//个人总结
        {
            TX_ZYTable_EveryDaySummaryShow yedss = new TX_ZYTable_EveryDaySummaryShow();
            yedss.qqType = "个人QQ";
            yedss.groupName = "早间";
            yedss.Text = this.buttonX11.Text; ;
            yedss.Show();
        }
    }
}
