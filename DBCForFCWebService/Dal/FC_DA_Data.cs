using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace DBCForFCWebService
{
    public class FC_DA_Data
    {
        public NewDataSet GetNewDataSet(string name,string cardNo) {
            string sql = string.Format("Select * from FC_Da_FWZM Where Name='{0}' and CardNo='{1}'", name, cardNo);
            DataSet ds = OleDBHelper.GetDataSet(sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                IList<Table> tables = new List<Table>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                    Table t = new Table();
                    t.ProveResultID = ds.Tables[0].Rows[i]["ProveResultID"].ToString();
                    t.BusiID = ds.Tables[0].Rows[i]["BusiID"].ToString();
                    t.CardNo = ds.Tables[0].Rows[i]["CardNo"].ToString();
                    t.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                    t.HouseSite = ds.Tables[0].Rows[i]["HouseSite"].ToString();
                    t.Source = ds.Tables[0].Rows[i]["Source"].ToString();
                    t.SourceDes = ds.Tables[0].Rows[i]["SourceDes"].ToString();
                    t.Area = ds.Tables[0].Rows[i]["Area"].ToString();
                    t.RightNo = ds.Tables[0].Rows[i]["RightNo"].ToString();
                    tables.Add(t);
                }
                NewDataSet set1 = new NewDataSet();
                set1.Tables = tables.ToArray();
                return set1;
            }
            else {
                return null;
            }
        }
    }
    [Serializable]
    public class NewDataSet {
        public Table[] Tables { get; set; }

        public string NOResult { get; set; }
    }
    [Serializable]
    public class Table {
        public string ProveResultID { get; set; }
        public string BusiID { get; set; }
        public string CardNo { get; set; }
        public string HouseSite { get; set; }
        public string Source { get; set; }
        public string SourceDes { get; set; }
        public string Area { get; set; }
        public string RightNo { get; set; }

        public string Name { get; set; }
    }
}