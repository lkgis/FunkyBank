using System.IO;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using FunkyBank.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunkyBank.Functions
{
    [DependencyInjectionConfig(typeof(ApiBootstrapper))]
    public static class CreateCustomerFunction
    {
        [FunctionName("CreateCustomer")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = "customers")] HttpRequest req,
            [Inject]ICustomerService customerService, ILogger logger)
        {
            logger.LogInformation($"Calling {nameof(CreateCustomerFunction)}");

            var createCustomerRequest =
                JsonConvert.DeserializeObject<CreateCustomerRequest>(await new StreamReader(req.Body).ReadToEndAsync());

            var isValid = createCustomerRequest.Validate();

            if (!isValid)
            {
                logger.LogError("Error: Invalid request");
                return new BadRequestObjectResult("Invalid request");
            }

            var operationResult =
                await customerService.CreateCustomerAsync(createCustomerRequest).ConfigureAwait(false);

            if (operationResult.Status)
            {
                return new OkObjectResult(operationResult.Data);
            }

            return new BadRequestObjectResult(operationResult.Message);
        }
    }
}