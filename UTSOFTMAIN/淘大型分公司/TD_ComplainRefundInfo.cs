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
    public partial class TD_ComplainRefundInfo : Form
    {
        public TD_ComplainRefundInfo()
        {
            InitializeComponent();
        }
        private static string windowText = "";
        private void TD_FKTKComplainRefundInfo_Load(object sender, EventArgs e)
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
            windowText = (sender as TD_ComplainRefundInfo).Text;
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.dateTimeInput2.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString());
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
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
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (
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
                //insert into Pub_ComplainRefundInfo values(
                //所属公司,日期,提交类型,
                //QQ,报名日期,QQ昵称,旺旺号,
                //联系电话,学员情况,信息备注,录入人,
                //录入时间,处理结果,退款账户,退款账户姓名,
                //退款金额,退款原因,处理备注,处理人,
                //处理时间,行政退款状态,退款交易流水号,
                //退款备注,退款人,退款时间,核实状态,
                //核实人,核实时间)
                sql1 = string.Format("insert into Pub_ComplainRefundInfo values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}'"+
                    " ,'{13}','{14}','{15}','{16}','{17}','{18}','{1}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}')",
                    this.comboBoxEx4.Text.Trim(),this.dateTimeInput1.Value.ToShortDateString(),this.comboBoxEx3.Text.Trim(),
                    this.textBoxX1.Text.Trim(), this.dateTimeInput2.Value.ToShortDateString(), tools.FilteSQLStr(this.textBoxX2.Text.Trim()), tools.FilteSQLStr(this.textBoxX3.Text.Trim()),
                    this.textBoxX4.Text.Trim(), this.textBoxX5.Text.Trim(),"",frmUTSOFTMAIN.StaffID,
                    this.dateTimeInput1.Value.ToShortDateString(), "", "", "",
                    "", "", "", frmUTSOFTMAIN.StaffID,
                    this.dateTimeInput1.Value.ToShortDateString(), "", "",
                    "", "", "", "", "", "");
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ:" + this.textBoxX1.Text.Trim() + "  类型:" + this.comboBoxEx3.Text;
                frmUTSOFTMAIN.OperationRemark = "增加成功！";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select *,"+
                    " stuff((select  ','+c.AssumedName from Pub_TSTK_JXManager b left join Users c on b.StaffID = c.StaffID and b.StaffID = b.AmountManager where b.ForID = a.id for xml path('')),1,1,'') as JXPerson"+
                    " from Pub_ComplainRefundInfo a "+
                    " where a.CompanyNames = '{0}' and a.QQ = '{1}'",
                     this.comboBoxEx4.Text.Trim(), this.textBoxX1.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
                dateClear();
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
            if (MessageBox.Show("你确定要删除选中行绩效数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqljxmanager = string.Format("select * from Pub_TSTK_JXManager where ForID = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            DataTable dtjxmanager = DBHelper.ExecuteQuery(sqljxmanager);
            if (dtjxmanager.Rows.Count > 0)
            {
                for (int i = 0; i < dtjxmanager.Rows.Count; i++)
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
            //删除投诉退款信息
            string sql1 = string.Format("delete from Pub_ComplainRefundInfo where id = '{0}'",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum2 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum2 > 0)
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
                sql1 = string.Format("update Pub_ComplainRefundInfo set " +
                    "QQ = '{3}'," +
                    "Join_Time = '{4}'," +
                    "NickName = '{5}'," +
                    "WWNum = '{6}',"+
                    "Telephone = '{7}'," +
                    "VIPSituation = '{8}'" +
                    " where  id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.dateTimeInput1.Value.ToShortDateString(),
                    this.comboBoxEx3.Text,
                    this.textBoxX1.Text,
                    this.dateTimeInput2.Value.ToShortDateString(),
                    this.textBoxX2.Text,
                    this.textBoxX3.Text,
                    this.textBoxX4.Text,
                    this.textBoxX5.Text);
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
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value= this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value =DateTime.Parse(this.dateTimeInput2.Value.ToShortDateString());
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value=this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value=this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value=this.textBoxX4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value=this.textBoxX5.Text;
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select *," +
                    " stuff((select  ','+c.AssumedName from Pub_TSTK_JXManager b left join Users c on b.StaffID = c.StaffID and "+
                    " b.StaffID = b.AmountManager where b.ForID = a.id and EnterTypeSmall != '绩效退款' for xml path('')),1,1,'') as JXPerson" +
                    " from Pub_ComplainRefundInfo a " +
                    " where a.CompanyNames = '{0}' and a.QQ like '%{1}%' and a.SubmitTime between '{2}' and '{3}' and "+
                    "a.SubmitType like '%{4}%'",
                     this.comboBoxEx4.Text.Trim(), this.textBoxX14.Text.Trim(),this.dateTimeInput3.Value.ToShortDateString(),
                     this.dateTimeInput4.Value.ToShortDateString(),this.comboBoxEx2.Text.Trim());

            if (this.textBoxX6.Text != "")
            {
                sql1 += string.Format(" and a.RefundValue = {0} ", this.textBoxX6.Text.Trim());
            }
            sql1 += "  order by a.Join_Time desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            this.labelX20.Text = tools.SumValue(dataGridViewX1, 16);
        }

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx4.DataSource = dtCompany.Copy();
            this.comboBoxEx4.DisplayMember = "CompanyNames";
            this.comboBoxEx4.ValueMember = "CompanyNames";
            this.comboBoxEx4.SelectedIndex = -1;
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
                    "淘大",
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
            string sql1 = string.Format("select * from Pub_ComplainRefundInfo where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            tdw.dtSource = DBHelper.ExecuteQuery(sql1);
            tdw.Show();
        }

        private void textBoxX1_Leave(object sender, EventArgs e)
        {
            if (textBoxX1.Text == "")
                return;
            string sql1 = string.Format("select Jointime,NickName,WWNum,Telephone from Pub_VIPMessage " +
                "where QQ = '{0}' and EnterType = '报名' and CompanyNames = '{1}'", textBoxX1.Text.Trim(),this.comboBoxEx4.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                this.dateTimeInput2.Value = DateTime.Parse(dt1.Rows[0][0].ToString());
                this.textBoxX2.Text = dt1.Rows[0][1].ToString();
                this.textBoxX3.Text = dt1.Rows[0][2].ToString();
                this.textBoxX4.Text = dt1.Rows[0][3].ToString();
            }
            else
            {
                MessageBox.Show("此QQ不存在于软件中，考虑QQ号码是否填写错误！");
            }
        }
        private void dateClear()
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.textBoxX1.Text = "";
            this.textBoxX2.Text = "";
            this.textBoxX3.Text = "";
            this.textBoxX4.Text = "";
            this.textBoxX5.Text = "";
        }
        private void buttonX7_Click(object sender, EventArgs e)
        {
            dateClear();
        }
    }
}
