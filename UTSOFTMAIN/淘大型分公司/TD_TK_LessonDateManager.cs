using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UTSOFTMAIN.淘大型分公司
{
    public partial class TD_TK_LessonDateManager : Form
    {
        public TD_TK_LessonDateManager()
        {
            InitializeComponent();
        }
        public static DataTable allLessonName = new DataTable();
        private void TD_TK_LessonDateManager_Load(object sender, EventArgs e)
        {
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput3.Value = System.DateTime.Now;

            string sqlLessonName = string.Format("select * from TK_LessonManage",
                frmUTSOFTMAIN.CompanyNames);// where CompanyNames = '{0}'
            allLessonName = DBHelper.ExecuteQuery(sqlLessonName);

            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
        }

        private void dataGridViewX1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sql1 = "";
            if (e.ColumnIndex == 2)
            {
                sql1 = string.Format("select b.LessonName,a.LessonTime,a.StudentName from TK_LessonDetail a left join "+
                    " TK_LessonManage b on a.ForID = b.id" +
                    " where a.ForID = {0} order by a.id,b.id desc",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            else if (e.ColumnIndex == 4) 
            {
                sql1 = string.Format("select b.LessonName,a.LessonTime,a.StudentName from TK_LessonDetail a left join " +
                    " TK_LessonManage b on a.ForID = b.id" +
                    " where a.ForID = {0} and a.LessonTime = '{1}' order by a.id,b.id desc",
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[0].Value.ToString(),
                    this.dataGridViewX1.Rows[e.RowIndex].Cells[3].Value.ToString());
            }
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            TD_TK_StudentMessageShow ttsmls = new TD_TK_StudentMessageShow();
            ttsmls.dtSource = dt1;
            ttsmls.Show();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select a.id,a.LessonName,a.StudentCount,b.LessonTime,count(*) as personcount from TK_LessonManage "+
                " a left join TK_LessonDetail b on a.id = b.ForID where LessonTime between '{1}' and '{2}'"+
                " and LessonName like '%{3}%'"+
                " group by a.LessonName,a.StudentCount,b.LessonTime, a.id,a.LessonOrder" +
                " order by a.LessonOrder asc",//CompanyNames = '{0}' and 
                frmUTSOFTMAIN.CompanyNames,
                this.dateTimeInput2.Value.ToShortDateString(),
                this.dateTimeInput3.Value.ToShortDateString(),
                this.comboBoxEx1.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                this.dataGridViewX1.DataSource = dt1;
            }
            else 
            {
                MessageBox.Show("未查询到数据！");
            }
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string[] columNames = new string[] { "LessonName" };
            DataTable dtLessonName = tools.dtFilter(allLessonName, columNames, "");
            if (dtLessonName.Rows.Count == 0)
                return;
            this.comboBoxEx1.DataSource = dtLessonName;
            this.comboBoxEx1.ValueMember = "LessonName";
            this.comboBoxEx1.DisplayMember = "LessonName";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        //取出文件夹中的所有TXT的路径和名称
        private static List<string> list = new List<string>();
        private void GetTxt(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();

            FileAttributes fa;
            foreach (FileInfo item in files)
            {
                //if (item.Extension == ".txt")
                //{
                //遍历时忽略隐藏文件
                fa = item.Attributes & FileAttributes.Hidden;
                if (fa != FileAttributes.Hidden)
                {
                    //MessageBox.Show(item.FullName);
                    //所有的记事本全路径全部放入list集合中
                    list.Add(item.FullName);
                }
                //}
            }
            DirectoryInfo[] dirs = dir.GetDirectories();   //获取子目录
            foreach (DirectoryInfo item in dirs)
            {
                GetTxt(item);
            }
        }
        //读取路径集合中的TXT中的QQ号码
        private Dictionary<string[], List<string>> readTxt(List<string> list)
        {
            this.textBoxX1.Text = "";
            this.textBoxX1.Text += "读取txt文件失败：" + "\r\n";
            Dictionary<string[], List<string>> fileQQDictionary = new Dictionary<string[], List<string>>();
            //循环list里所有记事本文件
            foreach (string item in list)
            {
                string filenameTemp = System.IO.Path.GetFileName(item);
                string filename = filenameTemp.Replace(".txt", "");
                string[] filenameitem = filename.Split(new char[] { '-' });

                filenameitem[0] = filenameitem[0].Substring(0, 8);
                filenameitem[0] = DateTime.ParseExact(filenameitem[0], "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture).ToShortDateString();

                string idSqlStr = string.Format("select top 1 id from TK_LessonManage where LessonName = '{0}'", filenameitem[1]);
                DataTable dtID = DBHelper.ExecuteQuery(idSqlStr);
                if (dtID.Rows.Count != 1)
                {
                    this.textBoxX1.Text += "      " + item + "\r\n";
                    continue;
                }
                string id = dtID.Rows[0][0].ToString();
                string[] idtime = new string[2];
                idtime[0] = id;
                idtime[1] = filenameitem[0];

                List<string> qqList = new List<string>();
                FileStream fs = new FileStream(item, FileMode.Open);
                StreamReader reader = new StreamReader(fs, Encoding.Default);
                try
                {
                    //循环读取记事本
                    while (true)
                    {
                        string str = reader.ReadLine();
                        if (str == null)
                            break;
                        else if (str.Trim() == "")
                            continue;
                        else
                            //这里就是每行数据了  你可以进行处理 取出符合要求的行
                            qqList.Add(str);
                    }
                    if (qqList.Count != 0)
                    {
                        //将读取到的记录放入字典
                        fileQQDictionary.Add(idtime, qqList);
                        //读取完毕删除文件
                        if (File.Exists(item))
                        {
                            FileInfo fi = new FileInfo(item);

                            if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                                fi.Attributes = FileAttributes.Normal;
                            File.Delete(item);
                        }
                    }
                }
                catch { this.textBoxX1.Text += "      " + item + "\r\n"; }
            }
            return fileQQDictionary;
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要导入吗?", "导入提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            list.Clear();
            DirectoryInfo info = new DirectoryInfo(this.textBoxX2.Text);
            GetTxt(info);
            Dictionary<string[], List<string>> allstudent = readTxt(list);
            int resultNum = 0;
            this.progressBarX1.Minimum = 0;
            this.progressBarX1.Maximum = allstudent.Count;
            this.progressBarX1.Value = 0;

            string sql1 = "";
            int resultNum1 = 0;
            foreach(var item in allstudent)
            {
                string[] key = item.Key;
                List<string> values = item.Value;
                foreach(var value in values)
                {
                    sql1 += string.Format("insert into TK_LessonDetail values ({0},'{1}','{2}',getdate(),'{3}')",
                        key[0], key[1], value, frmUTSOFTMAIN.StaffID);
                    resultNum1++;
                    if (resultNum1 == 100 ) 
                    {
                        resultNum += DBHelper.ExecuteUpdate(sql1);
                        sql1 = "";
                        resultNum1 = 0;
                    }
                }
                this.progressBarX1.Value += 1;
            }
            resultNum += DBHelper.ExecuteUpdate(sql1);

            this.progressBarX1.Value = allstudent.Count;
            MessageBox.Show("成功导入：" + resultNum + "条！");
            string sqlupdateLessonManage = string.Format("update a set StudentCount = b.studentnum from TK_LessonManage a left join "+
                " (select ForID,count(*) as studentnum from TK_LessonDetail group by ForID) b on a.id = b.ForID");
            DBHelper.ExecuteUpdate(sqlupdateLessonManage);
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                this.textBoxX2.Text = "";
                this.textBoxX2.Text = folder.SelectedPath;
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            if (MessageBox.Show("你确定要删除选中的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = "";
            try
            {
                for (int i = 0; i < this.dataGridViewX1.SelectedRows.Count; i++)
                {
                    sql1 += string.Format(" delete from TK_LessonDetail where ForID = {0} and LessonTime = '{1}' ",
                        this.dataGridViewX1.SelectedRows[i].Cells[0].Value.ToString(),
                        this.dataGridViewX1.SelectedRows[i].Cells[3].Value.ToString());
                }
            }
            catch
            {
                MessageBox.Show("数据格式输入错误");
                return;
            }
            int resultNum1 = DBHelper.ExecuteUpdate(sql1);
            if (resultNum1 > 0)
            {
                MessageBox.Show("删除成功");
                for (int i = this.dataGridViewX1.SelectedRows.Count; i > 0; i--)
                    dataGridViewX1.Rows.RemoveAt(dataGridViewX1.SelectedRows[i - 1].Index);
            }
            else
            {
                MessageBox.Show("删除失败，请检查输入数据！");
            }
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select b.LessonName,a.LessonTime,a.StudentName from TK_LessonDetail a left join " +
                    " TK_LessonManage b on a.ForID = b.id" +
                    " where a.LessonTime = '{0}' order by a.id,b.id desc",
                    this.dateTimeInput2.Value.ToShortDateString());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            TD_TK_StudentMessageShow ttsmls = new TD_TK_StudentMessageShow();
            ttsmls.dtSource = dt1;
            ttsmls.Show();
        }
    }
}
