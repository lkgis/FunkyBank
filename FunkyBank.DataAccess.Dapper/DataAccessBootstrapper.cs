using Autofac;
using FunkyBank.Interfaces;
using FunkyBank.Repositories;

namespace FunkyBank
{
    public class DataAccessBootstrapper
    {
        public static void Register(ContainerBuilder builder, DatabaseConfig config)
        {
            builder.RegisterType<DbConnectionFactory>().As<IDbConnectionFactory>();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().WithParameter("config", config);
        }
    }
}