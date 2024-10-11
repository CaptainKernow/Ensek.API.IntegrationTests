using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensek.API.IntegrationTests
{
    public class HttpClientManager
    {
        private static HttpClient httpClient;

        static HttpClientManager()
        {
            // add this to either config file or environment variables
            httpClient = new HttpClient { BaseAddress = new Uri("https://qacandidatetest.ensek.io") };
        }

        public static HttpClient ClientInstance => httpClient;

    }
}
