namespace SevenTest
{
    partial class FrmFindDiff
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chkBDCZH = new System.Windows.Forms.CheckBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkQLR = new System.Windows.Forms.CheckBox();
            this.chkZT = new System.Windows.Forms.CheckBox();
            this.chkHYT = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkHYT);
            this.panel1.Controls.Add(this.chkZT);
            this.panel1.Controls.Add(this.chkQLR);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.chkBDCZH);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 123);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 327);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(800, 327);
            this.dataGridView1.TabIndex = 0;
            // 
            // chkBDCZH
            // 
            this.chkBDCZH.AutoSize = true;
            this.chkBDCZH.Location = new System.Drawing.Point(42, 12);
            this.chkBDCZH.Name = "chkBDCZH";
            this.chkBDCZH.Size = new System.Drawing.Size(84, 16);
            this.chkBDCZH.TabIndex = 0;
            this.chkBDCZH.Text = "证号不存在";
            this.chkBDCZH.UseVisualStyleBackColor = true;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(281, 85);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(374, 85);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 2;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // chkQLR
            // 
            this.chkQLR.AutoSize = true;
            this.chkQLR.Location = new System.Drawing.Point(132, 12);
            this.chkQLR.Name = "chkQLR";
            this.chkQLR.Size = new System.Drawing.Size(96, 16);
            this.chkQLR.TabIndex = 3;
            this.chkQLR.Text = "权利人不一致";
            this.chkQLR.UseVisualStyleBackColor = true;
            // 
            // chkZT
            // 
            this.chkZT.AutoSize = true;
            this.chkZT.Location = new System.Drawing.Point(234, 12);
            this.chkZT.Name = "chkZT";
            this.chkZT.Size = new System.Drawing.Size(108, 16);
            this.chkZT.TabIndex = 4;
            this.chkZT.Text = "权证状态不一致";
            this.chkZT.UseVisualStyleBackColor = true;
            // 
            // chkHYT
            // 
            this.chkHYT.AutoSize = true;
            this.chkHYT.Location = new System.Drawing.Point(348, 12);
            this.chkHYT.Name = "chkHYT";
            this.chkHYT.Size = new System.Drawing.Size(96, 16);
            this.chkHYT.TabIndex = 5;
            this.chkHYT.Text = "户用途不一致";
            this.chkHYT.UseVisualStyleBackColor = true;
            // 
            // FrmFindDiff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmFindDiff";
            this.Text = "FrmFindDiff";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkHYT;
        private System.Windows.Forms.CheckBox chkZT;
        private System.Windows.Forms.CheckBox chkQLR;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.CheckBox chkBDCZH;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}