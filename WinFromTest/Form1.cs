using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Web4BDC.Bll;
using Web4BDC.Models;

namespace WinFromTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PageParams param = new PageParams();
            //param.PrjId = textBox1.Text.Trim();
            //FCDA_BLL.GetCanInsertSlbh();
            //try
            //{
            //    BDCFilterResult res=FCDA_BLL.Insert_FCDA(param);
            //    MessageBox.Show(res.IsSuccess + ":" + res.Message);
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
