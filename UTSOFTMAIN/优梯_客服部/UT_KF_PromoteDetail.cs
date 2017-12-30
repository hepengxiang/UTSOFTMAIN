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
    public partial class UT_KF_PromoteDetail : Form
    {
        public UT_KF_PromoteDetail()
        {
            InitializeComponent();
        }
        public static DataTable dtSource = new DataTable();
        private void UT_KF_PromoteDetail_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.DataSource = dtSource;
        }
    }
}
