using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SBTriggerFunction
{
    public static class Function1
    {
        //managedConnectionString is an application setting in the function
        //update local.settings.json for local
        [FunctionName("messageReceiver")]
        public static void Run([ServiceBusTrigger("testqueue", Connection = "managedConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
