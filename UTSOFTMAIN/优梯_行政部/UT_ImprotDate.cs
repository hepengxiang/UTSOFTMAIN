using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.优梯_行政部
{
    public partial class UT_ImprotDate : Form
    {
        public UT_ImprotDate()
        {
            InitializeComponent();
        }

        private void UT_ImprotDate_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.AutoGenerateColumns = true;

            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now;
        }
        DataTable dtEnter;
        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.dataGridViewX1.AutoGenerateColumns = true;

            if (this.comboBoxEx1.Text == "优梯")
            {
                if (this.comboBoxEx3.Text == "报名")
                {
                    dtEnter = tools.GetDataSetFromExcel("报名", this.textBoxX1, "A:P");
                }
                else if (this.comboBoxEx3.Text == "内部转账")
                {
                    dtEnter = tools.GetDataSetFromExcel("内部转账", this.textBoxX1, "A:J");
                }
                else if (this.comboBoxEx3.Text == "4Y4购买记录")
                {
                    dtEnter = tools.GetDataSetFromExcel("购买记录", this.textBoxX1, "A:G");
                }
                else if (this.comboBoxEx3.Text == "腾讯转移记录")
                {
                    dtEnter = tools.GetDataSetFromExcel("腾讯转移记录", this.textBoxX1, "A:K");
                }
                else if (this.comboBoxEx3.Text == "日常支出")
                {
                    dtEnter = tools.GetDataSetFromExcel("收支明细", this.textBoxX1, "A:I");
                }
                else
                {
                    MessageBox.Show("选择错误!");
                    return;
                }
            }
            else if (this.comboBoxEx1.Text == "淘大")
            {
                if (this.comboBoxEx3.Text == "预定")
                {
                    dtEnter = tools.GetDataSetFromExcel("预定", this.textBoxX1, "A:N");
                }
                else if (this.comboBoxEx3.Text == "报名")
                {
                    dtEnter = tools.GetDataSetFromExcel("报名", this.textBoxX1, "A:L");
                }
                else
                {
                    MessageBox.Show("请选择表类型");
                }
            }
            else if (this.comboBoxEx1.Text == "腾讯")
            {
                if (this.comboBoxEx3.Text == "预定")
                {
                    dtEnter = tools.GetDataSetFromExcel("预定", this.textBoxX1, "A:L");
                }
                else if (this.comboBoxEx3.Text == "报名")
                {
                    dtEnter = tools.GetDataSetFromExcel("报名", this.textBoxX1, "A:L");
                }
                else if (this.comboBoxEx3.Text == "支付宝转移")
                {
                    dtEnter = tools.GetDataSetFromExcel("支付宝转移", this.textBoxX1, "A:J");
                }
                else
                {
                    MessageBox.Show("请选择表类型");
                }
            }
            else
            {
                MessageBox.Show("请选择公司类型");
                return;
            }
            this.dataGridViewX1.DataSource = dtEnter;

            for (int i = 0; i < dataGridViewX1.Columns.Count; i++)
            {
                this.dataGridViewX1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text.Trim() == "" || this.comboBoxEx2.Text.Trim() == "" || this.comboBoxEx3.Text.Trim() == "")
            {
                MessageBox.Show("数据不全");
                return;
            }
            if (MessageBox.Show("你确定要导入数据库吗?", "导入数据库提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Maximum = dtEnter.Rows.Count;
            this.progressBarX1.Value = 0;
            this.progressBarX1.Step = 1;

            int successCount = 0;
            int defeatedCount = 0;

            for (int i = 0; i < dtEnter.Rows.Count; i++)
            {
                for (int j = 0; j < dtEnter.Columns.Count; j++)
                {
                    if (dtEnter.Rows[i][j].ToString() != "")
                    {
                        dtEnter.Rows[i][j] = tools.FilteSQLStr(dtEnter.Rows[i][j].ToString().Trim());
                    }
                }
                string sql1 = "";
                try
                {
                    if (this.comboBoxEx1.Text == "优梯")
                    {
                        if (this.comboBoxEx3.Text.Trim() == "报名")
                        {
                            sql1 = insertUT(dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "内部转账")
                        {
                            sql1 = insert_NBZZ(this.comboBoxEx1.Text.Trim(), dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "4Y4购买记录")
                        {
                            sql1 = insertUT_FYFBuy(dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "腾讯转移记录")
                        {
                            sql1 = insertUT_TXZY(dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "日常支出")
                        {
                            sql1 = insert_RCZC(this.comboBoxEx1.Text.Trim(), dtEnter.Rows[i]);
                        }
                    }
                    else if (this.comboBoxEx1.Text == "淘大")
                    {
                        if (this.comboBoxEx3.Text.Trim() == "预定")
                        {
                            sql1 = insertTD_YD(dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "报名")
                        {
                            sql1 = insertTD_BM(dtEnter.Rows[i]);
                        }
                    }
                    else if (this.comboBoxEx1.Text == "腾讯")
                    {
                        if (this.comboBoxEx3.Text.Trim() == "预定")
                        {
                            sql1 = insertTX_YD(dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "报名")
                        {
                            sql1 = insertTX_BM(dtEnter.Rows[i]);
                        }
                        else if (this.comboBoxEx3.Text.Trim() == "转移")
                        {
                            sql1 = insertTX_ZY(dtEnter.Rows[i]);
                        }
                    }
                }
                catch { }
                int temp_Count = DBHelper.ExecuteUpdate(sql1);
                this.progressBarX1.Value++;
                if (temp_Count == 0)
                {
                    defeatedCount++;
                }
                else
                    successCount += temp_Count;
                this.progressBarX1.Value++;

            }
            this.progressBarX1.Value = dtEnter.Rows.Count;
            MessageBox.Show("导入成功:" + successCount + "条，失败:" + defeatedCount + "条！");
        }

        private string insertUT(DataRow dtRow)
        {
            string[] dtTimes = dtRow[0].ToString().Split(new char[] { '月' });
            string sDate = "";
            string sql1 = "";
            if (dtTimes[0] == "12")
            {
                sDate = "2016-" + dtTimes[0] + "-" + dtTimes[1] + " 00:00:00";
            }
            else
            {
                sDate = "2017-" + dtTimes[0] + "-" + dtTimes[1] + " 00:00:00";
            }
            DateTime dtTime = DateTime.Parse(sDate);
            if (dtRow[1].ToString().Contains("报名"))
            {
                //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
                //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
                sql1 = string.Format("insert into Pub_VIPMessage values('优梯','{0}','报名','{1}','{2}','无','{3}'," +
                    "'{4}','{5}','{6}','{7}','无','{8}','无','{9}',{10},'无','{11}','{12}','{13}',getdate())",
                    dtTime,
                    dtRow[2].ToString(),
                    dtRow[3].ToString().Trim(),
                    dtRow[6].ToString().Trim(),
                    dtRow[5].ToString().Trim(),
                    dtRow[4].ToString().Trim(),
                    dtRow[8].ToString().Trim(),
                    dtRow[9].ToString().Trim(),
                    dtRow[10].ToString().Trim(),
                    dtRow[14].ToString().Trim(),//意向老师
                    dtRow[11].ToString().Trim(),//付款金额
                    dtRow[12].ToString().Trim(),//交易流水号
                    dtRow[13].ToString().Trim(),//备注
                    frmUTSOFTMAIN.StaffID);
            }
            else if (dtRow[1].ToString()=="")
            {
                //插入尾款补齐信息
                sql1 = string.Format("insert into Pub_VIPMessage values('优梯','{0}','尾款补齐','{1}','{2}','无','{3}'," +
                    "'{4}','{5}','{6}','{7}','无','{8}','无','{9}',{10},'无','{11}','{12}','{13}',getdate())",
                    dtTime,
                    dtRow[2].ToString().Trim(),
                    dtRow[3].ToString().Trim(),
                    dtRow[6].ToString().Trim(),
                    dtRow[5].ToString().Trim(),
                    dtRow[4].ToString().Trim(),
                    dtRow[8].ToString().Trim(),
                    dtRow[9].ToString().Trim(),
                    dtRow[10].ToString().Trim(),
                    dtRow[14].ToString().Trim(),//意向老师
                    dtRow[11].ToString().Trim(),//付款金额
                    dtRow[12].ToString().Trim(),//交易流水号
                    dtRow[13].ToString().Trim(),//备注
                    frmUTSOFTMAIN.StaffID);
            }
            else
            {
                //插入预定信息
                sql1 = string.Format("insert into Pub_VIPMessage values('优梯','{0}','预定','{1}','{2}','无','{3}'," +
                    "'{4}','{5}','{6}','{7}','无','{8}','无','{9}',{10},'无','{11}','{12}','{13}',getdate())",
                    dtTime,
                    dtRow[2].ToString().Trim(),
                    dtRow[3].ToString().Trim(),
                    dtRow[6].ToString().Trim(),
                    dtRow[5].ToString().Trim(),
                    dtRow[4].ToString().Trim(),
                    dtRow[8].ToString().Trim(),
                    dtRow[9].ToString().Trim(),
                    dtRow[10].ToString().Trim(),
                    dtRow[14].ToString().Trim(),//意向老师
                    dtRow[11].ToString().Trim(),//付款金额
                    dtRow[12].ToString().Trim(),//交易流水号
                    dtRow[13].ToString().Trim(),//备注
                    frmUTSOFTMAIN.StaffID);
            }
            return sql1;
        }
        private string insert_NBZZaaa(string companyNames, DataRow dtRow)
        {
            string sql1 = "";
            string value = "0";
            if (dtRow[2].ToString() != "")
                value = dtRow[2].ToString();
            else
                value = dtRow[3].ToString();
            //insert into Pub_SerialDetail values(所属公司,交易日期,交易类型,收入/支出/中转3,金额,事项,收款账户,
            //账户所属人,付款账户,账户所属人,交易流水号,备注,操作负责人,提交人,提交时间)
            sql1 = string.Format("insert into Pub_SerialDetail values('{0}','{1}','{2}','中转',{3},'{4}','{5}'," +
            ",'','','{6}','{7}','{8}','{9}',getdate()))",
                companyNames,
                dtRow[0].ToString(),
                dtRow[1].ToString().Trim(),
                value,
                dtRow[4].ToString().Trim(),
                dtRow[6].ToString().Trim(),
                dtRow[7].ToString().Trim(),
                dtRow[8].ToString().Trim(),
                dtRow[5].ToString().Trim(),
                frmUTSOFTMAIN.StaffID);
            return sql1;
        }
        
        private string insertUT_FYFBuy(DataRow dtRow)
        {
            
            string[] dtTimes = dtRow[0].ToString().Split(new char[] { '月' });
            string sDate = "";
            string sql1 = "";
            if (dtTimes[0] == "12")
            {
                sDate = "2016-" + dtTimes[0] + "-" + dtTimes[1] + " 00:00:00";
            }
            else
            {
                sDate = "2017-" + dtTimes[0] + "-" + dtTimes[1] + " 00:00:00";
            }
            DateTime dtTime = DateTime.Parse(sDate);
            sql1 = string.Format("insert into UT_FYFBuy values('{0}','{1}',{2},'{3}','{4}'," +
            "'{5}','{6}',getdate(),'','')",
                dtTime,
                dtRow[1].ToString().Trim(),
                dtRow[2].ToString().Trim(),
                dtRow[3].ToString().Trim(),
                dtRow[4].ToString().Trim(),
                dtRow[5].ToString().Trim(),
                dtRow[6].ToString().Trim());
            return sql1;
        }
        private string insertUT_TXZY(DataRow dtRow)
        {
            string[] dtTimes = dtRow[0].ToString().Split(new char[] { '月' });
            string sDate = "";
            string sql1 = "";
            if (dtTimes[0] == "12")
            {
                sDate = "2016-" + dtTimes[0] + "-" + dtTimes[1] + " 00:00:00";
            }
            else
            {
                sDate = "2017-" + dtTimes[0] + "-" + dtTimes[1] + " 00:00:00";
            }
            if (dtRow[7].ToString().Trim() == "0" || dtRow[7].ToString().Trim() == "")
                dtRow[7] = "0";
            DateTime dtTime = DateTime.Parse(sDate);
            //insert into Pub_SerialDetail values(购买时间,老QQ,新QQ,交易流水,返款账户,账户名称,联系电话,
            //返款金额,真实返款时间,备注,--,--,返款核实,--,--)
            sql1 = string.Format("insert into UT_TXZhuanYi values('{0}','{1}','{2}','{3}','{4}','{5}','{6}'," +
            " {7},'{8}','{10}','','','{9}','','')",
                dtTime,
                dtRow[1].ToString().Trim(),
                dtRow[2].ToString().Trim(),
                dtRow[3].ToString().Trim(),
                dtRow[4].ToString().Trim(),
                dtRow[5].ToString().Trim(),
                dtRow[6].ToString().Trim(),
                dtRow[7].ToString().Trim(),
                dtRow[8].ToString().Trim(),
                dtRow[9].ToString().Trim(),
                dtRow[10].ToString().Trim());
            return sql1;
        }

        private string insert_RCZC(string companyNames, DataRow dtRow)
        {
            string sql1 = "";
            string value = "0";
            string entertype = "";
            if (dtRow[2].ToString().Trim() != "")
            {
                value = dtRow[2].ToString();
                entertype = "收入";
            }
            else
            {
                value = dtRow[3].ToString();
                entertype = "支出";
            }
            //insert into Pub_SerialDetail values(所属公司,交易日期,交易类型,收入/支出/中转3,金额,事项,收款账户,
            //账户所属人,付款账户,账户所属人,交易流水号,备注,操作负责人,提交人,提交时间)
            sql1 = string.Format("insert into Pub_SerialDetail values('{0}','{1}','{2}','{3}',{4},'{5}','{6}',"+
            "'','','','{7}','{8}','{9}','{10}',getdate())",
                companyNames,
                dtRow[0].ToString(),
                dtRow[1].ToString().Trim(),
                entertype,
                value,
                dtRow[4].ToString().Trim(),
                dtRow[6].ToString().Trim(),
                dtRow[7].ToString().Trim(),
                dtRow[8].ToString().Trim(),
                dtRow[5].ToString().Trim(),
                frmUTSOFTMAIN.StaffID);
            return sql1;
        }
        private string insert_NBZZ(string companyNames, DataRow dtRow)
        {
            string sql1 = "";
            //insert into Pub_SerialDetail values(所属公司,交易日期,交易类型,收入/支出/中转3,金额,事项,收款账户,
            //账户所属人,付款账户,账户所属人,交易流水号,备注,操作负责人,提交人,提交时间)
            sql1 = string.Format("insert into Pub_SerialDetail values('{0}','{1}','{2}','中转',{3},'','{4}'," +
            "'{5}','{6}','{7}','{8}','{9}','{10}','{11}',getdate())",
                companyNames,
                dtRow[0].ToString(),
                dtRow[1].ToString().Trim(),
                dtRow[2].ToString().Trim(),
                dtRow[4].ToString().Trim(),
                dtRow[5].ToString().Trim(),
                dtRow[6].ToString().Trim(),
                dtRow[7].ToString().Trim(),
                dtRow[8].ToString().Trim(),
                dtRow[9].ToString().Trim(),
                dtRow[3].ToString().Trim(),
                frmUTSOFTMAIN.StaffID);
            return sql1;
        }



        private string insertTD_BM(DataRow dtRow)
        {
            string sql1 = "";

            //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
            //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
            sql1 = string.Format("insert into Pub_VIPMessage values('{0}','{1}','报名','{2}','{3}','{4}','{5}'," +
                "'{6}','{7}','{8}','{9}','无','无','{10}','{11}',{12},'无','{13}','{14}','{15}',getdate())",
                this.comboBoxEx2.Text.Trim(),
                dtRow[0].ToString(),
                dtRow[1].ToString(),//QQ昵称
                dtRow[2].ToString().Trim(),//QQ号
                dtRow[3].ToString().Trim(),//旺旺号
                dtRow[4].ToString().Trim(),//YY号 5
                dtRow[5].ToString().Trim(),//YY呢称
                dtRow[6].ToString().Trim(),//VIP群
                dtRow[7].ToString().Trim(),//姓名
                dtRow[8].ToString().Trim(),//联系电话
                dtRow[9].ToString().Trim(),//论坛名称 10
                dtRow[10].ToString().Trim(),//意向老师
                dtRow[11].ToString().Trim(),//付款金额 12
                dtRow[12].ToString().Trim(),//交易流水号
                dtRow[13].ToString().Trim(),//备注
                frmUTSOFTMAIN.StaffID);
            return sql1;
        }
        private string insertTD_YD(DataRow dtRow)
        {
            string sql1 = "";
            //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
            //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
            sql1 = string.Format("insert into Pub_VIPMessage values('{0}','{1}','预定','{2}','{3}','{4}','无'," +
                "'无','无','{5}','{6}','{7}','{8}','无','无',{9},'{10}','{11}','{12}','{13}',getdate())",
                this.comboBoxEx2.Text.Trim(),
                dtRow[0].ToString(),
                dtRow[1].ToString().Trim(),//QQ昵称
                dtRow[2].ToString().Trim(),//QQ号
                dtRow[3].ToString().Trim(),//旺旺号
                dtRow[4].ToString().Trim(),//姓名  5
                dtRow[5].ToString().Trim(),//联系电话
                dtRow[6].ToString().Trim(),//预定老师
                dtRow[7].ToString().Trim(),//付款渠道 8
                dtRow[8].ToString().Trim(),//付款金额
                dtRow[9].ToString().Trim(),//返款支付宝  10
                dtRow[10].ToString().Trim(),//交易流水号
                dtRow[11].ToString().Trim(),//补齐日期
                frmUTSOFTMAIN.StaffID);

            return sql1;
        }
        private string insertTX_BM(DataRow dtRow)
        {
            string sql1 = "";
            //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
            //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
            sql1 = string.Format("insert into Pub_VIPMessage values('{0}','{1}','{2}','{3}','{4}','无','无'," +
                "'无','{5}','{6}','{7}','无','{8}','无','无',{9},'无','{10}','{11}','{12}',getdate())",
                this.comboBoxEx2.Text.Trim(),
                dtRow[0].ToString(),
                dtRow[1].ToString(),//报名类型
                dtRow[2].ToString().Trim(),//QQ昵称  3
                dtRow[3].ToString().Trim(),//QQ号
                dtRow[4].ToString().Trim(),//所在VIP群  5
                //dtRow[5].ToString().Trim(),//绩效归属
                dtRow[6].ToString().Trim(),//姓名
                dtRow[7].ToString().Trim(),//联系电话  7
                dtRow[8].ToString().Trim(),//付款渠道
                dtRow[9].ToString().Trim(),//付款金额  
                dtRow[10].ToString().Trim(),//交易流水号 10
                dtRow[11].ToString().Trim(),//备注
                frmUTSOFTMAIN.StaffID);
            return sql1;
        }
        private string insertTX_YD(DataRow dtRow)
        {
            string sql1 = "";
            //insert into Pub_VIPMessage values(id,所属公司,录入日期,报名类型,QQ昵称,QQ号,旺旺号,YY号,
            //YY昵称,VIP群,姓名,联系电话,预定老师,付款渠道,论坛名称,意向老师,付款金额,返款支付宝,交易流水号,备注,提交人,提交时间)
            sql1 = string.Format("insert into Pub_VIPMessage values('{0}','{1}','{2}','{3}','{4}','无','无'," +
                "'无','无','{5}','{6}','无','{7}','无','无',{8},'无','{9}','{10}','{11}',getdate())",
                this.comboBoxEx2.Text.Trim(),
                dtRow[0].ToString(),
                dtRow[1].ToString(),//报名类型
                dtRow[2].ToString().Trim(),//QQ昵称  3
                dtRow[3].ToString().Trim(),//QQ号
                dtRow[4].ToString().Trim(),//姓名  5
                dtRow[5].ToString().Trim(),//联系电话
                dtRow[6].ToString().Trim(),//付款渠道
                dtRow[7].ToString().Trim(),//付款金额 8
                dtRow[8].ToString().Trim(),//交易流水号
                dtRow[9].ToString().Trim(),//备注 10
                frmUTSOFTMAIN.StaffID);

            return sql1;
        }
        private string insertTX_ZY(DataRow dtRow)
        {
            string sql1 = "";
            //insert into Pub_BackMoney values('所属公司','交易日期','记录类型','QQ昵称','QQ号','姓名','联系方式',金额,'备注','状态','腾讯交易流水号','提交人','提交时间')
            sql1 = string.Format("insert into Pub_BackMoney values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}','{9}','{10}','{11}',getdate())",
                this.comboBoxEx2.Text.Trim(),
                dtRow[0].ToString(),
                dtRow[1].ToString(),//记录类型
                dtRow[2].ToString().Trim(),//QQ昵称  3
                dtRow[3].ToString().Trim(),//QQ号
                dtRow[4].ToString().Trim(),//姓名  5
                dtRow[5].ToString().Trim(),//联系电话
                dtRow[6].ToString().Trim(),//金额
                dtRow[7].ToString().Trim(),//备注
                dtRow[8].ToString().Trim(),//状态
                dtRow[9].ToString().Trim(),//腾讯交易流水号
                frmUTSOFTMAIN.StaffID);
            return sql1;
        }

        private void buttonX11_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxEx6_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx6.DataSource = dtCompany.Copy();
            this.comboBoxEx6.DisplayMember = "CompanyNames";
            this.comboBoxEx6.ValueMember = "CompanyNames";
            this.comboBoxEx6.SelectedIndex = -1;
        }

        private void comboBoxEx2_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx2.DataSource = dtCompany.Copy();
            this.comboBoxEx2.DisplayMember = "CompanyNames";
            this.comboBoxEx2.ValueMember = "CompanyNames";
            this.comboBoxEx2.SelectedIndex = -1;
        }
    }
}
