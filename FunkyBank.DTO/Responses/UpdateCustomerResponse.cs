using FunkyBank.Models;

namespace FunkyBank.Responses
{
    public class UpdateCustomerResponse
    {
        public CustomerDisplayModel Customer { get; }


        public UpdateCustomerResponse(CustomerDisplayModel customer)
        {
            Customer = customer;
        }
    }
}