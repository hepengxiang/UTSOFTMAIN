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
    public partial class InWard_AllSalaryTable : Form
    {
        public InWard_AllSalaryTable()
        {
            InitializeComponent();
        }
        //上个月的第一天
        private static DateTime beforeMonthFirstDay = System.DateTime.Now.AddMonths(-1).AddDays(1 - System.DateTime.Now.AddMonths(-1).Day);

        //这个月的第一天
        private static DateTime thisMonthFirstDay = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);

        private void InWard_AllSalaryTable_Load(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx3.DataSource = dtCompany.Copy();
            this.comboBoxEx3.DisplayMember = "CompanyNames";
            this.comboBoxEx3.ValueMember = "CompanyNames";
            this.comboBoxEx3.SelectedIndex = -1;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.textBoxX21.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            this.textBoxX9.Text = this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString();
            this.textBoxX10.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX11.Text = this.dataGridViewX1.SelectedRows[0].Cells[17].Value.ToString();
            this.textBoxX12.Text = this.dataGridViewX1.SelectedRows[0].Cells[18].Value.ToString();
        }

        private void dataGridViewX1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10 || e.ColumnIndex == 11 || e.ColumnIndex == 12 || e.ColumnIndex == 13 || e.ColumnIndex == 16 || e.ColumnIndex == 17 || e.ColumnIndex == 18)
            {
                string sql1 = "";
                int years = DateTime.Parse(this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString()).Year;
                int months = DateTime.Parse(this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString()).Month;
                if (e.ColumnIndex == 10)
                {
                    string sql2 = string.Format("select a.id,a.CompanyNames,a.JXTime,b.NickName,a.QQ,b.JoinTime,c.Name,a.EnterType,"+
                        " a.EnterTypeSmall,a.Value,a.Amount,a.Remark,d.Name as jxName from Pub_JXManager a "+
                        " left join Pub_VIPMessage b on a.QQ = b.QQ"+
                        " left join Users c on a.StaffID = c.StaffID"+
                        " left join Users d on a.AmountManager = d.StaffID"+
                        " where datepart(yy,a.JXTime)={0} and datepart(mm,a.JXTime)={1}"+
                        " and b.CompanyNames = '{2}' and a.AmountManager = '{3}'",
                        years, months, this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(),
                         this.dataGridViewX1.Rows[e.RowIndex].Cells[22].Value.ToString());
                    Pub_JXSalaryDetail pjxsd = new Pub_JXSalaryDetail();
                    pjxsd.dtSource = DBHelper.ExecuteQuery(sql2);
                    pjxsd.Show();
                    return;
                }
                if (e.ColumnIndex == 11)
                {
                    sql1 = string.Format("select a.*,b.Name from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID"+
                        " where datepart(yy,a.KQJXTime) = {1} and datepart(mm,a.KQJXTime) = {2}"+
                        " and a.StaffID = '{0}' and KQJXType in ('病假','事假')",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(), years,months);
                }
                else if (e.ColumnIndex == 12)
                {
                    sql1 = string.Format("select a.*,b.Name from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                        " where datepart(yy,a.KQJXTime) = {1} and datepart(mm,a.KQJXTime) = {2}" +
                        " and a.StaffID = '{0}' and KQJXType in ('迟到')",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(), years, months);
                }
                else if (e.ColumnIndex == 13)
                {
                    sql1 = string.Format("select a.*,b.Name from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                        " where datepart(yy,a.KQJXTime) = {1} and datepart(mm,a.KQJXTime) = {2}" +
                        " and a.StaffID = '{0}' and KQJXType in ('基础加班','工作日加班','公休日加班','法定节假日加班')",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(), years, months);
                }
                else if (e.ColumnIndex == 16)
                {
                    sql1 = string.Format("select a.*,b.Name from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                        " where datepart(yy,a.KQJXTime) = {1} and datepart(mm,a.KQJXTime) = {2}" +
                        " and a.StaffID = '{0}' and KQJXType in ('保险')",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(), years, months);
                }
                else if (e.ColumnIndex == 17)
                {
                    sql1 = string.Format("select a.*,b.Name from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                        " where datepart(yy,a.KQJXTime) = {1} and datepart(mm,a.KQJXTime) = {2}" +
                        " and a.StaffID = '{0}' and KQJXType in ('优异奖金')",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(), years, months);
                }
                else if (e.ColumnIndex == 18)
                {
                    sql1 = string.Format("select a.*,b.Name from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                        " where datepart(yy,a.KQJXTime) = {1} and datepart(mm,a.KQJXTime) = {2}" +
                        " and a.StaffID = '{0}' and KQJXType in ('扣补工资')",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[2].Value.ToString(), years, months);
                }
                DataTable dtSendAll = DBHelper.ExecuteQuery(sql1);
                Pub_WorkSalaryDetail pwsd = new Pub_WorkSalaryDetail();
                pwsd.dtSource = dtSendAll;
                pwsd.Show();
            }
        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx3.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "DepartmentName";
            this.comboBoxEx4.DisplayMember = "DepartmentName";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx3.Text == "" || this.comboBoxEx4.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx3.Text, this.comboBoxEx4.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx5.DataSource = dtGroup;
            this.comboBoxEx5.ValueMember = "GroupName";
            this.comboBoxEx5.DisplayMember = "GroupName";
            this.comboBoxEx5.SelectedIndex = -1;
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx3.Text == "" || this.comboBoxEx4.Text == "" || this.comboBoxEx5.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx3.Text, this.comboBoxEx4.Text, this.comboBoxEx5.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx6.DataSource = dtPerson;
            this.comboBoxEx6.ValueMember = "StaffID";
            this.comboBoxEx6.DisplayMember = "AssumedName";
            this.comboBoxEx6.SelectedIndex = -1;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选定记录");
                return;
            }
            if (MessageBox.Show("确定要修改选中行的记录吗？", "修改工资表提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update Pub_YearSalary set jbsalary = {1}, gwgz = {2},cb = {3},fb = {4},qqj = {5}," +
                    "zjx = {6}, qjgz = {7}, cdgz = {8}, jbgz = {9}, scqts = {10}, bxbt = {11}, yyjj = {12}, kbgz = {13} where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    double.Parse(this.textBoxX21.Text.Trim()),
                    double.Parse(this.textBoxX1.Text.Trim()),
                    double.Parse(this.textBoxX2.Text.Trim()),
                    double.Parse(this.textBoxX3.Text.Trim()),
                    double.Parse(this.textBoxX4.Text.Trim()),
                    double.Parse(this.textBoxX5.Text.Trim()),
                    double.Parse(this.textBoxX6.Text.Trim()),
                    double.Parse(this.textBoxX7.Text.Trim()),
                    double.Parse(this.textBoxX8.Text.Trim()),
                    double.Parse(this.textBoxX9.Text.Trim()),
                    double.Parse(this.textBoxX10.Text.Trim()),
                    double.Parse(this.textBoxX11.Text.Trim()),
                    double.Parse(this.textBoxX12.Text.Trim()));
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
                string sqlupsa = string.Format("update Pub_YearSalary set" +
                    " yfgz = jbsalary + gwgz + cb + fb + qqj + grjx + tdjx + qjgz + cdgz + jbgz," +
                    " sfgz = jbsalary + gwgz + cb + fb + qqj + grjx + tdjx + qjgz + cdgz + jbgz + bxbt + yyjj + kbgz " +
                    " where id = {0}", this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                DBHelper.ExecuteUpdate(sqlupsa);
                string sqlFlush = string.Format("select a.*,b.Name,b.CompanyNames from Pub_YearSalary a left join Users b on a.StaffID = b.StaffID where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "";
                frmUTSOFTMAIN.OperationRemark = "修改工资表成功" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
            }

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if(this.dataGridViewX1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选定记录");
                return;
            }
            if (MessageBox.Show("你确定要删除【" + this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString() + "】的工资数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_YearSalary where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "修改工资表成功" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)//生成工资表
        {
            //上个月的第一天
            DateTime dtbeforemonthfirstday = System.DateTime.Now.AddMonths(-1).AddDays(1 - System.DateTime.Now.AddMonths(-1).Day);//上个月的第一天
            //这个月的第一天
            DateTime dtthismonthfirstday = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);//这个月的第一天
            //上个月的年份，上个月的月份
            int lastMonthYearStr = System.DateTime.Now.AddMonths(-1).Year;
            int lastMonthMonthStr = System.DateTime.Now.AddMonths(-1).Month;
            //查询上月份工资表是否已生成过
            string sqlCheckYearSalary = string.Format("select * from Pub_YearSalary where datepart(yy,Times) = {0} and datepart(mm,Times) = {1}",
                lastMonthYearStr, lastMonthMonthStr);
            DataTable dtCheckYearSalary = DBHelper.ExecuteQuery(sqlCheckYearSalary);
            if (dtCheckYearSalary.Rows.Count > 0)
            {
                if (MessageBox.Show("上月份工资表已生成过！是否需要重新生成？", "重新生成工资表提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                string sqlDeleteLstMonthYearSalary = string.Format("delete from Pub_YearSalary where datepart(yy,Times)={0} and datepart(mm,Times)={1}",
                    lastMonthYearStr, lastMonthMonthStr);
                DBHelper.ExecuteUpdate(sqlDeleteLstMonthYearSalary);
            }
            else
            {
                if (MessageBox.Show("确定要生成【" + System.DateTime.Now.AddMonths(-1).Month + "月份工资表】吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            //查询出所有需要生成工资表的人，在职，离职日期在上个月，入职日期在这个月之前，
            string sqlpersonall = string.Format("select * from Users where"+ 
                " (onjob = 1 and EmploymentDate <='{0}') or"+
                " (onjob = 0 and LeaveTime>='{1}')" + 
                " and UserType != '系统管理员'",
                dtthismonthfirstday.ToShortDateString(), dtbeforemonthfirstday.ToShortDateString());
            DataTable dtpersonall = DBHelper.ExecuteQuery(sqlpersonall);


            //查询基本工资信息
            string sqlBaseSalary = string.Format("select CompanyNames,DepartmentName ,GroupName,UserType,"+
                " coalesce(jbsalary,0) as jbsalary,coalesce(gwgz,0) as gwgz, "+
                " coalesce(cb,0) as  cb,coalesce(fb,0) as fb from Pub_BaseSalary");
            DataTable dtBaseSalary = DBHelper.ExecuteQuery(sqlBaseSalary);


            //查询预定报名绩效
            string sqlYDBMJXSalary = string.Format("select a.AmountManager,coalesce(sum(Amount),0) as 预定报名绩效工资 from"+ 
                " Pub_YDBM_JXManager a left join Pub_VIPMessage b on a.ForID = b.id"+
                " where datepart(yy,b.Jointime) = {0} and datepart(mm,b.Jointime) = {1}" +
                " group by a.AmountManager", lastMonthYearStr, lastMonthMonthStr);
            DataTable dtYDBMJXSalary = DBHelper.ExecuteQuery(sqlYDBMJXSalary);


            //查询投诉退款绩效
            string sqlTSTKJXSalary = string.Format("select a.AmountManager,coalesce(sum(Amount),0) as 投诉退款绩效工资 from"+ 
                " Pub_TSTK_JXManager a left join Pub_ComplainRefundInfo b on a.ForID = b.id"+
                " where datepart(yy,b.SubmitTime) = {0} and datepart(mm,b.SubmitTime) = {1}" +
                " group by a.AmountManager", lastMonthYearStr, lastMonthMonthStr);
            DataTable dtTSTKJXSalary = DBHelper.ExecuteQuery(sqlTSTKJXSalary);


            //查询考勤和其他扣补获取
            string sqlworksalary = string.Format("select StaffID," +
                " coalesce(sum(case when KQJXType in ('全勤奖') then KQJXValue else 0 end),0) as 全勤奖," +
                " coalesce(sum(case when KQJXType in ('病假','事假') then KQJXValue else 0 end),0) as 请假工资," +   
                " coalesce(sum(case when KQJXType in ('迟到') then KQJXValue else 0 end),0) as 迟到工资,   " +
                " coalesce(sum(case when KQJXType in ('基础加班','工作日加班','公休日加班','法定节假日加班') then KQJXValue else 0 end),0) as 加班工资 ,  " +
                " coalesce(sum(case when KQJXType in ('保险') then KQJXValue else 0 end),0) as 保险," +  
                " coalesce(sum(case when KQJXType in ('优异奖金') then KQJXCount else 0 end),0) as 优异奖金," +
                " coalesce(sum(case when KQJXType in ('扣补工资') then KQJXValue else 0 end),0) as 扣补工资," +
                " (25.75-(sum(case when KQJXType in ('病假','事假') then KQJXCount*0.125 else 0 end))) as 出勤天数" +
                " from Pub_WorkSalary" +
                " where datepart(yy,KQJXTime) = {0} and datepart(mm,KQJXTime) = {1}" +
                " group by StaffID",lastMonthYearStr, lastMonthMonthStr);
            DataTable dtworksalary = DBHelper.ExecuteQuery(sqlworksalary);
            
            this.progressBarX1.Value = 0;
            this.progressBarX1.Maximum = dtpersonall.Rows.Count;
            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Step = 1;

            for (int i = 0; i < dtpersonall.Rows.Count; i++)
            {
                DateTime Times; string sfzStr; int yfgz = 0, sfgz = 0;
                double jbsalary = 0, gwgz = 0, cb = 0, fb = 0, qqj = 0, grjx = 0, tdjx = 0, zjx = 0, qjgz = 0, cdgz = 0, jbgz = 0, scqts = 0, bxbt = 0, yyjj = 0, kbgz = 0;

                Times = DateTime.Now.AddMonths(-1);                            //时间
                sfzStr = dtpersonall.Rows[i][6].ToString();                      //身份证

                //基本类工资信息获取
                try
                {
                    string sqlOneBaseSalary = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}' and UserType = '{3}'",
                        dtpersonall.Rows[i][0].ToString(), dtpersonall.Rows[i][1].ToString(),
                        dtpersonall.Rows[i][2].ToString(), dtpersonall.Rows[i][3].ToString());
                    string[] oneBaseSalaryColumNames = new string[] { "jbsalary", "gwgz", "cb", "fb" };
                    DataTable dtOneBaseSalary = tools.dtFilter(dtBaseSalary, oneBaseSalaryColumNames, sqlOneBaseSalary);
                    jbsalary = double.Parse(dtOneBaseSalary.Rows[0][0].ToString());  //基本工资
                    gwgz = double.Parse(dtOneBaseSalary.Rows[0][1].ToString());      //岗位工资
                    cb = double.Parse(dtOneBaseSalary.Rows[0][2].ToString());        //餐补
                    fb = double.Parse(dtOneBaseSalary.Rows[0][3].ToString());        //房补
                }
                catch
                {
                    jbsalary = 1250;  //基本工资
                    gwgz = 0;      //岗位工资
                    cb = 0;        //餐补
                    fb = 0;        //房补 
                }

                //预定报名绩效获取          
                try 
                {
                    string sqlOneYDBMJXSalary = string.Format("AmountManager = '{0}'", dtpersonall.Rows[i][6].ToString());
                    string[] oneYDBMJXSalaryColumNames = new string[] { "预定报名绩效工资" };
                    DataTable dtOneYDBMJXSalary = tools.dtFilter(dtYDBMJXSalary, oneYDBMJXSalaryColumNames, sqlOneYDBMJXSalary);
                    grjx += double.Parse(dtOneYDBMJXSalary.Rows[0][0].ToString());
                }
                catch
                {
                    grjx += 0;
                }

                //投诉退款绩效获取
                try
                {
                    string sqlOneTSTKJXSalary = string.Format("AmountManager = '{0}'", dtpersonall.Rows[i][6].ToString());
                    string[] oneTSTKJXSalaryColumNames = new string[] { "投诉退款绩效工资" };
                    DataTable dtOneTSTKJXSalary = tools.dtFilter(dtTSTKJXSalary, oneTSTKJXSalaryColumNames, sqlOneTSTKJXSalary);
                    grjx += double.Parse(dtOneTSTKJXSalary.Rows[0][0].ToString());
                }
                catch
                {
                    grjx += 0;
                }
                

                //考勤和其他扣补获取
                try
                {
                    string sqlOneWorksalary = string.Format("StaffID = '{0}'", dtpersonall.Rows[i][6].ToString());
                    string[] oneWorksalaryColumNames = new string[] { "全勤奖", "请假工资", "迟到工资", "加班工资", "保险", "优异奖金", "扣补工资", "出勤天数" };
                    DataTable dtOneWorksalary = tools.dtFilter(dtworksalary, oneWorksalaryColumNames, sqlOneWorksalary);
                    qqj = double.Parse(dtOneWorksalary.Rows[0][0].ToString());       //全勤奖
                    qjgz = double.Parse(dtOneWorksalary.Rows[0][1].ToString());      //请假工资
                    cdgz = double.Parse(dtOneWorksalary.Rows[0][2].ToString());      //迟到工资
                    jbgz = double.Parse(dtOneWorksalary.Rows[0][3].ToString());      //加班工资
                    bxbt = double.Parse(dtOneWorksalary.Rows[0][4].ToString());      //保险
                    yyjj = double.Parse(dtOneWorksalary.Rows[0][5].ToString());      //优异奖金
                    kbgz = double.Parse(dtOneWorksalary.Rows[0][6].ToString());      //扣补工资
                    scqts = double.Parse(dtOneWorksalary.Rows[0][7].ToString());     //出勤天数
                }
                catch
                {
                    qqj = 0; qjgz = 0; cdgz = 0; jbgz = 0; bxbt = 0; yyjj = 0; kbgz = 0; scqts = 0;
                }
                zjx = (double)(grjx + tdjx);                                           //总绩效
                yfgz = (int)(jbsalary + gwgz + cb + fb + qqj + grjx + tdjx + qjgz + cdgz + jbgz);//应发工资
                sfgz = (int)(yfgz + bxbt + yyjj + kbgz);                              //实发工资

                string sqlInsertSalary = string.Format("insert into Pub_YearSalary " +
                    "values('{0}','{1}',{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},0)",
                    Times.ToShortDateString(), sfzStr, jbsalary, gwgz, cb, fb, qqj, grjx, tdjx, zjx, qjgz, cdgz, jbgz, scqts, yfgz, bxbt, yyjj, kbgz, sfgz);
                this.progressBarX1.Value += DBHelper.ExecuteUpdate(sqlInsertSalary);
            }
            MessageBox.Show("生成工资表完成！");
            //--------------日志开始------------------
            string operationRemarkStr = string.Format("");
            frmUTSOFTMAIN.OperationObject = "";
            frmUTSOFTMAIN.OperationRemark = "生成工资表完成" + operationRemarkStr;
            frmUTSOFTMAIN.addlog(this.buttonX3);
            //--------------日志结束------------------
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要发布【" + Convert.ToDateTime(DateTime.Now.AddMonths(-1)).ToString("yyyy-MM") + "】的工资表吗?", "发布提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update Pub_YearSalary set PublicState=1 where datepart(yy,Times)={0} and datepart(mm,Times)={1}",
                DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("发布成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = Convert.ToDateTime(DateTime.Now.AddMonths(-1)).ToString("yyyy-MM");
                frmUTSOFTMAIN.OperationRemark = "发布工资表成功" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX4);
                //--------------日志结束------------------
                for (int i = 0; i < this.dataGridViewX1.Rows.Count;i++ )
                {
                    this.dataGridViewX1.Rows[i].Cells[21].Value = 1;
                }
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.*,b.Name,b.CompanyNames from Pub_YearSalary a left join Users b on a.StaffID = b.StaffID where 1>0");
            if (this.comboBoxEx1.Text != "")
                sql1 += string.Format(" and datepart(yy,a.Times) = {0}", this.comboBoxEx1.Text.Trim());
            if (this.comboBoxEx2.Text != "")
                sql1 += string.Format(" and datepart(mm,a.Times) = {0}", this.comboBoxEx2.SelectedIndex+1);
            if (this.comboBoxEx3.Text != "")
                sql1 += string.Format(" and b.CompanyNames = '{0}'", this.comboBoxEx3.Text.Trim());
            if (this.comboBoxEx4.Text != "")
                sql1 += string.Format(" and DepartmentName = '{0}'", this.comboBoxEx4.Text.Trim());
            if (this.comboBoxEx5.Text != "")
                sql1 += string.Format(" and GroupName = '{0}'", this.comboBoxEx5.Text.Trim());
            if (this.comboBoxEx6.Text != "")
                sql1 += string.Format(" and a.StaffID = '{0}'", this.comboBoxEx6.SelectedValue.ToString());
            sql1 += "order by CompanyNames,DepartmentName,GroupName,UserType,AssumedName desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
        }
    }
}
