using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.福利市场
{
    public partial class FLSCDataManager : Form
    {
        public FLSCDataManager()
        {
            InitializeComponent();
        }

        private void FLSCDataManager_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery("select * from FuLiShiChangSWPrice_Temp order by id asc");
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("insert into FuLiShiChangSWPrice values('{0}',{1})",
                this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim());
            int resutNum = DBHelper.ExecuteUpdate(sql1);
            if (resutNum > 0)
            {
                MessageBox.Show("提交成功");
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery("select * from FuLiShiChangSWPrice_Temp order by id asc");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string sqldel = string.Format("delete from FuLiShiChangSWPrice_Temp where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum = DBHelper.ExecuteUpdate(sqldel);
            if (resultNum > 0)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
        }
    }
}
