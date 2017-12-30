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
    public partial class UT_frmDevloPart : Form
    {
        public UT_frmDevloPart()
        {
            InitializeComponent();
        }
        private void UT_frmDevloPart_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput2.Value = System.DateTime.Now;

            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '研发部'");
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtAssumedName = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx1.DataSource = dtAssumedName.Copy();
            this.comboBoxEx1.ValueMember = "StaffID";
            this.comboBoxEx1.DisplayMember = "AssumedName";
            this.comboBoxEx1.SelectedIndex = -1;
            this.comboBoxEx2.DataSource = dtAssumedName.Copy();
            this.comboBoxEx2.ValueMember = "StaffID";
            this.comboBoxEx2.DisplayMember = "AssumedName";
            this.comboBoxEx2.SelectedIndex = -1;

            string sql1 = string.Format("select a.id,b.Jointime,b.NickName,a.qq,b.QunNumber,b.Telephone,b.PayOringal,b.MeanTeacher "+
                " from UT_YFVisit a left join Pub_VIPMessage b on a.qq = b.qq where b.EnterType in ('预定转报名','报名')"+
                " and VisitPerson = ''");
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX1.DataSource = dt1;
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "" || this.comboBoxEx2.Text == "")
            {
                MessageBox.Show("请选定一次和二次接受人");
                return;
            }
            int count = 0;

            for (int i = this.dataGridViewX1.SelectedRows.Count; i > 0; i--)
            {
                count++;
                string sql1 = string.Format(" delete from UT_YFVisit where id = {0}", this.dataGridViewX1.SelectedRows[i - 1].Cells[0].Value.ToString());
                string sql2 = string.Format(" insert into UT_YFVisit (QQ,DistributionTime,VisitTime,VisitPerson,VisitType,VisitState,VisitRemark) " +
                "select '{0}','{1}','','{2}','一次跟踪','未完成','' union all " +
                "select '{0}','{4}','','{3}','二次跟踪','未完成',''",
                this.dataGridViewX1.SelectedRows[i - 1].Cells[3].Value.ToString(),
                System.DateTime.Now.ToShortDateString(),
                this.comboBoxEx1.SelectedValue.ToString(),
                this.comboBoxEx2.SelectedValue.ToString(),
                System.DateTime.Now.AddDays(4).ToShortDateString());
                DBHelper.ExecuteUpdate(sql1+sql2);
                this.dataGridViewX1.Rows.RemoveAt(this.dataGridViewX1.SelectedRows[i - 1].Index);
            }
            MessageBox.Show("分配成功" + count+"名学员！");
            //--------------日志开始------------------
            string operationRemarkStr = string.Format("");
            frmUTSOFTMAIN.OperationObject = "";
            frmUTSOFTMAIN.OperationRemark = "分配成功:" + count;
            frmUTSOFTMAIN.addlog(this.buttonX1);
            //--------------日志结束------------------
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX2.SelectedRows.Count == 0)
                return;
            if (this.dataGridViewX2.SelectedRows[0].Cells[8].Value.ToString() == "一次跟踪")
            {
                UT_YF_OneVisit ytyfone = new UT_YF_OneVisit();
                ytyfone.Text = "一次跟踪";
                ytyfone.id = this.dataGridViewX2.SelectedRows[0].Cells[0].Value.ToString();
                ytyfone.textDate = this.dataGridViewX2.SelectedRows[0].Cells[10].Value.ToString();
                ytyfone.btnVisable = true;
                ytyfone.Show();
            }
            else if (this.dataGridViewX2.SelectedRows[0].Cells[8].Value.ToString() == "二次跟踪")
            {
                UT_YF_TwoVisit ytyftwo = new UT_YF_TwoVisit();
                ytyftwo.Text = "二次跟踪";
                ytyftwo.qqStr = this.dataGridViewX2.SelectedRows[0].Cells[3].Value.ToString();
                ytyftwo.qqNickName = this.dataGridViewX2.SelectedRows[0].Cells[2].Value.ToString();
                ytyftwo.btnVisable = true;
                ytyftwo.Show();
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select b.AssumedName as 组员,count(*) as 管理学员数,"+
                " coalesce(sum(case when (VisitType = '一次跟踪' and VisitState = '已完成') then 1 else 0 end),0) as 一次跟踪已完成,"+
                " coalesce(sum(case when (VisitType = '一次跟踪' and VisitState = '未完成') then 1 else 0 end),0) as 一次跟踪未完成,"+
                " coalesce(sum(case when (VisitType = '二次跟踪' and VisitState = '已完成') then 1 else 0 end),0) as 二次跟踪已完成,"+
                " coalesce(sum(case when (VisitType = '二次跟踪' and VisitState = '未完成') then 1 else 0 end),0) as 二次跟踪未完成" +
                " from UT_YFVisit a left join Users b on a.VisitPerson = b.StaffID where b.onjob = 1 group by b.AssumedName");
            this.dataGridViewX3.DataSource = DBHelper.ExecuteQuery(sql1);
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from UT_LessonSituation where "+
                "LessonTime between '{0}' and '{1}' and LessonType like '%{2}%'",
                this.dateTimeInput1.Value.ToShortDateString(), this.dateTimeInput2.Value.ToShortDateString(),
                this.comboBoxEx4.Text);
            this.dataGridViewX5.DataSource = DBHelper.ExecuteQuery(sql1);
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from Pub_VIPMessage where QQ = '{0}' and CompanyNames = '优梯' and EnterType in ('预定转报名','报名')", this.textBoxX1.Text.Trim());
            this.dataGridViewX4.DataSource = DBHelper.ExecuteQuery(sql1);
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from UT_YFVisit where QQ = '{0}' and VisitType = '一次跟踪' and VisitState = '已完成'", this.textBoxX1.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if(dt1.Rows.Count == 0)
            {
                MessageBox.Show("无一次跟踪信息");
                return;
            }
            UT_YF_OneVisit ytyfone = new UT_YF_OneVisit();
            ytyfone.Text = "一次跟踪";
            ytyfone.textDate = dt1.Rows[0][7].ToString();
            ytyfone.Show();
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from UT_VIPMemberShopDiagnosis where QQ = '{0}'", this.textBoxX1.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("无二次跟踪信息");
                return;
            }
            UT_YF_TwoVisit ytyftwo = new UT_YF_TwoVisit();
            ytyftwo.daSource = dt1;
            ytyftwo.qqStr = this.textBoxX1.Text.Trim();
            ytyftwo.Text = "二次跟踪";
            ytyftwo.Show();
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format(" select a.id,b.JoinTime,b.NickName,b.QQ,b.QunNumber,"+
                " b.Telephone,b.PayOringal,b.MeanTeacher,a.VisitType,a.VisitState,a.VisitRemark"+
                " from UT_YFVisit a left join Pub_VIPMessage b on a.qq = b.qq where b.EnterType in ('预定转报名','报名') and "+
                " VisitPerson = '{0}' and DistributionTime <= getdate() and a.VisitState = '未完成'", frmUTSOFTMAIN.StaffID);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX2.DataSource = dt1;
        }

        private void dataGridViewX5_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.textBoxX22.Text = this.dataGridViewX5.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX23.Text = this.dataGridViewX5.SelectedRows[0].Cells[8].Value.ToString();
            this.textBoxX24.Text = this.dataGridViewX5.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX25.Text = this.dataGridViewX5.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX30.Text = this.dataGridViewX5.SelectedRows[0].Cells[18].Value.ToString();
        }

        
    }
}
