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
    public partial class UT_VIPMessage : Form
    {
        public UT_VIPMessage()
        {
            InitializeComponent();
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Pub_VIPMessage where QQ = '{0}' and CompanyNames = '优梯' "+
                "and EnterType in ('预定转报名','报名')", this.textBoxX1.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0) 
            {
                MessageBox.Show("未查询到此QQ信息");
            }
            this.dataGridViewX1.DataSource = dt1;
        }
    }
}
