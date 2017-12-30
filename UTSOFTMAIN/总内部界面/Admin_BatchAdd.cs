using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.总内部界面
{
    public partial class Admin_BatchAdd : Form
    {
        public Admin_BatchAdd()
        {
            InitializeComponent();
        }

        private void Admin_BatchAdd_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now.AddMonths(-1).AddDays(1 - System.DateTime.Now.AddMonths(-1).Day);
            this.dateTimeInput2.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            dtjbsalarytemp = DBHelper.ExecuteQuery("select * from Pub_BaseSalary");
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (this.dataGridViewX1.SelectedRows.Count == 0)
            //    return;
            //this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            //this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            //this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            //this.comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            //this.comboBoxEx5.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            //this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            //this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            //this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            if(this.comboBoxEx1.Text == "")
            {
                MessageBox.Show("请填写公司！");
                return;
            }
            if (this.comboBoxEx5.Text == "")
            {
               MessageBox.Show("请选定绩效类型！");
                return; 
            }
            try
            {
                double a = double.Parse(this.textBoxX1.Text.Trim());
            }
            catch
            {
                MessageBox.Show("请填写正确数量，若无数量，请填写 0！");
                return;
            }
            int resultNum1 = 0;
            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Value = 0;
            this.progressBarX1.Step = 1;
            try
            {
                string sql1 = string.Format("select StaffID from Users where CompanyNames like '%{0}%' and "+
                    "DepartmentName like '%{1}%' and GroupName like '%{2}%' and StaffID like '%{3}%' and UserType != '系统管理员'",
                        this.comboBoxEx1.Text.Trim(), this.comboBoxEx2.Text.Trim(), 
                        this.comboBoxEx3.Text.Trim(), this.comboBoxEx4.SelectedValue);
                DataTable dt1 = DBHelper.ExecuteQuery(sql1);
                this.progressBarX1.Maximum = (this.dateTimeInput2.Value - this.dateTimeInput1.Value).Days * dt1.Rows.Count;
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    DateTime dttem = DateTime.Parse(this.dateTimeInput1.Value.ToShortDateString());
                    if (this.comboBoxEx5.Text == "基础加班")
                    {
                        resultNum1 += insertWorkSalary(
                             this.comboBoxEx1.Text.Trim(),
                             dttem,
                             dt1.Rows[i][0].ToString(),
                             this.comboBoxEx5.Text,
                             double.Parse(this.textBoxX1.Text.Trim()),
                             this.textBoxX2.Text.Trim(),
                             this.textBoxX3.Text.Trim());

                        dttem = dttem.AddDays(1);
                        System.Windows.Forms.Application.DoEvents();
                        this.progressBarX1.Value++;
                    }
                    else
                    {
                        while (dttem <= DateTime.Parse(this.dateTimeInput2.Value.ToShortDateString()))
                        {
                            resultNum1 += insertWorkSalary(
                                 this.comboBoxEx1.Text.Trim(),
                                 dttem,
                                 dt1.Rows[i][0].ToString(),
                                 this.comboBoxEx5.Text,
                                 double.Parse(this.textBoxX1.Text.Trim()),
                                 this.textBoxX2.Text.Trim(),
                                 this.textBoxX3.Text.Trim());

                            dttem = dttem.AddDays(1);
                            System.Windows.Forms.Application.DoEvents();
                            this.progressBarX1.Value++;
                        }
                    }
                }
                this.progressBarX1.Value = (this.dateTimeInput2.Value - this.dateTimeInput1.Value).Days * dt1.Rows.Count;
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功 "+resultNum1+" 条！");
                string sqlFlush = string.Format("select a.id,a.KQJXTime,b.CompanyNames,b.DepartmentName,b.GroupName,b.AssumedName,b.StaffID," +
                " a.KQJXType,a.KQJXCount,a.KQJXValue,a.Remark"+
                " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID"+
                " where b.CompanyNames like '%{0}%' and b.DepartmentName like '%{1}%' and b.GroupName like '%{2}%' and b.StaffID like '%{3}%'"+
                " and charindex('{4}',a.KQJXType)>0",
                this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text, this.comboBoxEx4.SelectedValue, this.comboBoxEx5.Text, 
                this.dateTimeInput1.Value.ToShortDateString(),this.dateTimeInput2.Value.ToShortDateString());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);

                salaryAll();
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private DataTable dtjbsalarytemp = new DataTable();

        private int insertWorkSalary(string CompanyNames,DateTime KQJXTime,string staffIDStr,string KQJXType,double KQJXCount,string KQJXValue,string Remark) 
        {
            try
            {
                double unitjbsalary = 7.75;

                string sqljbsalaryall = string.Format("StaffID = '{0}'", staffIDStr);
                string[] columNamesall = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType" };
                DataTable dtjbsalaryall = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNamesall, sqljbsalaryall);

                string sqljbsalary = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}' and UserType = '{3}'",
                dtjbsalaryall.Rows[0][0].ToString(),
                dtjbsalaryall.Rows[0][1].ToString(),
                dtjbsalaryall.Rows[0][2].ToString(),
                dtjbsalaryall.Rows[0][3].ToString());
                string[] columNames = new string[] { "jbsalary" };
                DataTable dtjbsalary = tools.dtFilter(dtjbsalarytemp, columNames, sqljbsalary);

                if (dtjbsalary.Rows.Count != 0)
                {
                    unitjbsalary = Math.Round(double.Parse(dtjbsalary.Rows[0][0].ToString()) / 21.75 / 8-0.05, 2);
                }
                if (KQJXType == "迟到") 
                {
                    if (KQJXCount <= 10)
                    {
                        //考勤绩效时间,公司,考勤绩效所属人,绩效大类,    绩效数量,绩效金额,    备注,录入人,录入时间
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, 0, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);

                        string sqlTemp = string.Format("select * from Pub_WorkSalary " +
                            "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and " +
                            "KQJXCount <= 10 and KQJXType = '迟到' and StaffID = '{2}'",
                            KQJXTime.Year, KQJXTime.Month, staffIDStr);
                        DataTable dtTemp = DBHelper.ExecuteQuery(sqlTemp);

                        if (dtTemp.Rows.Count > 3)
                        {
                            string sqlUp = string.Format(string.Format("update Pub_WorkSalary set KQJXValue = -10 " +
                                "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and " +
                                "KQJXCount <= 10 and KQJXType = '迟到' and StaffID = '{2}'",
                                KQJXTime.Year,KQJXTime.Month,staffIDStr));
                            DBHelper.ExecuteUpdate(sqlUp);
                        }
                        return resultNum;
                    }
                    else if (KQJXCount > 10 && KQJXCount <= 30)
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, -10, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                    else// if (KQJXCount > 30)
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, -25, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                }
                else if (KQJXType == "早退")
                {
                    if (KQJXCount <= 30)
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, 0, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);

                        string sqlTemp = string.Format("select * from Pub_WorkSalary " +
                            "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and KQJXCount <= 30 and KQJXType = '早退' and StaffID = '{2}'",
                            KQJXTime.Year,KQJXTime.Month,staffIDStr);
                        DataTable dtTemp = DBHelper.ExecuteQuery(sqlTemp);

                        if (dtTemp.Rows.Count > 3)
                        {
                            string sqlUp = string.Format("update Pub_WorkSalary set KQJXValue = -10 " +
                                "where DATEPART(yy,KQJXTime) = {0} and DATEPART(mm,KQJXTime) = {1} and KQJXCount <= 10 and KQJXType = '早退' and StaffID = '{2}'",
                                KQJXTime.Year,KQJXTime.Month,staffIDStr);
                            DBHelper.ExecuteUpdate(sqlUp);
                        }
                        return resultNum;
                    }
                    else if (KQJXCount > 30 && KQJXCount <= 60)
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, -10, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                    else// if (KQJXCount > 60)
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, -25, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                }
                else if (KQJXType == "旷工")
                {
                    string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},-{5},'{6}','{7}',getdate())",
                        CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount,KQJXCount * unitjbsalary * 2,Remark,frmUTSOFTMAIN.StaffID);
                    int resultNum = DBHelper.ExecuteUpdate(sql1);
                    return resultNum;
                }
                else if (KQJXType == "病假")
                {
                    string datebj = Convert.ToDateTime(KQJXTime.ToShortDateString()).ToString("yyyyMMdd");
                    string resultbj = tools.IsHoliday(datebj);
                    string numbj = resultbj.Substring(resultbj.Length - 3, 1);
                    if (numbj == "1")
                    {
                        string sqlbj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},-{5},'{6}','{7}',getdate())",
                            CompanyNames,KQJXTime.ToShortDateString(),staffIDStr,KQJXType,KQJXCount, KQJXCount * unitjbsalary * 2,"(公休日病假)" + Remark,frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sqlbj);
                        return resultNum;
                    }
                    else
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},-{5},'{6}','{7}',getdate())",
                                CompanyNames,KQJXTime.ToShortDateString(),staffIDStr,KQJXType,KQJXCount,KQJXCount * unitjbsalary / 2,Remark,frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                }
                else if (KQJXType == "事假")
                {
                    string datesj = Convert.ToDateTime(KQJXTime.ToShortDateString()).ToString("yyyyMMdd");
                    string resultsj = tools.IsHoliday(datesj);
                    string numsj = resultsj.Substring(resultsj.Length - 3, 1);
                    if (numsj == "1")
                    {
                        string sqlsj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},-{5},'{6}','{7}',getdate())",
                                CompanyNames,KQJXTime.ToShortDateString(),staffIDStr,KQJXType,KQJXCount,KQJXCount * unitjbsalary * 2,"(公休日事假)" + Remark,frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sqlsj);
                        return resultNum;
                    }
                    else
                    {
                        string sql4 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},-{5},'{6}','{7}',getdate())",
                                CompanyNames,KQJXTime.ToShortDateString(),staffIDStr,KQJXType,KQJXCount,KQJXCount * unitjbsalary,Remark,frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql4);
                        return resultNum;
                    }
                }
                else if (KQJXType == "加班")
                {
                    string date = Convert.ToDateTime(KQJXTime.ToShortDateString()).ToString("yyyyMMdd");
                    string result = tools.IsHoliday(date);
                    string num = result.Substring(result.Length - 3, 1);
                    if(num == "0")
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','工作日加班',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 1.5, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                    else if (num == "1")
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','公休日加班',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 2, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                    else if (num == "2")
                    {
                        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','法定节假日加班',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 3, Remark, frmUTSOFTMAIN.StaffID);
                        int resultNum = DBHelper.ExecuteUpdate(sql1);
                        return resultNum;
                    }
                    else
                    {
                        return 0;
                    }
                }
                //else if (KQJXType == "周六加班")
                //{
                //    if (KQJXTime.DayOfWeek == DayOfWeek.Saturday)
                //    {
                //        string datebj = Convert.ToDateTime(KQJXTime.ToShortDateString()).ToString("yyyyMMdd");
                //        string resultbj = tools.IsHoliday(datebj);
                //        string numbj = resultbj.Substring(resultbj.Length - 3, 1);
                //        if (numbj == "2")
                //        {
                //            string sqlbj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','法定节假日加班',{4},{5},'{6}','{7}',getdate())",
                //                CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 3, Remark, frmUTSOFTMAIN.StaffID);
                //            int resultNum = DBHelper.ExecuteUpdate(sqlbj);
                //            return resultNum;
                //        }
                //        else
                //        {
                //            string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','公休日加班',{4},{5},'{6}','{7}',getdate())",
                //                    CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 2, Remark, frmUTSOFTMAIN.StaffID);
                //            int resultNum = DBHelper.ExecuteUpdate(sql1);
                //            return resultNum;
                //        }
                //    }
                //    else { return 0; }
                //}
                else if (KQJXType == "基础加班")
                {
                    //每个周末插入一条记录
                    //if (KQJXTime.DayOfWeek == DayOfWeek.Sunday)
                    //{
                    //    string datebj = Convert.ToDateTime(KQJXTime.ToShortDateString()).ToString("yyyyMMdd");
                    //    string resultbj = tools.IsHoliday(datebj);
                    //    string numbj = resultbj.Substring(resultbj.Length - 3, 1);
                    //    if (numbj == "2")
                    //    {
                    //        string sqlbj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','法定节假日加班',{4},{5},'{6}','{7}',getdate())",
                    //            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 3, Remark, frmUTSOFTMAIN.StaffID);
                    //        int resultNum = DBHelper.ExecuteUpdate(sqlbj);
                    //        return resultNum;
                    //    }
                    //    else
                    //    {
                    //        string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','公休日加班',{4},{5},'{6}','{7}',getdate())",
                    //                CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXCount * unitjbsalary * 2, Remark, frmUTSOFTMAIN.StaffID);
                    //        int resultNum = DBHelper.ExecuteUpdate(sql1);
                    //        return resultNum;
                    //    }
                    //}
                    //else { return 0; }
                    string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',1,{5},'{6}','{7}',getdate())",
                                    CompanyNames, KQJXTime.AddDays(1 - KQJXTime.Day).ToShortDateString(), 
                                    staffIDStr, KQJXType, KQJXCount, this.textBoxX2.Text.Trim(), Remark, 
                                    frmUTSOFTMAIN.StaffID);
                    int resultNum = DBHelper.ExecuteUpdate(sql1);
                    return resultNum;
                }
                else if (KQJXType == "优异奖金" || KQJXType == "扣补工资" || KQJXType == "保险")
                {
                    string sql1 = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            CompanyNames, KQJXTime.ToShortDateString(), staffIDStr, KQJXType, KQJXCount, KQJXValue, Remark, frmUTSOFTMAIN.StaffID);
                    int resultNum = DBHelper.ExecuteUpdate(sql1);
                    return resultNum;
                }
                else { return 0; }
            }
            catch
            { MessageBox.Show("提交失败"); return 0; }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要删除选中的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("delete from Pub_WorkSalary where id in (");
                for (int i = 0; i < this.dataGridViewX1.SelectedRows.Count; i++)
                {
                    sql1 += string.Format("{0},",this.dataGridViewX1.SelectedRows[i].Cells[0].Value.ToString());
                }
                sql1 = sql1.Substring(0, sql1.Length - 1);
                sql1 += ")";
            }
            catch
            {
                MessageBox.Show("数据格式输入错误");
                return;
            }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                while (this.dataGridViewX1.SelectedRows.Count>0)
                {
                    this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                }
                salaryAll();
            }
            else
            {
                MessageBox.Show("删除失败，请检查输入数据！");
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)//修改金额
        {
            if (MessageBox.Show("你确定要修改选中行金额吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update Pub_WorkSalary set KQJXValue = '{0}' where id in (", this.textBoxX2.Text.Trim());
                for (int i = 0; i < this.dataGridViewX1.SelectedRows.Count; i++)
                {
                    sql1 += string.Format("{0},", this.dataGridViewX1.SelectedRows[i].Cells[0].Value.ToString());
                }
                sql1 = sql1.Substring(0, sql1.Length - 1);
                sql1 += ")";
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
                for (int i = 0; i < this.dataGridViewX1.SelectedRows.Count; i++)
                {
                    this.dataGridViewX1.SelectedRows[i].Cells[9].Value = this.textBoxX2.Text.Trim();
                }
                salaryAll();
            }
            else
            {
                MessageBox.Show("修改失败，请检查输入数据！");
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

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}'",this.comboBoxEx1.Text.Trim());
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPerson;
            this.comboBoxEx2.ValueMember = "DepartmentName";
            this.comboBoxEx2.DisplayMember = "DepartmentName";
            this.comboBoxEx2.SelectedIndex = -1;
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "" || this.comboBoxEx2.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'",
                this.comboBoxEx1.Text, this.comboBoxEx2.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.DataSource = dtGroup;
            this.comboBoxEx3.ValueMember = "GroupName";
            this.comboBoxEx3.DisplayMember = "GroupName";
            this.comboBoxEx3.SelectedIndex = -1;
        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "" || this.comboBoxEx2.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "StaffID";
            this.comboBoxEx4.DisplayMember = "AssumedName";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void buttonX4_Click(object sender, EventArgs e)//查询
        {
            string sqlFlush = string.Format("select a.id,a.KQJXTime,b.CompanyNames,b.DepartmentName,b.GroupName,b.AssumedName,b.StaffID," +
                " a.KQJXType,a.KQJXCount,a.KQJXValue,a.Remark" +
                " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                " where b.CompanyNames like '%{0}%' and b.DepartmentName like '%{1}%' and b.GroupName like '%{2}%' and b.StaffID like '%{3}%'"+
                " and a.KQJXType like '%{4}%'" +
                " and a.KQJXTime between '{5}' and '{6}'",
                this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text, this.comboBoxEx4.SelectedValue, 
                this.comboBoxEx5.Text.Trim(),
                this.dateTimeInput1.Value.ToShortDateString(), 
                this.dateTimeInput2.Value.ToShortDateString(), 
                this.textBoxX3.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sqlFlush);
            if(dt1.Rows.Count ==0)
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            salaryAll();
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要生成全勤奖吗?", 
                "提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            //DateTime dtlastmonth = System.DateTime.Now.AddMonths(-1);//上个月的今天
            DateTime dtlastmonth = System.DateTime.Now.AddMonths(-1).AddDays(1 - System.DateTime.Now.AddMonths(-1).Day);//上个月的第一天
            string sqldel = string.Format("delete from Pub_WorkSalary where KQJXType = '全勤奖' and datepart(yy,KQJXTime) = {0} and datepart(mm,KQJXTime) = {1}",
                dtlastmonth.Year, dtlastmonth.Month);
            DBHelper.ExecuteUpdate(sqldel);

            string sql1 = string.Format("select StaffID,CompanyNames from Users where onjob = 1 and gularizationDate <= '{0}'",dtlastmonth);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);

            int resultNum1 = 0;
            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Value = 0;
            this.progressBarX1.Step = 1;
            this.progressBarX1.Maximum = dt1.Rows.Count;
            
            string sql2 = string.Format("select StaffID,sum(KQJXCount) as qjts from Pub_WorkSalary " +
                    "where KQJXType in('病假','事假') and datepart(yy,KQJXTime) = {0} and datepart(mm,KQJXTime) = {1} group by StaffID",
                    dtlastmonth.Year, dtlastmonth.Month);
            DataTable dt2 = DBHelper.ExecuteQuery(sql2);
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                string sql = string.Format("StaffID = '{0}'", dt1.Rows[0][0].ToString());
                string[] columNames = new string[] { "qjts" };
                DataTable dtqjts = tools.dtFilter(dt2, columNames, sql);
                if (dtqjts.Rows.Count > 0)
                {
                    if (double.Parse(dtqjts.Rows[0][0].ToString()) <= 24)
                    {
                        string sqlinsertqqj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            dt1.Rows[i][1].ToString(), dtlastmonth.ToShortDateString(), dt1.Rows[i][0].ToString(), "全勤奖", 1, 300, "", frmUTSOFTMAIN.StaffID);
                        resultNum1 += DBHelper.ExecuteUpdate(sqlinsertqqj);
                        
                    }
                }
                else 
                {
                    string sqlinsertqqj = string.Format("insert into Pub_WorkSalary values ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}',getdate())",
                            dt1.Rows[i][1].ToString(), dtlastmonth.ToShortDateString(), dt1.Rows[i][0].ToString(), "全勤奖", 1, 300, "", frmUTSOFTMAIN.StaffID);
                    resultNum1 += DBHelper.ExecuteUpdate(sqlinsertqqj);
                    
                }
                this.progressBarX1.Value++;
            }
            this.progressBarX1.Value = dt1.Rows.Count;
            if (resultNum1 > 0)
            {
                MessageBox.Show("增加成功 " + resultNum1 + " 条！");
                string sqlFlush = string.Format("select a.id,a.KQJXTime,b.CompanyNames,b.DepartmentName,b.GroupName,b.AssumedName,b.StaffID," +
                " a.KQJXType,a.KQJXCount,a.KQJXValue,a.Remark" +
                " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                " where a.KQJXType = '全勤奖'" +
                " and datepart(yy,a.KQJXTime) = {0} and datepart(mm,a.KQJXTime) = {1}",
                dtlastmonth.Year, dtlastmonth.Month);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                salaryAll();
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }
        private void salaryAll()
        {
            double a1 = 0;
            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {
                a1 += double.Parse(this.dataGridViewX1.Rows[i].Cells[9].Value.ToString());
            }
            this.labelX66.Text = "合计： " + a1.ToString() + " 元";
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要生成【" + System.DateTime.Now.AddMonths(-1).Month + "月份保险】吗?", "生成提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            DateTime dtlast2monthday = System.DateTime.Now.AddMonths(-2).AddDays(1 - System.DateTime.Now.AddMonths(-1).Day);//上上个月的第1天
            DateTime dtlastmonthday = System.DateTime.Now.AddMonths(-1).AddDays(1 - System.DateTime.Now.AddMonths(-1).Day);//上个月的第一天
            DateTime dtthismonthday = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);//这个月的第一天
            //删除需要生成工资月的保险
            string sql1 = string.Format("delete from Pub_WorkSalary where "+
                "KQJXType = '保险' and datepart(yy,KQJXTime) = {0} and datepart(mm,KQJXTime) = {1} ", 
                dtlastmonthday.Year, dtlastmonthday.Month);
            //插入需要生成工资月的保险
            sql1 += string.Format(
                " insert into Pub_WorkSalary"+
                " select a.CompanyNames,'{2}',a.StaffID,'保险',1,-6,'系统插入保险扣补金额','{3}',getdate() from Users "+
                " a left join Pub_WorkSalary b on a.StaffID = b.StaffID "+
                " where a.UserType != '系统管理员' and "+
                " (onjob = 1 and EmploymentDate <='{0}') or" +
                " (onjob = 0 and LeaveTime>='{1}')" + 
                " and gularizationDate <= '{2}'",
                dtthismonthday.ToShortDateString(),
                dtlastmonthday.AddDays(4).ToShortDateString(), 
                dtlastmonthday.ToShortDateString(),
                frmUTSOFTMAIN.StaffID);
            //更新需要生成工资月的保险
            sql1 += string.Format(" update a set a.KQJXValue = b.KQJXValue from Pub_WorkSalary a left join Pub_WorkSalary b on a.StaffID = b.StaffID"+
                " where b.KQJXType = '保险' and datepart(yy, a.KQJXTime) = {0} and datepart(mm, a.KQJXTime) = {1}" + 
                " and datepart(yy, b.KQJXTime) = {2} and datepart(mm, b.KQJXTime) = {3}",
                dtlastmonthday.Year, dtlastmonthday.Month,
                dtlast2monthday.Year, dtlast2monthday.Month);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if(resultNum>0)
            {
                MessageBox.Show("生成成功！");
                string sqlFlush = string.Format("select a.id,a.KQJXTime,b.CompanyNames,b.DepartmentName,b.GroupName,b.AssumedName,b.StaffID," +
                " a.KQJXType,a.KQJXCount,a.KQJXValue,a.Remark" +
                " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID" +
                " where a.KQJXType = '保险'" +
                " and datepart(yy,a.KQJXTime) = {0} and datepart(mm,a.KQJXTime) = {1}",
                dtlastmonthday.Year, dtlastmonthday.Month);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
                salaryAll();
            }
        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxEx5.Text = "";
            this.textBoxX1.Text = "0";
            this.textBoxX2.Text = "0";
            this.textBoxX3.Text = "";
        }

        private void dateTimeInput1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimeInput2.Value = this.dateTimeInput1.Value;
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx5.Text == "迟到"||this.comboBoxEx5.Text == "早退") 
            {
                this.labelX76.Text = "分钟";
                this.textBoxX1.Enabled = true;
            }
            else if (this.comboBoxEx5.Text == "病假" || this.comboBoxEx5.Text == "事假" ||
                this.comboBoxEx5.Text == "加班" || this.comboBoxEx5.Text == "旷工" || this.comboBoxEx5.Text == "基础加班")
            {
                this.labelX76.Text = "小时";
                this.textBoxX1.Enabled = true;
            }
            else if (this.comboBoxEx5.Text == "保险" || this.comboBoxEx5.Text == "优异奖金" || this.comboBoxEx5.Text == "扣补工资")
            {
                this.textBoxX1.Text = "0";
                this.textBoxX1.Enabled = false;
                this.labelX76.Text = "";
            }
            else 
            {
                this.textBoxX1.Enabled = true;
            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("只能修改绩效金额,或者加班类型,你确定要修改【" + this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString() + "】的绩效金额吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqlUp = "";
            if (this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString() == "工作日加班" ||
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString() == "公休日加班" ||
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString() == "法定节假日加班")
            {
                if (this.comboBoxEx5.Text.Trim() == "工作日加班" ||
                    this.comboBoxEx5.Text.Trim() == "公休日加班" ||
                    this.comboBoxEx5.Text.Trim() == "法定节假日加班")
                {
                    double kqJXValue = 0;
                    if (this.comboBoxEx5.Text.Trim() == "工作日加班")
                        kqJXValue = double.Parse(this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString()) * 7.75;
                    else if (this.comboBoxEx5.Text.Trim() == "公休日加班")
                        kqJXValue = double.Parse(this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString()) * 7.75 * 2;
                    else if (this.comboBoxEx5.Text.Trim() == "法定节假日加班")
                        kqJXValue = double.Parse(this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString()) * 7.75 * 3;
                    sqlUp = string.Format("update Pub_WorkSalary set KQJXValue = {0},KQJXType = '{2}' where id = {1}",
                     kqJXValue,
                     this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                     this.comboBoxEx5.Text.Trim());
                    int resultNum = DBHelper.ExecuteUpdate(sqlUp);
                    if (resultNum > 0)
                    {
                        MessageBox.Show("修改成功");
                        this.dataGridViewX1.SelectedRows[0].Cells[7].Value = this.comboBoxEx5.Text.Trim();
                        this.dataGridViewX1.SelectedRows[0].Cells[9].Value = kqJXValue;
                        salaryAll();
                    }
                    else
                        MessageBox.Show("修改失败");
                }
                else
                {
                    MessageBox.Show("绩效类型输入错误，只能是：工作日加班，公休日加班，法定节假日加班");
                    return;
                }
            }
        }
    }
}
