using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using MODEL;

namespace UTSOFTMAIN
{

    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        string auto = "";
        private void login_Load(object sender, EventArgs e)
        {
            try
            {
                HaveRun();
                UTLogin();
                lblVersionMsg.Text += tools.ReadClientVersion();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //检查主程序是否已运行
        public void HaveRun()
        {
            try
            {
                System.Diagnostics.Process[] pp = System.Diagnostics.Process.GetProcessesByName("UTSOFTMAIN");
                string a = pp.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            System.Diagnostics.Process[] Ps = System.Diagnostics.Process.GetProcessesByName("UTSOFTMAIN");

            if (Ps.Length > 1)
            {
                MessageBox.Show("UT教育已经运行，请注意屏幕右下角的图标提示");
                this.DialogResult = DialogResult.No;
                this.Close();
                this.Dispose();
                System.Environment.Exit(0);
            }
        }
        //测试网络部分
        private void UTLogin()
        {
            if (File.Exists("password.txt"))
            {
                StreamReader sr = new StreamReader("password.txt");
                string fileContent = string.Empty;
                fileContent = sr.ReadToEnd();
                sr.Close();
                string[] userpass = fileContent.Split('|');
                if (userpass.Length == 3)
                {
                    this.textBox1.Text = userpass[0];
                    this.textBox2.Text = userpass[1];
                    if (this.textBox2.Text.Length > 0)
                        this.checkBox1.Checked = true;
                    else
                        this.checkBox1.Checked = false;
                    auto = userpass[2];
                    if (auto == "Y")
                        this.checkBox2.Checked = true;
                    else
                        this.checkBox2.Checked = false;
                }
            }
            button2.Enabled = false;
            int test = 0;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    test = int.Parse(DBHelper.ExecuteQuery("select count(1) from Users").Rows[0][0].ToString());
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (test > 0)
                    break;
            }
            if (test > 0)
            {
                button2.Enabled = true;//测试网络部分
            }
            else
            {
                MessageBox.Show("网络故障或服务器繁忙，请稍后再试");
                System.Environment.Exit(0);
            }

            if (auto == "Y" && button2.Enabled == true)
            {
                userpasswdLogin();
            }
        }

        int init_login = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.BackgroundImage = imageList3.Images[2];
            this.Close();
            System.Environment.Exit(0);
        }

        //使用账号和密码登录
        int logintimes = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button2.BackgroundImage = imageList1.Images[2];
            userpasswdLogin();
        }

        private void userpasswdLogin()
        {
            if (this.textBox1.Text.Trim() == "" || this.textBox2.Text.Trim() == "")
            {
                MessageBox.Show("用户和密码不能为空");
                button2.Enabled = true;
                return;
            }
            string checkUser = tools.FilteSQLStr(this.textBox1.Text.Trim());
            string checkPSW = tools.FilteSQLStr(this.textBox2.Text.Trim()); ;

            string sqlstr = string.Format("select * from Users where AssumedName = '{0}' and PSW = '{1}'",checkUser, checkPSW);
            DataTable dt = DBHelper.ExecuteQuery(sqlstr);

            if (dt.Rows.Count > 0)
            {
                frmUTSOFTMAIN.StaffID = dt.Rows[0][6].ToString();//用户身份证号码
                frmUTSOFTMAIN.AssumedName = dt.Rows[0][4].ToString();//用户花名
                frmUTSOFTMAIN.PSW = dt.Rows[0][18].ToString();//用户密码
                frmUTSOFTMAIN.LoginNum = int.Parse(dt.Rows[0][19].ToString()) + 1;//用户登陆次数
                frmUTSOFTMAIN.CompanyNames = dt.Rows[0][0].ToString();//用户所属公司
                frmUTSOFTMAIN.DepartmentName = dt.Rows[0][1].ToString();//用户所属部门
                frmUTSOFTMAIN.GroupName = dt.Rows[0][2].ToString();//用户所属组
                frmUTSOFTMAIN.UserType = dt.Rows[0][3].ToString();//用户职位
                

                this.DialogResult = DialogResult.OK;
                if (!this.checkBox1.Checked)
                {
                    this.textBox2.Text = "";
                }
                string filecon = checkUser + "|" + checkPSW;
                if (this.checkBox2.Checked)
                    filecon += "|Y";
                else
                    filecon += "|N";
                FileStream fs = new FileStream("password.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(filecon);
                sw.Flush();
                sw.Close();
                fs.Close();
                SetLogin();
                this.Close();
            }
            else//密码错
            {
                logintimes++;
                if (logintimes >= 3)
                {
                    MessageBox.Show("用户或密码错误已达三次，程序退出！");
                    this.Close();
                    return;
                }
                MessageBox.Show("您的帐号密码错误，请重新输入，如忘记密码，请点击联系管理员！");
                button2.Enabled = true;
                return;
            }
        }
        private void SetLogin()
        {
            //frmUTSOFTMAIN.IPAddr = GetIP();//获取IP地址
            string addipstr = "";
            if (frmUTSOFTMAIN.IPAddr != "")
                addipstr = " , Last_LoginIpAddr='" + frmUTSOFTMAIN.IPAddr + "' ";

            string sqlstr3 = string.Format("update Users set LoginNum=LoginNum+1 where  AssumedName='{0}' and onjob=1", frmUTSOFTMAIN.AssumedName);
            int sqlresult = DBHelper.ExecuteUpdate(sqlstr3);
            //--------------日志开始------------------
            frmUTSOFTMAIN.OperationObject = "登录";
            frmUTSOFTMAIN.OperationRemark = "登陆次数："+frmUTSOFTMAIN.LoginNum;
            frmUTSOFTMAIN.addlogLogin();
            //--------------日志结束------------------
            if (sqlresult <= 0)
            {
                MessageBox.Show(frmUTSOFTMAIN.ErrMsg + "    \n[" + frmUTSOFTMAIN.AssumedName + "]登录--连接服务器失败--");
                System.Environment.Exit(0);
            }
        }
        
        int second_cnt = 0;
        private void timer_login_Tick(object sender, EventArgs e)//检查初始登录页面30秒内是否打开
        {
            second_cnt++;
            if (init_login == 1)
            {
                timer_login.Stop();
                this.timer_login.Enabled = false;
            }
            if (second_cnt == 30 && init_login == 0)
            {
                MessageBox.Show("抱歉，当前存在网络问题或服务器繁忙，请您稍候再试。");
                System.Environment.Exit(0);
            }
        }
        
        //获取IP地址
        public string GetIP()
        {
            DateTime begin = DateTime.Now;
            string tempip = "";
            try
            {
                WebRequest wr = WebRequest.Create("http://1212.ip138.com/ic.asp");
                Stream s = wr.GetResponse().GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);
                string all = sr.ReadToEnd(); //读取网站的数据
                Regex reg = new Regex("<center>(.*?)</center>");
                tempip = reg.Match(all).Groups[1].Value;
                tempip = tempip.Replace("您的IP是：", "");
                tempip = tempip.Replace("来自：", "");
                tempip = tempip.Split(new char[]{']'})[1];
                sr.Close();
                sr.Dispose();
                s.Close();
                s.Dispose();
            }
            catch
            {
                
            }
            return tempip;
        }
        Point downPoint;
        private void button7_MouseEnter(object sender, EventArgs e)
        {
            this.button7.BackgroundImage = imageList3.Images[4];
        }
        private void button7_MouseLeave(object sender, EventArgs e)
        {
            this.button7.BackgroundImage = imageList3.Images[3];
        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                button2_Click(null, null);
            } 
        }
        private void button7_Click(object sender, EventArgs e)//最小化
        {
            this.button7.BackgroundImage = imageList3.Images[5];
            this.WindowState = FormWindowState.Minimized;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)//自动登录选项检查
        {
            if (checkBox1.Checked == false && checkBox2.Checked == true)
            {
                MessageBox.Show("只有选择记住密码才能自动登录");
                checkBox2.Checked = false;
                return;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)//记住密码
        {
            if (checkBox1.Checked == true && (textBox1.Text.Length == 0 || textBox2.Text.Length == 0))
            {
                MessageBox.Show("只有选择记住密码才能自动登录");
                checkBox2.Checked = false;
                return;
            }
        }
        private void button2_MouseEnter(object sender, EventArgs e)//登录按钮进入
        {
            (sender as Button).BackgroundImage = imageList1.Images[1];
        }
        private void button2_MouseHover(object sender, EventArgs e)//登录按钮悬浮
        {
            (sender as Button).BackgroundImage = imageList1.Images[1];
        }
        private void button2_MouseLeave(object sender, EventArgs e)//登录按钮离开
        {
            (sender as Button).BackgroundImage = imageList1.Images[0];
        }
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = imageList3.Images[1];
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackgroundImage = imageList3.Images[0];
        }
        private void login_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }
        private void login_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - downPoint.X,
                    this.Location.Y + e.Y - downPoint.Y);
            }
        }
        private void MouseEnter_changeColor(object sender, EventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Forms.Button")
                (sender as Button).ForeColor = Color.DeepSkyBlue;
            if (sender.GetType().ToString() == "System.Windows.Forms.Label")
                (sender as Label).ForeColor = Color.DeepSkyBlue;
            if (sender.GetType().ToString() == "System.Windows.Forms.CheckBox")
                (sender as CheckBox).ForeColor = Color.DeepSkyBlue;
        }
        private void MouseLeave_changeColor(object sender, EventArgs e)
        {
            if (sender.GetType().ToString() == "System.Windows.Forms.Button")
                (sender as Button).ForeColor = Color.Silver;
            if (sender.GetType().ToString() == "System.Windows.Forms.Label")
                (sender as Label).ForeColor = Color.Silver;
            if (sender.GetType().ToString() == "System.Windows.Forms.CheckBox")
                (sender as CheckBox).ForeColor = Color.Silver;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

    }
}

