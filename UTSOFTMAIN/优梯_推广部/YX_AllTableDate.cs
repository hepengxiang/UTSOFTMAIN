using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.优梯_推广部
{
    public partial class YX_AllTableDate : Form
    {
        public YX_AllTableDate()
        {
            InitializeComponent();
        }

        private void YX_AllTableDate_Load(object sender, EventArgs e)
        {
            this.dateTimeInput1.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.AutoGenerateColumns = true;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql1 = "";
            if (this.comboBoxEx4.Text == "个人QQ")
            {
                if (this.comboBoxEx5.Text == "个人QQ")
                {
                    if(this.comboBoxEx6.Text == "QQ空间")
                    {
                        
                    }
                    else if (this.comboBoxEx6.Text == "个人QQ")
                    {
                        
                    }
                    else if (this.comboBoxEx6.Text == "每日小结")
                    {

                    }
                }
                else if (this.comboBoxEx5.Text == "QQ群")
                {
                    if (this.comboBoxEx6.Text == "资源群")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "活跃群")
                    {

                    }
                }
            }
            else if (this.comboBoxEx4.Text == "营销QQ")
            {
                if (this.comboBoxEx5.Text == "资源类")
                {
                    if (this.comboBoxEx6.Text == "后台订购")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "外部资源")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "个人CRM")
                    {

                    }
                }
                else if (this.comboBoxEx5.Text == "开发类")
                {
                    if (this.comboBoxEx6.Text == "意向数")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "近期资源")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "老资源")
                    {

                    }
                }
                else if (this.comboBoxEx5.Text == "总结类")
                {
                    if (this.comboBoxEx6.Text == "每日好评")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "表格汇总")
                    {

                    }
                    else if (this.comboBoxEx6.Text == "个人总结")
                    {

                    }
                }
            }
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if(dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = dt1;
            for (int i = 0; i < dataGridViewX1.Columns.Count; i++)
            {
                this.dataGridViewX1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void comboBoxEx2_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部'");
            string[] columNames = new string[] { "GroupName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPerson;
            this.comboBoxEx2.ValueMember = "GroupName";
            this.comboBoxEx2.DisplayMember = "GroupName";
            this.comboBoxEx2.SelectedIndex = -1;
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "")
                return;
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部' and GroupName = '{0}'", this.comboBoxEx2.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.DataSource = dtPerson;
            this.comboBoxEx3.ValueMember = "StaffID";
            this.comboBoxEx3.DisplayMember = "AssumedName";
            this.comboBoxEx3.SelectedIndex = -1;
        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxEx5.Text = "";
            this.comboBoxEx5.Items.Clear();
            this.comboBoxEx6.Text = "";
            this.comboBoxEx6.Items.Clear();
            if (this.comboBoxEx4.Text == "个人QQ")
            {
                this.comboBoxEx5.Items.Add("个人QQ");
                this.comboBoxEx5.Items.Add("QQ群");
            }
            else if (this.comboBoxEx4.Text == "营销QQ")
            {
                this.comboBoxEx5.Items.Add("资源类");
                this.comboBoxEx5.Items.Add("开发类");
                this.comboBoxEx5.Items.Add("总结类");
            }
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBoxEx6.Text = "";
            this.comboBoxEx6.Items.Clear();
            if (this.comboBoxEx4.Text == "个人QQ")
            {
                if(this.comboBoxEx5.Text == "个人QQ")
                {
                    this.comboBoxEx6.Items.Add("QQ空间");
                    this.comboBoxEx6.Items.Add("个人QQ");
                    this.comboBoxEx6.Items.Add("每日小结");
                }
                else if (this.comboBoxEx5.Text == "QQ群")
                {
                    this.comboBoxEx6.Items.Add("资源群");
                    this.comboBoxEx6.Items.Add("活跃群");
                }
            }
            else if (this.comboBoxEx4.Text == "营销QQ")
            {
                if (this.comboBoxEx5.Text == "资源类")
                {
                    this.comboBoxEx6.Items.Add("后台订购");
                    this.comboBoxEx6.Items.Add("外部资源");
                    this.comboBoxEx6.Items.Add("个人CRM");
                }
                else if (this.comboBoxEx5.Text == "开发类")
                {
                    this.comboBoxEx6.Items.Add("意向数");
                    this.comboBoxEx6.Items.Add("近期资源");
                    this.comboBoxEx6.Items.Add("老资源");
                }
                else if (this.comboBoxEx5.Text == "总结类")
                {
                    this.comboBoxEx6.Items.Add("每日好评");
                    this.comboBoxEx6.Items.Add("表格汇总");
                    this.comboBoxEx6.Items.Add("个人总结");
                }
            }
        }
    }
}
