using Dapper;
using FunkyBank.Interfaces;
using FunkyBank.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunkyBank.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseConfig _config;
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILogger<CustomerRepository> _logger;

        public CustomerRepository(DatabaseConfig config, IDbConnectionFactory connectionFactory, ILogger<CustomerRepository> logger)
        {
            if (config == null || string.IsNullOrEmpty(config.ConnectionString))
            {
                throw new ArgumentException(nameof(config));
            }

            _config = config;
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public async Task<OperationResult<List<Customer>>> GetCustomersAsync()
        {
            _logger.LogInformation($"Called {nameof(GetCustomerAsync)}");
            
            try
            {
                using (var connection = _connectionFactory.GetConnection(_config.ConnectionString))
                {
                    var customers = await connection.QueryAsync<Customer>("select * from customers")
                        .ConfigureAwait(false);
                    return OperationResult<List<Customer>>.Success(customers.ToList());
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error: Cannot rerieve customers");
                return OperationResult<List<Customer>>.Failure("Cannot retrieve customers");
            }
        }

        public async Task<OperationResult<Customer>> GetCustomerAsync(int customerId)
        {
            _logger.LogInformation($"Calling {nameof(GetCustomerAsync)}");
            try
            {
                using (var connection = _connectionFactory.GetConnection(_config.ConnectionString))
                {
                    var customer = await connection.QuerySingleOrDefaultAsync<Customer>(
                            "select * from customers where id=@customerId", new {customerId});
                    if (customer == null)
                    {
                        _logger.LogInformation($"Customer does not exist: {customerId}");
                        return OperationResult<Customer>.Failure("Customer does not exist");
                    }

                    return OperationResult<Customer>.Success(customer);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,$"Error: cannot retrieve customer: {customerId}");
                
                return OperationResult<Customer>.Failure("Cannot retrieve customer");
            }
        }

        public async Task<OperationResult<Customer>> CreateCustomerAsync(Customer customer)
        {
            _logger.LogInformation($"Calling {nameof(CreateCustomerAsync)}");
            
            if (customer == null)
            {
                _logger.LogError("Error: Cannot create customer from a null object");
                return OperationResult<Customer>.Failure("Cannot create customer from a null object");
            }

            try
            {
                const string query = "insert into customers (Name, Address) " +
                                     "output inserted.Id, inserted.Name, inserted.Address " +
                                     "values (@Name, @Address)";
                
                using (var connection = _connectionFactory.GetConnection(_config.ConnectionString))
                {
                    var insertedCustomers = await connection.QueryAsync<Customer>(query, customer);
                    var insertedCustomer = insertedCustomers.First();
                    _logger.LogInformation($"Customer created successfully: {JsonConvert.SerializeObject(insertedCustomer)}");

                    return OperationResult<Customer>.Success(insertedCustomer);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error: Cannot create customer");

                return OperationResult<Customer>.Failure("Cannot create customer");
            }
        }

        public async Task<OperationResult<Customer>> UpdateCustomerAsync(Customer customer)
        {
            _logger.LogInformation($"Calling {nameof(UpdateCustomerAsync)}");

            if (customer == null)
            {
                _logger.LogError("Error: Cannot update customer from a null object");
                return OperationResult<Customer>.Failure("Cannot update customer from  a null object");
            }

            try
            {
                const string query = "update customers set name=@Name, Address=@Address " +
                                     "output inserted.Id, inserted.Name, inserted.Address " +
                                     "where id=@Id";
                
                using (var connection = _connectionFactory.GetConnection(_config.ConnectionString))
                {
                    var updatedCustomer = await connection.QueryFirstOrDefaultAsync<Customer>(query, customer);

                    if (updatedCustomer == null)
                    {
                        _logger.LogError("Error: Customer does not exist to update");
                        return OperationResult<Customer>.Failure("Customer does not exist to update");
                    }

                    return OperationResult<Customer>.Success(updatedCustomer);
                }
            }
            catch(Exception exception)
            {
                _logger.LogError(exception,"Error: Cannot update customer");
                return OperationResult<Customer>.Failure("Cannot update customer");
            }
        }

        public async Task<OperationResult> DeleteCustomerAsync(int customerId)
        {
            _logger.LogInformation($"Calling {nameof(DeleteCustomerAsync)}");
            
            try
            {
                const string query = "delete customers" +
                                     "output deleted.Id, deleted.Name, deleted.Address" +
                                     "where Id=@customerId";
                
                using (var connection = _connectionFactory.GetConnection(_config.ConnectionString))
                {
                    var deletedCustomer = await connection.QueryFirstOrDefaultAsync<Customer>(query, new {customerId});
                    if (deletedCustomer == null)
                    {
                        _logger.LogError($"Error: Customer does not exist: {customerId}");

                        return OperationResult<Customer>.Failure("Customer does not exist");
                    }

                    return OperationResult<Customer>.Success();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception,"Error: cannot delete customer");
                return OperationResult<Customer>.Failure("Cannot delete customer");
            }
        }
    }
}