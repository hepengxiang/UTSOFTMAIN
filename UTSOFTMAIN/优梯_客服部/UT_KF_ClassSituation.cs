using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.优梯_客服部
{
    public partial class UT_KF_ClassSituation : Form
    {
        public UT_KF_ClassSituation()
        {
            InitializeComponent();
        }

        private void UT_KF_ClassSituation_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);


        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
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
            this.textBoxX15.Text = this.dataGridViewX1.SelectedRows[0].Cells[17].Value.ToString();
            this.textBoxX16.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加【" + this.textBoxX1.Text + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into UT_LessonSituation " +
                "values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}','{9}',{10},{11},{12},{13},{14},{15},{16},{17},'{18}',1)",
                this.dateTimeInput1.Value.ToShortDateString(), this.comboBoxEx1.Text.Trim(),tools.FilteSQLStr(this.textBoxX1.Text.Trim()), this.textBoxX2.Text.Trim(), 
                this.comboBoxEx2.Text.Trim(),this.textBoxX3.Text.Trim(),this.textBoxX4.Text.Trim(),this.textBoxX5.Text.Trim(), 
                this.textBoxX6.Text.Trim(), this.textBoxX7.Text.Trim(),this.textBoxX8.Text.Trim(), this.textBoxX9.Text.Trim(), 
                this.textBoxX10.Text.Trim(), this.textBoxX11.Text.Trim(), this.textBoxX12.Text.Trim(),this.textBoxX13.Text.Trim(), 
                this.textBoxX14.Text.Trim(), this.textBoxX15.Text.Trim(), this.textBoxX16.Text.Trim());
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                string sqlFlush = string.Format("select * from UT_LessonSituation where LessonTime = '{0}' and LessonType = '{1}'",
                    this.dateTimeInput1.Value.ToShortDateString(), this.comboBoxEx1.Text.Trim());
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
            string sql1 = string.Format("delete from UT_LessonSituation where LessonTime = '{0}' and LessonType = '{1}'",
                this.dateTimeInput1.Value.ToShortDateString(), this.comboBoxEx1.Text.Trim());
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
                sql1 = string.Format("update UT_LessonSituation set LessonName = '{2}',Teacher = '{3}',ReceivePerson = '{4}',LessonBeforNum1 = {5}," +
                "LessonStartNum1 = {6},LessonStartNum2 = {7},AdvertTime = '{8}',FirstPayTime = '{9}',AddPerson = '{10}',EnterNum = {11},ForecastNum = {12},"+
                "FollowNum = {13},AllEnter = {14},YXAllEnter = {15},YXAllIntent = {16},LessonBuyNum = {17},Summarize = '{18}'" +
                " where LessonTime = '{0}' and LessonType = '{1}'",
                this.dateTimeInput1.Value.ToShortDateString(), this.comboBoxEx1.Text, 
                this.textBoxX1.Text, this.textBoxX2.Text,this.comboBoxEx2.Text, this.textBoxX3.Text, this.textBoxX4.Text,
                this.textBoxX5.Text,this.textBoxX6.Text,this.textBoxX7.Text,this.textBoxX8.Text,
                this.textBoxX9.Text,this.textBoxX10.Text,this.textBoxX11.Text,this.textBoxX12.Text,
                this.textBoxX13.Text,this.textBoxX14.Text,this.textBoxX15.Text,this.textBoxX16.Text);
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
                string sqlFlush = string.Format("select * from UT_LessonSituation where LessonTime = '{0}' and LessonType = '{1}'",
                this.comboBoxEx1.Text.Trim(), this.comboBoxEx1.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from UT_LessonSituation where LessonTime between '{0}' and '{1}' order by LessonTime desc",
                this.dateTimeInput2.Value.ToShortDateString(),this.dateTimeInput3.Value.ToShortDateString());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void comboBoxEx2_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '推广部'");
            string[] columNames = new string[] { "AssumedName"};
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPerson;
            this.comboBoxEx2.ValueMember = "AssumedName";
            this.comboBoxEx2.DisplayMember = "AssumedName";
            this.comboBoxEx2.SelectedIndex = -1;
        }
    }
}
