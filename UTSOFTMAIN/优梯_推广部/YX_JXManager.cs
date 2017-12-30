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
    public partial class YX_JXManager : Form
    {
        public YX_JXManager()
        {
            InitializeComponent();
        }
        public static bool submitBtnShow = false;
        public static DataTable tableShow = null;
        private static string windowText = "";
        private void YX_JXManager_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
            this.dataGridViewX2.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
           

            this.dateTimeInput1.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput2.Value = System.DateTime.Now;
            if(!submitBtnShow)
            {
                this.dataGridViewX1.DataSource = tableShow;
                this.labelX3.Visible = submitBtnShow;
                this.comboBoxEx1.Visible = submitBtnShow;
                this.textBoxX2.Visible = submitBtnShow;
                this.buttonX1.Visible = submitBtnShow;
            }
           windowText = (sender as YX_JXManager).Text;
            
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0) 
            {
                MessageBox.Show("请先选定报名记录！");
                return;
            }
            if (MessageBox.Show("你确定要提交：QQ【" + this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString() + "】的数据吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            bool resultNum1 = false;
            try 
            {
                resultNum1 = PublicMethod.insert_YDBMJX(
                    int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString()),
                    this.comboBoxEx1.SelectedValue.ToString(),
                    DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString()),
                    "报名",
                    this.comboBoxEx3.Text.Trim(),
                    float.Parse(this.comboBoxEx2.Text.Trim()),
                    this.textBoxX2.Text.Trim());
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            if (resultNum1)
            {
                MessageBox.Show("增加成功！");
                string sqlFlush = string.Format("select a.id,a.ForID,b.DepartmentName,b.AssumedName,b.StaffID,a.EnterTypeSmall,a.Value,a.Remark"+
                " from Pub_YDBM_JXManager a left join Users b on a.StaffID = b.StaffID" +
                " where a.ForID = {0} and b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and a.Remark != '管理层预定报名类绩效（系统插入）'",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
                this.dataGridViewX2.DataSource = DBHelper.ExecuteQuery(sqlFlush);

                int d1SelectIndex = 0;
                d1SelectIndex = this.dataGridViewX1.SelectedRows[0].Index;
                string idStr = "";
                for (int i = 0; i < this.dataGridViewX1.Rows.Count;i++ ) 
                {
                    idStr += this.dataGridViewX1.Rows[i].Cells[0].Value.ToString()+",";
                }
                if (idStr == "")
                    return;
                idStr = idStr.Substring(0,idStr.Length-1);
                string sqlUpFlush = string.Format("select id,Jointime,QQ,NickName,Name,YYNum,YYName,Telephone,MeanTeacher," +
                " stuff((select  ','+c.AssumedName+'-'+b.EnterTypeSmall from Pub_YDBM_JXManager b left join Users c on b.StaffID = c.StaffID " +
                " and b.StaffID = b.AmountManager where b.ForID = a.id and c.CompanyNames = '优梯' and c.DepartmentName = '营销部'" +
                " for xml path('')),1,1,'') as JXPerson from Pub_VIPMessage a" +
                " where id in ({0})" +
                " order by Jointime desc",
                idStr);
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sqlUpFlush);
            }
            else
            {
                MessageBox.Show("增加失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from (select id,Jointime,QQ,NickName,Name,YYNum,YYName,Telephone,MeanTeacher," +
                " stuff((select  ','+c.AssumedName+'-'+b.EnterTypeSmall from Pub_YDBM_JXManager b left join Users c on b.StaffID = c.StaffID " +
                " and b.StaffID = b.AmountManager where b.ForID = a.id and c.CompanyNames = '优梯' and c.DepartmentName = '营销部'" +
                " for xml path('')),1,1,'') as JXPerson from Pub_VIPMessage a" +
                " where CompanyNames = '优梯' and charindex('报名',EnterType)>0 and Jointime between '{0}' and '{1}' and QQ like '%{2}%' ) b ",
                this.dateTimeInput1.Value.ToShortDateString(), this.dateTimeInput2.Value.ToShortDateString(), this.textBoxX1.Text.Trim());
            if(this.comboBoxEx4.Text != "")
            {
                sql1 += string.Format(" where charindex('{0}',b.JXPerson)>0 ",this.comboBoxEx4.Text.Trim());
            }
            sql1 += " order by Jointime desc ";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("未查询到数据");
                return;
            }
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sql1);
        }

        private void dataGridViewX2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.comboBoxEx1.SelectedIndex = -1;
                this.comboBoxEx1.SelectedText = this.dataGridViewX2.SelectedRows[0].Cells[3].Value.ToString();
                this.comboBoxEx1.SelectedValue = this.dataGridViewX2.SelectedRows[0].Cells[4].Value.ToString();
                this.comboBoxEx3.Text = this.dataGridViewX2.SelectedRows[0].Cells[5].Value.ToString();
                this.comboBoxEx2.Text = this.dataGridViewX2.SelectedRows[0].Cells[6].Value.ToString();
                this.textBoxX2.Text = this.dataGridViewX2.SelectedRows[0].Cells[7].Value.ToString();
            }
            catch { }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX2.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选定数据！");
                return;
            }
            if (MessageBox.Show("你确定要删除选中行绩效吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            bool resultNum1 = false;
            try
            {
                resultNum1 = PublicMethod.delete_YDBMJX(
                    int.Parse(this.dataGridViewX2.SelectedRows[0].Cells[0].Value.ToString()),
                    int.Parse(this.dataGridViewX2.SelectedRows[0].Cells[1].Value.ToString()),
                    this.dataGridViewX2.SelectedRows[0].Cells[4].Value.ToString(),
                    DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString()),
                    "报名","直接报名",
                    float.Parse(this.dataGridViewX2.SelectedRows[0].Cells[4].Value.ToString()));
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            if (resultNum1)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX2.Rows.Remove(this.dataGridViewX2.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx1.SelectedIndex = -1;
            this.comboBoxEx2.Text = "";
            this.textBoxX2.Text = "";
            string sql1 = string.Format("select a.id,a.ForID,b.DepartmentName,b.AssumedName,b.StaffID,a.EnterTypeSmall,a.Value,a.Remark " +
                " from Pub_YDBM_JXManager a left join Users b on a.StaffID = b.StaffID" +
                " where a.ForID = {0} and b.CompanyNames = '优梯' and b.DepartmentName = '营销部' and a.Remark != '管理层预定报名类绩效（系统插入）'",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            for (int i = this.dataGridViewX2.SelectedRows.Count-1; i >= 0; i--)
                this.dataGridViewX2.Rows.RemoveAt(i);
            if (dt1.Rows.Count == 0)
                return;
            this.dataGridViewX2.DataSource = DBHelper.ExecuteQuery(sql1);

            if (
                    PublicMethod.fromUpdatePower(
                    "优梯",
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString())
                )
            {
                this.buttonX1.Visible = true;
                this.buttonX3.Visible = true;
            }
            else
            {
                this.buttonX1.Visible = false;
                this.buttonX3.Visible = false;
            }
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部'");
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            this.comboBoxEx1.DataSource = dtPerson;
            this.comboBoxEx1.ValueMember = "StaffID";
            this.comboBoxEx1.DisplayMember = "AssumedName";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text == "")
                return;

            string sqlperson = string.Format("StaffID = '{0}'", this.comboBoxEx1.SelectedValue);
            string[] columNamesPerson = new string[] { "CompanyNames", "DepartmentName", "GroupName", "UserType" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNamesPerson, sqlperson);

            if (dtPerson.Rows.Count == 0)
                return;

            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}' " +
                "and GroupName = '{2}' and UserType = '{3}' and JXType = '{4}'",
                dtPerson.Rows[0][0].ToString(),
                dtPerson.Rows[0][1].ToString(),
                dtPerson.Rows[0][2].ToString(),
                dtPerson.Rows[0][3].ToString(), "报名");
            string[] columNames = new string[] { "JXTypeSmall" };
            DataTable dtJX = tools.dtFilter(frmUTSOFTMAIN.jxCaculateType, columNames, sql);
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

        private void comboBoxEx4_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '优梯' and DepartmentName = '营销部'");
            string[] columNames = new string[] { "AssumedName" };
            DataTable dtPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);
            if (dtPerson.Rows.Count == 0)
                return;
            this.comboBoxEx4.DataSource = dtPerson;
            this.comboBoxEx4.ValueMember = "AssumedName";
            this.comboBoxEx4.DisplayMember = "AssumedName";
            this.comboBoxEx4.SelectedIndex = -1;
        }

    }
}
