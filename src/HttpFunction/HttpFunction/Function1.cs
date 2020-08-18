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
                var clientId = "";
                var clientSecret = "";

                //Make a Call to API with generated access token using CLIENTSECRET

                //var credential = new ClientCredential(clientId, clientSecret);
                //var authContext = new AuthenticationContext("https://login.microsoftonline.com/");
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

                
                string pubsubConnection = "";
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
