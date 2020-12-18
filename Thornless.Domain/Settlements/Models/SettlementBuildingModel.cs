namespace Thornless.Domain.Settlements.Models
{
    public class SettlementBuildingModel
    {
        public int Id { get; set; }

        public int SettlementTypeId { get; set; }

        public int BuildingTypeId { get; set; }

        public int MinBuildings { get; set; }

        public int MaxBuildings { get; set; }
    }
}
