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
    public partial class Admin_CardSerialDetail : Form
    {
        public Admin_CardSerialDetail()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void Admin_CardSerialDetail_Load(object sender, EventArgs e)
        {
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            this.comboBoxEx1.Text = frmUTSOFTMAIN.CompanyNames;
            //this.comboBoxEx1.Enabled = false;

            windowText = (sender as Admin_CardSerialDetail).Text;

            cardvalueflush();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.comboBoxEx5.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.comboBoxEx6.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.comboBoxEx7.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            //this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
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
                sql1 += string.Format(" insert into Pub_SerialDetail " +
                "values('{0}','{1}','{2}','{3}',{4},'{5}','{6}','','','','','','{8}','{9}',getdate(),'日常收支')",
                this.comboBoxEx1.Text.Trim(),//公司
                this.dateTimeInput1.Value.ToShortDateString(),//日期
                this.comboBoxEx3.Text.Trim(),//交易类型
                this.comboBoxEx5.Text.Trim(),//收支类型
                this.textBoxX1.Text.Trim(),//金额
                this.comboBoxEx2.Text.Trim() + "-" + this.comboBoxEx4.Text.Trim(),//事项
                this.comboBoxEx7.Text.Trim(),//对方账号
                this.textBoxX3.Text.Trim(),//备注
                this.comboBoxEx6.Text.Trim(),//操作负责人
                frmUTSOFTMAIN.StaffID);

                sql1 += string.Format(" insert into Admin_CardSerialDetail " +
                "values('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}',{9},'{10}',IDENT_CURRENT('Pub_SerialDetail'))",
                this.comboBoxEx1.Text.Trim(),//公司
                this.dateTimeInput1.Value.ToShortDateString(),//日期
                this.comboBoxEx2.Text.Trim(),//卡类型
                this.comboBoxEx3.Text.Trim(),//交易类型
                this.comboBoxEx4.Text.Trim(),//款项类型
                this.comboBoxEx5.Text.Trim(),//收支类型
                this.textBoxX1.Text.Trim(),//金额
                this.comboBoxEx6.Text.Trim(),//操作负责人
                this.comboBoxEx7.Text.Trim(),//对方账号
                "0",//当前余额
                this.textBoxX3.Text.Trim());//备注
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");

                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "金额:" + this.textBoxX1.Text.Trim() + "卡类型:" + this.comboBoxEx2.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "增加成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select * from Admin_CardSerialDetail where CompanyNames = '{0}' and EnterTime = '{1}' and CardType = '{2}'" +
                    " and SerialType = '{3}' and PayType = '{4}' and PayOrGet = '{5}' and value = {6} and OperationPerson = '{7}' and OtherAccount = '{8}'" +
                    " and Remark = '{9}'",
                    this.comboBoxEx1.Text.Trim(),//公司
                    this.dateTimeInput1.Value.ToShortDateString(),//日期
                    this.comboBoxEx2.Text.Trim(),//卡类型
                    this.comboBoxEx3.Text.Trim(),//交易类型
                    this.comboBoxEx4.Text.Trim(),//款项类型
                    this.comboBoxEx5.Text.Trim(),//收支类型
                    this.textBoxX1.Text.Trim(),//金额
                    this.comboBoxEx6.Text.Trim(),//操作负责人
                    this.comboBoxEx7.Text.Trim(),//对方账号
                    this.textBoxX3.Text.Trim());//备注);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                cardvalueflush();
                dataClear();
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
            string sql1 = string.Format("delete from Admin_CardSerialDetail where id = {0} ",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            sql1 += string.Format("delete from Pub_SerialDetail where id = {0} ",
                this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "公司:" + this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "删除成功";
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                cardvalueflush();
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
                sql1 += string.Format("update Admin_CardSerialDetail set " +
                    "CompanyNames = '{1}',EnterTime = '{2}',CardType = '{3}',SerialType = '{4}'," +
                    "PayType = '{5}', PayOrGet = '{6}', value = {7},OperationPerson = '{8}'," +
                    "OtherAccount = '{9}',ValueNow = {10},Remark = '{11}' where id = {0} ",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.comboBoxEx1.Text.Trim(),//公司
                    this.dateTimeInput1.Value.ToShortDateString(),//日期
                    this.comboBoxEx2.Text.Trim(),//卡类型
                    this.comboBoxEx3.Text.Trim(),//交易类型
                    this.comboBoxEx4.Text.Trim(),//款项类型
                    this.comboBoxEx5.Text.Trim(),//收支类型
                    this.textBoxX1.Text.Trim(),//金额
                    this.comboBoxEx6.Text.Trim(),//操作负责人
                    this.comboBoxEx7.Text.Trim(),//对方账号
                    "0",//当前余额
                    this.textBoxX3.Text.Trim());//备注
                sql1 += string.Format("update Pub_SerialDetail set CompanyNames = '{1}',EnterTime = '{2}', EnterType = '{3}',PayOrGet = '{4}',"+
                    " Value = {5}, MatterRemark = '{6}', GetAccount = '{7}', Remark = '{8}', OperationPerson = '{9}' where id = {0} ",
                    this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString(),
                    this.comboBoxEx1.Text.Trim(),//公司
                    this.dateTimeInput1.Value.ToShortDateString(),//日期
                    this.comboBoxEx3.Text.Trim(),//交易类型
                    this.comboBoxEx5.Text.Trim(),//收支类型
                    this.textBoxX1.Text.Trim(),//金额
                    this.comboBoxEx2.Text.Trim() + "-" + this.comboBoxEx4.Text.Trim(),//事项
                    this.comboBoxEx7.Text.Trim(),//对方账号
                    this.textBoxX3.Text.Trim(),//备注
                    this.comboBoxEx6.Text.Trim());//操作负责人
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
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.dateTimeInput1.Value.ToShortDateString();
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.comboBoxEx2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.comboBoxEx3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.comboBoxEx4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.comboBoxEx5.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value = this.comboBoxEx6.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value = this.comboBoxEx7.Text;
                //this.dataGridViewX1.SelectedRows[0].Cells[10].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value = this.textBoxX3.Text;
                cardvalueflush();
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Admin_CardSerialDetail where CompanyNames = '{0}' and EnterTime between '{1}' and '{2}' and"+
                " CardType like '%{3}%' and SerialType like '%{4}%' and PayType like '%{5}%' and PayOrGet like '%{6}%' and OperationPerson like '%{7}%'"+
                " and OtherAccount like '%{8}%' and Remark like '%{9}%'",
                this.comboBoxEx1.Text.Trim(),
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput1.Value.ToShortDateString(),
                this.comboBoxEx2.Text.Trim(),
                this.comboBoxEx3.Text.Trim(),
                this.comboBoxEx4.Text.Trim(),
                this.comboBoxEx5.Text.Trim(),
                this.comboBoxEx6.Text.Trim(),
                this.comboBoxEx7.Text.Trim(),
                this.textBoxX3.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if(dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到记录");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            cardvalueflush();
        }

        private void cardvalueflush() 
        {
            string sqlJJKBalance = string.Format("select CompanyNames, CardType, " +
                " sum(case PayOrGet when '收入' then value when '支出' then -value else 0 end) as balance " +
                " from Admin_CardSerialDetail where EnterTime<='{0}' and CompanyNames = '优梯' group by CompanyNames,CardType",
                this.dateTimeInput1.Value.ToShortDateString());
            DataTable dbJJKBalance = DBHelper.ExecuteQuery(sqlJJKBalance);

            string sqlcol = string.Format("CompanyNames = '优梯'");
            string[] columNames = new string[] { "balance" };
            DataTable dtbalance = tools.dtFilter(dbJJKBalance, columNames, sqlcol);
            if (dtbalance.Rows.Count == 0)
            {
                this.labelX10.Text = "基金卡余额：0";
                this.labelX13.Text = "运营卡余额：0";
                return;
            }
            try { this.labelX10.Text = "基金卡余额：" + dtbalance.Rows[0][0].ToString(); }
            catch { this.labelX10.Text = "基金卡余额：0"; }
            try { this.labelX13.Text = "运营卡余额：" + dtbalance.Rows[1][0].ToString(); }
            catch { this.labelX13.Text = "运营卡余额：0"; }
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

        private void comboBoxEx2_TextChanged(object sender, EventArgs e)
        {
            if(this.comboBoxEx2.Text == "")
            {
                this.comboBoxEx2.Text = "";
                this.comboBoxEx3.Text = "";
                this.comboBoxEx4.Text = "";
                this.comboBoxEx5.Text = "";
                this.textBoxX1.Text = "0";
                this.comboBoxEx6.Text = "";
                this.comboBoxEx7.Text = "";
                this.textBoxX3.Text = "";
            }
        }

        private void comboBoxEx3_TextChanged(object sender, EventArgs e)
        {
            this.comboBoxEx4.Text = "";
            this.comboBoxEx5.Text = "";
            this.textBoxX1.Text = "0";
            this.comboBoxEx6.Text = "";
            this.comboBoxEx7.Text = "";
            this.textBoxX3.Text = "";
        }
        private void dataClear() 
        {
            this.comboBoxEx2.Text = "";
            this.comboBoxEx3.Text = "";
            this.comboBoxEx4.Text = "";
            this.comboBoxEx5.Text = "";
            this.textBoxX1.Text = "0";
            this.comboBoxEx6.Text = "";
            this.comboBoxEx7.Text = "";
            this.textBoxX3.Text = "";
        }
    }
}
