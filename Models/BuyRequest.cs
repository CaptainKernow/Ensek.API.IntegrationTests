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
    public class BuyRequest
    {
        private readonly HttpClient httpClient;
        private HttpResponseMessage response;
        private readonly ScenarioContext scenarioContext;

        public BuyRequest(ScenarioContext scenarioContext)
        {
            this.httpClient = HttpClientManager.ClientInstance;
            this.scenarioContext = scenarioContext;
        }

        public void BuyFuelFromTable(Table table)
        {
            foreach (var row in table.Rows)
            {
                var quantity = row["quantity"];
                var fuelType = row["fuelType"];
                var id = FuelTypeMap.GetFuelTypeId(fuelType);

                response = httpClient.PutAsync($"/ENSEK/buy/{id}/{quantity}", null).Result;
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                var responseContent = response.Content.ReadAsStringAsync().Result;
                var buyResponse = JsonConvert.DeserializeObject<BuyResponse>(responseContent);
                var buyMessage = buyResponse.Message;

                Console.WriteLine(responseContent);

                // take each message and store it in a scenario context name based on the fuel type and quantity
                // could we store these as a class object instead of a string to make them easier to access later?
                // no need to check message length, as this can be done as part of processing the message in the next step
                Console.WriteLine($"Saving message to scenario context as '{fuelType}_{quantity}'");
                scenarioContext[$"{fuelType}_{quantity}"] = buyMessage;
            }
        }

        public HttpResponseMessage BuySingleFuelType(string fuelType, int quantity)
        {
            var id = FuelTypeMap.GetFuelTypeId(fuelType);

            response = httpClient.PutAsync($"/ENSEK/buy/{id}/{quantity}", null).Result;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var responseContent = response.Content.ReadAsStringAsync().Result;
            var buyResponse = JsonConvert.DeserializeObject<BuyResponse>(responseContent);
            var buyMessage = buyResponse.Message;

            Console.WriteLine(responseContent);
            return response;
        }
    }
}
