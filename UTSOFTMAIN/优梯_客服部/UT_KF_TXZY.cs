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
    public partial class UT_KF_TXZY : Form
    {
        public UT_KF_TXZY()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void UT_KF_TXZY_Load(object sender, EventArgs e)
        {
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput1.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            windowText = (sender as UT_KF_TXZY).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
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
                //insert into Pub_SerialDetail values(购买时间,老QQ,新QQ,交易流水,返款账户,账户名称,联系电话,
                //返款金额,真实返款时间,备注,提交时间,提交人,核实状态,核实时间,核实人)
                sql1 = string.Format("insert into UT_TXZhuanYi values('{0}','{1}','{2}','{3}','{4}','{5}','{6}'," +
                    " {7},{8},'{9}',getdate(),'{11}','{10}','','')",
                this.dateTimeInput1.Value.ToShortDateString(),
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                "",
                "",
                "",
                0,
                0,
                this.textBoxX9.Text.Trim(),
                "",
                frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ"+this.textBoxX1.Text;
                frmUTSOFTMAIN.OperationRemark = sql1;
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select * from UT_TXZhuanYi where OldQQ = '{0}'",
                this.textBoxX1.Text.Trim());
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
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要删除【" + this.dateTimeInput2.Value.ToShortDateString() + "】的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from UT_TXZhuanYi where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
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
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要修改选中的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update UT_TXZhuanYi set " +
                " BuyTime = '{0}', OldQQ = '{1}',NewQQ = '{2}',SerialNum = '{3}',FKAcount = '{4}'," +
                " AcountName = '{5}',Telephone = '{6}',FKValue = {7},TrueFKTime = '{8}',Remark = '{9}',AuditingState = '{10}'" +
                " where id = {11}",
                this.dateTimeInput1.Value.ToShortDateString(),
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                "",
                "",
                "",
                0,
                "",
                this.textBoxX9.Text.Trim(),
                "",
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
                string operationRemarkStr = string.Format("修改后：购买时间={0}，老QQ = {1}，新QQ={2}，交易流水 = {3}，返款账户={4}，"+
                    "账户名称={5}，联系电话={6}，返款金额={7}，真实返款时间={8}，备注={9}，核实状态={10}",
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString(),
                    this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString());
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.dateTimeInput1.Value.ToShortDateString();
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value= this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value= this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value= this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[10].Value= this.textBoxX9.Text;
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from UT_TXZhuanYi where BuyTime between '{0}' and '{1}' and OldQQ like '%{2}%' and SerialNum like '%{3}%'",
                this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString(), 
                this.textBoxX11.Text.Trim(),this.textBoxX12.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {
            if(this.checkBoxX1.Checked)
            {
                this.textBoxX9.Text = "逸芸代付";
            }
        }
    }
}
