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

namespace Ensek.API.IntegrationTests.StepDefinitions
{
    [Binding]
    public class ApiTestsStepDefinitions
    {
        private readonly HttpClient httpClient;
        private HttpResponseMessage response;
        private string bearerToken;
        private LoginRequest loginRequest;
        private readonly ScenarioContext scenarioContext;


        public ApiTestsStepDefinitions(LoginRequest loginRequest, ScenarioContext scenarioContext)
        {
            this.httpClient = HttpClientManager.ClientInstance;
            this.loginRequest = loginRequest;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"the data has been reset")]
        public void GivenTheDataHasBeenReset()
        {
            // Login/get bearer token
            bearerToken = loginRequest.GetBearerToken();

            // add token to header and Reset the data
            // BUG: Reset currently giving 401 despite the bearer token being in the header

            // httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);
            // response = httpClient.PostAsync("/ENSEK/reset", null).Result;
            // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [When(@"I order the following quatities of fuel")]
        public void WhenIOrderTheFollowingQuatitiesOfFuel(Table table)
        {
            foreach (var row in table.Rows)
            {
                var quantity = row["quantity"];
                var fuelType =row["fuelType"];
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
            // fancy string manipulation to extract the values from the message (would be nicer if the values were var in the response)
            // store each GUID in scenario context for look up in the next step
        }

        [Then(@"that order can then be returned")]
        public void ThenThatOrderCanThenBeReturned()
        {
            // none of this feels very D.R.Y.
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
            // BUG: currently giving 500

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
            throw new PendingStepException();
        }

        [Then(@"I provide a count of orders made before todays date")]
        public void ThenIProvideACountOfOrdersMadeBeforeTodaysDate()
        {
            throw new PendingStepException();
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

        [Given(@"the returned available quantity of '([^']*)' fuel is '([^']*)'")]
        public void GivenTheReturnedAvailableQuantityOfFuelIs(string nuclear, string p1)
        {
            throw new PendingStepException();
        }

        [When(@"I try to order '([^']*)' units of '([^']*)' fuel")]
        public void WhenITryToOrderUnitsOfFuel(string p0, string nuclear)
        {
            throw new PendingStepException();
        }

        [Then(@"I recieve an error message")]
        public void ThenIRecieveAnErrorMessage()
        {
            throw new PendingStepException();
        }

        [Given(@"the returned quatity of '([^']*)' fuel is positive")]
        public void GivenTheReturnedQuatityOfFuelIsPositive(string gas)
        {
            throw new PendingStepException();
        }

        [When(@"I try to order '([^']*)' units of '([^']*)' fuel")]
        public void WhenITryToOrderUnitsOfFuel(string p0, string gas)
        {
            throw new PendingStepException();
        }

    }
}
