﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Web4BDC.Models.ZZJFModel;
using XMLHelper;

namespace SevenTest
{
    public partial class FrmZZDYTest : Form
    {
        public FrmZZDYTest()
        {
            InitializeComponent();
        }
        ZZDY_WS.ZZDZ_SUININGSoapClient ws = new ZZDY_WS.ZZDZ_SUININGSoapClient();
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource=ws.LND_PROC_GETCERTQUERY(txtZJHM.Text.Trim());
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            //ws.LND_PROC_GETCERTINFO
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否导入?", "提示", MessageBoxButtons.YesNo))
            {
                int index = e.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[index];
                string bdczh = row.Cells["BDCQZH"].Value.ToString();
                string ywh= row.Cells["YWH"].Value.ToString();
                string zjhm= row.Cells["QLRZJH"].Value.ToString();
                dataGridView2.DataSource=ws.LND_PROC_GETCERTINFO(ywh, zjhm, bdczh);
                tabControl1.SelectedIndex = 1;

            }
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            ws.LND_PROC_UPDATECERTSTATE(txtYWH.Text, txtQLRZJH.Text, txtBDCQZH.Text, 1, "自助打印机");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[index];
            string bdczh = row.Cells["BDCQZH"].Value.ToString();
            string ywh = row.Cells["YWH"].Value.ToString();
            string zjhm = row.Cells["QLRZJH"].Value.ToString();
            txtYWH.Text = ywh;
            txtQLRZJH.Text = zjhm;
            txtBDCQZH.Text = bdczh;
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            ws.LND_PROC_UPDATECERTINFO(txtYWH.Text, txtQLRZJH.Text, txtBDCQZH.Text, "王鹏", "32030219111001001X", "12345678901", null,"自助打印机");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    JFStateModel m = new JFStateModel(txtPrj.Text);
            //    txtXML.Text = m.SLBH + "_" + m.IS_SUCCESS + "_" + m.CODE + "_" + m.ZFJE;


            //}
            //catch(Exception ex)
            //{
            //    string str = ex.Message;
            //}


            string str = richTextBox1.Text;
            base64ToImage(str);

        }
        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

        private void base64ToImage(string base64str)
        {
            byte[] arr = Convert.FromBase64String(base64str);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);
            int w = bmp.Width;
            int h = bmp.Height;
            bmp.Save("C:\\DIV.PNG", ImageFormat.Png);
            this.pictureBox1.Image = bmp;
        }

        public static T ToObj<T>(string xml) //where T : Object
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader sw = new StringReader(xml);
            object obj = serializer.Deserialize(sw);
            if (obj is T)
            {
                return (T)obj;
            }
            else
            {
                return default(T);
            }
        }
    }
    
}
