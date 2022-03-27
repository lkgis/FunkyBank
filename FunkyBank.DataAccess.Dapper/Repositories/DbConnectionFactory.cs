using System.Data;
using System.Data.SqlClient;

namespace FunkyBank.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

    }
}