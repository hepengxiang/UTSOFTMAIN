using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UTSOFTMAIN.优梯_个人情况;

namespace UTSOFTMAIN
{
    public partial class UT_frmPersonSituation : Form
    {
        public UT_frmPersonSituation()
        {
            InitializeComponent();
        }

        private static DataTable glc1 = new DataTable();
        private static DataTable glc = new DataTable();
        private void UT_frmPersonSituation_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput4.Value = System.DateTime.Now;
            this.dateTimeInput5.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput6.Value = System.DateTime.Now;

            glc1 = DBHelper.ExecuteQuery("select a.CompanyNames,a.DepartmentName,a.AssumedName,a.StaffID from Users " +
                " a left join Pub_JXCaculate b on a.CompanyNames = b.CompanyNames " +
                " and a.DepartmentName = b.DepartmentName and a.GroupName = b.GroupName and a.UserType = b.UserType " +
                " where b.JXCardinalNum in ('本组','本部门') group by a.CompanyNames,a.DepartmentName,a.AssumedName,a.StaffID");

            string[] columNameglc = new string[] { "StaffID" };
            glc = tools.dtFilter(glc1, columNameglc, "");

            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx1.DataSource = dtCompany.Copy();
            this.comboBoxEx1.DisplayMember = "CompanyNames";
            this.comboBoxEx1.ValueMember = "CompanyNames";

            this.comboBoxEx5.DataSource = dtCompany.Copy();
            this.comboBoxEx5.DisplayMember = "CompanyNames";
            this.comboBoxEx5.ValueMember = "CompanyNames";

            this.comboBoxEx10.DataSource = dtCompany.Copy();
            this.comboBoxEx10.DisplayMember = "CompanyNames";
            this.comboBoxEx10.ValueMember = "CompanyNames";

            this.comboBoxEx14.DataSource = dtCompany.Copy();
            this.comboBoxEx14.DisplayMember = "CompanyNames";
            this.comboBoxEx14.ValueMember = "CompanyNames";

            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "总经理" || frmUTSOFTMAIN.UserType == "行政经理")
            {
                
            }
            else if (frmUTSOFTMAIN.UserType.Contains("主管") || frmUTSOFTMAIN.UserType.Contains("总监"))
            {
                this.comboBoxEx1.Enabled = false;
                this.comboBoxEx2.Enabled = false;

                this.comboBoxEx5.Enabled = false;
                this.comboBoxEx6.Enabled = false;

                this.comboBoxEx10.Enabled = false;
                this.comboBoxEx11.Enabled = false;

                this.comboBoxEx14.Enabled = false;
                this.comboBoxEx15.Enabled = false;
            }
            else if (frmUTSOFTMAIN.UserType.Contains("组长") || frmUTSOFTMAIN.UserType.Contains("金牌讲师"))
            {
                this.comboBoxEx1.Enabled = false;
                this.comboBoxEx2.Enabled = false;
                this.comboBoxEx3.Enabled = false;

                this.comboBoxEx5.Enabled = false;
                this.comboBoxEx6.Enabled = false;
                this.comboBoxEx7.Enabled = false;

                this.comboBoxEx10.Enabled = false;
                this.comboBoxEx11.Enabled = false;
                this.comboBoxEx12.Enabled = false;

                this.comboBoxEx14.Enabled = false;
                this.comboBoxEx15.Enabled = false;
                this.comboBoxEx16.Enabled = false;
            }
            else 
            {
                this.comboBoxEx1.Enabled = false;
                this.comboBoxEx2.Enabled = false;
                this.comboBoxEx3.Enabled = false;
                this.comboBoxEx4.Enabled = false;

                this.comboBoxEx5.Enabled = false;
                this.comboBoxEx6.Enabled = false;
                this.comboBoxEx7.Enabled = false;
                this.comboBoxEx8.Enabled = false;

                this.comboBoxEx10.Enabled = false;
                this.comboBoxEx11.Enabled = false;
                this.comboBoxEx12.Enabled = false;
                this.comboBoxEx13.Enabled = false;

                this.comboBoxEx14.Enabled = false;
                this.comboBoxEx15.Enabled = false;
                this.comboBoxEx16.Enabled = false;
            }
            this.comboBoxEx1.Text = frmUTSOFTMAIN.CompanyNames;
            comboBoxEx1_SelectedIndexChanged(null,null);
            this.comboBoxEx2.Text = frmUTSOFTMAIN.DepartmentName;
            //comboBoxEx2_SelectedIndexChanged(null, null);
            this.comboBoxEx3.Text = frmUTSOFTMAIN.GroupName;
            //comboBoxEx3_SelectedIndexChanged(null, null);
            this.comboBoxEx4.Text = frmUTSOFTMAIN.AssumedName;

            this.comboBoxEx5.Text = frmUTSOFTMAIN.CompanyNames;
            comboBoxEx5_SelectedIndexChanged(null, null);
            this.comboBoxEx6.Text = frmUTSOFTMAIN.DepartmentName;
            this.comboBoxEx7.Text = frmUTSOFTMAIN.GroupName;
            this.comboBoxEx8.Text = frmUTSOFTMAIN.AssumedName;

            this.comboBoxEx10.Text = frmUTSOFTMAIN.CompanyNames;
            comboBoxEx10_SelectedIndexChanged(null, null);
            this.comboBoxEx11.Text = frmUTSOFTMAIN.DepartmentName;
            this.comboBoxEx12.Text = frmUTSOFTMAIN.GroupName;
            this.comboBoxEx13.Text = frmUTSOFTMAIN.AssumedName;

            this.comboBoxEx14.Text = frmUTSOFTMAIN.CompanyNames;
            //comboBoxEx14_SelectedIndexChanged(null, null);
            this.comboBoxEx15.Text = frmUTSOFTMAIN.DepartmentName;
            
            this.comboBoxEx16.Text = frmUTSOFTMAIN.AssumedName;
            //comboBoxEx15_SelectedIndexChanged(null, null);

            
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
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "StaffID";
            this.comboBoxEx4.DisplayMember = "AssumedName";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select 时间,花名,身份证,sum(直接报名) as 直接报名,sum(跟踪报名) as 跟踪报名,sum(报名绩效) as 报名绩效,sum(投诉人数) as 投诉人数,"+
                " sum(投诉绩效) as 投诉绩效,sum(责任退款) as 责任退款,sum(绩效退款) as 绩效退款,sum(退款绩效) as 退款绩效 from("+
                " select b.Jointime as 时间, c.AssumedName as 花名, c.StaffID as 身份证," +
                " coalesce(sum(case when b.EnterType like '%报名%' and a.EnterTypeSmall like '%直接报名%' then a.Value else 0 end),0) as 直接报名," +
                " coalesce(sum(case when b.EnterType like '%报名%' and a.EnterTypeSmall like '%跟踪报名%' then a.Value else 0 end),0) as 跟踪报名," +
                " coalesce(sum(case when b.EnterType like '%报名%' then a.Amount else 0 end),0) as 报名绩效," +
                " coalesce(sum(case when b.EnterType = '投诉' then a.Value else 0 end),0) as 投诉人数," +
                " coalesce(sum(case when b.EnterType = '投诉' then a.Amount else 0 end),0) as 投诉绩效," +
                " coalesce(sum(case when b.EnterType = '退款' and a.EnterTypeSmall != '绩效退款' then a.Value else 0 end),0) as 责任退款," +
                " coalesce(sum(case when b.EnterType = '退款' and a.EnterTypeSmall = '绩效退款' then a.Value else 0 end),0) as 绩效退款," +
                " coalesce(sum(case when b.EnterType = '退款' then a.Amount else 0 end),0) as 退款绩效" +
                " from Pub_YDBM_JXManager a  left join Pub_VIPMessage b on a.ForID = b.id left join Users c on a.AmountManager = c.StaffID" +
                " where a.StaffID = a.AmountManager and c.CompanyNames like '%{0}%' and c.DepartmentName like '%{1}%' and c.GroupName like '%{2}%'" +
                " and a.AmountManager like '%{3}%' and b.Jointime between '{4}' and '{5}'" +
                " group by b.Jointime,c.AssumedName,c.StaffID" +
                " union all" +
                " select  b.SubmitTime as 时间, c.AssumedName as 花名, c.StaffID as 身份证," +
                " coalesce(sum(case when b.SubmitType like '%报名%' and a.EnterTypeSmall like '%直接报名%' then a.Value else 0 end),0) as 直接报名," +
                " coalesce(sum(case when b.SubmitType like '%报名%' and a.EnterTypeSmall like '%跟踪报名%' then a.Value else 0 end),0) as 跟踪报名," +
                " coalesce(sum(case when b.SubmitType like '%报名%' then a.Amount else 0 end),0) as 报名绩效," +
                " coalesce(sum(case when (b.SubmitType = '投诉' or (b.SubmitType = '退款' and b.Result = '正常')) then a.Value else 0 end),0) as 投诉人数," +
                " coalesce(sum(case when (b.SubmitType = '投诉' or (b.SubmitType = '退款' and b.Result = '正常')) then a.Amount else 0 end),0) as 投诉绩效," +
                " coalesce(sum(case when b.SubmitType = '退款' and a.EnterTypeSmall != '绩效退款' and  b.Result = '退款' then a.Value else 0 end),0) as 责任退款," +
                " coalesce(sum(case when b.SubmitType = '退款' and a.EnterTypeSmall = '绩效退款' and  b.Result = '退款' then a.Value else 0 end),0) as 绩效退款," +
                " coalesce(sum(case when b.SubmitType = '退款' and  b.Result = '退款' then a.Amount else 0 end),0) as 退款绩效" +
                " from Pub_TSTK_JXManager a  left join Pub_ComplainRefundInfo b on a.ForID = b.id left join Users c on a.AmountManager = c.StaffID" +
                " where a.StaffID = a.AmountManager and c.CompanyNames like '%{0}%' and  c.DepartmentName like '%{1}%' and c.GroupName like '%{2}%'" +
                " and a.AmountManager like '%{3}%' and b.SubmitTime between '{4}' and '{5}'" +
                " group by b.SubmitTime,c.AssumedName,c.StaffID" +
                //" order by 时间 desc"+
                " ) a group by 时间,花名,身份证",
                this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text, this.comboBoxEx4.SelectedValue,
                this.dateTimeInput1.Value.ToShortDateString(), this.dateTimeInput2.Value.ToShortDateString());
            //string sql1 = string.Format("select 时间,花名,身份证,sum(直接报名) as 直接报名,sum(跟踪报名) as 跟踪报名,sum(报名绩效) as 报名绩效,sum(投诉人数) as 投诉人数,"+
            //    " sum(投诉绩效) as 投诉绩效,sum(责任退款) as 责任退款,sum(绩效退款) as 绩效退款,sum(退款绩效) as 退款绩效 from (" +
            //    " select b.Jointime as 时间, c.AssumedName as 花名, c.StaffID as 身份证," +
            //    " coalesce(sum(case when b.EnterType like '%报名%' and a.EnterTypeSmall = '直接报名' then a.Value else 0 end),0) as 直接报名," +
            //    " coalesce(sum(case when b.EnterType like '%报名%' and a.EnterTypeSmall = '跟踪报名' then a.Value else 0 end),0) as 跟踪报名," +
            //    " coalesce(sum(case when b.EnterType like '%报名%' then a.Amount else 0 end),0) as 报名绩效," +
            //    " coalesce(sum(case when b.EnterType = '投诉' then a.Value else 0 end),0) as 投诉人数," +
            //    " coalesce(sum(case when b.EnterType = '投诉' then a.Amount else 0 end),0) as 投诉绩效," +
            //    " coalesce(sum(case when b.EnterType = '退款' and a.EnterTypeSmall != '绩效退款' then a.Value else 0 end),0) as 责任退款," +
            //    " coalesce(sum(case when b.EnterType = '退款' and a.EnterTypeSmall = '绩效退款' then a.Value else 0 end),0) as 绩效退款," +
            //    " coalesce(sum(case when b.EnterType = '退款' then a.Amount else 0 end),0) as 退款绩效" +
            //    " from Pub_YDBM_JXManager a  left join Pub_VIPMessage b on a.ForID = b.id left join Users c on a.AmountManager = c.StaffID" +
            //    " where c.CompanyNames like '%{0}%' and c.DepartmentName like '%{1}%' and c.GroupName like '%{2}%'" +
            //    " and a.AmountManager like '%{3}%' and b.Jointime between '{4}' and '{5}'" +
            //    " group by b.Jointime,c.AssumedName,c.StaffID" +
            //    " union all" +
            //    " select  b.SubmitTime as 时间, c.AssumedName as 花名, c.StaffID as 身份证," +
            //    " coalesce(sum(case when b.SubmitType like '%报名%' and a.EnterTypeSmall = '直接报名' then a.Value else 0 end),0) as 直接报名," +
            //    " coalesce(sum(case when b.SubmitType like '%报名%' and a.EnterTypeSmall = '跟踪报名' then a.Value else 0 end),0) as 跟踪报名," +
            //    " coalesce(sum(case when b.SubmitType like '%报名%' then a.Amount else 0 end),0) as 报名绩效," +
            //    " coalesce(sum(case when (b.SubmitType = '投诉' or (b.SubmitType = '退款' and b.Result = '正常')) then a.Value else 0 end),0) as 投诉人数," +
            //    " coalesce(sum(case when (b.SubmitType = '投诉' or (b.SubmitType = '退款' and b.Result = '正常')) then a.Amount else 0 end),0) as 投诉绩效," +
            //    " coalesce(sum(case when b.SubmitType = '退款' and a.EnterTypeSmall != '绩效退款' and  b.Result = '退款' then a.Value else 0 end),0) as 责任退款," +
            //    " coalesce(sum(case when b.SubmitType = '退款' and a.EnterTypeSmall = '绩效退款' and  b.Result = '退款' then a.Value else 0 end),0) as 绩效退款," +
            //    " coalesce(sum(case when b.SubmitType = '退款' and  b.Result = '退款' then a.Amount else 0 end),0) as 退款绩效" +
            //    " from Pub_TSTK_JXManager a  left join Pub_ComplainRefundInfo b on a.ForID = b.id left join Users c on a.AmountManager = c.StaffID" +
            //    " where c.CompanyNames like '%{0}%' and  c.DepartmentName like '%{1}%' and c.GroupName like '%{2}%'" +
            //    " and a.AmountManager like '%{3}%' and b.SubmitTime between '{4}' and '{5}'" +
            //    " group by b.SubmitTime,c.AssumedName,c.StaffID" +
            //    //" order by 时间 desc"+
            //    " ) a group by 时间,花名,身份证",
            //    this.comboBoxEx1.Text, this.comboBoxEx2.Text, this.comboBoxEx3.Text, this.comboBoxEx4.SelectedValue,
            //    this.dateTimeInput1.Value.ToShortDateString(), this.dateTimeInput2.Value.ToShortDateString());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sql1);
            //去掉管理层绩效数值
            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {
                for (int j = 0; j < glc.Rows.Count; j++)
                {
                    if (this.dataGridViewX1.Rows[i].Cells[0].Value.ToString() == glc.Rows[j][0].ToString())
                    {
                        this.dataGridViewX1.Rows[i].Cells[5].Value = "0";
                    }
                }
            }
            //显示合计
            double a1 = 0;
            double a2 = 0;
            double a3 = 0;
            double a4 = 0;
            double a5 = 0;
            double a6 = 0;
            double a7 = 0;
            double a8 = 0;
            double a9 = 0;
            for (int i = 0; i < this.dataGridViewX1.Rows.Count; i++)
            {
                a1 += double.Parse(this.dataGridViewX1.Rows[i].Cells[3].Value.ToString());
                a2 += double.Parse(this.dataGridViewX1.Rows[i].Cells[4].Value.ToString());
                a3 += double.Parse(this.dataGridViewX1.Rows[i].Cells[5].Value.ToString());
                a4 += double.Parse(this.dataGridViewX1.Rows[i].Cells[6].Value.ToString());
                a5 += -double.Parse(this.dataGridViewX1.Rows[i].Cells[7].Value.ToString());
                a6 += double.Parse(this.dataGridViewX1.Rows[i].Cells[8].Value.ToString());
                a7 += double.Parse(this.dataGridViewX1.Rows[i].Cells[9].Value.ToString());
                a8 += -double.Parse(this.dataGridViewX1.Rows[i].Cells[10].Value.ToString());
            }
            a9 = a3 + a5 + a8;
            this.labelX24.Text = a1.ToString();
            this.labelX25.Text = a2.ToString();
            this.labelX26.Text = a3.ToString();//报名绩效
            this.labelX27.Text = a4.ToString();
            this.labelX28.Text = a5.ToString();//投诉绩效
            this.labelX29.Text = a6.ToString();
            this.labelX30.Text = a7.ToString();
            this.labelX31.Text = a8.ToString();//退款绩效
            this.labelX35.Text = a9.ToString();
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx5.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx6.DataSource = dtPart;
            this.comboBoxEx6.ValueMember = "DepartmentName";
            this.comboBoxEx6.DisplayMember = "DepartmentName";
            this.comboBoxEx6.SelectedIndex = -1;
        }
        private void comboBoxEx6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx5.Text == "" || this.comboBoxEx6.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx5.Text, this.comboBoxEx6.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx7.DataSource = dtGroup;
            this.comboBoxEx7.ValueMember = "GroupName";
            this.comboBoxEx7.DisplayMember = "GroupName";
            this.comboBoxEx7.SelectedIndex = -1;
        }
        private void comboBoxEx7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx5.Text == "" || this.comboBoxEx6.Text == "" || this.comboBoxEx7.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'", this.comboBoxEx5.Text, this.comboBoxEx6.Text, this.comboBoxEx7.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx8.DataSource = dtPerson;
            this.comboBoxEx8.ValueMember = "StaffID";
            this.comboBoxEx8.DisplayMember = "AssumedName";
            this.comboBoxEx8.SelectedIndex = -1;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,a.KQJXTime,b.AssumedName,a.KQJXType,a.KQJXCount,a.KQJXValue,a.Remark" +
                " from Pub_WorkSalary a left join Users b on a.StaffID = b.StaffID where"+ 
                " b.CompanyNames like '%{0}%' and b.DepartmentName like '%{1}%' and"+
                " GroupName like '%{2}%' and a.StaffID like '%{3}%'"+
                " and a.KQJXTime between '{4}' and '{5}'",
                this.comboBoxEx5.Text,this.comboBoxEx6.Text,this.comboBoxEx7.Text,this.comboBoxEx8.SelectedValue,
                this.dateTimeInput3.Value.ToShortDateString(),this.dateTimeInput4.Value.ToShortDateString());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0) 
            {
                MessageBox.Show("未查询到数据！");
                return;
            }
            this.dataGridViewX2.DataSource = DBHelper.ExecuteQuery(sql1);
        }

        private void comboBoxEx10_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx10.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx11.DataSource = dtPart;
            this.comboBoxEx11.ValueMember = "DepartmentName";
            this.comboBoxEx11.DisplayMember = "DepartmentName";
            this.comboBoxEx11.SelectedIndex = -1;
        }
        private void comboBoxEx11_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx10.Text == "" || this.comboBoxEx11.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx10.Text, this.comboBoxEx11.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx12.DataSource = dtGroup;
            this.comboBoxEx12.ValueMember = "GroupName";
            this.comboBoxEx12.DisplayMember = "GroupName";
            this.comboBoxEx12.SelectedIndex = -1;
        }
        private void comboBoxEx12_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx10.Text == "" || this.comboBoxEx11.Text == "" || this.comboBoxEx12.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'", 
                this.comboBoxEx10.Text, this.comboBoxEx11.Text, this.comboBoxEx12.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx13.DataSource = dtPerson;
            this.comboBoxEx13.ValueMember = "StaffID";
            this.comboBoxEx13.DisplayMember = "AssumedName";
            this.comboBoxEx13.SelectedIndex = -1;
        }

        private void comboBoxEx14_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx14.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx15.DataSource = dtPart;
            this.comboBoxEx15.ValueMember = "DepartmentName";
            this.comboBoxEx15.DisplayMember = "DepartmentName";
            this.comboBoxEx15.SelectedIndex = -1;
        }

        private void comboBoxEx15_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx14.Text == "" || this.comboBoxEx15.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx14.Text, this.comboBoxEx15.Text);
            string[] columNames = new string[] { "AssumedName","StaffID" };
            DataTable dtGroup = tools.dtFilter(glc1, columNames, sql);
            if (dtGroup.Rows.Count == 0)
                return;
            this.comboBoxEx16.DataSource = dtGroup;
            this.comboBoxEx16.ValueMember = "StaffID";
            this.comboBoxEx16.DisplayMember = "AssumedName";
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select *,(a.cb+a.fb) as cfb,(a.qjgz+a.cdgz+a.jbgz) as kqgz, "+
                " convert(nvarchar(6),Times,112) as nianyue from Pub_YearSalary " +
                " a left join Users b on a.StaffID = b.StaffID " +
                " where datepart(yy,Times) = {0} and b.CompanyNames like '%{1}%' and DepartmentName like '%{2}%' and" +
                " GroupName like '%{3}%' and a.StaffID like '%{4}%' and PublicState = 1",
                this.comboBoxEx9.Text.Trim(),this.comboBoxEx10.Text.Trim(),
                this.comboBoxEx11.Text.Trim(),this.comboBoxEx12.Text.Trim(),
                this.comboBoxEx13.SelectedValue);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if(dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据!");
                return;
            }
            this.dataGridViewX3.DataSource = dt1;
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            //string sql1 = string.Format("select Jointime as 时间,count(QQ) as 报名人数 from ("+
            //    " select Jointime,c.QQ from Pub_YDBM_JXManager a"+ 
            //    " left join Users b on a.StaffID = b.StaffID"+
            //    " left join Pub_VIPMessage c on a.ForID = c.id"+
            //    " where b.CompanyNames like '%{0}%' and b.DepartmentName like '%{1}%' and b.GroupName like '%{2}%'"+
            //    " and Jointime between '{3}' and '{4}'"+
            //    " group by Jointime,c.QQ) a" +
            //    " group by Jointime",
            //string sql1 = string.Format("select Jointime as 时间,count(QQ) as 报名人数,sum(jxval) as 绩效金额 from"+ 
            //    " ("+ 
            //    " select Jointime,c.QQ,"+
            //    " coalesce(sum(case when a.AmountManager = '{5}' then a.Amount else 0 end),0) as jxval"+
            //    " from Pub_YDBM_JXManager a"+ 
            //    " left join Users b on a.StaffID = b.StaffID"+ 
            //    " left join Pub_VIPMessage c on a.ForID = c.id"+
            //    " where b.CompanyNames like '%{0}%' and b.DepartmentName like '%{1}%' and b.GroupName like '%{2}%' and Jointime between '{3}' and '{4}'" + 
            //    " group by Jointime,c.QQ"+
            //    " ) a"+ 
            //    " group by Jointime",
            //  this.comboBoxEx14.Text.Trim(),this.comboBoxEx15.Text.Trim(),this.comboBoxEx16.Text.Trim(),
            //    this.dateTimeInput5.Value.ToShortDateString(),this.dateTimeInput6.Value.ToShortDateString(),frmUTSOFTMAIN.StaffID);
            //DataTable dt1 = DBHelper.ExecuteQuery(sql1);

            string sql1 = string.Format("select a.AmountManager,a.AssumedName,Jointime as 时间,coalesce(sum(子报名人数),0) as 报名人数,coalesce(sum(子绩效金额),0) as 绩效金额 from " +
                " ("+
                " select a.AmountManager,b.AssumedName,Jointime,c.QQ,coalesce(sum(a.Value),0) as 子报名人数, coalesce(sum(a.Amount),0) as 子绩效金额 from Pub_YDBM_JXManager" +
                " a left join Users b on a.AmountManager = b.StaffID left join Pub_VIPMessage c on a.ForID = c.id " +
                " where Jointime between '{0}' and '{1}' and c.EnterType like '%报名%' and a.AmountManager = '{2}'"+
                " group by a.AmountManager,b.AssumedName,Jointime,c.QQ ,a.Value, a.Amount" +
                " ) a group by a.AmountManager,a.AssumedName,Jointime",
                this.dateTimeInput5.Value.ToShortDateString(),this.dateTimeInput6.Value.ToShortDateString(),this.comboBoxEx16.SelectedValue);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX4.DataSource = dt1;

            //显示合计
            double a1 = 0;
            double a2 = 0;
            for (int i = 0; i < this.dataGridViewX4.Rows.Count; i++)
            {
                a1 += double.Parse(this.dataGridViewX4.Rows[i].Cells[3].Value.ToString());
                a2 += double.Parse(this.dataGridViewX4.Rows[i].Cells[4].Value.ToString());
            }
            this.labelX33.Text = a1.ToString();
            this.labelX34.Text = a2.ToString();
        }

        private void dataGridViewX1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString());
            if (e.ColumnIndex == 0 || e.RowIndex == -1)
                return;
            string sql1 = "";
            if (e.ColumnIndex == 3)//直接报名人数
            {
                sql1 = string.Format("select b.CompanyNames as 公司, b.Jointime as 绩效时间,b.EnterType as 绩效大类,b.QQ as 绩效QQ,b.NickName as QQ昵称," +
                    " c.AssumedName as 绩效发生人,a.EnterTypeSmall as 绩效小类,a.Value as 绩效分值,a.Amount as 绩效金额,a.Remark as 绩效备注" +
                    " from Pub_YDBM_JXManager a " +
                    " left join Pub_VIPMessage b on a.ForID = b.id " +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.AmountManager = '{0}' and b.Jointime = '{1}' and " +
                    " b.EnterType like '%报名%' and a.EnterTypeSmall = '直接报名' and a.Remark != '管理层预定报名类绩效（系统插入）'",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
            else if (e.ColumnIndex == 4)//跟踪报名人数
            {
                sql1 = string.Format("select b.CompanyNames as 公司, b.Jointime as 绩效时间,b.EnterType as 绩效大类,b.QQ as 绩效QQ,b.NickName as QQ昵称," +
                    " c.AssumedName as 绩效发生人,a.EnterTypeSmall as 绩效小类,a.Value as 绩效分值,a.Amount as 绩效金额,a.Remark as 绩效备注" +
                    " from Pub_YDBM_JXManager a " +
                    " left join Pub_VIPMessage b on a.ForID = b.id " +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.AmountManager = '{0}' and b.Jointime = '{1}' and " +
                    " b.EnterType like '%报名%' and a.EnterTypeSmall = '跟踪报名' and a.Remark != '管理层预定报名类绩效（系统插入）'",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
            else if (e.ColumnIndex == 6)//投诉人数
            {
                sql1 = string.Format("select b.CompanyNames as 公司, b.SubmitTime as 绩效时间,b.SubmitType as 绩效大类,b.QQ as 绩效QQ,b.NickName as QQ昵称," +
                    " c.AssumedName as 绩效发生人,a.EnterTypeSmall as 绩效小类,a.Value as 绩效分值,a.Amount as 绩效金额,a.Remark as 绩效备注" +
                    " from Pub_TSTK_JXManager a " +
                    " left join Pub_ComplainRefundInfo b on a.ForID = b.id " +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.AmountManager = '{0}' and b.SubmitTime = '{1}' and " +
                    " (b.SubmitType = '投诉' or (b.SubmitType = '退款' and b.Result = '正常'))",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
            else if (e.ColumnIndex == 8)//责任退款人数
            {
                sql1 = string.Format("select b.CompanyNames as 公司, b.SubmitTime as 绩效时间,b.SubmitType as 绩效大类,b.QQ as 绩效QQ,b.NickName as QQ昵称," +
                    " c.AssumedName as 绩效发生人,a.EnterTypeSmall as 绩效小类,a.Value as 绩效分值,a.Amount as 绩效金额,a.Remark as 绩效备注" +
                    " from Pub_TSTK_JXManager a " +
                    " left join Pub_ComplainRefundInfo b on a.ForID = b.id " +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.AmountManager = '{0}' and b.SubmitTime = '{1}' and " +
                    " b.SubmitType = '退款' and a.EnterTypeSmall != '绩效退款' and  b.Result = '退款'",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
            else if (e.ColumnIndex == 9)//绩效退款人数
            {
                sql1 = string.Format("select b.CompanyNames as 公司, b.SubmitTime as 绩效时间,b.SubmitType as 绩效大类,b.QQ as 绩效QQ,b.NickName as QQ昵称," +
                    " c.AssumedName as 绩效发生人,a.EnterTypeSmall as 绩效小类,a.Value as 绩效分值,a.Amount as 绩效金额,a.Remark as 绩效备注" +
                    " from Pub_TSTK_JXManager a " +
                    " left join Pub_ComplainRefundInfo b on a.ForID = b.id " +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.AmountManager = '{0}' and b.SubmitTime = '{1}' and " +
                    " b.SubmitType = '退款' and a.EnterTypeSmall = '绩效退款' and  b.Result = '退款'",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
            if (sql1 == "")
                return;
            DataTable dtshow = DBHelper.ExecuteQuery(sql1);
            if (dtshow.Rows.Count == 0)
                return;
            JX_DetailShow jxds = new JX_DetailShow();
            jxds.dtSource = dtshow;
            jxds.Show();
        }

        private void dataGridViewX4_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex == -1)
                return;
            string sql1 = "";
            if (e.ColumnIndex == 3)//直接报名人数
            {
                sql1 = string.Format("select b.CompanyNames as 公司, b.Jointime as 绩效时间,b.EnterType as 绩效大类,b.QQ as 绩效QQ,b.NickName as QQ昵称," +
                    " c.AssumedName as 绩效发生人,a.EnterTypeSmall as 绩效小类,a.Value as 绩效分值,a.Amount as 绩效金额,a.Remark as 绩效备注" +
                    " from Pub_YDBM_JXManager a " +
                    " left join Pub_VIPMessage b on a.ForID = b.id " +
                    " left join Users c on a.StaffID = c.StaffID" +
                    " where a.AmountManager = '{0}' and b.Jointime = '{1}' and " +
                    " b.EnterType like '%报名%'",
                    this.dataGridViewX4.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX4.Rows[e.RowIndex].Cells[2].Value.ToString());
            }
            if (sql1 == "")
                return;
            DataTable dtshow = DBHelper.ExecuteQuery(sql1);
            if (dtshow.Rows.Count == 0)
                return;
            JX_DetailShow jxds = new JX_DetailShow();
            jxds.dtSource = dtshow;
            jxds.Show();
        }
    }
}
