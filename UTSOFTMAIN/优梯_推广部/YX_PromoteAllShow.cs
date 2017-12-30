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
    public partial class YX_PromoteAllShow : Form
    {
        public YX_PromoteAllShow()
        {
            InitializeComponent();
        }
        public static DataTable dgvdatasource;
        private void YXPromoteAllShow_Load(object sender, EventArgs e)
        {
            this.dataGridViewX1.DataSource = dgvdatasource;
            this.dataGridViewX1.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGridViewX_RowPostPaint1);
        }
        //显示dataGridViewX行号
        public static void dataGridViewX_RowPostPaint1(object sender, DataGridViewRowPostPaintEventArgs e)
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

        private void dataGridViewX1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dataGridViewX1.SelectedRows.Count == 0)
                return;
            frmUTSOFTMAIN.QQDetail = this.dataGridViewX1.SelectedRows[0].Cells[3].Value.ToString();
            frmUTSOFTMAIN.QQDetailTBName = "KeQQStudyRecord";
            CRMStudyDetailShow sds = new CRMStudyDetailShow();
            sds.Show();
        }
    }
}
