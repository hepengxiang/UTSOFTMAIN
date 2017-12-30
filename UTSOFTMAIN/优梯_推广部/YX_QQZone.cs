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
    public partial class YX_QQZone : Form
    {
        public YX_QQZone()
        {
            InitializeComponent();
        }
        public string groupName = "";
        public string enterType = "";
        private void YX_QQZone_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.rowMergeView1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            this.rowMergeView1.ColumnHeadersHeight = 40;
            this.rowMergeView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


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

        private void rowMergeView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.rowMergeView1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.rowMergeView1.SelectedRows[0].Cells[2].Value.ToString());
            this.textBoxX1.Text = this.rowMergeView1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX2.Text = this.rowMergeView1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX3.Text = this.rowMergeView1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX4.Text = this.rowMergeView1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX5.Text = this.rowMergeView1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX6.Text = this.rowMergeView1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX7.Text = this.rowMergeView1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX8.Text = this.rowMergeView1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX9.Text = this.rowMergeView1.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX10.Text = this.rowMergeView1.SelectedRows[0].Cells[12].Value.ToString();
            this.textBoxX11.Text = this.rowMergeView1.SelectedRows[0].Cells[13].Value.ToString();
            this.textBoxX12.Text = this.rowMergeView1.SelectedRows[0].Cells[14].Value.ToString();
            this.textBoxX13.Text = this.rowMergeView1.SelectedRows[0].Cells[15].Value.ToString();
            this.textBoxX14.Text = this.rowMergeView1.SelectedRows[0].Cells[16].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into UT_YXTable_QQZone "+
                    "values('{16}','{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},'{15}','{17}')",
                    this.dateTimeInput1.Value.ToShortDateString(), enterType,
                    int.Parse(this.textBoxX1.Text.Trim()), 
                    int.Parse(this.textBoxX2.Text.Trim()), 
                    int.Parse(this.textBoxX3.Text.Trim()),
                    int.Parse(this.textBoxX4.Text.Trim()),
                    int.Parse(this.textBoxX5.Text.Trim()),
                    int.Parse(this.textBoxX6.Text.Trim()),
                    int.Parse(this.textBoxX7.Text.Trim()),
                    int.Parse(this.textBoxX8.Text.Trim()),
                    int.Parse(this.textBoxX9.Text.Trim()),
                    int.Parse(this.textBoxX10.Text.Trim()),
                    int.Parse(this.textBoxX11.Text.Trim()),
                    int.Parse(this.textBoxX12.Text.Trim()),
                    int.Parse(this.textBoxX13.Text.Trim()),
                    this.textBoxX14.Text.Trim(),groupName,frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFulsh = string.Format("select a.id,b.AssumedName,a.EnterTime as 日期," +
                " a.SS_SendCount as 说说课程通知发布次数,a.SS_DayView as 说说课程通知浏览量24小时," +
                " a.SS_ZoneEnter as 说说课程通知空间访问数,a.ZMT_SendCount as 自媒体空间提醒@好友数," +
                " a.ZMT_DayView as 自媒体空间提醒浏览量,a.JR_SendCount as 节日彩蛋@好友数," +
                " a.JR_DayView as 节日彩蛋浏览量,a.GH_SendCount as 干货图文@好友数," +
                " a.GH_DayView as 干货图文浏览量,a.GS_SendCount as 公司风采@好友数," +
                " a.GS_DayView as 公司风采浏览量,a.XY_SendCount as 学员案例@好友数," +
                " a.XY_DayView as 学员案例浏览量,a.Remark as 浏览量24小时观看" +
                " from UT_YXTable_QQZone a left join Users b on a.SubmitPerson  = b.StaffID " +
                " where a.EnterTime = '{0}' and a.EnterType = '{1}' and a.GroupName = '{2}'" +
                " and a.SubmitPerson = '{3}'" +
                " order by a.EnterTime desc",
                this.dateTimeInput1.Value.ToShortDateString(),
                enterType, groupName, frmUTSOFTMAIN.StaffID);
                this.rowMergeView1.DataSource = DBHelper.ExecuteQuery(sqlFulsh);
            }
            else
            {
                MessageBox.Show("增加失败，请检查此记录是否重复录入！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.rowMergeView1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要删除选中行的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_YXTable_QQZone where id = {0}",
                this.rowMergeView1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                this.rowMergeView1.Rows.Remove(this.rowMergeView1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.rowMergeView1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要修改选中行的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_YXTable_QQZone set "+
                    " EnterTime = '{1}',SS_SendCount = {2},SS_DayView = {3},SS_ZoneEnter = {4}," +
                    " ZMT_SendCount = {5}, ZMT_DayView = {6},"+
                    " JR_SendCount = {7},JR_DayView = {8},"+
                    " GH_SendCount = {9},GH_DayView = {10},"+
                    " GS_SendCount = {11}, GS_DayView = {12},"+
                    " XY_SendCount = {13},XY_DayView = {14},"+
                    " Remark = '{15}'" +
                    " where id = {0}",
                    this.rowMergeView1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.dateTimeInput1.Value.ToShortDateString(), 
                    int.Parse(this.textBoxX1.Text.Trim()),
                    int.Parse(this.textBoxX2.Text.Trim()),
                    int.Parse(this.textBoxX3.Text.Trim()),
                    int.Parse(this.textBoxX4.Text.Trim()),
                    int.Parse(this.textBoxX5.Text.Trim()),
                    int.Parse(this.textBoxX6.Text.Trim()),
                    int.Parse(this.textBoxX7.Text.Trim()),
                    int.Parse(this.textBoxX8.Text.Trim()),
                    int.Parse(this.textBoxX9.Text.Trim()),
                    int.Parse(this.textBoxX10.Text.Trim()),
                    int.Parse(this.textBoxX11.Text.Trim()),
                    int.Parse(this.textBoxX12.Text.Trim()),
                    int.Parse(this.textBoxX13.Text.Trim()),
                    this.textBoxX14.Text.Trim());
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
                string sqlFulsh = string.Format("select a.id,b.AssumedName,a.EnterTime as 日期," +
                " a.SS_SendCount as 说说课程通知发布次数,a.SS_DayView as 说说课程通知浏览量24小时," +
                " a.SS_ZoneEnter as 说说课程通知空间访问数,a.ZMT_SendCount as 自媒体空间提醒@好友数," +
                " a.ZMT_DayView as 自媒体空间提醒浏览量,a.JR_SendCount as 节日彩蛋@好友数," +
                " a.JR_DayView as 节日彩蛋浏览量,a.GH_SendCount as 干货图文@好友数," +
                " a.GH_DayView as 干货图文浏览量,a.GS_SendCount as 公司风采@好友数," +
                " a.GS_DayView as 公司风采浏览量,a.XY_SendCount as 学员案例@好友数," +
                " a.XY_DayView as 学员案例浏览量,a.Remark as 浏览量24小时观看" +
                " from UT_YXTable_QQZone a left join Users b on a.SubmitPerson  = b.StaffID " +
                " where id = {0}",
                this.rowMergeView1.SelectedRows[0].Cells[0].Value.ToString());
                this.rowMergeView1.DataSource = DBHelper.ExecuteQuery(sqlFulsh);
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,b.AssumedName,a.EnterTime as 日期,"+
                " a.SS_SendCount as 说说课程通知发布次数,a.SS_DayView as 说说课程通知浏览量24小时," +
                " a.SS_ZoneEnter as 说说课程通知空间访问数,a.ZMT_SendCount as 自媒体空间提醒@好友数," +
                " a.ZMT_DayView as 自媒体空间提醒浏览量,a.JR_SendCount as 节日彩蛋@好友数," +
                " a.JR_DayView as 节日彩蛋浏览量,a.GH_SendCount as 干货图文@好友数," +
                " a.GH_DayView as 干货图文浏览量,a.GS_SendCount as 公司风采@好友数," +
                " a.GS_DayView as 公司风采浏览量,a.XY_SendCount as 学员案例@好友数," +
                " a.XY_DayView as 学员案例浏览量,a.Remark as 浏览量24小时观看" +
                " from UT_YXTable_QQZone a left join Users b on a.SubmitPerson  = b.StaffID "+
                " where a.EnterTime between '{0}' and '{1}' and a.EnterType = '{2}' and a.GroupName = '{3}'" +
                " and b.GroupName like '%{4}%' and a.SubmitPerson like '%{5}%'"+
                " order by a.EnterTime desc",
                this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString(), 
                enterType,groupName,this.comboBoxEx1.Text,this.comboBoxEx2.SelectedValue);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.rowMergeView1.DataSource = dt1;
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
