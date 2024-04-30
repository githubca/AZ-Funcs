using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Techbuild.Function
{
    public class HttpTriggerFunc
    {
        private readonly ILogger<HttpTriggerFunc> _logger;

        public HttpTriggerFunc(ILogger<HttpTriggerFunc> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerFunc")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("hello world, Welcome to Azure Functions!");
        }
    }
}
