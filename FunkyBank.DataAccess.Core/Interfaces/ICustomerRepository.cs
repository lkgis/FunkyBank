using FunkyBank.Models;

namespace FunkyBank.Interfaces
{
    public interface ICustomerRepository
    {
        Task<OperationResult<List<Customer>>> GetCustomersAsync();
        Task<OperationResult<Customer>> GetCustomerAsync(int customerId);
        Task<OperationResult<Customer>> CreateCustomerAsync(Customer customer);
        Task<OperationResult<Customer>> UpdateCustomerAsync(Customer customer);
        Task<OperationResult> DeleteCustomerAsync(int customerId);
    }
}