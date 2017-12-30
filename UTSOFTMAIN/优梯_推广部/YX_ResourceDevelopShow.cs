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
    public partial class YX_ResourceDevelopShow : Form
    {
        public YX_ResourceDevelopShow()
        {
            InitializeComponent();
        }
        public string qqType = "";//QQ类型
        public string groupName = "";//早间 晚间
        //public string addType = "";//后台订购，外部资源，个人CRM
        private void YX_ResourceDevelopShow_Load(object sender, EventArgs e)
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
            this.comboBoxEx6.SelectedIndex = 1;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text.Trim() == "" || this.textBoxX1.Text.Trim() == "0")
            {
                MessageBox.Show("开发总数不能为0或者为空！");
                return;
            }
            if (MessageBox.Show("你确定要增加【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into UT_YXTable_ResourceDevelop (QQType,AddType,GroupName,EnterTime,DevelopCount,FistLanguage,"+
                " CallBackCount,TalkCarefullyCount,FriendCount,ListenLessonCount,PromoteCount,EnterCount,SubmitPerson)" +
                " select '{0}','{1}','{2}','{3}',{4},'{5}',{6},{7},{8},{9},{10},{11},'{12}'",
                qqType, this.comboBoxEx6.Text, groupName, this.dateTimeInput1.Value.ToShortDateString(), 
                this.textBoxX1.Text.Trim(), 
                this.textBoxX2.Text.Trim(), 
                this.textBoxX3.Text.Trim(),
                this.textBoxX4.Text.Trim(), 
                this.textBoxX5.Text.Trim(), 
                this.textBoxX6.Text.Trim(),
                this.textBoxX7.Text.Trim(),
                this.textBoxX8.Text.Trim(),
                frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFlush = string.Format("select a.id,a.QQType,a.AddType,a.GroupName,a.EnterTime,"+
                " datename(weekday, a.EnterTime) as 星期,a.DevelopCount,a.FistLanguage,"+
                " a.CallBackCount,CONVERT(varchar(100), CAST(a.CallBackPercent AS decimal(10,2)))+'%' as CallBackPercent,"+ 
                " a.TalkCarefullyCount, CONVERT(varchar(100), CAST(a.TalkCarefullyPercent AS decimal(10,2)))+'%' as TalkCarefullyPercent,"+ 
                " a.FriendCount, CONVERT(varchar(100), CAST(a.FriendPercent AS decimal(10,2)))+'%' as FriendPercent,"+ 
                " a.ListenLessonCount, CONVERT(varchar(100), CAST(a.ListenLessonPercent AS decimal(10,2)))+'%' as ListenLessonPercent,"+ 
                " a.PromoteCount, CONVERT(varchar(100), CAST(a.PromotePercent AS decimal(10,2)))+'%' as PromotePercent,"+
                " a.EnterCount, CONVERT(varchar(100), CAST(a.EnterPercent AS decimal(10,2)))+'%' as EnterPercent," + 
                " b.AssumedName from UT_YXTable_ResourceDevelop a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.QQType like '%{0}%' and a.AddType like '%{1}%' and a.GroupName like '%{2}%' and" +
                " a.SubmitPerson like '%{3}%' and a.EnterTime = '{4}'",
                qqType, this.comboBoxEx6.Text, groupName, frmUTSOFTMAIN.StaffID, this.dateTimeInput1.Value.ToShortDateString());

                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);

                allDataCaculate();
            }
            else
                MessageBox.Show("增加失败，请检查输入数据！");
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_YXTable_ResourceDevelop where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                allDataCaculate();
            }
            else
                MessageBox.Show("删除失败");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text.Trim() == "" || this.textBoxX1.Text.Trim() == "0")
            {
                MessageBox.Show("开发总数不能为0或者为空！");
                return;
            }
            if (MessageBox.Show("你确定要修改【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_YXTable_ResourceDevelop set "+
                    " EnterTime = '{1}',DevelopCount = {2},FistLanguage = '{3}',CallBackCount = {4}," +
                    " TalkCarefullyCount = {5},FriendCount = {6},ListenLessonCount = {7},"+
                    " PromoteCount = {8},EnterCount = {9},AddType='{10}'" +
                    " where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(), 
                    this.dateTimeInput1.Value.ToShortDateString(),
                    this.textBoxX1.Text.Trim(), 
                    this.textBoxX2.Text.Trim(),
                    this.textBoxX3.Text.Trim(), 
                    this.textBoxX4.Text.Trim(), 
                    this.textBoxX5.Text.Trim(), 
                    this.textBoxX6.Text.Trim(),
                    this.textBoxX7.Text.Trim(),
                    this.textBoxX8.Text.Trim(),
                    this.comboBoxEx6.Text);
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
                string sqlFlush = string.Format("select a.id,a.QQType,a.AddType,a.GroupName,a.EnterTime," +
                " datename(weekday, a.EnterTime) as 星期,a.DevelopCount,a.FistLanguage," +
                " a.CallBackCount,CONVERT(varchar(100), CAST(a.CallBackPercent AS decimal(10,2)))+'%' as CallBackPercent," +
                " a.TalkCarefullyCount, CONVERT(varchar(100), CAST(a.TalkCarefullyPercent AS decimal(10,2)))+'%' as TalkCarefullyPercent," +
                " a.FriendCount, CONVERT(varchar(100), CAST(a.FriendPercent AS decimal(10,2)))+'%' as FriendPercent," +
                " a.ListenLessonCount, CONVERT(varchar(100), CAST(a.ListenLessonPercent AS decimal(10,2)))+'%' as ListenLessonPercent," +
                " a.PromoteCount, CONVERT(varchar(100), CAST(a.PromotePercent AS decimal(10,2)))+'%' as PromotePercent," +
                " a.EnterCount, CONVERT(varchar(100), CAST(a.EnterPercent AS decimal(10,2)))+'%' as EnterPercent," +
                " b.AssumedName from UT_YXTable_ResourceDevelop a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.id = {0}", this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                allDataCaculate();
            }
            else
                MessageBox.Show("修改失败，请检查输入数据！");
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,a.QQType,a.AddType,a.GroupName,a.EnterTime," +
                " datename(weekday, a.EnterTime) as 星期,a.DevelopCount,a.FistLanguage," +
                " a.CallBackCount,CONVERT(varchar(100), CAST(a.CallBackPercent AS decimal(10,2)))+'%' as CallBackPercent," +
                " a.TalkCarefullyCount, CONVERT(varchar(100), CAST(a.TalkCarefullyPercent AS decimal(10,2)))+'%' as TalkCarefullyPercent," +
                " a.FriendCount, CONVERT(varchar(100), CAST(a.FriendPercent AS decimal(10,2)))+'%' as FriendPercent," +
                " a.ListenLessonCount, CONVERT(varchar(100), CAST(a.ListenLessonPercent AS decimal(10,2)))+'%' as ListenLessonPercent," +
                " a.PromoteCount, CONVERT(varchar(100), CAST(a.PromotePercent AS decimal(10,2)))+'%' as PromotePercent," +
                " a.EnterCount, CONVERT(varchar(100), CAST(a.EnterPercent AS decimal(10,2)))+'%' as EnterPercent," +
                " b.AssumedName from UT_YXTable_ResourceDevelop a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.QQType like '%{0}%'  and a.GroupName like '%{1}%' and a.AddType like '%{2}%' and" +
                " b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and b.GroupName like '%{3}%' and " +
                " a.SubmitPerson like '%{4}%' and a.EnterTime between '{5}' and '{6}'",
                qqType, groupName, this.comboBoxEx5.Text, this.comboBoxEx1.Text,this.comboBoxEx2.SelectedValue,
                this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            allDataCaculate();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString());
            this.comboBoxEx6.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
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

        private void allDataCaculate()
        {
            double a1 = 0, a2 = 0, a3 = 0, a4 = 0, a5 = 0, a6 = 0, a7 = 0, a8 = 0, a9 = 0, a10 = 0, a11 = 0, a12 = 0, a13 = 0;
            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {
                a1 += double.Parse(this.dataGridViewX1.Rows[i].Cells[6].Value.ToString());//添加总数
                a2 += double.Parse(this.dataGridViewX1.Rows[i].Cells[8].Value.ToString());//回复人数
                a4 += double.Parse(this.dataGridViewX1.Rows[i].Cells[10].Value.ToString());//细聊人数
                a6 += double.Parse(this.dataGridViewX1.Rows[i].Cells[12].Value.ToString());//添加好友数
                a8 += double.Parse(this.dataGridViewX1.Rows[i].Cells[14].Value.ToString());//引导听课数
                a10 += double.Parse(this.dataGridViewX1.Rows[i].Cells[16].Value.ToString());//意向人数
                a12 += double.Parse(this.dataGridViewX1.Rows[i].Cells[18].Value.ToString());//报名人数
            }
            if (a1 != 0)
            {
                a3 = Math.Round(a2 / a1 * 100, 2);
                a5 = Math.Round(a4 / a1 * 100, 2);
                a7 = Math.Round(a6 / a1 * 100, 2);
                a9 = Math.Round(a8 / a1 * 100, 2);
                a11 = Math.Round(a10 / a1 * 100, 2);
                a13 = Math.Round(a12 / a1 * 100, 2);
            }
            this.labelX15.Text = a1.ToString();
            this.labelX16.Text = a2.ToString();
            this.labelX18.Text = a3.ToString() + "%";
            this.labelX19.Text = a4.ToString();
            this.labelX20.Text = a5.ToString() + "%";
            this.labelX22.Text = a6.ToString();
            this.labelX23.Text = a7.ToString() + "%";
            this.labelX24.Text = a8.ToString();
            this.labelX25.Text = a9.ToString() + "%";
            this.labelX26.Text = a10.ToString();
            this.labelX27.Text = a11.ToString() + "%";
            this.labelX28.Text = a12.ToString();
            this.labelX29.Text = a13.ToString() + "%";
        }
    }
}
