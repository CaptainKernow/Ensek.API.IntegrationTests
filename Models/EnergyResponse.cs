using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensek.API.IntegrationTests.Models
{
    public class EnergyResponse
    {
        public EnergyType Electric { get; set; }
        public EnergyType Gas { get; set; }
        public EnergyType Nuclear { get; set; }
        public EnergyType Oil { get; set; }
    }
}
