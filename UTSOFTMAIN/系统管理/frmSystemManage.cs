using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MODEL;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace UTSOFTMAIN
{
    public partial class frmSystemManage : Form
    {
        public frmSystemManage()
        {
            InitializeComponent();
        }
        public static DataTable dtAll1;
        private void frmSystemManage_Load(object sender, EventArgs e)
        {
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now.AddDays(1);
        }

        private void buttonX2_Click(object sender, EventArgs e)//权限管理保存
        {
            if (this.comboBoxEx1.Text == "")
            {
                MessageBox.Show("请先选定成员!");
                return;
            }
            if (MessageBox.Show("你确定要修改【" + this.comboBoxEx2.Text + "】的权限吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqlDel = string.Format("delete from SYS_UserPower where StaffID = '{0}' and CompanyNames = '{1}'", 
                this.comboBoxEx2.SelectedValue.ToString(),this.comboBoxEx1.Text);
            DBHelper.ExecuteUpdate(sqlDel);
            string sql1 = "insert into SYS_UserPower (CompanyNames,StaffID, MenuLeft, MenuRight, Visible)";
            foreach (Control tmpControl in this.myPanel2.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel gbx = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    string upMenu = gbx.Text;//上级菜单名称
                    if (upMenu == "主界面")
                    {
                        foreach (Control cheControl in gbx.Controls)
                        {
                            if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                            {
                                DevComponents.DotNetBar.Controls.CheckBoxX cheBox = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                                string menuName = cheBox.Text;//下级菜单名称
                                if (cheBox.Checked)
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','无',1 union all ",
                                       this.comboBoxEx1.Text,this.comboBoxEx2.SelectedValue.ToString(), menuName);
                                }
                                else
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','无',0 union all ",
                                       this.comboBoxEx1.Text, this.comboBoxEx2.SelectedValue.ToString(), menuName);
                                }
                            }
                        }
                    }
                    else 
                    {
                        foreach (Control cheControl in gbx.Controls)
                        {
                            if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                            {
                                DevComponents.DotNetBar.Controls.CheckBoxX cheBox = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                                string menuName = cheBox.Text;//下级菜单名称
                                if (cheBox.Checked)
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','{3}',1 union all ",
                                        this.comboBoxEx1.Text,this.comboBoxEx2.SelectedValue.ToString(), menuName, upMenu);
                                }
                                else
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','{3}',0 union all ",
                                        this.comboBoxEx1.Text, this.comboBoxEx2.SelectedValue.ToString(), menuName, upMenu);
                                }
                            }
                        }
                    }
                }
            }
            sql1 = sql1.Substring(0, sql1.Length - 10);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("修改权限成功");
                //--------------日志开始------------------
                frmUTSOFTMAIN.OperationObject = this.comboBoxEx2.Text;
                frmUTSOFTMAIN.OperationRemark = "修改权限成功";
                frmUTSOFTMAIN.addlog(this.buttonX2);
                //--------------日志结束------------------
            }
            else
            {
                MessageBox.Show("修改权限失败");
            }
        }
       
        private void buttonX9_Click(object sender, EventArgs e)//日志查询
        {
            string sqlstr = string.Format("select a.OperationTime, a.IPAddr, b.AssumedName, a.MenuLeft, a.MenuRight, a.MenuIn, a.OperationObject, a.OperationRemark" +
                " from SYS_ErpLog a left join Users b on a.StaffID = b.StaffID where b.onjob = 1" +
                " and a.OperationTime between '{0}' and '{1}'and a.IPAddr like '%{2}%'"+
                " and b.AssumedName like '%{3}%' and a.MenuLeft like '%{4}%' and a.MenuRight like '%{5}%'" + 
                " and a.MenuIn like '%{6}%' and a.OperationObject like '%{7}%' and a.OperationRemark like '%{8}%'"+
                " order by a.OperationTime desc",
                DateTime.Parse(this.dateTimeInput2.Value.ToShortDateString()),
                DateTime.Parse(this.dateTimeInput3.Value.ToShortDateString()),
                this.textBoxX2.Text.Trim(),
                this.textBoxX3.Text.Trim(),
                this.textBoxX4.Text.Trim(),
                this.textBoxX5.Text.Trim(),
                this.textBoxX6.Text.Trim(),
                this.textBoxX7.Text.Trim(),
                this.textBoxX8.Text.Trim());
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlstr);
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewX1.Rows.Count == 0)
                return;
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX6.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX1.SelectedRows[0].Cells[6].Value.ToString();
            this.textBoxX8.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();
        }

        private void textBoxX1_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text == "") 
            {
                this.textBoxX2.Text = "";
                this.textBoxX3.Text = "";
                this.textBoxX4.Text = "";
                this.textBoxX5.Text = "";
                this.textBoxX6.Text = "";
                this.textBoxX7.Text = "";
                this.textBoxX8.Text = "";
            }
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            this.comboBoxEx1.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx1.ValueMember = "CompanyNames";
            this.comboBoxEx1.DisplayMember = "CompanyNames";
            this.comboBoxEx1.SelectedIndex = -1;
        }
        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "")
                return;
            string[] columNames = new string[] { "DepartmentName" };
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx1.Text);
            this.comboBoxEx3.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx3.ValueMember = "DepartmentName";
            this.comboBoxEx3.DisplayMember = "DepartmentName";
            this.comboBoxEx3.SelectedIndex = -1;
        }
        private void comboBoxEx3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "" || this.comboBoxEx3.Text == "")
                return;
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx1.Text, this.comboBoxEx3.Text);
            this.comboBoxEx2.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx2.ValueMember = "StaffID";
            this.comboBoxEx2.DisplayMember = "AssumedName";
            this.comboBoxEx2.SelectedIndex = -1;
        }
        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx2.Text == "")
                return;
            foreach (Control tmpControl in this.myPanel2.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel gpl = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    foreach (Control tmpControl1 in gpl.Controls)
                    {
                        if (tmpControl1 is DevComponents.DotNetBar.Controls.CheckBoxX)
                        {
                            DevComponents.DotNetBar.Controls.CheckBoxX cbx = (DevComponents.DotNetBar.Controls.CheckBoxX)tmpControl1;
                            cbx.Checked = false;
                        }
                    }
                }
            }
            string sql1 = string.Format("select * from SYS_UserPower where StaffID = '{0}' and CompanyNames = '{1}'",
                this.comboBoxEx2.SelectedValue.ToString(), this.comboBoxEx1.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                foreach (Control tmpControl in this.myPanel2.Controls)
                {
                    if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                    {
                        DevComponents.DotNetBar.Controls.GroupPanel gpl = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                        string upMenuName = gpl.Text;
                        if (upMenuName == "主界面")
                        {
                            foreach (Control tmpControl1 in gpl.Controls)
                            {
                                if (tmpControl1 is DevComponents.DotNetBar.Controls.CheckBoxX)
                                {
                                    DevComponents.DotNetBar.Controls.CheckBoxX cbx = (DevComponents.DotNetBar.Controls.CheckBoxX)tmpControl1;
                                    cbx.Checked = false;
                                    string menuName = cbx.Text;
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        if (dt1.Rows[i][3].ToString() == menuName && dt1.Rows[i][4].ToString() == "无")
                                        {
                                            cbx.Checked = Convert.ToBoolean(dt1.Rows[i][5].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Control tmpControl1 in gpl.Controls)
                            {
                                if (tmpControl1 is DevComponents.DotNetBar.Controls.CheckBoxX)
                                {
                                    DevComponents.DotNetBar.Controls.CheckBoxX cbx = (DevComponents.DotNetBar.Controls.CheckBoxX)tmpControl1;
                                    cbx.Checked = false;
                                    string menuName = cbx.Text;
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        if (dt1.Rows[i][3].ToString() == menuName && dt1.Rows[i][4].ToString() == upMenuName)
                                        {
                                            cbx.Checked = Convert.ToBoolean(dt1.Rows[i][5].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            this.comboBoxEx4.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx4.ValueMember = "CompanyNames";
            this.comboBoxEx4.DisplayMember = "CompanyNames";
            this.comboBoxEx4.SelectedIndex = -1;
        }

        private void comboBoxEx4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "")
                return;
            string[] columNames = new string[] { "DepartmentName" };
            string sql = string.Format("CompanyNames = '{0}'", 
                this.comboBoxEx4.Text);
            this.comboBoxEx5.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx5.ValueMember = "DepartmentName";
            this.comboBoxEx5.DisplayMember = "DepartmentName";
            this.comboBoxEx5.SelectedIndex = -1;
        }

        private void comboBoxEx5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "" || this.comboBoxEx5.Text == "")
                return;
            string[] columNames = new string[] { "GroupName" };
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", 
                this.comboBoxEx4.Text, this.comboBoxEx5.Text);
            this.comboBoxEx6.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx6.ValueMember = "GroupName";
            this.comboBoxEx6.DisplayMember = "GroupName";
            this.comboBoxEx6.SelectedIndex = -1;
        }

        private void comboBoxEx6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx4.Text == "" || this.comboBoxEx5.Text == "" || this.comboBoxEx6.Text == "")
                return;
            string[] columNames = new string[] { "UserType" };
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}'",
                this.comboBoxEx4.Text, this.comboBoxEx5.Text, this.comboBoxEx6.Text);
            this.comboBoxEx7.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx7.ValueMember = "UserType";
            this.comboBoxEx7.DisplayMember = "UserType";
            this.comboBoxEx7.SelectedIndex = -1;
        }

        private void comboBoxEx7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx7.Text == "")
                return;
            foreach (Control tmpControl in this.myPanel2.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel gpl = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    foreach (Control tmpControl1 in gpl.Controls)
                    {
                        if (tmpControl1 is DevComponents.DotNetBar.Controls.CheckBoxX)
                        {
                            DevComponents.DotNetBar.Controls.CheckBoxX cbx = (DevComponents.DotNetBar.Controls.CheckBoxX)tmpControl1;
                            cbx.Checked = false;
                        }
                    }
                }
            }
            string sql1 = string.Format("select * from SYS_UserPowerType "+
                "where CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}' and UserType = '{3}'",
                this.comboBoxEx4.Text.Trim(),this.comboBoxEx5.Text.Trim(),this.comboBoxEx6.Text.Trim(),this.comboBoxEx7.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                foreach (Control tmpControl in this.myPanel2.Controls)
                {
                    if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                    {
                        DevComponents.DotNetBar.Controls.GroupPanel gpl = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                        string upMenuName = gpl.Text;
                        if (upMenuName == "主界面")
                        {
                            foreach (Control tmpControl1 in gpl.Controls)
                            {
                                if (tmpControl1 is DevComponents.DotNetBar.Controls.CheckBoxX)
                                {
                                    DevComponents.DotNetBar.Controls.CheckBoxX cbx = (DevComponents.DotNetBar.Controls.CheckBoxX)tmpControl1;
                                    cbx.Checked = false;
                                    string menuName = cbx.Text;
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        if (dt1.Rows[i][5].ToString() == menuName && dt1.Rows[i][6].ToString() == "无")
                                        {
                                            cbx.Checked = Convert.ToBoolean(dt1.Rows[i][7].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Control tmpControl1 in gpl.Controls)
                            {
                                if (tmpControl1 is DevComponents.DotNetBar.Controls.CheckBoxX)
                                {
                                    DevComponents.DotNetBar.Controls.CheckBoxX cbx = (DevComponents.DotNetBar.Controls.CheckBoxX)tmpControl1;
                                    cbx.Checked = false;
                                    string menuName = cbx.Text;
                                    for (int i = 0; i < dt1.Rows.Count; i++)
                                    {
                                        if (dt1.Rows[i][5].ToString() == menuName && dt1.Rows[i][6].ToString() == upMenuName)
                                        {
                                            cbx.Checked = Convert.ToBoolean(dt1.Rows[i][7].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.comboBoxEx7.Text == "")
            {
                MessageBox.Show("请先选定职位!");
                return;
            }
            if (MessageBox.Show("你确定要修改【此职位】的权限吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqlDel = string.Format("delete from SYS_UserPowerType "+
                "where CompanyNames = '{0}' and DepartmentName = '{1}' and GroupName = '{2}' and UserType = '{3}'",
                this.comboBoxEx4.Text.Trim(), this.comboBoxEx5.Text.Trim(), this.comboBoxEx6.Text.Trim(), this.comboBoxEx7.Text.Trim());

            DBHelper.ExecuteUpdate(sqlDel);
            string sql1 = "insert into SYS_UserPowerType (CompanyNames,DepartmentName,GroupName,UserType, MenuLeft, MenuRight, Visible)";
            foreach (Control tmpControl in this.myPanel2.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel gbx = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    string upMenu = gbx.Text;//上级菜单名称
                    if (upMenu == "主界面")
                    {
                        foreach (Control cheControl in gbx.Controls)
                        {
                            if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                            {
                                DevComponents.DotNetBar.Controls.CheckBoxX cheBox = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                                string menuName = cheBox.Text;//下级菜单名称
                                if (cheBox.Checked)
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','{3}','{4}','无',1 union all ",
                                       this.comboBoxEx4.Text.Trim(), this.comboBoxEx5.Text.Trim(), this.comboBoxEx6.Text.Trim(), 
                                       this.comboBoxEx7.Text.Trim(), menuName);
                                }
                                else
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','{3}','{4}','无',0 union all ",
                                       this.comboBoxEx4.Text.Trim(), this.comboBoxEx5.Text.Trim(), this.comboBoxEx6.Text.Trim(),
                                       this.comboBoxEx7.Text.Trim(), menuName);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Control cheControl in gbx.Controls)
                        {
                            if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                            {
                                DevComponents.DotNetBar.Controls.CheckBoxX cheBox = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                                string menuName = cheBox.Text;//下级菜单名称
                                if (cheBox.Checked)
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','{3}','{4}','{5}',1 union all ",
                                        this.comboBoxEx4.Text.Trim(), this.comboBoxEx5.Text.Trim(), this.comboBoxEx6.Text.Trim(),
                                       this.comboBoxEx7.Text.Trim(), menuName, upMenu);
                                }
                                else
                                {
                                    sql1 += string.Format(" select '{0}','{1}','{2}','{3}','{4}','{5}',0 union all ",
                                        this.comboBoxEx4.Text.Trim(), this.comboBoxEx5.Text.Trim(), this.comboBoxEx6.Text.Trim(),
                                       this.comboBoxEx7.Text.Trim(), menuName, upMenu);
                                }
                            }
                        }
                    }
                }
            }
            sql1 = sql1.Substring(0, sql1.Length - 10);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("修改职位权限成功");
                //--------------日志开始------------------
                frmUTSOFTMAIN.OperationObject = this.comboBoxEx4.Text.Trim() + this.comboBoxEx5.Text.Trim() + 
                    this.comboBoxEx6.Text.Trim() + this.comboBoxEx7.Text.Trim();
                frmUTSOFTMAIN.OperationRemark = "修改职位权限成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
            }
            else
            {
                MessageBox.Show("修改职位权限失败");
            }
        }

        private void comboBoxEx10_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "CompanyNames" };
            this.comboBoxEx10.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, "");
            this.comboBoxEx10.ValueMember = "CompanyNames";
            this.comboBoxEx10.DisplayMember = "CompanyNames";
            this.comboBoxEx10.SelectedIndex = -1;
        }
        private void comboBoxEx10_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx10.Text == "")
                return;
            string[] columNames = new string[] { "DepartmentName" };
            string sql = string.Format("CompanyNames = '{0}'", this.comboBoxEx10.Text);
            this.comboBoxEx8.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx8.ValueMember = "DepartmentName";
            this.comboBoxEx8.DisplayMember = "DepartmentName";
            this.comboBoxEx8.SelectedIndex = -1;
        }
        private void comboBoxEx8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx10.Text == "" || this.comboBoxEx8.Text == "")
                return;
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", this.comboBoxEx10.Text, this.comboBoxEx8.Text);
            this.comboBoxEx9.DataSource = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx9.ValueMember = "StaffID";
            this.comboBoxEx9.DisplayMember = "AssumedName";
            this.comboBoxEx9.SelectedIndex = -1;
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要重置密码吗?", "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            if(this.comboBoxEx9.Text == "")
            {
                MessageBox.Show("请先选定成员");
                return;
            }
            string sql1 = string.Format("update Users set PSW = 'ut8888..' where StaffID = '{0}'",this.comboBoxEx9.SelectedValue);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("重置成功，初始密码为: ut8888..");
            }
            else 
            {
                MessageBox.Show("重置失败");
            }
        }

        

    }
    
}

