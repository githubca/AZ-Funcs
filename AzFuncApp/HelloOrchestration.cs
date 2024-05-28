using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Techbuild.Function
{
    public static class HelloOrchestration
    {
        [Function(nameof(HelloOrchestration))]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(HelloOrchestration));
            logger.LogInformation("Saying hello.");
            logger.LogInformation($"executionContext Name: {context.Name}");
            var outputs = new List<string>();
            string a1 = await context.CallActivityAsync<string>(nameof(SayHello), "Toronto");
            string a2 = await context.CallActivityAsync<string>(nameof(SayBye), a1);

            string b1 = await context.CallActivityAsync<string>(nameof(SayHello), "Vancouver");
            string b2 = await context.CallActivityAsync<string>(nameof(SayBye), b1);

            string c1 = await context.CallActivityAsync<string>(nameof(SayHello), "Montreal");
            string c2 = await context.CallActivityAsync<string>(nameof(SayBye), c1);

            // Replace name and input with values relevant for your Durable Functions Activity
            outputs.Add(a1);
            outputs.Add(a2);
            outputs.Add(b1);
            outputs.Add(b2);
            outputs.Add(c1);
            outputs.Add(c2);


            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [Function(nameof(SayHello))]
        public static string SayHello([ActivityTrigger] string name, FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("SayHello");
            logger.LogInformation("Saying hello to {name}.", name);
            logger.LogInformation($"executionContext Name: {executionContext.FunctionDefinition.Name}");
            return $"Hello {name}!";
        }

        [Function(nameof(SayBye))]
        public static string SayBye([ActivityTrigger] string name, FunctionContext exeContext)
        {
            ILogger logger = exeContext.GetLogger(nameof(SayBye));
            logger.LogInformation("Saying bye to {name}.", name);
            logger.LogInformation($"executionContext Name: {exeContext.FunctionDefinition.Name}");

            return $"Bye {name}!";
        }

        [Function("HelloOrchestration_HttpStart")]
        public static async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client,
            FunctionContext executionContext)
        {
            ILogger logger = executionContext.GetLogger("HelloOrchestration_HttpStart");

            // Function input comes from the request content.
            string instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                nameof(HelloOrchestration));

            logger.LogInformation("Started orchestration with ID = '{instanceId}'.", instanceId);

            logger.LogInformation($"req url: {req.Url}");
            logger.LogInformation($"client Name: {client.Name}");
            logger.LogInformation($"executionContext Name: {executionContext.FunctionDefinition.Name}");

            // Returns an HTTP 202 response with an instance management payload.
            // See https://learn.microsoft.com/azure/azure-functions/durable/durable-functions-http-api#start-orchestration
            return await client.CreateCheckStatusResponseAsync(req, instanceId);
        }
    }
}
