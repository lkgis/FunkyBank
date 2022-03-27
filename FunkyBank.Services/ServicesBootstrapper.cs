using Autofac;

namespace FunkyBank
{
    public class ServicesBootstrapper
    {
        public static void Register(ContainerBuilder builder, DatabaseConfig config)
        {
            builder.RegisterType<CustomerService>().As<ICustomerService>();

            DataAccessBootstrapper.Register(builder, config);
        }
    }
}