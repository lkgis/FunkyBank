namespace FunkyBank.Requests
{
    public class CreateCustomerRequest : IValidatable
    {
        public CreateCustomerRequest(string name, string address)
        {
            Name = name;
            Address = address;
        }
        
        public string Name { get; }
        public string Address { get; }
        
        //Add application specific validations here
        
        public bool IsValid() => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Address);
    }
}