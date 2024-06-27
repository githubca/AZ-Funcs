using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzFuncApp
{
    public class Greeting
    {
        private readonly ILogger<Greeting> _logger;

        public Greeting(ILogger<Greeting> logger)
        {
            _logger = logger;
        }

        [Function("Greeting")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];
            int age = int.Parse(req.Query["age"]);
            Person p = new Person()
            {
                Name = name,
                Address = "toronto",
                Age = age + 1,
                Gender = "Male"
            };
            return new OkObjectResult(p);
        }
    }

    public class Person
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }

    }
}
