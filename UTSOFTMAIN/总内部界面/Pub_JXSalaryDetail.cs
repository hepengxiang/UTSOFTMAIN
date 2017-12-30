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
    public partial class Pub_JXSalaryDetail : Form
    {
        public Pub_JXSalaryDetail()
        {
            InitializeComponent();
        }
        public DataTable dtSource = new DataTable();
        private void Pub_JXSalaryDetail_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.DataSource = dtSource;
        }
    }
}
