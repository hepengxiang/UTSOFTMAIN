namespace UTSOFTMAIN
{
    partial class YX_PromoteAllShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewX1 = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column29 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column30 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column44 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column46 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewX1
            // 
            this.dataGridViewX1.AllowUserToAddRows = false;
            this.dataGridViewX1.AllowUserToDeleteRows = false;
            this.dataGridViewX1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewX1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewX1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewX1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column29,
            this.Column30,
            this.Column43,
            this.Column4,
            this.Column5,
            this.Column44,
            this.Column47,
            this.Column45,
            this.Column46});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewX1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewX1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridViewX1.Location = new System.Drawing.Point(1, 1);
            this.dataGridViewX1.Name = "dataGridViewX1";
            this.dataGridViewX1.ReadOnly = true;
            this.dataGridViewX1.RowTemplate.Height = 23;
            this.dataGridViewX1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewX1.Size = new System.Drawing.Size(1213, 476);
            this.dataGridViewX1.TabIndex = 1;
            this.dataGridViewX1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewX1_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "datediffNum";
            this.Column1.HeaderText = "跟踪时长";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "EnterTime";
            this.Column2.HeaderText = "初始日期";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 80;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "NickName";
            this.Column3.HeaderText = "QQ昵称";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 80;
            // 
            // Column29
            // 
            this.Column29.DataPropertyName = "QQ";
            this.Column29.HeaderText = "QQ号码";
            this.Column29.Name = "Column29";
            this.Column29.ReadOnly = true;
            this.Column29.Width = 80;
            // 
            // Column30
            // 
            this.Column30.DataPropertyName = "JoinSituation";
            this.Column30.HeaderText = "报名情况";
            this.Column30.Name = "Column30";
            this.Column30.ReadOnly = true;
            this.Column30.Width = 120;
            // 
            // Column43
            // 
            this.Column43.DataPropertyName = "PromoteResource";
            this.Column43.HeaderText = "意向来源";
            this.Column43.Name = "Column43";
            this.Column43.ReadOnly = true;
            this.Column43.Width = 80;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "StudyNumber";
            this.Column4.HeaderText = "学习次数";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "StudyTime";
            this.Column5.HeaderText = "学习时长";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 80;
            // 
            // Column44
            // 
            this.Column44.DataPropertyName = "PromoteGrade";
            this.Column44.HeaderText = "意向强度";
            this.Column44.Name = "Column44";
            this.Column44.ReadOnly = true;
            this.Column44.Width = 80;
            // 
            // Column47
            // 
            this.Column47.DataPropertyName = "TrackState";
            this.Column47.HeaderText = "跟踪状态";
            this.Column47.Name = "Column47";
            this.Column47.ReadOnly = true;
            this.Column47.Width = 80;
            // 
            // Column45
            // 
            this.Column45.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column45.DataPropertyName = "Remark";
            this.Column45.HeaderText = "意向情况";
            this.Column45.Name = "Column45";
            this.Column45.ReadOnly = true;
            // 
            // Column46
            // 
            this.Column46.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column46.DataPropertyName = "Trackplan";
            this.Column46.HeaderText = "跟踪计划";
            this.Column46.Name = "Column46";
            this.Column46.ReadOnly = true;
            // 
            // YXPromoteAllShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 478);
            this.Controls.Add(this.dataGridViewX1);
            this.Name = "YXPromoteAllShow";
            this.Text = "YXPromoteAllShow";
            this.Load += new System.EventHandler(this.YXPromoteAllShow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.DataGridViewX dataGridViewX1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column29;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column30;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column43;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column44;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column47;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column45;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column46;

    }
}