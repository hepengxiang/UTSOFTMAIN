using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace UTSOFTMAIN.福利市场
{
    public partial class FLSCSoftManager : Form
    {
        public FLSCSoftManager()
        {
            InitializeComponent();
        }
        private DataTable dtSoft = new DataTable();
        private void FLSCSoftManager_Load(object sender, EventArgs e)
        {
            dtSoft = DBHelper.ExecuteQuery("select SoftWareName,UnitPrice from FuLiShiChangSWPrice");
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(frmUTSOFTMAIN.dataGridViewX_RowPostPaint);
        }

        private void dataGridViewX1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            this.dateTimeInput2.Value = DateTime.Parse(this.dataGridViewX1.SelectedRows[0].Cells[1].Value.ToString());
            this.textBoxX1.Text = this.dataGridViewX1.SelectedRows[0].Cells[2].Value.ToString();
            this.textBoxX2.Text = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            this.comboBoxEx1.Text = this.dataGridViewX1.SelectedRows[0].Cells[4].Value.ToString();
            this.textBoxX3.Text = this.dataGridViewX1.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text == "" || this.textBoxX3.Text == "")
            {
                MessageBox.Show("请填写完整");
                return;
            }
            string sql1 = string.Format("insert into FuLiShiChangTable_Temp values('{0}','{1}','{2}','{3}',{4})",
                this.dateTimeInput2.Value.ToShortDateString(),
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text,
                this.comboBoxEx1.Text,
                this.textBoxX3.Text);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("增加成功");
                string sql2 = string.Format("select * from FuLiShiChangTable_Temp where QQ = '{0}'  order by id asc", 
                    this.textBoxX1.Text.Trim());
                this.dataGridViewX1.DataSource = DBHelper.ExecuteQuery(sql2);
                this.labelX7.Text = "总价：   " + sumPrice(this.dataGridViewX1, 5) + "   元";
            }
            else
            {
                MessageBox.Show("此记录已存在");
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选定数据");
                return;
            }
            if (MessageBox.Show("你确定要删除选中行的数据吗?", "删除提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("delete from FuLiShiChangTable_Temp where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString());
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("删除成功");
                this.dataGridViewX1.Rows.Remove(this.dataGridViewX1.SelectedRows[0]);
                this.labelX7.Text = "总价：   " + sumPrice(this.dataGridViewX1, 5) + "   元";
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (this.textBoxX1.Text == "" || this.comboBoxEx1.Text == "")
            {
                MessageBox.Show("QQ号和软件名称为必填项");
                return;
            }
            if (this.dataGridViewX1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请先选定要修改的记录");
                return;
            }
            if (MessageBox.Show("你确定要修改选中行的数据吗?", "修改提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            string sql1 = string.Format("update FuLiShiChangTable_Temp " +
                "set BuyDates = '{1}',QQ = '{2}',WWNum = '{3}',SoftWareName = '{4}',Price = {5} where id = {0}",
                this.dataGridViewX1.SelectedRows[0].Cells[0].Value.ToString(),
                this.dateTimeInput2.Value.ToShortDateString(),
                this.textBoxX1.Text.Trim(),
                this.textBoxX2.Text,
                this.comboBoxEx1.Text,
                this.textBoxX3.Text);
            int resultNum = DBHelper.ExecuteUpdate(sql1);
            if (resultNum > 0)
            {
                MessageBox.Show("修改成功");
                this.dataGridViewX1.SelectedRows[0].Cells[1].Value = this.dateTimeInput2.Value.ToShortDateString();
                this.dataGridViewX1.SelectedRows[0].Cells[2].Value = this.textBoxX1.Text.Trim();
                this.dataGridViewX1.SelectedRows[0].Cells[3].Value = this.textBoxX2.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[4].Value = this.comboBoxEx1.Text;
                this.dataGridViewX1.SelectedRows[0].Cells[5].Value = this.textBoxX3.Text;
                this.labelX7.Text = "总价：   " + sumPrice(this.dataGridViewX1, 5) + "   元";
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            string sql1 = string.Format("select * from FuLiShiChangTable_Temp where BuyDates between '{0}' and '{1}' " +
                "and QQ like '%{2}%' and WWNum like '%{3}%' and SoftWareName like '%{4}%' order by id asc",
                this.dateTimeInput1.Value.ToShortDateString(),
                this.dateTimeInput2.Value.ToShortDateString(),
                this.textBoxX1.Text.Trim(), this.textBoxX2.Text, this.comboBoxEx1.Text);
            DataTable dt1 = DBHelper.ExecuteQuery(sql1);
            this.dataGridViewX1.DataSource = dt1;
            this.labelX7.Text = "总价：   " + sumPrice(this.dataGridViewX1, 5) + "   元";
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要导出数据至excel表格吗?", "导出提醒", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            if (this.dataGridViewX1.Rows.Count > 0)
            {
                if (Directory.Exists(@"D:\\福利市场返现统计表(软件导出)") == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(@"D:\\福利市场返现统计表(软件导出)");
                }
                try
                {
                    dategridaviewToCsv(this.dataGridViewX1, @"D:\\福利市场返现统计表(软件导出)\" +
                    System.DateTime.Now.ToString("yyyyMMdd") + "福利市场返现统计表.xls");

                    MessageBox.Show("导出完成,路径为：D:\\\\福利市场返现统计表(软件导出)\\" +
                    System.DateTime.Now.ToString("yyyyMMdd") + "福利市场返现统计表.xls");
                    return;
                }
                catch
                {
                    MessageBox.Show("请先关闭对应导出日期的excel表程序");
                    return;
                }
            }
        }

        private void comboBoxEx1_DropDown(object sender, EventArgs e)
        {
            string sql = string.Format("");
            string[] columNames = new string[] { "SoftWareName", "Price" };
            DataTable dtPerson = tools.dtFilter(dtSoft, columNames, sql);
            if (dtPerson.Rows.Count == 0)
                return;
            this.comboBoxEx1.DataSource = dtPerson;
            this.comboBoxEx1.ValueMember = "UnitPrice";
            this.comboBoxEx1.DisplayMember = "SoftWareName";
            this.comboBoxEx1.SelectedIndex = -1;
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxEx1.Text != "")
                this.textBoxX3.Text = this.comboBoxEx1.ValueMember;
            else
                this.textBoxX3.Text = "";
        }
        private double sumPrice(DevComponents.DotNetBar.Controls.DataGridViewX dgv, int index)
        {
            double sumNum = 0;
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                sumNum += double.Parse(dgv.Rows[i].Cells[index].Value.ToString());
            }
            return sumNum;
        }
        /// <summary>
        /// 导出DataGridView到excel
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="file"></param>
        public void dategridaviewToCsv(DevComponents.DotNetBar.Controls.DataGridViewX dgv, string file)
        {
            string title = "";
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            //FileStream fs1 = File.Open(file, FileMode.Open, FileAccess.Read);
            StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                title += dgv.Columns[i].HeaderText + "\t"; //栏位：自动跳到下一单元格
            }
            if (title != "")
            {
                title = title.Substring(0, title.Length - 1) + "\n";
                sw.Write(title);
                for (int rowNum = 0; rowNum < dgv.Rows.Count; rowNum++)
                {
                    string line = "";
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        line += dgv.Rows[rowNum].Cells[i].Value.ToString().Trim() + "\t"; //内容：自动跳到下一单元格
                    }
                    line = line.Substring(0, line.Length - 1) + "\n";
                    sw.Write(line);
                }
            }
            sw.Close();
            fs.Close();
        }
    }
}
