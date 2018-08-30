using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Web4BDC.Bll;
using Web4BDC.Models;

namespace SevenTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("请先选择数据源");
                return;
            }
            Inport(tableName);
        }

        private void Inport(string sourceType)
        {
            DataTable dt = GetSource(sourceType);
            int count = dt.Rows.Count;
            ThreadPool.QueueUserWorkItem(delegate (object obj)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if(Stop)
                    {
                        break;
                    }
                    string slbh = row["slbh"].ToString();
                    BDCFilterResult res = ReRecode(slbh);
                    if (null!=res)
                    {
                        this.Invoke(new Action(() =>
                        {

                            this.listBox1.Items.Add(slbh+":"+(res.IsSuccess?"成功":"失败")+"||"+res.Message);
                            this.listBox1.Items.Add("--------------------------------");
                            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;

                           
                        }));
                    }
                    --count;
                    this.Invoke(new Action(() =>
                    {
                        label1.Text = "剩余" + count + "条";
                    }));
                }
                MessageBox.Show("OK");
            });
        }

        private DataTable GetSource(string sourceType)
        {
            string sql = "select slbh from {0} where slbh like '201%' and slbh not like '%-%'";
            sql = string.Format(sql, sourceType);
            if(sourceType.ToLower().Equals("dj_sjd"))
            {
                sql += " and djdl not in ('800','900','600')";
            }
            sql += " order by slbh";
            DbHelper sqldb = new DbHelper();
            sqldb.SetProvider(MyDBType.Oracle);
            return sqldb.ExecuteTable(MyDBType.Oracle, CommandType.Text, sql, null);
        }

        private BDCFilterResult ReRecode(string slbh)
        {

            PageParams pg = new PageParams();
            pg.PrjId = slbh;
            //pg.UserId = FCDA_BLL.GetUserID(pg.PrjId.Trim());

            BDCFilterResult res= FCDA_BLL.Insert_FCDA(pg);
            return res;
        }
        string tableName = string.Empty;
        private void radZX_CheckedChanged(object sender, EventArgs e)
        {
            tableName = ((RadioButton)sender).Tag.ToString();
        }
        bool Stop = false;
        private void button2_Click(object sender, EventArgs e)
        {
            Stop = true;
        }
    }
}
