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
    public partial class UT_TK_DealWith : Form
    {
        public UT_TK_DealWith()
        {
            InitializeComponent();
        }
        public DataTable dtSource = new DataTable();
        public string windowText = "";
        public string companyNameStr = "";
        public string companyType = "";
        private void TK_DealWith_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.DataSource = dtSource;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);

            this.dataGridViewX1_CellClick(null,null);
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString();
            if (this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString() == "")
                this.buttonX3.Enabled = false;
            else
                this.buttonX3.Enabled = true;
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[14].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[15].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[16].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX1.SelectedRows[0].Cells[17].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要增加退款信息吗?", "增加提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            if (this.comboBoxEx1.Text.Trim() == "退款")
            {
                if (this.textBoxX3.Text == "")
                    this.textBoxX3.Text = "0";
                try
                {
                    sql1 = string.Format("update Pub_ComplainRefundInfo set " +
                        " Result = '{1}',EnterTime2 = getdate(),Account = '{2}',AccountName= '{3}',RefundValue= {4},Reason= '{5}'" +
                        " where id = {0}",
                        this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                        this.comboBoxEx1.Text.Trim(), this.textBoxX1.Text.Trim(), this.textBoxX2.Text.Trim(),
                        this.textBoxX3.Text.Trim(), this.textBoxX4.Text.Trim());
                }
                catch { MessageBox.Show("输入数据类型有误！"); return; }
            }
            else if (this.comboBoxEx1.Text.Trim() == "正常")
            {
                try
                {
                    sql1 = string.Format("update Pub_ComplainRefundInfo set " +
                        " Result = '{1}' where id = {0}",
                        this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(), this.comboBoxEx1.Text.Trim());
                }
                catch { MessageBox.Show("输入数据类型有误！"); return; }
            }
            else
            {
                MessageBox.Show("请填写正确的处理结果！");
                return;
            }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("处理成功！");
                this.buttonX3.Enabled = true;
                if (this.comboBoxEx1.Text.Trim() == "退款")
                {
                    PublicMethod.insert_TSTK_QQManagerJX(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(), 
                        this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString(),
                        this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString());
                }
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ" + this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "处理成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[13].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[14].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[15].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[16].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[17].Value = this.textBoxX4.Text;
            }
            else
            {
                MessageBox.Show("处理失败，请检查输入数据！");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要修改信息吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                sql1 = string.Format("update Pub_ComplainRefundInfo set " +
                    " EnterTime2 = getdate(),Account = '{1}',AccountName= '{2}',RefundValue= {3},Reason= '{4}'" +
                    " where id = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                    this.textBoxX1.Text.Trim(), this.textBoxX2.Text.Trim(), this.textBoxX3.Text.Trim(),
                    this.textBoxX4.Text.Trim());
            }
            catch { MessageBox.Show("输入数据类型有误！"); return; }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("修改成功！");
                //--------------日志开始------------------
                string operationRemarkStr = string.Format("");
                frmUTSOFTMAIN.OperationObject = "QQ" + this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
                frmUTSOFTMAIN.OperationRemark = "修改信息成功";
                frmUTSOFTMAIN.addlog(this.buttonX1);
                //--------------日志结束------------------
                this.dataGridViewX1.SelectedRows[0].Cells[14].Value = this.textBoxX1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[15].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[16].Value = this.textBoxX3.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[17].Value = this.textBoxX4.Text;
            }
            else
            {
                MessageBox.Show("处理失败，请检查输入数据！");
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.comboBoxEx1.Text == "正常")
            {
                this.textBoxX1.Text = "";
                this.textBoxX2.Text = "";
                this.textBoxX3.Text = "0";
                this.textBoxX4.Text = "";
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            UT_KF_TSTKJXManager utjxm = new UT_KF_TSTKJXManager();
            utjxm.forID = int.Parse(this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            utjxm.companyNames = this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString();
            utjxm.jxTime = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString());
            utjxm.companyType = companyType;


            if (this.dataGridViewX1.SelectedRows[0].Cells[13].Value.ToString() == "正常") 
                utjxm.enterType = "投诉";
            else
                utjxm.enterType = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();

            string sqlJXPerson = string.Format("select c.AssumedName from  Pub_VIPMessage a" +
                " left join Pub_YDBM_JXManager b on a.id = b.ForID" +
                " left join Users c on b.StaffID = C.StaffID" +
                " where a.qq = '{0}' and charindex('报名',EnterType)>0" +
                " and a.CompanyNames = '{1}'" +
                " and b.StaffID = b.AmountManager" +
                " group by c.AssumedName",
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString(),
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString());
            DataTable dtJXPerson = DBHelper.ExecuteQuery(sqlJXPerson);

            if (dtJXPerson.Rows.Count > 0)
            {
                for (int i = 0; i < dtJXPerson.Rows.Count; i++)
                {
                    utjxm.jxperson += dtJXPerson.Rows[i][0].ToString() + " , ";
                }
            }

            if (
                    PublicMethod.fromUpdatePower(
                    this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString(),
                    windowText,
                    this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString())
                )
            {
                utjxm.updateState = true;
            }

            string sqlDTSource = string.Format("select a.id,c.QQ,c.NickName,b.DepartmentName,b.AssumedName,a.StaffID,a.EnterTypeSmall,a.Value,a.Remark" +
                    " from Pub_TSTK_JXManager a left join Users b on a.StaffID = b.StaffID left join Pub_ComplainRefundInfo c on a.ForID = c.id" +
                    " where a.StaffID = a.AmountManager and a.EnterTypeSmall != '绩效退款' and a.ForID = {0}",
                    this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            utjxm.dtSource = DBHelper.ExecuteQuery(sqlDTSource);
            utjxm.Show();
        }
    }
}
