namespace Ensek.API.IntegrationTests.Models
{
    public class EnergyType
    {
        public int EnergyId { get; set; }
        public double PricePerUnit { get; set; }
        public int QuantityOfUnits { get; set; }
        public string UnitType { get; set; }
    }
}