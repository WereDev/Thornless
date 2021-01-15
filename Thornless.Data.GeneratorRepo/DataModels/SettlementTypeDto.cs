using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Thornless.Data.GeneratorRepo.DataModels
{
    [Table("SettlementType")]
    internal class SettlementTypeDto : BaseIdDto
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public int MinSize { get; set; }

        public int MaxSize { get; set; }

        [ForeignKey(nameof(SettlementBuildingDistributionDto.SettlementTypeId))]
        public List<SettlementBuildingDistributionDto> Buildings { get; set; } = new List<SettlementBuildingDistributionDto>();
    }
}
