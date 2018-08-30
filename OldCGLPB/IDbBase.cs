using System.Data.Common;

namespace OldCGLPB
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
