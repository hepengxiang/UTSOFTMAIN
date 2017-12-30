using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace UTSOFTMAIN.总内部界面
{
    public partial class Admin_JXCaculate_BatchAdd : Form
    {
        public Admin_JXCaculate_BatchAdd()
        {
            InitializeComponent();
        }
        private void Admin_JXCaculate_BatchAdd_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
        }
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx4.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.comboBoxEx3.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.comboBoxEx2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.comboBoxEx15.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.comboBoxEx16.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.comboBoxEx17.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
            this.comboBoxEx18.Text = this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString();
            this.comboBoxEx19.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();
            this.textBoxX17.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();
            this.textBoxX18.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();
            this.textBoxX19.Text = this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString();
        }
        private static List<string> ids = new List<string>();
        private void buttonX1_Click(object sender, EventArgs e)
        {
            Label lbl = new Label();
            lbl.Text = this.comboBoxEx11.Text + "*" + this.comboBoxEx12.Text + "*" + this.comboBoxEx13.Text + "*" + this.comboBoxEx14.Text;
            lbl.Width = 240;
            lbl.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            lbl.BackColor = Color.Black;
            lbl.ForeColor = Color.Gold;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.DoubleClick += new EventHandler(lbl_DoubleClick);
            this.flowLayoutPanel1.Controls.Add(lbl);
        }

        void lbl_DoubleClick(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.Controls.Remove(sender as Label);
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要提交据吗?", "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            foreach(Control lab in this.flowLayoutPanel1.Controls)
            {
                string companyStr = lab.Text.Split(new char[] { '*' })[0];
                string departmentStr = lab.Text.Split(new char[] { '*' })[1];
                string groupStr = lab.Text.Split(new char[] { '*' })[2];
                string typeStr = lab.Text.Split(new char[] { '*' })[3];
                string sql1 = string.Format("insert into Pub_JXCaculate values" +
                "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},{10},{11},'{12}','{13}')",
                companyStr, departmentStr, groupStr, typeStr,
                this.comboBoxEx15.Text, this.comboBoxEx16.Text, this.comboBoxEx17.Text, this.comboBoxEx18.Text,
                this.comboBoxEx19.Text, this.textBoxX17.Text.Trim(), this.textBoxX18.Text.Trim(),
                this.textBoxX19.Text.Trim(), System.DateTime.Now, frmUTSOFTMAIN.StaffID);
                DBHelper.ExecuteUpdate(sql1);
                DataTable dtids = DBHelper.ExecuteQuery("select IDENT_CURRENT('Pub_JXCaculate')");
                ids.Add(dtids.Rows[0][0].ToString());
            }
            string sqlFlush = string.Format("select * from Pub_JXCaculate where id in (");
            foreach (string idstr in ids)
            {
                sqlFlush += idstr + ",";
            }
            sqlFlush = sqlFlush.Substring(0, sqlFlush.Length - 1);
            sqlFlush += ")";
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要增加删除选中数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from Pub_JXCaculate where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("删除成功!");
                frmUTSOFTMAIN.jxCaculateType = DBHelper.ExecuteQuery("select * from Pub_JXCaculate");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("CompanyNames = '{0}'," +
                    "DepartmentName = '{1}',GroupName = '{2}',UserType = '{3}'," +
                    "JXType = '{4}',JXTypeSmall = '{5}',JXCardinalNum = '{6}',DCCardinalNum = '{7}'," +
                    "DCCardinalType = '{8}',LowerLimit = '{9}',UpperLimit = '{10}',Value = '{11}'",
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
                    this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString());
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "增加：" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX3);
                //--------------日志结束------------------
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要增加修改选中数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update Pub_JXCaculate set CompanyNames = '{1}', DepartmentName = '{2}',GroupName = '{3}',UserType = '{4}'," +
                "JXType = '{5}',JXTypeSmall = '{6}',JXCardinalNum = '{7}',DCCardinalNum = '{8}',DCCardinalType = '{9}'," +
                "LowerLimit = {10},UpperLimit = {11},Value = {12} where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                this.comboBoxEx4.Text.Trim(),
                this.comboBoxEx3.Text.Trim(),
                this.comboBoxEx2.Text.Trim(),
                this.comboBoxEx1.Text.Trim(),
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
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.comboBoxEx4.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.comboBoxEx3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.comboBoxEx2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.comboBoxEx15.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[6].Value = this.comboBoxEx16.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[7].Value = this.comboBoxEx17.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[8].Value = this.comboBoxEx18.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[9].Value = this.comboBoxEx19.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[10].Value = this.textBoxX17.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[11].Value = this.textBoxX18.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[12].Value = this.textBoxX19.Text;
                frmUTSOFTMAIN.jxCaculateType = DBHelper.ExecuteQuery("select * from Pub_JXCaculate");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("CompanyNames = '{0}'," +
                    "DepartmentName = '{1}',GroupName = '{2}',UserType = '{3}'," +
                    "JXType = '{4}',JXTypeSmall = '{5}',JXCardinalNum = '{6}',DCCardinalNum = '{7}'," +
                    "DCCardinalType = '{8}',LowerLimit = '{9}',UpperLimit = '{10}',Value = '{11}'",
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
                    this.dataGridViewX1.SelectedRows[0].Cells[12].Value.ToString());
                frmUTSOFTMAIN.OperationObject = this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "修改：" + operationRemarkStr;
                frmUTSOFTMAIN.addlog(this.buttonX4);
                //--------------日志结束------------------
            }
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

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string sqlFlush = string.Format("select * from Pub_JXCaculate where " +
                "CompanyNames like '%{0}%' and DepartmentName like '%{1}%' and GroupName like '%{2}%' " +
                "and UserType like '%{3}%' and JXType like '%{4}%' order by SubmitTime desc",
                this.comboBoxEx4.Text, this.comboBoxEx3.Text, this.comboBoxEx2.Text,
                this.comboBoxEx1.Text, this.comboBoxEx15.Text);
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
        }

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            DataTable dtCompany = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx4.DataSource = dtCompany.Copy();
            this.comboBoxEx4.DisplayMember = "CompanyNames";
            this.comboBoxEx4.ValueMember = "CompanyNames";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx4.Text);
            string[] columNames = new string[] { "DepartmentName" };
            DataTable dtPart = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.DataSource = dtPart;
            this.comboBoxEx3.ValueMember = "DepartmentName";
            this.comboBoxEx3.DisplayMember = "DepartmentName";
            this.comboBoxEx3.SelectedIndex = -1;
        }

        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "" || this.comboBoxEx3.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx4.Text, this.comboBoxEx3.Text);
            string[] columNames = new string[] { "GroupName" };
            DataTable dtGroup = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.DataSource = dtGroup;
            this.comboBoxEx2.ValueMember = "GroupName";
            this.comboBoxEx2.DisplayMember = "GroupName";
            this.comboBoxEx2.SelectedIndex = -1;
        }

        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "" || this.comboBoxEx3.Text == "" || this.comboBoxEx2.Text == "")
                return;
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx4.Text, this.comboBoxEx3.Text, this.comboBoxEx2.Text);
            string[] columNames = new string[] { "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx1.DataSource = dtPerson;
            this.comboBoxEx1.ValueMember = "UserType";
            this.comboBoxEx1.DisplayMember = "UserType";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        

        //private void buttonX5_Click(object sender, EventArgs e)
        //{
        //    string sqlFlush = string.Format("select * from Pub_JXCaculate where " +
        //        "CompanyNames like '%{0}%' and DepartmentName like '%{1}%' and GroupName like '%{2}%' " +
        //        "and UserType like '%{3}%' and JXType like '%{4}%' order by SubmitTime desc",
        //        this.comboBoxEx11.Text, this.comboBoxEx12.Text, this.comboBoxEx13.Text,
        //        this.comboBoxEx14.Text, this.comboBoxEx15.Text);
        //    this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlFlush);
        //}  
    }
}
