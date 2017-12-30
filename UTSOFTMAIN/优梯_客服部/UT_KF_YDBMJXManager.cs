using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UTSOFTMAIN.优梯_客服部
{
    public partial class UT_KF_YDBMJXManager : Form
    {
        public int forID;
        public DataTable dtSource = new DataTable();
        public DateTime jxTime;
        public string enterType;
        public string companyNames = "优梯";
        public bool updateState = false;
        public UT_KF_YDBMJXManager()
        {
            InitializeComponent();
        }

        private void UT_KF_YDBMJXManager_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX1.DataSource = dtSource;
            this.buttonX1.Enabled = false;
            this.buttonX2.Enabled = false;
            if (updateState) 
            {
                this.buttonX1.Enabled = true;
                this.buttonX2.Enabled = true;
            }

            if (dtSource.Rows.Count!=0) 
            {
                this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
                this.comboBoxEx1_SelectedIndexChanged(null,null);
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            //this.comboBoxEx2.SelectedIndex = -1;
            this.comboBoxEx1_SelectedIndexChanged(null,null);
            this.comboBoxEx2.SelectedText = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.comboBoxEx2.SelectedValue = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要增加绩效吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            bool resultNum1;
            try
            {
                resultNum1 = PublicMethod.insert_YDBMJX(
                    forID,
                    this.comboBoxEx2.SelectedValue.ToString(),
                    jxTime,enterType,
                    this.comboBoxEx3.Text.Trim(),
                    float.Parse(this.comboBoxEx4.Text.Trim()),
                    this.textBoxX1.Text.Trim());
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            if (resultNum1)
            {
                MessageBox.Show("增加成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("对象：{0}，绩效类型：{1}，绩效小类：{2}，分值：{3}",
                    this.comboBoxEx2.SelectedValue.ToString(), enterType, this.comboBoxEx3.Text.Trim(),float.Parse(this.comboBoxEx4.Text.Trim()));
                frmUTSOFTMAIN.OperationObject = operationRemarkStr;
                frmUTSOFTMAIN.OperationRemark = "增加成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                string sqlFlush = string.Format("select a.id,c.QQ,c.NickName,b.DepartmentName,b.AssumedName,a.StaffID,a.EnterTypeSmall,a.Value,a.Remark" +
                    " from Pub_YDBM_JXManager a left join Users b on a.StaffID = b.StaffID left join Pub_VIPMessage c on a.ForID = c.id" +
                    " where a.StaffID = a.AmountManager and a.ForID = {0}", forID);
                    this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
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
            if (MessageBox.Show("你确定要删除选中行绩效数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            bool resultNum1 = true;
            try
            {
                resultNum1 = PublicMethod.delete_YDBMJX(
                    int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString()),
                    forID,
                    this.comboBoxEx2.SelectedValue.ToString(),
                    jxTime,
                    enterType,
                    this.comboBoxEx3.Text.Trim(),
                    float.Parse(this.comboBoxEx4.Text.Trim()));
            }
            catch { MessageBox.Show("删除失败！"); return; }
            if (resultNum1)
            {
                MessageBox.Show("删除成功");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("对象：{0}，绩效类型：{1}，绩效小类：{2}，分值：{3}",
                    this.comboBoxEx2.SelectedValue.ToString(), enterType, this.comboBoxEx3.Text.Trim(), float.Parse(this.comboBoxEx4.Text.Trim()));
                frmUTSOFTMAIN.OperationObject = operationRemarkStr;
                frmUTSOFTMAIN.OperationRemark = "删除成功";
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", companyNames,this.comboBoxEx1.Text);
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtPerson;
            this.comboBoxEx2.ValueMember = "StaffID";
            this.comboBoxEx2.DisplayMember = "AssumedName";
            //this.comboBoxEx2.SelectedIndex = -1;
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}'", companyNames);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx1.DataSource = dtPart;
            this.comboBoxEx1.ValueMember = "DepartmentName";
            this.comboBoxEx1.DisplayMember = "DepartmentName";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx2.Text == "")
                return;

            string sqlperson = string.Format("StaffID = '{0}'", this.comboBoxEx2.SelectedValue);
            string[] columNamesPerson = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNamesPerson, sqlperson);

            if (dtPerson.Rows.Count == 0)
                return;

            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' "+
                "and GroupName = '{2}' and UserType = '{3}' and JXType = '{4}'", 
                dtPerson.Rows[0][0].ToString(),
                dtPerson.Rows[0][1].ToString(),
                dtPerson.Rows[0][2].ToString(),
                dtPerson.Rows[0][3].ToString(), enterType);
            string[] columNames = new string[] { "JXTypeSmall" };
            DataTable dtJX= tools.dtFilter(frmUTSOFTMAIN.jxCaculateType, columNames, sql);
            if (dtJX.Rows.Count == 0) 
            {
                this.comboBoxEx3.DataSource = null;
                this.comboBoxEx3.Items.Clear();
                this.comboBoxEx3.Text = "";
                return;
            }
            this.comboBoxEx3.DataSource = dtJX;
            this.comboBoxEx3.ValueMember = "JXTypeSmall";
            this.comboBoxEx3.DisplayMember = "JXTypeSmall";
            //this.comboBoxEx3.SelectedIndex = -1;
        }

        //private void comboBoxEx3_DropDown(object sender, EventArgs e)
        //{
        //    string sql = string.Format("CompanyNames = '{0}' and JXType = '{1}'", companyNames, enterType);
        //    string[] columNames = new string[] { "JXTypeSmall" };
        //    DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.jxCaculateType, columNames, sql);
        //    this.comboBoxEx3.DataSource = dtPerson;
        //    this.comboBoxEx3.ValueMember = "JXTypeSmall";
        //    this.comboBoxEx3.DisplayMember = "JXTypeSmall";
        //    this.comboBoxEx3.SelectedIndex = -1;
        //}
    }
}
