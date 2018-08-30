namespace SevenTest
{
    partial class Form2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.radDJ = new System.Windows.Forms.RadioButton();
            this.radYG = new System.Windows.Forms.RadioButton();
            this.radDY = new System.Windows.Forms.RadioButton();
            this.radZX = new System.Windows.Forms.RadioButton();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.radZX);
            this.groupBox1.Controls.Add(this.radDY);
            this.groupBox1.Controls.Add(this.radYG);
            this.groupBox1.Controls.Add(this.radDJ);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 138);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 138);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(409, 312);
            this.panel1.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(409, 312);
            this.listBox1.TabIndex = 0;
            // 
            // radDJ
            // 
            this.radDJ.AutoSize = true;
            this.radDJ.Location = new System.Drawing.Point(35, 20);
            this.radDJ.Name = "radDJ";
            this.radDJ.Size = new System.Drawing.Size(71, 16);
            this.radDJ.TabIndex = 0;
            this.radDJ.TabStop = true;
            this.radDJ.Tag = "DJ_DJB";
            this.radDJ.Text = "登记数据";
            this.radDJ.UseVisualStyleBackColor = true;
            this.radDJ.CheckedChanged += new System.EventHandler(this.radZX_CheckedChanged);
            // 
            // radYG
            // 
            this.radYG.AutoSize = true;
            this.radYG.Location = new System.Drawing.Point(128, 20);
            this.radYG.Name = "radYG";
            this.radYG.Size = new System.Drawing.Size(71, 16);
            this.radYG.TabIndex = 1;
            this.radYG.TabStop = true;
            this.radYG.Tag = "DJ_YG";
            this.radYG.Text = "预告数据";
            this.radYG.UseVisualStyleBackColor = true;
            this.radYG.CheckedChanged += new System.EventHandler(this.radZX_CheckedChanged);
            // 
            // radDY
            // 
            this.radDY.AutoSize = true;
            this.radDY.Location = new System.Drawing.Point(217, 20);
            this.radDY.Name = "radDY";
            this.radDY.Size = new System.Drawing.Size(71, 16);
            this.radDY.TabIndex = 2;
            this.radDY.TabStop = true;
            this.radDY.Tag = "DJ_DY";
            this.radDY.Text = "抵押数据";
            this.radDY.UseVisualStyleBackColor = true;
            this.radDY.CheckedChanged += new System.EventHandler(this.radZX_CheckedChanged);
            // 
            // radZX
            // 
            this.radZX.AutoSize = true;
            this.radZX.Location = new System.Drawing.Point(303, 20);
            this.radZX.Name = "radZX";
            this.radZX.Size = new System.Drawing.Size(71, 16);
            this.radZX.TabIndex = 3;
            this.radZX.TabStop = true;
            this.radZX.Tag = "DJ_SJD";
            this.radZX.Text = "所有数据";
            this.radZX.UseVisualStyleBackColor = true;
            this.radZX.CheckedChanged += new System.EventHandler(this.radZX_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(3, 117);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(403, 18);
            this.progressBar1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(87, 54);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "导入";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(181, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(213, 54);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "停止";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RadioButton radZX;
        private System.Windows.Forms.RadioButton radDY;
        private System.Windows.Forms.RadioButton radYG;
        private System.Windows.Forms.RadioButton radDJ;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
    }
}