namespace Eda.RDBI.Models
{
    public class EquipmentFleetSummaryModel
    {
        public int RRID { get; set; }

		public int VINCount { get; set; }

        // Vehicle
        public string Brand { get; set; }

        public int Units { get; set; }

        public string PercentageFleet { get; set; }
    }
}