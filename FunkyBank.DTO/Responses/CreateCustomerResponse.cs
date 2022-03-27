using FunkyBank.Models;

namespace FunkyBank.Responses
{
    public class CreateCustomerResponse
    {
        public CustomerDisplayModel Customer { get; }

        public CreateCustomerResponse(CustomerDisplayModel customer)
        {
            Customer = customer;
        }
    }
}