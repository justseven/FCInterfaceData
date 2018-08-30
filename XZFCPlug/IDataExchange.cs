using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public interface IDataExchange
    {
        string DigitalSign(string userName, string userKey);
        string DataExtort(IDictionary<string, string> dicParam);

        string DataDelivery(IDictionary<string, string> dicParam);
    }
}
