using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTSOFTMAIN.优梯_客服部;

namespace UTSOFTMAIN
{
    public partial class TD_BM_VIPMessageTable : Form
    {
        public TD_BM_VIPMessageTable()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void TD_BM_VIPMessageTable_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx1.DataSource = dtCompany.Copy();
            this.comboBoxEx1.DisplayMember = "CompanyNames";
            this.comboBoxEx1.ValueMember = "CompanyNames";
            this.comboBoxEx1.Text = frmUTSOFTMAIN.CompanyNames;

            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" ||
                frmUTSOFTMAIN.UserType == "行政总监" || frmUTSOFTMAIN.UserType == "行政经理")
                this.comboBoxEx1.Enabled = true;
            else
                this.comboBoxEx1.Enabled = false;
            windowText = (sender as TD_BM_VIPMessageTable).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
            //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString();
            this.textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString();
            this.textBoxX11.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX12.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
            this.textBoxX13.Text = this.dataGridViewX1.SelectedRows[0].Cells[19].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if(
                !PublicMethod.fromUpdatePower(
                "淘大",
                windowText,
                this.dateTimeInput1.Value.ToShortDateString())
               )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要增加报名数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
                //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
                sql1 = string.Format("insert into Pub_VIPMessage values('{0}','{1}','报名','{2}','{3}','{4}','{5}',"+
                "'{6}','{7}','{8}','{9}','','','{10}','{11}',{12},'','{13}','{14}','{15}', getdate())",
                this.comboBoxEx1.Text.Trim(),
                this.dateTimeInput1.Value.ToShortDateString(),
                tools.FilteSQLStr(this.textBoxX1.Text.Trim()),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(),
                this.textBoxX6.Text.Trim(),
                this.textBoxX7.Text.Trim(),
                this.textBoxX8.Text.Trim(),
                this.textBoxX9.Text.Trim(),
                this.textBoxX10.Text.Trim(),
                this.textBoxX11.Text.Trim(),
                this.textBoxX12.Text.Trim(),
                this.textBoxX13.Text.Trim(),
                frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "报名类型 ：" + this.comboBoxEx1.Text.Trim() + "- QQ:" + this.textBoxX1.Text;
                frmUTSOFTMAIN.OperationRemark = "增加成功!";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson  from Pub_VIPMessage a where EnterType = '报名' and QQ = '{0}' and CompanyNames = '{1}'",
                    this.textBoxX2.Text.Trim(),this.comboBoxEx1.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                dateClear();
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (
                    !PublicMethod.fromUpdatePower(
                    "淘大",
                    windowText, 
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要删除选中行的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_VIPMessage where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "报名类型 ：" + this.comboBoxEx1.Text.Trim() + "- QQ:" + 
                    this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
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
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (
                    !PublicMethod.fromUpdatePower(
                    "淘大",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要修改选中行数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update Pub_VIPMessage set "+
                    "Jointime = '{1}', NickName = '{2}',QQ = '{3}',WWNum = '{4}',YYNum = '{5}'," +
                    "YYName = '{6}',QunNumber = '{7}',Name = '{8}',Telephone = '{9}',LTName = '{10}',"+
                    "MeanTeacher = '{11}',PayMoney = {12},SerialNum = '{13}',Remark = '{14}'" +
                    " where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.dateTimeInput1.Value.ToShortDateString(),
                    tools.FilteSQLStr(this.textBoxX1.Text.Trim()),
                    this.textBoxX2.Text.Trim(),
                    this.textBoxX3.Text.Trim(),
                    this.textBoxX4.Text.Trim(),
                    this.textBoxX5.Text.Trim(),
                    this.textBoxX6.Text.Trim(),
                    this.textBoxX7.Text.Trim(),
                    this.textBoxX8.Text.Trim(),
                    this.textBoxX9.Text.Trim(),
                    this.textBoxX10.Text.Trim(),
                    this.textBoxX11.Text.Trim(),
                    this.textBoxX12.Text.Trim(),
                    this.textBoxX13.Text.Trim());
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
                frmUTSOFTMAIN.OperationObject = "修改成功,QQ:" + this.textBoxX2.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = sql1;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value= this.dateTimeInput1.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value= this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value= this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value= this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value= this.textBoxX4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value= this.textBoxX5.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value= this.textBoxX6.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[10].Value= this.textBoxX7.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value= this.textBoxX8.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[14].Value= this.textBoxX9.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[15].Value= this.textBoxX10.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[16].Value= this.textBoxX11.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[18].Value= this.textBoxX12.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[19].Value= this.textBoxX13.Text;
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on b.StaffID = c.StaffID where b.ForID = a.id and b.StaffID = b.AmountManager for xml path('')),1,1,'') as JXPerson  from Pub_VIPMessage a where Jointime between '{1}' and '{2}' " +
                " and CompanyNames = '{0}' and QQ like '%{3}%' and EnterType like '%报名%'",
                this.comboBoxEx1.Text,
                this.dateTimeInput2.Value.ToShortDateString(), 
                this.dateTimeInput3.Value.ToShortDateString(), 
                this.textBoxX14.Text.Trim());
            if (this.textBoxX15.Text != "")
            {
                sql1 += string.Format(" and PayMoney = {0} ", this.textBoxX15.Text.Trim());
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

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx1.DataSource = dtCompany.Copy();
            this.comboBoxEx1.DisplayMember = "CompanyNames";
            this.comboBoxEx1.ValueMember = "CompanyNames";
            this.comboBoxEx1.SelectedIndex = -1;
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
                    "淘大",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                utjxm.updateState = true;
            }
            utjxm.Show();
        }
        private void dateClear() 
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.textBoxX8.Text = "";
            this.textBoxX9.Text = "";
            this.textBoxX10.Text = "";
            this.textBoxX11.Text = "0";
            this.textBoxX12.Text = "";
            this.textBoxX13.Text = "";
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            dateClear();
        }
    }
}
