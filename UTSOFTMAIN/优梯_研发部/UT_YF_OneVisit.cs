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
    public partial class UT_YF_OneVisit : Form
    {
        public UT_YF_OneVisit()
        {
            InitializeComponent();
        }
        public string textDate = "";
        public string id ;
        public bool btnVisable = false;
        private void UT_YF_OneVisit_Load(object sender, EventArgs e)
        {
            this.buttonX1.Visible = btnVisable;
            this.textBoxX1.Text = textDate;
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("update UT_YFVisit set VisitRemark = '{0}',"+
                "VisitTime = getdate(), VisitState = '已完成' where id = {1}", 
                this.textBoxX1.Text,id);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("提交成功");
            }
            else 
            {
                MessageBox.Show("提交失败");
            }
            //--------------日志开始------------------
            string operationRemarkStr = string.Format("");
            frmUTSOFTMAIN.OperationObject = "一次跟踪，id ：" +id;
            frmUTSOFTMAIN.OperationRemark = "提交成功!";
            frmUTSOFTMAIN.addlog(this.buttonX1);
            //--------------日志结束------------------
        }
    }
}
