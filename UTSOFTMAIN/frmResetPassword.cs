using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN
{
    public partial class frmResetPassword : Form
    {
        public frmResetPassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text.Trim() == "")
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            if (this.textBox1.Text.Trim() != this.textBox2.Text.Trim())
            {
                MessageBox.Show("两次输入的密码不一致");
                return;
            }
            string sql1 = string.Format("update Users set PSW = '{0}' where StaffID = '{1}'",this.textBox1.Text.Trim(),frmUTSOFTMAIN.StaffID);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if(resultNum>0)
            {
                MessageBox.Show("密码修改成功,程序即将退出！");
                System.Environment.Exit(0);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
        }
    }
}
