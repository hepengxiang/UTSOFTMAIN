using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.优梯_个人情况
{
    public partial class JX_DetailShow : Form
    {
        public JX_DetailShow()
        {
            InitializeComponent();
        }
        public DataTable dtSource = new DataTable();
        private void JX_DetailShow_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.DataSource = dtSource;
        }
    }
}
