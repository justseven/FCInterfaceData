using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OldCGLPB
{
    public partial class Form1 : Form
    {
        private static string SXK = ConfigurationManager.ConnectionStrings["bdcsxkConnection"].ConnectionString;
        public Form1()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(DialogResult.Yes==MessageBox.Show("是否导入?","提示",MessageBoxButtons.YesNo))
            {
                int index = e.RowIndex;
                if ((null != dataGridView1.Rows[index].Cells[1].Value && dataGridView1.Rows[index].Cells[1].Value.ToString() == "1") || (null != dataGridView1.Rows[index].Cells[2].Value && dataGridView1.Rows[index].Cells[2].Value.ToString() == "1"))
                {
                    MessageBox.Show("禁止操作");
                }
                else
                {
                    DataGridViewRow row = dataGridView1.Rows[index];
                    FC_H_QSDC h = CreateFC_H_QSDC(row);
                    InsertIntoBDC(h);
                    MessageBox.Show("导入成功!");
                }
                
            }
        }

        private FC_H_QSDC CreateFC_H_QSDC(DataGridViewRow row)
        {
            FC_H_QSDC h = new FC_H_QSDC();

            h.LSZTYBM = null == row.Cells["BuildingInfo_id"].Value ? null : row.Cells["BuildingInfo_id"].Value.ToString();
            h.TSTYBM= null == row.Cells["HouseInfo_id"].Value ? null : row.Cells["HouseInfo_id"].Value.ToString();
            h.DYH = null==row.Cells["H_CeCode"].Value ? null : row.Cells["H_CeCode"].Value.ToString();
            h.FJH= null == row.Cells["H_RoNum"].Value ? null : row.Cells["H_RoNum"].Value.ToString();
            h.GHYT = ConvertToGHYT(row.Cells["H_HoUse"]);
            h.JZMJ = GetJZMJ(row.Cells["H_ConAcre"]);
            h.ZL = GetZL(row.Cells["I_Dist"], row.Cells["I_ItSite"], row.Cells["I_ItName"], row.Cells["BuName"], row.Cells["H_CeCode"], row.Cells["H_RoNum"]);

            return h;


        }

        private string GetZL(DataGridViewCell dataGridViewCell1, DataGridViewCell dataGridViewCell2, DataGridViewCell dataGridViewCell3, DataGridViewCell dataGridViewCell4, DataGridViewCell dataGridViewCell5, DataGridViewCell dataGridViewCell6)
        {
            return "徐州市"+dataGridViewCell1.Value + dataGridViewCell2.Value + dataGridViewCell3.Value + dataGridViewCell4.Value + dataGridViewCell5.Value +"-"+ dataGridViewCell6.Value;
        }

        private decimal? GetJZMJ(DataGridViewCell dataGridViewCell)
        {
            if(null==dataGridViewCell.Value)
            {
                return null;
            }
            else
            {
                return Convert.ToDecimal(dataGridViewCell.Value);
            }
        }

        private string ConvertToGHYT(DataGridViewCell dataGridViewCell)
        {
            if(null== dataGridViewCell.Value)
            {
                return "80";
            }
            else
            {
                return ChangeYT(dataGridViewCell.Value.ToString());
            }
        }

        private void InsertIntoBDC(FC_H_QSDC h)
        {
            try
            {
                DbHelper.Conn = new Oracle.DataAccess.Client.OracleConnection(SXK);
                DbHelper.SetProvider(MyDBType.Oracle);
                string sql = DbHelper.CreateInsertStr<FC_H_QSDC>(h, "FC_H_QSDC", MyDBType.Oracle);
                sql = sql.Trim().Replace("\r\n", "");
                DbParameter[] param = DbHelper.GetParamArray<FC_H_QSDC>(h, MyDBType.Oracle);

                DbHelper.ExecuteNonQuery(DbHelper.Conn, System.Data.CommandType.Text, sql, param);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally { DbHelper.CloseConn(); }
        }

        private Oracle.DataAccess.Client.OracleParameter[] GetParams(FC_H_QSDC fc_dyxx)
        {
            return DbHelper.GetParamArray<FC_H_QSDC>(fc_dyxx);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string itemName = txtXMMC.Text.Trim();
            string zh = txtZH.Text.Trim();
            string dyh = txtDYH.Text.Trim();
            string fjh = txtFJH.Text.Trim();

            DataTable dt = GetLPBFromCG(itemName, zh, dyh, fjh);
            dataGridView1.DataSource = dt;
        }

        private DataTable GetLPBFromCG(string itemName, string zh, string dyh, string fjh)
        {
           string sql= @"SELECT *
  FROM[HPS].[dbo].[房屋楼盘表]
  where 1=1 ";
            sql += GetWhere(itemName, zh, dyh, fjh);
            try
            {
                DbHelper.SetProvider(MyDBType.Sql);
                return DbHelper.ExecuteTable(MyDBType.Sql, CommandType.Text, sql, null);
            }
            catch (Exception ex){ throw ex; }
            finally { DbHelper.CloseConn(); }
        }

        private string GetWhere(string itemName, string zh, string dyh, string fjh)
        {
            string sql = "";
            if(!string.IsNullOrEmpty(itemName))
            {
                sql += string.Format(" and I_ItName like '%{0}%' ", itemName);
            }
            if (!string.IsNullOrEmpty(zh))
            {
                sql += string.Format(" and BuName like '%{0}%' ", zh);
            }
            if (!string.IsNullOrEmpty(dyh))
            {
                sql += string.Format(" and H_CeCode like '%{0}%' ", dyh);
            }
            if (!string.IsNullOrEmpty(fjh))
            {
                sql += string.Format(" and H_RoNum like '%{0}%' ", fjh);
            }
            return sql;
        }

        private  bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        private string ChangeYT(string ghyt)
        {
            string input = ghyt.Trim();
            if (IsInt(input))
                return input;
            else if (input == "住宅")
                return "10";
            else if (input == "仓储")
                return "101";
            else if (input == "成套住宅")
                return "11";
            else if (input == "别墅")
                return "111";
            else if (input == "高档公寓")
                return "112";
            else if (input == "公寓")
                return "113";
            else if (input == "非成套住宅、阁楼")
                return "12";
            else if (input == "集体宿舍")
                return "13";
            else if (input == "车库")
                return "14";
            else if (input == "地下室")
                return "15";
            else if (input == "储藏室")
                return "17";
            else if (input == "阁楼")
                return "18";
            else if (input == "工业、交通、仓储")
                return "20";
            else if (input == "工业")
                return "21";
            else if (input == "共用设施")
                return "21";
            else if (input == "铁路")
                return "23";
            else if (input == "民航")
                return "24";
            else if (input == "航运")
                return "25";
            else if (input == "公共运输")
                return "26";
            else if (input == "仓储、车库位、储藏、车库" || input == "储藏" || input == "仓储" || input == "车库位" || input == "车位" || input == "车库")
                return "27";
            else if (input == "商业、金融、信息" || input == "商业" || input == "金融" || input == "信息")
                return "30";
            else if (input == "商业服务")
                return "31";
            else if (input == "经营")
                return "32";
            else if (input == "旅游")
                return "33";
            else if (input == "金融保险")
                return "34";
            else if (input == "电讯信息")
                return "35";
            else if (input == "教育、医疗、卫生、科研")
                return "40";
            else if (input == "教育")
                return "41";
            else if (input == "医疗卫生")
                return "42";
            else if (input == "科研")
                return "43";
            else if (input == "文化、娱乐、体育")
                return "89B";
            else if (input == "文化")
                return "51";
            else if (input == "新闻")
                return "52";
            else if (input == "娱乐")
                return "53";
            else if (input == "园林绿化")
                return "54";
            else if (input == "体育")
                return "55";
            else if (input == "办公")
                return "60";
            else if (input == "军事")
                return "70";
            else if (input == "其他")
                return "80";
            else if (input == "涉外")
                return "81";
            else if (input == "宗教")
                return "82";
            else if (input == "监狱")
                return "83";
            else if (input == "物管用房")
                return "84";
            else if (input == "公共设施、人防")
                return "85";
            else if (input == "储藏")
                return "87";
            else if (input == "会所")
                return "86";
            else if (input == "人防")
                return "85";
            else if (input == "幼托")
                return "88";
            else if (input == "开闭所")
                return "89A";
            else if (input == "配电间")
                return "89D";
            else if (input == "架空层")
                return "架空层";
            else
                return "80";
        }
    }
}
