using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug
{
    public interface IGetDataBase
    {
        IDictionary<string, string> Data2DBAndReturnId(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch);
         
        string Data2DB(IDictionary<string, string> ps, IList<string> paramNeeded, DbConnection connection, string pch);
    }
}
