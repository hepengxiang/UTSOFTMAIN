using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTSOFTMAIN.优梯_客服部;

namespace UTSOFTMAIN.腾讯型分公司
{
    public partial class TX_BM_VIPMessage : Form
    {
        public TX_BM_VIPMessage()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void TX_BM_VIPMessage_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx4.DataSource = dtCompany.Copy();
            this.comboBoxEx4.DisplayMember = "CompanyNames";
            this.comboBoxEx4.ValueMember = "CompanyNames";
            this.comboBoxEx4.Text = frmUTSOFTMAIN.CompanyNames;

            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" ||
                frmUTSOFTMAIN.UserType == "行政总监" || frmUTSOFTMAIN.UserType == "行政经理")
                this.comboBoxEx4.Enabled = true;
            else
                this.comboBoxEx4.Enabled = false;
            windowText = (sender as TX_BM_VIPMessage).Text;
        }
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            this.comboBoxEx5.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[19].Value.ToString();
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (
                !PublicMethod.fromUpdatePower(
                "腾讯",
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
                //insert into Pub_VIPMessage values(id,{0},{1},{2},{3},{4},旺旺号,YY号,
                //YY昵称,{5},{6},{7},预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
                sql1 = string.Format("insert into Pub_VIPMessage values('{0}','{1}','{2}','{3}','{4}','',''," +
                "'','{5}','{6}','{7}','','{8}','','',{9},'','{10}','{11}','{12}',getdate())",
                this.comboBoxEx4.Text.Trim(), this.dateTimeInput1.Value.ToShortDateString(),
                this.comboBoxEx1.Text.Trim(), tools.FilteSQLStr(this.textBoxX1.Text.Trim()),
                this.textBoxX2.Text.Trim(), this.textBoxX3.Text.Trim(), this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(), this.comboBoxEx2.Text.Trim(), this.comboBoxEx5.Text.Trim(), 
                this.textBoxX7.Text.Trim(), this.textBoxX8.Text.Trim(), frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "报名类型 ：" + this.comboBoxEx1.Text.Trim() + "- QQ:" + this.textBoxX2.Text;
                frmUTSOFTMAIN.OperationRemark = "增加成功!";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c "+
                    "on b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') "+
                    "as JXPerson  from Pub_VIPMessage a where CompanyNames = '{0}' and QQ = '{1}' "+
                    "and charindex('报名',EnterType)>0",
                    this.comboBoxEx4.Text.Trim(), this.textBoxX2.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
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
                    "腾讯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要删除选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_VIPMessage where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ ：" + this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "删除成功!";
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
            }
            else
            {
                MessageBox.Show("删除失败,请先删除此QQ对应的绩效归属人！");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (
                    !PublicMethod.fromUpdatePower(
                    "腾讯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (this.dataGridViewX1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选中需要修改的记录！");
                return;
            }
            if (MessageBox.Show("你确定要修改选中行的数据吗？", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                //EnterType = '{0}',Jointime = '{2}',报名类型和时间不允许修改
                sql1 = string.Format("update Pub_VIPMessage set NickName = '{1}', QQ = '{2}',QunNumber = '{3}'," +
                "Name = '{4}',Telephone = '{5}',PayOringal = '{6}',PayMoney = {7},SerialNum = '{8}',Remark = '{9}'" +
                "where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                this.textBoxX1.Text.Trim(), this.textBoxX2.Text.Trim(),this.textBoxX3.Text.Trim(),
                this.comboBoxEx2.Text.Trim(),this.textBoxX4.Text.Trim(),this.textBoxX5.Text.Trim(),
                this.comboBoxEx5.Text.Trim(), this.textBoxX7.Text.Trim(), this.textBoxX8.Text.Trim());
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
                frmUTSOFTMAIN.OperationObject = "修改成功,QQ:" + this.textBoxX2.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = sql1;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.textBoxX1.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.textBoxX2.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value = this.textBoxX3.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[10].Value = this.textBoxX4.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value = this.textBoxX5.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[13].Value = this.comboBoxEx2.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[16].Value = this.comboBoxEx5.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[18].Value = this.textBoxX7.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[19].Value = this.textBoxX8.Text.Trim();
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c "+
                " on b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') "+
                " as JXPerson  from Pub_VIPMessage a where CompanyNames = '{0}' and " +
                " Jointime between '{1}' and '{2}' and EnterType like '%{3}%' and charindex('报名',EnterType)>0 and QQ like '%{4}%'",
                this.comboBoxEx4.Text.Trim(),
                this.dateTimeInput2.Value.ToShortDateString(), this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx3.Text.Trim(), this.textBoxX9.Text.Trim());
            if (this.textBoxX6.Text != "")
            {
                sql1 += string.Format(" and PayMoney = {0} ",this.textBoxX6.Text.Trim());
            }
            sql1 += " order by Jointime desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            UT_KF_YDBMJXManager utjxm = new UT_KF_YDBMJXManager();
            utjxm.forID = int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            utjxm.jxTime = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            utjxm.enterType = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            utjxm.companyNames = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            string sqlDTSource = string.Format("select a.id,c.QQ,c.NickName,b.DepartmentName,b.AssumedName,a.StaffID,a.EnterTypeSmall,a.Value,a.Remark" +
                    " from Pub_YDBM_JXManager a left join Users b on a.StaffID = b.StaffID left join Pub_VIPMessage c on a.ForID = c.id" +
                    " where a.StaffID = a.AmountManager and a.ForID = {0}",
                    int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString()));
            utjxm.dtSource = DBHelper.ExecuteQuery(sqlDTSource);
            if (
                    PublicMethod.fromUpdatePower(
                    "腾讯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                utjxm.updateState = true;
            }
            utjxm.Show();
        }

        private void comboBoxEx5_DropDown(object sender, EventArgs e)
        {
            this.comboBoxEx5.Items.Clear();
            if(this.comboBoxEx4.Text == "智源")
            {
                this.comboBoxEx5.Items.Add("700");
                this.comboBoxEx5.Items.Add("1280");
                this.comboBoxEx5.Items.Add("1480");
                this.comboBoxEx5.Items.Add("1580");
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "智源")
            {
                if (this.comboBoxEx1.Text == "升级报名")
                {
                    this.comboBoxEx5.Text = "700";
                }
                else 
                {
                    this.comboBoxEx5.Text = "1580";
                }
            }
        }
    }
}
