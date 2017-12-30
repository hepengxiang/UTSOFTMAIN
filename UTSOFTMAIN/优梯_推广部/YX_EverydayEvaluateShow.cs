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
    public partial class YX_EverydayEvaluateShow : Form
    {
        public YX_EverydayEvaluateShow()
        {
            InitializeComponent();
        }
        public string groupName = "";
        private void YX_EverydayEvaluateShow_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);


            this.comboBoxEx1.Text = frmUTSOFTMAIN.GroupName;
            comboBoxEx1_SelectedIndexChanged(null, null);
            this.comboBoxEx2.Text = frmUTSOFTMAIN.AssumedName;
            
            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" || frmUTSOFTMAIN.UserType == "行政经理")
            {
                this.comboBoxEx1.Enabled = true;
                this.comboBoxEx2.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("主管"))
            {
                this.comboBoxEx1.Enabled = true;
                this.comboBoxEx2.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("组长"))
            {
                this.comboBoxEx1.Enabled = false;
                this.comboBoxEx2.Enabled = true;
            }
            else
            {
                this.comboBoxEx1.Enabled = false;
                this.comboBoxEx2.Enabled = false;
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into UT_YXTable_EverydayEvaluate " +
                "values('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}')",
                groupName,this.dateTimeInput1.Value.ToShortDateString(), this.textBoxX1.Text.Trim(), 
                this.textBoxX2.Text.Trim(), this.textBoxX3.Text.Trim(), this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(),frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFlush = string.Format("select a.id,a.GroupName, a.EnterTime as 好评日期," +
                " a.StartCourseTime as 开课时间, a.StartCourseID as 开课ID ," +
                " a.GoodEvaluateCount as 好评数量,a.CourseGuide as 课堂引导,a.MemberGuide as 成员引导, b.AssumedName" +
                " from UT_YXTable_EverydayEvaluate a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.SubmitPerson = '{0}' and a.EnterTime = '{1}' order by a.EnterTime desc", 
                frmUTSOFTMAIN.StaffID, this.dateTimeInput1.Value.ToShortDateString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_YXTable_EverydayEvaluate where id = {0}",
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
            if (MessageBox.Show("你确定要修改【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_YXTable_EverydayEvaluate set "+
                    "StartCourseTime = '{1}',StartCourseID = '{2}',GoodEvaluateCount = {3},CourseGuide = '{4}'," +
                    "MemberGuide = '{5}',EnterTime = '{6}' where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(), 
                this.textBoxX1.Text.Trim(), 
                this.textBoxX2.Text.Trim(), 
                this.textBoxX3.Text.Trim(), 
                this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(),
                this.dateTimeInput1.Value.ToShortDateString());
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
                string sqlFlush = string.Format("select a.id,a.GroupName, a.EnterTime as 好评日期," +
                " a.StartCourseTime as 开课时间, a.StartCourseID as 开课ID ," +
                " a.GoodEvaluateCount as 好评数量,a.CourseGuide as 课堂引导,a.MemberGuide as 成员引导, b.AssumedName" +
                " from UT_YXTable_EverydayEvaluate a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,a.GroupName, a.EnterTime as 好评日期," +
                " a.StartCourseTime as 开课时间, a.StartCourseID as 开课ID ," +
                " a.GoodEvaluateCount as 好评数量,a.CourseGuide as 课堂引导,a.MemberGuide as 成员引导 ,b.AssumedName" +
                " from UT_YXTable_EverydayEvaluate a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.EnterTime between '{0}' and '{1}' and " +
                " b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and b.GroupName like '%{2}%' and " +
                " a.SubmitPerson like '%{3}%' order by a.EnterTime desc",
                this.dateTimeInput2.Value.ToShortDateString(), 
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx1.Text,this.comboBoxEx2.SelectedValue);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if(dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部'");
            string[] columNames = new string[] { "GroupName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx1.DataSource = dtPerson;
            this.comboBoxEx1.ValueMember = "GroupName";
            this.comboBoxEx1.DisplayMember = "GroupName";
            this.comboBoxEx1.SelectedIndex = -1;

        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "")
                return;
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部' and GroupName = '{0}'", this.comboBoxEx1.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPerson;
            this.comboBoxEx2.ValueMember = "StaffID";
            this.comboBoxEx2.DisplayMember = "AssumedName";
            this.comboBoxEx2.SelectedIndex = -1;
        }

    }
}
