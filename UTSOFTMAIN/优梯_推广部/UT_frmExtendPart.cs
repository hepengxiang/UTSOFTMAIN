using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UTSOFTMAIN.优梯_推广部;

namespace UTSOFTMAIN
{
    public partial class UT_frmExtendPart : Form
    {

        public UT_frmExtendPart()
        {
            InitializeComponent();
        }
        private static string allMessage = "[UTCRMDB].[dbo].[CustomerInfo]";
        private static string detailMessage = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
        private static string lessonName = "";
        private void frmVIPServer_Load(object sender, EventArgs e)
        {
            this.comboBoxEx15.SelectedIndexChanged += new EventHandler(comboBoxEx15_SelectedIndexChanged);

            this.dateTimeInput1.Value = System.DateTime.Now.AddDays(-7);
            this.dateTimeInput2.Value = System.DateTime.Now;
            this.dateTimeInput5.Value = System.DateTime.Now.AddDays(-1);
            this.dateTimeInput6.Value = System.DateTime.Now;
            this.dateTimeInput7.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput8.Value = System.DateTime.Now;
            this.dateTimeInput9.Value = System.DateTime.Now.AddDays(1 - System.DateTime.Now.Day);
            this.dateTimeInput10.Value = System.DateTime.Now;
            //营销报表权限按钮显示
            frmUTSOFTMAIN.tablePowerBtnInit(this.flowLayoutPanel1);

            this.comboBoxEx4.SelectedIndex = 0;
        }
        ///////////////////////////////////////////数据分析/////////////////////////////////////////////////////////
        private void buttonX12_Click(object sender, EventArgs e)//分析数据
        {
            if (this.comboBoxEx4.Text == "内部资源")
            {
                allMessage = "[UTCRMDB].[dbo].[CustomerInfo]";
                detailMessage = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
            }
            else 
            {
                allMessage = "[UTCRMDB].[dbo].[ForeignCustomerInfo]";
                detailMessage = "[UTCRMDB].[dbo].[KeQQForeignRecord]";
            }
            try
            {
                string sql2 = string.Format("select count(*) as allEnter," +
                    " coalesce(sum(case when StudyNum is null then 1 else 0 end),0) as notEnter from " +
                    " (select a.QQ as qqNum, b.StudyNumber as StudyNum from Pub_VIPMessage a left join {2} b on a.qq = b.qq" +
                    " where a.Jointime between '{0}' and '{1}' and CompanyNames = '优梯' and charindex('报名',EnterType)>0) a",
                    this.dateTimeInput7.Value.ToShortDateString(),
                    this.dateTimeInput8.Value.ToShortDateString(), allMessage);
                DataTable dt2 = DBHelper.ExecuteQuery(sql2);
                if (dt2.Rows.Count == 0)
                    return;
                this.labelX34.Text = "总报名： " + dt2.Rows[0][0].ToString();
                this.labelX35.Text = "已抓取： " + (int.Parse(dt2.Rows[0][0].ToString()) - int.Parse(dt2.Rows[0][1].ToString())).ToString();
                this.labelX37.Text = "未抓取： " + dt2.Rows[0][1].ToString();
                string sql1 = "";

                if (this.textBoxX5.Text.Trim() != "")
                    lessonName = string.Format(" and charindex(''{0}'',SubjectName)>0 ", this.textBoxX5.Text.Trim());
                else
                    lessonName = "";
                if (this.comboBoxEx12.Text == "报名与学习次数")
                {
                    sql1 = getEnterWithStudyNum(this.dateTimeInput7.Value, this.dateTimeInput8.Value,
                        int.Parse(this.textBoxX14.Text.Trim()), int.Parse(this.textBoxX15.Text.Trim()),
                        int.Parse(this.textBoxX16.Text.Trim()), lessonName);
                }
                else if (this.comboBoxEx12.Text == "报名与学习时长")
                {
                    sql1 = getEnterWithStudyTime(this.dateTimeInput7.Value, this.dateTimeInput8.Value,
                        int.Parse(this.textBoxX14.Text.Trim()), int.Parse(this.textBoxX15.Text.Trim()),
                        int.Parse(this.textBoxX16.Text.Trim()), lessonName);
                }
                else if (this.comboBoxEx12.Text == "报名与订购时间")
                {
                    sql1 = getEnterWithBuyTime(this.dateTimeInput7.Value, this.dateTimeInput8.Value,
                        int.Parse(this.textBoxX14.Text.Trim()), int.Parse(this.textBoxX15.Text.Trim()),
                        int.Parse(this.textBoxX16.Text.Trim()), lessonName);
                }
                else if (this.comboBoxEx12.Text == "监控资源听课次数分布")
                {
                    sql1 = getCRMWithStudyNum(this.dateTimeInput7.Value, this.dateTimeInput8.Value,
                        int.Parse(this.textBoxX14.Text.Trim()), int.Parse(this.textBoxX15.Text.Trim()),
                        int.Parse(this.textBoxX16.Text.Trim()), lessonName);
                }
                else if (this.comboBoxEx12.Text == "监控资源听课时长分布")
                {
                    sql1 = getCRMWithStudyTime(this.dateTimeInput7.Value, this.dateTimeInput8.Value,
                        int.Parse(this.textBoxX14.Text.Trim()), int.Parse(this.textBoxX15.Text.Trim()),
                        int.Parse(this.textBoxX16.Text.Trim()), lessonName);
                }
                else if (this.comboBoxEx12.Text == "监控资源订购时间分布")
                {
                    sql1 = getCRMWithEnterTime(this.dateTimeInput7.Value, this.dateTimeInput8.Value,
                        int.Parse(this.textBoxX14.Text.Trim()), int.Parse(this.textBoxX15.Text.Trim()),
                        int.Parse(this.textBoxX16.Text.Trim()), lessonName);
                }
                addSeriesToChart(sql1, this.comboBoxEx12.Text);
            }
            catch { MessageBox.Show("操作异常"); }
        }
        private void comboBoxEx12_SelectedIndexChanged(object sender, EventArgs e)//分析项切换
        {
            if (this.comboBoxEx12.Text == "报名与学习次数")
            {
                this.textBoxX14.Text = "0";
                this.textBoxX15.Text = "5";
                this.textBoxX16.Text = "120";
            }
            else if (this.comboBoxEx12.Text == "报名与学习时长")
            {
                this.textBoxX14.Text = "0";
                this.textBoxX15.Text = "10";
                this.textBoxX16.Text = "100";
            }
            else if (this.comboBoxEx12.Text == "报名与订购时间")
            {
                this.textBoxX14.Text = "0";
                this.textBoxX15.Text = "7";
                this.textBoxX16.Text = "77";
            }
            else if (this.comboBoxEx12.Text == "监控资源听课次数分布")
            {
                this.textBoxX14.Text = "0";
                this.textBoxX15.Text = "5";
                this.textBoxX16.Text = "200";
            }
            else if (this.comboBoxEx12.Text == "监控资源听课时长分布")
            {
                this.textBoxX14.Text = "0";
                this.textBoxX15.Text = "50";
                this.textBoxX16.Text = "2000";
            }
            else if (this.comboBoxEx12.Text == "监控资源订购时间分布")
            {
                this.textBoxX14.Text = "0";
                this.textBoxX15.Text = "1";
                this.textBoxX16.Text = "0";
            }
        }
        /// <summary>
        /// 将柱状图添加到显示
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="seriesName"></param>
        private void addSeriesToChart(string sqlStr, string seriesName)
        {
            int maxNumY = 0;
            chart1.Series.Clear();
            DataTable dt1 = DBHelper.ExecuteQuery(sqlStr);
            Series series = new Series(seriesName);
            series.BorderWidth = 7;
            series.ShadowOffset = 4;
            //设置图表类型
            series.ChartType = SeriesChartType.Column;
            if (seriesName == "监控资源订购时间分布")
            {
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "yyyy-MM-dd-HH:mm:ss";
            }
            else
            {
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "";
            }
            for (int i = 0; i < dt1.Columns.Count; i++)
            {
                if (seriesName == "监控资源订购时间分布")
                {
                    series.Points.AddXY(dt1.Columns[i].ColumnName.ToString(), int.Parse(dt1.Rows[0][i].ToString()));
                }
                else
                {
                    series.Points.AddXY(int.Parse(dt1.Columns[i].ColumnName.ToString()), int.Parse(dt1.Rows[0][i].ToString()));
                }
                //顶部显示的数字
                series.Points[i].Label = dt1.Rows[0][i].ToString();
                series.Points[i].LabelForeColor = Color.Red;
                //鼠标放上去的提示内容
                series.Points[i].ToolTip = dt1.Rows[0][i].ToString();
                //获取最大值
                if (int.Parse(dt1.Rows[0][i].ToString()) > maxNumY)
                {
                    maxNumY = int.Parse(dt1.Rows[0][i].ToString());
                }
            }

            this.chart1.Series.Add(series);
            chart1.ChartAreas[0].AxisY.Maximum = maxNumY + 10;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.Interval = int.Parse(this.textBoxX15.Text.Trim());
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Position = 0;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = int.Parse(this.textBoxX15.Text.Trim()) * 10;
        }
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint selectedDataPoint = (DataPoint)result.Object;
                selectedDataPoint.BorderColor = Color.Red;
                string sql1 = "";
                string lessonNameTemp = "";
                lessonNameTemp = lessonName.Replace("''", "'");
                if (result.Series.Name == "报名与学习次数")
                {
                    sql1 = string.Format("select * from (" +
                        " select a.QQ as qqNum, sum(case when studytime>=0 then 1 else 0 end) as StudyNum, sum(StudyTime) as StudyTime, a.Jointime as BuyTime" +
                        " from Pub_VIPMessage a left join {4} b on a.qq = b.qq" +
                        " where a.Jointime between '{1}' and '{2}' {5} and dateadd(d,1,a.Jointime)>b.cometime and a.CompanyNames = '优梯' and a.EnterType = '报名'" +
                        " group by a.qq,a.Jointime) a" +
                        " where StudyNum != 0 and StudyNum >= {0}-{3} and  StudyNum < {0}",
                        selectedDataPoint.XValue.ToString(),
                        this.dateTimeInput7.Value.ToShortDateString(),
                        this.dateTimeInput8.Value.ToShortDateString(),
                        this.textBoxX15.Text.Trim(), detailMessage, lessonNameTemp);
                }
                else if (result.Series.Name == "报名与学习时长")
                {
                    sql1 = string.Format("select * from (" +
                        " select a.QQ as qqNum, sum(case when studytime>=0 then 1 else 0 end) as StudyNum, sum(StudyTime) as StudyTime, a.Jointime as BuyTime" +
                        " from Pub_VIPMessage a left join {4} b on a.qq = b.qq" +
                        " where a.Jointime between '{1}' and '{2}'  {5}  and dateadd(d,1,a.Jointime)>b.cometime and a.CompanyNames = '优梯' and a.EnterType = '报名'" +
                        " group by a.qq,a.Jointime) a" +
                        " where StudyTime >= {0}-{3} and  StudyTime < {0}",
                        selectedDataPoint.XValue.ToString(),
                        this.dateTimeInput7.Value.ToShortDateString(),
                        this.dateTimeInput8.Value.ToShortDateString(),
                        this.textBoxX15.Text.Trim(), detailMessage, lessonNameTemp);
                }
                else if (result.Series.Name == "报名与订购时间")
                {
                    sql1 = string.Format("select qqNum,StudyNum,StudyTime,BuyTime from " +
                        " (select a.qq as qqNum, " +
                        " sum(case when b.studytime>=0 then 1 else 0 end) as StudyNum, sum(b.StudyTime) as StudyTime," +
                        " a.Jointime as BuyTime, min(cometime) as enterTime from Pub_VIPMessage a left join {4} b on a.qq = b.qq " +
                        " where a.Jointime between '{1}' and '{2}'  {5}  and a.CompanyNames = '优梯' and a.EnterType = '报名'" +
                        " group by a.qq,a.Jointime) a" +
                        " where  datediff(day,enterTime,BuyTime) >= {0}-{3} and datediff(day,enterTime,BuyTime) < {0}",
                        selectedDataPoint.XValue.ToString(),
                        this.dateTimeInput7.Value.ToShortDateString(),
                        this.dateTimeInput8.Value.ToShortDateString(),
                        this.textBoxX15.Text.Trim(), detailMessage, lessonNameTemp);
                }
                else if (result.Series.Name == "监控资源听课次数分布")
                {
                    sql1 = string.Format("select a.qq as qqNum,a.StudyNumber as StudyNum,a.StudyTime as StudyTime, b.Jointime as BuyTime from {7} a " +
                        " left join Pub_VIPMessage b on a.qq = b.qq where ((b.CompanyNames = '优梯' and b.EnterType = '报名') or b.qq is null) and a.StudyNumber >={0}-{3} and a.StudyNumber < {0} and a.StudyNumber>0"+
                        " and a.QQ in (select QQ from {8} where  datepart(yy,cometime) = {4} and  datepart(mm,cometime) = {5} and  datepart(dd,cometime) = {6} {9}  group by QQ )",
                        selectedDataPoint.XValue.ToString(),
                        this.dateTimeInput7.Value.ToShortDateString(),
                        this.dateTimeInput8.Value.ToShortDateString(),
                        this.textBoxX15.Text.Trim(),
                        this.dateTimeInput8.Value.Year,
                        this.dateTimeInput8.Value.Month,
                        this.dateTimeInput8.Value.Day, allMessage, detailMessage, lessonNameTemp);//6
                }
                else if (result.Series.Name == "监控资源听课时长分布")
                {
                    sql1 = string.Format("select a.qq as qqNum,a.StudyNumber as StudyNum,a.StudyTime as StudyTime, b.Jointime as BuyTime from {7} a " +
                        " left join Pub_VIPMessage b on a.qq = b.qq where ((b.CompanyNames = '优梯' and b.EnterType = '报名') or b.qq is null) and a.StudyTime >={0}-{3} and a.StudyTime < {0} and a.StudyNumber>0"+
                        " and a.QQ in (select QQ from {8} where  datepart(yy,cometime) = {4} and  datepart(mm,cometime) = {5} and  datepart(dd,cometime) = {6} {9} group by QQ )",
                        selectedDataPoint.XValue.ToString(),
                        this.dateTimeInput7.Value.ToShortDateString(),
                        this.dateTimeInput8.Value.ToShortDateString(),
                        this.textBoxX15.Text.Trim(),
                        this.dateTimeInput8.Value.Year,
                        this.dateTimeInput8.Value.Month,
                        this.dateTimeInput8.Value.Day, allMessage, detailMessage, lessonNameTemp);//6
                }
                else if (result.Series.Name == "监控资源订购时间分布")
                {
                    sql1 = string.Format("select c.qqNum as qqNum,c.StudyNum as StudyNum, c.StudyTime as StudyTime, b.Jointime as BuyTime from" +
                        " (select qq as qqNum,sum(case when studytime>=0 then 1 else 0 end) as StudyNum, sum(StudyTime) as StudyTime, min(cometime) as entertime " +
                        " from {4} where 1>0 {5} group by qq) c" +
                        " left join Pub_VIPMessage b on c.qqNum = b.qq" +
                        " where ((b.CompanyNames = '优梯' and b.EnterType = '报名') or b.qq is null) and entertime>=dateadd(d,-{3},'{0}') and entertime<'{0}'",
                        selectedDataPoint.AxisLabel.ToString(),
                        this.dateTimeInput7.Value.ToShortDateString(),
                        this.dateTimeInput8.Value.ToShortDateString(),
                        this.textBoxX15.Text.Trim(), detailMessage, lessonNameTemp);
                }
                DataTable dtSendAll = DBHelper.ExecuteQuery(sql1);
                CRMStudyAllShow dsa = new CRMStudyAllShow();
                dsa.dgvdatasource = dtSendAll;
                dsa.qqDetailTB = detailMessage;
                dsa.Show();
            }
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            // Call HitTest  
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            // Reset Data Point Attributes  
            foreach (DataPoint point in chart1.Series[0].Points)
            {
                point.BackSecondaryColor = Color.Black;
                point.BackHatchStyle = ChartHatchStyle.None;
                point.BorderWidth = 1;
            }

            // If the mouse if over a data point  
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                // Find selected data point  
                DataPoint point = chart1.Series[0].Points[result.PointIndex];

                // Change the appearance of the data point  
                point.BackSecondaryColor = Color.White;
                point.BackHatchStyle = ChartHatchStyle.Percent25;
                point.BorderWidth = 2;
            }
            else
            {
                // Set default cursor  
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 报名与学习时长关系查询语句
        /// </summary>
        /// <param name="timestart">起始报名时间</param>
        /// <param name="timeend">截止报名时间</param>
        /// <param name="heardtitleint">从多少分钟开始查</param>
        /// <param name="stupnum">每次间隔多长时间</param>
        /// <param name="maxnum">查询截止分钟数</param>
        /// <returns>返回查询语句</returns>
        private string getEnterWithStudyTime(DateTime timestart, DateTime timeend, int heardtitleint, int stupnum, int maxnum, string lessonName)
        {
            string sql1 = string.Format("declare @timestart varchar(100), @timeend varchar(100)," +
                " @heardtitleint int," +
                " @heardtitlevarmax varchar(20)," +
                " @heardtitlevarmin varchar(20)," +
                " @stupnum int, @maxnum int, @sql varchar(max)" +
                " set @timestart = '{0}'" +
                " set @timeend = '{1}'" +
                " set @heardtitleint = {2}" +
                " set @stupnum = {3}" +
                " set @maxnum = {4}" +
                " set @sql = 'select '" +
                " while @heardtitleint<@maxnum-@stupnum" +
                " begin " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20)) " +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyTime>='+@heardtitlevarmin+' and StudyTime<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''',' " +
                " end " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20))" +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyTime>='+@heardtitlevarmin+' and StudyTime<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''''" +
                    " set @sql =@sql+'from (select a.QQ as qqNum, sum(StudyTime) as StudyTime from Pub_VIPMessage a left join {5} b on a.qq = b.qq where a.Jointime between" +
                    " '''+@timestart+''' and '''+@timeend+''' and dateadd(d,1,a.Jointime)>b.cometime and a.CompanyNames = ''优梯'' and a.EnterType = ''报名'' and StudyTime is not null {6} group by a.qq) c'" +
                " exec(@sql)", timestart.ToShortDateString(), timeend.ToShortDateString(), heardtitleint, stupnum, maxnum,detailMessage,lessonName);
            return sql1;
        }
        /// <summary>
        /// 报名与学习次数关系查询语句
        /// </summary>
        /// <param name="timestart">时间起始</param>
        /// <param name="timeend">时间截止</param>
        /// <param name="heardtitleint">最小量程</param>
        /// <param name="stupnum">精度</param>
        /// <param name="maxnum">最大量程</param>
        /// <returns></returns>
        private string getEnterWithStudyNum(DateTime timestart, DateTime timeend, int heardtitleint, int stupnum, int maxnum, string lessonName)
        {
            string sql1 = string.Format("declare @timestart varchar(100), @timeend varchar(100)," +
                " @heardtitleint int," +
                " @heardtitlevarmax varchar(20)," +
                " @heardtitlevarmin varchar(20)," +
                " @stupnum int, @maxnum int, @sql varchar(max)" +
                " set @timestart = '{0}'" +
                " set @timeend = '{1}'" +
                " set @heardtitleint = {2}" +
                " set @stupnum = {3}" +
                " set @maxnum = {4}" +
                " set @sql = 'select '" +
                " while @heardtitleint<@maxnum-@stupnum" +
                " begin " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20)) " +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyNum>='+@heardtitlevarmin+' and StudyNum<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''',' " +
                " end " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20))" +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyNum>='+@heardtitlevarmin+' and StudyNum<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''''" +

                    " set @sql =@sql+'from (" +
                    " select a.QQ as qqNum, sum(case when studytime>=0 then 1 else 0 end) as StudyNum from Pub_VIPMessage a left join {5} b on a.qq = b.qq " +
                    " where a.Jointime between '''+@timestart+''' and '''+@timeend+''' and dateadd(d,1,a.Jointime)>b.cometime and a.CompanyNames = ''优梯'' and a.EnterType = ''报名''  and studytime is not null {6} group by a.qq) c'" +

                " exec(@sql)", timestart.ToShortDateString(), timeend.ToShortDateString(), heardtitleint, stupnum, maxnum,detailMessage,lessonName);
            return sql1;
        }
        /// <summary>
        /// 报名与订购时间关系查询语句（此订购时间为监控抓取初次时间）
        /// </summary>
        /// <param name="timestart">时间起始</param>
        /// <param name="timeend">时间截止</param>
        /// <param name="heardtitleint">最小量程</param>
        /// <param name="stupnum">精度</param>
        /// <param name="maxnum">最大量程</param>
        /// <returns></returns>
        private string getEnterWithBuyTime(DateTime timestart, DateTime timeend, int heardtitleint, int stupnum, int maxnum, string lessonName)
        {
            string sql1 = string.Format("declare @timestart varchar(100), @timeend varchar(100)," +
                " @heardtitleint int," +
                " @heardtitlevarmax varchar(20)," +
                " @heardtitlevarmin varchar(20)," +
                " @stupnum int, @maxnum int, @sql varchar(max)" +
                " set @timestart = '{0}'" +
                " set @timeend = '{1}'" +
                " set @heardtitleint = {2}" +
                " set @stupnum = {3}" +
                " set @maxnum = {4}" +
                " set @sql = 'select '" +
                " while @heardtitleint<@maxnum-@stupnum" +
                " begin " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20)) " +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when datediff(day,enterTime,jointime)>='+@heardtitlevarmin+' and datediff(day,enterTime,jointime)<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''',' " +
                " end " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20))" +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when datediff(day,enterTime,jointime)>='+@heardtitlevarmin+' and datediff(day,enterTime,jointime)<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''''" +

                    " set @sql =@sql+'from (" +
                    " select a.qq as qqNum, a.Jointime as jointime, min(b.cometime) as enterTime from Pub_VIPMessage a left join {5} b on a.qq = b.qq " +
                    " where a.Jointime between '''+@timestart+''' and '''+@timeend+''' and dateadd(d,1,a.Jointime)>b.cometime and a.CompanyNames = ''优梯'' and a.EnterType = ''报名'' {6} group by a.qq, a.Jointime) c'" +

                " exec(@sql)", timestart.ToShortDateString(), timeend.ToShortDateString(), heardtitleint, stupnum, maxnum,detailMessage,lessonName);
            return sql1;
        }
        /// <summary>
        /// 监控学习次数分布
        /// </summary>
        /// <param name="timestart">时间起始</param>
        /// <param name="timeend">时间截止</param>
        /// <param name="heardtitleint">最小量程</param>
        /// <param name="stupnum">精度</param>
        /// <param name="maxnum">最大量程</param>
        /// <returns></returns>
        private string getCRMWithStudyNum(DateTime timestart, DateTime timeend, int heardtitleint, int stupnum, int maxnum, string lessonName)
        {
            string sql1 = string.Format("declare @timestart varchar(100), @timeend varchar(100)," +
                " @heardtitleint int," +
                " @heardtitlevarmax varchar(20)," +
                " @heardtitlevarmin varchar(20)," +
                " @stupnum int, @maxnum int, @sql varchar(max)" +
                " set @timestart = '{0}'" +
                " set @timeend = '{1}'" +
                " set @heardtitleint = {2}" +
                " set @stupnum = {3}" +
                " set @maxnum = {4}" +
                " set @sql = 'select '" +
                " while @heardtitleint<@maxnum-@stupnum" +
                " begin " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20)) " +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyNumber>='+@heardtitlevarmin+' and StudyNumber<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''',' " +
                " end " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20))" +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyNumber>='+@heardtitlevarmin+' and StudyNumber<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''''" +

                    " set @sql =@sql+'from {8} where StudyNumber>0" +
                    " and QQ in (select QQ from {9} where  datepart(yy,cometime) = {5} and  datepart(mm,cometime) = {6} and  datepart(dd,cometime) = {7} {10} group by QQ )'" +

                " exec(@sql)", 
                timestart.ToShortDateString(), 
                timeend.ToShortDateString(), 
                heardtitleint, 
                stupnum, 
                maxnum,
                this.dateTimeInput8.Value.Year,
                this.dateTimeInput8.Value.Month,
                this.dateTimeInput8.Value.Day, allMessage, detailMessage, lessonName);
            return sql1;
        }
        /// <summary>
        /// 监控学习时长分布
        /// </summary>
        /// <param name="timestart">时间起始</param>
        /// <param name="timeend">时间截止</param>
        /// <param name="heardtitleint">最小量程</param>
        /// <param name="stupnum">精度</param>
        /// <param name="maxnum">最大量程</param>
        /// <returns></returns>
        private string getCRMWithStudyTime(DateTime timestart, DateTime timeend, int heardtitleint, int stupnum, int maxnum, string lessonName)
        {
            string sql1 = string.Format("declare @timestart varchar(100), @timeend varchar(100)," +
                " @heardtitleint int," +
                " @heardtitlevarmax varchar(20)," +
                " @heardtitlevarmin varchar(20)," +
                " @stupnum int, @maxnum int, @sql varchar(max)" +
                " set @timestart = '{0}'" +
                " set @timeend = '{1}'" +
                " set @heardtitleint = {2}" +
                " set @stupnum = {3}" +
                " set @maxnum = {4}" +
                " set @sql = 'select '" +
                " while @heardtitleint<@maxnum-@stupnum" +
                " begin " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20)) " +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyTime>='+@heardtitlevarmin+' and StudyTime<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''',' " +
                " end " +
                    " set @heardtitlevarmax = cast((@heardtitleint+@stupnum) as varchar(20))" +
                    " set @heardtitlevarmin = cast(@heardtitleint as varchar(20))" +
                    " set @heardtitleint = @heardtitleint+@stupnum " +
                    " set @sql =@sql+'coalesce(sum(case when StudyTime>='+@heardtitlevarmin+' and StudyTime<'+@heardtitlevarmax+' then 1 else 0 end),0) as '''+@heardtitlevarmax+''''" +

                    " set @sql =@sql+'from {8} where StudyNumber>0 " +//注意此处少单引号
                    " and QQ in (select QQ from {9} where  datepart(yy,cometime) = {5} and  datepart(mm,cometime) = {6} and  datepart(dd,cometime) = {7} {10}  group by QQ )'" +

                " exec(@sql)", 
                timestart.ToShortDateString(),
                timeend.ToShortDateString(),
                heardtitleint,
                stupnum,
                maxnum,
                this.dateTimeInput8.Value.Year,
                this.dateTimeInput8.Value.Month,
                this.dateTimeInput8.Value.Day,allMessage,detailMessage,lessonName);
            return sql1;
        }
        /// <summary>
        /// 监控资源订购时间分布
        /// </summary>
        /// <param name="timestart">订购起始时间</param>
        /// <param name="timeend">订购截止时间</param>
        /// <param name="heardtitleint">起始分度</param>
        /// <param name="stupnum">精度</param>
        /// <param name="maxnum">最大分度</param>
        /// <returns></returns>
        private string getCRMWithEnterTime(DateTime timestart, DateTime timeend, int heardtitleint, int stupnum, int maxnum, string lessonName)
        {
            string sql1 = string.Format("declare" +
                " @sql varchar(max)," +
                " @stupnum int," +
                " @timestart datetime, " +
                " @timeend datetime  " +
                " set @stupnum = {0}" +
                " set @timestart = '{1}'" +
                " set @timeend = '{2}'" +
                " set @sql = 'select '" +
                " while @timestart<@timeend" +
                " begin" +
                    " set @sql =@sql+" +
                    " 'coalesce(sum(case when entertime>='''+CONVERT(varchar(100), @timestart, 23)+''' and entertime<'''+CONVERT(varchar(100), @timestart+@stupnum, 23)+''' then 1 else 0 end),0)" +
                    " as '''+CONVERT(varchar(100), @timestart+@stupnum, 23)+''','" +
                    " set @timestart = @timestart+@stupnum" +
                " end" +
                    " set @sql =@sql+" +
                    " 'coalesce(sum(case when entertime>='''+CONVERT(varchar(100), @timestart, 23)+''' and entertime<'''+CONVERT(varchar(100), @timestart+@stupnum, 23)+''' then 1 else 0 end),0)" +
                    " as '''+CONVERT(varchar(100), @timestart+@stupnum, 23)+''''" +
                    " set @sql =@sql+'from ( select qq as qqNum, min(cometime) as entertime from {3} where 1>0 {4} group by qq) c'" +
                " exec(@sql)", stupnum, timestart, timeend, detailMessage, lessonName);
            return sql1;
        }
        /////////////////////////////////////////////资源导入///////////////////////////////////////////////////////////
        private DataTable dtenterall = new DataTable();
        private void buttonX7_Click(object sender, EventArgs e)//查询内部资源
        {
            string sql1 = string.Format("select QQ, StudyNumber, StudyTime from [UTCRMDB].[dbo].[CustomerInfo] where 1>0" +
                " and qq in ( select distinct qq from [UTCRMDB].[dbo].[KeQQStudyRecord] where cometime>='{0}' and cometime<='{1}')" +
                " and StudyNumber between {2} and {3}" +
                " and StudyTime between {4} and {5}",
                this.dateTimeInput5.Value.ToShortDateString(), this.dateTimeInput6.Value.ToShortDateString(),
                this.textBoxX6.Text.Trim(), this.textBoxX13.Text.Trim(),
                this.textBoxX9.Text.Trim(), this.textBoxX12.Text.Trim());
            if (this.checkBoxX1.Checked)
                sql1 += " and qq not in (select qq from Pub_VIPMessage)";
            if (this.checkBoxX2.Checked)
                sql1 += " and qq not in (select qq from UT_YXResource)";
            if (this.checkBoxX3.Checked)
                sql1 += " and qq not in (select qq from UT_YXPromotes)";
            dtenterall = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX5.DataSource = dtenterall;
            this.labelX29.Text = "共 " + this.dataGridViewX5.Rows.Count + " 条记录";
        }
        private void buttonX11_Click(object sender, EventArgs e)//导入数据库
        {
            if (dtenterall.Rows.Count == 0)
            {
                MessageBox.Show("请先查询需要导入数据库中的数据");
                return;
            }
            if (MessageBox.Show("你确定要导入数据库吗?", "重置提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            int enterCount = 0;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = dtenterall.Rows.Count;
            this.progressBar1.Value = 0;
            this.progressBar1.Step = 1;
            string sql1 = "insert into UT_YXResource (UploadTime,QQ,DistributeTime,StaffID,GetState,Remark,OverTime,GetTime) ";
            for (int i = 0; i < dtenterall.Rows.Count; i++)
            {
                sql1 += string.Format("select '{0}','{1}','1900-1-1','','未分配','无','1900-1-1','1900-1-1' union all ",
                    System.DateTime.Now.ToShortDateString(),
                    dtenterall.Rows[i][0].ToString());
                if (i % 50 == 0)
                {
                    sql1 = sql1.Substring(0, sql1.Length - 10);
                    enterCount += DBHelper.ExecuteUpdate(sql1);
                    sql1 = "insert into UT_YXResource (UploadTime,QQ,DistributeTime,StaffID,GetState,Remark,OverTime,GetTime) ";
                }
                this.progressBar1.Value += 1;
            }
            sql1 = sql1.Substring(0, sql1.Length - 10);
            enterCount += DBHelper.ExecuteUpdate(sql1);
            this.progressBar1.Value = dtenterall.Rows.Count;
            MessageBox.Show("成功导入：" + enterCount + "条记录！");
        }
        private void dataGridViewX5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX5.SelectedRows.Count == 0)
                return;
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.qqDetail = this.dataGridViewX5.SelectedRows[0].Cells[0].Value.ToString();
            sds.qqDetailTBName = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
            sds.Show();
        }
        /////////////////////////////////////////////资源分配///////////////////////////////////////////////////////////
        private void buttonX3_Click(object sender, EventArgs e)//营销主管查询
        {
            string comSelVal = "";
            if (this.comboBoxEx1.SelectedValue != null) 
            {
                comSelVal = this.comboBoxEx1.SelectedValue.ToString();
            }
            string sql1 = string.Format("select" +
                " a.UploadTime as UploadTime ," +
                " a.QQ as QQ ," +
                " a.Remark as Remark ," +
                " c.AssumedName as GetPerson ," +
                " a.GetState as GetState ," +
                " b.StudyTime as StudyTime ," +
                " b.StudyNumber as StudyNumber" +
                " from UT_YXResource a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq left join Users c on a.StaffID = c.StaffID" +
                " where a.UploadTime between '{0}' and '{1}' and a.StaffID like '%{2}%' and a.GetState like '%{3}%'" +
                " order by a.UploadTime desc",
                this.dateTimeInput1.Value.ToShortDateString(),
                this.dateTimeInput2.Value.ToShortDateString(),
                comSelVal,
                this.comboBoxEx2.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX2.DataSource = dt1;
            this.labelX30.Text = "共 " + this.dataGridViewX2.Rows.Count + " 条记录";
        }
        private void buttonX4_Click(object sender, EventArgs e)//营销主管再分配
        {
            if (this.textBoxX3.Text.Trim() == "" || this.comboBoxEx3.Text == "")
            {
                MessageBox.Show("请先输入分配时间和要分配的人员");
                return;
            }
            string OverTime = System.DateTime.Now.AddDays(int.Parse(this.textBoxX3.Text.Trim())).ToShortDateString();
            string GetPerson = this.comboBoxEx3.SelectedValue.ToString();

            DialogResult RSS = MessageBox.Show(this, "确定要分配选中行数据码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    string sql1 = "";
                    int resultNum = 0;
                    for (int i = this.dataGridViewX2.SelectedRows.Count; i > 0; i--)
                    {
                        if (dataGridViewX2.SelectedRows[i - 1].Cells[6].ToString() != "正在跟踪")
                        {
                            //使用获得的ID删除数据库的数据
                            sql1 = string.Format(" update UT_YXResource set DistributeTime =CONVERT(varchar(100), getdate(), 23), OverTime ='{0}', StaffID ='{1}', GetState = '已分配' where QQ ='{2}' ",
                                OverTime,
                                GetPerson,
                                this.dataGridViewX2.SelectedRows[i - 1].Cells[1].Value.ToString());
                            resultNum += DBHelper.ExecuteUpdate(sql1);
                            dataGridViewX2.Rows.RemoveAt(dataGridViewX2.SelectedRows[i - 1].Index);
                        }
                    }
                    this.labelX30.Text = "共 " + this.dataGridViewX2.Rows.Count + " 条记录";
                    if (resultNum != 0)
                    {
                        //插入消息提醒
                        //string sqlQuery = string.Format("select * from UT_YXResource where TrackPerson= ''{0}''", this.comboBoxEx3.Text.Trim());
                        //string GoPageName = "frmVIPServer";//英文
                        //string GoMenuName = "意向提供";
                        //string GoTabItemName = "营销组员";
                        //string GoDGV = "dataGridViewX1";
                        //string sqlMessage = string.Format("insert into UT_MessageRemind values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',0)",
                        //    tools.sqldate(DateTime.Now),
                        //    frmUTSOFTMAIN.AssumedName,
                        //    this.comboBoxEx3.Text.Trim(),
                        //    "您接收到了新的意向，请注意跟踪",
                        //    sqlQuery, GoPageName, GoMenuName, GoTabItemName, GoDGV);
                        //int a = DBHelper.ExecuteUpdate(sqlMessage);
                    }
                    break;
                case DialogResult.No:
                    break;
            }
        }
        private void dataGridViewX2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX2.SelectedRows.Count == 0)
                return;
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.qqDetail = this.dataGridViewX2.SelectedRows[0].Cells[1].Value.ToString();
            sds.qqDetailTBName = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
            sds.Show();
        }
        ////////////////////////////////////////////组员情况///////////////////////////////////////////////////////////////
        private void dataGridViewX6_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 || e.RowIndex == -1)
                return;
            //if (e.ColumnIndex == 8)
            //{
            //    string sql1 = string.Format("select a.QQ as QQ, a.NickName as NickName, a.Name as Name, a.Jointime as Join_time," +
            //        " a.YYNum as YYNum, a.YYName as YYName, a.Telephone as Telephone, " +
            //        " a.MeanTeacher as MeanTeacher from Pub_VIPMessage a left join UT_JXManager b on a.qq = b.qq " +
            //        " where b.StaffID = '{0}' and a.CompanyNames = '优梯' and EnterType = '报名'",
            //        this.dataGridViewX6.Rows[e.RowIndex].Cells[9].Value.ToString());
            //    DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            //    UT_VIPMessageShow.dgvdatasource = dt1;
            //    UT_VIPMessageShow vipms = new UT_VIPMessageShow();
            //    vipms.Show();
            //}
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                string sql1 = "";
                if (e.ColumnIndex == 1)
                {
                    sql1 = string.Format("select a.QQ as qqNum, StudyNumber as StudyNum, StudyTime from UT_YXResource a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq " +
                        " where a.StaffID = '{0}' and a.GetState = '已分配'",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString());
                }
                else if (e.ColumnIndex == 2)
                {
                    sql1 = string.Format("select a.QQ as qqNum, StudyNumber as StudyNum, StudyTime from UT_YXResource a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq " +
                        " where a.StaffID = '{0}' and a.GetState = '已收录'",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString(),
                        this.dataGridViewX6);
                }
                DataTable dtSendAll = DBHelper.ExecuteQuery(sql1);
                CRMStudyAllShow dsa = new CRMStudyAllShow();
                dsa.dgvdatasource = dtSendAll;
                dsa.qqDetailTB = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
                dsa.Show();


            }
            else//34567
            {
                string sql1 = "";
                if (e.ColumnIndex == 3)//意向总数
                {
                    sql1 = string.Format("select (case when TrackState = '已过期' then datediff(day, a.EnterTime, a.OverTime) " +
                        " when TrackState = '已放弃' then datediff(day, a.EnterTime, a.GiveTime) " +
                        " when TrackState = '正在跟踪' then datediff(day, a.EnterTime, getdate())else 0 end) as datediffNum," +
                        " a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                        " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq  " +
                        " where a.StaffID = '{0}'" +
                        " order by a.EnterTime desc",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString());
                }
                else if (e.ColumnIndex == 4)//自录资源
                {
                    sql1 = string.Format("select (case when TrackState = '已过期' then datediff(day, a.EnterTime, a.OverTime) " +
                        " when TrackState = '已放弃' then datediff(day, a.EnterTime, a.GiveTime) " +
                        " when TrackState = '正在跟踪' then datediff(day, a.EnterTime, getdate())else 0 end) as datediffNum," +
                        " a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                        " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq  " +
                        " where a.StaffID = '{0}'" +
                        " and a.PromoteResource like '%{1}%'" +
                        " order by a.EnterTime desc",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString(),
                        "自主添加");
                }
                else if (e.ColumnIndex == 5)//正在跟踪
                {
                    sql1 = string.Format("select datediff(day, a.EnterTime, getdate()) as datediffNum,a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                        " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq  " +
                        " where a.StaffID = '{0}'" +
                        " and a.TrackState like '%{1}%'" +
                        " order by a.EnterTime desc",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString(),
                        "正在跟踪");
                }
                else if (e.ColumnIndex == 6)//已放弃数 
                {
                    sql1 = string.Format("select datediff(day, a.EnterTime, a.GiveTime) as datediffNum,a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                        " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq  " +
                        " where a.StaffID = '{0}'" +
                        " and a.TrackState like '%{1}%'" +
                        " order by a.EnterTime desc",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString(),
                        "已放弃");
                }
                else if (e.ColumnIndex == 7)//已过期数 
                {
                    sql1 = string.Format("select datediff(day, a.EnterTime, a.OverTime) as datediffNum,a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                        " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq  " +
                        " where a.StaffID = '{0}'" +
                        " and a.TrackState like '%{1}%'" +
                        " order by a.EnterTime desc",
                        this.dataGridViewX6.Rows[e.RowIndex].Cells[8].Value.ToString(),
                        "已过期");
                }
                DataTable dt1 = DBHelper.ExecuteQuery(sql1);
                YX_PromoteAllShow.dgvdatasource = dt1;
                YX_PromoteAllShow yxpa = new YX_PromoteAllShow();
                yxpa.Show();


            }
        }
        private void buttonX14_Click(object sender, EventArgs e)//刷新
        {
            string sqlzyqk = string.Format(" select  a.AssumedName as 花名,a.StaffID as 身份证," +
                " coalesce(b.获得资源,0) as 获得资源," +
                " coalesce(b.收录资源,0) as 收录资源," +
                " coalesce(c.意向总数,0) as 意向总数," +
                " coalesce(c.自主添加,0) as 自主添加," +
                " coalesce(c.正在跟踪,0) as 正在跟踪," +
                " coalesce(c.已放弃数,0) as 已放弃数," +
                " coalesce(c.已过期数,0) as 已过期数 from Users a" +
                " left join " +
                " (select StaffID,coalesce(count(1),0) as '获得资源'," +
                " coalesce(sum(case when GetState = '已收录' then 1 else 0 end),0) as '收录资源'" +
                " from UT_YXResource group by StaffID)" +
                " b on a.StaffID = b.StaffID" +
                " left join " +
                " (select StaffID,coalesce(count(1),0) as '意向总数'," +
                " coalesce(sum(case when PromoteResource = '自主添加' then 1 else 0 end),0) as '自主添加'," +
                " coalesce(sum(case when TrackState = '正在跟踪' then 1 else 0 end),0) as '正在跟踪'," +
                " coalesce(sum(case when TrackState = '已放弃'then 1 else 0 end),0) as '已放弃数'," +
                " coalesce(sum(case when TrackState = '已过期' then 1 else 0 end),0) as '已过期数'" +
                " from UT_YXPromotes group by StaffID)" +
                " c on a.StaffID = c.StaffID" +
                " where a.CompanyNames = '优梯' and a.DepartmentName = '营销部'");
            DataTable dtzyqk = DBHelper.ExecuteQuery(sqlzyqk);
            this.dataGridViewX6.DataSource = dtzyqk;
        }
        /////////////////////////////////////////////组员意向库///////////////////////////////////////////////////////////
        private string checkPromoteTrackNum(string qqNum)
        {
            string sql1 = string.Format("select b.AssumedName,a.TrackState from UT_YXPromotes a left join Users b on a.StaffID = b.StaffID  where a.QQ = '{0}'", qqNum);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            string messStr = "";
            //意向库中有此意向
            if (dt1.Rows.Count > 0)
            {
                messStr = "此意向已在意向库\n";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    messStr += string.Format("为【" + dt1.Rows[i][0].ToString() + "】的【" + dt1.Rows[i][1].ToString() + "】意向\n");
                }
            }
            //意向库中无此意向
            else
            {
                messStr = "此意向不在意向库\n";
            }
            messStr += "是否提交？";
            return messStr;
        }
        private void buttonX1_Click(object sender, EventArgs e)//营销组员提交
        {
            if (this.textBoxX1.Text.Trim() == "")
            {
                MessageBox.Show("请输入QQ");
                return;
            }
            string sql1 = string.Format("select * from Pub_VIPMessage where qq = '{0}' and CompanyNames = '优梯' and EnterType = '报名'", this.textBoxX1.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            if (dt1.Rows.Count > 0)
            {
                MessageBox.Show("此QQ已报名！");
                return;
            }
            string sql2 = string.Format("select * from UT_YXPromotes where QQ = '{0}' and StaffID = '{1}'", this.textBoxX1.Text.Trim(), frmUTSOFTMAIN.StaffID);
            DataTable dt2 = DBHelper.ExecuteQuery(sql2);
            //本人存在此QQ的记录
            if (dt2.Rows.Count > 0)
            {
                string sqlup = "";
                if (dt2.Rows[0][9].ToString() == "正在跟踪")//修改资料
                {
                    if (MessageBox.Show("此意向为您的【正在跟踪】意向,确定修改吗？", "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                    sqlup = string.Format("update UT_YXPromotes set NickName = '{2}',PromoteGrade = '{3}',JoinSituation = '{4}',Remark = '{5}'," +
                        "Trackplan = '{6}' where StaffID = '{0}' and QQ = '{1}'",
                        frmUTSOFTMAIN.StaffID,
                        this.textBoxX1.Text.Trim(),
                        tools.FilteSQLStr(this.textBoxX19.Text.Trim()),
                        this.comboBoxEx14.Text,
                        this.textBoxX2.Text,
                        this.textBoxX17.Text,
                        this.textBoxX18.Text);
                    int resultNumup = DBHelper.ExecuteUpdate(sqlup);
                    if (resultNumup > 0)
                    {
                        MessageBox.Show("修改资料成功！");
                        ZYQuery();
                    }
                }
                else if (dt2.Rows[0][9].ToString() == "已放弃")//修改资料，修改跟踪状态，初始化放弃时间，修改过期时间为今天往后数十天
                {
                    if (MessageBox.Show("此意向为您的【已放弃】意向,确定要继续跟踪吗？", "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                    sqlup = string.Format("update UT_YXPromotes set TrackState = '正在跟踪',OverTime = CONVERT(varchar(100), dateadd(d,10,getdate()), 23)," +
                        "GiveTime = '1900-1-1', NickName = '{2}',PromoteGrade = '{3}',JoinSituation = '{4}',Remark = '{5}'," +
                        "Trackplan = '{6}' where StaffID = '{0}' and QQ = '{1}'",
                        frmUTSOFTMAIN.StaffID,
                        this.textBoxX1.Text.Trim(),
                        tools.FilteSQLStr(this.textBoxX19.Text.Trim()),
                        this.comboBoxEx14.Text,
                        this.textBoxX2.Text,
                        this.textBoxX17.Text,
                        this.textBoxX18.Text);
                    int resultNumup = DBHelper.ExecuteUpdate(sqlup);
                    if (resultNumup > 0)
                    {
                        MessageBox.Show("提交成功，限定跟踪时间为10天");
                        ZYQuery();
                    }
                }
                else if (dt2.Rows[0][9].ToString() == "已过期")//修改资料，修改跟踪状态，修改过期时间为今天往后数十天
                {
                    if (MessageBox.Show("此意向为您的【已过期】意向,确定延期跟踪吗？", "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                    sqlup = string.Format("update UT_YXPromotes set TrackState = '正在跟踪',OverTime = CONVERT(varchar(100), dateadd(d,10,getdate()), 23)," +
                        " NickName = '{2}',PromoteGrade = '{3}',JoinSituation = '{4}',Remark = '{5}'," +
                        "Trackplan = '{6}' where StaffID = '{0}' and QQ = '{1}'",
                        frmUTSOFTMAIN.StaffID,
                        this.textBoxX1.Text.Trim(),
                        tools.FilteSQLStr(this.textBoxX19.Text.Trim()),
                        this.comboBoxEx14.Text,
                        this.textBoxX2.Text,
                        this.textBoxX17.Text,
                        this.textBoxX18.Text);
                    int resultNumup = DBHelper.ExecuteUpdate(sqlup);
                    if (resultNumup > 0)
                    {
                        MessageBox.Show("提交成功，限定跟踪时间为10天");
                        ZYQuery();
                    }
                }
            }
            //本人不存在此QQ的记录
            else
            {
                if (MessageBox.Show(checkPromoteTrackNum(this.textBoxX1.Text.Trim()), "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
                string sql3 = string.Format("insert into UT_YXPromotes values(CONVERT(varchar(100), getdate(), 23),'{2}','{1}','{4}'," +
                        "'自主添加','{3}','{5}','{6}','{0}','正在跟踪',CONVERT(varchar(100), dateadd(d,10,getdate()), 23),'1900-1-1')",
                     frmUTSOFTMAIN.StaffID,
                     this.textBoxX1.Text.Trim(),
                     tools.FilteSQLStr(this.textBoxX19.Text.Trim()),
                     this.comboBoxEx14.Text,
                     this.textBoxX2.Text,
                     this.textBoxX17.Text,
                     this.textBoxX18.Text);
                int resultNum = DBHelper.ExecuteUpdate(sql3);
                if (resultNum > 0)
                {
                    MessageBox.Show("提交成功，限定跟踪时间为10天");
                    ZYQuery();
                }
            }
        }
        private void ZYQuery()
        {
            if (frmUTSOFTMAIN.DepartmentName.Contains("营销"))
            {
                string sql1 = string.Format("select datediff(day, a.EnterTime, getdate()) as datediffNum,a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq" +
                " where a.StaffID = '{0}' and a.TrackState = '正在跟踪' order by a.EnterTime desc", frmUTSOFTMAIN.StaffID);
                DataTable dt1 = DBHelper.ExecuteQuery(sql1);
                this.dataGridViewX1.DataSource = dt1;
            }
        }
        private void buttonX2_Click(object sender, EventArgs e)//营销组员查询
        {
            string sql1 = string.Format("select datediff(day, a.EnterTime, getdate()) as datediffNum,a.EnterTime,a.NickName,a.QQ,a.JoinSituation,a.PromoteResource," +
                " a.PromoteGrade,a.TrackState,a.Remark,a.Trackplan,b.StudyNumber,b.StudyTime from UT_YXPromotes a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq" +
                " where a.StaffID = '{0}'", frmUTSOFTMAIN.StaffID);
            if (this.textBoxX1.Text.Trim() != "")
            {
                sql1 += string.Format(" and a.QQ like '%{0}%'", this.textBoxX1.Text.Trim());
                sql1 += " order by a.EnterTime desc";
                DataTable dtqq1 = DBHelper.ExecuteQuery(sql1);
                this.dataGridViewX1.DataSource = dtqq1;
                return;
            }
            else
            {
                sql1 += " and a.TrackState = '正在跟踪'";
            }
            if (this.textBoxX19.Text.Trim() != "")
                sql1 += string.Format(" and a.NickName like '%{0}%'", this.textBoxX19.Text.Trim());
            if (this.comboBoxEx14.Text != "")
                sql1 += string.Format(" and a.PromoteGrade like '%{0}%'", this.comboBoxEx14.Text);
            if (this.textBoxX2.Text.Trim() != "")
                sql1 += string.Format(" and a.JoinSituation like '%{0}%'", this.textBoxX2.Text.Trim());
            if (this.textBoxX17.Text.Trim() != "")
                sql1 += string.Format(" and a.Remark like '%{0}%'", this.textBoxX17.Text.Trim());
            if (this.textBoxX18.Text.Trim() != "")
                sql1 += string.Format(" and a.Trackplan like '%{0}%'", this.textBoxX18.Text.Trim());
            sql1 += " order by a.EnterTime desc";
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX1.DataSource = dt1;
        }
        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();//QQ号码
            this.textBoxX19.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();//QQ昵称
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[9].Value.ToString();//报名情况
            this.textBoxX17.Text = this.dataGridViewX1.SelectedRows[0].Cells[10].Value.ToString();//意向情况
            this.textBoxX18.Text = this.dataGridViewX1.SelectedRows[0].Cells[11].Value.ToString();//跟踪计划
            this.comboBoxEx14.Text = this.dataGridViewX1.SelectedRows[0].Cells[7].Value.ToString();//意向强度
        }
        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.qqDetail = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString(); ;
            sds.qqDetailTBName = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
            sds.Show();
        }
        private void buttonX13_Click(object sender, EventArgs e)//营销组员放弃跟踪
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选定记录");
                return;
            }
            if (this.dataGridViewX1.SelectedRows[0].Cells[8].Value.ToString() == "已放弃")
            {
                MessageBox.Show("请勿重复放弃");
                return;
            }
            if (MessageBox.Show("确定要放弃跟踪QQ【" + this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString() + "】吗？", "放弃提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update UT_YXPromotes set TrackState = '已放弃', GiveTime = CONVERT(varchar(100), getdate(), 23) where QQ = '{0}' and StaffID = '{1}'",
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString(),
                frmUTSOFTMAIN.StaffID);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("放弃成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
            }
        }
        private void textBoxX1_TextChanged(object sender, EventArgs e)//营销组员
        {
            if (this.textBoxX1.Text == "")
            {
                this.textBoxX19.Text = "";//QQ昵称
                this.textBoxX2.Text = "";//报名情况
                this.textBoxX17.Text = "";//意向情况
                this.textBoxX18.Text = "";//跟踪计划
                this.comboBoxEx14.Text = "";//意向强度
            }
        }
        /// <summary>
        /// 更改行显示颜色
        /// </summary>
        /// <param name="dgv">对象表格</param>
        /// <param name="qdNum">意向强度角标</param>
        /// <param name="ztNum">跟踪状态角标</param>
        private void changeColor(DevComponents.DotNetBar.Controls.DataGridViewX dgv, int qdNum, int ztNum)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                if (dgv.Rows[i].Cells[qdNum].Value.ToString() == "高")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.OrangeRed;
                }
                else if (dgv.Rows[i].Cells[qdNum].Value.ToString() == "中")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                }
                if (dgv.Rows[i].Cells[ztNum].Value.ToString() == "已报名")
                {
                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightSlateGray;
                }
            }
        }
        ///////////////////////////////////////////组员资源库/////////////////////////////////////////////////
        private void buttonX15_Click(object sender, EventArgs e)//组员资源库查询
        {
            string sql1 = string.Format("select" +
                " a.DistributeTime as DistributeTime ," +
                " a.OverTime as OverTime ," +
                " a.QQ as QQ ," +
                " b.StudyNumber as StudyNumber ," +
                " b.StudyTime as StudyTime ," +
                " a.Remark as Remark ," +
                " a.GetState as GetState" +
                " from UT_YXResource a left join [UTCRMDB].[dbo].[CustomerInfo] b on a.qq = b.qq" +
                " where a.DistributeTime between '{0}' and '{1}' and a.StaffID = '{2}' and a.GetState like '%{3}%' and a.qq like '%{4}%'" +
                " order by a.DistributeTime desc",
                this.dateTimeInput9.Value.ToShortDateString(),
                this.dateTimeInput10.Value.ToShortDateString(),
                frmUTSOFTMAIN.StaffID,
                this.comboBoxEx13.Text,
                this.textBoxX20.Text.Trim());
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX7.DataSource = dt1;
        }
        private void dataGridViewX7_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX7.SelectedRows.Count == 0)
                return;
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.qqDetail = this.dataGridViewX7.SelectedRows[0].Cells[2].Value.ToString();
            sds.qqDetailTBName = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
            sds.Show();
        }
        private void buttonX16_Click(object sender, EventArgs e)//组员资源库收录
        {
            if (this.dataGridViewX7.SelectedRows.Count == 0)
                return;
            string messStr = checkPromoteTrackNum(this.dataGridViewX7.SelectedRows[0].Cells[2].Value.ToString());
            if (messStr.Contains("此意向已在意向库"))
            {
                if (MessageBox.Show(messStr, "提交提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            //单条收录
            string sql1 = string.Format(
                    "update UT_YXResource set GetTime = CONVERT(varchar(100), getdate(), 23), GetState = '已收录' where QQ = '{0}' and StaffID = '{1}'" +
                    "insert into UT_YXPromotes values(CONVERT(varchar(100), getdate(), 23),'','{0}','','CRM','低','','','{1}','正在跟踪'," +
                    "CONVERT(varchar(100), dateadd(d,10,getdate()), 23),'1900-1-1')",
                    dataGridViewX7.SelectedRows[0].Cells[2].Value.ToString(),
                    frmUTSOFTMAIN.StaffID);
            DBHelper.ExecuteUpdate(sql1);
            dataGridViewX7.Rows.Remove(dataGridViewX7.SelectedRows[0]);
            MessageBox.Show("收录成功！");
            //多条收录
            /*
            int successCount = 0;
            for (int i = this.dataGridViewX7.SelectedRows.Count; i > 0; i--)
            {
                string sql1 = string.Format(
                    "update UT_YXResource set GetTime = CONVERT(varchar(100), getdate(), 23), GetState = '已收录' where QQ = '{0}' and GetPerson = '{1}'"+
                    "insert into UT_YXPromotes values(CONVERT(varchar(100), getdate(), 23),'','{0}','','内部监控','低','','','{1}','正在跟踪',"+
                    "CONVERT(varchar(100), dateadd(d,10,getdate()), 23),'1900-1-1')",
                    this.dataGridViewX7.SelectedRows[i - 1].Cells[2].Value.ToString(),
                    frmUTSOFTMAIN.AssumedName);
                successCount += DBHelper.ExecuteUpdate(sql1);
                dataGridViewX7.Rows.RemoveAt(dataGridViewX7.SelectedRows[i - 1].Index);
            }
            MessageBox.Show("成功收录 "+successCount/2+" 条!");
             * */
        }
        //////////////////////////////////////////表格权限管理////////////////////////////////////////////////////////
        private void comboBoxEx15_SelectedIndexChanged(object sender, EventArgs e)//报表权限人切换
        {
            frmUTSOFTMAIN.tablePowerShow(this.panel1, this.comboBoxEx15.SelectedValue.ToString());
        }
        private void buttonX37_Click(object sender, EventArgs e)//报表权限提交
        {
            
            frmUTSOFTMAIN.tablePowerSubmit(this.panel1,"优梯", this.comboBoxEx15.SelectedValue.ToString());
        }
        ///////////////////////////////////////////工作报表/////////////////////////////////////////////////

        private void comboBoxEx3_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", "优梯", "营销部");
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtTGPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);

            this.comboBoxEx3.DataSource = dtTGPerson.Copy();
            this.comboBoxEx3.DisplayMember = "AssumedName";
            this.comboBoxEx3.ValueMember = "StaffID";
            this.comboBoxEx3.SelectedIndex = -1;
        }

        private void comboBoxEx15_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", "优梯", "营销部");
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtTGPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);

            this.comboBoxEx15.DataSource = dtTGPerson.Copy();
            this.comboBoxEx15.DisplayMember = "AssumedName";
            this.comboBoxEx15.ValueMember = "StaffID";
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("CompanyNames = '{0}' and DepartmentName = '{1}'", "优梯", "营销部");
            string[] columNames = new string[] { "AssumedName", "StaffID" };
            DataTable dtTGPerson = tools.dtFilter(frmUTSOFTMAIN.dtAllPerson, columNames, sql);

            this.comboBoxEx1.DataSource = dtTGPerson.Copy();
            this.comboBoxEx1.DisplayMember = "AssumedName";
            this.comboBoxEx1.ValueMember = "StaffID";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        private void buttonX44_Click(object sender, EventArgs e)//查询内部
        {
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.qqDetail = this.textBoxX4.Text.Trim();
            sds.qqDetailTBName = "[UTCRMDB].[dbo].[KeQQStudyRecord]";
            sds.Show();
        }
        ///////////////////////////////////////////工作报表/////////////////////////////////////////////////

        private void buttonX5_Click(object sender, EventArgs e)//营销QQ 早间 资源添加
        {
            YX_ResourceAddShow yxras = new YX_ResourceAddShow();
            yxras.qqType = "营销QQ";
            yxras.groupName = "早间";
            yxras.Text = this.buttonX5.Text;
            yxras.Show();
        }

        private void buttonX6_Click(object sender, EventArgs e)//营销QQ 早间 资源开发
        {
            YX_ResourceDevelopShow yxrds = new YX_ResourceDevelopShow();
            yxrds.qqType = "营销QQ";
            yxrds.groupName = "早间";
            yxrds.Text = this.buttonX6.Text;
            yxrds.Show();
        }

        private void buttonX9_Click(object sender, EventArgs e)//营销QQ 晚间 资源添加
        {
            YX_ResourceAddShow yxras = new YX_ResourceAddShow();
            yxras.qqType = "营销QQ";
            yxras.groupName = "晚间";
            yxras.Text = this.buttonX9.Text;
            yxras.Show();
        }

        private void buttonX8_Click(object sender, EventArgs e)//营销QQ 晚间 资源开发
        {
            YX_ResourceDevelopShow yxrds = new YX_ResourceDevelopShow();
            yxrds.qqType = "营销QQ";
            yxrds.groupName = "晚间";
            yxrds.Text = this.buttonX8.Text;
            yxrds.Show();
        }

        private void buttonX17_Click(object sender, EventArgs e)//个人QQ 早间 资源添加
        {
            YX_ResourceAddShow yxras = new YX_ResourceAddShow();
            yxras.qqType = "个人QQ";
            yxras.groupName = "早间";
            yxras.Text = this.buttonX17.Text;
            yxras.Show();
        }

        private void buttonX10_Click(object sender, EventArgs e)//个人QQ 早间 资源开发
        {
            YX_ResourceDevelopShow yxrds = new YX_ResourceDevelopShow();
            yxrds.qqType = "个人QQ";
            yxrds.groupName = "早间";
            yxrds.Text = this.buttonX10.Text;
            yxrds.Show();
        }

        private void buttonX19_Click(object sender, EventArgs e)//个人QQ 早间 QQ群
        {
            YX_ResourceGroup yxrg = new YX_ResourceGroup();
            yxrg.groupName = "早间";
            yxrg.Text = this.buttonX19.Text;
            yxrg.Show();
        }

        private void buttonX18_Click(object sender, EventArgs e)//个人QQ 早间 QQ空间
        {
            YX_QQZone yxqqz = new YX_QQZone();
            yxqqz.groupName = "早间";
            yxqqz.Text = this.buttonX18.Text;
            yxqqz.Show();
        }

        private void buttonX26_Click(object sender, EventArgs e)//个人QQ 晚间 资源添加
        {
            YX_ResourceAddShow yxras = new YX_ResourceAddShow();
            yxras.qqType = "个人QQ";
            yxras.groupName = "晚间";
            yxras.Text = this.buttonX26.Text;
            yxras.Show();
        }

        private void buttonX22_Click(object sender, EventArgs e)//个人QQ 晚间 资源开发
        {
            YX_ResourceDevelopShow yxrds = new YX_ResourceDevelopShow();
            yxrds.qqType = "个人QQ";
            yxrds.groupName = "晚间";
            yxrds.Text = this.buttonX22.Text;
            yxrds.Show();
        }

        private void buttonX21_Click(object sender, EventArgs e)//个人QQ 晚间 QQ群
        {
            YX_ResourceGroup yxrg = new YX_ResourceGroup();
            yxrg.groupName = "晚间";
            yxrg.Text = this.buttonX21.Text;
            yxrg.Show();
        }

        private void buttonX20_Click(object sender, EventArgs e)//个人QQ 晚间 QQ空间
        {
            YX_QQZone yxqqz = new YX_QQZone();
            yxqqz.groupName = "晚间";
            yxqqz.Text = this.buttonX20.Text;
            yxqqz.Show();
        }

        private void buttonX23_Click(object sender, EventArgs e)//每日好评
        {
            YX_EverydayEvaluateShow yees = new YX_EverydayEvaluateShow();
            yees.Text = this.buttonX23.Text;
            yees.Show();
        }

        private void buttonX24_Click(object sender, EventArgs e)//表格汇总
        {
            YX_TableCollectShow txtcs = new YX_TableCollectShow();
            txtcs.Text = this.buttonX24.Text;
            txtcs.Show();
        }

        private void buttonX25_Click(object sender, EventArgs e)//个人总结
        {
            YX_EveryDaySummaryShow yedss = new YX_EveryDaySummaryShow();
            yedss.qqType = "个人QQ";
            yedss.groupName = "早间";
            yedss.Text = this.buttonX25.Text; ;
            yedss.Show();
        }

        private void buttonX27_Click(object sender, EventArgs e)//个人报名
        {
            YX_JXManager yxjxm = new YX_JXManager();
            YX_JXManager.submitBtnShow = true;
            yxjxm.Text = this.buttonX27.Text;
            yxjxm.Show();
        }

        private void buttonX28_Click(object sender, EventArgs e)
        {
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.qqDetail = this.textBoxX4.Text.Trim();
            sds.qqDetailTBName = "[UTCRMDB].[dbo].[KeQQForeignRecord]";
            sds.Show();
        }

        //private void buttonX42_Click(object sender, EventArgs e)//付费记录   --去除
        //{
        //    YX_FFJLTable yxffjl = new YX_FFJLTable();
        //    yxffjl.Text = this.buttonX42.Text;
        //    yxffjl.Show();
        //}

        //private void buttonX43_Click(object sender, EventArgs e)//监控机构   --去除
        //{
        //    YX_JKJGTable yxjk = new YX_JKJGTable();
        //    yxjk.Text = this.buttonX43.Text;
        //    yxjk.Show();
        //}
    }
}
