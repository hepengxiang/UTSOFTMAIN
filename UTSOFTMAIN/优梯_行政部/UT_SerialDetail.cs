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
    public partial class UT_SerialDetail : Form
    {
        public UT_SerialDetail()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void TD_SerialDetail_Load(object sender, EventArgs e)
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

            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" ||
                frmUTSOFTMAIN.UserType == "行政总监" || frmUTSOFTMAIN.UserType == "行政经理")
                this.comboBoxEx5.Enabled = true;
            else
                this.comboBoxEx5.Enabled = false;
            windowText = (sender as UT_SerialDetail).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx6.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString();
            this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
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
                sql1 = string.Format("insert into Pub_SerialDetail " +
                "values('{0}','{1}','{2}','{3}',{4},'{5}','{7}','','','','{6}','{10}','{8}','{9}',getdate(),'日常收支')",
                this.comboBoxEx5.Text.Trim(),//公司
                this.dateTimeInput1.Value.ToShortDateString(),//日期
                this.comboBoxEx6.Text.Trim(),//交易类型
                this.comboBoxEx1.Text.Trim(),//收支类型  收入，支出，中转
                this.textBoxX1.Text.Trim(),//金额
                this.comboBoxEx2.Text.Trim(),//事项  5
                this.textBoxX2.Text.Trim(),//交易流水号
                this.textBoxX3.Text.Trim(),//备注
                this.comboBoxEx3.Text.Trim(),//操作负责人
                frmUTSOFTMAIN.StaffID,
                this.textBoxX4.Text.Trim());//提交人
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
                string sqlFlush = string.Format("select * from Pub_SerialDetail where SerialNum = '{0}' and CompanyNames = '{1}'",
                    this.textBoxX2.Text.Trim(),
                    this.comboBoxEx5.Text.Trim());
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
            string sql1 = string.Format("delete from Pub_SerialDetail where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "交易流水号:" + this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
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
                sql1 = string.Format("update Pub_SerialDetail set " +
                    "EnterTime = '{1}',PayOrGet = '{2}',Value = {3},MatterRemark = '{4}'," +
                    "SerialNum = '{5}', GetAccount = '{6}', OperationPerson = '{7}',Remark = '{8}',EnterType = '{9}' where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.dateTimeInput1.Value.ToShortDateString(),
                    this.comboBoxEx1.Text.Trim(),
                    this.textBoxX1.Text.Trim(),
                    this.comboBoxEx2.Text.Trim(),
                    this.textBoxX2.Text.Trim(),
                    this.textBoxX3.Text.Trim(),
                    this.comboBoxEx3.Text.Trim(),
                    this.textBoxX4.Text.Trim(),
                    this.comboBoxEx6.Text.Trim());
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
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.dateTimeInput1.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.comboBoxEx6.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.comboBoxEx2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[12].Value = this.textBoxX4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[13].Value = this.comboBoxEx3.Text;
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Pub_SerialDetail where CompanyNames = '{0}' and TableType = '{1}'" +
                " and EnterTime between '{3}' and '{4}'" +
                " and SerialNum like '%{2}%' and OperationPerson like '%{5}%' and MatterRemark like '%{6}%' "+
                " and Remark like '%{7}%' and EnterType like '%{8}%'" +
                "",
                this.comboBoxEx5.Text.Trim(),
                "日常收支",
                this.textBoxX14.Text.Trim(),
                this.dateTimeInput2.Value.ToShortDateString(), 
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx4.Text,
                this.textBoxX5.Text.Trim(),
                this.textBoxX6.Text.Trim(),
                this.comboBoxEx7.Text.Trim());
            if (this.textBoxX7.Text != "")
            {
                sql1 += string.Format(" and Value = {0} ", this.textBoxX7.Text.Trim());
            }
            sql1 += " order by EnterTime desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据!");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void comboBoxEx3_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", 
                frmUTSOFTMAIN.CompanyNames);
            string[] columNames = new string[] { "AssumedName"};
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.DataSource = dtPerson;
            this.comboBoxEx3.ValueMember = "AssumedName";
            this.comboBoxEx3.DisplayMember = "AssumedName";
            this.comboBoxEx3.SelectedIndex = -1;
        }
        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'",
                frmUTSOFTMAIN.CompanyNames);
            string[] columNames = new string[] { "AssumedName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "AssumedName";
            this.comboBoxEx4.DisplayMember = "AssumedName";
            this.comboBoxEx4.SelectedIndex = -1;
        }
    }
}
