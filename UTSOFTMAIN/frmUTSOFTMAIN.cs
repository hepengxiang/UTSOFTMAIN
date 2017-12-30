using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MODEL;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace UTSOFTMAIN
{
    public partial class frmUTSOFTMAIN : Form
    {
        public static int reloginflag = 0;

        public static string IPAddr = "";//ip地址
        public static Form curWindow;
        public static int X = 0;
        public static int Y = 0;
        string autorunFileName = "";

        public static string StaffID;//用户身份证号码
        public static string AssumedName;//用户花名
        public static string PSW;//用户密码
        public static int LoginNum;//用户登陆次数
        public static string CompanyNames;//用户所属公司
        public static string DepartmentName;//用户所属部门
        public static string GroupName;//用户所属组
        public static string UserType;//用户职位


        public static string QQDetail = "";//营销点击信息跳转到此QQ详情
        public static string QQDetailTBName = "";//营销点击信息跳转到此QQ详情对应的表名
        public static bool dataChanged = false;


        public static ImageList imagelistLabel = new ImageList();
        public static ImageList imagelistButton = new ImageList();

        public static DataTable dtTablePower;//表格权限

        public static DataTable dtAllPerson = new DataTable();

        public static DataTable jxCaculateType = new DataTable();

        //日志数据
        public static List<OperationLog> lstlog = new List<OperationLog>();
        public static string MenuLeft = "";  //l操作名称1
        public static string OperationObject = ""; //操作对象
        public static string OperationRemark = ""; //相关内容说明


        //初始化全局菜单
        //public static List<GlobalMenu> curLstActor = new List<GlobalMenu>();
        
        public static List<GlobalMenu> lstmm = new List<GlobalMenu> { 
            //-------------一级菜单  left6  top3
            //UT软件 优梯-研发部
            new GlobalMenu { UpMenuName="无",MenuName = "优梯-VIP信息",  MenuType = "left",btnImg = 0,    pageName = "UT_VIPMessage",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "优梯-客服部",  MenuType = "left",btnImg = 3,    pageName = "UT_frmCostomPart",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "优梯-营销部",  MenuType = "left",btnImg = 6,    pageName = "UT_frmExtendPart",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "优梯-研发部",  MenuType = "left",btnImg = 9,    pageName = "UT_frmDevloPart",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "优梯-行政部",  MenuType = "left",btnImg = 27,    pageName = "frmInWard1",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "优梯-总经理",  MenuType = "left",btnImg = 18,    pageName = "UT_frmGeneralManager",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "个人薪资情况",  MenuType = "left",btnImg = 15,    pageName = "UT_frmPersonSituation",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "腾讯型公司",  MenuType = "left",btnImg = 21,    pageName = "TenXun_frmCompany",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "淘大型公司",  MenuType = "left",btnImg = 24,    pageName = "TaoDa_frmCompany",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "逸芸财务界面",  MenuType = "left",btnImg = 12,    pageName = "UT_AdminPart",     Visible = false },
            new GlobalMenu { UpMenuName="无",MenuName = "系统管理",  MenuType = "left",btnImg = 30,    pageName = "frmSystemManage",     Visible = false },
            //-------------二级菜单  left6  top3
            new GlobalMenu { UpMenuName="优梯-VIP信息",MenuName = "学员信息",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },

            new GlobalMenu { UpMenuName="优梯-客服部",MenuName = "论坛权限",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-客服部",MenuName = "组员情况",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-客服部",MenuName = "个人意向库",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-客服部",MenuName = "工作表格",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem4",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-客服部",MenuName = "表格权限管理",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem5",     Visible = false },

            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "资源分析",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "资源导入",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "资源分配",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "组员情况",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem4",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "组员资源库",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem5",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "组员意向库",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem6",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "表格权限管理",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem7",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-营销部",MenuName = "工作表格",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem8",     Visible = false },
            
            new GlobalMenu { UpMenuName="优梯-研发部",MenuName = "跟踪分配",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-研发部",MenuName = "跟踪回访",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-研发部",MenuName = "组员情况",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-研发部",MenuName = "学员信息",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem4",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-研发部",MenuName = "课堂情况",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem5",     Visible = false },
            
            new GlobalMenu { UpMenuName="优梯-行政部",MenuName = "在职员工信息",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-行政部",MenuName = "离职员工信息",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-行政部",MenuName = "工资生成",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-行政部",MenuName = "绩效规则定义",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem4",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-行政部",MenuName = "报表修改限制",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem6",     Visible = false },
            new GlobalMenu { UpMenuName="优梯-行政部",MenuName = "基本工资信息",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem7",     Visible = false },
            
            new GlobalMenu { UpMenuName="优梯-总经理",MenuName = "公司运营情况",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },

            new GlobalMenu { UpMenuName="个人薪资情况",MenuName = "个人绩效展示",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="个人薪资情况",MenuName = "考勤情况",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },
            new GlobalMenu { UpMenuName="个人薪资情况",MenuName = "工资表",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },
            new GlobalMenu { UpMenuName="个人薪资情况",MenuName = "管理绩效展示",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem4",     Visible = false },

            new GlobalMenu { UpMenuName="腾讯型公司",MenuName = "表格权限管理",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="腾讯型公司",MenuName = "报表入口",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },
            new GlobalMenu { UpMenuName="腾讯型公司",MenuName = "智源专属",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },

            new GlobalMenu { UpMenuName="淘大型公司",MenuName = "表格权限管理",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem1",     Visible = false },
            new GlobalMenu { UpMenuName="淘大型公司",MenuName = "报表入口",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem2",     Visible = false },

            new GlobalMenu { UpMenuName="系统管理",MenuName = "权限管理",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem4",     Visible = false },
            new GlobalMenu { UpMenuName="系统管理",MenuName = "日志查询",MenuType = "tabitem",btnImg = -1,    pageName = "tabItem3",     Visible = false },
        };

        public frmUTSOFTMAIN()
        {
            InitializeComponent();
            //HaveRun();//检查主程序是否已运行 
        }

        public static string ErrMsg = "";
        private void frmUTSOFTMAIN_Load(object sender, EventArgs e)
        {
            try
            {
                X = this.Location.X;
                Y = this.Location.Y;

                this.lblVersionMsg.Text += tools.ReadClientVersion();

                ClearMemory();

                GlobalVariable_init();//全局变量初始化
                ClearMemory();

                DataTableToListGM();//菜单权限
                ClearMemory();

                InterfaceShow(); //布局菜单
                ClearMemory();

                FirstPageSet();//设置首页
                ClearMemory();

                Notice_Timer_init();//提醒时钟开始
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static string FirstPage = "";
        //设置首显示页
        private void FirstPageSet()
        {
            if (this.flowLayoutPanel8.Controls.Count == 0)
                return;
            (this.flowLayoutPanel8.Controls[0] as Button).PerformClick();
        }
        //头像、全局变量初始化
        private void GlobalVariable_init()
        {
            dtTablePower = DBHelper.ExecuteQuery("select * from SYS_TablePower where StaffID = '" + StaffID + "'");
            dtAllPerson = DBHelper.ExecuteQuery("select CompanyNames,DepartmentName,GroupName,UserType,AssumedName,StaffID,onjob " +
                "from Users where onjob = 1 order by CompanyNames,DepartmentName desc");
            jxCaculateType = DBHelper.ExecuteQuery("select * from Pub_JXCaculate");
            try
            {
                label16.Text += AssumedName;
                label16.Width = 150;
            }
            catch (Exception ex)
            {
                MessageBox.Show("全局变量初始化" + ex.Message);
            }
            loadImageList();
        }
        //加入菜单---子菜单加入
        private void DataTableToListGM()
        {
            //查看是根据职位布局权限还是个人

            string sqlstr = string.Format("select * from SYS_UserPower where StaffID ='{0}' ", StaffID);
            DataTable dt = DBHelper.ExecuteQuery(sqlstr);

            string[] columNames = new string[] { "Visible" };
            string sql = string.Format("Visible = 1");
            DataTable dttemp = tools.dtFilter(dt, columNames, sql);
            if (dttemp.Rows.Count == 0)
            {
                string sqlStrType = string.Format("select * from SYS_UserPowerType " +
                    "where CompanyNames ='{0}' and DepartmentName = '{1}' and GroupName = '{2}' and UserType = '{3}'",
                    frmUTSOFTMAIN.CompanyNames, frmUTSOFTMAIN.DepartmentName, frmUTSOFTMAIN.GroupName, frmUTSOFTMAIN.UserType);
                dt = DBHelper.ExecuteQuery(sqlStrType);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < frmUTSOFTMAIN.lstmm.Count(); j++)
                    {
                        if (
                            dt.Rows[i][5].ToString().Trim() == frmUTSOFTMAIN.lstmm[j].MenuName.Trim() &&
                            dt.Rows[i][6].ToString().Trim() == frmUTSOFTMAIN.lstmm[j].UpMenuName.Trim()
                            )
                        {
                            frmUTSOFTMAIN.lstmm[j].Visible = (bool)dt.Rows[i][7];
                        }
                    }
                }
            }
            else 
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < frmUTSOFTMAIN.lstmm.Count(); j++)
                    {
                        if (
                            dt.Rows[i][3].ToString().Trim() == frmUTSOFTMAIN.lstmm[j].MenuName.Trim() &&
                            dt.Rows[i][4].ToString().Trim() == frmUTSOFTMAIN.lstmm[j].UpMenuName.Trim()
                            )
                        {
                            frmUTSOFTMAIN.lstmm[j].Visible = (bool)dt.Rows[i][5];
                        }
                    }
                }
            } 
        }

        //布局菜单和页面
        private void InterfaceShow()
        {
            foreach (GlobalMenu btm in frmUTSOFTMAIN.lstmm) 
            {
                try
                {
                    if ((btm.MenuType == "top" || btm.MenuType == "left") && btm.Visible)//左边栏和顶部
                    {
                        Button bt = new Button();
                        bt.Name = btm.MenuType + "|" + btm.btnImg.ToString();
                        bt.Tag = btm.pageName + "|" + btm.MenuName;

                        bt.Margin = bt.Padding = new Padding(0, 0, 0, 0);
                        bt.BackColor = Color.Transparent;
                        bt.FlatStyle = FlatStyle.Flat;
                        bt.FlatAppearance.BorderSize = 0;
                        bt.FlatAppearance.MouseDownBackColor = Color.Transparent;
                        bt.FlatAppearance.MouseOverBackColor = Color.Transparent;

                        bt.Click += new EventHandler(menuButtonClick);
                        bt.MouseEnter += new EventHandler(buttonenter);
                        bt.MouseLeave += new EventHandler(buttonleave);

                        if (btm.MenuType == "left")
                        {
                            bt.Size = new Size(212, 40);
                            bt.BackgroundImage = imageList2.Images[btm.btnImg];
                            flowLayoutPanel8.Controls.Add(bt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("加载【" + btm.MenuName + "】菜单出错" + ex.Message);
                }
            }
        }
        
        //检查主程序是否已运行
        public void HaveRun()
        {
            System.Diagnostics.Process[] Ps = System.Diagnostics.Process.GetProcessesByName("UTSOFTMAIN");

            if (Ps.Length > 1)//除了本身再没这个名字的进程
            {
                MessageBox.Show("UT教育已经运行，请注意屏幕右下角的图标提示");
                this.notifyIcon1.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(0);
            }
        }

        public static void gb_MouseEnter(object sender, EventArgs e)
        {
            (sender as Panel).BackColor = Color.WhiteSmoke;
            if ((sender as Panel).Parent != null)
                (sender as Panel).Parent.Focus();
        }
        public static void gb_MouseEnter1(object sender, EventArgs e)
        {
            (sender as GroupBox).BackColor = Color.WhiteSmoke;
            if ((sender as Panel).Parent != null)
                (sender as Panel).Parent.Focus();
        }
        public static void gb_MouseLeave(object sender, EventArgs e)
        {
            (sender as Panel).BackColor = Color.White;
            if( (sender as Panel).Parent !=null)
                (sender as Panel).Parent.Focus();
        }
        public static void gb_MouseLeave1(object sender, EventArgs e)
        {
            (sender as GroupBox).BackColor = Color.White;
            if ((sender as Panel).Parent != null)

                (sender as Panel).Parent.Focus();
        }
        //设置随机启动
        public void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    MessageBox.Show("该文件不存在!");
                    return;
                }
                String name = "UTSOFT";
                //String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                //MessageBox.Show(reg.ToString() + "\r\n name=" + name + "\r\n    fileName=" + fileName + "\r\n value=" + reg.GetValue(name, fileName).ToString());
                reg.GetValue(name, fileName).ToString();
                if (reg == null)
                {
                    //MessageBox.Show("原设定项没有找到");
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                }
                if (isAutoRun)
                    reg.SetValue(name, fileName);
                else
                    reg.SetValue(name, false);
                //MessageBox.Show("设定成功！", "提示");
            }
            catch
            {
                //throw new Exception(ex.ToString());   
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }
        //最小化
        private void button10_Click(object sender, EventArgs e)
        {
            Program.mainwin.WindowState = FormWindowState.Minimized;
        }
        //实现点击任务栏放大缩小
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Program.mainwin.Visible)
            {
                Program.mainwin.WindowState = FormWindowState.Minimized;
                Program.mainwin.Hide();
                Program.mainwin.Visible = false;
            }
            else
            {
                Program.mainwin.Visible = true;
                Program.mainwin.WindowState = FormWindowState.Normal;
                Program.mainwin.Activate();
            }
        }
        //设置
        private void button12_Click(object sender, EventArgs e)
        {
            Point xy = new Point();
            xy.X = Program.mainwin.Location.X + this.button12.Location.X;
            xy.Y = Program.mainwin.Location.Y + this.button12.Location.Y + 20;
            button12.ContextMenuStrip.Show(xy);
        }
        //设置 ，最大，最小，关闭按钮 鼠标进入
        private void button9_MouseEnter(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Red;
        }
        //设置 ，最大，最小，关闭按钮 鼠标离开
        private void button9_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Transparent;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Program.mainwin.WindowState = FormWindowState.Minimized;
            Program.mainwin.Hide();
            this.notifyIcon1.ShowBalloonTip(1000);
        }
        //顶部按钮离开
        private void buttonleave(object sender, EventArgs e)
        {
            Button leaveBtn = (sender as Button);

            leaveBtn.FlatAppearance.BorderSize = 0;
            if (leaveBtn != selectedBtn)
            {
                try
                {
                    int i = leaveBtn.Name.IndexOf('|');
                    string menutype = leaveBtn.Name.Substring(0, i);
                    string imgsn = (leaveBtn.Name).Substring(i + 1, leaveBtn.Name.Length - i - 1);
                    if (menutype == "left")
                    {
                        if (leaveBtn.BackgroundImage != null)
                            leaveBtn.BackgroundImage.Dispose();
                        leaveBtn.BackgroundImage = imageList2.Images[int.Parse(imgsn)];
                    }
                    else
                    {
                        if (leaveBtn.BackgroundImage != null)
                            leaveBtn.BackgroundImage.Dispose();
                        leaveBtn.BackgroundImage = imageList3.Images[int.Parse(imgsn)];
                    }
                }
                catch
                {
                    //MessageBox.Show("顶部按钮离开:"+ex.Message);
                }
            }
        }
        //顶部按钮进入
        Button selectedBtn;
        private void buttonenter(object sender, EventArgs e)
        {
            Button enterBtn = (sender as Button);

            enterBtn.FlatAppearance.BorderSize = 0;

            if (enterBtn != selectedBtn)
            {
                int i = enterBtn.Name.IndexOf('|');
                string menutype = enterBtn.Name.Substring(0, i);
                string imgsn = (enterBtn.Name).Substring(i + 1, enterBtn.Name.Length - i - 1);

                if (enterBtn.BackgroundImage != null)
                    enterBtn.BackgroundImage.Dispose();
                if (menutype == "left")
                {
                    enterBtn.BackgroundImage = imageList2.Images[int.Parse(imgsn) + 1];
                }
                else
                {
                    enterBtn.BackgroundImage = imageList3.Images[int.Parse(imgsn) + 1];
                }
            }

        }

        //用主按钮切换页面
        private void menuButtonClick(object sender, EventArgs e)
        {
            int i = 0;
            string menutype = "";
            string imgsn = "";
            MenuLeft = "";

            if (selectedBtn != null)
            {
                i = selectedBtn.Name.IndexOf('|');
                menutype = selectedBtn.Name.Substring(0, i);
                imgsn = (selectedBtn.Name).Substring(i + 1, selectedBtn.Name.Length - i - 1);

                if (selectedBtn.BackgroundImage != null)
                    selectedBtn.BackgroundImage.Dispose();
                if (menutype == "left")
                {
                    selectedBtn.BackgroundImage = imageList2.Images[int.Parse(imgsn)];
                }
                else
                {
                    selectedBtn.BackgroundImage = imageList3.Images[int.Parse(imgsn)];
                }
            }


            selectedBtn = (sender as Button);
            i = selectedBtn.Name.IndexOf('|');
            menutype = selectedBtn.Name.Substring(0, i);
            imgsn = (selectedBtn.Name).Substring(i + 1, selectedBtn.Name.Length - i - 1);

            if (selectedBtn.BackgroundImage != null)
                selectedBtn.BackgroundImage.Dispose();
            if (menutype == "left")
            {
                selectedBtn.BackgroundImage = imageList2.Images[int.Parse(imgsn) + 2];
            }
            else
            {
                selectedBtn.BackgroundImage = imageList3.Images[int.Parse(imgsn) + 2];
            }

            string UpMenuName = selectedBtn.Name;

            //切换到选中页面
            string winName = selectedBtn.Tag.ToString().Split(new char[]{'|'})[0];
            MenuLeft = selectedBtn.Tag.ToString().Split(new char[] { '|' })[1];
            int flag = 0;
            Type t = Type.GetType("UTSOFTMAIN." + winName);

            for (int j = 0; j < panel1.Controls.Count; j++)
            {
                if (panel1.Controls[j].GetType().ToString() == t.ToString())
                {
                    flag = 1;
                    curWindow = (Form)panel1.Controls[j];
                    panel1.Controls[j].Show();
                }
                else
                    panel1.Controls[j].Hide();
            }

            if (flag == 1)
                return;

            object obj = System.Activator.CreateInstance(t);//创建t类的实例 "obj"
            (obj as Form).TopLevel = false;
            (obj as Form).Parent = this.panel1;
            (obj as Form).Dock = DockStyle.Fill;
            (obj as Form).BackColor = Color.White;
            CheckAuthority(obj as Form);
            ChangeControl((obj as Form).Controls);
            (obj as Form).Show();
            curWindow = obj as Form;//当前窗口
        }
        //页面无权查看
        public static void CheckAuthority(Form form)
        {
            string formMenuName = "";
            string winName = form.GetType().ToString().Replace("UTSOFTMAIN.", "");
            foreach (GlobalMenu btm in frmUTSOFTMAIN.lstmm)//Form.text赋值
            {
                if (btm.pageName == winName)
                {
                    formMenuName = btm.MenuName;
                    form.Text = formMenuName;
                }
            }
            foreach (GlobalMenu btm in frmUTSOFTMAIN.lstmm)
            {
                if (btm.UpMenuName == formMenuName && btm.MenuType == "tabitem" && !btm.Visible) //页面无权查看
                {
                    tabitemResult = null;
                    findTbitem(form.Controls, btm.MenuName);
                    if (tabitemResult != null)
                    {
                        (form.Controls[0] as DevComponents.DotNetBar.TabControl).Tabs.Remove(tabitemResult);
                    }
                }
            }
        }
        public static DevComponents.DotNetBar.TabItem tabitemResult;
        public static void findTbitem(Control.ControlCollection ctc, string conName)
        {
            foreach (DevComponents.DotNetBar.TabControl tbCons in ctc)
            {
                DevComponents.DotNetBar.TabControl tbCon = (DevComponents.DotNetBar.TabControl)tbCons;
                foreach (DevComponents.DotNetBar.TabItem tabIts in tbCon.Tabs)
                {
                    if (tabIts.Text == conName)
                    {
                        tabitemResult = tabIts;
                    }
                }
            }
        }

        //改变控件的属性
        public static void ChangeControl(Control.ControlCollection ctc)
        {
            foreach (Control con in ctc)
            {
                if (con.GetType().ToString() == "DevComponents.DotNetBar.Controls.DataGridViewX")
                {
                    DataGridView dgv = (con as DataGridView);
                    dgv.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridViewX_RowPostPaint);
                    //dgv.BackgroundColor = Color.White;
                    dgv.AutoGenerateColumns = false;
                    dgv.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (con.HasChildren)
                    ChangeControl(con.Controls);
            }
        }
        //显示dataGridViewX行号
        public static void dataGridViewX_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush b = new SolidBrush((sender as DevComponents.DotNetBar.Controls.DataGridViewX).RowHeadersDefaultCellStyle.ForeColor);
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture), (sender as DevComponents.DotNetBar.Controls.DataGridViewX).DefaultCellStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
            catch { }
            finally
            {
                //b.Dispose();
            }
        }

        public static void btns_MouseEnter(object sender, EventArgs e)
        {
            if ((sender as Button).BackgroundImage != null)
                (sender as Button).BackgroundImage.Dispose();
            (sender as Button).BackgroundImage = imagelistButton.Images[1];
        }
        public static void btns_MouseHover(object sender, EventArgs e)
        {
            if ((sender as Button).BackgroundImage != null)
                (sender as Button).BackgroundImage.Dispose();
            (sender as Button).BackgroundImage = imagelistButton.Images[1];
        }
        static void btn_Click(object sender, EventArgs e)
        {
            if ((sender as Button).BackgroundImage != null)
                (sender as Button).BackgroundImage.Dispose();
            (sender as Button).BackgroundImage = imagelistButton.Images[2];
        }
        public static void btns_MouseLeave(object sender, EventArgs e)
        {
            if ((sender as Button).BackgroundImage != null)
                (sender as Button).BackgroundImage.Dispose();
            (sender as Button).BackgroundImage = imagelistButton.Images[0];
        }

        //通过tabpage名称获取tabpage对象
        /*
        public static DevComponents.DotNetBar.TabItem GetPage(DevComponents.DotNetBar.TabControl tc, string tpname)
        {
            DevComponents.DotNetBar.TabItem result = null;
           
            foreach (DevComponents.DotNetBar.TabItem tabIts in tc.Tabs)
            {
                if (tabIts.Text == tpname)
                {
                    result = tabIts;
                }
            }
            return result;
        }*/


        public static Control FindControlResult = new Control();
        public static void FindControl(Control.ControlCollection ctc, string conName)
        {
            foreach (Control con in ctc)
            {
                if (con.Name != null && con.Name == conName)
                {
                    FindControlResult = con;
                }
                if (con.Text != null && con.Text == conName)
                {
                    FindControlResult = con;
                }
                if (con.Tag != null && con.Tag.ToString() == conName)
                {
                    FindControlResult = con;
                }
                if (con.HasChildren)
                    FindControl(con.Controls, conName);
            }
        }

        private void 设为随机启动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("建议您将本软件设置为随机启动，软件将会自动提醒您参加课程学习，不遗漏任何精彩分享。\r\n           是否将UT教育软件设置为随系统启动？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SetAutoRun(autorunFileName, true);
            }
            else
            {
                SetAutoRun(autorunFileName, false);
            }
            MessageBox.Show("设置成功");
        }

        //日志写入--此处未验证弹窗日志
        public static void addlogLogin()
        {
            OperationLog opl = new OperationLog();
            opl.OperationTime = DateTime.Now;
            opl.IPAddr = IPAddr;
            opl.StaffID = StaffID;
            opl.MenuIn = "登陆";
            opl.OperationObject = OperationObject;
            opl.OperationRemark = OperationRemark;
            lstlog.Add(opl);
            OperationObject = "";
            OperationRemark = "";
        }
        public static void addlog(DevComponents.DotNetBar.ButtonX btn)
        {
            OperationLog opl = new OperationLog();
            opl.OperationTime = DateTime.Now;
            opl.IPAddr = IPAddr;
            opl.StaffID = StaffID;
            opl.MenuLeft = MenuLeft;
            if (btn != null)
            {
                opl.MenuIn = btn.Text;
                Control con = btn.Parent;
                for (int i = 0; i < 5; i++)
                {
                    if (con != null)
                    {
                        if (con is Form)//第一个父容器为窗体（弹窗界面）
                        {
                            Form windowsFrom = (Form)con;
                            opl.MenuRight = windowsFrom.Text;
                            break;
                        }
                        if (con is DevComponents.DotNetBar.TabControlPanel)//第一个父容器为选项卡（软件界面）
                        {
                            DevComponents.DotNetBar.TabControlPanel tabit = (DevComponents.DotNetBar.TabControlPanel)con;
                            opl.MenuRight = tabit.TabItem.Text;
                            break;
                        }
                        con = con.Parent;
                    }
                }
            }
            opl.OperationObject = OperationObject;
            opl.OperationRemark = OperationRemark;
            lstlog.Add(opl);
            OperationRemark = "";//操作对象
            OperationObject = "";//操作备注
        }
        //日志上传方法
        public void uploadlog()
        {
            if (lstlog.Count == 0)
                return;

            string sqlstr = "insert into SYS_ErpLog ";

            for (int i = 0; i < lstlog.Count; i++)
            {
                if (lstlog[i].OperationRemark == null)
                    lstlog[i].OperationRemark = "";
                else
                    lstlog[i].OperationRemark = lstlog[i].OperationRemark.Replace("'", "#");
                if (lstlog[i].OperationRemark.Length >= 400)
                    lstlog[i].OperationRemark = lstlog[i].OperationRemark.Substring(0, 400);

                string tmp = string.Format("select '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}' ",
                        frmUTSOFTMAIN.CompanyNames,
                        tools.sqldate(lstlog[i].OperationTime), 
                        lstlog[i].IPAddr, 
                        lstlog[i].StaffID,
                        lstlog[i].MenuLeft,
                        lstlog[i].MenuRight,
                        lstlog[i].MenuIn,
                        lstlog[i].OperationObject,
                        lstlog[i].OperationRemark );

                if (i == lstlog.Count - 1)
                {
                    tmp += "";
                }
                else
                {
                    tmp += " union ";
                }

                sqlstr += tmp;
            }

            if (DBHelper.ExecuteUpdate(sqlstr) > 0 || sqlstr.Length > 10000)
            {
                lstlog.Clear();
            }
        }
        /// <summary>
        /// 表格权限初始化
        /// </summary>
        /// <param name="pan">存放表格按钮的panel</param>
        public static void tablePowerBtnInit(FlowLayoutPanel flpan)
        {
            foreach (Control tmpControl in flpan.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    bool groupPanelShow = false;
                    DevComponents.DotNetBar.Controls.GroupPanel grop = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    string groupName = grop.Text;//GroupPanel组名称
                    foreach (Control btns in grop.Controls)
                    {
                        if (btns is DevComponents.DotNetBar.ButtonX)
                        {
                            DevComponents.DotNetBar.ButtonX btn = (DevComponents.DotNetBar.ButtonX)btns;
                            string btnName = btn.Text;//按钮名称
                            if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "行政经理")
                                btn.Visible = true;
                            else
                                btn.Visible = false;
                            for (int i = 0; i < frmUTSOFTMAIN.dtTablePower.Rows.Count; i++)
                            {
                                if (frmUTSOFTMAIN.dtTablePower.Rows[i][3].ToString() == frmUTSOFTMAIN.MenuLeft &&
                                    frmUTSOFTMAIN.dtTablePower.Rows[i][4].ToString() == groupName &&
                                    frmUTSOFTMAIN.dtTablePower.Rows[i][5].ToString() == btnName)
                                {
                                    bool btnVisible = Convert.ToBoolean(frmUTSOFTMAIN.dtTablePower.Rows[i][6].ToString());
                                    btn.Visible = btnVisible;
                                    if (btnVisible)
                                        groupPanelShow = true;
                                }
                            }
                        }
                    }
                    if (frmUTSOFTMAIN.UserType == "系统管理员" || frmUTSOFTMAIN.UserType == "行政经理")
                        grop.Visible = true;
                    else
                        grop.Visible = groupPanelShow;
                }
            }
        }
        /// <summary>
        /// 表格权限提交
        /// </summary>
        /// <param name="pan">存放表格选项的panel</param>
        /// <param name="staffIDSub">提交对象身份证</param>
        public static void tablePowerSubmit(Panel pan, string CompanyName, string staffIDSub)
        {
            DBHelper.ExecuteUpdate("delete from SYS_TablePower where StaffID = '" + staffIDSub + "'");
            string sql1 = string.Format("insert into SYS_TablePower");
            foreach (Control tmpControl in pan.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel grop = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    string groupName = grop.Text;//GroupPanel组名称
                    foreach (Control cheControl in grop.Controls)
                    {
                        if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                        {
                            DevComponents.DotNetBar.Controls.CheckBoxX check = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                            string checkName = check.Text;//按钮名称
                            if (check.Checked)
                            {
                                sql1 += string.Format(" select '{0}','{1}','{2}','{3}','{4}',1 union all ",
                                    CompanyName, staffIDSub, frmUTSOFTMAIN.MenuLeft, groupName, checkName);
                            }
                            else
                            {
                                sql1 += string.Format(" select '{0}','{1}','{2}','{3}','{4}',0 union all ",
                                    CompanyName, staffIDSub, frmUTSOFTMAIN.MenuLeft, groupName, checkName);
                            }
                        }
                    }
                }
            }
            sql1 = sql1.Substring(0, sql1.Length - 10);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
                MessageBox.Show("提交成功");
            else
                MessageBox.Show("提交失败");
        }
        /// <summary>
        /// 权限显示
        /// </summary>
        /// <param name="pan">存放表格选项的panel</param>
        /// <param name="staffIDSub">显示对象的身份证</param>
        public static void tablePowerShow(Panel pan, string staffIDSub) 
        {
            foreach (Control tmpControl in pan.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel grop = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    string groupName = grop.Text;//GroupPanel组名称
                    foreach (Control cheControl in grop.Controls)
                    {
                        if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                        {
                            DevComponents.DotNetBar.Controls.CheckBoxX check = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                            check.Checked = false;
                        }
                    }
                }
            }
            string sql1 = string.Format("select * from SYS_TablePower where StaffID = '{0}'", staffIDSub);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            foreach (Control tmpControl in pan.Controls)
            {
                if (tmpControl is DevComponents.DotNetBar.Controls.GroupPanel)
                {
                    DevComponents.DotNetBar.Controls.GroupPanel grop = (DevComponents.DotNetBar.Controls.GroupPanel)tmpControl;
                    string groupName = grop.Text;//GroupPanel组名称
                    foreach (Control cheControl in grop.Controls)
                    {
                        if (cheControl is DevComponents.DotNetBar.Controls.CheckBoxX)
                        {
                            DevComponents.DotNetBar.Controls.CheckBoxX check = (DevComponents.DotNetBar.Controls.CheckBoxX)cheControl;
                            string checkName = check.Text;//按钮名称
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if (dt1.Rows[i][3].ToString() == frmUTSOFTMAIN.MenuLeft &&
                                    dt1.Rows[i][4].ToString() == groupName &&
                                    dt1.Rows[i][5].ToString() == checkName) 
                                {
                                    check.Checked = Convert.ToBoolean(dt1.Rows[i][6].ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        #region 内存回收
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }
        #endregion


        public void loadImageList()
        {
            imagelistLabel.ImageSize = new Size(80, 35);
            imagelistLabel.ColorDepth = ColorDepth.Depth32Bit;
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                imagelistLabel.Images.Add(imageList1.Images[i]);
            }

            imagelistButton.ImageSize = new Size(253, 59);
            imagelistButton.ColorDepth = ColorDepth.Depth32Bit;
            for (int i = 0; i < imageList1.Images.Count; i++)
            {
                imagelistButton.Images.Add(imageList1.Images[i]);
            }
        }

        public static void lbl_MouseLeave(object sender, EventArgs e)
        {
            if ((sender as Label).Text == "打开")
                (sender as Label).Image = frmUTSOFTMAIN.imagelistLabel.Images[2];
            else
                (sender as Label).Image = frmUTSOFTMAIN.imagelistLabel.Images[0];
        }
        public static void lbl_MouseEnter(object sender, EventArgs e)
        {
            (sender as Label).Image = frmUTSOFTMAIN.imagelistLabel.Images[1];
        }

        private void 设为自动登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你要设置UT教育软件为自动登录吗？", "自动登录设置", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string filecon = frmUTSOFTMAIN.AssumedName + "|" + PSW;
                filecon += "|Y";
                FileStream fs = new FileStream("password.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(filecon);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            else
            {
                string filecon = frmUTSOFTMAIN.AssumedName + "|" + PSW;
                filecon += "|N";
                FileStream fs = new FileStream("password.txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(filecon);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {
            frmResetPassword frp = new frmResetPassword();
            frp.Show();
        }

    }

    //Panel开启双缓冲
    class MyPanel : Panel
    {
        public MyPanel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }
    }

    //FlowLayoutPanel开启双缓冲
    class MyFlowLayoutPanel : FlowLayoutPanel
    {
        public MyFlowLayoutPanel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }
    }

    //PictureBox
    class MyPictureBox : PictureBox
    {
        public MyPictureBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }
    }

    //DataGridView
    class MyDataGridView : DataGridView
    {
        public MyDataGridView()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            this.SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }
    }


    ///
    /// 支持滚轮的FlowLayoutPanel.
    ///
    class ScrollFlowLayoutPanel : FlowLayoutPanel
    {
        //声明 API 函数 

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(int hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        //定义消息常数 
        public const int CUSTOM_MESSAGE = 0X400 + 2;//自定义消息
        public const int WM_CLICK = 0x00F5;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_GETTEXT = 0xd;
        public ScrollFlowLayoutPanel()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            this.AutoScroll = false;
            this.FlowDirection = FlowDirection.TopDown;
            this.WrapContents = false;
            this.HorizontalScroll.Maximum = 0; // 把水平滚动范围设成0就看不到水平滚动条了
            this.AutoScroll = true; // 注意启用滚动的顺序，应是完成设置的最后一条语句
            this.DoubleBuffered = true;
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseClick(e);
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            this.Focus();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (this.VerticalScroll.Maximum > this.VerticalScroll.Value + 50)
                    this.VerticalScroll.Value += 50;
                else
                    this.VerticalScroll.Value = this.VerticalScroll.Maximum;
            }
            else
            {
                if (this.VerticalScroll.Value > 50)
                    this.VerticalScroll.Value -= 50;
                else
                {
                    this.VerticalScroll.Value = 0;
                }
            }
        }
    }
}
