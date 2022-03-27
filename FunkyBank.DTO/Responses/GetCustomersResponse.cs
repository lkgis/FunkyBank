using FunkyBank.Models;

namespace FunkyBank.Responses
{
    public class GetCustomersResponse
    {
        public List<CustomerDisplayModel> Customers { get; }

        public GetCustomersResponse(IEnumerable<CustomerDisplayModel> customers)
        {
            Customers = customers?.ToList() ?? new List<CustomerDisplayModel>();
        }
    }
}