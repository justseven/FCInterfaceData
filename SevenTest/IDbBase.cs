using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace SevenTest
{
    public interface IDbBase
    {
        DbCommand CreateCommand();
        DbConnection CreateConnection();

        DbConnection CreateConnection(string connStr);
        DbDataAdapter CreateDataAdapter();
        DbParameter CreateParameter();
    }
}
