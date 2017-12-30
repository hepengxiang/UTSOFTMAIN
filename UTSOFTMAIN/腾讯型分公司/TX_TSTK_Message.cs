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
    public partial class TX_TSTK_Message : Form
    {
        public TX_TSTK_Message()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void TX_TSTK_Message_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput4.Value = System.DateTime.Now;
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
            windowText = (sender as TX_TSTK_Message).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.dateTimeInput2.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString());
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            if (this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString().Contains("退款"))
            {
                this.buttonX5.Enabled = false;
                this.buttonX6.Enabled = true;
            }
            else
            {
                this.buttonX5.Enabled = true;
                this.buttonX6.Enabled = false;
            }
            //this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            //this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString();
            //this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString();
            //this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            //this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[17].Value.ToString();
            //this.textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells[22].Value.ToString();
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
                //string.Format("insert into Pub_ComplainRefundInfo " +
                //values(所属公司,日期,提交类型,QQ,报名日期,QQ昵称, 旺旺号,联系电话,学员情况,信息备注,录入人,录入时间,处理结果,退款账户,退款账户姓名,退款金额"+
                //退款原因,处理备注,处理人,处理时间,行政退款状态,退款交易流水号,退款备注,退款人,退款时间,核实状态,核实人,核实时间)",
                //sql1 = string.Format("insert into Pub_ComplainRefundInfo values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',信息备注," +
                //    "'{14}',getdate(),'{8}','{9}','{10}',{11},'{12}',处理备注,'{14}','{1}',行政退款状态,'{13}',退款备注,退款人,退款时间,核实状态,核实人,核实时间)",
                sql1 = string.Format("insert into Pub_ComplainRefundInfo values('{0}','{1}','{2}','{3}','{4}','{5}','','{6}','{7}','信息备注'," +
                    "'{14}',getdate(),'{8}','{9}','{10}',{11},'{12}','','{14}','{1}','','{13}','','','','','','')",
                this.comboBoxEx4.Text.Trim(), //公司  0
                this.dateTimeInput1.Value.ToShortDateString(),//录入时间 1
                this.comboBoxEx1.Text.Trim(),//提交类型2 
                this.textBoxX1.Text.Trim(), //QQ 3
                this.dateTimeInput2.Value.ToShortDateString(), //报名日期 4
                tools.FilteSQLStr(this.textBoxX2.Text.Trim()), //QQ昵称 5
                this.textBoxX3.Text.Trim(), //联系电话 6
                this.textBoxX4.Text.Trim(),//学员情况 7
                "",//处理结果 8
                "", //退款账户 9
                "",//退款账户姓名 10
                "0", //退款金额 11
                "", //退款原因 12
                "",//退款交易流水号 13
                frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ:" + this.textBoxX1.Text.Trim() + "  类型:" + this.comboBoxEx1.Text;
                frmUTSOFTMAIN.OperationRemark = "增加成功！";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select *,stuff((select  ','+c.AssumedName from Pub_TSTK_JXManager b left join Users c on b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson from Pub_ComplainRefundInfo a where CompanyNames = '{0}' and QQ = '{1}'",
                    this.comboBoxEx4.Text.Trim(), this.textBoxX1.Text.Trim());
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
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
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
            if (MessageBox.Show("你确定要删除选中行数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqljxmanager = string.Format("select * from Pub_TSTK_JXManager where ForID = {0}", 
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            DataTable dtjxmanager = DBHelper.ExecuteQuery(sqljxmanager);
            if(dtjxmanager.Rows.Count>0)
            {
                for (int i = 0; i < dtjxmanager.Rows.Count;i++ ) 
                {
                    bool a = PublicMethod.delete_TSTKJX(
                    int.Parse(dtjxmanager.Rows[i][0].ToString()),
                    int.Parse(dtjxmanager.Rows[i][1].ToString()),
                    dtjxmanager.Rows[i][2].ToString(),
                    DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString()),
                    this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString(),
                    dtjxmanager.Rows[i][3].ToString(),
                    float.Parse(dtjxmanager.Rows[i][4].ToString()));
                    MessageBox.Show(a.ToString());
                }
            }
            string sql1 = string.Format("delete from Pub_ComplainRefundInfo where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ" + this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "删除成功";
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
            if (MessageBox.Show("你确定要修改选中行的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                //EnterType = '{0}',Jointime = '{2}',报名类型和时间不允许修改
                sql1 = string.Format("update Pub_ComplainRefundInfo "+
                " set QQ = '{1}', Join_Time = '{2}',NickName = '{3}'," +
                " Telephone = '{4}', VIPSituation = '{5}'"+
                " where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                this.textBoxX1.Text.Trim(), this.dateTimeInput2.Value.ToShortDateString(),
                this.textBoxX2.Text.Trim(), this.textBoxX3.Text.Trim(),
                this.textBoxX4.Text.Trim());
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
                frmUTSOFTMAIN.OperationObject = "修改QQ：" + this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = sql1;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.dateTimeInput1.Value.ToShortDateString();
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.dateTimeInput2.Value.ToShortDateString();
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value = this.textBoxX4.Text;
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select *,"+
                " stuff((select  ','+c.AssumedName from Pub_TSTK_JXManager b left join Users c on "+
                " b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id and EnterTypeSmall != '绩效退款'"+
                " for xml path('')),1,1,'') as JXPerson from Pub_ComplainRefundInfo a where a.CompanyNames = '{0}' and " +
                " a.SubmitTime between '{1}' and '{2}' and a.SubmitType like '%{3}%' and a.QQ like '%{4}%' ",
                this.comboBoxEx4.Text.Trim(),
                this.dateTimeInput3.Value.ToShortDateString(), this.dateTimeInput4.Value.ToShortDateString(),
                this.comboBoxEx3.Text.Trim(), this.textBoxX9.Text.Trim());
            if (this.textBoxX5.Text != "")
            {
                sql1 += string.Format(" and RefundValue = {0} ", this.textBoxX5.Text.Trim());
            }
            sql1 += "  order by SubmitTime desc";
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
            UT_KF_TSTKJXManager utjxm = new UT_KF_TSTKJXManager();
            utjxm.forID = int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            utjxm.companyNames = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            utjxm.jxTime = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            utjxm.enterType = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            utjxm.companyType = "腾讯";

            string sqlJXPerson = string.Format("select c.AssumedName from  Pub_VIPMessage a" +
                " left join Pub_YDBM_JXManager b on a.id = b.ForID" +
                " left join Users c on b.StaffID = C.StaffID" +
                " where a.qq = '{0}' and charindex('报名',EnterType)>0" +
                " and a.CompanyNames = '{1}'" +
                " and b.StaffID = b.AmountManager" +
                " group by c.AssumedName",
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString());
            DataTable dtJXPerson = DBHelper.ExecuteQuery(sqlJXPerson);
            if (dtJXPerson.Rows.Count > 0)
            {
                for (int i = 0; i < dtJXPerson.Rows.Count; i++)
                {
                    utjxm.jxperson += dtJXPerson.Rows[i][0].ToString() + " , ";
                }
            }
            if (
                    PublicMethod.fromUpdatePower(
                    "腾讯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                utjxm.updateState = true;
            }
            string sqlDTSource = string.Format("select a.id,c.QQ,c.NickName,b.DepartmentName,b.AssumedName,a.StaffID,a.EnterTypeSmall,a.Value,a.Remark" +
                    " from Pub_TSTK_JXManager a left join Users b on a.StaffID = b.StaffID left join Pub_VIPMessage c on a.ForID = c.id" +
                    " where a.StaffID = a.AmountManager and a.EnterTypeSmall != '绩效退款' and a.ForID = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            utjxm.dtSource = DBHelper.ExecuteQuery(sqlDTSource);
            utjxm.Show();
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (!this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString().Contains("退款"))
                return;
            UT_TK_DealWith tdw = new UT_TK_DealWith();
            tdw.Text = this.buttonX6.Text;
            tdw.windowText = windowText;
            tdw.companyType = "腾讯";
            string sql1 = string.Format("select * from Pub_ComplainRefundInfo where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            tdw.dtSource = DBHelper.ExecuteQuery(sql1);
            tdw.Show();
        }

        private void textBoxX1_Leave(object sender, EventArgs e)
        {
            if (textBoxX1.Text == "")
                return;
            string sql1 = string.Format("select Jointime,NickName,Telephone from Pub_VIPMessage " +
                "where QQ = '{0}' and charindex('报名',EnterType)>0 and CompanyNames = '{1}'", textBoxX1.Text.Trim(), this.comboBoxEx4.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                this.dateTimeInput2.Value = DateTime.Parse(dt1.Rows[0][0].ToString());
                this.textBoxX2.Text = dt1.Rows[0][1].ToString();
                this.textBoxX3.Text = dt1.Rows[0][2].ToString();
            }
            else
            {
                MessageBox.Show("此QQ不存在于软件中，考虑QQ号码是否填写错误！");
            }
        }
    }
}
