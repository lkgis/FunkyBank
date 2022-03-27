using FunkyBank.Models;
using FunkyBank.Requests;
using FunkyBank.Responses;

namespace FunkyBank
{
    public interface ICustomerService
    {
        Task<OperationResult<CreateCustomerResponse>> CreateCustomerAsync(CreateCustomerRequest request);
        Task<OperationResult<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request);
        Task<OperationResult<GetCustomersResponse>> GetCustomersAsync(GetCustomersRequest request);
        Task<OperationResult<CustomerDisplayModel>> GetSpecificCustomerAsync(GetSpecificCustomerRequest request);
        Task<OperationResult> DeleteCustomerAsync(DeleteCustomerRequest request);
    }
}