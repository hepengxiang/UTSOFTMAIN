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
    public partial class UT_YF_TwoVisit : Form
    {
        public UT_YF_TwoVisit()
        {
            InitializeComponent();
        }
        public DataTable  daSource = new DataTable();
        public string qqStr = "";
        public string qqNickName = "";
        public bool btnVisable = false;
        private void UT_YF_TwoVisit_Load(object sender, EventArgs e)
        {
            this.buttonX1.Visible = btnVisable;
            if (daSource.Rows.Count == 0)
            {
                string sql1 = string.Format("select * from UT_VIPMemberShopDiagnosis where QQ = '{0}'", qqStr);
                daSource = DBHelper.ExecuteQuery(sql1);
            }
            if (daSource.Rows.Count > 0)
            {
                this.textBoxX13.Text = daSource.Rows[0][1].ToString();  //昵称
                this.textBoxX14.Text = daSource.Rows[0][2].ToString();//学员基本情况
                this.textBoxX15.Text = daSource.Rows[0][3].ToString();//店铺链接
                this.textBoxX16.Text = daSource.Rows[0][4].ToString();//店铺行业/等级
                this.textBoxX17.Text = daSource.Rows[0][5].ToString();//动态评分
                this.textBoxX18.Text = daSource.Rows[0][6].ToString();//最近30天内服务情况
                this.textBoxX19.Text = daSource.Rows[0][7].ToString();//最近一个月评价数量
                this.textBoxX20.Text = daSource.Rows[0][8].ToString();//店铺访客数
                this.textBoxX21.Text = daSource.Rows[0][9].ToString();//主营占比
                this.textBoxX22.Text = daSource.Rows[0][10].ToString();//店铺货源情况
                this.textBoxX23.Text = daSource.Rows[0][11].ToString();//店铺宝贝数
                this.textBoxX24.Text = daSource.Rows[0][12].ToString();//店铺产品优化情况
                this.textBoxX25.Text = daSource.Rows[0][13].ToString();//店铺问题
                this.textBoxX26.Text = daSource.Rows[0][14].ToString();//诊断方案

            }
            else
            {
                this.textBoxX13.Text = qqStr+" - "+qqNickName;  //昵称
                this.textBoxX14.Text = "";//学员基本情况
                this.textBoxX15.Text = "";//店铺链接
                this.textBoxX16.Text = "";//店铺行业/等级
                this.textBoxX17.Text = "";//动态评分
                this.textBoxX18.Text = "";//最近30天内服务情况
                this.textBoxX19.Text = "";//最近一个月评价数量
                this.textBoxX20.Text = "";//店铺访客数
                this.textBoxX21.Text = "";//主营占比
                this.textBoxX22.Text = "";//店铺货源情况
                this.textBoxX23.Text = "";//店铺宝贝数
                this.textBoxX24.Text = "";//店铺产品优化情况
                this.textBoxX25.Text = "";//店铺问题
                this.textBoxX26.Text = "";//诊断方案
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sqlCheck = string.Format("select * from UT_VIPMemberShopDiagnosis where QQ = '{0}'", qqStr);
            DataTable dtCheck = DBHelper.ExecuteQuery(sqlCheck);
            if (dtCheck.Rows.Count > 0)
            {
                string sqlUp = string.Format("update UT_VIPMemberShopDiagnosis set " +
                    "NickName = '{1}'," +
                    "MemberBaseStat = '{2}'," +
                    "ShopLink = '{3}'," +
                    "ShopLevel = '{4}'," +
                    "ShopScore = '{5}'," +
                    "ThirtyServerStat = '{6}'," +
                    "LatelyMonthRemarkCnt = '{7}'," +
                    "GuestCnt = '{8}'," +
                    "MainCorePercent = '{9}'," +
                    "ShopGoodsSourceStat = '{10}'," +
                    "ShopGoodsNumber = '{11}'," +
                    "ShopOptimizeStat = '{12}'," +
                    "ShopProblem = '{13}'," +
                    "DiagnosisProgramme = '{14}'" +
                    "where QQ = '{0}'",
                    qqStr,
                    this.textBoxX13.Text,
                    this.textBoxX14.Text,
                    this.textBoxX15.Text,
                    this.textBoxX16.Text,
                    this.textBoxX17.Text,
                    this.textBoxX18.Text,
                    this.textBoxX19.Text,
                    this.textBoxX20.Text,
                    this.textBoxX21.Text,
                    this.textBoxX22.Text,
                    this.textBoxX23.Text,
                    this.textBoxX24.Text,
                    this.textBoxX25.Text,
                    this.textBoxX26.Text
                    );
                int resultNumUp = DBHelper.ExecuteUpdate(sqlUp);
                if (resultNumUp > 0)
                {
                    MessageBox.Show("修改成功");
                    //--------------日志开始------------------
                    string operationRemarkStr = string.Format("");
                    frmUTSOFTMAIN.OperationObject = "二次跟踪修改，qq ：" + qqStr;
                    frmUTSOFTMAIN.OperationRemark = "修改成功!";
                    frmUTSOFTMAIN.addlog(this.buttonX1);
                    //--------------日志结束------------------
                }
                else
                {
                    MessageBox.Show("修改失败,请勿写入特殊符号");
                }
            }
            else
            {
                string sql1 = string.Format("insert into UT_VIPMemberShopDiagnosis values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}'" +
                    ",'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
                    qqStr,
                    this.textBoxX13.Text,
                    this.textBoxX14.Text,
                    this.textBoxX15.Text,
                    this.textBoxX16.Text,
                    this.textBoxX17.Text,
                    this.textBoxX18.Text,
                    this.textBoxX19.Text,
                    this.textBoxX20.Text,
                    this.textBoxX21.Text,
                    this.textBoxX22.Text,
                    this.textBoxX23.Text,
                    this.textBoxX24.Text,
                    this.textBoxX25.Text,
                    this.textBoxX26.Text,
                    frmUTSOFTMAIN.StaffID,
                    System.DateTime.Now.ToShortDateString());
                int resultNum = DBHelper.ExecuteUpdate(sql1);
                if (resultNum > 0)
                {
                    MessageBox.Show("增加成功");
                    string sql2 = string.Format("update UT_YFVisit set VisitState = '已完成' where QQ = '{0}' and VisitType = '二次跟踪' and VisitState = '未完成'",
                        qqStr, System.DateTime.Now.ToShortDateString());
                    DBHelper.ExecuteUpdate(sql2);
                    //--------------日志开始------------------
                    string operationRemarkStr = string.Format("");
                    frmUTSOFTMAIN.OperationObject = "二次跟踪，qq ：" + qqStr;
                    frmUTSOFTMAIN.OperationRemark = "提交成功!";
                    frmUTSOFTMAIN.addlog(this.buttonX1);
                    //--------------日志结束------------------
                }
            }
        }
    }
}
