using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.ServiceBus;
using System.Threading;
using System.Text;
using Microsoft.Azure.ServiceBus.Primitives;
using Microsoft.Identity.Client;

namespace HttpFunction
{
    public static class Function1
    {

        public static IQueueClient queueClient;
        [FunctionName("messageSender")]
        public static async Task Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                var clientId = "cd1fc716-c659-4fc9-8652-034817e0b826";
                var clientSecret = "sLAZs9_yYYOubRn~YOHZ4pHj7rmemV6-.v";

                //Make a Call to API with generated access token using CLIENTSECRET

                //var credential = new ClientCredential(clientId, clientSecret);
                //var authContext = new AuthenticationContext("https://login.microsoftonline.com/5e7db0db-d885-4b21-9c08-2c696ab14fc6");
                //var token = await authContext.AcquireTokenAsync(clientId, credential);
                //log.LogInformation(token is null ? "token is null" : "token is not null");
                //var accessToken = token.AccessToken;
                //log.LogInformation(accessToken);

                //-----------------------------

                //Make a call to API with generated access token using MSI


                //var tokenProvider = new AzureServiceTokenProvider();
                //string accessToken = await tokenProvider.GetAccessTokenAsync(clientId);

                //var httpClient = new HttpClient();
                //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                //var test = await httpClient.GetAsync("https://msi-test-api.azurewebsites.net/WeatherForecast");
                //if (test.IsSuccessStatusCode)
                //{
                //    var content = await test.Content.ReadAsStringAsync();
                //    log.LogInformation(content);
                //}
                //else
                //{
                //    log.LogInformation("Http Call not successfull");
                //    log.LogInformation(test.StatusCode.ToString());
                //}

                //-----------------------------------

                //Create an instance of QueueClient using MSI

                //string pubsubConnection = "Endpoint=sb://msi-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=IejCipbIVNpUJfN8AhuzYw/gZreiCKTfeyfCBKrk/9M=";
                string pubsubConnection = "sb://msi-test.servicebus.windows.net";
                string queueName = "testqueue";
                var tokenProvider = new ManagedIdentityTokenProvider();
                queueClient = new QueueClient(pubsubConnection, queueName, tokenProvider);
                string message = $"Message {Guid.NewGuid()}";
                log.LogInformation(message);
                await queueClient.SendAsync(new Message(Encoding.UTF8.GetBytes(message)));

            }
            catch (Exception ex)
            {

                log.LogError(ex,"Error");
            }
        }
        
    }
}
