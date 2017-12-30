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
    public partial class TX_ZYTable_EveryDaySummaryShow : Form
    {
        public TX_ZYTable_EveryDaySummaryShow()
        {
            InitializeComponent();
        }
        public string qqType = "";
        public string groupName = "";
        private void YX_EveryDaySummaryShow_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            comboBoxEx2_DropDown(null, null);
            this.comboBoxEx2.SelectedText = frmUTSOFTMAIN.AssumedName;
            this.comboBoxEx2.SelectedValue = frmUTSOFTMAIN.StaffID;
            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" || frmUTSOFTMAIN.UserType == "行政经理")
            {
                this.comboBoxEx2.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("主管"))
            {
                this.comboBoxEx2.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("组长"))
            {
                this.comboBoxEx2.Enabled = true;
            }
            else if (frmUTSOFTMAIN.AssumedName.Contains("阿付"))
            {
                this.comboBoxEx2.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("组长"))
            {
                this.comboBoxEx2.Enabled = true;
            }
            else
            {
                this.comboBoxEx2.Enabled = false;
            }
        }
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into TX_ZYTable_EveryDaySummary values('{0}','{1}','{2}','{3}','{4}')",
                qqType, groupName,this.dateTimeInput1.Value.ToShortDateString(), frmUTSOFTMAIN.StaffID, this.textBoxX1.Text.Trim());
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFlush = string.Format("select a.id, a.EnterTime as 日期, datename(weekday, a.EnterTime) as 星期, a.Content as 内容,b.AssumedName " +
                " from TX_ZYTable_EveryDaySummary a left join Users b on a.Manager = b.StaffID " +
                " where a.EnterTime = '{0}' and a.QQType = '{1}' and a.GroupName = '{2}' and a.Manager = '{3}' order by a.EnterTime desc",
                this.dateTimeInput1.Value.ToShortDateString(),qqType,groupName, frmUTSOFTMAIN.StaffID);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除选中行的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from TX_ZYTable_EveryDaySummary where id = {0}",this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
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
            if (MessageBox.Show("你确定要修改选中行的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update TX_ZYTable_EveryDaySummary set Content = '{1}',EnterTime = '{2}' where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                this.textBoxX1.Text,
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
                string sqlFlush = string.Format("select a.id, a.EnterTime as 日期, datename(weekday, a.EnterTime) as 星期, a.Content as 内容,b.AssumedName " +
                " from TX_ZYTable_EveryDaySummary a left join Users b on a.Manager = b.StaffID " +
                " where id = {0}",
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
            string sql1 = string.Format("select a.id, a.EnterTime as 日期, datename(weekday, a.EnterTime) as 星期, a.Content as 内容,b.AssumedName "+
                " from TX_ZYTable_EveryDaySummary a left join Users b on a.Manager = b.StaffID "+
                " where a.EnterTime between '{0}' and '{1}' and a.QQType = '{2}' and a.GroupName = '{3}' and a.Manager like '%{5}%' order by a.EnterTime desc",
                this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString(), qqType,groupName,"",this.comboBoxEx2.SelectedValue);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void comboBoxEx2_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '智源'");
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPerson;
            this.comboBoxEx2.ValueMember = "StaffID";
            this.comboBoxEx2.DisplayMember = "AssumedName";
            this.comboBoxEx2.SelectedIndex = -1;
        }
    }
}
