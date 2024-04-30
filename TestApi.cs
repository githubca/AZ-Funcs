using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Techbuild.Function
{
    public class TestApi
    {
        private readonly ILogger<HttpTriggerFunc> _logger;

        public TestApi(ILogger<HttpTriggerFunc> logger)
        {
            _logger = logger;
        }

        [Function("TestApi")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            Person p = new Person()
            {
                Name="Hanson",
                Address="Toronto",
                Age = 30
            };
            return new OkObjectResult(p);
        }
    }

    public class Person{
        public string Name { get; set;}
        public string Address { get; set;}
        public int Age { get; set;}

    }
}
