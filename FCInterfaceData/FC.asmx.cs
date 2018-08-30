
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using FCInterfaceData.Dal;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using Geo.Plug.DataExchange.XZFCPlug;

namespace FCInterfaceData
{ 
    /// <summary>
    /// FC 的摘要说明
    /// </summary>
    [WebService(Namespace = "")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class FC : System.Web.Services.WebService
    { 
        [WebMethod]
        public string HelloWorld(string hello)
        {
            return "Hello World";
        }
        [WebMethod]
        public BDC FC_SPFYGHT()
        {
            IGetDbData dbdata = new FC_SPFYGHT();
            BDC bdc = new BDC();
            bdc.head = new Head();
            bdc.head.flag = 1;
            bdc.head.msg = " ";
            bdc.data = new MyDataTable();
            bdc.data.dt=dbdata.GetDataTable();    
            return bdc;
        }
        [WebMethod]
        public BDC FC_Z(string Param)
        {
            IGetDbData dbdata = new FC_Z();
            BDC bdc = new BDC();
            bdc.head = new Head();
            bdc.head.flag = 1;
            bdc.head.msg = " ";
            bdc.data = new MyDataTable();
            bdc.data.dt = dbdata.GetDataTable();
            return bdc;
        }
        [WebMethod]
        public BDC FC_H()
        {
            IGetDbData dbdata = new FC_H();
            BDC bdc = new BDC();
            bdc.head = new Head();
            bdc.head.flag = 1;
            bdc.head.msg = " ";
            bdc.data = new MyDataTable();
            bdc.data.dt = dbdata.GetDataTable();
            return bdc;
        }
        [WebMethod]
        public BDC FC_GFQLRXX()
        {
            IGetDbData dbdata = new FC_GFQLRXX();
            BDC bdc = new BDC();
            bdc.head = new Head();
            bdc.head.flag = 1;
            bdc.head.msg = " ";
            bdc.data = new MyDataTable();
            bdc.data.dt = dbdata.GetDataTable();
            return bdc;
        }
        [WebMethod]
        public BDC FC_CLMMHT()
        {
            IGetDbData dbdata = new FC_CLMMHT();
            BDC bdc = new BDC();
            bdc.head = new Head();
            bdc.head.flag = 1;
            bdc.head.msg = " ";
            bdc.data = new MyDataTable();
            bdc.data.dt = dbdata.GetDataTable();
            return bdc;
        }
    }

}
