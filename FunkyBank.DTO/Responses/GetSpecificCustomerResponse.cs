using FunkyBank.Models;

namespace FunkyBank.Responses
{
    public class GetSpecificCustomerResponse
    {
        public CustomerDisplayModel Customer { get; }

        public GetSpecificCustomerResponse(CustomerDisplayModel customer)
        {
            Customer = customer;
        }
    }
}