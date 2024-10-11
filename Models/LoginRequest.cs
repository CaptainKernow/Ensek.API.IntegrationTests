using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Ensek.API.IntegrationTests.Models
{
    public class LoginRequest
    {
        private readonly HttpClient httpClient;
        private HttpResponseMessage response;

        public string username { get; set; }
        public string password { get; set; }

        public LoginRequest()
        {
            this.httpClient = HttpClientManager.ClientInstance;
        }

        public string GetBearerToken()
        {
            var loginRequest = new LoginRequest
            {
                // add these to either a config file or environment variables? maybe a dictionary of user types?
                username = "test",
                password = "testing"
            };

            var jsonContent = JsonConvert.SerializeObject(loginRequest);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            response = httpClient.PostAsync("/ENSEK/login", stringContent).Result;

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseContent);
            var bearerToken = loginResponse.Access_Token;

            // Store bearerToken in scenario context? Or just return it?
            Console.WriteLine("Bearer token: " + bearerToken);
            return bearerToken;

        }
    }
}
