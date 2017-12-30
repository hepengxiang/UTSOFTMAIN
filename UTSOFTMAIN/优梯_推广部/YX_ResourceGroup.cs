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
    public partial class YX_ResourceGroup : Form
    {
        public YX_ResourceGroup()
        {
            InitializeComponent();
        }
        public string groupName = "";//早晚间组
        //public string groupType = "";//群类型
        private DataTable dtAllType = new DataTable();
        private void YX_ResourceGroup_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            this.comboBoxEx4.Text = frmUTSOFTMAIN.GroupName;
            comboBoxEx4_SelectedIndexChanged(null, null);
            this.comboBoxEx3.Text = frmUTSOFTMAIN.AssumedName;
            
            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" || frmUTSOFTMAIN.UserType == "行政经理")
            {
                this.comboBoxEx4.Enabled = true;
                this.comboBoxEx3.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("主管"))
            {
                this.comboBoxEx4.Enabled = true;
                this.comboBoxEx3.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("组长"))
            {
                this.comboBoxEx4.Enabled = false;
                this.comboBoxEx3.Enabled = true;
            }
            else
            {
                this.comboBoxEx4.Enabled = false;
                this.comboBoxEx3.Enabled = false;
            }
            this.comboBoxEx5.SelectedIndex = 1;
            string sqlCom = string.Format("select GroupType,GroupID from UT_YXTable_ResourceGroup where GroupName = '{0}'" +
                " and SubmitPerson like '%{1}%' group by GroupType,GroupID", groupName, frmUTSOFTMAIN.StaffID);
            dtAllType = DBHelper.ExecuteQuery(sqlCom);
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加【" + this.dateTimeInput1.Value.ToShortDateString() + "】-【"+this.comboBoxEx1.Text.Trim()+"】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into UT_YXTable_ResourceGroup values('{0}','{1}','{2}','{3}',{4},{5},{6},'{7}','{8}','{9}')",
                    groupName,this.dateTimeInput1.Value.ToShortDateString(), this.comboBoxEx5.Text, this.comboBoxEx1.Text.Trim(),
                    int.Parse(this.textBoxX1.Text.Trim()),int.Parse(this.textBoxX2.Text.Trim()),int.Parse(this.textBoxX3.Text.Trim()),
                    this.textBoxX4.Text.Trim(),this.textBoxX5.Text.Trim(),frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFulsh = string.Format("select a.id,b.AssumedName,a.EnterTime as 日期,a.GroupType as 群类型,a.GroupID as 群名称,"+
                    " a.GroupActiveCount as 群活跃人数," +
                    " a.MessageCount as 会话量,a.FileCount as 相册文件,a.Notice as 公告,a.TalkMessage as 聊天内容" +
                    " from UT_YXTable_ResourceGroup a left join Users b on a.SubmitPerson = b.StaffID "+
                    " where a.EnterTime = '{0}' and a.GroupType = '{1}' and a.GroupID = '{2}' and a.SubmitPerson = '{3}'" +
                    " order by a.EnterTime desc",
                this.dateTimeInput1.Value.ToShortDateString(), this.comboBoxEx5.Text, this.comboBoxEx1.Text, frmUTSOFTMAIN.StaffID);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFulsh);
                string sqlCom = string.Format("select GroupType,GroupID from UT_YXTable_ResourceGroup where GroupName = '{0}'" +
                " and SubmitPerson like '%{1}%' group by GroupType,GroupID", groupName, frmUTSOFTMAIN.StaffID);
                dtAllType = DBHelper.ExecuteQuery(sqlCom);
            }
            else
            {
                MessageBox.Show("增加失败，请检查此记录是否重复录入！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要删除选中行数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_YXTable_ResourceGroup where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                string sqlCom = string.Format("select GroupType,GroupID from UT_YXTable_ResourceGroup where GroupName = '{0}'" +
                " and SubmitPerson like '%{1}%' group by GroupType,GroupID", groupName, frmUTSOFTMAIN.StaffID);
                dtAllType = DBHelper.ExecuteQuery(sqlCom);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要修改选中行的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_YXTable_ResourceGroup set "+
                    "EnterTime = '{1}',GroupID = '{2}',GroupActiveCount = {3},MessageCount = {4},FileCount = {5},Notice = '{6}'," +
                    "TalkMessage = '{7}' where id ={0} ",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(), 
                this.dateTimeInput1.Value.ToShortDateString(),
                this.comboBoxEx1.Text.Trim(),
                int.Parse(this.textBoxX1.Text.Trim()),
                int.Parse( this.textBoxX2.Text.Trim()), 
                int.Parse(this.textBoxX3.Text.Trim()), 
                this.textBoxX4.Text.Trim(), 
                this.textBoxX5.Text.Trim());
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
                string sqlFulsh = string.Format("select a.id,b.AssumedName,a.EnterTime as 日期,a.GroupType as 群类型,a.GroupID as 群名称,"+
                " a.GroupActiveCount as 群活跃人数," +
                " a.MessageCount as 会话量,a.FileCount as 相册文件,a.Notice as 公告,a.TalkMessage as 聊天内容" +
                " from UT_YXTable_ResourceGroup a left join Users b on a.SubmitPerson = b.StaffID where a.id = {0}" +
                " order by a.EnterTime desc",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFulsh);

                string sqlCom = string.Format("select GroupType,GroupID from UT_YXTable_ResourceGroup where GroupName = '{0}'" +
                " and SubmitPerson like '%{1}%' group by GroupType,GroupID", groupName, frmUTSOFTMAIN.StaffID);
                dtAllType = DBHelper.ExecuteQuery(sqlCom);
            }
            else
            {
                MessageBox.Show("修改失败！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,b.AssumedName,a.EnterTime as 日期,a.GroupType as 群类型,a.GroupID as 群名称," +
                " a.GroupActiveCount as 群活跃人数, a.MessageCount as 会话量,a.FileCount as 相册文件,"+
                " a.Notice as 公告,a.TalkMessage as 聊天内容 " +
                " from UT_YXTable_ResourceGroup a left join Users b on a.SubmitPerson = b.StaffID"+
                " where a.EnterTime between '{0}' and '{1}' and a.GroupType like '%{2}%' and a.GroupID like '%{3}%' and"+
                " b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and b.GroupName like '%{4}%' and a.SubmitPerson like '%{5}%'" +
                " order by a.EnterTime desc",
                this.dateTimeInput2.Value.ToShortDateString(), 
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx6.Text, this.comboBoxEx2.Text, 
                this.comboBoxEx4.Text, this.comboBoxEx3.SelectedValue);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部'");
            string[] columNames = new string[] { "GroupName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "GroupName";
            this.comboBoxEx4.DisplayMember = "GroupName";
            this.comboBoxEx4.SelectedIndex = -1;

        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "")
                return;
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部' and GroupName = '{0}'", this.comboBoxEx4.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.DataSource = dtPerson;
            this.comboBoxEx3.ValueMember = "StaffID";
            this.comboBoxEx3.DisplayMember = "AssumedName";
            this.comboBoxEx3.SelectedIndex = -1;
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx5.Text == "")
                return;
            string sql = string.Format("GroupType = '{0}'", this.comboBoxEx5.Text);
            string[] columNames = new string[] { "GroupID" };
            DataTable dtPerson = tools.dtFilter(dtAllType, columNames, sql);
            if (dtPerson.Rows.Count == 0)
                return;
            this.comboBoxEx1.DataSource = dtPerson.Copy();
            this.comboBoxEx1.DisplayMember = "GroupID";
            this.comboBoxEx1.ValueMember = "GroupID";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        private void comboBoxEx6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx6.Text == "")
                return;
            string sql = string.Format("GroupType = '{0}'", this.comboBoxEx6.Text);
            string[] columNames = new string[] { "GroupID" };
            DataTable dtPerson = tools.dtFilter(dtAllType, columNames, sql);
            if (dtPerson.Rows.Count == 0)
            {
                this.comboBoxEx2.Text = "";
                this.comboBoxEx2.DataSource = null;
                return;
            }
            this.comboBoxEx2.DataSource = dtPerson.Copy();
            this.comboBoxEx2.DisplayMember = "GroupID";
            this.comboBoxEx2.ValueMember = "GroupID";
            this.comboBoxEx2.SelectedIndex = -1;
        }
    }
}
