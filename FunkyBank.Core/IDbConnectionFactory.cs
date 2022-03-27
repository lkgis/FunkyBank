using System.Data;

namespace FunkyBank
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection(String connectionString);
    }
}