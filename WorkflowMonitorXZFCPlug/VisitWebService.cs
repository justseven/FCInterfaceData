using FCWebServices.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization; 

namespace WorkflowMonitorXZFCPlug
{
    public class VisitWebService
    {
        public DataSet UpdateSealStateForSPF(DataSet dsHouses, string Area)
        {
            //return string.Empty; 
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateSealStateForSPF(dsHouses, Area);
        }
        public DataSet UpdateMortgageStateForSPF(DataSet dsHouses, string Area)
        {
            //return string.Empty;
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateMortgageStateForSPF(dsHouses, Area);
        }
        public DataSet UpdateYGDJStateForSPF(DataSet dsHouses, string Area)
        {
            //return string.Empty;
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateYGDJStateForSPF(dsHouses, Area);
        }
        public DataSet UpdateCSDJStateForSPF(DataSet dsHouses, string Area)
        {
            //return string.Empty;
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateCSDJStateForSPF(dsHouses, Area);
        }

        public DataSet UpdateCSDJStateForCG(DataSet dsHouses, string Area)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateCSDJStateForCG(dsHouses, Area);
        }
        public DataSet UpdateMortgageStateForCG(DataSet dsHouses, string Area)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateMortgageStateForCG(dsHouses, Area);
        }
        public DataSet UpdateSealStateForCG(DataSet dsHouses, string Area)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateSealStateForCG(dsHouses, Area);
        }
        public DataSet UpdateYGDJStateForCG(DataSet dsHouses, string Area)
        {
            BDCSrvSoap soap = new BDCSrvSoapClient();
            return soap.UpdateYGDJStateForCG(dsHouses, Area);
        }




        //public string UpdateSealStateForSPF(DataSet dsHouses, string Area)
        //{
        //    return Serialize(dsHouses.Tables[0]);
        //}

        //public string UpdateMortgageStateForSPF(DataSet dsHouses, string Area)
        //{
        //    return Serialize(dsHouses.Tables[0]);
        //} 
        //public string UpdateYGDJStateForSPF(DataSet dsHouses, string Area)
        //{
        //    return Serialize(dsHouses.Tables[0]);
        //}

        //public string UpdateCSDJStateForSPF(DataSet dsHouses, string Area)
        //{
        //    return Serialize(dsHouses.Tables[0]);
        //} 
        //public static string Serialize(DataTable dt)
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        Dictionary<string, object> result = new Dictionary<string, object>();
        //        foreach (DataColumn dc in dt.Columns)
        //        {
        //            result.Add(dc.ColumnName, dr[dc].ToString());
        //        }
        //        list.Add(result);
        //    }
        //    return serializer.Serialize(list); ;
        //}
    }
}
