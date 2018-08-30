//using Geo.Plug.DataExchange;
using Geo.Plug.DataExchange.XZFCPlug;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {  
            IDictionary<string, string> ps = new Dictionary<string, string>();
            ps.Add("ExecuteCode", "0000");
            //ps.Add("HTBH", "2016020630037");
            //ps.Add("HTBH", "201601180002");
            //ps.Add("XMMC", "汇合");
            ps.Add("ZID", "5890166B-022A-42A8-AB90-709EE13FA8B7");
             //ps.Add("JZWMC", "4");  
            #if DEBUG
            Geo.Plug.DataExchange.XZFCPlug.IDataExchange dataExchange = new DataExchange();
#else
            Geo.Plug.DataExchange.IDataExchange dataExchange = new DataExchange();
            #endif
            dataExchange.DataExtort(ps);
        }
    }
}
