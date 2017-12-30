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
    public partial class Admin_DataMatch : Form
    {
        public Admin_DataMatch()
        {
            InitializeComponent();
        }

        DataTable dtTypeAll;
        private void Admin_DataMatch_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.AutoGenerateColumns = true;
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now;

            flushTypeAll();
            string[] columNames = new string[] { "EnterType" };
            DataTable dtCompany = tools.dtFilter(dtTypeAll, columNames, "");
            this.comboBoxEx1.DataSource = dtCompany;
            if (dtCompany.Rows.Count > 0)
            {
                this.comboBoxEx1.DisplayMember = "EnterType";
                this.comboBoxEx1.ValueMember = "EnterType";
            }
        }
        DataTable dtEnter;
        private void flushTypeAll() 
        {
            dtTypeAll = DBHelper.ExecuteQuery("select CompanyNames,EnterType from Admin_PaySerialDetaile group by CompanyNames,EnterType");
        }

        private void buttonX4_Click(object sender, EventArgs e)//打开支付宝后台文件
        {
            if (this.comboBoxEx4.Text == "" || this.comboBoxEx5.Text == "")
            {
                MessageBox.Show("数据不全");
                return;
            }
            this.dataGridViewX1.AutoGenerateColumns = true;
            if (this.comboBoxEx5.Text == "个人支付宝" || this.comboBoxEx5.Text == "逸芸支付宝")
            {
                dtEnter = tools.GetDataSetFromExcel("支付宝", this.textBoxX2, "A:P");
            }
            else if (this.comboBoxEx5.Text == "企业支付宝")
            {
                dtEnter = tools.GetDataSetFromExcel("帐务组合查询", this.textBoxX2, "A:Q");
            }
            else if (this.comboBoxEx5.Text == "腾讯后台")
            {
                dtEnter = tools.GetDataSetFromExcel("腾讯后台", this.textBoxX2, "A:H");
            }
            else if (this.comboBoxEx5.Text == "对公账户")
            {
                dtEnter = tools.GetDataSetFromExcel("对公账户", this.textBoxX2, "A:N");
            }
            this.dataGridViewX1.DataSource = dtEnter;

            for (int i = 0; i < dataGridViewX1.Columns.Count; i++)
            {
                this.dataGridViewX1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void buttonX5_Click(object sender, EventArgs e)//支付宝后台数据导入数据库
        {
            if (MessageBox.Show("你确定要将支付宝数据导入数据库吗?", "导入数据库提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Maximum = dtEnter.Rows.Count;
            this.progressBarX1.Value = 0;
            this.progressBarX1.Step = 1;

            int successCount = 0;
            int defeatedCount = 0;

            for (int i = 0; i < dtEnter.Rows.Count; i++)
            {
                string sql1= "";
                if (this.comboBoxEx5.Text == "逸芸支付宝") 
                {
                    sql1 = insert_PaySerial_YYPay(dtEnter.Rows[i]);
                }
                else if (this.comboBoxEx5.Text == "个人支付宝")
                {
                    sql1 = insert_PaySerial_Persom(dtEnter.Rows[i]);
                }
                else if (this.comboBoxEx5.Text == "企业支付宝")
                {
                    sql1 = insert_PaySerial_Company(dtEnter.Rows[i]);
                }
                else if (this.comboBoxEx5.Text == "腾讯后台")
                {
                    try { sql1 = insert_PaySerial_Tencent(dtEnter.Rows[i], dtEnter.Rows[i + 1]); }
                    catch { sql1 = insert_PaySerial_Tencent(dtEnter.Rows[i], null); }
                }
                else if (this.comboBoxEx5.Text == "对公账户")
                {
                    sql1 = insert_PaySerial_Duigong(dtEnter.Rows[i]);
                }
                int temp_Count = DBHelper.ExecuteUpdate(sql1);
                this.progressBarX1.Value++;
                if (temp_Count == 0)
                    defeatedCount++;
                else
                    successCount += temp_Count;
                this.progressBarX1.Value++;
            }
            //this.progressBarX1.Value = dtEnter.Rows.Count;
            MessageBox.Show("成功导入:" + successCount + "条，失败:" + defeatedCount + "条！");
        }

        private void buttonX6_Click(object sender, EventArgs e)//清空数据
        {
            string sql1 = string.Format("delete from Admin_PaySerialDetaile");
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("清空成功");
                this.dataGridViewX1.DataSource = null;
            }
        }

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            if (dtCompany.Rows.Count == 0)
                return;
            this.comboBoxEx4.DataSource = dtCompany.Copy();
            this.comboBoxEx4.DisplayMember = "CompanyNames";
            this.comboBoxEx4.ValueMember = "CompanyNames";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void comboBoxEx6_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(dtTypeAll, columNames, "");
            if (dtCompany.Rows.Count == 0)
                return;
            this.comboBoxEx6.DataSource = dtCompany.Copy();
            this.comboBoxEx6.DisplayMember = "CompanyNames";
            this.comboBoxEx6.ValueMember = "CompanyNames";
            this.comboBoxEx6.SelectedIndex = -1;
        }
        private void comboBoxEx6_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx6.Text);
            string[] columNames = new string[] { "EnterType" };
            DataTable dtCompany = tools.dtFilter(dtTypeAll, columNames, sql);
            if (dtCompany.Rows.Count == 0)
                return;
            this.comboBoxEx1.DataSource = dtCompany.Copy();
            this.comboBoxEx1.DisplayMember = "EnterType";
            this.comboBoxEx1.ValueMember = "EnterType";
            this.comboBoxEx1.SelectedIndex = -1;
        }
        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxEx6.Text == "") 
            {
                string[] columNames = new string[] { "EnterType" };
                DataTable dtCompany = tools.dtFilter(dtTypeAll, columNames, "");
                if (dtCompany.Rows.Count == 0)
                    return;
                this.comboBoxEx1.DataSource = dtCompany.Copy();
                this.comboBoxEx1.DisplayMember = "EnterType";
                this.comboBoxEx1.ValueMember = "EnterType";
            }
        }

        //逸芸支付宝记录插入
        private string insert_PaySerial_YYPay(DataRow dtRow)
        {
            string sql1 = "";
            string enterType = "";
            float value = 0;
            if (dtRow[10].ToString().Trim() != "")
            {
                if (dtRow[8].ToString().Trim().Contains("-收益发放"))
                {
                    enterType = "中转";
                    value = float.Parse(dtRow[9].ToString());
                    sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                        "values('{0}','逸芸支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,{4})",
                        this.comboBoxEx4.Text.Trim(),
                        DateTime.Parse(dtRow[2].ToString()),//交易日期
                        "余额宝收益发放",//交易类型
                        enterType,//收入/支出/中转
                        value,//金额
                        dtRow[0].ToString().Trim(),//交易号
                        tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                        dtRow[12].ToString().Trim(),//服务费
                        dtRow[8].ToString().Trim(),//备注  
                        frmUTSOFTMAIN.StaffID);
                }
                else
                {
                    enterType = dtRow[10].ToString();
                    value = float.Parse(dtRow[9].ToString());
                    sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                        "values('{0}','逸芸支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,0)",
                        this.comboBoxEx4.Text.Trim(),
                        DateTime.Parse(dtRow[2].ToString()),//交易日期
                        "",//交易类型
                        enterType,//收入/支出/中转
                        value,//金额
                        dtRow[0].ToString().Trim(),//交易号
                        tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                        dtRow[12].ToString().Trim(),//服务费
                        dtRow[8].ToString().Trim(),//备注  
                        frmUTSOFTMAIN.StaffID);
                }
            }
            else
            {
                enterType = "中转";
                value = float.Parse(dtRow[9].ToString());
                sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                    "values('{0}','逸芸支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,{4})",
                    this.comboBoxEx4.Text.Trim(),
                    DateTime.Parse(dtRow[2].ToString()),//交易日期
                    "内部中转",//交易类型
                    enterType,//收入/支出/中转
                    value,//金额
                    dtRow[0].ToString().Trim(),//交易号
                    tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                    dtRow[12].ToString().Trim(),//服务费
                    dtRow[8].ToString().Trim(),//备注  
                    frmUTSOFTMAIN.StaffID);
            }
            return sql1;
        }
        //个人支付宝记录插入
        private string insert_PaySerial_Persom(DataRow dtRow) 
        {
            string sql1 = "";
            string enterType = "";
            float value = 0;
            if (dtRow[10].ToString().Trim() != "")
            {
                if (dtRow[8].ToString().Trim().Contains("-收益发放"))
                {
                    enterType = "中转";
                    value = float.Parse(dtRow[9].ToString());
                    sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                        "values('{0}','个人支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,{4})",
                        this.comboBoxEx4.Text.Trim(),
                        DateTime.Parse(dtRow[2].ToString()),//交易日期
                        "余额宝收益发放",//交易类型
                        enterType,//收入/支出/中转
                        value,//金额
                        dtRow[0].ToString().Trim(),//交易号
                        tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                        dtRow[12].ToString().Trim(),//服务费
                        dtRow[8].ToString().Trim(),//备注  
                        frmUTSOFTMAIN.StaffID);
                }
                else
                {
                    enterType = dtRow[10].ToString();
                    value = float.Parse(dtRow[9].ToString());
                    sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                        "values('{0}','个人支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,0)",
                        this.comboBoxEx4.Text.Trim(),
                        DateTime.Parse(dtRow[2].ToString()),//交易日期
                        "",//交易类型
                        enterType,//收入/支出/中转
                        value,//金额
                        dtRow[0].ToString().Trim(),//交易号
                        tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                        dtRow[12].ToString().Trim(),//服务费
                        dtRow[8].ToString().Trim(),//备注  
                        frmUTSOFTMAIN.StaffID);
                }
            }
            else 
            {
                enterType = "中转";
                value = float.Parse(dtRow[9].ToString());
                sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                    "values('{0}','个人支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,{4})",
                    this.comboBoxEx4.Text.Trim(),
                    DateTime.Parse(dtRow[2].ToString()),//交易日期
                    "内部中转",//交易类型
                    enterType,//收入/支出/中转
                    value,//金额
                    dtRow[0].ToString().Trim(),//交易号
                    tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                    dtRow[12].ToString().Trim(),//服务费
                    dtRow[8].ToString().Trim(),//备注  
                    frmUTSOFTMAIN.StaffID);
            }
            return sql1;
        }
        //公司支付宝记录插入
        private string insert_PaySerial_Company(DataRow dtRow)
        {
            string sql1 = "";
            string enterType = "";
            float value = 0;
            if (dtRow[6].ToString() != "")
            {
                enterType = "收入";
                value = float.Parse(dtRow[9].ToString());
                sql1 = string.Format("insert into Admin_PaySerialDetaile " +
                    "values('{0}','公司支付宝','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,0)",
                    this.comboBoxEx4.Text.Trim(),
                    DateTime.Parse(dtRow[1].ToString()),//交易日期
                    "",//交易类型
                    enterType,//收入/支出/中转
                    dtRow[6].ToString().Trim(),//金额
                    dtRow[2].ToString().Trim(),//交易号
                    tools.FilteSQLStr(dtRow[13].ToString()),//交易对方
                    dtRow[9].ToString().Trim(),//服务费
                    dtRow[16].ToString().Trim(),//备注  
                    frmUTSOFTMAIN.StaffID);
            }
            return sql1;
        }
        //腾讯后台记录插入
        private string insert_PaySerial_Tencent(DataRow dtRow, DataRow dtRow1)
        {
            string sql1 = "";
            if (double.Parse(dtRow[5].ToString().Trim()) == 0)
                return "";
            if (dtRow[1].ToString().Trim() != "0")
            {
                sql1 += string.Format(" insert into Admin_PaySerialDetaile " +
                "values('{0}','腾讯后台','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,0)",
                this.comboBoxEx4.Text.Trim(),
                DateTime.Parse(dtRow[1].ToString()),//交易日期
                "",//交易类型
                "收入",//收入/支出/中转
                dtRow[5].ToString().Trim(),//金额
                dtRow[0].ToString().Trim(),//交易号
                tools.FilteSQLStr(dtRow[2].ToString()),//交易对方
                0,//服务费
                "腾讯后台数据",//备注  
                frmUTSOFTMAIN.StaffID);

                sql1 += string.Format(" insert into Admin_PaySerialDetaile " +
                    "values('{0}','腾讯后台','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,{4})",
                    this.comboBoxEx4.Text.Trim(),
                    DateTime.Parse(dtRow[1].ToString()),//交易日期
                    "腾讯平台手续费",//交易类型
                    "支出",//收入/支出/中转
                    double.Parse(dtRow[5].ToString().Trim()) - double.Parse(dtRow[6].ToString().Trim()),//金额
                    dtRow[0].ToString().Trim(),//交易号
                    tools.FilteSQLStr(dtRow[2].ToString()),//交易对方
                    0,//服务费
                    "腾讯后台服务费记录",//备注  
                    frmUTSOFTMAIN.StaffID);
            }
            else 
            {
                sql1 += string.Format(" insert into Admin_PaySerialDetaile " +
                "values('{0}','腾讯后台','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,{4})",
                this.comboBoxEx4.Text.Trim(),
                DateTime.Parse(dtRow1[1].ToString()),//交易日期
                "腾讯后台奖励",//交易类型
                "收入",//收入/支出/中转
                dtRow[5].ToString().Trim(),//金额
                dtRow[0].ToString().Trim(),//交易号
                tools.FilteSQLStr(dtRow[2].ToString()),//交易对方
                0,//服务费
                "腾讯后台奖励",//备注  
                frmUTSOFTMAIN.StaffID);
            }

            return sql1;
        }
        //对公账户记录插入
        private string insert_PaySerial_Duigong(DataRow dtRow)
        {
            string sql1 = "";
            if (dtRow[3].ToString() != "0")
            {
                sql1 += string.Format(" insert into Admin_PaySerialDetaile " +
                "values('{0}','对公账户','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,0)",
                this.comboBoxEx4.Text.Trim(),
                dtRow[2].ToString(),//交易日期
                "",//交易类型
                "支出",//收入/支出/中转
                dtRow[3].ToString().Trim(),//金额
                dtRow[13].ToString().Trim(),//交易号
                tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                0,//服务费
                dtRow[12].ToString(),//备注  
                frmUTSOFTMAIN.StaffID);
            }
            else 
            {
                sql1 += string.Format(" insert into Admin_PaySerialDetaile " +
                "values('{0}','对公账户','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}',getdate(),'{9}',0,0)",
                this.comboBoxEx4.Text.Trim(),
                dtRow[2].ToString(),//交易日期
                "",//交易类型
                "收入",//收入/支出/中转
                dtRow[4].ToString().Trim(),//金额
                dtRow[13].ToString().Trim(),//交易号
                tools.FilteSQLStr(dtRow[7].ToString()),//交易对方
                0,//服务费
                dtRow[12].ToString(),//备注  
                frmUTSOFTMAIN.StaffID);
            }
            return sql1;
        }

        private void buttonX3_Click(object sender, EventArgs e)//匹配预定报名
        {
            ppMethod("b.EnterType", "b.EnterType", "b.id", "b.PayMoney", "Pub_VIPMessage", "SerialNum");
        }
        private void buttonX7_Click(object sender, EventArgs e)//匹配退款
        {
            ppMethod("b.SubmitType", "b.SubmitType", "b.id", "b.RefundValue", "Pub_ComplainRefundInfo", "RefundSerialNum");
        }
        private void buttonX13_Click(object sender, EventArgs e)//匹配4Y4记录
        {
            ppMethod("b.id", "'4Y4购买'", "b.id", "b.PayValue", "UT_FYFBuy", "SerialNum");
        }
        private void buttonX12_Click(object sender, EventArgs e)//匹配UT腾讯转移
        {
            ppMethod("b.id", "'腾讯转移'", "b.id", "b.FKValue", "UT_TXZhuanYi", "SerialNum");
        }
        private void buttonX10_Click(object sender, EventArgs e)//匹配日常收支和内部转账
        {
            ppMethod("b.EnterType", "b.EnterType", "b.id", "b.Value", "Pub_SerialDetail", "SerialNum");
        }
        private void buttonX9_Click(object sender, EventArgs e)//匹配腾讯型公司转移返款
        {
            ppMethod("b.EnterType", "b.EnterType", "b.id", "b.Value", "Pub_BackMoney", "TXSerialNum");
        }
        private void buttonX8_Click(object sender, EventArgs e)//匹配淘大型公司刷单
        {
            ppMethod("b.id", "'刷单'", "b.id", "b.FlushMoney", "Pub_FlushList", "SerialNum");
        }
        private void ppMethod( string enterType,string enterValue, string idstr, string parentValue,string tablename, string serialNum)
        {
            string sqlpp = string.Format("update a set " +
                " a.EnterType =(case when  {9} is null then '未匹配到' else {12} end)," +
                " a.ParentID = (case when  {10} is null then 0 else {10} end)," +
                " a.ParentValue = (case when  {11} is null then 0 else {11} end)" +
                " from Admin_PaySerialDetaile a left join {7} b on charindex(a.SerialNum,b.{8})>0" +
                " where a.CompanyNames like '%{0}%' and a.EnterTime between '{1}' and '{2}' and" +
                " (a.EnterType = '未匹配到' or a.EnterType = '') and a.PayOrGet like '%{4}%' and "+
                " a.SerialNum like '%{5}%' and a.SerialPerson like '%{6}%' and a.TableName like '%{13}%'",
                this.comboBoxEx6.Text.Trim(),
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx1.Text.Trim(),
                this.comboBoxEx2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.textBoxX4.Text.Trim(), 
                tablename, serialNum, 
                enterType, idstr, 
                parentValue, enterValue,
                this.comboBoxEx3.Text.Trim());
            if (this.textBoxX1.Text.Trim() != "")
                sqlpp += string.Format(" and a.Value = {0}", this.textBoxX1.Text.Trim());
            if (this.checkBoxX1.Checked == true)
                sqlpp += " and Value != ParentValue";
            DBHelper.ExecuteUpdate(sqlpp);
            ppDate();
        }
        private void ppDate() 
        {
            string sql1 = string.Format("select "+
                " id,CompanyNames as 公司,TableName as 账户类型," +
                " EnterTime as 时间,"+
                " EnterType as 类型,"+
                " PayOrGet as 收支类型,"+
                " Value as 金额,"+
                " '*'+SerialNum as 交易号,"+
                " SerialPerson as 交易对方,"+
                " ServerValue as 服务费,"+
                " Remark as 备注," +
                " ParentValue as 配对金额,"+
                " SubmitTime,SubmitPerson,ParentID" +
                " from  Admin_PaySerialDetaile" +
                " where CompanyNames like '%{0}%' and EnterTime between '{1}' and '{2}' and" +
                " EnterType like '%{3}%' and PayOrGet like '%{4}%' and SerialNum like '%{5}%' "+
                " and SerialPerson like '%{6}%' and TableName like '%{7}%'",
                this.comboBoxEx6.Text.Trim(),
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx1.Text.Trim(),
                this.comboBoxEx2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.textBoxX4.Text.Trim(),
                this.comboBoxEx3.Text.Trim());
            if (this.textBoxX1.Text.Trim() != "")
                sql1 += string.Format(" and Value = {0}", this.textBoxX1.Text.Trim());
            if (this.checkBoxX1.Checked == true)
                sql1 += " and Value != ParentValue and EnterType not in('未匹配到','')";
            sql1 += " order by EnterTime asc";

            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("无记录！");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;

            for (int i = 0; i < dataGridViewX1.Columns.Count; i++)
            {

                if (i == 1 || i == 5 || i == 6 || i == 9 || i == 11)
                {
                    this.dataGridViewX1.Columns[i].Width = 90;
                }
                else
                {
                    this.dataGridViewX1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }

            this.dataGridViewX1.Columns[0].Visible = false;
            this.dataGridViewX1.Columns[12].Visible = false;
            this.dataGridViewX1.Columns[13].Visible = false;
            this.dataGridViewX1.Columns[14].Visible = false;
            double allValue = 0;
            for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
            {
                allValue += double.Parse(this.dataGridViewX1.Rows[i].Cells[6].Value.ToString());
                if (this.dataGridViewX1.Rows[i].Cells[6].Value.ToString() == this.dataGridViewX1.Rows[i].Cells[11].Value.ToString()&&
                    this.dataGridViewX1.Rows[i].Cells[4].Value.ToString() != ""&&
                    this.dataGridViewX1.Rows[i].Cells[4].Value.ToString() != "未匹配到")
                {
                    this.dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(143, 188, 143);
                }
                else if (this.dataGridViewX1.Rows[i].Cells[6].Value.ToString() != this.dataGridViewX1.Rows[i].Cells[11].Value.ToString() &&
                    this.dataGridViewX1.Rows[i].Cells[4].Value.ToString() != "" &&
                    this.dataGridViewX1.Rows[i].Cells[4].Value.ToString() != "未匹配到")
                {
                    this.dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 181, 197);
                }
                ////p排除
                //if (double.Parse(this.dataGridViewX1.Rows[i].Cells[6].Value.ToString())*10 == 
                //    double.Parse(this.dataGridViewX1.Rows[i].Cells[11].Value.ToString()) &&
                //    this.dataGridViewX1.Rows[i].Cells[4].Value.ToString() != "" &&
                //    this.dataGridViewX1.Rows[i].Cells[4].Value.ToString() != "未匹配到")
                //{
                //    this.dataGridViewX1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(143, 188, 143);
                //}
            }
            this.labelX10.Text = "总金额： " + allValue + " 元";
            flushTypeAll();
        }
        private void changeColor() 
        {
            
        }
        private void buttonX11_Click(object sender, EventArgs e)
        {
            ppDate();
        }  
    }
}
