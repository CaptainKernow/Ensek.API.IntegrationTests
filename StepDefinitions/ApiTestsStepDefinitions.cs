using Ensek.API.IntegrationTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
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


        public ApiTestsStepDefinitions(LoginRequest loginRequest)
        {
            httpClient = new HttpClient { BaseAddress = new Uri("https://qacandidatetest.ensek.io") };
            this.loginRequest = loginRequest;
        }

        [Given(@"the data has been reset")]
        public void GivenTheDataHasBeenReset()
        {
            // Login/get bearer token
            loginRequest.GetBearerToken();

            // Reset the data
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + bearerToken);


        }

        [When(@"I order <quantity> of <fuelType> fuel")]
        public void WhenIOrderQuantityOfFuelTypeFuel()
        {
            Assert.That(true is true);
        }

        [Then(@"a successful response response shows fuel remaing, quatity bought and ID")]
        public void ThenASuccessfulResponseResponseShowsFuelRemaingQuatityBoughtAndID()
        {
            Assert.That(true is true);
        }

        [Then(@"that order can then be returned")]
        public void ThenThatOrderCanThenBeReturned(Table table)
        {
            Assert.That(true is true);
        }

        [When(@"I order '([^']*)' units of '([^']*)' fuel")]
        public void WhenIOrderUnitsOfFuel(string p0, string gas)
        {
            throw new PendingStepException();
        }

        [Then(@"that order is returned")]
        public void ThenThatOrderIsReturned()
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
    }
}
