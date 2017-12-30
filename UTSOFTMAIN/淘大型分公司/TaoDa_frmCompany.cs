using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using UTSOFTMAIN.淘大型分公司;

namespace UTSOFTMAIN
{
    public partial class TaoDa_frmCompany : Form
    {
        public TaoDa_frmCompany()
        {
            InitializeComponent();
        }
        private void TaoDa_frmCompany_Load(object sender, EventArgs e)
        {
            if (frmUTSOFTMAIN.AssumedName == "天空" || frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType.Contains("行政"))
            {
                this.buttonX7.Visible = true;
                this.buttonX8.Visible = true;
            }
            else 
            {
                this.buttonX7.Visible = false;
                this.buttonX8.Visible = false;
            }
        }
        private void buttonX4_Click(object sender, EventArgs e)
        {
            TD_YD_VIPMessageTable tableShow = new TD_YD_VIPMessageTable();
            tableShow.Text = buttonX4.Text;
            tableShow.Show();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            TD_BM_VIPMessageTable tableShow = new TD_BM_VIPMessageTable();
            tableShow.Text = buttonX2.Text;
            tableShow.Show();
        }

        private void buttonX3_Click(object sender, EventArgs e)//日常收支记录
        {
            TD_SerialDetail tsd = new TD_SerialDetail();
            tsd.Text = buttonX3.Text;
            tsd.Show();
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            TD_ComplainRefundInfo tableShow = new TD_ComplainRefundInfo();
            tableShow.Text = buttonX9.Text;
            tableShow.Show();
        }

        private void buttonX5_Click(object sender, EventArgs e)//内部转账
        {
            TD_InsideSerial tis = new TD_InsideSerial();
            tis.Text = this.buttonX5.Text;
            tis.Show();
        }

        private void buttonX6_Click(object sender, EventArgs e)//刷单记录
        {
            TD_FlushList tfl = new TD_FlushList();
            tfl.Text = this.buttonX6.Text;
            tfl.Show();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {

        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            TD_TK_LessonAdd ttla = new TD_TK_LessonAdd();
            ttla.Text = this.buttonX7.Text;
            ttla.Show();
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            TD_TK_LessonDateManager ttldm = new TD_TK_LessonDateManager();
            ttldm.Text = this.buttonX8.Text;
            ttldm.Show();
        }

        
    }
}
