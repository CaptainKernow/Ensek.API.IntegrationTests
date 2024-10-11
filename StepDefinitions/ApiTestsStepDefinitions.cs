using Ensek.API.IntegrationTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Configuration;
using System.Drawing.Printing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Ensek.API.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ApiTestsStepDefinitions
    {
        private readonly HttpClient httpClient;
        private HttpResponseMessage response;
        private string bearerToken;
        private LoginRequest loginRequest;
        private BuyRequest buyRequest;
        private int earlierOrderCount;
        private readonly ScenarioContext scenarioContext;


        public ApiTestsStepDefinitions(LoginRequest loginRequest, ScenarioContext scenarioContext)
        {
            this.httpClient = HttpClientManager.ClientInstance;
            this.loginRequest = loginRequest;
            this.buyRequest = buyRequest;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"the data has been reset")]
        public void GivenTheDataHasBeenReset()
        {
            bearerToken = loginRequest.GetBearerToken();

            // add token to header and Reset the data
            // BUG: Reset currently giving 401 despite the bearer token being in the header
            // TODO: Move into a method in the LoginRequest class or its own class
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
            response = httpClient.PostAsync("/ENSEK/reset", null).Result;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [When(@"I order the following quatities of fuel")]
        public void WhenIOrderTheFollowingQuatitiesOfFuel(Table table)
        {
            buyRequest.BuyFuelFromTable(table);
        }

        [Then(@"a successful response response shows fuel remaing, quatity bought and ID")]
        public void ThenASuccessfulResponseResponseShowsFuelRemaingQuatityBoughtAndID()
        {
            var electricResponse = scenarioContext["electric_10"];
            var gasResponse = scenarioContext["gas_20"];
            var oilResponse = scenarioContext["oil_30"];

            Assert.That(electricResponse, Is.Not.Null);
            Assert.That(gasResponse, Is.Not.Null);
            Assert.That(oilResponse, Is.Not.Null);

            // check that each message contains the correct information
            // calcualate how much fuel should cost based on the quantity bought and the fuel type unit price
            // BUG: Values are not correct way around for some fuel types
            // BUG: Prices/cost don't seem to match the quantity bought
            // TODO: add fancy string manipulation to extract the values from the message (would be nicer if the values were var in the response)
            // store each Order ID GUID in scenario context for look up in the next step
        }

        [Then(@"that order can then be returned")]
        public void ThenThatOrderCanThenBeReturned()
        {
            // TODO: none of this feels very D.R.Y.
            // define each GUID as a variable from the scenario context, again this would be easier if the context was a class object maybe?
            // hard coded for now, but sould be extracted from each response message/object
            var electricGuid = "f8c3b158-cf85-4ff0-8821-3593bf0595cc";
            var gasGuid = "00e3581c-daf4-4155-af64-a9bb89208ae8";
            var oilGuid = "d8642c06-c504-4877-9d05-1bbfecf0fb4";

            // use the GUID to return the order
            var electricOrderResponse = httpClient.GetAsync($"/ENSEK/orders/{electricGuid}").Result;
            Assert.That(electricOrderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var gasOrderResponse = httpClient.GetAsync($"/ENSEK/orders/{gasGuid}").Result;
            Assert.That(gasOrderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var oilOrderResponse = httpClient.GetAsync($"/ENSEK/orders/{oilGuid}").Result;
            Assert.That(oilOrderResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // assert that the response contains the correct information
            // BUG: get order by is currently giving 500 response
            var electricOrderContent = electricOrderResponse.Content.ReadAsStringAsync().Result;
            var electricOrder = JsonConvert.DeserializeObject<OrderResponse>(electricOrderContent);
            Assert.That(electricOrder.Quantity, Is.EqualTo(10));
            Assert.That(electricOrder.Fuel, Is.EqualTo("electric"));
            Assert.That(electricOrder.Id, Is.EqualTo(electricGuid));
            Assert.That(electricOrder.Time, Is.Not.Null);

            // assert on the remiaining orders and their expected values
        }

        [Given(@"A list of orders can be returned")]
        public void GivenAListOfOrdersCanBeReturned()
        {
            response = httpClient.GetAsync("/ENSEK/orders").Result;
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [When(@"I list orders made before todays date")]
        public void WhenIListOrdersMadeBeforeTodaysDate()
        {
            earlierOrderCount = 0;
            var orderResponseContent = response.Content.ReadAsStringAsync().Result;
            List<OrderResponse> orders = JsonConvert.DeserializeObject<List<OrderResponse>>(orderResponseContent);

            Console.WriteLine($"{orders.Count} orders found");

            foreach (var order in orders)
            {
                if (order.Time <= DateTime.Today)
                {
                    Console.WriteLine($"Order ID: {order.Id}, Fuel: {order.Fuel}, Quantity: {order.Quantity}, Time: {order.Time}");
                    earlierOrderCount++;
                }
            }
        }

        [Then(@"I provide a count of orders made before todays date")]
        public void ThenIProvideACountOfOrdersMadeBeforeTodaysDate()
        {
            Console.WriteLine($"{earlierOrderCount} orders made before today's date");
        }

        [Given(@"I have placed an order for '([^']*)' units of '([^']*)'")]
        public void GivenIHavePlacedAnOrderForUnitsOf(string p0, string gas)
        {
            throw new PendingStepException();
        }

        [When(@"I delete that order")]
        public void WhenIDeleteThatOrder()
        {
            throw new PendingStepException();
        }

        [Then(@"the order is removed from the system")]
        public void ThenTheOrderIsRemovedFromTheSystem()
        {
            throw new PendingStepException();
        }

        [Given(@"the returned available quantity of '([^']*)' fuel is (.*)")]
        public void GivenTheReturnedAvailableQuantityOfFuelIs(string fuelType, int quantity)
        {
            response = httpClient.GetAsync("/ENSEK/energy").Result;
            var energyResponseContent = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(energyResponseContent);

            List<EnergyType> energyTypes = JsonConvert.DeserializeObject<List<EnergyType>>(energyResponseContent);

            foreach (var energyType in energyTypes)
            {
                if (energyType.UnitType == fuelType)
                {
                    Assert.That(energyType.QuantityOfUnits, Is.EqualTo(quantity));
                }
            }
        }

        [When(@"I try to order (.*) units of '([^']*)' fuel")]
        public void WhenITryToOrderUnitsOfFuel(int quantity, string fuelType)
        {
            response = buyRequest.BuySingleFuelType(fuelType, quantity);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Then(@"I recieve an error message")]
        public void ThenIRecieveAnErrorMessage()
        {
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var buyResponse = JsonConvert.DeserializeObject<BuyResponse>(responseContent);
            var buyMessage = buyResponse.Message;

            // this could be made dynamic by passing the fuel type
            Assert.That(buyMessage, Is.EqualTo("There is no nuclear fuel to purchase!"));
        }

        [Given(@"the returned quatity of '([^']*)' fuel is positive")]
        public void GivenTheReturnedQuatityOfFuelIsPositive(string gas)
        {
            throw new PendingStepException();
        }
    }
}
