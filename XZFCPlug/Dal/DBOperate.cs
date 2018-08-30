using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Geo.Plug.DataExchange.XZFCPlug.Dal
{
    public class DBOperate
    {
        public static int InsertRows(IList<DbCommand> commands )
        { 
            return DBHelper.ExecuteTransaction(commands);
        }
    }
}
