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
    public partial class UT_KF_FYFTable : Form
    {
        public UT_KF_FYFTable()
        {
            InitializeComponent();
        }


        private static string windowText = "";
        private void UT_KF_FYFTable_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            windowText = (sender as UT_KF_FYFTable).Text;
        }
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
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
            if (MessageBox.Show("你确定要增加【" + this.textBoxX1.Text + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into UT_FYFBuy " +
                "values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','未审核','')",
                this.dateTimeInput1.Value.ToShortDateString(), tools.FilteSQLStr(this.textBoxX1.Text.Trim()), this.textBoxX2.Text.Trim(),
                this.comboBoxEx1.Text.Trim(), this.textBoxX3.Text.Trim(), this.textBoxX4.Text.Trim(), frmUTSOFTMAIN.StaffID,System.DateTime.Now);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = this.textBoxX1.Text;
                frmUTSOFTMAIN.OperationRemark = "增加成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select a.id,a.BuyTime,a.QQ,a.PayValue,a.PayOrignal,a.SerialNum,a.Remark,b.AssumedName," +
                " a.EnterTime,a.AuditingState,a.AuditingTime from UT_FYFBuy a left join Users b on a.StaffID = B.StaffID" +
                " where a.QQ = '{0}'", this.textBoxX1.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                this.labelX9.Text = tools.SumValue(dataGridViewX1, 3);
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
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要删除【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_FYFBuy where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "删除成功";
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                this.labelX9.Text = tools.SumValue(dataGridViewX1, 3);
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
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要修改【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_FYFBuy set SerialNum = '{0}', BuyTime = '{1}',QQ = '{2}',PayValue = {3},PayOrignal = '{4}'," +
                "Remark = '{5}' where id = {6}",
                this.textBoxX3.Text, 
                this.dateTimeInput1.Value.ToShortDateString(), 
                this.textBoxX1.Text, 
                this.textBoxX2.Text, 
                this.comboBoxEx1.Text,  
                this.textBoxX4.Text,
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
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
                string operationRemarkStr = string.Format("修改后：时间={0}，QQ = {1}，支付金额={2}，支付渠道 = {3}，交易流水={4}",
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString());
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.dateTimeInput1.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.textBoxX4.Text;
                this.labelX9.Text = tools.SumValue(dataGridViewX1, 3);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,a.BuyTime,a.QQ,a.PayValue,a.PayOrignal,a.SerialNum,a.Remark,b.AssumedName,"+
                " a.EnterTime,a.AuditingState,a.AuditingTime from UT_FYFBuy a left join Users b on a.StaffID = B.StaffID" +
                " where a.BuyTime between '{0}' and '{1}' and a.QQ like '%{2}%'",
                this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString(),this.textBoxX5.Text.Trim());
            if (this.textBoxX6.Text != "")
            {
                sql1 += string.Format(" and PayValue = {0} ", this.textBoxX6.Text.Trim());
            }
            sql1 += " order by a.id desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            this.labelX9.Text = tools.SumValue(dataGridViewX1, 3);
        }
    }
}
