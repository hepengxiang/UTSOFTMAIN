using System;
using System.Data;
using System.Windows.Forms;
using UTSOFTMAIN.优梯_客服部;

namespace UTSOFTMAIN
{
    public partial class UT_frmCostomPart : Form
    {
        public UT_frmCostomPart()
        {
            InitializeComponent();
        }

        private void UT_frmCostomPart_Load(object sender, EventArgs e)
        {
            DataTable dt1 = DBHelper.ExecuteQuery("select AssumedName,StaffID from Users where onjob = 1 "+
                " and CompanyNames = '优梯' and charindex('客服',DepartmentName)>0");
            this.comboBoxEx1.DataSource = dt1;
            this.comboBoxEx1.ValueMember = "StaffID";
            this.comboBoxEx1.DisplayMember = "AssumedName";
            this.comboBoxEx1.SelectedIndex = -1;
            this.comboBoxEx1.SelectedIndexChanged += new EventHandler(comboBoxEx1_SelectedIndexChanged);
            frmUTSOFTMAIN.tablePowerBtnInit(this.flowLayoutPanel1);
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text.Trim() == "") 
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (MessageBox.Show("你确定要开通【" + this.textBoxX1.Text + "】的权限吗?", "开通权限提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            ///////////////////////////////////////////////////////////////////////// 
            //官网中增加此QQ。或者将此用户身份转为VIP会员
            int webexistsflag = 0;
            string salt = "";
            string sqlstr3 = string.Format("SELECT username,salt from pre_ucenter_members where username='{0}'", this.textBoxX1.Text.Trim());
            //string sqlstr3 = string.Format("SELECT MAX(uid) from pre_ucenter_members", this.textBoxX1.Text.Trim());
            DataTable dt3 = MySql.ExecuteQuery(sqlstr3);
            if (dt3 != null && dt3.Rows.Count > 0)
            {
                
                webexistsflag = 1;
                salt = dt3.Rows[0][1].ToString();
                if (MessageBox.Show("该用户名称在UT网站中已存在，请确认该用户是否已在网站注册，本次操作将改变网站中该用户的角色和密码，是否继续？", "改变网站数据提醒",
                    MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            if (webexistsflag == 1)//网站中已有该成员,将身份变成VIP
            {

                string sqlstr5 = string.Format("update pre_common_member set groupid='21' ,adminid='-1' where username='{0}' ", this.textBoxX1.Text.Trim());
                string password = tools.MD5(tools.MD5("88888888") + salt);
                string sqlstr6 = string.Format("update pre_ucenter_members set password='{0}' where username='{1}'  ", password, this.textBoxX1.Text.Trim());

                if (MySql.ExecuteUpdate(sqlstr5) > 0 && MySql.ExecuteUpdate(sqlstr6) > 0)
                {
                    MessageBox.Show("添加成功");
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
            }
            else//网站中新增该成员
            {
                salt = tools.RandomStr(6);
                string password = password = tools.MD5(tools.MD5("88888888") + salt);

                string sqlstr5 = string.Format("insert into pre_common_member " +
                        " SELECT (SELECT MAX(uid) from pre_common_member)+1 as uid,'','{0}','',	'0', '0','1','0','-1', " +
                        " '21','0','',	'1453894923','144','0','9999','0','0','0','0','0','0','0'  "
                        , this.textBoxX1.Text.Trim());

                string sqlstr6 = string.Format("insert into pre_ucenter_members " +
                        "SELECT (SELECT MAX(uid) from pre_ucenter_members)+1 as uid,'{0}','{1}','','','','','0','0','0','{2}','' "
                        , this.textBoxX1.Text.Trim(), password, salt);
                int result6 = MySql.ExecuteUpdate(sqlstr6);
                if (result6>0)
                {
                    int result5 = MySql.ExecuteUpdate(sqlstr5);
                    if (result5>0)
                    {
                        MessageBox.Show("添加成功");
                    }
                    else
                    {
                        MessageBox.Show(frmUTSOFTMAIN.ErrMsg + "添加失败");
                    }
                }
                else
                {
                    MessageBox.Show(frmUTSOFTMAIN.ErrMsg + "添加失败");
                }
            }
            /////////////////////////////////////////////////////////////////////////
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text.Trim() == "")
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            if (MessageBox.Show("你确定要关闭[" + this.textBoxX1.Text.Trim() + "]的论坛权限吗吗?", "关闭论坛权限提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sqlstr1 = string.Format("DELETE from pre_ucenter_members WHERE  username = '{0}' ", this.textBoxX1.Text.Trim());
            int resultNum1 = MySql.ExecuteUpdate(sqlstr1);
            string sqlstr2 = string.Format("DELETE from pre_common_member WHERE username = '{0}' ", this.textBoxX1.Text.Trim());
            int resultNum2 = MySql.ExecuteUpdate(sqlstr2);
            if (resultNum1 > 0 && resultNum2 > 0)
            {
                MessageBox.Show("成功关闭[" + this.textBoxX1.Text.Trim() + "]的论坛权限");
            }
            else
            {
                MessageBox.Show("关闭失败");
                return;
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select b.AssumedName,count(*) as allpro,"+
                " sum(case when PromotesStrage = '高' and TrackState != '放弃跟踪' then 1 else 0 end) as highpro,"+
                " sum(case when PromotesStrage = '中' and TrackState != '放弃跟踪' then 1 else 0 end) as midlpro,"+
                " sum(case when PromotesStrage = '低' and TrackState != '放弃跟踪' then 1 else 0 end) as deeppro,"+
                " sum(case when TrackState = '放弃跟踪' then 1 else 0 end) as givepro,b.StaffID" +
                " from UT_KFPromotes a left join Users b on a.StaffID = b.StaffID"+
                " where b.onjob = 1 group by b.AssumedName,b.StaffID");
            this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sql1);
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (this.textBoxX4.Text.Trim() == "")
            {
                MessageBox.Show("QQ号码栏不能为空");
                return;
            }
            string sql1 = string.Format("select * from UT_KFPromotes  a left join Users b on a.StaffID = b.StaffID " +
                " where onjob = 1 and a.QQ like '%{0}%'", this.textBoxX4.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                //此意向已经在意向库，且有人跟踪，跟踪人不是自己
                if (dt1.Rows[0][14].ToString() != "" && dt1.Rows[0][14].ToString() != frmUTSOFTMAIN.AssumedName)
                {
                    MessageBox.Show("此意向[" + dt1.Rows[0][14].ToString() + "]正在跟踪");
                    //if (MessageBox.Show("此意向[" + dt1.Rows[0][14].ToString() + "]正在跟踪", "跟踪提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                        //return;
                }
                //此意向已经在意向库，跟踪人是自己,则更新数据
                else if (dt1.Rows[0][14].ToString() != "" && dt1.Rows[0][14].ToString() == frmUTSOFTMAIN.AssumedName)
                {
                    string sql2 = string.Format("update UT_KFPromotes set PromoteType='{5}', QunNum = '{0}',PromoteTeacher = '{1}', Remark = '{2}',PromotesStrage = '{4}' where QQ = '{3}'",
                        this.textBoxX5.Text,
                        this.textBoxX2.Text,
                        this.textBoxX7.Text,
                        this.textBoxX4.Text.Trim(),
                        this.comboBoxEx10.Text.Trim(),
                        this.comboBoxEx4.Text.Trim());
                    int resultNum = DBHelper.ExecuteUpdate(sql2);
                    if (resultNum > 0)
                    {
                        MessageBox.Show("更新成功");
                        string sqlflush = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark," +
                       " a.PromotesStrage from UT_KFPromotes  a left join Users b on a.StaffID = b.StaffID " +
                       " where onjob = 1 and a.QQ like '%{0}%'", this.textBoxX4.Text.Trim());
                        this.dataGridViewX3.DataSource = DBHelper.ExecuteQuery(sqlflush);
                    }
                }
            }
            else
            {
                string sql2 = string.Format("insert into UT_KFPromotes values(getdate(),'{0}','{1}','{2}','{3}','{4}','{5}','正在跟踪','{6}','')",
                    this.comboBoxEx4.Text,
                    this.textBoxX4.Text.Trim(),
                    this.textBoxX5.Text,
                    this.textBoxX2.Text,
                    this.textBoxX7.Text,
                    frmUTSOFTMAIN.StaffID,
                    this.comboBoxEx10.Text.Trim()
                    );
                int resultNum = DBHelper.ExecuteUpdate(sql2);
                if (resultNum > 0)
                {
                    MessageBox.Show("提交成功");
                    string sqlflush = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark,"+
                        " a.PromotesStrage from UT_KFPromotes  a left join Users b on a.StaffID = b.StaffID " +
                        " where onjob = 1 and a.QQ like '%{0}%'", this.textBoxX4.Text.Trim());
                    this.dataGridViewX3.DataSource= DBHelper.ExecuteQuery(sqlflush);
                }
                else
                {
                    MessageBox.Show("提交失败");
                }
            }
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            string sqlflush = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark," +
                           " a.PromotesStrage from UT_KFPromotes  a left join Users b on a.StaffID = b.StaffID " +
                           " where onjob = 1 and a.StaffID = '{0}'" +
                           " and a.PromoteType like '%{1}%'" +
                           " and a.QQ like '%{2}%'" +
                           " and a.QunNum like '%{3}%'" +
                           " and a.PromoteTeacher like '%{4}%'" +
                           " ", 
                           frmUTSOFTMAIN.StaffID,
                           this.comboBoxEx4.Text.Trim(), this.textBoxX4.Text.Trim(), this.textBoxX5.Text.Trim(), 
                           this.textBoxX2.Text.Trim(),this.comboBoxEx10.Text.Trim());
            this.dataGridViewX3.DataSource = DBHelper.ExecuteQuery(sqlflush);

        }

        private void dataGridViewX3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX3.SelectedRows.Count == 0)
                return;
            this.comboBoxEx4.Text = this.dataGridViewX3.SelectedRows[0].Cells[1].Value.ToString();
            this.textBoxX4.Text = this.dataGridViewX3.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX5.Text = this.dataGridViewX3.SelectedRows[0].Cells[3].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX3.SelectedRows[0].Cells[4].Value.ToString();
            this.comboBoxEx10.Text = this.dataGridViewX3.SelectedRows[0].Cells[5].Value.ToString();
            this.textBoxX7.Text = this.dataGridViewX3.SelectedRows[0].Cells[6].Value.ToString();
        }
        //预定表
        private void buttonX4_Click(object sender, EventArgs e)
        {
            UT_KF_VIPMessage utkfvip = new UT_KF_VIPMessage();
            utkfvip.Text = this.buttonX4.Text;
            utkfvip.Show();
        }
        //投诉退款表
        private void buttonX9_Click(object sender, EventArgs e)
        {
            UT_KF_ComplainRefundInfo utkfcr = new UT_KF_ComplainRefundInfo();
            utkfcr.Text = this.buttonX9.Text;
            utkfcr.Show();
        }
        //4Y4表
        private void buttonX10_Click(object sender, EventArgs e)
        {
            UT_KF_FYFTable utkfyf = new UT_KF_FYFTable();
            utkfyf.Text = this.buttonX10.Text;
            utkfyf.Show();
        }
        //课堂情况表
        private void buttonX11_Click(object sender, EventArgs e)
        {
            UT_KF_ClassSituation utkfcs = new UT_KF_ClassSituation();
            utkfcs.Text = this.buttonX11.Text;
            utkfcs.Show();
        }
        //表格权限提交
        private void buttonX12_Click(object sender, EventArgs e)
        {
            frmUTSOFTMAIN.tablePowerSubmit(this.panel1, "优梯", this.comboBoxEx1.SelectedValue.ToString());
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            frmUTSOFTMAIN.tablePowerShow(this.panel1, this.comboBoxEx1.SelectedValue.ToString());
        }

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                string sql1 = "";
                if (e.ColumnIndex == 1)
                {
                    sql1 = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark,b.AssumedName,"+
                        " a.TrackState,a.PromotesStrage,a.GiveUpTime from UT_KFPromotes a left join Users b on a.StaffID = b.StaffID " +
                        " where a.StaffID = '{0}'",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[6].Value.ToString());
                }
                else if (e.ColumnIndex == 2)
                {
                    sql1 = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark,b.AssumedName,"+
                        " a.TrackState,a.PromotesStrage,a.GiveUpTime from UT_KFPromotes a left join Users b on a.StaffID = b.StaffID " +
                        " where a.StaffID = '{0}' and a.PromotesStrage = '高' and a.TrackState = '正在跟踪'",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[6].Value.ToString());
                }
                else if (e.ColumnIndex == 3)
                {
                    sql1 = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark,b.AssumedName,"+
                        " a.TrackState,a.PromotesStrage,a.GiveUpTime from UT_KFPromotes a left join Users b on a.StaffID = b.StaffID " +
                        " where a.StaffID = '{0}' and a.PromotesStrage = '中' and a.TrackState = '正在跟踪'",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[6].Value.ToString());
                }
                else if (e.ColumnIndex == 4)
                {
                    sql1 = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark,b.AssumedName,"+
                        " a.TrackState,a.PromotesStrage,a.GiveUpTime from UT_KFPromotes a left join Users b on a.StaffID = b.StaffID " +
                        " where a.StaffID = '{0}' and a.PromotesStrage = '低' and a.TrackState = '正在跟踪'",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[6].Value.ToString());
                }
                else
                {
                    sql1 = string.Format("select a.UploadTime,a.PromoteType,a.QQ,a.QunNum,a.PromoteTeacher,a.Remark,b.AssumedName,"+
                        " a.TrackState,a.PromotesStrage,a.GiveUpTime from UT_KFPromotes a left join Users b on a.StaffID = b.StaffID " +
                        " where a.StaffID = '{0}' and a.TrackState = '放弃跟踪'",
                        this.dataGridViewX1.Rows[e.RowIndex].Cells[6].Value.ToString());
                }
                DataTable dtSendAll = DBHelper.ExecuteQuery(sql1);
                UT_KF_PromoteDetail.dtSource = dtSendAll;
                UT_KF_PromoteDetail dsa = new UT_KF_PromoteDetail();
                dsa.Show();
            }
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            UT_KF_TXZY ukt = new UT_KF_TXZY();
            ukt.Text = this.buttonX7.Text;
            ukt.Show();
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            UT_FlushList ufl = new UT_FlushList();
            ufl.Text = this.buttonX8.Text;
            ufl.Show();
        }
    }
}
