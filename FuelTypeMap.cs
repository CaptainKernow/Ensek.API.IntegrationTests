using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ensek.API.IntegrationTests
{
    public static class FuelTypeMap
    {
        public static Dictionary<string, int> FuelTypeToId = new Dictionary<string, int>
        {
            // we could get these mappings form the GET /ENSEK/energy endpoint on startup to ensure they are correct
            { "gas", 1 },
            { "nuclear", 2 },
            { "electric", 3 },
            { "oil", 4 }
        };

        public static int GetFuelTypeId(string fuelType)
        {
            int Id = FuelTypeMap.FuelTypeToId[fuelType];
            return Id;
        }
    }
}
