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
    public partial class TD_TK_LessonAdd : Form
    {     
        public TD_TK_LessonAdd()
        {
            InitializeComponent();
        }

        private void TD_TK_LessonAdd_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            string sqlFlush = string.Format("select id,LessonName,LessonOrder from TK_LessonManage " +
                    "order by LessonOrder asc", frmUTSOFTMAIN.CompanyNames);//where CompanyNames = '{0}' 
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into TK_LessonManage values('{0}','{1}',{2},0,0,0,getdate(),getdate(),'{3}')",
                    frmUTSOFTMAIN.CompanyNames, this.textBoxX1.Text.Trim(), this.textBoxX2.Text.Trim(), frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFlush = string.Format("select id,LessonName,LessonOrder from TK_LessonManage "+
                    "where CompanyNames = '{0}' and SubmitPerson = '{1}' order by LessonOrder asc",
                    frmUTSOFTMAIN.CompanyNames,frmUTSOFTMAIN.StaffID);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from TK_LessonManage where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要修改选中数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update TK_LessonManage set LessonName = '{1}',LessonOrder = {2} where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.textBoxX1.Text.Trim(),this.textBoxX2.Text.Trim());
            }
            catch
            {
                MessageBox.Show("数据格式输入错误");
                return;
            }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("修改成功");
                string sqlFlush = string.Format("select id,LessonName,LessonOrder from TK_LessonManage " +
                    "where CompanyNames = '{0}' and SubmitPerson = '{1}' order by LessonOrder asc",
                    frmUTSOFTMAIN.CompanyNames, frmUTSOFTMAIN.StaffID);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }
    }
}
