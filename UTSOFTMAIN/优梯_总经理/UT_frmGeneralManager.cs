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
    public partial class UT_frmGeneralManager : Form
    {
        public UT_frmGeneralManager()
        {
            InitializeComponent();
        }
        private void UT_frmGeneralManager_Load(object sender, EventArgs e)
        {
            this.comboBoxEx1.Text = System.DateTime.Now.Year.ToString();
            this.comboBoxEx2.SelectedIndex = System.DateTime.Now.Month - 1;
            buttonX1_Click(null,null);
        }
        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select"+ 
                " 绩效时间,星期,sum(报名) as 报名,sum(投诉) as 投诉,sum(退款) as 退款"+
                " from ("+
                " select Jointime as 绩效时间, datename(weekday, Jointime) as 星期,"+ 
                " coalesce(sum(case when charindex('报名',EnterType)>0 then 1 else 0 end),0) as 报名,"+ 
                " coalesce(sum(case when charindex('投诉',EnterType)>0 then 1 else 0 end),0) as 投诉, "+
                " coalesce(sum(case when charindex('退款',EnterType)>0 then 1 else 0 end),0) as 退款  "+
                " from Pub_VIPMessage "+
                " where CompanyNames like '%优梯%'"+   
                " and datepart(yy,Jointime) = {0} and datepart(mm,Jointime) = {1}"+
                " group by Jointime, EnterType"+
                " union all"+
                " select SubmitTime as 绩效时间, datename(weekday, SubmitTime) as 星期," + 
                " coalesce(sum(case when charindex('报名',SubmitType)>0 then 1 else 0 end),0) as 报名,"+ 
                " coalesce(sum(case when charindex('投诉',SubmitType)>0 then 1 else 0 end),0) as 投诉,"+ 
                " coalesce(sum(case when charindex('退款',SubmitType)>0 then 1 else 0 end),0) as 退款"+  
                " from Pub_ComplainRefundInfo "+
                " where CompanyNames like '%优梯%'"+
                " and datepart(yy,SubmitTime) = {0} and datepart(mm,SubmitTime) = {1}" +
                " group by SubmitTime, SubmitType" +
                " ) a"+
                " group by 绩效时间,星期" +
                " order by 绩效时间 desc",
               this.comboBoxEx1.Text, this.comboBoxEx2.SelectedIndex + 1);
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sql1);
            sumEnterPerson();
        }
        private void sumEnterPerson() 
        {
            int enterCount = 0;
            for (int i = 0; i < this.dataGridViewX1.Rows.Count;i++ ) 
            {
                enterCount += int.Parse(this.dataGridViewX1.Rows[i].Cells[2].Value.ToString());
            }
            this.labelX5.Text = "合计报名人数: " + enterCount +" 人";
        }
        
    }
}
