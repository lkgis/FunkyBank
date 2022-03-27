using System;
using System.Threading.Tasks;
using AzureFunctions.Autofac;
using FunkyBank.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunkyBank.Functions
{
    [DependencyInjectionConfig(typeof(ApiBootstrapper))]
    public static class GetCustomersFunction
    {
        [FunctionName("GetCustomers")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers")]
            HttpRequest req, [Inject] ICustomerService customerService, ILogger logger)
        {
            logger.LogInformation($"Calling {nameof(GetCustomersFunction)} : {DateTime.UtcNow.AddHours(3)}");

            var operationResult =
                await customerService.GetCustomersAsync(new GetCustomersRequest()).ConfigureAwait(false);

            if (operationResult.Status)
            {
                return new OkObjectResult(operationResult.Data);
            }

            logger.LogError(operationResult.Message);
            return new BadRequestObjectResult(operationResult.Message);
            /*string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult) new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");*/

        }
    }
}