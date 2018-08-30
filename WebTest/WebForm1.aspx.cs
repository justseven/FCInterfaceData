using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkflowMonitorXZFCPlug;

namespace WebTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{ViewName:{0},Address:{1},Data:{2}}", "viewName", "add", "DataTableToJson(dt)");
            //ReceiptFlow f = new ReceiptFlow();
            //f.ProcessSubmit("Wrk-160115144331-2A0B92RS",null);
        }
    }
}