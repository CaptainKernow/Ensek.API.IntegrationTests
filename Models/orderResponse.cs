using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensek.API.IntegrationTests.Models
{
    public class OrderResponse
    {
        public string Fuel { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }
        public DateTime Time { get; set; }
    }
}
