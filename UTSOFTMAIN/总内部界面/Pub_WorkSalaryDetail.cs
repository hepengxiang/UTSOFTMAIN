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
    public partial class Pub_WorkSalaryDetail : Form
    {
        public Pub_WorkSalaryDetail()
        {
            InitializeComponent();
        }
        public DataTable dtSource = new DataTable();
        private void Pub_WorkSalaryDetail_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.DataSource = dtSource;
        }
    }
}
