using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("SettlementBuildingDistribution")]
    internal class SettlementBuildingDistributionDto : BaseIdDto
    {
        public int SettlementTypeId { get; set; }

        public int BuildingTypeId { get; set; }

        public int MinBuildings { get; set; }

        public int MaxBuildings { get; set; }
    }
}
