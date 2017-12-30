using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace UTSOFTMAIN
{
    static public class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        /// 
        public static frmUTSOFTMAIN mainwin = null;
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionEventHandler);
            //Application.SetCompatibleTextRenderingDefault(false);
            login qqlogin = new login();
            qqlogin.StartPosition = FormStartPosition.CenterScreen;
            qqlogin.ShowDialog();
            if (frmUTSOFTMAIN.reloginflag == 1)
            {
                qqlogin = new login();
                qqlogin.StartPosition = FormStartPosition.CenterScreen;
                qqlogin.ShowDialog();
                frmUTSOFTMAIN.reloginflag = 0;
            }

            if (qqlogin.DialogResult == DialogResult.OK)
            {
                qqlogin.Close();
                qqlogin.Dispose();
                mainwin = new frmUTSOFTMAIN();
                mainwin.MouseDown += new MouseEventHandler(mainwin_MouseDown);
                mainwin.MouseMove += new MouseEventHandler(mainwin_MouseMove);
                Application.Run(mainwin);
            }
            else
            {
                System.Environment.Exit(0);
            }
        }

        static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(e.ExceptionObject.ToString());
            }

            catch
            {

            }
        }

        public static Point downPoint;
        private static void mainwin_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = new Point(e.X, e.Y);
        }

        private static void mainwin_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mainwin.Location = new Point(mainwin.Location.X + e.X - downPoint.X,
                    mainwin.Location.Y + e.Y - downPoint.Y);
            }
        }
    }
}
