using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTSOFTMAIN.总内部界面;
using UTSOFTMAIN.福利市场;

namespace UTSOFTMAIN
{
    public partial class frmInWard1 : Form
    {
        public frmInWard1()
        {
            InitializeComponent();
        }

        private DataTable dtjbsalarytemp = new DataTable();
        
        private void UT_frmAdminPart_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now;
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now;
            this.dateTimeInput4.Value = DateTime.Parse("2020-2-2");
            this.comboBoxEx5.SelectedIndex = 0;

            this.dateTimeInput13.Value = System.DateTime.Now.AddMonths(-1);
            this.dateTimeInput14.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput15.Value = System.DateTime.Now;

            string sqlFlush = string.Format("select * from Pub_JXCaculate order by SubmitTime desc");
            this.dataGridViewX3.DataSource = DBHelper.ExecuteQuery(sqlFlush);

            dtjbsalarytemp = DBHelper.ExecuteQuery("select * from Pub_BaseSalary");

            fromShow();
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.dateTimeInput5.Value =DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString());
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.dateTimeInput1.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString());
            this.dateTimeInput2.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString());
            this.comboBoxEx5.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            this.dateTimeInput3.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString());
            this.dateTimeInput4.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString());
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[17].Value.ToString();
            this.checkBox1.Checked = bool.Parse(this.dataGridViewX1.SelectedRows[0].Cells[23].Value.ToString());
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加[" + this.textBoxX1.Text.Trim() + "]吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            int isPartManager = 0;
            if (this.checkBox1.Checked)
                isPartManager = 1;
            else
                isPartManager = 0;
            string sql1 = string.Format("insert into Users values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',"+
                "'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}',"+
                "'{16}','{17}','ut8888..',0,getdate(),1,'',{18})",
                this.comboBoxEx1.Text,
                this.comboBoxEx2.Text,
                this.comboBoxEx3.Text,
                this.comboBoxEx4.Text,
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.dateTimeInput5.Value,
                this.textBoxX4.Text,
                this.textBoxX5.Text,
                this.textBoxX6.Text,
                this.dateTimeInput1.Value,
                this.dateTimeInput2.Value,
                this.comboBoxEx5.Text,
                this.dateTimeInput3.Value,
                this.dateTimeInput4.Value,
                this.textBoxX7.Text,
                this.textBoxX8.Text,
                isPartManager);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if(resultNum>0)
            {
                MessageBox.Show("新增员工成功！");
                frmUTSOFTMAIN.dtAllPerson = DBHelper.ExecuteQuery("select CompanyNames,DepartmentName,GroupName,UserType,AssumedName,StaffID,onjob " +
                "from Users order by CompanyNames,DepartmentName desc");
                string sqlflush = string.Format("select * from Users where onjob = 1 and CompanyNames = '{0}' and DepartmentName = '{1}'" +
                " order by EmploymentDate desc",this.comboBoxEx1.Text.Trim(),this.comboBoxEx2.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlflush);
                //--------------日志开始------------------
                string OperationRemarkStr = string.Format("公司='{0}', 部门='{1}', 组别='{2}', 职位='{3}', 花名='{4}'," +
                " 姓名='{5}', 生日='{7}', 电话='{8}', QQ='{9}', 合同信息='{10}', 入职日期='{11}', 转正日期='{12}', 入住公司='{13}'," +
                " 入住时间='{14}', 离开时间='{15}', 是否部门主管 = {18}",
                this.comboBoxEx1.Text,
                this.comboBoxEx2.Text,
                this.comboBoxEx3.Text,
                this.comboBoxEx4.Text,
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.dateTimeInput5.Value,
                this.textBoxX4.Text,
                this.textBoxX5.Text,
                this.textBoxX6.Text,
                this.dateTimeInput1.Value,
                this.dateTimeInput2.Value,
                this.comboBoxEx5.Text,
                this.dateTimeInput3.Value,
                this.dateTimeInput4.Value,
                this.textBoxX7.Text,
                this.textBoxX8.Text,
                this.checkBox1.Checked);
                frmUTSOFTMAIN.OperationObject = this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "成员新增:" + OperationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除[" + this.textBoxX1.Text.Trim() + "]吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Users where StaffID = '{0}'",this.textBoxX3.Text.Trim());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("删除成功！");
                frmUTSOFTMAIN.dtAllPerson = DBHelper.ExecuteQuery("select CompanyNames,DepartmentName,GroupName,UserType,AssumedName,StaffID,onjob " +
                "from Users order by CompanyNames,DepartmentName desc");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                //--------------日志开始------------------
                frmUTSOFTMAIN.OperationObject = this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "成员删除";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要修改[" + this.textBoxX1.Text.Trim() + "]吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            int isPartManager = 0;
            if(this.checkBox1.Checked)
                isPartManager = 1;
            else
                isPartManager = 0;
            string sql1 = string.Format("update Users set CompanyNames='{0}', DepartmentName='{1}', GroupName='{2}', UserType='{3}', AssumedName='{4}',"+
                " Name='{5}', BirthDay='{7}', Telphone='{8}', QQ='{9}', ContractInfo='{10}', EmploymentDate='{11}', gularizationDate='{12}', StayCompany='{13}'," +
                " EnterTime='{14}', AwayTime='{15}', StayRemark='{16}',  Remark='{17}', IsPartManager = {18} where  StaffID='{6}'",
                this.comboBoxEx1.Text,
                this.comboBoxEx2.Text,
                this.comboBoxEx3.Text,
                this.comboBoxEx4.Text,
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.dateTimeInput5.Value,
                this.textBoxX4.Text,
                this.textBoxX5.Text,
                this.textBoxX6.Text,
                this.dateTimeInput1.Value,
                this.dateTimeInput2.Value,
                this.comboBoxEx5.Text,
                this.dateTimeInput3.Value,
                this.dateTimeInput4.Value,
                this.textBoxX7.Text,
                this.textBoxX8.Text,
                isPartManager);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("修改成功！");
                frmUTSOFTMAIN.dtAllPerson = DBHelper.ExecuteQuery("select CompanyNames,DepartmentName,GroupName,UserType,AssumedName,StaffID,onjob " +
                "from Users order by CompanyNames,DepartmentName desc");
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.comboBoxEx2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.comboBoxEx3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.comboBoxEx4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value = this.dateTimeInput5.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value = this.textBoxX4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value = this.textBoxX5.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[10].Value = this.textBoxX6.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value = this.dateTimeInput1.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[12].Value = this.dateTimeInput2.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[13].Value = this.comboBoxEx5.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[14].Value = this.dateTimeInput3.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[15].Value = this.dateTimeInput4.Value;
                this.dataGridViewX1.SelectedRows[0].Cells[16].Value = this.textBoxX7.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[17].Value = this.textBoxX8.Text;
                //--------------日志开始------------------
                string OperationRemarkStr1 = string.Format("公司='{0}', 部门='{1}', 组别='{2}', 职位='{3}', 花名='{4}'," +
                " 姓名='{5}', 生日='{7}', 电话='{8}', QQ='{9}', 合同信息='{10}', 入职日期='{11}', 转正日期='{12}', 入住公司='{13}'," +
                " 入住时间='{14}', 离开时间='{15}'",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
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
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[17].Value.ToString());
                string OperationRemarkStr2 = string.Format("公司='{0}', 部门='{1}', 组别='{2}', 职位='{3}', 花名='{4}'," +
                " 姓名='{5}', 生日='{7}', 电话='{8}', QQ='{9}', 合同信息='{10}', 入职日期='{11}', 转正日期='{12}', 入住公司='{13}'," +
                " 入住时间='{14}', 离开时间='{15}', 是否部门主管 = {18}",
                this.comboBoxEx1.Text,
                this.comboBoxEx2.Text,
                this.comboBoxEx3.Text,
                this.comboBoxEx4.Text,
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.dateTimeInput5.Value,
                this.textBoxX4.Text,
                this.textBoxX5.Text,
                this.textBoxX6.Text,
                this.dateTimeInput1.Value,
                this.dateTimeInput2.Value,
                this.comboBoxEx5.Text,
                this.dateTimeInput3.Value,
                this.dateTimeInput4.Value,
                this.textBoxX7.Text,
                this.textBoxX8.Text,
                isPartManager);
                frmUTSOFTMAIN.OperationObject = "修改前：" + OperationRemarkStr1;
                frmUTSOFTMAIN.OperationRemark = "修改后："+OperationRemarkStr2;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要将[" + this.textBoxX1.Text.Trim() + "]状态变为离职吗?", "确认提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update Users set onjob = 0,LeaveTime = '{1}' where StaffID = '{0}'", this.textBoxX3.Text.Trim(),this.dateTimeInput12.Value.ToShortDateString());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("确认离职成功！");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                //--------------日志开始------------------
                frmUTSOFTMAIN.OperationObject = this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "确认离职成功";
                frmUTSOFTMAIN.addlog(this.buttonX4);
                //--------------日志结束------------------
            }
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Users where onjob = 1"+
                " and CompanyNames like '%{0}%'" +
                " and DepartmentName like '%{1}%'" +
                " and GroupName like '%{2}%'" +
                " and UserType like '%{3}%'" +
                " and AssumedName like '%{4}%'" +
                " and Name like '%{5}%'" +
                " order by EmploymentDate desc",
                this.comboBoxEx1.Text.Trim(),this.comboBoxEx2.Text.Trim(),this.comboBoxEx3.Text.Trim(),this.comboBoxEx4.Text.Trim(),
                this.textBoxX1.Text.Trim(), this.textBoxX2.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }

        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {
            if (textBoxX2.Text == "")
            {
                this.comboBoxEx1.Text = "";
                this.comboBoxEx2.Text = "";
                this.comboBoxEx3.Text = "";
                this.comboBoxEx4.Text = "";
                this.textBoxX1.Text = "";
                this.textBoxX2.Text = "";
                this.textBoxX3.Text = "";
                this.dateTimeInput5.Value =System.DateTime.Now;
                this.textBoxX4.Text = "";
                this.textBoxX5.Text = "";
                this.textBoxX6.Text = "";
                this.dateTimeInput1.Value = System.DateTime.Now;
                this.dateTimeInput2.Value = System.DateTime.Now.AddMonths(2).AddDays(1 - System.DateTime.Now.AddMonths(2).Day);
                this.comboBoxEx5.Text = "否";
                this.dateTimeInput3.Value = DateTime.Parse("2020-2-2");
                this.dateTimeInput4.Value = DateTime.Parse("2020-2-2");
                this.textBoxX7.Text = "";
                this.textBoxX8.Text = "";
            }
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEx5.Text == "是")
            {
                this.dateTimeInput3.Value = System.DateTime.Now;
                this.dateTimeInput4.Value = DateTime.Parse("2020-2-2");
            }
            else 
            {
                this.dateTimeInput3.Value = DateTime.Parse("2020-2-2");
                this.dateTimeInput4.Value = DateTime.Parse("2020-2-2");
            }
            
        }

        private void textBoxX3_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxX3.Text.Length != 18)
            {
                this.errorProvider1.SetError(this.textBoxX3, "身份证格式错误");
            }
            else 
            {
                this.errorProvider1.Clear();
                string birthdayNum = this.textBoxX3.Text.Substring(6, 8);
                DateTime birthdayTime = DateTime.ParseExact(birthdayNum, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                this.dateTimeInput5.Value = birthdayTime;
            }
        }

        private void dataGridViewX2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX2.SelectedRows.Count == 0)
                return;
            this.comboBoxEx10.Text = this.dataGridViewX2.SelectedRows[0].Cells[0].Value.ToString();
            this.comboBoxEx9.Text = this.dataGridViewX2.SelectedRows[0].Cells[1].Value.ToString();
            this.comboBoxEx8.Text = this.dataGridViewX2.SelectedRows[0].Cells[2].Value.ToString();
            this.comboBoxEx7.Text = this.dataGridViewX2.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX16.Text = this.dataGridViewX2.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX15.Text = this.dataGridViewX2.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX14.Text = this.dataGridViewX2.SelectedRows[0].Cells[6].Value.ToString();
            this.dateTimeInput6.Value = DateTime.Parse(this.dataGridViewX2.SelectedRows[0].Cells[7].Value.ToString());
            this.textBoxX13.Text = this.dataGridViewX2.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX12.Text = this.dataGridViewX2.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX11.Text = this.dataGridViewX2.SelectedRows[0].Cells[10].Value.ToString();
            this.dateTimeInput10.Value = DateTime.Parse(this.dataGridViewX2.SelectedRows[0].Cells[11].Value.ToString());
            this.dateTimeInput9.Value = DateTime.Parse(this.dataGridViewX2.SelectedRows[0].Cells[12].Value.ToString());
            this.comboBoxEx6.Text = this.dataGridViewX2.SelectedRows[0].Cells[13].Value.ToString();
            this.dateTimeInput8.Value = DateTime.Parse(this.dataGridViewX2.SelectedRows[0].Cells[14].Value.ToString());
            this.dateTimeInput7.Value = DateTime.Parse(this.dataGridViewX2.SelectedRows[0].Cells[15].Value.ToString());
            this.textBoxX10.Text = this.dataGridViewX2.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX9.Text = this.dataGridViewX2.SelectedRows[0].Cells[17].Value.ToString();
            this.dateTimeInput11.Value = DateTime.Parse(this.dataGridViewX2.SelectedRows[0].Cells[22].Value.ToString());
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Users where onjob = 0" +
                " and CompanyNames like '%{0}%'" +
                " and DepartmentName like '%{1}%'" +
                " and GroupName like '%{2}%'" +
                " and UserType like '%{3}%'" +
                " and AssumedName like '%{4}%'" +
                " and Name like '%{5}%'" +
                " order by LeaveTime desc",
                this.comboBoxEx10.Text.Trim(), this.comboBoxEx9.Text.Trim(), this.comboBoxEx8.Text.Trim(), this.comboBoxEx7.Text.Trim(),
                this.textBoxX16.Text.Trim(), this.textBoxX15.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX2.DataSource = dt1;
        }

        private void dataGridViewX3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX3.SelectedRows.Count == 0)
                return;
            this.comboBoxEx11.Text = this.dataGridViewX3.SelectedRows[0].Cells[1].Value.ToString();
            this.comboBoxEx12.Text = this.dataGridViewX3.SelectedRows[0].Cells[2].Value.ToString();
            this.comboBoxEx13.Text = this.dataGridViewX3.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx14.Text = this.dataGridViewX3.SelectedRows[0].Cells[4].Value.ToString();
            this.comboBoxEx15.Text = this.dataGridViewX3.SelectedRows[0].Cells[5].Value.ToString();
            this.comboBoxEx16.Text = this.dataGridViewX3.SelectedRows[0].Cells[6].Value.ToString();
            this.comboBoxEx17.Text = this.dataGridViewX3.SelectedRows[0].Cells[7].Value.ToString();
            this.comboBoxEx18.Text = this.dataGridViewX3.SelectedRows[0].Cells[8].Value.ToString();
            this.comboBoxEx19.Text = this.dataGridViewX3.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX17.Text = this.dataGridViewX3.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX18.Text = this.dataGridViewX3.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX19.Text = this.dataGridViewX3.SelectedRows[0].Cells[12].Value.ToString();
        }

        private void comboBoxEx15_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}' and JXType = '{1}'", this.comboBoxEx11.Text, this.comboBoxEx15.Text);
            string[] columNames = new string[] { "JXTypeSmall" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.jxCaculateType, columNames, sql);
            this.comboBoxEx16.DataSource = dtPerson;
            this.comboBoxEx16.ValueMember = "JXTypeSmall";
            this.comboBoxEx16.DisplayMember = "JXTypeSmall";
            this.comboBoxEx16.SelectedIndex = -1;
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要提交据吗?", "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("insert into Pub_JXCaculate values" +
                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},{10},{11},'{12}','{13}')",
                this.comboBoxEx11.Text,this.comboBoxEx12.Text,this.comboBoxEx13.Text,this.comboBoxEx14.Text,
                this.comboBoxEx15.Text,this.comboBoxEx16.Text,this.comboBoxEx17.Text,this.comboBoxEx18.Text,
                this.comboBoxEx19.Text,this.textBoxX17.Text.Trim(),this.textBoxX18.Text.Trim(),
                this.textBoxX19.Text.Trim(),System.DateTime.Now,frmUTSOFTMAIN.StaffID);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("增加绩效计算标准成功！");
                frmUTSOFTMAIN.jxCaculateType = DBHelper.ExecuteQuery("select * from Pub_JXCaculate");
                string sqlFlush = string.Format("select * from Pub_JXCaculate where " +
                " CompanyNames like '%{0}%' and DepartmentName like '%{1}%'" +
                " and JXType like '%{4}%' order by SubmitTime desc",
                this.comboBoxEx11.Text, this.comboBoxEx12.Text, this.comboBoxEx13.Text,
                this.comboBoxEx14.Text, this.comboBoxEx15.Text);
                this.dataGridViewX3.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}'," +
                    "绩效大类 = '{4}',绩效小类 = '{5}',绩效基数 = '{6}',档次基准 = '{7}',"+
                    "档次类型 = '{8}',下限 = '{9}',上限 = '{10}',单价 = '{11}'",
                    this.comboBoxEx11.Text, this.comboBoxEx12.Text, this.comboBoxEx13.Text, this.comboBoxEx14.Text,
                    this.comboBoxEx15.Text, this.comboBoxEx16.Text, this.comboBoxEx17.Text, this.comboBoxEx18.Text,
                    this.comboBoxEx19.Text, this.textBoxX17.Text.Trim(), this.textBoxX18.Text.Trim(),this.textBoxX19.Text.Trim());
                frmUTSOFTMAIN.OperationObject = this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "增加：" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX7);
                //--------------日志结束------------------
            }
            else 
            {
                MessageBox.Show("提交失败，请检查数据类型是否输入错误！");
            }
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加删除选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_JXCaculate where id = {0}",
                this.dataGridViewX3.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if(resultNum>0)
            {
                MessageBox.Show("删除成功!");
                frmUTSOFTMAIN.jxCaculateType = DBHelper.ExecuteQuery("select * from Pub_JXCaculate");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}'," +
                    "绩效大类 = '{4}',绩效小类 = '{5}',绩效基数 = '{6}',档次基准 = '{7}'," +
                    "档次类型 = '{8}',下限 = '{9}',上限 = '{10}',单价 = '{11}'",
                    this.dataGridViewX3.SelectedRows[0].Cells[1].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[2].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[3].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[4].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[5].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[6].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[7].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[8].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[9].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[10].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[11].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[12].Value.ToString());
                frmUTSOFTMAIN.OperationObject = operationRemarkStr;
                frmUTSOFTMAIN.OperationRemark = "删除成功";
                frmUTSOFTMAIN.addlog(this.buttonX8);
                //--------------日志结束------------------
                this.dataGridViewX3.Rows.Remove(this.dataGridViewX3.SelectedRows[0]);
            }
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            //提交所有人的迟到，早退，请假，旷工，保险，美工部绩效各成员工资，商图社各成员绩效工资  1h 7.75 3.88
            if (this.comboBoxEx21.Text == "")
            {
                MessageBox.Show("必须选定绩效人");
                return;
            }
            try
            {
                double.Parse(this.textBoxX20.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请在数量栏填入数字！");
                return;
            }
            bool submitSuccess = true;
            try
            {
                double unitjbsalary = 7.75;
                string sqljbsalary = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}' and UserType = '{3}'",
                frmUTSOFTMAIN.CompanyNames,
                frmUTSOFTMAIN.DepartmentName,
                frmUTSOFTMAIN.GroupName,
                frmUTSOFTMAIN.UserType);
                string[] columNames = new string[] { "jbsalary" };
                DataTable dtjbsalary = tools.dtFilter(dtjbsalarytemp, columNames, sqljbsalary);
                if (dtjbsalary.Rows.Count != 0)
                {
                    unitjbsalary = Math.Round(double.Parse(dtjbsalary.Rows[0][0].ToString()) / 21.75 / 8 - 0.05, 2);
                }
                switch (this.comboBoxEx22.Text)
                {
                    case "迟到":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        if (double.Parse(this.textBoxX20.Text.Trim()) <= 10)
                        {
                            //考勤绩效时间,公司,考勤绩效所属人,绩效大类,    绩效数量,绩效金额,    备注,录入人,录入时间
                            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','迟到',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                0,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sql1);
                            string sqlTemp = string.Format("select * from Pub_WorkSalary " +
                                "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and " +
                                "KQJXCount <= 10 and KQJXType = '迟到' and StaffID = '{2}'",
                                this.dateTimeInput13.Value.Year,
                                this.dateTimeInput13.Value.Month,
                                this.comboBoxEx21.SelectedValue.ToString());
                            DataTable dtTemp = DBHelper.ExecuteQuery(sqlTemp);
                            if (dtTemp.Rows.Count > 3)
                            {
                                string sqlUp = string.Format(string.Format("update Pub_WorkSalary set KQJXValue = -10 " +
                                    "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and " +
                                    "KQJXCount <= 10 and KQJXType = '迟到' and StaffID = '{2}'",
                                    this.dateTimeInput13.Value.Year,
                                    this.dateTimeInput13.Value.Month,
                                    this.comboBoxEx21.SelectedValue.ToString()));
                                DBHelper.ExecuteUpdate(sqlUp);
                            }
                        }
                        else if (double.Parse(this.textBoxX20.Text.Trim()) > 10 && double.Parse(this.textBoxX20.Text.Trim()) <= 30)
                        {
                            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','迟到',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                -10,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sql1);
                        }
                        else if (double.Parse(this.textBoxX20.Text.Trim()) > 30)
                        {
                            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','迟到',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                -25,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sql1);
                        }
                        break;
                    case "早退":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        if (double.Parse(this.textBoxX20.Text.Trim()) <= 30)
                        {
                            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','早退',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                0,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sql1);
                            string sqlTemp = string.Format("select * from Pub_WorkSalary " +
                                "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and KQJXCount <= 30 and KQJXType = '早退' and StaffID = '{2}'",
                                this.dateTimeInput13.Value.Year,
                                this.dateTimeInput13.Value.Month,
                                this.comboBoxEx21.SelectedValue.ToString());
                            DataTable dtTemp = DBHelper.ExecuteQuery(sqlTemp);
                            if (dtTemp.Rows.Count > 3)
                            {
                                string sqlUp = string.Format("update Pub_WorkSalary set KQJXValue = -10 " +
                                    "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and KQJXCount <= 10 and KQJXType = '早退' and StaffID = '{2}'",
                                    this.dateTimeInput13.Value.Year,
                                    this.dateTimeInput13.Value.Month,
                                    this.comboBoxEx21.SelectedValue.ToString());
                                DBHelper.ExecuteUpdate(sqlUp);
                            }
                        }
                        else if (double.Parse(this.textBoxX20.Text.Trim()) > 30 && double.Parse(this.textBoxX20.Text.Trim()) <= 60)
                        {
                            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','早退',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                -10,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sql1);
                        }
                        else if (double.Parse(this.textBoxX20.Text.Trim()) > 60)
                        {
                            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','早退',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                -25,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sql1);
                        }
                        break;
                    case "旷工":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string sql2 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','旷工',{4},-{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary * 2,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                        DBHelper.ExecuteUpdate(sql2);
                        break;
                    case "病假":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string datebj = Convert.ToDateTime(this.dateTimeInput13.Text.ToString()).ToString("yyyyMMdd");
                        string resultbj = tools.IsHoliday(datebj);
                        string numbj = resultbj.Substring(resultbj.Length - 3, 1);
                        if (numbj == "1")
                        {
                            string sqlbj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','病假',{4},-{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary * 2,
                                "(公休日病假)" + this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sqlbj);
                            break;
                        }
                        string sql3 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','病假',{4},-{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary / 2,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                        DBHelper.ExecuteUpdate(sql3);
                        break;
                    case "事假":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string datesj = Convert.ToDateTime(this.dateTimeInput13.Text.ToString()).ToString("yyyyMMdd");
                        string resultsj = tools.IsHoliday(datesj);
                        string numsj = resultsj.Substring(resultsj.Length - 3, 1);
                        if (numsj == "1")
                        {
                            string sqlsj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','事假',{4},-{5},'{6}','{7}',getdate())",
                                    this.comboBoxEx20.Text,
                                    this.dateTimeInput13.Value.ToShortDateString(),
                                    this.comboBoxEx21.SelectedValue.ToString(),
                                    this.comboBoxEx22.Text,
                                    this.textBoxX20.Text.Trim(),
                                    double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary * 2,
                                    "(公休日事假)"+this.textBoxX22.Text.Trim(),
                                    frmUTSOFTMAIN.StaffID);
                            DBHelper.ExecuteUpdate(sqlsj);
                            break;
                        }
                        string sql4 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','事假',{4},-{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                        DBHelper.ExecuteUpdate(sql4);
                        break;
                    case "加班":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string date = Convert.ToDateTime(this.dateTimeInput13.Text.ToString()).ToString("yyyyMMdd");
                        string result = tools.IsHoliday(date);
                        string num = result.Substring(result.Length - 3, 1);
                        switch (num)
                        {
                            case "0":
                                string sql5 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','工作日加班',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary * 1.5,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                                DBHelper.ExecuteUpdate(sql5);
                                break;
                            case "1":
                                string sql6 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','公休日加班',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary * 2,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                                DBHelper.ExecuteUpdate(sql6);
                                break;
                            case "2":
                                string sql7 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','法定节假日加班',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                double.Parse(this.textBoxX20.Text.Trim()) * unitjbsalary * 3,
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                                DBHelper.ExecuteUpdate(sql7);
                                break;
                            default:
                                MessageBox.Show("网络延迟，判断加班类型失败,请重试");
                                break;
                        }
                        break;
                    case "保险":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string sql8 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','保险',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                this.textBoxX21.Text.Trim(),
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                        DBHelper.ExecuteUpdate(sql8);
                        break;
                    case "优异奖金":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string sql9 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','优异奖金',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                this.textBoxX21.Text.Trim(),
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                        DBHelper.ExecuteUpdate(sql9);
                        break;
                    case "扣补工资":
                        if (this.textBoxX20.Text == "")
                        {
                            MessageBox.Show("数量栏不能为空");
                            break;
                        }
                        string sql10 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','扣补工资',{4},{5},'{6}','{7}',getdate())",
                                this.comboBoxEx20.Text,
                                this.dateTimeInput13.Value.ToShortDateString(),
                                this.comboBoxEx21.SelectedValue.ToString(),
                                this.comboBoxEx22.Text,
                                this.textBoxX20.Text.Trim(),
                                this.textBoxX21.Text.Trim(),
                                this.textBoxX22.Text.Trim(),
                                frmUTSOFTMAIN.StaffID);
                        DBHelper.ExecuteUpdate(sql10);
                        break;
                    default:
                        submitSuccess = false;
                        break;
                }
            }
            catch
            { MessageBox.Show("提交失败"); return; }
            if (submitSuccess)
            {
                MessageBox.Show("提交成功");
                string sqlFlush = string.Format("select a.id, a.KQJXTime, a.CompanyNames,b.DepartmentName, b.AssumedName, b.StaffID, " +
                    " a.KQJXType, a.KQJXCount, a.KQJXValue, a.Remark, a.EnterPerson, a.EnterTime" +
                    " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID "+
                    " where datepart(yy,KQJXTime) = {0} and datepart(mm,KQJXTime) = {1}" +
                    " and a.StaffID like '%{2}%'",
                    this.dateTimeInput13.Value.Year,
                    this.dateTimeInput13.Value.Month,
                    this.comboBoxEx21.SelectedValue);
                this.dataGridViewX4.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = this.comboBoxEx22.Text + ":" + this.textBoxX20.Text;
                frmUTSOFTMAIN.OperationRemark = "增加：" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX9);
                //--------------日志结束------------------
                salaryAll();
            }
            else
                MessageBox.Show("未找到此绩效类型！");
        }

        private void buttonX10_Click(object sender, EventArgs e)//删除
        {
            if (MessageBox.Show("你确定要删除【" + this.dataGridViewX4.SelectedRows[0].Cells[2].Value.ToString() + "】的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_WorkSalary where id = {0}",
                this.dataGridViewX4.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX4.SelectedRows[0].Cells[2].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "删除成功" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX10);
                //--------------日志结束------------------
                this.dataGridViewX4.Rows.Remove(this.dataGridViewX4.SelectedRows[0]);
                salaryAll();
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX11_Click(object sender, EventArgs e)//修改
        {
            if (MessageBox.Show("只能修改绩效金额,或者加班类型,你确定要修改【" + this.dataGridViewX4.SelectedRows[0].Cells[2].Value.ToString() + "】的绩效金额吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqlUp = "";
            bool flushjb = false;
            if (this.dataGridViewX4.SelectedRows[0].Cells[6].Value.ToString() == "工作日加班" ||
                this.dataGridViewX4.SelectedRows[0].Cells[6].Value.ToString() == "公休日加班" ||
                this.dataGridViewX4.SelectedRows[0].Cells[6].Value.ToString() == "法定节假日加班")
            {
                if (this.comboBoxEx22.Text.Trim() == "工作日加班" ||
                    this.comboBoxEx22.Text.Trim() == "公休日加班" ||
                    this.comboBoxEx22.Text.Trim() == "法定节假日加班")
                {
                    double kqJXValue = 0;
                    if (this.comboBoxEx22.Text.Trim() == "工作日加班")
                        kqJXValue = double.Parse(this.textBoxX20.Text.Trim()) * 7.75;
                    else if (this.comboBoxEx22.Text.Trim() == "公休日加班")
                        kqJXValue = double.Parse(this.textBoxX20.Text.Trim()) * 7.75 * 2;
                    else if (this.comboBoxEx22.Text.Trim() == "法定节假日加班")
                        kqJXValue = double.Parse(this.textBoxX20.Text.Trim()) * 7.75 * 3;
                    sqlUp = string.Format("update Pub_WorkSalary set KQJXValue = {0},KQJXType = '{2}' where id = {1}",
                     kqJXValue,
                     this.dataGridViewX4.SelectedRows[0].Cells[0].Value.ToString(),
                     this.comboBoxEx22.Text.Trim());
                    flushjb = true;
                }
                else
                {
                    MessageBox.Show("绩效类型输入错误，只能是：工作日加班，公休日加班，法定节假日加班");
                    return;
                }
            }
            else 
            {
                sqlUp = string.Format("update Pub_WorkSalary set KQJXValue = {0} where id = {1}",
                 this.textBoxX21.Text.Trim(),
                 this.dataGridViewX4.SelectedRows[0].Cells[0].Value.ToString());
                flushjb = false;
            }
            
            int resultNum = DBHelper.ExecuteUpdate(sqlUp);
            if (resultNum > 0)
            {
                MessageBox.Show("修改成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "绩效金额 =" + this.dataGridViewX4.SelectedRows[0].Cells[2].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "修改成功" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX11);
                //--------------日志结束------------------
                if (flushjb)
                {
                    this.dataGridViewX4.SelectedRows[0].Cells[6].Value = this.comboBoxEx22.Text.Trim();
                    this.dataGridViewX4.SelectedRows[0].Cells[8].Value = this.textBoxX21.Text.Trim();
                }
                else
                    this.dataGridViewX4.SelectedRows[0].Cells[8].Value = this.textBoxX21.Text.Trim();
                salaryAll();
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }

        private void buttonX12_Click(object sender, EventArgs e)//查询
        {
            string sfzStr = "";
            if (this.comboBoxEx24.SelectedValue != null) 
            {
                sfzStr = this.comboBoxEx24.SelectedValue.ToString();
            }
            string sql1 = string.Format("select a.id, a.KQJXTime, a.CompanyNames,b.DepartmentName, b.AssumedName, b.StaffID, "+
                " a.KQJXType, a.KQJXCount, a.KQJXValue, a.Remark, a.EnterPerson, a.EnterTime" +
                " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID where " +
                " a.KQJXTime between '{0}' and '{1}' and a.CompanyNames like '%{2}%'"+
                " and a.StaffID like '%{3}%' and a.KQJXType like '%{4}%'",
                this.dateTimeInput14.Value.ToShortDateString(),
                this.dateTimeInput15.Value.ToShortDateString(),
                this.comboBoxEx23.Text.Trim(), sfzStr,this.comboBoxEx25.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if(dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX4.DataSource = dt1;
            salaryAll();
        }

        private void comboBoxEx20_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'",this.comboBoxEx20.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx30.DataSource = dtPerson;
            this.comboBoxEx30.ValueMember = "DepartmentName";
            this.comboBoxEx30.DisplayMember = "DepartmentName";
            this.comboBoxEx30.SelectedIndex = -1;
        }
        private void comboBoxEx30_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx20.Text == "" || this.comboBoxEx30.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx20.Text, this.comboBoxEx30.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx21.DataSource = dtPerson;
            this.comboBoxEx21.ValueMember = "StaffID";
            this.comboBoxEx21.DisplayMember = "AssumedName";
            this.comboBoxEx21.SelectedIndex = -1;
        }
        private void comboBoxEx23_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx23.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx31.DataSource = dtPerson;
            this.comboBoxEx31.ValueMember = "DepartmentName";
            this.comboBoxEx31.DisplayMember = "DepartmentName";
            this.comboBoxEx31.SelectedIndex = -1;
        }
        
        private void comboBoxEx31_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx23.Text == "" || this.comboBoxEx31.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx23.Text, this.comboBoxEx31.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx24.DataSource = dtPerson;
            this.comboBoxEx24.ValueMember = "StaffID";
            this.comboBoxEx24.DisplayMember = "AssumedName";
            this.comboBoxEx24.SelectedIndex = -1;
        }
        private void comboBoxEx10_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx1.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx9.DataSource = dtPart;
            this.comboBoxEx9.ValueMember = "DepartmentName";
            this.comboBoxEx9.DisplayMember = "DepartmentName";
            this.comboBoxEx9.SelectedIndex = -1;
        }
        private void comboBoxEx9_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx10.Text == "" || this.comboBoxEx9.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx10.Text, this.comboBoxEx9.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx8.DataSource = dtGroup;
            this.comboBoxEx8.ValueMember = "GroupName";
            this.comboBoxEx8.DisplayMember = "GroupName";
            this.comboBoxEx8.SelectedIndex = -1;
        }
        private void comboBoxEx8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx10.Text == "" || this.comboBoxEx9.Text == "" || this.comboBoxEx8.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx10.Text, this.comboBoxEx9.Text, this.comboBoxEx8.Text);
            string[] columNames = new string[] { "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx7.DataSource = dtPerson;
            this.comboBoxEx7.ValueMember = "UserType";
            this.comboBoxEx7.DisplayMember = "UserType";
            this.comboBoxEx7.SelectedIndex = -1;
            
        }
        private void dataGridViewX4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX4.SelectedRows.Count == 0)
                return;
            this.dateTimeInput13.Value = DateTime.Parse(this.dataGridViewX4.SelectedRows[0].Cells[1].Value.ToString());
            this.comboBoxEx20.Text = this.dataGridViewX4.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxEx20_SelectedIndexChanged(null, null);
            this.comboBoxEx30.SelectedIndex = -1;
            this.comboBoxEx30.Text = this.dataGridViewX4.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx21.Text = "";
            this.comboBoxEx21.SelectedText = this.dataGridViewX4.SelectedRows[0].Cells[4].Value.ToString();
            this.comboBoxEx21.SelectedValue = this.dataGridViewX4.SelectedRows[0].Cells[5].Value.ToString();
            //if (this.dataGridViewX4.SelectedRows[0].Cells[6].Value.ToString().Contains("加班"))
            //    this.comboBoxEx22.Text = "加班";
            //else
                this.comboBoxEx22.Text = this.dataGridViewX4.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX20.Text = this.dataGridViewX4.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX21.Text = this.dataGridViewX4.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX22.Text = this.dataGridViewX4.SelectedRows[0].Cells[9].Value.ToString();
            
        }
        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx1.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPart;
            this.comboBoxEx2.ValueMember = "DepartmentName";
            this.comboBoxEx2.DisplayMember = "DepartmentName";
            this.comboBoxEx2.SelectedIndex = -1;
        }
        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "" || this.comboBoxEx2.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx1.Text, this.comboBoxEx2.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.DataSource = dtGroup;
            this.comboBoxEx3.ValueMember = "GroupName";
            this.comboBoxEx3.DisplayMember = "GroupName";
            this.comboBoxEx3.SelectedIndex = -1;
        }
        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "" || this.comboBoxEx2.Text == "" || this.comboBoxEx3.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text);
            string[] columNames = new string[] { "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "UserType";
            this.comboBoxEx4.DisplayMember = "UserType";
            this.comboBoxEx4.SelectedIndex = -1;
        }
        private void comboBoxEx11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx11.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx11.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx12.DataSource = dtPart;
            this.comboBoxEx12.ValueMember = "DepartmentName";
            this.comboBoxEx12.DisplayMember = "DepartmentName";
            this.comboBoxEx12.SelectedIndex = -1;

            string sql1 = string.Format("CompanyNames = '{0}'", this.comboBoxEx11.Text);
            string[] columNames1 = new string[] { "JXType" };
            DataTable dtPart1 = tools.dtFilter(frmUTSOFTMAIN.jxCaculateType, columNames1, sql1);
            this.comboBoxEx15.DataSource = dtPart1;
            this.comboBoxEx15.ValueMember = "JXType";
            this.comboBoxEx15.DisplayMember = "JXType";
            this.comboBoxEx15.SelectedIndex = -1;
        }
        private void comboBoxEx12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx11.Text == "" || this.comboBoxEx12.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx11.Text, this.comboBoxEx12.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx13.DataSource = dtGroup;
            this.comboBoxEx13.ValueMember = "GroupName";
            this.comboBoxEx13.DisplayMember = "GroupName";
            this.comboBoxEx13.SelectedIndex = -1;
        }
        private void comboBoxEx13_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx11.Text == "" || this.comboBoxEx12.Text == "" || this.comboBoxEx13.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx11.Text, this.comboBoxEx12.Text, this.comboBoxEx13.Text);
            string[] columNames = new string[] { "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx14.DataSource = dtPerson;
            this.comboBoxEx14.ValueMember = "UserType";
            this.comboBoxEx14.DisplayMember = "UserType";
            this.comboBoxEx14.SelectedIndex = -1;
        }
////////////////////////////////////////////////////考勤信息录入//////////////////////////////////////////////////////////////////////////
 ////////////////////////////////////////////////////基本工资信息//////////////////////////////////////////////////////////////////////////
        private void comboBoxEx35_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx35.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx34.DataSource = dtPart;
            this.comboBoxEx34.ValueMember = "DepartmentName";
            this.comboBoxEx34.DisplayMember = "DepartmentName";
            this.comboBoxEx34.SelectedIndex = -1;
        }

        private void comboBoxEx34_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx35.Text == "" || this.comboBoxEx34.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx35.Text, this.comboBoxEx34.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx33.DataSource = dtGroup;
            this.comboBoxEx33.ValueMember = "GroupName";
            this.comboBoxEx33.DisplayMember = "GroupName";
            this.comboBoxEx33.SelectedIndex = -1;
        }

        private void comboBoxEx33_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx35.Text == "" || this.comboBoxEx34.Text == "" || this.comboBoxEx33.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx35.Text, this.comboBoxEx34.Text, this.comboBoxEx33.Text);
            string[] columNames = new string[] { "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx32.DataSource = dtPerson;
            this.comboBoxEx32.ValueMember = "UserType";
            this.comboBoxEx32.DisplayMember = "UserType";
            this.comboBoxEx32.SelectedIndex = -1;
        }

        private void dataGridViewX6_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX6.SelectedRows.Count == 0)
                return;
            this.comboBoxEx35.Text = this.dataGridViewX6.SelectedRows[0].Cells[1].Value.ToString();
            this.comboBoxEx34.Text = this.dataGridViewX6.SelectedRows[0].Cells[2].Value.ToString();
            this.comboBoxEx33.Text = this.dataGridViewX6.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx32.Text = this.dataGridViewX6.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX23.Text = this.dataGridViewX6.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX24.Text = this.dataGridViewX6.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX25.Text = this.dataGridViewX6.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX26.Text = this.dataGridViewX6.SelectedRows[0].Cells[8].Value.ToString();
        }

        private void buttonX18_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要新增吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("insert into Pub_BaseSalary values('{0}','{1}','{2}','{3}',{4},{5},{6},{7},'{8}',getdate())",
                    this.comboBoxEx35.Text, this.comboBoxEx34.Text, this.comboBoxEx33.Text, this.comboBoxEx32.Text,
                    this.textBoxX23.Text, this.textBoxX24.Text, this.textBoxX25.Text, this.textBoxX26.Text, frmUTSOFTMAIN.StaffID);
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if(resultNum>0)
            {
                MessageBox.Show("新增成功");
                string sqlFlush = string.Format("select * from Pub_BaseSalary order by id desc");
                this.dataGridViewX6.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("CompanyNames = '{0}',"+
                    "DepartmentName = '{1}',GroupName = '{2}',UserType = '{3}',"+
                    "jbsalary = '{4}',gwgz = '{5}',cb = '{6}',fb = '{7}'", 
                    this.comboBoxEx35.Text, 
                    this.comboBoxEx34.Text, 
                    this.comboBoxEx33.Text, 
                    this.comboBoxEx32.Text,
                    this.textBoxX23.Text, 
                    this.textBoxX24.Text, 
                    this.textBoxX25.Text, 
                    this.textBoxX26.Text);
                frmUTSOFTMAIN.OperationObject = this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "新增:" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX18);
                //--------------日志结束------------------
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX17_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要删除选中行的数据的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_BaseSalary where id = {0}",this.dataGridViewX6.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}',",
                    this.dataGridViewX6.SelectedRows[0].Cells[1].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[2].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[3].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[4].Value.ToString());
                frmUTSOFTMAIN.OperationObject = "删除对象："+this.textBoxX1.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "删除成功";
                frmUTSOFTMAIN.addlog(this.buttonX17);
                //--------------日志结束------------------
                this.dataGridViewX6.Rows.Remove(this.dataGridViewX6.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX16_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要修改选中行的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update Pub_BaseSalary set CompanyNames = '{0}',DepartmentName = '{1}',GroupName = '{2}',UserType = '{3}'," +
                "jbsalary = '{4}',gwgz = '{5}',cb = '{6}',fb = '{7}' where id = {8}",
                this.comboBoxEx35.Text, this.comboBoxEx34.Text, this.comboBoxEx33.Text, this.comboBoxEx32.Text,
                this.textBoxX23.Text, this.textBoxX24.Text, this.textBoxX25.Text, this.textBoxX26.Text,
                this.dataGridViewX6.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("修改成功");

                //--------------日志开始------------------
                string operationRemarkStr1 = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}'," +
                    "基本工资 = '{4}',岗位工资 = '{5}',餐补 = '{6}',房补 = '{7}'",
                    this.dataGridViewX6.SelectedRows[0].Cells[1].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[2].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[3].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[4].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[5].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[6].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[7].Value.ToString(),
                    this.dataGridViewX6.SelectedRows[0].Cells[8].Value.ToString());
                string operationRemarkStr2 = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}'," +
                    "基本工资 = '{4}',岗位工资 = '{5}',餐补 = '{6}',房补 = '{7}'",
                    this.comboBoxEx35.Text,
                    this.comboBoxEx34.Text,
                    this.comboBoxEx33.Text,
                    this.comboBoxEx32.Text,
                    this.textBoxX23.Text,
                    this.textBoxX24.Text,
                    this.textBoxX25.Text,
                    this.textBoxX26.Text);
                frmUTSOFTMAIN.OperationObject = "修改前：" + operationRemarkStr1;
                frmUTSOFTMAIN.OperationRemark = "修改后：" + operationRemarkStr2;
                frmUTSOFTMAIN.addlog(this.buttonX16);
                //--------------日志结束------------------

                this.dataGridViewX6.SelectedRows[0].Cells[1].Value = this.comboBoxEx35.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[2].Value = this.comboBoxEx34.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[3].Value = this.comboBoxEx33.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[4].Value = this.comboBoxEx32.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[5].Value = this.textBoxX23.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[6].Value = this.textBoxX24.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[7].Value = this.textBoxX25.Text;
                this.dataGridViewX6.SelectedRows[0].Cells[8].Value = this.textBoxX26.Text;
                
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }
        }

        private void buttonX13_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Pub_BaseSalary where "+
                " CompanyNames like '%{0}%' and DepartmentName like '%{1}%' and GroupName like '%{2}%' and UserType like '%{3}%'"+
                " order by CompanyNames,DepartmentName,GroupName desc",
                this.comboBoxEx35.Text, this.comboBoxEx34.Text, this.comboBoxEx33.Text, this.comboBoxEx32.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX6.DataSource = dt1;
        }

        private void buttonX19_Click(object sender, EventArgs e)//查看工资表
        {
            InWard_AllSalaryTable iwast = new InWard_AllSalaryTable();
            iwast.Show();
        }

        private void comboBoxEx22_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx22.Text == "迟到" || this.comboBoxEx22.Text == "早退")
            {
                this.labelX76.Text = "分钟";
                this.textBoxX20.Enabled = true;
            }
            else if (this.comboBoxEx22.Text == "病假" || this.comboBoxEx22.Text == "事假" ||
                this.comboBoxEx22.Text == "加班" || this.comboBoxEx22.Text == "旷工")
            {
                this.labelX76.Text = "小时";
                this.textBoxX20.Enabled = true;
            }
            else if (this.comboBoxEx22.Text == "保险" || this.comboBoxEx22.Text == "优异奖金" || this.comboBoxEx22.Text == "扣补工资")
            {
                this.textBoxX20.Text = "0";
                this.textBoxX20.Enabled = false;
                this.labelX76.Text = "";
            }
            else
            {
                this.textBoxX20.Enabled = true;
            }
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

        private void comboBoxEx10_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx10.DataSource = dtCompany.Copy();
            this.comboBoxEx10.DisplayMember = "CompanyNames";
            this.comboBoxEx10.ValueMember = "CompanyNames";
            this.comboBoxEx10.SelectedIndex = -1;
        }

        private void comboBoxEx35_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx35.DataSource = dtCompany.Copy();
            this.comboBoxEx35.DisplayMember = "CompanyNames";
            this.comboBoxEx35.ValueMember = "CompanyNames";
            this.comboBoxEx35.SelectedIndex = -1;
        }

        private void comboBoxEx11_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx11.DataSource = dtCompany.Copy();
            this.comboBoxEx11.DisplayMember = "CompanyNames";
            this.comboBoxEx11.ValueMember = "CompanyNames";
            this.comboBoxEx11.SelectedIndex = -1;
        }

        private void comboBoxEx20_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx20.DataSource = dtCompany.Copy();
            this.comboBoxEx20.DisplayMember = "CompanyNames";
            this.comboBoxEx20.ValueMember = "CompanyNames";
            this.comboBoxEx20.SelectedIndex = -1;
        }
        private void comboBoxEx23_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx23.DataSource = dtCompany.Copy();
            this.comboBoxEx23.DisplayMember = "CompanyNames";
            this.comboBoxEx23.ValueMember = "CompanyNames";
            this.comboBoxEx23.SelectedIndex = -1;
        }

        private void buttonX15_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("delete from SYS_FromPower");
            DBHelper.ExecuteUpdate(sql1);
            string companyType = "";
            string fromName = "";
            string daysLength = "";
            string sql2 = "insert into SYS_FromPower (CompanyType,FromName,DaysLength)";
            foreach (Control tmpControl in this.panel18.Controls) 
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    companyType = tmpControl.Text;
                    foreach (Control panelcon in tmpControl.Controls)
                    {
                        if (panelcon is Panel)
                        {
                            foreach (Control late in panelcon.Controls)
                            {
                                try
                                { daysLength = int.Parse(late.Text).ToString(); }
                                catch { fromName = late.Text; }
                            }
                            sql2 += string.Format(" select '{0}','{1}',{2} union all ",companyType, fromName, daysLength);
                        }
                    }
                }
            }
            sql2 = sql2.Substring(0, sql2.Length - 10);
            int resultNum = DBHelper.ExecuteUpdate(sql2);
            if (resultNum > 0)
            {
                MessageBox.Show("修改权限成功");
                //--------------日志开始------------------
                frmUTSOFTMAIN.OperationObject = "";
                frmUTSOFTMAIN.OperationRemark = "修改权限成功";
                frmUTSOFTMAIN.addlog(this.buttonX15);
                //--------------日志结束------------------
            }
            else
            {
                MessageBox.Show("修改权限失败");
            }
        }
        private void fromShow() 
        {
            string sql1 = string.Format("select CompanyType,FromName,DaysLength from SYS_FromPower");
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            string companyType = "";
            string fromName = "";
            foreach (Control tmpControl in this.panel18.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    companyType = tmpControl.Text;
                    foreach (Control panelcon in tmpControl.Controls)
                    {
                        if (panelcon is Panel)
                        {
                            foreach (Control late in panelcon.Controls)
                            {
                                if (late is DevComponents.DotNetBar.LabelX) 
                                {
                                    fromName = late.Text;
                                }
                            }
                            foreach (Control late in panelcon.Controls)
                            {
                                if (late is DevComponents.DotNetBar.Controls.TextBoxX) 
                                {
                                    string sql = string.Format("CompanyType = '{0}' and FromName = '{1}'", companyType, fromName);
                                    string[] columNames = new string[] { "DaysLength" };
                                    DataTable dtFromName = new DataTable();
                                    try { dtFromName = tools.dtFilter(dt1, columNames, sql); }
                                    catch { }
                                    if (dtFromName.Rows.Count != 0)
                                        late.Text = dtFromName.Rows[0][0].ToString();
                                    else
                                        late.Text = "0";
                                }
                            }
                        }
                    }
                }
            }
        }

        private void buttonX14_Click(object sender, EventArgs e)
        {
            Admin_BatchAdd aba = new Admin_BatchAdd();
            aba.Text = this.buttonX14.Text;
            aba.Show();
        }

        private void comboBoxEx18_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx18.Text == "本月")
            {
                this.comboBoxEx19.Text = "次数";
                this.textBoxX17.Text = "0";
                this.textBoxX18.Text = "1000";
                this.labelX63.Text = "次";
                this.labelX64.Text = "次";
            }
            else if (this.comboBoxEx18.Text == "上月")
            {
                this.comboBoxEx19.Text = "百分比";
                this.labelX63.Text = "%";
                this.labelX64.Text = "%";
            }
        }

        private void buttonX20_Click(object sender, EventArgs e)
        {
            string sqlFlush = string.Format("select * from Pub_JXCaculate where " +
                "CompanyNames like '%{0}%' and DepartmentName like '%{1}%' and GroupName like '%{2}%' " +
                "and UserType like '%{3}%' and JXType like '%{4}%' order by SubmitTime desc",
                this.comboBoxEx11.Text, this.comboBoxEx12.Text, this.comboBoxEx13.Text,
                this.comboBoxEx14.Text, this.comboBoxEx15.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sqlFlush);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX3.DataSource = dt1;
        }

        private void dateTimeInput1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimeInput2.Value = this.dateTimeInput1.Value.AddMonths(2).AddDays(1 - this.dateTimeInput1.Value.AddMonths(2).Day);
        }

        private void buttonX21_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX3.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要增加修改选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update Pub_JXCaculate set CompanyNames = '{1}', DepartmentName = '{2}',GroupName = '{3}',UserType = '{4}',"+
                "JXType = '{5}',JXTypeSmall = '{6}',JXCardinalNum = '{7}',DCCardinalNum = '{8}',DCCardinalType = '{9}',"+
                "LowerLimit = {10},UpperLimit = {11},Value = {12} where id = {0}",
                this.dataGridViewX3.SelectedRows[0].Cells[0].Value.ToString(),
                this.comboBoxEx11.Text.Trim(),
                this.comboBoxEx12.Text.Trim(),
                this.comboBoxEx13.Text.Trim(),
                this.comboBoxEx14.Text.Trim(),
                this.comboBoxEx15.Text.Trim(),
                this.comboBoxEx16.Text.Trim(),
                this.comboBoxEx17.Text.Trim(),
                this.comboBoxEx18.Text.Trim(),
                this.comboBoxEx19.Text.Trim(),
                this.textBoxX17.Text.Trim(),
                this.textBoxX18.Text.Trim(),
                this.textBoxX19.Text.Trim());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("修改成功!");

                //--------------日志开始------------------
                string operationRemarkStr1 = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}'," +
                    "绩效大类 = '{4}',绩效小类 = '{5}',绩效基数 = '{6}',档次基准 = '{7}'," +
                    "档次类型 = '{8}',下限 = '{9}',上限 = '{10}',单价 = '{11}'",
                    this.dataGridViewX3.SelectedRows[0].Cells[1].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[2].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[3].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[4].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[5].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[6].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[7].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[8].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[9].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[10].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[11].Value.ToString(),
                    this.dataGridViewX3.SelectedRows[0].Cells[12].Value.ToString());
                string operationRemarkStr2 = string.Format("公司 = '{0}'," +
                    "部门 = '{1}',小组 = '{2}',职位 = '{3}'," +
                    "绩效大类 = '{4}',绩效小类 = '{5}',绩效基数 = '{6}',档次基准 = '{7}'," +
                    "档次类型 = '{8}',下限 = '{9}',上限 = '{10}',单价 = '{11}'",
                    this.comboBoxEx11.Text, this.comboBoxEx12.Text, this.comboBoxEx13.Text, this.comboBoxEx14.Text,
                    this.comboBoxEx15.Text, this.comboBoxEx16.Text, this.comboBoxEx17.Text, this.comboBoxEx18.Text,
                    this.comboBoxEx19.Text, this.textBoxX17.Text.Trim(), this.textBoxX18.Text.Trim(), this.textBoxX19.Text.Trim());

                frmUTSOFTMAIN.OperationObject = "修改前：" + operationRemarkStr1;
                frmUTSOFTMAIN.OperationRemark = "修改后：" + operationRemarkStr2;
                frmUTSOFTMAIN.addlog(this.buttonX21);
                //--------------日志结束------------------

                this.dataGridViewX3.SelectedRows[0].Cells[1].Value = this.comboBoxEx11.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[2].Value = this.comboBoxEx12.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[3].Value = this.comboBoxEx13.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[4].Value = this.comboBoxEx14.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[5].Value = this.comboBoxEx15.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[6].Value = this.comboBoxEx16.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[7].Value = this.comboBoxEx17.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[8].Value = this.comboBoxEx18.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[9].Value = this.comboBoxEx19.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[10].Value = this.textBoxX17.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[11].Value = this.textBoxX18.Text;
                this.dataGridViewX3.SelectedRows[0].Cells[12].Value = this.textBoxX19.Text;
                frmUTSOFTMAIN.jxCaculateType = DBHelper.ExecuteQuery("select * from Pub_JXCaculate");
                
            }
        }
        private void salaryAll() 
        {
            double a1 = 0;
            for (int i = 0; i < this.dataGridViewX4.Rows.Count; i++)
            {
                a1 += double.Parse(this.dataGridViewX4.Rows[i].Cells[8].Value.ToString());
            }
            this.labelX66.Text = "合计： "+ a1.ToString() +" 元";
        }
        private void buttonX22_Click(object sender, EventArgs e)
        {
            Admin_JXCaculate_BatchAdd ajba = new Admin_JXCaculate_BatchAdd();
            ajba.Show();
        }

        private void dateTimeInput13_ValueChanged(object sender, EventArgs e)
        {
            this.comboBoxEx22.Text = "";
            this.labelX76.Text = "无";
            this.textBoxX20.Text = "";
            this.textBoxX21.Text = "0";
            this.textBoxX22.Text = "";
        }
    }
}
