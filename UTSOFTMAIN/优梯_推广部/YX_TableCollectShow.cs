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
    public partial class YX_TableCollectShow : Form
    {
        public YX_TableCollectShow()
        {
            InitializeComponent();
        }
        public string groupName = "";
        private void YX_TableCollectShow_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            this.comboBoxEx1.Text = frmUTSOFTMAIN.GroupName;
            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" || frmUTSOFTMAIN.UserType == "行政经理")
            {
                this.comboBoxEx1.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("主管"))
            {
                this.comboBoxEx1.Enabled = true;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("组长"))
            {
                this.comboBoxEx1.Enabled = false;
            }
            else
            {
                this.comboBoxEx1.Enabled = false;
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString();
            this.textBoxX11.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            this.textBoxX12.Text = this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString();
            this.textBoxX13.Text = this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString();
            this.textBoxX14.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX15.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
            this.textBoxX16.Text = this.dataGridViewX1.SelectedRows[0].Cells[20].Value.ToString();
        }
        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            string sql1 = string.Format("select a.QQ as QQ, a.NickName as NickName, a.Name as Name, a.Join_time as Join_time,a.YYNum as YYNum,"+
                " a.YYName as YYName, a.Telephone as Telephone, a.MeanTeacher as MeanTeacher, (case when b.Manager is null then '无' else b.Manager end) as Manager, b.Remark as Remark" + 
                " from UT_VIPMessage a left join UT_JXManager b on a.qq = b.qq"+
                " where (b.EnterType = '营销部报名' or b.EnterType is null) and a.Join_time = '{0}'"+
                " order by a.Join_time desc",
                DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString()).ToShortDateString());
            YX_JXManager.tableShow = DBHelper.ExecuteQuery(sql1);
            YX_JXManager.submitBtnShow = false;
            YX_JXManager yxjxm = new YX_JXManager();
            yxjxm.Text = "个人报名";
            yxjxm.Show();
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try{
            sql1 = string.Format("insert into UT_YXTable_TableCollect values"+
                "('{17}','{0}',{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},'{16}','{18}')",
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
                int.Parse(this.textBoxX14.Text.Trim()),
                int.Parse(this.textBoxX15.Text.Trim()),
                this.textBoxX16.Text.Trim(), groupName,
                frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("新增成功！");
                string sqlFlush = string.Format("select a.id,a.GroupName,a.EnterTime as 日期,a.AllBuy as 总订购,a.HBMoning as 湖北早间," +
                " a.HBEveing as 湖北晚间,a.SZBuy as 深圳订购,a.ID179191,a.ID183841,a.ID184021,a.ID179203,a.ID150986," +
                " a.AllListenCount as 听课总数,a.HBTopNum as 湖北最高,a.SZTopNum as 深圳最高,a.AllEnter as 总报名," +
                " a.CourseGet as 课堂抓取,convert(varchar,convert(decimal(10,2),CAST (a.CourseGet as decimal(10,2))/(case when a.AllEnter = 0 then 1 else a.AllEnter  end))*100)+'%' as 抓取率," +
                " FollowEnter as 跟踪报名,convert(varchar,convert(decimal(10,2),CAST (a.FollowEnter as decimal(10,2))/(case when a.AllEnter = 0 then 1 else a.AllEnter  end))*100)+'%' as 占比," +
                " a.Remark as 备注,b.AssumedName from UT_YXTable_TableCollect a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.EnterTime = '{0}' and" +
                " b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and b.GroupName like '%{1}%'" +
                " order by a.EnterTime desc",
                this.dateTimeInput1.Value.ToShortDateString(),
                frmUTSOFTMAIN.GroupName);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                allDataCaculate();
            }
            else 
            {
                MessageBox.Show("新增失败，此日期记录是否已插入？");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要删除选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_YXTable_TableCollect where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                allDataCaculate();
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
            if (MessageBox.Show("你确定要修改选中数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_YXTable_TableCollect set" +
                " AllBuy = {1},HBMoning = {2},HBEveing = {3},SZBuy = {4},ID179191 = {5},ID183841 = {6},ID184021 = {7},ID179203 = {8},ID150986 = {9},"+
                " AllListenCount = {10},HBTopNum = {11},SZTopNum = {12},AllEnter = {13},CourseGet = {14},FollowEnter = {15},Remark = '{16}',EnterTime = '{17}'" +
                " where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
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
                int.Parse(this.textBoxX14.Text.Trim()),
                int.Parse(this.textBoxX15.Text.Trim()),
                this.textBoxX16.Text.Trim(),
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
                MessageBox.Show("修改成功！");
                string sqlFlush = string.Format("select a.id,a.GroupName,a.EnterTime as 日期,a.AllBuy as 总订购,a.HBMoning as 湖北早间," +
                " a.HBEveing as 湖北晚间,a.SZBuy as 深圳订购,a.ID179191,a.ID183841,a.ID184021,a.ID179203,a.ID150986," +
                " a.AllListenCount as 听课总数,a.HBTopNum as 湖北最高,a.SZTopNum as 深圳最高,a.AllEnter as 总报名," +
                " a.CourseGet as 课堂抓取,convert(varchar,convert(decimal(10,2),CAST (a.CourseGet as decimal(10,2))/(case when a.AllEnter = 0 then 1 else a.AllEnter  end))*100)+'%' as 抓取率," +
                " FollowEnter as 跟踪报名,convert(varchar,convert(decimal(10,2),CAST (a.FollowEnter as decimal(10,2))/(case when a.AllEnter = 0 then 1 else a.AllEnter  end))*100)+'%' as 占比," +
                " a.Remark as 备注,b.AssumedName from UT_YXTable_TableCollect a left join Users b on a.SubmitPerson = b.StaffID" +
                " where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                allDataCaculate();
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,a.GroupName,a.EnterTime as 日期,a.AllBuy as 总订购,a.HBMoning as 湖北早间," +
                " a.HBEveing as 湖北晚间,a.SZBuy as 深圳订购,a.ID179191,a.ID183841,a.ID184021,a.ID179203,a.ID150986," +
                " a.AllListenCount as 听课总数,a.HBTopNum as 湖北最高,a.SZTopNum as 深圳最高,a.AllEnter as 总报名," +
                " a.CourseGet as 课堂抓取,convert(varchar,convert(decimal(10,2),"+
                " CAST (a.CourseGet as decimal(10,2))/(case when a.AllEnter = 0 then 1 else a.AllEnter  end))*100)+'%' as 抓取率," +
                " FollowEnter as 跟踪报名,convert(varchar,convert(decimal(10,2),"+
                " CAST (a.FollowEnter as decimal(10,2))/(case when a.AllEnter = 0 then 1 else a.AllEnter  end))*100)+'%' as 占比," +
                " a.Remark as 备注,b.AssumedName from UT_YXTable_TableCollect a left join Users b on a.SubmitPerson = b.StaffID" +
                " where a.EnterTime between '{0}' and '{1}' and " +
                " b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and b.GroupName like '%{2}%'" +
                " order by a.EnterTime desc",
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx1.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            allDataCaculate();
        }
        private void allDataCaculate()
        {
            double a1 = 0, a2 = 0, a3 = 0, a4 = 0, a5 = 0, a6 = 0, a7 = 0, a8 = 0, 
                a9 = 0, a10 = 0, a11 = 0, a12 = 0, a13 = 0, a14 = 0, a15 = 0, a16 = 0, a17 = 0;
            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {

                a1 += double.Parse(this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());//添加总数
                a2 += double.Parse(this.dataGridViewX1.Rows[i].Cells[4].Value.ToString());//通过人数
                a3 += double.Parse(this.dataGridViewX1.Rows[i].Cells[5].Value.ToString());//已存在无效人数
                a4 += double.Parse(this.dataGridViewX1.Rows[i].Cells[6].Value.ToString());//主动加我人数
                a5 += double.Parse(this.dataGridViewX1.Rows[i].Cells[7].Value.ToString());//添加成功总数
                a6 += double.Parse(this.dataGridViewX1.Rows[i].Cells[8].Value.ToString());//添加成功总数
                a7 += double.Parse(this.dataGridViewX1.Rows[i].Cells[9].Value.ToString());//添加成功总数
                a8 += double.Parse(this.dataGridViewX1.Rows[i].Cells[10].Value.ToString());//添加成功总数
                a9 += double.Parse(this.dataGridViewX1.Rows[i].Cells[11].Value.ToString());//添加成功总数
                a10 += double.Parse(this.dataGridViewX1.Rows[i].Cells[12].Value.ToString());//添加成功总数
                a11 += double.Parse(this.dataGridViewX1.Rows[i].Cells[13].Value.ToString());//添加成功总数
                a12 += double.Parse(this.dataGridViewX1.Rows[i].Cells[14].Value.ToString());//添加成功总数
                a13 += double.Parse(this.dataGridViewX1.Rows[i].Cells[15].Value.ToString());//添加成功总数
                a14 += double.Parse(this.dataGridViewX1.Rows[i].Cells[16].Value.ToString());//跟踪报名
                a16 += double.Parse(this.dataGridViewX1.Rows[i].Cells[18].Value.ToString());//添加成功总数
            }
            if (a1 != 0)
            {
                a15 = Math.Round(a14 / a13 * 100, 2);
                a17 = Math.Round(a16 / a13 * 100, 2);
            }
            this.labelX22.Text = a1.ToString();
            this.labelX23.Text = a2.ToString();
            this.labelX24.Text = a3.ToString();
            this.labelX25.Text = a4.ToString();
            this.labelX26.Text = a5.ToString();
            this.labelX27.Text = a6.ToString();
            this.labelX28.Text = a7.ToString();
            this.labelX29.Text = a8.ToString();
            this.labelX30.Text = a9.ToString();
            this.labelX31.Text = a10.ToString();
            this.labelX32.Text = a11.ToString();
            this.labelX33.Text = a12.ToString();
            this.labelX34.Text = a13.ToString();
            this.labelX35.Text = a14.ToString();
            this.labelX36.Text = a15.ToString() + "%";

            this.labelX37.Text = a16.ToString();
            this.labelX38.Text = a17.ToString() + "%";
        }
    }
}
