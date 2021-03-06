using FunkyBank.Interfaces;
using FunkyBank.Models;
using FunkyBank.Requests;
using FunkyBank.Responses;
using Microsoft.Extensions.Logging;

namespace FunkyBank
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, ILogger<CustomerService> logger)
        {
            if (customerRepository == null)
            {
                throw new ArgumentNullException(nameof(customerRepository));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _customerRepository = customerRepository;
            _logger = logger;
        }
        public async Task<OperationResult<CreateCustomerResponse>> CreateCustomerAsync(CreateCustomerRequest request)
        {
            _logger.LogInformation($"Calling {nameof(CreateCustomerAsync)}");

            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError("Error: Invalid request");
                return OperationResult<CreateCustomerResponse>.Failure("Invalid request. Cannot create customer");
            }

            var operationResult = await _customerRepository.CreateCustomerAsync(new Customer
            {
                Name = request.Name,
                Address = request.Address
            }).ConfigureAwait(false);

            if (operationResult.Status)
            {
                _logger.LogInformation("Customer created successfully");

                var createdCustomer = operationResult.Data;
                return OperationResult<CreateCustomerResponse>.Success(
                    new CreateCustomerResponse(new CustomerDisplayModel(createdCustomer.Name,
                        createdCustomer.Address)));
            }
            
            _logger.LogInformation("Error: Cannot create customer");
            return OperationResult<CreateCustomerResponse>.Failure("Error: Cannot create customer");
        }

        public async Task<OperationResult<UpdateCustomerResponse>> UpdateCustomerAsync(UpdateCustomerRequest request)
        {
            _logger.LogInformation($"Calling {nameof(UpdateCustomerAsync)}");
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError("Error: Invalid request, cannot update customer");
                return OperationResult<UpdateCustomerResponse>.Failure("Invalid request. Cannot update customer");
            }

            var operationResult = await _customerRepository.UpdateCustomerAsync(new Customer
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address
            }).ConfigureAwait(false);

            if (operationResult.Status)
            {
                _logger.LogInformation("Customer updated successfully");

                var updatedCustomer = operationResult.Data;
                return OperationResult<UpdateCustomerResponse>.Success(
                    new UpdateCustomerResponse(new CustomerDisplayModel(updatedCustomer.Name,
                        updatedCustomer.Address)));
            }
            
            _logger.LogError("Error: Cannot update customer");
            return OperationResult<UpdateCustomerResponse>.Failure("Cannot update customer");
        }

        public async Task<OperationResult<GetCustomersResponse>> GetCustomersAsync(GetCustomersRequest request)
        {
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError("Error: Invalid request");
                return OperationResult<GetCustomersResponse>.Failure("Invalid request");
            }

            var operationResult = await _customerRepository.GetCustomersAsync().ConfigureAwait(false);

            if (operationResult.Status)
            {
                _logger.LogInformation("Retrieved customers successfully");

                var displayCustomers = operationResult.Data.Select(x => new CustomerDisplayModel(x.Name, x.Address));
                return OperationResult<GetCustomersResponse>.Success(new GetCustomersResponse(displayCustomers));
            }
            
            _logger.LogError("Error: Cannot get customers");
            return OperationResult<GetCustomersResponse>.Failure("Cannot get customers");
        }

        public async Task<OperationResult<CustomerDisplayModel>> GetSpecificCustomerAsync(GetSpecificCustomerRequest request)
        {
            var isValid = request.Validate();
            if (!isValid)
            {
                _logger.LogError("Error: Invalid request");
                return OperationResult<CustomerDisplayModel>.Failure("Invalid request");
            }

            var operationResults = await _customerRepository.GetCustomerAsync(request.Id).ConfigureAwait(false);

            if (operationResults.Status)
            {
                _logger.LogInformation("Customer retrieved successfully");

                return OperationResult<CustomerDisplayModel>.Success(
                    new CustomerDisplayModel(operationResults.Data.Name, operationResults.Data.Address));
            }
            
            _logger.LogError("Error: Cannot get specific customer");
            return OperationResult<CustomerDisplayModel>.Failure("Cannot get specific customer");
        }

        public async Task<OperationResult> DeleteCustomerAsync(DeleteCustomerRequest request)
        {
            var isValid = request.Validate();
            
            if (!isValid)
            {
                _logger.LogError("Error: Invalid request");
                return OperationResult.Failure("Invalid request");
            }

            var operationResult = await _customerRepository.DeleteCustomerAsync(request.Id).ConfigureAwait(false);

            if (operationResult.Status)
            {
                _logger.LogInformation("Customer deleted successfully");
                
                return OperationResult.Success();
            }
            
            _logger.LogError("Error: Cannot delete customer");
            
            return OperationResult.Failure("Cannot delete customer");
        }
    }
}