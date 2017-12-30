using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

    public partial class frmUTSOFTMAIN : Form
    {
        private void Notice_Timer_init()
        {
            timer1.Enabled = true;
            timer1.Start();
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uploadlog();
            this.notifyIcon1.Visible = false;
            this.notifyIcon1.Dispose();
            System.Environment.Exit(0);
        }

        int MessageTime = 0;
        int OtherMessageTime = 99999;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (OtherMessageTime == 99999)
            {
                //营销部意向过期入库,分配资源跟踪超时更新
                if (frmUTSOFTMAIN.DepartmentName == "营销部")
                {
                    string sqlYXPromotes = string.Format(
                        //营销部资源表更新
                        "update UT_YXResource set TrackState = '已过期' where OverTime < getdate() and TrackState = '已分配'");
                    DBHelper.ExecuteUpdate(sqlYXPromotes);
                }
            }
            if (MessageTime > 59)//每1分钟执行
            {
                MessageTime = 0;
                ClearMemory();
                string sqlstr_checklogin = string.Format("select LoginNum from Users where StaffID='{0}' ", frmUTSOFTMAIN.StaffID);
                DataTable dt_checklogin = DBHelper.ExecuteQuery(sqlstr_checklogin);
                if (dt_checklogin.Rows.Count != 0 ) 
                {
                    if (frmUTSOFTMAIN.LoginNum < int.Parse(dt_checklogin.Rows[0][0].ToString()))
                    {
                        timer1.Stop();
                        MessageBox.Show("您的账号已在另一电脑登录，程序退出");
                        Application.Exit();
                    }
                }
                uploadlog();//每分钟上传一次日志
            }
            MessageTime++;
            OtherMessageTime++;
        }
    }
}
