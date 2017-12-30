using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.优梯_行政部
{
    public partial class Admin_AcountDetailCount : Form
    {
        public Admin_AcountDetailCount()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void Admin_AcountDetailCount_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx5.DataSource = dtCompany.Copy();
            this.comboBoxEx5.DisplayMember = "CompanyNames";
            this.comboBoxEx5.ValueMember = "CompanyNames";
            this.comboBoxEx5.Text = frmUTSOFTMAIN.CompanyNames;
            windowText = (sender as Admin_AcountDetailCount).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx5.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx6.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (
                    !PublicMethod.fromUpdatePower(
                    "优梯",
                    windowText,
                    this.dateTimeInput1.Value.ToShortDateString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要增加数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into Pub_AcountDetailCount " +
                "values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}',getdate(),'{8}')",
                this.comboBoxEx5.Text.Trim(),//公司
                this.dateTimeInput1.Value.ToShortDateString(),//日期
                this.comboBoxEx6.Text.Trim(),//交易类型
                this.textBoxX1.Text.Trim(),//金额
                this.textBoxX2.Text.Trim(),//交易流水号
                this.textBoxX3.Text.Trim(),//备注
                this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(),
                frmUTSOFTMAIN.StaffID);//提交人
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "金额:" + this.textBoxX1.Text.Trim() + "交易流水号:" + this.textBoxX2.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "增加成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select * from Pub_AcountDetailCount where CompanyNames = '{0}'and EnterTime = '{1}' and SerialType = '{2}'",
                    this.comboBoxEx5.Text.Trim(),
                    this.dateTimeInput1.Value.ToShortDateString(),
                    this.comboBoxEx6.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (
                    !PublicMethod.fromUpdatePower(
                    "优梯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要删除选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_AcountDetailCount where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "转账类型:" + this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "删除成功";
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (
                    !PublicMethod.fromUpdatePower(
                    "优梯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要修改选中数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update Pub_AcountDetailCount set " +
                    "CompanyNames = '{1}',EnterTime = '{2}',SerialType = '{3}',Value = {4}," +
                    "GetAcount = '{5}', PayAcount = '{6}', TransitAcount = '{7}',Remark = '{8}' where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.comboBoxEx5.Text.Trim(),
                    this.dateTimeInput1.Value.ToShortDateString(),
                    this.comboBoxEx6.Text.Trim(),
                    this.textBoxX1.Text.Trim(),
                    this.textBoxX2.Text.Trim(),
                    this.textBoxX3.Text.Trim(),
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
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "修改成功";
                frmUTSOFTMAIN.OperationRemark = sql1;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.comboBoxEx5.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.dateTimeInput1.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.comboBoxEx6.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value = this.textBoxX4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value = this.textBoxX5.Text;
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Pub_AcountDetailCount where CompanyNames like '%{0}%' and EnterTime between '{1}' and '{2}'" +
                "and SerialType like '%{3}%' order by EnterTime desc",
                this.comboBoxEx1.Text.Trim(),this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx4.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据!");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void comboBoxEx5_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx5.DataSource = dtCompany.Copy();
            this.comboBoxEx5.DisplayMember = "CompanyNames";
            this.comboBoxEx5.ValueMember = "CompanyNames";
            this.comboBoxEx5.SelectedIndex = -1;
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx1.DataSource = dtCompany.Copy();
            this.comboBoxEx1.DisplayMember = "CompanyNames";
            this.comboBoxEx1.ValueMember = "CompanyNames";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        private void comboBoxEx6_DropDown(object sender, EventArgs e)
        {
            string sql1 = string.Format("select SerialType from Pub_AcountDetailCount "+
                "where CompanyNames like '%{0}%' group by SerialType", 
                this.comboBoxEx5.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
                return;
            this.comboBoxEx6.DataSource = dt1;
            this.comboBoxEx6.DisplayMember = "SerialType";
            this.comboBoxEx6.ValueMember = "SerialType";
            this.comboBoxEx6.SelectedIndex = -1;
        }

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string sql1 = string.Format("select SerialType from Pub_AcountDetailCount " +
                "where CompanyNames like '%{0}%' group by SerialType",
                this.comboBoxEx1.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
                return;
            this.comboBoxEx4.DataSource = dt1;
            this.comboBoxEx4.DisplayMember = "SerialType";
            this.comboBoxEx4.ValueMember = "SerialType";
            this.comboBoxEx4.SelectedIndex = -1;
        }
    }
}
