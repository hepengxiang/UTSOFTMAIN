using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Threading;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.Runtime.InteropServices;
using MODEL;
using System.Data;
using System.Data.OleDb;
using DevComponents.DotNetBar.Controls;

namespace UTSOFTMAIN
{
    public class ExtendedWebBrowser : System.Windows.Forms.WebBrowser
    {
        System.Windows.Forms.AxHost.ConnectionPointCookie cookie;
        WebBrowserExtendedEvents events;

        //This method will be called to give you a chance to create your own event sink

        protected override void CreateSink()
        {
            //MAKE SURE TO CALL THE BASE or the normal events won't fire

            base.CreateSink();
            events = new WebBrowserExtendedEvents(this);
            cookie = new System.Windows.Forms.AxHost.ConnectionPointCookie(this.ActiveXInstance, events, typeof(DWebBrowserEvents2));
        }

        protected override void DetachSink()
        {
            if (null != cookie)
            {
                cookie.Disconnect();
                cookie = null;
            }
            base.DetachSink();
        }

        //This new event will fire when the page is navigating

        public event EventHandler<WebBrowserExtendedNavigatingEventArgs> BeforeNavigate;
        public event EventHandler<WebBrowserExtendedNavigatingEventArgs> BeforeNewWindow;

        protected void OnBeforeNewWindow(string url, out bool cancel)
        {
            EventHandler<WebBrowserExtendedNavigatingEventArgs> h = BeforeNewWindow;
            WebBrowserExtendedNavigatingEventArgs args = new WebBrowserExtendedNavigatingEventArgs(url, null);
            if (null != h)
            {
                h(this, args);
            }
            cancel = args.Cancel;
        }

        protected void OnBeforeNavigate(string url, string frame, out bool cancel)
        {
            EventHandler<WebBrowserExtendedNavigatingEventArgs> h = BeforeNavigate;
            WebBrowserExtendedNavigatingEventArgs args = new WebBrowserExtendedNavigatingEventArgs(url, frame);
            if (null != h)
            {
                h(this, args);
            }
            //Pass the cancellation chosen back out to the events

            cancel = args.Cancel;
        }
        //This class will capture events from the WebBrowser

        class WebBrowserExtendedEvents : System.Runtime.InteropServices.StandardOleMarshalObject, DWebBrowserEvents2
        {
            ExtendedWebBrowser _Browser;
            public WebBrowserExtendedEvents(ExtendedWebBrowser browser) { _Browser = browser; }

            //Implement whichever events you wish

            public void BeforeNavigate2(object pDisp, ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
            {
                _Browser.OnBeforeNavigate((string)URL, (string)targetFrameName, out cancel);
            }

            public void NewWindow3(object pDisp, ref bool cancel, ref object flags, ref object URLContext, ref object URL)
            {
                _Browser.OnBeforeNewWindow((string)URL, out cancel);
            }

        }
        [System.Runtime.InteropServices.ComImport(), System.Runtime.InteropServices.Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"),
        System.Runtime.InteropServices.InterfaceTypeAttribute(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIDispatch),
        System.Runtime.InteropServices.TypeLibType(System.Runtime.InteropServices.TypeLibTypeFlags.FHidden)]
        public interface DWebBrowserEvents2
        {

            [System.Runtime.InteropServices.DispId(250)]
            void BeforeNavigate2(
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
                [System.Runtime.InteropServices.In] ref object URL,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object targetFrameName, [System.Runtime.InteropServices.In] ref object postData,
                [System.Runtime.InteropServices.In] ref object headers,
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.Out] ref bool cancel);
            [System.Runtime.InteropServices.DispId(273)]
            void NewWindow3(
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
                [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] ref bool cancel,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object URLContext,
                [System.Runtime.InteropServices.In] ref object URL);

        }
    }
    public class WebBrowserExtendedNavigatingEventArgs : System.ComponentModel.CancelEventArgs
    {
        private string _Url;
        public string Url
        {
            get { return _Url; }
        }

        private string _Frame;
        public string Frame
        {
            get { return _Frame; }
        }

        public WebBrowserExtendedNavigatingEventArgs(string url, string frame)
            : base()
        {
            _Url = url;
            _Frame = frame;
        }
    }
    class tools
    {
        public static string ftpURI = "ftp://122.114.36.252/UTSOFT/";
        public static string ftpPhotos = "ftp://122.114.36.252/UTSOFT/Photos/";
        public static string ftpChatPhotos = "ftp://122.114.36.252/UTSOFT/ChatImages/";
        public static string ftpUserID = "utsoft";
        public static string ftpPassword = "9JZkhjduPQvD";

        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        [DllImport("user32.dll", ExactSpelling = true)]
        private static extern bool EnumChildWindows(IntPtr hwndParent, WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        //[DllImport("user32.dll")] 
        //private static extern IntPtr FindWindowW(string lpClassName, string lpWindowName); 
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }

        private List<WindowInfo> EnumChildWindowsCallback(IntPtr handle)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumChildWindows(handle, delegate(IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd 
                wnd.hWnd = hWnd;
                //get window name 
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list 
                wndList.Add(wnd);
                return true;
            }, 0);
            //return wndList.Where(it => it.szWindowName == name && it.szClassName == classname).ToList();

            return wndList;
        }

        public List<WindowInfo> GetAllDesktopWindows(string name, string classname)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows 
            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd 
                wnd.hWnd = hWnd;
                //get window name 
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class 
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list 
                wndList.Add(wnd);
                return true;
            }, 0);
            return wndList;
            //return wndList.Where(it => it.szWindowName == name && it.szClassName == classname).ToList();
        } 

        public static string oddstr(string str)
        {
            string result = str;
            result = result.Replace('\'', '"');
            result = result.Replace('[', ' ');
            result = result.Replace(']', ' ');
            result = result.Replace('%', ' ');
            result = result.Replace('_', '-');
            result = result.Replace('^', ' ');
            return result;
        }
        public static string IsHoliday(string date)
        {
            string url = "http://www.easybots.cn/api/holiday.php?d=";
            url = url + date;
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Timeout = 2000;
            httpRequest.Method = "GET";
            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                StreamReader sr = new StreamReader(httpResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
                string result = sr.ReadToEnd();
                result = result.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                int status = (int)httpResponse.StatusCode;
                sr.Close();
                return result;
            }
            catch (Exception)
            {
                return "{\"20161216\":\"3\"}";
            }  
        } 
        public static string sqldate(DateTime csharpdate)
        {
            string ms = "";
            if (csharpdate.Millisecond > 99)
                ms = csharpdate.Millisecond.ToString();
            else if(csharpdate.Millisecond > 9)
                ms ="0"+ csharpdate.Millisecond.ToString();
            else
                ms = "00" + csharpdate.Millisecond.ToString();


            return csharpdate.Year.ToString() + "/" +
                (csharpdate.Month > 9 ? csharpdate.Month.ToString() : "0" + csharpdate.Month.ToString()) + "/"
                + (csharpdate.Day > 9 ? csharpdate.Day.ToString() : "0" + csharpdate.Day.ToString()) + " "
                + (csharpdate.Hour > 9 ? csharpdate.Hour.ToString() : "0" + csharpdate.Hour.ToString()) +":"
                + (csharpdate.Minute > 9 ? csharpdate.Minute.ToString() : "0" + csharpdate.Minute.ToString()) + ":"
                + (csharpdate.Second > 9 ? csharpdate.Second.ToString() : "0" + csharpdate.Second.ToString())+"."
                + ms;
        }

        public static string sqldatenoms(DateTime csharpdate)
        {
            return csharpdate.Year.ToString() + "/" +
                (csharpdate.Month > 9 ? csharpdate.Month.ToString() : "0" + csharpdate.Month.ToString()) + "/"
                + (csharpdate.Day > 9 ? csharpdate.Day.ToString() : "0" + csharpdate.Day.ToString()) + " "
                + (csharpdate.Hour > 9 ? csharpdate.Hour.ToString() : "0" + csharpdate.Hour.ToString()) + ":"
                + (csharpdate.Minute > 9 ? csharpdate.Minute.ToString() : "0" + csharpdate.Minute.ToString()) + ":"
                + (csharpdate.Second > 9 ? csharpdate.Second.ToString() : "0" + csharpdate.Second.ToString()) ;
        }
        public static int getIndexForTable(DataTable dts,string strs,int colIndex) 
        {
            int index = 0;
            for (int i = 0; i < dts.Rows.Count;i++ ) 
            {
                if(dts.Rows[i][colIndex].ToString() == strs)
                {
                    index = i;
                }
            }
            return index;
        }
        public static int killprocess(string appname, string AppPath)
        {
            int result = 0;
            bool isrun = false;//是否正在运行

            System.Diagnostics.Process[] Ps = System.Diagnostics.Process.GetProcessesByName(appname);
            if (Ps.Length == 0)
            {
                //MessageBox.Show("获取进程列表失败");
                return 0;//没这个名字的进程无须kill
            }
            foreach (System.Diagnostics.Process p in Ps)
            {
                if (p.MainModule.FileName == AppPath)
                {
                    isrun = true;
                    break;
                }
            }
            if (isrun)
            {
                //MessageBox.Show(this, "检测到【" + AppFileName + "】正在运行，升级程序将将其强制关闭。 \r\n请在按下确定前手动关闭以避免数据丢失。", "自动升级", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Ps = System.Diagnostics.Process.GetProcessesByName(appname);
                foreach (System.Diagnostics.Process p in Ps)
                {
                    if (p.MainModule.FileName == AppPath)
                    {
                        try
                        {
                            p.Kill();
                            p.WaitForExit(10000);
                        }
                        catch (Exception exx)
                        {
                            throw exx;
                        }
                    }
                }
                System.Threading.Thread.Sleep(1000);
            }

            return result;
        }

        private static FtpWebRequest GetRequest(string URI, string username, string password)
        {
            //根据服务器信息FtpWebRequest创建类的对象
            FtpWebRequest result = (FtpWebRequest)FtpWebRequest.Create(new Uri(URI));
            //提供身份验证信息
            result.Credentials = new System.Net.NetworkCredential(username, password);
            //设置请求完成之后是否保持到FTP服务器的控制连接，默认值为true
            result.KeepAlive = false;
            return result;
        }
        /// <summary>
        /// 过滤表
        /// </summary>
        /// <param name="dt">需要过滤的表</param>
        /// <param name="columName">需要取出的列名称的集合</param>
        /// <param name="sqlStr">过滤御语句</param>
        /// <returns></returns>
        public static DataTable dtFilter(DataTable dt, string[] columName, string sqlStr)
        {
            DataTable dt1 = new DataTable();
            try
            {
                DataRow[] drArr = dt.Select(sqlStr);//查询
                DataTable dtNew = dt.Clone();
                for (int i = 0; i < drArr.Length; i++)
                {
                    dtNew.ImportRow(drArr[i]);
                }
                 dt1 = dtNew.DefaultView.ToTable(true, columName);
            }
            catch { return dt1; }
            return dt1;
        }

        /// <summary> 
        /// 执行DOS命令，返回DOS命令的输出 
        /// </summary> 
        /// <param name="dosCommand">dos命令</param> 
        /// <param name="milliseconds">等待命令执行的时间（单位：毫秒）， 
        /// 如果设定为0，则无限等待</param> 
        /// <returns>返回DOS命令的输出</returns> 
        public static string runexe(string command, int seconds)
        {
            string output = ""; //输出字符串 
            if (command != null && !command.Equals(""))
            {
                Process process = new Process();//创建进程对象 
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = "cmd.exe";//设定需要执行的命令 
                startInfo.Arguments = "/C " + command;//“/C”表示执行完命令后马上退出 
                startInfo.UseShellExecute = false;//不使用系统外壳程序启动 
                startInfo.RedirectStandardInput = false;//不重定向输入 
                startInfo.RedirectStandardOutput = true; //重定向输出 
                startInfo.CreateNoWindow = true;//不创建窗口 
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())//开始进程 
                    {
                        if (seconds == 0)
                        {
                            process.WaitForExit();//这里无限等待进程结束 
                        }
                        else
                        {
                            process.WaitForExit(seconds); //等待进程结束，等待时间为指定的毫秒 
                        }
                        output = process.StandardOutput.ReadToEnd();//读取进程的输出 
                    }
                }
                catch
                {
                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }
            return output;
        }

        public static string RandomStr()//15位长度
        {
            return RandomStr(15);
        }

        public static string RandomStr(int length)//指定长度
        {
            Random rd = new Random();
            string str = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            for (int i = 0; i < length; i++)
                result += str[rd.Next(0, str.Length)];
            return result;
        }

        ///
        /// 本地全路径文件名
        /// 服务器文件名(含以soft为根的路径)
        ///
        public static int HttpDownloadFile(string LocalFileName, string RemoteFileName, ref long finishedsize)
        {
            int flag = -1;
            //打开上次下载的文件
            //long SPosition = 0;
            //实例化流对象
            FileStream FStream;
            //判断要下载的文件夹是否存在
            if (File.Exists(LocalFileName))
            {
                File.Delete(LocalFileName);
                //打开要下载的文件
                //FStream = File.OpenWrite(LocalFileName);
                //获取已经下载的长度
                //SPosition = FStream.Length;
                //FStream.Seek(SPosition, SeekOrigin.Current);
            }
            else
            {
                //文件不保存创建一个文件
                //FStream = new FileStream(LocalFileName, FileMode.Create);
                ///SPosition = 0;
            }
            FStream = new FileStream(LocalFileName, FileMode.Create);
            try
            {
                //打开网络连接
                HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create("http://www.utjiaoyu.com/UTSOFT/" + RemoteFileName);
                //if (SPosition > 0)
                //    myRequest.AddRange((int)SPosition);             //设置Range值
                //向服务器请求,获得服务器的回应数据流
                Stream myStream = myRequest.GetResponse().GetResponseStream();
                //定义一个字节数据
                byte[] btContent = new byte[512];
                int intSize = 0;
                intSize = myStream.Read(btContent, 0, 512);
                finishedsize = intSize;
                while (intSize > 0)
                {
                    FStream.Write(btContent, 0, intSize);
                    intSize = myStream.Read(btContent, 0, 512);
                    finishedsize += intSize;
                }
                //关闭流
                FStream.Close();
                myStream.Close();
                flag = 0;        //返回true下载成功
            }
            catch (Exception ex)
            {
                FStream.Close();
                flag = -1;       //返回false下载失败
                frmUTSOFTMAIN.ErrMsg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// Http上传文件
        /// </summary>
        public static void HttpUploadFile(string localFile, string uploadUrl)
        {
            //System.IO.FileInfo f = new FileInfo(localFile);

            uploadUrl = "http://www.utjiaoyu.com/soft/" + uploadUrl;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uploadUrl);
            //req.Credentials = new NetworkCredential("utcrm", "1qazse4");//用户名,密码
            //req.ContentLength = f.Length;
            //req.ContentType = "text/html";
            req.PreAuthenticate = true;
            req.Method = WebRequestMethods.Http.Put;
            req.AllowWriteStreamBuffering = true;

            // Retrieve request stream 
            Stream reqStream = req.GetRequestStream();

            // Open the local file
            FileStream rdr = new FileStream(localFile, FileMode.Open);

            // Allocate byte buffer to hold file contents
            byte[] inData = new byte[4096];

            // loop through the local file reading each data block
            //  and writing to the request stream buffer
            int bytesRead = rdr.Read(inData, 0, inData.Length);
            while (bytesRead > 0)
            {
                reqStream.Write(inData, 0, bytesRead);
                bytesRead = rdr.Read(inData, 0, inData.Length);
            }

            rdr.Close();
            reqStream.Close();

            req.GetResponse();
        }

        public static int Download(string filePath, string fileName,ref long lengthing)
        {
            string localfilename = fileName;

            if (fileName.Contains("\\"))
            {
                int pos = fileName.LastIndexOf("\\");
                localfilename = fileName.Substring(pos+1);
            }
            return HttpDownloadFile(filePath + localfilename, fileName, ref lengthing);
        }

        public static int Download(string filePath, string fileName)
        {
            long tmp = 0;
            string localfilename = fileName;

            if (fileName.Contains("\\"))
            {
                int pos = fileName.LastIndexOf("\\");
                localfilename = fileName.Substring(pos + 1);
            }
            return HttpDownloadFile(filePath + localfilename, fileName, ref tmp);
        }

        public static int ftpDownload(string filePath, string fileName,ref long lengthing)
        {
            string ServerDir = "";
            string ServerFile = "";
            int result = 0;
            if (fileName.Contains("\\"))
            {
                ServerDir = fileName.Substring(0, fileName.LastIndexOf("\\") + 1);
                ServerFile = fileName.Replace(ServerDir, "");
            }
            else
            {
                ServerDir = "";
                ServerFile = fileName;
            }

            FtpWebRequest reqFTP =null;
            FtpWebResponse response=null;
            Stream ftpStream=null;
            FileStream outputStream=null;
            try
            {
                outputStream = new FileStream(filePath + "\\" + ServerFile, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + ServerDir + ServerFile));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                long totalDownloadedByte = 0;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                    totalDownloadedByte += readCount;
                    lengthing = totalDownloadedByte;
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (File.Exists(filePath + "\\" + ServerFile))
                        File.Delete(filePath + "\\" + ServerFile);
                }
                catch { }
                MessageBox.Show(ex.Message);
                result = - 1;
            }
            finally
            {
                reqFTP.Abort();
                if (ftpStream != null)
                {
                    ftpStream.Close();
                    ftpStream.Dispose();
                }
                if(response != null)
                    response.Close();
                if (outputStream != null)
                {
                    outputStream.Close();
                    outputStream.Dispose();
                }
            }
            return result;
        }

        public static int ftpDownload(string filePath, string fileName)
        {
            string ServerDir = "";
            string ServerFile = "";
            int result = 0;
            if (fileName.Contains("\\"))
            {
                ServerDir = fileName.Substring(0, fileName.LastIndexOf("\\") + 1);
                ServerFile = fileName.Replace(ServerDir, "");
            }
            else
            {
                ServerDir = "";
                ServerFile = fileName;
            }

            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;
            FileStream outputStream = null;
            try
            {
                outputStream = new FileStream(filePath + "\\" + ServerFile, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + ServerDir + ServerFile));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    if (File.Exists(filePath + "\\" + ServerFile))
                        File.Delete(filePath + "\\" + ServerFile);
                }
                catch { }
                MessageBox.Show(ex.Message);
                result = -1;
            }
            finally
            {
                reqFTP.Abort();
                if (ftpStream != null)
                {
                    ftpStream.Close();
                    ftpStream.Dispose();
                }
                if (response != null)
                    response.Close();
                if (outputStream != null)
                {
                    outputStream.Close();
                    outputStream.Dispose();
                }
            }
            return result;
        }

        public static int DownloadPhoto(string filePath, string fileName)//下载照片
        {
            long tmp = 0;
            return HttpDownloadFile(filePath + fileName, "\\Photos\\" + fileName, ref tmp);//下载照片
        }

        public static int ftpDownloadPhoto(string filePath, string fileName)//下载照片
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI +"\\Photos\\"+ fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public static int DownloadNote(string filePath, string fileName)//下载学员笔记附件
        {
            long tmp = 0;
            return HttpDownloadFile(filePath + fileName, "\\StudyNotes\\" + fileName, ref tmp);//下载照片
        }

        public static int ftpDownloadNote(string filePath, string fileName)//下载学员笔记附件
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath , FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + "\\StudyNotes\\" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public static int Upload1(string filename, string targetPath) //http上传文件
        {
            int flag = 0;
            int pos = targetPath.LastIndexOf("/");
            int pos1=filename.LastIndexOf("\\");
            string file=filename.Substring(0,pos1+1)+targetPath.Substring(pos+1);
            string url = "http://www.utjiaoyu.com/soft/"+ targetPath.Substring(0,pos+1);
            File.Copy(filename, file);
            HttpUploadFile(file,url);
            return flag ;
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileinfo">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        /// <param name="hostname">ftp地址</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static int Upload(string filename, string targetFileName)
        {
            int flag = 0;
            FileInfo fileinfo = new FileInfo(filename);
            string URI = ftpURI + targetFileName;
            System.Net.FtpWebRequest ftp = GetRequest(URI, ftpUserID, ftpPassword);
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //指定文件传输的数据类型
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            //告诉ftp文件大小
            ftp.ContentLength = fileinfo.Length;
            //缓冲大小设置为2KB
            const int BufferSize = 2048;
            byte[] content = new byte[BufferSize];
            int dataRead;

            //打开一个文件流 (System.IO.FileStream) 去读上传的文件
            try
            {
                using (FileStream fs = fileinfo.OpenRead())
                {
                    try
                    {
                        //把上传的文件写入流
                        using (Stream rs = ftp.GetRequestStream())
                        {
                            do
                            {
                                //每次读文件流的2KB
                                dataRead = fs.Read(content, 0, BufferSize);
                                rs.Write(content, 0, dataRead);
                            } while (!(dataRead < BufferSize));
                            rs.Close();
                        }

                    }
                    catch { flag = 1; }
                    finally
                    {
                        fs.Close();
                    }
                }
            }
            catch { flag = 1; }

            try
            {
                ftp.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message);
                flag = 1;
            }
            finally
            {
            }
            return flag;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileinfo">需要上传的文件</param>
        /// <param name="targetDir">目标路径</param>
        /// <param name="hostname">ftp地址</param>
        /// <param name="username">ftp用户名</param>
        /// <param name="password">ftp密码</param>
        public static int UploadImage(byte[] imageBytes, string targetFileName)
        {
            int flag = 0;
            string URI = ftpURI + targetFileName;

            System.Net.FtpWebRequest ftp = GetRequest(URI, ftpUserID, ftpPassword);
            ftp.Method = System.Net.WebRequestMethods.Ftp.UploadFile;
            //指定文件传输的数据类型
            ftp.UseBinary = true;
            ftp.UsePassive = true;

            Stream rs = ftp.GetRequestStream();
            try
            { rs.Write(imageBytes, 0, imageBytes.Length); }
            catch 
            { flag = 1; }
            finally
            { rs.Close(); }

            try
            {
                ftp.GetResponse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                flag = 1;
            }
            finally
            {
            }
            return flag;
        }
        public static Stream imgToPic(string fileName)//将图片显示到picturebox
        {
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpChatPhotos + fileName));
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse respFtp = (FtpWebResponse)reqFtp.GetResponse();
                Stream stream = respFtp.GetResponseStream();
                return stream;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public static bool CheckFTPFile(string fileName)//判断文件是否存在
        {
            FtpWebRequest reqFTP;

            // dirName = name of the directory to create.
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpChatPhotos));
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            Encoding encode = Encoding.GetEncoding("GB2312");//转换中文
            StreamReader ftpStream = new StreamReader(response.GetResponseStream(), encode);

            List<string> files = new List<string>();
            string line = ftpStream.ReadLine();
            while (line != null)
            {
                files.Add(line);
                line = ftpStream.ReadLine();
            }

            ftpStream.Close();
            response.Close();

            return files.Contains(fileName);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static bool fileDelete(string ftpPath, string ftpName)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            StreamReader streamReader = null;
            try
            {
                string uri = ftpURI + ftpPath + ftpName;
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.KeepAlive = false;
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                long size = ftpWebResponse.ContentLength;
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                streamReader = new StreamReader(ftpResponseStream);
                string result = String.Empty;
                result = streamReader.ReadToEnd();

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }
                if (ftpWebResponse != null)
                {
                    ftpWebResponse.Close();
                }
            }
            return success;
        }


        public static object _fileobject = new object();
        public static void ListToXmlFile(Type type, Object obj, string filename)
        {
            lock (_fileobject)
            {
                tools.ListToXmlFile1(type, obj, filename);
            }
        }
        public static void ListToXmlFile1(Type type, Object obj, string filename)
        {
            //Type type = typeof(object);
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.Write(str);
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        public static object XmlFileToList(Type type, string filename)
        {
            StreamReader fsr = new StreamReader(filename, System.Text.Encoding.UTF8);
            string xml = fsr.ReadToEnd();
            fsr.Close();

            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch
            {

                return null;
            }
        }

        public static void Writelog(string str)//log写入文件
        {
            string filename = "C:\\Users\\Administrator\\Desktop\\debug_log.txt";
            FileStream fs = new FileStream(filename, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(str);
            sw.Flush();
            sw.Close();
            fs.Close();
        }

        //延时函数
        public static void Delay(int DelayTime = 100)
        {
            int time = Environment.TickCount;
            while (true)
            {
                if (Environment.TickCount - time >= DelayTime)
                {
                    break;
                }
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }
        /// <summary>
        /// 导出数据到excel
        /// </summary>
        /// <param name="table"></param>
        /// <param name="file"></param>
        public static void dataTableToCsv(DataTable table, string file)
        {
            string title = "";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            //FileStream fs1 = File.Open(file, FileMode.Open, FileAccess.Read);
            StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                title += table.Columns[i].ColumnName + "\t"; //栏位：自动跳到下一单元格
            }
            if (title != "")
            {
                title = title.Substring(0, title.Length - 1) + "\n";
                sw.Write(title);
                foreach (DataRow row in table.Rows)
                {
                    string line = "";
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        line += row[i].ToString().Trim() + "\t"; //内容：自动跳到下一单元格
                    }
                    line = line.Substring(0, line.Length - 1) + "\n";
                    sw.Write(line);
                }
            }
            sw.Close();
            fs.Close();
        }
        /// <summary>  
        /// Creating a Watermarked Photograph with GDI+ for .NET  
        /// </summary>  
        /// <param name="rSrcImgPath">原始图片的物理路径</param>  
        /// <param name="rMarkText">水印文字（不显示水印文字设为空串）</param>  
        /// <param name="rDstImgPath">输出合成后的图片的物理路径</param>  
        public static void BuildTextWatermark(string rSrcImgPath, string rMarkText, string rDstImgPath)
        {
            //以下（代码）从一个指定文件创建了一个Image 对象，然后为它的 Width 和 Height定义变量。  
            //这些长度待会被用来建立一个以24 bits 每像素的格式作为颜色数据的Bitmap对象。  
            Image imgPhoto = Image.FromFile(rSrcImgPath);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(72, 72);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //这个代码以100%它的原始大小绘制imgPhoto 到Graphics 对象的（x=0,y=0）位置。  
            //以后所有的绘图都将发生在原来照片的顶部。  
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.DrawImage(
                 imgPhoto,
                 new Rectangle(0, 0, phWidth, phHeight),
                 0,
                 0,
                 phWidth,
                 phHeight,
                 GraphicsUnit.Pixel);
            //为了最大化版权信息的大小，我们将测试7种不同的字体大小来决定我们能为我们的照片宽度使用的可能的最大大小。  
            //为了有效地完成这个，我们将定义一个整型数组，接着遍历这些整型值测量不同大小的版权字符串。  
            //一旦我们决定了可能的最大大小，我们就退出循环，绘制文本  
            //对角线的长度
            int duijiaoxian = (int)( Math.Sqrt(phHeight*phHeight + phWidth*phWidth) );
            //MessageBox.Show("h=" + phHeight.ToString() + "   w=" + phWidth.ToString() + "    duijiaoxian=" + duijiaoxian.ToString());
            int[] sizes = new int[] { 72,56,48,36,28,26,22,20,18,16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < sizes.Length; i++)
            {
                crFont = new Font("arial", sizes[i],
                      FontStyle.Bold);
                crSize = grPhoto.MeasureString(rMarkText,
                      crFont);
                if ((ushort)crSize.Width < (ushort)duijiaoxian)
                    break;
            }
            
            //因为所有的照片都有各种各样的高度，所以就决定了从图象底部开始的5%的位置开始。  
            //使用rMarkText字符串的高度来决定绘制字符串合适的Y坐标轴。  
            //通过计算图像的中心来决定X轴，然后定义一个StringFormat 对象，设置StringAlignment 为Center。  
            float yPosFromBottom = - (crSize.Height / 2);
            float xCenterOfImg = (phWidth / 2);
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            //现在我们已经有了所有所需的位置坐标来使用60%黑色的一个Color(alpha值153)创建一个SolidBrush 。  
            //在偏离右边1像素，底部1像素的合适位置绘制版权字符串。  
            //这段偏离将用来创建阴影效果。使用Brush重复这样一个过程，在前一个绘制的文本顶部绘制同样的文本。
            float angle1 = (float)(Math.Atan(  (double)phHeight/(double)phWidth )/Math.PI*180);
            
            grPhoto.RotateTransform( angle1);
            float x1 = duijiaoxian/2 ;
            float y1 = yPosFromBottom; 
            SolidBrush semiTransBrush2 =new SolidBrush(Color.FromArgb(50, 0, 0, 0));
            grPhoto.DrawString(rMarkText,
                 crFont,
                 semiTransBrush2,
                 new PointF(x1 + 1, y1 + 1),
                 StrFormat);
            SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
            grPhoto.DrawString(rMarkText,
                 crFont,
                 semiTransBrush,
                 new PointF(x1, y1),
                 StrFormat);
            //根据前面修改后的照片创建一个Bitmap。把这个Bitmap载入到一个新的Graphic对象。  
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(
                 imgPhoto.HorizontalResolution,
                 imgPhoto.VerticalResolution);
            Graphics grWatermark =
                 Graphics.FromImage(bmWatermark);

            //最后的步骤将是使用新的Bitmap取代原来的Image。 销毁Graphic对象，然后把Image 保存到文件系统。  
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();
            imgPhoto.Save(rDstImgPath, ImageFormat.Jpeg);
            imgPhoto.Dispose();
        }  

         /// <summary>  
        /// Creating a Watermarked Photograph with GDI+ for .NET  
        /// </summary>  
        /// <param name="rSrcImgPath">原始图片的物理路径</param>  
        /// <param name="rMarkImgPath">水印图片的物理路径</param>  
        /// <param name="rMarkText">水印文字（不显示水印文字设为空串）</param>  
        /// <param name="rDstImgPath">输出合成后的图片的物理路径</param>  
        /// @整理: anyrock@mending.cn  
        //public static void BuildWatermark(string rSrcImgPath, string rMarkImgPath, string rMarkText, string rDstImgPath)
        public static void BuildWatermark(string rSrcImgPath, Image rMarkImgPath, string rMarkText, string rDstImgPath)
        {
            //以下（代码）从一个指定文件创建了一个Image 对象，然后为它的 Width 和 Height定义变量。  
            //这些长度待会被用来建立一个以24 bits 每像素的格式作为颜色数据的Bitmap对象。  
            Image imgPhoto = Image.FromFile(rSrcImgPath);
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(72, 72);
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //这个代码载入水印图片，水印图片已经被保存为一个BMP文件，以绿色(A=0,R=0,G=255,B=0)作为背景颜色。  
            //再一次，会为它的Width 和Height定义一个变量。  
            Image imgWatermark = new Bitmap(rMarkImgPath);
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;
            //这个代码以100%它的原始大小绘制imgPhoto 到Graphics 对象的（x=0,y=0）位置。  
            //以后所有的绘图都将发生在原来照片的顶部。  
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            grPhoto.DrawImage(
                 imgPhoto,
                 new Rectangle(0, 0, phWidth, phHeight),
                 0,
                 0,
                 phWidth,
                 phHeight,
                 GraphicsUnit.Pixel);
            //为了最大化版权信息的大小，我们将测试7种不同的字体大小来决定我们能为我们的照片宽度使用的可能的最大大小。  
            //为了有效地完成这个，我们将定义一个整型数组，接着遍历这些整型值测量不同大小的版权字符串。  
            //一旦我们决定了可能的最大大小，我们就退出循环，绘制文本  
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };
            Font crFont = null;
            SizeF crSize = new SizeF();
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i],
                      FontStyle.Bold);
                crSize = grPhoto.MeasureString(rMarkText,
                      crFont);
                if ((ushort)crSize.Width < (ushort)phWidth)
                    break;
            }
            //因为所有的照片都有各种各样的高度，所以就决定了从图象底部开始的5%的位置开始。  
            //使用rMarkText字符串的高度来决定绘制字符串合适的Y坐标轴。  
            //通过计算图像的中心来决定X轴，然后定义一个StringFormat 对象，设置StringAlignment 为Center。  
            int yPixlesFromBottom = (int)(phHeight * .05);
            float yPosFromBottom = ((phHeight -
                 yPixlesFromBottom) - (crSize.Height / 2));
            float xCenterOfImg = (phWidth / 2);
            StringFormat StrFormat = new StringFormat();
            StrFormat.Alignment = StringAlignment.Center;
            //现在我们已经有了所有所需的位置坐标来使用60%黑色的一个Color(alpha值153)创建一个SolidBrush 。  
            //在偏离右边1像素，底部1像素的合适位置绘制版权字符串。  
            //这段偏离将用来创建阴影效果。使用Brush重复这样一个过程，在前一个绘制的文本顶部绘制同样的文本。  
            SolidBrush semiTransBrush2 =
                 new SolidBrush(Color.FromArgb(153, 0, 0, 0));
            grPhoto.DrawString(rMarkText,
                 crFont,
                 semiTransBrush2,
                 new PointF(xCenterOfImg + 1, yPosFromBottom + 1),
                 StrFormat);
            SolidBrush semiTransBrush = new SolidBrush(
                 Color.FromArgb(153, 255, 255, 255));
            grPhoto.DrawString(rMarkText,
                 crFont,
                 semiTransBrush,
                 new PointF(xCenterOfImg, yPosFromBottom),
                 StrFormat);
            //根据前面修改后的照片创建一个Bitmap。把这个Bitmap载入到一个新的Graphic对象。  
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(
                 imgPhoto.HorizontalResolution,
                 imgPhoto.VerticalResolution);
            Graphics grWatermark =
                 Graphics.FromImage(bmWatermark);
            //通过定义一个ImageAttributes 对象并设置它的两个属性，我们就是实现了两个颜色的处理，以达到半透明的水印效果。  
            //处理水印图象的第一步是把背景图案变为透明的(Alpha=0, R=0, G=0, B=0)。我们使用一个Colormap 和定义一个RemapTable来做这个。  
            //就像前面展示的，我的水印被定义为100%绿色背景，我们将搜到这个颜色，然后取代为透明。  
            ImageAttributes imageAttributes =
                 new ImageAttributes();
            ColorMap colorMap = new ColorMap();
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };
            //第二个颜色处理用来改变水印的不透明性。  
            //通过应用包含提供了坐标的RGBA空间的5x5矩阵来做这个。  
            //通过设定第三行、第三列为0.3f我们就达到了一个不透明的水平。结果是水印会轻微地显示在图象底下一些。  
            imageAttributes.SetRemapTable(remapTable,
                 ColorAdjustType.Bitmap);
            float[][] colorMatrixElements = {   
                                                     new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},  
                                                     new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},  
                                                     new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},  
                                                     new float[] {0.0f,  0.0f,  0.0f,  0.3f, 0.0f},  
                                                     new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}  
                                                };
            ColorMatrix wmColorMatrix = new
                 ColorMatrix(colorMatrixElements);
            imageAttributes.SetColorMatrix(wmColorMatrix,
                 ColorMatrixFlag.Default,
                 ColorAdjustType.Bitmap);
            //随着两个颜色处理加入到imageAttributes 对象，我们现在就能在照片右手边上绘制水印了。  
            //我们会偏离10像素到底部，10像素到左边。  
            int markWidth;
            int markHeight;
            //mark比原来的图宽  
            if (phWidth <= wmWidth)
            {
                markWidth = phWidth - 10;
                markHeight = (markWidth * wmHeight) / wmWidth;
            }
            else if (phHeight <= wmHeight)
            {
                markHeight = phHeight - 10;
                markWidth = (markHeight * wmWidth) / wmHeight;
            }
            else
            {
                markWidth = wmWidth;
                markHeight = wmHeight;
            }
            int xPosOfWm = ((phWidth - markWidth) - 10);
            int yPosOfWm = 10;
            grWatermark.DrawImage(imgWatermark,
                 new Rectangle(xPosOfWm, yPosOfWm, markWidth,
                 markHeight),
                 0,
                 0,
                 wmWidth,
                 wmHeight,
                 GraphicsUnit.Pixel,
                 imageAttributes);
            //最后的步骤将是使用新的Bitmap取代原来的Image。 销毁两个Graphic对象，然后把Image 保存到文件系统。  
            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();
            imgPhoto.Save(rDstImgPath, ImageFormat.Jpeg);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
        }

        public static string MD5(string password)
        {
            byte[] textBytes = System.Text.Encoding.Default.GetBytes(password);
            try
            {
                System.Security.Cryptography.MD5CryptoServiceProvider cryptHandler;
                cryptHandler = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash = cryptHandler.ComputeHash(textBytes);
                string ret = "";
                foreach (byte a in hash)
                {
                    if (a < 16)
                        ret += "0" + a.ToString("x");
                    else
                        ret += a.ToString("x");
                }
                return ret;
            }
            catch
            {
                throw;
            }
        }

        //获取excel表格数据至dataSet
        static public DataTable GetDataSetFromExcel(string sheelname,TextBoxX tbx,string colLen)
        {
            DataTable dt=new DataTable();
            DataSet excelDs = new DataSet();//用于读取excel
            DataSet ds = new DataSet();

            OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "*.xls|*.xls";
            ofd.Filter = "Microsoft Excel files(*.xls)|*.xls;*.xlsx";//过滤一下，只要表格格式的
            string fullPath = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fullPath = ofd.FileName;
                tbx.Text = ofd.FileName;
            }
            else
                return dt;
            try 
            {
                string tableNames = GetExcelTableNames(fullPath);//获取全部表名
                string[] tbNames = tableNames.Split('|');

                if (sheelname == "")//默认选择第一张表
                {
                    string TSql = "SELECT  * FROM [" + tbNames[1] + colLen+"]";
                    excelDs = ExcelToDataSet(fullPath, TSql);
                }
                else 
                {
                    foreach (string tn in tbNames)//获取含有表名的表
                    {
                        if (tn.Contains(sheelname))
                        {
                            //tmp += tn + "|";
                            //设置T_Sql
                            string TSql = "SELECT  * FROM [" + tn + colLen+"]";
                            //读取数据
                            excelDs = ExcelToDataSet(fullPath, TSql);
                            break;
                        }
                    }
                }
                dt = excelDs.Tables[0];
                dt = excelDs.Tables[0];
                return removeEmpty(dt);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return dt;
            }
            
        }
        //去掉空行
        public static DataTable removeEmpty(DataTable dt)
        {
            List<DataRow> removelist = new List<DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool rowdataisnull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {

                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {

                        rowdataisnull = false;
                    }

                }
                if (rowdataisnull)
                {
                    removelist.Add(dt.Rows[i]);
                }

            }
            for (int i = 0; i < removelist.Count; i++)
            {
                dt.Rows.Remove(removelist[i]);
            }
            return dt;
        }
        /// <summary>
        /// 动态取Excel表名
        /// </summary>
        /// <param name="fullPath">文件路径</param>
        /// <returns></returns>
        static public string GetExcelTableNames(string fullPath)
        {
            string tableNames = null;
            if (File.Exists(fullPath))
            {
                using (OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet." +
                "OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + fullPath))
                {
                    //此连接只能操作Excel2007之前(.xls)文件
                    conn.Open();
                    for (int i = 0; i < conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows.Count; i++)
                    {
                        tableNames += conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[i][2].ToString().Trim() + "|";
                    }
                }
            }

            return tableNames;
        }
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <param name="TSql">TSql</param>
        /// <returns>DataSet</returns>
        static public DataSet ExcelToDataSet(string filename, string TSql)
        {
            DataSet excelDs = new DataSet();
            //string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;IMEX=1;data source=" + filename;
            string strCon = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + filename + "; Extended Properties=\"Excel 8.0; HDR=YES; IMEX=1;\"";
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = TSql;
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            myCommand.Fill(excelDs);
            myConn.Close();
            return excelDs;
        }
        /// <summary>
        /// 实现数据的四舍五入法
        /// </summary>
        /// <param name="v">要进行处理的数据</param>
        /// <param name="x">保留的小数位数</param>
        /// <returns>四舍五入后的结果</returns>
        static public int doubleToInt(string vStr)
        {
            double v = double.Parse(vStr);
            //如果是负数
            if (v < 0)
            {
                v = v-0.5;
            }
            else
            {
                v = v + 0.5;
            }

            return (int)v;
        }
        /// <summary>
        /// 过滤不安全的字符串
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string FilteSQLStr(string Str)
        {
            Str = Str.Replace("'", "");
            Str = Str.Replace("\"", "");
            Str = Str.Replace("&", "&amp");
            Str = Str.Replace("<", "&lt");
            Str = Str.Replace(">", "&gt");
            return Str;
        }

        public static string SumValue(DataGridViewX dgvx,int index)
        {
            double sumVal = 0;
            try {
                for (int i = 0; i < dgvx.Rows.Count; i++)
                {
                    try { sumVal += double.Parse(dgvx.Rows[i].Cells[index].Value.ToString()); }
                    catch { sumVal += 0; }
                }
                return "合计金额 ： " + doubleToInt(sumVal.ToString()).ToString() + " 元";
            }
            catch { return "合计金额 ： " + doubleToInt(sumVal.ToString()).ToString() + " 元"; }
        }

        public static string ReadClientVersion()
        {
            string versionStr = "";
            XmlDocument _XmlClient = new XmlDocument();
            _XmlClient.Load(Application.StartupPath + @"\UpdateSetting.Client.xml");
            if (_XmlClient != null)
            {
                XmlElement element = _XmlClient["Main"]["Version"];
                if (element != null)
                {
                    versionStr += element.Attributes["value"].Value;
                }
                element = null;
            }
            return versionStr;
        }
    }
}
