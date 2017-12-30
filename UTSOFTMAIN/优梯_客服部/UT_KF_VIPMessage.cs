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
    public partial class UT_KF_VIPMessage : Form
    {
        public UT_KF_VIPMessage()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void UT_KF_VIPMessage_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            windowText = (sender as UT_KF_VIPMessage).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
            //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            this.comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
            this.textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells[19].Value.ToString();
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
            if (this.textBoxX2.Text.Trim()==""|| this.comboBoxEx1.Text=="")
            {
                MessageBox.Show("QQ号码和报名类型不能为空");
                return;
            }
            string qqcheck = string.Format("select 1 from Pub_VIPMessage where CompanyNames = '优梯' and EnterType = '{0}' and QQ = '{1}'",
                this.comboBoxEx1.Text.Trim(),this.textBoxX2.Text.Trim());
            DataTable dtqqcheck = DBHelper.ExecuteQuery(qqcheck);
            if(dtqqcheck.Rows.Count>0)
            {
                MessageBox.Show("此QQ的此报名类型的记录在软件中已存在！");
                return;
            }
            if (MessageBox.Show("你确定要增加【" + this.textBoxX1.Text + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
                //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
                //insert into Pub_VIPMessage values(优梯,'{0}','{1}','{2}','{3}',旺旺号,'{4}',
                //'{5}','{6}','{7}','{8}',预定老师,'{9}',论坛名称,'{13}',{10},返款支付宝,'{11}','{12}','{14}',getdate())
                sql1 = string.Format("insert into Pub_VIPMessage values('优梯','{0}','{1}','{2}','{3}','无','{4}'," +
                " '{5}','{6}','{7}','{8}','无','{9}','无','{13}',{10},'无','{11}','{12}','{14}',getdate())",
                this.dateTimeInput1.Value.ToShortDateString(), 
                this.comboBoxEx1.Text.Trim(),tools.FilteSQLStr(this.textBoxX1.Text.Trim()), 
                this.textBoxX2.Text.Trim(), this.textBoxX3.Text.Trim(),this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(), this.textBoxX6.Text.Trim(), this.textBoxX7.Text.Trim(),
                this.comboBoxEx2.Text.Trim(), this.textBoxX8.Text.Trim(), this.textBoxX9.Text.Trim(),
                 this.textBoxX10.Text.Trim(), this.comboBoxEx4.Text.Trim(), frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据格式有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                if (this.comboBoxEx1.Text.Trim().Contains("报名"))
                {
                    if (this.dateTimeInput1.Value > DateTime.Now.AddDays(-3)) 
                    {
                        string sqlCheck = string.Format("select * from UT_YFVisit where QQ = '{0}'", this.textBoxX2.Text.Trim());
                        DataTable dtcheck = DBHelper.ExecuteQuery(sqlCheck);
                        if (dtcheck.Rows.Count == 0) 
                        {
                            //插入研发跟踪信息
                            string sqlInsertYFVisit = string.Format("insert into UT_YFVisit values('{0}','','','','','','')", this.textBoxX2.Text.Trim());
                            DBHelper.ExecuteUpdate(sqlInsertYFVisit);
                        }
                    }
                    string sqlFlush = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson  from Pub_VIPMessage a where EnterType = '{0}' and QQ = '{1}' and CompanyNames = '优梯'",
                        this.comboBoxEx1.Text.Trim(), this.textBoxX2.Text.Trim());
                    this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                }
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "报名类型 ：" + this.comboBoxEx1.Text.Trim() + "- QQ:" + this.textBoxX1.Text;
                frmUTSOFTMAIN.OperationRemark = "增加成功!";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                dataClear();
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
                    "优梯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                MessageBox.Show("不允许操作本数据！请联系管理员");
                return;
            }
            if (MessageBox.Show("你确定要删除【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_VIPMessage where id = '{0}'",
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
            if (this.dataGridViewX1.SelectedRows.Count == 0) 
            {
                MessageBox.Show("请先选中需要修改的记录！");
                return;
            }
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
            if (MessageBox.Show("你确定要修改【" + this.dateTimeInput1.Value.ToShortDateString() + "】的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                //EnterType = '{0}',Jointime = '{2}',报名类型和时间不允许修改
                sql1 = string.Format("update Pub_VIPMessage set QQ = '{1}', NickName = '{3}',QunNumber = '{4}',YYName = '{5}'," +
                "YYNum = '{6}',Name = '{7}',Telephone = '{8}',PayOringal = '{9}',PayMoney = {10},SerialNum = '{11}',Remark = '{12}',MeanTeacher = '{13}' "+
                "where id = {14}",
                this.comboBoxEx1.Text.Trim(), this.textBoxX2.Text.Trim(), 
                this.dateTimeInput1.Value.ToShortDateString(),this.textBoxX1.Text.Trim(), this.textBoxX5.Text.Trim(),
                this.textBoxX4.Text.Trim(), this.textBoxX3.Text.Trim(),this.textBoxX6.Text.Trim(), this.textBoxX7.Text.Trim(),this.comboBoxEx2.Text.Trim(),
                this.textBoxX8.Text.Trim(), this.textBoxX9.Text.Trim(), this.textBoxX10.Text.Trim(), this.comboBoxEx4.Text.Trim(),
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
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ ：" + this.textBoxX2.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = sql1;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select * from Pub_VIPMessage where EnterType = '{0}' and QQ = '{1}' and CompanyNames = '优梯'", 
                    this.comboBoxEx1.Text.Trim(),this.textBoxX2.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            if (this.textBoxX12.Text.Trim() == "" && this.textBoxX11.Text.Trim() == "") 
            {
                sql1 = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on "+
                    "b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson  "+
                    "from Pub_VIPMessage a where Jointime between '{0}' and '{1}' " +
                " and CompanyNames = '优梯' and EnterType like '%{3}%'",
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.textBoxX12.Text.Trim(),
                this.comboBoxEx3.Text.Trim(),
                this.textBoxX11.Text.Trim());  
            }
            else if (this.textBoxX12.Text.Trim() != "" && this.textBoxX11.Text.Trim() == "") 
            {
                sql1 = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on "+
                    "b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson  "+
                    "from Pub_VIPMessage a where CompanyNames = '优梯' and QQ like '%{2}%' and SerialNum like '%{4}%'",
                this.dateTimeInput2.Value.ToShortDateString(), 
                this.dateTimeInput3.Value.ToShortDateString(), 
                this.textBoxX12.Text.Trim(), 
                this.comboBoxEx3.Text.Trim(),
                 this.textBoxX11.Text.Trim());
            }
            else if (this.textBoxX12.Text.Trim() == "" && this.textBoxX11.Text.Trim() != "")
            {
                sql1 = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on " +
                    "b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson  " +
                    "from Pub_VIPMessage a where CompanyNames = '优梯' and SerialNum like '%{4}%'",
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.textBoxX12.Text.Trim(),
                this.comboBoxEx3.Text.Trim(),
                 this.textBoxX11.Text.Trim());
            }
            else 
            {
                sql1 = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_YDBM_JXManager b left join Users c on " +
                    "b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson  " +
                    "from Pub_VIPMessage a where CompanyNames = '优梯' and QQ like '%{2}%' and SerialNum like '%{4}%'",
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.textBoxX12.Text.Trim(),
                this.comboBoxEx3.Text.Trim(),
                 this.textBoxX11.Text.Trim());
            }
            if (this.textBoxX13.Text != "")
            {
                sql1 += string.Format(" and PayMoney = {0} ", this.textBoxX13.Text.Trim());
            }
            sql1 += " order by Jointime desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            this.labelX20.Text = tools.SumValue(dataGridViewX1,16);
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            UT_KF_YDBMJXManager utjxm = new UT_KF_YDBMJXManager();
            utjxm.forID =int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            utjxm.jxTime = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            utjxm.enterType = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            string sqlDTSource = string.Format("select a.id,c.QQ,c.NickName,b.DepartmentName,b.AssumedName,a.StaffID,a.EnterTypeSmall,a.Value,a.Remark" +
                    " from Pub_YDBM_JXManager a left join Users b on a.StaffID = b.StaffID left join Pub_VIPMessage c on a.ForID = c.id" +
                    " where a.StaffID = a.AmountManager and a.ForID = {0}",
                    int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString()));
            utjxm.dtSource = DBHelper.ExecuteQuery(sqlDTSource);
            if (
                    PublicMethod.fromUpdatePower(
                    "优梯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                utjxm.updateState = true;
            }
            utjxm.Show();
        }
        private void dataClear() 
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.comboBoxEx1.Text = "";
            this.comboBoxEx2.Text = "";
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
            this.textBoxX6.Text = "";
            this.textBoxX7.Text = "";
            this.textBoxX8.Text = "0";
            this.textBoxX9.Text = "";
            this.textBoxX10.Text = "";
        }
        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '研发部'");
            string[] columNames = new string[] { "AssumedName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "AssumedName";
            this.comboBoxEx4.DisplayMember = "AssumedName";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            dataClear();
        }

    }
}
