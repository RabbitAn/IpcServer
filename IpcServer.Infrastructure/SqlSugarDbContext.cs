using Microsoft.Extensions.Configuration;
using SqlSugar;

namespace IpcServer.Infrastructure;

public class SqlSugarDbContext
{
    public SqlSugarClient Db { get; }
    public SqlSugarDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        Console.WriteLine(connectionString);
        Db = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = DbType.PostgreSQL, 
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute
        });
    }
}