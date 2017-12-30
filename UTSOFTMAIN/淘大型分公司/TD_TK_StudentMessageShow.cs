using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.淘大型分公司
{
    public partial class TD_TK_StudentMessageShow : Form
    {
        public TD_TK_StudentMessageShow()
        {
            InitializeComponent();
        }
        public DataTable dtSource = new DataTable();
        private void TD_TK_StudentMessageShow_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.DataSource = dtSource;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
        }
    }
}
